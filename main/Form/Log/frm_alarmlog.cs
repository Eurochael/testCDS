using DevExpress.Utils.Win;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cds
{
    public partial class frm_alarmlog : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public Query_Parameter query_Parameter = new Query_Parameter();

        public Thread thd_load_alarm_log;
        public delegate void Del_Grid_Setting();
        public delegate void Del_Data_Setting();

        public RepositoryItemTextEdit Grid_Txt_Readonly = new RepositoryItemTextEdit();
        public RepositoryItemTextEdit Grid_Txt = new RepositoryItemTextEdit();
        public RepositoryItemComboBox Grid_Combo_type = new RepositoryItemComboBox();

        public DataSet dataset_bind_alarmlog = new DataSet();

        public frm_alarmlog()
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
                        dt_start.EditValue = DateTime.Now.AddDays(-7); dt_end.EditValue = DateTime.Now;
                        btn_search.PerformClick();
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
        private void frm_alarmlog_Load(object sender, EventArgs e)
        {
       
        }
        private void frm_alarmlog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thd_load_alarm_log != null) { thd_load_alarm_log.Abort(); thd_load_alarm_log = null; }
        }
        private void dateEdit1_Popup(object sender, EventArgs e)
        {
            //PopupDateEditForm f = (sender as IPopupControl).PopupWindow as PopupDateEditForm;
            //uc_alarm_Contents f = (sender as IPopupControl).PopupWindow as uc_alarm_Contents;
            //f.Location = new Point(f.Location.X - (dateEdit1.Width), f.Location.Y);
            //foreach (System.Windows.Forms.Control c in f.Calendar.Controls)
            //{
            //    TimeEdit edit = c as TimeEdit;
            //    edit.Properties.EditMask = "HH";
            //    edit.Size = new Size(50, 20);
            //    if (edit != null)
            //    {
            //        edit.EditValue = DateTime.Now;
            //        break;
            //    }

            //}
            //f.ResumeLayout();
        }



        private void btn_search_Click(object sender, EventArgs e)
        {
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
                    if (thd_load_alarm_log != null)
                    {
                        thd_load_alarm_log.Abort(); thd_load_alarm_log = null;
                        thd_load_alarm_log = new Thread(Load_Alarm_Log); thd_load_alarm_log.Start();
                        this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "Alarm Log Loading...", true, frm_process_indicator.enum_call_by.alarmlog);
                    }
                    else
                    {
                        this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "Alarm Log Loading...", true, frm_process_indicator.enum_call_by.alarmlog);
                        thd_load_alarm_log = new Thread(Load_Alarm_Log); thd_load_alarm_log.Start();
                    }
                }
         
                else if (btn_event.Name == "btn_cancel")
                {

                }
            }
            Program.eventlog_form.Insert_Event(this.Name + ".Button Click : " + btn_event.Name, (int)frm_eventlog.enum_event_type.USER_BUTTON, (int)frm_eventlog.enum_occurred_type.USER, true);
            Program.log_md.LogWrite(this.Name + "." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);

        }
        public void Load_Alarm_Log_call_cancel()
        {
            try
            {
                if (thd_load_alarm_log != null) { thd_load_alarm_log.Abort(); thd_load_alarm_log = null; }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Load_Alarm_Log_call_cancel." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
            }
        }
        public void Load_Alarm_Log()
        {
            string result = "", query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {
                //query = "SELECT alarm_logs.alarm_occurred_time, alarm_logs.alarm_cleared_time , alarm_logs.alarm_occurred_by" + System.Environment.NewLine;
                query = "SELECT SUBSTRING(DATE_FORMAT(alarm_logs.alarm_occurred_time, '%Y-%m-%d %T.%f'),1,23) as 'alarm_occurred_time'" + System.Environment.NewLine;
                query += ", SUBSTRING(DATE_FORMAT(alarm_logs.alarm_cleared_time, '%Y-%m-%d %T.%f'),1,23) as 'alarm_cleared_time' , alarm_logs.alarm_occurred_by" + System.Environment.NewLine;
                query += ", alarm_logs.alarm_id, alarm_list.alarm_level, alarm_list.alarm_name, alarm_logs.alarm_remark, alarm_logs.alarm_cleared" + System.Environment.NewLine;
                query += ", alarm_logs.alarm_cleared_by, alarm_list.alarm_visible, alarm_list.alarm_comment" + System.Environment.NewLine;
                query += "FROM alarm_logs JOIN alarm_list ON alarm_logs.alarm_id = alarm_list.alarm_id" + System.Environment.NewLine;
                query += "AND alarm_logs.alarm_id IS NOT NULL" + System.Environment.NewLine;
                query += "AND alarm_list.alarm_visible = '1'" + System.Environment.NewLine;
                // alarm_logs.alarm_id as list 
                if (query_Parameter.start.Year != 1111 || query_Parameter.end.Year != 1111)
                {
                    query += "AND alarm_logs.alarm_occurred_time BETWEEN '" + query_Parameter.start.ToString("yyyy-MM-dd HH:mm:ss") + "'" + " AND " + "'" + query_Parameter.end.ToString("yyyy-MM-dd HH:mm:ss") + "'" + System.Environment.NewLine;
                }
                for (int idx = 0; idx < query_Parameter.token.Count; idx++)
                {
                    if(idx == 0)
                    {
                        query += "And (alarm_list.alarm_comment like '%" + query_Parameter.token[idx] + "%'" + System.Environment.NewLine;
                    }
                    else
                    {
                        query += " OR alarm_list.alarm_comment like '&" + query_Parameter.token[idx] + "%'" + System.Environment.NewLine;
                    }
                    if(idx == query_Parameter.token.Count -1)
                    {
                        query += ")" + System.Environment.NewLine;
                    }
                    
                }


                query += "ORDER BY alarm_logs.alarm_occurred_time DESC" + System.Environment.NewLine;
                Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_local_cds.connection, query, dataset, ref err);

                if (dataset.Tables.Count >= 0 && dataset.Tables[0].Rows.Count >= 0)
                {
                    result = "";
                    //data binding update
                    dataset_bind_alarmlog = dataset.Copy();
                    this.BeginInvoke(new Del_Data_Setting(Data_Setting));
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    result = "Data Not Exist";
                    Program.log_md.LogWrite(this.Name + ".Load_Alarm_Log." + result, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Load_Alarm_Log." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally {
            if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; }
                this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "", false, frm_process_indicator.enum_call_by.none);
            }

        }
        public string Data_Initial_Check()
        {
            string result = "";
            DateTime search_start, search_end;

            try
            {
                grid_alarmlog.SuspendLayout();
                grid_alarmlog.DataSource = null;
                dataset_bind_alarmlog.Tables.Clear();

                search_start = Convert.ToDateTime(dt_start.EditValue);
                if (dt_start.EditValue == null) { query_Parameter.start = Convert.ToDateTime("1111-11-11 00:00:00"); }
                else
                {
                    search_start = Convert.ToDateTime(dt_start.EditValue);
                    query_Parameter.start = new DateTime(search_start.Year, search_start.Month, search_start.Day, search_start.Hour, 00, 00);
                }
                if (dt_end.EditValue == null) { query_Parameter.end = Convert.ToDateTime("1111-11-11 00:00:00"); }
                else
                {
                    search_end = Convert.ToDateTime(dt_end.EditValue);
                    query_Parameter.end = new DateTime(search_end.Year, search_end.Month, search_end.Day, search_end.Hour, 59, 59);
                }
                query_Parameter.token.Clear();
                for (int idx=0; idx < txt_token.Properties.Tokens.Count; idx++)
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
                grid_alarmlog.ResumeLayout();
            }
            return result;
        }
        public void Data_Setting()
        {
            try
            {

                grid_alarmlog.SuspendLayout();
                grid_alarmlog.DataSource = dataset_bind_alarmlog.Tables[0];
                gp_alarm_log.Text = "alarm search result = " + dataset_bind_alarmlog.Tables[0].Rows.Count + "";
                Grid_Setting();
                //grid_alarmlog.DataMember = "DataSET";
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Data_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally 
            {
                grid_alarmlog.ResumeLayout(); 
            }
        }
        public void Grid_Setting()
        {
            try
            {
                gridview_alarmlog.OptionsView.ShowGroupPanel = false;
                gridview_alarmlog.OptionsMenu.EnableGroupPanelMenu = false;
                gridview_alarmlog.OptionsMenu.EnableColumnMenu = false;
                gridview_alarmlog.OptionsCustomization.AllowSort = false;
                gridview_alarmlog.OptionsCustomization.AllowFilter = false;
                gridview_alarmlog.OptionsCustomization.AllowColumnMoving = false;
                gridview_alarmlog.OptionsCustomization.AllowColumnResizing = false;
                gridview_alarmlog.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
                gridview_alarmlog.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                gridview_alarmlog.OptionsView.ColumnAutoWidth = false;
                gridview_alarmlog.GroupPanelText = "";
                for (int i = 0; i <= gridview_alarmlog.Columns.Count - 1; i++)
                {
                    //공통 적용 
                    gridview_alarmlog.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridview_alarmlog.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gridview_alarmlog.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridview_alarmlog.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    if(gridview_alarmlog.Columns[i].Name.IndexOf("alarm_occurred_time") >= 0  )
                    {
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmlog.Columns[i].Visible = true;
                        gridview_alarmlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmlog.Columns[i].Width = 180;
                        gridview_alarmlog.Columns[i].Caption = "Occurred Time";
                        gridview_alarmlog.Columns[i].VisibleIndex = 0;
                    }
                    else if (gridview_alarmlog.Columns[i].Name.IndexOf("alarm_cleared_time") >= 0)
                    {
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmlog.Columns[i].Visible = true;
                        gridview_alarmlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmlog.Columns[i].Width = 180;
                        gridview_alarmlog.Columns[i].Caption = "Cleared_time";
                        gridview_alarmlog.Columns[i].VisibleIndex = 1;
                    }
                    else if (gridview_alarmlog.Columns[i].Name == "col"+ "alarm_occurred_by")
                    {
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmlog.Columns[i].Visible = true;
                        gridview_alarmlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmlog.Columns[i].Width = 100;
                        gridview_alarmlog.Columns[i].Caption = "Occurred By";
                        gridview_alarmlog.Columns[i].VisibleIndex = -1;
                    }
                    else if (gridview_alarmlog.Columns[i].Name == "col" + "alarm_id")
                    {
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmlog.Columns[i].Visible = true;
                        gridview_alarmlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmlog.Columns[i].Width = 90;
                        gridview_alarmlog.Columns[i].Caption = "ID";
                        gridview_alarmlog.Columns[i].VisibleIndex = 2;
                    }
                    else if (gridview_alarmlog.Columns[i].Name == "col" + "alarm_level")
                    {
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmlog.Columns[i].Visible = true;
                        gridview_alarmlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmlog.Columns[i].Width = 60;
                        gridview_alarmlog.Columns[i].Caption = "Level";
                        gridview_alarmlog.Columns[i].VisibleIndex = 3;
                    }
                    else if (gridview_alarmlog.Columns[i].Name == "col" + "alarm_name")
                    {
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmlog.Columns[i].Visible = true;
                        gridview_alarmlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmlog.Columns[i].Width = 250;
                        gridview_alarmlog.Columns[i].Caption = "Name";
                        gridview_alarmlog.Columns[i].VisibleIndex = 4;
                    }
                    else if (gridview_alarmlog.Columns[i].Name == "col" + "alarm_remark")
                    {
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmlog.Columns[i].Visible = true;
                        gridview_alarmlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmlog.Columns[i].Width = 600;
                        gridview_alarmlog.Columns[i].Caption = "Remark";
                        gridview_alarmlog.Columns[i].VisibleIndex = 6;
                    }
                    else if (gridview_alarmlog.Columns[i].Name == "col" + "alarm_comment")
                    {
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmlog.Columns[i].Visible = true;
                        gridview_alarmlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmlog.Columns[i].Width = 400;
                        gridview_alarmlog.Columns[i].Caption = "Comment";
                        gridview_alarmlog.Columns[i].VisibleIndex = 5;
                    }
                    else if (gridview_alarmlog.Columns[i].Name == "col" + "alarm_cleared")
                    {
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmlog.Columns[i].Visible = true;
                        gridview_alarmlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmlog.Columns[i].Width = 60;
                        gridview_alarmlog.Columns[i].Caption = "Cleared";
                        gridview_alarmlog.Columns[i].VisibleIndex = -1;

                    }

                    else if (gridview_alarmlog.Columns[i].Name == "col" + "alarm_cleared_by")
                    {
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_alarmlog.Columns[i].Visible = true;
                        gridview_alarmlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_alarmlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_alarmlog.Columns[i].Width = 80;
                        gridview_alarmlog.Columns[i].Caption = "Cleared by";
                        gridview_alarmlog.Columns[i].VisibleIndex = 6;
                    }
                    else if (gridview_alarmlog.Columns[i].Name == "col" + "alarm_visible")
                    {
                        gridview_alarmlog.Columns[i].Visible = false;
                    }
                    
                }

                }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Grid_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { }

            //private void tokenEdit_CustomDrawTokenText(object sender, TokenEditCustomDrawTokenTextEventArgs e) 
            //{ 
            //    e.Info.PaintAppearance.DrawString(e.Cache, e.Description, e.Info.DescriptionBounds, e.Info.PaintAppearance.Font, Brushes.Blue, e.Info.PaintAppearance.GetStringFormat()); 
            //    e.Handled = true; 
            //}
        }
        private void txt_token_Properties_EditValueChanged(object sender, EventArgs e)
        {
            if(txt_token.EditText == "")
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

    }
}