
namespace cds
{
    partial class UC_PumpController_PB12
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btn_set_storke_fl = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txt_sv_value = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_sv_value.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_read_data
            // 
            this.btn_read_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_read_data.Location = new System.Drawing.Point(0, 0);
            this.btn_read_data.Name = "btn_read_data";
            this.btn_read_data.Size = new System.Drawing.Size(266, 40);
            this.btn_read_data.TabIndex = 0;
            this.btn_read_data.Text = "Read Stroke Count";
            this.btn_read_data.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.panel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(270, 67);
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
            this.groupControl2.Controls.Add(this.panel4);
            this.groupControl2.Controls.Add(this.panel5);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 67);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(270, 463);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Write";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(2, 113);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(266, 10);
            this.panel4.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.labelControl2);
            this.panel5.Controls.Add(this.labelControl1);
            this.panel5.Controls.Add(this.txt_sv_value);
            this.panel5.Controls.Add(this.btn_set_storke_fl);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(2, 23);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(266, 90);
            this.panel5.TabIndex = 7;
            // 
            // btn_set_storke_fl
            // 
            this.btn_set_storke_fl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_set_storke_fl.Location = new System.Drawing.Point(0, 50);
            this.btn_set_storke_fl.Name = "btn_set_storke_fl";
            this.btn_set_storke_fl.Size = new System.Drawing.Size(266, 40);
            this.btn_set_storke_fl.TabIndex = 9;
            this.btn_set_storke_fl.Text = "Set";
            this.btn_set_storke_fl.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(87, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(119, 14);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "(Ex) 1.234L -> 1234)";
            // 
            // txt_sv_value
            // 
            this.txt_sv_value.EditValue = "0000";
            this.txt_sv_value.Location = new System.Drawing.Point(82, 3);
            this.txt_sv_value.Name = "txt_sv_value";
            this.txt_sv_value.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_sv_value.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txt_sv_value.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txt_sv_value.Properties.MaskSettings.Set("mask", "d");
            this.txt_sv_value.Size = new System.Drawing.Size(132, 20);
            this.txt_sv_value.TabIndex = 10;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(5, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(71, 14);
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "1Stroke-FL : ";
            // 
            // UC_PumpController_PB12
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "UC_PumpController_PB12";
            this.Size = new System.Drawing.Size(270, 600);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_sv_value.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_read_data;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btn_set_storke_fl;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_sv_value;
    }
}
