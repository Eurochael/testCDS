using DevExpress.XtraSpreadsheet.DocumentFormats.Xlsb;
using Microsoft.VisualBasic.Logging;
using System;
using System.Text;

namespace cds
{
    public class Class_Concentration_CS150C
    {
        //PC -> CS150C : R, DT <CR><LF>
        //CS150C -> PC(Sucess) : DT,1,2,3,4,5,6,7,8,9,10<CR><LF>
        //1 : Data Number
        //2 : Chemical Solution
        //3 : State of B.G
        //4 : No Meaning
        //5 : Concentration of First
        //6 : Concentration of Second
        //7 : Concentration of Third
        //8 : Monitor Error
        //9 : Data Error
        //10 : Warning

        //CS150C -> PC(Fail) : ER,1<CR><LF>
        //1 Error Code
        //#1 : Header error
        //#2 : Wrong operation error
        //#3 : Numeric value error
        //#4 : Monitor error


        //<CR><LF> 고정
        public struct ST_Concentration_CS150C
        {
            public string Alarm;
            public float h2o2_concentration;
            public float h2o2_concentration_old;
            public string h2o2_Alarm;

            public float h2so4_concentration;
            public float h2so4_concentration_old;
            public string h2so4_Alarm;
        }
        public enum enum_measure_status
        {
            stop = 0,
            during_measurement_of_chemical_soulution_type = 1,
            during_background_correction_and_judgment = 2,
            during_background_correction_witch_is_successful_without_e07x = 3,
            during_background_correction_witch_is_successful_with_e07x = 4,
            error = 10
        }
        public static string command_Read_Data = "R,DT";
        public byte[] Measurement_Data_Read(int idx_serial)
        {
            return Message_Command_To_Byte_CS150C_TO_Send(command_Read_Data, idx_serial); ;
        }
        public byte[] Message_Command_To_Byte_CS150C_TO_Send(string command, int idx_serial)
        {
            byte[] send_msg = new byte[0];
            int checksum = 0;
            string checksum_parse;
            string msg = "";
            try
            {
                msg = command + Convert.ToChar(0x0D) + Convert.ToChar(0x0A);
                send_msg = Encoding.UTF8.GetBytes(msg);
            }
            catch (Exception ex)
            {
                send_msg = null;
            }
            if (idx_serial != -1) { Program.main_form.Serial_data_Enqueue(ref send_msg, idx_serial); }
            return send_msg;
        }
        public string Recieve_Data_To_Parse_CS150C(string rcv_data, ref string parse, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
        {
            string result = "";
            try
            {
                rcv_data = rcv_data.Replace(Convert.ToChar(0x0D), ' ');
                rcv_data = rcv_data.Replace(Convert.ToChar(0x0A), ' ');
                result = rcv_data;
                rcv_data_parse.dt_rcvtime = DateTime.Now;
                rcv_data_parse.reading_data = null;
                rcv_data_parse.function_code = 0;
                rcv_data_parse.rcv_data_to_string = "";
                rcv_data_parse.crc = "";
                rcv_data_parse.read_data_byte_cnt = 0;
                rcv_data_parse.reading_data = null;
                Program.main_form.SerialData.CS600F.log = rcv_data;
                //Program.log_md.LogWrite("rcv_data", Module_Log.enumLog.SERAL_DATA_10, "", Module_Log.enumLevel.ALWAYS);
                //DD, 002, 1, 0, 025.79,-00.01,001.02,0099.0,000000,000000,000000,06,001
                //DD
                //1 : Data No
                //2 : Status
                //3 : solution Typ
                //4 : Temp
                //5 : Chem 1 NH4PF
                //6 : Chem 2 HF
                //7 : Chem 3 H2O
                //8 : Chem 4
                //9 : Chem 5
                //10 : Chem 6
                //11 : Error
                //12 : Back correction Fail Count
                if (rcv_data.Split(',').Length > 4)
                {
                    Program.main_form.SerialData.CS150C.h2o2_concentration = Convert.ToSingle(rcv_data.Split(',')[5]);
                    Program.main_form.SerialData.CS150C.h2so4_concentration = Convert.ToSingle(rcv_data.Split(',')[6]);
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Concentration_CS_150C_Alarm, "", false, false);
                }
                else if(rcv_data.IndexOf("ER") >= 0) 
                {
                    Program.main_form.SerialData.CS150C.Alarm = rcv_data;
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Concentration_CS_150C_Alarm, Program.main_form.SerialData.CS150C.Alarm, false, true);
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("CS150C" + "." + "Recieve_Data_To_Parse_CS150C" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;
        }
        public void Measure_Status(string data)
        {
            int measure_status = 0;

            try
            {
                if (int.TryParse(data, out measure_status) == true)
                {
                    //if (measure_status == 0) { Program.main_form.SerialData.CS600F.measure_status = enum_measure_status.stop; }
                    //else if (measure_status == 1) { Program.main_form.SerialData.CS600F.measure_status = enum_measure_status.during_measurement_of_chemical_soulution_type; }
                    //else if (measure_status == 2) { Program.main_form.SerialData.CS600F.measure_status = enum_measure_status.during_background_correction_and_judgment; }
                    //else if (measure_status == 3) { Program.main_form.SerialData.CS600F.measure_status = enum_measure_status.during_background_correction_witch_is_successful_without_e07x; }
                    //else if (measure_status == 4) { Program.main_form.SerialData.CS600F.measure_status = enum_measure_status.during_background_correction_witch_is_successful_with_e07x; }
                }
                else
                {
                    //Program.main_form.SerialData.CS600F.measure_status = enum_measure_status.error;
                }


            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite("CS600F" + "." + "Measure_Status" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }
    }
}
