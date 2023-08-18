
namespace cds
{
    partial class UC_HeatExchanger
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
            this.btn_read_AllData = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_heat_exchanger_off = new DevExpress.XtraEditors.SimpleButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btn_heat_exchanger_on = new DevExpress.XtraEditors.SimpleButton();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btn_set_offset_chem_out = new DevExpress.XtraEditors.SimpleButton();
            this.btn_set_offset_chem_in = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txt_sv_value = new DevExpress.XtraEditors.TextEdit();
            this.btn_sv_set = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_sv_value.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_read_AllData
            // 
            this.btn_read_AllData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_read_AllData.Location = new System.Drawing.Point(0, 0);
            this.btn_read_AllData.Name = "btn_read_AllData";
            this.btn_read_AllData.Size = new System.Drawing.Size(266, 40);
            this.btn_read_AllData.TabIndex = 0;
            this.btn_read_AllData.Text = "Read All Data";
            this.btn_read_AllData.Click += new System.EventHandler(this.btn_read_pv_Click);
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
            this.panel1.Controls.Add(this.btn_read_AllData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 40);
            this.panel1.TabIndex = 9;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.panel2);
            this.groupControl2.Controls.Add(this.panel3);
            this.groupControl2.Controls.Add(this.panel7);
            this.groupControl2.Controls.Add(this.panel9);
            this.groupControl2.Controls.Add(this.panel5);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 69);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(270, 532);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Write";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_heat_exchanger_off);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 250);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 40);
            this.panel2.TabIndex = 14;
            // 
            // btn_heat_exchanger_off
            // 
            this.btn_heat_exchanger_off.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_heat_exchanger_off.Location = new System.Drawing.Point(0, 0);
            this.btn_heat_exchanger_off.Name = "btn_heat_exchanger_off";
            this.btn_heat_exchanger_off.Size = new System.Drawing.Size(266, 40);
            this.btn_heat_exchanger_off.TabIndex = 1;
            this.btn_heat_exchanger_off.Text = "Heter OFF";
            this.btn_heat_exchanger_off.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(2, 240);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 10);
            this.panel3.TabIndex = 13;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btn_heat_exchanger_on);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(2, 200);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(266, 40);
            this.panel7.TabIndex = 12;
            // 
            // btn_heat_exchanger_on
            // 
            this.btn_heat_exchanger_on.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_heat_exchanger_on.Location = new System.Drawing.Point(0, 0);
            this.btn_heat_exchanger_on.Name = "btn_heat_exchanger_on";
            this.btn_heat_exchanger_on.Size = new System.Drawing.Size(266, 40);
            this.btn_heat_exchanger_on.TabIndex = 1;
            this.btn_heat_exchanger_on.Text = "Heter ON";
            this.btn_heat_exchanger_on.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(2, 190);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(266, 10);
            this.panel9.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btn_set_offset_chem_out);
            this.panel5.Controls.Add(this.btn_set_offset_chem_in);
            this.panel5.Controls.Add(this.labelControl1);
            this.panel5.Controls.Add(this.txt_sv_value);
            this.panel5.Controls.Add(this.btn_sv_set);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(2, 23);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(266, 167);
            this.panel5.TabIndex = 4;
            // 
            // btn_set_offset_chem_out
            // 
            this.btn_set_offset_chem_out.Location = new System.Drawing.Point(129, 112);
            this.btn_set_offset_chem_out.Name = "btn_set_offset_chem_out";
            this.btn_set_offset_chem_out.Size = new System.Drawing.Size(136, 50);
            this.btn_set_offset_chem_out.TabIndex = 18;
            this.btn_set_offset_chem_out.Text = "Offset Set(Chem Out)";
            this.btn_set_offset_chem_out.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_set_offset_chem_in
            // 
            this.btn_set_offset_chem_in.Location = new System.Drawing.Point(130, 56);
            this.btn_set_offset_chem_in.Name = "btn_set_offset_chem_in";
            this.btn_set_offset_chem_in.Size = new System.Drawing.Size(136, 50);
            this.btn_set_offset_chem_in.TabIndex = 17;
            this.btn_set_offset_chem_in.Text = "Offset Set(Chem In)";
            this.btn_set_offset_chem_in.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(8, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(113, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "(Ex) 1000 -> 100.0)";
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
            this.txt_sv_value.Size = new System.Drawing.Size(120, 20);
            this.txt_sv_value.TabIndex = 2;
            // 
            // btn_sv_set
            // 
            this.btn_sv_set.Location = new System.Drawing.Point(130, 0);
            this.btn_sv_set.Name = "btn_sv_set";
            this.btn_sv_set.Size = new System.Drawing.Size(136, 50);
            this.btn_sv_set.TabIndex = 1;
            this.btn_sv_set.Text = "SV Data Send";
            this.btn_sv_set.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // UC_HeatExchanger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "UC_HeatExchanger";
            this.Size = new System.Drawing.Size(270, 600);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_sv_value.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_read_AllData;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private DevExpress.XtraEditors.SimpleButton btn_set_offset_chem_out;
        private DevExpress.XtraEditors.SimpleButton btn_set_offset_chem_in;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_sv_value;
        private DevExpress.XtraEditors.SimpleButton btn_sv_set;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton btn_heat_exchanger_off;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel7;
        private DevExpress.XtraEditors.SimpleButton btn_heat_exchanger_on;
    }
}
