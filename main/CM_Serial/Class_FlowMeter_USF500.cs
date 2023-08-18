using System;
using System.Linq;

namespace cds
{
    public class Class_FlowMeter_USF500
    {
        /// <summary>
        /// DHF
        /// 1번 USF - 1국번 1번 채널은 DIW, 2번 채널은 HF
        /// 
        /// APM / 도면이랑 다르다.
        /// 도면
        /// 1번 USF - 1국번 1번 채널은 HOT DIW
        /// 2번 USF - 2국번 1번 채널은 H2O2, 2번 채널은 NH4OH
        /// 실제 배선(실제 배선으로 진행)
        /// 1번 USF - 1국번 1번 채널은 NH4OH
        /// 2번 USF - 2국번 1번 채널은 HDIW, 2번 채널은 N2O2
        /// 
        /// DHF
        /// 1번 USF - 1국번 1번 채널은 DIW
        /// 1번 USF - 1국번 2번 채널은 HF
        /// </summary>
        /// 
        public bool flag_no_1_main; //USF500 기기 자체 Command
        public bool flag_no_1_to_ch1;
        public bool flag_no_1_to_ch2;
        public bool flag_no_2_main; //USF500 기기 자체 Command
        public bool flag_no_2_to_ch1;
        public bool flag_no_2_to_ch2;
        public int falg_count = 0;


        public struct ST_FlowMeter_USF500
        {
            public DateTime last_rcv_time_ch1;
            public DateTime last_rcv_time_ch2;
            public DateTime last_rcv_time_ch3;
            public DateTime last_rcv_time_ch4;

            public double NH4OH_flow;
            public double NH4OH_totalusage;
            public string NH4OH_status;

            public double DIW_flow;
            public double DIW_totalusage;
            public string DIW_status;

            public double H2O2_flow;
            public double H2O2_totalusage;
            public string H2O2_status;

            public double HF_flow;
            public double HF_totalusage;
            public string HF_status;

            public double DSP_flow;
            public double DSP_totalusage;
            public string DSP_status;

            public double H2SO4_flow;
            public double H2SO4_totalusage;
            public string H2SO4_status;

            public string usf500_1_status;
            public string usf500_2_status;
        }
        public enum enum_alarm_usf500
        {
            RUNNING_0_CH2 = 13,
            RUNNING_0_CH1 = 12,
            ERROR_INIT_0_CH2 = 11,
            ERROR_INIT_0_CH1 = 10,
            ERROR_MEASURE_CH2 = 9,
            ERROR_MEASURE_CH1 = 8,
            ERROR_SET = 7,
            STATUS_DO_CH2 = 2,
            STATUS_DO_CH1 = 1,
            NOT_USE = 0,
        }
        public enum enum_alarm_ch_status_usf500
        {
            RUNNING_0 = 7,
            ERROR_INIT_0 = 6,
            ERROR_MEASURE = 0,
        }
        //serialPort2.Close();
        //        serialPort2.PortName = "COM5";
        //        serialPort2.BaudRate = 9600;
        //        serialPort2.Parity = System.IO.Ports.Parity.None;
        //        serialPort2.DataBits = 8;
        //        serialPort2.StopBits = System.IO.Ports.StopBits.One;
        //        serialPort2.Open();
        public static byte[] command_read_Regi21 =
        {
                0x01,        //ID
                0x03,        //FunctionCode
                0x00, 0x14,  //시작주소 Ex) 21읽을 때 21 -1 -> 20으로 입력해야함(사양서 주의 사항)
                0x00, 0x01,  //데이터 길이(64)  
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] command_read_Regi_1 =
        {
            //Version
                0x01,        //ID
                0x04,        //FunctionCode
                0x00, 0x00,  //시작주소 Ex) 1읽을 때 1 -1 -> 0으로 입력해야함(사양서 주의 사항)
                0x00, 0x01,  //데이터 길이(64)  
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] read_ch1_flow_Status =
        //public static byte[] command_read_Regi_101_102_103 =
        {
            //Inst Flow, Integ Flow, CH Status CH1
                0x01,        //ID
                0x04,        //FunctionCode
                0x00, 0x64,  //시작주소 Ex) 1읽을 때 101 -1 -> 100으로 입력해야함(사양서 주의 사항)
                0x00, 0x03,  //데이터 길이(64)  
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] read_Status =
        //public static byte[] command_read_Regi_101_102_103 =
        {
            //Read Status
                0x01,        //ID
                0x04,        //FunctionCode
                0x00, 0x02,  //시작주소 1: version, 2: Status
                0x00, 0x01,  //데이터 길이 2
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] read_ch2_flow_Status =
        //public static byte[] command_read_Regi_201_202_203 =
        {
            //Inst Flow, Integ Flow, CH Status CH2
                0x01,        //ID
                0x04,        //FunctionCode
                0x00, 0xC8,  //시작주소 Ex) 1읽을 때 101 -1 -> 100으로 입력해야함(사양서 주의 사항)
                0x00, 0x03,  //데이터 길이(64)  
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] command_read_Regi_121 =
{
            //Integ Flow CH1
                0x01,        //ID
                0x04,        //FunctionCode
                0x00, 0x78,  //시작주소 Ex) 121읽을 때 121 -1 -> 120으로 입력해야함(사양서 주의 사항)
                0x00, 0x02,  //데이터 길이(64)  
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] command_read_Regi_221 =
        {
            //Integ Flow CH2
                0x01,        //ID
                0x04,        //FunctionCode
                0x00, 0xDC,  //시작주소 Ex) 121읽을 때 121 -1 -> 120으로 입력해야함(사양서 주의 사항)
                0x00, 0x02,  //데이터 길이(64)  
                0x00, 0x00   //CheckSum 다시 계산함
         };

