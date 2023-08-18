
namespace cds
{
    partial class frm_occured_alarm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_occured_alarm));
            this.dpnl_bottom = new cds.DoubleBufferedPanel();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_buzzer = new DevExpress.XtraEditors.SimpleButton();
            this.btn_exit = new DevExpress.XtraEditors.SimpleButton();
            this.btn_colappsed_all = new DevExpress.XtraEditors.SimpleButton();
            this.btn_expand_all = new DevExpress.XtraEditors.SimpleButton();
            this.btn_clear_all = new DevExpress.XtraEditors.SimpleButton();
            this.dpnl_body = new cds.DoubleBufferedPanel();
            this.lbl_level = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_name = new DevExpress.XtraEditors.LabelControl();
            this.lbl_no = new DevExpress.XtraEditors.LabelControl();
            this.lbl_occured_time = new DevExpress.XtraEditors.LabelControl();
            this.grid_occurred_alarm_list = new DevExpress.XtraGrid.GridControl();
            this.gridview_occurred_alarm = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.advBandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.timer_most_alarm_check = new System.Windows.Forms.Timer(this.components);
            this.dpnl_bottom.SuspendLayout();
            this.dpnl_body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_occurred_alarm_list)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridview_occurred_alarm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dpnl_bottom
            // 
            this.dpnl_bottom.Controls.Add(this.simpleButton3);
            this.dpnl_bottom.Controls.Add(this.simpleButton1);
            this.dpnl_bottom.Controls.Add(this.btn_buzzer);
            this.dpnl_bottom.Controls.Add(this.btn_exit);
            this.dpnl_bottom.Controls.Add(this.btn_colappsed_all);
            this.dpnl_bottom.Controls.Add(this.btn_expand_all);
            this.dpnl_bottom.Controls.Add(this.btn_clear_all);
            this.dpnl_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dpnl_bottom.Location = new System.Drawing.Point(0, 687);
            this.dpnl_bottom.Name = "dpnl_bottom";
            this.dpnl_bottom.Size = new System.Drawing.Size(785, 62);
            this.dpnl_bottom.TabIndex = 1;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(450, 27);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(75, 23);
            this.simpleButton3.TabIndex = 14;
            this.simpleButton3.Text = "simpleButton3";
            this.simpleButton3.Visible = false;
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(350, 27);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 13;
            this.simpleButton1.Text = "simpleButton1";
            this.simpleButton1.Visible = false;
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btn_buzzer
            // 
            this.btn_buzzer.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_buzzer.Appearance.Options.UseFont = true;
            this.btn_buzzer.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_buzzer.ImageOptions.Image")));
            this.btn_buzzer.Location = new System.Drawing.Point(487, 11);
            this.btn_buzzer.Name = "btn_buzzer";
            this.btn_buzzer.Size = new System.Drawing.Size(140, 40);
            this.btn_buzzer.TabIndex = 12;
            this.btn_buzzer.Text = "Buzzer Stop";
            this.btn_buzzer.Click += new System.EventHandler(this.btn_clear_all_Click);
            // 
            // btn_exit
            // 
            this.btn_exit.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_exit.Appearance.Options.UseFont = true;
            this.btn_exit.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_exit.ImageOptions.Image")));
            this.btn_exit.Location = new System.Drawing.Point(633, 11);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(140, 40);
            this.btn_exit.TabIndex = 11;
            this.btn_exit.Text = "Close";
            this.btn_exit.Click += new System.EventHandler(this.btn_clear_all_Click);
            // 
            // btn_colappsed_all
            // 
            this.btn_colappsed_all.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_colappsed_all.Appearance.Options.UseFont = true;
            this.btn_colappsed_all.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_colappsed_all.ImageOptions.Image")));
            this.btn_colappsed_all.Location = new System.Drawing.Point(303, 11);
            this.btn_colappsed_all.Name = "btn_colappsed_all";
            this.btn_colappsed_all.Size = new System.Drawing.Size(140, 40);
            this.btn_colappsed_all.TabIndex = 10;
            this.btn_colappsed_all.Text = "Collapsed";
            this.btn_colappsed_all.Click += new System.EventHandler(this.btn_clear_all_Click);
            // 
            // btn_expand_all
            // 
            this.btn_expand_all.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_expand_all.Appearance.Options.UseFont = true;
            this.btn_expand_all.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_expand_all.ImageOptions.Image")));
            this.btn_expand_all.Location = new System.Drawing.Point(157, 11);
            this.btn_expand_all.Name = "btn_expand_all";
            this.btn_expand_all.Size = new System.Drawing.Size(140, 40);
            this.btn_expand_all.TabIndex = 7;
            this.btn_expand_all.Text = "Expanded";
            this.btn_expand_all.Click += new System.EventHandler(this.btn_clear_all_Click);
            // 
            // btn_clear_all
            // 
            this.btn_clear_all.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_clear_all.Appearance.Options.UseFont = true;
            this.btn_clear_all.Enabled = false;
            this.btn_clear_all.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_clear_all.ImageOptions.Image")));
            this.btn_clear_all.Location = new System.Drawing.Point(11, 11);
            this.btn_clear_all.Name = "btn_clear_all";
            this.btn_clear_all.Size = new System.Drawing.Size(140, 40);
            this.btn_clear_all.TabIndex = 4;
            this.btn_clear_all.Text = "Clear All";
            this.btn_clear_all.Click += new System.EventHandler(this.btn_clear_all_Click);
            // 
            // dpnl_body
            // 
            this.dpnl_body.Controls.Add(this.lbl_level);
            this.dpnl_body.Controls.Add(this.simpleButton2);
            this.dpnl_body.Controls.Add(this.lbl_name);
            this.dpnl_body.Controls.Add(this.lbl_no);
            this.dpnl_body.Controls.Add(this.lbl_occured_time);
            this.dpnl_body.Controls.Add(this.grid_occurred_alarm_list);
            this.dpnl_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpnl_body.Location = new System.Drawing.Point(0, 0);
            this.dpnl_body.Name = "dpnl_body";
            this.dpnl_body.Size = new System.Drawing.Size(785, 687);
            this.dpnl_body.TabIndex = 2;
            // 
            // lbl_level
            // 
            this.lbl_level.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lbl_level.Appearance.Options.UseFont = true;
            this.lbl_level.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbl_level.Location = new System.Drawing.Point(254, 4);
            this.lbl_level.Name = "lbl_level";
            this.lbl_level.Size = new System.Drawing.Size(43, 18);
            this.lbl_level.TabIndex = 8;
            this.lbl_level.Text = "Level";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(633, 4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 7;
            this.simpleButton2.Text = "simpleButton2";
            this.simpleButton2.Visible = false;
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // lbl_name
            // 
            this.lbl_name.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lbl_name.Appearance.Options.UseFont = true;
            this.lbl_name.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbl_name.Location = new System.Drawing.Point(362, 4);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(43, 18);
            this.lbl_name.TabIndex = 6;
            this.lbl_name.Text = "Name";
            // 
            // lbl_no
            // 
            this.lbl_no.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lbl_no.Appearance.Options.UseFont = true;
            this.lbl_no.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbl_no.Location = new System.Drawing.Point(313, 4);
            this.lbl_no.Name = "lbl_no";
            this.lbl_no.Size = new System.Drawing.Size(43, 18);
            this.lbl_no.TabIndex = 5;
            this.lbl_no.Text = "No";
            // 
            // lbl_occured_time
            // 
            this.lbl_occured_time.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lbl_occured_time.Appearance.Options.UseFont = true;
            this.lbl_occured_time.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbl_occured_time.Location = new System.Drawing.Point(20, 4);
            this.lbl_occured_time.Name = "lbl_occured_time";
            this.lbl_occured_time.Size = new System.Drawing.Size(102, 18);
            this.lbl_occured_time.TabIndex = 4;
            this.lbl_occured_time.Text = "Occured Time";
            // 
            // grid_occurred_alarm_list
            // 
            this.grid_occurred_alarm_list.Location = new System.Drawing.Point(10, 27);
            this.grid_occurred_alarm_list.MainView = this.gridview_occurred_alarm;
            this.grid_occurred_alarm_list.Name = "grid_occurred_alarm_list";
            this.grid_occurred_alarm_list.Size = new System.Drawing.Size(772, 654);
            this.grid_occurred_alarm_list.TabIndex = 3;
            this.grid_occurred_alarm_list.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridview_occurred_alarm,
            this.advBandedGridView1});
            // 
            // gridview_occurred_alarm
            // 
            this.gridview_occurred_alarm.ColumnPanelRowHeight = 0;
            this.gridview_occurred_alarm.FooterPanelHeight = 0;
            this.gridview_occurred_alarm.GridControl = this.grid_occurred_alarm_list;
            this.gridview_occurred_alarm.GroupRowHeight = 0;
            this.gridview_occurred_alarm.LevelIndent = 0;
            this.gridview_occurred_alarm.Name = "gridview_occurred_alarm";
            this.gridview_occurred_alarm.PreviewIndent = 0;
            this.gridview_occurred_alarm.RowHeight = 0;
            this.gridview_occurred_alarm.ViewCaptionHeight = 0;
            this.gridview_occurred_alarm.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            this.gridview_occurred_alarm.CustomDrawGroupRow += new DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventHandler(this.gridView1_CustomDrawGroupRow_1);
            this.gridview_occurred_alarm.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridView1_RowStyle);
            this.gridview_occurred_alarm.GroupRowExpanding += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gridview_occurred_alarm_GroupRowExpanding);
            this.gridview_occurred_alarm.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.gridView1_CustomRowFilter);
            this.gridview_occurred_alarm.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gridView1_CustomColumnDisplayText);
            this.gridview_occurred_alarm.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // advBandedGridView1
            // 
            this.advBandedGridView1.BandPanelRowHeight = 0;
            this.advBandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.advBandedGridView1.ColumnPanelRowHeight = 0;
            this.advBandedGridView1.FooterPanelHeight = 0;
            this.advBandedGridView1.GridControl = this.grid_occurred_alarm_list;
            this.advBandedGridView1.GroupRowHeight = 0;
            this.advBandedGridView1.LevelIndent = 0;
            this.advBandedGridView1.Name = "advBandedGridView1";
            this.advBandedGridView1.PreviewIndent = 0;
            this.advBandedGridView1.RowHeight = 0;
            this.advBandedGridView1.ViewCaptionHeight = 0;
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            // 
            // timer_most_alarm_check
            // 
            this.timer_most_alarm_check.Tick += new System.EventHandler(this.timer_most_alarm_check_Tick);
            // 
            // frm_occured_alarm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(785, 749);
            this.ControlBox = false;
            this.Controls.Add(this.dpnl_body);
            this.Controls.Add(this.dpnl_bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frm_occured_alarm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Occured_Alarm";
            this.Activated += new System.EventHandler(this.frm_occured_alarm_Activated);
            this.Deactivate += new System.EventHandler(this.frm_occured_alarm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_occured_alarm_FormClosing);
            this.Load += new System.EventHandler(this.frm_occured_alarm_Load);
            this.dpnl_bottom.ResumeLayout(false);
            this.dpnl_body.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_occurred_alarm_list)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridview_occurred_alarm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DoubleBufferedPanel dpnl_bottom;
        private DoubleBufferedPanel dpnl_body;
        private DevExpress.XtraGrid.GridControl grid_occurred_alarm_list;
        private DevExpress.XtraGrid.Views.Grid.GridView gridview_occurred_alarm;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView advBandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraEditors.SimpleButton btn_expand_all;
        private DevExpress.XtraEditors.SimpleButton btn_colappsed_all;
        private DevExpress.XtraEditors.SimpleButton btn_exit;
        private DevExpress.XtraEditors.LabelControl lbl_name;
        private DevExpress.XtraEditors.LabelControl lbl_no;
        private DevExpress.XtraEditors.LabelControl lbl_occured_time;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl lbl_level;
        private System.Windows.Forms.Timer timer_most_alarm_check;
        public DevExpress.XtraEditors.SimpleButton btn_clear_all;
        public DevExpress.XtraEditors.SimpleButton btn_buzzer;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}