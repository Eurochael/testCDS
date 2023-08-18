
namespace cds
{
    partial class frm_popup_page
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
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.btn_communication = new DevExpress.XtraEditors.SimpleButton();
            this.doubleBufferedPanel1 = new cds.DoubleBufferedPanel();
            this.btn_iomonitor = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new cds.DoubleBufferedPanel();
            this.btn_schematic = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.btn_mixingstep = new DevExpress.XtraEditors.SimpleButton();
            this.panel4 = new cds.DoubleBufferedPanel();
            this.btn_parameter = new DevExpress.XtraEditors.SimpleButton();
            this.panel3 = new cds.DoubleBufferedPanel();
            this.btn_alarm = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.btn_totalusagelog = new DevExpress.XtraEditors.SimpleButton();
            this.panel5 = new cds.DoubleBufferedPanel();
            this.btn_alarmlog = new DevExpress.XtraEditors.SimpleButton();
            this.panel6 = new cds.DoubleBufferedPanel();
            this.btn_eventlog = new DevExpress.XtraEditors.SimpleButton();
            this.panel7 = new cds.DoubleBufferedPanel();
            this.btn_trendlog = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new cds.DoubleBufferedPanel();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            this.xtraTabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(186, 328);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3});
            this.xtraTabControl1.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.btn_communication);
            this.xtraTabPage1.Controls.Add(this.doubleBufferedPanel1);
            this.xtraTabPage1.Controls.Add(this.btn_iomonitor);
            this.xtraTabPage1.Controls.Add(this.panel2);
            this.xtraTabPage1.Controls.Add(this.btn_schematic);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.xtraTabPage1.Size = new System.Drawing.Size(184, 304);
            this.xtraTabPage1.Text = "Monitor";
            // 
            // btn_communication
            // 
            this.btn_communication.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_communication.Appearance.Options.UseFont = true;
            this.btn_communication.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_communication.Location = new System.Drawing.Point(3, 143);
            this.btn_communication.Name = "btn_communication";
            this.btn_communication.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btn_communication.Size = new System.Drawing.Size(178, 60);
            this.btn_communication.TabIndex = 13;
            this.btn_communication.Text = "Communication";
            this.btn_communication.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // doubleBufferedPanel1
            // 
            this.doubleBufferedPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.doubleBufferedPanel1.Font = new System.Drawing.Font("Tahoma", 1F);
            this.doubleBufferedPanel1.Location = new System.Drawing.Point(3, 133);
            this.doubleBufferedPanel1.Name = "doubleBufferedPanel1";
            this.doubleBufferedPanel1.Size = new System.Drawing.Size(178, 10);
            this.doubleBufferedPanel1.TabIndex = 12;
            // 
            // btn_iomonitor
            // 
            this.btn_iomonitor.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_iomonitor.Appearance.Options.UseFont = true;
            this.btn_iomonitor.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_iomonitor.Location = new System.Drawing.Point(3, 73);
            this.btn_iomonitor.Name = "btn_iomonitor";
            this.btn_iomonitor.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btn_iomonitor.Size = new System.Drawing.Size(178, 60);
            this.btn_iomonitor.TabIndex = 11;
            this.btn_iomonitor.Text = "IO - Monitor";
            this.btn_iomonitor.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("Tahoma", 1F);
            this.panel2.Location = new System.Drawing.Point(3, 63);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(178, 10);
            this.panel2.TabIndex = 10;
            // 
            // btn_schematic
            // 
            this.btn_schematic.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_schematic.Appearance.Options.UseFont = true;
            this.btn_schematic.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_schematic.Location = new System.Drawing.Point(3, 3);
            this.btn_schematic.Name = "btn_schematic";
            this.btn_schematic.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btn_schematic.Size = new System.Drawing.Size(178, 60);
            this.btn_schematic.TabIndex = 9;
            this.btn_schematic.Text = "Main";
            this.btn_schematic.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.btn_mixingstep);
            this.xtraTabPage2.Controls.Add(this.panel4);
            this.xtraTabPage2.Controls.Add(this.btn_parameter);
            this.xtraTabPage2.Controls.Add(this.panel3);
            this.xtraTabPage2.Controls.Add(this.btn_alarm);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.xtraTabPage2.Size = new System.Drawing.Size(184, 304);
            this.xtraTabPage2.Text = "Configuration";
            // 
            // btn_mixingstep
            // 
            this.btn_mixingstep.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_mixingstep.Appearance.Options.UseFont = true;
            this.btn_mixingstep.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_mixingstep.Location = new System.Drawing.Point(3, 143);
            this.btn_mixingstep.Name = "btn_mixingstep";
            this.btn_mixingstep.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btn_mixingstep.Size = new System.Drawing.Size(178, 60);
            this.btn_mixingstep.TabIndex = 19;
            this.btn_mixingstep.Text = "Mixing Step";
            this.btn_mixingstep.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Font = new System.Drawing.Font("Tahoma", 1F);
            this.panel4.Location = new System.Drawing.Point(3, 133);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(178, 10);
            this.panel4.TabIndex = 18;
            // 
            // btn_parameter
            // 
            this.btn_parameter.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_parameter.Appearance.Options.UseFont = true;
            this.btn_parameter.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_parameter.Location = new System.Drawing.Point(3, 73);
            this.btn_parameter.Name = "btn_parameter";
            this.btn_parameter.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btn_parameter.Size = new System.Drawing.Size(178, 60);
            this.btn_parameter.TabIndex = 17;
            this.btn_parameter.Text = "Parameter";
            this.btn_parameter.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Font = new System.Drawing.Font("Tahoma", 1F);
            this.panel3.Location = new System.Drawing.Point(3, 63);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(178, 10);
            this.panel3.TabIndex = 16;
            // 
            // btn_alarm
            // 
            this.btn_alarm.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_alarm.Appearance.Options.UseFont = true;
            this.btn_alarm.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_alarm.Location = new System.Drawing.Point(3, 3);
            this.btn_alarm.Name = "btn_alarm";
            this.btn_alarm.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btn_alarm.Size = new System.Drawing.Size(178, 60);
            this.btn_alarm.TabIndex = 15;
            this.btn_alarm.Text = "Alarm";
            this.btn_alarm.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.btn_totalusagelog);
            this.xtraTabPage3.Controls.Add(this.panel5);
            this.xtraTabPage3.Controls.Add(this.btn_alarmlog);
            this.xtraTabPage3.Controls.Add(this.panel6);
            this.xtraTabPage3.Controls.Add(this.btn_eventlog);
            this.xtraTabPage3.Controls.Add(this.panel7);
            this.xtraTabPage3.Controls.Add(this.btn_trendlog);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.xtraTabPage3.Size = new System.Drawing.Size(184, 304);
            this.xtraTabPage3.Text = "Log";
            // 
            // btn_totalusagelog
            // 
            this.btn_totalusagelog.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_totalusagelog.Appearance.Options.UseFont = true;
            this.btn_totalusagelog.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_totalusagelog.Location = new System.Drawing.Point(3, 213);
            this.btn_totalusagelog.Name = "btn_totalusagelog";
            this.btn_totalusagelog.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btn_totalusagelog.Size = new System.Drawing.Size(178, 60);
            this.btn_totalusagelog.TabIndex = 24;
            this.btn_totalusagelog.Text = "Total Usage Log";
            this.btn_totalusagelog.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Font = new System.Drawing.Font("Tahoma", 1F);
            this.panel5.Location = new System.Drawing.Point(3, 203);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(178, 10);
            this.panel5.TabIndex = 23;
            // 
            // btn_alarmlog
            // 
            this.btn_alarmlog.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_alarmlog.Appearance.Options.UseFont = true;
            this.btn_alarmlog.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_alarmlog.Location = new System.Drawing.Point(3, 143);
            this.btn_alarmlog.Name = "btn_alarmlog";
            this.btn_alarmlog.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btn_alarmlog.Size = new System.Drawing.Size(178, 60);
            this.btn_alarmlog.TabIndex = 22;
            this.btn_alarmlog.Text = "Alarm Log";
            this.btn_alarmlog.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Font = new System.Drawing.Font("Tahoma", 1F);
            this.panel6.Location = new System.Drawing.Point(3, 133);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(178, 10);
            this.panel6.TabIndex = 21;
            // 
            // btn_eventlog
            // 
            this.btn_eventlog.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_eventlog.Appearance.Options.UseFont = true;
            this.btn_eventlog.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_eventlog.Location = new System.Drawing.Point(3, 73);
            this.btn_eventlog.Name = "btn_eventlog";
            this.btn_eventlog.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btn_eventlog.Size = new System.Drawing.Size(178, 60);
            this.btn_eventlog.TabIndex = 20;
            this.btn_eventlog.Text = "Event Log";
            this.btn_eventlog.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Font = new System.Drawing.Font("Tahoma", 1F);
            this.panel7.Location = new System.Drawing.Point(3, 63);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(178, 10);
            this.panel7.TabIndex = 19;
            // 
            // btn_trendlog
            // 
            this.btn_trendlog.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_trendlog.Appearance.Options.UseFont = true;
            this.btn_trendlog.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_trendlog.Location = new System.Drawing.Point(3, 3);
            this.btn_trendlog.Name = "btn_trendlog";
            this.btn_trendlog.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btn_trendlog.Size = new System.Drawing.Size(178, 60);
            this.btn_trendlog.TabIndex = 18;
            this.btn_trendlog.Text = "Trend Log";
            this.btn_trendlog.Click += new System.EventHandler(this.btn_schematic_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.xtraTabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 328);
            this.panel1.TabIndex = 1;
            // 
            // frm_popup_page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 334);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_popup_page";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Select Funtion";
            this.Deactivate += new System.EventHandler(this.frm_popup_page_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_popup_page_FormClosed);
            this.Load += new System.EventHandler(this.frm_popup_page_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraEditors.SimpleButton btn_iomonitor;
        private cds.DoubleBufferedPanel panel2;
        private DevExpress.XtraEditors.SimpleButton btn_schematic;
        private cds.DoubleBufferedPanel panel1;
        private DevExpress.XtraEditors.SimpleButton btn_mixingstep;
        private cds.DoubleBufferedPanel panel4;
        private DevExpress.XtraEditors.SimpleButton btn_parameter;
        private cds.DoubleBufferedPanel panel3;
        private DevExpress.XtraEditors.SimpleButton btn_alarm;
        private DevExpress.XtraEditors.SimpleButton btn_totalusagelog;
        private cds.DoubleBufferedPanel panel5;
        private DevExpress.XtraEditors.SimpleButton btn_alarmlog;
        private cds.DoubleBufferedPanel panel6;
        private DevExpress.XtraEditors.SimpleButton btn_eventlog;
        private cds.DoubleBufferedPanel panel7;
        private DevExpress.XtraEditors.SimpleButton btn_trendlog;
        private DevExpress.XtraEditors.SimpleButton btn_communication;
        private DoubleBufferedPanel doubleBufferedPanel1;
    }
}