
namespace cds
{
    partial class UC_ThermoStat_HE_3320C
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
            this.btn_read_pv = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.pnl_tank_b_func = new System.Windows.Forms.Panel();
            this.btn_tank_b_off = new DevExpress.XtraEditors.SimpleButton();
            this.btn_tank_b_on = new DevExpress.XtraEditors.SimpleButton();
            this.panel13 = new System.Windows.Forms.Panel();
            this.pnl_tank_a_func = new System.Windows.Forms.Panel();
            this.btn_tank_a_off = new DevExpress.XtraEditors.SimpleButton();
            this.btn_tank_a_on = new DevExpress.XtraEditors.SimpleButton();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btn_heater_off = new DevExpress.XtraEditors.SimpleButton();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btn_heater_on = new DevExpress.XtraEditors.SimpleButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btn_set_output = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txt_sv_value = new DevExpress.XtraEditors.TextEdit();
            this.btn_sv_set = new DevExpress.XtraEditors.SimpleButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_write_sv_apply = new DevExpress.XtraEditors.SimpleButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_write_alarm_reset = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.pnl_tank_b_func.SuspendLayout();
            this.pnl_tank_a_func.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_sv_value.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_read_pv
            // 
            this.btn_read_pv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_read_pv.Location = new System.Drawing.Point(2, 23);
            this.btn_read_pv.Name = "btn_read_pv";
            this.btn_read_pv.Size = new System.Drawing.Size(266, 40);
            this.btn_read_pv.TabIndex = 0;
            this.btn_read_pv.Text = "Read Supply PV";
            this.btn_read_pv.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btn_read_pv);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(270, 65);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "Read";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.pnl_tank_b_func);
            this.groupControl2.Controls.Add(this.panel13);
            this.groupControl2.Controls.Add(this.pnl_tank_a_func);
            this.groupControl2.Controls.Add(this.panel11);
            this.groupControl2.Controls.Add(this.panel8);
            this.groupControl2.Controls.Add(this.panel9);
            this.groupControl2.Controls.Add(this.panel7);
            this.groupControl2.Controls.Add(this.panel6);
            this.groupControl2.Controls.Add(this.panel5);
            this.groupControl2.Controls.Add(this.panel4);
            this.groupControl2.Controls.Add(this.panel2);
            this.groupControl2.Controls.Add(this.panel3);
            this.groupControl2.Controls.Add(this.panel1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 65);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(270, 535);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Write";
            // 
            // pnl_tank_b_func
            // 
            this.pnl_tank_b_func.Controls.Add(this.btn_tank_b_off);
            this.pnl_tank_b_func.Controls.Add(this.btn_tank_b_on);
            this.pnl_tank_b_func.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_tank_b_func.Location = new System.Drawing.Point(2, 397);
            this.pnl_tank_b_func.Name = "pnl_tank_b_func";
            this.pnl_tank_b_func.Size = new System.Drawing.Size(266, 40);
            this.pnl_tank_b_func.TabIndex = 12;
            this.pnl_tank_b_func.Visible = false;
            // 
            // btn_tank_b_off
            // 
            this.btn_tank_b_off.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_tank_b_off.Location = new System.Drawing.Point(141, 0);
            this.btn_tank_b_off.Name = "btn_tank_b_off";
            this.btn_tank_b_off.Size = new System.Drawing.Size(125, 40);
            this.btn_tank_b_off.TabIndex = 5;
            this.btn_tank_b_off.Text = "Tank B OFF";
            this.btn_tank_b_off.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_tank_b_on
            // 
            this.btn_tank_b_on.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_tank_b_on.Location = new System.Drawing.Point(0, 0);
            this.btn_tank_b_on.Name = "btn_tank_b_on";
            this.btn_tank_b_on.Size = new System.Drawing.Size(125, 40);
            this.btn_tank_b_on.TabIndex = 4;
            this.btn_tank_b_on.Text = "Tank B ON";
            this.btn_tank_b_on.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel13
            // 
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(2, 387);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(266, 10);
            this.panel13.TabIndex = 11;
            // 
            // pnl_tank_a_func
            // 
            this.pnl_tank_a_func.Controls.Add(this.btn_tank_a_off);
            this.pnl_tank_a_func.Controls.Add(this.btn_tank_a_on);
            this.pnl_tank_a_func.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_tank_a_func.Location = new System.Drawing.Point(2, 347);
            this.pnl_tank_a_func.Name = "pnl_tank_a_func";
            this.pnl_tank_a_func.Size = new System.Drawing.Size(266, 40);
            this.pnl_tank_a_func.TabIndex = 10;
            this.pnl_tank_a_func.Visible = false;
            // 
            // btn_tank_a_off
            // 
            this.btn_tank_a_off.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_tank_a_off.Location = new System.Drawing.Point(141, 0);
            this.btn_tank_a_off.Name = "btn_tank_a_off";
            this.btn_tank_a_off.Size = new System.Drawing.Size(125, 40);
            this.btn_tank_a_off.TabIndex = 3;
            this.btn_tank_a_off.Text = "Tank A OFF";
            this.btn_tank_a_off.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_tank_a_on
            // 
            this.btn_tank_a_on.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_tank_a_on.Location = new System.Drawing.Point(0, 0);
            this.btn_tank_a_on.Name = "btn_tank_a_on";
            this.btn_tank_a_on.Size = new System.Drawing.Size(125, 40);
            this.btn_tank_a_on.TabIndex = 2;
            this.btn_tank_a_on.Text = "Tank A ON";
            this.btn_tank_a_on.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel11
            // 
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(2, 337);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(266, 10);
            this.panel11.TabIndex = 9;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btn_heater_off);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(2, 297);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(266, 40);
            this.panel8.TabIndex = 8;
            // 
            // btn_heater_off
            // 
            this.btn_heater_off.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_heater_off.Location = new System.Drawing.Point(0, 0);
            this.btn_heater_off.Name = "btn_heater_off";
            this.btn_heater_off.Size = new System.Drawing.Size(266, 40);
            this.btn_heater_off.TabIndex = 1;
            this.btn_heater_off.Text = "Heter OFF";
            this.btn_heater_off.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(2, 287);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(266, 10);
            this.panel9.TabIndex = 7;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btn_heater_on);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(2, 247);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(266, 40);
            this.panel7.TabIndex = 6;
            // 
            // btn_heater_on
            // 
            this.btn_heater_on.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_heater_on.Location = new System.Drawing.Point(0, 0);
            this.btn_heater_on.Name = "btn_heater_on";
            this.btn_heater_on.Size = new System.Drawing.Size(266, 40);
            this.btn_heater_on.TabIndex = 1;
            this.btn_heater_on.Text = "Heter ON";
            this.btn_heater_on.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(2, 237);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(266, 10);
            this.panel6.TabIndex = 5;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btn_set_output);
            this.panel5.Controls.Add(this.labelControl1);
            this.panel5.Controls.Add(this.txt_sv_value);
            this.panel5.Controls.Add(this.btn_sv_set);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(2, 123);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(266, 114);
            this.panel5.TabIndex = 3;
            // 
            // btn_set_output
            // 
            this.btn_set_output.Location = new System.Drawing.Point(141, 56);
            this.btn_set_output.Name = "btn_set_output";
            this.btn_set_output.Size = new System.Drawing.Size(125, 50);
            this.btn_set_output.TabIndex = 17;
            this.btn_set_output.Text = "Offset Set";
            this.btn_set_output.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(8, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(127, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "(Ex) 10000 -> 100.00)";
            // 
            // txt_sv_value
            // 
            this.txt_sv_value.EditValue = "0000";
            this.txt_sv_value.Location = new System.Drawing.Point(3, 5);
            this.txt_sv_value.Name = "txt_sv_value";
            this.txt_sv_value.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_sv_value.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txt_sv_value.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txt_sv_value.Properties.MaskSettings.Set("mask", "d");
            this.txt_sv_value.Size = new System.Drawing.Size(132, 20);
            this.txt_sv_value.TabIndex = 2;
            // 
            // btn_sv_set
            // 
            this.btn_sv_set.Location = new System.Drawing.Point(141, 0);
            this.btn_sv_set.Name = "btn_sv_set";
            this.btn_sv_set.Size = new System.Drawing.Size(125, 50);
            this.btn_sv_set.TabIndex = 1;
            this.btn_sv_set.Text = "SV Data Send";
            this.btn_sv_set.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(2, 113);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(266, 10);
            this.panel4.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_write_sv_apply);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 73);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 40);
            this.panel2.TabIndex = 1;
            // 
            // btn_write_sv_apply
            // 
            this.btn_write_sv_apply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_write_sv_apply.Location = new System.Drawing.Point(0, 0);
            this.btn_write_sv_apply.Name = "btn_write_sv_apply";
            this.btn_write_sv_apply.Size = new System.Drawing.Size(266, 40);
            this.btn_write_sv_apply.TabIndex = 1;
            this.btn_write_sv_apply.Text = "SV Data Apply";
            this.btn_write_sv_apply.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(2, 63);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 10);
            this.panel3.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_write_alarm_reset);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 40);
            this.panel1.TabIndex = 0;
            // 
            // btn_write_alarm_reset
            // 
            this.btn_write_alarm_reset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_write_alarm_reset.Location = new System.Drawing.Point(0, 0);
            this.btn_write_alarm_reset.Name = "btn_write_alarm_reset";
            this.btn_write_alarm_reset.Size = new System.Drawing.Size(266, 40);
            this.btn_write_alarm_reset.TabIndex = 1;
            this.btn_write_alarm_reset.Text = "Alarm Reset";
            this.btn_write_alarm_reset.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // UC_ThermoStat_HE_3320C
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "UC_ThermoStat_HE_3320C";
            this.Size = new System.Drawing.Size(270, 600);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.pnl_tank_b_func.ResumeLayout(false);
            this.pnl_tank_a_func.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_sv_value.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_read_pv;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btn_write_alarm_reset;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton btn_write_sv_apply;
        private System.Windows.Forms.Panel panel5;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_sv_value;
        private DevExpress.XtraEditors.SimpleButton btn_sv_set;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel8;
        private DevExpress.XtraEditors.SimpleButton btn_heater_off;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel7;
        private DevExpress.XtraEditors.SimpleButton btn_heater_on;
        private System.Windows.Forms.Panel panel6;
        private DevExpress.XtraEditors.SimpleButton btn_set_output;
        private System.Windows.Forms.Panel pnl_tank_b_func;
        private DevExpress.XtraEditors.SimpleButton btn_tank_b_off;
        private DevExpress.XtraEditors.SimpleButton btn_tank_b_on;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel pnl_tank_a_func;
        private DevExpress.XtraEditors.SimpleButton btn_tank_a_off;
        private DevExpress.XtraEditors.SimpleButton btn_tank_a_on;
        private System.Windows.Forms.Panel panel11;
    }
}