        public static byte[] ch1_zeroset =
        //public static byte[] command_write_Regi_103_value_1 =
        {
            //CH1 조정 Value 0 = 0점 조정, Value 1 = 적산 Reset
                0x01,        //ID
                0x06,        //FunctionCode
                0x00, 0x66,  //시작주소 Ex) 103읽을 때 103 -1 -> 102으로 입력해야함(사양서 주의 사항)
                0x00, 0x01,  //Writing Data 0:처리 후 0으로 초기화, 1:0점 조정, 2:적산 Reset
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] ch1_totalusage_reset =
//public static byte[] command_write_Regi_103_value_2 =
{
            //CH1 조정 Value 0 = 0점 조정, Value 1 = 적산 Reset
                0x01,        //ID
                0x06,        //FunctionCode
                0x00, 0x66,  //시작주소 Ex) 103읽을 때 103 -1 -> 102으로 입력해야함(사양서 주의 사항)
                0x00, 0x02,  //Writing Data 0:처리 후 0으로 초기화, 1:0점 조정, 2:적산 Reset
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] ch2_zeroset =
       //public static byte[] command_write_Regi_203_value_1 =
       {
            //CH2 조정 Value 0 = 0점 조정, Value 1 = 적산 Reset
                0x01,        //ID
                0x06,        //FunctionCode
                0x00, 0xCA,  //시작주소 Ex) 203읽을 때 203 -1 -> 202으로 입력해야함(사양서 주의 사항)
                0x00, 0x01,  //Writing Data 0:처리 후 0으로 초기화, 1:0점 조정, 2:적산 Reset
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] ch2_totalusage_reset =
//public static byte[] command_write_Regi_203_value_2 =
{
            //CH2 조정 Value 0 = 0점 조정, Value 1 = 적산 Reset
                0x01,        //ID
                0x06,        //FunctionCode
                0x00, 0xCA,  //시작주소 Ex) 203읽을 때 203 -1 -> 202으로 입력해야함(사양서 주의 사항)
                0x00, 0x02,  //Writing Data 0:처리 후 0으로 초기화, 1:0점 조정, 2:적산 Reset
                0x00, 0x00   //CheckSum 다시 계산함
         };

