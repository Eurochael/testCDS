using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cds
{
    public partial class frm_io_monitor : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _initialized = false;
        private Boolean _actived = false;

        private DataTable dt_di = new DataTable("DI");
        private DataTable dt_do = new DataTable("DO");
        private DataTable dt_ai = new DataTable("AI");
        private DataTable dt_ao = new DataTable("AO");

        //Grid에서 사용할 Reposit Item
        public RepositoryItemTextEdit Grid_Txt_Readonly = new RepositoryItemTextEdit();
        public RepositoryItemTextEdit Grid_Txt = new RepositoryItemTextEdit();
        public RepositoryItemComboBox Grid_Combo_type_digital = new RepositoryItemComboBox();
        public RepositoryItemComboBox Grid_Combo_type_analog = new RepositoryItemComboBox();
        public RepositoryItemToggleSwitch Grid_toggle = new RepositoryItemToggleSwitch();
        public RepositoryItemButtonEdit Grid_Set_Item = new RepositoryItemButtonEdit();
        public frm_io_monitor()
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
                        timer_ui_change.Interval = 500; timer_ui_change.Enabled = true;
                        //if (_initialized == false) { _initialized = true; Setting_Initial(); }
                    }
                    else
                    {
                        timer_ui_change.Enabled = false;
                    }
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + ".actived." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
            }
        }
        public string Set_Grid_Begin(Boolean act_di, Boolean act_do, Boolean act_ai, Boolean act_ao, Boolean act_serial)
        {
            string result = "";
            try
            {
                //프로그램 로드 시 1회만 수행
                //DI
                dt_di.Columns.Clear(); dt_di.Rows.Clear();
                dt_di.Columns.Add("Address", Type.GetType("System.Int32"));
                dt_di.Columns.Add("Description", Type.GetType("System.String"));
                dt_di.Columns.Add("Value", Type.GetType("System.Boolean"));
                dt_di.Columns.Add("type", Type.GetType("System.String"));
                dt_di.Columns.Add("Idx", Type.GetType("System.Int32"));
                dt_di.Columns.Add("Use", Type.GetType("System.Boolean"));

                //DO
                dt_do.Columns.Clear(); dt_do.Rows.Clear();
                dt_do.Columns.Add("Address", Type.GetType("System.Int32"));
                dt_do.Columns.Add("Description", Type.GetType("System.String"));
                dt_do.Columns.Add("Set", Type.GetType("System.String"));
                dt_do.Columns.Add("Value", Type.GetType("System.Boolean"));
                dt_do.Columns.Add("type", Type.GetType("System.String"));
                dt_do.Columns.Add("Idx", Type.GetType("System.Int32"));
                dt_do.Columns.Add("Use", Type.GetType("System.Boolean"));

                //AI
                dt_ai.Columns.Clear(); dt_ai.Rows.Clear();
                dt_ai.Columns.Add("Address", Type.GetType("System.Int32"));
                dt_ai.Columns.Add("Description", Type.GetType("System.String"));
                dt_ai.Columns.Add("Value", Type.GetType("System.Decimal"));
                dt_ai.Columns.Add("Min", Type.GetType("System.Int32"));
                dt_ai.Columns.Add("Max", Type.GetType("System.Int32"));
                dt_ai.Columns.Add("Type", Type.GetType("System.String"));
                dt_ai.Columns.Add("Idx", Type.GetType("System.Int32"));
                dt_ai.Columns.Add("Use", Type.GetType("System.Boolean"));

                //AO
                dt_ao.Columns.Clear(); dt_ao.Rows.Clear();
                dt_ao.Columns.Add("Address", Type.GetType("System.Int32"));
                dt_ao.Columns.Add("Description", Type.GetType("System.String"));
                dt_ao.Columns.Add("Value", Type.GetType("System.Decimal"));
                dt_ao.Columns.Add("Set", Type.GetType("System.String"));
                dt_ao.Columns.Add("Min", Type.GetType("System.Int32"));
                dt_ao.Columns.Add("Max", Type.GetType("System.Int32"));
                dt_ao.Columns.Add("Type", Type.GetType("System.String"));
                dt_ao.Columns.Add("Idx", Type.GetType("System.Int32"));
                dt_ao.Columns.Add("Use", Type.GetType("System.Boolean"));

                grid_di.DataSource = null; grid_di.DataSource = dt_di;
                grid_do.DataSource = null; grid_do.DataSource = dt_do;
                grid_ai.DataSource = null; grid_ai.DataSource = dt_ai;
                grid_ao.DataSource = null; grid_ao.DataSource = dt_ao;

                Load_Config(true,true,true,true,true);
            }
            catch (Exception ex)
            { 
                Program.log_md.LogWrite(this.Name + ".Set_Grid_Begin." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                result = ex.ToString();
            }
            return result;
        }
        public string Setting_initial()
        {
            string result = "";
            //프로그램 로드 시 1회만 수행
            try
            {

                //Grid_Set_Item.tex
                Grid_Set_Item.TextEditStyle = TextEditStyles.HideTextEditor;
                Grid_Set_Item.Buttons.Clear();
                Grid_Set_Item.Buttons.Add(new EditorButton(ButtonPredefines.Glyph, "ON", -1, true, true, false, null));
                Grid_Set_Item.Buttons.Add(new EditorButton(ButtonPredefines.Glyph, "OFF", -1, true, true, false, null));
                Grid_Set_Item.ButtonsStyle = BorderStyles.Flat;
                Grid_Set_Item.Buttons[0].Appearance.ForeColor = Color.Green;
                Grid_Set_Item.Buttons[1].Appearance.ForeColor = Color.Blue;

                Grid_Set_Item.ButtonClick -= ButtonEdit_ButtonClick;
                Grid_Set_Item.ButtonClick += ButtonEdit_ButtonClick;

            }
            catch(Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Setting_initial." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                result = ex.ToString();
            }
            return result;
        }
        private void ButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
            {
                Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
            }
            bool state = false;
            Console.WriteLine(Convert.ToByte(view_do.GetRowCellValue(view_do.FocusedRowHandle, "Idx")) + " / "  + e.Button.Caption);

            if(e.Button.Caption.ToUpper() == "OFF")
            {
                state = false;
            }
            else if (e.Button.Caption.ToUpper() == "ON")
            {
                state = true;
            }
            Program.ethercat_md.DO_Write_Alone(Convert.ToByte(view_do.GetRowCellValue(view_do.FocusedRowHandle,"Idx")), state);
            return;
        }
        public string Load_Config(Boolean act_di, Boolean act_do, Boolean act_ai, Boolean act_ao, Boolean act_serial)
        {
            
            string result = "";
            string result_load_config = "";
            int real_row_cnt = 0;
            try
            {
                //DI
                if(act_di == true)
                {
                    grid_di.BeginUpdate();
                    result_load_config = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_di, "");
                    if(result_load_config == "")
                    {
                        real_row_cnt = 0;
                        dt_di.Rows.Clear();
                        for (int idx = 0; idx < Program.IO.DI.use_cnt; idx++)
                        {
                                dt_di.Rows.Add(Program.IO.DI.Tag[idx].address,
                                     Program.IO.DI.Tag[idx].description,
                                     Program.IO.DI.Tag[idx].value,
                                     Program.IO.DI.Tag[idx].unit,
                                     idx,
                                     Program.IO.DI.Tag[idx].use);
                            if(Program.IO.DI.Tag[idx].use == true)
                            {
                                real_row_cnt = real_row_cnt + 1;
                            }
                        }
                        gp_di.Text = "Digital In" + "(" + real_row_cnt + ")";
                    }
                    else
                    {
                        return result_load_config;
                    }
                }

                //DO
                if (act_do == true)
                {
                    grid_do.BeginUpdate();
                    result_load_config = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_do, "");
                    if (result_load_config == "")
                    {
                        real_row_cnt = 0;
                        dt_do.Rows.Clear();
                        for (int idx = 0; idx < Program.IO.DO.use_cnt; idx++)
                        {
                                dt_do.Rows.Add(Program.IO.DO.Tag[idx].address,
                                                              Program.IO.DO.Tag[idx].description,
                                                              "",
                                                              Program.IO.DO.Tag[idx].value,
                                                              Program.IO.DO.Tag[idx].unit,
                                                              idx,
                                                              Program.IO.DO.Tag[idx].use);
                            if (Program.IO.DO.Tag[idx].use == true)
                            {
                                real_row_cnt = real_row_cnt + 1;
                            }
                        }
                        gp_do.Text = "Digital Out" + "(" + real_row_cnt + ")";

                    }
                    else
                    {
                        return result_load_config;
                    }
                }

                //AI
                if(act_ai == true)
                {
                    grid_ai.BeginUpdate();
                    result_load_config = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_ai, "");
                    if (result_load_config == "")
                    {
                        real_row_cnt = 0;
                        dt_ai.Rows.Clear();
                        for (int idx = 0; idx < Program.IO.AI.use_cnt; idx++)
                        {
                                dt_ai.Rows.Add(Program.IO.AI.Tag[idx].address,
                                                        Program.IO.AI.Tag[idx].description,
                                                        Program.IO.AI.Tag[idx].value,
                                                        Program.IO.AI.Tag[idx].min,
                                                        Program.IO.AI.Tag[idx].max,
                                                        Program.IO.AI.Tag[idx].unit,
                                                        idx,
                                                        Program.IO.AI.Tag[idx].use);
                            if (Program.IO.AI.Tag[idx].use == true)
                            {
                                real_row_cnt = real_row_cnt + 1;
                            }
                            gp_ai.Text = "Analog In" + "(" + real_row_cnt + ")";

                        }
                    }
                    else
                    {
                        return result_load_config;
                    }
                }

                //AO
                if (act_ao == true)
                {
                    grid_ao.BeginUpdate();
                    result_load_config = Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_ai, "");
                    if (result_load_config == "")
                    {
                        real_row_cnt = 0;
                        dt_ao.Rows.Clear();
                        for (int idx = 0; idx < Program.IO.AO.use_cnt; idx++)
                        {
                                dt_ao.Rows.Add(Program.IO.AO.Tag[idx].address,
                                Program.IO.AO.Tag[idx].description,
                                Program.IO.AO.Tag[idx].value,
                                "",
                                Program.IO.AO.Tag[idx].min,
                                Program.IO.AO.Tag[idx].max,
                                Program.IO.AO.Tag[idx].unit,
                                idx,
                                Program.IO.AO.Tag[idx].use);
                            if (Program.IO.AO.Tag[idx].use == true)
                            {
                                real_row_cnt = real_row_cnt + 1;
                            }
                            gp_ao.Text = "Analog Out" + "(" + real_row_cnt + ")";

                        }
                    }
                    else
                    {
                        return result_load_config;
                    }

                }
                Set_Grid_End(act_di, act_do, act_ai, act_ao, act_serial);
            }

            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Load_Data." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                result = ex.ToString();
            }
            finally
            {
                grid_di.EndUpdate();
                grid_do.EndUpdate();
                grid_ai.EndUpdate();
                grid_ao.EndUpdate();
            }
            return result;
        }
        public string Save_Config(Boolean act_di, Boolean act_do, Boolean act_ai, Boolean act_ao, Boolean act_serial)
        {
            string result = "";
            try
            {
                if (act_di == true)
                {
                    for (int idx = 0; idx < dt_di.Rows.Count; idx++)
                    {
                        Program.IO.DI.Tag[idx].address = Convert.ToInt32(dt_di.Rows[idx]["Address"]);
                        Program.IO.DI.Tag[idx].description = Convert.ToString(dt_di.Rows[idx]["Description"]);
                        Program.IO.DI.Tag[idx].unit = Convert.ToString(dt_di.Rows[idx]["Type"]);
                    }
                    result = "";
                    Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_di, "");
                }
                if (act_do == true)
                {
                    for (int idx = 0; idx < dt_do.Rows.Count; idx++)
                    {
                        Program.IO.DO.Tag[idx].address = Convert.ToInt32(dt_do.Rows[idx]["Address"]);
                        Program.IO.DO.Tag[idx].description = Convert.ToString(dt_do.Rows[idx]["Description"]);
                        Program.IO.DO.Tag[idx].unit = Convert.ToString(dt_do.Rows[idx]["Type"]);
                    }
                    result = "";
                    Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_do, "");
                }
                if (act_ai == true)
                {
                    for (int idx = 0; idx < dt_ai.Rows.Count; idx++)
                    {
                        Program.IO.AI.Tag[idx].address = Convert.ToInt32(dt_ai.Rows[idx]["Address"]);
                        Program.IO.AI.Tag[idx].description = Convert.ToString(dt_ai.Rows[idx]["Description"]);
                        Program.IO.AI.Tag[idx].unit = Convert.ToString(dt_ai.Rows[idx]["Type"]);
                        Program.IO.AI.Tag[idx].min = Convert.ToInt32(dt_ai.Rows[idx]["Min"]);
                        Program.IO.AI.Tag[idx].max = Convert.ToInt32(dt_ai.Rows[idx]["Max"]);
                        if (btn_auto_tunning_gain.IsOn == true)
                        {
                            Analog_To_digital_Auto_Tunning(idx);
                        }
                    }
                    result = "";

                    Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_ai, "");
                }
                if (act_ao == true)
                {
                    for (int idx = 0; idx < dt_ao.Rows.Count; idx++)
                    {
                        Program.IO.AO.Tag[idx].address = Convert.ToInt32(dt_ao.Rows[idx]["Address"]);
                        Program.IO.AO.Tag[idx].description = Convert.ToString(dt_ao.Rows[idx]["Description"]);
                        Program.IO.AO.Tag[idx].unit = Convert.ToString(dt_ao.Rows[idx]["Type"]);
                    }
                    result = "";
                    Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_ao, "");
                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".Save_Config_DI." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); result = ex.ToString(); }
            finally { }
            return result;
        }
        public void Analog_To_digital_Auto_Tunning(int idx)
        {
            double gain = 0, offset = 0;

            try
            {
                //아날로그 4mA ~ 20mA 출력값만 Auto Tunning 적용
                if (Program.IO.AI.Tag[idx].unit != "4~20mA") { return; }
                if(Program.IO.AI.Tag[idx].max == 0)
                {
                    Program.IO.AI.Tag[idx].gain = 0;
                    Program.IO.AI.Tag[idx].offset = 0;
                }
                else
                {
                    gain = (Program.IO.AI.Tag[idx].max - Program.IO.AI.Tag[idx].min) / (20 - 4);
                    offset = Program.IO.AI.Tag[idx].max - (20 * gain);

                    Program.IO.AI.Tag[idx].gain = gain;
                    Program.IO.AI.Tag[idx].offset = offset;
                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Analog_To_digital_Auto_Tunning.[" + idx + "]" + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                Program.IO.AI.Tag[idx].gain = 0;
                Program.IO.AI.Tag[idx].offset = 0;
            }
            finally { }

        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                if (btn_event.Name == "btn_cancel")
                {
                    
                    if (Program.main_md.Message_By_Application("Load Data?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                    {
                        Load_Config(true, true, true, true, true);
                    }
                }
                else if (btn_event.Name == "btn_change")
                {

                    if (Program.cg_app_info.eq_mode == enum_eq_mode.auto || Program.cg_app_info.semi_auto_state == true)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto or Semi-Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                    else if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                    else
                    {

                        if (Program.main_md.Message_By_Application("Save Data?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                        {
                            Save_Config(true, true, true, true, false);
                        }
                    }
                }
                Program.eventlog_form.Insert_Event(this.Name + ".Button Click : " + btn_event.Name, (int)frm_eventlog.enum_event_type.USER_BUTTON, (int)frm_eventlog.enum_occurred_type.USER, true);
                Program.log_md.LogWrite(this.Name + "." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        public void Set_Grid_End(Boolean act_di, Boolean act_do, Boolean act_ai, Boolean act_ao, Boolean act_serial)
        {
            var view = view_di;
            try
            {
                Grid_Combo_type_digital.Items.Clear();
                Grid_Combo_type_digital.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                Grid_Combo_type_digital.Items.AddRange(new String[] { "N.O", "N.C" });

                //반드시 Comi enum과 동일하게 Index 설정해야함 주석 지우지 말 것
                //_n10_24_to_p10_24_v = 0,
                //_n5_12_to_p5_12_v = 1,
                //_n2_56_to_p2_56_v = 2,
                //_0_to_10_24_v = 3,
                //_0_to_5_12_v = 4,
                //_4_to_20_ma = 5,
                //_0_to_20_ma = 6,
                //_0_to_24_ma = 7,
                //"-10.24v~10.24v", "-5.12v~5.12v", "-2.56v~2.56v", "0v~10.24v", "0v~5.12v", "4mA~20mA", "0mA~20mA", "0mA~24mA"
                Grid_Combo_type_analog.Items.Clear();
                Grid_Combo_type_analog.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                Grid_Combo_type_analog.Items.AddRange(new String[] { "-10.24v~10.24v", "-5.12v~5.12v", "-2.56v~2.56v", "0v~10.24v", "0v~5.12v", "4mA~20mA", "0mA~20mA", "0mA~24mA" });

                if (act_di == true)
                {
                    view = view_di;
                    view.OptionsView.ShowGroupPanel = false;
                    view.OptionsMenu.EnableGroupPanelMenu = false;
                    view.OptionsMenu.EnableColumnMenu = false;
                    view.OptionsCustomization.AllowSort = false;
                    view.OptionsCustomization.AllowFilter = false;
                    view.OptionsCustomization.AllowColumnMoving = false;
                    view.OptionsCustomization.AllowColumnResizing = false;
                    view.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
                    view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                    view.OptionsView.ShowIndicator = false;
                    view.OptionsView.ColumnAutoWidth = false;
                    view.OptionsSelection.EnableAppearanceFocusedRow = true;
                    view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
                    view.GroupPanelText = " ";

                    for (int idx = 0; idx <= view.Columns.Count - 1; idx++)
                    {
                        //공통 적용
                        view.Columns[idx].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        view.Columns[idx].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        view.Columns[idx].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        view.Columns[idx].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        view.Columns[idx].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        view.Columns[idx].AppearanceCell.Font = new Font("arial", 11);
                        if (view.Columns[idx].Name.ToUpper() == "COL" + "ADDRESS")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Address";

                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "DESCRIPTION")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = false;
                            view.Columns[idx].Width = 585;
                            view.Columns[idx].Caption = "Description";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "VALUE")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].ColumnEdit = Grid_toggle;
                            view.Columns[idx].OptionsColumn.AllowEdit = false;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Value";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "TYPE")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Type";
                            view.Columns[idx].ColumnEdit = Grid_Combo_type_digital;
                        }
                        else
                        {
                            view.Columns[idx].Visible = false;
                        }

                    }
                    view.ClearSorting();
                    view.Columns["Address"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
                if (act_do == true)
                {
                    view = view_do;
                    view.OptionsView.ShowGroupPanel = false;
                    view.OptionsMenu.EnableGroupPanelMenu = false;
                    view.OptionsMenu.EnableColumnMenu = false;
                    view.OptionsCustomization.AllowSort = false;
                    view.OptionsCustomization.AllowFilter = false;
                    view.OptionsCustomization.AllowColumnMoving = false;
                    view.OptionsCustomization.AllowColumnResizing = false;
                    view.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
                    view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                    view.OptionsView.ShowIndicator = false;
                    view.OptionsView.ColumnAutoWidth = false;

                    view.OptionsSelection.EnableAppearanceFocusedRow = true;
                    view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
                    view.GroupPanelText = " ";

                    for (int idx = 0; idx <= view.Columns.Count - 1; idx++)
                    {
                        //공통 적용
                        view.Columns[idx].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        view.Columns[idx].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        view.Columns[idx].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        view.Columns[idx].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        view.Columns[idx].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        view.Columns[idx].AppearanceCell.Font = new Font("arial", 11);
                        if (view.Columns[idx].Name.ToUpper() == "COL" + "ADDRESS")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Address";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "DESCRIPTION")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = false;
                            view.Columns[idx].Width = 445;
                            view.Columns[idx].Caption = "Description";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "VALUE")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].ColumnEdit = Grid_toggle;
                            view.Columns[idx].OptionsColumn.AllowEdit = false;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Value";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "SET")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
                            view.Columns[idx].ColumnEdit = Grid_Set_Item;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Set";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "TYPE")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Type";
                            view.Columns[idx].ColumnEdit = Grid_Combo_type_digital;
                        }
                        else
                        {
                            view.Columns[idx].Visible = false;
                        }
                    }
                    view.ClearSorting();
                    view.Columns["Address"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
                if (act_ai == true)
                {
                    view = view_ai;
                    view.OptionsView.ShowGroupPanel = false;
                    view.OptionsMenu.EnableGroupPanelMenu = false;
                    view.OptionsMenu.EnableColumnMenu = false;
                    view.OptionsCustomization.AllowSort = false;
                    view.OptionsCustomization.AllowFilter = false;
                    view.OptionsCustomization.AllowColumnMoving = false;
                    view.OptionsCustomization.AllowColumnResizing = false;
                    view.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
                    view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                    view.OptionsView.ShowIndicator = false;
                    view.OptionsView.ColumnAutoWidth = false;

                    view.OptionsSelection.EnableAppearanceFocusedRow = true;
                    view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
                    view.GroupPanelText = " ";

                    for (int idx = 0; idx <= view.Columns.Count - 1; idx++)
                    {
                        //공통 적용
                        view.Columns[idx].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        view.Columns[idx].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        view.Columns[idx].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        view.Columns[idx].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        view.Columns[idx].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        view.Columns[idx].AppearanceCell.Font = new Font("arial", 11);
                        if (view.Columns[idx].Name.ToUpper() == "COL" + "ADDRESS")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Address";

                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "DESCRIPTION")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = false;
                            view.Columns[idx].Width = 465;
                            view.Columns[idx].Caption = "Description";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "VALUE")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].ColumnEdit = Grid_Txt;
                            view.Columns[idx].OptionsColumn.AllowEdit = false;
                            view.Columns[idx].Width = 90;
                            view.Columns[idx].Caption = "Value";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "MIN")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
                            view.Columns[idx].ColumnEdit = Grid_Txt;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 90;
                            view.Columns[idx].Caption = "Min";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "MAX")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
                            view.Columns[idx].ColumnEdit = Grid_Txt;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 90;
                            view.Columns[idx].Caption = "Max";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "TYPE")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Type";
                            view.Columns[idx].ColumnEdit = Grid_Combo_type_analog;
                        }
                        else
                        {
                            view.Columns[idx].Visible = false;
                        }
                    }
                    view.ClearSorting();
                    view.Columns["Address"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
                if (act_ao == true)
                {
                    view = view_ao;
                    view.OptionsView.ShowGroupPanel = false;
                    view.OptionsMenu.EnableGroupPanelMenu = false;
                    view.OptionsMenu.EnableColumnMenu = false;
                    view.OptionsCustomization.AllowSort = false;
                    view.OptionsCustomization.AllowFilter = false;
                    view.OptionsCustomization.AllowColumnMoving = false;
                    view.OptionsCustomization.AllowColumnResizing = false;
                    view.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
                    view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                    view.OptionsView.ShowIndicator = false;
                    view.OptionsView.ColumnAutoWidth = false;

                    view.OptionsSelection.EnableAppearanceFocusedRow = true;
                    view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
                    view.GroupPanelText = " ";

                    for (int idx = 0; idx <= view.Columns.Count - 1; idx++)
                    {
                        //공통 적용
                        view.Columns[idx].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        view.Columns[idx].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        view.Columns[idx].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        view.Columns[idx].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        view.Columns[idx].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        view.Columns[idx].AppearanceCell.Font = new Font("arial", 11);
                        if (view.Columns[idx].Name.ToUpper() == "COL" + "ADDRESS")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Address";

                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "DESCRIPTION")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = false;
                            view.Columns[idx].Width = 375;
                            view.Columns[idx].Caption = "Description";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "VALUE")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].ColumnEdit = Grid_Txt;
                            view.Columns[idx].OptionsColumn.AllowEdit = false;
                            view.Columns[idx].Width = 90;
                            view.Columns[idx].Caption = "Value";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "SET")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].ColumnEdit = Grid_Txt;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 90;
                            view.Columns[idx].Caption = "Set";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "MIN")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
                            view.Columns[idx].ColumnEdit = Grid_Txt;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 90;
                            view.Columns[idx].Caption = "Min";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "MAX")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
                            view.Columns[idx].ColumnEdit = Grid_Txt;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 90;
                            view.Columns[idx].Caption = "Max";
                        }
                        else if (view.Columns[idx].Name.ToUpper() == "COL" + "TYPE")
                        {
                            view.Columns[idx].Visible = true;
                            view.Columns[idx].OptionsColumn.AllowEdit = true;
                            view.Columns[idx].Width = 140;
                            view.Columns[idx].Caption = "Type";
                            view.Columns[idx].ColumnEdit = Grid_Combo_type_analog;
                        }
                        else
                        {
                            view.Columns[idx].Visible = false;
                        }
                    }
                }
                view.ClearSorting();
                view.Columns["Address"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            }

            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".Set_Grid_End." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally { }
        }
        private void view_di_MouseWheel(object sender, MouseEventArgs e)
        {
            //Edit 중에 스크롤 입력 시 Edit 종료 후 스크롤 이동
            GridView view = sender as GridView;   
            try{ view.CloseEditor(); }catch (Exception ex){}
            //Address Validate시 Error 시 Pass 위함, 로그 생성 필요 없음
        }
        private void timer_ui_change_Tick(object sender, EventArgs e)
        {
            UI_Change();
        }
        public void UI_Change()
        {
            try
            {
                //DI
                for (int idx = 0; idx < Program.IO.DI.use_cnt; idx++)
                {
                    if(btn_view_raw.IsOn == true)
                    {
                        dt_di.Rows[idx]["value"] = Program.IO.DI.Tag[idx].value_raw;
                    }
                    else
                    {
                        dt_di.Rows[idx]["value"] = Program.IO.DI.Tag[idx].value;
                    }
                }
                //DO
                for (int idx = 0; idx < Program.IO.DO.use_cnt; idx++)
                {
                    if (btn_view_raw.IsOn == true)
                    {
                        dt_do.Rows[idx]["value"] = Program.IO.DO.Tag[idx].value_raw;
                    }
                    else
                    {
                        dt_do.Rows[idx]["value"] = Program.IO.DO.Tag[idx].value;
                    }
                   
                }
                //AI
                for (int idx = 0; idx < Program.IO.AI.use_cnt; idx++)
                {
                    if (btn_view_raw.IsOn == true)
                    {
                        dt_ai.Rows[idx]["value"] = Program.IO.AI.Tag[idx].value_raw;
                    }
                    else
                    {
                        dt_ai.Rows[idx]["value"] = Program.IO.AI.Tag[idx].value;
                    }
                }
                //AO
                for (int idx = 0; idx < Program.IO.AO.use_cnt; idx++)
                {
                    if (btn_view_raw.IsOn == true)
                    {
                        dt_ao.Rows[idx]["value"] = Program.IO.AO.Tag[idx].value_raw;
                    }
                    else
                    {
                        dt_ao.Rows[idx]["value"] = Program.IO.AO.Tag[idx].value;
                    }
                }
            }
            catch(Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".UI_Change." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        private void view_di_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            GridView gridView = sender as GridView;
            DataRow[] rows = null;
            if (gridView.FocusedColumn.FieldName == "Address")
            {
                rows = dt_di.Select("Use = true AND Address = '" + e.Value + "'");
                if (rows.Length >= 1)
                {
                    e.Valid = false;
                    e.ErrorText = "Duplicated Address";
                }
            }
        }
        private void view_do_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            GridView gridView = sender as GridView;
            DataRow[] rows = null;
            if (gridView.FocusedColumn.FieldName == "Address")
            {
                rows = dt_do.Select("Use = true AND Address = '" + e.Value + "'");
                if (rows.Length >= 1)
                {
                    e.Valid = false;
                    e.ErrorText = "Duplicated Address";
                }
            }
        }
        private void view_ai_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            GridView gridView = sender as GridView;
            DataRow[] rows = null;
            if (gridView.FocusedColumn.FieldName == "Address")
            {
                rows = dt_ai.Select("Use = true AND Address = '" + e.Value + "'");
                if (rows.Length >= 1)
                {
                    e.Valid = false;
                    e.ErrorText = "Duplicated Address";
                }
            }
        }
        private void view_ao_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            GridView gridView = sender as GridView;
            DataRow[] rows = null;
            if (gridView.FocusedColumn.FieldName == "Address")
            {
                rows = dt_ao.Select("Use = true AND Address = '" + e.Value + "'");
                if (rows.Length >= 1)
                {
                    e.Valid = false;
                    e.ErrorText = "Duplicated Address";
                }
            }
        }
        private void view_di_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            GridView columnView = sender as GridView;
            bool use = Convert.ToBoolean( columnView.GetListSourceRowCellValue(e.ListSourceRow, "Use"));
            if (use == false)
            {
                e.Visible = false;
                e.Handled = true;
            }
        }
        private void view_do_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            GridView columnView = sender as GridView;
            bool use = Convert.ToBoolean(columnView.GetListSourceRowCellValue(e.ListSourceRow, "Use"));
            if (use == false)
            {
                e.Visible = false;
                e.Handled = true;
            }
        }
        private void view_ai_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            GridView columnView = sender as GridView;
            bool use = Convert.ToBoolean(columnView.GetListSourceRowCellValue(e.ListSourceRow, "Use"));
            if (use == false)
            {
                e.Visible = false;
                e.Handled = true;
            }
        }
        private void view_ao_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            GridView columnView = sender as GridView;
            bool use = Convert.ToBoolean(columnView.GetListSourceRowCellValue(e.ListSourceRow, "Use"));
            if (use == false)
            {
                e.Visible = false;
                e.Handled = true;
            }
        }

        private void view_ai_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            GridView columnView = sender as GridView;
            if(e.Column.FieldName == "Value" && e.ListSourceRowIndex != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
            {
                decimal value = Convert.ToDecimal(e.Value);
                e.DisplayText = string.Format("{0:f2}", value);
            }
        }

        private void btn_auto_tunning_gain_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
            {
                e.Cancel = true;
                Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
            }
        }

        private void btn_view_raw_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
            {
                e.Cancel = true;
                Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
            }
        }
    }
}