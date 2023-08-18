using System;
using System.Linq;
using System.Text;

namespace cds
{
    public class Class_TempController_M74
    {

        public struct ST_TempController_M74
        {
            //M74R은 M74에 귀속된다, 국번이 별도로 없음
            /// <summary>
            /// DHF
            /// 1번 M74 - 1국번 1번 채널은 Tank A Temp, 2번 채널은 Tank B Temp
            /// </summary>

            /// <summary>
            /// APM
            /// M74(1) + M74(2) 병렬 -> Serial 8번 포트 / M74(3) + M74R(4) 병렬 -> Serial 2번 포트
            ///1번 M74 - 1국번 1번 채널은 TANK A Temp, 2번 채널은 TANK B Temp, 3번 채널은 TS-09
            ///2번 M74 - 2국번 1번 채널은 SUPPLY A TEMP, 2번 채널은 SUPPLY B Temp, 3번 채널은 CIRCULATION Temp
            ///3번 M74 - 3국번 1번 채널은 SUPPLY A HEATER, 2번 채널은 SUPPLY B HEATER, 3번 채널은 CIRCULATION1 HEATER, 4번 채널은 CIRCULATION2 HEATER
            ///4번 M74R - 
            ///
            /// IPA
            /// Temp Barrier1 : Tank Temp / Temp Barrier2 : Suppy Temp / Temp Barrier3 : Supply Heater
            /// </summary>

            //public double tank_a_temp, tank_b_temp, ts_09_temp;
            //public double supply_a_temp, supply_b_temp, circulation_temp;
            //public double supply_heater_a_temp, supply_heater_b_temp, circulation_heater1_temp, circulation_heater2_temp;

            //public int tank_a_status, tank_b_status, ts_09_status;
            //public int supply_a_status, supply_b_status, circulation_status;
            //public int supply_heater_a_status, supply_heater_b_status, circulation_heater1_status, circulation_heater2_status;

            public ST_M74_Unit tank_a;
            public ST_M74_Unit tank_b;
            public ST_M74_Unit ts_09;
            public ST_M74_Unit supply_a;
            public ST_M74_Unit supply_b;
            public ST_M74_Unit circulation;

            //address 3
            public ST_M74_Unit supply_heater_a;
            public ST_M74_Unit supply_heater_b;
            public ST_M74_Unit circulation_heater1;
            public ST_M74_Unit circulation_heater2;

            public DateTime last_rcv_time_ch1;
            public DateTime last_rcv_time_ch2;
            public DateTime last_rcv_time_ch3;

            public struct ST_M74_Unit
            {
                public double pv;
                public double sv;
                public double mv;
                public int status;
                public int alarm_output;
                public double offset;
                public enum_m74_status mode;
            }

            //public double M74_1_PV_CH1, M74_1_PV_CH2, M74_1_PV_CH3, M74_1_PV_CH4;
            //public double M74_2_PV_CH1, M74_2_PV_CH2, M74_2_PV_CH3, M74_2_PV_CH4;
            //public double M74_3_PV_CH1, M74_3_PV_CH2, M74_3_PV_CH3, M74_3_PV_CH4;

            //public double M74_1_SV_CH1, M74_1_SV_CH2, M74_1_SV_CH3, M74_1_SV_CH4;
            //public double M74_2_SV_CH1, M74_2_SV_CH2, M74_2_SV_CH3, M74_2_SV_CH4;
            //public double M74_3_SV_CH1, M74_3_SV_CH2, M74_3_SV_CH3, M74_3_SV_CH4;

            //public int M74_1_Alarm_CH1, M74_1_Alarm_CH2, M74_1_Alarm_CH3, M74_1_Alarm_CH4;
            //public int M74_2_Alarm_CH1, M74_2_Alarm_CH2, M74_2_Alarm_CH3, M74_2_Alarm_CH4;
            //public int M74_3_Alarm_CH1, M74_3_Alarm_CH2, M74_3_Alarm_CH3, M74_3_Alarm_CH4;

        }
        public enum enum_m74_status
        {
            error = 0,
            output_alarm = 1,
            run = 2,
            stop = 3,
            at = 4
        }

        public enum enum_m74_type
        {
            tank_a = 0,
            tank_b = 1,
            ts_09 = 2,
            supply_a = 3,
            supply_b = 4,
            circulation = 5,
            supply_heater_a = 6,
            supply_heater_b = 7,
            circulation_heater1 = 8,
            circulation_heater2 = 9,
        }

        public enum enum_call_by
        {
            M74_1 = 0, M74_2 = 1
        }
        public enum enum_ch_status_map
        {
            System_Error = 15,
            Sub_Firmware_Error = 14,
            EEPROM_Error = 13,
            RJC_Error = 12,
            System_Comm_Error = 11,
            AT_Error = 10,
            Minus_Over_Input = 9,
            Plus_Over_Input = 8,
            Minus_Burn_Out = 7,
            Plus_Burn_Out = 6,
            RJ_Adjust_Error = 5,
            Run = 4,
            Manual_Control = 3,
            Auto_Tunning = 2,
            Output_of_Heating = 1,
            Output_of_Cooling = 0,
        }
        public enum enum_alarm_output
        {
            SPARE_15 = 15,
            SPARE_14 = 14,
            SPARE_13 = 13,
            SPARE_12 = 12,
            SPARE_11 = 11,
            SPARE_10 = 10,
            SPARE_9 = 9,
            SPARE_8 = 8,
            SPARE_7 = 7,
            SPARE_6 = 6,
            HOC = 5,
            HBA = 4,
            ALARM_4 = 3,
            ALARM_3 = 2,
            ALARM_2 = 1,
            ALARM_1 = 0,
        }
        //serialPort1.Close();
        //        serialPort1.PortName = "COM5";
        //        serialPort1.BaudRate = 19200;
        //        serialPort1.Parity = System.IO.Ports.Parity.Even;
        //        serialPort1.DataBits = 8;
        //        serialPort1.StopBits = System.IO.Ports.StopBits.One;
        //        serialPort1.Open();

        public static string command_scan_device = "01DRS,01,0049";
        public static string command_CH1_CH2_Read = "01DRS,02,0001";
        public static string command_CH1_Data_Read_PV_SV = "01DRR,02,0001,0005";
        public static string command_CH2_Data_Read_PV_SV = "01DRR,02,0002,0006";
        public static string command_CH3_Data_Read_PV_SV = "01DRR,02,0003,0007";
        public static string command_CH4_Data_Read_PV_SV = "01DRR,02,0004,0008";

