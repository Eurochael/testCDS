using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cds.Class_ThermoStat_HE_3320C;

namespace cds
{
    public class Class_HeatExchanger
    {
        /// <summary>
        /// Read Start D1100
        /// Write Start D1200
        /// </summary>
        /// 
        public bool run_state = false;
        public DateTime dt_validate_check = new DateTime();
        public DateTime dt_last_rcv;
        public double pv_temp;
        public double chemical_set;
        public double chemical_in;
        public double chemical_out;
        public double filter_in_press;
        public double filter_out_press;
        public double chemical_in_press;
        public int watch_dog;
        public int watch_dog_old;
        public int watch_dog_count = 10;
        public bool heater_on;
        public bool flag_run_req;
        public int Alarm_1110;
        public int Alarm_1111;
        /// <summary>
        /// D1100 부터 15개 Read
        /// </summary>
        public static string Read_AllData = "00 01 00 00 00 08 01 04 00 00 00 0F";
        //00 01 00 00 00 06 01 06 00 00 09 C4 D1200 2500 Set
        /// <summary>
        /// D1200 Set Value
        /// </summary>
        public static string command_Set_Temp = "00 01 00 00 00 06 01 06 00 00 {0} {1}";
        /// <summary>
        /// D1201 Chemical In Offset
        /// </summary>
        public static string command_Chemical_In_Offset = "00 01 00 00 00 06 01 06 00 01 {0} {1}";
        /// <summary>
        /// D1202 Chemical Out Offset
        /// </summary>
        public static string command_Chemical_Out_Offset = "00 01 00 00 00 06 01 06 00 02 {0} {1}";
        public static string command_Run_Stop = "00 01 00 00 00 06 01 06 00 03 {0} {1}";
        public static string command_WatchDog = "00 01 00 00 00 06 01 06 00 04 {0} {1}";
        public enum enum_enum_AlarmD1110
        {
            Emergency_Stop = 0,
            TPR_Alarm = 1,
            Inverter_Alarm = 2,
            Tank_Overtemp_TH = 3,
            Leak = 4,
            Compressor_Overload = 5,
            Compressor_Low_pressure = 6,
            Compressor_High_pressure = 7,
            Tank_Level_low = 8,
            Door_1_open = 9,
            Door_2_open = 10,
            Door_3_open = 11,
            Door_4_open = 12,
            Door_5_open = 13,
            Temp_High_limit = 14,
            Temp_Low_limit = 15,
        }
        public enum enum_enum_AlarmD1111
        {
            PCW_Pressure_low = 0,
            CDA_Pressuer_low = 1,
            Temp_Upper_limit_DEV_Alarm = 2,
            Temp_Reached_time_Alarm = 3,
            Chemical_in_low_pressure = 4,
            Filter_in_high_pressure = 5,
            Filter_out_high_pressure = 6,
            Temp_Warning = 7,
            MAIN_MC_OFF = 8,
            Spare_9 = 9,
            Spare_10 = 10,
            Spare_11 = 11,
            Spare_12 = 12,
            Spare_13 = 13,
            Spare_14 = 14,
            Spare_15 = 15,
        }

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
                if (send_msg != null) { Program.main_form.heat_exchanger_q_snd_data.Enqueue(send_msg);};
            }
            catch (Exception ex)
            {
                send_msg = null;
            }
            return send_msg;
        }
        public byte[] Set_SV(int value)
        {
            string set_command = "";
            set_command = string.Format(command_Set_Temp, value.ToString("X4").Substring(0,2), value.ToString("X4").Substring(2,2));
            return Message_Command_To_Byte(set_command); ;
        }
        public byte[] Set_Offset_Chemical_in(int value)
        {
            string set_command = "";
            set_command = string.Format(command_Chemical_In_Offset, value.ToString("X4").Substring(0, 2), value.ToString("X4").Substring(2, 2));
            return Message_Command_To_Byte(set_command); ;
        }
        public byte[] Set_Offset_Chemical_Out(int value)
        {
            string set_command = "";
            set_command = string.Format(command_Chemical_Out_Offset, value.ToString("X4").Substring(0, 2), value.ToString("X4").Substring(2, 2));
            return Message_Command_To_Byte(set_command); ;
        }
        public byte[] Heater_On_Off(bool state)
        {
            int value = 1;

            if (state == true) { value = 1; }
            else { value = 0; }
            string set_command = "";
            set_command = string.Format(command_Run_Stop, value.ToString("X4").Substring(0, 2), value.ToString("X4").Substring(2, 2));
            return Message_Command_To_Byte(set_command); ;
        }
        public byte[] WatchDog(bool state)
        {
            int value = 1;
            if (state == true) { value = 1; }
            else { value = 0; }
            string set_command = "";
            set_command = string.Format(command_WatchDog, value.ToString("X4").Substring(0, 2), value.ToString("X4").Substring(2, 2));
            return Message_Command_To_Byte(set_command); ;
        }

        public string Recieve_Data_To_Parse_HeatExchanger(TCP_IP_SOCKET.Cls_Socket.Socket_Event socket_event)
        {
            int data_idx = 0;
            string result = "", log = "", log_dt_head = "";
            Class_Serial_Common.Rcv_Data rcv_data_parse = default;

            try
            {
                log_dt_head = DateTime.Now.ToString("HH:mm:ss.fff");
                socket_event.status = TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Recieve;
                if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Open)
                {
                    log = "HeatExchanger Socket Open";
                    Program.Heat_Exchanger.dt_validate_check = DateTime.Now;
                    Program.Heat_Exchanger.run_state = true;
                }
                else if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Close)
                {
                    log = "Heat_Exchanger Socket Close";
                    Program.Heat_Exchanger.run_state = false;
                }
                else if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Recieve)
                {
                    Program.Heat_Exchanger.dt_validate_check = DateTime.Now;
                    Program.Heat_Exchanger.run_state = true;

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
                    if (Program.main_form.log_heat_exchanger_q_rcv_data.Count > 20) { Program.main_form.log_heat_exchanger_q_rcv_data.Dequeue(); }
                    Program.main_form.log_heat_exchanger_q_rcv_data.Enqueue(log_dt_head + " : " + log);


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
                        if (rcv_data_parse.read_data_byte_cnt >= 30)
                        {
                            //00 01 00 00 00 21 01 04 1E 09 C4 0A 28 0A 8C 0A F0 0B 54 0B B8 00 01 00 00 00 01 00 00 20 01 00 20 00 00 00 00 00 00

                            //PV
                            //tmp_data = socket_event.data_to_array[11].ToString("X2") + socket_event.data_to_array[12].ToString("X2") + socket_event.data_to_array[9].ToString("X2") + socket_event.data_to_array[10].ToString("X2");
                            Program.Heat_Exchanger.pv_temp = Convert.ToInt16(rcv_data_parse.reading_data[3+0], 16) * 0.1;
                            Program.Heat_Exchanger.chemical_set = Convert.ToInt16(rcv_data_parse.reading_data[3 + 1], 16) * 0.1;
                            Program.Heat_Exchanger.chemical_in = Convert.ToInt16(rcv_data_parse.reading_data[3 + 2], 16) * 0.1;
                            Program.Heat_Exchanger.chemical_out = Convert.ToInt16(rcv_data_parse.reading_data[3 + 3], 16) * 0.1;
                            Program.Heat_Exchanger.filter_in_press = Convert.ToInt16(rcv_data_parse.reading_data[3 + 4], 16) * 0.1;
                            Program.Heat_Exchanger.filter_out_press = Convert.ToInt16(rcv_data_parse.reading_data[3 + 5], 16) * 0.1;
                            Program.Heat_Exchanger.chemical_in_press = Convert.ToInt16(rcv_data_parse.reading_data[3 + 6], 16) * 0.1;
                            Program.Heat_Exchanger.watch_dog = Convert.ToInt16(rcv_data_parse.reading_data[3 + 8]);
                            if (Program.Heat_Exchanger.watch_dog_old != Program.Heat_Exchanger.watch_dog)
                            {
                                watch_dog_count = 0;
                            }
                            else
                            {
                                watch_dog_count = watch_dog_count + 1;
                            }
                            if(watch_dog_count >= 10)
                            {
                                if(watch_dog_count >= 100) { watch_dog_count = 100; }
                                //Program.Heat_Exchanger.run_state = false;
                            }
                            else
                            {
                                Program.Heat_Exchanger.run_state = true;
                                Program.Heat_Exchanger.heater_on = Convert.ToBoolean(Convert.ToInt16(rcv_data_parse.reading_data[3 + 7]));
                            }
                            Program.Heat_Exchanger.watch_dog_old = Program.Heat_Exchanger.watch_dog;
                            Alarm_check(ref rcv_data_parse);
                        }
                    }

                }
                Program.log_md.LogWrite("Rcv  ," + log, Module_Log.enumLog.HEAT_EXCHANGER, "", Module_Log.enumLevel.ALWAYS);

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                log = result;
                Program.log_md.LogWrite(log, Module_Log.enumLog.HEAT_EXCHANGER, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;
        }

        public void Alarm_check(ref Class_Serial_Common.Rcv_Data rcv_data_parse)
        {
            int result = 0;
            string tml_binary_data = "";
            string log = "";
            string binary_data;
            int idx_parse = 0;
            int idx = 0;
            bool alarm_alive = false;
            string remark = "";
            idx_parse = 5; idx = 0;

            // 10 : Alarm D1110
            idx_parse = 13; idx = 0; alarm_alive = false; remark = "";
            tml_binary_data = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16), 2).PadLeft(16, '0');
            binary_data = new string(tml_binary_data.Reverse().ToArray());
            foreach (var temp in Enum.GetValues(typeof(enum_enum_AlarmD1110)))
            {
                if (binary_data.Substring(idx, 1) == "1")
                {
                    alarm_alive = true; remark = temp.ToString(); //remark = remark +  ", " +  temp.ToString();
                    break;
                }
                idx = idx + 1;
            }
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Heat_Exchanger_Error_D1110, remark, false, alarm_alive);

            // 10 : Alarm D1111
            idx_parse = 14; idx = 0; alarm_alive = false; remark = "";
            tml_binary_data = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16), 2).PadLeft(16, '0');
            binary_data = new string(tml_binary_data.Reverse().ToArray());
            foreach (var temp in Enum.GetValues(typeof(enum_enum_AlarmD1111)))
            {
                if (binary_data.Substring(idx, 1) == "1")
                {
                    alarm_alive = true; remark = temp.ToString();  //remark = remark + "," + temp.ToString();
                    break;
                }
                idx = idx + 1;
            }
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Heat_Exchanger_Error_D1111, remark, false, alarm_alive);
        }
    }
}
