
namespace cds
{
    partial class UC_FlowMeter_Sonotec
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
            this.btn_totalusage_zeroset = new DevExpress.XtraEditors.SimpleButton();
            this.btn_zeroset = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_read_data
            // 
            this.btn_read_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_read_data.Location = new System.Drawing.Point(0, 0);
            this.btn_read_data.Name = "btn_read_data";
            this.btn_read_data.Size = new System.Drawing.Size(266, 40);
            this.btn_read_data.TabIndex = 0;
            this.btn_read_data.Text = "Read Data";
            this.btn_read_data.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.panel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(270, 115);
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
            this.groupControl2.Controls.Add(this.panel2);
            this.groupControl2.Controls.Add(this.panel4);
            this.groupControl2.Controls.Add(this.panel5);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 115);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(270, 532);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Write";
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
            this.panel5.Controls.Add(this.btn_zeroset);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(2, 23);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(266, 40);
            this.panel5.TabIndex = 7;
            // 
            // btn_totalusage_zeroset
            // 
            this.btn_totalusage_zeroset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_totalusage_zeroset.Location = new System.Drawing.Point(0, 0);
            this.btn_totalusage_zeroset.Name = "btn_totalusage_zeroset";
            this.btn_totalusage_zeroset.Size = new System.Drawing.Size(266, 40);
            this.btn_totalusage_zeroset.TabIndex = 10;
            this.btn_totalusage_zeroset.Text = "TotalUsage ZeroSet";
            this.btn_totalusage_zeroset.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_zeroset
            // 
            this.btn_zeroset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_zeroset.Location = new System.Drawing.Point(0, 0);
            this.btn_zeroset.Name = "btn_zeroset";
            this.btn_zeroset.Size = new System.Drawing.Size(266, 40);
            this.btn_zeroset.TabIndex = 9;
            this.btn_zeroset.Text = "Zero Set";
            this.btn_zeroset.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_totalusage_zeroset);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 73);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 40);
            this.panel2.TabIndex = 11;
            // 
            // UC_FlowMeter_Sonotec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "UC_FlowMeter_Sonotec";
            this.Size = new System.Drawing.Size(270, 600);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_read_data;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btn_totalusage_zeroset;
        private DevExpress.XtraEditors.SimpleButton btn_zeroset;
        private System.Windows.Forms.Panel panel2;
    }
}
