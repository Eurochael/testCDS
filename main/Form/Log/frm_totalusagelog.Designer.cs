
namespace cds
{
    partial class frm_totalusagelog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_totalusagelog));
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView1 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbtn_month = new System.Windows.Forms.RadioButton();
            this.rbtn_week = new System.Windows.Forms.RadioButton();
            this.rbtn_day = new System.Windows.Forms.RadioButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.pnl_body = new cds.DoubleBufferedPanel();
            this.gp_total_usage_log = new DevExpress.XtraEditors.GroupControl();
            this.Chart_total_usage_chart = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.pnl_body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gp_total_usage_log)).BeginInit();
            this.gp_total_usage_log.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_total_usage_chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl1.Location = new System.Drawing.Point(1060, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelControl1.Size = new System.Drawing.Size(220, 890);
            this.panelControl1.TabIndex = 17;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.panel1);
            this.groupControl1.Location = new System.Drawing.Point(5, 58);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(200, 115);
            this.groupControl1.TabIndex = 25;
            this.groupControl1.Text = "View Type";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbtn_month);
            this.panel1.Controls.Add(this.rbtn_week);
            this.panel1.Controls.Add(this.rbtn_day);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(196, 90);
            this.panel1.TabIndex = 24;
            // 
            // rbtn_month
            // 
            this.rbtn_month.AutoSize = true;
            this.rbtn_month.Font = new System.Drawing.Font("Tahoma", 12F);
            this.rbtn_month.Location = new System.Drawing.Point(4, 62);
            this.rbtn_month.Name = "rbtn_month";
            this.rbtn_month.Size = new System.Drawing.Size(71, 23);
            this.rbtn_month.TabIndex = 2;
            this.rbtn_month.Text = "Month";
            this.rbtn_month.UseVisualStyleBackColor = true;
            // 
            // rbtn_week
            // 
            this.rbtn_week.AutoSize = true;
            this.rbtn_week.Font = new System.Drawing.Font("Tahoma", 12F);
            this.rbtn_week.Location = new System.Drawing.Point(4, 33);
            this.rbtn_week.Name = "rbtn_week";
            this.rbtn_week.Size = new System.Drawing.Size(65, 23);
            this.rbtn_week.TabIndex = 1;
            this.rbtn_week.Text = "Week";
            this.rbtn_week.UseVisualStyleBackColor = true;
            // 
            // rbtn_day
            // 
            this.rbtn_day.AutoSize = true;
            this.rbtn_day.Checked = true;
            this.rbtn_day.Font = new System.Drawing.Font("Tahoma", 12F);
            this.rbtn_day.Location = new System.Drawing.Point(4, 4);
            this.rbtn_day.Name = "rbtn_day";
            this.rbtn_day.Size = new System.Drawing.Size(54, 23);
            this.rbtn_day.TabIndex = 0;
            this.rbtn_day.TabStop = true;
            this.rbtn_day.Text = "Day";
            this.rbtn_day.UseVisualStyleBackColor = true;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btn_search);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(2, 818);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(216, 70);
            this.panelControl2.TabIndex = 20;
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.Location = new System.Drawing.Point(112, 5);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(97, 60);
            this.btn_search.TabIndex = 2;
            this.btn_search.Text = "Search";
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
            this.labelControl1.Size = new System.Drawing.Size(216, 33);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Total Usage Log";
            // 
            // pnl_body
            // 
            this.pnl_body.Controls.Add(this.gp_total_usage_log);
            this.pnl_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_body.Location = new System.Drawing.Point(0, 0);
            this.pnl_body.Name = "pnl_body";
            this.pnl_body.Padding = new System.Windows.Forms.Padding(20);
            this.pnl_body.Size = new System.Drawing.Size(1060, 890);
            this.pnl_body.TabIndex = 21;
            // 
            // gp_total_usage_log
            // 
            this.gp_total_usage_log.Controls.Add(this.Chart_total_usage_chart);
            this.gp_total_usage_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_total_usage_log.Location = new System.Drawing.Point(20, 20);
            this.gp_total_usage_log.Name = "gp_total_usage_log";
            this.gp_total_usage_log.Size = new System.Drawing.Size(1020, 850);
            this.gp_total_usage_log.TabIndex = 20;
            this.gp_total_usage_log.Text = "Total Usage Search Result";
            // 
            // Chart_total_usage_chart
            // 
            this.Chart_total_usage_chart.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.Chart_total_usage_chart.Diagram = xyDiagram1;
            this.Chart_total_usage_chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart_total_usage_chart.Legend.MarkerMode = DevExpress.XtraCharts.LegendMarkerMode.CheckBox;
            this.Chart_total_usage_chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.Chart_total_usage_chart.Location = new System.Drawing.Point(2, 23);
            this.Chart_total_usage_chart.Name = "Chart_total_usage_chart";
            series1.Name = "Series 1";
            this.Chart_total_usage_chart.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.Chart_total_usage_chart.SeriesTemplate.View = swiftPlotSeriesView1;
            this.Chart_total_usage_chart.Size = new System.Drawing.Size(1016, 825);
            this.Chart_total_usage_chart.TabIndex = 18;
            // 
            // frm_totalusagelog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 890);
            this.Controls.Add(this.pnl_body);
            this.Controls.Add(this.panelControl1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_totalusagelog";
            this.Text = "frm_mixing_step";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.pnl_body.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gp_total_usage_log)).EndInit();
            this.gp_total_usage_log.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_total_usage_chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DoubleBufferedPanel pnl_body;
        private DevExpress.XtraEditors.GroupControl gp_total_usage_log;
        private DevExpress.XtraCharts.ChartControl Chart_total_usage_chart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbtn_month;
        private System.Windows.Forms.RadioButton rbtn_week;
        private System.Windows.Forms.RadioButton rbtn_day;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}