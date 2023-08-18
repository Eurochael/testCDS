
namespace cds
{
    partial class frm_trendlog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_trendlog));
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView1 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btn_log_clear = new DevExpress.XtraEditors.SimpleButton();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gp_trend_log = new DevExpress.XtraEditors.GroupControl();
            this.Chart_HistoryChart = new DevExpress.XtraCharts.ChartControl();
            this.timer_trendsave = new System.Windows.Forms.Timer(this.components);
            this.pnl_body = new cds.DoubleBufferedPanel();
            this.dbl_fpnl_legend = new cds.DoubleFlowLayoutPanel();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gp_trend_log)).BeginInit();
            this.gp_trend_log.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_HistoryChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).BeginInit();
            this.pnl_body.SuspendLayout();
            this.dbl_fpnl_legend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.pnl_body);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl1.Location = new System.Drawing.Point(963, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelControl1.Size = new System.Drawing.Size(317, 890);
            this.panelControl1.TabIndex = 17;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btn_log_clear);
            this.panelControl2.Controls.Add(this.btn_search);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(2, 878);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(313, 10);
            this.panelControl2.TabIndex = 19;
            this.panelControl2.Visible = false;
            // 
            // btn_log_clear
            // 
            this.btn_log_clear.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_log_clear.ImageOptions.Image")));
            this.btn_log_clear.Location = new System.Drawing.Point(5, 5);
            this.btn_log_clear.Name = "btn_log_clear";
            this.btn_log_clear.Size = new System.Drawing.Size(97, 60);
            this.btn_log_clear.TabIndex = 3;
            this.btn_log_clear.Text = "Clear";
            this.btn_log_clear.Visible = false;
            this.btn_log_clear.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.Location = new System.Drawing.Point(112, 5);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(97, 60);
            this.btn_search.TabIndex = 2;
            this.btn_search.Text = "Search";
            this.btn_search.Visible = false;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl1.Location = new System.Drawing.Point(2, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(313, 33);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Trend Log";
            // 
            // gp_trend_log
            // 
            this.gp_trend_log.Controls.Add(this.Chart_HistoryChart);
            this.gp_trend_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_trend_log.Location = new System.Drawing.Point(0, 0);
            this.gp_trend_log.Name = "gp_trend_log";
            this.gp_trend_log.Size = new System.Drawing.Size(963, 890);
            this.gp_trend_log.TabIndex = 19;
            this.gp_trend_log.Text = "Trend Search Result";
            // 
            // Chart_HistoryChart
            // 
            this.Chart_HistoryChart.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.False;
            this.Chart_HistoryChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart_HistoryChart.Legend.MarkerMode = DevExpress.XtraCharts.LegendMarkerMode.CheckBox;
            this.Chart_HistoryChart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.Chart_HistoryChart.Location = new System.Drawing.Point(2, 23);
            this.Chart_HistoryChart.Name = "Chart_HistoryChart";
            this.Chart_HistoryChart.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.Chart_HistoryChart.SeriesTemplate.View = swiftPlotSeriesView1;
            this.Chart_HistoryChart.Size = new System.Drawing.Size(959, 865);
            this.Chart_HistoryChart.TabIndex = 19;
            // 
            // timer_trendsave
            // 
            this.timer_trendsave.Tick += new System.EventHandler(this.timer_trendsave_Tick);
            // 
            // pnl_body
            // 
            this.pnl_body.Controls.Add(this.dbl_fpnl_legend);
            this.pnl_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_body.Location = new System.Drawing.Point(2, 45);
            this.pnl_body.Name = "pnl_body";
            this.pnl_body.Padding = new System.Windows.Forms.Padding(10);
            this.pnl_body.Size = new System.Drawing.Size(313, 833);
            this.pnl_body.TabIndex = 20;
            // 
            // dbl_fpnl_legend
            // 
            this.dbl_fpnl_legend.Controls.Add(this.checkEdit1);
            this.dbl_fpnl_legend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbl_fpnl_legend.Location = new System.Drawing.Point(10, 10);
            this.dbl_fpnl_legend.Name = "dbl_fpnl_legend";
            this.dbl_fpnl_legend.Size = new System.Drawing.Size(293, 813);
            this.dbl_fpnl_legend.TabIndex = 19;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(3, 3);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "checkEdit1";
            this.checkEdit1.Size = new System.Drawing.Size(173, 18);
            this.checkEdit1.TabIndex = 0;
            // 
            // frm_trendlog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 890);
            this.Controls.Add(this.gp_trend_log);
            this.Controls.Add(this.panelControl1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_trendlog";
            this.Text = "frm_mixing_step";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_trendlog_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gp_trend_log)).EndInit();
            this.gp_trend_log.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_HistoryChart)).EndInit();
            this.pnl_body.ResumeLayout(false);
            this.dbl_fpnl_legend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DoubleBufferedPanel pnl_body;
        private DevExpress.XtraEditors.GroupControl gp_trend_log;
        private DoubleFlowLayoutPanel dbl_fpnl_legend;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_log_clear;
        private DevExpress.XtraCharts.ChartControl Chart_HistoryChart;
        private System.Windows.Forms.Timer timer_trendsave;
    }
}