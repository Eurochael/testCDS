
namespace cds
{
    partial class frm_Loading_Appstart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Loading_Appstart));
            this.progressPanel1 = new DevExpress.XtraWaitForm.ProgressPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_title = new DevExpress.XtraEditors.LabelControl();
            this.labelStatus = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lbox_process_log = new DevExpress.XtraEditors.ListBoxControl();
            this.timer_seqeunce = new System.Windows.Forms.Timer(this.components);
            this.pbar = new DevExpress.XtraEditors.ProgressBarControl();
            this.btn_pause = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_retry = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbox_process_log)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressPanel1
            // 
            this.progressPanel1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.progressPanel1.Appearance.Options.UseBackColor = true;
            this.progressPanel1.AppearanceCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.progressPanel1.AppearanceCaption.Options.UseFont = true;
            this.progressPanel1.AppearanceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.progressPanel1.AppearanceDescription.Options.UseFont = true;
            this.progressPanel1.Caption = "Parameter Loading..";
            this.progressPanel1.Description = "20";
            this.progressPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressPanel1.ImageHorzOffset = 20;
            this.progressPanel1.Location = new System.Drawing.Point(0, 18);
            this.progressPanel1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(621, 33);
            this.progressPanel1.TabIndex = 0;
            this.progressPanel1.Text = "progressPanel1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.progressPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 15, 0, 15);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(621, 69);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lbl_title
            // 
            this.lbl_title.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbl_title.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.lbl_title.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.lbl_title.Appearance.Options.UseBackColor = true;
            this.lbl_title.Appearance.Options.UseFont = true;
            this.lbl_title.Appearance.Options.UseTextOptions = true;
            this.lbl_title.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lbl_title.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.lbl_title.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_title.Location = new System.Drawing.Point(0, 0);
            this.lbl_title.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(1196, 48);
            this.lbl_title.TabIndex = 17;
            this.lbl_title.Text = "CDS";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(639, 425);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(55, 14);
            this.labelStatus.TabIndex = 15;
            this.labelStatus.Text = "Starting...";
            this.labelStatus.Visible = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lbox_process_log);
            this.groupControl1.Location = new System.Drawing.Point(12, 61);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1176, 311);
            this.groupControl1.TabIndex = 20;
            this.groupControl1.Text = "Process Log";
            // 
            // lbox_process_log
            // 
            this.lbox_process_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbox_process_log.Location = new System.Drawing.Point(2, 23);
            this.lbox_process_log.Name = "lbox_process_log";
            this.lbox_process_log.Size = new System.Drawing.Size(1172, 286);
            this.lbox_process_log.TabIndex = 0;
            // 
            // timer_seqeunce
            // 
            this.timer_seqeunce.Tick += new System.EventHandler(this.timer_seqeunce_Tick);
            // 
            // pbar
            // 
            this.pbar.Location = new System.Drawing.Point(14, 376);
            this.pbar.Name = "pbar";
            this.pbar.Properties.ShowTitle = true;
            this.pbar.Size = new System.Drawing.Size(1172, 34);
            this.pbar.TabIndex = 21;
            // 
            // btn_pause
            // 
            this.btn_pause.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.btn_pause.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_pause.ImageOptions.Image")));
            this.btn_pause.Location = new System.Drawing.Point(1070, 425);
            this.btn_pause.Name = "btn_pause";
            this.btn_pause.Size = new System.Drawing.Size(116, 68);
            this.btn_pause.TabIndex = 22;
            this.btn_pause.Text = "Pause";
            this.btn_pause.Click += new System.EventHandler(this.btn_pause_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.tableLayoutPanel1);
            this.panelControl1.Location = new System.Drawing.Point(12, 425);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(621, 69);
            this.panelControl1.TabIndex = 23;
            // 
            // btn_retry
            // 
            this.btn_retry.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.btn_retry.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_retry.ImageOptions.Image")));
            this.btn_retry.Location = new System.Drawing.Point(948, 426);
            this.btn_retry.Name = "btn_retry";
            this.btn_retry.Size = new System.Drawing.Size(116, 68);
            this.btn_retry.TabIndex = 24;
            this.btn_retry.Text = "retry";
            this.btn_retry.Click += new System.EventHandler(this.btn_retry_Click);
            // 
            // frm_Loading_Appstart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1196, 506);
            this.ControlBox = false;
            this.Controls.Add(this.btn_retry);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.btn_pause);
            this.Controls.Add(this.pbar);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.labelStatus);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frm_Loading_Appstart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lbox_process_log)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraWaitForm.ProgressPanel progressPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.LabelControl lbl_title;
        private DevExpress.XtraEditors.LabelControl labelStatus;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ListBoxControl lbox_process_log;
        private System.Windows.Forms.Timer timer_seqeunce;
        private DevExpress.XtraEditors.ProgressBarControl pbar;
        private DevExpress.XtraEditors.SimpleButton btn_pause;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_retry;
    }
}