        public void TotalUsage_Reset(enum_chemical ccss)
        {
            if(Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if(ccss == enum_chemical.DIW)
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 1, Class_FlowMeter_USF500.ch1_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = 0;
                }
                else if (ccss == enum_chemical.H2O2)
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 2, Class_FlowMeter_USF500.ch2_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage = 0;
                }
                else if (ccss == enum_chemical.NH4OH)
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_USF500.ch1_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_USF500.NH4OH_totalusage = 0;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                if (ccss == enum_chemical.HF)
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 2, Class_FlowMeter_USF500.ch2_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage = 0;
                }
                else if (ccss == enum_chemical.DIW)
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_USF500.ch1_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = 0;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (ccss == enum_chemical.H2O2)
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 2, Class_FlowMeter_USF500.ch2_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage = 0;
                }
                else if (ccss == enum_chemical.H2SO4)
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 1, Class_FlowMeter_USF500.ch1_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_USF500.H2SO4_totalusage = 0;
                }
                else if (ccss == enum_chemical.HF)
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 2, Class_FlowMeter_USF500.ch2_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage = 0;
                }
                else if (ccss == enum_chemical.DIW)
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_USF500.ch1_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = 0;
                }
            }

        }
        public byte[] Message_Command_Apply_CRC_TO_Send(byte address, byte ch, byte[] command, int idx_serial)
        {
            byte[] send_msg = null;
            byte[] crc = new byte[2];
            try
            {
                if (address == 1 && ch == 0)
                {
                    Program.FlowMeter_USF500.flag_no_1_main = true; Program.FlowMeter_USF500.flag_no_2_main = false;
                    Program.FlowMeter_USF500.flag_no_1_to_ch1 = false; Program.FlowMeter_USF500.flag_no_1_to_ch2 = false;
                    Program.FlowMeter_USF500.flag_no_2_to_ch1 = false; Program.FlowMeter_USF500.flag_no_2_to_ch2 = false;
                }
                else if (address == 1 && ch == 1)
                {
                    Program.FlowMeter_USF500.flag_no_1_main = false; Program.FlowMeter_USF500.flag_no_2_main = false;
                    Program.FlowMeter_USF500.flag_no_1_to_ch1 = true; Program.FlowMeter_USF500.flag_no_1_to_ch2 = false;
                    Program.FlowMeter_USF500.flag_no_2_to_ch1 = false; Program.FlowMeter_USF500.flag_no_2_to_ch2 = false;
                }
                else if (address == 1 && ch == 2)
                {
                    Program.FlowMeter_USF500.flag_no_1_main = false; Program.FlowMeter_USF500.flag_no_2_main = false;
                    Program.FlowMeter_USF500.flag_no_1_to_ch1 = false; Program.FlowMeter_USF500.flag_no_1_to_ch2 = true;
                    Program.FlowMeter_USF500.flag_no_2_to_ch1 = false; Program.FlowMeter_USF500.flag_no_2_to_ch2 = false;
                }
                else if (address == 2 && ch == 0)
                {
                    Program.FlowMeter_USF500.flag_no_1_main = false; Program.FlowMeter_USF500.flag_no_2_main = true;
                    Program.FlowMeter_USF500.flag_no_1_to_ch1 = false; Program.FlowMeter_USF500.flag_no_1_to_ch2 = false;
                    Program.FlowMeter_USF500.flag_no_2_to_ch1 = true; Program.FlowMeter_USF500.flag_no_2_to_ch2 = false;
                }
                else if (address == 2 && ch == 1)
                {
                    Program.FlowMeter_USF500.flag_no_1_main = false; Program.FlowMeter_USF500.flag_no_2_main = false;
                    Program.FlowMeter_USF500.flag_no_1_to_ch1 = false; Program.FlowMeter_USF500.flag_no_1_to_ch2 = false;
                    Program.FlowMeter_USF500.flag_no_2_to_ch1 = true; Program.FlowMeter_USF500.flag_no_2_to_ch2 = false;
                }
                else if (address == 2 && ch == 2)
                {
                    Program.FlowMeter_USF500.flag_no_1_main = false; Program.FlowMeter_USF500.flag_no_2_main = false;
                    Program.FlowMeter_USF500.flag_no_1_to_ch1 = false; Program.FlowMeter_USF500.flag_no_1_to_ch2 = false;
                    Program.FlowMeter_USF500.flag_no_2_to_ch1 = false; Program.FlowMeter_USF500.flag_no_2_to_ch2 = true;
                }
                else
                {

                }
                command[0] = address;
                //Checksum 계산
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
        public string Recieve_Data_To_Parse_USF500(ref byte[] rcv_data, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
        {
            int data_idx = 0;
            string result = "";
            try
            {
                //USF500 Rcv 구조
                //Ex
                //Send    Address(1), Function Code(1), Register Address(2), Reading Rester Count(2), CRC(2)
                //Send    01        , 03              , 00 14              , 00 01                  , C4 0E

                //Rcv     Address(1), Function Code(1), Reading Byte Count(1), Reading Data(2*N), CRC(2)      
                //                                                             Data1 Data2 Data3
                //Rcv     01        , 03              , 06                   , 01 F4 03 E8 00 02, 90 00
                rcv_data_parse.dt_rcvtime = DateTime.Now;
                rcv_data_parse.reading_data = null;
                rcv_data_parse.function_code = 0;
                rcv_data_parse.rcv_data_to_string = "";
                rcv_data_parse.crc = "";
                rcv_data_parse.read_data_byte_cnt = 0;
                rcv_data_parse.reading_data = null;

                if (rcv_data == null) { result = "Empty Array"; }
                else
                {
                    for (int idx = 0; idx < rcv_data.Length; idx++)
                    {

                        if (idx != 0) { }


                        if (idx == 0) { rcv_data_parse.address = rcv_data[0]; Console.Write(rcv_data[idx].ToString("X2")); }
                        if (idx == 1)
                        {
                            rcv_data_parse.function_code = rcv_data[1]; Console.Write(rcv_data[idx].ToString("X2"));
                        }
                        if (idx == 2) { rcv_data_parse.read_data_byte_cnt = rcv_data[2]; rcv_data_parse.reading_data = new string[rcv_data_parse.read_data_byte_cnt / 2]; Console.Write(rcv_data[idx].ToString("X2")); }
                        if (idx > 2 && idx < rcv_data.Length - 2)
                        {
                            //Function Code 6(Write)일때의 정상 응답은 보낸 명령과 동일하게 온다
                            //Function Code 6(Write)의 Error 응답은 Function Code에 0x80을 더한 만큼 온다
                            if (rcv_data_parse.function_code != 6)
                            {

                                rcv_data_parse.reading_data[data_idx] = rcv_data[idx].ToString("X2") + rcv_data[idx + 1].ToString("X2");
                                //Console.Write(rcv_data_parse.reading_data[data_idx]);
                                data_idx = data_idx + 1; idx = idx + 1;


                                //public static byte[] read_ch1_flow_Status =
                                //public static byte[] command_read_Regi_101_102_103 =
                                //Inst Flow, Integ Flow, CH Status CH1
                                //0000 0000 0003
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
                }

                if (rcv_data_parse.function_code != 6)
                {

                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                    {
                        if (rcv_data_parse.address == 1)
                        {
                            Program.main_form.SerialData.FlowMeter_USF500.last_rcv_time_ch1 = DateTime.Now;
                            if (Program.FlowMeter_USF500.flag_no_1_main == true)
                            {
                                Program.main_form.SerialData.FlowMeter_USF500.usf500_1_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[0], 16), 2).PadLeft(16, '0');
                                //Console.WriteLine("USF500 - 1 : " + Program.main_form.SerialData.FlowMeter_USF500.usf500_1_status);
                            }
                            else if (Program.FlowMeter_USF500.flag_no_1_to_ch1 == true)
                            {
                                //Console.WriteLine("1-1");
                                Program.main_form.SerialData.FlowMeter_USF500.NH4OH_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value == true)
                                {
                                    Program.main_form.SerialData.FlowMeter_USF500.NH4OH_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[1], 16) * 0.001;
                                }
                                Program.main_form.SerialData.FlowMeter_USF500.NH4OH_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                                //Program.log_md.LogWrite("APM - NH4OH Flow : " + Convert.ToInt32(rcv_data_parse.reading_data[0], 16) + " / Total : " + Convert.ToInt32(rcv_data_parse.reading_data[1], 16), Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                            }
                            else if (Program.FlowMeter_USF500.flag_no_1_to_ch2 == true)
                            {
                                //Program.main_form.SerialData.FlowMeter_USF500.NH4OH_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                                //Program.main_form.SerialData.FlowMeter_USF500.NH4OH_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[1], 16) * 0.001;
                                //Console.WriteLine(Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0'));
                            }
                        }
                        else if (rcv_data_parse.address == 2)
                        {
                            Program.main_form.SerialData.FlowMeter_USF500.last_rcv_time_ch2 = DateTime.Now;
                            if (Program.FlowMeter_USF500.flag_no_2_main == true)
                            {
                                Program.main_form.SerialData.FlowMeter_USF500.usf500_2_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[0], 16), 2).PadLeft(16, '0');
                                //Console.WriteLine("USF500 - 2 : " + Program.main_form.SerialData.FlowMeter_USF500.usf500_2_status);
                            }
                            else if (Program.FlowMeter_USF500.flag_no_2_to_ch1 == true)
                            {
                                //Console.WriteLine("2-1");
                                Program.main_form.SerialData.FlowMeter_USF500.DIW_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.01;
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                                {
                                    Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[1], 16) * 0.01;
                                }
                                Program.main_form.SerialData.FlowMeter_USF500.DIW_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                                //Program.log_md.LogWrite("APM - DIW Flow : " + Convert.ToInt32(rcv_data_parse.reading_data[0], 16) + " / Total : " + Convert.ToInt32(rcv_data_parse.reading_data[1], 16), Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                            }
                            else if (Program.FlowMeter_USF500.flag_no_2_to_ch2 == true)
                            {
                                //Console.WriteLine("2-2");
                                Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                                {
                                    Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[1], 16) * 0.001;
                                }
                                Program.main_form.SerialData.FlowMeter_USF500.H2O2_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                                //Program.log_md.LogWrite("APM - H2O2 Flow : " + Convert.ToInt32(rcv_data_parse.reading_data[0], 16) + " / Total : " + Convert.ToInt32(rcv_data_parse.reading_data[1], 16), Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                            }
                        }

                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                    {
                        if (rcv_data_parse.address == 1)
                        {
                            Program.main_form.SerialData.FlowMeter_USF500.last_rcv_time_ch1 = DateTime.Now;
                            if (Program.FlowMeter_USF500.flag_no_1_main == true)
                            {
                                Program.main_form.SerialData.FlowMeter_USF500.usf500_1_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[0], 16), 2).PadLeft(16, '0');
                                //Console.WriteLine("USF500 - 1 : " + Program.main_form.SerialData.FlowMeter_USF500.usf500_1_status);
                            }
                            else if (Program.FlowMeter_USF500.flag_no_1_to_ch1 == true)
                            {
                                Program.main_form.SerialData.FlowMeter_USF500.DIW_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.01;
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                                {
                                    Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[1], 16) * 0.01;
                                }
                                Program.main_form.SerialData.FlowMeter_USF500.DIW_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                                Alarm_Check(rcv_data_parse.address, 1, Program.main_form.SerialData.FlowMeter_USF500.DIW_status);
                            }
                            else if (Program.FlowMeter_USF500.flag_no_1_to_ch2 == true)
                            {
                                //Program.main_form.SerialData.FlowMeter_USF500.HF_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                                Program.main_form.SerialData.FlowMeter_USF500.HF_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                                {
                                    Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[1], 16) * 0.001;
                                }
                                Program.main_form.SerialData.FlowMeter_USF500.HF_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                                Alarm_Check(rcv_data_parse.address, 2, Program.main_form.SerialData.FlowMeter_USF500.HF_status);
                            }
                        }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (rcv_data_parse.address == 1)
                        {
                            Program.main_form.SerialData.FlowMeter_USF500.last_rcv_time_ch1 = DateTime.Now;
                            if (Program.FlowMeter_USF500.flag_no_1_main == true)
                            {
                                Program.main_form.SerialData.FlowMeter_USF500.usf500_1_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[0], 16), 2).PadLeft(16, '0');
                                //Console.WriteLine("USF500 - 1 : " + Program.main_form.SerialData.FlowMeter_USF500.usf500_1_status);
                            }
                            else if (Program.FlowMeter_USF500.flag_no_1_to_ch1 == true)
                            {
                                Program.main_form.SerialData.FlowMeter_USF500.DIW_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.01;
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                                {
                                    Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[1], 16) * 0.01;
                                }
                                Program.main_form.SerialData.FlowMeter_USF500.DIW_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                                Alarm_Check(rcv_data_parse.address, 1, Program.main_form.SerialData.FlowMeter_USF500.DIW_status);
                            }
                            else if (Program.FlowMeter_USF500.flag_no_1_to_ch2 == true)
                            {
                                //Program.main_form.SerialData.FlowMeter_USF500.HF_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                                Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                                {
                                    Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[1], 16) * 0.001;
                                }
                                Program.main_form.SerialData.FlowMeter_USF500.H2O2_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                                Alarm_Check(rcv_data_parse.address, 2, Program.main_form.SerialData.FlowMeter_USF500.H2O2_status);
                            }
                        }
                          else if (rcv_data_parse.address == 2)
                        {
                            Program.main_form.SerialData.FlowMeter_USF500.last_rcv_time_ch2 = DateTime.Now;
                            if (Program.FlowMeter_USF500.flag_no_2_main == true)
                            {
                                Program.main_form.SerialData.FlowMeter_USF500.usf500_2_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[0], 16), 2).PadLeft(16, '0');
                                //Console.WriteLine("USF500 - 2 : " + Program.main_form.SerialData.FlowMeter_USF500.usf500_2_status);
                            }
                            else if (Program.FlowMeter_USF500.flag_no_2_to_ch1 == true)
                            {
                                //Console.WriteLine("2-1");
                                Program.main_form.SerialData.FlowMeter_USF500.H2SO4_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B].value == true)
                                {
                                    Program.main_form.SerialData.FlowMeter_USF500.H2SO4_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[1], 16) * 0.001;
                                }
                                Program.main_form.SerialData.FlowMeter_USF500.H2SO4_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                                //Program.log_md.LogWrite("APM - DIW Flow : " + Convert.ToInt32(rcv_data_parse.reading_data[0], 16) + " / Total : " + Convert.ToInt32(rcv_data_parse.reading_data[1], 16), Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                            }
                            else if (Program.FlowMeter_USF500.flag_no_2_to_ch2 == true)
                            {
                                //Console.WriteLine("2-2");
                                Program.main_form.SerialData.FlowMeter_USF500.HF_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                                {
                                    Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[1], 16) * 0.001;
                                }
                                Program.main_form.SerialData.FlowMeter_USF500.HF_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                                //Program.log_md.LogWrite("APM - H2O2 Flow : " + Convert.ToInt32(rcv_data_parse.reading_data[0], 16) + " / Total : " + Convert.ToInt32(rcv_data_parse.reading_data[1], 16), Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("USF500" + "." + "Recieve_Data_To_Parse_USF500" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;
        }
        public int Alarm_Check(int address, int ch, string binary_data)
        {
            int result = 0;
            string tml_binary_data = "";
            string log = "";
            bool alarm_alive = false;
            try
            {
                tml_binary_data = binary_data.Reverse().ToString();
                if (binary_data.Length != 16)
                {
                    if(address == 1)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Flowmeter1_Alarm, "Binary Data Error", false, true);
                    }
                    else if (address == 2)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Flowmeter2_Alarm, "Binary Data Error", false, true);
                    }

                    return 0;
                }

                //00 00 00 00 00 00 00 00 16bit
                if (binary_data.Substring((int)enum_alarm_ch_status_usf500.ERROR_MEASURE + 1, 1) == "1")
                {
                    alarm_alive = true;
                }
                else if (binary_data.Substring((int)enum_alarm_ch_status_usf500.ERROR_MEASURE + 1, 1) == "0")
                {
                    alarm_alive = false;
                }

                if (address == 1 && ch == 1)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Flowmeter1_CH1_Measure_Error, "CH1 Measure Error(" + tml_binary_data + ")", false, alarm_alive);
                }
                else if (address == 1 && ch == 2)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Flowmeter1_CH2_Measure_Error, "CH2 Measure Error(" + tml_binary_data + ")", false, alarm_alive);
                }
                if (address == 2 && ch == 1)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Flowmeter2_CH1_Measure_Error, "CH1 Measure Error(" + tml_binary_data + ")", false, alarm_alive);
                }
                else if (address == 2 && ch == 2)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Flowmeter2_CH2_Measure_Error, "CH2 Measure Error(" + tml_binary_data + ")", false, alarm_alive);
                }

                if (binary_data.Substring((int)enum_alarm_ch_status_usf500.ERROR_INIT_0 + 1, 1) == "1")
                {
                    alarm_alive = true;
                }
                else if (binary_data.Substring((int)enum_alarm_ch_status_usf500.ERROR_INIT_0 + 1, 1) == "0")
                {
                    alarm_alive = false;
                }
                if (address == 1 && ch == 1)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Flowmeter1_Alarm, "CH1 Init Error(" + tml_binary_data + ")", false, alarm_alive);
                }
                else if (address == 1 && ch == 2)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Flowmeter1_Alarm, "CH2 Init Error(" + tml_binary_data + ")", false, alarm_alive);
                }
                if (address == 2 && ch == 1)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Flowmeter2_Alarm, "CH1 Init Error(" + tml_binary_data + ")", false, alarm_alive);
                }
                else if (address == 2 && ch == 2)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Flowmeter2_Alarm, "CH2 Init Error(" + tml_binary_data + ")", false, alarm_alive);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return result;

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
