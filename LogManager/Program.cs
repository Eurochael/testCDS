using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace LogManager
{
    static class Program
    {
        //DLL Module
        public static LOG.LogClass LOG = new LOG.LogClass();
        public static DB_MARIA.MainModule DB_MARIA = new DB_MARIA.MainModule();

        //Class Module
        public static Module_Main main_md = new Module_Main();
        public static Module_YAML yaml_md = new Module_YAML();
        public static Module_Log log_md = new Module_Log();
        public static Module_DB database_md = new Module_DB();

        //Class Instance Class : 구조체 전역변수 선언 후 등록
        public static Config_Main cg_main = new Config_Main();
        public static Config_App_Info cg_appinfo = new Config_App_Info();
        public static Config_log_path cg_logpath = new Config_log_path();
        public static Config_db_path cg_dbpath = new Config_db_path();

        //form
        public static frm_main main_form;

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        /// 

        [STAThread]

        static void Main()
        {
            if (IsExistProcess(Process.GetCurrentProcess().ProcessName))
            {
                Application.ExitThread();
                Environment.Exit(0);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                main_form = new frm_main();
                Application.Run(main_form);
            }
                
        }

        static bool IsExistProcess(string processName)
        {
            Process[] process = Process.GetProcesses();
            int cnt = 0;
            foreach (var p in process)
            {
                if (p.ProcessName == processName)
                    cnt++;
                if (cnt > 1)
                    return true;
            }
            return false;
        }
    }
}
