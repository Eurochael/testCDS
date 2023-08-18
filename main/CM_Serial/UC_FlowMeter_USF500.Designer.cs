
namespace cds
{
    partial class UC_FlowMeter_USF500
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btn_read_main_status = new DevExpress.XtraEditors.SimpleButton();
            this.cmb_address_read = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btn_read_ch2_data = new DevExpress.XtraEditors.SimpleButton();
            this.btn_read_ch1_data = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cmb_address_write = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btn_totalusage_zeroset = new DevExpress.XtraEditors.SimpleButton();
            this.btn_zeroset = new DevExpress.XtraEditors.SimpleButton();
            this.cmb_ch = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_address_read.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_address_write.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_ch.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btn_read_main_status);
            this.groupControl1.Controls.Add(this.cmb_address_read);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.btn_read_ch2_data);
            this.groupControl1.Controls.Add(this.btn_read_ch1_data);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(274, 190);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "Read";
            // 
            // btn_read_main_status
            // 
            this.btn_read_main_status.Location = new System.Drawing.Point(70, 52);
            this.btn_read_main_status.Name = "btn_read_main_status";
            this.btn_read_main_status.Size = new System.Drawing.Size(194, 40);
            this.btn_read_main_status.TabIndex = 20;
            this.btn_read_main_status.Text = "Read Main Status";
            this.btn_read_main_status.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // cmb_address_read
            // 
            this.cmb_address_read.Location = new System.Drawing.Point(70, 26);
            this.cmb_address_read.Name = "cmb_address_read";
            this.cmb_address_read.Properties.Appearance.Options.UseTextOptions = true;
            this.cmb_address_read.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.cmb_address_read.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_address_read.Properties.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cmb_address_read.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb_address_read.Size = new System.Drawing.Size(194, 20);
            this.cmb_address_read.TabIndex = 19;
            this.cmb_address_read.SelectedIndexChanged += new System.EventHandler(this.cmb_address_read_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(9, 29);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(55, 14);
            this.labelControl2.TabIndex = 18;
            this.labelControl2.Text = "Address : ";
            // 
            // btn_read_ch2_data
            // 
            this.btn_read_ch2_data.Location = new System.Drawing.Point(70, 142);
            this.btn_read_ch2_data.Name = "btn_read_ch2_data";
            this.btn_read_ch2_data.Size = new System.Drawing.Size(194, 40);
            this.btn_read_ch2_data.TabIndex = 11;
            this.btn_read_ch2_data.Text = "Read Data CH2 Flow && Status";
            this.btn_read_ch2_data.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_read_ch1_data
            // 
            this.btn_read_ch1_data.Location = new System.Drawing.Point(70, 96);
            this.btn_read_ch1_data.Name = "btn_read_ch1_data";
            this.btn_read_ch1_data.Size = new System.Drawing.Size(194, 40);
            this.btn_read_ch1_data.TabIndex = 0;
            this.btn_read_ch1_data.Text = "Read Data CH1 Flow && Status";
            this.btn_read_ch1_data.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.panel4);
            this.groupControl2.Controls.Add(this.panel5);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 190);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(274, 532);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Write";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(2, 178);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(270, 81);
            this.panel4.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cmb_address_write);
            this.panel5.Controls.Add(this.labelControl4);
            this.panel5.Controls.Add(this.btn_totalusage_zeroset);
            this.panel5.Controls.Add(this.btn_zeroset);
            this.panel5.Controls.Add(this.cmb_ch);
            this.panel5.Controls.Add(this.labelControl1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(2, 23);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(270, 155);
            this.panel5.TabIndex = 7;
            // 
            // cmb_address_write
            // 
            this.cmb_address_write.Location = new System.Drawing.Point(68, 3);
            this.cmb_address_write.Name = "cmb_address_write";
            this.cmb_address_write.Properties.Appearance.Options.UseTextOptions = true;
            this.cmb_address_write.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.cmb_address_write.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_address_write.Properties.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cmb_address_write.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb_address_write.Size = new System.Drawing.Size(194, 20);
            this.cmb_address_write.TabIndex = 17;
            this.cmb_address_write.SelectedIndexChanged += new System.EventHandler(this.cmb_address_wrtie_SelectedIndexChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(7, 6);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(55, 14);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "Address : ";
            // 
            // btn_totalusage_zeroset
            // 
            this.btn_totalusage_zeroset.Location = new System.Drawing.Point(68, 100);
            this.btn_totalusage_zeroset.Name = "btn_totalusage_zeroset";
            this.btn_totalusage_zeroset.Size = new System.Drawing.Size(194, 40);
            this.btn_totalusage_zeroset.TabIndex = 10;
            this.btn_totalusage_zeroset.Text = "TotalUsage ZeroSet";
            this.btn_totalusage_zeroset.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_zeroset
            // 
            this.btn_zeroset.Location = new System.Drawing.Point(68, 54);
            this.btn_zeroset.Name = "btn_zeroset";
            this.btn_zeroset.Size = new System.Drawing.Size(194, 40);
            this.btn_zeroset.TabIndex = 9;
            this.btn_zeroset.Text = "Zero Set";
            this.btn_zeroset.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // cmb_ch
            // 
            this.cmb_ch.Location = new System.Drawing.Point(68, 28);
            this.cmb_ch.Name = "cmb_ch";
            this.cmb_ch.Properties.Appearance.Options.UseTextOptions = true;
            this.cmb_ch.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.cmb_ch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_ch.Properties.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cmb_ch.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb_ch.Size = new System.Drawing.Size(194, 20);
            this.cmb_ch.TabIndex = 5;
            this.cmb_ch.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.cmb_ch_QueryPopUp);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(35, 30);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(27, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "CH : ";
            // 
            // UC_FlowMeter_USF500
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "UC_FlowMeter_USF500";
            this.Size = new System.Drawing.Size(274, 600);
            this.Load += new System.EventHandler(this.UC_FlowMeter_USF500_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_address_read.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_address_write.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_ch.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.Panel panel5;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_ch;
        private System.Windows.Forms.Panel panel4;
        private DevExpress.XtraEditors.SimpleButton btn_read_ch2_data;
        private DevExpress.XtraEditors.SimpleButton btn_totalusage_zeroset;
        private DevExpress.XtraEditors.SimpleButton btn_zeroset;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_address_read;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_read_ch1_data;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_address_write;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton btn_read_main_status;
    }
}
