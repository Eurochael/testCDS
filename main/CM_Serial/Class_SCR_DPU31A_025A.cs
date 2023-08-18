using System;
using System.Linq;

namespace cds
{
    public class Class_SCR_DPU31A_025A
    {
        /// <summary>
        /// APM
        /// SCR1 : SUPPLY A
        /// SCR2 : SUPPLY B
        /// SCR3 : CIRCULATION1
        /// SCR4 : CIRCULATION2
        /// </summary>
        public struct ST_SCR_DPU31A_025A
        {

            public int SCR1_operation;
            public double SCR1_pv;
            public bool SCR1_run_state;
            public DateTime last_rcv_time_ch1;

            public int SCR2_operation;
            public double SCR2_pv;
            public bool SCR2_run_state;
            public DateTime last_rcv_time_ch2;

            public int SCR3_operation;
            public double SCR3_pv;
            public bool SCR3_run_state;
            public DateTime last_rcv_time_ch3;

            public int SCR4_operation;
            public double SCR4_pv;
            public bool SCR4_run_state;
            public DateTime last_rcv_time_ch4;

        }
        public enum enum_scr_operation_map
        {
            SPARE_15 = 15,
            SPARE_14 = 14,
            SPARE_13 = 13,
            EMS_POWER = 12,
            AUTO_MANUAL = 11,
            RUN_STOP = 10,
            FREQ_ERROR = 9,
            SCR_ERROR = 8,
            LOAD_OPEN = 7,
            PHASE_LOSS = 6,
            FUSE_CUT = 5,
            OVER_TEMP = 4,
            OVER_VOLT = 3,
            OVER_CURRENT = 2,
            I_OC = 1,
            FAULT = 0,
        }
        //serialPort2.Close();
        //        serialPort2.PortName = "COM5";
        //        serialPort2.BaudRate = 9600;
        //        serialPort2.Parity = System.IO.Ports.Parity.None;
        //        serialPort2.DataBits = 8;
        //        serialPort2.StopBits = System.IO.Ports.StopBits.One;
        //        serialPort2.Open();
        public static byte[] command_read_300104 =
        {
            //소프트웨어 버전 = 10
                0x01,        //ID
                0x04,        //Input Register 04
                0x00, 0x67,  //시작 번지 0066 : 하드웨어 버전, 0067 : 소프트웨어 버전
                0x00, 0x01,  //데이터 개수
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] command_read_300105_300114 =
        {
            //모델명1 ~ 모델명10 까지 조회
                0x01,        //ID
                0x04,        //Input Register 04
                0x00, 0x68,  //시작 번지 0068 : 모델명1
                0x00, 0x0A,  //데이터 개수
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] read_alarm =
        //public static byte[] command_read_400036_400045 =
        {
            //400036 : OC Alarm, 400038 : OV Alarm, 400039 : Fuse Alarm, 400041 : Alarm Heat sink
            //400042 : SCR, 400043 : Heater Braek, 400044 : Auto Gain, 400045 : Heater Break Time

                0x01,        //ID
                0x03,        //Holding Register 03
                0x00, 0x23,  //시작 번지 0023 : 모델명1
                0x00, 0x08,  //데이터 개수
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] command_read_400031_400045 =
        {   //400031 : Opearation, 400032 : Output Slope, 400033 : Input Correction
            //400034 : Input Correction, 400035 : Input Slope Correction
            //400036 : OC Alarm, 400038 : OV Alarm, 400039 : Fuse Alarm, 400041 : Alarm Heat sink
            //400042 : SCR, 400043 : Heater Braek, 400044 : Auto Gain, 400045 : Heater Break Time

                0x01,        //ID
                0x03,        //Holding Register 03
                0x00, 0x1E,  //시작 번지 001E
                0x00, 0x0D,  //데이터 개수 400031(0x0E) Operation  ~ 400045(002C) Heater Break Time
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] read_operation =
        //public static byte[] command_read_400031 =
        {   //400031 : Opearation

                0x01,        //ID
                0x03,        //Holding Register 03
                0x00, 0x1E,  //시작 번지 001E
                0x00, 0x01,  //데이터 개수 400031(0x0E) Operation
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public struct Rcv_Data
        {
            public DateTime dt_rcvtime;
            public byte address;
            public byte function_code;
            public byte read_data_byte_cnt;
            public string[] reading_data;
            public string crc;
            public string rcv_data_to_string;
        }

        public byte[] Message_Command_Apply_CRC_TO_Send(byte address, byte[] command, int idx_serial)
        {
            byte[] send_msg = null;
            byte[] crc = new byte[2];
            try
            {
                //Checksum 계산
                command[0] = address;
                crc = CRC16(command, command.Length - 2, true);
                command[command.Length - 2] = crc[0];
                command[command.Length - 1] = crc[1];
                Array.Resize(ref send_msg, command.Length); Array.Copy(command, send_msg, command.Length);
            }
            catch (Exception ex)
            {
                send_msg = null;
            }
            if (idx_serial != -1) { Program.main_form.Serial_data_Enqueue(ref send_msg, idx_serial); }
            return send_msg;
        }
        public string Recieve_Data_To_Parse_DPU31A_025A(ref byte[] rcv_data, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
        {
            int data_idx = 0;
            string result = "";
            try
            {
                //DPU31A_025A Rcv 구조
                // 국번 + 0x84로 응답됨
                //예외 코드 01:지원안함, 02:요청한 데이터의 시작번지가 이상, 03:데이터 개수 이상, 04:데이터 패킷 이상
                //Ex
                //Send    Address(1), Function Code(1), Register Address(2), Reading Rester Count(2), CRC(2)
                //Send    01        , 03              , 00 14              , 00 01                  , C4 0E

                //Rcv     Address(1), Function Code(1), Reading Byte Count(1), Reading Data(2*N), CRC(2)      
                //                                                             Data1 Data2 Data3
                //Rcv     01        , 03              , 06                   , 01 F4 03 E8 00 02, 90 00
                rcv_data_parse.dt_rcvtime = DateTime.Now;
                rcv_data_parse.reading_data = null;
                rcv_data_parse.function_code = 0;
                if (rcv_data == null) { result = "Empty Array"; }
                else
                {
                    for (int idx = 0; idx < rcv_data.Length; idx++)
                    {

                        if (idx != 0) { Console.Write(","); }


                        if (idx == 0) { rcv_data_parse.address = rcv_data[0]; }
                        if (idx == 1)
                        {
                            rcv_data_parse.function_code = rcv_data[1];
                        }
                        if (idx == 2) { rcv_data_parse.read_data_byte_cnt = rcv_data[2]; rcv_data_parse.reading_data = new string[rcv_data_parse.read_data_byte_cnt / 2]; Console.Write(rcv_data[idx].ToString("X2")); }
                        if (idx > 2 && idx < rcv_data.Length - 2)
                        {
                            //Function Code 6(Write)일때의 정상 응답은 보낸 명령과 동일하게 온다
                            //Error 응답은 Function Code에 0x83을 더한 만큼 온다
                            //Operation 응답 예
                            //Convert.ToString(Convert.ToInt32("2041", 16), 2);
                            //Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[0], 16), 2).PadLeft(16, '0');

                            if (rcv_data_parse.function_code != 6)
                            { //Holding, Input Register Rcv

                                //모델명 4450 -> DP, 2D33 -> -3, 3030 -> 00  - DP-300
                                rcv_data_parse.reading_data[data_idx] = rcv_data[idx].ToString("X2") + rcv_data[idx + 1].ToString("X2");
                                //Console.Write(rcv_data_parse.reading_data[data_idx]);

                                data_idx = data_idx + 1; idx = idx + 1;
                            }
                            else
                            {
                                //Console.Write(rcv_data[idx].ToString("X2"));
                            }
                        }
                        else if (idx == rcv_data.Length - 2)
                        {
                            rcv_data_parse.crc = rcv_data[idx].ToString("X2") + rcv_data[idx + 1].ToString("X2");
                            //Console.Write(rcv_data_parse.crc);
                            break;
                        }

                    }

                    if (rcv_data_parse.function_code == 3)
                    {
                        Alarm_check(rcv_data_parse.reading_data[0], ref rcv_data_parse);
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("SCR" + "." + "Recieve_Data_To_Parse_DPU31A_025A" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;
        }

        public void Alarm_check(string operaion_data, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
        {
            string tml_binary_data = "";
            string log = "";
            string binary_data;
            bool alarm_alive = false;
            binary_data = Convert.ToString(Convert.ToInt32(operaion_data, 16), 2).PadLeft(16, '0');//Convert.ToString(operaion_data, 2).PadLeft(16, '0');
            //SCR Reverse not use
            tml_binary_data = new string(binary_data.Reverse().ToArray());
            //SPARE_15 = 15,
            //SPARE_14 = 14,
            //SPARE_13 = 13,
            //EMS_POWER = 12,
            //AUTO_MANUAL = 11,
            //RUN_STOP = 10,
            //FREQ_ERROR = 9,
            //SCR_ERROR = 8,
            //LOAD_OPEN = 7,
            //PHASE_LOSS = 6,
            //FUSE_CUT = 5,
            //OVER_TEMP = 4,
            //OVER_VOLT = 3,
            //OVER_CURRENT = 2,
            //I_OC = 1,
            //FAULT = 0,


            //Not Run APM Real Data
            //0000 0000 0100 0001
            //1000 0010 0000 0000
            //Run APM Real Data
            //0001 0000 0000 0000
            //2023-04-13 15:02:58.193 Send,01 03 00 1E 00 01 E4 0C
            //2023-04-13 15:02:58.294 Parse,,Rcv,01 03 02 00 41 78 74
            //2023-04-13 15:02:56.356 Send,03 03 00 1E 00 01 E5 EE
            //2023-04-13 15:02:56.457 Parse,,Rcv,03 03 02 10 00 CC 44
            //Operaion Map
            //
            //Program.log_md.LogWrite("SCR Raw : " + "" + operaion_data + " / " + rcv_data_parse.address + " / " + rcv_data_parse.function_code, Module_Log.enumLog.SERAL_DATA_10, "", Module_Log.enumLevel.ALWAYS);
            //Program.log_md.LogWrite("SCR BINARY : " + "" + binary_data + " / " +  rcv_data_parse.address + " / " + rcv_data_parse.function_code, Module_Log.enumLog.SERAL_DATA_10, "", Module_Log.enumLevel.ALWAYS);
            //Program.log_md.LogWrite("SCR REVERSE BINARY : " + "" + tml_binary_data + " / " + rcv_data_parse.address + " / " + rcv_data_parse.function_code, Module_Log.enumLog.SERAL_DATA_10, "", Module_Log.enumLevel.ALWAYS);
            //Program.log_md.LogWrite("---------------------------------", Module_Log.enumLog.SERAL_DATA_10, "", Module_Log.enumLevel.ALWAYS);
            //if (binary_data.Substring(0, 1) == "1") { if (log != "") { log = log + ","; } else { log = log + "FAULT"; } }
            if (tml_binary_data.Substring(1, 1) == "1") { log = log + "I-IC"; }
            if (tml_binary_data.Substring(2, 1) == "1") { log = log + "OVER CURRENT"; }
            if (tml_binary_data.Substring(3, 1) == "1") { log = log + "OVER VOLT"; }

            if (tml_binary_data.Substring(4, 1) == "1") { log = log + "OVER TEMP"; }
            if (tml_binary_data.Substring(5, 1) == "1") { log = log + "FUSE CUT"; }
            //if (tml_binary_data.Substring(6, 1) == "1") { if (log != "") { log = log + ","; } else { log = log + "PHASE LOSS"; } }
            //if (tml_binary_data.Substring(7, 1) == "1") { if (log != "") { log = log + ","; } else { log = log + "LOAD OPEN"; } }

            if (tml_binary_data.Substring(8, 1) == "1") { log = log + "SCR ERROR"; }

            //if (tml_binary_data.Substring(9, 1) == "1") { if (log != "") { log = log + ","; } else { log = log + "FREQ ERROR"; } }

            if (log != "")
            {
                alarm_alive = true;
            }
            else
            {
                alarm_alive = false;
            }
            if (rcv_data_parse.address == 1)
            {
                Program.main_form.SerialData.SCR.last_rcv_time_ch1 = DateTime.Now;
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SCR1_Error, log, false, alarm_alive);
            }
            else if (rcv_data_parse.address == 2)
            {
                Program.main_form.SerialData.SCR.last_rcv_time_ch2 = DateTime.Now;
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SCR2_Error, log, false, alarm_alive);
            }
            else if (rcv_data_parse.address == 3)
            {
                Program.main_form.SerialData.SCR.last_rcv_time_ch3 = DateTime.Now;
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SCR3_Error, log, false, alarm_alive);
            }
            else if (rcv_data_parse.address == 4)
            {
                Program.main_form.SerialData.SCR.last_rcv_time_ch4 = DateTime.Now;
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SCR4_Error, log, false, alarm_alive);
            }
        }


        //값 쓰기
        //string test = Convert.ToInt32(textBox1.Text).ToString("X4");
        //command_write_1[4] = Convert.ToByte(test.Substring(0, 2), 16);
        //command_write_1[5] = Convert.ToByte(test.Substring(2, 2), 16);
        //Send_Message_To_항온조(command_write_1);


        /// <summary>
        /// CRC16 Return :  Address To Before CRC,  reverse = High, Log Change
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] CRC16(byte[] data, bool return_reverse)
        {
            int crc = 0xFFFF;
            byte[] result_crc = new byte[2];
            for (int idx = 0; idx < data.Length; idx++)
            {
                crc ^= data[idx];
                for (int bit = 0; bit < 8; bit++)
                {
                    if (Convert.ToBoolean(crc & 0x0001))
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }
            if (return_reverse == false)
            {
                result_crc[0] = Convert.ToByte(crc.ToString("X4").Substring(0, 2), 16);
                result_crc[1] = Convert.ToByte(crc.ToString("X4").Substring(2, 2), 16);
            }
            else
            {
                result_crc[1] = Convert.ToByte(crc.ToString("X4").Substring(0, 2), 16);
                result_crc[0] = Convert.ToByte(crc.ToString("X4").Substring(2, 2), 16);
            }
            return result_crc;

        }
        /// <summary>
        /// CRC16 Return :  Address To Before CRC,  reverse = High, Log Change
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] CRC16(byte[] data, int before_crc_len, bool return_reverse)
        {
            int crc = 0xFFFF;
            byte[] result_crc = new byte[2];
            for (int idx = 0; idx < before_crc_len; idx++)
            {
                crc ^= data[idx];
                for (int bit = 0; bit < 8; bit++)
                {
                    if (Convert.ToBoolean(crc & 0x0001))
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }
            if (return_reverse == false)
            {
                result_crc[0] = Convert.ToByte(crc.ToString("X4").Substring(0, 2), 16);
                result_crc[1] = Convert.ToByte(crc.ToString("X4").Substring(2, 2), 16);
            }
            else
            {
                result_crc[1] = Convert.ToByte(crc.ToString("X4").Substring(0, 2), 16);
                result_crc[0] = Convert.ToByte(crc.ToString("X4").Substring(2, 2), 16);
            }
            return result_crc;

        }

    }
}
