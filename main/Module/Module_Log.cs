using System;

namespace cds
{
    class Module_Log
    {
        public static Boolean use_terminallog = true;
        public enum enumLog
        {
            Alarm = 1,
            Process = 3,
            Error = 6,
            SEQ_APPLOAD = 7,
            ProcessData = 8,
            Button = 9,
            SQL = 10,
            DEBUG = 11,
            OriginalData = 12,
            SelectData = 13,
            App_Info = 14,
            TrendData = 20,
            DigitalData_In = 21,
            DigitalData_Out = 22,
            SocketData = 30,
            SEQ_MAIN = 40,
            SEQ_SUPPLY = 41,
            SEQ_PUMP_CONTROL = 42,
            SEQ_SEMI_AUTO = 43,
            SEQ_SEMI_AUTO_A = 44,
            SEQ_SEMI_AUTO_B = 45,
            SEQ_MONITORING = 46,
            SERAL_DATA_1 = 70,
            SERAL_DATA_2 = 71,
            SERAL_DATA_3 = 72,
            SERAL_DATA_4 = 73,
            SERAL_DATA_5 = 74,
            SERAL_DATA_6 = 75,
            SERAL_DATA_7 = 76,
            SERAL_DATA_8 = 77,
            SERAL_DATA_9 = 78,
            SERAL_DATA_10 = 79,
            DEBUG_DIO = 80,
            ABB_DATA = 90,
            CTC_SOCKET_DATA = 91,
            HEAT_EXCHANGER = 92,
            RESOURCE_1 = 92,
            RESOURCE_2 = 93
        }
        public enum enumLevel
        {
            //ALWAYS : 중요 로그로 상시 저장
            //Level_1 : 선택적 저장 버튼 등
            //Level_2 : Setup 기간에만 사용
            ALWAYS = 0, LEVEL_1 = 1, LEVEL_2 = 2
        }
        public enum enumTerminal
        {
            None = 1
        }
        public void LogWrite(string text, enumLog logindex, string filename, enumLevel level)
        {
            if (Program.cg_app_info.log_save_level_min < (int)level) { return; }
            else
            {
                if (text == "") { return; }
                string LogHead = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff ") + text;
                if (filename == "") { filename = DateTime.Now.ToString("yyyyMMdd_HH_mm_ss"); }
                if((int)level != 0 && (int)level > Program.cg_app_info.log_save_level_min)
                {
                    return;
                }
                try
                {
                    switch (logindex)
                    {
                        case enumLog.Alarm:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_Alarm\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_Alarm]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.Error:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_Error\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_Error]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SEQ_APPLOAD:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SEQ\APP_LOAD\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SEQ]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SEQ_MAIN:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SEQ\MAIN\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SEQ]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SEQ_SUPPLY:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SEQ\SUPPLY\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SEQ]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SEQ_PUMP_CONTROL:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SEQ\PUMPCONTROL\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SEQ]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SEQ_SEMI_AUTO_A:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SEQ\SEMI_AUTO_A\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SEQ]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SEQ_SEMI_AUTO_B:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SEQ\SEMI_AUTO_B\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SEQ]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SEQ_MONITORING:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SEQ\MONITORING\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SEQ]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.App_Info:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_AppInfo\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_AppInfo]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.Process:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_Process\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_Process]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.ProcessData:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_ProcessData\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_ProcessData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.DEBUG:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_DEBUG\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_DEBUG]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.DEBUG_DIO:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_DEBUG_DIO\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_DEBUG_DIO]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.Button:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_Button\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_Button]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.OriginalData:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_OriginalData\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_OriginalData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.TrendData:
                            //Program.LOG.LogWrite(LogHead, Program.cg_main.path.log_trend + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_TrendData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + "_" + DateTime.Now.ToString("mm").Substring(0, 1) + @".log");
                            break;
                        case enumLog.DigitalData_In:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log_io + DateTime.Now.ToString("yyyy-MM-dd") + @"\Digital\In\\", @"[Log_DigitalData_IN]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.DigitalData_Out:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log_io + DateTime.Now.ToString("yyyy-MM-dd") + @"\Digital\Out\\", @"[Log_DigitalData_OUT]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SocketData:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SocketData\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SocketData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SERAL_DATA_1:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SerialData\Serial_1\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SerialData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SERAL_DATA_2:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SerialData\Serial_2\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SerialData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SERAL_DATA_3:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SerialData\Serial_3\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SerialData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SERAL_DATA_4:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SerialData\Serial_4\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SerialData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SERAL_DATA_5:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SerialData\Serial_5\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SerialData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SERAL_DATA_6:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SerialData\Serial_6\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SerialData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SERAL_DATA_7:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SerialData\Serial_7\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SerialData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SERAL_DATA_8:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SerialData\Serial_8\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SerialData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SERAL_DATA_9:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SerialData\Serial_9\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SerialData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.SERAL_DATA_10:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_SerialData\Serial_10\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_SerialData]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.ABB_DATA:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_Socket\ABB\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_ABB]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.CTC_SOCKET_DATA:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_Socket\CTC\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_CTC]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.RESOURCE_1:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_Resource_1\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_Resource_1]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                        case enumLog.RESOURCE_2:
                            Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_Resource_2\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_Resource_2]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                            break;
                    }
                }
                catch (Exception ex) { }

                finally
                {

                }
              
            }

            

        }

        public void terminal_log(String log, enumTerminal LogIndex)
        {
            if (use_terminallog == false) { return; }
            switch (LogIndex)
            {
                case enumTerminal.None:
                    Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " : " + log);
                    break;
                default:
                    Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " : " + log);
                    break;
            }
        }
    }
}
