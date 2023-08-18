using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using static cds.tank_class;

namespace cds
{
    public partial class frm_schematic : DevExpress.XtraEditors.XtraForm
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private Boolean _actived = false;
        private Boolean _initialized = false;
        private Boolean _hdiw_bypass = false;

        public Thread thd_seq_main;
        public Thread thd_seq_supply;
        public Thread thd_seq_drain_valve_monitoring;
        public Thread thd_seq_pump_control;
        public Thread thd_seq_drain_pump_control;
        public Thread thd_seq_monitoring;
        public Thread thd_seq_circulation_monitoring;

        public Color Level_on = Color.Lime;
        public Color Level_on_HH_Over = Color.Red;
        public Color Level_off = Color.White;

        //Pipe On Clolr
        public Color Pipe_on = Color.Blue;
        public Color Pipe_on_supply = Color.Blue;

        public Color Pipe_on_drain = Color.Red;
        public Color Pipe_on_circulation = Color.Lime;
        public Color Pipe_on_ccss = Color.Orange;

        //Pipe Off Color 전체 동일
        public Color Pipe_off = Color.LightGray;

        public UC_APM APM;
        public UC_IPA IPA;
        public UC_DSP DSP;
        public UC_DSP_MIX DSP_MIX;
        public UC_DHF DHF;
        public UC_LAL LAL;

        //CCSS Total Daily Usage
        public float TotalUsage_By_Daily_CCSS1 = 0;
        public float TotalUsage_By_Daily_CCSS2 = 0;
        public float TotalUsage_By_Daily_CCSS3 = 0;
        public float TotalUsage_By_Daily_CCSS4 = 0;

        //Flag
        public bool Seq_Stop_By_Interlock = false;
        public bool Tank_Low_Level_Touch_tank_a = true; //Tank_Low_Level_Check 함수 사용
        public bool Tank_Low_Level_Touch_tank_b = true; // Tank_Low_Level_Check 함수 사용
        public bool Tank_Drain_Start_tank_a = false; // Auto, Semi Auto Drain Sequence Interlock
        public bool Tank_Drain_Start_tank_b = false; // Auto, Semi Auto Drain Sequence Interlock
        public bool chemical_drain_on = false;
        public bool chemical_drain_off = false;
        public bool EMPTY_FORCE_OFF = false;
        public bool MAIN_RETURN_DRAIN_FORCE_ON = false;
        public bool Check_Availability_response_status = true; //Token 408 Simulation Use
        public bool Manual_Exchange_Req_By_User = false;// Manual Excahnge Button Click


        public bool supply_enable_send = false;
        public bool supply_disable_send = false;
        public string Seq_Drain_Valve_Monitoring_tank_a_log_old = "";
        public string Seq_Drain_Valve_Monitoring_tank_a_log_cur = "";
        public string Seq_Drain_Valve_Monitoring_tank_b_log_old = "";
        public string Seq_Drain_Valve_Monitoring_tank_b_log_cur = "";

        public DateTime dt_start_totalusage_diw_by_sonotec;
        public DateTime dt_start_totalusage_ipa_by_sonotec;
        public DateTime dt_start_totalusage_dsp_by_sonotec;
        public DateTime dt_start_totalusage_lal_by_sonotec;
        public DateTime dt_start_tank_a_level_lowlow;
        public DateTime dt_start_tank_b_level_lowlow;
        //CTC Auto, Manual 상태 전송시 사용
        public bool auto_mode_on_trigger = false;
        public bool manual_mode_on_trigger = false;

        //CTC Alarm Level에 따른 Process Start, Stop시 사용
        public bool no_process_req = false;
        public bool no_process_cancel = false;

        //Circulation Pump Run Interval 0: Sensor 1>: Interval
        public int circulation_pump_run_interval = 0;

