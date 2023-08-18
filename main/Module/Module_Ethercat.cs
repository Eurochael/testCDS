using System;

namespace cds
{
    class Module_Ethercat
    {
        public bool run_state = false;
        public enum_error_type error_state = enum_error_type.None;
        public DateTime dt_serial_validate_check = new DateTime();

        public enum enum_error_type
        {
            None = 0,
            Serial_Daemon = 1,
            Init_Error = 2,
            Serial_CM_Error = 3,
            Run_Error = 4
        }
        public string DO_Write_Alone(int idx, bool state)
        {

            string result = "";
            try
            {
                if (Program.cg_app_info.mode_simulation.use == true)
                {
                    Program.IO.DO.Tag[idx].value = state;
                }
                else
                {
                    if (Program.IO.DO.Tag[idx].use == true)
                    {
                        result = Program.COMI_ETHERCAT.DO_Write_Alone(Convert.ToByte(Program.IO.DO.Tag[idx].address), state);
                    }
                    else
                    {
                        Program.log_md.LogWrite("Module_Ethercat" + ".DO_Write_Alone.(idx : " + idx + ", " + Program.IO.DO.Tag[idx].name + "," + Program.IO.DO.Tag[idx].address + ") : DO Not Use", Module_Log.enumLog.DEBUG_DIO, "", Module_Log.enumLevel.ALWAYS);
                    }
                    //DO Write adress로 출력  // DO 사용 Index로
                    //DI Read address로 입력  // DI 사용 Index로
                    //result = Program.COMI_ETHERCAT.DO_Write_Alone(Convert.ToByte(address), state);
                }
            }
            catch(Exception e) { }
            
            return result;
        }


    }
}
