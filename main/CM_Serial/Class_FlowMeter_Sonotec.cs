using System;

namespace cds
{
    public class Class_FlowMeter_Sonotec
    {
        public struct ST_FlowMeter_Sonotec
        {
            public double IPA_flow;
            public double IPA_totalusage;
            public string IPA_status;

            public double DIW_flow;
            public double DIW_totalusage;
            public string DIW_status;

            public double DSP_flow;
            public double DSP_totalusage;
            public string DSP_status;

            public double LAL_flow;
            public double LAL_totalusage;
            public string LAL_status;

            public string sonotec_1_status;

        }
        public enum enum_alarm_Sonotec
        {
            SERIOUS_FAULT = 7,
            NOT_USE4 = 6,
            MEASURE_FLOW_ERROR = 5,
            NOT_USE3 = 4,
            RESET_FLAG = 3,
            NOT_USE2 = 2,
            MEASURE_ERROR = 1,
            NOT_USE1 = 0,
        }
        //public static byte[] command_read_0004 =
        public static byte[] read_flow =
{
                //Read Register Flow
                0x01,        //ID
                0x03,        //Input Register 03
                0x00, 0x04,  //시작 번지 0
                0x00, 0x02,  //데이터 값  //HIGH : FF 00, LOW : 00 00
                0x85, 0xCA   //CheckSum 다시 계산함
         };
        //public static byte[] command_read_0006 =
        public static byte[] read_totalusage_flow =
{
                //Read Register Flow
                0x01,        //ID
                0x03,        //Input Register 03
                0x00, 0x06,  //시작 번지 0
                0x00, 0x02,  //데이터 값  //HIGH : FF 00, LOW : 00 00
                0x85, 0xCA   //CheckSum 다시 계산함
         };
        //public static byte[] command_read_0000_to_0006 =
        public static byte[] read_status_flow_totalusage =
{
                //Read Register Flow
                0x01,        //ID
                0x03,        //Input Register 03
                0x00, 0x00,  //시작 번지 0
                0x00, 0x08,  //데이터 값  //HIGH : FF 00, LOW : 00 00
                0x85, 0xCA   //CheckSum 다시 계산함
         };
        public static byte[] zeroset =
        //public static byte[] command_write_coil_05_ON =
{
                //Set SV Temp
                0x01,        //ID
                0x03,        //Input Register 03
                0x00, 0x05,  //시작 번지 0
                0xFF, 0x00,  //데이터 값  //HIGH : FF 00, LOW : 00 00
                0x9C, 0x3B   //CheckSum 다시 계산함
         };
        public static byte[] totalusage_reset =
        //public static byte[] command_write_coil_08_ON =
        {
                //Set SV Temp
                0x01,        //ID
                0x05,        //Input Register 05
                0x00, 0x08,  //시작 번지 0
                0xFF, 0x00,  //데이터 값  //HIGH : FF 00, LOW : 00 00
                0x00, 0x00   //CheckSum 다시 계산함
         };