        public static string command_CH1_Data_Read_PV_SV_STATUS = "01DRR,03,0001,0005,0027";
        public static string command_CH2_Data_Read_PV_SV_STATUS = "01DRR,03,0002,0006,0028";
        public static string command_CH3_Data_Read_PV_SV_STATUS = "01DRR,03,0003,0007,0029";
        public static string command_CH4_Data_Read_PV_SV_STATUS = "01DRR,03,0004,0008,0030";

        //각 채널별 PV, SV, MV, CT
        //public string command_CH1_Data_Read = "01DRR,04,0001,0005,0009,0013";
        //public string command_CH2_Data_Read = "01DRR,04,0002,0006,0010,0014";
        //public string command_CH3_Data_Read = "01DRR,04,0003,0007,0011,0015";
        //public string command_CH4_Data_Read = "01DRR,04,0004,0008,0012,0016";

        //각 채널별 PV, SV, CH Status 조회 + Alarm 출력 조회
        public static string command_CH1_To_CH4_Data_Read_SIMPLE = "01DRR,24,0001,0005,0027,0002,0006,0028,0003,0007,0029,0004,0008,0030,0031,0032,0033,0034,0009,0010,0011,0012,0189,0289,0389,0489";
        //각 채널별 PV, SV, MV, CT, CH Status + Alarm 출력 조회
        public static string command_CH1_To_CH4_Data_Read = "01DRR,24,0001,0005,0009,0013,0027,0031,0002,0006,0010,0014,0028,0032,0003,0007,0011,0015,0029,0033,0004,0008,0012,0016,0030,0034";

        public static string command_CH1_Set_Temp = "01DWR,01,0082,{0}";
        public static string command_CH2_Set_Temp = "01DWR,01,0083,{0}";
        public static string command_CH3_Set_Temp = "01DWR,01,0084,{0}";
        public static string command_CH4_Set_Temp = "01DWR,01,0085,{0}";

        public static string command_CH1_Set_Offset = "01DWR,01,0189,{0}";
        public static string command_CH2_Set_Offset = "01DWR,01,0289,{0}";
        public static string command_CH3_Set_Offset = "01DWR,01,0389,{0}";
        public static string command_CH4_Set_Offset = "01DWR,01,0489,{0}";

        public static string command_CH1_Set_Output = "01DWR,01,0166,{0}";
        public static string command_CH2_Set_Output = "01DWR,01,0266,{0}";
        public static string command_CH3_Set_Output = "01DWR,01,0366,{0}";
        public static string command_CH4_Set_Output = "01DWR,01,0466,{0}";

        public static string command_CH1_RUN_STOP = "01DWR,01,0059,{0}"; //0 STOP : 1 RUN
        public static string command_CH2_RUN_STOP = "01DWR,01,0060,{0}";
        public static string command_CH3_RUN_STOP = "01DWR,01,0061,{0}";
        public static string command_CH4_RUN_STOP = "01DWR,01,0062,{0}";

        public static string command_CH1_AT_RUN_STOP = "01DWR,01,0086,{0}"; //0 STOP : 1 RUN
        public static string command_CH2_AT_RUN_STOP = "01DWR,01,0087,{0}";
        public static string command_CH3_AT_RUN_STOP = "01DWR,01,0088,{0}";
        public static string command_CH4_AT_RUN_STOP = "01DWR,01,0089,{0}";

        //public string command_CH1_TO_CH4_RUN = "01DWR,04,0059,0060,0061,0062";

