using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static TCP_IP_SOCKET.Cls_Socket;

namespace cds
{
    public partial class frm_main : DevExpress.XtraEditors.XtraForm
    {

        private Boolean _actived = false;
        //thread
        public Thread thd_data_operate;
        public Thread thd_socket_operate;
        //AIO DIO 는 Rcv 수시로 진행 / Send는 Event 발생 시
        public Thread thd_ethercat_commi_rcv_ad;
        public const int serial_use_cnt = 10;
        public const int abb_use_cnt = 1;

        //Socket Send Rcv 각 Interval 맞춰서 진행, Send Event 원할 시 Send Queue에 넣는다. LAL, DHF만 사용
        public Thread thd_Rcv_Send_ABB;
        public Thread thd_Rcv_Send_HeatExchanger;
        public Thread thd_CTC_Message_Manager;
        //Serial은 Send Rcv 각 Interval 맞춰서 진행, Send Event 원할 시 Send Queue에 넣는다. //Serial 개수는 8개로 고정되있음, 8개의 멀티스레드로 동작
        public Thread[] thd_ethercat_commi_rcv_send_serial = new Thread[serial_use_cnt];

        //통신 로그 저장 Serial queue
        public bool[] serial_port_state = new bool[serial_use_cnt];
        public bool[] serial_auto_snd = new bool[serial_use_cnt];
        public DateTime[] dt_auto_send_last = new DateTime[serial_use_cnt];
        public DateTime[] dt_rcv_serial_last = new DateTime[serial_use_cnt];
        public DateTime[] dt_serial_validate_check = new DateTime[serial_use_cnt];
        public Queue<byte[]>[] serial_q_rcv_data = new Queue<byte[]>[serial_use_cnt];
        public Queue<byte[]>[] serial_q_snd_data = new Queue<byte[]>[serial_use_cnt];
        //Commmunication Class에서 View를 위한 Log queue, 큐는 최대 5개만 유지
        public const int log_q_count = 10;
        public Queue<string>[] log_serial_q_rcv_data = new Queue<string>[serial_use_cnt];
        public Queue<string>[] log_serial_q_snd_data = new Queue<string>[serial_use_cnt];

        //통신 로그 저장 abb queue
        public bool abb_auto_snd = false;
        public int abb_auto_delay = 10;
        public DateTime dt_abb_auto_send_last = new DateTime();
        public DateTime dt_rcv_abb_last = new DateTime();
        public Queue<byte[]> abb_q_rcv_data = new Queue<byte[]>();
        public Queue<byte[]> abb_q_snd_data = new Queue<byte[]>();
        //Commmunication Class에서 View를 위한 Log queue, 큐는 최대 5개만 유지
        public Queue<string> log_abb_q_rcv_data = new Queue<string>();
        public Queue<string> log_abb_q_snd_data = new Queue<string>();

        //통신 로그 저장 ctc queue
        public DataTable dt_message_manage = new DataTable(); //CTC RCV, SEND Message Retry 관리 table
        public ConcurrentQueue<UInt32> queue_delete_manage = new ConcurrentQueue<UInt32>();
        public bool ctc_auto_snd = false;
        public DateTime dt_rcv_ctc_last = new DateTime();
        public Queue<string> log_ctc_q_rcv_data = new Queue<string>();
        public Queue<string> log_ctc_q_snd_data = new Queue<string>();

        //통신 로그 저장 Dsp Mix Heat Exchanger queue
        public bool heat_exchanger_auto_snd = false;
        public int heat_exchanger_auto_delay = 10;
        public DateTime dt_heat_exchanger_auto_send_last = new DateTime();
        public DateTime dt_rcv_heat_exchanger_last = new DateTime();
        public Queue<byte[]> heat_exchanger_q_rcv_data = new Queue<byte[]>();
        public Queue<byte[]> heat_exchanger_q_snd_data = new Queue<byte[]>();
        //Commmunication Class에서 View를 위한 Log queue, 큐는 최대 5개만 유지
        public Queue<string> log_heat_exchanger_q_rcv_data = new Queue<string>();
        public Queue<string> log_heat_exchanger_q_snd_data = new Queue<string>();

        public DevExpress.XtraEditors.SimpleButton last_click;

        public delegate void Del_Loading_Appstart(Boolean status);
        public delegate bool Del_Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm enum_alarm, Config_IO.Config_Digial_Tag di_tag);
        public delegate bool Del_Message_By_Application(string message, frm_messagebox.enum_apptype apptype);

        //Serial Data 변수 Main에서 받은 Rcv Data를 각 Chemical Class의 변수에 할당한다.
        public class_serial_data_merge SerialData = new class_serial_data_merge();

        //System Resource
        public float cpu_temp = 0;
        public float cpu_fan_speed = 0;
        public string hardware_info = "";
        //Flag
        public bool CDS_enable_to_ctc = false;
        public bool CDS_enable_to_ctc_app_load = false;
        public bool CDS_process_stop_to_ctc = false;
        public bool CDS_auto_mode = false;
        public bool CDS_manual_mode = false;
        public bool APP_LOG_MANAGER_STATE = false;
        public bool APP_SERIAL_DAEMON_STATE = false;
        public bool Tank_A_Temp_Alarm_Start = false;
        public bool Tank_B_Temp_Alarm_Start = false;

        public class class_serial_data_merge
        {
            public Class_Concentration_CM210DC.ST_Concentration_CM210DC CM210DC;
            public Class_Concentration_CS600F.ST_Concentration_CS600F CS600F;
            public Class_Concentration_CS150C.ST_Concentration_CS150C CS150C;
            public Class_Concentration_HF700.ST_Concentration_HF700 HF700;
            public Class_SCR_DPU31A_025A.ST_SCR_DPU31A_025A SCR;
            public Class_TempController_M74.ST_TempController_M74 TEMP_CONTROLLER; // M74(3) + M74R(4)
            public Class_PumpController_BP21.ST_PumpController_BP21 CIRCULATION_PUMP_CONTROLLER;
            public Class_FlowMeter_USF500.ST_FlowMeter_USF500 FlowMeter_USF500;
            public Class_FlowMeter_Sonotec.ST_FlowMeter_Sonotec FlowMeter_Sonotec;
            public Class_PumpController_PB12.ST_PumpController_PB12 SUPPLY_A_PUMP_CONTROLLER;
            public Class_PumpController_PB12.ST_PumpController_PB12 SUPPLY_B_PUMP_CONTROLLER;
            public Class_ThermoStat_HE_3320C.ST_ThermoStat_HE_3320C Circulation_Thermostat;
            public Class_ThermoStat_HE_3320C.ST_ThermoStat_HE_3320C Supply_A_Thermostat;
            public Class_ThermoStat_HE_3320C.ST_ThermoStat_HE_3320C Supply_B_Thermostat;
            //public Class_TempController_M74.ST_TempController_M74 TEMP_CONTROLLER1; // M74(1) + M74(2)
        }

