using System;
using System.Text;

namespace cds
{
    public class Class_Concentration_HF700
    {
        //PC -> HF700 : R, DT <CR><LF>
        //HF700 -> PC(Sucess) : DT,1,2,3,4,5,6,7,8,9,10<CR><LF>
        //1 : Data Number
        //2 : Full Scale for measurements
        //3 : Monitor Status
        //4 : 
        //5 : HFMeasured value
        //6 : PH
        //7 : Temp
        //8 : Monitor Error
        //9 : Reagent Error
        //10 : Warning

        //CS150C -> PC(Fail) : ER,1<CR><LF>
        //1 Error Code
        //#1 : Header error
        //#2 : Wrong operation error
        //#3 : Numeric value error
        //#4 : Monitor error
        public struct ST_Concentration_HF700
        {
            public float concentration;
            public float concentration_old;
            public double Temp;
            public string Alarm;
        }
        public static string command_Read_Data = "R,DT";

        public byte[] Message_Command_To_Byte_HF700_TO_Send(string command, int idx_serial)
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

        public string Recieve_Data_To_Parse_HF700(string rcv_data, ref string parse, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
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

                if(rcv_data.Split(',').Length >4)
                {
                    Program.main_form.SerialData.HF700.concentration = Convert.ToSingle(rcv_data.Split(',')[5]);
                    Program.main_form.SerialData.HF700.concentration = Convert.ToSingle(rcv_data.Split(',')[7]);
                    Program.main_form.SerialData.HF700.Alarm = "";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Concentration_HF700_Alarm, "", false, false);
                }
                else
                {
                    Program.main_form.SerialData.HF700.Alarm = rcv_data;
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Concentration_HF700_Alarm, Program.main_form.SerialData.HF700.Alarm, false, true);
                }
           


            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("HF700" + "." + "Recieve_Data_To_Parse_HF700" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;
        }
    }
}
