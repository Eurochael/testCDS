namespace Config_Create
{
    partial class Form1
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_create_di = new System.Windows.Forms.Button();
            this.fpnl_di = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Initialize = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_create_do = new System.Windows.Forms.Button();
            this.fpnl_do = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_create_ai = new System.Windows.Forms.Button();
            this.fpnl_ai = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_create_ao = new System.Windows.Forms.Button();
            this.fpnl_ao = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_create_di);
            this.groupBox1.Controls.Add(this.fpnl_di);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1250, 859);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "di";
            // 
            // btn_create_di
            // 
            this.btn_create_di.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_create_di.Location = new System.Drawing.Point(3, 817);
            this.btn_create_di.Name = "btn_create_di";
            this.btn_create_di.Size = new System.Drawing.Size(1244, 39);
            this.btn_create_di.TabIndex = 4;
            this.btn_create_di.Text = "Create";
            this.btn_create_di.UseVisualStyleBackColor = true;
            this.btn_create_di.Click += new System.EventHandler(this.btn_create_di_Click);
            // 
            // fpnl_di
            // 
            this.fpnl_di.AutoScroll = true;
            this.fpnl_di.Dock = System.Windows.Forms.DockStyle.Top;
            this.fpnl_di.Font = new System.Drawing.Font("Tahoma", 7F);
            this.fpnl_di.Location = new System.Drawing.Point(3, 17);
            this.fpnl_di.Name = "fpnl_di";
            this.fpnl_di.Size = new System.Drawing.Size(1244, 800);
            this.fpnl_di.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_Initialize);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1264, 94);
            this.panel1.TabIndex = 6;
            // 
            // btn_Initialize
            // 
            this.btn_Initialize.Location = new System.Drawing.Point(12, 12);
            this.btn_Initialize.Name = "btn_Initialize";
            this.btn_Initialize.Size = new System.Drawing.Size(242, 43);
            this.btn_Initialize.TabIndex = 6;
            this.btn_Initialize.Text = "Initial";
            this.btn_Initialize.UseVisualStyleBackColor = true;
            this.btn_Initialize.Click += new System.EventHandler(this.btn_Initialize_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_create_do);
            this.groupBox2.Controls.Add(this.fpnl_do);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1250, 859);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "do";
            // 
            // btn_create_do
            // 
            this.btn_create_do.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_create_do.Location = new System.Drawing.Point(3, 817);
            this.btn_create_do.Name = "btn_create_do";
            this.btn_create_do.Size = new System.Drawing.Size(1244, 39);
            this.btn_create_do.TabIndex = 5;
            this.btn_create_do.Text = "Create";
            this.btn_create_do.UseVisualStyleBackColor = true;
            this.btn_create_do.Click += new System.EventHandler(this.btn_create_do_Click);
            // 
            // fpnl_do
            // 
            this.fpnl_do.AutoScroll = true;
            this.fpnl_do.Dock = System.Windows.Forms.DockStyle.Top;
            this.fpnl_do.Font = new System.Drawing.Font("Tahoma", 7F);
            this.fpnl_do.Location = new System.Drawing.Point(3, 17);
            this.fpnl_do.Name = "fpnl_do";
            this.fpnl_do.Size = new System.Drawing.Size(1244, 800);
            this.fpnl_do.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_create_ai);
            this.groupBox3.Controls.Add(this.fpnl_ai);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1256, 865);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ai";
            // 
            // btn_create_ai
            // 
            this.btn_create_ai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_create_ai.Location = new System.Drawing.Point(3, 817);
            this.btn_create_ai.Name = "btn_create_ai";
            this.btn_create_ai.Size = new System.Drawing.Size(1250, 45);
            this.btn_create_ai.TabIndex = 5;
            this.btn_create_ai.Text = "Create";
            this.btn_create_ai.UseVisualStyleBackColor = true;
            this.btn_create_ai.Click += new System.EventHandler(this.btn_create_ai_Click);
            // 
            // fpnl_ai
            // 
            this.fpnl_ai.AutoScroll = true;
            this.fpnl_ai.Dock = System.Windows.Forms.DockStyle.Top;
            this.fpnl_ai.Font = new System.Drawing.Font("Tahoma", 7F);
            this.fpnl_ai.Location = new System.Drawing.Point(3, 17);
            this.fpnl_ai.Name = "fpnl_ai";
            this.fpnl_ai.Size = new System.Drawing.Size(1250, 800);
            this.fpnl_ai.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_create_ao);
            this.groupBox4.Controls.Add(this.fpnl_ao);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1256, 865);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "ao";
            // 
            // btn_create_ao
            // 
            this.btn_create_ao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_create_ao.Location = new System.Drawing.Point(3, 817);
            this.btn_create_ao.Name = "btn_create_ao";
            this.btn_create_ao.Size = new System.Drawing.Size(1250, 45);
            this.btn_create_ao.TabIndex = 5;
            this.btn_create_ao.Text = "Create";
            this.btn_create_ao.UseVisualStyleBackColor = true;
            this.btn_create_ao.Click += new System.EventHandler(this.btn_create_ao_Click);
            // 
            // fpnl_ao
            // 
            this.fpnl_ao.AutoScroll = true;
            this.fpnl_ao.Dock = System.Windows.Forms.DockStyle.Top;
            this.fpnl_ao.Font = new System.Drawing.Font("Tahoma", 7F);
            this.fpnl_ao.Location = new System.Drawing.Point(3, 17);
            this.fpnl_ao.Name = "fpnl_ao";
            this.fpnl_ao.Size = new System.Drawing.Size(1250, 800);
            this.fpnl_ao.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 94);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1264, 891);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1256, 865);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DI";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1256, 865);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DO";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1256, 865);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "AI";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1256, 865);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "AO";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label1.Location = new System.Drawing.Point(316, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(942, 74);
            this.label1.TabIndex = 7;
            this.label1.Text = "반드시 관리자 권한으로 실행";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 985);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config Create";
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_create_di;
        private System.Windows.Forms.FlowLayoutPanel fpnl_di;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_Initialize;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_create_do;
        private System.Windows.Forms.FlowLayoutPanel fpnl_do;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_create_ai;
        private System.Windows.Forms.FlowLayoutPanel fpnl_ai;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_create_ao;
        private System.Windows.Forms.FlowLayoutPanel fpnl_ao;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
    }
}

