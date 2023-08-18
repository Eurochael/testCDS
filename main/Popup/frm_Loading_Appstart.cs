using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace cds
{
    public partial class frm_Loading_Appstart : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public frm_Loading_Appstart()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.progressPanel1.AutoHeight = true;
            timer_seqeunce.Interval = 100; timer_seqeunce.Enabled = true;
        }
        public Boolean actived
        {
            get { return _actived; }
            set
            {
                try
                {
                    _actived = value;
                    if (_actived == true)
                    {

                    }
                    else
                    {
                        timer_seqeunce.Enabled = false;
                    }
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + ".actived." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
            }
        }
        public void next_seq(int seq, string seq_complete, string seq_request)
        {
            string seq_complete_dt_add = "";
            //마지막 수행 시간 Update Next seq 입력
            Program.cg_apploading.dt_last_seq_run = DateTime.Now;

            Program.cg_apploading.seq_no_cur = seq;

            //현재 수행 내용 입력 = complete
            Program.cg_apploading.complete_text = seq_complete;
            //다음 수행 요청 내용 입력 = reqeust
            Program.cg_apploading.request_text = seq_request;

            //상태값 표기
            pbar.EditValue = seq;
            progressPanel1.Caption = seq_complete;
            progressPanel1.Description = seq + " / " + Config_AppLoading.seq_no_max;
            //Sequence 로그 생성
            Program.log_md.LogWrite(seq_complete, Module_Log.enumLog.SEQ_APPLOAD, "", Module_Log.enumLevel.ALWAYS);

            seq_complete_dt_add = Program.cg_apploading.seq_no_old + " / " + DateTime.Now.ToString("HH:mm:ss.fff") + " : " + seq_complete;
            lbox_process_log.Items.Insert(0, seq_complete_dt_add);

            if (Program.cg_apploading.seq_no_cur == Config_AppLoading.seq_no_error)
            {
                if (Program.cg_apploading.seq_no_old != Config_AppLoading.seq_no_error)
                {
                    Program.cg_apploading.seq_no_error_occurred = Program.cg_apploading.seq_no_old;
                }
            }
            if (Program.cg_apploading.seq_no_old != Program.cg_apploading.seq_no_cur)
            {
                Program.log_md.LogWrite("(" + Program.cg_apploading.seq_no_old + ")" + seq_complete, Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                Program.main_form.Insert_System_Message("(" + Program.cg_apploading.seq_no_old + ")" + seq_complete);
                Program.cg_apploading.seq_no_old = Program.cg_apploading.seq_no_cur;
            }

        }
        public void Setting_Inital_Variable()
        {
            //Serial 통신 변수 초기화
            for (int idx = 0; idx < frm_main.serial_use_cnt; idx++)
            {
                //send rcv queue
                Program.main_form.serial_q_rcv_data[idx] = new Queue<byte[]>();
                Program.main_form.serial_q_snd_data[idx] = new Queue<byte[]>();
                //log queue
                Program.main_form.log_serial_q_snd_data[idx] = new Queue<string>();
                Program.main_form.log_serial_q_rcv_data[idx] = new Queue<string>();

                //Data Auto Read True
                Program.main_form.serial_auto_snd[idx] = true;
            }
            //ABB 통신 변수 초기화
            Program.main_form.abb_auto_snd = true;

            //abb queue
            Program.main_form.log_abb_q_rcv_data = new Queue<string>();
            Program.main_form.log_abb_q_snd_data = new Queue<string>();

            //ctc queue
            Program.main_form.log_ctc_q_rcv_data = new Queue<string>();
            Program.main_form.log_ctc_q_snd_data = new Queue<string>();
        }
        private void timer_seqeunce_Tick(object sender, EventArgs e)
        {
            string log_tmp = "";
            //전 Seq 실행 후 경과 시간 체크
            Program.cg_apploading.tp_elapse = DateTime.Now - Program.cg_apploading.dt_last_seq_run;

            switch (Program.cg_apploading.seq_no_cur)
            {
                case 0:
                    //변수 초기화
                    if (Program.cg_apploading.init_variable == false)
                    {
                        //프로세스바 초기화
                        pbar.Properties.Minimum = 0;
                        pbar.Properties.Maximum = Config_AppLoading.seq_no_max;
                        pbar.EditValue = 0;

                        Program.cg_apploading.init_variable = true;
                        Program.cg_apploading.load_complete = false;
                        Program.cg_apploading.Config_Sync_Complete = false;
                        //Maing Config의 경로는 XML에서 가져온다
                        Program.cg_apploading.result = Module_XML.XML_Inidata_Read(Application.StartupPath + "/Setting.xml", "main/path/", "main_config");
                        //Program.cg_apploading.simulation_debug = Module_XML.XML_Inidata_Read(Application.StartupPath + "/Setting.xml", "main/path/", "debug_simulation");
                        //if(Program.cg_apploading.simulation_debug == "true")
                        //{
                        //    Program.cg_apploading.simulation_debug_path = Module_XML.XML_Inidata_Read(Application.StartupPath + "/Setting.xml", "main/path/", "debug_simulation_path");
                        //}
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(Config_AppLoading.seq_no_error, "Main Path Load Fail", "");
                        }
                        else
                        {
                            Program.cg_main.path.yaml = Program.cg_apploading.result;
                            Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_main, "");
                            Setting_Inital_Variable();
                            Program.log_md.LogWrite("APP START - PARAMETER LOAD REQ", Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                            Program.log_md.LogWrite("APP START", Module_Log.enumLog.SEQ_MAIN, "", Module_Log.enumLevel.ALWAYS);
                            Program.log_md.LogWrite("APP START", Module_Log.enumLog.SEQ_SUPPLY, "", Module_Log.enumLevel.ALWAYS);
                            Program.log_md.LogWrite("APP START", Module_Log.enumLog.SEQ_PUMP_CONTROL, "", Module_Log.enumLevel.ALWAYS);
                            Program.log_md.LogWrite("APP START", Module_Log.enumLog.SEQ_SEMI_AUTO, "", Module_Log.enumLevel.ALWAYS);
                            Program.log_md.LogWrite("APP START", Module_Log.enumLog.SEQ_SEMI_AUTO_A, "", Module_Log.enumLevel.ALWAYS);
                            Program.log_md.LogWrite("APP START", Module_Log.enumLog.SEQ_SEMI_AUTO_B, "", Module_Log.enumLevel.ALWAYS);
                            next_seq(10, "Parameter Load Seqence Start & Config Main Load Complete", "Config Main Load");
                        }

                    }
                    else
                    {
                        next_seq(10, "Parameter Load Seqence Start & Config Main Load Fail", "Config Main Load");
                    }
                    break;

                case 10:
                    if (Program.cg_apploading.result == "")
                    {
                        if (thd_Config_File_Upload != null) { if (thd_Config_File_Upload.IsAlive == false) { thd_Config_File_Upload.Abort(); thd_Config_File_Upload = null; thd_Config_File_Upload = new Thread(Config_File_Upload); thd_Config_File_Upload.Start(); } }
                        else { thd_Config_File_Upload = new Thread(Config_File_Upload); thd_Config_File_Upload.Start(); }
                        next_seq(11, "Config Main Load Complete", "Server Config Check");
                    }
                    else
                    {
                        next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                    }
                    break;

                case 11:
                    if(Program.cg_app_info.ignore_ctc_config_sync == false)
                    {
                        lbl_title.Text = "CDS " + "(" + "CTC Sync Use" + ")";
                        if (Program.cg_apploading.tp_elapse.TotalSeconds >= 10)
                        {
                            //Config Main yaml to class Instance
                            Program.cg_apploading.Config_Sync_Complete = false;
                            next_seq(20, "Server Config Check Fail, Use Saved Yaml Config", "Config Trend Load");
                        }
                        else if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                        {
                            //Config Main yaml to class Instance
                            if (Program.cg_apploading.Config_Sync_Complete == true)
                            {
                                lbl_title.ForeColor = Color.Lime;
                                lbl_title.Text = "CDS " + "(" + "CTC Sync Use - Complete" + ")";
                                Console.WriteLine("Sync Complete -> Loading Appstart Seq Next Move");
                                next_seq(20, "Server Config Check Complete", "Config Trend Load");
                            }
                            else
                            {
                                lbl_title.ForeColor = Color.Red;
                                lbl_title.Text = "CDS " + "(" + "CTC Sync Use - Fail" + ")";
                            }
                        }
                    }
                    else
                    {
                        lbl_title.Text = "CDS " + "(" + "CTC Sync Not Use" + ")";
                        next_seq(20, "Server Config Check Not Use", "Config Trend Load");
                    }
                    
                   
                    break;

                case 20:
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        //Config Main yaml to class Instance
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(21, "Config Main Load Complete", "Config Trend Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }

                    }
                    break;

                case 21:
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_trend, "");
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(22, "Config Trend Load Complete", "Config Socket Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }
                    }
                    break;
                case 22:
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_socket, "");
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(23, "Config Socket Load Complete", "Config Trace Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }
                    }
                    break;
                case 23:
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_trace, "");
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(24, "Config Trace Load Complete", "Config App Info Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }
                    }
                    break;
                case 24:
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        if (Program.cg_apploading.result == "") { Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_app_info, ""); }

                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(25, "Config App Info Load Complete", "Config Mixing Step Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }
                    }
                    break;
                case 25:
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        //mixing Step Load는 main만 진행
                        Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_mixing_step, "");
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(26, "Config Mixing Step Load Complete", "Config Unit IO Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }
                    }
                    break;
                case 26:
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_unit_io, "");
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(60, "Config Unit IO Load Complete", "Config Sync(Host) Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }
                    }
                    break;

                case 60:
                    ////서버에서 Config 파일 로그 실패 시 Local 파일 동기화
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_sync, "");
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(70, "Config Sync(Local) Load Complete", "Parameter Data Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }
                    }
                    break;
                case 70:
                    ////Parameter DataTable Load
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        Program.cg_apploading.result = Program.parameter_form.Load_ParameterList(true);
                        Program.parameter_form.Data_Setting();
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(80, "Parameter Data Load Complete", "Alarm Data Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }

                    }
                    break;
                case 80:
                    ////Alarm DataTable Load
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        Program.cg_apploading.result = Program.alarm_form.Load_AlarmList(true);
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(100, "Alarm Data Load Complete", "Trend Data Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }

                    }
                    break;

                case 100:
                    ////Trend Data 설정
                    Program.cg_apploading.result = Program.trendlog_form.Load_TrendData();
                    if (Program.cg_apploading.result == "")
                    {
                        next_seq(110, "Trend Data Load Complete", "Offset Parameter Config Load");
                    }
                    else
                    {
                        next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                    }
                    break;

                case 110:
                    ////Offset Parameter Load
                    Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_parameter, "");
                    if (Program.cg_apploading.result == "")
                    {
                        next_seq(300, "Offset Parameter Config Load", "DIO AIO Config Load");
                    }
                    else
                    {
                        next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                    }
                    break;


                case 300:
                    ////DI, DO, AI, AO, SERIAL Config Load
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        if (Program.cg_apploading.result == "") { Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_di, ""); }
                        if (Program.cg_apploading.result == "") { Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_do, ""); }
                        if (Program.cg_apploading.result == "") { Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_ai, ""); }
                        if (Program.cg_apploading.result == "") { Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_ao, ""); }
                        if (Program.cg_apploading.result == "") { Program.cg_apploading.result = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_serial, ""); }
                        if (Program.cg_apploading.result == "")
                        {
                            Program.io_monitor_form.Setting_initial();
                            Program.io_monitor_form.Set_Grid_Begin(true, true, true, true, true);
                            next_seq(700, "IO Data Load Complete", "COMI ETHERCAT INIT Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }

                    }
                    break;

                case 700:
                    ////설정 완료
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        if (Program.cg_app_info.mode_simulation.use_ethercat_real == false)
                        {

                        }
                        else
                        {
                            if (Program.cg_apploading.result == "") Program.cg_apploading.result = Program.COMI_ETHERCAT.MasterDeviceInit();
                            Program.log_md.LogWrite("COMI EHTERCAT START INFO : " + Program.COMI_ETHERCAT.inital_log, Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                            if (Program.cg_apploading.result == "")
                            {
                                for (int idx = 0; idx < Program.IO.AI.use_cnt; idx++)
                                //for (int idx = 0; idx < 1; idx++)
                                {
                                    //Range는 Comi IDE에서 설정이 필요함
                                    //Program.cg_apploading.result = Program.COMI_ETHERCAT.Analog_Range_Setting(Convert.ToUInt16(Program.IO.AI.Tag[idx].address), (int)Program.IO.AI.Tag[idx].range);
                                    if (Program.cg_apploading.result != "")
                                    {
                                        Program.ethercat_md.run_state = false;
                                    }
                                    else
                                    {
                                        Program.ethercat_md.run_state = true;
                                    }

                                }
                            }

                        }

                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(710, "COMI ETHERCAT INIT Load Complete", "Serial Setting & Load");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }
                    }
                    break;

                case 710:
                    ////Serial Port Setting & Open
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        //가상데몬
                        //Program.cg_apploading.result = Program.main_form.Commi_Serial_Setting();
                        Program.main_form.Commi_Serial_Setting();
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(720, "Serial Setting & Load Complete", "Alarm Cleared By App");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }
                    }

                    break;

                case 720:
                    ////Alarm Clear by reboot And Occrred Alarm Show
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        Program.alarm_form.Alarm_cleared_by_APP_Restart(frm_alarm.enum_cleared_by.REBOOT);
                        Program.main_md.Occured_Alarm_FormShow(false);
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(740, "Alarm Cleared By App Complete", "App Parameter Initialize");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }

                    }

                    break;

                case 740:
                    ////Socket Server Start
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        if (Program.cg_socket.server_port_auto_create == true)
                        {

                            Program.cg_apploading.result = Program.SOCKET.Server_Start(Program.cg_socket.server_port + Program.cg_socket.ctc_network_no - 18);
                        }
                        else
                        {
                            Program.cg_apploading.result = Program.SOCKET.Server_Start(Program.cg_socket.server_port);
                        }
                        if (Program.cg_apploading.result == "")
                        {
                            next_seq(800, "Socket Server Start Complete", "APP FORM LOAD");
                        }
                        else
                        {
                            next_seq(Config_AppLoading.seq_no_error, Program.cg_apploading.result, "");
                        }

                    }

                    break;
                case 800:
                    ////설정 완료
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        if (Program.cg_apploading.CTC_DB_Fail == true)
                        {
                            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CTC_Database_Exception, Program.cg_apploading.CTC_DB_Fail_Text, true, false);
                        }
                        Program.log_md.LogWrite("APP START COMPLETE", Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                        Program.cg_apploading.load_complete = true;
                        Program.main_form.Setting_initial();
                        Program.main_form.Insert_System_Message("CDS Parameter Initialize Complete");
                        next_seq(Config_AppLoading.seq_no_max, "App Parameter Initialize Complete", "");
                    }
                    break;


                case Config_AppLoading.seq_no_error:
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        Program.main_form.Insert_System_Message("CDS Initialize Fail : " + Program.cg_apploading.result);
                        Program.log_md.LogWrite("APP START - Parameter Error : " + Program.cg_apploading.result, Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                        //next_seq(Config_AppLoading.seq_no_error, "Parameter Load Fail", "");
                        timer_seqeunce.Enabled = false;
                        Program.main_form.Opacity = 100;
                        Program.main_form.Show();
                    }
                    break;

                case Config_AppLoading.seq_no_max:
                    if (Program.cg_apploading.tp_elapse.TotalMilliseconds >= Program.cg_apploading.seq_interval_default)
                    {
                        timer_seqeunce.Enabled = false;
                        Program.main_form.Opacity = 100;
                        Program.main_form.SuspendLayout();
                        Program.main_form.Show();
                        Program.main_form.ResumeLayout();
                        Program.main_form.Insert_System_Message("CDS Initialize Complete");
                        this.Close();

                    }
                    break;
            }

        }

        public Thread thd_Config_File_Upload;
        private void Config_File_Upload()
        {

            //public enum Config_type
            //{
            //    cg_main = 1,
            //    cg_app_info = 2,
            //    cg_sync = 3,
            //    cg_trend = 4,
            //    cg_socket = 5,
            //    cg_trace = 6,
            //    cg_mixing_step = 7,
            //    cg_di = 10,
            //    cg_do = 11,
            //    cg_ai = 12,
            //    cg_ao = 13,
            //    cg_serial = 14,
            //    cg_unit_io = 20,
            //    cg_mixing_step_custom1 = 30,
            //}
            string result = "", result_1 = "", result_2 = "", result_3 = "";
            string query = "";
            string filename = "";
            string err = "";
            bool File_download_fail = false;
            int file_id = 0;
            bool sync_error_occur = false;
            DataSet dataset = new DataSet();

            try
            {
                //초기 Setup
                //Unit ID를 가져오기 위함
                result_1 = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_socket, "");
                result_2 = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_app_info, "");
                result_3 = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_main, "");

                if (Program.cg_app_info.ignore_ctc_config_sync == false)
                {
                    if (result_1 == "" && result_2 == "" && result_3 == "")
                    {
                        for (int idx = 0; idx < 11; idx++)
                        {
                            switch (idx)
                            {
                                //Sequence 4
                                case 0:
                                    filename = Program.cg_main.path.yaml + @"config_main.yaml";
                                    file_id = 2;// (int)Config_type.cg_main;
                                    break;
                                case 1:
                                    filename = Program.cg_main.path.yaml + @"config_app_info.yaml";
                                    file_id = 1;// (int)Config_type.cg_app_info;
                                    break;
                                case 2:
                                    filename = Program.cg_main.path.yaml + @"config_trend.yaml";
                                    file_id = 7;// (int)Config_type.cg_trend;
                                    break;
                                case 3:
                                    filename = Program.cg_main.path.yaml + @"config_socket.yaml";
                                    file_id = 5;// (int)Config_type.cg_socket;
                                    break;
                                case 4:
                                    filename = Program.cg_main.path.yaml + @"config_trace.yaml";
                                    file_id = 6;// (int)Config_type.cg_trace;
                                    break;
                                case 5:
                                    filename = Program.cg_main.path.yaml + @"config_mixing_step.yaml";
                                    file_id = 3;// (int)Config_type.cg_mixing_step;
                                    break;
                                case 6:
                                    filename = Program.cg_main.path.yaml + @"config_di.yaml";
                                    file_id = 8;// (int)Config_type.cg_di;
                                    break;
                                case 7:
                                    filename = Program.cg_main.path.yaml + @"config_do.yaml";
                                    file_id = 9;// (int)Config_type.cg_do;
                                    break;
                                case 8:
                                    filename = Program.cg_main.path.yaml + @"config_ai.yaml";
                                    file_id = 10;// (int)Config_type.cg_ai;
                                    break;
                                case 9:
                                    filename = Program.cg_main.path.yaml + @"config_ao.yaml";
                                    file_id = 11;// (int)Config_type.cg_ao;
                                    break;
                                case 10:
                                    filename = Program.cg_main.path.yaml + @"config_serial.yaml";
                                    file_id = 12;// (int)Config_type.cg_serial;
                                    break;
                                case 11:
                                    filename = Program.cg_main.path.yaml + @"config_sequence.yaml";
                                    file_id = 4;// (int)Config_type.cg_serial;
                                    break;
                            }

                            query = ""; if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; dataset = new DataSet(); }
                            query = "SELECT * FROM configurations WHERE unit_id = '" + Program.cg_socket.ctc_network_no + "' AND file_id = '" + file_id + "'";
                            Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_server.connection, query, dataset, ref err);
                            if (err != "") { Program.log_md.LogWrite("", Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                            if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                            {
                                Console.WriteLine("Sync Complete : " + filename + "(" + file_id + ")");
                                //파일이 있을 경우 다운로드
                                //Config CDS 변경 시 CTC에서 변경 후 다운로드 해야함
                                Program.database_md.MariaDB_MainQuery_File_Download(Program.cg_main.db_server.connection, filename, Program.cg_socket.ctc_network_no, file_id, ref result, ref err);
                                if (err == "")
                                {
                                    //Program.cg_apploading.Config_Sync_Complete = true;
                                    Program.log_md.LogWrite(this.Name + ".Config_File DownLoad OK " + filename + Program.cg_socket.ctc_network_no + file_id, Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                                }
                                else
                                {
                                    Program.cg_apploading.CTC_DB_Fail = true;
                                    Program.cg_apploading.CTC_DB_Fail_Text = err.Replace("'", "");
                                    Program.cg_apploading.Config_Sync_Complete = false;
                                    Program.log_md.LogWrite(this.Name + ".Config_File DownLoad Fail " + filename + Program.cg_socket.ctc_network_no + file_id + " / " + result, Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                                    break;
                                }
                            }
                            else
                            {
                                sync_error_occur = true;
                                Program.log_md.LogWrite(this.Name + ".Config_File Empty: " + filename + Program.cg_socket.ctc_network_no + file_id, Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                                Program.cg_apploading.CTC_DB_Fail = true;
                                Program.cg_apploading.CTC_DB_Fail_Text = err.Replace("'", "");
                                Program.cg_apploading.Config_Sync_Complete = false;
                                break;
                                //파일이 없을 경우 업로드
                                //업로드 하지 않음
                                //Program.database_md.MariaDB_MainQuery_File_Upload(Program.cg_main.db_server.connection, filename, Program.cg_socket.ctc_network_no, file_id, ref result);
                            }
                        }

                        if(sync_error_occur == false)
                        {
                            Program.cg_apploading.Config_Sync_Complete = true;
                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
              
            }
        }
        private void btn_pause_Click(object sender, EventArgs e)
        {
            if (timer_seqeunce.Enabled == true)
            {
                timer_seqeunce.Enabled = false;
            }
            else
            {
                timer_seqeunce.Enabled = true;
            }
            Program.log_md.LogWrite(this.Name + ".btn_pause_Click.", Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }

        private void btn_retry_Click(object sender, EventArgs e)
        {
            if (Program.cg_apploading.seq_no_error_occurred == 0)
            {
                Program.main_md.Message_By_Application("Nothing Error", frm_messagebox.enum_apptype.Only_OK);
                return;
            }
            else
            {
                //중단된 시점에서 재시작
                Program.cg_apploading.seq_no_cur = Program.cg_apploading.seq_no_error_occurred;
                //초기 시점에서 재시작
                Program.cg_apploading.seq_no_cur = 0;
                timer_seqeunce.Enabled = true;
            }
            Program.log_md.LogWrite(this.Name + ".btn_retry_Click.", Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }
    }
}