        public void Loding_Appstart(Boolean status)
        {
            if (status == true)
            {
                Program.loading_appstart_form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                Program.loading_appstart_form.ShowDialog();
            }
            else
            {
                Program.loading_appstart_form.Close();
            }
        }
        #region ui_change 사용 변수
        public structure_ui_change_para st_ui_change_para;
        public struct structure_ui_change_para
        {
            public int subtimer_count;//= 10;
            public TimeSpan[] tp_check_interval;//= new TimeSpan[ui_change_subtimer_count];
            public DateTime[] dt_check_last_act;//= new DateTime[ui_change_subtimer_count];
            public Boolean timer_initial;// = false;
        }
        #endregion
        #region data_operate 사용 변수
        public structure_data_operate_para st_data_operate_para;
        public struct structure_data_operate_para
        {
            public int subtimer_count;//= 10;
            public TimeSpan[] tp_check_interval;//= new TimeSpan[ui_change_subtimer_count];
            public DateTime[] dt_check_last_act;//= new DateTime[ui_change_subtimer_count];
            public Boolean timer_initial;// = false;
        }
        #endregion
        #region socket_operate 사용 변수
        public structure_data_operate_para st_socket_operate;
        public struct structure_socket_operate
        {
            public int subtimer_count;//= 10;
            public TimeSpan[] tp_check_interval;//= new TimeSpan[ui_change_subtimer_count];
            public DateTime[] dt_check_last_act;//= new DateTime[ui_change_subtimer_count];
            public Boolean timer_initial;// = false;
        }
        #endregion
        public frm_main()
        {
            InitializeComponent();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                //https://docs.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles
                var cp = base.CreateParams; // Extend the CreateParams property of the Button class.
                                            //cp.Style |= 0x00000040; // BS_ICON value
                                            //CS_DROPSHADOW = 0x00020000; 그림자 제거
                                            //WS_EX_TOOLWINDOW = 0x80; APP Icon 숨김
                                            //WS_EX_COMPOSITED = 0x2000000; Flicker 제거
                                            //CP_NOCLOSE_BUTTON = 0x200; TitleBar 제거
                                            //WM_NOACTIVATE = 0x8000000L; Focus 방지
                cp.ExStyle |= 0x02000000;
                return cp;
            }
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

                    }
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + ".actived." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
            }
        }
        private void frm_main_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            this.BeginInvoke(new Del_Loading_Appstart(Loding_Appstart), true);
            Setting_variable();
        }
        private void frm_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            setting_dispose(false);
        }
        private void frm_main_KeyDown(object sender, KeyEventArgs e)
        {
            //Skin Change Form Load
            if (e.Alt == true && e.Control == true && e.KeyCode == Keys.T)
            { Program.skin_form.ShowDialog(); }

            //Simulation From Load
            if (e.Alt == true && e.Control == true && e.KeyCode == Keys.S)
            { frm_simulation simulation_form = new frm_simulation(); simulation_form.Show(); }

            //Sequence 분석 Tool Front View
            if (e.Alt == true && e.Control == true && e.KeyCode == Keys.V)
            { Program.schematic_form.fpnl_seq_monitor.Visible = true; }
        }
        public void Setting_variable()
        {
            try
            {
                //Serial Data Parse Class 초기화 1회 진행
                SerialData = new class_serial_data_merge();

                SerialData.SCR = new Class_SCR_DPU31A_025A.ST_SCR_DPU31A_025A();
                SerialData.SCR.last_rcv_time_ch1 = DateTime.Now; SerialData.SCR.last_rcv_time_ch2 = DateTime.Now; SerialData.SCR.last_rcv_time_ch3 = DateTime.Now; SerialData.SCR.last_rcv_time_ch4 = DateTime.Now;
                SerialData.TEMP_CONTROLLER = new Class_TempController_M74.ST_TempController_M74();
                SerialData.TEMP_CONTROLLER.last_rcv_time_ch1 = DateTime.Now; SerialData.TEMP_CONTROLLER.last_rcv_time_ch2 = DateTime.Now; SerialData.TEMP_CONTROLLER.last_rcv_time_ch3 = DateTime.Now;
                SerialData.TEMP_CONTROLLER.tank_a = new Class_TempController_M74.ST_TempController_M74.ST_M74_Unit();
                SerialData.TEMP_CONTROLLER.tank_b = new Class_TempController_M74.ST_TempController_M74.ST_M74_Unit();
                SerialData.TEMP_CONTROLLER.ts_09 = new Class_TempController_M74.ST_TempController_M74.ST_M74_Unit();
                SerialData.TEMP_CONTROLLER.supply_a = new Class_TempController_M74.ST_TempController_M74.ST_M74_Unit();
                SerialData.TEMP_CONTROLLER.supply_b = new Class_TempController_M74.ST_TempController_M74.ST_M74_Unit();
                SerialData.TEMP_CONTROLLER.supply_heater_a = new Class_TempController_M74.ST_TempController_M74.ST_M74_Unit();
                SerialData.TEMP_CONTROLLER.supply_heater_b = new Class_TempController_M74.ST_TempController_M74.ST_M74_Unit();
                SerialData.TEMP_CONTROLLER.circulation_heater1 = new Class_TempController_M74.ST_TempController_M74.ST_M74_Unit();
                SerialData.TEMP_CONTROLLER.circulation_heater2 = new Class_TempController_M74.ST_TempController_M74.ST_M74_Unit();

                SerialData.FlowMeter_USF500 = new Class_FlowMeter_USF500.ST_FlowMeter_USF500();
                SerialData.FlowMeter_USF500.last_rcv_time_ch1 = DateTime.Now; SerialData.FlowMeter_USF500.last_rcv_time_ch2 = DateTime.Now;
                SerialData.SUPPLY_A_PUMP_CONTROLLER = new Class_PumpController_PB12.ST_PumpController_PB12();
                SerialData.SUPPLY_B_PUMP_CONTROLLER = new Class_PumpController_PB12.ST_PumpController_PB12();
                //SerialData.TEMP_CONTROLLER = new Class_TempController_M74.ST_TempController_M74();

                st_ui_change_para.subtimer_count = 15;
                st_ui_change_para.timer_initial = false;
                st_ui_change_para.tp_check_interval = new TimeSpan[st_ui_change_para.subtimer_count];
                st_ui_change_para.dt_check_last_act = new DateTime[st_ui_change_para.subtimer_count];

                st_data_operate_para.subtimer_count = 15;
                st_data_operate_para.timer_initial = false;
                st_data_operate_para.tp_check_interval = new TimeSpan[st_data_operate_para.subtimer_count];
                st_data_operate_para.dt_check_last_act = new DateTime[st_data_operate_para.subtimer_count];

                //통신 MSG Retry 관리 함수
                dt_message_manage = new DataTable();
                dt_message_manage = new DataTable("CM_CTC");
                dt_message_manage.Columns.Clear();
                dt_message_manage.Rows.Clear();
                dt_message_manage.Columns.Add("DATETIME_SEND", Type.GetType("System.DateTime"));
                dt_message_manage.Columns.Add("DATETIME_SEND_BY_RETRY", Type.GetType("System.DateTime"));
                dt_message_manage.Columns.Add("DATETIMEKEY", Type.GetType("System.String"));
                dt_message_manage.Columns.Add("TOKEN_ID", Type.GetType("System.Int32"));
                dt_message_manage.Columns.Add("IP", Type.GetType("System.String"));
                dt_message_manage.Columns.Add("PORT", Type.GetType("System.Int32"));
                dt_message_manage.Columns.Add("MSG_NO", Type.GetType("System.UInt32"));
                dt_message_manage.Columns.Add("TRY_CNT", Type.GetType("System.Int32"));
                dt_message_manage.Columns.Add("DATA", Type.GetType("System.Byte[]"));
                dt_message_manage.Columns.Add("COMPLETE", Type.GetType("System.Boolean"));

            }
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".setting_variable." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally { }
        }
        public void Setting_initial()
        {
            string result = "";
            this.SuspendLayout();
            pnl_body.SuspendLayout();
            try
            {
                if (Program.cg_app_info.mode_simulation.use == true)
                {
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_HH].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_HH].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_OVERFLOW_CHECK].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_OVERFLOW_CHECK].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.BOTTOM_VAT_LEAK1].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.Drain_Tank_H].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANK_VAT_LEAK].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.DIKE_LEAK].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.EMS].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.E_RIGHT_DOOR].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.GPS_UPS_TRIP].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.MAIN_EQ_TRIP].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.MAIN_UNIT_MIX_TRIP].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.LEFT_FAN_ALARM].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.RIGHT_FAN_ALARM].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.C_DOOR_ALARM].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.EXH_LL].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.Gas_Detec_Alarm].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.ELEC_DOOR_OVERRIDE].value = false;

                    Program.main_md.user_info.type = Module_User.User_Type.Admin; Program.main_md.user_info.id = "Admin"; Program.main_md.user_info.password = "1";
                    Program.main_md.login(Program.main_md.user_info);
                }
                //form skin register
                //UserLookAndFeel.Default.SkinName = "My Office 2019 Black";
                //if (result != "") { Program.log_md.LogWrite("frm_main.setting_initial." + result, Module_Log.enumLog.DEBUG, ""); }
                //Form Load 사전에 로드
                Console.WriteLine("Form Start : " + DateTime.Now.ToString("HH:mm:ss.fff"));
                Program.main_md.FormShow(Program.io_monitor_form, this.pnl_body);
                //Console.WriteLine("FORM 2" + DateTime.Now.ToString("HH:mm:fff"));
                Program.main_md.FormShow(Program.communications_form, this.pnl_body);
                //Console.WriteLine("FORM 3" + DateTime.Now.ToString("HH:mm:fff"));
                Program.main_md.FormShow(Program.alarm_form, this.pnl_body);
                //Console.WriteLine("FORM 4" + DateTime.Now.ToString("HH:mm:fff"));
                Program.main_md.FormShow(Program.parameter_form, this.pnl_body);
                //Console.WriteLine("FORM 5" + DateTime.Now.ToString("HH:mm:fff"));
                Program.main_md.FormShow(Program.mixing_step_form, this.pnl_body);
                //Console.WriteLine("FORM 6" + DateTime.Now.ToString("HH:mm:fff"));
                Program.main_md.FormShow(Program.trendlog_form, this.pnl_body);
                //Console.WriteLine("FORM 7" + DateTime.Now.ToString("HH:mm:fff"));
                Program.main_md.FormShow(Program.alarmlog_form, this.pnl_body);
                //Console.WriteLine("FORM 8" + DateTime.Now.ToString("HH:mm:fff"));
                Program.main_md.FormShow(Program.eventlog_form, this.pnl_body);
                //Console.WriteLine("FORM 9" + DateTime.Now.ToString("HH:mm:fff"));
                Program.main_md.FormShow(Program.totalusagelog_form, this.pnl_body);
                //Console.WriteLine("FORM 10" + DateTime.Now.ToString("HH:mm:fff"));
                Program.main_md.FormShow(Program.schematic_form, this.pnl_body);
                Console.WriteLine("Form End : " + DateTime.Now.ToString("HH:mm:ss.fff"));
                //btn_monitor.PerformClick();

                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    pnl_abb_status_main.Visible = true;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    pnl_abb_status_main.Visible = false;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    pnl_abb_status_main.Visible = false;
                    pnl_status_tank_b.Visible = false;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    pnl_abb_status_main.Visible = false;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    pnl_abb_status_main.Visible = false;
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RECLAIM_DRAIN, true);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    pnl_abb_status_main.Visible = false;
                }
                //버전 정보 표기
                lbl_app_version.Text = Application.ProductVersion.ToString();
                //Chemical 사용 정보 표기
                if (Program.cg_app_info.mode_simulation.use == true)
                {
                    lbl_title.Text = " CDS-" + Program.cg_app_info.eq_type.ToString().ToUpper();// + "(" + "Simul" + ")";
                }
                else
                {
                    lbl_title.Text = " CDS-" + Program.cg_app_info.eq_type.ToString().ToUpper();
                }
                //User Combobox Index 설정
                if (Program.popup_login.cmb_user_id.Properties.Items.Count > 0) { Program.popup_login.cmb_user_id.SelectedIndex = 0; }
                timer_checkthread.Interval = 1000; timer_checkthread.Enabled = true;
                timer_uichange.Interval = 100; timer_uichange.Enabled = true;
                timer_alarm_check.Interval = 200; timer_alarm_check.Enabled = true;
            }
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".setting_initial." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally
            {
                this.ResumeLayout();
                pnl_body.ResumeLayout();
            }

        }
        public void setting_dispose(bool req_by_user)
        {
            try
            {
                if (req_by_user == true) { Program.log_md.LogWrite("APP END", Module_Log.enumLog.App_Info, "User Power Off", Module_Log.enumLevel.ALWAYS); }
                LogManager_Kill();
                Program.schematic_form.SUPPLY_A_PUMP_ON_OFF(false);
                Program.schematic_form.SUPPLY_B_PUMP_ON_OFF(false);
                Program.schematic_form.CIRCULATION_PUMP_ON_OFF(false);
                Program.seq.monitoring.use_auto_drain = false;
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Pump_On, false);

                Program.schematic_form.CIRCULATION_1_HEATER_ON_OFF(false);
                Program.schematic_form.SUPPLY_A_HEATER_ON_OFF(false);
                Program.schematic_form.SUPPLY_B_HEATER_ON_OFF(false);

                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.BUZZER, false);

                //timer & thread disable
                Program.log_md.LogWrite("APP END", Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                Program.log_md.LogWrite("APP END", Module_Log.enumLog.SEQ_MAIN, "", Module_Log.enumLevel.ALWAYS);
                Program.log_md.LogWrite("APP END", Module_Log.enumLog.SEQ_SUPPLY, "", Module_Log.enumLevel.ALWAYS);
                Program.log_md.LogWrite("APP END", Module_Log.enumLog.SEQ_PUMP_CONTROL, "", Module_Log.enumLevel.ALWAYS);
                Program.log_md.LogWrite("APP END", Module_Log.enumLog.SEQ_SEMI_AUTO, "", Module_Log.enumLevel.ALWAYS);
                Program.log_md.LogWrite("APP END", Module_Log.enumLog.SEQ_SEMI_AUTO_A, "", Module_Log.enumLevel.ALWAYS);
                Program.log_md.LogWrite("APP END", Module_Log.enumLog.SEQ_SEMI_AUTO_B, "", Module_Log.enumLevel.ALWAYS);

                System.Threading.Thread.Sleep(1000);
                timer_checkthread.Enabled = false;
                timer_uichange.Enabled = false;
                timer_alarm_check.Enabled = false;
                if (thd_data_operate != null) { thd_data_operate.Abort(); thd_data_operate = null; }
                if (thd_socket_operate != null) { thd_socket_operate.Abort(); thd_socket_operate = null; }
                if (thd_ethercat_commi_rcv_ad != null) { thd_ethercat_commi_rcv_ad.Abort(); thd_ethercat_commi_rcv_ad = null; }
                if (thd_Rcv_Send_ABB != null) { thd_Rcv_Send_ABB.Abort(); thd_Rcv_Send_ABB = null; }
                if (thd_Rcv_Send_HeatExchanger != null) { thd_Rcv_Send_HeatExchanger.Abort(); thd_Rcv_Send_HeatExchanger = null; }
                for (int idx = 0; idx < Program.IO.SERIAL.use_cnt; idx++)
                {
                    if (thd_ethercat_commi_rcv_send_serial[idx] != null) { thd_ethercat_commi_rcv_send_serial[idx].Abort(); thd_ethercat_commi_rcv_send_serial[idx] = null; }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Program.LOG.close();
                if (Application.MessageLoop == true)
                {
                    Application.ExitThread();
                    Application.Exit();
                    Environment.Exit(1);
                }
                else
                {
                    Environment.Exit(1);
                }
            }
        }
        private void timer_uichange_Tick(object sender, EventArgs e)
        {
            UI_Change();
        }
        private void timer_checkthread_Tick(object sender, EventArgs e)
        {
            Check_thread();
        }
        private void timer_alarm_check_Tick(object sender, EventArgs e)
        {
            Alarm_Check();
        }
        public void Check_thread()
        {
            //return;
            if (Program.cg_apploading.load_complete == true)
            {
                if (thd_data_operate != null) { if (thd_data_operate.IsAlive == false) { thd_data_operate.Abort(); thd_data_operate = null; thd_data_operate = new Thread(Data_Operate); thd_data_operate.Start(); } }
                else { thd_data_operate = new Thread(Data_Operate); thd_data_operate.Start(); }

                if (thd_socket_operate != null) { if (thd_socket_operate.IsAlive == false) { thd_socket_operate.Abort(); thd_socket_operate = null; thd_socket_operate = new Thread(socket_operate); thd_socket_operate.Start(); } }
                else { thd_socket_operate = new Thread(socket_operate); thd_socket_operate.Start(); }

                if (thd_ethercat_commi_rcv_ad != null) { if (thd_ethercat_commi_rcv_ad.IsAlive == false) { thd_ethercat_commi_rcv_ad.Abort(); thd_ethercat_commi_rcv_ad = null; thd_ethercat_commi_rcv_ad = new Thread(Ethercat_commi_Rcv_AD); thd_ethercat_commi_rcv_ad.Start(); } }
                else { thd_ethercat_commi_rcv_ad = new Thread(Ethercat_commi_Rcv_AD); thd_ethercat_commi_rcv_ad.Start(); }


                if (Program.cg_socket.message_retry_cnt != 0)
                {
                    if (thd_CTC_Message_Manager != null) { if (thd_CTC_Message_Manager.IsAlive == false) { thd_CTC_Message_Manager.Abort(); thd_CTC_Message_Manager = null; thd_CTC_Message_Manager = new Thread(CTC_Message_Manager); thd_CTC_Message_Manager.Start(); } }
                    else { thd_CTC_Message_Manager = new Thread(CTC_Message_Manager); thd_CTC_Message_Manager.Start(); }
                }

                //APM만 ABB310 사용 + 2023-06-27 DSP ABB 농도계 추가됨
                if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    if (thd_Rcv_Send_ABB != null) { if (thd_Rcv_Send_ABB.IsAlive == false) { thd_Rcv_Send_ABB.Abort(); thd_Rcv_Send_ABB = null; thd_Rcv_Send_ABB = new Thread(Send_ABB); thd_Rcv_Send_ABB.Start(); } }
                    else { thd_Rcv_Send_ABB = new Thread(Send_ABB); thd_Rcv_Send_ABB.Start(); }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    if (thd_Rcv_Send_HeatExchanger != null) { if (thd_Rcv_Send_HeatExchanger.IsAlive == false) { thd_Rcv_Send_HeatExchanger.Abort(); thd_Rcv_Send_HeatExchanger = null; thd_Rcv_Send_HeatExchanger = new Thread(Send_Heat_Exchanger); thd_Rcv_Send_HeatExchanger.Start(); } }
                    else { thd_Rcv_Send_HeatExchanger = new Thread(Send_Heat_Exchanger); thd_Rcv_Send_HeatExchanger.Start(); }
                }
                if (Program.cg_app_info.mode_simulation.use == false)
                {
                    for (int idx = 0; idx < Program.IO.SERIAL.use_cnt; idx++)
                    {
                        var thread_index = idx;
                        if (idx < Program.IO.SERIAL.use_cnt)
                        {
                            if (thd_ethercat_commi_rcv_send_serial[idx] != null)
                            {

                                if (thd_ethercat_commi_rcv_send_serial[idx].IsAlive == false)
                                {
                                    thd_ethercat_commi_rcv_send_serial[idx].Abort(); thd_ethercat_commi_rcv_send_serial[idx] = null;
                                    thd_ethercat_commi_rcv_send_serial[idx] = new Thread(() => Ethercat_commi_Rcv_Send_Serial(thread_index)); thd_ethercat_commi_rcv_send_serial[idx].Start();
                                }
                            }
                            else { thd_ethercat_commi_rcv_send_serial[idx] = new Thread(() => Ethercat_commi_Rcv_Send_Serial(thread_index)); thd_ethercat_commi_rcv_send_serial[idx].Start(); }
                        }
                    }
                }
            }
        }
        public void CTC_Message_Manager()
        {
            TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info connection_info = new TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info();
            UInt32 msg_no = 0;
            Ping ping = new Ping();
            PingOptions options = new PingOptions();
            PingReply reply;
            try
            {
                while (true)
                {
                    if (Program.CTC.run_state == true)
                    {
                        while (true)
                        {
                            if (queue_delete_manage.Count > 0)
                            {
                                queue_delete_manage.TryDequeue(out msg_no);
                                DataRow[] dataRowArray = Program.main_form.dt_message_manage.Select("msg_no = '" + msg_no + "'");
                                if (dataRowArray != null && dataRowArray.Length > 0)
                                {
                                    dataRowArray[0]["COMPLETE"] = true;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (dt_message_manage.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dt_message_manage.Rows[0]["COMPLETE"]) == true)
                            {
                                //Console.WriteLine("REMOVE : " + dt_message_manage.Rows[0]["TOKEN_ID"]);
                                dt_message_manage.Rows[0].Delete();
                                dt_message_manage.AcceptChanges();
                            }
                            else
                            {
                                if (Convert.ToInt32(dt_message_manage.Rows[0]["TRY_CNT"]) >= Program.cg_socket.message_retry_cnt)
                                {
                                    //Console.WriteLine("REMOVE : " + dt_message_manage.Rows[0]["TOKEN_ID"]);
                                    dt_message_manage.Rows[0].Delete();
                                    dt_message_manage.AcceptChanges();
                                    //Alarm 발생
                                }
                                else
                                {
                                    if ((DateTime.Now - Convert.ToDateTime(dt_message_manage.Rows[0]["DATETIME_SEND_BY_RETRY"])).TotalMilliseconds >= Program.cg_socket.message_retry_interval)
                                    {
                                        dt_message_manage.Rows[0]["TRY_CNT"] = Convert.ToInt32(dt_message_manage.Rows[0]["TRY_CNT"]) + 1;
                                        dt_message_manage.Rows[0]["DATETIME_SEND_BY_RETRY"] = DateTime.Now;
                                        connection_info.ip = dt_message_manage.Rows[0]["IP"].ToString();
                                        connection_info.port = Convert.ToInt32(dt_message_manage.Rows[0]["PORT"]);
                                        Program.SOCKET.SendMsg(connection_info, (byte[])dt_message_manage.Rows[0]["DATA"]);
                                        Program.main_md.Save_ByteArray_Send_Log("Re-Send(" + dt_message_manage.Rows[0]["TRY_CNT"] + ") -> " + DateTime.Now.ToString("HH:mm:ss.fff : ")
                                            + "(" + Convert.ToInt32(dt_message_manage.Rows[0]["TOKEN_ID"]).ToString() + "),"
                                            + ", " + connection_info.ip + ":" + connection_info.port, (byte[])dt_message_manage.Rows[0]["DATA"]);
                                    }
                                }
                            }

                        }
                        System.Threading.Thread.Sleep(10);
                    }
                    else
                    {

                        try
                        {
                            System.Threading.Thread.Sleep(2000);
                            reply = ping.Send("192.168.0.100", 1000);
                            if (reply.Status != IPStatus.Success)
                            {
                                Program.log_md.LogWrite("192.168.0.100 Ping Fail : " + reply.Status.ToString(), Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);
                            }
                            else
                            {
                                Program.log_md.LogWrite("192.168.0.100 Ping OK :", Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);
                            }
                        }
                        catch (Exception ex)
                        { Program.log_md.LogWrite("192.168.0.100 Ping Fail : " + ex.ToString(), Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS); }
                        finally
                        {
                            Program.log_md.LogWrite("Socket Info" + Program.SOCKET.Socket_Info(), Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);
                        }

                    }

                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".CTC_Message_Manager." + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        public void Data_Operate()
        {
            try
            {
                if (st_data_operate_para.timer_initial == false)
                {
                    for (int index = 0; index < st_data_operate_para.subtimer_count; index++)
                    { st_data_operate_para.tp_check_interval[index] = new TimeSpan(0); st_data_operate_para.dt_check_last_act[index] = DateTime.Now.AddDays(-1); }
                    st_data_operate_para.timer_initial = true;
                }
                //

                //
                while (true)
                {
                    //System.Threading.Thread.Sleep(2000);
                    //Program.IO.DI.Tag[(int)Config_IO.enum_di.EMS].value = true;
                    //System.Threading.Thread.Sleep(2000);
                    //Program.IO.DI.Tag[(int)Config_IO.enum_di.ELEC_DOOR_OVERRIDE].value = true;
                    //System.Threading.Thread.Sleep(2000);
                    //Program.IO.DI.Tag[(int)Config_IO.enum_di.C_DOOR_ALARM].value = true;
                    //System.Threading.Thread.Sleep(2000);
                    //Program.IO.DI.Tag[(int)Config_IO.enum_di.Drain_Tank_H].value = true;
                    //System.Threading.Thread.Sleep(2000);

                    //Program.IO.DI.Tag[(int)Config_IO.enum_di.EMS].value = false;
                    //Program.IO.DI.Tag[(int)Config_IO.enum_di.ELEC_DOOR_OVERRIDE].value = false;
                    //Program.IO.DI.Tag[(int)Config_IO.enum_di.C_DOOR_ALARM].value = false;
                    //Program.IO.DI.Tag[(int)Config_IO.enum_di.Drain_Tank_H].value = false;
                    //System.Threading.Thread.Sleep(5000);
                    if (Program.cg_apploading.load_complete == false)
                    {
                    }
                    else
                    {
                        for (int index = 0; index < st_data_operate_para.subtimer_count; index++)
                        {
                            st_data_operate_para.tp_check_interval[index] = DateTime.Now - st_data_operate_para.dt_check_last_act[index];
                            switch (index)
                            {

                                case 0: //Log Manager Application 실행 감지 / Serial Daemon 실행 감지
                                    if (st_data_operate_para.tp_check_interval[index].TotalMinutes >= 1)
                                    {
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        if (Program.cg_app_info.internal_info.log_manager_run == true)
                                        {
                                            APP_LOG_MANAGER_STATE = Check_Logmanager();
                                        }
                                        APP_SERIAL_DAEMON_STATE = Check_Comi_Serial_Daemon();
                                        //Program.log_md.terminal_log("OK", Module_Log.enumTerminal.None);
                                    }
                                    break;

                                case 1: //프로그램 상태 기록
                                    if (st_data_operate_para.tp_check_interval[index].TotalMilliseconds >= 60000)
                                    {
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        Program.cg_app_info.internal_info.usage_app = Program.main_md.GetMemoryUsage(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                                        Program.log_md.LogWrite(this.Name + "." + "Program Memory : " + Program.cg_app_info.internal_info.usage_app.ToString() + "Mbyte", Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                                    }
                                    break;

                                case 2: //드라이브 상태 기록
                                    if (st_data_operate_para.tp_check_interval[index].TotalMilliseconds >= 60000)
                                    {
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        Program.cg_app_info.internal_info.usage_drive_c = Program.main_md.GetTotalFreeSpace(@"C:\") * 0.000001;
                                        Program.cg_app_info.internal_info.usage_drive_d = Program.main_md.GetTotalFreeSpace(@"D:\") * 0.000001;

                                        Program.log_md.LogWrite(this.Name + "." + "C: Memory : " + Program.cg_app_info.internal_info.usage_drive_c.ToString() + "Gbyte", Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                                        Program.log_md.LogWrite(this.Name + "." + "D: Memory : " + Program.cg_app_info.internal_info.usage_drive_d.ToString() + "Gbyte", Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                                    }
                                    break;

                                case 3:
                                    if (st_data_operate_para.tp_check_interval[index].TotalMilliseconds >= 1000)
                                    {
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        //Program.log_md.terminal_log("OK", Module_Log.enumTerminal.None);
                                    }
                                    break;
                                case 4: //CDS -> CTC Command Send Excange Req(Manual, User)
                                    if (st_data_operate_para.tp_check_interval[index].TotalMilliseconds >= 100)
                                    {
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        if (Program.seq.manual_exchange_req_by_user == true)
                                        {
                                            Program.seq.manual_exchange_req_by_user = false;
                                            if (Program.cg_app_info.mode_simulation.use == true)
                                            {
                                                Program.seq.manual_exchange_ack_by_ctc = true;
                                            }
                                        }
                                    }
                                    break;

                                case 5:
                                    if (st_data_operate_para.tp_check_interval[index].TotalMilliseconds >= 500)
                                    {
                                        //CDS 내부 로그 저장
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        CDS_Data_Log();
                                    }
                                    break;

                                case 6: //CDS -> CTC No Process Reqeust
                                    if (st_data_operate_para.tp_check_interval[index].TotalMilliseconds >= 100)
                                    {
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY)
                                        {
                                            if (CDS_enable_to_ctc == false)
                                            {
                                                CDS_enable_to_ctc = true;
                                                Program.CTC.Message_CDS_Disable_Event_451(false);
                                            }
                                        }
                                        else
                                        {
                                            if (CDS_enable_to_ctc == true)
                                            {
                                                CDS_enable_to_ctc = false;
                                                //Program.CTC.Message_CDS_Disable_Event_451();
                                            }
                                            //APP 로드 후 초기 1회만 전달
                                            if (CDS_enable_to_ctc_app_load == false)
                                            {
                                                CDS_enable_to_ctc_app_load = true;
                                                //Program.CTC.Message_CDS_Enable_Event_450();
                                            }
                                        }

                                        if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY || Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.LIGHT)
                                        {
                                            if (CDS_process_stop_to_ctc == false)
                                            {
                                                CDS_process_stop_to_ctc = true;

                                                Program.CTC.Message_No_Process_Request_Event_454();
                                            }
                                        }
                                        else
                                        {
                                            if (CDS_process_stop_to_ctc == true)
                                            {
                                                CDS_process_stop_to_ctc = false;
                                                Program.CTC.Message_No_Process_Request_Cancel_Event_455();
                                            }
                                        }

                                    }
                                    break;

                                case 7: //CDS -> CTC Auto Mode, Manual Mode Event Send
                                    if (st_data_operate_para.tp_check_interval[index].TotalMilliseconds >= 500)
                                    {
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                                        {
                                            if (CDS_auto_mode == false)
                                            {
                                                CDS_auto_mode = true;
                                                Program.CTC.Message_Auto_Mode_Event_456();
                                            }
                                            Program.schematic_form.circulation_pump_run_interval = 0;
                                        }
                                        else
                                        {
                                            CDS_auto_mode = false;
                                        }

                                        if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                                        {
                                            if (CDS_manual_mode == false)
                                            {
                                                CDS_manual_mode = true;
                                                Program.CTC.Message_Manual_Mode_Event_457();
                                                if (Program.seq.supply.CDS_enable_status_to_ctc == true) { Program.CTC.Message_CDS_Disable_Event_451(false); }
                                            }
                                            Program.schematic_form.circulation_pump_run_interval = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Pump_Run_Interval);
                                            if (Program.schematic_form.circulation_pump_run_interval != 0)
                                            {
                                                if (Program.schematic_form.circulation_pump_run_interval < 500) { Program.schematic_form.circulation_pump_run_interval = 500; }
                                            }
                                        }
                                        else
                                        {
                                            CDS_manual_mode = false;
                                        }

                                    }
                                    break;

                                case 8:
                                    if (st_data_operate_para.tp_check_interval[index].TotalMilliseconds >= 1000)
                                    {
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        //Program.schematic_form.Concentration_Check(tank_class.enum_tank_type.NONE, false);
                                    }
                                    break;
                                case 9:
                                    if (st_data_operate_para.tp_check_interval[index].TotalMinutes >= 10)
                                    {
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        Calculation_Daily_Usage();
                                        Get_Computer_Data();
                                    }
                                    break;
                                case 10:
                                    if (Program.cg_socket.trace_send_interval != 0)
                                    {
                                        if (Program.cg_socket.trace_send_interval <= 500) { Program.cg_socket.trace_send_interval = 500; }
                                        if (st_data_operate_para.tp_check_interval[index].TotalMilliseconds >= Program.cg_socket.trace_send_interval)
                                        {
                                            st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                            Program.CTC.Message_FDC_Data_Send_106();
                                        }
                                    }


                                    break;
                            }

                        }
                    }

                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".data_operate." + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        public void Get_Computer_Data()
        {
            Double temperature = 0;
            string instanceName = "";
            string fan_speed_cpu = "";
            string fan_speed_mainboard = "";
            string fan_speed_other = "";
            string log = "";
            Computer computerHardware = new Computer();
            try
            {

                // Module_OpenHardware.PCInformation.Instance.DataUpdate();

                computerHardware.CPUEnabled = true;
                computerHardware.MainboardEnabled = true;
                computerHardware.FanControllerEnabled = true;
                computerHardware.RAMEnabled = true;
                computerHardware.HDDEnabled = true;
                computerHardware.Open();
                Program.main_form.hardware_info = "";
                foreach (var hardware in computerHardware.Hardware)
                {
                    // This will be in the mainboard
                    hardware.Update();
                    foreach (var sensor in hardware.Sensors)
                    {
                        Program.main_form.hardware_info = Program.main_form.hardware_info + " ## " + $"{sensor.Name} / {sensor.SensorType} / Data: {sensor.Value} ";
                    }
                }

                log = "PC INFO : " + hardware_info;
                Program.log_md.LogWrite(log, Module_Log.enumLog.RESOURCE_2, "", Module_Log.enumLevel.ALWAYS);

                ManagementObjectSearcher searcher_temp = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                foreach (ManagementObject obj in searcher_temp.Get())
                {
                    temperature = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                    // Convert the value to celsius degrees
                    temperature = (temperature - 2732) / 10.0;
                    instanceName = obj["InstanceName"].ToString();
                    cpu_temp = (float)temperature;
                }

            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".Get_Computer_Data." + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { if (computerHardware != null) { computerHardware.Close(); computerHardware = null; } }

        }
        public void Calculation_Daily_Usage()
        {
            string query = "";
            DataSet ds_total_usage = new DataSet();
            string err = "";
            try
            {
                for (int idx_ccss = 0; idx_ccss < 4; idx_ccss++)
                {
                    if (ds_total_usage != null) { ds_total_usage.Clear(); ds_total_usage.Dispose(); ds_total_usage = new DataSet(); }
                    query = "SELECT DATE(`totalusage_saved_time`) AS `date`,sum(`totalusage_value`) AS 'sum'" + System.Environment.NewLine;
                    query += "FROM totalusage_logs" + System.Environment.NewLine;
                    query += "WHERE totalusage_index_ccss = '" + idx_ccss + "'" + System.Environment.NewLine;
                    query += "GROUP BY `date`" + System.Environment.NewLine;
                    query += "ORDER BY `date` DESC" + System.Environment.NewLine;
                    Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_local_cds.connection, query, ds_total_usage, ref err);

                    if (ds_total_usage.Tables.Count >= 0 && ds_total_usage.Tables[0].Rows.Count > 0)
                    {
                        if (DateTime.Now.ToString("yyyy-MM-dd") == Convert.ToDateTime(ds_total_usage.Tables[0].Rows[0]["date"]).ToString("yyyy-MM-dd"))
                        {
                            if (idx_ccss == 0) { Program.schematic_form.TotalUsage_By_Daily_CCSS1 = Convert.ToSingle(ds_total_usage.Tables[0].Rows[0]["sum"]); }
                            else if (idx_ccss == 1) { Program.schematic_form.TotalUsage_By_Daily_CCSS2 = Convert.ToSingle(ds_total_usage.Tables[0].Rows[0]["sum"]); }
                            else if (idx_ccss == 2) { Program.schematic_form.TotalUsage_By_Daily_CCSS3 = Convert.ToSingle(ds_total_usage.Tables[0].Rows[0]["sum"]); }
                            else if (idx_ccss == 3) { Program.schematic_form.TotalUsage_By_Daily_CCSS4 = Convert.ToSingle(ds_total_usage.Tables[0].Rows[0]["sum"]); }
                        }
                        else
                        {
                            if (idx_ccss == 0) { Program.schematic_form.TotalUsage_By_Daily_CCSS1 = 0; }
                            else if (idx_ccss == 1) { Program.schematic_form.TotalUsage_By_Daily_CCSS2 = 0; }
                            else if (idx_ccss == 2) { Program.schematic_form.TotalUsage_By_Daily_CCSS3 = 0; }
                            else if (idx_ccss == 3) { Program.schematic_form.TotalUsage_By_Daily_CCSS4 = 0; }
                        }
                    }
                    else
                    {
                        if (idx_ccss == 0) { Program.schematic_form.TotalUsage_By_Daily_CCSS1 = 0; }
                        else if (idx_ccss == 1) { Program.schematic_form.TotalUsage_By_Daily_CCSS2 = 0; }
                        else if (idx_ccss == 2) { Program.schematic_form.TotalUsage_By_Daily_CCSS3 = 0; }
                        else if (idx_ccss == 3) { Program.schematic_form.TotalUsage_By_Daily_CCSS4 = 0; }
                    }
                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".Calculation_Daily_Usage." + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { if (ds_total_usage != null) { ds_total_usage.Clear(); ds_total_usage.Dispose(); } }
        }
        public Boolean Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm enum_alarm, Config_IO.Config_Digial_Tag di_tag)
        {
            Boolean result = false;
            Config_Alarm config_alarm = null;
            var alarm_contents = config_alarm;
            alarm_contents = Program.alarm_list.Return_Object_by_ID((int)enum_alarm);
            if (alarm_contents == null) { return false; }
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                //di_tag.value = test_di;
            }

            if (alarm_contents != null)
            {
                if (di_tag.unit == "N.C")
                {
                    if (di_tag.value == true)
                    {
                        if ((DateTime.Now - alarm_contents.last_off_time).TotalSeconds >= alarm_contents.wdt)
                        {
                            alarm_contents.occurred_time = DateTime.Now;
                            if (alarm_contents.enable == 1)
                            {
                                Program.alarm_form.Insert_Alarm_Contents(alarm_contents, true);
                            }
                            else
                            {
                                if (alarm_contents.clear_request == true)
                                {
                                    //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                                    alarm_contents.clear_request = false;
                                    alarm_contents.cleared_by = 0;
                                    alarm_contents.cleared_time = DateTime.Now;
                                    Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                                }
                            }
                        }
                        else
                        {

                        }
                        if (alarm_contents.clear_request == true)
                        {
                            alarm_contents.clear_request = false; //Alarm이 On일때 Alarm Clear 신호 무시
                        }
                    }
                    else
                    {
                        //Program.IO.DI.Tag[(int)enum_di].last_off_time = DateTime.Now;
                        alarm_contents.last_off_time = DateTime.Now;
                        if (alarm_contents.clear_request == true)
                        {
                            //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                            alarm_contents.clear_request = false;
                            alarm_contents.cleared_by = 0;
                            alarm_contents.cleared_time = DateTime.Now;
                            Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                        }

                    }
                }
                else if (di_tag.unit == "N.O")
                {
                    if (di_tag.value == true)
                    {
                        if ((DateTime.Now - alarm_contents.last_off_time).TotalSeconds >= alarm_contents.wdt)
                        {
                            alarm_contents.occurred_time = DateTime.Now;
                            if (alarm_contents.enable == 1)
                            {
                                Program.alarm_form.Insert_Alarm_Contents(alarm_contents, true);
                            }
                            else
                            {
                                if (alarm_contents.clear_request == true)
                                {
                                    //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                                    alarm_contents.clear_request = false;
                                    alarm_contents.cleared_by = 0;
                                    alarm_contents.cleared_time = DateTime.Now;
                                    Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                                }
                            }
                        }
                        else
                        {

                        }
                        if (alarm_contents.clear_request == true)
                        {
                            alarm_contents.clear_request = false; //Alarm이 On일때 Alarm Clear 신호 무시
                        }
                    }
                    else
                    {
                        //Program.IO.DI.Tag[(int)enum_di].last_off_time = DateTime.Now;
                        alarm_contents.last_off_time = DateTime.Now;
                        if (alarm_contents.clear_request == true)
                        {
                            //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                            alarm_contents.clear_request = false;
                            alarm_contents.cleared_by = 0;
                            alarm_contents.cleared_time = DateTime.Now;
                            Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                        }

                    }
                }

            }




            return result;

        }
        public Boolean Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm enum_alarm, double ai_value, double min, bool force_clear)
        {
            Boolean result = false;
            Config_Alarm config_alarm = null;
            var alarm_contents = config_alarm;
            alarm_contents = Program.alarm_list.Return_Object_by_ID((int)enum_alarm);
            if (alarm_contents == null) { return false; }
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                //di_tag.value = test_di;
            }
            if (force_clear == true)
            {
                alarm_contents.last_off_time = DateTime.Now;
                if (alarm_contents.clear_request == true || alarm_contents.enable == 0)
                {
                    alarm_contents.clear_request = false;
                    alarm_contents.cleared_by = 0;
                    alarm_contents.cleared_time = DateTime.Now;
                    Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                }
                return false;

            }
            if (ai_value < min)
            {
                if ((DateTime.Now - alarm_contents.last_off_time).TotalSeconds >= alarm_contents.wdt)
                {
                    alarm_contents.occurred_time = DateTime.Now;
                    if (alarm_contents.enable == 1)
                    {
                        alarm_contents.remark = "Min : " + min + ", Value : " + ai_value;
                        Program.alarm_form.Insert_Alarm_Contents(alarm_contents, true);
                    }
                    else
                    {
                        if (alarm_contents.clear_request == true)
                        {
                            //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                            alarm_contents.clear_request = false;
                            alarm_contents.cleared_by = 0;
                            alarm_contents.cleared_time = DateTime.Now;
                            Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                        }
                    }
                }
                else
                {

                }
                if (alarm_contents.clear_request == true)
                {
                    alarm_contents.clear_request = false; //Alarm이 On일때 Alarm Clear 신호 무시
                }
            }
            else
            {
                //Program.IO.DI.Tag[(int)enum_di].last_off_time = DateTime.Now;
                alarm_contents.last_off_time = DateTime.Now;
                if (alarm_contents.clear_request == true)
                {
                    //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                    alarm_contents.clear_request = false;
                    alarm_contents.cleared_by = 0;
                    alarm_contents.cleared_time = DateTime.Now;
                    Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                }

            }

            return result;

        }
        public Boolean Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm enum_alarm, double ai_value, double max, bool force_clear)
        {
            Boolean result = false;
            Config_Alarm config_alarm = null;
            var alarm_contents = config_alarm;
            alarm_contents = Program.alarm_list.Return_Object_by_ID((int)enum_alarm);
            if (alarm_contents == null) { return false; }
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                //di_tag.value = test_di;
            }
            if (force_clear == true)
            {
                alarm_contents.last_off_time = DateTime.Now;
                if (alarm_contents.enable == 0) { alarm_contents.clear_request = true; }
                if (alarm_contents.clear_request == true)
                {
                    alarm_contents.clear_request = false;
                    alarm_contents.cleared_by = 0;
                    alarm_contents.cleared_time = DateTime.Now;
                    Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                }
                return false;
            }
            if (ai_value > max)
            {
                if ((DateTime.Now - alarm_contents.last_off_time).TotalSeconds >= alarm_contents.wdt)
                {
                    alarm_contents.occurred_time = DateTime.Now;
                    if (alarm_contents.enable == 1)
                    {
                        alarm_contents.remark = "Max : " + max + ", Value : " + ai_value;
                        Program.alarm_form.Insert_Alarm_Contents(alarm_contents, true);
                    }
                    else
                    {
                        if (alarm_contents.clear_request == true)
                        {
                            //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                            alarm_contents.clear_request = false;
                            alarm_contents.cleared_by = 0;
                            alarm_contents.cleared_time = DateTime.Now;
                            Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                        }
                    }
                }
                else
                {

                }
                if (alarm_contents.clear_request == true)
                {
                    alarm_contents.clear_request = false; //Alarm이 On일때 Alarm Clear 신호 무시
                }
            }
            else
            {
                //Program.IO.DI.Tag[(int)enum_di].last_off_time = DateTime.Now;
                alarm_contents.last_off_time = DateTime.Now;
                if (alarm_contents.clear_request == true)
                {
                    //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                    alarm_contents.clear_request = false;
                    alarm_contents.cleared_by = 0;
                    alarm_contents.cleared_time = DateTime.Now;
                    Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                }

            }

            return result;

        }
        public Boolean Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm enum_alarm, bool value, string remark)
        {
            Boolean result = false;
            Config_Alarm config_alarm = null;
            var alarm_contents = config_alarm;
            alarm_contents = Program.alarm_list.Return_Object_by_ID((int)enum_alarm);
            if (alarm_contents == null) { return false; }
            if (Program.cg_app_info.mode_simulation.use == true)
            {
            }
            if (value == true)
            {
                if ((DateTime.Now - alarm_contents.last_off_time).TotalSeconds >= alarm_contents.wdt)
                {
                    alarm_contents.occurred_time = DateTime.Now;
                    if (alarm_contents.enable == 1)
                    {
                        Program.alarm_form.Insert_Alarm_Contents(alarm_contents, true);
                    }
                    else
                    {
                        if (alarm_contents.clear_request == true)
                        {
                            //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                            alarm_contents.clear_request = false;
                            alarm_contents.cleared_by = 0;
                            alarm_contents.cleared_time = DateTime.Now;
                            Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                        }
                    }
                }
                else
                {

                }
                if (alarm_contents.clear_request == true)
                {
                    alarm_contents.clear_request = false; //Alarm이 On일때 Alarm Clear 신호 무시
                }
            }
            else
            {
                //Program.IO.DI.Tag[(int)enum_di].last_off_time = DateTime.Now;
                alarm_contents.last_off_time = DateTime.Now;
                if (alarm_contents.clear_request == true)
                {
                    //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                    alarm_contents.clear_request = false;
                    alarm_contents.cleared_by = 0;
                    alarm_contents.cleared_time = DateTime.Now;
                    Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                }

            }
            return result;

        }
        public Boolean Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm enum_alarm)
        {
            Boolean result = false;
            Config_Alarm config_alarm = null;
            var alarm_contents = config_alarm;
            alarm_contents = Program.alarm_list.Return_Object_by_ID((int)enum_alarm);
            if (alarm_contents == null) { return false; }
            if (Program.cg_app_info.mode_simulation.use == true)
            {
            }
            if (alarm_contents.thread_call == true)
            {
                alarm_contents.thread_call = false;
                //auto_thread_call_false = false
                //내부 Seqeunce 또는 Thread에서는 단발성으로 호출될 때 Alarm Clear를 위함
                //내부 Sequence 또는 Thread에서 지속적으로 호출할 때 Alarm Clear를 하지 않기 위해 사용
                //=> 해당 Thread Call을 수동으로 False(Alarm 해제 조건이 OK 일 때) 할 때까지 Alarm이 활성화됨

                if ((DateTime.Now - alarm_contents.last_off_time).TotalSeconds >= alarm_contents.wdt)
                {
                    alarm_contents.occurred_time = DateTime.Now;
                    if (alarm_contents.auto_thread_call_keep_alarm_disable == false)
                    {
                        if (alarm_contents.alarm_alive == true)
                        {
                            if (alarm_contents.enable == 1)
                            {
                                Program.alarm_form.Insert_Alarm_Contents(alarm_contents, true);
                            }
                            else
                            {
                                if (alarm_contents.clear_request == true)
                                {
                                    alarm_contents.clear_request = false;
                                    alarm_contents.cleared_by = 0;
                                    alarm_contents.cleared_time = DateTime.Now;
                                    Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                                }
                            }
                        }
                        else
                        {
                            if (alarm_contents.clear_request == true)
                            {
                                alarm_contents.clear_request = false;
                                alarm_contents.cleared_by = 0;
                                alarm_contents.cleared_time = DateTime.Now;
                                Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                            }
                        }
                    }
                    else
                    {
                        if (alarm_contents.enable == 1)
                        {
                            Program.alarm_form.Insert_Alarm_Contents(alarm_contents, true);
                        }
                        else
                        {
                            if (alarm_contents.clear_request == true)
                            {
                                alarm_contents.clear_request = false;
                                alarm_contents.cleared_by = 0;
                                alarm_contents.cleared_time = DateTime.Now;
                                Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                            }
                        }
                    }
                }
                else
                {

                }
                if (alarm_contents.clear_request == true)
                {
                    alarm_contents.clear_request = false; //Alarm이 On일때 Alarm Clear 신호 무시
                }
            }
            else
            {
                //Program.IO.DI.Tag[(int)enum_di].last_off_time = DateTime.Now;
                alarm_contents.last_off_time = DateTime.Now;
                if (alarm_contents.auto_thread_call_keep_alarm_disable == true)
                {
                    if (alarm_contents.clear_request == true)
                    {
                        //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                        alarm_contents.clear_request = false;
                        alarm_contents.cleared_by = 0;
                        alarm_contents.cleared_time = DateTime.Now;
                        Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                    }
                }
                else
                {
                    if (alarm_contents.alarm_alive == false)
                    {
                        if (alarm_contents.clear_request == true)
                        {
                            //Occurred Alarm 창에서 사용자가 직접 Alarm Clear 선택했을때만 Alarm Clear Update Queue에 삽입
                            alarm_contents.clear_request = false;
                            alarm_contents.cleared_by = 0;
                            alarm_contents.cleared_time = DateTime.Now;
                            Program.alarm_form.Update_Alarm_Contents(alarm_contents, true);
                        }
                    }

                }
            }

            return result;

        }
        public void LevelSensor_Alarm_Check_Tank_A()
        {
            bool check_error = false;
            string result = "";
            result = "TANK A Level H : " + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value;
            result = result + ",TANK A Level M : " + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value;
            result = result + ",TANK A Level L : " + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value;
            result = result + ",TANK A Level LL : " + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value;
            result = result + ",TANK A Level Empty : " + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value;
            //Tank A
            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_HH].value == true)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                {

                    check_error = true;
                }
            }
            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == true)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                {
                    check_error = true;
                }
            }
            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value == true)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                {
                    check_error = true;
                }
            }
            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value == true)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                {
                    check_error = true;
                }
            }
            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value == true)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                {
                    check_error = true;
                }
            }

            if (check_error == true)
            {
                Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Level_Sensor_Weird_Tank_A, true, result);
            }
            else
            {
                Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Level_Sensor_Weird_Tank_A, false, "");
            }
        }
        public void LevelSensor_Alarm_Check_Tank_B()
        {
            bool check_error = false;
            //Tank A
            string result = "";
            result = "TANK B Level H : " + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value;
            result = result + ",TANK B Level M : " + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value;
            result = result + ",TANK B Level L : " + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value;
            result = result + ",TANK B Level LL : " + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value;
            result = result + ",TANK B Level Empty : " + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value;
            //Tank B
            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_HH].value == true)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                {
                    check_error = true;
                }
            }
            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == true)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                {
                    check_error = true;
                }
            }
            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value == true)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                {
                    check_error = true;
                }
            }
            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value == true)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value == false ||
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                {
                    check_error = true;
                }
            }
            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value == true)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                {
                    check_error = true;
                }
            }

            if (check_error == true)
            {
                Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Level_Sensor_Weird_Tank_B, true, result);
            }
            else
            {
                Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Level_Sensor_Weird_Tank_B, false, "");
            }
        }
        public void Alarm_Check()
        {
            string result = "";
            Config_Alarm config_alarm = null;
            var alarm_contents = config_alarm;
            int min_tmp = 0, max_tmp = 0; int value_tmp = 0;
            try
            {
                if (Program.cg_apploading.load_complete == false)
                {

                }
                else
                {
                    if (Program.cg_app_info.mode_simulation.use == true)
                    {
                        //return;
                    }
                    ///공통 알람 IO 상태값만 가지고 띄운다
                    //DI
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Stop_EMS, Program.IO.DI.Tag[(int)Config_IO.enum_di.EMS]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.E_RIGHT_Door_Open, Program.IO.DI.Tag[(int)Config_IO.enum_di.E_RIGHT_DOOR]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.ELEC_DOOR_OVERRIDE, Program.IO.DI.Tag[(int)Config_IO.enum_di.ELEC_DOOR_OVERRIDE]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Cooling_Left_Fan_Error, Program.IO.DI.Tag[(int)Config_IO.enum_di.LEFT_FAN_ALARM]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Cooling_Right_Fan_Error, Program.IO.DI.Tag[(int)Config_IO.enum_di.RIGHT_FAN_ALARM]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Level_High_High_Tank_A, Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_HH]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Level_High_High_Tank_B, Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_HH]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Level_High_High_Drain_Tank, Program.IO.DI.Tag[(int)Config_IO.enum_di.Drain_Tank_H]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Over_Flow_Tank_A, Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_OVERFLOW_CHECK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Over_Flow_Tank_B, Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_OVERFLOW_CHECK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Bottom_Vat, Program.IO.DI.Tag[(int)Config_IO.enum_di.BOTTOM_VAT_LEAK1]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Dike, Program.IO.DI.Tag[(int)Config_IO.enum_di.DIKE_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Fault, Program.IO.DI.Tag[(int)Config_IO.enum_di.LEAK_FAULT]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.C_Door_Open, Program.IO.DI.Tag[(int)Config_IO.enum_di.C_DOOR_ALARM]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Low_Exhaust_AlarmSensor, Program.IO.DI.Tag[(int)Config_IO.enum_di.EXH_LL]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.GPS_UPS_TRIP, Program.IO.DI.Tag[(int)Config_IO.enum_di.GPS_UPS_TRIP]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.MAIN_EQ_TRIP, Program.IO.DI.Tag[(int)Config_IO.enum_di.MAIN_EQ_TRIP]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.MAIN_UNIT_MIX_TRIP, Program.IO.DI.Tag[(int)Config_IO.enum_di.MAIN_UNIT_MIX_TRIP]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Circulation_Pump_Leak, Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Supply_A_Pump_Leak, Program.IO.DI.Tag[(int)Config_IO.enum_di.SUPPLY_A_PUMP_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Supply_B_Pump_Leak, Program.IO.DI.Tag[(int)Config_IO.enum_di.SUPPLY_B_PUMP_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Stop_Main_GAS_Dectect, Program.IO.DI.Tag[(int)Config_IO.enum_di.Gas_Detec_Alarm]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Purge_Unit_Alarm, Program.IO.DI.Tag[(int)Config_IO.enum_di.Purge_Unit_Alarm]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Tank_Vat, Program.IO.DI.Tag[(int)Config_IO.enum_di.TANK_VAT_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.HDIW_POWER_OFF, Program.IO.DI.Tag[(int)Config_IO.enum_di.HDIW_POWER_ON]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.HDIW_REMOTE_MODE_OFF, Program.IO.DI.Tag[(int)Config_IO.enum_di.HDIW_REMOTE_MODE]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.HDIW_TOTAL_ALARM, Program.IO.DI.Tag[(int)Config_IO.enum_di.HDIW_TOTAL_ALARM]);

                    //2023-04-06 DSP DI Alarm 추가
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Circulation_Pump_L_Leak, Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_L_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Circulation_Pump_R_Leak, Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_R_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Circulation_Pump_Stroke1, Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_STROKE_1]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Circulation_Pump_Stroke2, Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_STROKE_2]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Supply_A_Pump_L_Leak, Program.IO.DI.Tag[(int)Config_IO.enum_di.SUPPLY_A_PUMP_L_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Supply_A_Pump_R_Leak, Program.IO.DI.Tag[(int)Config_IO.enum_di.SUPPLY_A_PUMP_R_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Supply_A_Pump_Stroke1, Program.IO.DI.Tag[(int)Config_IO.enum_di.SUPPLY_A_PUMP_STROKE_1]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Supply_A_Pump_Stroke2, Program.IO.DI.Tag[(int)Config_IO.enum_di.SUPPLY_A_PUMP_STROKE_2]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Supply_B_Pump_L_Leak, Program.IO.DI.Tag[(int)Config_IO.enum_di.SUPPLY_B_PUMP_L_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Leak_Supply_B_Pump_R_Leak, Program.IO.DI.Tag[(int)Config_IO.enum_di.SUPPLY_B_PUMP_R_LEAK]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Supply_B_Pump_Stroke1, Program.IO.DI.Tag[(int)Config_IO.enum_di.SUPPLY_B_PUMP_STROKE_1]);
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Supply_B_Pump_Stroke2, Program.IO.DI.Tag[(int)Config_IO.enum_di.SUPPLY_B_PUMP_STROKE_2]);

                    //2023-04-07 Trip Alarm 추가
                    Alarm_Cal_WDT_BY_Status(frm_alarm.enum_alarm.Interlock_Trip, Program.IO.DI.Tag[(int)Config_IO.enum_di.Interlock_trip]);

                    Alarm_Check_Flow();
                    Alarm_Check_Pressure();
                    Alarm_Check_Abnormal_Flow();
                    Alarm_Check_By_ThreadCall();
                    Alarm_Check_By_System();

                    //Alarm Level 센서 Chain Alarm Check
                    LevelSensor_Alarm_Check_Tank_A();
                    LevelSensor_Alarm_Check_Tank_B();
                    Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Exhaust, Program.IO.AI.Tag[(int)Config_IO.enum_ai.EXHAUST].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Exhaust_Pressure_High), false);
                    Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Exhaust, Program.IO.AI.Tag[(int)Config_IO.enum_ai.EXHAUST].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Exhaust_Pressure_Low), false);

                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Alarm_Check" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        public void Alarm_Check_Flow()
        {
            try
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), false);
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), true);
                    }
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS2, Program.main_form.SerialData.FlowMeter_USF500.NH4OH_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS2, Program.main_form.SerialData.FlowMeter_USF500.NH4OH_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Flowrate_Low), false);
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS2, Program.main_form.SerialData.FlowMeter_USF500.NH4OH_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS2, Program.main_form.SerialData.FlowMeter_USF500.NH4OH_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Flowrate_Low), true);
                    }
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_Low), false);

                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_Low), true);
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), false);

                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), true);
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.DSP_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.DSP_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), false);

                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.DSP_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.DSP_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), true);
                    }
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_Low), false);

                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_Low), true);
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {

                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.HF_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.HF_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), false);
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.HF_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.HF_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), true);
                    }
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_Low), false);

                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_Low), true);
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {

                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.LAL_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.LAL_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), false);
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.LAL_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_Sonotec.LAL_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), true);
                    }
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_Low), false);

                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_Low), true);
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {

                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), false);
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS1, Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Flowrate_Low), true);
                    }
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS2, Program.main_form.SerialData.FlowMeter_USF500.H2SO4_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS2, Program.main_form.SerialData.FlowMeter_USF500.H2SO4_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Flowrate_Low), false);
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS2, Program.main_form.SerialData.FlowMeter_USF500.H2SO4_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS2, Program.main_form.SerialData.FlowMeter_USF500.H2SO4_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Flowrate_Low), true);
                    }
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS3, Program.main_form.SerialData.FlowMeter_USF500.HF_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS3, Program.main_form.SerialData.FlowMeter_USF500.HF_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Flowrate_Low), false);
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS3, Program.main_form.SerialData.FlowMeter_USF500.HF_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS3, Program.main_form.SerialData.FlowMeter_USF500.HF_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Flowrate_Low), true);
                    }
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_High), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_Low), false);

                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_High), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flowrate_CCSS4, Program.main_form.SerialData.FlowMeter_USF500.DIW_flow, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Flowrate_Low), true);
                    }
                }



                if (Program.cg_app_info.eq_type != enum_eq_type.ipa)
                {
                    if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_A), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_A), false);
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_B), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_B), false);

                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_A), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_A), true);
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_B), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_B), true);

                    }
                }
                else
                {
                    if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_A), false);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_A), false);
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_A), true);
                        Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_A), true);
                    }
                }
                if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                {
                    Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_B), false);
                    Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_B), false);
                }
                else
                {
                    Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_B), true);
                    Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_B), true);
                }

                if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                {
                    Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Tank_Circulation_Pump, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Tank_Circulation), false);
                    Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Tank_Circulation_Pump, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Tank_Circulation), false);
                }
                else
                {
                    Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Tank_Circulation_Pump, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Tank_Circulation), true);
                    Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Tank_Circulation_Pump, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Tank_Circulation), true);
                }

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.Drain_Pump_On].value == true)
                {
                    Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Drain, Program.IO.AI.Tag[(int)Config_IO.enum_ai.DRAIN_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Drain_Flowrate_High), false);
                    Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Drain, Program.IO.AI.Tag[(int)Config_IO.enum_ai.DRAIN_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Drain_Flowrate_Low), false);
                }
                else
                {
                    Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Drain, Program.IO.AI.Tag[(int)Config_IO.enum_ai.DRAIN_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Drain_Flowrate_High), true);
                    Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Drain, Program.IO.AI.Tag[(int)Config_IO.enum_ai.DRAIN_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Drain_Flowrate_Low), true);
                }
                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_HDIW, Program.IO.AI.Tag[(int)Config_IO.enum_ai.HDIW_FLOW_MONITORING].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.HDIW_Flowrate_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_HDIW, Program.IO.AI.Tag[(int)Config_IO.enum_ai.HDIW_FLOW_MONITORING].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.HDIW_Flowrate_Low), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Tank_A_N2, Program.IO.AI.Tag[(int)Config_IO.enum_ai.TANKA_PN2_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Tank_A_N2_Flowrate_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Tank_A_N2, Program.IO.AI.Tag[(int)Config_IO.enum_ai.TANKA_PN2_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Tank_A_N2_Flowrate_Low), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Flow_Tank_B_N2, Program.IO.AI.Tag[(int)Config_IO.enum_ai.TANKB_PN2_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Tank_B_N2_Flowrate_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Flow_Tank_B_N2, Program.IO.AI.Tag[(int)Config_IO.enum_ai.TANKB_PN2_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Tank_B_N2_Flowrate_Low), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Circulation_Flow_PCW, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_THERMOSTAT_PCW_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.PCW_Circulation_FLow_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Circulation_Flow_PCW, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_THERMOSTAT_PCW_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.PCW_Circulation_FLow_Low), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Supply_A_Flow_PCW, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_THERMOSTAT_PCW_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.PCW_Supply_A_FLow_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Supply_A_Flow_PCW, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_THERMOSTAT_PCW_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.PCW_Supply_A_FLow_Low), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Supply_B_Flow_PCW, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_THERMOSTAT_PCW_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.PCW_Supply_B_FLow_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Supply_B_Flow_PCW, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_THERMOSTAT_PCW_FLOW].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.PCW_Supply_B_FLow_Low), false);
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Alarm_Check_Flow" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }

        }
        public void Alarm_Check_Pressure()
        {
            double value_tmp_supply_in_ai = 0, value_tmp_supply_out_ai = 0;
            //Pressure
            //CCSS1
            try
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    value_tmp_supply_in_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.NH4OH_SUPPLY_FILTER_IN_PRESS].value;
                    value_tmp_supply_out_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.NH4OH_SUPPLY_FILTER_OUT_PRESS].value;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    value_tmp_supply_in_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_IN_PRESS].value;
                    value_tmp_supply_out_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_IN_PRESS].value;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    value_tmp_supply_in_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.DSP_SUPPLY_FILTER_IN_PRESS].value;
                    value_tmp_supply_out_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.DSP_SUPPLY_FILTER_OUT_PRESS].value;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    value_tmp_supply_in_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS].value;
                    value_tmp_supply_out_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS].value;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    value_tmp_supply_in_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.LAL_SUPPLY_FILTER_IN_PRESS].value;
                    value_tmp_supply_out_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.LAL_SUPPLY_FILTER_OUT_PRESS].value;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    value_tmp_supply_in_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_IN_PRESS].value;
                    value_tmp_supply_out_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_OUT_PRESS].value;
                }
                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Chem_In_CCSS1, value_tmp_supply_in_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Filter_IN_Pressure_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Chem_In_CCSS1, value_tmp_supply_in_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Filter_IN_Pressure_Low), false);
                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Chem_Out_CCSS1, value_tmp_supply_out_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Filter_OUT_Pressure_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Chem_Out_CCSS1, value_tmp_supply_out_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Filter_OUT_Pressure_Low), false);

                //CCSS2
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    value_tmp_supply_in_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_IN_PRESS].value;
                    value_tmp_supply_out_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_OUT_PRESS].value;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    value_tmp_supply_in_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2SO4_SUPPLY_FILTER_IN_PRESS].value;
                    value_tmp_supply_out_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2SO4_SUPPLY_FILTER_OUT_PRESS].value;
                }
                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Chem_In_CCSS2, value_tmp_supply_in_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Filter_IN_Pressure_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Chem_In_CCSS2, value_tmp_supply_in_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Filter_IN_Pressure_Low), false);
                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Chem_Out_CCSS2, value_tmp_supply_out_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Filter_OUT_Pressure_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Chem_Out_CCSS2, value_tmp_supply_out_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Filter_OUT_Pressure_Low), false);

                //CCSS3
                if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    value_tmp_supply_in_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS].value;
                    value_tmp_supply_out_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS].value;
                }
                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Chem_In_CCSS3, value_tmp_supply_in_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Filter_IN_Pressure_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Chem_In_CCSS3, value_tmp_supply_in_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Filter_IN_Pressure_Low), false);
                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Chem_Out_CCSS3, value_tmp_supply_out_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Filter_OUT_Pressure_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Chem_Out_CCSS3, value_tmp_supply_out_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Filter_OUT_Pressure_Low), false);

                //CCSS4
                value_tmp_supply_in_ai = Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value;
                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Chem_In_CCSS4, value_tmp_supply_in_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Filter_IN_Pressure_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Chem_In_CCSS4, value_tmp_supply_in_ai, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Filter_IN_Pressure_Low), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Tank_Circulation_Pump, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Tank_Circulation), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Tank_Circulation_Pump, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Tank_Circulation), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_IN_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Supply_A_IN), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_IN_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Supply_A_IN), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_IN_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Supply_B_IN), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_IN_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Supply_B_IN), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_OUT_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_OUT_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Supply_A_OUT), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_OUT_Supply_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_OUT_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Supply_A_OUT), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_OUT_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_OUT_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Supply_B_OUT), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_OUT_Supply_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_OUT_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Supply_B_OUT), false);


                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Return_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_A].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Retrun_A), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Return_A, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_A].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Retrun_A), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Return_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_B].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Retrun_B), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Return_B, Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_B].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Retrun_B), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Main_N2, Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_PN2_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.N2_Pressure_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Main_N2, Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_PN2_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.N2_Pressure_Low), false);

                //AI
                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_MAIN_CDA1_PRESSPUMP, Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA1_PRESS_PUMP].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_MAIN_CDA1_PRESS_PUMP), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_MAIN_CDA1_PRESSPUMP, Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA1_PRESS_PUMP].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_MAIN_CDA1_PRESS_PUMP), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_MAIN_CDA2_PRESSSOL, Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA2_PRESS_SOL].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_MAIN_CDA2_PRESS_SOL), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_MAIN_CDA2_PRESSSOL, Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA2_PRESS_SOL].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_MAIN_CDA2_PRESS_SOL), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_MAIN_CDA3_PRESSDRAIN, Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA3_PRESS_DRAIN].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_MAIN_CDA3_PRESS_DRAIN), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_MAIN_CDA3_PRESSDRAIN, Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA3_PRESS_DRAIN].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_MAIN_CDA3_PRESS_DRAIN), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_PCW, Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_PCW_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.PCW_Pressure_High), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_PCW, Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_PCW_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.PCW_Pressure_Low), false);

                Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Pressure_Heater_N2, Program.IO.AI.Tag[(int)Config_IO.enum_ai.HEATER_N2_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Heater_N2), false);
                Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Pressure_Heater_N2, Program.IO.AI.Tag[(int)Config_IO.enum_ai.HEATER_N2_PRESS].value, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Heater_N2), false);

            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Alarm_Check_Pressure" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }

        }
        public void Alarm_Check_Abnormal_Flow()
        {
            double value_tmp = 0;
            try
            {
                //CCSS1
                value_tmp = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Abnormal_Flow_Min);
                if (value_tmp == 0) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem1, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Abnormal_Flow_Min), true); }
                else
                {
                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value == false) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem1, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Abnormal_Flow_Min), false); }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == false) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem1, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Abnormal_Flow_Min), false); }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == false || Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == false) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem1, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Abnormal_Flow_Min), false); }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == false || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == false) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem1, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Abnormal_Flow_Min), false); }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == false || Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == false) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem1, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Abnormal_Flow_Min), false); }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == false || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == false) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem1, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Abnormal_Flow_Min), false); }
                    }
                }
                //CCSS2
                value_tmp = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Abnormal_Flow_Min);
                if (value_tmp == 0) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem2, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Abnormal_Flow_Min), true); }
                else
                {
                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == false) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem2, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Abnormal_Flow_Min), false); }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A].value == false || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B].value == false) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem2, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Abnormal_Flow_Min), false); }
                    }

                }
                //CCSS3
                value_tmp = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Abnormal_Flow_Min);
                if (value_tmp == 0) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem3, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Abnormal_Flow_Min), true); }
                else
                {
                    if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == false || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == false) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem3, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Abnormal_Flow_Min), false); }
                    }

                }
                //CCSS4
                value_tmp = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Abnormal_Flow_Min);
                if (value_tmp == 0) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem4, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Abnormal_Flow_Min), true); }
                else
                {
                    if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == false || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == false) { Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Abnormal_Flow_Dectect_Chem4, value_tmp, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem4_Abnormal_Flow_Min), false); }
                    }
                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Alarm_Check_Abnormal_Flow" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
            //CCSS1

        }
        public void Alarm_Check_By_ThreadCall()
        {
            try
            {
                //프로그램 내 Thread Call 알람처리, 각 Thread에서 호출 +  Alarm_Check 에서도 있어야함
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CTC_Database_Exception);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Fail_Chemical_Change_Command);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Chemical_CM_Value_No_Change);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.High_Concentration_Chem1);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Low_Concentration_Chem1);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.High_Concentration_Chem2);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Low_Concentration_Chem2);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.High_Concentration_Chem3);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Low_Concentration_Chem3);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.High_Concentration_Chem4);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Low_Concentration_Chem4);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.HDIW_Supply_Temp_Error);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Pump_Empty_Trouble_Tank_Circulation_Pump);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Pump_Empty_Trouble_Supply_A_Pump);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Pump_Empty_Trouble_Supply_B_Pump);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_A_Chem1);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_A_Chem2);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_A_Chem3);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_A_Chem4);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_A_Chem1);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_A_Chem2);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_A_Chem3);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_A_Chem4);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_B_Chem1);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_B_Chem2);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_B_Chem3);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_B_Chem4);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_B_Chem1);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_B_Chem2);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_B_Chem3);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_B_Chem4);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Circulation_Pump_Error_In_Charge);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Prep_Pump_Operation_Error);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Not_Ready_Tank_A);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Not_Ready_Tank_B);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Level_Low_Low_Tank_A);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Level_Low_Low_Tank_B);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Drain_Time_Over_Tank_A);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Drain_Time_Over_Tank_B);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Over_Time_Chemical_Change_Request);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Heating_Time_Over_Temp_Controll_Tank_A);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Heating_Time_Over_Temp_Controll_Tank_B);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Charge_Time_Over_Tank_A);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Charge_Time_Over_Tank_B);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Ready_Time_Over_Tank_A);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Ready_Time_Over_Tank_B);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CM_Fail_Prep_Tank_A);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CM_Fail_Prep_Tank_B);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CDS_Alive_Interlock);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Database_Alarm_ID_Not_Register);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Database_Parameter_ID_Not_Register);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Concentration_Fail_Tank_A);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Concentration_Fail_Tank_B);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Flowmeter1_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Flowmeter1_CH1_Measure_Error);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Flowmeter1_CH2_Measure_Error);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Flowmeter2_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Flowmeter2_CH1_Measure_Error);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Flowmeter2_CH2_Measure_Error);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Life_Time_Over_Tank_A);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Life_Time_Over_Tank_B);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Can_Not_Use_Tank);


                //ThermoStat
                if (Program.cg_app_info.eq_type == enum_eq_type.dsp || Program.cg_app_info.eq_type == enum_eq_type.dhf || Program.cg_app_info.eq_type == enum_eq_type.lal || Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Error);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Encount1);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Encount2);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Encount3);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Encount4);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Encount5);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Warning1);

                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Error);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Encount1);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Encount2);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Encount3);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Encount4);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Encount5);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Warning1);

                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Error);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Encount1);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Encount2);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Encount3);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Encount4);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Encount5);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Warning1);
                }
                else
                {
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Concentration_ABB_Alarm);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SCR1_Error);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SCR2_Error);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SCR3_Error);
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.SCR4_Error);
                }
                //M74
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller1_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller1_CH1_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller1_CH2_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller1_CH3_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller1_CH4_Alarm);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller2_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller2_CH1_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller2_CH2_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller2_CH3_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller2_CH4_Alarm);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller3_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller3_CH1_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller3_CH2_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller3_CH3_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller3_CH4_Alarm);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller4_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller4_CH1_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller4_CH2_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller4_CH3_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Temp_Controller4_CH4_Alarm);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Heater_Interlock_Tank_A);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Heater_Interlock_Tank_B);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Heater_Interlock_Circulation);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Heater_Interlock_Circulation2);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Concentration_CS_600F_Alarm);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Concentration_CS_150C_Alarm);

                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Heat_Exchanger_Error_D1110);
                Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.Heat_Exchanger_Error_D1111);

                if (Program.cg_app_info.eq_type == enum_eq_type.ipa && Program.cg_app_info.ipa_ccss_ready_use == true)
                {
                    Alarm_Cal_WDT_BY_Thread_Call(frm_alarm.enum_alarm.IPA_CCSS_Not_Ready_Signal);
                }

            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Alarm_Check_By_ThreadCall" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }

        }
        public void Alarm_Check_By_System()
        {
            double value_tmp = 0;
            int default_time_out_data_rcv = 10;
            try
            {
                ///System Alarm
                if (Program.schematic_form.timer_manual_sequence_tank_a.Enabled == true ||
                   Program.schematic_form.timer_manual_sequence_tank_b.Enabled == true)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SemiAuto_Mode_Operation, true, "");
                }
                else
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SemiAuto_Mode_Operation, false, "");
                }
                if (Program.cg_app_info.internal_info.log_manager_run == true && Program.main_form.APP_LOG_MANAGER_STATE == false)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Application_Start_Error_Log_Manager, true, "");
                }
                else
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Application_Start_Error_Log_Manager, false, "");
                }

                //Supply A
                if (Program.main_form.SerialData.Supply_A_Thermostat.run_state == true || Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.mode == Class_TempController_M74.enum_m74_status.run)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true)
                    {
                        if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Set))
                        {
                            Tank_A_Temp_Alarm_Start = true;
                        }
                        if (Tank_A_Temp_Alarm_Start == true)
                        {
                            Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Temperature_Tank_A, Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_High), false);
                            Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Temperature_Tank_A, Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Low), false);
                        }
                    }
                }
                else
                {
                    //heater Off후  트리거 초기화
                    Tank_A_Temp_Alarm_Start = false;
                    //Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Temperature_Tank_A, Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_High), true);
                    //Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Temperature_Tank_A, Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Low), true);
                }

                //Supply B
                if (Program.main_form.SerialData.Supply_B_Thermostat.run_state == true || Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.mode == Class_TempController_M74.enum_m74_status.run)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true)
                    {
                        if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_Set))
                        {
                            Tank_B_Temp_Alarm_Start = true;
                        }
                        if (Tank_B_Temp_Alarm_Start == true)
                        {
                            Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Temperature_Tank_B, Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_High), false);
                            Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Temperature_Tank_B, Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_Low), false);
                        }

                    }

                }
                else
                {
                    //heater Off후  트리거 초기화
                    Tank_B_Temp_Alarm_Start = false;
                    //Alarm_Cal_WDT_BY_Range_Check_Max(frm_alarm.enum_alarm.High_Temperature_Tank_B, Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_High), true);
                    //Alarm_Cal_WDT_BY_Range_Check_Min(frm_alarm.enum_alarm.Low_Temperature_Tank_B, Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv, Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Low), true);
                }

                if (Program.main_form.APP_SERIAL_DAEMON_STATE == false)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Application_Start_Error_Serial_Daemon, true, "");
                }
                else
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Application_Start_Error_Serial_Daemon, false, "");
                }

                if (Program.CTC.run_state == false)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Connect_Fail_Host, true, "");
                }
                else
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Connect_Fail_Host, false, "");
                }
                if (Program.ABB.run_state == false)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Concentration_ABB_Connection_Fail, true, "");
                }
                else
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Concentration_ABB_Connection_Fail, false, "");
                }
                value_tmp = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Disk_Storage_Log_Min);
                if (value_tmp == 0 || value_tmp == -1)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.No_Space_Disk_Storage_Log, false, "");
                }
                else
                {
                    if (Program.cg_app_info.internal_info.usage_drive_d != 0 && Program.cg_app_info.internal_info.usage_drive_d < value_tmp)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.No_Space_Disk_Storage_Log, true, @"D:\ = " + Program.cg_app_info.internal_info.usage_drive_d);
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.No_Space_Disk_Storage_Log, false, "");
                    }
                }

                value_tmp = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Disk_Storage_Program_Min);
                if (value_tmp == 0 || value_tmp == -1)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.No_Space_Disk_Storage_Program, false, "");
                }
                else
                {
                    if (Program.cg_app_info.internal_info.usage_drive_c != 0 && Program.cg_app_info.internal_info.usage_drive_c < value_tmp)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.No_Space_Disk_Storage_Program, true, @"C:\ = " + Program.cg_app_info.internal_info.usage_drive_c);
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.No_Space_Disk_Storage_Program, false, "");
                    }
                }
                //COMI
                if (Program.ethercat_md.error_state == Module_Ethercat.enum_error_type.None)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Ethercat_Communication_Error, false, "");
                }
                else if (Program.ethercat_md.error_state == Module_Ethercat.enum_error_type.Init_Error)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Ethercat_Communication_Error, true, "COMI DLL INIT ERROR");
                }
                else if (Program.ethercat_md.error_state == Module_Ethercat.enum_error_type.Run_Error)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Ethercat_Communication_Error, true, "COMI DLL RUN ERROR");
                }
                else if (Program.ethercat_md.error_state == Module_Ethercat.enum_error_type.Serial_CM_Error)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Ethercat_Communication_Error, true, "Serial Communication Fail");
                }
                else if (Program.ethercat_md.error_state == Module_Ethercat.enum_error_type.Serial_Daemon)
                {
                    Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Ethercat_Communication_Error, true, "Serial Daemon Fail");
                }

                //Serail Port Status
                //Program.main_form.SerialData.FlowMeter_USF500.last_rcv_time_ch1 = DateTime.Now.Add
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER] == true)
                    {
                        if ((DateTime.Now - Program.main_form.SerialData.FlowMeter_USF500.last_rcv_time_ch1).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, false, "");
                        }
                        if ((DateTime.Now - Program.main_form.SerialData.FlowMeter_USF500.last_rcv_time_ch2).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter2_Connnection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter2_Connnection_Fail, false, "");
                        }
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, true, "");
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter2_Connnection_Fail, true, "");
                    }

                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_apm_serial_index.SUPPLY_A_PUMP_CONTROLLER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController1_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController1_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_apm_serial_index.SUPPLY_B_PUMP_CONTROLLER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController2_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController2_Connection_Fail, true, "");
                    }

                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74] == true)
                    {
                        if ((DateTime.Now - Program.main_form.SerialData.TEMP_CONTROLLER.last_rcv_time_ch1).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, false, "");
                        }
                        if ((DateTime.Now - Program.main_form.SerialData.TEMP_CONTROLLER.last_rcv_time_ch2).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller2_Connection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller2_Connection_Fail, false, "");
                        }
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, true, "");
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller2_Connection_Fail, true, "");
                    }

                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_apm_serial_index.TEMP_CONTROLLER_M74R] == true)
                    {
                        if ((DateTime.Now - Program.main_form.SerialData.TEMP_CONTROLLER.last_rcv_time_ch3).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller3_Connection_Fail, true, "");
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller4_Connection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller3_Connection_Fail, false, "");
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller4_Connection_Fail, false, "");
                        }
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller3_Connection_Fail, true, "");
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller4_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_apm_serial_index.SCR] == true)
                    {
                        if ((DateTime.Now - Program.main_form.SerialData.SCR.last_rcv_time_ch1).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR1_Connection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR1_Connection_Fail, false, "");
                        }
                        if ((DateTime.Now - Program.main_form.SerialData.SCR.last_rcv_time_ch2).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR2_Connection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR2_Connection_Fail, false, "");
                        }
                        if ((DateTime.Now - Program.main_form.SerialData.SCR.last_rcv_time_ch3).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR3_Connection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR3_Connection_Fail, false, "");
                        }
                        if ((DateTime.Now - Program.main_form.SerialData.SCR.last_rcv_time_ch4).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR4_Connection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR4_Connection_Fail, false, "");
                        }
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR1_Connection_Fail, true, "");
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR2_Connection_Fail, true, "");
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR3_Connection_Fail, true, "");
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR4_Connection_Fail, true, "");

                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_ipa_serial_index.SONOTEC_FLOWMETER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_ipa_serial_index.SUPPLY_A_PUMP_CONTROLLER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController1_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController1_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_ipa_serial_index.TEMP_CONTROLLER_M74] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_ipa_serial_index.SCR] == true)
                    {
                        if ((DateTime.Now - Program.main_form.SerialData.SCR.last_rcv_time_ch1).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR1_Connection_Fail, false, "");
                        }
                    }
                    else
                    {
                        if ((DateTime.Now - Program.main_form.SerialData.SCR.last_rcv_time_ch1).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SCR1_Connection_Fail, true, "");
                        }
                    }
                }

                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.SONOTEC_FLOWMETER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.SUPPLY_A_PUMP_CONTROLLER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController1_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController1_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.SUPPLY_B_PUMP_CONTROLLER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController2_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController2_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.TEMP_CONTROLLER_M74] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.SUPPLY_A_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.SUPPLY_B_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Connection_Fail, true, "");
                    }
                }

                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dhf_serial_index.USF500_FLOWMETER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dhf_serial_index.SUPPLY_A_PUMP_CONTROLLER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController1_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController1_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dhf_serial_index.SUPPLY_B_PUMP_CONTROLLER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController2_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController2_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.CONCENTRATION] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Concentration_CM210_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Concentration_CM210_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dhf_serial_index.TEMP_CONTROLLER_M74] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dhf_serial_index.CIRCULATION_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dhf_serial_index.SUPPLY_A_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dhf_serial_index.SUPPLY_B_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Connection_Fail, true, "");
                    }
                }

                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_lal_serial_index.SONOTEC_FLOWMETER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_lal_serial_index.SUPPLY_A_PUMP_CONTROLLER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController1_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController1_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_lal_serial_index.SUPPLY_B_PUMP_CONTROLLER] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController2_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.PumpController2_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_lal_serial_index.TEMP_CONTROLLER_M74] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Temp_Controller1_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_lal_serial_index.CIRCULATION_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_lal_serial_index.SUPPLY_A_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_lal_serial_index.SUPPLY_B_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_lal_serial_index.CONCENTRATION] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Concentration_CS_600F_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Concentration_CS_600F_Connection_Fail, true, "");
                    }

                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_mix_serial_index.USF500_FLOWMETER] == true)
                    {
                        if ((DateTime.Now - Program.main_form.SerialData.FlowMeter_USF500.last_rcv_time_ch1).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, false, "");
                        }
                        if ((DateTime.Now - Program.main_form.SerialData.FlowMeter_USF500.last_rcv_time_ch2).TotalSeconds >= default_time_out_data_rcv)
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter2_Connnection_Fail, true, "");
                        }
                        else
                        {
                            Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter2_Connnection_Fail, false, "");
                        }
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter1_Connnection_Fail, true, "");
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Flowmeter2_Connnection_Fail, true, "");
                    }

                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_mix_serial_index.CIRCULATION_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.CIRCULATION_THERMOSTAT_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_mix_serial_index.SUPPLY_A_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_A_THERMOSTAT_Connection_Fail, true, "");
                    }
                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_mix_serial_index.SUPPLY_B_THERMOSTAT] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.SUPPLY_B_THERMOSTAT_Connection_Fail, true, "");
                    }

                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_mix_serial_index.CM_HF_700] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Concentration_HF700_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Concentration_HF700_Connection_Fail, true, "");
                    }

                    if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_mix_serial_index.CM_CS_150C] == true)
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Concentration_CS_150C_Connection_Fail, false, "");
                    }
                    else
                    {
                        Alarm_Cal_WDT_BY_System(frm_alarm.enum_alarm.Concentration_CS_150C_Connection_Fail, true, "");
                    }

                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Alarm_Check_By_System" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }

        }
        private delegate void Del_System_Message_Add(string msg);
        public void Insert_System_Message(string msg)
        {
            this.BeginInvoke(new Del_System_Message_Add(System_Message_Add), msg);
        }
        public void System_Message_Add(string msg)
        {
            try
            {
                if (lbox_system_message.Items.Count >= 30)
                {
                    lbox_system_message.Items.RemoveAt(lbox_system_message.Items.Count - 1);
                }
                lbox_system_message.Items.Insert(0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg);
                lbox_system_message.SelectedIndex = 0;
            }
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".System_Message_Add." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally { }

        }
        public void socket_operate()
        {
            //Recieve Socket Packet
            TCP_IP_SOCKET.Cls_Socket.Socket_Event socket_event = new TCP_IP_SOCKET.Cls_Socket.Socket_Event();
            string result = "", error_text = "";
            try
            {
                if (st_socket_operate.timer_initial == false)
                {
                    for (int index = 0; index < st_socket_operate.subtimer_count; index++)
                    { st_socket_operate.tp_check_interval[index] = new TimeSpan(0); st_socket_operate.dt_check_last_act[index] = new DateTime(0); }
                    st_socket_operate.timer_initial = true;
                }
                //
                while (true)
                {
                    if (Program.cg_apploading.load_complete == false)
                    {

                    }
                    else
                    {
                        if (Program.SOCKET.m_Col_Socket_Event_List.Count > 0)
                        {
                            if (Program.SOCKET.m_Col_Socket_Event_List.TryDequeue(out socket_event) == true)
                            {
                                //Recieve_Data_To_Parse_HeatExchanger
                                if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Open)
                                {
                                    if (socket_event.info_ip == "10.10.10.1" || (socket_event.info_ip == "127.0.0.1" && socket_event.info_port == 502))
                                    {
                                        Program.ABB.Recieve_Data_To_Parse_ABB(socket_event);
                                    }
                                    else if (socket_event.info_ip == Program.cg_socket.heat_exchanger_ip && socket_event.info_port == 502)
                                    {
                                        Program.Heat_Exchanger.Recieve_Data_To_Parse_HeatExchanger(socket_event);
                                    }
                                    else
                                    {
                                        Program.CTC.Socket_Data_Parse(socket_event);
                                    }
                                }
                                else if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Close)
                                {
                                    if (socket_event.info_ip == "10.10.10.1" || (socket_event.info_ip == "127.0.0.1" && socket_event.info_port == 502))
                                    {
                                        Program.ABB.Recieve_Data_To_Parse_ABB(socket_event);
                                    }
                                    else if (socket_event.info_ip == Program.cg_socket.heat_exchanger_ip && socket_event.info_port == 502)
                                    {
                                        Program.Heat_Exchanger.Recieve_Data_To_Parse_HeatExchanger(socket_event);
                                    }
                                    else
                                    {
                                        Program.CTC.Socket_Data_Parse(socket_event);
                                    }
                                }
                                else if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Recieve)
                                {
                                    if (socket_event.info_ip == "10.10.10.1" || (socket_event.info_ip == "127.0.0.1" && socket_event.info_port == 502))
                                    {
                                        Program.ABB.Recieve_Data_To_Parse_ABB(socket_event);
                                    }
                                    else if (socket_event.info_ip == Program.cg_socket.heat_exchanger_ip && socket_event.info_port == 502)
                                    {
                                        Program.Heat_Exchanger.Recieve_Data_To_Parse_HeatExchanger(socket_event);
                                    }
                                    else
                                    {
                                        Program.CTC.Socket_Data_Parse(socket_event);
                                    }
                                }
                                else if (socket_event.status == TCP_IP_SOCKET.Cls_Socket.Socket_Event_Type.Send)
                                {
                                    //lbox_socket_status.Items.Add(socket_event.dt_occur.ToString() + " / " + socket_event.status.ToString() + " / " + socket_event.data_to_string);
                                }
                                else if (socket_event.status == Socket_Event_Type.None)
                                {
                                    Insert_System_Message("Socket Data Null");
                                    Program.log_md.LogWrite(this.Name + "." + "socket_operate" + "." + "Socket Event is Null", Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                                }

                            }
                            else
                            {
                                Insert_System_Message("Socket Data Fail");
                                Program.log_md.LogWrite(this.Name + "." + "socket_operate" + "." + "Socket Event is Null", Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                            }



                        }
                        if (Program.SOCKET.m_Col_Error_List.Count > 0)
                        {
                            error_text = Program.SOCKET.m_Col_Error_List.Dequeue();
                            if (socket_event.info_ip == null || socket_event.info_port == null)
                            {
                                error_text = "Socket Error : Connection Fail";
                            }
                            else
                            {
                                error_text = "Socket Error : " + socket_event.info_ip.ToString() + " / " + socket_event.info_port.ToString();
                            }
                            Program.log_md.LogWrite(error_text, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                        }
                    }
                    System.Threading.Thread.Sleep(50);

                    //일정 시간 Date Rcv가 없으면 ABB Socket Close로 결정 짓는다
                    if ((DateTime.Now - Program.ABB.dt_abb_validate_check).TotalSeconds >= Program.cg_app_info.abb_connect_time)
                    {
                        Program.ABB.dt_abb_validate_check = DateTime.Now;
                        Program.ABB.run_state = false;
                    }
                    //일정 시간 Date Rcv가 없으면 HEAT EXCHANGER Socket Close로 결정 짓는다
                    //TimeOut Error abb와 동일사용
                    if ((DateTime.Now - Program.Heat_Exchanger.dt_validate_check).TotalSeconds >= Program.cg_app_info.abb_connect_time)
                    {
                        Program.Heat_Exchanger.dt_validate_check = DateTime.Now;
                        if (Program.cg_app_info.mode_simulation.use == false)
                        {
                            Program.Heat_Exchanger.run_state = false;
                        }

                    }

                    //일정 시간 Date Rcv가 없으면 CTC Close로 결정 짓는다
                    if ((DateTime.Now - Program.CTC.dt_ctc_validate_check).TotalSeconds >= Program.cg_app_info.ctc_connect_time)
                    {
                        Program.CTC.dt_ctc_validate_check = DateTime.Now;
                        //수행 안함
                        //Program.CTC.run_state = false;
                    }
                }

            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "socket_operate" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        /// <summary>
        /// 주의 IO Conig에서 Address를 변경해서 사용하기 떄문에 아래 규칙을 준수해야한다.
        /// DI, DO, AI, AO Read시 Enum으로 배열 객체 그대로 접근 => Ethercat_commi_Rcv_AD Address에 맞는 데이터를 대입한다.
        /// DO, AO시 해당 Enum의 참조 접근이 아닌, Address로 접근한다.
        /// </summary>
        public void Ethercat_commi_Rcv_AD()
        {

            //Data 저장 변수
            bool[] read_di = new bool[Program.IO.DI.use_cnt];
            bool[] read_do = new bool[Program.IO.DO.use_cnt];
            int[] read_ai = new int[Program.IO.AI.use_cnt];
            int[] read_ao = new int[Program.IO.AO.use_cnt];

            //Digital Data 변동 확인을 위한 변수, 값이 변경되었을 때만 로그를 생성한다. 
            string log_di = "", log_di_old = "";
            string log_do = "", log_do_old = "";
            int level_sensor_change_delay = 0;
            int empty_level_sensor_change_delay = 0;
            try
            {
                while (true)
                {
                    level_sensor_change_delay = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Level_Sensor_Change_Time_Delay);
                    empty_level_sensor_change_delay = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Empty_Level_Sensor_Change_Time_Delay);
                    if (Program.cg_app_info.mode_simulation.use == true)
                    {

                        //DI Data Mapping
                        log_di = "";
                        //Program.COMI_ETHERCAT.DI_Read(ref read_di);
                        for (int idx = 0; idx < Program.IO.DI.use_cnt; idx++)
                        {
                            //Program.IO.DI.Tag[idx].value = read_di[Program.IO.DI.Tag[idx].address];
                            log_di = log_di + "," + Convert.ToByte(Program.IO.DI.Tag[idx].value);
                        }
                        if (log_di != log_di_old) { Program.log_md.LogWrite(log_di, Module_Log.enumLog.DigitalData_In, "", Module_Log.enumLevel.ALWAYS); }
                        log_di_old = log_di;

                        System.Threading.Thread.Sleep(20);
                        //DO Data Mapping
                        log_do = "";
                        //Program.COMI_ETHERCAT.DO_Read(ref read_do);
                        for (int idx = 0; idx < Program.IO.DO.use_cnt; idx++)
                        {
                            //Program.IO.DO.Tag[idx].value = read_do[Program.IO.DO.Tag[idx].address];
                            log_do = log_do + "," + Convert.ToByte(Program.IO.DO.Tag[idx].value);
                        }
                        if (log_do != log_do_old) { Program.log_md.LogWrite(log_do, Module_Log.enumLog.DigitalData_Out, "", Module_Log.enumLevel.ALWAYS); }
                        log_do_old = log_do;

                        System.Threading.Thread.Sleep(20);
                        //Analog In Mapping
                        //Program.COMI_ETHERCAT.AI_Read(ref read_ai);
                        for (int idx = 0; idx < Program.IO.AI.use_cnt; idx++)
                        {
                            //Program.IO.AI.Tag[idx].value = read_ai[Program.IO.AI.Tag[idx].address];
                        }
                        System.Threading.Thread.Sleep(20);
                        //Analog Out Mapping
                        //Program.COMI_ETHERCAT.AO_Read(ref read_ao);
                        for (int idx = 0; idx < Program.IO.AO.use_cnt; idx++)
                        {
                            // Program.IO.AO.Tag[idx].value = read_ao[Program.IO.AO.Tag[idx].address];
                        }
                        System.Threading.Thread.Sleep(20);
                    }
                    else
                    {
                        if (Program.COMI_ETHERCAT.comi_dll_Initilized == true)
                        {
                            //DI Data Mapping
                            log_di = "";
                            Program.COMI_ETHERCAT.DI_Read(ref read_di);
                            for (int idx = 0; idx < Program.IO.DI.use_cnt; idx++)
                            {

                                Program.IO.DI.Tag[idx].value_raw = read_di[Program.IO.DI.Tag[idx].address];
                                if (Program.IO.DI.Tag[idx].unit == "N.C")
                                {
                                    Program.IO.DI.Tag[idx].value = !Program.IO.DI.Tag[idx].value_raw;
                                }
                                else
                                {
                                    Program.IO.DI.Tag[idx].value = Program.IO.DI.Tag[idx].value_raw;

                                    if (idx == (int)Config_IO.enum_di.TANKA_EMPTY_CHECK || idx == (int)Config_IO.enum_di.TANKB_EMPTY_CHECK)
                                    {
                                        if (Program.IO.DI.Tag[idx].value_raw == true)
                                        {
                                            if ((DateTime.Now - Program.IO.DI.Tag[idx].dt_off).TotalSeconds >= empty_level_sensor_change_delay)
                                            {
                                                Program.IO.DI.Tag[idx].value = Program.IO.DI.Tag[idx].value_raw;
                                            }
                                            Program.IO.DI.Tag[idx].dt_on = DateTime.Now;
                                        }
                                        else
                                        {
                                            if ((DateTime.Now - Program.IO.DI.Tag[idx].dt_on).TotalSeconds >= empty_level_sensor_change_delay)
                                            {
                                                Program.IO.DI.Tag[idx].value = Program.IO.DI.Tag[idx].value_raw;
                                            }
                                            Program.IO.DI.Tag[idx].dt_off = DateTime.Now;
                                        }
                                    }
                                    else if (idx == (int)Config_IO.enum_di.TANKA_LEVEL_H || idx == (int)Config_IO.enum_di.TANKA_LEVEL_M ||
                                       idx == (int)Config_IO.enum_di.TANKA_LEVEL_L || idx == (int)Config_IO.enum_di.TANKA_LEVEL_LL ||
                                       idx == (int)Config_IO.enum_di.TANKA_EMPTY_CHECK ||
                                       idx == (int)Config_IO.enum_di.TANKB_LEVEL_H || idx == (int)Config_IO.enum_di.TANKB_LEVEL_M ||
                                       idx == (int)Config_IO.enum_di.TANKB_LEVEL_L || idx == (int)Config_IO.enum_di.TANKB_LEVEL_LL ||
                                       idx == (int)Config_IO.enum_di.TANKB_EMPTY_CHECK)
                                    {

                                        if (Program.IO.DI.Tag[idx].value_raw == true)
                                        {
                                            if ((DateTime.Now - Program.IO.DI.Tag[idx].dt_off).TotalSeconds >= level_sensor_change_delay)
                                            {
                                                Program.IO.DI.Tag[idx].value = Program.IO.DI.Tag[idx].value_raw;
                                            }
                                            Program.IO.DI.Tag[idx].dt_on = DateTime.Now;
                                        }
                                        else
                                        {
                                            if ((DateTime.Now - Program.IO.DI.Tag[idx].dt_on).TotalSeconds >= level_sensor_change_delay)
                                            {
                                                Program.IO.DI.Tag[idx].value = Program.IO.DI.Tag[idx].value_raw;
                                            }
                                            Program.IO.DI.Tag[idx].dt_off = DateTime.Now;
                                        }
                                    }
                                    else
                                    {

                                    }

                                }


                                if (Program.IO.DI.Tag[idx].value_raw == true)
                                {
                                    Program.IO.DI.Tag[idx].dt_on = DateTime.Now;
                                }
                                else
                                {
                                    Program.IO.DI.Tag[idx].dt_off = DateTime.Now;
                                }


                                log_di = log_di + "," + Convert.ToByte(Program.IO.DI.Tag[idx].value_raw);
                            }
                            if (log_di != log_di_old) { Program.log_md.LogWrite(log_di, Module_Log.enumLog.DigitalData_In, "", Module_Log.enumLevel.ALWAYS); }
                            log_di_old = log_di;

                            System.Threading.Thread.Sleep(1);
                            //DO Data Mapping
                            log_do = "";
                            Program.COMI_ETHERCAT.DO_Read(ref read_do);
                            for (int idx = 0; idx < Program.IO.DO.use_cnt; idx++)
                            {
                                Program.IO.DO.Tag[idx].value = read_do[Program.IO.DO.Tag[idx].address];
                                log_do = log_do + "," + Convert.ToByte(Program.IO.DO.Tag[idx].value);
                            }
                            if (log_do != log_do_old) { Program.log_md.LogWrite(log_do, Module_Log.enumLog.DigitalData_Out, "", Module_Log.enumLevel.ALWAYS); }
                            log_do_old = log_do;

                            System.Threading.Thread.Sleep(1);
                            //Analog In Mapping

                            Program.COMI_ETHERCAT.AI_Read(ref read_ai);
                            for (int idx = 0; idx < Program.IO.AI.use_cnt; idx++)
                            {
                                Program.IO.AI.Tag[idx].value_raw = read_ai[Program.IO.AI.Tag[idx].address];
                                if (Program.IO.AI.Tag[idx].gain == 0 && Program.IO.AI.Tag[idx].offset == 0)
                                {
                                    Program.IO.AI.Tag[idx].value = 0;
                                }
                                else
                                {
                                    if (idx == (int)Config_IO.enum_ai.EXHAUST)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CIRCULATION_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CONCENTRATION_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.MAIN_PCW_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.SUPPLY_A_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.SUPPLY_B_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.SUPPLY_A_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.SUPPLY_B_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CIRCULATION_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.NH4OH_SUPPLY_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.NH4OH_SUPPLY_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.HOT_DIW_SUPPLY_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.DIW_SUPPLY_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.DSP_SUPPLY_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.DSP_SUPPLY_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.H2SO4_SUPPLY_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.H2SO4_SUPPLY_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.HEATER_N2_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.RECLAIM_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.RECLAIM_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.LAL_SUPPLY_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.LAL_SUPPLY_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CHEMICAL_RETURN_A)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CHEMICAL_RETURN_B)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.TANKA_PN2_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.TANKB_PN2_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.HEATER_PN2_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.TANK_PN2_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.MAIN_PN2_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.MAIN_CDA1_PRESS_PUMP)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.MAIN_CDA2_PRESS_SOL)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.MAIN_CDA3_PRESS_DRAIN)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.DRAIN_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.HDIW_FLOW_MONITORING)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.SUPPLY_A_THERMOSTAT_PCW_FLOW || idx == (int)Config_IO.enum_ai.SUPPLY_B_THERMOSTAT_PCW_FLOW || idx == (int)Config_IO.enum_ai.CIRCULATION_THERMOSTAT_PCW_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CM_SOL_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CM_PUMP_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CM_DIW_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CM_SAMPLING_PRESS)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                        Program.IO.AI.Tag[idx].value = Program.IO.AI.Tag[idx].value * 0.1;
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CM_SAMPLING_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else if (idx == (int)Config_IO.enum_ai.CM_DIW_FLOW)
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                    else
                                    {
                                        Program.IO.AI.Tag[idx].value = Math.Round(((Program.IO.AI.Tag[idx].value_raw * 0.001) * Program.IO.AI.Tag[idx].gain) + Program.IO.AI.Tag[idx].offset, 4);
                                    }
                                }
                            }
                            System.Threading.Thread.Sleep(1);
                            //Analog Out Mapping
                            Program.COMI_ETHERCAT.AO_Read(ref read_ao);
                            for (int idx = 0; idx < Program.IO.AO.use_cnt; idx++)
                            {
                                Program.IO.AO.Tag[idx].value_raw = read_ao[Program.IO.AO.Tag[idx].address];
                                if (Program.IO.AO.Tag[idx].gain == 0 && Program.IO.AO.Tag[idx].offset == 0)
                                {
                                    Program.IO.AO.Tag[idx].value = 0;
                                }
                                else
                                {
                                    Program.IO.AO.Tag[idx].value = Math.Round(((Program.IO.AO.Tag[idx].value_raw * 0.001) * Program.IO.AO.Tag[idx].gain), 3) + Program.IO.AO.Tag[idx].offset;
                                }
                            }
                            System.Threading.Thread.Sleep(1);

                        }
                    }

                    System.Threading.Thread.Sleep(20);
                }

            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Ethercat_commi_Rcv_AD" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        public string Commi_Serial_Setting()
        {
            string result = "";
            var serial_port = serialPort1;

            try
            {
                //Serial Setting 초기 1회만 수행함
                for (int idx = 0; idx < Program.IO.SERIAL.use_cnt; idx++)
                {
                    if (idx == 0) { serial_port = serialPort1; }
                    else if (idx == 1) { serial_port = serialPort2; }
                    else if (idx == 2) { serial_port = serialPort3; }
                    else if (idx == 3) { serial_port = serialPort4; }
                    else if (idx == 4) { serial_port = serialPort5; }
                    else if (idx == 5) { serial_port = serialPort6; }
                    else if (idx == 6) { serial_port = serialPort7; }
                    else if (idx == 7) { serial_port = serialPort8; }
                    else if (idx == 8) { serial_port = serialPort9; }
                    else if (idx == 9) { serial_port = serialPort10; }

                    result = Serial_Parameter_Setting(ref serial_port, idx);
                    if (result == "")
                    {

                    }
                    else
                    {
                        Program.log_md.LogWrite(this.Name + "." + "Commi_Serial_Setting" + "." + result, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                    }
                }
                result = "";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + "." + "Commi_Serial_Setting" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);

            }
            finally { }

            return result;
        }
        public string Commi_Serial_Setting(int idx)
        {
            string result = "";
            var serial_port = serialPort1;

            try
            {
                if (idx == 0) { serial_port = serialPort1; }
                else if (idx == 1) { serial_port = serialPort2; }
                else if (idx == 2) { serial_port = serialPort3; }
                else if (idx == 3) { serial_port = serialPort4; }
                else if (idx == 4) { serial_port = serialPort5; }
                else if (idx == 5) { serial_port = serialPort6; }
                else if (idx == 6) { serial_port = serialPort7; }
                else if (idx == 7) { serial_port = serialPort8; }
                else if (idx == 8) { serial_port = serialPort9; }
                else if (idx == 9) { serial_port = serialPort10; }

                result = Serial_Parameter_Setting(ref serial_port, idx);


                result = "";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + "." + "Commi_Serial_Setting." + "Open NG(Comport : " + Program.IO.SERIAL.Tag[idx].comport + ")", Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);

            }
            finally { }

            return result;
        }
        public string Serial_Parameter_Setting(ref System.IO.Ports.SerialPort serial_port, int idx)
        {
            string result = "", log = "";
            try
            {
                if (Program.IO.SERIAL.Tag[idx].use == true)
                {
                    //baudrate
                    serial_port.BaudRate = Convert.ToInt32(Program.IO.SERIAL.Tag[idx].buadrate.ToString().Replace("_", ""));
                    //parity
                    if (Program.IO.SERIAL.Tag[idx].parity.ToString() == "none") { serial_port.Parity = System.IO.Ports.Parity.None; }
                    else if (Program.IO.SERIAL.Tag[idx].parity.ToString() == "odd") { serial_port.Parity = System.IO.Ports.Parity.Odd; }
                    else if (Program.IO.SERIAL.Tag[idx].parity.ToString() == "even") { serial_port.Parity = System.IO.Ports.Parity.Even; }
                    else { serial_port.Parity = System.IO.Ports.Parity.None; }

                    //stopbit
                    if (Program.IO.SERIAL.Tag[idx].stopbit.ToString() == "bit1") { serial_port.StopBits = System.IO.Ports.StopBits.One; }
                    else if (Program.IO.SERIAL.Tag[idx].stopbit.ToString() == "bit2") { serial_port.StopBits = System.IO.Ports.StopBits.Two; }
                    else { serial_port.StopBits = System.IO.Ports.StopBits.One; }

                    //databit
                    if (Program.IO.SERIAL.Tag[idx].databit.ToString() == "bit8") { serial_port.DataBits = 8; }
                    else if (Program.IO.SERIAL.Tag[idx].databit.ToString() == "bit7") { serial_port.DataBits = 7; }
                    else { serial_port.DataBits = 8; }

                    //port open
                    if (serial_port.IsOpen == true) { serial_port.Close(); }

                    if(Program.cg_app_info.mode_simulation.use == true)
                    {
                        serial_port_state[idx] = true;
                    }
                    else
                    {
                        //comport
                        serial_port.PortName = "COM" + Program.IO.SERIAL.Tag[idx].comport;
                        serial_port.Open();
                    }
                    
                    //serial_port_state[idx] = true;
                    result = "";

                    log = "Open OK(" + Program.IO.SERIAL.Tag[idx].comport + ", " + Program.IO.SERIAL.Tag[idx].description + ")";
                    Serial_Log_Write(idx, log);

                }

            }
            catch (Exception ex)
            {
                serial_port_state[idx] = false;
                log = "Open Fail(" + Program.IO.SERIAL.Tag[idx].comport + ", " + Program.IO.SERIAL.Tag[idx].description + ")";
                result = log;
                Serial_Log_Write(idx, log);
            }
            finally { }

            return result;
        }
        public void Serial_Log_Write(int idx, string log)
        {
            if (idx == 0)
            {
                Program.log_md.LogWrite(log, Module_Log.enumLog.SERAL_DATA_1, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (idx == 1)
            {
                Program.log_md.LogWrite(log, Module_Log.enumLog.SERAL_DATA_2, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (idx == 2)
            {
                Program.log_md.LogWrite(log, Module_Log.enumLog.SERAL_DATA_3, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (idx == 3)
            {
                Program.log_md.LogWrite(log, Module_Log.enumLog.SERAL_DATA_4, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (idx == 4)
            {
                Program.log_md.LogWrite(log, Module_Log.enumLog.SERAL_DATA_5, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (idx == 5)
            {
                Program.log_md.LogWrite(log, Module_Log.enumLog.SERAL_DATA_6, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (idx == 6)
            {
                Program.log_md.LogWrite(log, Module_Log.enumLog.SERAL_DATA_7, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (idx == 7)
            {
                Program.log_md.LogWrite(log, Module_Log.enumLog.SERAL_DATA_8, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (idx == 8)
            {
                Program.log_md.LogWrite(log, Module_Log.enumLog.SERAL_DATA_9, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (idx == 9)
            {
                Program.log_md.LogWrite(log, Module_Log.enumLog.SERAL_DATA_10, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        /// <summary>
        /// daemon Serial Data Rcv Multi Thread
        /// </summary>
        /// <param name="idx_serial"></param>
        public void Send_ABB()
        {
            byte[] send_msg = null;
            byte[] read_q = null;
            string result = "";
            int delay_rcv_from_send = 1000;

            TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info connection_info = new TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info();
            connection_info.ip = Program.cg_socket.abb_ip;
            connection_info.port = Program.cg_socket.abb_port;

            string log = "", log_dt_head = "";

            try
            {

                if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    while (true)
                    {
                        if (this.abb_auto_snd == true)
                        {
                            if ((DateTime.Now - this.dt_abb_auto_send_last).TotalMilliseconds >= 5000)
                            {
                                this.dt_abb_auto_send_last = DateTime.Now;
                                //1회 전송 시 응답안됨, 2회 전송해야 응답되며, 실제 농도계 설치 완료 후 확인 필요
                                Program.ABB.Message_Command_To_Byte(Class_ABB.read_property_value1_to_4);
                                System.Threading.Thread.Sleep(200);
                                //1회 전송 시 응답안됨, 2회 전송해야 응답되며, 실제 농도계 설치 완료 후 확인 필요
                                Program.ABB.Message_Command_To_Byte(Class_ABB.read_DO_Status);
                            }
                        }

                        if (this.abb_q_snd_data.Count > 10)
                        {
                            //queue가 10개 이상이면, 비정상 동작으로 간주한다.
                            Program.log_md.LogWrite(this.Name + "." + "Rcv_Send_ABB Queue Count > 10 : ", Module_Log.enumLog.ABB_DATA, "", Module_Log.enumLevel.ALWAYS);
                            this.abb_q_snd_data.Clear();
                        }
                        else if (this.abb_q_snd_data.Count > 0)
                        {
                            //Q Read
                            read_q = this.abb_q_snd_data.Dequeue();

                            //Send
                            if (result == "")
                            {
                                Program.SOCKET.SendMsg(connection_info, read_q);
                                if (this.log_abb_q_snd_data.Count > log_q_count) { this.log_abb_q_snd_data.Dequeue(); }

                                log_dt_head = DateTime.Now.ToString("HH:mm:ss.fff");
                                if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.HEX)
                                {
                                    log = BitConverter.ToString(read_q).Replace("-", " ");
                                }
                                else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.BYTE)
                                {
                                    log = String.Join(" ", read_q);
                                }
                                else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.STRING)
                                {
                                    log = Encoding.Default.GetString(read_q);
                                }

                                if (this.log_abb_q_snd_data.Count > log_q_count) { this.log_abb_q_snd_data.Dequeue(); }
                                this.log_abb_q_snd_data.Enqueue(log_dt_head + " : " + log);
                                Program.log_md.LogWrite("Send," + log, Module_Log.enumLog.ABB_DATA, "", Module_Log.enumLevel.ALWAYS);
                            }
                            else { }
                        }
                        else
                        {
                            break;
                        }
                        Thread.Sleep(100);

                    }
                }
            
            }

            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + "." + "Send_ABB" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        public void Send_Heat_Exchanger()
        {
            byte[] send_msg = null;
            byte[] read_q = null;
            string result = "";
            int delay_rcv_from_send = 500;

            TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info connection_info = new TCP_IP_SOCKET.Cls_Socket.Socket_Connection_Info();
            connection_info.ip = Program.cg_socket.heat_exchanger_ip;
            connection_info.port = Program.cg_socket.heat_exchanger_port;

            string log = "", log_dt_head = "";
            DateTime watchdog_send_time = DateTime.Now;
            bool watchdog_flag = false;
            try
            {

                if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    while (true)
                    {
                        if (this.heat_exchanger_auto_snd == true)
                        {
                            if ((DateTime.Now - this.dt_heat_exchanger_auto_send_last).TotalMilliseconds >= delay_rcv_from_send)
                            {
                                this.dt_heat_exchanger_auto_send_last = DateTime.Now;
                                Program.Heat_Exchanger.Message_Command_To_Byte(Class_HeatExchanger.Read_AllData);

                                if ((DateTime.Now - watchdog_send_time).TotalMilliseconds >= 1000)
                                {
                                    watchdog_send_time = DateTime.Now;
                                    if (watchdog_flag == true)
                                    {
                                        watchdog_flag = false;
                                        Program.Heat_Exchanger.WatchDog(true);
                                    }
                                    else
                                    {
                                        watchdog_flag = true;
                                        Program.Heat_Exchanger.WatchDog(false);
                                    }
                                }
                            }
                        }

                        if (this.heat_exchanger_q_snd_data.Count > 10)
                        {
                            //queue가 10개 이상이면, 비정상 동작으로 간주한다.
                            Program.log_md.LogWrite(this.Name + "." + "Send_Heat_Exchanger Queue Count > 10 : ", Module_Log.enumLog.HEAT_EXCHANGER, "", Module_Log.enumLevel.ALWAYS);
                            this.heat_exchanger_q_snd_data.Clear();
                        }
                        else if (this.heat_exchanger_q_snd_data.Count > 0)
                        {
                            //Q Read
                            read_q = this.heat_exchanger_q_snd_data.Dequeue();

                            //Send
                            if (result == "")
                            {
                                Program.SOCKET.SendMsg(connection_info, read_q);
                                if (this.log_heat_exchanger_q_snd_data.Count > log_q_count) { this.log_heat_exchanger_q_snd_data.Dequeue(); }

                                log_dt_head = DateTime.Now.ToString("HH:mm:ss.fff");
                                if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.HEX)
                                {
                                    log = BitConverter.ToString(read_q).Replace("-", " ");
                                }
                                else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.BYTE)
                                {
                                    log = String.Join(" ", read_q);
                                }
                                else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.STRING)
                                {
                                    log = Encoding.Default.GetString(read_q);
                                }

                                if (this.log_heat_exchanger_q_snd_data.Count > log_q_count) { this.log_heat_exchanger_q_snd_data.Dequeue(); }
                                this.log_heat_exchanger_q_snd_data.Enqueue(log_dt_head + " : " + log);
                                Program.log_md.LogWrite("Send," + log, Module_Log.enumLog.HEAT_EXCHANGER, "", Module_Log.enumLevel.ALWAYS);
                            }
                            else { }
                        }
                        else
                        {
                            //break;
                        }
                        Thread.Sleep(50);


                        //Console.WriteLine("DD");
                    }
                }

            }

            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + "." + "Send_Heat_Exchanger" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        public void Serial_data_Enqueue(ref byte[] send_msg, int idx_serial)
        {
            //if (this.serial_port_state[idx_serial] == true)
            //{
            //}
            if (send_msg != null) { this.serial_q_snd_data[idx_serial].Enqueue(send_msg); };

        }
        public void ABB_data_Enqueue(ref byte[] send_msg)
        {
            if (send_msg != null) { this.abb_q_snd_data.Enqueue(send_msg); };
        }
        /// <summary>
        /// daemon Serial Data Rcv Multi Thread
        /// </summary>
        /// <param name="idx_serial"></param>
        /// 
        public bool Rcv_Send_Func(int idx_serial, int interval_rcv_from_send, ref System.IO.Ports.SerialPort serial_port)
        {
            bool queue_Empty = false; //queue가 없으면 또는 Error면 true
            byte[] read_q = new byte[0];
            string log = "", log_dt_head = "";
            byte[] rcv_msg = new byte[0];
            int rcv_byte_cnt = 0;
            string result = "";
            string data_tmp = "";
            string parse = "";
            Class_Serial_Common.Rcv_Data rcv_data_parse = new Class_Serial_Common.Rcv_Data();
            try
            {
                if (this.serial_q_snd_data[idx_serial].Count > 10)
                {
                    //queue가 10개 이상이면, 비정상 동작으로 간주한다.
                    Serial_Log_Write(idx_serial, "Ethercat_commi_Snd Queue Count > 10");
                    this.serial_q_snd_data[idx_serial].Clear();
                }
                else if (this.serial_q_snd_data[idx_serial].Count > 0)
                {
                    //Q Read
                    read_q = this.serial_q_snd_data[idx_serial].Dequeue();
                    if (serial_port.IsOpen == true && read_q != null)
                    {
                        //Send
                        result = "";
                        serial_port.Write(read_q, 0, read_q.Length);
                        log_dt_head = DateTime.Now.ToString("HH:mm:ss.fff");
                        //Send Data Log
                        if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.HEX)
                        {
                            log = BitConverter.ToString(read_q).Replace("-", " ");
                        }
                        else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.BYTE)
                        {
                            log = String.Join(" ", read_q);
                        }
                        else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.STRING)
                        {
                            log = Encoding.Default.GetString(read_q);
                        }
                        if (this.log_serial_q_snd_data[idx_serial].Count > log_q_count) { this.log_serial_q_snd_data[idx_serial].Dequeue(); }
                        this.log_serial_q_snd_data[idx_serial].Enqueue(log_dt_head + " : " + log);
                        Serial_Log_Write(idx_serial, "Send," + log);
                        //Interval
                        System.Threading.Thread.Sleep(interval_rcv_from_send);
                        //Rcv
                        rcv_byte_cnt = serial_port.BytesToRead;
                        if (rcv_byte_cnt == 0)
                        {

                        }
                        else
                        {
                            dt_serial_validate_check[idx_serial] = DateTime.Now;
                            Array.Resize(ref rcv_msg, rcv_byte_cnt);
                            serial_port.Read(rcv_msg, 0, rcv_byte_cnt);
                            rcv_data_parse.read_data_byte_cnt = rcv_byte_cnt;
                            log_dt_head = DateTime.Now.ToString("HH:mm:ss.fff");
                            //Data Parsing
                            if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.PUMP_CONTROLLER_PB12)
                            {
                                if (Program.IO.SERIAL.Tag[idx_serial].description.IndexOf("A") >= 0)
                                {
                                    result = Program.PumpController_PB12.Recieve_Data_To_Parse_PB12(Class_PumpController_PB12.enum_call_by.SUPPLY_A, Encoding.UTF8.GetString(rcv_msg), ref parse, ref rcv_data_parse);
                                }
                                else if (Program.IO.SERIAL.Tag[idx_serial].description.IndexOf("B") >= 0)
                                {
                                    result = Program.PumpController_PB12.Recieve_Data_To_Parse_PB12(Class_PumpController_PB12.enum_call_by.SUPPLY_B, Encoding.UTF8.GetString(rcv_msg), ref parse, ref rcv_data_parse);
                                }
                            }
                            else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.CONCENTRATION_CM210A_DC)
                            {
                                result = Program.CM210DC.Recieve_Data_To_Parse_CM210(Encoding.UTF8.GetString(rcv_msg), ref parse, ref rcv_data_parse);
                            }
                            else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.CONCENTRATION_CS600F)
                            {
                                result = Program.CS600F.Recieve_Data_To_Parse_CS600F(Encoding.UTF8.GetString(rcv_msg), ref parse, ref rcv_data_parse);
                            }
                            else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.CONCENTRATION_CS_150C)
                            {
                                result = Program.CS150C.Recieve_Data_To_Parse_CS150C(Encoding.UTF8.GetString(rcv_msg), ref parse, ref rcv_data_parse);
                            }
                            else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.CONCENTRATION_HF_700)
                            {
                                result = Program.HF700.Recieve_Data_To_Parse_HF700(Encoding.UTF8.GetString(rcv_msg), ref parse, ref rcv_data_parse);
                            }
                            else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.TEMP_CONTROLLER_M74)
                            {
                                result = Program.TempController_M74.Recieve_Data_To_Parse_M74(Class_TempController_M74.enum_call_by.M74_1, Encoding.UTF8.GetString(rcv_msg), ref parse, ref rcv_data_parse);
                            }
                            else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.TEMP_CONTROLLER_M74R)
                            {
                                result = Program.TempController_M74.Recieve_Data_To_Parse_M74(Class_TempController_M74.enum_call_by.M74_2, Encoding.UTF8.GetString(rcv_msg), ref parse, ref rcv_data_parse);
                            }
                            else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.SCR_DPU31A_025A)
                            {
                                result = Program.SCR_DPU31A_025A.Recieve_Data_To_Parse_DPU31A_025A(ref rcv_msg, ref rcv_data_parse);
                            }
                            else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.FLOWMETER_USF500)
                            {
                                result = Program.FlowMeter_USF500.Recieve_Data_To_Parse_USF500(ref rcv_msg, ref rcv_data_parse);
                            }
                            else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.THERMOSTAT_HE_3320C)
                            {
                                result = Program.ThermoStart_HE_3320C.Recieve_Data_To_Parse_HE_3320C(idx_serial, rcv_msg, ref parse, ref rcv_data_parse);
                            }
                            else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.FLOWMETER_SONOTEC)
                            {
                                result = Program.FlowMeter_SONOTEC.Recieve_Data_By_SONOTEC(ref rcv_msg, ref rcv_data_parse);
                            }

                            //Rcv Data Log
                            dt_rcv_serial_last[idx_serial] = DateTime.Now;
                            log_dt_head = DateTime.Now.ToString("HH:mm:ss.fff");
                            if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.HEX)
                            {
                                log = BitConverter.ToString(rcv_msg).Replace("-", " ");
                            }
                            else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.BYTE)
                            {
                                log = String.Join(" ", rcv_msg);
                            }
                            else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.STRING)
                            {
                                log = Encoding.Default.GetString(rcv_msg);
                            }
                            Serial_Log_Write(idx_serial, "Parse," + result + ",Rcv" + "," + log);
                            if (this.log_serial_q_rcv_data[idx_serial].Count > log_q_count) { this.log_serial_q_rcv_data[idx_serial].Dequeue(); }
                            this.log_serial_q_rcv_data[idx_serial].Enqueue(log_dt_head + " : " + log);
                        }

                    }
                    else
                    {
                        serial_port_state[idx_serial] = false;
                        queue_Empty = true;
                    }

                }

                else
                {
                    if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.CONCENTRATION_CM210A_DC)
                    {
                        if (serial_port.IsOpen == true)
                        {
                            rcv_byte_cnt = serial_port.BytesToRead;
                            if (rcv_byte_cnt == 0)
                            {

                            }
                            else
                            {
                                dt_serial_validate_check[idx_serial] = DateTime.Now;
                                //Array.Resize(ref rcv_msg, rcv_byte_cnt);
                                //serial_port.Read(rcv_msg, 0, rcv_byte_cnt);
                                //rcv_data_parse.read_data_byte_cnt = rcv_byte_cnt;
                                log_dt_head = DateTime.Now.ToString("HH:mm:ss.fff");
                                data_tmp = serial_port.ReadLine();
                                rcv_msg = Encoding.Default.GetBytes(data_tmp);
                                //HF,0.000,wt%,TEMP, 25.2
                                result = Program.CM210DC.Recieve_Data_To_Parse_CM210(data_tmp, ref parse, ref rcv_data_parse);
                                //Rcv Data Log
                                dt_rcv_serial_last[idx_serial] = DateTime.Now;
                                log_dt_head = DateTime.Now.ToString("HH:mm:ss.fff");
                                if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.HEX)
                                {
                                    log = BitConverter.ToString(rcv_msg).Replace("-", " ");
                                }
                                else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.BYTE)
                                {
                                    log = String.Join(" ", rcv_msg);
                                }
                                else if (Program.cg_app_info.log_serial_view_type == enum_serial_view_type.STRING)
                                {
                                    log = Encoding.Default.GetString(rcv_msg);
                                }
                                Serial_Log_Write(idx_serial, "Parse," + result + ",Rcv" + "," + log);
                                if (this.log_serial_q_rcv_data[idx_serial].Count > log_q_count) { this.log_serial_q_rcv_data[idx_serial].Dequeue(); }
                                this.log_serial_q_rcv_data[idx_serial].Enqueue(log_dt_head + " : " + log);
                            }
                        }

                    }
                    else
                    {
                        queue_Empty = true;
                    }
                }

                //일정 시간 Date Rcv가 없으면 Serial Close로 결정 짓는다
                if ((DateTime.Now - dt_serial_validate_check[idx_serial]).TotalSeconds >= Program.cg_app_info.serial_connect_time)
                {
                    //Port Open 정상 시간 기록, Data 수신
                    //dt_serial_validate_check[idx_serial] = DateTime.Now;
                    serial_port_state[idx_serial] = false;
                    //Port Open 재 요청
                    //Commi_Serial_Setting(idx_serial);
                }
                else
                {
                    serial_port_state[idx_serial] = true;
                }

            }
            catch (Exception ex)
            {
                queue_Empty = true;
            }
            finally
            {

            }
            return queue_Empty;
        }
        public void Ethercat_commi_Rcv_Send_Serial(int idx_serial)
        {
            var serial_port = serialPort1;
            int delay_rcv_from_send = 100;
            try
            {
                //Serial 설정 시 가상 데몬 시리얼 설정, AES-CBC Node설정, 컴포트 설정 3가지를 동일하게 수정해야함
                //각 Chemical별 Serial Config는 고정
                Program.log_md.LogWrite(this.Name + "." + "Ethercat_commi_Rcv_Send_Serial Multi Thread Start : " + idx_serial, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                while (true)
                {
                    if (Program.COMI_ETHERCAT.comi_dll_Initilized == true || Program.cg_apploading.load_complete == true)
                    {
                        if (idx_serial == 0) { serial_port = serialPort1; }
                        else if (idx_serial == 1) { serial_port = serialPort2; }
                        else if (idx_serial == 2) { serial_port = serialPort3; }
                        else if (idx_serial == 3) { serial_port = serialPort4; }
                        else if (idx_serial == 4) { serial_port = serialPort5; }
                        else if (idx_serial == 5) { serial_port = serialPort6; }
                        else if (idx_serial == 6) { serial_port = serialPort7; }
                        else if (idx_serial == 7) { serial_port = serialPort8; }
                        else if (idx_serial == 8) { serial_port = serialPort9; }
                        else if (idx_serial == 9) { serial_port = serialPort10; }

                        if (idx_serial != -1)
                        {
                            if (Program.cg_app_info.eq_type != enum_eq_type.none)
                            {
                                if (Program.IO.SERIAL.Tag[idx_serial] != null && Program.IO.SERIAL.Tag[idx_serial].use == true)
                                {
                                    if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.THERMOSTAT_HE_3320C)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            if ((DateTime.Now - this.dt_auto_send_last[idx_serial]).TotalMilliseconds >= 500)
                                            {
                                                this.dt_auto_send_last[idx_serial] = DateTime.Now;
                                                Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.read_supply_pv, idx_serial);
                                            }
                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send * 5, ref serial_port) == true)
                                            {
                                                break;
                                            }

                                        }

                                    }
                                    else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.FLOWMETER_USF500)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            if ((DateTime.Now - this.dt_auto_send_last[idx_serial]).TotalMilliseconds >= 50)
                                            {
                                                this.dt_auto_send_last[idx_serial] = DateTime.Now;
                                                if (Program.FlowMeter_USF500.falg_count >= 5)
                                                {
                                                    Program.FlowMeter_USF500.falg_count = 0;
                                                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 2, Class_FlowMeter_USF500.read_ch2_flow_Status, idx_serial);
                                                }
                                                else if (Program.FlowMeter_USF500.falg_count >= 4)
                                                {
                                                    Program.FlowMeter_USF500.falg_count = Program.FlowMeter_USF500.falg_count + 1;
                                                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 1, Class_FlowMeter_USF500.read_ch1_flow_Status, idx_serial);
                                                }
                                                else if (Program.FlowMeter_USF500.falg_count >= 3)
                                                {
                                                    Program.FlowMeter_USF500.falg_count = Program.FlowMeter_USF500.falg_count + 1;
                                                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 0, Class_FlowMeter_USF500.read_Status, idx_serial);
                                                    //CH2 Not Use
                                                    if (Program.IO.SERIAL.Tag[idx_serial].ch_total_cnt == 1) { Program.FlowMeter_USF500.falg_count = 0; }
                                                }
                                                else if (Program.FlowMeter_USF500.falg_count >= 2)
                                                {
                                                    Program.FlowMeter_USF500.falg_count = Program.FlowMeter_USF500.falg_count + 1;
                                                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 2, Class_FlowMeter_USF500.read_ch2_flow_Status, idx_serial);
                                                }
                                                else if (Program.FlowMeter_USF500.falg_count >= 1)
                                                {
                                                    Program.FlowMeter_USF500.falg_count = Program.FlowMeter_USF500.falg_count + 1;
                                                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_USF500.read_ch1_flow_Status, idx_serial);
                                                }
                                                else if (Program.FlowMeter_USF500.falg_count >= 0)
                                                {
                                                    Program.FlowMeter_USF500.falg_count = Program.FlowMeter_USF500.falg_count + 1;
                                                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 0, Class_FlowMeter_USF500.read_Status, idx_serial);
                                                }

                                            }

                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send, ref serial_port) == true)
                                            {
                                                break;
                                            }


                                        }

                                    }
                                    else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.SCR_DPU31A_025A)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            if ((DateTime.Now - this.dt_auto_send_last[idx_serial]).TotalMilliseconds >= 1000)
                                            {
                                                this.dt_auto_send_last[idx_serial] = DateTime.Now;
                                                for (int idx_ch_cnt = 0; idx_ch_cnt < Program.IO.SERIAL.Tag[idx_serial].ch_total_cnt; idx_ch_cnt++)
                                                {
                                                    if (idx_ch_cnt == 0) { Program.SCR_DPU31A_025A.Message_Command_Apply_CRC_TO_Send(1, Class_SCR_DPU31A_025A.read_operation, idx_serial); }
                                                    if (idx_ch_cnt == 1) { Program.SCR_DPU31A_025A.Message_Command_Apply_CRC_TO_Send(2, Class_SCR_DPU31A_025A.read_operation, idx_serial); }
                                                    if (idx_ch_cnt == 2) { Program.SCR_DPU31A_025A.Message_Command_Apply_CRC_TO_Send(3, Class_SCR_DPU31A_025A.read_operation, idx_serial); }
                                                    if (idx_ch_cnt == 3) { Program.SCR_DPU31A_025A.Message_Command_Apply_CRC_TO_Send(4, Class_SCR_DPU31A_025A.read_operation, idx_serial); }
                                                }

                                            }

                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send, ref serial_port) == true)
                                            {
                                                break;
                                            }

                                        }

                                    }
                                    else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.FLOWMETER_SONOTEC)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            if ((DateTime.Now - this.dt_auto_send_last[idx_serial]).TotalMilliseconds >= 500)
                                            {
                                                this.dt_auto_send_last[idx_serial] = DateTime.Now;
                                                Program.FlowMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.read_status_flow_totalusage, idx_serial);
                                            }
                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send, ref serial_port) == true)
                                            {
                                                break;
                                            }
                                        }

                                    }
                                    else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.TEMP_CONTROLLER_M74)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            if ((DateTime.Now - this.dt_auto_send_last[idx_serial]).TotalMilliseconds >= 1000)
                                            {
                                                this.dt_auto_send_last[idx_serial] = DateTime.Now;
                                                if (Program.IO.SERIAL.Tag[idx_serial].ch_total_cnt == 1)
                                                {
                                                    Program.TempController_M74.Message_Command_To_Byte_M74_TO_Send(1, Class_TempController_M74.command_CH1_To_CH4_Data_Read_SIMPLE, idx_serial);
                                                }
                                                if (Program.IO.SERIAL.Tag[idx_serial].ch_total_cnt == 2)
                                                {
                                                    Program.TempController_M74.Message_Command_To_Byte_M74_TO_Send(1, Class_TempController_M74.command_CH1_To_CH4_Data_Read_SIMPLE, idx_serial);
                                                    Program.TempController_M74.Message_Command_To_Byte_M74_TO_Send(2, Class_TempController_M74.command_CH1_To_CH4_Data_Read_SIMPLE, idx_serial);
                                                }
                                            }
                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send * 2, ref serial_port) == true)
                                            {
                                                break;
                                            }
                                        }

                                    }
                                    else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.TEMP_CONTROLLER_M74R)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            if ((DateTime.Now - this.dt_auto_send_last[idx_serial]).TotalMilliseconds >= 1000)
                                            {
                                                this.dt_auto_send_last[idx_serial] = DateTime.Now;
                                                if (Program.IO.SERIAL.Tag[idx_serial].ch_total_cnt == 1)
                                                {
                                                    Program.TempController_M74.Message_Command_To_Byte_M74_TO_Send(3, Class_TempController_M74.command_CH1_To_CH4_Data_Read_SIMPLE, idx_serial);
                                                }
                                            }
                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send, ref serial_port) == true)
                                            {
                                                break;
                                            }

                                        }

                                    }
                                    else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.PUMP_CONTROLLER_PB12)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            if ((DateTime.Now - this.dt_auto_send_last[idx_serial]).TotalMilliseconds >= 1000)
                                            {
                                                this.dt_auto_send_last[idx_serial] = DateTime.Now;
                                                Program.PumpController_PB12.Message_Command_To_Byte_BP12_TO_Send(Class_PumpController_PB12.read_total_stroke_count, idx_serial);
                                            }
                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send, ref serial_port) == true)
                                            {
                                                break;
                                            }

                                        }
                                    }
                                    else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.CONCENTRATION_CM210A_DC)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            //CM210, CS600F은 1초마다 주기적으로 데이터가 수신된다
                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send, ref serial_port) == true)
                                            {
                                                break;
                                            }

                                        }

                                    }
                                    else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.CONCENTRATION_CS600F)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            if ((DateTime.Now - this.dt_auto_send_last[idx_serial]).TotalMilliseconds >= 1000)
                                            {
                                                this.dt_auto_send_last[idx_serial] = DateTime.Now;
                                                Program.CS600F.Message_Command_To_Byte_CS600F_TO_Send(Class_Concentration_CS600F.command_Read_Data, idx_serial);
                                            }
                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send, ref serial_port) == true)
                                            {
                                                break;
                                            }

                                        }

                                    }
                                    else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.CONCENTRATION_CS_150C)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            if ((DateTime.Now - this.dt_auto_send_last[idx_serial]).TotalMilliseconds >= 1000)
                                            {
                                                this.dt_auto_send_last[idx_serial] = DateTime.Now;
                                                Program.CS150C.Message_Command_To_Byte_CS150C_TO_Send(Class_Concentration_CS150C.command_Read_Data, idx_serial);
                                            }
                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send, ref serial_port) == true)
                                            {
                                                break;
                                            }

                                        }

                                    }
                                    else if (Program.IO.SERIAL.Tag[idx_serial].unit == Config_IO.enum_unit.CONCENTRATION_HF_700)
                                    {
                                        if (this.serial_auto_snd[idx_serial] == true)
                                        {
                                            if ((DateTime.Now - this.dt_auto_send_last[idx_serial]).TotalMilliseconds >= 1000)
                                            {
                                                this.dt_auto_send_last[idx_serial] = DateTime.Now;
                                                Program.HF700.Message_Command_To_Byte_HF700_TO_Send(Class_Concentration_HF700.command_Read_Data, idx_serial);
                                            }
                                        }
                                        while (true)
                                        {
                                            if (Rcv_Send_Func(idx_serial, delay_rcv_from_send, ref serial_port) == true)
                                            {
                                                break;
                                            }

                                        }

                                    }
                                }
                            }
                        }



                    }
                    System.Threading.Thread.Sleep(100);
                }

            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Ethercat_commi_Rcv_Send_Serial" + ".[" + idx_serial + "] " + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally
            {
                Program.log_md.LogWrite(this.Name + "." + "Ethercat_commi_Rcv_Send_Serial Multi Thread End : " + idx_serial, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        public void Log_Hex_To_String(string description, ref byte[] data)
        {
            string log = "";
            try
            {

                if (data == null) { log = description + " : " + "Rcv Array Empty"; }
                else { log = description + " : " + System.BitConverter.ToString(data); }
                Console.WriteLine(log);
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Log_Hex_To_String" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        public void UI_Change()
        {
            string tmp_value = "";
            int serial_state_check = 0;
            try
            {
                if (st_ui_change_para.timer_initial == false)
                {
                    for (int index = 0; index < st_ui_change_para.subtimer_count; index++)
                    { st_ui_change_para.tp_check_interval[index] = new TimeSpan(0); st_ui_change_para.dt_check_last_act[index] = new DateTime(0); }
                    st_ui_change_para.timer_initial = true;
                    //Version View
                    //cls_mainModule.GetFileVersionInfo(cls_mainModule.m_str_AppPath + @"\Promethus.exe").ToString();
                }

                for (int index = 0; index < st_ui_change_para.subtimer_count; index++)
                {
                    st_ui_change_para.tp_check_interval[index] = DateTime.Now - st_ui_change_para.dt_check_last_act[index];
                    switch (index)
                    {
                        case 0:
                            if (st_ui_change_para.tp_check_interval[index].TotalMilliseconds >= 300)
                            {
                                st_ui_change_para.dt_check_last_act[index] = DateTime.Now;
                                //시간 갱신
                                lbl_time.Text = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                                //Program.log_md.terminal_log("OK", Module_Log.enumTerminal.None);
                            }
                            break;
                        case 1:
                            if (st_ui_change_para.tp_check_interval[index].TotalMilliseconds >= 5000)
                            {
                                st_ui_change_para.dt_check_last_act[index] = DateTime.Now;
                                //사용자 유휴 시간 검증
                                //사용자 로그인 / 아웃 상태 확인
                                if (Program.main_md.user_info.type != Module_User.User_Type.None)
                                {
                                    Program.main_md.Check_User_Login_Time_Expire();
                                }
                                else
                                {

                                }
                            }

                            break;
                        case 2:
                            if (st_ui_change_para.tp_check_interval[index].TotalMilliseconds >= 1000)
                            {
                                st_ui_change_para.dt_check_last_act[index] = DateTime.Now;
                                //Alarm 발생 시 표기 + Most Alarm Level 갱신
                                if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY)
                                {
                                    pic_alarm.Image = Properties.Resources.Alarm_Red;
                                }
                                else if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.LIGHT)
                                {
                                    pic_alarm.Image = Properties.Resources.Alarm_Yellow;
                                }
                                else if (Program.occured_alarm_form.most_occured_alarm_level == (int)frm_alarm.enum_level.WARNING)
                                {
                                    if (Program.occured_alarm_form.cnt_occured_alarm_total == 0)
                                    {
                                        pic_alarm.Image = Properties.Resources.Alarm_Black;
                                    }
                                    else
                                    {
                                        pic_alarm.Image = Properties.Resources.Alarm_Blue;
                                    }
                                }
                            }
                            else if (st_ui_change_para.tp_check_interval[index].TotalMilliseconds >= 500)
                            {
                                pic_alarm.Image = Properties.Resources.Alarm_Black;
                            }
                            break;

                        case 3:
                            if (st_ui_change_para.tp_check_interval[index].TotalMilliseconds >= 500)
                            {
                                st_ui_change_para.dt_check_last_act[index] = DateTime.Now;
                                //Status 표기
                                if (Program.ABB.run_state == false)
                                {
                                    pnl_status_communication_abb.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                                }
                                else
                                {
                                    if (Program.ABB.Online == true)
                                    {
                                        pnl_status_communication_abb.BackgroundImage = Properties.Resources.LED_GREEN_NONE;
                                    }
                                    else
                                    {
                                        pnl_status_communication_abb.BackgroundImage = Properties.Resources.LED_ORANGE_NONE;
                                    }
                                }

                                if (Program.CTC.run_state == false)
                                {
                                    pnl_status_communication_ctc.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                                }
                                else
                                {
                                    pnl_status_communication_ctc.BackgroundImage = Properties.Resources.LED_GREEN_NONE;
                                }

                                if (Program.ethercat_md.run_state == true && Program.COMI_ETHERCAT.comi_dll_Initilized == true)
                                {
                                    if (Check_Comi_Serial_Daemon() == true)
                                    {
                                        serial_state_check = 0;
                                        for (int idx = 0; idx < Program.IO.SERIAL.use_cnt; idx++)
                                        {
                                            if (Program.IO.SERIAL.Tag[idx].use == true)
                                            {
                                                if (this.serial_port_state[idx] == false)
                                                {
                                                    serial_state_check += 1;
                                                }
                                            }

                                        }
                                        if (serial_state_check == 0)
                                        {
                                            pnl_status_communication_ethercat.BackgroundImage = Properties.Resources.LED_GREEN_NONE;
                                            Program.ethercat_md.error_state = Module_Ethercat.enum_error_type.None;
                                        }
                                        else
                                        {
                                            pnl_status_communication_ethercat.BackgroundImage = Properties.Resources.LED_ORANGE_NONE;
                                            Program.ethercat_md.error_state = Module_Ethercat.enum_error_type.Serial_CM_Error;
                                        }

                                    }
                                    else if (Check_Comi_Serial_Daemon() == false)
                                    {
                                        pnl_status_communication_ethercat.BackgroundImage = Properties.Resources.LED_ORANGE_NONE;
                                        Program.ethercat_md.error_state = Module_Ethercat.enum_error_type.Serial_Daemon;
                                    }
                                }
                                else
                                {
                                    pnl_status_communication_ethercat.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                                    if (Program.COMI_ETHERCAT.comi_dll_Initilized == false)
                                    {
                                        Program.ethercat_md.error_state = Module_Ethercat.enum_error_type.Init_Error;
                                    }
                                    else
                                    {
                                        Program.ethercat_md.error_state = Module_Ethercat.enum_error_type.Run_Error;
                                    }

                                }
                            }
                            break;
                        case 4:
                            if (st_ui_change_para.tp_check_interval[index].TotalMilliseconds >= 500)
                            {
                                st_ui_change_para.dt_check_last_act[index] = DateTime.Now;
                                //Tank Status A 표기
                                if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == true)
                                {
                                    tmp_value = Program.tank[(int)tank_class.enum_tank_type.TANK_A].status.ToString().ToLower();
                                    lbl_tank_state_a.Text = Char.ToUpper(tmp_value[0]) + tmp_value.Substring(1);
                                    lbl_tank_state_a.BackColor = Program.tank[(int)tank_class.enum_tank_type.TANK_A].BackColor_Tank_By_Status(Program.tank[(int)tank_class.enum_tank_type.TANK_A].status);
                                    lbl_tank_state_a.ForeColor = Program.tank[(int)tank_class.enum_tank_type.TANK_A].ForeColor_Tank_By_Status(Program.tank[(int)tank_class.enum_tank_type.TANK_A].status);
                                }
                                else
                                {
                                    lbl_tank_state_a.Text = "Disable";
                                }

                                //Tank Status B 표기
                                if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == true)
                                {
                                    tmp_value = Program.tank[(int)tank_class.enum_tank_type.TANK_B].status.ToString().ToLower();
                                    lbl_tank_state_b.Text = Char.ToUpper(tmp_value[0]) + tmp_value.Substring(1);
                                    lbl_tank_state_b.BackColor = Program.tank[(int)tank_class.enum_tank_type.TANK_B].BackColor_Tank_By_Status(Program.tank[(int)tank_class.enum_tank_type.TANK_B].status);
                                    lbl_tank_state_b.ForeColor = Program.tank[(int)tank_class.enum_tank_type.TANK_B].ForeColor_Tank_By_Status(Program.tank[(int)tank_class.enum_tank_type.TANK_B].status);
                                }
                                else
                                {
                                    lbl_tank_state_b.Text = "Disable";
                                }
                                //Mode 표기
                                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                                {
                                    lbl_eq_mode.Text = "Auto";
                                    lbl_eq_mode.BackColor = Color.Lime;
                                }
                                else
                                {
                                    lbl_eq_mode.Text = "Manual";
                                    if (Program.schematic_form.timer_manual_sequence_tank_a.Enabled == true || Program.schematic_form.timer_manual_sequence_tank_b.Enabled == true)
                                    {
                                        lbl_eq_mode.BackColor = Color.Yellow;
                                    }
                                    else
                                    {
                                        lbl_eq_mode.BackColor = Color.White;
                                    }
                                }

                                //Process 중인 Tank 표기
                                if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.REFILL)
                                {
                                    lbl_process_tank.Text = "Tank A";
                                }
                                else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.REFILL)
                                {
                                    lbl_process_tank.Text = "Tank B";
                                }
                                else
                                {
                                    lbl_process_tank.Text = "None";
                                }
                            }
                            break;

                        case 5:
                            if (st_ui_change_para.tp_check_interval[index].TotalMilliseconds >= 500)
                            {
                                st_ui_change_para.dt_check_last_act[index] = DateTime.Now;

                            }

                            break;
                    }
                }

            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "ui_change" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        private void btn_overview_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                //Main Level1 button만 색상 표기
                if (btn_event.Name == "btn_monitor" || btn_event.Name == "btn_Configuration" || btn_event.Name == "btn_log" || btn_event.Name == "btn_exit")
                {
                    btn_focus(true);
                    btn_focus(btn_event, true);
                }

                //Page 상단 Header
                if (btn_event.Name == "btn_user_login")
                {
                    if (Program.main_md.user_info.type == Module_User.User_Type.None)
                    {
                        Program.popup_login.StartPosition = FormStartPosition.CenterScreen;
                        Program.popup_login.ShowDialog();
                    }
                    else
                    {
                        if (Program.main_md.Message_By_Application("Are you sure want to Logout?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                        {
                            Program.main_md.logout(ref Program.main_md.user_info);
                        }
                    }
                }
                //Page 하단 Footer
                else if (btn_event.Name == "btn_monitor")
                {
                    Program.popup_page.setting_initial(frm_popup_page.enum_call_by.monitor, btn_monitor.Size, btn_monitor.FindForm().PointToClient(btn_monitor.Parent.PointToScreen(btn_monitor.Location)));
                    //Program.popup_page.DelayAction(50, new Action(() => { Program.popup_page.Form_Acitve(true); }));
                    Program.popup_page.Form_Acitve(true);
                }
                //Configuration
                else if (btn_event.Name == "btn_Configuration")
                {
                    Program.popup_page.setting_initial(frm_popup_page.enum_call_by.configuration, btn_Configuration.Size, btn_Configuration.FindForm().PointToClient(btn_Configuration.Parent.PointToScreen(btn_Configuration.Location)));
                    Program.popup_page.Form_Acitve(true);
                    //Program.popup_page.DelayAction(50, new Action(() => { Program.popup_page.Form_Acitve(true); }));
                }
                else if (btn_event.Name == "btn_log")
                {
                    Program.popup_page.setting_initial(frm_popup_page.enum_call_by.log, btn_log.Size, btn_log.FindForm().PointToClient(btn_log.Parent.PointToScreen(btn_log.Location)));
                    Program.popup_page.Form_Acitve(true);
                    //Program.popup_page.DelayAction(5, new Action(() => { Program.popup_page.Form_Acitve(true); }));
                }
                else if (btn_event.Name == "btn_exit")
                {

                    if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK);
                    }
                    else
                    {
                        if (Program.main_md.Message_By_Application("Are you sure you want to quit the program?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                        {
                            setting_dispose(true);
                        }
                    }

                }
                if (btn_event.Name != "btn_exit")
                {
                    last_click = btn_event;
                }
            }
            Program.log_md.LogWrite(this.Name + ".btn_overview_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
            //if()
        }
        public void btn_focus(DevExpress.XtraEditors.SimpleButton btn_temp, Boolean status)
        {
            Color clr_on = Color.CornflowerBlue;
            Color clr_off = Color.Transparent;
            if (status == true)
            { btn_temp.Appearance.BackColor = clr_on; }
            else
            { btn_temp.Appearance.BackColor = clr_off; }
        }
        public void btn_focus(Boolean all_clear)
        {
            Color clr_clear = Color.Transparent;
            if (all_clear == true)
            {
                btn_monitor.Appearance.BackColor = clr_clear; btn_Configuration.Appearance.BackColor = clr_clear;
                btn_log.Appearance.BackColor = clr_clear; btn_exit.Appearance.BackColor = clr_clear;
            }
        }
        public bool Check_Logmanager()
        {
            bool result = false;

            try
            {
                if (Process.GetProcessesByName("LogManager").Length >= 1)
                {
                    result = true;
                }
                else
                {
                    Program.log_md.LogWrite(this.Name + ".check_logmanager." + "LogManager Start Request", Module_Log.enumLog.App_Info, "", Module_Log.enumLevel.ALWAYS);
                    if (System.IO.File.Exists(Application.StartupPath + @"\LogManager.exe") == true)
                    {
                        Process.Start(Application.StartupPath + @"\LogManager.exe");
                    }
                    result = false;
                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".check_logmanager." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally { }

            return result;
        }
        public void LogManager_Kill()
        {
            if (Process.GetProcessesByName("LogManager").Length >= 1)
            {
                Process.GetProcessesByName("LogManager")[0].Kill();
            }
        }
        public bool Check_Comi_Serial_Daemon()
        {
            bool result = false;

            try
            {
                if (Process.GetProcessesByName("Serial_Daemon").Length >= 1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".Check_Comi_Serial_Daemon." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally { }

            return result;
        }
        public bool bol_Send_Msg_temp = true;
        private void pic_alarm_Click(object sender, EventArgs e)
        {
            Program.main_md.Occured_Alarm_FormShow(true);
        }
        private void btn_ctc_to_cds_cc_confirm_Click(object sender, EventArgs e)
        {
            if (Program.CTC.run_state == true)
            {
                if (Program.seq.supply.CC_START_TANK == "")
                {
                    Program.CTC.Message_Check_Availability_408();
                }
                else
                {
                    Program.CTC.Message_Chemical_Change_Confirm_401();
                }
            }
            else
            {
                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                {
                    if (Program.seq.supply.c_c_need == true || Program.seq.supply.ready_flag == true)
                    {
                        if (Program.seq.supply.req_c_c_start_cds_to_ctc == true)
                        {
                            Program.seq.supply.ctc_supply_request = true;
                            Program.seq.supply.rep_c_c_start_cds_to_ctc = true;
                        }
                    }
                }
                else
                {
                    if (Program.seq.supply.req_c_c_start_cds_to_ctc == true)
                    {
                        Program.seq.supply.ctc_supply_request = true;
                        Program.seq.supply.rep_c_c_start_cds_to_ctc = true;
                    }
                }
            }
            //
            //Tank Change 

        }
        public void CDS_Data_Log()
        {
            string log = "";
            log = ",Alarm Total/H/L/W, " + Program.occured_alarm_form.cnt_occured_alarm_total + ", " + Program.occured_alarm_form.cnt_occured_alarm_heavy_total + ", " + Program.occured_alarm_form.cnt_occured_alarm_light_total + ", " + Program.occured_alarm_form.cnt_occured_alarm_warning_total;
            log += "," + "Mode, " + Program.cg_app_info.eq_mode.ToString();
            log += "," + "Tank A Status, " + Program.tank[(int)tank_class.enum_tank_type.TANK_A].status.ToString();
            log += "," + "Tank B Status, " + Program.tank[(int)tank_class.enum_tank_type.TANK_B].status.ToString();
            log += "," + "Pump Circulation State, " + Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state.ToString();
            log += "," + "Pump Supply A State, " + Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_PUMP_A_START].value.ToString();
            log += "," + "Pump Supply B State, " + Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_PUMP_B_START].value.ToString();
            log += "," + "Pump Drain State, " + Program.IO.DO.Tag[(int)Config_IO.enum_do.Drain_Pump_On].value.ToString();

            if (Program.main_form.SerialData.Circulation_Thermostat.run_state == true || Program.main_form.SerialData.TEMP_CONTROLLER.circulation.mode == Class_TempController_M74.enum_m74_status.run)
            {
                log += "," + "Heater Circulation State, " + "True";
            }
            else
            {
                log += "," + "Heater Circulation State, " + "False";
            }
            if (Program.main_form.SerialData.Supply_A_Thermostat.run_state == true || Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.mode == Class_TempController_M74.enum_m74_status.run)
            {
                log += "," + "Heater Supply A State, " + "True";
            }
            else
            {
                log += "," + "Heater Supply A State, " + "False";
            }
            if (Program.main_form.SerialData.Supply_B_Thermostat.run_state == true || Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.mode == Class_TempController_M74.enum_m74_status.run)
            {
                log += "," + "Heater Supply B State, " + "True";
            }
            else
            {
                log += "," + "Heater Supply B State, " + "False";
            }
            log += "," + "TANK A TEMP, " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Set).ToString() + ", " + Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv;
            log += "," + "Tank A MV, " + Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.mv;
            log += "," + "Tank A OFFSET, " + Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.offset;
            log += "," + "TANK B TEMP, " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Set).ToString() + ", " + Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv;
            log += "," + "Tank B MV, " + Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.mv;
            log += "," + "Tank B OFFSET, " + Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.offset;

            log += "," + "TANK Circulation TEMP, " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation.sv + ", " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation.pv;
            log += "," + "TANK Circulation MV, " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation.mv;
            log += "," + "TANK Circulation OFFSET, " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation.offset;

            log += "," + "TANK Circulation Heater1, " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv + ", " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv;
            log += "," + "Tank Circulation Heater1 MV, " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.mv;
            log += "," + "Tank Circulation Heater1 OFFSET, " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset;

            log += "," + "TANK Circulation Heater2, " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.sv + ", " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.pv;
            log += "," + "Tank Circulation Heater2 MV, " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.mv;
            log += "," + "Tank Circulation Heater2 OFFSET, " + Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.offset;

            log += "," + "TANK A Supply TEMP, " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Set).ToString() + ", " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.pv;
            log += "," + "TANK A Supply MV, " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.mv;
            log += "," + "TANK A Supply OFFSET, " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.offset;
            log += "," + "TANK A Supply Heater, " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv + ", " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv;
            log += "," + "Tank A Supply Heater MV, " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.mv;
            log += "," + "Tank A Supply Heater OFFSET, " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.offset;

            log += "," + "TANK B Supply TEMP, " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_Set).ToString() + ", " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.pv;
            log += "," + "TANK B Supply MV, " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.mv;
            log += "," + "TANK B Supply OFFSET, " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.offset;
            log += "," + "TANK B Supply Heater, " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.sv + ", " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv;
            log += "," + "Tank B Supply Heater MV, " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.mv;
            log += "," + "Tank B Supply Heater OFFSET, " + Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.offset;

            log += "," + "TANK TS-09 TEMP, " + Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.pv;

            log += "," + "Circulation Pressure, " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value;
            log += "," + "Circulation Flow, " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value;

            log += "," + "Supply A Out Filter Pressure, " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_OUT_PRESS].value;
            log += "," + "Supply B Out Filter Pressure, " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_OUT_PRESS].value;
            log += "," + "Supply A Flow, " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value;
            log += "," + "Supply B Flow, " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value;
            log += "," + "Supply A Return Pressure, " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_A].value;
            log += "," + "Supply B Return Pressure, " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_B].value;
            Program.log_md.LogWrite(log, Module_Log.enumLog.RESOURCE_1, "", Module_Log.enumLevel.ALWAYS);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string[] OutStringSplit;
            string send_msg = "";
            char cSpace = ' ';
            byte[] send_data = null;
            int idx = 0;
            try
            {
                //01 04 1E 00 00 00 00 00 00 09 0A 00 00 03 E8 00 40 00 00 00 00 00 00 00 00 00 00 00 01 09 4A 08 DC A7 1D
                //         0     1     2     3     4     5     6     7     8     9     10    11    12    13    14    15
                //1 4 30 8 250 0 0 0 0 8 106 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 9 26 9 33 7 139
                //1 4 30 9 147 0 0 0 0 8 205 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 9 26 9 82 19 16
                send_data = new byte[35];
                send_data[0] = 1;
                send_data[1] = 4;
                send_data[2] = 30;
                send_data[3] = 0;
                send_data[4] = 0;
                send_data[5] = 0;
                send_data[6] = 0;
                send_data[7] = 0;
                send_data[8] = 0;
                send_data[9] = 9;
                send_data[10] = 10;
                send_data[11] = 0;
                send_data[12] = 0;
                send_data[13] = 03;
                send_data[14] = 232;
                send_data[15] = 0;
                send_data[16] = 64;
                send_data[17] = 0;
                send_data[18] = 0;
                send_data[19] = 0;
                send_data[20] = 0;
                send_data[21] = 0;
                send_data[22] = 0;
                send_data[23] = 0;
                send_data[24] = 0;
                send_data[25] = 0;
                send_data[26] = 0;
                send_data[27] = 0;
                send_data[28] = 1;
                send_data[29] = 9;
                send_data[30] = 74;
                send_data[31] = 8;
                send_data[32] = 220;
                send_data[33] = 167;
                send_data[34] = 29;

                Class_Serial_Common.Rcv_Data rcv_data_parse = new Class_Serial_Common.Rcv_Data();
                Program.ThermoStart_HE_3320C.Recieve_Data_To_Parse_HE_3320C(2, send_data, ref send_msg, ref rcv_data_parse);
                //string parse = "01 04 1E 08 FA 00 00 00 00 08 6B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 09 1A 09 21 CA 17";

                //string parse = "";
                //Class_Serial_Common.Rcv_Data rcv_data_parse = new Class_Serial_Common.Rcv_Data();
                //Program.CS600F.Recieve_Data_To_Parse_CS600F("DD,662,1,1,025.69,-000.6,004.70,0093.8,000000,000000,000000,013,000", ref parse, ref rcv_data_parse);
                return;
                TCP_IP_SOCKET.Cls_Socket.Socket_Event socket_event = new TCP_IP_SOCKET.Cls_Socket.Socket_Event();
                //send_msg = "00 01 00 00 00 13 00 04 10 A6 87 44 2D 00 01 00 00 ED 29 44 F8 FF 5C C4 79";
                send_msg = "00 01 00 00 00 04 00 02 01 43";
                OutStringSplit = send_msg.Split(cSpace);
                Array.Resize(ref send_data, OutStringSplit.Length);
                foreach (string splitNumber in OutStringSplit)
                {
                    send_data[idx] = Convert.ToByte(splitNumber, 16);
                    //if (rbtn_view_hex.Checked == true)
                    //{

                    //}
                    //else if (rbtn_view_byte.Checked == true)
                    //{
                    //    send_data[idx] = Convert.ToByte(splitNumber);
                    //}
                    //else if (rbtn_view_string.Checked == true)
                    //{
                    //    send_data[idx] = Encoding.UTF8.GetBytes(splitNumber)[0];
                    //}
                    idx++;
                }
                socket_event.data_to_array = send_data;
                Program.ABB.Recieve_Data_To_Parse_ABB(socket_event);
                return;
                string aaaaa = "000046";
                Console.WriteLine(aaaaa.Substring(1, 4));


                //
                string[] read_data;
                string query, err = "";
                read_data = System.IO.File.ReadAllLines("D:\\CDS\\Update 1.1.28.0\\para.txt");
                Config_Parameter para_object = new Config_Parameter();
                for (int idx2 = 1; idx2 < read_data.Length; idx++)
                {
                    para_object.comment = read_data[idx].Split(';')[1];
                    para_object.id = Convert.ToInt32(read_data[idx].Split(';')[0]);

                    query = "Update parameters";
                    query += " Set cds_parameter_comment = '" + para_object.comment.ToString() + "'";
                    query += " WHERE cds_parameter_id = '" + para_object.id + "'";
                    Program.database_md.MariaDB_MainQuery(Program.cg_main.db_local_cds.connection, query, ref err);
                }

            }
            catch (Exception ex)
            {

            }
        }

    }
}
