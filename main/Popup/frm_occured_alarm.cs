using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using static cds.frm_alarm;

namespace cds
{
    public partial class frm_occured_alarm : DevExpress.XtraEditors.XtraForm
    {
        public enum_level most_occured_alarm_level = enum_level.WARNING;
        public int cnt_occured_alarm_total = 0;
        public int cnt_occured_alarm_heavy_total = 0;
        public int cnt_occured_alarm_light_total = 0;
        public int cnt_occured_alarm_warning_total = 0;
        private Color odd_row_color = Color.WhiteSmoke;
        private Color even_row_color = Color.LightGray;
        private Boolean _actived = false;
        private Boolean _initial = false;

        public Boolean req_alarm_reset_by_ctc = false;
        public uint message_no = 0;
        public uint token = 0;
        public Module_Socket.Head rcv_head_alarm_reset = new Module_Socket.Head();

        public Boolean form_forced_close = false;
        //Alarm List 사용 데이터 객체
        public DataSet ds_alarm = new DataSet();
        public DataTable dt_alarm = new DataTable("alarm");

        //Grid에서 사용할 Reposit Item
        public RepositoryItemComboBox rp_cmbbox = new RepositoryItemComboBox();
        public RepositoryItemButtonEdit rp_btn = new RepositoryItemButtonEdit();
        public RepositoryItemMemoEdit rp_memo = new RepositoryItemMemoEdit();
        public RepositoryItemRichTextEdit rp_rich = new RepositoryItemRichTextEdit();
        public RepositoryItemHypertextLabel rp_htl = new RepositoryItemHypertextLabel();

        public delegate void Del_Display_Insert_Alarm(string comment, string note, DateTime Occured_time, int alarm_id, int alarm_level);
        public delegate void Del_Display_Delete_Alarm(DataRow[] data_row);
        public delegate void Del_Data_Setting();