        public bool Reclaim_Rcv = false;
        public enum enum_tank_level
        {
            OVER = 0,
            HH = 1,
            H = 2,
            M = 3,
            L = 4,
            LL = 5,
            EMPTY = 6,
        }
        public frm_schematic()
        {
            InitializeComponent();
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
                        if (_initialized == false) { _initialized = true; Setting_Initial(); }
                        timer_uichange.Interval = 200; timer_uichange.Enabled = true;
                        timer_check_thread.Interval = 1000; timer_check_thread.Enabled = true;
                    }
                    else
                    {
                        timer_uichange.Enabled = false;
                    }
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + ".actived." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
            }
        }
        public void Setting_Initial()
        {
            //Manual Mode 시작
            Program.cg_app_info.eq_mode = enum_eq_mode.manual;

            if (Program.cg_app_info.mode_simulation.use == true)
            {
                fpnl_seq_monitor.BringToFront();
                fpnl_seq_monitor.Visible = true;
            }
            else
            {
                fpnl_seq_monitor.Visible = false;
            }

            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                APM = new UC_APM();
                pnl_body.Controls.Add(APM);
                APM.Dock = DockStyle.Fill;
                pnl_common_split_level_3.Visible = false; btn_calibration.Visible = false; gp_common.Size = new Size(120, 174);
                if (Program.cg_mixing_step.mixing_use == false)
                {
                    APM.lbl_tank_a_concentration.Visible = false; APM.lbl_tank_b_concentration.Visible = false;
                    APM.lbl_tank_a_concentrate.Visible = false; APM.lbl_tank_b_concentrate.Visible = false;
                }
                if (_hdiw_bypass == false) { _hdiw_bypass = true; Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HOT_DIW_BY_PASS, true); }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                IPA = new UC_IPA();
                pnl_body.Controls.Add(IPA);
                IPA.Dock = DockStyle.Fill;
                if (Program.cg_mixing_step.mixing_use == false)
                {
                    IPA.lbl_tank_a_concentration.Visible = false;
                    IPA.lbl_tank_a_concentrate.Visible = false;
                }


                //btn_tank_drain_a.Visible = false; pnl_split_level_1.Visible = false;
                btn_tank_diw_flush_a.Visible = false; pnl_split_level_2.Visible = false;
                //btn_tank_chem_flush_a.Visible = false; pnl_split_level_3.Visible = true;
                btn_tank_diw_flush_supply_a.Visible = false; pnl_split_level_4.Visible = false;
                //btn_tank_chem_flush_supply_a.Visible = true; pnl_split_level_5.Visible = true;
                btn_tank_auto_flush_a.Visible = false;

                btn_exchange.Visible = false;
                pnl_common_split_level_2.Visible = false; gp_common.Size = new Size(120, 128);
                pnl_common_split_level_3.Visible = false; btn_calibration.Visible = false;
                gp_tank_a.Size = new Size(120, 170);
                gp_tank_b.Visible = false;

            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                DSP = new UC_DSP();
                pnl_body.Controls.Add(DSP);
                DSP.Dock = DockStyle.Fill;
                pnl_common_split_level_3.Visible = false; btn_calibration.Visible = false; gp_common.Size = new Size(120, 174);

                //DSP 농도계 추가됨에 따라 원액에서도 농도계 사용
                //if (Program.cg_mixing_step.mixing_use == false)
                //{
                    DSP.lbl_tank_a_concentration.Visible = true; DSP.lbl_tank_b_concentration.Visible = true;
                    DSP.lbl_tank_a_concentrate.Visible = true; DSP.lbl_tank_b_concentrate.Visible = true;
                //}
                if (Program.cg_app_info.circulation_pump_mode == enum_pump_mode.none)
                {
                    DSP.pnl_circulation_pump_left.Visible = true; DSP.pnl_circulation_pump_right.Visible = true; DSP.pnl_circulation_pump_leak.Visible = true;
                }
                else if (Program.cg_app_info.circulation_pump_mode == enum_pump_mode.type1)
                {
                    DSP.pnl_circulation_pump_left.Visible = false; DSP.pnl_circulation_pump_right.Visible = false;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                DSP_MIX = new UC_DSP_MIX();
                pnl_body.Controls.Add(DSP_MIX);
                DSP_MIX.Dock = DockStyle.Fill;
                pnl_common_split_level_3.Visible = false; btn_calibration.Visible = false; gp_common.Size = new Size(120, 174);
                if (Program.cg_mixing_step.mixing_use == false)
                {
                    DSP_MIX.lbl_tank_a_concentration.Visible = false; DSP_MIX.lbl_tank_b_concentration.Visible = false;
                    DSP_MIX.lbl_tank_a_concentrate.Visible = false; DSP_MIX.lbl_tank_b_concentrate.Visible = false;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                DHF = new UC_DHF();
                pnl_body.Controls.Add(DHF);
                DHF.Dock = DockStyle.Fill;
                if (Program.cg_mixing_step.mixing_use == false)
                {
                    DHF.lbl_tank_a_concentration.Visible = false; DHF.lbl_tank_b_concentration.Visible = false;
                    DHF.lbl_tank_a_concentrate.Visible = false; DHF.lbl_tank_b_concentrate.Visible = false;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                LAL = new UC_LAL();
                pnl_body.Controls.Add(LAL);
                LAL.Dock = DockStyle.Fill;

                if (Program.cg_mixing_step.mixing_use == false)
                {
                    LAL.lbl_tank_a_concentration.Visible = false; LAL.lbl_tank_b_concentration.Visible = false;
                    LAL.lbl_tank_a_concentrate.Visible = false; LAL.lbl_tank_b_concentrate.Visible = false;
                }
            }

            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
            {
                gp_tank_a.Enabled = false;
            }
            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == false)
            {
                gp_tank_b.Enabled = false;
            }

            //System Botting 후 항온조 및 PCW ON
            Thermostat_Power_ON_OFF(true);
            PCW_VALVE_ON_OFF(true);

        }
        private void timer_main_Tick(object sender, EventArgs e)
        {
            if (Program.cg_apploading.load_complete == true)
            {
                UI_Change();
                Reclaim_Signal_Check();
            }
        }
        public void Reclaim_Signal_Check()
        {
            if (Reclaim_Rcv == true)
            {
                Reclaim_Rcv = false;
                Program.schematic_form.DelayAction_CC_Response(Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Reclaim_Flush) * 1000, new Action(() => { Reclaim_Valve_Event(); }));
            }
        }
        public void Reclaim_Valve_Event()
        {
            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RECLAIM_DRAIN, false);
            //Program.CTC.Message_CDS_Enable_Event_450();
            if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
            {
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_RECLAIM_TO_TANK_A, true);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_RECLAIM_TO_TANK_B, false);
            }
            else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
            {
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_RECLAIM_TO_TANK_A, false);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_RECLAIM_TO_TANK_B, true);
            }
        }
        private void timer_manual_sequence_tank_a_Tick(object sender, EventArgs e)
        {
            if (Program.seq.semi_auto_tank_a.semi_auto_type == enum_semi_auto.CALIBRATION_LAL)
            {
                //Class_Serial_Common.Rcv_Data rcv_data_parse = new Class_Serial_Common.Rcv_Data();
                //string parse = "";
                //Program.CS600F.Recieve_Data_To_Parse_CS600F(Program.main_form.textBox1.Text + 0x0D + 0x0A, ref parse, ref rcv_data_parse);
                Seq_Manual_Concentration_Calibration_By_LAL(tank_class.enum_tank_type.TANK_A, Program.seq.semi_auto_tank_a.no_cur, true);
            }
            else
            {
                Seq_Manual_Drain(tank_class.enum_tank_type.TANK_A, Program.seq.semi_auto_tank_a.no_cur, true);
            }
        }
        private void timer_manual_sequence_tank_b_Tick(object sender, EventArgs e)
        {
            Seq_Manual_Drain(tank_class.enum_tank_type.TANK_B, Program.seq.semi_auto_tank_b.no_cur, true);
        }
        private void timer_check_thread_Tick(object sender, EventArgs e)
        {
            Check_Thread();
        }
        public void Check_Thread()
        {
            if (Program.cg_apploading.load_complete == true)
            {
                if (thd_seq_main != null) { if (thd_seq_main.IsAlive == false) { thd_seq_main.Abort(); thd_seq_main = null; thd_seq_main = new Thread(Seq_Main); thd_seq_main.Start(); } }
                else { thd_seq_main = new Thread(Seq_Main); thd_seq_main.Start(); }

                if (thd_seq_supply != null) { if (thd_seq_supply.IsAlive == false) { thd_seq_supply.Abort(); thd_seq_supply = null; thd_seq_supply = new Thread(Seq_Supply); thd_seq_supply.Start(); } }
                else { thd_seq_supply = new Thread(Seq_Supply); thd_seq_supply.Start(); }

                if (Program.cg_app_info.eq_type != enum_eq_type.ipa)
                {
                    if (thd_seq_pump_control != null) { if (thd_seq_pump_control.IsAlive == false) { thd_seq_pump_control.Abort(); thd_seq_pump_control = null; thd_seq_pump_control = new Thread(Seq_Circulation_Pump_Control); thd_seq_pump_control.Start(); } }
                    else { thd_seq_pump_control = new Thread(Seq_Circulation_Pump_Control); thd_seq_pump_control.Start(); }

                    if (thd_seq_circulation_monitoring != null) { if (thd_seq_circulation_monitoring.IsAlive == false) { thd_seq_circulation_monitoring.Abort(); thd_seq_circulation_monitoring = null; thd_seq_circulation_monitoring = new Thread(Seq_Circulation_Monitoring); thd_seq_circulation_monitoring.Start(); } }
                    else { thd_seq_circulation_monitoring = new Thread(Seq_Circulation_Monitoring); thd_seq_circulation_monitoring.Start(); }

                }
                //if (thd_seq_drain_pump_control != null) { if (thd_seq_drain_pump_control.IsAlive == false) { thd_seq_drain_pump_control.Abort(); thd_seq_drain_pump_control = null; thd_seq_drain_pump_control = new Thread(Seq_Drain_Pump_Control); thd_seq_drain_pump_control.Start(); } }
                //else { thd_seq_drain_pump_control = new Thread(Seq_Drain_Pump_Control); thd_seq_drain_pump_control.Start(); }

                if (thd_seq_drain_valve_monitoring != null) { if (thd_seq_drain_valve_monitoring.IsAlive == false) { thd_seq_drain_valve_monitoring.Abort(); thd_seq_drain_valve_monitoring = null; thd_seq_drain_valve_monitoring = new Thread(Seq_Drain_Valve_Monitoring); thd_seq_drain_valve_monitoring.Start(); } }
                else { thd_seq_drain_valve_monitoring = new Thread(Seq_Drain_Valve_Monitoring); thd_seq_drain_valve_monitoring.Start(); }

                if (thd_seq_monitoring != null) { if (thd_seq_monitoring.IsAlive == false) { thd_seq_monitoring.Abort(); thd_seq_monitoring = null; thd_seq_monitoring = new Thread(Seq_Monitoring); thd_seq_monitoring.Start(); } }
                else { thd_seq_monitoring = new Thread(Seq_Monitoring); thd_seq_monitoring.Start(); }
            }
        }
        public void Tank_Button_Status_Change()
        {
            Color run_color = Color.Lime;
            Color default_color = SystemColors.MenuHighlight;
            if (timer_manual_sequence_tank_a.Enabled == true)
            {
                if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DRAIN)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_drain).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_a.dt_btn_state_change_drain = DateTime.Now;
                        btn_tank_drain_a.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_drain).TotalSeconds >= 1)
                    {
                        btn_tank_drain_a.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_drain_a.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_diw_flush).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_a.dt_btn_state_change_diw_flush = DateTime.Now;
                        btn_tank_diw_flush_a.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_diw_flush).TotalSeconds >= 1)
                    {
                        btn_tank_diw_flush_a.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_diw_flush_a.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_diw_flush).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_a.dt_btn_state_change_diw_flush = DateTime.Now;
                        btn_tank_diw_flush_supply_a.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_diw_flush).TotalSeconds >= 1)
                    {
                        btn_tank_diw_flush_supply_a.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_diw_flush_supply_a.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_chem_flush).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_a.dt_btn_state_change_chem_flush = DateTime.Now;
                        btn_tank_chem_flush_a.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_chem_flush).TotalSeconds >= 1)
                    {
                        btn_tank_chem_flush_a.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_chem_flush_a.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_chem_flush).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_a.dt_btn_state_change_chem_flush = DateTime.Now;
                        btn_tank_chem_flush_supply_a.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_chem_flush).TotalSeconds >= 1)
                    {
                        btn_tank_chem_flush_supply_a.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_chem_flush_supply_a.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_chem_flush).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_a.dt_btn_state_change_chem_flush = DateTime.Now;
                        btn_tank_auto_flush_a.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_chem_flush).TotalSeconds >= 1)
                    {
                        btn_tank_auto_flush_a.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_auto_flush_a.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CALIBRATION_LAL)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_calibration).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_a.dt_btn_state_change_calibration = DateTime.Now;
                        btn_calibration.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_a.dt_btn_state_change_calibration).TotalSeconds >= 1)
                    {
                        btn_calibration.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_calibration.Appearance.BackColor = run_color;
                    }
                }
            }
            else
            {
                btn_tank_drain_a.Appearance.BackColor = default_color;
                btn_tank_diw_flush_a.Appearance.BackColor = default_color;
                btn_tank_chem_flush_a.Appearance.BackColor = default_color;
                btn_tank_diw_flush_supply_a.Appearance.BackColor = default_color;
                btn_tank_chem_flush_supply_a.Appearance.BackColor = default_color;
                btn_tank_auto_flush_a.Appearance.BackColor = default_color;
                btn_calibration.Appearance.BackColor = default_color;
            }

            if (timer_manual_sequence_tank_b.Enabled == true)
            {
                if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DRAIN)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_drain).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_b.dt_btn_state_change_drain = DateTime.Now;
                        btn_tank_drain_b.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_drain).TotalSeconds >= 1)
                    {
                        btn_tank_drain_b.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_drain_b.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_diw_flush).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_b.dt_btn_state_change_diw_flush = DateTime.Now;
                        btn_tank_diw_flush_b.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_diw_flush).TotalSeconds >= 1)
                    {
                        btn_tank_diw_flush_b.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_diw_flush_b.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_diw_flush).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_b.dt_btn_state_change_diw_flush = DateTime.Now;
                        btn_tank_diw_flush_supply_b.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_diw_flush).TotalSeconds >= 1)
                    {
                        btn_tank_diw_flush_supply_b.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_diw_flush_supply_b.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_chem_flush).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_b.dt_btn_state_change_chem_flush = DateTime.Now;
                        btn_tank_chem_flush_b.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_chem_flush).TotalSeconds >= 1)
                    {
                        btn_tank_chem_flush_b.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_chem_flush_b.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_chem_flush).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_b.dt_btn_state_change_chem_flush = DateTime.Now;
                        btn_tank_chem_flush_supply_b.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_chem_flush).TotalSeconds >= 1)
                    {
                        btn_tank_chem_flush_supply_b.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_chem_flush_supply_b.Appearance.BackColor = run_color;
                    }
                }
                else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                {
                    if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_chem_flush).TotalSeconds >= 2)
                    {
                        Program.seq.semi_auto_tank_b.dt_btn_state_change_chem_flush = DateTime.Now;
                        btn_tank_auto_flush_b.Appearance.BackColor = run_color;
                    }
                    else if ((DateTime.Now - Program.seq.semi_auto_tank_b.dt_btn_state_change_chem_flush).TotalSeconds >= 1)
                    {
                        btn_tank_auto_flush_b.Appearance.BackColor = default_color;
                    }
                    else
                    {
                        btn_tank_auto_flush_b.Appearance.BackColor = run_color;
                    }
                }
            }
            else
            {
                btn_tank_drain_b.Appearance.BackColor = default_color;
                btn_tank_diw_flush_b.Appearance.BackColor = default_color;
                btn_tank_chem_flush_b.Appearance.BackColor = default_color;
                btn_tank_diw_flush_supply_b.Appearance.BackColor = default_color;
                btn_tank_chem_flush_supply_b.Appearance.BackColor = default_color;
                btn_tank_auto_flush_b.Appearance.BackColor = default_color;
            }
        }
        /// <summary>
        /// Sequence 외부에서 Sequnce Log 강제 추가를 위함
        /// </summary>
        /// <param name="seq_type"></param>
        /// <param name="call_by"></param>
        /// <param name="log"></param>
        public void Sequence_Log_Add(enum_seq_type seq_type, string call_by, string log)
        {
            if (seq_type == enum_seq_type.MAIN)
            {
                Program.log_md.LogWrite("*****" + call_by + " : " + log, Module_Log.enumLog.SEQ_MAIN, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (seq_type == enum_seq_type.SEMI_AUTO_A)
            {
                Program.log_md.LogWrite("*****" + call_by + " : " + log, Module_Log.enumLog.SEQ_SEMI_AUTO_A, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (seq_type == enum_seq_type.SEMI_AUTO_B)
            {
                Program.log_md.LogWrite("*****" + call_by + " : " + log, Module_Log.enumLog.SEQ_SEMI_AUTO_A, "", Module_Log.enumLevel.ALWAYS);
            }
            else if (seq_type == enum_seq_type.NONE)
            {
                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                {
                    Program.log_md.LogWrite("*****" + call_by + " : " + log, Module_Log.enumLog.SEQ_MAIN, "", Module_Log.enumLevel.ALWAYS);
                }
                if (timer_manual_sequence_tank_a.Enabled == true)
                {
                    Program.log_md.LogWrite("*****" + call_by + " : " + log, Module_Log.enumLog.SEQ_SEMI_AUTO_A, "", Module_Log.enumLevel.ALWAYS);
                }
                if (timer_manual_sequence_tank_b.Enabled == true)
                {
                    Program.log_md.LogWrite("*****" + call_by + " : " + log, Module_Log.enumLog.SEQ_SEMI_AUTO_B, "", Module_Log.enumLevel.ALWAYS);
                }
                Program.log_md.LogWrite(call_by + " : " + log, Module_Log.enumLog.SEQ_MONITORING, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        //public Boolean Seq_Cur_To_Next(tank_class.enum_tank_type tanK_type ,ref DateTime dt_lastact, ref tank_class.enum_seq_no next_seq_no)
        public Boolean Seq_Cur_To_Next(tank_class.enum_seq_no cur_seq_no, tank_class.enum_seq_no next_seq_no, string Memo)
        {
            Program.seq.main.last_act_time = DateTime.Now; Program.seq.main.no_cur = next_seq_no;
            if (Memo == "")
            {
                Program.seq.main.state_display = "" + cur_seq_no.ToString() + " -> " + next_seq_no.ToString();
            }
            else
            {
                Program.seq.main.state_display = "" + cur_seq_no.ToString() + " -> " + next_seq_no.ToString() + " / " + Memo;
            }
            //Program.seq.main.memo_current = Memo;
            return true;
        }
        public Boolean Seq_Supply_Cur_To_Next(tank_class.enum_seq_no_supply cur_seq_no, tank_class.enum_seq_no_supply next_seq_no, string Memo)
        {
            Program.seq.supply.last_act_time = DateTime.Now; Program.seq.supply.no_cur = next_seq_no;
            Program.seq.supply.state_display = "" + cur_seq_no.ToString() + " -> " + next_seq_no.ToString();
            Program.seq.supply.state_display2 = Memo;
            return true;
        }
        public Boolean Seq_Pump_Control_Cur_To_Next(tank_class.enum_seq_no_pump_control cur_seq_no, tank_class.enum_seq_no_pump_control next_seq_no, string Memo)
        {
            Program.seq.pumpcontrol.last_act_time = DateTime.Now; Program.seq.pumpcontrol.no_cur = next_seq_no;
            Program.seq.pumpcontrol.state_display = "" + cur_seq_no.ToString() + " -> " + next_seq_no.ToString();
            Program.seq.pumpcontrol.state_display2 = Memo;
            return true;
        }
        public Boolean Seq_Semi_Auto_Tank_A_Cur_To_Next(tank_class.enum_seq_no_semi_auto cur_seq_no, tank_class.enum_seq_no_semi_auto next_seq_no, string Memo)
        {
            Program.seq.semi_auto_tank_a.last_act_time = DateTime.Now; Program.seq.semi_auto_tank_a.no_cur = next_seq_no;
            Program.seq.semi_auto_tank_a.state_display = "" + cur_seq_no.ToString() + " -> " + next_seq_no.ToString();
            //Program.seq.semi_auto_tank_a.memo_current = Memo;
            return true;
        }
        public Boolean Seq_Semi_Auto_Tank_B_Cur_To_Next(tank_class.enum_seq_no_semi_auto cur_seq_no, tank_class.enum_seq_no_semi_auto next_seq_no, string Memo)
        {
            Program.seq.semi_auto_tank_b.last_act_time = DateTime.Now; Program.seq.semi_auto_tank_b.no_cur = next_seq_no;
            Program.seq.semi_auto_tank_b.state_display = "" + cur_seq_no.ToString() + " -> " + next_seq_no.ToString();
            //Program.seq.semi_auto_tank_b.memo_current = Memo;
            return true;
        }
        public Boolean Seq_Semi_Auto_Tank_ALL_Cur_To_Next(tank_class.enum_seq_no_semi_auto cur_seq_no, tank_class.enum_seq_no_semi_auto next_seq_no, string Memo)
        {
            Program.seq.semi_auto_tank_all.last_act_time = DateTime.Now; Program.seq.semi_auto_tank_all.no_cur = next_seq_no;
            Program.seq.semi_auto_tank_all.state_display = "" + cur_seq_no.ToString() + " -> " + next_seq_no.ToString();
            //Program.seq.semi_auto_tank_all.memo_current = Memo;
            return true;
        }
        private void frm_schematic_Load(object sender, EventArgs e)
        {
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                Program.schematic_form.Tank_Value_Clear(tank_class.enum_tank_type.TANK_ALL, true);
                Program.schematic_form.Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, false);
                Program.schematic_form.Tank_Value_Clear(tank_class.enum_tank_type.TANK_B, false);
            }
        }
        public void Sampling_Port_Close_By_Application()
        {
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SAMPLING].value == true)
            {
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SAMPLING, false);
                Program.main_md.Message_By_Application("Sampling OK", frm_messagebox.enum_apptype.Only_OK);
            }
            else
            {
                //Sampling Port 누르고, 팝업전 사용자가 종료 시 행동 취하지 않음
            }
        }
        public void All_Stop()
        {
            Program.eventlog_form.Insert_Event("All_Stop", (int)frm_eventlog.enum_event_type.SYSTEM, (int)frm_eventlog.enum_occurred_type.USER, true);
            Tank_Supply_End(Program.seq.supply.cur_tank);
            Program.seq.supply.CC_START_TANK = "";
            Program.schematic_form.Tank_Value_Clear(tank_class.enum_tank_type.TANK_ALL, true);
            Program.schematic_form.Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, false);
            Program.schematic_form.Tank_Value_Clear(tank_class.enum_tank_type.TANK_B, false);
            Program.seq.main.no_cur = enum_seq_no.MODE_CHECK;
            Program.seq.supply.no_cur = enum_seq_no_supply.TANK_READY_CHECK;
            Program.seq.supply.c_c_need = false;
            Program.seq.supply.ctc_supply_request = false;
            Program.seq.supply.ready_flag = false;
            Program.seq.supply.refill_run_state = false;
            Program.seq.main.concentration_measuring = false;
            Program.seq.supply.concentration_measuring = false;
            //Semi Auto Timer 중지
            timer_manual_sequence_tank_a.Enabled = false;
            timer_manual_sequence_tank_b.Enabled = false;

            Program.cg_app_info.eq_mode = enum_eq_mode.manual;
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HOT_DIW_BY_PASS, true);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_FLUSHING_DIW, false);
                Program.CS600F.Message_Command_To_Byte_CS600F_TO_Send(Class_Concentration_CS600F.command_Write_CALIBRATION_END, (int)Config_IO.enum_lal_serial_index.CONCENTRATION);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                CIRCULATION_Heat_Exchanger_ON_OFF(false);
            }
        }
        private void btn_all_stop_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
                {
                    Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
                }
                ///Common
                if (btn_event.Name == "btn_all_stop")
                {
                    if (Program.main_md.Message_By_Application("Stop Running All Sequence CDS?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                    {
                        All_Stop();
                    }
                }
                else if (btn_event.Name == "btn_mode_change")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual || Program.cg_app_info.eq_mode == enum_eq_mode.none)
                    {
                        if (timer_manual_sequence_tank_a.Enabled == true || timer_manual_sequence_tank_a.Enabled == true)
                        {
                            if (Program.main_md.Message_By_Application("Semi Auto is Running, Cancel Semi Auto?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                            {
                                All_Stop();
                                Program.eventlog_form.Insert_Event("Semi Auto Cancel By User(Manual Button)", (int)frm_eventlog.enum_event_type.Manual_To_Auto, (int)frm_eventlog.enum_occurred_type.USER, true);
                            }
                            else
                            {
                                return;
                            }
                        }
                        else if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY)
                        {
                            Program.main_md.Message_By_Application("Heavy Alarm Occured!, Clear Alarm before Manual -> Auto", frm_messagebox.enum_apptype.Only_OK);
                            return;
                        }

                        if (Program.main_md.Message_By_Application("EQ Mode Change Manual -> Auto?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                        {
                            Program.eventlog_form.Insert_Event("EQ Mode Manual -> Auto", (int)frm_eventlog.enum_event_type.Manual_To_Auto, (int)frm_eventlog.enum_occurred_type.USER, true);
                            Program.schematic_form.Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, true);
                            Program.schematic_form.Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, false);
                            Program.schematic_form.Tank_Value_Clear(tank_class.enum_tank_type.TANK_B, false);
                            Program.cg_app_info.eq_mode = enum_eq_mode.auto;
                            Program.seq.supply.CC_START_TANK = "";
                            Program.seq.supply.CDS_enable_status_to_ctc = false;
                        }
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        if (Program.main_md.Message_By_Application("EQ Mode Change Auto -> Manual?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                        {
                            Program.eventlog_form.Insert_Event("EQ Mode Auto -> Manual", (int)frm_eventlog.enum_event_type.Auto_To_Manual, (int)frm_eventlog.enum_occurred_type.USER, true);
                            Program.cg_app_info.eq_mode = enum_eq_mode.manual;
                            Program.seq.main.no_cur = tank_class.enum_seq_no.NONE;
                        }
                    }
                }
                else if (btn_event.Name == "btn_exchange")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status != tank_class.enum_tank_status.READY && Program.tank[(int)tank_class.enum_tank_type.TANK_B].status != tank_class.enum_tank_status.READY)
                        {
                            Program.main_md.Message_By_Application("Exchange Function Only Use Tank A or B Status Ready", frm_messagebox.enum_apptype.Only_OK); return;
                        }
                        else if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.READY || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.READY)
                        {
                            if (Program.main_md.Message_By_Application("Are you sure Tank Exchange?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                            {
                                Manual_Exchange_Req_By_User = true;
                            }
                        }

                    }
                    else
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Manual Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_calibration")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.CALIBRATION_LAL, false, tank_class.enum_tank_type.TANK_A);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }

                ///Tank A
                else if (btn_event.Name == "btn_tank_drain_a")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.DRAIN, false, tank_class.enum_tank_type.TANK_A);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_tank_diw_flush_a")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.DIW_FLUSH, false, tank_class.enum_tank_type.TANK_A);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_tank_chem_flush_a")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.CHEMICAL_FLUSH, false, tank_class.enum_tank_type.TANK_A);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_tank_diw_flush_supply_a")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY, false, tank_class.enum_tank_type.TANK_A);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_tank_chem_flush_supply_a")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY, false, tank_class.enum_tank_type.TANK_A);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_tank_auto_flush_a")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.AUTO_FLUSH, false, tank_class.enum_tank_type.TANK_A);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }

                ///Tank B
                else if (btn_event.Name == "btn_tank_drain_b")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.DRAIN, false, tank_class.enum_tank_type.TANK_B);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_tank_diw_flush_b")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.DIW_FLUSH, false, tank_class.enum_tank_type.TANK_B);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_tank_chem_flush_b")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.CHEMICAL_FLUSH, false, tank_class.enum_tank_type.TANK_B);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_tank_diw_flush_supply_b")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY, false, tank_class.enum_tank_type.TANK_B);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_tank_chem_flush_supply_b")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY, false, tank_class.enum_tank_type.TANK_B);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }
                else if (btn_event.Name == "btn_tank_auto_flush_b")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {
                        Manual_Sequence_Start(tank_class.enum_semi_auto.AUTO_FLUSH, false, tank_class.enum_tank_type.TANK_B);
                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                }


            }
            Program.eventlog_form.Insert_Event(this.Name + ".Button Click : " + btn_event.Name, (int)frm_eventlog.enum_event_type.USER_BUTTON, (int)frm_eventlog.enum_occurred_type.USER, true);
            Program.log_md.LogWrite(this.Name + "btn_tank_a_exchange_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }
        public void DelayAction_CC_Response(int millisecond, Action action)
        {
            //일정 시간 후 함수 호출 대리자 사용함
            var timer = new DispatcherTimer();
            timer.Tick += delegate
            {
                action.Invoke();
                timer.Stop();
            };
            timer.Interval = TimeSpan.FromMilliseconds(millisecond);
            timer.Start();
        }
        public void Manual_Sequence_Cancel()
        {
            Program.seq.semi_auto_tank_a.no_cur = tank_class.enum_seq_no_semi_auto.NONE;
            Program.seq.semi_auto_tank_b.no_cur = tank_class.enum_seq_no_semi_auto.NONE;

            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.NONE;
            timer_manual_sequence_tank_a.Enabled = false;

            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.NONE;
            timer_manual_sequence_tank_b.Enabled = false;

            thd_seq_drain_valve_monitoring.Abort(); thd_seq_drain_valve_monitoring = null;
            if (thd_seq_drain_valve_monitoring != null) { if (thd_seq_drain_valve_monitoring.IsAlive == false) { thd_seq_drain_valve_monitoring.Abort(); thd_seq_drain_valve_monitoring = null; thd_seq_drain_valve_monitoring = new Thread(Seq_Drain_Valve_Monitoring); thd_seq_drain_valve_monitoring.Start(); } }
            else { thd_seq_drain_valve_monitoring = new Thread(Seq_Drain_Valve_Monitoring); thd_seq_drain_valve_monitoring.Start(); }

            //All Clear
            Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, true);

            //Tank 각각 Clear
            Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, false);
            Tank_Value_Clear(tank_class.enum_tank_type.TANK_B, false);

            All_Stop();
        }
        public void Manual_Sequence_Start(tank_class.enum_semi_auto selected_type, bool select_tank_popup, tank_class.enum_tank_type selected_tank_type)
        {
            ///함수 호출 저전 tank가 선택되어 있다면, Popup 후 선택 안함 select_tank_popup 변수 사용
            if (selected_tank_type == tank_class.enum_tank_type.TANK_A)
            {
                if (timer_manual_sequence_tank_a.Enabled == true || timer_manual_sequence_tank_b.Enabled == true)
                {
                    if (timer_manual_sequence_tank_a.Enabled == true)
                    {
                        if (Program.main_md.Message_By_Application("Already Running Manual Seqeunce" + System.Environment.NewLine + " Cancel existing " + Program.seq.semi_auto_tank_a.semi_auto_type.ToString() + " Seqeunce ? ", frm_messagebox.enum_apptype.Ok_Or_Cancel))
                        {
                            Manual_Sequence_Cancel();
                            return;
                        }
                    }
                    else if (timer_manual_sequence_tank_b.Enabled == true)
                    {
                        if (Program.main_md.Message_By_Application("Already Running Manual Seqeunce" + System.Environment.NewLine + " Cancel existing " + Program.seq.semi_auto_tank_b.semi_auto_type.ToString() + " Seqeunce ? ", frm_messagebox.enum_apptype.Ok_Or_Cancel))
                        {
                            Manual_Sequence_Cancel();
                            return;
                        }
                    }
                }
                else
                {
                    if (selected_type == tank_class.enum_semi_auto.DRAIN)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank A Drain Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_DRAIN_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.DIW_FLUSH)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank A DIW FLUSH Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_DIW_FLUSH_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank A DIW FLUSH AND SUPPLY Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_DIW_FLUSH_AND_SUPPLY_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank A DIW FLUSH Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_CHEM_FLUSH_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank A DIW FLUSH AND SUPPLY Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_CHEM_FLUSH_AND_SUPPLY_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank A AUTO FLUSH Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_CHEM_FLUSH_AND_SUPPLY_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.CALIBRATION_LAL)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Concentration Calibration", (int)frm_eventlog.enum_event_type.SEMI_AUTO_CALIBRATION, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }

                    if (Program.main_md.Message_By_Application("Semi Auto Tank A - " + selected_type.ToString().Replace('_', ' ') + " Running?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                    {
                        Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, true); Tank_Value_Clear(tank_class.enum_tank_type.TANK_B, true);
                        Program.seq.semi_auto_tank_a.semi_auto_type = selected_type;
                        if (selected_type == tank_class.enum_semi_auto.CALIBRATION_LAL)
                        {
                            Program.seq.semi_auto_tank_a.no_cur = tank_class.enum_seq_no_semi_auto.CAL_NONE;
                        }
                        else
                        {
                            Program.seq.semi_auto_tank_a.no_cur = tank_class.enum_seq_no_semi_auto.INITIAL;
                        }
                        timer_manual_sequence_tank_a.Interval = 200; timer_manual_sequence_tank_a.Enabled = true;
                    }

                }
            }
            else if (selected_tank_type == tank_class.enum_tank_type.TANK_B)
            {
                if (timer_manual_sequence_tank_a.Enabled == true || timer_manual_sequence_tank_b.Enabled == true)
                {
                    if (timer_manual_sequence_tank_a.Enabled == true)
                    {
                        if (Program.main_md.Message_By_Application("Already Running Manual Seqeunce" + System.Environment.NewLine + " Cancel existing " + Program.seq.semi_auto_tank_a.semi_auto_type.ToString() + " Seqeunce ? ", frm_messagebox.enum_apptype.Ok_Or_Cancel))
                        {
                            Manual_Sequence_Cancel();
                            return;
                        }
                    }
                    else if (timer_manual_sequence_tank_b.Enabled == true)
                    {
                        if (Program.main_md.Message_By_Application("Already Running Manual Seqeunce" + System.Environment.NewLine + " Cancel existing " + Program.seq.semi_auto_tank_b.semi_auto_type.ToString() + " Seqeunce ? ", frm_messagebox.enum_apptype.Ok_Or_Cancel))
                        {
                            Manual_Sequence_Cancel();
                            return;
                        }
                    }
                }
                else
                {
                    if (selected_type == tank_class.enum_semi_auto.DRAIN)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank B Drain Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_DRAIN_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.DIW_FLUSH)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank B DIW FLUSH Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_DIW_FLUSH_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank B DIW FLUSH AND SUPPLY Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_DIW_FLUSH_AND_SUPPLY_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank B DIW FLUSH Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_CHEM_FLUSH_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank B DIW FLUSH AND SUPPLY Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_CHEM_FLUSH_AND_SUPPLY_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    else if (selected_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                    {
                        Program.eventlog_form.Insert_Event("Semi Auto - Tank B AUTO FLUSH Start", (int)frm_eventlog.enum_event_type.SEMI_AUTO_CHEM_FLUSH_AND_SUPPLY_START, (int)frm_eventlog.enum_occurred_type.USER, true);
                    }
                    if (Program.main_md.Message_By_Application("Semi Auto Tank B - " + selected_type.ToString().Replace('_', ' ') + " Running?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                    {
                        Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, true); Tank_Value_Clear(tank_class.enum_tank_type.TANK_B, true);
                        Program.seq.semi_auto_tank_b.semi_auto_type = selected_type;

                        Program.seq.semi_auto_tank_b.no_cur = tank_class.enum_seq_no_semi_auto.INITIAL;
                        timer_manual_sequence_tank_b.Interval = 200; timer_manual_sequence_tank_b.Enabled = true;
                    }
                }
            }

        }
        public int Concentration_Check(tank_class.enum_tank_type selected_tank, bool call_by_supply_or_charge)
        {
            //APM, DHF, DSP MIX
            int result = 0;
            string log = "";
            //농도 이상 확인

            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                //CCSS1 H2O2
                if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) <= Program.ABB.concentration_1
                        && Program.ABB.concentration_1 <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High))
                {
                    log = "Chem1 Concentration OK Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) +
                        " Value : " + Program.ABB.concentration_1 +
                        " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High);
                }
                else
                {
                    log = "Chem1 Concentration Fail Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) +
                             " Value : " + Program.ABB.concentration_1 +
                             " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High);
                    result += 1; Program.seq.supply.c_c_need_text = "Chem1_Concentration Error";
                }


                if (call_by_supply_or_charge == true)
                {
                    if (selected_tank == tank_class.enum_tank_type.TANK_A) { Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss1 = Program.ABB.concentration_1; }
                    else if (selected_tank == tank_class.enum_tank_type.TANK_B) { Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss1 = Program.ABB.concentration_1; }
                }
                else
                {
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) > Program.ABB.concentration_1)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Low_Concentration_Chem1, "Concentration1 : " + Program.ABB.concentration_1, true, false);
                    }
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High) < Program.ABB.concentration_1)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.High_Concentration_Chem1, "Concentration1 : " + Program.ABB.concentration_1, true, false);
                    }
                }

                if (Program.ABB.concentration_1 == Program.ABB.concentration_1_old)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Chemical_CM_Value_No_Change, "Concentration1 : " + Program.ABB.concentration_1, true, false);
                }
                Program.ABB.concentration_1_old = Program.ABB.concentration_1;

                //CCSS2 NH4OH
                if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_Low) <= Program.ABB.concentration_2
                       && Program.ABB.concentration_2 <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_High))
                {
                    log = "Chem2 Concentration OK Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_Low) +
                        " Value : " + Program.ABB.concentration_2 +
                        " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_High);
                }
                else
                {
                    log = "Chem2 Concentration Fail Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_Low) +
                        " Value : " + Program.ABB.concentration_2 +
                        " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_High);
                    result += 1; Program.seq.supply.c_c_need_text = "Chem2_Concentration Error";
                }

                if (call_by_supply_or_charge == true)
                {
                    if (selected_tank == tank_class.enum_tank_type.TANK_A) { Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss2 = Program.ABB.concentration_2; }
                    else if (selected_tank == tank_class.enum_tank_type.TANK_B) { Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss2 = Program.ABB.concentration_2; }
                }
                else
                {
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_Low) > Program.ABB.concentration_2)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Low_Concentration_Chem2, "Concentration2 : " + Program.ABB.concentration_2, true, false);
                    }
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_High) < Program.ABB.concentration_2)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.High_Concentration_Chem2, "Concentration2 : " + Program.ABB.concentration_2, true, false);
                    }
                }


                if (Program.ABB.concentration_2 == Program.ABB.concentration_2_old)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Chemical_CM_Value_No_Change, "Concentration2 : " + Program.ABB.concentration_1, true, false);
                }
                Program.ABB.concentration_2_old = Program.ABB.concentration_2;
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                //CCSS1 DSP
                if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) <= Program.ABB.concentration_1
                        && Program.ABB.concentration_1 <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High))
                {
                    log = "Chem1 Concentration OK Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) +
                        " Value : " + Program.ABB.concentration_1 +
                        " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High);
                }
                else
                {
                    log = "Chem1 Concentration Fail Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) +
                             " Value : " + Program.ABB.concentration_1 +
                             " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High);
                    result += 1; Program.seq.supply.c_c_need_text = "Chem1_Concentration Error";
                }


                if (call_by_supply_or_charge == true)
                {
                    if (selected_tank == tank_class.enum_tank_type.TANK_A) { Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss1 = Program.ABB.concentration_1; }
                    else if (selected_tank == tank_class.enum_tank_type.TANK_B) { Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss1 = Program.ABB.concentration_1; }
                }
                else
                {
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) > Program.ABB.concentration_1)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Low_Concentration_Chem1, "Concentration1 : " + Program.ABB.concentration_1, true, false);
                    }
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High) < Program.ABB.concentration_1)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.High_Concentration_Chem1, "Concentration1 : " + Program.ABB.concentration_1, true, false);
                    }
                }

                if (Program.ABB.concentration_1 == Program.ABB.concentration_1_old)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Chemical_CM_Value_No_Change, "Concentration1 : " + Program.ABB.concentration_1, true, false);
                }
                Program.ABB.concentration_1_old = Program.ABB.concentration_1;
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                //CCSS1 DHF
                if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) <= Program.main_form.SerialData.CM210DC.Concentration
                        && Program.main_form.SerialData.CM210DC.Concentration <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High))
                {
                    log = "Chem1 Concentration OK Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) +
                        " Value : " + Program.main_form.SerialData.CM210DC.Concentration +
                        " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High);
                }
                else
                {
                    result += 1; Program.seq.supply.c_c_need_text = "Chem1_Concentration Error";
                }

                if (call_by_supply_or_charge == true)
                {
                    if (selected_tank == tank_class.enum_tank_type.TANK_A) { Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss1 = Program.main_form.SerialData.CM210DC.Concentration; }
                    else if (selected_tank == tank_class.enum_tank_type.TANK_B) { Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss1 = Program.main_form.SerialData.CM210DC.Concentration; }
                }
                else
                {
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) > Program.main_form.SerialData.CM210DC.Concentration)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Low_Concentration_Chem1, "Concentration1 : " + Program.main_form.SerialData.CM210DC.Concentration, true, false);
                    }
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High) < Program.main_form.SerialData.CM210DC.Concentration)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.High_Concentration_Chem1, "Concentration1 : " + Program.main_form.SerialData.CM210DC.Concentration, true, false);
                    }
                }



                if (Program.main_form.SerialData.CM210DC.Concentration == Program.main_form.SerialData.CM210DC.concentration_old)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Chemical_CM_Value_No_Change, "Concentration1 : " + Program.main_form.SerialData.CM210DC.Concentration, true, false);
                }
                Program.main_form.SerialData.CM210DC.concentration_old = Program.main_form.SerialData.CM210DC.Concentration;
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                //CCSS1 LAL
                if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) <= Program.main_form.SerialData.CS600F.concentration
                        && Program.main_form.SerialData.CS600F.concentration <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High))
                {
                    log = "Chem1 Concentration OK Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) +
                        " Value : " + Program.main_form.SerialData.CS600F.concentration +
                        " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High);
                }
                else
                {
                    result += 1; Program.seq.supply.c_c_need_text = "Chem1_Concentration Error";
                }

                if (call_by_supply_or_charge == true)
                {
                    if (selected_tank == tank_class.enum_tank_type.TANK_A) { Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss1 = Program.main_form.SerialData.CS600F.concentration; }
                    else if (selected_tank == tank_class.enum_tank_type.TANK_B) { Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss1 = Program.main_form.SerialData.CS600F.concentration; }
                }
                else
                {
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) > Program.main_form.SerialData.CS600F.concentration)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Low_Concentration_Chem1, "Concentration1 : " + Program.main_form.SerialData.CS600F.concentration, true, false);
                    }
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High) < Program.main_form.SerialData.CS600F.concentration)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.High_Concentration_Chem1, "Concentration1 : " + Program.main_form.SerialData.CS600F.concentration, true, false);
                    }
                }

                if (Program.main_form.SerialData.CS600F.concentration == Program.main_form.SerialData.CS600F.concentration_old)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Chemical_CM_Value_No_Change, "Concentration1 : " + Program.main_form.SerialData.CS600F.concentration, true, false);
                }
                Program.main_form.SerialData.CS600F.concentration_old = Program.main_form.SerialData.CS600F.concentration;
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                //CCSS1 H2O2
                if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) <= Program.main_form.SerialData.CS150C.h2o2_concentration
                        && Program.main_form.SerialData.CS150C.h2o2_concentration <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High))
                {
                    log = "Chem1 Concentration OK Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) +
                        " Value : " + Program.main_form.SerialData.CS150C.h2o2_concentration +
                        " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High);
                }
                else
                {
                    log = "Chem1 Concentration Fail Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) +
                             " Value : " + Program.main_form.SerialData.CS150C.h2o2_concentration +
                             " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High);
                    result += 1; Program.seq.supply.c_c_need_text = "Chem1_Concentration Error";
                }


                if (call_by_supply_or_charge == true)
                {
                    if (selected_tank == tank_class.enum_tank_type.TANK_A) { Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss1 = Program.main_form.SerialData.CS150C.h2o2_concentration; }
                    else if (selected_tank == tank_class.enum_tank_type.TANK_B) { Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss1 = Program.main_form.SerialData.CS150C.h2o2_concentration; }
                }
                else
                {
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_Low) > Program.main_form.SerialData.CS150C.h2o2_concentration)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Low_Concentration_Chem1, "Concentration1 : " + Program.main_form.SerialData.CS150C.h2o2_concentration, true, false);
                    }
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem1_Concentration_High) < Program.main_form.SerialData.CS150C.h2o2_concentration)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.High_Concentration_Chem1, "Concentration1 : " + Program.main_form.SerialData.CS150C.h2o2_concentration, true, false);
                    }
                }

                if (Program.main_form.SerialData.CS150C.h2o2_concentration == Program.main_form.SerialData.CS150C.h2o2_concentration_old)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Chemical_CM_Value_No_Change, "Concentration1 : " + Program.main_form.SerialData.CS150C.h2o2_concentration, true, false);
                }
                Program.main_form.SerialData.CS150C.h2o2_concentration = Program.main_form.SerialData.CS150C.h2o2_concentration_old;
                //CCSS2 H2SO4
                if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_Low) <= Program.main_form.SerialData.CS150C.h2so4_concentration
                        && Program.main_form.SerialData.CS150C.h2so4_concentration <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_High))
                {
                    log = "Chem2 Concentration OK Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_Low) +
                        " Value : " + Program.main_form.SerialData.CS150C.h2so4_concentration +
                        " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_High);
                }
                else
                {
                    log = "Chem2 Concentration Fail Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_Low) +
                             " Value : " + Program.main_form.SerialData.CS150C.h2so4_concentration +
                             " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_High);
                    result += 1; Program.seq.supply.c_c_need_text = "Chem2_Concentration Error";
                }


                if (call_by_supply_or_charge == true)
                {
                    if (selected_tank == tank_class.enum_tank_type.TANK_A) { Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss2 = Program.main_form.SerialData.CS150C.h2so4_concentration; }
                    else if (selected_tank == tank_class.enum_tank_type.TANK_B) { Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss2 = Program.main_form.SerialData.CS150C.h2so4_concentration; }
                }
                else
                {
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_Low) > Program.main_form.SerialData.CS150C.h2so4_concentration)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Low_Concentration_Chem2, "Concentration2 : " + Program.main_form.SerialData.CS150C.h2so4_concentration, true, false);
                    }
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem2_Concentration_High) < Program.main_form.SerialData.CS150C.h2so4_concentration)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.High_Concentration_Chem2, "Concentration2 : " + Program.main_form.SerialData.CS150C.h2so4_concentration, true, false);
                    }
                }

                if (Program.main_form.SerialData.CS150C.h2so4_concentration == Program.main_form.SerialData.CS150C.h2so4_concentration_old)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Chemical_CM_Value_No_Change, "Concentration2 : " + Program.main_form.SerialData.CS150C.h2so4_concentration, true, false);
                }
                Program.main_form.SerialData.CS150C.h2so4_concentration = Program.main_form.SerialData.CS150C.h2so4_concentration_old;
                //CCSS3 DHF
                if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Concentration_Low) <= Program.main_form.SerialData.HF700.concentration
                 && Program.main_form.SerialData.HF700.concentration <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Concentration_High))
                {
                    log = "Chem3 Concentration OK Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Concentration_Low) +
                        " Value : " + Program.main_form.SerialData.HF700.concentration +
                        " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Concentration_High);
                }
                else
                {
                    log = "Chem3 Concentration Fail Low : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Concentration_Low) +
                             " Value : " + Program.main_form.SerialData.HF700.concentration +
                             " High : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Concentration_High);
                    result += 1; Program.seq.supply.c_c_need_text = "Chem3_Concentration Error";
                }


                if (call_by_supply_or_charge == true)
                {
                    if (selected_tank == tank_class.enum_tank_type.TANK_A) { Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss3 = Program.main_form.SerialData.HF700.concentration; }
                    else if (selected_tank == tank_class.enum_tank_type.TANK_B) { Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss3 = Program.main_form.SerialData.HF700.concentration; }
                }
                else
                {
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Concentration_Low) > Program.main_form.SerialData.HF700.concentration)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Low_Concentration_Chem3, "Concentration3 : " + Program.main_form.SerialData.HF700.concentration, true, false);
                    }
                    if (Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Chem3_Concentration_High) < Program.main_form.SerialData.HF700.concentration)
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.High_Concentration_Chem3, "Concentration3 : " + Program.main_form.SerialData.HF700.concentration, true, false);
                    }
                }

                if (Program.main_form.SerialData.HF700.concentration == Program.main_form.SerialData.HF700.concentration_old)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Chemical_CM_Value_No_Change, "Concentration3 : " + Program.main_form.SerialData.HF700.concentration, true, false);
                }
                Program.main_form.SerialData.HF700.concentration = Program.main_form.SerialData.HF700.concentration_old;
            }
            else
            {
                result = 0;
            }

            if (call_by_supply_or_charge == true)
            {
                Program.eventlog_form.Insert_Event(log, (int)frm_eventlog.enum_event_type.CONCENTRATION, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
            }
            return result;
        }
        public void UI_Change()
        {
            try
            {
                this.SuspendLayout();
                Tank_Button_Status_Change();

                txt_seq_main.Text = Program.seq.main.state_display;
                txt_seq_supply.Text = Program.seq.supply.state_display;
                txt_seq_semi_auto_tank_a.Text = Program.seq.semi_auto_tank_a.state_display;
                txt_seq_semi_auto_tank_b.Text = Program.seq.semi_auto_tank_b.state_display;
                txt_seq_semi_auto_tank_all.Text = Program.seq.semi_auto_tank_all.state_display;

                Program.tank[(int)tank_class.enum_tank_type.TANK_A].enable = Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].enable = Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable;

                if (timer_manual_sequence_tank_a.Enabled == true || timer_manual_sequence_tank_b.Enabled == true)
                {
                    Program.cg_app_info.semi_auto_state = true;
                }
                else
                {
                    Program.cg_app_info.semi_auto_state = false;
                }

                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    APM.UI_Change();
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    IPA.UI_Change();
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    DSP.UI_Change();
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    DSP_MIX.UI_Change();
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    DHF.UI_Change();
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    LAL.UI_Change();
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".UI_Change." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);

            }
            finally
            {
                this.ResumeLayout();
            }
        }
        public string Tank_Volume_View(tank_class selected_tank)
        {
            string result = "";
            //APM의 경우 고객 요청에 따라 NH4OH를 첫번째 표기
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Mixing_Rate) != 0)
                {
                    if (result != "") { result = result + "/"; }
                    result = result + string.Format("{0:f2}", selected_tank.ccss_data[1].input_volmue_real) + "(" + string.Format("{0:f2}", selected_tank.ccss_data[1].input_volume) + ")";
                }
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Mixing_Rate) != 0)
                {
                    if (result != "") { result = result + "/"; }
                    result = result + string.Format("{0:f2}", selected_tank.ccss_data[0].input_volmue_real) + "(" + string.Format("{0:f2}", selected_tank.ccss_data[0].input_volume) + ")";
                }
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Mixing_Rate) != 0)
                {
                    if (result != "") { result = result + "/"; }
                    result = result + string.Format("{0:f2}", selected_tank.ccss_data[2].input_volmue_real) + "(" + string.Format("{0:f2}", selected_tank.ccss_data[2].input_volume) + ")";
                }
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Mixing_Rate) != 0)
                {
                    if (result != "") { result = result + "/"; }
                    result = result + string.Format("{0:f2}", selected_tank.ccss_data[3].input_volmue_real) + "(" + string.Format("{0:f2}", selected_tank.ccss_data[3].input_volume) + ")";
                }
            }
            else
            {
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Mixing_Rate) != 0)
                {
                    if (result != "") { result = result + "/"; }
                    result = result + string.Format("{0:f2}", selected_tank.ccss_data[0].input_volmue_real) + "(" + string.Format("{0:f2}", selected_tank.ccss_data[0].input_volume) + ")";
                }
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Mixing_Rate) != 0)
                {
                    if (result != "") { result = result + "/"; }
                    result = result + string.Format("{0:f2}", selected_tank.ccss_data[1].input_volmue_real) + "(" + string.Format("{0:f2}", selected_tank.ccss_data[1].input_volume) + ")";
                }
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Mixing_Rate) != 0)
                {
                    if (result != "") { result = result + "/"; }
                    result = result + string.Format("{0:f2}", selected_tank.ccss_data[2].input_volmue_real) + "(" + string.Format("{0:f2}", selected_tank.ccss_data[2].input_volume) + ")";
                }
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Mixing_Rate) != 0)
                {
                    if (result != "") { result = result + "/"; }
                    result = result + string.Format("{0:f2}", selected_tank.ccss_data[3].input_volmue_real) + "(" + string.Format("{0:f2}", selected_tank.ccss_data[3].input_volume) + ")";
                }
            }

            return result;
        }
        public bool Visible_CC_status(tank_class.enum_tank_type selected_tank)
        {
            if (selected_tank == tank_class.enum_tank_type.TANK_A)
            {
                if (Program.seq.supply.c_c_need == false && Program.seq.supply.ready_flag == false)
                {
                    return false;
                }
                else
                {
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (selected_tank == tank_class.enum_tank_type.TANK_B)
            {
                if (Program.seq.supply.c_c_need == false && Program.seq.supply.ready_flag == false)
                {
                    return false;
                }
                else
                {
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        public bool Visible_Confirm_status(tank_class.enum_tank_type selected_tank)
        {
            if (selected_tank == tank_class.enum_tank_type.TANK_A)
            {
                if (Program.seq.supply.c_c_need == false && Program.seq.supply.ready_flag == false)
                {
                    return false;
                }
                else
                {
                    return true;
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY)
                    {
                        if (Program.seq.supply.req_c_c_start_cds_to_ctc == true || Program.seq.supply.ready_flag == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (Program.seq.supply.ready_flag == true)
                        {
                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.READY)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else if (selected_tank == tank_class.enum_tank_type.TANK_B)
            {
                if (Program.seq.supply.c_c_need == false && Program.seq.supply.ready_flag == false)
                {
                    return false;
                }
                else
                {
                    return true;
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY)
                    {
                        if (Program.seq.supply.req_c_c_start_cds_to_ctc == true || Program.seq.supply.ready_flag == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (Program.seq.supply.ready_flag == true)
                        {
                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.READY)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }
        public Color BackColor_Tank_By_TempController_status(ref Class_TempController_M74.ST_TempController_M74.ST_M74_Unit unit)
        {
            Color result = Color.White;

            if (unit.mode == Class_TempController_M74.enum_m74_status.at)
            {
                result = Color.Lime;
            }
            else if (unit.mode == Class_TempController_M74.enum_m74_status.run)
            {
                result = Color.Orange;
            }
            else if (unit.mode == Class_TempController_M74.enum_m74_status.error)
            {
                result = Color.Red;
            }
            else if (unit.mode == Class_TempController_M74.enum_m74_status.stop)
            {
                result = Color.White;
            }
            return result;
        }
        public string Label_Tank_Level_status(tank_class.enum_tank_type selected_tank, enum_tank_level tank_level)
        {
            string result = "";

            if (tank_level == enum_tank_level.HH)
            {
                if (selected_tank == enum_tank_type.TANK_A)
                {
                    result = tank_level.ToString() + System.Environment.NewLine + "(" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_HH) + "L)";
                }
                else if (selected_tank == enum_tank_type.TANK_B)
                {
                    result = tank_level.ToString() + System.Environment.NewLine + "(" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_B_Level_HH) + "L)";
                }
            }
            else if (tank_level == enum_tank_level.H)
            {
                if (selected_tank == enum_tank_type.TANK_A)
                {
                    result = tank_level.ToString() + System.Environment.NewLine + "(" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_H) + "L)";
                }
                else if (selected_tank == enum_tank_type.TANK_B)
                {
                    result = tank_level.ToString() + System.Environment.NewLine + "(" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_B_Level_H) + "L)";
                }
            }
            else if (tank_level == enum_tank_level.M)
            {
                if (selected_tank == enum_tank_type.TANK_A)
                {
                    result = tank_level.ToString() + System.Environment.NewLine + "(" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_M) + "L)";
                }
                else if (selected_tank == enum_tank_type.TANK_B)
                {
                    result = tank_level.ToString() + System.Environment.NewLine + "(" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_B_Level_M) + "L)";
                }
            }
            else if (tank_level == enum_tank_level.L)
            {
                if (selected_tank == enum_tank_type.TANK_A)
                {
                    result = tank_level.ToString() + System.Environment.NewLine + "(" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_L) + "L)";
                }
                else if (selected_tank == enum_tank_type.TANK_B)
                {
                    result = tank_level.ToString() + System.Environment.NewLine + "(" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_B_Level_L) + "L)";
                }
            }
            else if (tank_level == enum_tank_level.LL)
            {
                if (selected_tank == enum_tank_type.TANK_A)
                {
                    result = tank_level.ToString() + System.Environment.NewLine + "(" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_LL) + "L)";
                }
                else if (selected_tank == enum_tank_type.TANK_B)
                {
                    result = tank_level.ToString() + System.Environment.NewLine + "(" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_B_Level_LL) + "L)";
                }
            }
            else
            {
                result = "";
            }
            return result;
        }
        private void frm_schematic_FormClosed(object sender, FormClosedEventArgs e)
        {
            Frm_Dispose();
        }
        public void Frm_Dispose()
        {
            if (thd_seq_main != null) { thd_seq_main.Abort(); thd_seq_main = null; }
            if (thd_seq_monitoring != null) { thd_seq_monitoring.Abort(); thd_seq_monitoring = null; }
            if (thd_seq_pump_control != null) { thd_seq_pump_control.Abort(); thd_seq_pump_control = null; }
            if (thd_seq_supply != null) { thd_seq_supply.Abort(); thd_seq_supply = null; }

            if (thd_seq_circulation_monitoring != null) { thd_seq_circulation_monitoring.Abort(); thd_seq_circulation_monitoring = null; }
            if (thd_seq_drain_pump_control != null) { thd_seq_drain_pump_control.Abort(); thd_seq_drain_pump_control = null; }
            if (thd_seq_drain_valve_monitoring != null) { thd_seq_drain_valve_monitoring.Abort(); thd_seq_drain_valve_monitoring = null; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Program.main_form.chk_storke_mode.Visible = true;
            Program.main_form.label1.Visible = true;
            Program.main_form.numeric_interval.Visible = true;
            Program.main_form.chk_storke_mode.Visible = true;
            Program.main_form.btn_ctc_to_cds_supply_start_req.Visible = true;
            Program.main_form.btn_ctc_to_cds_cc_confirm.Visible = true;
        }
        private void btn_seq_viewer_Click(object sender, EventArgs e)
        {
            fpnl_seq_monitor.Visible = false;
            Program.main_form.label1.Visible = false;
            Program.main_form.numeric_interval.Visible = false;
            Program.main_form.chk_storke_mode.Visible = false;
            Program.main_form.btn_ctc_to_cds_supply_start_req.Visible = false;
            Program.main_form.btn_ctc_to_cds_cc_confirm.Visible = false;

        }
        public void HotDiw_Manage()
        {
            bool tmp_dio = true;
            if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY)
            {
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, false);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HOT_DIW_BY_PASS, true);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                //Auto 상태에서 Supply Tank가 없는 경우 HDIW Remote Start Output Signal On한 후 TS-09 온도센서의 온도 값을 확인 후 Tank로 공급한다.
                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                {
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status != tank_class.enum_tank_status.SUPPLY
                        && Program.tank[(int)tank_class.enum_tank_type.TANK_B].status != tank_class.enum_tank_status.SUPPLY)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, true);
                    }
                    //Tank A Middle Level Off시 사전 준비
                    else if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY)
                    {
                        tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value;
                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (tmp_dio == false) { Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, true); }
                        }
                    }
                    //Tank B Middle Level Off시 사전 준비
                    else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY)
                    {
                        tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value;
                        // Middle Level Off시 사전 준비
                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (tmp_dio == false) { Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, true); }
                        }
                    }
                    else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY)
                    {
                        tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value;
                        // Middle Level Off시 사전 준비
                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (tmp_dio == false) { Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, true); }
                        }
                    }
                    else if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY
                        && Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, false);
                    }
                    else
                    {
                        //Auto Mode일 때는 Seqeuence에서만 Close 한다.
                        //Input 완료 후 등
                    }
                }

                else if (Program.cg_app_info.eq_mode == enum_eq_mode.manual && (Program.schematic_form.timer_manual_sequence_tank_a.Enabled == true || Program.schematic_form.timer_manual_sequence_tank_b.Enabled == true))
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, true);
                }

                else if (Program.cg_app_info.eq_mode == enum_eq_mode.manual && Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, true);
                }
                else if (Program.cg_app_info.eq_mode == enum_eq_mode.manual && Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, false);
                }
                //HDIW_Temp_Check();
            }

        }

    }
}