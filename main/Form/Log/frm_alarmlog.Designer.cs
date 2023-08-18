
namespace cds
{
    partial class frm_alarmlog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_alarmlog));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_event = new DevExpress.XtraEditors.SimpleButton();
            this.btn_clear = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.txt_token = new DevExpress.XtraEditors.TokenEdit();
            this.dt_end = new DevExpress.XtraEditors.DateEdit();
            this.dt_start = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gp_alarm_log = new DevExpress.XtraEditors.GroupControl();
            this.grid_alarmlog = new DevExpress.XtraGrid.GridControl();
            this.gridview_alarmlog = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnl_body = new cds.DoubleBufferedPanel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_token.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_end.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_end.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_start.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_start.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gp_alarm_log)).BeginInit();
            this.gp_alarm_log.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_alarmlog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridview_alarmlog)).BeginInit();
            this.pnl_body.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_event);
            this.panelControl1.Controls.Add(this.btn_clear);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Controls.Add(this.txt_token);
            this.panelControl1.Controls.Add(this.dt_end);
            this.panelControl1.Controls.Add(this.dt_start);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl1.Location = new System.Drawing.Point(1060, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelControl1.Size = new System.Drawing.Size(220, 890);
            this.panelControl1.TabIndex = 17;
            // 
            // btn_event
            // 
            this.btn_event.Location = new System.Drawing.Point(8, 268);
            this.btn_event.Name = "btn_event";
            this.btn_event.Size = new System.Drawing.Size(187, 46);
            this.btn_event.TabIndex = 22;
            this.btn_event.Text = "test button";
            this.btn_event.Visible = false;
            this.btn_event.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_clear
            // 
            this.btn_clear.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_clear.ImageOptions.Image")));
            this.btn_clear.Location = new System.Drawing.Point(101, 216);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(94, 46);
            this.btn_clear.TabIndex = 21;
            this.btn_clear.Text = "Clear";
            this.btn_clear.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Options.UseTextOptions = true;
            this.labelControl6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl6.Location = new System.Drawing.Point(8, 161);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(187, 23);
            this.labelControl6.TabIndex = 20;
            this.labelControl6.Text = "Option(Split \',\')";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Options.UseTextOptions = true;
            this.labelControl5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl5.Location = new System.Drawing.Point(8, 106);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(53, 23);
            this.labelControl5.TabIndex = 19;
            this.labelControl5.Text = "End";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btn_cancel);
            this.panelControl2.Controls.Add(this.btn_search);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(2, 818);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(216, 70);
            this.panelControl2.TabIndex = 18;
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.Location = new System.Drawing.Point(6, 5);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(97, 60);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Click += new System.EventHandler(this.btn_search_Click);
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
            // txt_token
            // 
            this.txt_token.Location = new System.Drawing.Point(8, 190);
            this.txt_token.Name = "txt_token";
            this.txt_token.Properties.AutoHeightMode = DevExpress.XtraEditors.TokenEditAutoHeightMode.RestrictedExpand;
            this.txt_token.Properties.EditMode = DevExpress.XtraEditors.TokenEditMode.Manual;
            this.txt_token.Properties.MaxExpandLines = 6;
            this.txt_token.Properties.Separators.AddRange(new string[] {
            ","});
            this.txt_token.Properties.EditValueChanged += new System.EventHandler(this.txt_token_Properties_EditValueChanged);
            this.txt_token.Size = new System.Drawing.Size(187, 20);
            this.txt_token.TabIndex = 15;
            this.txt_token.ValidateToken += new DevExpress.XtraEditors.TokenEditValidateTokenEventHandler(this.txt_token_ValidateToken);
            // 
            // dt_end
            // 
            this.dt_end.EditValue = null;
            this.dt_end.Location = new System.Drawing.Point(8, 135);
            this.dt_end.Name = "dt_end";
            this.dt_end.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_end.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
            this.dt_end.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_end.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            this.dt_end.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH";
            this.dt_end.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dt_end.Properties.EditFormat.FormatString = "yyyy-MM-dd HH";
            this.dt_end.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dt_end.Properties.MaskSettings.Set("mask", "yyyy-MM-dd HH");
            this.dt_end.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.dt_end.Size = new System.Drawing.Size(187, 20);
            this.dt_end.TabIndex = 13;
            // 
            // dt_start
            // 
            this.dt_start.EditValue = null;
            this.dt_start.Location = new System.Drawing.Point(8, 80);
            this.dt_start.Name = "dt_start";
            this.dt_start.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_start.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
            this.dt_start.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_start.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            this.dt_start.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH";
            this.dt_start.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dt_start.Properties.EditFormat.FormatString = "yyyy-MM-dd HH";
            this.dt_start.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dt_start.Properties.MaskSettings.Set("mask", "yyyy-MM-dd HH");
            this.dt_start.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.dt_start.Size = new System.Drawing.Size(187, 20);
            this.dt_start.TabIndex = 11;
            this.dt_start.Popup += new System.EventHandler(this.dateEdit1_Popup);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(8, 51);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(53, 23);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "Start";
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
            this.labelControl1.Text = "Alarm Log";
            // 
            // gp_alarm_log
            // 
            this.gp_alarm_log.Controls.Add(this.grid_alarmlog);
            this.gp_alarm_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_alarm_log.Location = new System.Drawing.Point(20, 20);
            this.gp_alarm_log.Name = "gp_alarm_log";
            this.gp_alarm_log.Size = new System.Drawing.Size(1020, 850);
            this.gp_alarm_log.TabIndex = 18;
            this.gp_alarm_log.Text = "alarm search result = 0";
            // 
            // grid_alarmlog
            // 
            this.grid_alarmlog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_alarmlog.Location = new System.Drawing.Point(2, 23);
            this.grid_alarmlog.MainView = this.gridview_alarmlog;
            this.grid_alarmlog.Name = "grid_alarmlog";
            this.grid_alarmlog.Size = new System.Drawing.Size(1016, 825);
            this.grid_alarmlog.TabIndex = 1;
            this.grid_alarmlog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridview_alarmlog});
            // 
            // gridview_alarmlog
            // 
            this.gridview_alarmlog.GridControl = this.grid_alarmlog;
            this.gridview_alarmlog.Name = "gridview_alarmlog";
            // 
            // pnl_body
            // 
            this.pnl_body.Controls.Add(this.gp_alarm_log);
            this.pnl_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_body.Location = new System.Drawing.Point(0, 0);
            this.pnl_body.Name = "pnl_body";
            this.pnl_body.Padding = new System.Windows.Forms.Padding(20);
            this.pnl_body.Size = new System.Drawing.Size(1060, 890);
            this.pnl_body.TabIndex = 19;
            // 
            // frm_alarmlog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 890);
            this.Controls.Add(this.pnl_body);
            this.Controls.Add(this.panelControl1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_alarmlog";
            this.Text = "frm_mixing_step";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_alarmlog_FormClosed);
            this.Load += new System.EventHandler(this.frm_alarmlog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_token.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_end.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_end.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_start.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_start.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gp_alarm_log)).EndInit();
            this.gp_alarm_log.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_alarmlog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridview_alarmlog)).EndInit();
            this.pnl_body.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.GroupControl gp_alarm_log;
        private DevExpress.XtraGrid.GridControl grid_alarmlog;
        private DevExpress.XtraGrid.Views.Grid.GridView gridview_alarmlog;
        private DoubleBufferedPanel pnl_body;
        private DevExpress.XtraEditors.DateEdit dt_end;
        private DevExpress.XtraEditors.DateEdit dt_start;
        private DevExpress.XtraEditors.TokenEdit txt_token;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraEditors.SimpleButton btn_clear;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.SimpleButton btn_event;
    }
}