        public Queue<Config_Alarm> q_alarm_event = new Queue<Config_Alarm>();
        public frm_occured_alarm()
        {
            InitializeComponent();
        }
        private void frm_occured_alarm_Load(object sender, EventArgs e)
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            if (_initial == false)
            {
                _initial = true;
                Setting_Initial();
                //Devexpress bug / Group Auto Expands Func Prevent
                //DisableCurrencyManager -> Inteligence No
                gridview_occurred_alarm.DisableCurrencyManager = true;
                Program.occured_alarm_form.Grid_Mapping_Add(); timer_most_alarm_check.Interval = 100; timer_most_alarm_check.Enabled = true;
            }
        }
        private void timer_most_alarm_check_Tick(object sender, EventArgs e)
        {
            Get_Most_Alarm_Level_Count();
        }
        private void frm_occured_alarm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (form_forced_close == true)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                this.Hide();
            }
        }
        private void btn_clear_all_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                if (btn_event.Name == "btn_clear_all")
                {
                    if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                    else
                    {
                        grid_occurred_alarm_list.SuspendLayout();
                        Alarm_Reset_request(enum_cleared_by.USER);
                    }

                }
                else if (btn_event.Name == "btn_expand_all")
                {
                    GridView_Expanded();
                }
                else if (btn_event.Name == "btn_colappsed_all")
                {
                    //Group 2회 진행
                    GridView_Colappsed();
                }
                else if (btn_event.Name == "btn_exit")
                {
                    this.Hide();
                }
                else if (btn_event.Name == "btn_buzzer")
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.BUZZER, false);
                }

            }
            Program.log_md.LogWrite(sender.ToString(), Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }

        public void Setting_Initial()
        {
            Table_Setting();
        }
        public void GridView_Expanded()
        {
            gridview_occurred_alarm.ExpandAllGroups();
        }
        public void GridView_Colappsed()
        {
            gridview_occurred_alarm.CollapseAllGroups();
        }
        public void Grid_Mapping_Add()
        {
            try
            {
                lbl_occured_time.Location = new Point(75, 4);
                lbl_level.Location = new Point(235, 4);
                lbl_no.Location = new Point(310, 4);
                lbl_name.Location = new Point(380, 4);

                grid_occurred_alarm_list.DataSource = null;
                ds_alarm.Tables.Clear();
                ds_alarm.Tables.Add(dt_alarm);

                gridview_occurred_alarm.BeginUpdate();
                grid_occurred_alarm_list.DataSource = ds_alarm.Tables[0];
                grid_occurred_alarm_list.MainView = gridview_occurred_alarm;
                Grid_Setting();
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Grid_Mapping_Add." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                gridview_occurred_alarm.EndUpdate();
            }
        }
        public void Table_Setting()
        {
            try
            {
                dt_alarm.Clear();
                dt_alarm.Dispose();
                dt_alarm = null;
                //Alarm Main 설정
                dt_alarm = new DataTable("alarm");
                dt_alarm.Columns.Clear();
                dt_alarm.Rows.Clear();
                dt_alarm.Columns.Add("CONTENTS", Type.GetType("System.String"));
                dt_alarm.Columns.Add("DATETIME", Type.GetType("System.DateTime"));
                dt_alarm.Columns.Add("INDEX", Type.GetType("System.Int32"));
                dt_alarm.Columns.Add("DATETIMEKEY", Type.GetType("System.String"));
                dt_alarm.Columns.Add("NO", Type.GetType("System.Int32"));
                dt_alarm.Columns.Add("NAME", Type.GetType("System.String"));
                dt_alarm.Columns.Add("VIEW", Type.GetType("System.Boolean"));
                dt_alarm.Columns.Add("LEVEL", Type.GetType("System.Int32"));
                dt_alarm.Columns.Add("REMARK", Type.GetType("System.String"));

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Table_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {

            }
        }
        public int Get_Alarm_Count_By_Level(frm_alarm.enum_level level)
        {
            int count = 0;
            DataRow[] rows = null;
            DataTable dt_alarm_copy = dt_alarm.Copy();
            try
            {
                if (level == frm_alarm.enum_level.WARNING)
                {
                    rows = dt_alarm_copy.Select(); count = rows.Length;
                    most_occured_alarm_level = enum_level.WARNING;
                }
                else if (level == frm_alarm.enum_level.LIGHT)
                {
                    rows = dt_alarm_copy.Select("LEVEL = '1'"); count = rows.Length;
                    most_occured_alarm_level = enum_level.LIGHT;
                }
                else if (level == frm_alarm.enum_level.HEAVY)
                {
                    rows = dt_alarm_copy.Select("LEVEL = '2'"); count = rows.Length;
                    most_occured_alarm_level = enum_level.HEAVY;
                }
                else if (level == frm_alarm.enum_level.ALL)
                {
                    count = dt_alarm_copy.Rows.Count;
                    cnt_occured_alarm_total = count;
                }
                else
                {
                    rows = dt_alarm_copy.Select("LEVEL = '" + level + "'"); count = rows.Length;
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Get_Alarm_Count_By_Level." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {

            }
            return count;

        }
        /// <summary>
        /// Occured Alarm Timer에서 수행 변수
        /// </summary>
        /// <returns></returns>
        public void Get_Most_Alarm_Level_Count()
        {
            DataRow[] rows = null;
            DataTable dt_alarm_copy = dt_alarm.Copy();
            try
            {
                cnt_occured_alarm_total = dt_alarm_copy.Rows.Count;
                rows = dt_alarm_copy.Select("LEVEL = '2'");
                if (rows.Length > 0)
                {
                    cnt_occured_alarm_heavy_total = rows.Length;
                }
                else
                {
                    cnt_occured_alarm_heavy_total = 0;
                }
                rows = dt_alarm_copy.Select("LEVEL = '1'");
                if (rows.Length > 0)
                {
                    cnt_occured_alarm_light_total = rows.Length;
                }
                else
                {
                    cnt_occured_alarm_light_total = 0;
                }
                rows = dt_alarm_copy.Select("LEVEL = '0'");
                if (rows.Length > 0)
                {
                    cnt_occured_alarm_warning_total = rows.Length;
                }
                else
                {
                    cnt_occured_alarm_warning_total = 0;
                }
                if (cnt_occured_alarm_heavy_total > 0)
                {
                    most_occured_alarm_level = enum_level.HEAVY;
                }
                else if (cnt_occured_alarm_light_total > 0)
                {
                    most_occured_alarm_level = enum_level.LIGHT;
                }
                else if (cnt_occured_alarm_warning_total > 0)
                {
                    most_occured_alarm_level = enum_level.WARNING;
                }

                cnt_occured_alarm_total = cnt_occured_alarm_light_total + cnt_occured_alarm_warning_total + cnt_occured_alarm_heavy_total;
                if (cnt_occured_alarm_total == 0) { most_occured_alarm_level = 0; }
                this.Text = "Occured_Alarm" + "(" + "Most Level : " + most_occured_alarm_level + ", Alarm Count : " + cnt_occured_alarm_total + ")";

                if (req_alarm_reset_by_ctc == true)
                {
                    req_alarm_reset_by_ctc = false;
                    Alarm_Reset_request(enum_cleared_by.CTC);
                    Program.schematic_form.DelayAction_CC_Response(1000, new Action(() => { Program.CTC.Alarm_Clear_Response(); ; }));
                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Get_Most_Alarm_Level_Count." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {

            }

        }
        public bool Insert_Alarm(Config_Alarm alarm_object)
        {
            bool result_add_complete = false;
            DataRow[] data_row;
            String contents = "";
            try
            {
                data_row = dt_alarm.Select("INDEX = '" + alarm_object.id + "'");
                if (data_row != null && data_row.Length > 0)
                {
                    result_add_complete = false;
                }
                else
                {
                    contents = ""
                                   + "<size=16><color=128, 128, 128>Comment " + "" + "</color></size=16><br>"
                                   + "<size=12><color=0,0,0>" + alarm_object.comment + "<color></size=12><br>"
                                   //+ "<size=10><color=255,255,255>-----------------------------------------------<color></size=10><br>"
                                   + "<size=16><color=128, 128, 128>Note " + "" + "</color></size=16><br>"
                                   + "<size=12><color=0,0,0>" + alarm_object.remark + "<color></size=12><br>";

                    //DataRow data_row_add = dt_alarm.NewRow();
                    //data_row_add["CONTENTS"] = contents;
                    //data_row_add["DATETIME"] = alarm_object.occurred_time;
                    //data_row_add["INDEX"] = alarm_object.id;
                    //data_row_add["DATETIMEKEY"] = alarm_object.occurred_time.ToString("yyyy-MM-dd HH:mm:ss");
                    //data_row_add["NO"] = alarm_object.id;
                    //data_row_add["NAME"] = alarm_object.name;
                    //data_row_add["VIEW"] = true;
                    //data_row_add["LEVEL"] = alarm_object.level;
                    //data_row_add = new DataRow(contents, alarm_object.occurred_time, alarm_object.id, alarm_object.occurred_time.ToString("yyyy-MM-dd HH:mm:ss"), alarm_object.id, alarm_object.name, true, alarm_object.level);
                    //dt_alarm.Rows.InsertAt(data_row_add, 0);
                    DataRow dt_row_tmp = dt_alarm.NewRow();
                    dt_row_tmp[0] = contents;
                    dt_row_tmp[1] = alarm_object.occurred_time;
                    dt_row_tmp[2] = alarm_object.id;
                    dt_row_tmp[3] = alarm_object.occurred_time.ToString("yyyyMMddHHmmss_") + alarm_object.id;
                    dt_row_tmp[4] = alarm_object.id;
                    dt_row_tmp[5] = alarm_object.name;
                    dt_row_tmp[6] = true;
                    dt_row_tmp[7] = alarm_object.level;
                    dt_row_tmp[8] = alarm_object.remark;
                    dt_alarm.Rows.InsertAt(dt_row_tmp, 0);
                    //dt_alarm.Rows.Add(contents, alarm_object.occurred_time, alarm_object.id, alarm_object.occurred_time.ToString("yyyyMMddHHmmss_") + alarm_object.id, alarm_object.id, alarm_object.name, true, alarm_object.level);
                    result_add_complete = true;
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.BUZZER, true);

                    GridColumn col_dt = gridview_occurred_alarm.Columns["DATETIMEKEY"];
                    gridview_occurred_alarm.SortInfo.ClearAndAddRange(new GridColumnSortInfo[] { new GridColumnSortInfo(col_dt, DevExpress.Data.ColumnSortOrder.Descending), }, 1);
                    if (_actived == false) { GridView_Colappsed(); }
                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Insert_Alarm." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { }
            return result_add_complete;
        }
        public void Alarm_Reset_request(enum_cleared_by cleared_by)
        {
            //Config_Alarm alarm_object
            foreach (DataRow row in dt_alarm.Rows)
            {
                Program.alarm_list.Alarm_Reset_Request_by_ID((int)row["NO"], cleared_by);
            }

        }
        public void Display_Insert_Alarm(string name, string comment, string note, DateTime Occured_time, int alarm_id, int alarm_level)
        {
            String contents = "";
            DataRow datarow = null;
            try
            {
                contents = ""
                                       + "<size=16><color=128, 128, 128>Comment " + "" + "</color></size=16><br>"
                                       + "<size=12><color=0,0,0>" + comment + "<color></size=12><br>"
                                       //+ "<size=10><color=255,255,255>-----------------------------------------------<color></size=10><br>"
                                       + "<size=16><color=128, 128, 128>Note " + "" + "</color></size=16><br>"
                                       + "<size=12><color=0,0,0>" + note + "<color></size=12><br>";
                //dt_alarm.Rows.Add(contents, Occured_time, alarm_id, Occured_time.ToString("yyyy-MM-dd HH:mm:ss"), alarm_id, name, true, alarm_level);
                if (_actived == false) { }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Display_Insert_Alarm." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { }
        }
        public bool Delete_Alarm(Config_Alarm alarm_object)
        {
            DataRow[] data_row;
            bool result_remove_complete = false;
            try
            {
                data_row = dt_alarm.Select("INDEX = '" + alarm_object.id + "'");
                if (data_row != null && data_row.Length > 0)
                {
                    foreach (DataRow row in data_row)
                    {
                        dt_alarm.Rows.Remove(row);
                        result_remove_complete = true;
                    }
                }
                else
                {
                    result_remove_complete = false;
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Delete_Alarm." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { }
            return result_remove_complete;
        }
        public void Display_Delete_Alarm(DataRow[] data_row)
        {
            try
            {
                if (data_row != null && data_row.Length > 0)
                {
                    dt_alarm.Rows.Remove(data_row[0]);

                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Display_Delete_Alarm." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { }
        }
        public void Grid_Setting()
        {
            try
            {
                rp_htl.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
                rp_htl.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                rp_htl.Appearance.Options.UseTextOptions = true;
                rp_htl.Appearance.BackColor = Color.Transparent;
                rp_htl.AllowFocused = false;
                //rp_htl.Appearance.
                //rp_htl.Appearance.BackColor2 = Color.Black;
                if (grid_occurred_alarm_list.RepositoryItems.IndexOf(rp_htl) < 0)
                {
                    grid_occurred_alarm_list.RepositoryItems.Add(rp_htl);
                }
                //gridControl1.RepositoryItems.Clear();

                gridview_occurred_alarm.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
                gridview_occurred_alarm.ClearSelection();
                gridview_occurred_alarm.FocusRectStyle = DrawFocusRectStyle.None;

                gridview_occurred_alarm.GroupFormat = "[#image]{1} {2}";
                gridview_occurred_alarm.OptionsMenu.EnableGroupPanelMenu = false;
                gridview_occurred_alarm.OptionsMenu.EnableColumnMenu = false;

                gridview_occurred_alarm.OptionsCustomization.AllowSort = false;
                gridview_occurred_alarm.OptionsCustomization.AllowFilter = false;
                gridview_occurred_alarm.OptionsCustomization.AllowColumnMoving = false;
                gridview_occurred_alarm.OptionsCustomization.AllowColumnResizing = false;

                gridview_occurred_alarm.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
                gridview_occurred_alarm.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                gridview_occurred_alarm.OptionsView.GroupDrawMode = DevExpress.XtraGrid.Views.Grid.GroupDrawMode.Default;
                gridview_occurred_alarm.OptionsView.RowAutoHeight = true;
                gridview_occurred_alarm.OptionsView.ShowGroupedColumns = false;
                gridview_occurred_alarm.OptionsView.ShowGroupPanel = false;
                gridview_occurred_alarm.OptionsView.ShowColumnHeaders = false;
                gridview_occurred_alarm.OptionsView.ShowIndicator = false;
                gridview_occurred_alarm.OptionsView.ShowGroupExpandCollapseButtons = true;

                gridview_occurred_alarm.OptionsView.ShowHorizontalLines = DefaultBoolean.False;
                gridview_occurred_alarm.OptionsView.ShowVerticalLines = DefaultBoolean.False;

                gridview_occurred_alarm.OptionsView.AllowHtmlDrawGroups = true;
                gridview_occurred_alarm.OptionsSelection.EnableAppearanceFocusedRow = false;
                gridview_occurred_alarm.OptionsSelection.EnableAppearanceHideSelection = false;
                gridview_occurred_alarm.OptionsSelection.EnableAppearanceFocusedCell = false;

                gridview_occurred_alarm.OptionsBehavior.KeepGroupExpandedOnSorting = false;

                gridview_occurred_alarm.GroupPanelText = "alarm";
                
                
                //Grid Group화
                gridview_occurred_alarm.Columns["DATETIMEKEY"].GroupIndex = 0;
                gridview_occurred_alarm.OptionsBehavior.KeepFocusedRowOnUpdate = false;

                //GridColumn col_dt = gridview_occurred_alarm.Columns["DATETIME"];
                //GridColumn col_id = gridview_occurred_alarm.Columns["INDEX"];

                //gridview_occurred_alarm.SortInfo.ClearAndAddRange(new GridColumnSortInfo[] {
                //new GridColumnSortInfo(col_id, DevExpress.Data.ColumnSortOrder.Ascending),
                //new GridColumnSortInfo(col_dt, DevExpress.Data.ColumnSortOrder.Ascending),
                //}, 2);
                for (int i = 0; i <= gridview_occurred_alarm.Columns.Count - 1; i++)
                {
                    if (gridview_occurred_alarm.Columns[i].Name == "col" + "CONTENTS")
                    {
                        gridview_occurred_alarm.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_occurred_alarm.Columns[i].Visible = true;
                        gridview_occurred_alarm.Columns[i].ColumnEdit = rp_htl;
                        gridview_occurred_alarm.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_occurred_alarm.Columns[i].Width = 140;
                        gridview_occurred_alarm.Columns[i].Caption = "CONTENTS";
                    }
                    else
                    {
                        gridview_occurred_alarm.Columns[i].Visible = false;
                    }

                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Grid_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { }
        }
        private void gridView1_CustomDrawGroupRow_1(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            //Console.WriteLine(e.RowHandle);
            //Focus rowhandle backcolor change group row <0
            if (e.RowHandle < 0 && e.RowHandle % 2 == -1)
            {
                e.Appearance.BackColor = odd_row_color;
            }
            else
            {
                e.Appearance.BackColor = even_row_color;
            }

            if (e.RowHandle < 0 && gridview_occurred_alarm.FocusedRowHandle == e.RowHandle)
            {
                // e.Appearance.ForeColor = Color.Red;
            }

            //GridGroupRowInfo rowInfo = e.Info as GridGroupRowInfo;
            //GridColumnInfoArgs cInfo = rowInfo.ViewInfo.ColumnsInfo[rowInfo.Column];
            //int indent = 50;
            //if (cInfo != null)
            //    rowInfo.ButtonBounds = new Rectangle(cInfo.Bounds.X + indent, rowInfo.ButtonBounds.Y, rowInfo.ButtonBounds.Width, rowInfo.ButtonBounds.Height);
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //Console.WriteLine("DATA ROW" + e.RowHandle);
            if (e.RowHandle >= 0 && e.RowHandle % 2 == 0)
            {
                e.Appearance.BackColor = odd_row_color;
            }
            else
            {
                e.Appearance.BackColor = even_row_color;
            }
        }

        private void gridView1_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null) return;
            try
            {
                if (e.Column.FieldName == "DATETIMEKEY")
                {
                    if (view.GetRowCellValue(view.GetChildRowHandle(e.GroupRowHandle, 0), "NAME") != null)
                    {
                        double rowValue = Convert.ToDouble(view.GetRowCellValue(view.GetChildRowHandle(e.GroupRowHandle, 0), "INDEX"));
                        //Convert.ToDouble(view.GetGroupRowValue(e.GroupRowHandle, e.Column));
                        string name = view.GetRowCellValue(view.GetChildRowHandle(e.GroupRowHandle, 0), "NAME").ToString();
                        string occurred_time = Convert.ToDateTime(view.GetRowCellValue(view.GetChildRowHandle(e.GroupRowHandle, 0), "DATETIME")).ToString("yyyy-MM-dd HH:mm:ss");
                        int level = Convert.ToInt32(view.GetRowCellValue(view.GetChildRowHandle(e.GroupRowHandle, 0), "LEVEL"));
                        //<color=128, 128, 128>
                        e.DisplayText = "<color=0, 0, 0><size=12>" + occurred_time + "        " + level + "        " + string.Format("{0:0000}", rowValue) + "        " + name + "";
                    }
                    else
                    {
                        Console.WriteLine("Group Null :" + view.GetChildRowHandle(e.GroupRowHandle, 0));
                    }

                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".gridView1_CustomColumnDisplayText." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {

            }

        }

        private void gridView1_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            ColumnView view = sender as ColumnView;
            string view_name = view.GetListSourceRowCellValue(e.ListSourceRow, "VIEW").ToString();
            if (view_name == "False")
            {
                // Make the current row visible.
                e.Visible = false;
                // Prevent default processing, so the row will be visible 
                // regardless of the view's filter.
                e.Handled = true;
            }
            if (view_name == "True")
            {
                // Make the current row visible.
                e.Visible = true;
                // Prevent default processing, so the row will be visible 
                // regardless of the view's filter.
                e.Handled = true;
            }
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
                return;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.HitTest == GridHitTest.RowIndicator)
            {//그룹 버튼을 클릭했는지 확인
                //MessageBox.Show(string.Format("DoubleClick on row indicator, row #{0}", info.RowHandle));
            }
            if (info.InRow || info.InRowCell)
            {
                if (info.InRowCell != false)
                {
                    //gridView1.CollapseGroupRow(-1 * (info.RowHandle + 1));
                }
                //string colCaption = info.Column == null ? "N/A" : info.Column.GetCaption();
                //MessageBox.Show(string.Format("DoubleClick on row: {0}, column: {1}.", info.RowHandle, colCaption));
            }
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            odd_row_color = Color.WhiteSmoke;
            even_row_color = Color.LightGray;

            lbl_occured_time.Location = new Point(75, 4);
            lbl_level.Location = new Point(235, 4);
            lbl_no.Location = new Point(310, 4);
            lbl_name.Location = new Point(380, 4);
        }

        private void gridview_occurred_alarm_GroupRowExpanding(object sender, RowAllowEventArgs e)
        {
            //Console.WriteLine(e.RowHandle);
        }

        private void frm_occured_alarm_Activated(object sender, EventArgs e)
        {
            _actived = true;
        }

        private void frm_occured_alarm_Deactivate(object sender, EventArgs e)
        {
            this.Hide();
            _actived = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridview_occurred_alarm.BeginDataUpdate();
            try
            {
                gridview_occurred_alarm.ClearSorting();
                gridview_occurred_alarm.Columns["DATETIME"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            }
            finally
            {
                gridview_occurred_alarm.EndDataUpdate();

            }
            return;
            //dt_alarm.DefaultView.Sort = "DATETIME ASC";
            GridColumn col_dt = gridview_occurred_alarm.Columns["DATETIME"];
            GridColumn col_id = gridview_occurred_alarm.Columns["INDEX"];

            gridview_occurred_alarm.SortInfo.ClearAndAddRange(new GridColumnSortInfo[] {
            new GridColumnSortInfo(col_id, DevExpress.Data.ColumnSortOrder.Ascending),
            new GridColumnSortInfo(col_dt, DevExpress.Data.ColumnSortOrder.Ascending),
            }, 2);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            GridColumn col_dt = gridview_occurred_alarm.Columns["DATETIMEKEY"];
            gridview_occurred_alarm.SortInfo.ClearAndAddRange(new GridColumnSortInfo[] { new GridColumnSortInfo(col_dt, DevExpress.Data.ColumnSortOrder.Descending), }, 1);
        }

    }
}