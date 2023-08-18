using System;
using System.Diagnostics;
using System.Windows.Forms;
namespace cds
{
    static class Program
    {
        //Class Module
        public static Module_main main_md = new Module_main();
        public static Module_sub sub_md = new Module_sub();
        public static Module_Log log_md = new Module_Log();
        public static Module_Ethercat ethercat_md = new Module_Ethercat();
        public static Module_DB database_md = new Module_DB();
        public static Module_XML xml_md = new Module_XML();
        public static Module_User user_md = new Module_User();
        public static Module_YAML yaml_md = new Module_YAML();

        //Class Module Serial
        public static Class_TempController_M74 TempController_M74 = new Class_TempController_M74();
        public static Class_ThermoStat_HE_3320C ThermoStart_HE_3320C = new Class_ThermoStat_HE_3320C();
        public static Class_FlowMeter_USF500 FlowMeter_USF500 = new Class_FlowMeter_USF500();
        public static Class_FlowMeter_Sonotec FlowMeter_SONOTEC = new Class_FlowMeter_Sonotec();
        public static Class_PumpController_PB12 PumpController_PB12 = new Class_PumpController_PB12();
        public static Class_SCR_DPU31A_025A SCR_DPU31A_025A = new Class_SCR_DPU31A_025A();
        public static Class_Concentration_CM210DC CM210DC = new Class_Concentration_CM210DC();
        public static Class_Concentration_CS600F CS600F = new Class_Concentration_CS600F();
        public static Class_Concentration_CS150C CS150C = new Class_Concentration_CS150C();
        public static Class_Concentration_HF700 HF700 = new Class_Concentration_HF700();
        //Class Module Socket (ABB ASP31 / Heat Excahnger / CTC)
        public static Class_ABB ABB = new Class_ABB();
        public static Class_HeatExchanger Heat_Exchanger = new Class_HeatExchanger();
        public static Module_Socket CTC = new Module_Socket();

        //DLL Module
        public static LOG.LogClass LOG = new LOG.LogClass();
        public static DB_MARIA.MainModule DB_MARIA = new DB_MARIA.MainModule();
        public static TCP_IP_SOCKET.Cls_Socket SOCKET = new TCP_IP_SOCKET.Cls_Socket();
        public static Comi_Ethercat.Class_Main COMI_ETHERCAT = new Comi_Ethercat.Class_Main();

        //Form
        public static frm_main main_form;
        public static frm_skin skin_form;
        public static frm_overview overview_form;

        public static frm_io_monitor io_monitor_form;
        public static frm_communications communications_form;
        public static frm_schematic schematic_form;

        //2 Tank 사양 고정 3Tank까지 사용 가능
        public static tank_class[] tank;
        public static seq_info seq;

        public static frm_alarm alarm_form;
        public static frm_parameter parameter_form;
        public static frm_mixing_step mixing_step_form;

        public static frm_trendlog trendlog_form;
        public static frm_eventlog eventlog_form;
        public static frm_alarmlog alarmlog_form;
        public static frm_totalusagelog totalusagelog_form;

        public static frm_messagebox message_form;
        public static frm_occured_alarm occured_alarm_form;
        public static frm_Loading_Appstart loading_appstart_form;

        public static frm_process_indicator process_indicator_form;
        public static frm_popup_page popup_page;
        public static frm_popup_login popup_login;
        public static frm_popup_manual_sequence_select popup_manual_sequence;

        //Class Instance1 Class : 구조체 전역변수로 선언 후 등록
        public static Config_Main cg_main = new Config_Main();
        public static Config_Offset_Parameter cg_offset = new Config_Offset_Parameter();
        public static Config_App_Info cg_app_info = new Config_App_Info();
        public static Config_Trend_DataList cg_trend_datalist = new Config_Trend_DataList();
        public static Config_Socket cg_socket = new Config_Socket();
        public static Config_AppLoading cg_apploading = new Config_AppLoading();
        public static Config_Trace cg_trace = new Config_Trace();
        public static Config_Mixing_Step cg_mixing_step = new Config_Mixing_Step();

        public static Config_IO IO = new Config_IO();

        //public static Config_IO.DI DI = new Config_IO.DI();
        //public static Config_IO.DO DO = new Config_IO.DO();
        //public static Config_IO.AI AI = new Config_IO.AI();
        //public static Config_IO.AO AO = new Config_IO.AO();
        //public static Config_IO.DI SERIAL = new Config_IO.DI();

        public static Config_Unit_Io cg_unit_no = new Config_Unit_Io();

        //알람, 파라메타 List Class(구조체)
        public static Alarm_List alarm_list = new Alarm_List();
        public static Parameter_List parameter_list = new Parameter_List();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
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
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //Dump Add
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Module_main.CurrentDomain_UnhandledException);

                //Class to Instance2 : 클래스 구조체가 자식이 있을 경우 Instance 초기화 필요
                //상속 사x`용 안함 / doublebuffer 등 기타 도구만 사용
                Program.cg_main.path = new App_Path();
                Program.cg_main.db_server = new MariaDB.Server();
                Program.cg_main.db_local_cds = new MariaDB.Local_cds();
                Program.cg_main.db_Local_fdc = new MariaDB.Local_fdc();
                Program.cg_app_info.mode_simulation = new Mode_Simulation();
                Program.cg_app_info.internal_info = new System_Internal_Info();

                seq = new seq_info();
                seq.main = new seq_type_main();
                seq.supply = new seq_type_supply();
                seq.pumpcontrol = new seq_type_pumpcontrol();
                seq.monitoring = new seq_type_monitoring();
                seq.semi_auto_tank_a = new seq_type_semi_auto();
                seq.semi_auto_tank_b = new seq_type_semi_auto();
                seq.semi_auto_tank_all = new seq_type_semi_auto();
                seq.ccss_variable = new ccss_variable[3];

                for (int idx = 0; idx < 3; idx++)
                {
                    seq.ccss_variable[idx] = new ccss_variable();
                }
                //2 Tank 사양 고정 3Tank까지 사용 가능 Tank Class 초기화
                tank = new tank_class[tank_class.use_tank_cnt];
                for (int idx = 0; idx < tank_class.use_tank_cnt; idx++)
                {
                    Program.tank[idx] = new tank_class();
                    Program.tank[idx].ccss_data = new tank_class.ccss_data_info[tank_class.ccss_cnt];
                    for (int idx2 = 0; idx2 < tank_class.ccss_cnt; idx2++)
                    {
                        Program.tank[idx].ccss_data[idx2] = new tank_class.ccss_data_info();
                    }
                }
                //form Instance Create

                skin_form = new frm_skin();
                overview_form = new frm_overview();
                io_monitor_form = new frm_io_monitor();
                schematic_form = new frm_schematic();

                mixing_step_form = new frm_mixing_step();
                alarm_form = new frm_alarm();
                parameter_form = new frm_parameter();

                trendlog_form = new frm_trendlog();
                eventlog_form = new frm_eventlog();
                alarmlog_form = new frm_alarmlog();
                totalusagelog_form = new frm_totalusagelog();

                message_form = new frm_messagebox();
                occured_alarm_form = new frm_occured_alarm();
                loading_appstart_form = new frm_Loading_Appstart();

                process_indicator_form = new frm_process_indicator();

                popup_page = new frm_popup_page();
                popup_login = new frm_popup_login();
                popup_manual_sequence = new frm_popup_manual_sequence_select();

                communications_form = new frm_communications();

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
