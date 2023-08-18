
namespace cds
{
    partial class UC_TempController_M74
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
            this.cmb_address_read = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cmb_ch = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btn_set_offset = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btn_set_output = new DevExpress.XtraEditors.SimpleButton();
            this.btn_at_stop = new DevExpress.XtraEditors.SimpleButton();
            this.btn_at_run = new DevExpress.XtraEditors.SimpleButton();
            this.cmb_address_write = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txt_sv_value = new DevExpress.XtraEditors.TextEdit();
            this.btn_read_data = new DevExpress.XtraEditors.SimpleButton();
            this.btn_stop = new DevExpress.XtraEditors.SimpleButton();
            this.btn_run = new DevExpress.XtraEditors.SimpleButton();
            this.btn_set_value = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_address_read.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_ch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_address_write.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_sv_value.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_read_pv
            // 
            this.btn_read_pv.Location = new System.Drawing.Point(71, 51);
            this.btn_read_pv.Name = "btn_read_pv";
            this.btn_read_pv.Size = new System.Drawing.Size(194, 35);
            this.btn_read_pv.TabIndex = 0;
            this.btn_read_pv.Text = "Read Data CH1 To CH4";
            this.btn_read_pv.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cmb_address_read);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.btn_read_pv);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(270, 92);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "Read";
            // 
            // cmb_address_read
            // 
            this.cmb_address_read.Location = new System.Drawing.Point(71, 25);
            this.cmb_address_read.Name = "cmb_address_read";
            this.cmb_address_read.Properties.Appearance.Options.UseTextOptions = true;
            this.cmb_address_read.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.cmb_address_read.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_address_read.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cmb_address_read.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb_address_read.Size = new System.Drawing.Size(194, 20);
            this.cmb_address_read.TabIndex = 15;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(10, 28);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(55, 14);
            this.labelControl4.TabIndex = 14;
            this.labelControl4.Text = "Address : ";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.panel4);
            this.groupControl2.Controls.Add(this.panel5);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 92);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(270, 532);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Write";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(2, 402);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(266, 11);
            this.panel4.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cmb_ch);
            this.panel5.Controls.Add(this.btn_set_offset);
            this.panel5.Controls.Add(this.labelControl5);
            this.panel5.Controls.Add(this.btn_set_output);
            this.panel5.Controls.Add(this.btn_at_stop);
            this.panel5.Controls.Add(this.btn_at_run);
            this.panel5.Controls.Add(this.cmb_address_write);
            this.panel5.Controls.Add(this.labelControl3);
            this.panel5.Controls.Add(this.labelControl2);
            this.panel5.Controls.Add(this.txt_sv_value);
            this.panel5.Controls.Add(this.btn_read_data);
            this.panel5.Controls.Add(this.btn_stop);
            this.panel5.Controls.Add(this.btn_run);
            this.panel5.Controls.Add(this.btn_set_value);
            this.panel5.Controls.Add(this.labelControl1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(2, 23);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(266, 379);
            this.panel5.TabIndex = 7;
            // 
            // cmb_ch
            // 
            this.cmb_ch.Location = new System.Drawing.Point(69, 33);
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
            this.cmb_ch.TabIndex = 19;
            this.cmb_ch.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.cmb_ch_QueryPopUp);
            // 
            // btn_set_offset
            // 
            this.btn_set_offset.Location = new System.Drawing.Point(179, 151);
            this.btn_set_offset.Name = "btn_set_offset";
            this.btn_set_offset.Size = new System.Drawing.Size(86, 40);
            this.btn_set_offset.TabIndex = 18;
            this.btn_set_offset.Text = "Offset Set";
            this.btn_set_offset.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl5.Location = new System.Drawing.Point(36, 104);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(137, 41);
            this.labelControl5.TabIndex = 17;
            this.labelControl5.Text = "(Ex) CH1-> Out 1 = 1\r\n      CH2 -> Out 1 = 21";
            // 
            // btn_set_output
            // 
            this.btn_set_output.Location = new System.Drawing.Point(179, 105);
            this.btn_set_output.Name = "btn_set_output";
            this.btn_set_output.Size = new System.Drawing.Size(86, 40);
            this.btn_set_output.TabIndex = 16;
            this.btn_set_output.Text = "Outport Set";
            this.btn_set_output.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_at_stop
            // 
            this.btn_at_stop.Location = new System.Drawing.Point(153, 323);
            this.btn_at_stop.Name = "btn_at_stop";
            this.btn_at_stop.Size = new System.Drawing.Size(110, 40);
            this.btn_at_stop.TabIndex = 15;
            this.btn_at_stop.Text = "AT STOP";
            this.btn_at_stop.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_at_run
            // 
            this.btn_at_run.Location = new System.Drawing.Point(38, 323);
            this.btn_at_run.Name = "btn_at_run";
            this.btn_at_run.Size = new System.Drawing.Size(110, 40);
            this.btn_at_run.TabIndex = 14;
            this.btn_at_run.Text = "AT RUN";
            this.btn_at_run.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // cmb_address_write
            // 
            this.cmb_address_write.Location = new System.Drawing.Point(69, 7);
            this.cmb_address_write.Name = "cmb_address_write";
            this.cmb_address_write.Properties.Appearance.Options.UseTextOptions = true;
            this.cmb_address_write.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.cmb_address_write.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_address_write.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cmb_address_write.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb_address_write.Size = new System.Drawing.Size(194, 20);
            this.cmb_address_write.TabIndex = 13;
            this.cmb_address_write.SelectedIndexChanged += new System.EventHandler(this.cmb_address_write_SelectedIndexChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(8, 10);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(55, 14);
            this.labelControl3.TabIndex = 12;
            this.labelControl3.Text = "Address : ";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(57, 85);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(113, 14);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "(Ex) 100.0 -> 1000)";
            // 
            // txt_sv_value
            // 
            this.txt_sv_value.Location = new System.Drawing.Point(69, 59);
            this.txt_sv_value.Name = "txt_sv_value";
            this.txt_sv_value.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_sv_value.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txt_sv_value.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txt_sv_value.Properties.MaskSettings.Set("mask", "d");
            this.txt_sv_value.Size = new System.Drawing.Size(104, 20);
            this.txt_sv_value.TabIndex = 10;
            // 
            // btn_read_data
            // 
            this.btn_read_data.Location = new System.Drawing.Point(38, 231);
            this.btn_read_data.Name = "btn_read_data";
            this.btn_read_data.Size = new System.Drawing.Size(225, 40);
            this.btn_read_data.TabIndex = 9;
            this.btn_read_data.Text = "Data Read";
            this.btn_read_data.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(153, 277);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(110, 40);
            this.btn_stop.TabIndex = 8;
            this.btn_stop.Text = "STOP";
            this.btn_stop.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_run
            // 
            this.btn_run.Location = new System.Drawing.Point(38, 277);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(110, 40);
            this.btn_run.TabIndex = 7;
            this.btn_run.Text = "RUN";
            this.btn_run.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // btn_set_value
            // 
            this.btn_set_value.Location = new System.Drawing.Point(179, 59);
            this.btn_set_value.Name = "btn_set_value";
            this.btn_set_value.Size = new System.Drawing.Size(86, 40);
            this.btn_set_value.TabIndex = 6;
            this.btn_set_value.Text = "Temp Set";
            this.btn_set_value.Click += new System.EventHandler(this.btn_read_pv_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(36, 36);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(27, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "CH : ";
            // 
            // UC_TempController_M74
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "UC_TempController_M74";
            this.Size = new System.Drawing.Size(270, 600);
            this.Load += new System.EventHandler(this.UC_TempController_M74_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_address_read.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_ch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_address_write.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_sv_value.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_read_pv;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.Panel panel5;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_set_value;
        private System.Windows.Forms.Panel panel4;
        private DevExpress.XtraEditors.SimpleButton btn_read_data;
        private DevExpress.XtraEditors.SimpleButton btn_stop;
        private DevExpress.XtraEditors.SimpleButton btn_run;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txt_sv_value;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_address_read;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_address_write;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btn_at_stop;
        private DevExpress.XtraEditors.SimpleButton btn_at_run;
        private DevExpress.XtraEditors.SimpleButton btn_set_output;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btn_set_offset;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_ch;
    }
}
