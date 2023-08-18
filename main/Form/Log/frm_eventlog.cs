using DevExpress.XtraEditors.Repository;
using System;
using System.Data;
using System.Threading;

namespace cds
{
    public partial class frm_eventlog : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public Query_Parameter query_Parameter = new Query_Parameter();

        public Thread thd_load_event_log;
        public delegate void Del_Grid_Setting();
        public delegate void Del_Data_Setting();

        public RepositoryItemTextEdit Grid_Txt_Readonly = new RepositoryItemTextEdit();
        public RepositoryItemTextEdit Grid_Txt = new RepositoryItemTextEdit();
        public RepositoryItemComboBox Grid_Combo_type = new RepositoryItemComboBox();

        public DataSet dataset_bind_alarmlog = new DataSet();
        public frm_eventlog()
        {
            InitializeComponent();
        }

        public enum enum_event_type
        {
            NONE = 0,
            Login = 10,
            Logout = 20,
            Manual_To_Auto = 30,
            Auto_To_Manual = 40,
            SEMI_AUTO_DRAIN_START = 100,
            SEMI_AUTO_DIW_FLUSH_START = 101,
            SEMI_AUTO_CHEM_FLUSH_START = 102,
            SEMI_AUTO_AUTO_FLUSH_START = 103,
            SEMI_AUTO_DIW_FLUSH_AND_SUPPLY_START = 104,
            SEMI_AUTO_CHEM_FLUSH_AND_SUPPLY_START = 105,
            SEMI_AUTO_CALIBRATION = 106, // LAL Concentration Calibration
            CONCENTRATION = 300,
            CTC = 1000,
            USER_BUTTON = 2000,
            PARAMETER_CHANGED = 2500,
            ALARM_CHANGED = 2501,
            SYSTEM = 3000,
            SEQ = 5000,
            SEQ_SUPPLY = 5001,
            SEQ_SUB = 5002,
        }
        public enum enum_occurred_type
        {
            NONE = 0,
            DEFAULT = 1,
            SYSTEM = 2,
            PROCESS = 3,
            USER = 4,

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
                    if (thd_load_event_log != null)
                    {
                        thd_load_event_log.Abort(); thd_load_event_log = null;
                        thd_load_event_log = new Thread(Load_Event_Log); thd_load_event_log.Start();
                        this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "Event Log Loading...", true, frm_process_indicator.enum_call_by.evnetlog);
                    }
                    else
                    {
                        this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "Event Log Loading...", true, frm_process_indicator.enum_call_by.evnetlog);
                        thd_load_event_log = new Thread(Load_Event_Log); thd_load_event_log.Start();
                    }
                }
                else if (btn_event.Name == "btn_event")
                {
                    Insert_Event("User Test", 0, 0, true);
                }
                else if (btn_event.Name == "btn_cancel")
                {

                }
            }
            Program.eventlog_form.Insert_Event(this.Name + ".Button Click : " + btn_event.Name, (int)frm_eventlog.enum_event_type.USER_BUTTON, (int)frm_eventlog.enum_occurred_type.USER, true);
            Program.log_md.LogWrite(this.Name + "." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }
        public void Load_Event_Log()
        {
            string result = "", query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {
                //query = "SELECT event_occurred_time, event_occurred_by, event_id" + System.Environment.NewLine;
                query = "SELECT SUBSTRING(DATE_FORMAT(event_occurred_time, '%Y-%m-%d %T.%f'),1,23) as 'event_occurred_time'" + System.Environment.NewLine;
                query += ", event_occurred_by" + System.Environment.NewLine;
                query += ", event_id" + System.Environment.NewLine;
                query += ", text" + System.Environment.NewLine;
                query += "FROM event_logs" + System.Environment.NewLine;
                query += "WHERE event_id IS NOT NULL" + System.Environment.NewLine;
                query += "AND event_visible = '1'" + System.Environment.NewLine;
                // alarm_logs.alarm_id as list 
                if (query_Parameter.start.Year != 1111 || query_Parameter.end.Year != 1111)
                {
                    query += "AND event_occurred_time BETWEEN '" + query_Parameter.start.ToString("yyyy-MM-dd HH:mm:ss") + "'" + " AND " + "'" + query_Parameter.end.ToString("yyyy-MM-dd HH:mm:ss") + "'" + System.Environment.NewLine;
                }
                for (int idx = 0; idx < query_Parameter.token.Count; idx++)
                {
                    if (idx == 0)
                    {
                        query += "And (text like '%" + query_Parameter.token[idx] + "%'" + System.Environment.NewLine;
                    }
                    else
                    {
                        query += " OR text like '&" + query_Parameter.token[idx] + "%'" + System.Environment.NewLine;
                    }
                    if (idx == query_Parameter.token.Count - 1)
                    {
                        query += ")" + System.Environment.NewLine;
                    }

                }


                query += "ORDER BY event_occurred_time DESC" + System.Environment.NewLine;
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
                    Program.log_md.LogWrite(this.Name + ".Load_Event_Log." + result, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Load_Event_Log." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; }
                this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "", false, frm_process_indicator.enum_call_by.none);
            }

        }
        public void Insert_Event(string text, int id, int occurred_by, bool visible)
        {
            int result = 0;
            string query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {
                query = "INSERT INTO event_logs(event_occurred_time, event_occurred_by, event_id, text, event_visible)";
                query += " VALUES (";
                query += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                query += ",'" + occurred_by + "'";
                query += ",'" + id + "'";
                query += ",'" + text + "'";
                query += "," + visible + "";
                query += ")";

                result = Program.database_md.MariaDB_MainQuery(Program.cg_main.db_local_cds.connection, query, ref err);
                if (result == 1)
                {
                    //동일 데이타 업데이트 시 return 0
                    //Insert 또는 변동 데이터 업데이트 시 return 1
                }
                else
                {
                    Program.log_md.LogWrite(this.Name + ".Insert_Event." + result + "/" + query, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Insert_Event." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return;
        }
        public string Data_Initial_Check()
        {
            string result = "";
            DateTime search_start, search_end;
            try
            {
                grid_eventlog.SuspendLayout();
                grid_eventlog.DataSource = null;
                dataset_bind_alarmlog.Tables.Clear();

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
                grid_eventlog.ResumeLayout();
            }
            return result;
        }
        public void Data_Setting()
        {
            try
            {

                grid_eventlog.SuspendLayout();
                grid_eventlog.DataSource = dataset_bind_alarmlog.Tables[0];
                gp_alarm_log.Text = "event search result = " + dataset_bind_alarmlog.Tables[0].Rows.Count + "";
                Grid_Setting();
                //grid_alarmlog.DataMember = "DataSET";
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Data_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                grid_eventlog.ResumeLayout();
            }
        }
        public void Grid_Setting()
        {
            try
            {
                gridview_evntlog.OptionsView.ShowGroupPanel = false;
                gridview_evntlog.OptionsMenu.EnableGroupPanelMenu = false;
                gridview_evntlog.OptionsMenu.EnableColumnMenu = false;
                gridview_evntlog.OptionsCustomization.AllowSort = false;
                gridview_evntlog.OptionsCustomization.AllowFilter = false;
                gridview_evntlog.OptionsCustomization.AllowColumnMoving = false;
                gridview_evntlog.OptionsCustomization.AllowColumnResizing = false;
                gridview_evntlog.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
                gridview_evntlog.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                gridview_evntlog.OptionsView.ColumnAutoWidth = false;
                gridview_evntlog.GroupPanelText = "";
                for (int i = 0; i <= gridview_evntlog.Columns.Count - 1; i++)
                {
                    //공통 적용 
                    gridview_evntlog.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridview_evntlog.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    gridview_evntlog.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridview_evntlog.Columns[i].AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    if (gridview_evntlog.Columns[i].Name.IndexOf("event_occurred_time") >= 0)
                    {
                        gridview_evntlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_evntlog.Columns[i].Visible = true;
                        gridview_evntlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_evntlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_evntlog.Columns[i].Width = 200;
                        gridview_evntlog.Columns[i].Caption = "Occurred Time";
                    }
                    else if (gridview_evntlog.Columns[i].Name == "col" + "event_occurred_by")
                    {
                        gridview_evntlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_evntlog.Columns[i].Visible = true;
                        gridview_evntlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_evntlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_evntlog.Columns[i].Width = 100;
                        gridview_evntlog.Columns[i].Caption = "Occurred By";
                    }
                    else if (gridview_evntlog.Columns[i].Name == "col" + "event_id")
                    {
                        gridview_evntlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_evntlog.Columns[i].Visible = true;
                        gridview_evntlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_evntlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_evntlog.Columns[i].Width = 100;
                        gridview_evntlog.Columns[i].Caption = "ID";
                    }
                    else if (gridview_evntlog.Columns[i].Name == "col" + "text")
                    {
                        gridview_evntlog.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gridview_evntlog.Columns[i].Visible = true;
                        gridview_evntlog.Columns[i].ColumnEdit = Grid_Txt_Readonly;
                        gridview_evntlog.Columns[i].OptionsColumn.AllowEdit = false;
                        gridview_evntlog.Columns[i].Width = 600;
                        gridview_evntlog.Columns[i].Caption = "Text";
                    }

                    else if (gridview_evntlog.Columns[i].Name == "col" + "event_visible")
                    {
                        gridview_evntlog.Columns[i].Visible = false;
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
    }
}