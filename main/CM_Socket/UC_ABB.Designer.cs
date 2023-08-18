
namespace cds
{
    partial class UC_ABB
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_read_data = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btn_reference_trigger_off = new DevExpress.XtraEditors.SimpleButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btn_reference_trigger_on = new DevExpress.XtraEditors.SimpleButton();
            this.btn_chemistry_ch2_select = new DevExpress.XtraEditors.SimpleButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btn_chemistry_ch1_select = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_read_do_status = new DevExpress.XtraEditors.SimpleButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.btn_online = new DevExpress.XtraEditors.SimpleButton();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.btn_offline = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel13.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_read_data
            // 
            this.btn_read_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_read_data.Location = new System.Drawing.Point(0, 0);
            this.btn_read_data.Name = "btn_read_data";
            this.btn_read_data.Size = new System.Drawing.Size(266, 40);
            this.btn_read_data.TabIndex = 0;
            this.btn_read_data.Text = "Read Data Property 1 To 4";
            this.btn_read_data.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.panel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(270, 69);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "Read";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_read_data);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 40);
            this.panel1.TabIndex = 9;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.panel12);
            this.groupControl2.Controls.Add(this.panel13);
            this.groupControl2.Controls.Add(this.panel10);
            this.groupControl2.Controls.Add(this.panel11);
            this.groupControl2.Controls.Add(this.panel3);
            this.groupControl2.Controls.Add(this.panel2);
            this.groupControl2.Controls.Add(this.btn_chemistry_ch2_select);
            this.groupControl2.Controls.Add(this.panel8);
            this.groupControl2.Controls.Add(this.panel9);
            this.groupControl2.Controls.Add(this.panel6);
            this.groupControl2.Controls.Add(this.panel7);
            this.groupControl2.Controls.Add(this.panel4);
            this.groupControl2.Controls.Add(this.panel5);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 69);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(270, 532);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Write";
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(2, 163);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(266, 10);
            this.panel8.TabIndex = 16;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.btn_reference_trigger_off);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(2, 123);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(266, 40);
            this.panel9.TabIndex = 15;
            // 
            // btn_reference_trigger_off
            // 
            this.btn_reference_trigger_off.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_reference_trigger_off.Location = new System.Drawing.Point(0, 0);
            this.btn_reference_trigger_off.Name = "btn_reference_trigger_off";
            this.btn_reference_trigger_off.Size = new System.Drawing.Size(266, 40);
            this.btn_reference_trigger_off.TabIndex = 10;
            this.btn_reference_trigger_off.Text = "Reference Trigger OFF";
            this.btn_reference_trigger_off.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(2, 113);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(266, 10);
            this.panel6.TabIndex = 14;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btn_reference_trigger_on);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(2, 73);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(266, 40);
            this.panel7.TabIndex = 13;
            // 
            // btn_reference_trigger_on
            // 
            this.btn_reference_trigger_on.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_reference_trigger_on.Location = new System.Drawing.Point(0, 0);
            this.btn_reference_trigger_on.Name = "btn_reference_trigger_on";
            this.btn_reference_trigger_on.Size = new System.Drawing.Size(266, 40);
            this.btn_reference_trigger_on.TabIndex = 10;
            this.btn_reference_trigger_on.Text = "Reference Trigger ON";
            this.btn_reference_trigger_on.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_chemistry_ch2_select
            // 
            this.btn_chemistry_ch2_select.Location = new System.Drawing.Point(-1, 375);
            this.btn_chemistry_ch2_select.Name = "btn_chemistry_ch2_select";
            this.btn_chemistry_ch2_select.Size = new System.Drawing.Size(266, 40);
            this.btn_chemistry_ch2_select.TabIndex = 10;
            this.btn_chemistry_ch2_select.Text = "Chemistry CH2 Select(NH4OH)";
            this.btn_chemistry_ch2_select.Visible = false;
            this.btn_chemistry_ch2_select.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(2, 63);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(266, 10);
            this.panel4.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btn_chemistry_ch1_select);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(2, 23);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(266, 40);
            this.panel5.TabIndex = 7;
            // 
            // btn_chemistry_ch1_select
            // 
            this.btn_chemistry_ch1_select.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_chemistry_ch1_select.Location = new System.Drawing.Point(0, 0);
            this.btn_chemistry_ch1_select.Name = "btn_chemistry_ch1_select";
            this.btn_chemistry_ch1_select.Size = new System.Drawing.Size(266, 40);
            this.btn_chemistry_ch1_select.TabIndex = 9;
            this.btn_chemistry_ch1_select.Text = "Chemistry CH1 Select(SC1)";
            this.btn_chemistry_ch1_select.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_read_do_status);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 173);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 40);
            this.panel2.TabIndex = 18;
            // 
            // btn_read_do_status
            // 
            this.btn_read_do_status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_read_do_status.Location = new System.Drawing.Point(0, 0);
            this.btn_read_do_status.Name = "btn_read_do_status";
            this.btn_read_do_status.Size = new System.Drawing.Size(266, 40);
            this.btn_read_do_status.TabIndex = 10;
            this.btn_read_do_status.Text = "Read DO Status";
            this.btn_read_do_status.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(2, 213);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 10);
            this.panel3.TabIndex = 19;
            // 
            // panel10
            // 
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(2, 263);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(266, 10);
            this.panel10.TabIndex = 21;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.btn_online);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(2, 223);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(266, 40);
            this.panel11.TabIndex = 20;
            // 
            // btn_online
            // 
            this.btn_online.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_online.Location = new System.Drawing.Point(0, 0);
            this.btn_online.Name = "btn_online";
            this.btn_online.Size = new System.Drawing.Size(266, 40);
            this.btn_online.TabIndex = 10;
            this.btn_online.Text = "Online";
            this.btn_online.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel12
            // 
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(2, 313);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(266, 10);
            this.panel12.TabIndex = 23;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.btn_offline);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(2, 273);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(266, 40);
            this.panel13.TabIndex = 22;
            // 
            // btn_offline
            // 
            this.btn_offline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_offline.Location = new System.Drawing.Point(0, 0);
            this.btn_offline.Name = "btn_offline";
            this.btn_offline.Size = new System.Drawing.Size(266, 40);
            this.btn_offline.TabIndex = 10;
            this.btn_offline.Text = "Offline";
            this.btn_offline.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // UC_ABB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "UC_ABB";
            this.Size = new System.Drawing.Size(270, 600);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_read_data;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btn_chemistry_ch2_select;
        private DevExpress.XtraEditors.SimpleButton btn_chemistry_ch1_select;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private DevExpress.XtraEditors.SimpleButton btn_reference_trigger_off;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private DevExpress.XtraEditors.SimpleButton btn_reference_trigger_on;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.SimpleButton btn_read_do_status;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel13;
        private DevExpress.XtraEditors.SimpleButton btn_offline;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private DevExpress.XtraEditors.SimpleButton btn_online;
    }
}
