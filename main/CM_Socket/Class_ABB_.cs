using System;
using System.Linq;
using System.Text;

namespace cds
{
    class Class_ABB
    {
        /// <summary>
        /// APM
        /// 1:H2O2 / 2:NH4OH
        /// </summary>

        public float concentration_1 = 0;
        public float concentration_2 = 0;
        public float concentration_3 = 0;
        public float concentration_4 = 0;

        public float concentration_1_old = 0;
        public float concentration_2_old = 0;
        public float concentration_3_old = 0;
        public float concentration_4_old = 0;

        public bool Online = false;
        public bool run_state = false;
        public DateTime dt_abb_validate_check = new DateTime();
        public DateTime dt_last_rcv;

        public static string Heartbit = "00 01 00 00 00 08 00 04 04 C2 00 01";

        public static string read_property_value1 = "00 01 00 00 00 08 00 04 00 0A 00 02";
        public static string read_property_value2 = "00 01 00 00 00 08 00 04 00 0C 00 02";
        public static string read_property_value3 = "00 01 00 00 00 08 00 04 00 0E 00 02";
        public static string read_property_value4 = "00 01 00 00 00 08 00 04 00 10 00 02";
        public static string read_property_value1_to_4 = "00 01 00 00 00 08 00 04 00 0A 00 08";
        //00 01 00 00 00 13 00 04 10 E0 24 44 17 00 01 00 00 3E F2 44 E3 FF 5C C4 79

        public static string read_DO_Status = "00 01 00 00 00 08 00 02 00 00 00 07"; //Alarm To online
                                                                                     //Rcv 00 01 00 00 00 04 00 02 01 43 Alarm(Anal, Process) on + Online

        public static string write_chemistry_01_selected = "00 01 00 00 00 08 00 05 00 00 00 01"; //(0:not / 1:select)
        public static string write_chemistry_02_selected = "00 01 00 00 00 08 00 05 00 01 00 01"; //(0:not / 1:select)

        public static string write_online = "00 01 00 00 00 08 00 05 00 0E 00 01"; //(0:not / 1:select)
        public static string write_offline = "00 01 00 00 00 08 00 05 00 0E 00 00"; //(0:not / 1:select)


