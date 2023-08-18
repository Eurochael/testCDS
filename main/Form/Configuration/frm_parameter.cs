using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace cds
{
    public partial class frm_parameter : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public Query_Parameter query_Parameter = new Query_Parameter();

        public RepositoryItemTextEdit Grid_Txt_Readonly = new RepositoryItemTextEdit();
        public RepositoryItemTextEdit Grid_Txt = new RepositoryItemTextEdit();
        public RepositoryItemComboBox Grid_Combo_type = new RepositoryItemComboBox();

        public DataSet dataset_bind_parameter_config = new DataSet();

        private bool cell_value_changed = false;
        private bool cell_value_saved = false;
        public frm_parameter()
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
                        timer_change_check.Interval = 200;timer_change_check.Enabled = true;
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
        public enum enum_parmater
        {
            Disk_Storage_Log_Min = 52,
            Disk_Storage_Program_Min = 53,
            Chem1_Abnormal_Flow_Min = 101,
            Chem1_Filter_IN_Pressure_High = 102,
            Chem1_Filter_IN_Pressure_Low = 103,
            Chem1_Filter_OUT_Pressure_High = 104,
            Chem1_Filter_OUT_Pressure_Low = 105,
            Chem1_Flowrate_High = 106,
            Chem1_Flowrate_Low = 107,
            Chem1_Mixing_Rate = 108,
            Chem1_Pressure_High = 109,
            Chem1_Pressure_Low = 110,
            Chem1_Supply_Time_Max = 111,
            Chem1_Supply_Time_Min = 112,
            Chem1_Concentration_High = 113,
            Chem1_Concentration_Low = 114,
            Chem2_Abnormal_Flow_Min = 141,
            Chem2_Filter_IN_Pressure_High = 142,
            Chem2_Filter_IN_Pressure_Low = 143,
            Chem2_Filter_OUT_Pressure_High = 144,
            Chem2_Filter_OUT_Pressure_Low = 145,
            Chem2_Flowrate_High = 146,
            Chem2_Flowrate_Low = 147,
            Chem2_Mixing_Rate = 148,
            Chem2_Pressure_High = 149,
            Chem2_Pressure_Low = 150,
            Chem2_Supply_Time_Max = 151,
            Chem2_Supply_Time_Min = 152,
            Chem2_Concentration_High = 153,
            Chem2_Concentration_Low = 154,
            Chem3_Abnormal_Flow_Min = 181,
            Chem3_Filter_IN_Pressure_High = 182,
            Chem3_Filter_IN_Pressure_Low = 183,
            Chem3_Filter_OUT_Pressure_High = 184,
            Chem3_Filter_OUT_Pressure_Low = 185,
            Chem3_Flowrate_High = 186,
            Chem3_Flowrate_Low = 187,
            Chem3_Mixing_Rate = 188,
            Chem3_Pressure_High = 189,
            Chem3_Pressure_Low = 190,
            Chem3_Supply_Time_Max = 191,
            Chem3_Supply_Time_Min = 192,
            Chem3_Concentration_High = 193,
            Chem3_Concentration_Low = 194,
            Chem4_Abnormal_Flow_Min = 221,
            Chem4_Filter_IN_Pressure_High = 222,
            Chem4_Filter_IN_Pressure_Low = 223,
            Chem4_Filter_OUT_Pressure_High = 224,
            Chem4_Filter_OUT_Pressure_Low = 225,
            Chem4_Flowrate_High = 226,
            Chem4_Flowrate_Low = 227,
            Chem4_Mixing_Rate = 228,
            Chem4_Pressure_High = 229,
            Chem4_Pressure_Low = 230,
            Chem4_Supply_Time_Max = 231,
            Chem4_Supply_Time_Min = 232,
            Chem4_Concentration_High = 233,
            Chem4_Concentration_Low = 234,
            CDA_Pressure_High = 261,
            CDA_Pressure_Low = 262,
            CDA_Sol_Pressure_High = 263,
            CDA_Sol_Pressure_Low = 264,
            Exhaust_Pressure_High = 265,
            Exhaust_Pressure_Low = 266,
            N2_Pressure_High = 267,
            N2_Pressure_Low = 268,
            PCW_Pressure_High = 275,
            PCW_Pressure_Low = 276,
            PCW_Circulation_FLow_High = 277,
            PCW_Circulation_FLow_Low = 278,
            PCW_Supply_A_FLow_High = 279,
            PCW_Supply_A_FLow_Low = 280,
            PCW_Supply_B_FLow_High = 281,
            PCW_Supply_B_FLow_Low = 282,
            HDIW_Supply_Min_Temp = 290,
            HDIW_Supply_Max_Temp = 291,
            HDIW_Temp_Delay_Time = 292,
            HDIW_Flowrate_High = 293,
            HDIW_Flowrate_Low = 294,
            Tank_A_Level_LL = 301,
            Tank_A_Level_L = 302,
            Tank_A_Level_M = 303,
            Tank_A_Level_H = 304,
            Tank_A_Level_HH = 305,
            Circulation_Tank_A_Temp_Set = 306,
            Circulation_Tank_A_Temp_High = 307,
            Circulation_Tank_A_Temp_Low = 308,
            Circulation_Tank_A_Heating_Time_Out = 309,
            Circulation_Tank_A_Drain_Time_Out = 310,
            Circulation_Tank_A_Ready_Time_Out = 311,
            Supply_Tank_A_Temp_Set = 312,
            Supply_Tank_A_Temp_High = 313,
            Supply_Tank_A_Temp_Low = 314,
            Supply_Tank_A_Use_Time_Out = 318,
            Tank_A_N2_Flowrate_High = 320,
            Tank_A_N2_Flowrate_Low = 321,
            Tank_A_Empty_Sensor_Monitoring_Value = 322,
            Tank_B_Level_LL = 401,
            Tank_B_Level_L = 402,
            Tank_B_Level_M = 403,
            Tank_B_Level_H = 404,
            Tank_B_Level_HH = 405,
            Circulation_Tank_B_Temp_Set = 406,
            Circulation_Tank_B_Temp_High = 407,
            Circulation_Tank_B_Temp_Low = 408,
            Circulation_Tank_B_Heating_Time_Out = 409,
            Circulation_Tank_B_Drain_Time_Out = 410,
            Circulation_Tank_B_Ready_Time_Out = 411,
            Supply_Tank_B_Temp_Set = 412,
            Supply_Tank_B_Temp_High = 413,
            Supply_Tank_B_Temp_Low = 414,
            Supply_Tank_B_Use_Time_Out = 418,
            Tank_B_N2_Flowrate_High = 420,
            Tank_B_N2_Flowrate_Low = 421,
            Tank_B_Empty_Sensor_Monitoring_Value = 422,
            Level_Sensor_Change_Time_Delay = 501,
            Empty_Level_Sensor_Change_Time_Delay = 502,
            Charge_Time_Delay = 503,
            CM_Check_Drain_Time = 504,
            CM_Check_Time_Delay = 505,
            Chemical_Change_Min_Level = 506,
            SPARE506 = 507,
            Refill_Start_Level = 508,
            Refill_End_Level = 509,
            CC_Wafer_Count_High = 510,
            CC_Life_Time_High = 511,
            Tank_Flush_Charge_Level = 530,
            Tank_DIW_Flush_Count = 531,
            Tank_DIW_Flush_Supply_Time = 532,
            Tank_Chemical_Flush_Count = 533,
            Tank_Chemical_Flush_Supply_Time = 534,
            Tank_Auto_Flush_Count = 535,
            Tank_Auto_Flush_Supply_Time = 536,
            Pressure_High_Tank_Circulation = 711,
            Pressure_Low_Tank_Circulation = 712,
            Pump_Pressure_High_Tank_Circulation = 713,
            Pump_Pressure_Low_Tank_Circulation = 714,
            Pump_CDA_Pressure_High_Tank_Circulation = 715,
            Pump_CDA_Pressure_Low_Tank_Circulation = 716,
            Flowrate_High_Tank_Circulation = 721,
            Flowrate_Low_Tank_Circulation = 722,
            SPARE723 = 723,
            Pump_Sensor_Wait_Delay_Tank_Circulation = 731,
            Pump_Sensor_Wait_Count = 732,
            Temp_High_Tank_Circulation = 741,
            Temp_Low_Tank_Circulation = 742,
            Heater_On_Dleay_Time_Tank_Circulation = 743,
            Drain_Valve_Off_Time_Delay_Tank_Circulation = 760,
            Reclaim_Flush = 761,
            Tank_Circulation_Level = 762,
            Circulation_Temp_OK_Time_Delay = 763,
            Supply_Temp_OK_Time_Delay = 764,
            Circulation_Pump_Run_Interval = 765,
            Pressure_High_Supply_A_IN = 911,
            Pressure_Low_Supply_A_IN = 912,
            Pressure_High_Supply_A_OUT = 913,
            Pressure_Low_Supply_A_OUT = 914,
            Pressure_High_Supply_B_IN = 915,
            Pressure_Low_Supply_B_IN = 916,
            Pressure_High_Supply_B_OUT = 917,
            Pressure_Low_Supply_B_OUT = 918,
            Flowrate_High_Supply_A = 923,
            Flowrate_Low_Supply_A = 924,
            Flowrate_High_Supply_B = 925,
            Flowrate_Low_Supply_B = 926,
            Pump_Pressure_High_Supply_A = 931,
            Pump_Pressure_Low_Supply_A = 932,
            Pump_Pressure_High_Supply_B = 933,
            Pump_Pressure_Low_Supply_B = 934,
            Temp_High_Supply_A = 941,
            Temp_Low_Supply_A = 942,
            Temp_High_Supply_B = 943,
            Temp_Low_Supply_B = 944,
            Heater_On_Dleay_Time = 945,
            Pressure_High_MAIN_CDA1_PRESS_PUMP = 965,
            Pressure_Low_MAIN_CDA1_PRESS_PUMP = 966,
            Pressure_High_MAIN_CDA2_PRESS_SOL = 967,
            Pressure_Low_MAIN_CDA2_PRESS_SOL = 968,
            Pressure_High_MAIN_CDA3_PRESS_DRAIN = 969,
            Pressure_Low_MAIN_CDA3_PRESS_DRAIN = 970,
            Keep_Supply_Time_Chemical_Changed = 971,
            Pressure_High_Retrun_A = 1111,
            Pressure_Low_Retrun_A = 1112,
            Pressure_High_Retrun_B = 1113,
            Pressure_Low_Retrun_B = 1114,
            Pump_CDA_Pressure_High_Supply_A = 1311,
            Pump_CDA_Pressure_Low_Supply_A = 1312,
            Pump_CDA_Pressure_High_Supply_B = 1313,
            Pump_CDA_Pressure_Low_Supply_B = 1314,
            Drain_Flowrate_High = 1500,
            Drain_Flowrate_Low = 1501,
            Pressure_High_Heater_N2 = 1605,
            Pressure_Low_Heater_N2 = 1606,
            Sampling_Port_Use_Delay_Time = 1800,
            Drain_Pump_Off_Time_Delay = 1801,
            Chemical_Drain_Start_Level = 1802,
            Chemical_Change_Confirm_Time_Out = 1803,
            Concentration_Measure_Interval = 1804,
            Concentration_Check_Need_Drain_Time_Delay = 1805,
            CM_Drain_Pump_Off_Time_Delay = 1806,
        }

        private void frm_parameter_Load(object sender, EventArgs e)
        {

        }
        private void frm_parameter_FormClosed(object sender, FormClosedEventArgs e)
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
                    Load_ParameterList(false);
                }
                else if (btn_event.Name == "btn_cancel")
                {
                    Load_Parameter_Contents(txt_id.Text);
                }
                else if (btn_event.Name == "btn_Export")
                {
                    FolderBrowserDialog export_dir = new FolderBrowserDialog();
                    if (export_dir.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            export_csv_full_name = export_dir.SelectedPath + @"\" + "CDS_" + Program.cg_app_info.eq_type.ToString() + "_Parameter_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                            gridview_parameterconfig.ExportToCsv(export_csv_full_name);
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
                        if (Convert.ToDouble(txt_set.Text) > Convert.ToDouble(txt_max.Text))
                        {
                            Program.main_md.Message_By_Application("Value Error : Set >= Max", frm_messagebox.enum_apptype.Only_OK);
                            txt_set.Text = txt_max.Text;
                            return;
                        }
                        else if (Convert.ToDouble(txt_set.Text) < Convert.ToDouble(txt_min.Text))
                        {
                            Program.main_md.Message_By_Application("Value Error : Set <= Min", frm_messagebox.enum_apptype.Only_OK);
                            txt_set.Text = txt_min.Text;
                            return;
                        }

                        focused_rowhandle_old = gridview_parameterconfig.FocusedRowHandle;
                        Save_Parameter_Contents();

                        gridview_parameterconfig.FocusedRowHandle = focused_rowhandle_old;

                        //Prameter 변경 후 Volume 등 다시 계산한다.
                        Program.mixing_step_form.Setting_Mixing_Order(tank_class.enum_tank_type.TANK_ALL);
                    }

                }
                else if (btn_event.Name == "simpleButton3")
                {

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
                grid_parameter_config.SuspendLayout();
                grid_parameter_config.DataSource = null;
                dataset_bind_parameter_config.Tables.Clear();

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
                grid_parameter_config.ResumeLayout();
            }
            return result;
        }
        public void Data_Setting()
        {
            int search_result = 0;
            try
            {
                grid_parameter_config.SuspendLayout();
                grid_parameter_config.DataSource = dataset_bind_parameter_config.Tables[0];
                for (int idx = 0; idx < dataset_bind_parameter_config.Tables[0].Rows.Count; idx++)
                {
                    if (dataset_bind_parameter_config.Tables[0].Rows[idx]["cds_parameter_visible"].ToString() == "1")
                    {
                        search_result = search_result + 1;
                    }
                }
                gp_parameter_config.Text = "Parameter List search result = " + search_result + "";

                if (dataset_bind_parameter_config.Tables[0].Rows.Count > 0)
                {
                    gridview_parameterconfig.FocusedRowHandle = 0;
                }
                Grid_Setting();
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Data_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                grid_parameter_config.ResumeLayout();
            }
        }
        public void Grid_Setting()
        {
            try
            {
                gridview_parameterconfig.OptionsView.ShowGroupPanel = false;
                gridview_parameterconfig.OptionsMenu.EnableGroupPanelMenu = false;
                gridview_parameterconfig.OptionsMenu.EnableColumnMenu = false;
                gridview_parameterconfig.OptionsCustomization.AllowSort = false;
                gridview_parameterconfig.OptionsCustomization.AllowFilter = false;
                gridview_parameterconfig.OptionsCustomization.AllowColumnMoving = false;
                gridview_parameterconfig.OptionsCustomization.AllowColumnResizing = false;
                gridview_parameterconfig.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
                gridview_parameterconfig.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                gridview_parameterconfig.OptionsView.ColumnAutoWidth = false;
                gridview_parameterconfig.GroupPanelText = "";
                for (int i = 0; i <= gridview_parameterconfig.Columns.Count - 1; i++)
                {
                    //공통 적용 
                    gridview_parameterconfig.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridview_parameterconfig.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gridview_parameterconfig.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridview_parameterconfig.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    if (gridview_parameterconfig.Columns[i].Name == "col" + "cds_parameter_id")
                    {
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_parameterconfig.Columns[i].Visible = true;
                        gridview_parameterconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_parameterconfig.Columns[i].Width = 60;
                        gridview_parameterconfig.Columns[i].Caption = "ID";
                        gridview_parameterconfig.Columns[i].VisibleIndex = 0;
                    }
                    else if (gridview_parameterconfig.Columns[i].Name == "col" + "cds_parameter_name")
                    {
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_parameterconfig.Columns[i].Visible = true;
                        gridview_parameterconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_parameterconfig.Columns[i].Width = 300;
                        gridview_parameterconfig.Columns[i].Caption = "Name";
                        gridview_parameterconfig.Columns[i].VisibleIndex = 1;
                    }
                    else if (gridview_parameterconfig.Columns[i].Name == "col" + "cds_parameter_comment")
                    {
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_parameterconfig.Columns[i].Visible = true;
                        gridview_parameterconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_parameterconfig.Columns[i].Width = 500;
                        gridview_parameterconfig.Columns[i].Caption = "Comment";
                        gridview_parameterconfig.Columns[i].VisibleIndex = 7;
                    }
                    else if (gridview_parameterconfig.Columns[i].Name == "col" + "cds_parameter_minimum")
                    {
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_parameterconfig.Columns[i].Visible = true;
                        gridview_parameterconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_parameterconfig.Columns[i].Width = 100;
                        gridview_parameterconfig.Columns[i].Caption = "Min";
                        gridview_parameterconfig.Columns[i].VisibleIndex = 3;
                    }
                    else if (gridview_parameterconfig.Columns[i].Name == "col" + "cds_parameter_maximum")
                    {
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_parameterconfig.Columns[i].Visible = true;
                        gridview_parameterconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_parameterconfig.Columns[i].Width = 100;
                        gridview_parameterconfig.Columns[i].Caption = "Max";
                        gridview_parameterconfig.Columns[i].VisibleIndex = 4;
                    }
                    else if (gridview_parameterconfig.Columns[i].Name == "col" + "cds_parameter_value")
                    {
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_parameterconfig.Columns[i].Visible = true;
                        gridview_parameterconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_parameterconfig.Columns[i].Width = 100;
                        gridview_parameterconfig.Columns[i].Caption = "Value";
                        gridview_parameterconfig.Columns[i].VisibleIndex = 2;
                    }
                    else if (gridview_parameterconfig.Columns[i].Name == "col" + "cds_parameter_unit")
                    {
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_parameterconfig.Columns[i].Visible = true;
                        gridview_parameterconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_parameterconfig.Columns[i].Width = 120;
                        gridview_parameterconfig.Columns[i].Caption = "Unit";
                        gridview_parameterconfig.Columns[i].VisibleIndex = 5;
                    }
                    else if (gridview_parameterconfig.Columns[i].Name == "col" + "report_cds_parameter_to_host")
                    {
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_parameterconfig.Columns[i].Visible = true;
                        gridview_parameterconfig.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_parameterconfig.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_parameterconfig.Columns[i].Width = 120;
                        gridview_parameterconfig.Columns[i].Caption = "Report Host";
                        gridview_parameterconfig.Columns[i].VisibleIndex = 6;
                    }
                    else if (gridview_parameterconfig.Columns[i].Name == "col" + "cds_parameter_visible")
                    {
                        gridview_parameterconfig.Columns[i].Visible = false;
                    }
                    else
                    {
                        gridview_parameterconfig.Columns[i].Visible = false;
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
        public void Parameter_Selected_Change(int focused_rowhandle)
        {
            if (focused_rowhandle < 0) { return; }
            txt_id.Text = gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_id"]).ToString();
            txt_name.Text = gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_name"]).ToString();
            txt_min.Text = gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_minimum"]).ToString();
            txt_set.Text = gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_value"]).ToString();
            txt_max.Text = gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_maximum"]).ToString();
            txt_comment.Text = gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_comment"]).ToString();



            if (gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["report_cds_parameter_to_host"]).ToString() == "1") { btn_hostsend.IsOn = true; }
            else { btn_hostsend.IsOn = false; }

            //mask 값 변경
            if (txt_name.Text.ToUpper().IndexOf("PRESS") >= 0 ||
                 txt_name.Text.ToUpper().IndexOf("FLOW") >= 0)
            {
                txt_set.Properties.MaskSettings.MaskExpression = "f1";
                txt_min.Properties.MaskSettings.MaskExpression = "f1";
                txt_max.Properties.MaskSettings.MaskExpression = "f1";
            }
            else
            {
                txt_set.Properties.MaskSettings.MaskExpression = "f0";
                txt_min.Properties.MaskSettings.MaskExpression = "f0";
                txt_max.Properties.MaskSettings.MaskExpression = "f0";
            }


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
                    if(cell_value_saved == true)
                    {
                        cell_value_saved = false;
                    }
                    else
                    {
                        if (Program.main_md.Message_By_Application("Value is changed, Change Selected Cell?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                        {
                            Parameter_Selected_Change(e.FocusedRowHandle);
                        }
                        else
                        {
                            Parameter_Selected_Change(e.PrevFocusedRowHandle);
                        }
                    }

                }
               
            }
            else
            {
                Parameter_Selected_Change(e.FocusedRowHandle);
            }
            timer_change_check.Enabled = true;
        }
        public string Load_ParameterList(Boolean initialize)
        {
            string result = "", query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {
                if (initialize == false)
                {
                    query = "SELECT * FROM Parameters" + "\r\n";
                    query += "WHERE cds_parameter_visible is not null" + "\r\n";
                    //query += "WHERE cds_parameter_visible = '" + 1 + "'" + "\r\n";
                    for (int idx = 0; idx < query_Parameter.token.Count; idx++)
                    {
                        if (idx == 0)
                        {
                            query += "And (cds_parameter_name like '%" + query_Parameter.token[idx] + "%'" + System.Environment.NewLine;
                        }
                        else
                        {
                            query += " OR cds_parameter_name like '%" + query_Parameter.token[idx] + "%'" + System.Environment.NewLine;
                        }
                        if (idx == query_Parameter.token.Count - 1)
                        {
                            query += ")" + System.Environment.NewLine;
                        }

                    }
                    query += "ORDER BY cds_parameter_id" + System.Environment.NewLine;
                }
                else
                {
                    query = "SELECT * FROM Parameters" + "\r\n";
                    query += "WHERE cds_parameter_visible is not null" + "\r\n";
                    //query += "WHERE cds_parameter_visible = '" + 1 + "'" + "\r\n";
                    query += "ORDER BY cds_parameter_id" + System.Environment.NewLine;
                }

                Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_local_cds.connection, query, dataset, ref err);

                if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {

                    result = "";
                    //data binding update
                    dataset_bind_parameter_config = dataset.Copy();
                    Data_Setting();

                    //프로그램 로딩시 1회만 수행
                    //Sequence등에서 사용하기 위해, 보관하는 Alarm List, Parameter List 동일
                    if (initialize == true)
                    {
                        //초기화
                        Program.parameter_list.total_cnt = dataset.Tables[0].Rows.Count;
                        Program.parameter_list.index_id_mapping.Clear();
                        Array.Resize(ref Program.parameter_list.contents, Program.parameter_list.total_cnt);
                        for (int idx = 0; idx < dataset.Tables[0].Rows.Count; idx++)
                        {
                            //객체 생성
                            Program.parameter_list.contents[idx] = new Config_Parameter();
                            //객체 값 입력
                            Program.parameter_list.contents[idx].id = Convert.ToInt32(dataset.Tables[0].Rows[idx]["cds_parameter_id"]);
                            Program.parameter_list.contents[idx].name = dataset.Tables[0].Rows[idx]["cds_parameter_name"].ToString();
                            Program.parameter_list.contents[idx].comment = dataset.Tables[0].Rows[idx]["cds_parameter_comment"].ToString();
                            Program.parameter_list.contents[idx].value_min = Convert.ToInt32(dataset.Tables[0].Rows[idx]["cds_parameter_minimum"]);
                            Program.parameter_list.contents[idx].value = Convert.ToInt32(dataset.Tables[0].Rows[idx]["cds_parameter_value"]);
                            Program.parameter_list.contents[idx].value_max = Convert.ToInt32(dataset.Tables[0].Rows[idx]["cds_parameter_maximum"]);
                            Program.parameter_list.contents[idx].report_to_host = Convert.ToByte(dataset.Tables[0].Rows[idx]["report_cds_parameter_to_host"]);
                            Program.parameter_list.contents[idx].visible = Convert.ToByte(dataset.Tables[0].Rows[idx]["cds_parameter_visible"]);
                            Program.parameter_list.contents[idx].unit = dataset.Tables[0].Rows[idx]["cds_parameter_unit"].ToString();
                            //id <-> index mapping
                            //key = id
                            //value = index
                            Program.parameter_list.index_id_mapping.Add(Program.parameter_list.contents[idx].id, idx);
                        }

                    }


                    result = "";
                }
                else
                {
                    result = err;
                    if (result == "") { result = "Parameter Database Is null"; }
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Load_ParameterList." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return result;
        }
        public string Load_Parameter_Contents(string parameter_id)
        {
            string result = "", query = "", err = "";
            int idx_parameter = -1;
            DataSet dataset = new DataSet();
            try
            {
                Program.parameter_list.index_id_mapping.TryGetValue(Convert.ToInt32(parameter_id), out idx_parameter);
                if (idx_parameter == -1) { result = "Load Load_Parameter_Contents Error"; }
                else
                {
                    query = "SELECT * FROM parameters" + "\r\n";
                    query += "WHERE cds_parameter_visible = '" + 1 + "'" + "\r\n";
                    query += "AND cds_parameter_id = '" + parameter_id + "'" + "\r\n";
                    query += "ORDER BY cds_parameter_id" + System.Environment.NewLine;
                    Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_local_cds.connection, query, dataset, ref err);

                    if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count == 1)
                    {
                    

                        //GUI 값 입력
                        txt_id.Text = dataset.Tables[0].Rows[0]["cds_parameter_id"].ToString();
                        txt_name.Text = dataset.Tables[0].Rows[0]["cds_parameter_name"].ToString();
                        txt_min.Text = dataset.Tables[0].Rows[0]["cds_parameter_minimum"].ToString();
                        txt_set.Text = dataset.Tables[0].Rows[0]["cds_parameter_value"].ToString();
                        txt_max.Text = dataset.Tables[0].Rows[0]["cds_parameter_maximum"].ToString();
                        txt_comment.Text = dataset.Tables[0].Rows[0]["cds_parameter_comment"].ToString();

                        if (dataset.Tables[0].Rows[0]["report_cds_parameter_to_host"].ToString() == "1") { btn_hostsend.IsOn = true; }
                        else { btn_hostsend.IsOn = false; }

                        //객체 값 입력

                        Program.parameter_list.contents[idx_parameter].id = Convert.ToInt32(dataset.Tables[0].Rows[0]["cds_parameter_id"]);
                        Program.parameter_list.contents[idx_parameter].name = dataset.Tables[0].Rows[0]["cds_parameter_name"].ToString();
                        Program.parameter_list.contents[idx_parameter].comment = dataset.Tables[0].Rows[0]["cds_parameter_comment"].ToString();
                        Program.parameter_list.contents[idx_parameter].value_min = Convert.ToInt32(dataset.Tables[0].Rows[0]["cds_parameter_minimum"]);
                        Program.parameter_list.contents[idx_parameter].value = Convert.ToSingle(dataset.Tables[0].Rows[0]["cds_parameter_value"]);
                        Program.parameter_list.contents[idx_parameter].value_max = Convert.ToInt32(dataset.Tables[0].Rows[0]["cds_parameter_maximum"]);
                        Program.parameter_list.contents[idx_parameter].report_to_host = Convert.ToByte(dataset.Tables[0].Rows[0]["report_cds_parameter_to_host"]);
                        Program.parameter_list.contents[idx_parameter].visible = Convert.ToByte(dataset.Tables[0].Rows[0]["cds_parameter_visible"]);
                        Program.parameter_list.contents[idx_parameter].unit = dataset.Tables[0].Rows[0]["cds_parameter_unit"].ToString();
                        result = "";
                    }
                    else
                    {
                        result = err;
                        Program.log_md.LogWrite(this.Name + ".Load_Parameter_Contents." + result, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                    }
                }


            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Load_Parameter_Contents." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }

            return result;
        }
        public string Save_Parameter_Contents()
        {
            string result = "", query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {
                query = "UPDATE parameters ";
                query += "SET cds_parameter_id = '" + txt_id.Text + "'";
                query += "   ,cds_parameter_name = '" + txt_name.Text + "'";
                query += "   ,cds_parameter_minimum = '" + txt_min.Text + "'";
                query += "   ,cds_parameter_value = '" + txt_set.Text + "'";
                query += "   ,cds_parameter_maximum = '" + txt_max.Text + "'";
                query += "   ,cds_parameter_comment= '" + txt_comment.Text + "'";
                //query += "   ,alarm_unit= " + DBNull.Value +  "";
                if (btn_hostsend.IsOn == true)
                {
                    query += "   ,report_cds_parameter_to_host= '" + "1" + "'";
                }
                else
                {
                    query += "   ,report_cds_parameter_to_host= '" + "0" + "'";
                }
                query += "WHERE cds_parameter_id = '" + txt_id.Text + "'";
                if (Program.database_md.MariaDB_MainQuery(Program.cg_main.db_local_cds.connection, query, ref err) != 0)
                {
                    Program.eventlog_form.Insert_Event("Parameter Changed : " + "NAME :" + txt_name.Text + "(" + txt_id.Text + ")" +
                        " Min : " + txt_min.Text + " Value : " + txt_set.Text + " Max : " + txt_max.Text
                        , (int)frm_eventlog.enum_event_type.PARAMETER_CHANGED, (int)frm_eventlog.enum_occurred_type.USER, true);
                    cell_value_saved = true;
                    Load_Parameter_Contents(txt_id.Text);
                    Load_ParameterList(false);

                    Program.CTC.Message_Database_Changed_Notice_109(Program.parameter_list.Return_Object_by_ID(Convert.ToInt32(txt_id.Text)));
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
                Program.log_md.LogWrite(this.Name + ".Save_Parameter_Contents." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return result;
        }
        private void timer_change_check_Tick(object sender, EventArgs e)
        {
            Color changed_color = Color.Lime;
            Color normal_color = Color.White;
            int cnt_value_changed = 0;
            int focused_rowhandle = gridview_parameterconfig.FocusedRowHandle;
            if (focused_rowhandle < 0)
            {
                return;
            }
            else
            {
                if (txt_id.Text != gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_id"]).ToString())
                {
                    cell_value_changed = false;
                    return;
                }

                if (txt_name.Text != gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_name"]).ToString())
                {
                    txt_name.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    txt_name.BackColor = normal_color;
                }
                if (txt_min.Text != gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_minimum"]).ToString())
                {
                    txt_min.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    txt_min.BackColor = normal_color;
                }
                if (Math.Abs(Convert.ToSingle(txt_set.Text)) != Math.Abs(Convert.ToSingle(gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_value"]))))
                {
                    txt_set.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    txt_set.BackColor = normal_color;
                }
                if (Math.Abs(Convert.ToSingle(txt_max.Text)) != Math.Abs(Convert.ToSingle(gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_maximum"]).ToString())))
                {
                    txt_max.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    txt_max.BackColor = normal_color;
                }
                if (txt_comment.Text != gridview_parameterconfig.GetRowCellValue(focused_rowhandle, gridview_parameterconfig.Columns["cds_parameter_comment"]).ToString())
                {
                    txt_comment.BackColor = changed_color; cnt_value_changed += 1;
                }
                else
                {
                    txt_comment.BackColor = normal_color;
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
        private void gridview_parameterconfig_CustomRowFilter(object sender, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e)
        {
            ColumnView view = sender as ColumnView;
            string view_name = view.GetListSourceRowCellValue(e.ListSourceRow, "cds_parameter_visible").ToString();
            try
            {
                if (view_name == "0")
                {
                    e.Visible = false; e.Handled = true;
                }
                else
                {
                    e.Visible = true; e.Handled = true;
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}