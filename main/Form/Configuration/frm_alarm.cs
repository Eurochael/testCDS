using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace cds
{
    public partial class frm_alarm : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public Query_Parameter query_Parameter = new Query_Parameter();

        public RepositoryItemTextEdit Grid_Txt_Readonly = new RepositoryItemTextEdit();
        public RepositoryItemTextEdit Grid_Txt = new RepositoryItemTextEdit();
        public RepositoryItemComboBox Grid_Combo_type = new RepositoryItemComboBox();

        public DataSet dataset_bind_alarm_config = new DataSet();

        private bool cell_value_changed = false;
        private bool cell_value_saved = false;
        public frm_alarm()
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
                        btn_search.PerformClick();
                        timer_change_check.Interval = 200; timer_change_check.Enabled = true;
                    }
                    else
                    {
                        timer_change_check.Enabled = false;
                    }
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + ".actived." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
            }
        }

        public enum enum_level
        {
            WARNING = 0, LIGHT = 1, HEAVY = 2, ALL = 5
        }

        public enum enum_cleared_by
        {
            //0:reset, 1:pc reboot, 2:null
            USER = 0,
            CTC = 1,
            REBOOT = 2,

        }
        public enum enum_alarm
        {
            E_RIGHT_Door_Open = 1,
            ELEC_DOOR_OVERRIDE = 2,
            C_Door_Open = 3,
            GPS_UPS_TRIP = 4,
            Connect_Fail_Host = 5,
            Cooling_Left_Fan_Error = 6,
            Cooling_Right_Fan_Error = 7,
            CDS_Alive_Interlock = 8,
            Stop_EMS = 9,
            Stop_Power_Off = 10,
            Stop_Main_EMS = 11,
            Stop_Main_Power_Off = 12,
            Stop_Main_GAS_Dectect = 13,
            Leak_Bottom_Vat = 14,
            Leak_Tank_Vat = 15,
            Leak_Dike = 16,
            Interlock_Trip = 17,
            Chemical_CM_Value_No_Change = 18,
            CM_Fail_Prep_Tank_A = 22,
            CM_Fail_Prep_Tank_B = 23,
            Prep_Pump_Operation_Error = 24,
            Application_Start_Error_Serial_Daemon = 27,
            Application_Start_Error_Log_Manager = 28,
            Database_Alarm_ID_Not_Register = 29,
            Database_Parameter_ID_Not_Register = 30,
            CTC_Database_Exception = 51,
            No_Space_Disk_Storage_Log = 52,
            No_Space_Disk_Storage_Program = 53,
            Error_Log_Manager_Run = 54,
            Ethercat_Communication_Error = 55,
            Over_Time_Chemical_Change_Request = 61,
            Fail_Chemical_Change_Command = 62,
            SemiAuto_Mode_Operation = 63,
            Abnormal_Flow_Dectect_Chem1 = 101,
            High_Pressure_Chem_In_CCSS1 = 102,
            Low_Pressure_Chem_In_CCSS1 = 103,
            High_Pressure_Chem_Out_CCSS1 = 107,
            Low_Pressure_Chem_Out_CCSS1 = 108,
            Supply_Max_Time_Over_Tank_A_Chem1 = 109,
            Supply_Min_Time_Over_Tank_A_Chem1 = 110,
            Supply_Max_Time_Over_Tank_B_Chem1 = 111,
            Supply_Min_Time_Over_Tank_B_Chem1 = 112,
            High_Flowrate_CCSS1 = 113,
            Low_Flowrate_CCSS1 = 114,
            Abnormal_Flow_Dectect_Chem2 = 141,
            High_Pressure_Chem_In_CCSS2 = 142,
            Low_Pressure_Chem_In_CCSS2 = 143,
            High_Pressure_Chem_Out_CCSS2 = 147,
            Low_Pressure_Chem_Out_CCSS2 = 148,
            Supply_Max_Time_Over_Tank_A_Chem2 = 149,
            Supply_Min_Time_Over_Tank_A_Chem2 = 150,
            Supply_Max_Time_Over_Tank_B_Chem2 = 151,
            Supply_Min_Time_Over_Tank_B_Chem2 = 152,
            High_Flowrate_CCSS2 = 153,
            Low_Flowrate_CCSS2 = 154,
            Abnormal_Flow_Dectect_Chem3 = 181,
            High_Pressure_Chem_In_CCSS3 = 182,
            Low_Pressure_Chem_In_CCSS3 = 183,
            High_Pressure_Chem_Out_CCSS3 = 189,
            Low_Pressure_Chem_Out_CCSS3 = 190,
            Supply_Max_Time_Over_Tank_A_Chem3 = 191,
            Supply_Min_Time_Over_Tank_A_Chem3 = 192,
            Supply_Max_Time_Over_Tank_B_Chem3 = 197,
            Supply_Min_Time_Over_Tank_B_Chem3 = 198,
            High_Flowrate_CCSS3 = 199,
            Low_Flowrate_CCSS3 = 200,
            Abnormal_Flow_Dectect_Chem4 = 221,
            High_Pressure_Chem_In_CCSS4 = 222,
            Low_Pressure_Chem_In_CCSS4 = 223,
            High_Pressure_Chem_Out_CCSS4 = 227,
            Low_Pressure_Chem_Out_CCSS4 = 228,
            Supply_Max_Time_Over_Tank_A_Chem4 = 229,
            Supply_Min_Time_Over_Tank_A_Chem4 = 230,
            Supply_Max_Time_Over_Tank_B_Chem4 = 231,
            Supply_Min_Time_Over_Tank_B_Chem4 = 232,
            High_Flowrate_CCSS4 = 233,
            Low_Flowrate_CCSS4 = 234,
            High_MAIN_CDA1_PRESSPUMP = 261,
            Low_MAIN_CDA1_PRESSPUMP = 262,
            High_MAIN_CDA2_PRESSSOL = 263,
            Low_MAIN_CDA2_PRESSSOL = 264,
            High_MAIN_CDA3_PRESSDRAIN = 265,
            Low_MAIN_CDA3_PRESSDRAIN = 266,
            High_Exhaust = 267,
            Low_Exhaust = 268,
            High_Pressure_Main_N2 = 269,
            Low_Pressure_Main_N2 = 270,
            High_Pressure_PCW = 275,
            Low_Pressure_PCW = 276,
            High_Circulation_Flow_PCW = 277,
            Low_Circulation_Flow_PCW = 278,
            High_Supply_A_Flow_PCW = 279,
            Low_Supply_A_Flow_PCW = 280,
            High_Supply_B_Flow_PCW = 281,
            Low_Supply_B_Flow_PCW = 282,
            High_Concentration_Chem1 = 283,
            Low_Concentration_Chem1 = 284,
            High_Concentration_Chem2 = 285,
            Low_Concentration_Chem2 = 286,
            High_Concentration_Chem3 = 287,
            Low_Concentration_Chem3 = 288,
            High_Concentration_Chem4 = 289,
            Low_Concentration_Chem4 = 290,
            HDIW_Supply_Temp_Error = 291,
            High_Flow_HDIW = 292,
            Low_Flow_HDIW = 293,
            Tank_Level_Not_HighUsing_Mixing_Tank_A = 294,
            Tank_Level_Not_HighUsing_Mixing_Tank_B = 295,
            Level_Low_Low_Tank_A = 301,
            Heating_Time_Over_Temp_Controll_Tank_A = 302,
            High_Temperature_Tank_A = 303,
            Low_Temperature_Tank_A = 304,
            Level_High_High_Tank_A = 305,
            High_Flow_Tank_A_N2 = 306,
            Low_Flow_Tank_A_N2 = 307,
            Life_Time_Over_Tank_A = 308,
            Charge_Time_Over_Tank_A = 309,
            Concentration_Fail_Tank_A = 310,
            Drain_Time_Over_Tank_A = 311,
            Level_Empty_Fail_Tank_A = 312,
            Not_Ready_Tank_A = 313,
            Ready_Time_Over_Tank_A = 314,
            Level_Sensor_Weird_Tank_A = 316,
            Over_Flow_Tank_A = 317,
            Use_Time_Over_Tank_A = 318,
            Heater_Interlock_Tank_A = 319,
            Level_Low_Low_Tank_B = 401,
            Heating_Time_Over_Temp_Controll_Tank_B = 402,
            High_Temperature_Tank_B = 403,
            Low_Temperature_Tank_B = 404,
            Level_High_High_Tank_B = 405,
            High_Flow_Tank_B_N2 = 406,
            Low_Flow_Tank_B_N2 = 407,
            Charge_Time_Over_Tank_B = 408,
            Life_Time_Over_Tank_B = 409,
            Process_Ready_Off = 410,
            Drain_Time_Over_Tank_B = 411,
            Level_Empty_Fail_Tank_B = 412,
            Not_Ready_Tank_B = 413,
            Ready_Time_Over_Tank_B = 414,
            Level_Sensor_Weird_Tank_B = 416,
            Over_Flow_Tank_B = 417,
            Use_Time_Over_Tank_B = 418,
            Heater_Interlock_Tank_B = 419,
            Concentration_Fail_Tank_B = 529,
            Can_Not_Use_Tank = 530,
            High_Pressure_Tank_Circulation_Pump = 711,
            Low_Pressure_Tank_Circulation_Pump = 712,
            High_Flow_Tank_Circulation_Pump = 721,
            Low_Flow_Tank_Circulation_Pump = 722,
            Pump_Empty_Trouble_Tank_Circulation_Pump = 723,
            Pump_Empty_Trouble_Supply_A_Pump = 724,
            Pump_Empty_Trouble_Supply_B_Pump = 725,
            Circulation_Pump_Error_In_Charge = 761,
            Heater_Interlock_Circulation = 762,
            Heater_Interlock_Circulation2 = 763,
            Leak_Circulation_Pump_Leak = 800,
            Leak_Circulation_Pump_L_Leak = 801,
            Leak_Circulation_Pump_R_Leak = 802,
            Circulation_Pump_Stroke1 = 803,
            Circulation_Pump_Stroke2 = 804,
            High_Pressure_IN_Supply_A = 911,
            Low_Pressure_IN_Supply_A = 912,
            High_Pressure_OUT_Supply_A = 913,
            Low_Pressure_OUT_Supply_A = 914,
            High_Pressure_IN_Supply_B = 915,
            Low_Pressure_IN_Supply_B = 916,
            High_Pressure_OUT_Supply_B = 917,
            Low_Pressure_OUT_Supply_B = 918,
            High_Flow_Supply_A = 919,
            Low_Flow_Supply_A = 920,
            High_Flow_Supply_B = 921,
            Low_Flow_Supply_B = 922,
            High_Abnormal_Temp_In_Tank_A = 941,
            Low_Abnormal_Temp_In_Tank_A = 942,
            High_Abnormal_Temp_In_Tank_B = 943,
            Low_Abnormal_Temp_In_Tank_B = 944,
            Leak_Supply_A_Pump_Leak = 1000,
            Leak_Supply_B_Pump_Leak = 1001,
            Leak_Supply_A_Pump_L_Leak = 1002,
            Leak_Supply_A_Pump_R_Leak = 1003,
            Supply_A_Pump_Stroke1 = 1004,
            Supply_A_Pump_Stroke2 = 1005,
            Leak_Supply_B_Pump_L_Leak = 1006,
            Leak_Supply_B_Pump_R_Leak = 1007,
            Supply_B_Pump_Stroke1 = 1008,
            Supply_B_Pump_Stroke2 = 1009,
            High_Pressure_Return_A = 1111,
            Low_Pressure_Return_A = 1112,
            High_Pressure_Return_B = 1113,
            Low_Pressure_Return_B = 1114,
            High_Flow_Drain = 1500,
            Low_Flow_Drain = 1501,
            Level_High_High_Drain_Tank = 1504,
            HDIW_POWER_OFF = 1601,
            HDIW_REMOTE_MODE_OFF = 1602,
            HDIW_TOTAL_ALARM = 1603,
            Low_Exhaust_AlarmSensor = 1604,
            High_Pressure_Heater_N2 = 1605,
            Low_Pressure_Heater_N2 = 1606,
            Flowmeter1_Connnection_Fail = 2002,
            Flowmeter1_Alarm = 2003,
            Flowmeter1_CH1_Measure_Error = 2004,
            Flowmeter1_CH2_Measure_Error = 2005,
            Flowmeter2_Connnection_Fail = 2006,
            Flowmeter2_Alarm = 2007,
            Flowmeter2_CH1_Measure_Error = 2008,
            Flowmeter2_CH2_Measure_Error = 2009,
            PumpController1_Connection_Fail = 2010,
            PumpController1_Error = 2011,
            PumpController2_Connection_Fail = 2020,
            PumpController2_Error = 2021,
            Concentration_ABB_Connection_Fail = 2030,
            Concentration_ABB_Alarm = 2031,
            Concentration_CM210_Connection_Fail = 2040,
            Concentration_CM210_Alarm = 2041,
            Concentration_HF700_Connection_Fail = 2050,
            Concentration_HF700_Alarm = 2051,
            Concentration_CS_150C_Connection_Fail = 2060,
            Concentration_CS_150C_Alarm = 2061,
            Concentration_CS_600F_Connection_Fail = 2062,
            Concentration_CS_600F_Alarm = 2063,
            Temp_Controller1_Connection_Fail = 2070,
            Temp_Controller1_Alarm = 2071,
            Temp_Controller1_CH1_Alarm = 2072,
            Temp_Controller1_CH2_Alarm = 2073,
            Temp_Controller1_CH3_Alarm = 2074,
            Temp_Controller1_CH4_Alarm = 2075,
            Temp_Controller2_Connection_Fail = 2080,
            Temp_Controller2_Alarm = 2081,
            Temp_Controller2_CH1_Alarm = 2082,
            Temp_Controller2_CH2_Alarm = 2083,
            Temp_Controller2_CH3_Alarm = 2084,
            Temp_Controller2_CH4_Alarm = 2085,
            Temp_Controller3_Connection_Fail = 2090,
            Temp_Controller3_Alarm = 2091,
            Temp_Controller3_CH1_Alarm = 2092,
            Temp_Controller3_CH2_Alarm = 2093,
            Temp_Controller3_CH3_Alarm = 2094,
            Temp_Controller3_CH4_Alarm = 2095,
            Temp_Controller4_Connection_Fail = 2100,
            Temp_Controller4_Alarm = 2101,
            Temp_Controller4_CH1_Alarm = 2102,
            Temp_Controller4_CH2_Alarm = 2103,
            Temp_Controller4_CH3_Alarm = 2104,
            Temp_Controller4_CH4_Alarm = 2105,
            SUPPLY_A_THERMOSTAT_Connection_Fail = 2110,
            SUPPLY_A_THERMOSTAT_Error = 2111,
            SUPPLY_A_THERMOSTAT_Encount1 = 2112,
            SUPPLY_A_THERMOSTAT_Encount2 = 2113,
            SUPPLY_A_THERMOSTAT_Encount3 = 2114,
            SUPPLY_A_THERMOSTAT_Encount4 = 2115,
            SUPPLY_A_THERMOSTAT_Encount5 = 2116,
            SUPPLY_A_THERMOSTAT_Warning1 = 2117,
            SUPPLY_B_THERMOSTAT_Connection_Fail = 2120,
            SUPPLY_B_THERMOSTAT_Error = 2121,
            SUPPLY_B_THERMOSTAT_Encount1 = 2122,
            SUPPLY_B_THERMOSTAT_Encount2 = 2123,
            SUPPLY_B_THERMOSTAT_Encount3 = 2124,
            SUPPLY_B_THERMOSTAT_Encount4 = 2125,
            SUPPLY_B_THERMOSTAT_Encount5 = 2126,
            SUPPLY_B_THERMOSTAT_Warning1 = 2127,
            CIRCULATION_THERMOSTAT_Connection_Fail = 2130,
            CIRCULATION_THERMOSTAT_Error = 2131,
            CIRCULATION_THERMOSTAT_Encount1 = 2132,
            CIRCULATION_THERMOSTAT_Encount2 = 2133,
            CIRCULATION_THERMOSTAT_Encount3 = 2134,
            CIRCULATION_THERMOSTAT_Encount4 = 2135,
            CIRCULATION_THERMOSTAT_Encount5 = 2136,
            CIRCULATION_THERMOSTAT_Warning1 = 2137,
            SCR1_Connection_Fail = 2140,
            SCR1_Error = 2141,
            SCR2_Connection_Fail = 2150,
            SCR2_Error = 2151,
            SCR3_Connection_Fail = 2160,
            SCR3_Error = 2161,
            SCR4_Connection_Fail = 2170,
            SCR4_Error = 2171,
            Purge_Unit_Alarm = 2200,
            MAIN_UNIT_MIX_TRIP = 2201,
            MAIN_EQ_TRIP = 2202,
            Leak_Fault = 2203,
            IPA_CCSS_Not_Ready_Signal = 2204,
            Heat_Exchanger_Connection_Fail = 2300,
            Heat_Exchanger_Error_D1110 = 2301,
            Heat_Exchanger_Error_D1111 = 2302,

        }

        private void frm_alarmlog_Load(object sender, EventArgs e)
        {

        }
        private void frm_alarmlog_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            int focused_rowhandle_old = -1;
            string export_csv_full_name = "";
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                if (btn_event.Name == "btn_clear")
                {
                    txt_token.Properties.Tokens.Clear();
                    txt_token.EditValue = null;
                }
                else if (btn_event.Name == "btn_search")
                {
                    Data_Initial_Check();
                    Load_AlarmList(false);
                }
                else if (btn_event.Name == "btn_cancel")
                {
                    Load_Alarm_Contents(txt_id.Text);
                }
                else if (btn_event.Name == "btn_Export")
                {
                    FolderBrowserDialog export_dir = new FolderBrowserDialog();
                    if (export_dir.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            export_csv_full_name = export_dir.SelectedPath + @"\" + "CDS_" + Program.cg_app_info.eq_type.ToString() + "_Alarm_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                            gridview_alarmconfig.ExportToCsv(export_csv_full_name);
                            Program.main_md.Message_By_Application("Export Complete", frm_messagebox.enum_apptype.Only_OK);
                        }
                        catch (Exception ex)
                        {
                            Program.main_md.Message_By_Application("Export Fail" + ex.ToString(), frm_messagebox.enum_apptype.Only_OK);
                        }
                    }
                }
                else if (btn_event.Name == "btn_change")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.auto || Program.cg_app_info.semi_auto_state == true)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto or Semi-Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                    else if (Program.main_md.user_info.type != Module_User.User_Type.Admin)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                    else
                    {
                        focused_rowhandle_old = gridview_alarmconfig.FocusedRowHandle;
                        Save_Alarm_Contents();
                        Load_Alarm_Contents(txt_id.Text);
                        Load_AlarmList(false);
                        gridview_alarmconfig.FocusedRowHandle = focused_rowhandle_old;
                    }
                }

            }
            Program.eventlog_form.Insert_Event(this.Name + ".Button Click : " + btn_event.Name, (int)frm_eventlog.enum_event_type.USER_BUTTON, (int)frm_eventlog.enum_occurred_type.USER, true);
            Program.log_md.LogWrite(this.Name + "." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);

        }

        public string Data_Initial_Check()
        {
            string result = "";
            try
            {
                grid_alarm_config.SuspendLayout();
                grid_alarm_config.DataSource = null;
                dataset_bind_alarm_config.Tables.Clear();

                query_Parameter.token.Clear();
                for (int idx = 0; idx < txt_token.Properties.Tokens.Count; idx++)
                {
                    query_Parameter.token.Add(txt_token.Properties.Tokens[idx].Description);
                }
                result = "";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Data_Initial_Check." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                grid_alarm_config.ResumeLayout();
            }
            return result;
        }
        public void Data_Setting()
        {
            int search_result = 0;
            try
            {
                grid_alarm_config.SuspendLayout();
                grid_alarm_config.DataSource = dataset_bind_alarm_config.Tables[0];

                for (int idx = 0; idx < dataset_bind_alarm_config.Tables[0].Rows.Count; idx++)
                {
                    if (dataset_bind_alarm_config.Tables[0].Rows[idx]["alarm_visible"].ToString() == "1")
                    {
                        search_result = search_result + 1;
                    }
                }
                gp_alarm_config.Text = "alarm List search result = " + search_result + "";

                if (dataset_bind_alarm_config.Tables[0].Rows.Count > 0)
                {
                    gridview_alarmconfig.FocusedRowHandle = 0;
                }
                Grid_Setting();
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Data_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                grid_alarm_config.ResumeLayout();
            }
        }
        public void Grid_Setting()
        {
            try
            {
                gridview_alarmconfig.OptionsView.ShowGroupPanel = false;
                gridview_alarmconfig.OptionsMenu.EnableGroupPanelMenu = false;
                gridview_alarmconfig.OptionsMenu.EnableColumnMenu = false;
                gridview_alarmconfig.OptionsCustomization.AllowSort = false;
                gridview_alarmconfig.OptionsCustomization.AllowFilter = false;
                gridview_alarmconfig.OptionsCustomization.AllowColumnMoving = false;
                gridview_alarmconfig.OptionsCustomization.AllowColumnResizing = false;
                gridview_alarmconfig.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
                gridview_alarmconfig.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                gridview_alarmconfig.OptionsView.ColumnAutoWidth = false;
                gridview_alarmconfig.GroupPanelText = "";
                for (int i = 0; i <= gridview_alarmconfig.Columns.Count - 1; i++)
                {
                    //공통 적용 
                    gridview_alarmconfig.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridview_alarmconfig.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gridview_alarmconfig.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridview_alarmconfig.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    if (gridview_alarmconfig.Columns[i].Name == "col" + "alarm_id")
                    {
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmconfig.Columns[i].Visible = true;
                        gridview_alarmconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmconfig.Columns[i].Width = 60;
                        gridview_alarmconfig.Columns[i].Caption = "ID";
                        gridview_alarmconfig.Columns[i].VisibleIndex = 0;
                    }
                    else if (gridview_alarmconfig.Columns[i].Name == "col" + "alarm_name")
                    {
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmconfig.Columns[i].Visible = true;
                        gridview_alarmconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmconfig.Columns[i].Width = 300;
                        gridview_alarmconfig.Columns[i].Caption = "Name";
                        gridview_alarmconfig.Columns[i].VisibleIndex = 1;
                    }
                    else if (gridview_alarmconfig.Columns[i].Name == "col" + "alarm_comment")
                    {
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmconfig.Columns[i].Visible = true;
                        gridview_alarmconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmconfig.Columns[i].Width = 500;
                        gridview_alarmconfig.Columns[i].Caption = "Comment";
                        gridview_alarmconfig.Columns[i].VisibleIndex = 6;
                    }
                    else if (gridview_alarmconfig.Columns[i].Name == "col" + "alarm_enabled")
                    {
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmconfig.Columns[i].Visible = true;
                        gridview_alarmconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmconfig.Columns[i].Width = 80;
                        gridview_alarmconfig.Columns[i].Caption = "Enable";
                        gridview_alarmconfig.Columns[i].VisibleIndex = 2;
                    }
                    else if (gridview_alarmconfig.Columns[i].Name == "col" + "alarm_wdt")
                    {
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmconfig.Columns[i].Visible = true;
                        gridview_alarmconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmconfig.Columns[i].Width = 100;
                        gridview_alarmconfig.Columns[i].Caption = "WDT";
                        gridview_alarmconfig.Columns[i].VisibleIndex = 3;
                    }
                    else if (gridview_alarmconfig.Columns[i].Name == "col" + "report_alarm_to_host")
                    {
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmconfig.Columns[i].Visible = true;
                        gridview_alarmconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmconfig.Columns[i].Width = 120;
                        gridview_alarmconfig.Columns[i].Caption = "Report Host";
                        gridview_alarmconfig.Columns[i].VisibleIndex = 5;
                    }
                    else if (gridview_alarmconfig.Columns[i].Name == "col" + "alarm_level")
                    {
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmconfig.Columns[i].Visible = true;
                        gridview_alarmconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmconfig.Columns[i].Width = 100;
                        gridview_alarmconfig.Columns[i].Caption = "Level";
                        gridview_alarmconfig.Columns[i].VisibleIndex = 4;
                    }
                    else if (gridview_alarmconfig.Columns[i].Name == "col" + "alarm_unit_id")
                    {
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmconfig.Columns[i].Visible = true;
                        gridview_alarmconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmconfig.Columns[i].Width = 120;
                        gridview_alarmconfig.Columns[i].Caption = "Unit";
                        gridview_alarmconfig.Columns[i].VisibleIndex = -1;
                    }
                    else if (gridview_alarmconfig.Columns[i].Name == "col" + "alarm_visible")
                    {
                        gridview_alarmconfig.Columns[i].Visible = false;
                    }
                    else
                    {
                        gridview_alarmconfig.Columns[i].Visible = false;
                    }

                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Grid_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { }

        }
        private void txt_token_Properties_EditValueChanged(object sender, EventArgs e)
        {
            if (txt_token.EditText == "")
            {
                txt_token.Properties.Tokens.Clear();
                txt_token.EditValue = null;
            }
        }
        private void txt_token_ValidateToken(object sender, DevExpress.XtraEditors.TokenEditValidateTokenEventArgs e)
        {
            //Console.WriteLine(e.Description);
            e.IsValid = true;
        }

        public void Alarm_Selected_Change(int focused_rowhandle)
        {
            if (focused_rowhandle < 0) { return; }
            gridview_alarmconfig.FocusedRowHandle = focused_rowhandle;
            txt_id.Text = gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_id"]).ToString();
            txt_name.Text = gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_name"]).ToString();
            txt_wdt.Text = gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_wdt"]).ToString();
            txt_level.Text = gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_level"]).ToString();
            txt_comment.Text = gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_comment"]).ToString();

            if (gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["report_alarm_to_host"]).ToString() == "1") { btn_hostsend.IsOn = true; }
            else { btn_hostsend.IsOn = false; }

            if (gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_enabled"]).ToString() == "1") { btn_use.IsOn = true; }
            else { btn_use.IsOn = false; }

            //txt_id.Text = Program.alarm_list.contents[index].id.ToString();
            //txt_name.Text = Program.alarm_list.contents[index].name.ToString();
            //txt_wdt.Text = Program.alarm_list.contents[index].wdt.ToString();
            //txt_level.Text = Program.alarm_list.contents[index].level.ToString();
        }

        private void gridview_alarmconfig_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null) return;
            if (cell_value_changed == true)
            {
                cell_value_changed = false;

                if (timer_change_check.Enabled == true)
                {
                    if (cell_value_changed == true)
                    {
                        cell_value_changed = false;
                    }
                    else
                    {
                        if (Program.main_md.Message_By_Application("Value is changed, Change Selected Cell?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                        {
                            Alarm_Selected_Change(e.FocusedRowHandle);
                        }
                        else
                        {
                            Alarm_Selected_Change(e.PrevFocusedRowHandle);
                        }
                    }
                }

            }
            else
            {
                Alarm_Selected_Change(e.FocusedRowHandle);
            }
            timer_change_check.Enabled = true;

        }
        public string Load_AlarmList(Boolean initialize)
        {
            string result = "", query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {
                if (initialize == false)
                {
                    query = "SELECT * FROM alarm_list" + "\r\n";
                    //query += "WHERE alarm_visible = '" + 1 + "'" + "\r\n";
                    query += "WHERE alarm_visible is not null" + "\r\n";
                    for (int idx = 0; idx < query_Parameter.token.Count; idx++)
                    {
                        if (idx == 0)
                        {
                            query += "And (alarm_name like '%" + query_Parameter.token[idx] + "%'" + System.Environment.NewLine;
                        }
                        else
                        {
                            query += " OR alarm_name like '%" + query_Parameter.token[idx] + "%'" + System.Environment.NewLine;
                        }
                        if (idx == query_Parameter.token.Count - 1)
                        {
                            query += ")" + System.Environment.NewLine;
                        }

                    }
                    query += "ORDER BY alarm_id" + System.Environment.NewLine;
                }
                else
                {
                    query = "SELECT * FROM alarm_list" + "\r\n";
                    query += "WHERE alarm_visible is not null" + "\r\n";
                    //query += "WHERE alarm_visible = '" + 1 + "'" + "\r\n";
                    query += "ORDER BY alarm_id" + System.Environment.NewLine;
                }

                Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_local_cds.connection, query, dataset, ref err);

                if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {

                    result = "";
                    //data binding update
                    dataset_bind_alarm_config = dataset.Copy();
                    Data_Setting();

                    //프로그램 로딩시 1회만 수행
                    //Sequence등에서 사용하기 위해, 보관하는 Alarm List, Parameter List 동일
                    if (initialize == true)
                    {
                        //초기화
                        Program.alarm_list.total_cnt = dataset.Tables[0].Rows.Count;
                        Program.alarm_list.index_id_mapping.Clear();
                        Array.Resize(ref Program.alarm_list.contents, Program.alarm_list.total_cnt);
                        for (int idx = 0; idx < dataset.Tables[0].Rows.Count; idx++)
                        {
                            //객체 생성
                            Program.alarm_list.contents[idx] = new Config_Alarm();
                            //객체 값 입력
                            Program.alarm_list.contents[idx].id = Convert.ToInt32(dataset.Tables[0].Rows[idx]["alarm_id"]);
                            Program.alarm_list.contents[idx].name = dataset.Tables[0].Rows[idx]["alarm_name"].ToString();
                            Program.alarm_list.contents[idx].comment = dataset.Tables[0].Rows[idx]["alarm_comment"].ToString();
                            Program.alarm_list.contents[idx].enable = Convert.ToByte(dataset.Tables[0].Rows[idx]["alarm_enabled"]);
                            Program.alarm_list.contents[idx].wdt = Convert.ToInt32(dataset.Tables[0].Rows[idx]["alarm_wdt"]);
                            Program.alarm_list.contents[idx].report_to_host = Convert.ToByte(dataset.Tables[0].Rows[idx]["report_alarm_to_host"]);
                            Program.alarm_list.contents[idx].visible = Convert.ToByte(dataset.Tables[0].Rows[idx]["alarm_visible"]);
                            Program.alarm_list.contents[idx].level = Convert.ToByte(dataset.Tables[0].Rows[idx]["alarm_level"]);
                            Program.alarm_list.contents[idx].unit = Convert.ToByte(dataset.Tables[0].Rows[idx]["alarm_unit_id"]);

                            //id <-> index mapping
                            //key = id
                            //value = index
                            Program.alarm_list.index_id_mapping.Add(Program.alarm_list.contents[idx].id, idx);
                        }

                    }

                    result = "";
                }
                else
                {
                    result = err;
                    if (result == "") { result = "Alarm Database Is null"; }
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Load_AlarmList." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return result;
        }
        public string Load_Alarm_Contents(string alarm_id)
        {
            string result = "", query = "", err = "";
            int idx_alarm = -1;
            DataSet dataset = new DataSet();
            try
            {
                Program.alarm_list.index_id_mapping.TryGetValue(Convert.ToInt32(alarm_id), out idx_alarm);
                if (idx_alarm == -1) { result = "Load Alarm_Contents Error"; }
                else
                {
                    query = "SELECT * FROM alarm_list" + "\r\n";
                    query += "WHERE alarm_visible = '" + 1 + "'" + "\r\n";
                    query += "AND alarm_id = '" + alarm_id + "'" + "\r\n";
                    query += "ORDER BY alarm_id" + System.Environment.NewLine;
                    Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_local_cds.connection, query, dataset, ref err);

                    if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count == 1)
                    {
                        //GUI 값 입력
                        txt_id.Text = dataset.Tables[0].Rows[0]["alarm_id"].ToString();
                        txt_name.Text = dataset.Tables[0].Rows[0]["alarm_name"].ToString();
                        txt_wdt.Text = dataset.Tables[0].Rows[0]["alarm_wdt"].ToString();
                        txt_level.Text = dataset.Tables[0].Rows[0]["alarm_level"].ToString();
                        txt_comment.Text = dataset.Tables[0].Rows[0]["alarm_comment"].ToString();

                        if (dataset.Tables[0].Rows[0]["report_alarm_to_host"].ToString() == "1") { btn_hostsend.IsOn = true; }
                        else { btn_hostsend.IsOn = false; }

                        if (dataset.Tables[0].Rows[0]["alarm_enabled"].ToString() == "1") { btn_use.IsOn = true; }
                        else { btn_use.IsOn = false; }
                        //객체 값 입력

                        Program.alarm_list.contents[idx_alarm].id = Convert.ToInt32(dataset.Tables[0].Rows[0]["alarm_id"]);
                        Program.alarm_list.contents[idx_alarm].name = dataset.Tables[0].Rows[0]["alarm_name"].ToString();
                        Program.alarm_list.contents[idx_alarm].comment = dataset.Tables[0].Rows[0]["alarm_comment"].ToString();
                        Program.alarm_list.contents[idx_alarm].enable = Convert.ToByte(dataset.Tables[0].Rows[0]["alarm_enabled"]);
                        Program.alarm_list.contents[idx_alarm].wdt = Convert.ToInt32(dataset.Tables[0].Rows[0]["alarm_wdt"]);
                        Program.alarm_list.contents[idx_alarm].report_to_host = Convert.ToByte(dataset.Tables[0].Rows[0]["report_alarm_to_host"]);
                        Program.alarm_list.contents[idx_alarm].visible = Convert.ToByte(dataset.Tables[0].Rows[0]["alarm_visible"]);
                        Program.alarm_list.contents[idx_alarm].level = Convert.ToByte(dataset.Tables[0].Rows[0]["alarm_level"]);
                        Program.alarm_list.contents[idx_alarm].unit = Convert.ToByte(dataset.Tables[0].Rows[0]["alarm_unit_id"]);
                        result = "";
                    }
                    else
                    {
                        result = err;
                        Program.log_md.LogWrite(this.Name + ".Load_Alarm_Contents." + result, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                    }
                }


            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Load_Alarm_Contents." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }

            return result;
        }
        public string Save_Alarm_Contents()
        {
            string result = "", query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {
                query = "UPDATE alarm_list ";
                query += "SET alarm_id = '" + txt_id.Text + "'";
                query += "   ,alarm_name = '" + txt_name.Text + "'";
                query += "   ,alarm_wdt = '" + txt_wdt.Text + "'";
                query += "   ,alarm_level= '" + txt_level.Text + "'";
                query += "   ,alarm_comment= '" + txt_comment.Text + "'";
                //query += "   ,alarm_unit= " + DBNull.Value +  "";
                if (btn_use.IsOn == true)
                {
                    query += "   ,alarm_enabled= '" + "1" + "'";
                }
                else
                {
                    query += "   ,alarm_enabled= '" + "0" + "'";
                }
                if (btn_hostsend.IsOn == true)
                {
                    query += "   ,report_alarm_to_host= '" + "1" + "'";
                }
                else
                {
                    query += "   ,report_alarm_to_host= '" + "0" + "'";
                }
                query += "WHERE alarm_id = '" + txt_id.Text + "'";
                if (Program.database_md.MariaDB_MainQuery(Program.cg_main.db_local_cds.connection, query, ref err) != 0)
                {
                    cell_value_saved = true;
                    Program.eventlog_form.Insert_Event("Alarm Changed : " + "NAME :" + txt_name.Text + "(" + txt_id.Text + ")" +
                       " wdt : " + txt_wdt.Text + " alarm_level : " + txt_level.Text
                       , (int)frm_eventlog.enum_event_type.ALARM_CHANGED, (int)frm_eventlog.enum_occurred_type.USER, true);
                    Program.main_md.Message_By_Application("Save Success", frm_messagebox.enum_apptype.Only_OK);
                }
                else
                {
                    if (err != "") { Program.main_md.Message_By_Application("Save Fail", frm_messagebox.enum_apptype.Only_OK); }
                }

                timer_change_check.Enabled = false; cell_value_changed = false;

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Save_Alarm_Contents." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return result;
        }
        public void Insert_Alarm_Contents(Config_Alarm alarm_object, Boolean view_occured_list)
        {
            int result = 0;
            string query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {

                if (view_occured_list == true)
                {
                    if (Program.occured_alarm_form.Insert_Alarm(alarm_object) == true)
                    {
                        if (alarm_object.remark == null) { alarm_object.remark = ""; }
                        Program.CTC.Message_Alarm_Occurred_102(alarm_object.id, alarm_object.remark);

                        query = "INSERT INTO alarm_logs(alarm_occurred_time, alarm_occurred_by, alarm_id, alarm_remark)";
                        query += " VALUES (";
                        query += "'" + alarm_object.occurred_time.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                        query += ",'" + alarm_object.occurred_by + "'";
                        query += ",'" + alarm_object.id + "'";
                        query += ",'" + alarm_object.remark + "'";
                        query += ")";

                        result = Program.database_md.MariaDB_MainQuery(Program.cg_main.db_local_cds.connection, query, ref err);
                        if (result == 1)
                        {
                            //동일 데이타 업데이트 시 return 0
                            //Insert 또는 변동 데이터 업데이트 시 return 1

                        }
                        else
                        {
                            Program.log_md.LogWrite(this.Name + ".Insert_Alarm_Contents." + result + " / " + query + " / ", Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                        }
                    }
                    else
                    {
                        //Alarm이 DataTable에 등록되있다.
                    }
                }




            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Insert_Alarm_Contents." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return;
        }
        public void Update_Alarm_Contents(Config_Alarm alarm_object, Boolean view_occured_list)
        {
            int result = 0;
            string query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {
                if (view_occured_list == true)
                {
                    if (Program.occured_alarm_form.Delete_Alarm(alarm_object) == true)
                    {
                        query = "Update alarm_logs";
                        query += " Set alarm_cleared_time = '" + alarm_object.cleared_time.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                        query += "   ,alarm_cleared_by = '" + alarm_object.cleared_by + "'";
                        query += "WHERE alarm_id = '" + alarm_object.id + "'";
                        query += "And alarm_cleared_by is null";
                        result = Program.database_md.MariaDB_MainQuery(Program.cg_main.db_local_cds.connection, query, ref err);
                        if (result == 1)
                        {
                            //동일 데이타 업데이트 시 return 0
                            //Insert 또는 변동 데이터 업데이트 시 return 1
                        }
                        else
                        {
                            //Program.log_md.LogWrite(this.Name + ".Update_Alarm_Contents." + result, Module_Log.enumLog.Error, "");
                        }
                    }
                    else
                    {
                        //Alarm이 DataTable에 없음
                    }
                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Update_Alarm_Contents." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return;
        }

        public void Alarm_cleared_by_APP_Restart(enum_cleared_by cleared_by)
        {
            int result = 0;
            string query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {
                query = "Update alarm_logs";
                query += " Set alarm_cleared_time = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                query += "    ,alarm_cleared_by = '" + (int)cleared_by + "'"; //0:reset, 1:pc reboot, 2:null
                query += "Where alarm_cleared_time is null";
                result = Program.database_md.MariaDB_MainQuery(Program.cg_main.db_local_cds.connection, query, ref err);
                if (result == 1)
                {
                    //동일 데이타 업데이트 시 return 0
                    //Insert 또는 변동 데이터 업데이트 시 return 1
                }
                else
                {
                    //Program.log_md.LogWrite(this.Name + ".Alarm_cleared_by_APP_Restart." + result, Module_Log.enumLog.Error, "");
                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Update_Alarm_Contents." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return;
        }

        private void timer_change_check_Tick(object sender, EventArgs e)
        {
            Color changed_color = Color.Lime;
            Color normal_color = Color.White;
            int cnt_value_changed = 0;
            int focused_rowhandle = gridview_alarmconfig.FocusedRowHandle;
            if (focused_rowhandle < 0)
            {
                return;
            }
            else
            {
                if (txt_id.Text != gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_id"]).ToString())
                {
                    cell_value_changed = false;
                    return;
                }

                if (txt_name.Text != gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_name"]).ToString())
                {
                    txt_name.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    txt_name.BackColor = normal_color;
                }
                if (txt_wdt.Text != gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_wdt"]).ToString())
                {
                    txt_wdt.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    txt_wdt.BackColor = normal_color;
                }
                if (txt_level.Text != gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_level"]).ToString())
                {
                    txt_level.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    txt_level.BackColor = normal_color;
                }
                if (txt_comment.Text != gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_comment"]).ToString())
                {
                    txt_comment.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    txt_comment.BackColor = normal_color;
                }
                if (Convert.ToInt32(btn_use.IsOn) != Convert.ToInt32(gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["alarm_enabled"])))
                {
                    btn_use.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    btn_use.BackColor = normal_color;
                }
                if (Convert.ToInt32(btn_hostsend.IsOn) != Convert.ToInt32(gridview_alarmconfig.GetRowCellValue(focused_rowhandle, gridview_alarmconfig.Columns["report_alarm_to_host"])))
                {
                    btn_hostsend.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    btn_hostsend.BackColor = normal_color;
                }
                if (cnt_value_changed >= 1)
                {
                    cell_value_changed = true;
                }
                else
                {
                    cell_value_changed = false;
                }
            }

        }

        private void gridview_alarmconfig_CustomRowFilter(object sender, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e)
        {
            ColumnView view = sender as ColumnView;
            string view_visible = view.GetListSourceRowCellValue(e.ListSourceRow, "alarm_visible").ToString();
            string view_name = view.GetListSourceRowCellValue(e.ListSourceRow, "alarm_name").ToString();
            try
            {
                if (view_visible == "0")
                {
                    e.Visible = false; e.Handled = true;
                }
                else
                {
                    e.Visible = true; e.Handled = true;
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}