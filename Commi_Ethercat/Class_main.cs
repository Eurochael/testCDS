using ComiLib.EtherCAT.Slave;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using ec = ComiLib.EtherCAT.SafeNativeMethods;
namespace Comi_Ethercat
{
    public partial class Class_Main
    {
        public bool comi_dll_Initilized = false;
        public int netID = 0;
        public int errorCode = 0;
        //Initialize 후 인식된 Slave개수를 가져온다. 해당 개수 만큼 For문 진행하며, Slave 상태 체크
        public int slave_cnt = 0;


        //AES CBC 전용(Serial) 변수
        public const int serial_use_cnt = 8; // RS232 + RS422 + RS485 사용 총 개수
        static class Const
        {
            public const int Bit_TransimitRequest = 0;
            public const int Bit_ReceiveAccepted = 1;

            public const int Bit_TransimitAccepted = 0;
            public const int Bit_ReceiveRequest = 1;

        }

        //AES-CBC Serial 통신간 사용
        IntPtr inPdoPtr = IntPtr.Zero;
        IntPtr outPdoPtr = IntPtr.Zero;
        int outPdoSize = 0;

        public const int send_byte_cnt = 22;
        public const int rcv_byte_cnt = 22;

        private AES_CBC.InPDO inPDO = new AES_CBC.InPDO();
        private AES_CBC.OutPDO outPDO = new AES_CBC.OutPDO();

        char[] InputStr = new char[22];
        ushort aescbc_ai_slaveAddr = 0x0; // AES CBC의 AI Slave에 Serial 기능이 들어있음
        bool isExist_AES_CBC = false;
        int[] receive_request = new int[8];
        string OutString, OutString_Hex;
        string[] InputString = new string[8], InputString_Hex = new string[8];
        //TextBox[] txtDataOut, txtDataIn;
        //Button[] btnDataOut;
        int SerOfs = 0;
        public string inital_log = "";