        public static string write_reference_trigger_on = "00 01 00 00 00 08 00 05 00 0F 00 01"; //(0:not / 1:select)
        public static string write_reference_trigger_off = "00 01 00 00 00 08 00 05 00 0F 00 01"; //(0:not / 1:select)
        public byte[] Message_Command_To_Byte(string command)
        {
            char cSpace = ' ';
            byte[] send_msg = null;
            int idx = 0;
            try
            {
                string[] OutStringSplit = command.Split(cSpace);
                Array.Resize(ref send_msg, OutStringSplit.Length);
                foreach (string splitNumber in OutStringSplit)
                {
                    send_msg[idx] = Convert.ToByte(splitNumber, 16);
                    idx++;
                }
                //2회 이상 송신해야 응답, 확인 필요
                if (send_msg != null) { Program.main_form.abb_q_snd_data.Enqueue(send_msg); Program.main_form.abb_q_snd_data.Enqueue(send_msg); };
            }
            catch (Exception ex)
            {
                send_msg = null;
            }
            return send_msg;

        }
        public string Recieve_Data_To_Parse_ABB(TCP_IP_SOCKET.Cls_Socket.Socket_Event socket_event)
        {
            //Class_Serial_Common.Rcv_Data rcv_data_parse;
            int data_idx = 0;
            string result = "", log = "", log_alarm = "", log_dt_head = "", tmp_data = "";
            string tml_binary_data = "";
            string binary_data;

            bool alarm_alive = false;
            try
            {
                log_dt_head = DateTime.Now.ToString("HH:mm:ss.fff");
                socket_event.status = TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Recieve;
                if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Open)
                {
                    log = "ABB Socket Open";
                    Program.ABB.dt_abb_validate_check = DateTime.Now;
                    Program.ABB.run_state = true;
                }
                else if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Close)
                {
                    log = "ABB Socket Close";
                    Program.ABB.run_state = false;
                }
                else if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Recieve)
                {
                    Program.ABB.dt_abb_validate_check = DateTime.Now;
                    Program.ABB.run_state = true;

                    if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.HEX)
                    {
                        log = BitConverter.ToString(socket_event.data_to_array).Replace("-", " ");
                    }
                    else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.BYTE)
                    {
                        log = String.Join(" ", socket_event.data_to_array);
                    }
                    else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.STRING)
                    {
                        log = Encoding.Default.GetString(socket_event.data_to_array);
                    }
                    if (Program.main_form.log_abb_q_rcv_data.Count > 20) { Program.main_form.log_abb_q_rcv_data.Dequeue(); }
                    Program.main_form.log_abb_q_rcv_data.Enqueue(log_dt_head + " : " + log);
                    //농도계 값 파싱

                    Class_Serial_Common.Rcv_Data rcv_data_parse;

                    rcv_data_parse.dt_rcvtime = DateTime.Now;
                    rcv_data_parse.reading_data = null;
                    rcv_data_parse.function_code = 0;
                    rcv_data_parse.read_data_byte_cnt = 0;
                    if (socket_event.data_to_array == null) { result = "Empty Array"; }
                    else
                    {
                        for (int idx = 0; idx < socket_event.data_to_array.Length; idx++)
                        {

                            if (idx != 0) { Console.Write(","); }


                            if (idx == 0) { rcv_data_parse.address = socket_event.data_to_array[1]; }
                            if (idx == 1)
                            {
                                rcv_data_parse.function_code = socket_event.data_to_array[7];
                            }
                            if (idx == 2) { rcv_data_parse.read_data_byte_cnt = socket_event.data_to_array[5]; rcv_data_parse.reading_data = new string[rcv_data_parse.read_data_byte_cnt - 1 / 2]; }
                            if (idx > 2 && idx < socket_event.data_to_array.Length - 2)
                            {

                                if (rcv_data_parse.function_code != 6)
                                { //Holding, Input Register Rcv

                                    rcv_data_parse.reading_data[data_idx] = socket_event.data_to_array[idx].ToString("X2") + socket_event.data_to_array[idx + 1].ToString("X2");

                                    data_idx = data_idx + 1; idx = idx + 1;
                                }
                                else
                                {
                                }
                            }
                            else if (idx == socket_event.data_to_array.Length - 2)
                            {
                                rcv_data_parse.crc = socket_event.data_to_array[idx].ToString("X2") + socket_event.data_to_array[idx + 1].ToString("X2");
                                break;
                            }

                        }


                    }
                    if (rcv_data_parse.function_code == 4)
                    {
                        //Read Propert 1 to 4
                        //00 01 00 00 00 13 00 04 10 A6 87 44 2D 00 01 00 00 ED 29 44 F8 FF 5C C4 79
                        //                       PR1 A6 87 44 2D -> 44 2D A6 87 694.602                             
                        //                       PR2 00 01 00 00 -> 00 00 00 01 0 
                        //                       PR3 ED 29 44 F8 -> 44 F8 ED 29 1991.411

                        //Read Status Offline / online
                        //Send : 00 01 00 00 00 08 00 02 00 06 00 02
                        //Rcv  : 00 01 00 00 00 04 00 02 01 00(OffLine)
                        //       00 01 00 00 00 04 00 02 01 01(OnLine

                        //float[] tmp = new float[4];
                        //Buffer.BlockCopy(socket_event.data_to_array, 9, tmp, 0, 4);
                        if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                        {
                            if (rcv_data_parse.read_data_byte_cnt >= 19)
                            {
                                //NH4OH
                                tmp_data = socket_event.data_to_array[11].ToString("X2") + socket_event.data_to_array[12].ToString("X2") + socket_event.data_to_array[9].ToString("X2") + socket_event.data_to_array[10].ToString("X2");
                                Program.ABB.concentration_2 = Hex_To_Float(tmp_data);

                                //H2O2
                                tmp_data = socket_event.data_to_array[15].ToString("X2") + socket_event.data_to_array[16].ToString("X2") + socket_event.data_to_array[13].ToString("X2") + socket_event.data_to_array[14].ToString("X2");
                                Program.ABB.concentration_1 = Hex_To_Float(tmp_data);
                            }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                        {
                            if (rcv_data_parse.read_data_byte_cnt >= 19)
                            {
                                //DSP
                                tmp_data = socket_event.data_to_array[11].ToString("X2") + socket_event.data_to_array[12].ToString("X2") + socket_event.data_to_array[9].ToString("X2") + socket_event.data_to_array[10].ToString("X2");
                                Program.ABB.concentration_1 = Hex_To_Float(tmp_data);

                            }

                        }
                    }
                    else if (rcv_data_parse.function_code == 2)
                    {
                        binary_data = Convert.ToString(Convert.ToInt32(socket_event.data_to_array[9].ToString("X2"), 16), 2).PadLeft(16, '0');//Convert.ToString(operaion_data, 2).PadLeft(16, '0');
                                                                                                                                              //SCR Reverse not use
                        tml_binary_data = new string(binary_data.Reverse().ToArray());

                        if (tml_binary_data.Substring(0, 1) == "1") { log_alarm = log_alarm + ",Analyzer Alarm"; }
                        if (tml_binary_data.Substring(1, 1) == "1") { log_alarm = log_alarm + ",Process Alarm"; }
                        if (tml_binary_data.Substring(6, 1) == "1") { Online = true; } else { Online = false; }
                        if (log_alarm != "")
                        {
                            alarm_alive = true;
                        }
                        else
                        {
                            alarm_alive = false;
                        }
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Concentration_ABB_Alarm, log_alarm, false, alarm_alive);

                    }
                }
                Program.log_md.LogWrite("Rcv  ," + log, Module_Log.enumLog.ABB_DATA, "", Module_Log.enumLevel.ALWAYS);

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                log = result;
                Program.log_md.LogWrite(log, Module_Log.enumLog.ABB_DATA, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;
        }
        public float Hex_To_Float(string Hex)
        {
            float result = 0;

            try
            {
                uint num = uint.Parse(Hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                byte[] bfloat = BitConverter.GetBytes(num);
                result = BitConverter.ToSingle(bfloat, 0);
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(ex.ToString(), Module_Log.enumLog.ABB_DATA, "", Module_Log.enumLevel.ALWAYS);
            }
            return result;
        }

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
