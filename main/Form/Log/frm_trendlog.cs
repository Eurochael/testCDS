using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace cds
{
    public partial class frm_trendlog : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        private Boolean _initialed = false;
        public Thread thd_load_chart_log;
        public delegate void Del_Chart_Setting(DateTime start, DateTime end);
        public delegate void Del_Data_Setting_By_Series_Array(object seriesdata);
        public delegate void Del_Data_Setting_By_Series(int idx_series, SeriesPoint[] seriesdata);
        public delegate void Del_Data_Setting_By_Series_Alone(int idx_series, SeriesPoint seriesdata);
        public CheckEdit[] check_legend;
        public DataTable dt_trendlog = new DataTable("trend");
        public enum enum_trend_type
        {
            SUPPLY_TEMP_A = 0,
            SUPPLY_TEMP_B = 1,
            SUPPLY_HEATER_A = 2,
            SUPPLY_HEATER_B = 3,
            TANK_TEMP_A = 4,
            TANK_TEMP_B = 5,
            CIRCULATION_HEATER_1 = 6,
            CIRCULATION_HEATER_2 = 7,
            CIRCULATION_TEMP = 8,
            TS_09 = 9,
            THERMOSTAT_SUPPLY_A = 10,
            THERMOSTAT_SUPPLY_B = 11,
            THERMOSTAT_CIRCULATION = 12,
            SUPPLY_A_PRESSURE_OUT_FILTER = 13,
            SUPPLY_B_PRESSURE_OUT_FILTER = 14,
            SUPPLY_A_FLOW = 15,
            SUPPLY_B_FLOW = 16,
            SUPPLY_A_PRESSURE_RETURN = 17,
            SUPPLY_B_PRESSURE_RETURN = 18,
            NONE = 99,
        }
        public frm_trendlog()
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
                        timer_trendsave.Interval = 1000; timer_trendsave.Enabled = true;
                        Chart_HistoryChart.Visible = true;
                        Checked_Initial();
                        //if (_initialed == false) { _initialed = true; btn_search.PerformClick(); }
                    }
                    else
                    {
                        Chart_HistoryChart.Visible = false;
                        Series_Hide();
                    }
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + ".actived." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
            }
        }
        private void frm_trendlog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thd_load_chart_log != null) { thd_load_chart_log.Abort(); thd_load_chart_log = null; }
        }
        public string Load_TrendData()
        {
            string result;
            try
            {
                result = "";
                //Program.cg_trend_datalist.total_cnt = 20;
                //Array.Resize(ref Program.cg_trend_datalist.trend_data, Program.cg_trend_datalist.total_cnt);

                check_legend = new CheckEdit[Program.cg_trend_datalist.total_cnt];
                dbl_fpnl_legend.Controls.Clear();
                for (int idx = 0; idx < Program.cg_trend_datalist.total_cnt; idx++)
                {
                    //Program.cg_trend_datalist.trend_data[idx] = new Trend_Data();
                    //Program.cg_trend_datalist.trend_data[idx].name = "Temp" + idx;
                    //Program.cg_trend_datalist.trend_data[idx].value = 0;

                    check_legend[idx] = new CheckEdit();
                    check_legend[idx].Text = Program.cg_trend_datalist.trend_data[idx].name;
                    check_legend[idx].Tag = idx;
                    check_legend[idx].Size = new Size(280, 20);
                    if (idx == 0)
                    {
                        check_legend[idx].Checked = true;
                    }
                    else
                    {
                        check_legend[idx].Checked = false;
                    }
                    check_legend[idx].CheckedChanged -= checkEdit_CheckedChanged; check_legend[idx].CheckedChanged += checkEdit_CheckedChanged;
                    check_legend[idx].EditValueChanging -= checkEdit_EditValueChanging; check_legend[idx].EditValueChanging += checkEdit_EditValueChanging; 
                    dbl_fpnl_legend.Controls.Add(check_legend[idx]);
                }

                Series_Initial();

                dt_trendlog.Clear();
                dt_trendlog.Dispose();
                dt_trendlog = null;
                //trend table 설정
                dt_trendlog = new DataTable("trendlog");
                dt_trendlog.Columns.Clear();
                dt_trendlog.Rows.Clear();
                dt_trendlog.Columns.Add("DATETIME", Type.GetType("System.DateTime"));
                //dt_trendlog.Columns.Add("DATETIMEKEY", Type.GetType("System.String"));
                for (int idx = 0; idx < Program.cg_trend_datalist.total_cnt; idx++)
                {
                    dt_trendlog.Columns.Add("VALUE" + (idx + 1), Type.GetType("System.Int32"));
                }

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Load_Chart_Log." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {

            }
            return result;
        }
        public string Save_TrendData()
        {
            if (Program.cg_apploading.load_complete == false) { return ""; }
            string result = "", data = "";
            SeriesPoint[] seires_tmp = new SeriesPoint[Program.cg_trend_datalist.total_cnt];
            SeriesPoint[] seires_tmp2 = new SeriesPoint[Program.cg_trend_datalist.total_cnt];
            DateTime dt_log_start = DateTime.Now;
            try
            {
                Chart_HistoryChart.SuspendLayout();
                for (int idx = 0; idx < Program.cg_trend_datalist.total_cnt; idx++)
                {
                    if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.SUPPLY_TEMP_A)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.SUPPLY_TEMP_B)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.SUPPLY_HEATER_A)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.SUPPLY_HEATER_B)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.TANK_TEMP_A)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.TANK_TEMP_B)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.CIRCULATION_HEATER_1)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.CIRCULATION_HEATER_2)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.CIRCULATION_TEMP)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.circulation.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.TS_09)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.THERMOSTAT_SUPPLY_A)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.THERMOSTAT_SUPPLY_B)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.THERMOSTAT_CIRCULATION)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.SUPPLY_A_PRESSURE_OUT_FILTER)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_OUT_PRESS].value;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.SUPPLY_B_PRESSURE_OUT_FILTER)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_OUT_PRESS].value;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.SUPPLY_A_FLOW)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.SUPPLY_B_FLOW)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.SUPPLY_A_PRESSURE_RETURN)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_A].value;
                    }
                    else if (Program.cg_trend_datalist.trend_data[idx].type == enum_trend_type.SUPPLY_B_PRESSURE_RETURN)
                    {
                        Program.cg_trend_datalist.trend_data[idx].value = Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_B].value;
                    }
                    seires_tmp[idx] = new SeriesPoint(dt_log_start, Program.cg_trend_datalist.trend_data[idx].value);
                    if (Program.cg_trend_datalist.trend_data[idx].use == true)
                    {
                        Chart_HistoryChart.Series[idx].Points.Add(seires_tmp[idx]);
                        if (Chart_HistoryChart.Series[idx].Points.Count > 0)
                        {
                            if ((DateTime.Now - Convert.ToDateTime(Chart_HistoryChart.Series[idx].Points[0].Argument)).TotalMinutes >= Program.cg_trend_datalist.fifo_max_minute)
                            {
                                Chart_HistoryChart.Series[idx].Points.RemoveAt(0);
                            }
                        }
                        if (Chart_HistoryChart.Series[idx].Points.Count > 0)
                        {
                            if (this._actived == true)
                            {
                                //상시 호출 시 부하 주의
                                Chart_Setting(Convert.ToDateTime(Chart_HistoryChart.Series[idx].Points[0].Argument), Convert.ToDateTime(Chart_HistoryChart.Series[idx].Points[Chart_HistoryChart.Series[idx].Points.Count - 1].Argument));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Load_Chart_Log." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                Chart_HistoryChart.ResumeLayout();
            }
            return result;
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                if (btn_event.Name == "btn_search")
                {
                    Series_Initial();
                    if (thd_load_chart_log != null)
                    {
                        thd_load_chart_log.Abort(); thd_load_chart_log = null;
                        thd_load_chart_log = new Thread(Load_Chart_Log_By_APPMemory); thd_load_chart_log.Start();
                        this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "Trend Data Loading...", true, frm_process_indicator.enum_call_by.trendlog);
                    }
                    else
                    {
                        this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "Trend Data Loading...", true, frm_process_indicator.enum_call_by.trendlog);
                        thd_load_chart_log = new Thread(Load_Chart_Log_By_APPMemory); thd_load_chart_log.Start();
                    }
                }

                else if (btn_event.Name == "btn_log_clear")
                {
                    Series_Initial();
                }
            }
            Program.eventlog_form.Insert_Event(this.Name + ".Button Click : " + btn_event.Name, (int)frm_eventlog.enum_event_type.USER_BUTTON, (int)frm_eventlog.enum_occurred_type.USER, true);
            Program.log_md.LogWrite(this.Name + "." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }
        public void Series_Initial()
        {
            try
            {
                Chart_HistoryChart.Series.Clear();
                for (int idx = 0; idx < Program.cg_trend_datalist.total_cnt; idx++)
                {
                    Chart_HistoryChart.Series.Add(Program.cg_trend_datalist.trend_data[idx].name, ViewType.Line);
                    if (Program.cg_trend_datalist.trend_data[idx].use == false)
                    {
                        Chart_HistoryChart.Series[idx].Visible = false;
                        check_legend[idx].Visible = false;
                    }
                    else
                    {
                        if (idx == 0)
                        {
                            Chart_HistoryChart.Series[idx].Visible = false;
                            check_legend[idx].Visible = true;
                        }
                        else
                        {
                            Chart_HistoryChart.Series[idx].Visible = false;
                            check_legend[idx].Visible = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Series_Initial." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

        }
        public void Series_Hide()
        {
            try
            {
                for (int idx = 0; idx < Program.cg_trend_datalist.total_cnt; idx++)
                {
                    check_legend[idx].Visible = false;
                    check_legend[idx].Checked = false;
                    Chart_HistoryChart.Series[idx].Visible = false;
                    //if (Program.cg_trend_datalist.trend_data[idx].use == false)
                    //{
                      
                    //}
                    //else
                    //{
                    //check_legend[idx].Visible = true;
                    //    Chart_HistoryChart.Series[idx].Visible = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Series_Hide." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

        }
        public void Checked_Initial()
        {
            for (int idx = 0; idx < Program.cg_trend_datalist.total_cnt; idx++)
            {
                if (Program.cg_trend_datalist.trend_data[idx].use == false)
                {
                    Chart_HistoryChart.Series[idx].Visible = false;
                    check_legend[idx].Checked = false; 
                    check_legend[idx].Visible = false;
                }
                else
                {
                    Chart_HistoryChart.Series[idx].Visible = false;
                    check_legend[idx].Checked = false;
                    check_legend[idx].Visible = true;
                }

            }
            Chart_Setting(DateTime.Now.AddHours(-1), DateTime.Now);
        }
        public void Load_Chart_Log_By_APPMemory()
        {
            string result = "";
            DateTime dt_start = DateTime.Now;
            DateTime dt_end = DateTime.Now;
            DateTime dt_series_display_start = DateTime.Now;
            SeriesPoint seriesdata;
            int data_row = 0;// dt_trendlog.Rows.Count;
            DataTable dt_copy_trend = new DataTable("Copy_Table");
            try
            {
                dt_series_display_start = DateTime.Now;
                dt_copy_trend = dt_trendlog.Copy();
                data_row = dt_copy_trend.Rows.Count;
                for (int idx = 0; idx < data_row; idx++)
                {
                    if (idx == 0)
                    {
                        dt_start = Convert.ToDateTime(dt_copy_trend.Rows[idx]["DATETIME"]);
                        dt_end = Convert.ToDateTime(dt_copy_trend.Rows[dt_trendlog.Rows.Count - 1]["DATETIME"]);
                    }
                    for (int idx2 = 0; idx2 < Program.cg_trend_datalist.total_cnt; idx2++)
                    {

                        //Check된 Series만 차트에 출력한다.
                        if (check_legend[idx2].Checked == true)
                        {
                            seriesdata = new SeriesPoint(Convert.ToDateTime(dt_copy_trend.Rows[idx]["DATETIME"]), dt_copy_trend.Rows[idx]["VALUE" + (idx2 + 1)]);
                            this.BeginInvoke(new Del_Data_Setting_By_Series_Alone(Data_Setting_By_Series_Alone), idx2, seriesdata);
                        }
                        if (idx != 0 && idx % 100 == 0)
                        {
                            System.Threading.Thread.Sleep(20);
                        }

                    }
                    Program.main_md.process_desciption = idx + " / " + (data_row) + "  " + (int)(DateTime.Now - dt_series_display_start).TotalSeconds + "Sec....";

                }
                System.Threading.Thread.Sleep(200);
                this.BeginInvoke(new Del_Chart_Setting(Chart_Setting), dt_start, dt_end);
                System.Threading.Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite(this.Name + ".Load_Chart_Log." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
                Program.main_md.process_desciption = "";
                this.BeginInvoke(new Module_main.Del_process_indicator_popup(Program.main_md.process_indicator_popup), "", false, frm_process_indicator.enum_call_by.trendlog);
            }
        }
        public void Data_Setting_By_Series_Array(object seriesdata)
        {
            try
            {
                if (seriesdata is SeriesPoint[][])
                {
                    for (int idx = 0; idx < (seriesdata as SeriesPoint[][]).Length - 1; idx++)
                    {
                        if ((seriesdata as SeriesPoint[][])[idx] != null)
                        {
                            Chart_HistoryChart.Series[idx].Points.AddRange((seriesdata as SeriesPoint[][])[idx]);
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
        public void Data_Setting_By_Series(int idx_series, SeriesPoint[] seriesdata)
        {
            try
            {
                if (seriesdata is SeriesPoint[])
                {
                    if (seriesdata != null)
                    {
                        Chart_HistoryChart.Series[idx_series].Points.AddRange(seriesdata);
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
                        Chart_HistoryChart.Series[idx_series].Points.Add(seriesdata);
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
        public void Chart_Setting(DateTime start, DateTime end)
        {
            try
            {
                gp_trend_log.Text = "Trend Search Result - " + start.ToString("HH:mm:ss") + " ~ " + end.ToString("HH:mm:ss");
                if (Chart_HistoryChart.Series.Count > 0)
                {
                    gp_trend_log.Text = gp_trend_log.Text + " (" + Chart_HistoryChart.Series[0].Points.Count + ")";
                }
                if ((Chart_HistoryChart.Diagram as XYDiagram) != null)
                {
                    (Chart_HistoryChart.Diagram as XYDiagram).AxisX.WholeRange.SetMinMaxValues(start, end);
                    (Chart_HistoryChart.Diagram as XYDiagram).AxisX.WholeRange.Auto = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).AxisX.Visible = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;

                    (Chart_HistoryChart.Diagram as XYDiagram).AxisX.DateTimeOptions.FormatString = "HH:mm:ss";
                    (Chart_HistoryChart.Diagram as XYDiagram).AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Continuous;

                    (Chart_HistoryChart.Diagram as XYDiagram).AxisY.WholeRange.Auto = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).AxisY.VisualRange.Auto = true;

                    (Chart_HistoryChart.Diagram as XYDiagram).AxisY.Range.Auto = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).AxisY.Range.ScrollingRange.Auto = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).AxisX.Range.Auto = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).AxisX.Range.ScrollingRange.Auto = true;

                    (Chart_HistoryChart.Diagram as XYDiagram).EnableAxisXScrolling = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).EnableAxisYScrolling = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).EnableAxisXZooming = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).EnableAxisYZooming = true;


                    (Chart_HistoryChart.Diagram as XYDiagram).EnableAxisXScrolling = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).EnableAxisYScrolling = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).EnableAxisXZooming = true;
                    (Chart_HistoryChart.Diagram as XYDiagram).EnableAxisYZooming = true;
                }

                Chart_HistoryChart.Series[0].ArgumentScaleType = ScaleType.DateTime;
                Chart_HistoryChart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
                Chart_HistoryChart.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.True;
                Chart_HistoryChart.CrosshairOptions.ShowCrosshairLabels = true;
                Chart_HistoryChart.CrosshairOptions.ShowGroupHeaders = true;

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Chart_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
            }
        }
        public void Trend_Log_call_cancel()
        {
            try
            {
                if (thd_load_chart_log != null) { thd_load_chart_log.Abort(); thd_load_chart_log = null; }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".Trend_Log_call_cancel." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
            }
        }
        private void checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            int idx_series = -1;
            try
            {
                if ((sender is CheckEdit) == true)
                {

                    if (Program.main_md.IsNumeric((sender as CheckEdit).Tag.ToString()) == true)
                    {
                        idx_series = Convert.ToInt32((sender as CheckEdit).Tag);
                        if (check_legend[idx_series].Checked == true)
                        {
                            Chart_HistoryChart.Series[idx_series].Visible = true;
                        }
                        else
                        {
                            Chart_HistoryChart.Series[idx_series].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".checkEdit_CheckedChanged." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally
            {
            }
        }
        private void checkEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            int view_count = 0;
            for (int idx = 0; idx < Program.cg_trend_datalist.total_cnt; idx++)
            {
                if (Chart_HistoryChart.Series[idx].Visible == true)
                {
                    view_count = view_count + 1;
                }
            }
            
            if (e.NewValue.ToString() == "True" && view_count >= 5)
            {
                e.Cancel = true;
                Program.main_md.Message_By_Application("Cannot Maximum View Count 5", frm_messagebox.enum_apptype.Only_OK); return;
            }
        }
        private void Chart_HistoryChart_CustomDrawSeries(object sender, CustomDrawSeriesEventArgs e)
        {

        }

        private void Chart_HistoryChart_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {

        }
        private void timer_trendsave_Tick(object sender, EventArgs e)
        {
            //Program.log_md.LogWrite(this.Name + ".Trend_Log_call_cancel." + "S" + DateTime.Now.ToString("ss.fff"), Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            //Console.WriteLine("S" + DateTime.Now.ToString("ss.fff"));
            Save_TrendData();
            //Console.WriteLine("E" + DateTime.Now.ToString("ss.fff"));
            //Program.log_md.LogWrite(this.Name + ".Trend_Log_call_cancel." + "E" + DateTime.Now.ToString("ss.fff"), Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
        }


    }
}