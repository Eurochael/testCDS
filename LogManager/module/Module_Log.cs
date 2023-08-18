using System;

namespace LogManager
{
    class Module_Log
    {
        public static Boolean use_terminallog = true;
        public enum enumLog
        {
            Error = 0,
            Process = 1
        }
        public enum enumTerminal
        {
            None = 1
        }
        public void LogWrite(string text, enumLog logindex, string filename)
        {
            string LogHead = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff ") + text;
            if (filename == "") { filename = DateTime.Now.ToString("yyyyMMdd_HH_mm_ss"); }

            switch (logindex)
            {
                case enumLog.Error:
                    Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_Manager\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_Error]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                    break;
                case enumLog.Process:
                    Program.LOG.LogWrite(LogHead, Program.cg_main.path.log + @"\Log_Manager\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\", @"[Log_Process_Log_Manager]_" + DateTime.Now.ToString("yyyyMMdd_HH") + @".log");
                    break;
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
