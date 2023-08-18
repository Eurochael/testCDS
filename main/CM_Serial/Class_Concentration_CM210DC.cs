using System;

namespace cds
{
    public class Class_Concentration_CM210DC
    {
        //HF,0.000,wt%,TEMP, 25.2
        public struct ST_Concentration_CM210DC
        {
            public float Concentration;
            public float concentration_old;
            public double Temp;
            public int Alarm;
        }

        public string Recieve_Data_To_Parse_CM210(string rcv_data, ref string parse, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
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
                    //HF,0.000,%  ,TEMP, 26.4
                    Program.main_form.SerialData.CM210DC.Concentration = Convert.ToSingle(rcv_data.Split(',')[1]);
                }
         
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("CM210" + "." + "Recieve_Data_To_Parse_CM210" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;
        }
    }
}