        public string MasterDeviceInit()
        {
            string result = "";
            //Device 초기화
            try
            {

                //Master Load
                
                if (!ec.ecGn_LoadDevice(ref errorCode))
                {
                    switch (errorCode)
                    {
                        case 5:
                            result = "Check for Mater Device에 12V Input Power";
                            break;

                        case 8:
                            //result = "Mater Device가 부팅되지 않았습니다. Windows의 FastBoot(빠른시작켜기)가 활성화 되어 있는 경우 비활성화 하세요";
                            result = "Mater Device is not booted, check for windows CMOS -> FastBoot";
                            break;
                    }
                    return result;
                }

                // Config된 slave 수를 확인합니다.
                // Configuration 단계에서 설정된 슬레이브의 수로 현재 연결된 슬레이브 수와는 관련이 없습니다.
                // Config 상세 정보  https://winoar.com/dokuwiki/platform:ethercat:1_setup:10_config:20_configuration
                uint cfgCount = ec.ecNet_GetCfgSlaveCount(netID, ref errorCode);
                if (errorCode != 0)
                {
                    result = "Error" + errorCode;
                    return result;
                }
                // 현재 네트워크에 연결되어 있는 slave 수를 확인합니다.
                uint slaveCount = ec.ecNet_ScanSlaves(netID, ref errorCode);
                slave_cnt = Convert.ToInt32(slaveCount);
                if (errorCode != 0)
                {
                    result = "Error" + errorCode;
                    return result;
                }

                // 설정된 모듈 수만큼 스캔되었는지 확인합니다.
                if (cfgCount != slaveCount)
                {
                    result = "Difference Scan Slave Module Count / Setting Salave Count" + "\r\n";
                    //result = "현재 스캔된 슬레이브의 모듈 수와 설정된 슬레이브의 모듈 수가  다릅니다." + "\r\n";
                    result += string.Format("ScanSlave : {0}. CfgCount : {1}", slaveCount, cfgCount) + "\r\n";
                    result += "Check Power or Network Status And COMI Rebot" + "\r\n";
                    //result += "전원이 들어가지 않았거나 네트워크와 연결되지 않은 슬레이브 모듈이 있는지 확인하시기 바랍니다." + "\r\n";
                    //result += "마스터를 포함한 슬레이브 모듈의 개수가 ScanSlave와 같다면 Configuration 을 재실행 하시기 바랍니다." + "\r\n";
                    return result;
                }

                // SW Version(FW, WDM, SDK)이 서로 호환되는 버전인지 확인합니다.
                // 호환되지 않는 버전이 설치되어 있는 경우 오동작 할 수 있습니다.
                result = GetVersionCompResult();
                if (result != "")
                {
                    result = "Version compare fail";
                    return result;
                }
                //AddLog("Version compare compt");

                // 슬레이브의 Input / Output이 반대로 연결된 모듈이 있는지 확인합니다.
                result = CheckReveseConnection();
                if (CheckReveseConnection() != "")
                {
                    result = "Reverse Module is exist";
                    //result = "역삽입된 모듈이 있습니다.";
                    return result;
                }

                // Network의 alStatus를 OP로 설정합니다.
                ec.ecNet_SetAlState(netID, ec.EEcAlState.OP, ref errorCode);
                if (errorCode != 0)
                {
                    result = "Error" + errorCode;
                    return result;
                }
                //AddLog("Set AlState to OP");

                // Network의 alStatus가 변경되는 경우, 모든 Slave의 alStatus도 Network의 alStatus로 변경됩니다.
                // slave의 alStatus가 OP가 되지 않는 경우, 해당 slave를 점검하시기 바랍니다.
                // https://winoar.com/dokuwiki/platform:ethercat:1_setup:10_config:ts:30_safeop_failed

                // 모든 모듈의 alStatus가 OP가 되었는지 확인합니다.
                // alStatus가 OP가 아닌 slave는 정상적으로 제어되지 않습니다.
                ec.EEcAlState alState = ec.EEcAlState.INITIAL;
                Stopwatch sw = new Stopwatch();
                sw.Start();

                bool isSuccess = false;
                while (sw.ElapsedMilliseconds < 10000 && !isSuccess)
                {
                    isSuccess = true;
                    for (int i = 0; i < slaveCount; i++)
                    {
                        alState = ec.ecSlv_GetAlState_A(netID, i, ref errorCode);
                        if (alState != ec.EEcAlState.OP || errorCode != 0)
                        {
                            isSuccess = false;
                            break;
                        }
                    }
                    Thread.Sleep(200);
                }

                if (!isSuccess)
                {
                    for (int i = 0; i < slaveCount; i++)
                    {
                        alState = ec.ecSlv_GetAlState_A(netID, i, ref errorCode);
                        if (alState != ec.EEcAlState.OP || errorCode != 0)
                            // result = string.Format("슬레이브 : {0} 의 AlState를 OP로 설정하는데 실패하였습니다.", i);
                            result = string.Format("Slave : {0}  AlState OP Fail", i);
                    }

                    return result;
                }
                // AddLog("MasterDevice Init Compt");
                comi_dll_Initilized = true;
                inital_log = "설정 모듈 개수 : " + cfgCount + " / 스캔 모듈 개수 :" + slaveCount + " / Version : " + result + " / All OP : " + "True";

                result = "";
            }
            catch (BadImageFormatException)
            {
                return string.Format("ecGn_LoadDevice Failed : DLL Version (x86/x64) Load Fail.");
                //return string.Format("ecGn_LoadDevice Failed : DLL 버전(x86/x64)이 OS와 맞지 않습니다.");
            }
            catch (DllNotFoundException)
            {
                return string.Format("ecGn_LoadDevice Failed : DLL Not Found.");
                //return string.Format("ecGn_LoadDevice Failed : DLL을 찾을 수 없습니다.");
            }

            catch (Exception ex)
            {
                return string.Format("ecGn_LoadDevice Failed : Exception - {0}", ex.ToString());
            }

            finally
            {

            }
            return result;
        }
        private string GetVersionCompResult()
        {
            string result = "";
            ec.TEcFileVerInfo_SDK sdkInfo = new ec.TEcFileVerInfo_SDK();
            ec.TEcFileVerInfo_WDM driverInfo = new ec.TEcFileVerInfo_WDM();
            ec.TEcFileVerInfo_FW fwInfo = new ec.TEcFileVerInfo_FW();

            bool isSuccess = ec.ecNet_GetVerInfo(netID, ref sdkInfo, ref driverInfo, ref fwInfo, ref errorCode);

            if (!isSuccess)
            {
                //FW - SDK 호환성 결과
                switch (sdkInfo.nFwCompResult)
                {
                    case (int)ec.EEcVerCompatResult.ecVER_MISMATCH_LOWER: result = "Library version is higher than the Firmware"; return result;
                    case (int)ec.EEcVerCompatResult.ecVER_MISMATCH_HIGHER: result = "Library version is lower than the Firmware"; return result;
                    case (int)ec.EEcVerCompatResult.ecVER_MATCH: result = ""; break; //"FW-SDK : OK"
                    default: result = "Firmware Version is invalid"; return result;
                }

                //FW-WDM 호환성 결과
                switch (driverInfo.nFwCompResult)
                {
                    case (int)ec.EEcVerCompatResult.ecVER_MISMATCH_LOWER: result = "Driver version is higher than the Firmware"; return result;
                    case (int)ec.EEcVerCompatResult.ecVER_MISMATCH_HIGHER: result = "Driver version is lower than the Firmware"; return result;
                    case (int)ec.EEcVerCompatResult.ecVER_MATCH: result = ""; break; //FW-WDM : OK
                    default: result = "Firmware Version is invalid"; return result;
                }

                //SDK-WDM
                switch (sdkInfo.nWdmCompResult)
                {
                    case (int)ec.EEcVerCompatResult.ecVER_MISMATCH_LOWER: result = "Driver version is lower than the Library"; return result;
                    case (int)ec.EEcVerCompatResult.ecVER_MISMATCH_HIGHER: result = "Library version is lower than the Driver"; return result;
                    case (int)ec.EEcVerCompatResult.ecVER_MATCH: result = ""; break; //SDK-WDM : OK
                    default: result = "Driver Version is invalid"; return result;
                }
            }

            return result;
        }
        private string CheckReveseConnection()
        {
            string result = "";
            result = CanCheckReverseConnection();
            if (result != "")
                return result;

            // 역삽입된 슬레이브의 수를 확인합니다.
            int scanSlaveCount = 0;
            int reverseConnectionCount = ec.ecNet_CheckReverseConnections(netID, ref scanSlaveCount, ref errorCode);

            // 정의된 슬레이브 수가 있다면, 스캔된 슬레이브의 수와 비교합니다.
            //if (definedSlaveCount != scanSlaveCount)
            //{
            //    // 두 변수값의 차이는 Config 되었지만 현재 연결되어 있지 않은 슬레이브의 수입니다.
            //    AddLog(string.Format("Disconnected Slave Count = {0}", definedSlaveCount - scanSlaveCount));
            //}

            // reverseConnectionCount 값이 0인 경우, 역삽입된 모듈이 없는 경우이므로 정상입니다.
            if (reverseConnectionCount == 0)
            {
                result = "";//string.Format("ReverseConnection is nothing.");
                return result;
            }
            else
            {
                result = string.Format("ReverseConnectionCount = {0}", reverseConnectionCount);

                bool isReverseConnected = false;
                // 역삽입된 모듈이 있는 경우, 슬레이브별로 역삽입 여부를 확인합니다.
                for (ushort i = 0; i < scanSlaveCount; i++)
                {
                    isReverseConnected = ec.ecSlv_IsReverseConnected_A(netID, i, ref errorCode);

                    if (isReverseConnected)
                        result = string.Format("Check SlaveIndex {0} : ReverseConnected", i);
                }
                return result;
            }
        }
        /// <summary>
        /// 역삽입 검출 기능을 사용할 수 있는지 확인
        /// DLL : 1.5.3.2 ( FW : 1.92 / WDM : 1.5.0.6)  이상의 버전에서 사용 가능
        /// </summary>        
        private string CanCheckReverseConnection()
        {
            string result = "";
            ec.TEcFileVerInfo_SDK sdkInfo = new ec.TEcFileVerInfo_SDK();
            ec.TEcFileVerInfo_WDM driverInfo = new ec.TEcFileVerInfo_WDM();
            ec.TEcFileVerInfo_FW fwInfo = new ec.TEcFileVerInfo_FW();

            //FW / Driver / Library의 버전을 확인합니다.
            bool isSuccess = ec.ecNet_GetVerInfo(netID, ref sdkInfo, ref driverInfo, ref fwInfo, ref errorCode);
            string sdkVer = string.Format("{0}{1}{2}{3}", sdkInfo.CurVer.MajorVer, sdkInfo.CurVer.MinorVer, sdkInfo.CurVer.BuildNo, sdkInfo.CurVer.RevNo);
            int curVer = int.Parse(sdkVer);

            // Library의 버전이 1.5.3.2 이하인 경우 해당 기능을 사용할 수 없습니다.
            if (curVer < 1532)
            {
                result = "CheckReverseConnection : Not Supported version";
                return result;
            }

            return result;
        }

        #region "IO 관련"
        // IO 관련 함수군은 Global Channel(전역 번호) 함수군과 Local Channel(지역 채널) 함수군으로 구분된다.
        // Global 함수군은 네트워크에 연결된 모든 채널을 종합하여 채널 번호를 할당한다. (DI 와 Do 는 구분)
        // Local 함수군은 각각의 슬레이브에 독립적으로 채널 번호를 할당한다. (모든 슬레이브의 첫번 째 채널값이 0)
        // Global 함수군은 슬레이브의 연결 순서 변경, 통신 문제 등에 의해 의도치 않은 값으로 확인될 가능성이 있으므로
        // 가능한 Local 함수군을 사용하는 것이 좋다.