        public void TotalUsage_Reset(enum_chemical ccss)
        {
            if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                if (ccss == enum_chemical.DIW)
                {
                    //Program.FloMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.totalusage_reset, (int)Config_IO.enum_ipa_serial_index.SONOTEC_FLOWMETER);
                }
                else if (ccss == enum_chemical.IPA)
                {
                    Program.FlowMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.totalusage_reset, (int)Config_IO.enum_ipa_serial_index.SONOTEC_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_Sonotec.IPA_totalusage = 0;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                if (ccss == enum_chemical.DIW)
                {
                    //Program.FloMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.totalusage_reset, (int)Config_IO.enum_ipa_serial_index.SONOTEC_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage = 0;
                }
                else if (ccss == enum_chemical.DSP)
                {
                    Program.FlowMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.totalusage_reset, (int)Config_IO.enum_ipa_serial_index.SONOTEC_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage = 0;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                if (ccss == enum_chemical.DIW)
                {
                    //Program.FloMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.totalusage_reset, (int)Config_IO.enum_ipa_serial_index.SONOTEC_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage = 0;
                }
                else if (ccss == enum_chemical.LAL)
                {
                    Program.FlowMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.totalusage_reset, (int)Config_IO.enum_ipa_serial_index.SONOTEC_FLOWMETER);
                    Program.main_form.SerialData.FlowMeter_Sonotec.LAL_totalusage = 0;
                }
            }
        }
        public byte[] Message_Command_Apply_CRC_TO_Send(byte address, byte ch, byte[] command, int idx_serial)
        {
            byte[] send_msg = null;
            byte[] crc = new byte[2];
            try
            {
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
        public string Recieve_Data_By_SONOTEC(ref byte[] rcv_data, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
        {
            int data_idx = 0;
            string result = "";
            string log = "";
            rcv_data_parse.dt_rcvtime = DateTime.Now;
            rcv_data_parse.reading_data = null;
            rcv_data_parse.function_code = 0;
            rcv_data_parse.rcv_data_to_string = "";
            rcv_data_parse.crc = "";
            rcv_data_parse.read_data_byte_cnt = 0;
            rcv_data_parse.reading_data = null;
            try
            {
                if (rcv_data == null) { result = "Empty Array"; }
                else
                {
                    for (int idx = 0; idx < rcv_data.Length; idx++)
                    {

                        if (idx != 0) { }
                        log = log + "," + rcv_data[idx];

                        if (idx == 0) { rcv_data_parse.address = rcv_data[0]; }
                        if (idx == 1)
                        {
                            rcv_data_parse.function_code = rcv_data[1];
                            //Console.Write(rcv_data[idx].ToString("X2"));
                        }
                        //if (idx == 2) { rcv_data_parse.read_data_byte_cnt = rcv_data[2]; rcv_data_parse.reading_data = new string[rcv_data_parse.read_data_byte_cnt]; Console.Write(rcv_data[idx].ToString("X2")); }
                        if (idx == 2) { rcv_data_parse.read_data_byte_cnt = rcv_data[2]; rcv_data_parse.reading_data = new string[rcv_data_parse.read_data_byte_cnt / 2]; }
                        if (idx > 2 && idx < rcv_data.Length - 2)
                        {
                            //Error Code는 Function Code + X80으로 응답
                            //Operation 응답 예
                            if (rcv_data_parse.function_code == 1)
                            {
                                //Read Status 8Byte
                                rcv_data_parse.reading_data[data_idx] = rcv_data[idx].ToString("X2"); //+ rcv_data[idx + 1].ToString("X2");
                                //Console.Write(rcv_data_parse.reading_data[data_idx]);
                                //0x00 FS: Not in use = 0, FBS:Bubble Alarm
                                //0x01 A warning has occurred, measurement is moving on
                                //0x02
                                //0x03 Reset Flag: a restart has been performed within the last request interval
                                //0x04
                                //0x05 Error in flow measurement, flow value is affected, is not valid
                                //0x06
                                //0x07 serious device fault, measurement is halted
                                //Program.main_form.SerialData.sono.Concenctration = Convert.ToDouble(rcv_data.Split(',')[1]);

                            }
                            else if (rcv_data_parse.function_code == 5)
                            {
                                //Write Coil
                                //rcv_data_parse.reading_data[data_idx] = rcv_data[idx].ToString("X2"); //+ rcv_data[idx + 1].ToString("X2");
                                //Console.Write(rcv_data_parse.reading_data[data_idx]);
                                //Console.Write(rcv_data[idx].ToString("X2"));
                            }
                            else if (rcv_data_parse.function_code != 6)
                            { //Holding, Input Register Rcv
                                rcv_data_parse.reading_data[data_idx] = rcv_data[idx].ToString("X2") + rcv_data[idx + 1].ToString("X2");
                                data_idx = data_idx + 1; idx = idx + 1;

                                //rcv_data_parse.reading_data[data_idx] = rcv_data[idx].ToString("X2") + rcv_data[idx + 1].ToString("X2");
                                //Console.Write(rcv_data_parse.reading_data[data_idx]);

                                //                            data_idx = data_idx + 1;// idx = idx + 1;
                                //Console.Write(rcv_data[idx].ToString("X2"));
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

                if (rcv_data_parse.function_code == 3)
                {
                    ///적산 사용시 ul -> ml 변환시 1000사용 필요
                    if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                    {
                        if (rcv_data_parse.address == 1)
                        {
                            Program.main_form.SerialData.FlowMeter_Sonotec.DSP_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == true)
                            {
                                //Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[5], 16)* 0.0001;
                            }
                            Program.main_form.SerialData.FlowMeter_Sonotec.DSP_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                        }

                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {
                        if (rcv_data_parse.address == 1)
                        {
                            Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                            {
                                //Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[5], 16)* 0.0001;
                            }
                            Program.main_form.SerialData.FlowMeter_Sonotec.DSP_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                        }
                        

                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                    {
                        if (rcv_data_parse.address == 1)
                        {
                            Program.main_form.SerialData.FlowMeter_Sonotec.LAL_flow = Convert.ToInt32(rcv_data_parse.reading_data[0], 16) * 0.001;
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == true)
                            {
                                //Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage = Convert.ToInt32(rcv_data_parse.reading_data[5], 16)* 0.0001;
                            }
                            Program.main_form.SerialData.FlowMeter_Sonotec.LAL_status = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[2], 16), 2).PadLeft(16, '0');
                        }


                    }
                    //Program.log_md.LogWrite("Recieve_Data_By_SONOTEC" + "." + Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value.ToString() + "," + log, Module_Log.enumLog.OriginalData, "", Module_Log.enumLevel.ALWAYS);
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("SONOTEC" + "." + "Recieve_Data_By_SONOTEC" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;


        }
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
