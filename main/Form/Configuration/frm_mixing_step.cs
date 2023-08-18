using DevExpress.XtraDiagram.Base;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace cds
{
    public partial class frm_mixing_step : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        private Boolean _initialized = false;
        //Alarm List 사용 데이터 객체
        DataSet ds_mixing_step = new DataSet();
        DataTable dt_mixing_step = new DataTable("mixing_step");

        //Grid에서 사용할 Reposit Item
        public RepositoryItemTextEdit Grid_Txt_Readonly = new RepositoryItemTextEdit();
        public RepositoryItemPictureEdit Grid_PictureEdit = new RepositoryItemPictureEdit();
        public RepositoryItemTextEdit Grid_Txt = new RepositoryItemTextEdit();

        public frm_mixing_step()
        {
            InitializeComponent();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
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
                        Set_Grid_Begin();
                        Load_Data(enum_mixing_select.MAIN, "");
                        Set_Grid_End();

                        if (_initialized == false)
                        {
                            _initialized = true;
                            if(Program.cg_mixing_step.refill_use == true)
                            {
                                gp_group_refill_status.Visible = true;
                            }
                            else
                            {
                                gp_group_refill_status.Visible = false;
                            }
                            Setting_Mixing_Order(tank_class.enum_tank_type.TANK_ALL);
                        }
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
        public enum enum_mixing_select
        {
            MAIN = 0,
            CUSTOM1 = 1,
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
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
                    if (btn_event.Name == "btn_cancel")
                    {
                        if (Program.main_md.Message_By_Application("Load Step Data?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                        {
                            Load_Data(enum_mixing_select.MAIN, "");
                        }
                    }
                    else if (btn_event.Name == "btn_change")
                    {
                        if (Program.main_md.Message_By_Application("Save Step Data?", frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                        {
                            Save_Data(enum_mixing_select.MAIN, "");
                            Load_Data(enum_mixing_select.MAIN, "");
                        }

                    }
                    else if (btn_event.Name == "btn_ccss1_up" || btn_event.Name == "btn_ccss2_up" || btn_event.Name == "btn_ccss3_up" || btn_event.Name == "btn_ccss4_up")
                    {
                        Mixing_Step_Move(btn_event.Tag.ToString());
                    }
                    else if (btn_event.Name == "btn_ccss1_down" || btn_event.Name == "btn_ccss2_down" || btn_event.Name == "btn_ccss3_down" || btn_event.Name == "btn_ccss4_down")
                    {
                        Mixing_Step_Move(btn_event.Tag.ToString());
                    }
                    else if (btn_event.Name == "btn_custom_1_save")
                    {
                        if (cmb_mixinglist.Text == "")
                        {
                            Program.main_md.Message_By_Application("Mixing Step Custom File Name is Empty", frm_messagebox.enum_apptype.Only_OK);
                        }
                        else
                        {
                            Save_Data(enum_mixing_select.CUSTOM1, cmb_mixinglist.Text + ".yaml");
                        }
                    }
                    else if (btn_event.Name == "btn_custom_1_load")
                    {
                        if (System.IO.File.Exists(Program.cg_main.path.yaml + Program.cg_main.path.mixing_step_save_folder_name + @"\" + cmb_mixinglist.Text + ".yaml") == false)
                        {
                            Program.main_md.Message_By_Application("Mixing Step Custom File is not Exist", frm_messagebox.enum_apptype.Only_OK);
                        }
                        else
                        {
                            Load_Data(enum_mixing_select.CUSTOM1, cmb_mixinglist.Text + ".yaml");
                        }
                    }
                }

            }
            Program.eventlog_form.Insert_Event(this.Name + ".Button Click : " + btn_event.Name, (int)frm_eventlog.enum_event_type.USER_BUTTON, (int)frm_eventlog.enum_occurred_type.USER, true);
            Program.log_md.LogWrite(this.Name + "." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }
        public void Mixing_Step_Move(string tag_value)
        {
            string direction = "";
            int idx_current_columns = 0;
            int idx_target = 0;
            bool parent_not_exist;
            bool parent_not_exist2;
            try
            {
                if (tag_value.Split('_').Length != 2) { return; }

                direction = tag_value.Split('_')[0];
                idx_current_columns = Convert.ToInt32(tag_value.Split('_')[1]) - 1;
                //Tag 값은 Direction_Index로 정의
                //Tag 값은 DataTable로 관리함

                //현재 CCSS열의 Row 위치 찾기
                for (int idx = 0; idx < Program.cg_mixing_step.step_cnt; idx++)
                {
                    if (dt_mixing_step.Rows[idx][idx_current_columns + 1].ToString() == "0")
                    {

                    }
                    else
                    {
                        if (direction == "UP")
                        {
                            idx_target = idx - 1;
                            // 0행에서 더이상 이동할 수 없음
                            if (idx_target <= 0) { idx_target = 0; }

                            //하위 행 데이터가 없을 경우 위로 이동 불가
                            parent_not_exist = true;
                            for (int idx_search = 0; idx_search < Program.cg_mixing_step.ccss_cnt; idx_search++)
                            {
                                //자기 자신을 제외한 나머지 행을 Check
                                //다음 행에서 값이 있는지 확인 후 한개라도 존재하고, 현재 행에 아무 값도 없을 경우 이동 불가
                                //DataTable의 컬럼 0은 Step 정보이므로, Index 주의 필요
                                if (idx + 1 == Program.cg_mixing_step.step_cnt)
                                {
                                    ////마지막 행일 경우 
                                    //if (idx_search + 1 != idx_current_columns + 1 &&  dt_mixing_step.Rows[idx][idx_search + 1].ToString() == "1")
                                    //{
                                    parent_not_exist = false;
                                    break;
                                    //}
                                }
                                else
                                {
                                    //하위 행에 데이타가 있는 상태에서 자신 이외에 현재 행을 비우게됨을 방지함, 하위 행에 데이타가 없으면 이동
                                    //하위 행 전체 체크
                                    parent_not_exist2 = false;
                                    for (int idx_search2 = 0; idx_search2 < Program.cg_mixing_step.ccss_cnt; idx_search2++)
                                    {
                                        if (dt_mixing_step.Rows[idx + 1][idx_search2 + 1].ToString() == "1")
                                        {
                                            if (idx_search2 + 1 != idx_current_columns + 1 && dt_mixing_step.Rows[idx + 1][idx_search2 + 1].ToString() == "1")
                                            {
                                                parent_not_exist2 = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //parent_not_exist = false;
                                            //break;
                                        }
                                    }

                                    //하위 행이 아무것도 없을 경우 바로 상행 가능
                                    if (parent_not_exist2 == false)
                                    {
                                        //for문 종료 시 외부 체크 문 통과 
                                        parent_not_exist = false;
                                        break;
                                    }
                                    //하위 행이 존재해, 상행 불가능
                                    else
                                    {

                                    }
                                    if (idx_search + 1 != idx_current_columns + 1 && dt_mixing_step.Rows[idx][idx_search + 1].ToString() == "1")
                                    {
                                        parent_not_exist = false;
                                        break;
                                    }

                                }

                            }
                            if (parent_not_exist == true)
                            {
                                Program.main_md.Message_By_Application("Can`t Move Step", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                dt_mixing_step.Rows[idx][idx_current_columns + 1] = 0;
                                dt_mixing_step.Rows[idx_target][idx_current_columns + 1] = 1;
                            }

                            break;
                        }
                        else if (direction == "DOWN")
                        {
                            //첫번째 행은 Not Use로 어떤 행이던 이동할 수 있다.
                            if (idx == 0)
                            {
                                idx_target = idx + 1;
                                dt_mixing_step.Rows[idx][idx_current_columns + 1] = 0;
                                dt_mixing_step.Rows[idx_target][idx_current_columns + 1] = 1;
                                break;
                            }
                            else
                            {
                                idx_target = idx + 1;
                                // 마지막 행에서 더이상 이동할 수 없음
                                if (idx_target >= Program.cg_mixing_step.step_cnt - 1) { idx_target = Program.cg_mixing_step.step_cnt - 1; }

                                //자신이 이동했을 때 상위 행 데이터가 없을 경우 아래로 이동 불가
                                //한행에는 항상 한개의 데이타는 있어야함 = 중간에 비는 Step이 없어야함
                                parent_not_exist = true;
                                for (int idx_search = 0; idx_search < Program.cg_mixing_step.ccss_cnt; idx_search++)
                                {
                                    //자기 자신을 제외한 나머지 행을 Check
                                    //현재 행에서 값이 있는지 확인 후 아무 것도 없으면 이동 불가
                                    //DataTable의 컬럼 0은 Step 정보이므로, Index 주의 필요
                                    //Console.WriteLine(idx_target + " - " + idx_search + 1 + " : " + dt_mixing_step.Rows[idx_target][idx_search + 1].ToString());


                                    //if (idx_search + 1 != idx_current_columns + 1 && dt_mixing_step.Rows[idx_target][idx_search].ToString() == "1")
                                    //idx_search +1은 Step_index 추가 계산
                                    //idx_current_columns +1 = 현재 선택한 버튼의 Column Index
                                    if ((idx_search + 1 != idx_current_columns + 1) && dt_mixing_step.Rows[idx][idx_search + 1].ToString() == "1")
                                    {
                                        parent_not_exist = false;
                                        break;
                                    }
                                }
                                if (parent_not_exist == true)
                                {
                                    Program.main_md.Message_By_Application("Can`t Move Step", frm_messagebox.enum_apptype.Only_OK);
                                }
                                else
                                {
                                    dt_mixing_step.Rows[idx][idx_current_columns + 1] = 0;
                                    dt_mixing_step.Rows[idx_target][idx_current_columns + 1] = 1;
                                }
                                break;
                            }

                        }
                        else
                        {

                        }
                    }
                }

            }
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".Mixing_Step_Move." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally { }
        }
        public void Set_Grid_Begin()
        {
            try
            {
                if (ds_mixing_step != null) { ds_mixing_step.Dispose(); ds_mixing_step = null; ds_mixing_step = new DataSet(); }

                ds_mixing_step = new DataSet("mixing_step");
                dt_mixing_step = new DataTable("mixing_step");
                ds_mixing_step.Tables.Add(dt_mixing_step);

                dt_mixing_step.Columns.Clear();
                dt_mixing_step.Rows.Clear();


                dt_mixing_step.Columns.Add("STEP_INDEX", Type.GetType("System.String"));
                for (int idx = 0; idx < Program.cg_mixing_step.ccss_cnt; idx++)
                {
                    //Program.cg_mixing_step.ccss_info[idx].name.ToUpper()
                    dt_mixing_step.Columns.Add("CCSS" + (idx + 1), Type.GetType("System.Int32"));
                    //dt_mixing_step.Columns.Add(Program.cg_mixing_step.ccss_info[idx].name, Type.GetType("System.Int32"));
                }
                for (int idx = 0; idx < Program.cg_mixing_step.step_cnt; idx++)
                {
                    if (Program.cg_mixing_step.ccss_cnt == 1)
                    {
                        dt_mixing_step.Rows.Add(Program.cg_mixing_step.step_info[idx].name, 0);
                    }
                    else if (Program.cg_mixing_step.ccss_cnt == 2)
                    {
                        dt_mixing_step.Rows.Add(Program.cg_mixing_step.step_info[idx].name, 0, 0);
                    }
                    else if (Program.cg_mixing_step.ccss_cnt == 3)
                    {
                        dt_mixing_step.Rows.Add(Program.cg_mixing_step.step_info[idx].name, 0, 0, 0);
                    }
                    else if (Program.cg_mixing_step.ccss_cnt == 4)
                    {
                        dt_mixing_step.Rows.Add(Program.cg_mixing_step.step_info[idx].name, 0, 0, 0, 0);
                    }


                }

                gridview_mixing_step.BeginUpdate();
                gridview_mixing_step.Columns.Clear();
                gridcontrol1.DataSource = null;
                gridcontrol1.DataSource = dt_mixing_step;

            }
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".Set_Grid_Begin." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally
            {
                gridview_mixing_step.EndUpdate();
            }
        }
        public void Setting_Initialize()
        {
            try
            {

            }
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".Setting_Initialize." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally
            {
            }
        }
        public void Load_Data(enum_mixing_select mixing_select, string filename)
        {
            try
            {
                if (mixing_select == enum_mixing_select.MAIN)
                {
                    if (Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_mixing_step, "") != "")
                    {
                        //load Fail
                        return;
                    }
                }
                else if (mixing_select == enum_mixing_select.CUSTOM1)
                {
                    if (Program.sub_md.Config_Yaml_To_Class(Module_sub.Config_type.cg_mixing_step_custom1, filename) != "")
                    {
                        //load Fail
                        Program.main_md.Message_By_Application("Custom " + filename + " File Open Error", frm_messagebox.enum_apptype.Only_OK);
                        return;
                    }
                    else
                    {
                        Program.main_md.Message_By_Application("Load Complete", frm_messagebox.enum_apptype.Only_OK);
                    }
                }

                gp_name_ccss1.Visible = false;
                gp_name_ccss2.Visible = false;
                gp_name_ccss3.Visible = false;
                gp_name_ccss4.Visible = false;
                for (int idx_row = 0; idx_row < Program.cg_mixing_step.step_cnt; idx_row++)
                {
                    for (int idx_col = 0; idx_col < Program.cg_mixing_step.ccss_cnt; idx_col++)
                    {
                        if (idx_row == 0)
                        {
                            //ROW(Step Index) 설정
                            switch (idx_col)
                            {
                                case 0:
                                    gp_name_ccss1.Text = Program.cg_mixing_step.ccss_info[idx_col].name;
                                    if (Program.cg_mixing_step.ccss_info[idx_col].use == true) { gp_name_ccss1.Visible = true; }
                                    else { gp_name_ccss1.Visible = false; }
                                    break;
                                case 1:
                                    gp_name_ccss2.Text = Program.cg_mixing_step.ccss_info[idx_col].name;
                                    if (Program.cg_mixing_step.ccss_info[idx_col].use == true) { gp_name_ccss2.Visible = true; }
                                    else { gp_name_ccss2.Visible = false; }
                                    break;
                                case 2:
                                    gp_name_ccss3.Text = Program.cg_mixing_step.ccss_info[idx_col].name;
                                    if (Program.cg_mixing_step.ccss_info[idx_col].use == true) { gp_name_ccss3.Visible = true; }
                                    else { gp_name_ccss3.Visible = false; }
                                    break;
                                case 3:
                                    gp_name_ccss4.Text = Program.cg_mixing_step.ccss_info[idx_col].name;
                                    if (Program.cg_mixing_step.ccss_info[idx_col].use == true) { gp_name_ccss4.Visible = true; }
                                    else { gp_name_ccss4.Visible = false; }
                                    break;
                            }
                        }
                        //step index 제외로 Columns + 1
                        dt_mixing_step.Rows[idx_row][idx_col + 1] = Program.cg_mixing_step.mixing_data[(idx_row * Program.cg_mixing_step.ccss_cnt) + (idx_col)];

                    }
                }

                btn_use.IsOn = Program.cg_mixing_step.refill_use;
                cmb_refill_ccss.Properties.Items.Clear();
                foreach (var temp in Enum.GetValues(typeof(enum_ccss)))
                {
                    cmb_refill_ccss.Properties.Items.Add(temp);
                }
                cmb_refill_ccss.SelectedItem = Program.cg_mixing_step.refill_ccss;
            }
            //Error 발생 시 공정 사고를 방지해 Qeueu Clear / Qeuec 가 0개라면 Exchange 중지
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".Load_Data." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally { }
        }
        public void Save_Data(enum_mixing_select mixing_select, string filename)
        {
            try
            {

                for (int idx_row = 0; idx_row < Program.cg_mixing_step.step_cnt; idx_row++)
                {
                    for (int idx_col = 0; idx_col < Program.cg_mixing_step.ccss_cnt; idx_col++)
                    {
                        //step index 제외
                        Program.cg_mixing_step.mixing_data[(idx_row * Program.cg_mixing_step.ccss_cnt) + (idx_col)] = Convert.ToInt32(dt_mixing_step.Rows[idx_row][idx_col + 1]);
                    }
                }
                Program.cg_mixing_step.refill_use = btn_use.IsOn;
                Program.cg_mixing_step.refill_ccss = (enum_ccss)Enum.Parse(typeof(enum_ccss), cmb_refill_ccss.Text);

                if (mixing_select == enum_mixing_select.MAIN)
                {
                    if (Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_mixing_step, "") == "")
                    {
                        Program.main_md.Message_By_Application("Save Complete.", frm_messagebox.enum_apptype.Only_OK);
                    }
                    else
                    {
                        Program.main_md.Message_By_Application("Save Fail.", frm_messagebox.enum_apptype.Only_OK);
                    }
                }
                else if (mixing_select == enum_mixing_select.CUSTOM1)
                {
                    if (Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_mixing_step_custom1, filename) == "")
                    {
                        Program.main_md.Message_By_Application("Save Complete.", frm_messagebox.enum_apptype.Only_OK);
                    }
                    else
                    {
                        Program.main_md.Message_By_Application("Save Fail.", frm_messagebox.enum_apptype.Only_OK);
                    }
                }

            }
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".Save_Data." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally { }
        }
        public void Set_Grid_End()
        {
            string col_name_temp = "";
            try
            {
                gridview_mixing_step.OptionsView.ShowGroupPanel = false;
                gridview_mixing_step.OptionsMenu.EnableGroupPanelMenu = false;
                gridview_mixing_step.OptionsMenu.EnableColumnMenu = false;
                gridview_mixing_step.OptionsCustomization.AllowSort = false;
                gridview_mixing_step.OptionsCustomization.AllowFilter = false;
                gridview_mixing_step.OptionsCustomization.AllowColumnMoving = false;
                gridview_mixing_step.OptionsCustomization.AllowColumnResizing = false;
                gridview_mixing_step.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
                gridview_mixing_step.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                gridview_mixing_step.OptionsView.ShowIndicator = false;
                gridview_mixing_step.OptionsView.ColumnAutoWidth = false;
                gridview_mixing_step.OptionsView.RowAutoHeight = false;

                gridview_mixing_step.OptionsSelection.EnableAppearanceFocusedRow = false;
                gridview_mixing_step.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
                gridview_mixing_step.GroupPanelText = " ";


                Grid_PictureEdit.NullText = " ";
                Grid_PictureEdit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;

                //Program.cls_main.Grid_Txt_Readonly.ReadOnly = true;
                //Program.cls_main.Grid_Combo_type.Items.Clear();
                //Program.cls_main.Grid_Combo_type.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                //Program.cls_main.Grid_Combo_type.Items.AddRange(new String[] { "round", "oneway" });
                for (int i = 0; i <= gridview_mixing_step.Columns.Count - 1; i++)
                {
                    //////////////////////////////////////////////////////
                    ///Step       CCSS1   CCSS2   CCSS3   CCSS4
                    /// Not Use
                    /// STEP1
                    /// STEP2
                    /// STEP3
                    /// STEP4
                    if (gridview_mixing_step.Columns[i].Name.ToUpper() == "COL" + "STEP_INDEX")
                    {
                        gridview_mixing_step.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_mixing_step.Columns[i].Visible = true;
                        //grid_mixing_step.Columns["STEP"].ColumnEdit = Program.cls_main.Grid_Txt_Readonly;
                        gridview_mixing_step.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_mixing_step.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_mixing_step.Columns[i].Width = 180;
                        gridview_mixing_step.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridview_mixing_step.Columns[i].Caption = " ";

                    }

                    else if (gridview_mixing_step.Columns[i].Name.ToUpper() == "COL" + "CCSS1")
                    {
                        gridview_mixing_step.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        //grid_mixing_step.Columns["STEP"].ColumnEdit = Program.cls_main.Grid_Txt_Readonly;
                        gridview_mixing_step.Columns[i].ColumnEdit = Grid_PictureEdit;
                        gridview_mixing_step.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_mixing_step.Columns[i].Width = 210;
                        gridview_mixing_step.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridview_mixing_step.Columns[i].UnboundDataType = typeof(Int32);
                        gridview_mixing_step.Columns[i].Caption = Program.cg_mixing_step.ccss_info[0].name;
                        if (Program.cg_mixing_step.ccss_info[0].use == true) { gridview_mixing_step.Columns[i].Visible = true; }
                        else { gridview_mixing_step.Columns[i].Visible = false; }


                    }
                    else if (gridview_mixing_step.Columns[i].Name.ToUpper() == "COL" + "CCSS2")
                    {
                        gridview_mixing_step.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        //grid_mixing_step.Columns["STEP"].ColumnEdit = Program.cls_main.Grid_Txt_Readonly;
                        gridview_mixing_step.Columns[i].ColumnEdit = Grid_PictureEdit;
                        gridview_mixing_step.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_mixing_step.Columns[i].Width = 210;
                        gridview_mixing_step.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridview_mixing_step.Columns[i].UnboundDataType = typeof(Int32);
                        gridview_mixing_step.Columns[i].Caption = Program.cg_mixing_step.ccss_info[1].name;
                        if (Program.cg_mixing_step.ccss_info[1].use == true) { gridview_mixing_step.Columns[i].Visible = true; }
                        else { gridview_mixing_step.Columns[i].Visible = false; }
                    }
                    else if (gridview_mixing_step.Columns[i].Name.ToUpper() == "COL" + "CCSS3")
                    {
                        gridview_mixing_step.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        //grid_mixing_step.Columns["STEP"].ColumnEdit = Program.cls_main.Grid_Txt_Readonly;
                        gridview_mixing_step.Columns[i].ColumnEdit = Grid_PictureEdit;
                        gridview_mixing_step.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_mixing_step.Columns[i].Width = 210;
                        gridview_mixing_step.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridview_mixing_step.Columns[i].UnboundDataType = typeof(Int32);
                        gridview_mixing_step.Columns[i].Caption = Program.cg_mixing_step.ccss_info[2].name;
                        if (Program.cg_mixing_step.ccss_info[2].use == true) { gridview_mixing_step.Columns[i].Visible = true; }
                        else { gridview_mixing_step.Columns[i].Visible = false; }
                    }
                    else if (gridview_mixing_step.Columns[i].Name.ToUpper() == "COL" + "CCSS4")
                    {
                        gridview_mixing_step.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        //grid_mixing_step.Columns["STEP"].ColumnEdit = Program.cls_main.Grid_Txt_Readonly;
                        gridview_mixing_step.Columns[i].ColumnEdit = Grid_PictureEdit;
                        gridview_mixing_step.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_mixing_step.Columns[i].Width = 210;
                        gridview_mixing_step.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridview_mixing_step.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridview_mixing_step.Columns[i].UnboundDataType = typeof(Int32);
                        gridview_mixing_step.Columns[i].Caption = Program.cg_mixing_step.ccss_info[3].name;
                        if (Program.cg_mixing_step.ccss_info[3].use == true) { gridview_mixing_step.Columns[i].Visible = true; }
                        else { gridview_mixing_step.Columns[i].Visible = false; }
                    }
                }


                for (int idx_col = 0; idx_col <= gridview_mixing_step.Columns.Count - 1; idx_col++)
                {
                    for (int idx_row = 0; idx_row <= gridview_mixing_step.RowCount - 1; idx_row++)
                    {
                        //gridview_mixing_step.SetRowCellValue(0, "CCSS4", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_32x32.png"));
                    }
                }

            }

            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".Set_Grid_End." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }

            finally { }
        }
        private void gridview_mixing_step_CalcRowHeight(object sender, DevExpress.XtraGrid.Views.Grid.RowHeightEventArgs e)
        {
            e.RowHeight = 162;
        }
        private void gridview_mixing_step_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            try
            {
                if (e.Column.FieldName.IndexOf("STEP_INDEX") < 0)
                {
                    e.DefaultDraw();
                    if (Convert.ToInt32(e.CellValue) == 0)
                    {
                        //e.Cache.DrawImage(null, e.Bounds.Location);
                    }
                    else
                    {

                        //e.Cache
                        e.Cache.DrawImage(DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_32x32.png"), e.Bounds.X + 90, e.Bounds.Y + 70);
                        e.Handled = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".gridview_mixing_step_CustomDrawCell." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

        }
        private void gridview_mixing_step_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Image" && e.IsGetData)
                {
                    GridView view = sender as GridView;
                    var cellvalue = view.GetRowCellValue(e.ListSourceRowIndex, "Step");

                    if (Convert.ToInt32(cellvalue) == 0)
                    {
                        //e.Cache.DrawImage(null, e.Bounds.Location);
                    }
                    else
                    {
                        //e.Cache.DrawImage(DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_32x32.png"), e.Bounds.Location);
                    }
                    //e.Value = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_32x32.png");
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".gridview_mixing_step_CustomUnboundColumnData." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        private void gridview_mixing_step_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //if (e.Column.FieldName.IndexOf("STEP_INDEX") >= 0) return;

            //var riteEdit = new RepositoryItemPictureEdit();
            //var gv = sender as GridView;
            //var fieldName = gv.GetRowCellValue(e.RowHandle, gv.Columns["chem1"]).ToString();
            //riteEdit.NullText = " ";
            //riteEdit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            //switch (fieldName)
            //{
            //    case "0"://"000000/000"  
            //        //riteEdit.con
            //        //gv.Set
            //        //
            //        //riteEdit.Images = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/apply_32x32.png");
            //        //e.RepositoryItem = riteEdit;
            //        break;
            //    case "1":

            //        break;
            //}
        }
        private void gridview_mixing_step_CustomRowFilter(object sender, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e)
        {
            //사용하지 않는 Row 숨김 Columns 숨김은 Grid_Set_end에서 가능
            if (Program.cg_mixing_step.step_info[e.ListSourceRow].use == false)
            {
                e.Visible = false;
                e.Handled = true;
            }
        }
        private void frm_mixing_step_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Program.seq.mixing_order 사용
        /// </summary>
        public void Setting_Mixing_Order(tank_class.enum_tank_type selected_tank)
        {
            string volume_name = "", volumne_ratio = "";
            string volume_a = "", volume_b = "";
            float volume_ratio_sum = 0; //분모
            double tank_a_volume_h_level = 60, tank_b_volume_h_level = 60; //Tank 사용 H Level Volume
            double tmp_para_Value = 0;

            //정상 저장되었을 때 Queue에 Mixing 순서대로 담는다.
            //Sequence Exchange 진행 시 queue에서 한개씩 빼서 Supply 진행
            //해당 CCSS 구조체의 Type을 넣는다 Type은 DIW, CCS1, CCS2 등으로 구성되어, Sequence에서 알 수있다.
            //Setting_Mixing_Order 수행은 두 군데서 진행
            //1. Mixing Load시 호출
            //2. Sequence 진행 시 호출

            try
            {
                if (selected_tank == tank_class.enum_tank_type.TANK_A)
                {
                    if (Program.seq.semi_auto_tank_a.mixing_order == null)
                    {
                        Program.seq.semi_auto_tank_a.mixing_order = new System.Collections.Generic.Queue<CCSS_Info>();
                        Program.seq.semi_auto_tank_a.mixing_order_list = new List<CCSS_Info>();
                    }
                    Program.seq.semi_auto_tank_a.mixing_order.Clear();
                    Program.seq.semi_auto_tank_a.mixing_order_list.Clear();
                }
                else if (selected_tank == tank_class.enum_tank_type.TANK_B)
                {
                    if (Program.seq.semi_auto_tank_b.mixing_order == null)
                    {
                        Program.seq.semi_auto_tank_b.mixing_order = new System.Collections.Generic.Queue<CCSS_Info>();
                        Program.seq.semi_auto_tank_b.mixing_order_list = new List<CCSS_Info>();
                    }
                    Program.seq.semi_auto_tank_b.mixing_order.Clear();
                    Program.seq.semi_auto_tank_b.mixing_order_list.Clear();
                }
                else if (selected_tank == tank_class.enum_tank_type.TANK_ALL)
                {
                    //main Sequence 사용 queue
                    if (Program.seq.semi_auto_tank_a.mixing_order == null)
                    {
                        Program.seq.semi_auto_tank_a.mixing_order = new System.Collections.Generic.Queue<CCSS_Info>();
                        Program.seq.semi_auto_tank_a.mixing_order_list = new List<CCSS_Info>();
                    }
                    Program.seq.semi_auto_tank_a.mixing_order.Clear();

                    if (Program.seq.semi_auto_tank_b.mixing_order == null)
                    {
                        Program.seq.semi_auto_tank_b.mixing_order = new System.Collections.Generic.Queue<CCSS_Info>();
                        Program.seq.semi_auto_tank_b.mixing_order_list = new List<CCSS_Info>();

                    }
                    Program.seq.semi_auto_tank_b.mixing_order.Clear();
                    Program.seq.semi_auto_tank_b.mixing_order_list.Clear();

                    //main Sequence 사용 queue
                    if (Program.seq.mixing_order == null)
                    {
                        Program.seq.mixing_order = new System.Collections.Generic.Queue<CCSS_Info>();
                        Program.seq.mixing_order_list = new List<CCSS_Info>();
                    }

                    Program.seq.mixing_order.Clear();
                    Program.seq.mixing_order_list.Clear();
                }

                for (int idx_row = 0; idx_row < Program.cg_mixing_step.step_cnt; idx_row++)
                {
                    for (int idx_col = 0; idx_col < Program.cg_mixing_step.ccss_cnt; idx_col++)
                    {
                        //step index 제외로 Columns + 1
                        //dt_mixing_step.Rows[idx_row][idx_col + 1] = Program.cg_mixing_step.mixing_data[(idx_row * Program.cg_mixing_step.ccss_cnt) + (idx_col)];
                        //Not Use 제외
                        if (idx_row != 0)
                        {
                            if (dt_mixing_step.Rows[idx_row][idx_col + 1].ToString() == "1")
                            {
                                //Row값으로 동시 투입 여부 결정
                                Program.cg_mixing_step.ccss_info[idx_col].ccss_row = idx_row;

                                Program.seq.mixing_order.Enqueue(Program.cg_mixing_step.ccss_info[idx_col]);
                                Program.seq.mixing_order_list.Add(Program.cg_mixing_step.ccss_info[idx_col]);

                                Program.seq.semi_auto_tank_a.mixing_order.Enqueue(Program.cg_mixing_step.ccss_info[idx_col]);
                                Program.seq.semi_auto_tank_a.mixing_order_list.Add(Program.cg_mixing_step.ccss_info[idx_col]);

                                Program.seq.semi_auto_tank_b.mixing_order.Enqueue(Program.cg_mixing_step.ccss_info[idx_col]);
                                Program.seq.semi_auto_tank_b.mixing_order_list.Add(Program.cg_mixing_step.ccss_info[idx_col]);
                            }
                        }
                        else if (idx_row == 0)
                        {
                            //Mixing Volume Name 입력
                            if (Program.cg_mixing_step.ccss_info[idx_col].use == true)
                            {
                                //if (idx_col == 0)
                                //{
                                //    volume_name = Program.cg_mixing_step.ccss_info[idx_col].name;
                                //}
                                //else
                                //{
                                //    if (volume_name == "") { volume_name = Program.cg_mixing_step.ccss_info[idx_col].name; }
                                //    else
                                //        volume_name = volume_name + " / " + Program.cg_mixing_step.ccss_info[idx_col].name;
                                //    { }
                                //}

                                //각 Chemical의 Input Volume을 계산하기 위해 분모 계산
                                //Ex) 1:4:20 일때 분모는 1+4+20 = 25
                            }
                        }
                    }
                }


                // Mixing Volume 계산

                volumne_ratio = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Mixing_Rate).ToString();
                volume_ratio_sum = volume_ratio_sum + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Mixing_Rate);

                volumne_ratio = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Mixing_Rate).ToString();
                volume_ratio_sum = volume_ratio_sum + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Mixing_Rate);

                volumne_ratio = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Mixing_Rate).ToString();
                volume_ratio_sum = volume_ratio_sum + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Mixing_Rate);

                volumne_ratio = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Mixing_Rate).ToString();
                volume_ratio_sum = volume_ratio_sum + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Mixing_Rate);

                Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio = "";
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Mixing_Rate) != 0)
                {
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio == "")
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Mixing_Rate).ToString();
                    }
                    else
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio = Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Mixing_Rate).ToString();
                    }
                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                    {
                        if (volume_name == "") { volume_name = "H2O2"; }
                        else { volume_name = volume_name + " / " + "H2O2"; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {
                        if (volume_name == "") { volume_name = "IPA"; }
                        else { volume_name = volume_name + " / " + "IPA"; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                    {
                        if (volume_name == "") { volume_name = "DSP"; }
                        else { volume_name = volume_name + " / " + "DSP"; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                    {
                        if (volume_name == "") { volume_name = "DHF"; }
                        else { volume_name = volume_name + " / " + "DHF"; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                    {
                        if (volume_name == "") { volume_name = "LAL"; }
                        else { volume_name = volume_name + " / " + "LAL"; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (volume_name == "") { volume_name = "H2O2"; }
                        else { volume_name = volume_name + " / " + "H2O2"; }
                    }
                }
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Mixing_Rate) != 0)
                {
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio == "")
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Mixing_Rate).ToString();
                    }
                    else
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio = Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Mixing_Rate).ToString();
                    }
                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                    {
                        if (volume_name == "") { volume_name = "NH4OH"; }
                        else { volume_name = volume_name + " / " + "NH4OH"; }
                    }
                    if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (volume_name == "") { volume_name = "H2SO4"; }
                        else { volume_name = volume_name + " / " + "H2SO4"; }
                    }
                }
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Mixing_Rate) != 0)
                {
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio == "")
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Mixing_Rate).ToString();
                    }
                    else
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio = Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Mixing_Rate).ToString();
                    }
                    if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (volume_name == "") { volume_name = "HF"; }
                        else { volume_name = volume_name + " / " + "HF"; }
                    }
                }
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Mixing_Rate) != 0)
                {
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio == "")
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Mixing_Rate).ToString();
                    }
                    else
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio = Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Mixing_Rate).ToString();
                    }
                    if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.dsp || Program.cg_app_info.eq_type == enum_eq_type.dhf || Program.cg_app_info.eq_type == enum_eq_type.lal || Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (volume_name == "") { volume_name = "DIW"; }
                        else { volume_name = volume_name + " / " + "DIW"; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {

                    }
                }
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].volume_ratio = Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio;
                //Volume H Level 기준으로 계산
                //Ex) H Level이 60L, CCSS1:1 / CCSS2:2 일때 Chem1 투입량은 20L, Chem2 투입략은 40L
                Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_name = volume_name;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].volume_name = volume_name;

                //Tank A Parameter Setting
                //Tank A H Level Volume
                tank_a_volume_h_level = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_H);
                //Tank B H Level Volume
                tank_b_volume_h_level = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_B_Level_H);


                //CCSS1 Input Volume
                Program.tank[(int)tank_class.enum_tank_type.TANK_A].total_volume = 0;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].total_volume = 0;
                tmp_para_Value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Mixing_Rate);
                if (tmp_para_Value == 0)
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume = 0;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume = 0;
                }
                else
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume = (tank_a_volume_h_level / volume_ratio_sum) * tmp_para_Value;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume = (tank_b_volume_h_level / volume_ratio_sum) * tmp_para_Value;

                    if (volume_a == "") { volume_a = Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume.ToString(); }
                    else { volume_a = volume_a + "/" + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume.ToString(); }

                    if (volume_b == "") { volume_b = Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume.ToString(); }
                    else { volume_b = volume_b + "/" + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume.ToString(); }

                }
                //CCSS2 Input Volume
                tmp_para_Value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Mixing_Rate);
                if (tmp_para_Value == 0)
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume = 0;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume = 0;
                }
                else
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume = (tank_a_volume_h_level / volume_ratio_sum) * tmp_para_Value;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume = (tank_b_volume_h_level / volume_ratio_sum) * tmp_para_Value;

                    if (volume_a == "") { volume_a = Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume.ToString(); }
                    else { volume_a = volume_a + "/" + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume.ToString(); }

                    if (volume_b == "") { volume_b = Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume.ToString(); }
                    else { volume_b = volume_b + "/" + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume.ToString(); }
                }

                //CCSS3 Input Volume
                tmp_para_Value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Mixing_Rate);
                if (tmp_para_Value == 0)
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume = 0;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume = 0;
                }
                else
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume = (tank_a_volume_h_level / volume_ratio_sum) * tmp_para_Value;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume = (tank_b_volume_h_level / volume_ratio_sum) * tmp_para_Value;

                    if (volume_a == "") { volume_a = Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume.ToString(); }
                    else { volume_a = volume_a + "/" + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume.ToString(); }

                    if (volume_b == "") { volume_b = Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume.ToString(); }
                    else { volume_b = volume_b + "/" + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume.ToString(); }
                }

                //CCSS4 Input Volume
                tmp_para_Value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Mixing_Rate);
                if (tmp_para_Value == 0)
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss4].input_volume = 0;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss4].input_volume = 0;
                }
                else
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss4].input_volume = (tank_a_volume_h_level / volume_ratio_sum) * tmp_para_Value;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss4].input_volume = (tank_b_volume_h_level / volume_ratio_sum) * tmp_para_Value;

                    if (volume_a == "") { volume_a = Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume.ToString(); }
                    else { volume_a = volume_a + "/" + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume.ToString(); }

                    if (volume_b == "") { volume_b = Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume.ToString(); }
                    else { volume_b = volume_b + "/" + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume.ToString(); }
                }

                Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume = volume_a;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].volume = volume_b;

                Program.tank[(int)tank_class.enum_tank_type.TANK_A].total_volume = Program.tank[(int)tank_class.enum_tank_type.TANK_A].total_volume + (float)Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume;
                Program.tank[(int)tank_class.enum_tank_type.TANK_A].total_volume = Program.tank[(int)tank_class.enum_tank_type.TANK_A].total_volume + (float)Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume;
                Program.tank[(int)tank_class.enum_tank_type.TANK_A].total_volume = Program.tank[(int)tank_class.enum_tank_type.TANK_A].total_volume + (float)Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume;
                Program.tank[(int)tank_class.enum_tank_type.TANK_A].total_volume = Program.tank[(int)tank_class.enum_tank_type.TANK_A].total_volume + (float)Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss4].input_volume;

                Program.tank[(int)tank_class.enum_tank_type.TANK_B].total_volume = Program.tank[(int)tank_class.enum_tank_type.TANK_B].total_volume + (float)Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_volume;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].total_volume = Program.tank[(int)tank_class.enum_tank_type.TANK_B].total_volume + (float)Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss2].input_volume;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].total_volume = Program.tank[(int)tank_class.enum_tank_type.TANK_B].total_volume + (float)Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss3].input_volume;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].total_volume = Program.tank[(int)tank_class.enum_tank_type.TANK_B].total_volume + (float)Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss4].input_volume;
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Setting_Mixing_Order." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);

            }



        }
        private void cmb_mixinglist_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cmb_MixingStep_Load();
        }
        public void Cmb_MixingStep_Load()
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Program.cg_main.path.yaml + Program.cg_main.path.mixing_step_save_folder_name);
            cmb_mixinglist.Properties.Items.Clear();
            foreach (System.IO.FileInfo File in di.GetFiles())
            {
                if (File.Extension.ToLower().CompareTo(".yaml") == 0)
                {
                    cmb_mixinglist.Properties.Items.Add(File.Name.Replace(".yaml", ""));
                }
            }

        }
    }
}