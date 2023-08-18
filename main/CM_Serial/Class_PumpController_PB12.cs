using System;
using System.Text;

namespace cds
{
    public class Class_PumpController_PB12
    {
        public struct ST_PumpController_PB12
        {
            public int Alarm;
            public bool sol_control_error;
            public bool run_state;
        }
        public enum enum_call_by
        {
            SUPPLY_A = 0, SUPPLY_B = 1
        }

        public static string read_total_stroke_count = "%01#RDD0612006121"; //rcv %01$RD000000000011
        public static string read_short_count = "%01#RDD0612506125"; //rcv  %01$RD000016
        public static string read_current_setting = "%01#RDD8000180001"; //rcv %01$RD0123400023400234002342340023400234000023400099999999990050000000000007000000000015

        //Example: To set 1stroke-FL to 1.234 L, send it as "1234".
        public static string write_1stroke_fl = "%01#WDD0010100101{0}"; //Ex %01#WDD00101001011234 rcv : %01$WD13 / error rcv : %01!0207
        public static string write_set_fl_a = "%01#WDD0010200102{0}";
        public static string write_set_fl_b = "%01#WDD0010300103{0}";
        public static string write_set_fl_c = "%01#WDD0010300103{0}";

        public static string write_flow_alarm_variation_range_setting = "%01#WDD0010700107{0}";

        public static string write_clearing_flow_alarm = "%01#WCSR05001"; //rcv %01$WC14
        public static string write_clearing_overtime_alarm = "%01#WCSR05011"; //rcv %01$WC14
        public static string write_clearing_setting_alarm = "%01#WCSR05021"; //rcv %01$WC14
        public static string write_clearing_life_alarm = "%01#WCSR05031"; //rcv %01$WC14
        public static string write_clearing_leak_alarm = "%01#WCSR05041"; //rcv %01$WC14

        public void Pump_ON_OFF(ref ST_PumpController_PB12 Pump, bool state)
        {
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                Pump.run_state = state;
            }
            else
            {
                Pump.run_state = state;
            }
        }
        public byte[] SET_FLOW(int flow, int idx_serial)
        {
            return Message_Command_To_Byte_BP12_TO_Send(string.Format(write_1stroke_fl, flow.ToString("X4")), idx_serial); ;
        }
        public byte[] Message_Command_To_Byte_BP12_TO_Send(string command, int idx_serial)
        {
            byte[] send_msg = new byte[0];
            int checksum = 0;
            string checksum_parse = "**";
            string msg = "";
            try
            {
                msg = command + checksum_parse + Convert.ToChar(0x0D);
                send_msg = Encoding.UTF8.GetBytes(msg);
            }
            catch (Exception ex)
            {
                send_msg = null;
            }
            if (idx_serial != -1) { Program.main_form.Serial_data_Enqueue(ref send_msg, idx_serial); }
            return send_msg;
        }
        public string Recieve_Data_To_Parse_PB12(enum_call_by call_by, string rcv_data, ref string parse, ref Class_Serial_Common.Rcv_Data rcv_data_parse)
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

                parse = rcv_data.Replace(Convert.ToChar(0x02), ' ').Replace(Convert.ToChar(0x0D), ' ').Replace(Convert.ToChar(0x0A), ' ');
                parse = parse.Trim();
                result = parse;
                //커맨드로 구분되지 않기 때문에
                //rcv_data.Split(',').Length로 구분 / Command를 Split으로 구분할 수 있도록 명령어를 나눠놓음
                //Console.WriteLine("**********************");
                //Console.WriteLine("RCV RAW DATA : " + parse);
                if (rcv_data.Split(',').Length > 2 && rcv_data.IndexOf(Convert.ToChar(0x02)) >= 0 && rcv_data.IndexOf(Convert.ToChar(0x0D)) >= 0)
                {

                    if (rcv_data.Split(',').Length == 3)
                    {

                    }
                    else if (rcv_data.Split(',').Length == 4)
                    {
                    }
                    else if (rcv_data.Split(',').Length == 6)
                    {
                    }
                    else if (rcv_data.Split(',').Length == 14)
                    {

                    }
                    else if (rcv_data.Split(',').Length == 26)
                    {

                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("PB12" + "." + "Recieve_Data_To_Parse_PB12" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

            return result;
        }
    }
}