        public static string command_CH1_STOP = "01DWR,01,0059";
        public string Message_Command_To_Byte_M74_TO_Send(byte address, string command, ref byte[] send_msg, int idx_serial)
        {
            string result = "";
            int checksum = 0;
            string checksum_parse;
            string msg = "";
            try
            {
                command.Remove(0, 2); //국번 변경
                command = string.Format("{0:X2} ", address) + command;
                //Checksum 계산
                for (int idx = 0; idx < command.Length; idx++)
                { checksum = checksum + command[idx]; }
                checksum_parse = checksum.ToString("X").Substring(checksum.ToString("X").Length - 2, 2);
                msg = Convert.ToChar(0x02) + command + checksum_parse + Convert.ToChar(0x0D) + Convert.ToChar(0x0A);
                send_msg = Encoding.UTF8.GetBytes(msg);
                result = "";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            if (idx_serial != -1) { Program.main_form.Serial_data_Enqueue(ref send_msg, idx_serial); }
            return result;
        }
        public void M74_Temp_Set(enum_m74_type type, int value)
        {

            if (type == enum_m74_type.tank_a)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH1_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                    Program.TempController_M74.Set_CH1_OUTPORT(1, Convert.ToInt32("1"), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH1_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_ipa_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.tank_b)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH2_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                    if (Program.cg_app_info.use_heating_output_mapping_tank == false)
                    {
                        Program.TempController_M74.Set_CH1_OUTPORT(1, Convert.ToInt32("1"), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                    }
                    else
                    {
                        Program.TempController_M74.Set_CH1_OUTPORT(1, Convert.ToInt32("21"), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                    }
                }
            }
            else if (type == enum_m74_type.ts_09)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH3_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_a)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(2, string.Format(command_CH1_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH2_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_ipa_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_b)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(2, string.Format(command_CH2_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.circulation)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(2, string.Format(command_CH3_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_heater_a)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH1_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH3_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_ipa_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_heater_b)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH2_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.circulation_heater1)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH3_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.circulation_heater2)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH4_Set_Temp, value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }


        }
        public void M74_Run_And_Stop(enum_m74_type type, bool run)
        {
            int bool_to_value = 0;
            if (run == true)
            {
                bool_to_value = 1;
            }
            else
            {
                bool_to_value = 0;
            }
            if (type == enum_m74_type.tank_a)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH1_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH1_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_ipa_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.tank_b)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH2_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.ts_09)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH3_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_a)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(2, string.Format(command_CH1_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH2_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_ipa_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_b)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(2, string.Format(command_CH2_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.circulation)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(2, string.Format(command_CH3_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_heater_a)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH1_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH3_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_ipa_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_heater_b)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH2_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.circulation_heater1)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH3_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.circulation_heater2)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH4_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }


        }
        public void M74_AT_Run_And_Stop(enum_m74_type type, bool run)
        {
            int bool_to_value = 0;
            if (run == true)
            {
                bool_to_value = 1;
            }
            else
            {
                bool_to_value = 0;
            }
            if (type == enum_m74_type.tank_a)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH1_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH1_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_ipa_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.tank_b)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH2_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.ts_09)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH3_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_a)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(2, string.Format(command_CH1_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH2_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_ipa_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_b)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(2, string.Format(command_CH2_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.circulation)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(2, string.Format(command_CH3_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_heater_a)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH1_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Message_Command_To_Byte_M74_TO_Send(1, string.Format(command_CH3_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_ipa_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.supply_heater_b)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH2_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.circulation_heater1)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH3_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }
            else if (type == enum_m74_type.circulation_heater2)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Message_Command_To_Byte_M74_TO_Send(3, string.Format(command_CH4_AT_RUN_STOP, bool_to_value.ToString("X4")), (int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74);
                }
            }


        }
        public byte[] Set_CH1_SV(byte address, int value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH1_Set_Temp, value.ToString("X4")), idx_serial); ;
        }
        public byte[] Set_CH2_SV(byte address, int value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH2_Set_Temp, value.ToString("X4")), idx_serial); ;
        }
        public byte[] Set_CH3_SV(byte address, int value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH3_Set_Temp, value.ToString("X4")), idx_serial); ;
        }
        public byte[] Set_CH4_SV(byte address, int value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH4_Set_Temp, value.ToString("X4")), idx_serial); ;
        }
        public byte[] OffSet_CH1_SV(byte address, Int16 value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH1_Set_Offset, value.ToString("X4")), idx_serial); ;
        }
        public byte[] OffSet_CH2_SV(byte address, Int16 value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH2_Set_Offset, value.ToString("X4")), idx_serial); ;
        }
        public byte[] OffSet_CH3_SV(byte address, Int16 value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH3_Set_Offset, value.ToString("X4")), idx_serial); ;
        }
        public byte[] OffSet_CH4_SV(byte address, Int16 value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH4_Set_Offset, value.ToString("X4")), idx_serial); ;
        }
        public byte[] Set_CH1_OUTPORT(byte address, int value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH1_Set_Output, value.ToString("X4")), idx_serial); ;
        }
        public byte[] Set_CH2_OUTPORT(byte address, int value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH2_Set_Output, value.ToString("X4")), idx_serial); ;
        }
        public byte[] Set_CH3_OUTPORT(byte address, int value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH3_Set_Output, value.ToString("X4")), idx_serial); ;
        }
        public byte[] Set_CH4_OUTPORT(byte address, int value, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH4_Set_Output, value.ToString("X4")), idx_serial); ;
        }
        public byte[] RUN_CH1(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH1_RUN_STOP, 1.ToString("X4")), idx_serial); ;
        }
        public byte[] RUN_CH2(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH2_RUN_STOP, 1.ToString("X4")), idx_serial); ;
        }
        public byte[] RUN_CH3(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH3_RUN_STOP, 1.ToString("X4")), idx_serial); ;
        }
        public byte[] RUN_CH4(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH4_RUN_STOP, 1.ToString("X4")), idx_serial); ;
        }
        public byte[] STOP_CH1(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH1_RUN_STOP, 0.ToString("X4")), idx_serial); ;
        }
        public byte[] STOP_CH2(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH2_RUN_STOP, 0.ToString("X4")), idx_serial); ;
        }
        public byte[] STOP_CH3(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH3_RUN_STOP, 0.ToString("X4")), idx_serial); ;
        }
        public byte[] STOP_CH4(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH4_RUN_STOP, 0.ToString("X4")), idx_serial); ;
        }
        public byte[] AT_RUN_CH1(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH1_AT_RUN_STOP, 1.ToString("X4")), idx_serial); ;
        }
        public byte[] AT_RUN_CH2(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH2_AT_RUN_STOP, 1.ToString("X4")), idx_serial); ;
        }
        public byte[] AT_RUN_CH3(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH3_AT_RUN_STOP, 1.ToString("X4")), idx_serial); ;
        }
        public byte[] AT_RUN_CH4(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH4_AT_RUN_STOP, 1.ToString("X4")), idx_serial); ;
        }
        public byte[] AT_STOP_CH1(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH1_AT_RUN_STOP, 0.ToString("X4")), idx_serial); ;
        }
        public byte[] AT_STOP_CH2(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH2_AT_RUN_STOP, 0.ToString("X4")), idx_serial); ;
        }
        public byte[] AT_STOP_CH3(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH3_AT_RUN_STOP, 0.ToString("X4")), idx_serial); ;
        }
        public byte[] AT_STOP_CH4(byte address, int idx_serial)
        {
            return Message_Command_To_Byte_M74_TO_Send(address, string.Format(command_CH4_AT_RUN_STOP, 0.ToString("X4")), idx_serial); ;
        }
        public byte[] Message_Command_To_Byte_M74_TO_Send(byte address, string command, int idx_serial)
        {
            byte[] send_msg = new byte[0];
            int checksum = 0;
            string checksum_parse;
            string msg = "";
            try
            {
                command = command.Remove(0, 2); //국번 변경
                command = string.Format("{0:X2} ", address).Trim() + command;
                //Checksum 계산
                for (int idx = 0; idx < command.Length; idx++)
                { checksum = checksum + command[idx]; }
                checksum_parse = checksum.ToString("X").Substring(checksum.ToString("X").Length - 2, 2);
                msg = Convert.ToChar(0x02) + command + checksum_parse + Convert.ToChar(0x0D) + Convert.ToChar(0x0A);
                send_msg = Encoding.UTF8.GetBytes(msg);
            }
            catch (Exception ex)
            {
                send_msg = null;
            }
            if (idx_serial != -1) { Program.main_form.Serial_data_Enqueue(ref send_msg, idx_serial); }
            return send_msg;
        }

        public string Recieve_Data_To_Parse_M74(enum_call_by call_by, string rcv_data, ref string parse, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
        {
            string result = "";
            try
            {
                rcv_data_parse.dt_rcvtime = DateTime.Now;
                rcv_data_parse.reading_data = null;
                rcv_data_parse.function_code = 0;
                rcv_data_parse.rcv_data_to_string = "";
                rcv_data_parse.crc = "";
                rcv_data_parse.read_data_byte_cnt = 0;
                rcv_data_parse.reading_data = null;
                rcv_data_parse.address = 0;
                parse = rcv_data.Replace(Convert.ToChar(0x02), ' ').Replace(Convert.ToChar(0x0D), ' ').Replace(Convert.ToChar(0x0A), ' ');
                parse = parse.Trim();
                rcv_data_parse.address = Convert.ToByte(parse.ToString().Substring(0, 2));
                //커맨드로 구분되지 않기 때문에
                //rcv_data.Split(',').Length로 구분 / Command를 Split으로 구분할 수 있도록 명령어를 나눠놓음
                //Console.WriteLine("**********************");
                //Console.WriteLine("RCV RAW DATA : " + parse);
                if (rcv_data.Split(',').Length > 2 && rcv_data.IndexOf(Convert.ToChar(0x02)) >= 0 && rcv_data.IndexOf(Convert.ToChar(0x0D)) >= 0)
                {
                    //rcv_data_parse.address = Convert.ToByte(rcv_data.Split(',')[1], 16);
                    if (rcv_data.Split(',').Length == 3)
                    {

                    }
                    else if (rcv_data.Split(',').Length == 4)
                    {
                        parse = string.Format("RCV PARSE DATA : CH1 PV : {0}, SV : {1}", Convert.ToInt32(rcv_data.Split(',')[2], 16), Convert.ToInt32(rcv_data.Split(',')[3].Substring(0, 4), 16));
                    }
                    else if (rcv_data.Split(',').Length == 6)
                    {
                        parse = string.Format("RCV PARSE DATA : CH1 PV : {0}, SV : {1}, MV : {2}, CT : {3}", Convert.ToInt32(rcv_data.Split(',')[2], 16), Convert.ToInt32(rcv_data.Split(',')[3], 16), Convert.ToInt32(rcv_data.Split(',')[4], 16), Convert.ToInt32(rcv_data.Split(',')[5].Substring(0, 4), 16));
                    }
                    else if (rcv_data.Split(',').Length == 26)
                    {
                        //CH1 ~ CH4 까지 4개의 Data 동시 Read Simple
                        //Ex 25000,30,320,25000,0,848,25000,0,336,25000,0,336
                        rcv_data_parse.rcv_data_to_string = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}",
                            Convert.ToInt32(rcv_data.Split(',')[2], 16), Convert.ToInt32(rcv_data.Split(',')[3], 16), Convert.ToInt32(rcv_data.Split(',')[4], 16),
                            Convert.ToInt32(rcv_data.Split(',')[5], 16), Convert.ToInt32(rcv_data.Split(',')[6], 16), Convert.ToInt32(rcv_data.Split(',')[7], 16),
                            Convert.ToInt32(rcv_data.Split(',')[8], 16), Convert.ToInt32(rcv_data.Split(',')[9], 16), Convert.ToInt32(rcv_data.Split(',')[10], 16),
                            Convert.ToInt32(rcv_data.Split(',')[11], 16), Convert.ToInt32(rcv_data.Split(',')[12], 16), Convert.ToInt32(rcv_data.Split(',')[13], 16),
                            Convert.ToInt32(rcv_data.Split(',')[14], 16), Convert.ToInt32(rcv_data.Split(',')[15], 16), Convert.ToInt32(rcv_data.Split(',')[16], 16),
                            Convert.ToInt32(rcv_data.Split(',')[17], 16), Convert.ToInt32(rcv_data.Split(',')[18], 16), Convert.ToInt32(rcv_data.Split(',')[19], 16),
                            Convert.ToInt32(rcv_data.Split(',')[20], 16), Convert.ToInt32(rcv_data.Split(',')[21], 16), Convert.ToInt16(rcv_data.Split(',')[22], 16),
                            Convert.ToInt16(rcv_data.Split(',')[23], 16), Convert.ToInt16(rcv_data.Split(',')[24], 16), Convert.ToInt16(rcv_data.Split(',')[25].Substring(0, 4), 16));

                        ///APM
                        ///1번 M74 - 1국번 1번 채널은 TANK A Temp, 2번 채널은 TANK B Temp, 3번 채널은 TS-09
                        ///2번 M74 - 2국번 1번 채널은 SUPPLY A TEMP, 2번 채널은 SUPPLY B Temp, 3번 채널은 CIRCULATION Temp
                        ///3번 M74 - 3국번 1번 채널은 SUPPLY A HEATER, 2번 채널은 SUPPLY B HEATER, 3번 채널은 CIRCULATION1 HEATER, 4번 채널은 CIRCULATION2 HEATER

                        //PV SV Status

                        //각 채널별 PV, SV, MV, CT,  CH Status, Alarm Status 조회
                        //command_CH1_To_CH4_Data_Read_SIMPLE = "01DRR,12,0001,0005,0027,0002,0006,0028,0003,0007,0029,0004,0008,0030,0031,0032,0033,0034,0009,0010,0011,0012";
                        //                                                CH1            CH2            CH3            CH4
                        //마지막 데이터의 경우 Checksum 제외 후 사용
                        //Program.log_md.LogWrite("M74" + "." + "Recieve_Data_To_Parse_M74" + "." + rcv_data_parse.rcv_data_to_string, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);

                        //Offset은 Double World in16으로 환산
                        if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                        {

                            if (rcv_data_parse.address == 1)
                            {
                                Program.main_form.SerialData.TEMP_CONTROLLER.last_rcv_time_ch1 = DateTime.Now;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = Convert.ToInt32(rcv_data.Split(',')[2], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.sv = Convert.ToInt32(rcv_data.Split(',')[3], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.status = Convert.ToInt32(rcv_data.Split(',')[4], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.alarm_output = Convert.ToInt32(rcv_data.Split(',')[14], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.mv = Convert.ToInt16(rcv_data.Split(',')[18], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.offset = Convert.ToInt16(rcv_data.Split(',')[22], 16) * 0.1;
                                Alarm_check(enum_m74_type.tank_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.status);
                                Alarm_Out_check(enum_m74_type.tank_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.alarm_output);

                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = Convert.ToInt32(rcv_data.Split(',')[5], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.sv = Convert.ToInt32(rcv_data.Split(',')[6], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.status = Convert.ToInt32(rcv_data.Split(',')[7], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.alarm_output = Convert.ToInt32(rcv_data.Split(',')[15], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.mv = Convert.ToInt16(rcv_data.Split(',')[19], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.offset = Convert.ToInt16(rcv_data.Split(',')[23], 16) * 0.1;
                                Alarm_check(enum_m74_type.tank_b, rcv_data_parse.address, 2, Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.status);
                                Alarm_Out_check(enum_m74_type.tank_b, rcv_data_parse.address, 2, Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.alarm_output);

                                Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.pv = Convert.ToInt32(rcv_data.Split(',')[8], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.sv = Convert.ToInt32(rcv_data.Split(',')[9], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.status = Convert.ToInt32(rcv_data.Split(',')[10], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.alarm_output = Convert.ToInt32(rcv_data.Split(',')[16], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.mv = Convert.ToInt16(rcv_data.Split(',')[20], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.offset = Convert.ToInt16(rcv_data.Split(',')[24], 16) * 0.1;
                                Alarm_check(enum_m74_type.ts_09, rcv_data_parse.address, 3, Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.status);
                                Alarm_Out_check(enum_m74_type.ts_09, rcv_data_parse.address, 3, Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.alarm_output);
                            }
                            else if (rcv_data_parse.address == 2)
                            {
                                Program.main_form.SerialData.TEMP_CONTROLLER.last_rcv_time_ch2 = DateTime.Now;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.pv = Convert.ToInt32(rcv_data.Split(',')[2], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.sv = Convert.ToInt32(rcv_data.Split(',')[3], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.status = Convert.ToInt32(rcv_data.Split(',')[4], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.alarm_output = Convert.ToInt32(rcv_data.Split(',')[14], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.mv = Convert.ToInt16(rcv_data.Split(',')[18], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.offset = Convert.ToInt16(rcv_data.Split(',')[22], 16) * 0.1;
                                Alarm_check(enum_m74_type.supply_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.status);
                                Alarm_Out_check(enum_m74_type.supply_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.alarm_output);

                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.pv = Convert.ToInt32(rcv_data.Split(',')[5], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.sv = Convert.ToInt32(rcv_data.Split(',')[6], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.status = Convert.ToInt32(rcv_data.Split(',')[7], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.alarm_output = Convert.ToInt32(rcv_data.Split(',')[15], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.mv = Convert.ToInt16(rcv_data.Split(',')[19], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.offset = Convert.ToInt16(rcv_data.Split(',')[23], 16) * 0.1;
                                Alarm_check(enum_m74_type.supply_b, rcv_data_parse.address, 2, Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.status);
                                Alarm_Out_check(enum_m74_type.supply_b, rcv_data_parse.address, 2, Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.alarm_output);

                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation.pv = Convert.ToInt32(rcv_data.Split(',')[8], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation.sv = Convert.ToInt32(rcv_data.Split(',')[9], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation.status = Convert.ToInt32(rcv_data.Split(',')[10], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation.alarm_output = Convert.ToInt32(rcv_data.Split(',')[16], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation.mv = Convert.ToInt16(rcv_data.Split(',')[20], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation.offset = Convert.ToInt16(rcv_data.Split(',')[24], 16) * 0.1;
                                Alarm_check(enum_m74_type.circulation, rcv_data_parse.address, 3, Program.main_form.SerialData.TEMP_CONTROLLER.circulation.status);
                                Alarm_Out_check(enum_m74_type.circulation, rcv_data_parse.address, 3, Program.main_form.SerialData.TEMP_CONTROLLER.circulation.alarm_output);
                            }
                            else if (rcv_data_parse.address == 3)
                            {
                                Program.main_form.SerialData.TEMP_CONTROLLER.last_rcv_time_ch3 = DateTime.Now;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv = Convert.ToInt32(rcv_data.Split(',')[2], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv = Convert.ToInt32(rcv_data.Split(',')[3], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.status = Convert.ToInt32(rcv_data.Split(',')[4], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.alarm_output = Convert.ToInt32(rcv_data.Split(',')[14], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.mv = Convert.ToInt16(rcv_data.Split(',')[18], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.offset = Convert.ToInt16(rcv_data.Split(',')[22], 16) * 0.1;
                                Alarm_check(enum_m74_type.supply_heater_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.status);
                                Alarm_Out_check(enum_m74_type.supply_heater_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.alarm_output);

                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv = Convert.ToInt32(rcv_data.Split(',')[5], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.sv = Convert.ToInt32(rcv_data.Split(',')[6], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.status = Convert.ToInt32(rcv_data.Split(',')[7], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.alarm_output = Convert.ToInt32(rcv_data.Split(',')[15], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.mv = Convert.ToInt16(rcv_data.Split(',')[19], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.offset = Convert.ToInt16(rcv_data.Split(',')[23], 16) * 0.1;
                                Alarm_check(enum_m74_type.supply_heater_b, rcv_data_parse.address, 2, Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.status);
                                Alarm_Out_check(enum_m74_type.supply_heater_b, rcv_data_parse.address, 2, Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.alarm_output);

                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv = Convert.ToInt32(rcv_data.Split(',')[8], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = Convert.ToInt32(rcv_data.Split(',')[9], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.status = Convert.ToInt32(rcv_data.Split(',')[10], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.alarm_output = Convert.ToInt32(rcv_data.Split(',')[16], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.mv = Convert.ToInt16(rcv_data.Split(',')[20], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset = Convert.ToInt16(rcv_data.Split(',')[24], 16) * 0.1;
                                Alarm_check(enum_m74_type.circulation_heater1, rcv_data_parse.address, 3, Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.status);
                                Alarm_Out_check(enum_m74_type.circulation_heater1, rcv_data_parse.address, 3, Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.alarm_output);

                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.pv = Convert.ToInt32(rcv_data.Split(',')[11], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.sv = Convert.ToInt32(rcv_data.Split(',')[12], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.status = Convert.ToInt32(rcv_data.Split(',')[13], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.alarm_output = Convert.ToInt32(rcv_data.Split(',')[17], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.mv = Convert.ToInt16(rcv_data.Split(',')[21], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.offset = Convert.ToInt16(rcv_data.Split(',')[25].Substring(0, 4), 16) * 0.1;
                                Alarm_check(enum_m74_type.circulation_heater2, rcv_data_parse.address, 4, Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.status);
                                Alarm_Out_check(enum_m74_type.circulation_heater2, rcv_data_parse.address, 4, Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.alarm_output);

                            }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.dsp || Program.cg_app_info.eq_type == enum_eq_type.dhf || Program.cg_app_info.eq_type == enum_eq_type.lal || Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                        {
                            if (rcv_data_parse.address == 1)
                            {

                                if (Program.cg_app_info.he3320c_mode == enum_he3320c_mode.monitor_tank)
                                {
                                    Program.main_form.SerialData.TEMP_CONTROLLER.last_rcv_time_ch1 = DateTime.Now;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv = Convert.ToInt32(rcv_data.Split(',')[2], 16) * 0.1;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = Convert.ToInt32(rcv_data.Split(',')[3], 16) * 0.1;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.status = Convert.ToInt32(rcv_data.Split(',')[4], 16);
                                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.alarm_output = Convert.ToInt32(rcv_data.Split(',')[14], 16);
                                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.mv = Convert.ToInt16(rcv_data.Split(',')[18], 16) * 0.1;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset = Convert.ToInt16(rcv_data.Split(',')[22], 16) * 0.1;
                                    Alarm_check(enum_m74_type.tank_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.status);
                                    Alarm_Out_check(enum_m74_type.tank_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.alarm_output);
                                }
                                else
                                {
                                    Program.main_form.SerialData.TEMP_CONTROLLER.last_rcv_time_ch1 = DateTime.Now;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = Convert.ToInt32(rcv_data.Split(',')[2], 16) * 0.1;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.sv = Convert.ToInt32(rcv_data.Split(',')[3], 16) * 0.1;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.status = Convert.ToInt32(rcv_data.Split(',')[4], 16);
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.alarm_output = Convert.ToInt32(rcv_data.Split(',')[14], 16);
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.mv = Convert.ToInt16(rcv_data.Split(',')[18], 16) * 0.1;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.offset = Convert.ToInt16(rcv_data.Split(',')[22], 16) * 0.1;
                                    Alarm_check(enum_m74_type.tank_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.status);
                                    Alarm_Out_check(enum_m74_type.tank_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.alarm_output);

                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = Convert.ToInt32(rcv_data.Split(',')[5], 16) * 0.1;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.sv = Convert.ToInt32(rcv_data.Split(',')[6], 16) * 0.1;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.status = Convert.ToInt32(rcv_data.Split(',')[7], 16);
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.alarm_output = Convert.ToInt32(rcv_data.Split(',')[15], 16);
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.mv = Convert.ToInt16(rcv_data.Split(',')[19], 16) * 0.1;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.offset = Convert.ToInt16(rcv_data.Split(',')[23], 16) * 0.1;
                                    Alarm_check(enum_m74_type.tank_b, rcv_data_parse.address, 2, Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.status);
                                    Alarm_Out_check(enum_m74_type.tank_b, rcv_data_parse.address, 2, Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.alarm_output);

                                }
                            }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                        {
                            if (rcv_data_parse.address == 1)
                            {
                                Program.main_form.SerialData.TEMP_CONTROLLER.last_rcv_time_ch1 = DateTime.Now;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = Convert.ToInt32(rcv_data.Split(',')[2], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.sv = Convert.ToInt32(rcv_data.Split(',')[3], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.status = Convert.ToInt32(rcv_data.Split(',')[4], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.alarm_output = Convert.ToInt32(rcv_data.Split(',')[14], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.mv = Convert.ToInt16(rcv_data.Split(',')[18], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.offset = Convert.ToInt16(rcv_data.Split(',')[22], 16) * 0.1;
                                Alarm_check(enum_m74_type.tank_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.status);
                                Alarm_Out_check(enum_m74_type.tank_a, rcv_data_parse.address, 1, Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.alarm_output);

                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.pv = Convert.ToInt32(rcv_data.Split(',')[5], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.sv = Convert.ToInt32(rcv_data.Split(',')[6], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.status = Convert.ToInt32(rcv_data.Split(',')[7], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.alarm_output = Convert.ToInt32(rcv_data.Split(',')[15], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.mv = Convert.ToInt16(rcv_data.Split(',')[19], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.offset = Convert.ToInt16(rcv_data.Split(',')[23], 16) * 0.1;
                                Alarm_check(enum_m74_type.supply_a, rcv_data_parse.address, 2, Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.status);
                                Alarm_Out_check(enum_m74_type.supply_a, rcv_data_parse.address, 2, Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.alarm_output);

                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv = Convert.ToInt32(rcv_data.Split(',')[8], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv = Convert.ToInt32(rcv_data.Split(',')[9], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.status = Convert.ToInt32(rcv_data.Split(',')[10], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.alarm_output = Convert.ToInt32(rcv_data.Split(',')[16], 16);
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.mv = Convert.ToInt16(rcv_data.Split(',')[20], 16) * 0.1;
                                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.offset = Convert.ToInt16(rcv_data.Split(',')[24], 16) * 0.1;
                                Alarm_check(enum_m74_type.supply_heater_a, rcv_data_parse.address, 3, Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.status);
                                Alarm_Out_check(enum_m74_type.supply_heater_a, rcv_data_parse.address, 3, Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.alarm_output);

                            }
                        }
                        //parse = string.Format("RCV PARSE DATA : CH1 PV : {0}, SV : {1}, CH STATUS : {2}", Convert.ToInt32(rcv_data.Split(',')[2], 16), Convert.ToInt32(rcv_data.Split(',')[3], 16), Convert.ToInt32(rcv_data.Split(',')[4], 16));
                        //parse = string.Format("RCV PARSE DATA : CH2 PV : {0}, SV : {1}, CH STATUS : {2}", Convert.ToInt32(rcv_data.Split(',')[5], 16), Convert.ToInt32(rcv_data.Split(',')[6], 16), Convert.ToInt32(rcv_data.Split(',')[7], 16));
                        //parse = string.Format("RCV PARSE DATA : CH3 PV : {0}, SV : {1}, CH STATUS : {2}", Convert.ToInt32(rcv_data.Split(',')[8], 16), Convert.ToInt32(rcv_data.Split(',')[9], 16), Convert.ToInt32(rcv_data.Split(',')[10], 16));
                        //parse = string.Format("RCV PARSE DATA : CH4 PV : {0}, SV : {1}, CH STATUS : {2}", Convert.ToInt32(rcv_data.Split(',')[11], 16), Convert.ToInt32(rcv_data.Split(',')[12], 16), Convert.ToInt32(rcv_data.Split(',')[13].Substring(0, 4), 16));
                    }
                    else if (rcv_data.Split(',').Length == 26)
                    {
                        //CH1 ~ CH4 까지 4개의 Data 동시 Read
                        //parse = string.Format("RCV PARSE DATA : CH1 PV : {0}, SV : {1}, MV : {2}, CT : {3}, CH STATUS : {4}. ALARM STATUS : {5}", Convert.ToInt32(rcv_data.Split(',')[2], 16), Convert.ToInt32(rcv_data.Split(',')[3], 16), Convert.ToInt32(rcv_data.Split(',')[4], 16), Convert.ToInt32(rcv_data.Split(',')[5], 16), Convert.ToInt32(rcv_data.Split(',')[6], 16), Convert.ToInt32(rcv_data.Split(',')[7], 16));
                        //parse = string.Format("RCV PARSE DATA : CH2 PV : {0}, SV : {1}, MV : {2}, CT : {3}, CH STATUS : {4}. ALARM STATUS : {5}", Convert.ToInt32(rcv_data.Split(',')[8], 16), Convert.ToInt32(rcv_data.Split(',')[9], 16), Convert.ToInt32(rcv_data.Split(',')[10], 16), Convert.ToInt32(rcv_data.Split(',')[11], 16), Convert.ToInt32(rcv_data.Split(',')[12], 16), Convert.ToInt32(rcv_data.Split(',')[13], 16));
                        //parse = string.Format("RCV PARSE DATA : CH3 PV : {0}, SV : {1}, MV : {2}, CT : {3}, CH STATUS : {4}. ALARM STATUS : {5}", Convert.ToInt32(rcv_data.Split(',')[14], 16), Convert.ToInt32(rcv_data.Split(',')[15], 16), Convert.ToInt32(rcv_data.Split(',')[16], 16), Convert.ToInt32(rcv_data.Split(',')[17], 16), Convert.ToInt32(rcv_data.Split(',')[18], 16), Convert.ToInt32(rcv_data.Split(',')[19], 16));
                        //parse = string.Format("RCV PARSE DATA : CH4 PV : {0}, SV : {1}, MV : {2}, CT : {3}, CH STATUS : {4}. ALARM STATUS : {5}", Convert.ToInt32(rcv_data.Split(',')[20], 16), Convert.ToInt32(rcv_data.Split(',')[21], 16), Convert.ToInt32(rcv_data.Split(',')[22], 16), Convert.ToInt32(rcv_data.Split(',')[23], 16), Convert.ToInt32(rcv_data.Split(',')[24], 16), Convert.ToInt32(rcv_data.Split(',')[25].Substring(0, 4), 16));
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("M74" + "." + "Recieve_Data_To_Parse_M74" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;
        }
        public void Alarm_check(enum_m74_type type, int address, int ch, int status)
        {
            int idx = 0;
            string tml_binary_data = "";
            string binary_data;
            bool alarm_exist = false;
            string alarm_text = "";
            idx = 0;
            tml_binary_data = Convert.ToString(status, 2).PadLeft(16, '0');
            binary_data = new string(tml_binary_data.Reverse().ToArray());
            var M74_Unit = Program.main_form.SerialData.TEMP_CONTROLLER.tank_a;

            //System_Error = 15,
            //Sub_Firmware_Error = 14,
            //EEPROM_Error = 13,
            //RJC_Error = 12,
            //System_Comm_Error = 11,
            //AT_Error = 10,
            //Minus_Over_Input = 9,
            //Plus_Over_Input = 8,
            //Minus_Burn_Out = 7,
            //Plus_Burn_Out = 6,
            //RJ_Adjust_Error = 5,
            //Run = 4, //No Alarm
            //Manual_Control = 3,
            //Auto_Tunning = 2, //No Alarm
            //Output_of_Heating = 1, //No Alarm
            //Output_of_Cooling = 0, //No Alarm



            //Alert
            foreach (var temp in Enum.GetValues(typeof(enum_ch_status_map)))
            {
                if (idx != 0 && idx != 1 && idx != 2 && idx != 3 && idx != 4 && idx != 6 && idx != 7 && idx != 8 && idx != 9)
                {
                    if (binary_data.Substring(idx, 1) == "1")
                    {
                        alarm_text = alarm_text + temp.ToString() + ",";
                        alarm_exist = true;

                    }

                }
                idx = idx + 1;
            }
            if (alarm_exist == true)
            {

                if (address == 1 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_CH1_Alarm, alarm_text, false, true); }
                else if (address == 1 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_CH2_Alarm, alarm_text, false, true); }
                else if (address == 1 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_CH3_Alarm, alarm_text, false, true); }
                else if (address == 1 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_CH4_Alarm, alarm_text, false, true); }
                else if (address == 2 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_CH1_Alarm, alarm_text, false, true); }
                else if (address == 2 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_CH2_Alarm, alarm_text, false, true); }
                else if (address == 2 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_CH3_Alarm, alarm_text, false, true); }
                else if (address == 2 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_CH4_Alarm, alarm_text, false, true); }
                else if (address == 3 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_CH1_Alarm, alarm_text, false, true); }
                else if (address == 3 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_CH2_Alarm, alarm_text, false, true); }
                else if (address == 3 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_CH3_Alarm, alarm_text, false, true); }
                else if (address == 3 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_CH4_Alarm, alarm_text, false, true); }
            }

            idx = 0; alarm_exist = false;

            //Clear
            foreach (var temp in Enum.GetValues(typeof(enum_ch_status_map)))
            {
                if (idx != 0 && idx != 1 && idx != 2 && idx != 3 && idx != 4 && idx != 6 && idx != 7 && idx != 8 && idx != 9)
                {
                    if (binary_data.Substring(idx, 1) == "0")
                    {

                    }
                    else
                    {
                        alarm_exist = true;
                    }
                }
                idx = idx + 1;
            }

            if (alarm_exist == false)
            {
                if (address == 1 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_CH1_Alarm, "", false, false); }
                else if (address == 1 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_CH2_Alarm, "", false, false); }
                else if (address == 1 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_CH3_Alarm, "", false, false); }
                else if (address == 1 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_CH4_Alarm, "", false, false); }
                else if (address == 2 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_CH1_Alarm, "", false, false); }
                else if (address == 2 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_CH2_Alarm, "", false, false); }
                else if (address == 2 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_CH3_Alarm, "", false, false); }
                else if (address == 2 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_CH4_Alarm, "", false, false); }
                else if (address == 3 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_CH1_Alarm, "", false, false); }
                else if (address == 3 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_CH2_Alarm, "", false, false); }
                else if (address == 3 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_CH3_Alarm, "", false, false); }
                else if (address == 3 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_CH4_Alarm, "", false, false); }
            }

            if (alarm_exist == true)
            {
                M74_Unit.mode = enum_m74_status.error;
            }
            else
            {
                if (binary_data.Substring(2, 1) == "1") { M74_Unit.mode = enum_m74_status.at; }
                else if (binary_data.Substring(4, 1) == "1") { M74_Unit.mode = enum_m74_status.run; }
                else if (binary_data.Substring(4, 1) == "0") { M74_Unit.mode = enum_m74_status.stop; }
            }

            if (type == enum_m74_type.tank_a)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.mode = M74_Unit.mode;
            }
            else if (type == enum_m74_type.tank_b)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.mode = M74_Unit.mode;
            }
            else if (type == enum_m74_type.ts_09)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.mode = M74_Unit.mode;
            }
            else if (type == enum_m74_type.supply_a)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.mode = M74_Unit.mode;
            }
            else if (type == enum_m74_type.supply_b)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.mode = M74_Unit.mode;
            }
            else if (type == enum_m74_type.circulation)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.circulation.mode = M74_Unit.mode;
            }
            else if (type == enum_m74_type.supply_heater_a)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.mode = M74_Unit.mode;
            }
            else if (type == enum_m74_type.supply_heater_b)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.mode = M74_Unit.mode;
            }
            else if (type == enum_m74_type.circulation_heater1)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.mode = M74_Unit.mode;
            }
            else if (type == enum_m74_type.circulation_heater2)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.mode = M74_Unit.mode;
            }



        }
        public void Alarm_Out_check(enum_m74_type type, int address, int ch, int status)
        {
            int idx = 0;
            string tml_binary_data = "";
            string binary_data;
            bool alarm_exist = false;
            idx = 0;
            tml_binary_data = Convert.ToString(status, 2).PadLeft(16, '0');
            binary_data = new string(tml_binary_data.Reverse().ToArray());
            var M74_Unit = Program.main_form.SerialData.TEMP_CONTROLLER.tank_a;
            string alarm_text = "";
            //SPARE_15 = 15,
            //SPARE_14 = 14,
            //SPARE_13 = 13,
            //SPARE_12 = 12,
            //SPARE_11 = 11,
            //SPARE_10 = 10,
            //SPARE_9 = 9,
            //SPARE_8 = 8,
            //SPARE_7 = 7,
            //SPARE_6 = 6,
            //HOC = 5,
            //HBA = 4,
            //ALARM_4 = 3,
            //ALARM_3 = 2,
            //ALARM_2 = 1,
            //ALARM_1 = 0,


            //Alert
            alarm_text = "";
            foreach (var temp in Enum.GetValues(typeof(enum_alarm_output)))
            {
                if (idx != 6 && idx != 7 && idx != 8 && idx != 9 && idx != 10 && idx != 11 && idx != 12 && idx != 13 && idx != 14 && idx != 15)
                {
                    if (binary_data.Substring(idx, 1) == "1")
                    {
                        alarm_text = alarm_text + temp.ToString() + ",";
                        alarm_exist = true;
                        break;
                    }

                }
                idx = idx + 1;
            }

            if (alarm_exist == true)
            {
                M74_Unit.mode = enum_m74_status.error;
                if (address == 1 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_Alarm, alarm_text, false, true); }
                else if (address == 1 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_Alarm, alarm_text, false, true); }
                else if (address == 1 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_Alarm, alarm_text, false, true); }
                else if (address == 1 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_Alarm, alarm_text, false, true); }
                else if (address == 2 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_Alarm, alarm_text, false, true); }
                else if (address == 2 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_Alarm, alarm_text, false, true); }
                else if (address == 2 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_Alarm, alarm_text, false, true); }
                else if (address == 2 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_Alarm, alarm_text, false, true); }
                else if (address == 3 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_Alarm, alarm_text, false, true); }
                else if (address == 3 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_Alarm, alarm_text, false, true); }
                else if (address == 3 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_Alarm, alarm_text, false, true); }
                else if (address == 3 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_Alarm, alarm_text, false, true); }

                if (type == enum_m74_type.tank_a)
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.mode = M74_Unit.mode;
                }
                else if (type == enum_m74_type.tank_b)
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.mode = M74_Unit.mode;
                }
                else if (type == enum_m74_type.ts_09)
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.mode = M74_Unit.mode;
                }
                else if (type == enum_m74_type.supply_a)
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.mode = M74_Unit.mode;
                }
                else if (type == enum_m74_type.supply_b)
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.mode = M74_Unit.mode;
                }
                else if (type == enum_m74_type.circulation)
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation.mode = M74_Unit.mode;
                }
                else if (type == enum_m74_type.supply_heater_a)
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.mode = M74_Unit.mode;
                }
                else if (type == enum_m74_type.supply_heater_b)
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.mode = M74_Unit.mode;
                }
                else if (type == enum_m74_type.circulation_heater1)
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.mode = M74_Unit.mode;
                }
                else if (type == enum_m74_type.circulation_heater2)
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.mode = M74_Unit.mode;
                }

            }

            idx = 0; alarm_exist = false;
            //Clear
            foreach (var temp in Enum.GetValues(typeof(enum_alarm_output)))
            {
                if (idx != 6 && idx != 7 && idx != 8 && idx != 9 && idx != 10 && idx != 11 && idx != 12 && idx != 13 && idx != 14 && idx != 15)
                {
                    if (binary_data.Substring(idx, 1) == "0")
                    {

                    }
                    else
                    {
                        alarm_exist = true;
                    }
                }
                idx = idx + 1;
            }

            if (alarm_exist == false)
            {
                if (address == 1 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_Alarm, "", false, false); }
                else if (address == 1 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_Alarm, "", false, false); }
                else if (address == 1 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_Alarm, "", false, false); }
                else if (address == 1 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller1_Alarm, "", false, false); }
                else if (address == 2 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_Alarm, "", false, false); }
                else if (address == 2 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_Alarm, "", false, false); }
                else if (address == 2 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_Alarm, "", false, false); }
                else if (address == 2 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller2_Alarm, "", false, false); }
                else if (address == 3 && ch == 1) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_Alarm, "", false, false); }
                else if (address == 3 && ch == 2) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_Alarm, "", false, false); }
                else if (address == 3 && ch == 3) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_Alarm, "", false, false); }
                else if (address == 3 && ch == 4) { Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Temp_Controller3_Alarm, "", false, false); }
            }
            else
            {

            }


        }
    }
}
