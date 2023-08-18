using System;
using System.Linq;

namespace cds
{
    public class Class_ThermoStat_HE_3320C
    {
        public struct ST_ThermoStat_HE_3320C
        {
            public double pv_temp;
            public double offset;
            public bool run_state;
            public bool heater_on;
            public bool flag_run_req;
            public int Alarm_1;
            public int Alarm_2;
            public int Alarm_3;
            public int Alarm_4;
        }

        //serialPort2.Close();
        //        serialPort2.PortName = "COM5";
        //        serialPort2.BaudRate = 9600;
        //        serialPort2.Parity = System.IO.Ports.Parity.None;
        //        serialPort2.DataBits = 8;
        //        serialPort2.StopBits = System.IO.Ports.StopBits.One;
        //        serialPort2.Open();
        public enum he3320c_selected_tank
        {
            tank_a, tank_b
        }
        public enum enum_thermostat_type
        {
            Supply_A = 0,
            Supply_B = 1,
            Circulation = 2
        }
        public enum enum_control_mode
        {
            OFF = 0,
            Heater_On = 1,
            Tuning_On = 2,
            Error = 1000
        }

        public enum enum_Heavy_Alarm_Counter_1
        {
            SPARE = 0,
            SMPS2_DC_OFF_ALARM = 1,
            SMPS2_T_ALARM = 2,
            LEAK_ALARM = 3,
            TPR_ALARM = 4,
            SPARE_5 = 5,
            SPARE_6 = 6,
            SPARE_7 = 7,
            SPARE_8 = 8,
            SMPS3_DC_OFF_ALARM = 9,
            SMPS3_T_ALARM = 10,
            SPARE_11 = 11,
            SPARE_12 = 12,
            SPARE_13 = 13,
            SPARE_14 = 14,
            SPARE_15 = 15,
        }
        public enum enum_Heavy_Alarm_Counter_2
        {
            SPARE_1 = 0,
            SPARE_2 = 1,
            SPARE_3 = 2,
            SPARE_4 = 3,
            SPARE_5 = 4,
            CHEMICAL_IN_HEAT_BLOCK_TEMP_ALARM1 = 5,
            CHEMICAL_IN_HEAT_BLOCK_TEMP_ALARM2 = 6,
            CHEMICAL_IN_HEAT_BLOCK_TEMP_BURN_OUT_ALARM = 7,
            CHEMICAL_MIDLE_HEAT_BLOCK_TEMP_ALARM1 = 8,
            CHEMICAL_MIDLE_HEAT_BLOCK_TEMP_ALARM2 = 9,
            CHEMICAL_MIDDLE_HEAT_BLOCK_TEMP_BURN_OUT_ALARM = 10,
            CHEMICAL_OUT_HEAT_BLOCK_TEMP_ALARM1 = 11,
            CHEMICAL_OUT_HEAT_BLOCK_TEMP_ALARM2 = 12,
            CHEMICAL_OUT_HEAT_BLOCK_TEMP_BURN_OUT_ALARM = 13,
            PCW_OUT_TEMP_ALARM1 = 14,
            PCW_OUT_TEMP_ALARM2 = 15,

        }
        public enum enum_Heavy_Alarm_Counter_3
        {
            PCW_OUT_TEMP_BURN_OUT_ALARM = 0,
            CHEMICAL_IN_TEMP_ALARM1 = 1,
            CHEMICAL_IN_TEMP_ALARM2 = 2,
            CHEMICAL_IN_TEMP_BURN_OUT_ALARM = 3,
            CHEMICAL_MIDLE_TEMP_ALARM1 = 4,
            CHEMICAL_MIDLE_TEMP_ALARM2 = 5,
            CHEMICAL_MIDLE_TEMP_BURN_OUT_ALARM = 6,
            CHEMICAL_OUT_TEMP_ALARM1 = 7,
            CHEMICAL_OUT_TEMP_ALARM2 = 8,
            CHEMICAL_OUT_TEMP_BURN_OUT_ALARM = 9,
            SUPPLY_LINE_TEMP_ALARM1 = 10,
            SUPPLY_LINE_TEMP_ALARM2 = 11,
            SUPPLY_LINE_TEMP_BURN_OUT_ALARM = 12,
            SPARE_13 = 13,
            SPARE_14 = 14,
            SPARE_15 = 15,
        }
        public enum enum_Heavy_Alarm_Counter_4
        {
            SPARE_1 = 0,
            SPARE_2 = 1,
            SPARE_3 = 2,
            CHEMICAL_IN_HEAT_BLOCK_TEMP_HIGH_ALARM = 3,
            CHEMICAL_IN_HEAT_BLOCK_TEMP_LOW_ALARM = 4,
            CHEMICAL_MIDLE_HEAT_BLOCK_TEMP_HIGH_ALARM = 5,
            CHEMICAL_MIDLE_HEAT_BLOCK_TEMP_LOW_ALARM = 6,
            CHEMICAL_OUT_HEAT_BLOCK_TEMP_HIGH_ALARM = 7,
            CHEMICAL_OUT_HEAT_BLOCK_TEMP_LOW_ALARM = 8,
            PCW_OUT_TEMP_HIGH_ALARM = 9,
            PCW_OUT_TEMP_LOW_ALARM = 10,
            SPARE_11 = 11,
            SPARE_12 = 12,
            CHEMICAL_IN_TEMP_HIGH_ALARM = 13,
            CHEMICAL_IN_TEMP_LOW_ALARM = 14,
            CHEMICAL_MIDLE_TEMP_HIGH_ALARM = 15,
        }
        public enum enum_Heavy_Alarm_Counter_5
        {
            CHEMICAL_MIDLE_TEMP_LOW_ALARM = 0,
            CHEMICAL_OUT_TEMP_HIGH_ALARM = 1,
            CHEMICAL_OUT_TEMP_LOW_ALARM = 2,
            SUPPLY_LINE_TEMP_HIGH_ALARM = 3,
            SUPPLY_LINE_TEMP_LOW_ALARM = 4,
            SPARE_5 = 5,
            SPARE_6 = 6,
            SPARE_7 = 7,
            SPARE_8 = 8,
            SPARE_9 = 9,
            SPARE_10 = 10,
            SPARE_11 = 11,
            SPARE_12 = 12,
            SPARE_13 = 13,
            SPARE_14 = 14,
            SPARE_15 = 15,
        }
        public enum enum_Warning_Alarm_Counter_1
        {
            High_Warning_Temp_Supply_Control = 0,
            Low_Warning_Temp_Supply_Control = 1,
            SPARE_2 = 2,
            SPARE_3 = 3,
            SPARE_4 = 4,
            SPARE_5 = 5,
            SPARE_6 = 6,
            SPARE_7 = 7,
            SPARE_8 = 8,
            SPARE_9 = 9,
            SPARE_10 = 10,
            SPARE_11 = 11,
            SPARE_12 = 12,
            SPARE_13 = 13,
            SPARE_14 = 14,
            SPARE_15 = 15,
        }
        public static byte[] command_read_500_511 =
        {
            //Supply Control PV(500) ~ Warning Alarm Counter#1까지 Read(511)
            //500 PV-Supply 0.01 0~999
            //505 제어모드 0:OFF, 1:Heater On, 2:Tuning On, 1000:Error
            //506 Conter#1 알람 Heavy Level 정보 코드
            //507 Conter#2 알람 Heavy Level 정보 코드
            //507 Conter#3 알람 Heavy Level 정보 코드
            //507 Conter#4 알람 Heavy Level 정보 코드
            //507 Conter#5 알람 Heavy Level 정보 코드
                0x01,        //ID
                0x04,        //Input Register 04
                0x01, 0xF4,  //시작 번지 500
                0x00, 0x0C,  //데이터 개수
                0x00, 0x00   //CheckSum 다시 계산함
         };
        //public static byte[] command_read_500 =
        public static byte[] read_supply_pv =
{
            //Supply Control PV(500) ~ Warning Alarm Counter#1까지 Read(511)
            //500 PV-Supply 0.01 0~999
            //505 제어모드 0:OFF, 1:Heater On, 2:Tuning On, 1000:Error
            //506 Conter#1 알람 Heavy Level 정보 코드
            //507 Conter#2 알람 Heavy Level 정보 코드
            //507 Conter#3 알람 Heavy Level 정보 코드
            //507 Conter#4 알람 Heavy Level 정보 코드
            //507 Conter#5 알람 Heavy Level 정보 코드
                0x01,        //ID
                0x04,        //Input Register 04
                0x01, 0xF4,  //시작 번지 500
                0x00, 0x0F,  //데이터 개수
                0x71, 0xC4   //CheckSum 다시 계산함
         };
        public static byte[] command_write_Coil_0 =
{
            //0:Temp Off 1:Temp On
                0x01,        //ID
                0x01,        //Input Register 05
                0x00, 0x13,  //시작 번지 0066 : 하드웨어 버전, 0067 : 소프트웨어 버전
                0x00, 0x25,  //데이터 개수
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] command_write_1 =
        {
                //Set SV Temp
                0x01,        //ID
                0x06,        //Input Register 06
                0x00, 0x01,  //시작 번지 1
                0x00, 0x25,  //데이터 값
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] command_write_16 =
        {
                //Set oFFSET
                0x01,        //ID
                0x06,        //Input Register 06
                0x00, 0x10,  //시작 번지 16
                0x00, 0x25,  //데이터 값
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] command_write_23 =
{
                //Set Tank Select A
                0x01,        //ID
                0x06,        //Input Register 06
                0x00, 0x17,  //시작 번지 23
                0x00, 0x25,  //데이터 값
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] command_write_24 =
        {
                //Set Tank Select B
                0x01,        //ID
                0x06,        //Input Register 06
                0x00, 0x18,  //시작 번지 24
                0x00, 0x25,  //데이터 값
                0x00, 0x00   //CheckSum 다시 계산함
         };
        //public static byte[] command_write_coil_00_ON =
        public static byte[] Heater_ON =
        {
                //Set SV Temp
                0x01,        //ID
                0x05,        //Input Register 05
                0x00, 0x00,  //시작 번지 0
                0xFF, 0x00,  //데이터 값  //HIGH : FF 00, LOW : 00 00
                0x00, 0x00   //CheckSum 다시 계산함
         };
        //public static byte[] command_write_coil_00_OFF =
        public static byte[] Heater_OFF =
        {
                //Set SV Temp
                0x01,        //ID
                0x05,        //Input Register 05
                0x00, 0x00,  //시작 번지 0
                0x00, 0x00,  //데이터 값  //HIGH : FF 00, LOW : 00 00
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] alarm_reset =
        //public static byte[] command_write_coil_05_ON =
        {
                //Set SV Temp
                0x01,        //ID
                0x05,        //Input Register 05
                0x00, 0x05,  //시작 번지 0
                0xFF, 0x00,  //데이터 값  //HIGH : FF 00, LOW : 00 00
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public static byte[] data_apply =
        //public static byte[] command_write_coil_06_ON =
        {
                //Set SV Temp
                0x01,        //ID
                0x05,        //Input Register 05
                0x00, 0x06,  //시작 번지 0
                0xFF, 0x00,  //데이터 값  //HIGH : FF 00, LOW : 00 00
                0x00, 0x00   //CheckSum 다시 계산함
         };
        public byte[] Set_SV(int value, int idx_serial)
        {
            string value_parse = value.ToString("X4");
            command_write_1[4] = Convert.ToByte(value_parse.Substring(0, 2), 16);
            command_write_1[5] = Convert.ToByte(value_parse.Substring(2, 2), 16);
            return Message_Command_Apply_CRC_TO_Send(command_write_1, idx_serial); ;
        }
        public byte[] Set_Offset(Int16 value, int idx_serial)
        {
            string value_parse = value.ToString("X4");
            command_write_16[4] = Convert.ToByte(value_parse.Substring(0, 2), 16);
            command_write_16[5] = Convert.ToByte(value_parse.Substring(2, 2), 16);
            return Message_Command_Apply_CRC_TO_Send(command_write_16, idx_serial); ;
        }

        public byte[] Select_Tank(he3320c_selected_tank selected_tank, bool active, int idx_serial)
        {
            if (selected_tank == he3320c_selected_tank.tank_a)
            {
                command_write_23[4] = 0;
                if (active == true)
                {
                    command_write_23[5] = 1;
                }
                else
                {
                    command_write_23[5] = 0;
                }
                return Message_Command_Apply_CRC_TO_Send(command_write_23, idx_serial);
            }
            else if (selected_tank == he3320c_selected_tank.tank_b)
            {
                command_write_24[4] = 0;
                if (active == true)
                {
                    command_write_24[5] = 1;
                }
                else
                {
                    command_write_24[5] = 0;
                }
                return Message_Command_Apply_CRC_TO_Send(command_write_24, idx_serial);
            }
            else
            {
                return null;
            }

        }
        public byte[] Heater_ON_OFF(bool status, int idx_serial)
        {
            byte[] return_value;
            if (status == true)
            {
                return_value = Message_Command_Apply_CRC_TO_Send(Heater_ON, idx_serial); ;
            }
            else
            {
                return_value = Message_Command_Apply_CRC_TO_Send(Heater_OFF, idx_serial); ;
            }
            return return_value;
        }
        public string Message_Command_Apply_CRC_TO_Send(byte[] command, ref byte[] send_msg, int idx_serial)
        {
            string result = "";
            byte[] crc = new byte[2];
            try
            {
                //Checksum 계산
                crc = CRC16(command, command.Length - 2, true);
                command[command.Length - 2] = crc[0];
                command[command.Length - 1] = crc[1];
                Array.Resize(ref send_msg, command.Length); Array.Copy(command, send_msg, command.Length);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            if (idx_serial != -1) { Program.main_form.Serial_data_Enqueue(ref send_msg, idx_serial); }
            return result;
        }
        public byte[] Message_Command_Apply_CRC_TO_Send(byte[] command, int idx_serial)
        {
            byte[] send_msg = null;
            byte[] crc = new byte[2];
            try
            {
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
        public string Recieve_Data_To_Parse_HE_3320C(int idx_serial, byte[] rcv_data, ref string parse, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
        {
            //Class_Serial_Common.Rcv_Data rcv_data_parse;
            int data_idx = 0;
            string result = "";
            bool status_ok = false;
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
                rcv_data_parse.rcv_data_to_string = "";
                rcv_data_parse.crc = "";
                rcv_data_parse.read_data_byte_cnt = 0;
                rcv_data_parse.reading_data = null;

                if (rcv_data == null) { result = "Empty Array"; return result; }
                else
                {
                    for (int idx = 0; idx < rcv_data.Length; idx++)
                    {

                        if (idx != 0) { }
                        if (idx == 0)
                        {
                            rcv_data_parse.address = rcv_data[0];
                            //Console.Write(rcv_data[idx].ToString("X2"));
                            if (rcv_data_parse.address >= 100)
                            {
                                //Error Code Return
                                result = "Command Rcv Error"; return result;
                            }
                        }
                        if (idx == 1)
                        {
                            rcv_data_parse.function_code = rcv_data[1]; //Console.Write(rcv_data[idx].ToString("X2"));
                        }
                        if (idx == 2) { rcv_data_parse.read_data_byte_cnt = rcv_data[2]; rcv_data_parse.reading_data = new string[rcv_data_parse.read_data_byte_cnt / 2]; }
                        if (idx > 2 && idx < rcv_data.Length - 2)
                        {
                            if (rcv_data_parse.function_code == 0)
                            {

                            }
                            else if (rcv_data_parse.function_code == 5)
                            {
                                //보낸 문자와 동일하게 리턴함
                            }
                            else if (rcv_data_parse.function_code == 6)
                            {
                                //보낸 문자와 동일하게 리턴함
                            }
                            else if (rcv_data_parse.function_code != 6)
                            {

                                rcv_data_parse.reading_data[data_idx] = rcv_data[idx].ToString("X2") + rcv_data[idx + 1].ToString("X2");
                                //rcv_data_parse.reading_data[data_idx] = rcv_data[idx].ToString("X2");
                                //Console.Write(rcv_data_parse.reading_data[data_idx]);
                                rcv_data_parse.rcv_data_to_string = rcv_data_parse.rcv_data_to_string + "," + rcv_data_parse.reading_data[data_idx];
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
                    parse = rcv_data_parse.rcv_data_to_string;

                    if (rcv_data_parse.function_code == 4)
                    { //Coil, Holding, Input Register Rcv
                      //Address 500~ 14개 읽을 때 응답은 아래와 같다.
                      //2023-06-27 항온조 Tank 온도 전환 기능 추가 적용 후 메시지
                      //01 04 1E 00 00 00 00 00 00 09 0A 00 00 03 E8 00 40 00 00 00 00 00 00 00 00 00 00 00 01 09 4A 08 DC A7 1D
                      //         0     1     2     3     4     5     6     7     8     9     10    11    12    13    14    15
                      //rcv_data_parse.reading_data의 상위 Index번호로 Mapping / 01 04 1E 3자리는 제외
                      //Ex) 5번째 03 E8은 -> 1000으로 Control Mode가 1000 -> Error의미

                      //3831 -> PV값 *0.01 14385 -> 143.85
                      //6 Control Mode 0 : OFF, 1:Heater On, 2:Tuning On, 1000:Error
                      //7번째 03E8 -> 1000 = Error
                      //14개 데이타요청 시 아래와 같이 Mapping됨
                      //,FD53,0000,0000,0A17,0000,03E8,0000,0000,0800,0000,0000,0000,0001,0000,0000
                      // 1    2    3    4    5    6    7    8    9    10   11   12   13   14   15
                      // 1 : FD53 -> -685 -> -6.85 (PV)
                      // 9 : 0800 ->  0000-1000-0000-0000 -> SUPPLY LINE TEMP ALARM2

                        //HE3320C Data -> M74 Data Mapping
                        for (int idx_status_check = 0; idx_status_check < rcv_data_parse.reading_data.Length; idx_status_check++)
                        {
                            //항온조 전원 On 후 약 10초간 허수(전체 데이터 0000)으로 Returun 되며, 진수로 리턴될 시(=이니셜 종료) Heater On 명령을 위함
                            if (rcv_data_parse.reading_data[idx_status_check] != "0000")
                            {
                                status_ok = true;
                            }
                        }

                        if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                        {
                            if (idx_serial == (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Circulation_Thermostat.run_state = true;
                                    ///Circulation 항온조 Taget Tank A, B볼 때 값 치환
                                    if (Program.cg_app_info.he3320c_mode == enum_he3320c_mode.monitor_tank)
                                    {
                                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = Convert.ToInt16(rcv_data_parse.reading_data[13], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = Convert.ToInt16(rcv_data_parse.reading_data[14], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                        Alarm_check(enum_thermostat_type.Circulation, ref rcv_data_parse);
                                    }
                                    else
                                    {
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                        Alarm_check(enum_thermostat_type.Circulation, ref rcv_data_parse);
                                    }
                                }
                                else
                                {
                                    Program.main_form.SerialData.Circulation_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Circulation_Thermostat.run_state = false;
                                }
                            }
                            else if (idx_serial == (int)Config_IO.enum_dsp_serial_index.SUPPLY_A_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Supply_A_Thermostat.run_state = true;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                    Alarm_check(enum_thermostat_type.Supply_A, ref rcv_data_parse);
                                }
                                else
                                {
                                    Program.main_form.SerialData.Supply_A_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Supply_A_Thermostat.run_state = false;
                                }
                            }
                            else if (idx_serial == (int)Config_IO.enum_dsp_serial_index.SUPPLY_B_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Supply_B_Thermostat.run_state = true;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                    Alarm_check(enum_thermostat_type.Supply_B, ref rcv_data_parse);
                                }
                                else
                                {
                                    Program.main_form.SerialData.Supply_B_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Supply_B_Thermostat.run_state = false;
                                }
                            }

                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                        {
                            if (idx_serial == (int)Config_IO.enum_dhf_serial_index.CIRCULATION_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Circulation_Thermostat.run_state = true;
                                    ///Circulation 항온조 Taget Tank A, B볼 때 값 치환
                                    if (Program.cg_app_info.he3320c_mode == enum_he3320c_mode.monitor_tank)
                                    {
                                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = Convert.ToInt16(rcv_data_parse.reading_data[13], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = Convert.ToInt16(rcv_data_parse.reading_data[14], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                        Alarm_check(enum_thermostat_type.Circulation, ref rcv_data_parse);
                                    }
                                    else
                                    {
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                        Alarm_check(enum_thermostat_type.Circulation, ref rcv_data_parse);
                                    }
                                }
                                else
                                {
                                    Program.main_form.SerialData.Circulation_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Circulation_Thermostat.run_state = false;
                                }
                            }
                            else if (idx_serial == (int)Config_IO.enum_dhf_serial_index.SUPPLY_A_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Supply_A_Thermostat.run_state = true;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                    Alarm_check(enum_thermostat_type.Supply_A, ref rcv_data_parse);
                                }
                                else
                                {
                                    Program.main_form.SerialData.Supply_A_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Supply_A_Thermostat.run_state = false;
                                }
                            }
                            else if (idx_serial == (int)Config_IO.enum_dhf_serial_index.SUPPLY_B_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Supply_B_Thermostat.run_state = true;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                    Alarm_check(enum_thermostat_type.Supply_B, ref rcv_data_parse);
                                }
                                else
                                {
                                    Program.main_form.SerialData.Supply_B_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Supply_B_Thermostat.run_state = false;
                                }
                            }

                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                        {
                            if (idx_serial == (int)Config_IO.enum_lal_serial_index.CIRCULATION_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Circulation_Thermostat.run_state = true;
                                    ///Circulation 항온조 Taget Tank A, B볼 때 값 치환
                                    if (Program.cg_app_info.he3320c_mode == enum_he3320c_mode.monitor_tank)
                                    {
                                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = Convert.ToInt16(rcv_data_parse.reading_data[13], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = Convert.ToInt16(rcv_data_parse.reading_data[14], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                        Alarm_check(enum_thermostat_type.Circulation, ref rcv_data_parse);
                                    }
                                    else
                                    {
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                        Alarm_check(enum_thermostat_type.Circulation, ref rcv_data_parse);
                                    }
                                }
                                else
                                {
                                    Program.main_form.SerialData.Circulation_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Circulation_Thermostat.run_state = false;
                                }
                            }
                            else if (idx_serial == (int)Config_IO.enum_lal_serial_index.SUPPLY_A_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Supply_A_Thermostat.run_state = true;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                    Alarm_check(enum_thermostat_type.Supply_A, ref rcv_data_parse);
                                }
                                else
                                {
                                    Program.main_form.SerialData.Supply_A_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Supply_A_Thermostat.run_state = false;
                                }
                            }
                            else if (idx_serial == (int)Config_IO.enum_lal_serial_index.SUPPLY_B_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Supply_B_Thermostat.run_state = true;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                    Alarm_check(enum_thermostat_type.Supply_B, ref rcv_data_parse);
                                }
                                else
                                {
                                    Program.main_form.SerialData.Supply_B_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Supply_B_Thermostat.run_state = false;
                                }
                            }

                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                        {
                            if (idx_serial == (int)Config_IO.enum_dsp_mix_serial_index.CIRCULATION_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Circulation_Thermostat.run_state = true;
                                    ///Circulation 항온조 Taget Tank A, B볼 때 값 치환
                                    if (Program.cg_app_info.he3320c_mode == enum_he3320c_mode.monitor_tank)
                                    {
                                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = Convert.ToInt16(rcv_data_parse.reading_data[13], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = Convert.ToInt16(rcv_data_parse.reading_data[14], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                        Alarm_check(enum_thermostat_type.Circulation, ref rcv_data_parse);
                                    }
                                    else
                                    {
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                        Alarm_check(enum_thermostat_type.Circulation, ref rcv_data_parse);
                                    }
                                }
                                else
                                {
                                    Program.main_form.SerialData.Circulation_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Circulation_Thermostat.run_state = false;
                                }
                            }
                            else if (idx_serial == (int)Config_IO.enum_dsp_mix_serial_index.SUPPLY_A_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Supply_A_Thermostat.run_state = true;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                    Alarm_check(enum_thermostat_type.Supply_A, ref rcv_data_parse);
                                }
                                else
                                {
                                    Program.main_form.SerialData.Supply_A_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Supply_A_Thermostat.run_state = false;
                                }
                            }
                            else if (idx_serial == (int)Config_IO.enum_dsp_mix_serial_index.SUPPLY_B_THERMOSTAT)
                            {
                                if (status_ok == true)
                                {
                                    Program.main_form.SerialData.Supply_B_Thermostat.run_state = true;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv = Convert.ToInt16(rcv_data_parse.reading_data[0], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.sv = Convert.ToInt16(rcv_data_parse.reading_data[1], 16) * 0.01;
                                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.offset = Convert.ToInt16(rcv_data_parse.reading_data[2], 16) * 0.01;
                                    Alarm_check(enum_thermostat_type.Supply_B, ref rcv_data_parse);
                                }
                                else
                                {
                                    Program.main_form.SerialData.Supply_B_Thermostat.heater_on = false;
                                    Program.main_form.SerialData.Supply_B_Thermostat.run_state = false;
                                }
                            }

                        }


                    }

                }

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("HE3320" + "." + "Recieve_Data_To_Parse_HE_3320C" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;
        }

        public void Alarm_check(enum_thermostat_type type, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
        {
            int result = 0;
            string tml_binary_data = "";
            string log = "";
            string binary_data;
            int idx_parse = 0;
            int idx = 0;
            bool alarm_alive = false;
            string remark = "";
            //,FD53,0000,0000,0A17,0000,03E8,0000,0000,0800,0000,0000,0000,0001,0000,0000
            // 1    2    3    4    5    6    7    8    9    10   11   12   13   14
            // 1 : PV
            // 6 : Control Mode

            idx_parse = 5; idx = 0;
            if (Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16) == (int)enum_control_mode.OFF)
            {
                alarm_alive = false;
                if (type == enum_thermostat_type.Supply_A)
                {
                    Program.main_form.SerialData.Supply_A_Thermostat.heater_on = false;
                }
                else if (type == enum_thermostat_type.Supply_B)
                {
                    Program.main_form.SerialData.Supply_B_Thermostat.heater_on = false;
                }
                else if (type == enum_thermostat_type.Circulation)
                {
                    Program.main_form.SerialData.Circulation_Thermostat.heater_on = false;
                }
            }
            else if (Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16) == (int)enum_control_mode.Heater_On)
            {
                alarm_alive = false;
                if (type == enum_thermostat_type.Supply_A)
                {
                    Program.main_form.SerialData.Supply_A_Thermostat.heater_on = true;
                }
                else if (type == enum_thermostat_type.Supply_B)
                {
                    Program.main_form.SerialData.Supply_B_Thermostat.heater_on = true;
                }
                else if (type == enum_thermostat_type.Circulation)
                {
                    Program.main_form.SerialData.Circulation_Thermostat.heater_on = true;
                }
            }
            else if (Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16) == (int)enum_control_mode.Tuning_On)
            {
                alarm_alive = false;
            }
            else if (Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16) == (int)enum_control_mode.Error)
            {
                alarm_alive = true;
                if (type == enum_thermostat_type.Supply_A)
                {
                    Program.main_form.SerialData.Supply_A_Thermostat.heater_on = false;
                }
                else if (type == enum_thermostat_type.Supply_B)
                {
                    Program.main_form.SerialData.Supply_B_Thermostat.heater_on = false;
                }
                else if (type == enum_thermostat_type.Circulation)
                {
                    Program.main_form.SerialData.Circulation_Thermostat.heater_on = false;
                }
            }

            if (type == enum_thermostat_type.Supply_A)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Error, "", false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Supply_B)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Error, "", false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Circulation)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Error, "", false, alarm_alive);
            }

            // 7 : Heavy Alarm Counter#1
            idx_parse = 6; idx = 0; alarm_alive = false; remark = "";
            tml_binary_data = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16), 2).PadLeft(16, '0');
            binary_data = new string(tml_binary_data.Reverse().ToArray());
            foreach (var temp in Enum.GetValues(typeof(enum_Heavy_Alarm_Counter_1)))
            {
                if (binary_data.Substring(idx, 1) == "1")
                {
                    alarm_alive = true; remark = temp.ToString();
                    break;
                }
                idx = idx + 1;
            }

            if (type == enum_thermostat_type.Supply_A)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Encount1, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Supply_B)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Encount1, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Circulation)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Encount1, remark, false, alarm_alive);
            }