        /// <summary>
        /// DI Value Read 16단위 조회 가능
        /// </summary>  
        public string DI_Read(ref bool[] di_tmp)
        {
            string result = "";
            int total_ch_cnt = di_tmp.Length;
            byte ch_read_cnt = 16;
            uint diVal = 0;
            try
            {
                //// Global 함수군 사용 시
                //if (rdoDiGlobal.Checked)
                //{
                //    // 본 예제에서는 InitChannel 부터 numChannel 만큼의 Di Channel값을 확인한다.
                //    // numChannel이 16보다 큰 경우 16개만 확인한다.

                //    // InitChannel : 입력 상태를 확인할 시작 채널 번호
                //    // NumChannel : 입력 상태를 확인할 채널의 수
                //    // Return 값의 각 비트값이 채널값이다 ex) 첫번째 비트값이 InitChannel에 해당하는 채널의 신호상태 값
                //    diVal = ec.ecdiGetMulti(netID, InitChannel, numChannel, ref errorCode);
                //}
                //else // Local Channel 함수군 사용 시
                //{
                //    ushort slaveAddr = 0;
                //    if (ushort.TryParse(tbxDiSlaveAddr.Text, out slaveAddr) && slaveAddr > 0x200)
                //        diVal = ec.ecdiGetMulti_L(netID, slaveAddr, InitChannel, numChannel, ref errorCode);

                //}

                //DI Read 최대 16개씩 읽을 수 있으며, Init CH로 가져온다
                for (int idx = 0; idx < (total_ch_cnt / ch_read_cnt); idx++)
                {
                    if (idx == 0)
                    {
                        diVal = ec.ecdiGetMulti(netID, 0, ch_read_cnt, ref errorCode);
                    }
                    else
                    {
                        diVal = ec.ecdiGetMulti(netID, Convert.ToUInt32((ch_read_cnt * idx)), ch_read_cnt, ref errorCode);
                    }
                    if (errorCode != 0) //에러처리
                    {
                        result = "Error" + errorCode;
                        return result;
                    }
                    else
                    {

                    }

                    for (int idx2 = 0; idx2 < ch_read_cnt; idx2++)
                    {
                        //chks[i].Text = string.Format("Ch {0:00}", InitChannel + i);
                        //chks[i].Checked = ((diVal >> i) & 1) == 1;
                        di_tmp[Convert.ToUInt32((ch_read_cnt * idx)) + idx2] = ((diVal >> idx2) & 1) == 1;
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                return result;
            }
        }
        public string DO_Read(ref bool[] do_tmp)
        {
            string result = "";
            int total_ch_cnt = do_tmp.Length;
            byte ch_read_cnt = 16;
            uint diVal = 0;
            try
            {

                //DI Read 최대 16개씩 읽을 수 있으며, Init CH로 가져온다
                for (int idx = 0; idx < (total_ch_cnt / ch_read_cnt); idx++)
                {
                    if (idx == 0)
                    {
                        diVal = ec.ecdoGetMulti(netID, 0, ch_read_cnt, ref errorCode);
                    }
                    else
                    {
                        diVal = ec.ecdoGetMulti(netID, Convert.ToUInt32((ch_read_cnt * idx)), ch_read_cnt, ref errorCode);
                    }
                    if (errorCode != 0) //에러처리
                    {
                        result = "Error" + errorCode;
                        return result;
                    }
                    else
                    {

                    }

                    for (int idx2 = 0; idx2 < ch_read_cnt; idx2++)
                    {
                        //chks[i].Text = string.Format("Ch {0:00}", InitChannel + i);
                        //chks[i].Checked = ((diVal >> i) & 1) == 1;
                        do_tmp[Convert.ToUInt32((ch_read_cnt * idx)) + idx2] = ((diVal >> idx2) & 1) == 1;
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                return result;
            }
        }
        public string DO_Write_Alone(byte address, bool state)
        {
            string result = "";
            try
            {
                ec.ecdoPutOne(netID, address, state, ref errorCode);
                if (errorCode != 0) // 에러처리
                {
                    result = "Error" + errorCode;
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                return result;
            }
        }
        /// <summary>
        /// DI Value Read 16단위 cnffur 가능
        /// </summary>  
        public string DO_Write_Multi(bool[] di, bool state)
        {
            string result = "";
            byte ch_read_cnt = 16;
            int total_ch_cnt = di.Length;
            uint doVal = 0;
            try
            {
                //DI Read 최대 16개씩 쓸 수 있으며, Init CH로 시작한다
                // DoPutMulti의 OutState는 각 비트가 채널값이 된다.
                // 1,2,3 채널 출력시의 OutState값은 7
                // 1,2,3,4 채널 출력시의 OutState값은 15

                for (int idx = 0; idx < (total_ch_cnt / ch_read_cnt); idx++)
                {
                    doVal = 0;
                    if (idx == 0)
                    {
                        for (int idx2 = 0; idx2 < 16; idx2++)
                        {
                            if (di[idx2] == true)
                            {
                                doVal |= (uint)(1 << idx2);
                            }
                        }
                        ec.ecdoPutMulti(netID, 0, 16, doVal, ref errorCode);
                    }
                    else
                    {
                        for (int idx2 = 0; idx2 < ch_read_cnt; idx2++)
                        {
                            if (di[Convert.ToUInt32((ch_read_cnt * idx)) + idx2] == true)
                            {
                                doVal |= (uint)(1 << idx2);
                            }
                        }
                        ec.ecdoPutMulti(netID, Convert.ToUInt32((ch_read_cnt * idx)), ch_read_cnt, doVal, ref errorCode);
                        //doVal = ec.ecdiGetMulti(netID, Convert.ToUInt32(((ch_read_cnt) * idx)), ch_read_cnt, ref errorCode);
                    }

                    if (errorCode != 0) //에러처리
                    {
                        result = "Error" + errorCode;
                        return result;
                    }
                    else
                    {

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                return result;
            }
        }

        /// <summary>
        /// AI Value Read / AI Range설정은 커미조아 COMI IDE에서 진행, 프로그램에서는 데이터만 수신함
        /// </summary>  
        public string AI_Read(ref int[] ai_tmp)
        {
            string result = "";
            int total_ch_cnt = ai_tmp.Length;
            int total_ch_cnt_read_by_comi = 0;
            int aiVal = 0;
            try
            {

                //AI Channel 수를 확인한다.
                //total_ch_cnt_read_by_comi = ec.ecaiGetNumChannels(netID, ref errorCode);
                errorCode = 0;
                if (errorCode != 0) // 에러처리
                {
                    result = "Error" + errorCode;
                    return result;
                }
                //실제 AI 개수 보다 , 사용자가 설정한 AI가 많으면 False Return , 실제 AI 개수 만큼만 For문 돈다.
                //if (total_ch_cnt > total_ch_cnt_read_by_comi)
                //{
                //    result = "Real Connected AI Array Size Falut (" + total_ch_cnt_read_by_comi + ")";
                //    return result;
                //}



                // ecaiGetChanVal_I 는 Ai 모듈의 값 타입이 정수형일때 사용한다. 모듈 매뉴얼 확인
                // ecaiGetChanVal_F 는Ai 모듈의 값 타입이 Float 이거나 Real 일때 사용한다. 
                // ecaiGetChanVal_FS 은 Ai 모듈의 값타입이 digit 인경우, Scale을 정해 반환받을 때 사용한다. ex) 0 ~ 65535 반환(digit), -10 ~ 10 (V)로 확인
                // COMIZOA 의 AI 모듈은 AiType에 상관없이 mV, mA 단위의 정수형으로 반환한다.
                for (int idx = 0; idx < total_ch_cnt; idx++)
                {
                    aiVal = ec.ecaiGetChanVal_I(netID, (uint)idx, ref errorCode);
                    ai_tmp[idx] = aiVal;
                    if (errorCode != 0) // 에러처리
                    {
                        result = "Error" + errorCode;
                        return result;
                    }
                }




                //// Global 함수군 사용 시
                //if (rdoDiGlobal.Checked)
                //{
                //    // 본 예제에서는 InitChannel 부터 numChannel 만큼의 Di Channel값을 확인한다.
                //    // numChannel이 16보다 큰 경우 16개만 확인한다.

                //    // InitChannel : 입력 상태를 확인할 시작 채널 번호
                //    // NumChannel : 입력 상태를 확인할 채널의 수
                //    // Return 값의 각 비트값이 채널값이다 ex) 첫번째 비트값이 InitChannel에 해당하는 채널의 신호상태 값
                //    diVal = ec.ecdiGetMulti(netID, InitChannel, numChannel, ref errorCode);
                //}
                //else // Local Channel 함수군 사용 시
                //{
                //    ushort slaveAddr = 0;
                //    if (ushort.TryParse(tbxDiSlaveAddr.Text, out slaveAddr) && slaveAddr > 0x200)
                //        diVal = ec.ecdiGetMulti_L(netID, slaveAddr, InitChannel, numChannel, ref errorCode);

                //}

                return result;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                return result;
            }
        }

        /// <summary>
        /// AI Value Read / AI Range설정은 커미조아 COMI IDE에서 진행, 프로그램에서는 데이터만 수신함
        /// </summary>  
        public string AO_Read(ref int[] ao_tmp)
        {
            string result = "";
            int total_ch_cnt = ao_tmp.Length;
            int total_ch_cnt_read_by_comi = 0;
            int aiVal = 0;
            try
            {

                //AI Channel 수를 확인한다.
                //total_ch_cnt_read_by_comi = ec.ecaiGetNumChannels(netID, ref errorCode);
                errorCode = 0;
                if (errorCode != 0) // 에러처리
                {
                    result = "Error" + errorCode;
                    return result;
                }
                //실제 AI 개수 보다 , 사용자가 설정한 AI가 많으면 False Return , 실제 AI 개수 만큼만 For문 돈다.
                //if (total_ch_cnt > total_ch_cnt_read_by_comi)
                //{
                //    result = "Real Connected AI Array Size Falut (" + total_ch_cnt_read_by_comi + ")";
                //    return result;
                //}



                // ecaiGetChanVal_I 는 Ai 모듈의 값 타입이 정수형일때 사용한다. 모듈 매뉴얼 확인
                // ecaiGetChanVal_F 는Ai 모듈의 값 타입이 Float 이거나 Real 일때 사용한다. 
                // ecaiGetChanVal_FS 은 Ai 모듈의 값타입이 digit 인경우, Scale을 정해 반환받을 때 사용한다. ex) 0 ~ 65535 반환(digit), -10 ~ 10 (V)로 확인
                // COMIZOA 의 AI 모듈은 AiType에 상관없이 mV, mA 단위의 정수형으로 반환한다.
                for (int idx = 0; idx < total_ch_cnt; idx++)
                {
                    aiVal = ec.ecaoGetOutValue_I(netID, (uint)idx, ref errorCode);
                    ao_tmp[idx] = aiVal;
                    if (errorCode != 0) // 에러처리
                    {
                        result = "Error" + errorCode;
                        return result;
                    }
                }




                //// Global 함수군 사용 시
                //if (rdoDiGlobal.Checked)
                //{
                //    // 본 예제에서는 InitChannel 부터 numChannel 만큼의 Di Channel값을 확인한다.
                //    // numChannel이 16보다 큰 경우 16개만 확인한다.

                //    // InitChannel : 입력 상태를 확인할 시작 채널 번호
                //    // NumChannel : 입력 상태를 확인할 채널의 수
                //    // Return 값의 각 비트값이 채널값이다 ex) 첫번째 비트값이 InitChannel에 해당하는 채널의 신호상태 값
                //    diVal = ec.ecdiGetMulti(netID, InitChannel, numChannel, ref errorCode);
                //}
                //else // Local Channel 함수군 사용 시
                //{
                //    ushort slaveAddr = 0;
                //    if (ushort.TryParse(tbxDiSlaveAddr.Text, out slaveAddr) && slaveAddr > 0x200)
                //        diVal = ec.ecdiGetMulti_L(netID, slaveAddr, InitChannel, numChannel, ref errorCode);

                //}

                return result;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                return result;
            }
        }
        #endregion

        #region "Serial 관련"
        public string Init_AES_CBC_SERIAL()
        {
            string result = "";
            try
            {
                // AES CBC 모듈의 존재 여부와 SlavePhysAddress를 확인하는 코드이다.
                // 예제를 위한 코드이며, 하나의 모듈만 체크한다.
                uint slaveCount = ec.ecNet_ScanSlaves(netID, ref errorCode);
                for (int s = 0; s < slaveCount; s++)
                {
                    ec.TEcSlaveCfg slaveCfg = new ec.TEcSlaveCfg();
                    ec.ecCfg_GetSlaveConfig(netID, s, ref slaveCfg, ref errorCode);
                    if (slaveCfg.ProdCode == 0x5032F502)
                    {
                        isExist_AES_CBC = true;
                        aescbc_ai_slaveAddr = slaveCfg.PhysAddr;
                        break;
                    }
                }


                if (!isExist_AES_CBC)
                {
                    return "isExist_AES_CBC is False";

                }

                inPDO.InPDOs_AI = new ComiLib.EtherCAT.Slave.AES_CBC_AIO_InPDO[8];
                inPDO.InPDOs_SER = new ComiLib.EtherCAT.Slave.AES_CBC_SER_InPDO[8];
                outPDO.OutPDOs_AO = new ComiLib.EtherCAT.Slave.AES_CBC_AIO_OutPDO();
                outPDO.OutPDOs_SER = new ComiLib.EtherCAT.Slave.AES_CBC_SER_OutPDO[8];

                for (int i = 0; i < serial_use_cnt; i++)
                {
                    outPDO.OutPDOs_SER[i].DataOut = new byte[22];
                    inPDO.InPDOs_SER[i].DataIn = new byte[22];
                }

                //InPDO Data의 시작위치를 확인한다
                inPdoPtr = ec.ecSlv_InPDO_GetBufPtr(netID, aescbc_ai_slaveAddr, 0, ref errorCode);

                //OutPDO Data의 시작위치를 확인한다
                outPdoPtr = ec.ecSlv_OutPDO_GetBufPtr(netID, aescbc_ai_slaveAddr, 0, ref errorCode);
                outPdoSize = Marshal.SizeOf(typeof(AES_CBC.OutPDO));

                //InPDO Data를 시작위치를 기준으로 구조체로 변경하여 가져온다.
                inPDO = (AES_CBC.InPDO)(Marshal.PtrToStructure(inPdoPtr, typeof(AES_CBC.InPDO)));
                //OutPDO Data를 시작위치를 기준으로 구조체로 변경하여 가져온다.
                outPDO = (AES_CBC.OutPDO)(Marshal.PtrToStructure(outPdoPtr, typeof(AES_CBC.OutPDO)));

                // receive request 값 저장
                for (int i = 0; i < serial_use_cnt; i++)
                {
                    receive_request[i] = GETBIT(inPDO.InPDOs_SER[i].StatusByte, Const.Bit_ReceiveRequest);
                }

                RecveAccept();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        private void RecveAccept()
        {
            inPDO = (AES_CBC.InPDO)(Marshal.PtrToStructure(inPdoPtr, typeof(AES_CBC.InPDO)));
            outPDO = (AES_CBC.OutPDO)(Marshal.PtrToStructure(outPdoPtr, typeof(AES_CBC.OutPDO)));

            for (int i = 0; i < serial_use_cnt; i++)
            {
                // Receive Accepted 토글
                byte toggle = 0;
                // 선택된 채널의 Serial OutPDO Receive Accepted 위치(CtrlByte의 1번 bit)
                SerOfs = Marshal.SizeOf(typeof(AES_CBC_AIO_OutPDO)) + Marshal.SizeOf(typeof(AES_CBC_SER_OutPDO)) * i;
                if (GETBIT(outPDO.OutPDOs_SER[i].CtrlByte, Const.Bit_ReceiveAccepted) == 1)
                {
                    toggle = SETBIT(outPDO.OutPDOs_SER[i].CtrlByte, Const.Bit_ReceiveAccepted, 0);
                    Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                }
                else
                {
                    toggle = SETBIT(outPDO.OutPDOs_SER[i].CtrlByte, Const.Bit_ReceiveAccepted, 1);
                    Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                }
            }
        }
        private void RecveAccept(int idx_serial)
        {
            inPDO = (AES_CBC.InPDO)(Marshal.PtrToStructure(inPdoPtr, typeof(AES_CBC.InPDO)));
            outPDO = (AES_CBC.OutPDO)(Marshal.PtrToStructure(outPdoPtr, typeof(AES_CBC.OutPDO)));
            // Receive Accepted 토글
            byte toggle = 0;
            // 선택된 채널의 Serial OutPDO Receive Accepted 위치(CtrlByte의 1번 bit)
            SerOfs = Marshal.SizeOf(typeof(AES_CBC_AIO_OutPDO)) + Marshal.SizeOf(typeof(AES_CBC_SER_OutPDO)) * idx_serial;
            if (GETBIT(outPDO.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted) == 1)
            {
                toggle = SETBIT(outPDO.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted, 0);
                Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
            }
            else
            {
                toggle = SETBIT(outPDO.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted, 1);
                Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
            }
        }
        private byte SETBIT(Byte data, int shft, int value)
        {
            // bit set 1
            if (value == 1)
            {
                data |= (byte)(1 << shft);
            }
            // bit set 0
            else
            {
                data &= (byte)(~(1 << shft));
            }

            return data;
        }

        private int GETBIT(byte data, int shft)
        {
            return ((data >> shft) & 0x1);
        }
        public string Analog_Range_Setting(uint ai_address, int idx_range)
        {
            string result = "";
            int slaveIndex = 0;// ec.ecaiGetSlaveIndex(netID, (uint)aiCh, ref errorCode);
            int aiRangeMode = 0;// cbxAiRangeMode.SelectedIndex;
            int index = 0;// 0x8000 + aiCh * 0x10;
            int subIndex = 9;
            int dataSize = 1;
            int isComptAccess = 0;

            try
            {
                //IO GUI combo list"-10.24v~10.24v", "-5.12v~5.12v", "-2.56v~2.56v", "0v~10.24v", "0v~5.12v", "4mA~20mA", "0mA~20mA", "0mA~24mA"

                //Range Index
                //"0 : -10.24 ~ 10.24 (V)",
                //"1 : -5.12 ~ 5.12 (V)",
                //"2 : -2.56 ~ 2.56 (V)",
                //"3 : 0 ~ 10.24 (V)",
                //"4 : 0 ~ 5.12 (V)",
                //"5 : 4 ~ 20 (mA)",
                //"6 : 0 ~ 20 (mA)",
                //"7 : 0 ~ 24 (mA)",

                //enum_aes_cbc_analog_range range
                slaveIndex = ec.ecaiGetSlaveIndex(netID, ai_address, ref errorCode);
                aiRangeMode = idx_range;
                //SlaveIndex가 변경될 시 Address Index 변경 필요 / 각 슬레이브당 Range 0x8000부터 시작함 
                //2022-08-19 커미조아 확인 요청
                if (ai_address >= 16)
                {
                    index = 0x8000 + ((int)ai_address - 16) * 0x10; //((int)ai_address - 8)
                }
                else
                {
                    index = 0x8000 + (int)ai_address * 0x10; //((int)ai_address - 8)
                }
                subIndex = 9;
                dataSize = 1;
                isComptAccess = 0;

                ec.ecSlv_WriteCoeSdo_A(netID, slaveIndex, index, subIndex, isComptAccess, dataSize, ref aiRangeMode, ref errorCode);
                if (errorCode != 0)
                {
                    result = "Analog_Range_Setting Error";
                    return result;
                }

                Thread.Sleep(2);
                WriteToSave(slaveIndex);

            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        private void WriteToSave(int slaveIndex)
        {
            // 모듈에 변경값을 기록한다.                 
            int isSave = 0; // Setting 값을 저장하지 않음
            ec.ecSlv_WriteCoeSdo_A(netID, slaveIndex, 0xF001, 0x00, 0, 2, ref isSave, ref errorCode);
            System.Threading.Thread.Sleep(2);
            // Flag 변경이 확인되야 하므로, 0을 먼저 기록하고 1을 기록한다.
            isSave = 1; // Setting 값을 저장
            ec.ecSlv_WriteCoeSdo_A(netID, slaveIndex, 0xF001, 0x00, 0, 2, ref isSave, ref errorCode);
        }
        public string Serial_Setting_Load(ref string read_log)
        {
            string result = "";
            try
            {
                uint slaveCount = ec.ecNet_ScanSlaves(netID, ref errorCode);

                if (errorCode != 0)
                {
                    result = "Serial_Setting -> slaveCount Scan Error";
                    return result;
                }


                //AES-CBC Model Get
                for (int s = 0; s < slaveCount; s++)
                {
                    ec.TEcSlaveCfg slaveCfg = new ec.TEcSlaveCfg();
                    ec.ecCfg_GetSlaveConfig(netID, s, ref slaveCfg, ref errorCode);
                    //0x5032F501 - AES - CBC - DIO
                    //0x5032F502 - AES - CBC - AIO
                    //0x5032D2AF - ETS - AI16AH - E
                    //0xx5032D2A8 - ETS - AI08AH - E
                    //0xx5032D912 - ETS - DO16N

                    if (slaveCfg.ProdCode == 0x5032F502)
                    {
                        aescbc_ai_slaveAddr = slaveCfg.PhysAddr;
                        break;
                    }
                }

                for (int i = 0; i < serial_use_cnt; i++)
                {
                    if (read_log != "") { read_log = read_log + ","; }

                    int databit = 0, stopbit = 0, paritybit = 0, baudrate = 0, serialtype = 0;

                    ec.ecSlv_ReadCoeSdo(netID, aescbc_ai_slaveAddr, 0x8900 + 0x10 * i, 0x01, 0, 1, ref databit, ref errorCode);
                    databit = databit & 0x1;
                    System.Threading.Thread.Sleep(2);
                    ec.ecSlv_ReadCoeSdo(netID, aescbc_ai_slaveAddr, 0x8900 + 0x10 * i, 0x02, 0, 1, ref stopbit, ref errorCode);
                    stopbit = stopbit & 0x1;
                    System.Threading.Thread.Sleep(2);
                    ec.ecSlv_ReadCoeSdo(netID, aescbc_ai_slaveAddr, 0x8900 + 0x10 * i, 0x03, 0, 1, ref paritybit, ref errorCode);
                    paritybit = paritybit & 0x3;
                    System.Threading.Thread.Sleep(2);
                    ec.ecSlv_ReadCoeSdo(netID, aescbc_ai_slaveAddr, 0x8900 + 0x10 * i, 0x04, 0, 1, ref baudrate, ref errorCode);
                    baudrate = baudrate & 0x7;
                    System.Threading.Thread.Sleep(2);
                    ec.ecSlv_ReadCoeSdo(netID, aescbc_ai_slaveAddr, 0x8900 + 0x10 * i, 0x05, 0, 1, ref serialtype, ref errorCode);
                    serialtype = serialtype & 0x1;

                    enum_aes_cbc_serial_databit idx_to_enum_databit = (enum_aes_cbc_serial_databit)databit;
                    enum_aes_cbc_serial_databit idx_to_enum_stopbit = (enum_aes_cbc_serial_databit)stopbit;
                    enum_aes_cbc_serial_databit idx_to_enum_paritybit = (enum_aes_cbc_serial_databit)paritybit;
                    enum_aes_cbc_serial_databit idx_to_enum_baudrate = (enum_aes_cbc_serial_databit)baudrate;
                    enum_aes_cbc_serial_databit idx_to_enum_serialtype = (enum_aes_cbc_serial_databit)serialtype;

                    read_log = "Read INDEX(" + i + ") " + idx_to_enum_databit.ToString();
                    read_log += "," + idx_to_enum_stopbit.ToString();
                    read_log += "," + idx_to_enum_paritybit.ToString();
                    read_log += "," + idx_to_enum_baudrate.ToString();
                    read_log += "," + idx_to_enum_serialtype.ToString();

                    //cbxDataBit[i].SelectedIndex = databit;
                    //cbxStopBit[i].SelectedIndex = stopbit;
                    //cbxParityBit[i].SelectedIndex = paritybit;
                    //cbxBaudRate[i].SelectedIndex = baudrate;
                    //cbxSerialType[i].SelectedIndex = serialtype;
                }


            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        public string Serial_Setting_Save(int idx_serial, int baudrate, int databit, int stopbit, int paritybit, int serialtype, ref string write_log)
        {
            string result = "";

            int idx_to_adress = 0;
            //Index Map
            //--databit 0:7bit / 1:8bit
            //--Stopbit 0:1bit / 1:2bit
            //--paritybit 0:none / 1:Odd / 2:Even
            //--baudrate 0:2400 / 1:4800 / 2:9600 / 3:14400 / 4:19200 / 5:38400 / 6:57600 / 7:115200
            //serialtype 0:232/422 / 1:485

            //SDO Address Index
            // 0x8900 = 0
            // 0x8910 = 1
            // 0x8920 = 2
            // 0x8930 = 3
            // 0x8940 = 4
            // 0x8950 = 5
            // 0x8960 = 6
            // 0x8970 = 7

            if (idx_serial == 0) { idx_to_adress = 0x8900; }
            else if (idx_serial == 1) { idx_to_adress = 0x8910; }
            else if (idx_serial == 2) { idx_to_adress = 0x8920; }
            else if (idx_serial == 3) { idx_to_adress = 0x8930; }
            else if (idx_serial == 4) { idx_to_adress = 0x8940; }
            else if (idx_serial == 5) { idx_to_adress = 0x8950; }
            else if (idx_serial == 6) { idx_to_adress = 0x8960; }
            else if (idx_serial == 7) { idx_to_adress = 0x8970; }

            enum_aes_cbc_serial_databit idx_to_enum_databit = (enum_aes_cbc_serial_databit)databit;
            enum_aes_cbc_serial_databit idx_to_enum_stopbit = (enum_aes_cbc_serial_databit)stopbit;
            enum_aes_cbc_serial_databit idx_to_enum_paritybit = (enum_aes_cbc_serial_databit)paritybit;
            enum_aes_cbc_serial_databit idx_to_enum_baudrate = (enum_aes_cbc_serial_databit)baudrate;
            enum_aes_cbc_serial_databit idx_to_enum_serialtype = (enum_aes_cbc_serial_databit)serialtype;

            try
            {
                //Set
                ec.ecSlv_WriteCoeSdo(netID, aescbc_ai_slaveAddr, idx_to_adress, 0x1, 0, 1, ref databit, ref errorCode);
                System.Threading.Thread.Sleep(2);
                ec.ecSlv_WriteCoeSdo(netID, aescbc_ai_slaveAddr, idx_to_adress, 0x2, 0, 1, ref stopbit, ref errorCode);
                System.Threading.Thread.Sleep(2);
                ec.ecSlv_WriteCoeSdo(netID, aescbc_ai_slaveAddr, idx_to_adress, 0x3, 0, 1, ref paritybit, ref errorCode);
                System.Threading.Thread.Sleep(2);
                ec.ecSlv_WriteCoeSdo(netID, aescbc_ai_slaveAddr, idx_to_adress, 0x4, 0, 1, ref baudrate, ref errorCode);
                System.Threading.Thread.Sleep(2);
                ec.ecSlv_WriteCoeSdo(netID, aescbc_ai_slaveAddr, idx_to_adress, 0x5, 0, 1, ref serialtype, ref errorCode);
                System.Threading.Thread.Sleep(2);

                write_log = "Write Req INDEX(" + idx_serial + ") " + idx_to_enum_databit.ToString();
                write_log += "," + idx_to_enum_stopbit.ToString();
                write_log += "," + idx_to_enum_paritybit.ToString();
                write_log += "," + idx_to_enum_baudrate.ToString();
                write_log += "," + idx_to_enum_serialtype.ToString();

                //Save
                // 모듈에 변경값을 기록한다.                 
                int isSave = 0; // Setting 값을 저장하지 않음
                ec.ecSlv_WriteCoeSdo(netID, aescbc_ai_slaveAddr, 0xF001, 0x00, 0, 2, ref isSave, ref errorCode);
                System.Threading.Thread.Sleep(2);

                // Flag 변경이 확인되야 하므로, 0을 먼저 기록하고 1을 기록한다.
                isSave = 1; // Setting 값을 저장
                ec.ecSlv_WriteCoeSdo(netID, aescbc_ai_slaveAddr, 0xF001, 0x00, 0, 2, ref isSave, ref errorCode);

                if (errorCode != 0)
                {
                    result = "Serial_Setting_Save -> Serial Data Save Error";
                    return result;
                }
                write_log += "," + "SAVE SUCESS";

            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        public string Send_Serial_data(int idx_serial, byte[] send_msg)
        {
            //RecveAccept();
            string result = "";
            byte[] inputbyte = new byte[send_byte_cnt];
            AES_CBC.InPDO inPDO_send = new AES_CBC.InPDO();
            AES_CBC.OutPDO outPDO_send = new AES_CBC.OutPDO();
            try
            {
                inPDO_send = (AES_CBC.InPDO)(Marshal.PtrToStructure(inPdoPtr, typeof(AES_CBC.InPDO)));
                outPDO_send = (AES_CBC.OutPDO)(Marshal.PtrToStructure(outPdoPtr, typeof(AES_CBC.OutPDO)));

                if (send_msg.Length > send_byte_cnt) { result = "Send Msg Length Error"; return result; }

                // 선택된 채널의 Serial OutPDO OutputLength 위치
                SerOfs = Marshal.SizeOf(typeof(AES_CBC_AIO_OutPDO)) + Marshal.SizeOf(typeof(AES_CBC_SER_OutPDO)) * idx_serial + 1;
                Marshal.WriteByte(outPdoPtr, SerOfs, (byte)send_msg.Length);

                // DataOut 영역에 Write
                for (int i = 0; i < send_msg.Length; i++)
                {
                    // 선택된 채널의 Serial OutPDO DataOut 위치
                    SerOfs += 1;
                    Marshal.WriteByte(outPdoPtr, SerOfs, (byte)send_msg[i]);
                }

                // Transimit Request 토글
                byte toggle = 0;
                // 선택된 채널의 Serial OutPDO Transmit Request 위치(CtrlByte의 0번 bit)
                SerOfs = Marshal.SizeOf(typeof(AES_CBC_AIO_OutPDO)) + Marshal.SizeOf(typeof(AES_CBC_SER_OutPDO)) * idx_serial;
                if (GETBIT(outPDO_send.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_TransimitRequest) == 1)
                {
                    toggle = SETBIT(outPDO_send.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_TransimitRequest, 0);
                    Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                }
                else
                {
                    toggle = SETBIT(outPDO_send.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_TransimitRequest, 1);
                    Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                }
                RecveAccept(idx_serial);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        /// <summary>
        /// Data가 나뉘어 오기때문에 Parsing 주의
        /// Ex Rcv) 01 04 01 F4 00 01 71 C4
        /// Ex Rcv) 01 -> 04 01 F4 00 -> 01 71 C4 순서로 3번에 걸쳐 들어옴
        /// </summary>
        /// <param name="idx_serial"></param>
        /// <param name="rcv_data"></param>
        /// <returns></returns>
        public string Rcv_Serial_Data(int idx_serial, ref byte[] rcv_data)
        {
            string result = "";
            int data_lengh = 0;
            byte[] inputbyte = new byte[send_byte_cnt];
            byte toggle = 0;
            AES_CBC.InPDO inPDO_rcv = new AES_CBC.InPDO();
            AES_CBC.OutPDO outPDO_rcv = new AES_CBC.OutPDO();
            try
            {
                rcv_data = null;
                if (idx_serial != 0) { return ""; }
                InputString_Hex[idx_serial] = "";
                //Array.Resize(ref rcv_data, send_byte_cnt);
                System.Threading.Thread.Sleep(10);
                inPDO_rcv = (AES_CBC.InPDO)(Marshal.PtrToStructure(inPdoPtr, typeof(AES_CBC.InPDO)));
                outPDO_rcv = (AES_CBC.OutPDO)(Marshal.PtrToStructure(outPdoPtr, typeof(AES_CBC.OutPDO)));

                //Data 1차 수신

                // Receive Request flag가 토글 되면 데이터가 들어온다.
                if (GETBIT(inPDO_rcv.InPDOs_SER[idx_serial].StatusByte, Const.Bit_ReceiveRequest) != receive_request[idx_serial])
                {
                    // InputLength 만큼 버퍼에 저장
                    data_lengh = data_lengh + inPDO_rcv.InPDOs_SER[idx_serial].InputLength;
                    for (int j = 0; j < inPDO_rcv.InPDOs_SER[idx_serial].InputLength; j++)
                    {
                        InputString_Hex[idx_serial] += string.Format("{0:X2} ", inPDO_rcv.InPDOs_SER[idx_serial].DataIn[j]);
                    }
                    // receive request 갱신
                    receive_request[idx_serial] = GETBIT(inPDO_rcv.InPDOs_SER[idx_serial].StatusByte, Const.Bit_ReceiveRequest);
                    // Receive Accepted 토글
                    toggle = 0;
                    // 선택된 채널의 Serial OutPDO Receive Accepted 위치(CtrlByte의 1번 bit)
                    SerOfs = Marshal.SizeOf(typeof(AES_CBC_AIO_OutPDO)) + Marshal.SizeOf(typeof(AES_CBC_SER_OutPDO)) * idx_serial;
                    if (GETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted) == 1)
                    {
                        toggle = SETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted, 0);
                        Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                    }
                    else
                    {
                        //Array.Copy(inPDO.InPDOs_SER[idx_serial].DataIn, rcv_data, inPDO.InPDOs_SER[idx_serial].DataIn.Length);
                        toggle = SETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted, 1);
                        Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                    }
                }
                System.Threading.Thread.Sleep(50);
                //Data 2차 수신

                // Receive Request flag가 토글 되면 데이터가 들어온다.
                if (GETBIT(inPDO_rcv.InPDOs_SER[idx_serial].StatusByte, Const.Bit_ReceiveRequest) != receive_request[idx_serial])
                {
                    // InputLength 만큼 버퍼에 저장
                    data_lengh = data_lengh + inPDO_rcv.InPDOs_SER[idx_serial].InputLength;
                    for (int j = 0; j < inPDO_rcv.InPDOs_SER[idx_serial].InputLength; j++)
                    {
                        InputString_Hex[idx_serial] += string.Format("{0:X2} ", inPDO_rcv.InPDOs_SER[idx_serial].DataIn[j]);
                    }
                    // receive request 갱신
                    receive_request[idx_serial] = GETBIT(inPDO_rcv.InPDOs_SER[idx_serial].StatusByte, Const.Bit_ReceiveRequest);
                    // Receive Accepted 토글
                    toggle = 0;
                    // 선택된 채널의 Serial OutPDO Receive Accepted 위치(CtrlByte의 1번 bit)
                    SerOfs = Marshal.SizeOf(typeof(AES_CBC_AIO_OutPDO)) + Marshal.SizeOf(typeof(AES_CBC_SER_OutPDO)) * idx_serial;
                    if (GETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted) == 1)
                    {
                        toggle = SETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted, 0);
                        Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                    }
                    else
                    {
                        //Array.Copy(inPDO.InPDOs_SER[idx_serial].DataIn, rcv_data, inPDO.InPDOs_SER[idx_serial].DataIn.Length);
                        toggle = SETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted, 1);
                        Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                    }
                }
                System.Threading.Thread.Sleep(50);
                //Data 2차 수신

                // Receive Request flag가 토글 되면 데이터가 들어온다.
                if (GETBIT(inPDO_rcv.InPDOs_SER[idx_serial].StatusByte, Const.Bit_ReceiveRequest) != receive_request[idx_serial])
                {
                    // InputLength 만큼 버퍼에 저장
                    data_lengh = data_lengh + inPDO_rcv.InPDOs_SER[idx_serial].InputLength;
                    for (int j = 0; j < inPDO_rcv.InPDOs_SER[idx_serial].InputLength; j++)
                    {
                        InputString_Hex[idx_serial] += string.Format("{0:X2} ", inPDO_rcv.InPDOs_SER[idx_serial].DataIn[j]);
                    }
                    // receive request 갱신
                    receive_request[idx_serial] = GETBIT(inPDO_rcv.InPDOs_SER[idx_serial].StatusByte, Const.Bit_ReceiveRequest);
                    // Receive Accepted 토글
                    toggle = 0;
                    // 선택된 채널의 Serial OutPDO Receive Accepted 위치(CtrlByte의 1번 bit)
                    SerOfs = Marshal.SizeOf(typeof(AES_CBC_AIO_OutPDO)) + Marshal.SizeOf(typeof(AES_CBC_SER_OutPDO)) * idx_serial;
                    if (GETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted) == 1)
                    {
                        toggle = SETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted, 0);
                        Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                    }
                    else
                    {
                        //Array.Copy(inPDO.InPDOs_SER[idx_serial].DataIn, rcv_data, inPDO.InPDOs_SER[idx_serial].DataIn.Length);
                        toggle = SETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted, 1);
                        Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                    }
                }
                System.Threading.Thread.Sleep(50);
                //Data 3차 수신

                // Receive Request flag가 토글 되면 데이터가 들어온다.
                if (GETBIT(inPDO_rcv.InPDOs_SER[idx_serial].StatusByte, Const.Bit_ReceiveRequest) != receive_request[idx_serial])
                {
                    // InputLength 만큼 버퍼에 저장
                    data_lengh = data_lengh + inPDO_rcv.InPDOs_SER[idx_serial].InputLength;
                    for (int j = 0; j < inPDO_rcv.InPDOs_SER[idx_serial].InputLength; j++)
                    {
                        InputString_Hex[idx_serial] += string.Format("{0:X2} ", inPDO_rcv.InPDOs_SER[idx_serial].DataIn[j]);
                    }
                    // receive request 갱신
                    receive_request[idx_serial] = GETBIT(inPDO_rcv.InPDOs_SER[idx_serial].StatusByte, Const.Bit_ReceiveRequest);
                    // Receive Accepted 토글
                    toggle = 0;
                    // 선택된 채널의 Serial OutPDO Receive Accepted 위치(CtrlByte의 1번 bit)
                    SerOfs = Marshal.SizeOf(typeof(AES_CBC_AIO_OutPDO)) + Marshal.SizeOf(typeof(AES_CBC_SER_OutPDO)) * idx_serial;
                    if (GETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted) == 1)
                    {
                        toggle = SETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted, 0);
                        Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                    }
                    else
                    {
                        //Array.Copy(inPDO.InPDOs_SER[idx_serial].DataIn, rcv_data, inPDO.InPDOs_SER[idx_serial].DataIn.Length);
                        toggle = SETBIT(outPDO_rcv.OutPDOs_SER[idx_serial].CtrlByte, Const.Bit_ReceiveAccepted, 1);
                        Marshal.WriteByte(outPdoPtr, SerOfs, toggle);
                    }
                }
                System.Threading.Thread.Sleep(50);
                //4차에도 정상적이지 않은(짤린) 데이터는 Error
                //데이터가 계속 짤릴 경우 차수마다 Interval sleep 추가 필요
                if (InputString_Hex[idx_serial].Split(' ').Length > 1)
                {
                    Array.Resize(ref rcv_data, InputString_Hex[idx_serial].Split(' ').Length - 1);
                    for (int idx = 0; idx < InputString_Hex[idx_serial].Split(' ').Length - 1; idx++)
                    {
                        rcv_data[idx] = Convert.ToByte(InputString_Hex[idx_serial].Split(' ')[idx], 16);
                    }
                }

            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        #endregion
    }


}
