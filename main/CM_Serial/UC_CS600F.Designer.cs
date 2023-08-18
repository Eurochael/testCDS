
namespace cds
{
    partial class UC_CS600F
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_CS600F));
            this.btn_read_data = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btn_cm_change_parallel = new DevExpress.XtraEditors.SimpleButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pictureEdit3 = new DevExpress.XtraEditors.PictureEdit();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btn_calibration_end = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btn_calibration_start = new DevExpress.XtraEditors.SimpleButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_cm_change_serial = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_read_data
            // 
            this.btn_read_data.Location = new System.Drawing.Point(2, 23);
            this.btn_read_data.Name = "btn_read_data";
            this.btn_read_data.Size = new System.Drawing.Size(266, 40);
            this.btn_read_data.TabIndex = 0;
            this.btn_read_data.Text = "Measure Data(DD)";
            this.btn_read_data.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btn_read_data);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(270, 71);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "Read";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.panel7);
            this.groupControl2.Controls.Add(this.panel8);
            this.groupControl2.Controls.Add(this.panel5);
            this.groupControl2.Controls.Add(this.panel6);
            this.groupControl2.Controls.Add(this.panel2);
            this.groupControl2.Controls.Add(this.panel4);
            this.groupControl2.Controls.Add(this.panel3);
            this.groupControl2.Controls.Add(this.panel1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 71);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(270, 535);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Write";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.labelControl1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(2, 315);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(266, 44);
            this.panel7.TabIndex = 7;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btn_cm_change_parallel);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(2, 275);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(266, 40);
            this.panel8.TabIndex = 6;
            // 
            // btn_cm_change_parallel
            // 
            this.btn_cm_change_parallel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_cm_change_parallel.Location = new System.Drawing.Point(0, 0);
            this.btn_cm_change_parallel.Name = "btn_cm_change_parallel";
            this.btn_cm_change_parallel.Size = new System.Drawing.Size(266, 40);
            this.btn_cm_change_parallel.TabIndex = 1;
            this.btn_cm_change_parallel.Text = "Communication Mode Change(parallel)";
            this.btn_cm_change_parallel.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.pictureEdit3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(2, 231);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(266, 44);
            this.panel5.TabIndex = 5;
            // 
            // pictureEdit3
            // 
            this.pictureEdit3.EditValue = ((object)(resources.GetObject("pictureEdit3.EditValue")));
            this.pictureEdit3.Location = new System.Drawing.Point(109, 4);
            this.pictureEdit3.Name = "pictureEdit3";
            this.pictureEdit3.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit3.Size = new System.Drawing.Size(51, 36);
            this.pictureEdit3.TabIndex = 10;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btn_calibration_end);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(2, 191);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(266, 40);
            this.panel6.TabIndex = 4;
            // 
            // btn_calibration_end
            // 
            this.btn_calibration_end.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_calibration_end.Location = new System.Drawing.Point(0, 0);
            this.btn_calibration_end.Name = "btn_calibration_end";
            this.btn_calibration_end.Size = new System.Drawing.Size(266, 40);
            this.btn_calibration_end.TabIndex = 1;
            this.btn_calibration_end.Text = "Calibration End";
            this.btn_calibration_end.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureEdit2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 147);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 44);
            this.panel2.TabIndex = 3;
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.EditValue = ((object)(resources.GetObject("pictureEdit2.EditValue")));
            this.pictureEdit2.Location = new System.Drawing.Point(109, 4);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit2.Size = new System.Drawing.Size(51, 36);
            this.pictureEdit2.TabIndex = 9;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btn_calibration_start);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(2, 107);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(266, 40);
            this.panel4.TabIndex = 2;
            // 
            // btn_calibration_start
            // 
            this.btn_calibration_start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_calibration_start.Location = new System.Drawing.Point(0, 0);
            this.btn_calibration_start.Name = "btn_calibration_start";
            this.btn_calibration_start.Size = new System.Drawing.Size(266, 40);
            this.btn_calibration_start.TabIndex = 1;
            this.btn_calibration_start.Text = "Calibration Start";
            this.btn_calibration_start.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pictureEdit1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(2, 63);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 44);
            this.panel3.TabIndex = 1;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(109, 4);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Size = new System.Drawing.Size(51, 36);
            this.pictureEdit1.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_cm_change_serial);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 40);
            this.panel1.TabIndex = 0;
            // 
            // btn_cm_change_serial
            // 
            this.btn_cm_change_serial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_cm_change_serial.Location = new System.Drawing.Point(0, 0);
            this.btn_cm_change_serial.Name = "btn_cm_change_serial";
            this.btn_cm_change_serial.Size = new System.Drawing.Size(266, 40);
            this.btn_cm_change_serial.TabIndex = 1;
            this.btn_cm_change_serial.Text = "Communication Mode Change(Serial)";
            this.btn_cm_change_serial.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(62, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(143, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Please Proceed in order";
            // 
            // UC_CS600F
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "UC_CS600F";
            this.Size = new System.Drawing.Size(270, 600);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_read_data;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btn_cm_change_serial;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private DevExpress.XtraEditors.SimpleButton btn_cm_change_parallel;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private DevExpress.XtraEditors.SimpleButton btn_calibration_start;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private DevExpress.XtraEditors.SimpleButton btn_calibration_end;
        private DevExpress.XtraEditors.PictureEdit pictureEdit3;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