            // 8 : Heavy Alarm Counter#2
            idx_parse = 7; idx = 0; alarm_alive = false; remark = "";
            tml_binary_data = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16), 2).PadLeft(16, '0');
            binary_data = new string(tml_binary_data.Reverse().ToArray());
            foreach (var temp in Enum.GetValues(typeof(enum_Heavy_Alarm_Counter_2)))
            {
                if (binary_data.Substring(idx, 1) == "1")
                {
                    alarm_alive = true; remark = temp.ToString();
                    break;
                }
                idx = idx + 1;
            }
            if (type == enum_thermostat_type.Supply_A)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Encount2, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Supply_B)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Encount2, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Circulation)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Encount2, remark, false, alarm_alive);
            }



            // 9 : Heavy Alarm Counter#3
            idx_parse = 8; idx = 0; alarm_alive = false; remark = "";
            tml_binary_data = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16), 2).PadLeft(16, '0');
            binary_data = new string(tml_binary_data.Reverse().ToArray());
            foreach (var temp in Enum.GetValues(typeof(enum_Heavy_Alarm_Counter_3)))
            {
                if (binary_data.Substring(idx, 1) == "1")
                {
                    alarm_alive = true; remark = temp.ToString();
                    break;
                }
                idx = idx + 1;
            }
            if (type == enum_thermostat_type.Supply_A)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Encount3, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Supply_B)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Encount3, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Circulation)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Encount3, remark, false, alarm_alive);
            }

            // 10 : Heavy Alarm Counter#4
            idx_parse = 9; idx = 0; alarm_alive = false; remark = "";
            tml_binary_data = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16), 2).PadLeft(16, '0');
            binary_data = new string(tml_binary_data.Reverse().ToArray());
            foreach (var temp in Enum.GetValues(typeof(enum_Heavy_Alarm_Counter_4)))
            {
                if (binary_data.Substring(idx, 1) == "1")
                {
                    alarm_alive = true; remark = temp.ToString();
                    break;
                }
                idx = idx + 1;
            }
            if (type == enum_thermostat_type.Supply_A)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Encount4, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Supply_B)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Encount4, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Circulation)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Encount4, remark, false, alarm_alive);
            }

            // 11 : Heavy Alarm Counter#5
            idx_parse = 10; idx = 0; alarm_alive = false; remark = "";
            tml_binary_data = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16), 2).PadLeft(16, '0');
            binary_data = new string(tml_binary_data.Reverse().ToArray());
            foreach (var temp in Enum.GetValues(typeof(enum_Heavy_Alarm_Counter_5)))
            {
                if (binary_data.Substring(idx, 1) == "1")
                {
                    alarm_alive = true; remark = temp.ToString();
                    break;
                }
                idx = idx + 1;
            }
            if (type == enum_thermostat_type.Supply_A)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Encount5, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Supply_B)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Encount5, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Circulation)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Encount5, remark, false, alarm_alive);
            }


            // 12 : Warning Alarm Counter#1
            idx_parse = 11; idx = 0; alarm_alive = false; remark = "";
            tml_binary_data = Convert.ToString(Convert.ToInt32(rcv_data_parse.reading_data[idx_parse], 16), 2).PadLeft(16, '0');
            binary_data = new string(tml_binary_data.Reverse().ToArray());
            foreach (var temp in Enum.GetValues(typeof(enum_Warning_Alarm_Counter_1)))
            {
                if (binary_data.Substring(idx, 1) == "1")
                {
                    alarm_alive = true; remark = temp.ToString();
                    break;
                }
                idx = idx + 1;
            }
            if (type == enum_thermostat_type.Supply_A)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Warning1, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Supply_B)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Warning1, remark, false, alarm_alive);
            }
            else if (type == enum_thermostat_type.Circulation)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Warning1, remark, false, alarm_alive);
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
