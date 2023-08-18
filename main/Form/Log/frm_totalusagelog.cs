using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;

namespace cds
{
    public partial class frm_totalusagelog : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public Thread thd_load_total_usage_log;
        public delegate void Del_Chart_Setting(DateTime start, DateTime end);
        public delegate void Del_Data_Setting(object seriesdata, int series_idx);
        public delegate void Del_Data_Setting_By_Series_Alone(int idx_series, SeriesPoint seriesdata);

        public frm_totalusagelog()
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
                if (btn_event.Name == "btn_search")
                {
                    //Load_TrendData();
                    Data_Initial_Check();
                    if (thd_load_total_usage_log != null)
                    {
                        thd_load_total_usage_log.Abort(); thd_load_total_usage_log = null;
                        thd_load_total_usage_log = new Thread(Load_Total_Usage_Log); thd_load_total_usage_log.Start();
                        this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "Total Usage Data Loading...", true, frm_process_indicator.enum_call_by.totalusagelog);
                    }
                    else
                    {
                        this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "Total Usage Data Loading...", true, frm_process_indicator.enum_call_by.totalusagelog);
                        thd_load_total_usage_log = new Thread(Load_Total_Usage_Log); thd_load_total_usage_log.Start();
                    }
                }

                else if (btn_event.Name == "btn_cancel")
                {

                }
            }
            Program.eventlog_form.Insert_Event(this.Name + ".Button Click : " + btn_event.Name, (int)frm_eventlog.enum_event_type.USER_BUTTON, (int)frm_eventlog.enum_occurred_type.USER, true);
            Program.log_md.LogWrite(this.Name + "." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }
        public void Data_Initial_Check()
        {
            Chart_total_usage_chart.Series.Clear();
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                Chart_total_usage_chart.Series.Add("H2O2", DevExpress.XtraCharts.ViewType.Bar);
                Chart_total_usage_chart.Series.Add("NH4OH", DevExpress.XtraCharts.ViewType.Bar);
                Chart_total_usage_chart.Series.Add("CCSS3", DevExpress.XtraCharts.ViewType.Bar);
                Chart_total_usage_chart.Series.Add("DIW", DevExpress.XtraCharts.ViewType.Bar);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                Chart_total_usage_chart.Series.Add("IPA", DevExpress.XtraCharts.ViewType.Bar);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                Chart_total_usage_chart.Series.Add("DSP", DevExpress.XtraCharts.ViewType.Bar);
                Chart_total_usage_chart.Series.Add("DIW", DevExpress.XtraCharts.ViewType.Bar);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                Chart_total_usage_chart.Series.Add("DHF", DevExpress.XtraCharts.ViewType.Bar);
                Chart_total_usage_chart.Series.Add("DIW", DevExpress.XtraCharts.ViewType.Bar);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                Chart_total_usage_chart.Series.Add("LAL", DevExpress.XtraCharts.ViewType.Bar);
                Chart_total_usage_chart.Series.Add("DIW", DevExpress.XtraCharts.ViewType.Bar);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                Chart_total_usage_chart.Series.Add("H2O2", DevExpress.XtraCharts.ViewType.Bar);
                Chart_total_usage_chart.Series.Add("H2SO4", DevExpress.XtraCharts.ViewType.Bar);
                Chart_total_usage_chart.Series.Add("DHF", DevExpress.XtraCharts.ViewType.Bar);
                Chart_total_usage_chart.Series.Add("DIW", DevExpress.XtraCharts.ViewType.Bar);
            }
        }
        public void Load_Total_Usage_Log()
        {
            string result, err = "";
            DateTime dt_start = DateTime.Now; //Convert.ToDateTime("2022-03-03 18:00:00");//
            DateTime dt_search = DateTime.Now;
            object series_list;
            string query = "";
            SeriesPoint seriesdata;
            DataSet ds_total_usage = new DataSet();
            int real_idx = 0;
            bool usage_ok = false;
            try
            {
                //CCSS 단위로 적산량 통계

                //CCSS 개수만큼 생성

                if (ds_total_usage != null) { ds_total_usage.Clear(); ds_total_usage.Dispose(); ds_total_usage = new DataSet(); }
                query = "SELECT DATE(`totalusage_saved_time`) AS `date`,sum(`totalusage_value`) AS 'sum'" + System.Environment.NewLine;
                query += "FROM totalusage_logs" + System.Environment.NewLine;
                query += " GROUP BY `date`;" + System.Environment.NewLine;

                Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_local_cds.connection, query, ds_total_usage, ref err);
                //날짜의 시작과 끝을 가져온다
                dt_start = Convert.ToDateTime(ds_total_usage.Tables[0].Rows[0]["date"]);
                dt_search = Convert.ToDateTime(ds_total_usage.Tables[0].Rows[ds_total_usage.Tables[0].Rows.Count - 1]["date"]);

                //for (int idx = 0; idx < Program.cg_mixing_step.ccss_cnt; idx++)
                real_idx = -1;
                for (int idx = 0; idx < 4; idx++)
                {
                    //dataset 초기화
                    if (ds_total_usage != null) { ds_total_usage.Clear(); ds_total_usage.Dispose(); ds_total_usage = new DataSet(); }

                    if (rbtn_day.Checked == true)
                    {
                        query = "SELECT DATE(`totalusage_saved_time`) AS `date`,sum(`totalusage_value`) AS 'sum'" + System.Environment.NewLine;
                        query += "FROM totalusage_logs" + System.Environment.NewLine;
                        query += "WHERE totalusage_index_ccss = '" + idx + "'" + System.Environment.NewLine;
                        query += " GROUP BY `date`;" + System.Environment.NewLine;
                    }
                    else if (rbtn_week.Checked == true)
                    {
                        query = "SELECT DATE_FORMAT(DATE_SUB(`totalusage_saved_time`, INTERVAL (DAYOFWEEK(`totalusage_saved_time`)-1) DAY), '%Y/%m/%d') as start" + System.Environment.NewLine;
                        query += ",DATE_FORMAT(DATE_SUB(`totalusage_saved_time`, INTERVAL (DAYOFWEEK(`totalusage_saved_time`)-7) DAY), '%Y/%m/%d') as end" + System.Environment.NewLine;
                        query += ",DATE_FORMAT(`totalusage_saved_time`, '%Y%U') AS date_none" + System.Environment.NewLine;
                        query += ",DATE_FORMAT(DATE_SUB(`totalusage_saved_time`, INTERVAL (DAYOFWEEK(`totalusage_saved_time`)-1) DAY), '%Y/%m/%d') as date" + System.Environment.NewLine;
                        query += ",sum(`totalusage_value`) AS 'sum'" + System.Environment.NewLine;
                        query += "FROM totalusage_logs" + System.Environment.NewLine;
                        query += "WHERE totalusage_index_ccss = '" + idx + "'" + System.Environment.NewLine;
                        query += " GROUP BY `date`;" + System.Environment.NewLine;
                    }
                    else if (rbtn_month.Checked == true)
                    {
                        query = "SELECT MONTH(`totalusage_saved_time`) AS `Month`,sum(`totalusage_value`) AS 'sum'" + System.Environment.NewLine;
                        query += ",DATE_FORMAT(`totalusage_saved_time`, '%Y/%m/%d') as date" + System.Environment.NewLine;
                        query += "FROM totalusage_logs" + System.Environment.NewLine;
                        query += "WHERE totalusage_index_ccss = '" + idx + "'" + System.Environment.NewLine;
                        query += " GROUP BY `Month`;" + System.Environment.NewLine;
                    }
                    Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_local_cds.connection, query, ds_total_usage, ref err);



                    if (ds_total_usage.Tables.Count >= 0 && ds_total_usage.Tables[0].Rows.Count >= 0)
                    {
                        result = "";
                        usage_ok = false;
                        if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                        {
                            if (idx == 0) { usage_ok = true; }
                            else if (idx == 1) { usage_ok = true; }
                            else if (idx == 3) { usage_ok = true; }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                        {
                            if (idx == 0) { usage_ok = true; }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                        {
                            if (idx == 0) { usage_ok = true; }
                            else if (idx == 3) { usage_ok = true; }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                        {
                            if (idx == 0) { usage_ok = true; }
                            else if (idx == 3) { usage_ok = true; }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                        {
                            if (idx == 0) { usage_ok = true; }
                            else if (idx == 3) { usage_ok = true; }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                        {
                            if (idx == 0) { usage_ok = true; }
                            else if (idx == 1) { usage_ok = true; }
                            else if (idx == 2) { usage_ok = true; }
                            else if (idx == 3) { usage_ok = true; }
                        }
                        if (usage_ok == true)
                        {
                            real_idx = real_idx + 1;
                            for (int idx2 = 0; idx2 < ds_total_usage.Tables[0].Rows.Count; idx2++)
                            {

                                try
                                {
                                    seriesdata = new SeriesPoint(Convert.ToDateTime(ds_total_usage.Tables[0].Rows[idx2]["date"]), Convert.ToDouble(ds_total_usage.Tables[0].Rows[idx2]["sum"]));
                                    this.BeginInvoke(new Del_Data_Setting_By_Series_Alone(Data_Setting_By_Series_Alone), real_idx, seriesdata);
                                }
                                catch (Exception ex)
                                {

                                }
                                System.Threading.Thread.Sleep(5);
                            }
                        }
                        System.Threading.Thread.Sleep(200);

                    }
                    else
                    {
                        result = "Data Not Exist";
                        Program.log_md.LogWrite(this.Name + ".Load_Event_Log." + result, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                    }

                }

                System.Threading.Thread.Sleep(200);
                this.BeginInvoke(new Del_Chart_Setting(Chart_Setting), dt_start, dt_search);
                System.Threading.Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Load_Chart_Log." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                if (ds_total_usage != null) { ds_total_usage.Clear(); ds_total_usage.Dispose(); ds_total_usage = null; }
                this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "", false, frm_process_indicator.enum_call_by.trendlog);
            }
        }
        public void Data_Setting(object seriesdata, int series_idx)
        {
            try
            {
                if (seriesdata is SeriesPoint[][])
                {
                    for (int idx = 0; idx < (seriesdata as SeriesPoint[][]).Length - 1; idx++)
                    {
                        if ((seriesdata as SeriesPoint[][])[idx] != null)
                        {
                            Chart_total_usage_chart.Series[series_idx].Points.AddRange((seriesdata as SeriesPoint[][])[idx]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Data_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
            }
        }
        public void Data_Setting_By_Series_Alone(int idx_series, SeriesPoint seriesdata)
        {
            try
            {
                if (seriesdata is SeriesPoint)
                {
                    if (seriesdata != null)
                    {
                        Chart_total_usage_chart.Series[idx_series].Points.Add(seriesdata);
                    }
                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Data_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
            }
        }
        private int GetWeekNumber(DateEdit edit)
        {
            DateTimeFormatInfo format = DevExpress.Data.Mask.DateTimeMaskManager.GetGoodCalendarDateTimeFormatInfo(CultureInfo.CurrentCulture);
            return format.Calendar.GetWeekOfYear(edit.DateTime, format.CalendarWeekRule, format.FirstDayOfWeek);
        }

        public void Chart_Setting(DateTime start, DateTime end)
        {
            string title = "";
            double sum = 0;
            double[] seires_value;
            bool usage_ok = false;
            int real_idx = 0;
            try
            {
                gp_total_usage_log.Text = "Total Usage Search Result - " + start.ToString("MM-dd") + " ~ " + end.ToString("MM-dd");
                Chart_total_usage_chart.SuspendLayout();

                (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.WholeRange.SetMinMaxValues(start, end);
                (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.WholeRange.Auto = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.Visible = true;
                //(Chart_total_usage_chart.Diagram as XYDiagram).AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;

                if (rbtn_day.Checked == true)
                {
                    (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.Label.TextPattern = "{A:MM-dd}";
                    (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
                    (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;

                }
                else if (rbtn_week.Checked == true)
                {
                    (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.Label.TextPattern = "{A:MM-dd}";
                    (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
                    (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Week;
                }
                else if (rbtn_month.Checked == true)
                {
                    (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.Label.TextPattern = "{A:MM-dd}";
                    (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;
                    (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Month;
                }


                (Chart_total_usage_chart.Diagram as XYDiagram).AxisY.WholeRange.Auto = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).AxisY.VisualRange.Auto = true;

                (Chart_total_usage_chart.Diagram as XYDiagram).AxisY.Range.Auto = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).AxisY.Range.ScrollingRange.Auto = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.Range.Auto = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).AxisX.Range.ScrollingRange.Auto = true;

                (Chart_total_usage_chart.Diagram as XYDiagram).EnableAxisXScrolling = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).EnableAxisYScrolling = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).EnableAxisXZooming = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).EnableAxisYZooming = true;


                (Chart_total_usage_chart.Diagram as XYDiagram).EnableAxisXScrolling = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).EnableAxisYScrolling = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).EnableAxisXZooming = true;
                (Chart_total_usage_chart.Diagram as XYDiagram).EnableAxisYZooming = true;

                Chart_total_usage_chart.Series[0].ArgumentScaleType = ScaleType.DateTime;
                Chart_total_usage_chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;


                title = "Total : "; real_idx = -1;
                for (int idx = 0; idx < 4; idx++)
                {
                    sum = 0; usage_ok = false;
                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                    {
                        if (idx == 0) { usage_ok = true; }
                        else if (idx == 1) { usage_ok = true; }
                        else if (idx == 3) { usage_ok = true; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {
                        if (idx == 0) { usage_ok = true; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                    {
                        if (idx == 0) { usage_ok = true; }
                        else if (idx == 3) { usage_ok = true; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                    {
                        if (idx == 0) { usage_ok = true; }
                        else if (idx == 3) { usage_ok = true; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                    {
                        if (idx == 0) { usage_ok = true; }
                        else if (idx == 3) { usage_ok = true; }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (idx == 0) { usage_ok = true; }
                        else if (idx == 1) { usage_ok = true; }
                        else if (idx == 2) { usage_ok = true; }
                        else if (idx == 3) { usage_ok = true; }
                    }
                    if (usage_ok == true)
                    {
                        real_idx = real_idx + 1;
                        for (int idx2 = 0; idx2 < Chart_total_usage_chart.Series[real_idx].Points.Count; idx2++)
                        {
                            sum = sum + Chart_total_usage_chart.Series[real_idx].Points[idx2].Values[0];
                        }
                        if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                        {
                            if (idx == 0) { title = title + "H2O2" + "(" + sum + ")  "; }
                            else if (idx == 1) { title = title + "NH4OH" + "(" + sum + ")  "; }
                            else if (idx == 3) { title = title + "DIW" + "(" + sum + ")  "; }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                        {
                            if (idx == 0) { title = title + "IPA" + "(" + sum + ")  "; }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                        {
                            if (idx == 0) { title = title + "DSP" + "(" + sum + ")  "; }
                            else if (idx == 3) { title = title + "DIW" + "(" + sum + ")  "; }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                        {
                            if (idx == 0) { title = title + "DHF" + "(" + sum + ")  "; }
                            else if (idx == 3) { title = title + "DIW" + "(" + sum + ")  "; }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                        {
                            if (idx == 0) { title = title + "LAL" + "(" + sum + ")  "; }
                            else if (idx == 3) { title = title + "DIW" + "(" + sum + ")  "; }
                        }
                        else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                        {
                            if (idx == 0) { title = title + "H2O2" + "(" + sum + ")  "; }
                            else if (idx == 1) { title = title + "H2SO4" + "(" + sum + ")  "; }
                            else if (idx == 2) { title = title + "HF" + "(" + sum + ")  "; }
                            else if (idx == 3) { title = title + "DIW" + "(" + sum + ")  "; }
                        }
                        Chart_total_usage_chart.Series[real_idx].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                    }


                }

                Chart_total_usage_chart.Titles.Clear();
                ChartTitle chart_title = new ChartTitle();
                Chart_total_usage_chart.Titles.Add(chart_title);
                chart_title.Text = title;
                chart_title.Alignment = StringAlignment.Far;

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Chart_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                Chart_total_usage_chart.ResumeLayout();
            }
        }

        public void Insert_Total_Usage(tank_class.enum_tank_type call_selected_tank, int ccss_idx, double totalusage)
        {
            int result = 0;
            string query = "", err = "";
            DataSet dataset = new DataSet();
            try
            {
                if (totalusage != 0)
                {
                    query = "INSERT INTO totalusage_logs(totalusage_saved_time, totalusage_id, totalusage_index_ccss, totalusage_value)";
                    query += " VALUES (";
                    query += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                    query += ",'" + (int)call_selected_tank + "'";
                    query += ",'" + ccss_idx + "'";
                    query += ",'" + totalusage + "'";
                    query += ")";

                    result = Program.database_md.MariaDB_MainQuery(Program.cg_main.db_local_cds.connection, query, ref err);
                    if (result == 1)
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                    }
                    else
                    {
                        Program.log_md.LogWrite(this.Name + ".Insert_Total_Usage." + result, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                    }
                }


            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Insert_Total_Usage." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return;
        }
    }
}