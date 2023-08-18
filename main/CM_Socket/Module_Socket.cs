using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace cds
{
    public class Module_Socket
    {
        #region "CDS <-> CTC 구조체 통신 메세지"
        // CDS Message
        //    typedef struct
        //    {
        //     union
        //     {
        //          struct
        //          {
        //              DWORD Length;
        //              WORD Flag;
        //              DWORD Token;
        //              DWORD MessageNo;
        //              WORD NetNo;
        //}
        //      Msg;
        //      char Head[16];
        //} MsgHead;

        //union
        //{
        //    strRecipeInfo RecipeInfo;
        //    long AlarmCode;
        //    char Data[2000];
        //}
        //      MsgData;
        //}     CDSMSG, *LPCDSMSG;

        //        - Length : Header 이후에 전송될 데이터의 크기
        //        - Flag : Message Flag
        //          Response OK   0x0001       응답 OK
        //          Response NG   0x0002       응답 NG
        //          Action OK      0x0004       행위 결과(모션, …) OK
        //          Action NG      0x0008       행위 결과(모션, …) NG
        //          Send message  0x0080       송신 메시지일경우 set
        //        - Token : 메시지의 종류
        //        - Message No. : 전송 메시지의 고유 번호
        //        - Net No. : 송신 측에 부여된 고유 네트워크 번호
        //        - Data : Header의 Length와 실제 데이터 크기는 같아야 한다.

        public bool run_state = false;
        public DateTime dt_ctc_validate_check = new DateTime();
        public Thread thd_Database_Sync_By_CTC_Connected;

        //Message No 자동 증가 적용

        private UInt32 _message_no = 0;
        public UInt32 message_no
        {
            get
            {
                if (_message_no > 50000) { _message_no = 0; }
                return _message_no += 1;
            }
            set { _message_no = message_no; }
        }
        public const int head_length = 16;
        public enum DataBase_Changed_Notice_Type
        {
            Alarm = 0x0000,
            Parameter = 0x0001
        }
        public enum Flag
        {
            Response_OK = 0x0001,
            Response_NG = 0x0002,
            Action_OK = 0x0004,
            Action_NG = 0x0008,
            Send_Message = 0x0080,
            Send_Message_simulation = 0x0080,
        }
        public enum TokenList
        {
            Device_Status_100 = 100, //Socket Open 후 1회 전달
            Alarm_Occurred_102 = 102,
            Alarm_Reset_103 = 103,
            Unit_Auto_Mode_104 = 104,
            Unit_Manual_Mode_105 = 105,
            FDC_Data_Send_106 = 106,
            Unit_Version_Info_Send_108 = 108,
            DateBase_Change_Notice_109 = 109, //Prameter Database가 변경되면 알림
            System_Reboot_110 = 110,
            System_Shutdown_111 = 111,
            Chemical_Change_Request_400 = 400,//CTC Supply Start신호, 400 전달 후 401받으면 Supply 시작, 시간 내 못받으면 알람 발생
            Chemical_Change_Confirm_401 = 401,
            Manual_Chemical_Change_402 = 402, //CTC Exchange Request  400 -> 401 시나리오 시작
            Wafer_Count_403 = 403,
            Reclaim_Enabled_404 = 404,
            Job_Linked_405 = 405,
            Stop_Supply_406 = 406,
            IPA_Usage_407 = 407,
            Check_Availability_408 = 408,
            Supply_Enable_Event_Send_450 = 450, //Supply 시작 되고, Reclaim Loop 완료 후 Lal 해당, 해당 되지 않는 Chemical은 0초 후 신호는 전송
            Supply_Disable_Event_Send_451 = 451,
            Chemical_Change_Start_Event_Send_452 = 452, //401 수신 후 진행 할 때
            Chemical_Change_End_Event_Send_453 = 453, // 450 -> 453
            No_Process_Request_Event_454 = 454, //Light, Heavy 알람 발생 시 전달
            No_Process_Request_Cancel_Event_455 = 455,
            Auto_Mode_Event_456 = 456,
            Manual_Mode_Event_457 = 457,
            Tank_A_Supply_Start_Event_Send_458 = 458, //Supply 시작할 때
            Tank_A_Supply_End_Event_Send_459 = 459,
            Tank_B_Supply_Start_Event_Send_460 = 460,
            Tank_B_Supply_End_Event_Send_461 = 461,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Head
        {
            public UInt32 length;               //4              Header 이후에 전송될 데이터의 크기
            public ushort flag;                 //2 -> 4 4Pack으로 고정됨
            public UInt32 token;                //4
            public UInt32 messgeno;             //4
            public ushort networkno;            //2 -> 4
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct Data_None
        {
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct Data_Device_Status
        {
            public UInt32 status;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct Data_Alarm_Occurred
        {
            public Int32 alarmcode;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)] public string remark; //TCHAR Version[512] = [1024]

        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct Database_Changed_Notice
        {
            public Int32 type;
            public Int32 id;
            public float value;
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)] public string unit; //TCHAR Remark [25]
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct IPAUsage
        {
            public Int32 flow_count;
            public float current_flow;
            public float remain_flow;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)] public float[] flow_list;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)] public float[] flow_instantneous_rate_of_change;
        }
        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct stCDS
        {
            public stCDS(bool initialize)
            {
                operationstate = EOperationState.Manual;
                ccss = new stCCSSStatus[4];
                tank = new stTankStatus[3];
                for (int idx = 0; idx < 3; idx++)
                {
                    tank[idx].chemicalvolume = new float[4];
                    tank[idx].concentrationsolution = new float[4];
                }
                supplyTank = new stTankStatus { chemicalvolume = new float[4], concentrationsolution = new float[4] };

                tankcir = new stTankCirStatus[2];
                supply = new stSupplyStatus[2];
                for (int idx = 0; idx < 2; idx++)
                {
                    supply[idx].concentrationsolution = new float[4];
                }
                utility = new stUitlityStatus();
                other = new stOtherStatus();
                for (int idx = 0; idx < 4; idx++)
                {
                    other.dailyUsageAccumulatedvolume = new float[4];
                }
                for (int idx = 0; idx < 2; idx++)
                {
                    other.storagetemp = new float[2];
                }
            }

            public EOperationState operationstate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public stCCSSStatus[] ccss; //stCCSSStatus ccss[4]; // 0:CCSS1, 1:CCSS2, 2:CCSS3, 3:CCSS4
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public stTankStatus[] tank;//stTankStatus tank[3]; // 0:Tank A, 1:Tank B, 2:Tank C
            public stTankStatus supplyTank;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public stTankCirStatus[] tankcir;//[2]; // 0:Tank Cirulcation A, 1:Tank Circulation B
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public stSupplyStatus[] supply;//[2]; // 0:Supply A, 1:Supply B
            public stUitlityStatus utility;
            public stOtherStatus other;
        };
        public enum EOperationState
        {
            Auto = 0,
            Manual = 1
        };
        public enum ETankState
        {
            tsSupply = 0,
            tsDrain = 1,
            tsPrep = 2,
            tsReady = 3,
            tsNoUse = 9
        };
        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct stCCSSStatus
        {
            public float volume;
            public float flowrate;
            public float accumulatedvolume;
            public float pressure;
            public float filterpressure;
        };
        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct stTankStatus
        {
            public ETankState curstate;
            public int lifetime;
            public UInt32 wafercount;
            public float volume;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] chemicalvolume; //4
            public float temperature;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] concentrationsolution; //4
        };
        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct stTankCirStatus
        {
            public int pump_on;
            public float temperature;
        };
        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct stSupplyStatus
        {
            public int pump_on;
            public float flowrate;
            public float temperature;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] concentrationsolution; //4
        };
        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct stUitlityStatus
        {
            public float pcwpressure;
            public float pn2pressure;
            public float cdapressure;
            public float pumpcdapressure;
        };
        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct stOtherStatus
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] dailyUsageAccumulatedvolume; //4
            public float cputemp;
            public float mainboardtemp;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public float[] storagetemp; //2
            public UInt32 cpufanspeed;
            public UInt32 mainboardfanspeed;
            public UInt32 otherfanspeed;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct Data_Version_Info
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string version; //TCHAR Version[32] = [64]
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct Chemical_Change_Event
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)] public string supplytank; //TCHAR Version[1] = [2]
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct Data_Reclaim_Enable
        {
            public bool reclaim_enable;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct Data_Job_Linked
        {
            public bool job_linked;
        }
        public byte[] GetBytes_By_Packet<T>(Head head, T data)
        {
            byte[] btBuffer = new byte[Marshal.SizeOf(head)];
            MemoryStream ms = new MemoryStream(btBuffer, true);
            BinaryWriter bw = new BinaryWriter(ms);
            byte[] bt_PWD;
            try
            {
                //구조체 사이즈 
                int head_size = Marshal.SizeOf(head);
                int data_size = Marshal.SizeOf(data);

                //data_size 구조체가 1인경우 None Type
                if (data_size == 1) { data_size = 0; }

                //사이즈 만큼 메모리 할당 받기
                byte[] return_array = new byte[head_size + data_size];

                //구조체 주소값 가져오기
                IntPtr head_ptr = Marshal.AllocHGlobal(head_size);
                Marshal.StructureToPtr(head, head_ptr, false);
                Marshal.Copy(head_ptr, return_array, 0, head_size);
                Marshal.FreeHGlobal(head_ptr);

                if (data_size != 0)
                {
                    IntPtr data_ptr = Marshal.AllocHGlobal(data_size);
                    Marshal.StructureToPtr(data, data_ptr, false);
                    Marshal.Copy(data_ptr, return_array, head_size, data_size);
                    Marshal.FreeHGlobal(data_ptr);
                }

                // Program.main_md.Save_ByteArray_Log("Send:  ", return_array);
                return return_array;
                //Header

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (bw != null) { bw.Close(); bw.Dispose(); bw = null; }
                if (ms != null) { ms.Close(); ms.Dispose(); ms = null; }
            }
            return btBuffer;
        }
        #endregion "CDS <-> CTC 구조체 통신 메세지"
        public void Socket_Data_Parse(TCP_IP_SOCKET.Cls_Socket.Socket_Event socket_event)
        {
            //Recieve Socket Packet To Data

            TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info send_info = new TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info();
            send_info.ip = socket_event.info_ip;
            send_info.port = socket_event.info_port;
            Module_Socket.Head rcv_head = new Module_Socket.Head();
            Module_Socket.Head send_head = new Module_Socket.Head();
            Byte[] full_data;
            UInt16 send_flag;
            string data_parse = "";
            string data_parse2 = "";
            string tmp_value = "";
            bool check_parse_result = true;
            string log = "";
            try
            {
                if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Open)
                {
                    log = "CTC Socket Open";
                    dt_ctc_validate_check = DateTime.Now;
                    run_state = true;
                    Program.main_form.Insert_System_Message("CTC Connected");
                    Program.eventlog_form.Insert_Event("CTC Connected", (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                    Program.main_md.Save_ByteArray_Rcv_Log("CTC Connected -> " + DateTime.Now.ToString("HH:mm:ss.fff"), null);
                    Database_And_Alarm_Sync_By_CTC_Connected();

                }
                else if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Close)
                {
                    log = "CTC Socket Close";
                    run_state = false;
                    Program.main_form.Insert_System_Message("CTC Disconnected");
                    Program.eventlog_form.Insert_Event("CTC Disconnected", (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                    Program.main_md.Save_ByteArray_Rcv_Log("CTC Disconnected -> " + DateTime.Now.ToString("HH:mm:ss.fff"), null);
                }
                else if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Recieve)
                {
                    dt_ctc_validate_check = DateTime.Now;
                    run_state = true;

                    int int_buff_length;
                    byte[] byte_buff;
                    byte[] copy_data;
                    int_buff_length = socket_event.data_length;

                    while (int_buff_length >= Module_Socket.head_length)
                    {
                        byte_buff = new byte[int_buff_length];
                        System.Array.Copy(socket_event.data_to_array, socket_event.data_length - int_buff_length, byte_buff, 0, byte_buff.Length);
                        rcv_head = Parse_Packet_Head(byte_buff);
                        copy_data = new byte[rcv_head.length];
                        Array.Copy(byte_buff, 16, copy_data, 0, rcv_head.length);

                        //if (socket_event.data_length < Module_Socket.head_length)
                        //{
                        //    check_parse_result = false;
                        //}
                        check_parse_result = true;
                        //socket_event.data_to_array -> byte_buff;
                        //수신 데이터가 이상이 없을 경우
                        if (check_parse_result == true)
                        {
                            //수신 데이터 파싱
                            //rcv_head = Parse_Packet_Head(byte_buff);
                            //Program.main_md.Save_ByteArray_Rcv_Log("Rcv : LENGTH : " + rcv_head.length + ", FLAG : " + rcv_head.flag.ToString("X") + ", TOKEN : " + rcv_head.token + ", MSGNO : " + rcv_head.messgeno + ", NETNO : " + rcv_head.networkno + socket_event.info_ip_port_type1, byte_buff);
                            //파싱된 토큰값으로 메시지 구분
                            if (rcv_head.token == (Int32)Module_Socket.TokenList.Device_Status_100)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Device_Status_100);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_Device_Status send_Data = new Module_Socket.Data_Device_Status();
                                    send_Data.status = GetCDSStatus();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Device_Status_100);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Alarm_Occurred_102)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Alarm_Occurred_102);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Alarm_Occurred_102);
                                }

                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Alarm_Reset_103)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Alarm_Reset_103);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Program.occured_alarm_form.message_no = rcv_head.messgeno;
                                    Program.occured_alarm_form.token = rcv_head.token;
                                    Program.occured_alarm_form.req_alarm_reset_by_ctc = true;
                                }

                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.FDC_Data_Send_106)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.FDC_Data_Send_106);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.FDC_Data_Send_106);
                                }
                            }

                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Unit_Version_Info_Send_108)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Unit_Version_Info_Send_108);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_Version_Info send_Data = new Module_Socket.Data_Version_Info();
                                    send_Data.version = Application.ProductVersion.ToString();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Unit_Version_Info_Send_108);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.DateBase_Change_Notice_109)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.DateBase_Change_Notice_109);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.DateBase_Change_Notice_109);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.System_Reboot_110)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.System_Reboot_110);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.System_Reboot_110);
                                    System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.System_Shutdown_111)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.System_Shutdown_111);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.System_Shutdown_111);
                                    System.Diagnostics.Process.Start("shutdown.exe", "/s /t 0");
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Chemical_Change_Request_400)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Chemical_Change_Request_400);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    //Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    //send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    //send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    //full_data = GetBytes_By_Packet(send_head, send_Data);
                                    //Message_Send(ref full_data, send_head, Module_Socket.TokenList.Chemical_Change_Request_400);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Chemical_Change_Confirm_401)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Chemical_Change_Confirm_401);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Chemical_Change_Confirm_401);

                                    //Tank Change 
                                    if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                                    {
                                        if (Program.seq.supply.c_c_need == true || Program.seq.supply.ready_flag == true)
                                        {
                                            if (Program.seq.supply.req_c_c_start_cds_to_ctc == true)
                                            {
                                                //Program.seq.supply.ctc_supply_request = true;
                                                Program.seq.supply.rep_c_c_start_cds_to_ctc = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Program.seq.supply.req_c_c_start_cds_to_ctc == true)
                                        {
                                            //Program.seq.supply.ctc_supply_request = true;
                                            Program.seq.supply.rep_c_c_start_cds_to_ctc = true;
                                        }
                                    }

                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Manual_Chemical_Change_402)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Manual_Chemical_Change_402);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Manual_Chemical_Change_402);
                                    Program.seq.supply.ctc_c_c_request = true;
                                    if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY ||
                                        Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.LIGHT)
                                    {
                                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Fail_Chemical_Change_Command, "", true, false);
                                    }
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Wafer_Count_403)
                            {

                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Wafer_Count_403);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Wafer_Count_403);

                                    if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                    {
                                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt = Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt + 1;
                                    }
                                    else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                    {
                                        Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt = Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt + 1;
                                    }
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Reclaim_Enabled_404)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Reclaim_Enabled_404);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_Reclaim_Enable rcv_data_parse = new Module_Socket.Data_Reclaim_Enable();
                                    rcv_data_parse.reclaim_enable = BitConverter.ToBoolean(byte_buff, 16);

                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Reclaim_Enabled_404);

                                    if (Program.cg_app_info.eq_type == enum_eq_type.lal && Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                                    {
                                        Program.seq.supply.ctc_reclaim_request = rcv_data_parse.reclaim_enable;
                                        if (Program.seq.supply.ctc_reclaim_request == true)
                                        {
                                            if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Reclaim_Flush) != 0)
                                            {
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RECLAIM_DRAIN, true);
                                                Program.schematic_form.Reclaim_Rcv = true; //frm_schematic -> timer -> Reclaim_Signal_Check 에서 호출
                                            }
                                        }
                                        else
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RECLAIM_DRAIN, true);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_RECLAIM_TO_TANK_A, false);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_RECLAIM_TO_TANK_B, false);
                                        }
                                    }


                                }

                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Job_Linked_405)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Job_Linked_405);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_Job_Linked rcv_data_parse = new Module_Socket.Data_Job_Linked();
                                    rcv_data_parse.job_linked = BitConverter.ToBoolean(byte_buff, 16);

                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Job_Linked_405);

                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Stop_Supply_406)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Stop_Supply_406);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None rcv_data_parse = new Module_Socket.Data_None();
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Stop_Supply_406);

                                    Program.eventlog_form.Insert_Event("Rcv Stop_Supply_406 -> All Stop ", (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                    Program.schematic_form.All_Stop();
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.IPA_Usage_407)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.IPA_Usage_407);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.IPAUsage rcv_data_parse = new Module_Socket.IPAUsage();
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();

                                    //byte[] type = new byte[4];
                                    //byte[] id = new byte[4];
                                    //byte[] value = new byte[4];
                                    //byte[] unit = new byte[50];
                                    //string str_unit = "";
                                    //Array.Copy(byte_buff, 20, type, 0, 4);
                                    //Array.Copy(byte_buff, 24, id, 0, 4);
                                    //Array.Copy(byte_buff, 28, value, 0, 4);
                                    //Array.Copy(byte_buff, 32, unit, 0, 50);
                                    //rcv_data_parse.type = BitConverter.ToInt32(type, 0);
                                    //rcv_data_parse.id = BitConverter.ToInt32(id, 0);
                                    //rcv_data_parse.value = BitConverter.ToSingle(value, 0);


                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.IPA_Usage_407);

                                    if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                                    {
                                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            Program.schematic_form.Tank_Supply_End(Program.seq.supply.cur_tank);
                                        }
                                        else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            Program.schematic_form.Tank_Supply_End(Program.seq.supply.cur_tank);
                                        }
                                    }

                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Check_Availability_408)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Check_Availability_408);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();

                                    if (Program.schematic_form.Check_Availability_response_status == true)
                                    {
                                        send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    }
                                    else
                                    {
                                        send_flag = (ushort)Module_Socket.Flag.Response_NG;
                                    }
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Check_Availability_408);
                                }
                                else if (rcv_head.flag == (Int32)Module_Socket.Flag.Response_OK)
                                {
                                    //Tank Change 
                                    if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                                    {
                                        if (Program.seq.supply.c_c_need == true || Program.seq.supply.ready_flag == true)
                                        {
                                            if (Program.seq.supply.req_c_c_start_cds_to_ctc == true)
                                            {
                                                Program.seq.supply.ctc_supply_request = true;
                                                Program.seq.supply.rep_c_c_start_cds_to_ctc = true;
                                                Program.eventlog_form.Insert_Event("Check_Availability_408 Response OK", (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                            }
                                        }
                                    }

                                }
                                else if (rcv_head.flag == (Int32)Module_Socket.Flag.Response_NG)
                                {
                                    Program.eventlog_form.Insert_Event("Check_Availability_408 Response NG", (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                    Program.schematic_form.All_Stop();
                                    Program.main_form.BeginInvoke(new frm_main.Del_Message_By_Application(Program.main_md.Message_By_Application), "Check_Availability_408 Response NG", frm_messagebox.enum_apptype.Only_OK);
                                }

                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Supply_Enable_Event_Send_450)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Supply_Enable_Event_Send_450);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Supply_Enable_Event_Send_450);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Supply_Disable_Event_Send_451)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Supply_Disable_Event_Send_451);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Supply_Disable_Event_Send_451);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Chemical_Change_Start_Event_Send_452)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Chemical_Change_Start_Event_Send_452);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Chemical_Change_Start_Event_Send_452);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Chemical_Change_End_Event_Send_453)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Chemical_Change_End_Event_Send_453);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Chemical_Change_End_Event_Send_453);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.No_Process_Request_Event_454)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.No_Process_Request_Event_454);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.No_Process_Request_Event_454);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.No_Process_Request_Cancel_Event_455)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.No_Process_Request_Cancel_Event_455);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.No_Process_Request_Cancel_Event_455);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Auto_Mode_Event_456)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Auto_Mode_Event_456);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Auto_Mode_Event_456);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Manual_Mode_Event_457)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Manual_Mode_Event_457);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Manual_Mode_Event_457);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Tank_A_Supply_Start_Event_Send_458)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Tank_A_Supply_Start_Event_Send_458);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Tank_A_Supply_Start_Event_Send_458);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Tank_A_Supply_End_Event_Send_459)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Tank_A_Supply_End_Event_Send_459);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Tank_A_Supply_End_Event_Send_459);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Tank_B_Supply_Start_Event_Send_460)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Tank_B_Supply_Start_Event_Send_460);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Tank_B_Supply_Start_Event_Send_460);
                                }
                            }
                            else if (rcv_head.token == (Int32)Module_Socket.TokenList.Tank_B_Supply_End_Event_Send_461)
                            {
                                Message_Rcv(ref byte_buff, rcv_head, Module_Socket.TokenList.Tank_B_Supply_End_Event_Send_461);
                                if (rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message || rcv_head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                                {
                                    Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                                    send_flag = (ushort)Module_Socket.Flag.Response_OK;
                                    send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, rcv_head.token, rcv_head.messgeno, Program.cg_socket.ctc_network_no);
                                    full_data = GetBytes_By_Packet(send_head, send_Data);
                                    Message_Send(ref full_data, send_head, Module_Socket.TokenList.Tank_B_Supply_End_Event_Send_461);
                                }
                            }
                            else
                            {
                                Program.main_md.Save_ByteArray_Rcv_Log("Rcv Unkown Meesage :  " + send_info.ip + ":" + send_info.port, byte_buff);
                            }

                        }
                        else
                        {
                            Program.main_md.Save_ByteArray_Rcv_Log("Rcv Meesage Data Length Fail :  " + send_info.ip + ":" + send_info.port, byte_buff);
                        }

                        int_buff_length = int_buff_length - ((int)rcv_head.length + Module_Socket.head_length);
                        byte_buff = null;
                    }


                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite("Module_Socket" + "." + "Socket_Data_Parse" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally
            {

            }
        }
        public Module_Socket.Head Make_Packet_Head(UInt32 length, ushort flag, UInt32 token, UInt32 messageno, ushort networkno)
        {
            Module_Socket.Head head = new Module_Socket.Head();
            try
            {
                head.length = length;
                head.flag = flag;
                head.token = token;
                head.messgeno = messageno;
                head.networkno = networkno;
            }
            catch (Exception ex)
            { Program.log_md.LogWrite("Module_Socket" + "." + "Make_Packet_Head" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally
            {
            }
            return head;
        }
        public Module_Socket.Head Parse_Packet_Head(byte[] head_array)
        {
            Module_Socket.Head head = new Module_Socket.Head();
            try
            {
                head.length = BitConverter.ToUInt32(head_array, 0);
                head.flag = BitConverter.ToUInt16(head_array, 4);
                head.token = BitConverter.ToUInt32(head_array, 6);
                head.messgeno = BitConverter.ToUInt32(head_array, 10);
                head.networkno = BitConverter.ToUInt16(head_array, 14);

            }
            catch (Exception ex)
            { Program.log_md.LogWrite("Module_Socket" + "." + "Make_Packet_Head" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally
            {
            }
            return head;
        }
        public void Message_Send(ref byte[] full_data, Module_Socket.Head head, Module_Socket.TokenList tokenList)
        {
            if (Program.CTC.run_state == true)
            {
                TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info connection_info = new TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info();
                if (Program.cg_socket.ctc_ip_use_send == true)
                {
                    connection_info.ip = Program.cg_socket.ctc_ip; connection_info.port = Convert.ToInt32(Program.cg_socket.ctc_port);
                }
                else
                {
                    connection_info.ip = Program.SOCKET.ctc_connected_ip; connection_info.port = Convert.ToInt32(Program.SOCKET.ctc_connected_port);
                    if (connection_info.ip == "")
                    {
                        Program.main_md.Save_ByteArray_Send_Log("Send Fail CTC Not Connected -> " + DateTime.Now.ToString("HH:mm:ss.fff : ") + "(" + tokenList.ToString() + ")," + "Len : " + head.length + ", Flag : " + head.flag.ToString("X") + ", token : " + head.token + ", msgno : " + head.messgeno + ", netno : " + head.networkno + ", " + Program.cg_socket.ctc_ip + ":" + Program.cg_socket.ctc_port, full_data);
                        return;
                    }
                }
                Program.SOCKET.SendMsg(connection_info, full_data);
                Program.main_md.Save_ByteArray_Send_Log("Send -> " + DateTime.Now.ToString("HH:mm:ss.fff : ") + "(" + tokenList.ToString() + ")," + "Len : " + head.length + ", Flag : " + head.flag.ToString("X") + ", token : " + head.token + ", msgno : " + head.messgeno + ", netno : " + head.networkno + ", " + Program.cg_socket.ctc_ip + ":" + Program.cg_socket.ctc_port, full_data);
                if (head.flag == (Int32)Module_Socket.Flag.Send_Message || head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
                {
                    if (tokenList != TokenList.FDC_Data_Send_106)
                    {
                        Message_Manager_Add((int)tokenList, connection_info.ip, connection_info.port, head.messgeno, ref full_data);
                    }
                    if (tokenList != TokenList.Alarm_Occurred_102 && tokenList != TokenList.FDC_Data_Send_106 && tokenList != TokenList.Wafer_Count_403)
                    {
                        Program.main_form.Insert_System_Message("Send -> " + tokenList.ToString());
                        Program.eventlog_form.Insert_Event("CDS Send : " + tokenList.ToString(), (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                    }

                }
            }

        }
        public void Message_Manager_Add(int tokenID, string ip, int port, UInt32 msg_no, ref byte[] data)
        {

            if (Program.cg_socket.message_retry_cnt != 0)
            {
                Program.main_form.dt_message_manage.Rows.Add(DateTime.Now, DateTime.Now, DateTime.Now.ToString(""), tokenID, ip, port, msg_no, 0, data, false);
            }

        }
        public void Message_Manager_Remove(int tokenID, UInt32 msg_no)
        {
            UInt32 out_msg_no;
            if (Program.cg_socket.message_retry_cnt != 0)
            {
                Program.main_form.queue_delete_manage.Enqueue(msg_no);
                if (Program.main_form.queue_delete_manage.Count >= 100) { Program.main_form.queue_delete_manage.TryDequeue(out msg_no); }
            }
        }
        public void Message_Rcv(ref byte[] full_data, Module_Socket.Head head, Module_Socket.TokenList tokenList)
        {
            TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info connection_info = new TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info();
            connection_info.ip = Program.cg_socket.ctc_ip; connection_info.port = Convert.ToInt32(Program.cg_socket.ctc_port);
            Program.main_md.Save_ByteArray_Rcv_Log("Rcv -> " + DateTime.Now.ToString("HH:mm:ss.fff : ") + "(" + tokenList.ToString() + ")," + "Len : " + head.length + ", Flag : " + head.flag.ToString("X") + ", token : " + head.token + ", msgno : " + head.messgeno + ", netno : " + head.networkno + ", " + Program.cg_socket.ctc_ip + ":" + Program.cg_socket.ctc_port, full_data);
            if (head.flag == (Int32)Module_Socket.Flag.Send_Message || head.flag == (Int32)Module_Socket.Flag.Send_Message_simulation)
            {
                if (tokenList != TokenList.Alarm_Occurred_102 && tokenList != TokenList.FDC_Data_Send_106 && tokenList != TokenList.Wafer_Count_403)
                {
                    Program.main_form.Insert_System_Message("Rcv -> " + tokenList.ToString());
                    Program.eventlog_form.Insert_Event("CTC Rcv : " + tokenList.ToString(), (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                }
            }
            else if (head.flag == (Int32)Module_Socket.Flag.Response_OK || head.flag == (Int32)Module_Socket.Flag.Response_NG)
            {
                Message_Manager_Remove((int)tokenList, head.messgeno);
            }
        }
        public void Alarm_Clear_Response()
        {
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            Module_Socket.Head rcv_head = new Module_Socket.Head();
            Module_Socket.Head send_head = new Module_Socket.Head();
            Byte[] full_data;
            UInt16 send_flag;
            int alarm_cnt = 0;
            alarm_cnt = Program.occured_alarm_form.cnt_occured_alarm_light_total + Program.occured_alarm_form.cnt_occured_alarm_heavy_total;
            DataTable dt_alarm_copy = Program.occured_alarm_form.dt_alarm.Copy();


            if (alarm_cnt == 0)
            {
                send_flag = (ushort)Module_Socket.Flag.Response_OK;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Response_NG;
            }
            //RCV 후 Clear 조건에 따라 다름
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, Program.occured_alarm_form.token, Program.occured_alarm_form.message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Alarm_Reset_103);

            //Occured Alarm 전송 Sync
            for (int idx = 0; idx < dt_alarm_copy.Rows.Count; idx++)
            {
                Program.CTC.Message_Alarm_Occurred_102(Convert.ToInt32(dt_alarm_copy.Rows[idx]["NO"]), dt_alarm_copy.Rows[idx]["remark"].ToString());
                //Program.CTC.Message_Alarm_Occurred_102(Convert.ToInt32(dt_alarm_copy.Rows[idx]["NO"]), ".");
            }
        }
        public uint GetCDSStatus()
        {
            string binary_status = "";// "00000000000"; //0 ~ 10 사용 안함
            UInt32 status = 0;

            // Ready
            if (Program.cg_apploading.load_complete == true && Program.occured_alarm_form.cnt_occured_alarm_heavy_total == 0 && Program.seq.supply.CDS_enable_status_to_ctc == true)
            {
                binary_status = binary_status + "1";
            }
            else
            {
                binary_status = binary_status + "0";
            }

            // Chemical Change Request
            if (Program.seq.supply.req_c_c_start_cds_to_ctc == true)
            {
                binary_status = binary_status + "1";
            }
            else
            {
                binary_status = binary_status + "0";
            }
            // Light Alarm
            if (Program.occured_alarm_form.cnt_occured_alarm_light_total == 0)
            {
                binary_status = binary_status + "0";
            }
            else
            {
                binary_status = binary_status + "1";
            }

            // Heavy Alarm
            if (Program.occured_alarm_form.cnt_occured_alarm_heavy_total == 0)
            {
                binary_status = binary_status + "0";
            }
            else
            {
                binary_status = binary_status + "1";
            }
            // Mode
            if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
            {
                binary_status = binary_status + "1";
            }
            else
            {
                binary_status = binary_status + "0";
            }

            binary_status = binary_status + "00000000000"; //0 ~ 10 사용 안함
            status = Convert.ToUInt32(binary_status, 2);

            return status;
        }
        public void Message_Device_Status_100()
        {

            Byte[] full_data;
            UInt16 send_flag;
            string binary_status = "";// "00000000000"; //0 ~ 10 사용 안함
            UInt32 status = 0;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_Device_Status send_Data = new Module_Socket.Data_Device_Status();

            // Ready
            if (Program.cg_apploading.load_complete == true && Program.occured_alarm_form.cnt_occured_alarm_heavy_total == 0)
            {
                binary_status = binary_status + "1";
            }
            else
            {
                binary_status = binary_status + "0";
            }

            // Chemical Change Request


            // TODO: implementation
            binary_status = binary_status + "0";

            // Light Alarm
            if (Program.occured_alarm_form.cnt_occured_alarm_light_total == 0)
            {
                binary_status = binary_status + "0";
            }
            else
            {
                binary_status = binary_status + "1";
            }

            // Heavy Alarm
            if (Program.occured_alarm_form.cnt_occured_alarm_heavy_total == 0)
            {
                binary_status = binary_status + "0";
            }
            else
            {
                binary_status = binary_status + "1";
            }

            // Mode
            if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
            {
                binary_status = binary_status + "1";
            }
            else
            {
                binary_status = binary_status + "0";
            }

            binary_status = binary_status + "00000000000"; //0 ~ 10 사용 안함
                                                           //binary_status_reverse = new string(binary_status.Reverse().ToArray());
            status = Convert.ToUInt32(binary_status, 2);

            send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;

            send_Data.status = status;
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Device_Status_100, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Device_Status_100);
        }
        public void Message_Alarm_Occurred_102(Int32 alarmcode, string remark)
        {

            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_Alarm_Occurred send_Data = new Module_Socket.Data_Alarm_Occurred();
            send_Data.alarmcode = alarmcode;
            send_Data.remark = remark;
            //send_Data.remark = ".";
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Alarm_Occurred_102, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Alarm_Occurred_102);
        }
        /// <summary>
        /// CTC -> Send Test를 위해 생성
        /// </summary>
        public void Message_Alarm_Reset_103()
        {

            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Alarm_Reset_103, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Alarm_Reset_103);
        }
        public void Message_FDC_Data_Send_106()
        {

            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            //FDC_Data_Send send_Data = new FDC_Data_Send(true);
            stCDS send_Data = new stCDS(true);

            try
            {
                if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                {
                    send_Data.operationstate = EOperationState.Manual;
                }
                else
                {
                    send_Data.operationstate = EOperationState.Auto;
                }

                //send_Data.ccss = new ST_CCSSStatus[4];
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    //CCSS1

                    send_Data.ccss[0].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage);
                    send_Data.ccss[0].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow);
                    send_Data.ccss[0].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage);
                    send_Data.ccss[0].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_IN_PRESS].value));
                    send_Data.ccss[0].filterpressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_OUT_PRESS].value));
                    //CCSS2
                    send_Data.ccss[1].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.NH4OH_totalusage);
                    send_Data.ccss[1].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.NH4OH_flow);
                    send_Data.ccss[1].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.NH4OH_totalusage);
                    send_Data.ccss[1].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.NH4OH_SUPPLY_FILTER_IN_PRESS].value));
                    send_Data.ccss[1].filterpressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.NH4OH_SUPPLY_FILTER_OUT_PRESS].value));
                    //CCSS3
                    send_Data.ccss[2].volume = 0;
                    send_Data.ccss[2].flowrate = 0;
                    send_Data.ccss[2].accumulatedvolume = 0;
                    send_Data.ccss[2].pressure = 0;
                    send_Data.ccss[2].filterpressure = 0;
                    //CCSS4
                    send_Data.ccss[3].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage);
                    send_Data.ccss[3].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.DIW_flow);
                    send_Data.ccss[3].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage);
                    send_Data.ccss[3].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value));
                    send_Data.ccss[3].filterpressure = 0;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    //CCSS1
                    send_Data.ccss[0].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.IPA_totalusage);
                    send_Data.ccss[0].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow);
                    send_Data.ccss[0].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.IPA_totalusage);
                    send_Data.ccss[0].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_IN_PRESS].value));
                    send_Data.ccss[0].filterpressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_OUT_PRESS].value));
                    //CCSS2
                    send_Data.ccss[1].volume = 0;
                    send_Data.ccss[1].flowrate = 0;
                    send_Data.ccss[1].accumulatedvolume = 0;
                    send_Data.ccss[1].pressure = 0;
                    send_Data.ccss[1].filterpressure = 0;
                    //CCSS3
                    send_Data.ccss[2].volume = 0;
                    send_Data.ccss[2].flowrate = 0;
                    send_Data.ccss[2].accumulatedvolume = 0;
                    send_Data.ccss[2].pressure = 0;
                    send_Data.ccss[2].filterpressure = 0;
                    //CCSS4
                    send_Data.ccss[3].volume = 0;
                    send_Data.ccss[3].flowrate = 0;
                    send_Data.ccss[3].accumulatedvolume = 0;
                    send_Data.ccss[3].pressure = 0;
                    send_Data.ccss[3].filterpressure = 0;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    //CCSS1
                    send_Data.ccss[0].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage);
                    send_Data.ccss[0].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DSP_flow);
                    send_Data.ccss[0].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage);
                    send_Data.ccss[0].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DSP_SUPPLY_FILTER_IN_PRESS].value));
                    send_Data.ccss[0].filterpressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DSP_SUPPLY_FILTER_OUT_PRESS].value));
                    //CCSS2
                    send_Data.ccss[1].volume = 0;
                    send_Data.ccss[1].flowrate = 0;
                    send_Data.ccss[1].accumulatedvolume = 0;
                    send_Data.ccss[1].pressure = 0;
                    send_Data.ccss[1].filterpressure = 0;
                    //CCSS3
                    send_Data.ccss[2].volume = 0;
                    send_Data.ccss[2].flowrate = 0;
                    send_Data.ccss[2].accumulatedvolume = 0;
                    send_Data.ccss[2].pressure = 0;
                    send_Data.ccss[2].filterpressure = 0;
                    //CCSS4
                    send_Data.ccss[3].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage);
                    send_Data.ccss[3].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow);
                    send_Data.ccss[3].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage);
                    send_Data.ccss[3].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value));
                    send_Data.ccss[3].filterpressure = 0;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    //CCSS1
                    send_Data.ccss[0].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage);
                    send_Data.ccss[0].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.HF_flow);
                    send_Data.ccss[0].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage);
                    send_Data.ccss[0].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS].value));
                    send_Data.ccss[0].filterpressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS].value));
                    //CCSS2
                    send_Data.ccss[1].volume = 0;
                    send_Data.ccss[1].flowrate = 0;
                    send_Data.ccss[1].accumulatedvolume = 0;
                    send_Data.ccss[1].pressure = 0;
                    send_Data.ccss[1].filterpressure = 0;
                    //CCSS3
                    send_Data.ccss[2].volume = 0;
                    send_Data.ccss[2].flowrate = 0;
                    send_Data.ccss[2].accumulatedvolume = 0;
                    send_Data.ccss[2].pressure = 0;
                    send_Data.ccss[2].filterpressure = 0;
                    //CCSS4
                    send_Data.ccss[3].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage);
                    send_Data.ccss[3].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.DIW_flow);
                    send_Data.ccss[3].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage);
                    send_Data.ccss[3].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value));
                    send_Data.ccss[3].filterpressure = 0;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    //CCSS1
                    send_Data.ccss[0].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.LAL_totalusage);
                    send_Data.ccss[0].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.LAL_flow);
                    send_Data.ccss[0].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.LAL_totalusage);
                    send_Data.ccss[0].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.LAL_SUPPLY_FILTER_IN_PRESS].value));
                    send_Data.ccss[0].filterpressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.LAL_SUPPLY_FILTER_OUT_PRESS].value));
                    //CCSS2
                    send_Data.ccss[1].volume = 0;
                    send_Data.ccss[1].flowrate = 0;
                    send_Data.ccss[1].accumulatedvolume = 0;
                    send_Data.ccss[1].pressure = 0;
                    send_Data.ccss[1].filterpressure = 0;
                    //CCSS3
                    send_Data.ccss[2].volume = 0;
                    send_Data.ccss[2].flowrate = 0;
                    send_Data.ccss[2].accumulatedvolume = 0;
                    send_Data.ccss[2].pressure = 0;
                    send_Data.ccss[2].filterpressure = 0;
                    //CCSS4
                    send_Data.ccss[3].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage);
                    send_Data.ccss[3].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow);
                    send_Data.ccss[3].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage);
                    send_Data.ccss[3].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value));
                    send_Data.ccss[3].filterpressure = 0;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    //CCSS1
                    send_Data.ccss[0].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage);
                    send_Data.ccss[0].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow);
                    send_Data.ccss[0].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage);
                    send_Data.ccss[0].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_IN_PRESS].value));
                    send_Data.ccss[0].filterpressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_OUT_PRESS].value));
                    //CCSS2
                    send_Data.ccss[1].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.H2SO4_totalusage);
                    send_Data.ccss[1].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.H2SO4_flow);
                    send_Data.ccss[1].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.H2SO4_totalusage);
                    send_Data.ccss[1].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2SO4_SUPPLY_FILTER_IN_PRESS].value));
                    send_Data.ccss[1].filterpressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2SO4_SUPPLY_FILTER_OUT_PRESS].value));
                    //CCSS3
                    send_Data.ccss[2].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage);
                    send_Data.ccss[2].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.HF_flow);
                    send_Data.ccss[2].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage);
                    send_Data.ccss[2].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS].value));
                    send_Data.ccss[2].filterpressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS].value));
                    //CCSS4
                    send_Data.ccss[3].volume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage);
                    send_Data.ccss[3].flowrate = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow);
                    send_Data.ccss[3].accumulatedvolume = Convert.ToSingle(Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage);
                    send_Data.ccss[3].pressure = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value));
                    send_Data.ccss[3].filterpressure = 0;
                }
                // send_Data.tank = new ST_TankStatus[3];
                if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].enable == false)
                {
                    send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].curstate = ETankState.tsNoUse;
                }
                else if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.DRAIN ||
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.DRAIN_WAIT)
                {
                    send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].curstate = ETankState.tsDrain;
                }
                else if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.CHARGE)
                {
                    send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].curstate = ETankState.tsPrep;
                }
                else if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.READY)
                {
                    send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].curstate = ETankState.tsReady;
                }
                else if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY ||
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.REFILL)
                {
                    send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].curstate = ETankState.tsSupply;
                }

                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].lifetime = Convert.ToInt32(Program.tank[(int)tank_class.enum_tank_type.TANK_A].life_time_to_minute);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].wafercount = Convert.ToUInt32(Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].volume = Convert.ToUInt32(Program.tank[(int)tank_class.enum_tank_type.TANK_A].total_volume);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].temperature = Convert.ToSingle(string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv));

                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].chemicalvolume = new float[4];
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].chemicalvolume[0] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].chemicalvolume[1] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].chemicalvolume[2] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].chemicalvolume[3] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss4].input_volume);

                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].concentrationsolution = new float[4];
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].concentrationsolution[0] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss1);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].concentrationsolution[1] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss2);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].concentrationsolution[2] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss3);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_A].concentrationsolution[3] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss4);


                if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].enable == false)
                {
                    send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].curstate = ETankState.tsNoUse;
                }
                else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.DRAIN ||
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.DRAIN_WAIT)
                {
                    send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].curstate = ETankState.tsDrain;
                }
                else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.CHARGE)
                {
                    send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].curstate = ETankState.tsPrep;
                }
                else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.READY)
                {
                    send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].curstate = ETankState.tsReady;
                }
                else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY ||
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.REFILL)
                {
                    send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].curstate = ETankState.tsSupply;
                }


                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].lifetime = Convert.ToInt32(Program.tank[(int)tank_class.enum_tank_type.TANK_B].life_time_to_minute);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].wafercount = Convert.ToUInt32(Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].volume = Convert.ToUInt32(Program.tank[(int)tank_class.enum_tank_type.TANK_B].total_volume);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].temperature = Convert.ToSingle(string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv));

                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].chemicalvolume = new float[4];
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].chemicalvolume[0] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].chemicalvolume[1] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].chemicalvolume[2] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].chemicalvolume[3] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss4].input_volume);

                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].concentrationsolution = new float[4];
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].concentrationsolution[0] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss1);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].concentrationsolution[1] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss2);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].concentrationsolution[2] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss3);
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_B].concentrationsolution[3] = Convert.ToSingle(Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss4);

                send_Data.tank[(int)tank_class.enum_tank_type.TANK_C].chemicalvolume = new float[4];
                send_Data.tank[(int)tank_class.enum_tank_type.TANK_C].concentrationsolution = new float[4];

                //Process Tank 
                if (Program.tank[(int)Program.seq.supply.cur_tank].enable == false)
                {
                    send_Data.tank[(int)Program.seq.supply.cur_tank].curstate = ETankState.tsNoUse;
                }
                else if (Program.tank[(int)Program.seq.supply.cur_tank].status == tank_class.enum_tank_status.DRAIN ||
                    Program.tank[(int)Program.seq.supply.cur_tank].status == tank_class.enum_tank_status.DRAIN_WAIT)
                {
                    send_Data.tank[(int)Program.seq.supply.cur_tank].curstate = ETankState.tsDrain;
                }
                else if (Program.tank[(int)Program.seq.supply.cur_tank].status == tank_class.enum_tank_status.CHARGE)
                {
                    send_Data.tank[(int)Program.seq.supply.cur_tank].curstate = ETankState.tsPrep;
                }
                else if (Program.tank[(int)Program.seq.supply.cur_tank].status == tank_class.enum_tank_status.READY)
                {
                    send_Data.tank[(int)Program.seq.supply.cur_tank].curstate = ETankState.tsReady;
                }
                else if (Program.tank[(int)Program.seq.supply.cur_tank].status == tank_class.enum_tank_status.SUPPLY ||
                    Program.tank[(int)Program.seq.supply.cur_tank].status == tank_class.enum_tank_status.REFILL)
                {
                    send_Data.tank[(int)Program.seq.supply.cur_tank].curstate = ETankState.tsSupply;
                }


                send_Data.tank[(int)Program.seq.supply.cur_tank].lifetime = Convert.ToInt32(Program.tank[(int)Program.seq.supply.cur_tank].life_time_to_minute);
                send_Data.tank[(int)Program.seq.supply.cur_tank].wafercount = Convert.ToUInt32(Program.tank[(int)Program.seq.supply.cur_tank].wafer_cnt);
                send_Data.tank[(int)Program.seq.supply.cur_tank].volume = Convert.ToUInt32(Program.tank[(int)Program.seq.supply.cur_tank].total_volume);
                send_Data.tank[(int)Program.seq.supply.cur_tank].temperature = Convert.ToSingle(string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv));

                send_Data.tank[(int)Program.seq.supply.cur_tank].chemicalvolume = new float[4];
                send_Data.tank[(int)Program.seq.supply.cur_tank].chemicalvolume[0] = Convert.ToSingle(Program.tank[(int)Program.seq.supply.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume);
                send_Data.tank[(int)Program.seq.supply.cur_tank].chemicalvolume[1] = Convert.ToSingle(Program.tank[(int)Program.seq.supply.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume);
                send_Data.tank[(int)Program.seq.supply.cur_tank].chemicalvolume[2] = Convert.ToSingle(Program.tank[(int)Program.seq.supply.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume);
                send_Data.tank[(int)Program.seq.supply.cur_tank].chemicalvolume[3] = Convert.ToSingle(Program.tank[(int)Program.seq.supply.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].input_volume);

                send_Data.tank[(int)Program.seq.supply.cur_tank].concentrationsolution = new float[4];
                send_Data.tank[(int)Program.seq.supply.cur_tank].concentrationsolution[0] = Convert.ToSingle(Program.tank[(int)Program.seq.supply.cur_tank].concentration_ccss1);
                send_Data.tank[(int)Program.seq.supply.cur_tank].concentrationsolution[1] = Convert.ToSingle(Program.tank[(int)Program.seq.supply.cur_tank].concentration_ccss2);
                send_Data.tank[(int)Program.seq.supply.cur_tank].concentrationsolution[2] = Convert.ToSingle(Program.tank[(int)Program.seq.supply.cur_tank].concentration_ccss3);
                send_Data.tank[(int)Program.seq.supply.cur_tank].concentrationsolution[3] = Convert.ToSingle(Program.tank[(int)Program.seq.supply.cur_tank].concentration_ccss4);

 

                //send_Data.tankcir = new STTankCirStatus[2];
                //CIR
                if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                {
                    send_Data.tankcir[0].pump_on = 1;
                }
                else
                {
                    send_Data.tankcir[0].pump_on = 0;
                }
                send_Data.tankcir[0].temperature = Convert.ToSingle(string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.circulation.pv));

                send_Data.tankcir[1].pump_on = 0;
                send_Data.tankcir[1].temperature = 0;


                //send_Data.supply = new STSupplyStatus[2];

                if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                {
                    send_Data.supply[0].pump_on = 1;
                }
                else
                {
                    send_Data.supply[0].pump_on = 0;
                }

                send_Data.supply[0].flowrate = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value));
                send_Data.supply[0].temperature = Convert.ToSingle(string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.pv));

                send_Data.supply[0].concentrationsolution = new float[4];
                send_Data.supply[0].concentrationsolution[0] = 0;
                send_Data.supply[0].concentrationsolution[1] = 0;
                send_Data.supply[0].concentrationsolution[2] = 0;
                send_Data.supply[0].concentrationsolution[3] = 0;

                if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                {
                    send_Data.supply[1].pump_on = 1;
                }
                else
                {
                    send_Data.supply[1].pump_on = 0;
                }
                send_Data.supply[1].flowrate = Convert.ToSingle(string.Format("{0:f3}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value));
                send_Data.supply[1].temperature = Convert.ToSingle(string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.pv));

                send_Data.supply[1].concentrationsolution = new float[4];
                send_Data.supply[1].concentrationsolution[0] = 0;
                send_Data.supply[1].concentrationsolution[1] = 0;
                send_Data.supply[1].concentrationsolution[2] = 0;
                send_Data.supply[1].concentrationsolution[3] = 0;


                //send_Data.utility = new STUitlityStatus();

                send_Data.utility.pcwpressure = Convert.ToSingle(string.Format("{0:f1}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_PCW_PRESS].value));
                send_Data.utility.pn2pressure = Convert.ToSingle(string.Format("{0:f1}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_PN2_PRESS].value));
                send_Data.utility.cdapressure = Convert.ToSingle(string.Format("{0:f1}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA2_PRESS_SOL].value));
                send_Data.utility.pumpcdapressure = Convert.ToSingle(string.Format("{0:f1}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA1_PRESS_PUMP].value));

                //send_Data.other = new STOtherStatus();
                //CCSS Volume 총 사용량
                send_Data.other.dailyUsageAccumulatedvolume = new float[4];
                send_Data.other.dailyUsageAccumulatedvolume[0] = Program.schematic_form.TotalUsage_By_Daily_CCSS1;
                send_Data.other.dailyUsageAccumulatedvolume[1] = Program.schematic_form.TotalUsage_By_Daily_CCSS2;
                send_Data.other.dailyUsageAccumulatedvolume[2] = Program.schematic_form.TotalUsage_By_Daily_CCSS3;
                send_Data.other.dailyUsageAccumulatedvolume[3] = Program.schematic_form.TotalUsage_By_Daily_CCSS4;


                send_Data.other.cputemp = Program.main_form.cpu_temp;
                send_Data.other.cpufanspeed = (UInt32)Program.main_form.cpu_fan_speed;
                send_Data.other.storagetemp = new float[2];
                send_Data.other.storagetemp[0] = (float)Program.cg_app_info.internal_info.usage_drive_c;
                send_Data.other.storagetemp[1] = (float)Program.cg_app_info.internal_info.usage_drive_d;

                send_Data.other.mainboardfanspeed = 0;
                send_Data.other.otherfanspeed = 0;
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite("Module_Socket" + ".Message_FDC_Data_Send_106." + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
            }
            //send_Data.remark = System.Text.Encoding.UTF8.GetBytes(remark.PadRight(1024, ' '));
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.FDC_Data_Send_106, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.FDC_Data_Send_106);
        }
        /// <summary>
        /// CTC -> Send Test를 위해 생성
        /// </summary>
        public void Message_CDS_Version_Info_108()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Unit_Version_Info_Send_108, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Unit_Version_Info_Send_108);

        }
        public void Message_Database_Changed_Notice_109(Config_Parameter config_parameter)
        {

            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Database_Changed_Notice send_Data = new Module_Socket.Database_Changed_Notice();
            //Array.Resize(ref send_Data.remark, 1024);
            if (config_parameter != null)
            {
                send_Data.type = (int)DataBase_Changed_Notice_Type.Parameter;
                send_Data.id = config_parameter.id;
                send_Data.value = config_parameter.value;
                //send_Data.unit = config_parameter.unit;
            }
            else
            {
                send_Data.type = (int)DataBase_Changed_Notice_Type.Parameter;
                send_Data.id = -1;
                send_Data.value = 0;
                //send_Data.unit = "";
            }

            //send_Data.remark = System.Text.Encoding.UTF8.GetBytes(remark.PadRight(1024, ' '));
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.DateBase_Change_Notice_109, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.DateBase_Change_Notice_109);
        }
        /// <summary>
        /// CTC -> Send Test를 위해 생성
        /// </summary>
        public void Message_System_Reboot_110()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.System_Reboot_110, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.System_Reboot_110);
        }
        /// <summary>
        /// CTC -> Send Test를 위해 생성
        /// </summary>
        public void Message_System_Shutdown_111()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.System_Shutdown_111, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.System_Shutdown_111);
        }
        public void Message_Chemical_Change_Request_400()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Chemical_Change_Request_400, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Chemical_Change_Request_400);
            //응답 변수 초기화 true로 활성화되어야, CC 진행한다.
            Program.seq.supply.req_c_c_start_cds_to_ctc = true; Program.seq.supply.rep_c_c_start_cds_to_ctc = false;

        }
        public void Message_Chemical_Change_Confirm_401()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Chemical_Change_Confirm_401, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Chemical_Change_Confirm_401);
        }
        public void Message_Manual_Chemical_Change_402()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Manual_Chemical_Change_402, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Manual_Chemical_Change_402);
        }
        public void Message_Increase_Wafer_Count_403()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Wafer_Count_403, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Wafer_Count_403);
        }
        public void Message_Reclaim_Enabled_404(bool reclaim)
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_Reclaim_Enable send_Data = new Module_Socket.Data_Reclaim_Enable();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_Data.reclaim_enable = reclaim;
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Reclaim_Enabled_404, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Reclaim_Enabled_404);
        }
        public void Message_Stop_Supply_406()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Stop_Supply_406, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Stop_Supply_406);
        }
        public void Message_Check_Availability_408()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Check_Availability_408, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Check_Availability_408);
            //응답 변수 초기화 true로 활성화되어야, CC 진행한다.
            Program.seq.supply.ctc_supply_request = false;
            Program.seq.supply.req_c_c_start_cds_to_ctc = true; Program.seq.supply.rep_c_c_start_cds_to_ctc = false;

        }
        public void Message_CDS_Enable_Event_450()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Supply_Enable_Event_Send_450, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);


            if (Program.cg_app_info.keep_supply_and_cir_off_delay_by_change == false)
            {
                Message_Send(ref full_data, send_head, Module_Socket.TokenList.Supply_Enable_Event_Send_450);
            }
            else if (Program.cg_app_info.keep_supply_and_cir_off_delay_by_change == true && Program.seq.supply.CC_START_TANK == "")
            {
                Message_Send(ref full_data, send_head, Module_Socket.TokenList.Supply_Enable_Event_Send_450);
            }
            Program.seq.supply.CDS_enable_status_to_ctc = true;
        }
        public void Message_CDS_Disable_Event_451(bool force_send)
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Supply_Disable_Event_Send_451, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);

            if (force_send == true)
            {
                Message_Send(ref full_data, send_head, Module_Socket.TokenList.Supply_Disable_Event_Send_451);
            }
            else if (Program.cg_app_info.keep_supply_and_cir_off_delay_by_change == false)
            {
                Message_Send(ref full_data, send_head, Module_Socket.TokenList.Supply_Disable_Event_Send_451);
            }
            else if (Program.cg_app_info.keep_supply_and_cir_off_delay_by_change == true && Program.seq.supply.CC_START_TANK == "")
            {
                Message_Send(ref full_data, send_head, Module_Socket.TokenList.Supply_Disable_Event_Send_451);
            }
            Program.seq.supply.CDS_enable_status_to_ctc = false;
        }
        public void Message_Chemical_Change_Start_Event_452(string supplytank)
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Chemical_Change_Event send_Data = new Module_Socket.Chemical_Change_Event();
            send_Data.supplytank = supplytank;
            Program.seq.supply.CC_START_TANK = supplytank;
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Chemical_Change_Start_Event_Send_452, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Chemical_Change_Start_Event_Send_452);
        }
        public void Message_Chemical_Change_End_Event_453(string supplytank)
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Chemical_Change_Event send_Data = new Module_Socket.Chemical_Change_Event();
            send_Data.supplytank = supplytank;
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Chemical_Change_End_Event_Send_453, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Chemical_Change_End_Event_Send_453);
        }
        public void Message_No_Process_Request_Event_454()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.No_Process_Request_Event_454, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.No_Process_Request_Event_454);
        }
        public void Message_No_Process_Request_Cancel_Event_455()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.No_Process_Request_Cancel_Event_455, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.No_Process_Request_Cancel_Event_455);
        }
        public void Message_Auto_Mode_Event_456()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Auto_Mode_Event_456, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Auto_Mode_Event_456);
        }
        public void Message_Manual_Mode_Event_457()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Manual_Mode_Event_457, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Manual_Mode_Event_457);
        }
        public void Message_Tank_A_Supply_Start_Event_458()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Tank_A_Supply_Start_Event_Send_458, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Tank_A_Supply_Start_Event_Send_458);
            Program.seq.supply.supply_status = true;
        }
        public void Message_Tank_A_Supply_End_Event_459()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Tank_A_Supply_End_Event_Send_459, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Tank_A_Supply_End_Event_Send_459);
        }
        public void Message_Tank_B_Supply_Start_Event_460()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Tank_B_Supply_Start_Event_Send_460, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Tank_B_Supply_Start_Event_Send_460);
            Program.seq.supply.supply_status = true;
        }
        public void Message_Tank_B_Supply_End_Event_461()
        {
            Byte[] full_data;
            UInt16 send_flag;
            Module_Socket.Head send_head = new Module_Socket.Head();
            Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message_simulation;
            }
            else
            {
                send_flag = (ushort)Module_Socket.Flag.Send_Message;
            }
            send_head = Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)Module_Socket.TokenList.Tank_B_Supply_End_Event_Send_461, message_no, Program.cg_socket.ctc_network_no);
            full_data = GetBytes_By_Packet(send_head, send_Data);
            Message_Send(ref full_data, send_head, Module_Socket.TokenList.Tank_B_Supply_End_Event_Send_461);
        }
        public void Database_And_Alarm_Sync_By_CTC_Connected()
        {
            try
            {
                if (thd_Database_Sync_By_CTC_Connected != null) { if (thd_Database_Sync_By_CTC_Connected.IsAlive == false) { thd_Database_Sync_By_CTC_Connected.Abort(); thd_Database_Sync_By_CTC_Connected = null; thd_Database_Sync_By_CTC_Connected = new Thread(Func_Database_And_Alarm_Sync_By_CTC_Connected); thd_Database_Sync_By_CTC_Connected.Start(); } }
                else { thd_Database_Sync_By_CTC_Connected = new Thread(Func_Database_And_Alarm_Sync_By_CTC_Connected); thd_Database_Sync_By_CTC_Connected.Start(); }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite("Module_Socket" + ".Database_Sync_By_CTC_Connected." + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }

        public void Func_Database_And_Alarm_Sync_By_CTC_Connected()
        {
            string result = "", query = "", err = "";
            bool alarm_sync_complete = true, para_sync_complete = true;
            DataSet dataset = new DataSet();
            try
            {
                //Alarm
                query = "SELECT * FROM alarm_list" + System.Environment.NewLine;
                query += "WHERE alarm_id is not null" + System.Environment.NewLine;
                query += "ORDER BY alarm_id" + System.Environment.NewLine;

                if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; dataset = new DataSet(); }
                Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_local_cds.connection, query, dataset, ref err);
                if (err != "") { Program.log_md.LogWrite("", Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    query = "Delete From alarm_list Where alarm_unit_id = '" + Program.cg_socket.ctc_network_no + "'";
                    Program.database_md.MariaDB_MainQuery(Program.cg_main.db_server.connection, query, ref err);
                    if (err != "") { Program.log_md.LogWrite("", Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                    for (int idx = 0; idx < dataset.Tables[0].Rows.Count; idx++)
                    {
                        query = "INSERT INTO alarm_list" + System.Environment.NewLine;
                        query += "(`alarm_id`, `alarm_name`, `alarm_comment`, `alarm_enabled`, `alarm_wdt`, `report_alarm_to_host`, `alarm_visible`, `alarm_level`, `alarm_unit_id`, `ctc_alarm_action_tm`, `ctc_alarm_action_pm`)" + System.Environment.NewLine;
                        query += "VALUES" + System.Environment.NewLine;
                        query += "('" + dataset.Tables[0].Rows[idx]["alarm_id"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["alarm_name"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["alarm_comment"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["alarm_enabled"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["alarm_wdt"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["report_alarm_to_host"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["alarm_visible"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["alarm_level"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + Program.cg_socket.ctc_network_no.ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + "0" + "'" + System.Environment.NewLine;
                        query += ",'" + "0" + "'" + System.Environment.NewLine;
                        query += ")" + System.Environment.NewLine;
                        Program.database_md.MariaDB_MainQuery(Program.cg_main.db_server.connection, query, ref err);
                        if (err != "") { alarm_sync_complete = false; }
                        if (err != "") { Program.log_md.LogWrite("", Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                        System.Threading.Thread.Sleep(2);

                    }
                    if (alarm_sync_complete == false)
                    {
                        Program.main_form.Insert_System_Message("CDS -> CTC Alarm Update Fail");
                        Program.eventlog_form.Insert_Event("CDS -> CTC Alarm Update Fail", (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                    }
                    else
                    {
                        Program.main_form.Insert_System_Message("CDS -> CTC Alarm Update Complete");
                        Program.eventlog_form.Insert_Event("CDS -> CTC Alarm Update Complete", (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                    }

                }
                else
                {
                    result = err;
                    if (result == "") { result = "Alarm Database Is null"; }
                }

                //Parameter
                if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; dataset = new DataSet(); }
                query = "SELECT * FROM parameters" + System.Environment.NewLine;
                query += "WHERE cds_parameter_id is not null" + System.Environment.NewLine;
                query += "ORDER BY cds_parameter_id" + System.Environment.NewLine;
                if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; dataset = new DataSet(); }
                Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_local_cds.connection, query, dataset, ref err);
                if (err != "") { Program.log_md.LogWrite("", Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    query = "Delete From parameters Where parameter_unit_id = '" + Program.cg_socket.ctc_network_no + "'";
                    Program.database_md.MariaDB_MainQuery(Program.cg_main.db_server.connection, query, ref err);
                    for (int idx = 0; idx < dataset.Tables[0].Rows.Count; idx++)
                    {
                        query = "INSERT INTO parameters" + System.Environment.NewLine;
                        query += "(`parameter_id`, `parameter_name`, `parameter_value`, `parameter_unit`, `parameter_comment`, `parameter_minimum`, `parameter_maximum`, `parameter_default`, `parameter_data_type`, `report_parameter_to_host`, `parameter_unit_id`, `parameter_visible`)" + System.Environment.NewLine;
                        query += "VALUES" + System.Environment.NewLine;
                        query += "('" + dataset.Tables[0].Rows[idx]["cds_parameter_id"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["cds_parameter_name"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["cds_parameter_value"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["cds_parameter_unit"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["cds_parameter_comment"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["cds_parameter_minimum"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["cds_parameter_maximum"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["cds_parameter_default"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["cds_parameter_data_type"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["report_cds_parameter_to_host"].ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + Program.cg_socket.ctc_network_no.ToString() + "'" + System.Environment.NewLine;
                        query += ",'" + dataset.Tables[0].Rows[idx]["cds_parameter_visible"].ToString() + "'" + System.Environment.NewLine;
                        query += ")" + System.Environment.NewLine;

                        Program.database_md.MariaDB_MainQuery(Program.cg_main.db_server.connection, query, ref err);
                        if (err != "") { para_sync_complete = false; }
                        if (err != "") { Program.log_md.LogWrite("", Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                        System.Threading.Thread.Sleep(2);
                    }
                    if (para_sync_complete == false)
                    {
                        Program.main_form.Insert_System_Message("CDS -> CTC Parameter Update Fail");
                        Program.eventlog_form.Insert_Event("CDS -> CTC Parameter Update Fail", (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                    }
                    else
                    {
                        Program.main_form.Insert_System_Message("CDS -> CTC Parameter Update Complete");
                        Program.eventlog_form.Insert_Event("CDS -> CTC Parameter Update Complete", (int)frm_eventlog.enum_event_type.CTC, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                    }

                }
                else
                {
                    result = err;
                    if (result == "") { result = "Parameter Database Is null"; }
                }


                DataTable dt_alarm_copy = Program.occured_alarm_form.dt_alarm.Copy();
                //Occured Alarm 전송 Sync
                for (int idx = 0; idx < dt_alarm_copy.Rows.Count; idx++)
                {
                    Program.CTC.Message_Alarm_Occurred_102(Convert.ToInt32(dt_alarm_copy.Rows[idx]["NO"]), dt_alarm_copy.Rows[idx]["remark"].ToString());
                    //Program.CTC.Message_Alarm_Occurred_102(Convert.ToInt32(dt_alarm_copy.Rows[idx]["NO"]), ".");
                    System.Threading.Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite("Module_Socket" + ".Func_Database_Sync_By_CTC_Connected." + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; }
            }
        }

    }


}
