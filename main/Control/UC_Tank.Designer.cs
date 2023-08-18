
namespace cds.Control
{
    partial class UC_Tank
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
            this.dbl_main = new cds.DoubleBufferedPanel();
            this.dbl_right = new cds.DoubleBufferedPanel();
            this.dbl_left = new cds.DoubleBufferedPanel();
            this.dbl_right_level_1 = new cds.DoubleBufferedPanel();
            this.doubleBufferedPanel1 = new cds.DoubleBufferedPanel();
            this.doubleBufferedPanel2 = new cds.DoubleBufferedPanel();
            this.doubleBufferedPanel3 = new cds.DoubleBufferedPanel();
            this.doubleBufferedPanel4 = new cds.DoubleBufferedPanel();
            this.dbl_left_level_1 = new cds.DoubleBufferedPanel();
            this.dbl_left_level_2 = new cds.DoubleBufferedPanel();
            this.dbl_left_level_3 = new cds.DoubleBufferedPanel();
            this.dbl_left_level_4 = new cds.DoubleBufferedPanel();
            this.dbl_left_level_5 = new cds.DoubleBufferedPanel();
            this.dbl_left_level_6 = new cds.DoubleBufferedPanel();
            this.dbl_left_level_7 = new cds.DoubleBufferedPanel();
            this.dbl_main.SuspendLayout();
            this.dbl_right.SuspendLayout();
            this.dbl_left.SuspendLayout();
            this.SuspendLayout();
            // 
            // dbl_main
            // 
            this.dbl_main.Controls.Add(this.dbl_left);
            this.dbl_main.Controls.Add(this.dbl_right);
            this.dbl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbl_main.Location = new System.Drawing.Point(0, 0);
            this.dbl_main.Name = "dbl_main";
            this.dbl_main.Size = new System.Drawing.Size(180, 200);
            this.dbl_main.TabIndex = 0;
            // 
            // dbl_right
            // 
            this.dbl_right.Controls.Add(this.doubleBufferedPanel4);
            this.dbl_right.Controls.Add(this.doubleBufferedPanel3);
            this.dbl_right.Controls.Add(this.doubleBufferedPanel2);
            this.dbl_right.Controls.Add(this.doubleBufferedPanel1);
            this.dbl_right.Controls.Add(this.dbl_right_level_1);
            this.dbl_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.dbl_right.Location = new System.Drawing.Point(59, 0);
            this.dbl_right.Name = "dbl_right";
            this.dbl_right.Size = new System.Drawing.Size(121, 200);
            this.dbl_right.TabIndex = 1;
            // 
            // dbl_left
            // 
            this.dbl_left.Controls.Add(this.dbl_left_level_7);
            this.dbl_left.Controls.Add(this.dbl_left_level_6);
            this.dbl_left.Controls.Add(this.dbl_left_level_5);
            this.dbl_left.Controls.Add(this.dbl_left_level_4);
            this.dbl_left.Controls.Add(this.dbl_left_level_3);
            this.dbl_left.Controls.Add(this.dbl_left_level_2);
            this.dbl_left.Controls.Add(this.dbl_left_level_1);
            this.dbl_left.Dock = System.Windows.Forms.DockStyle.Right;
            this.dbl_left.Location = new System.Drawing.Point(-62, 0);
            this.dbl_left.Name = "dbl_left";
            this.dbl_left.Size = new System.Drawing.Size(121, 200);
            this.dbl_left.TabIndex = 2;
            // 
            // dbl_right_level_1
            // 
            this.dbl_right_level_1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dbl_right_level_1.Location = new System.Drawing.Point(0, 160);
            this.dbl_right_level_1.Name = "dbl_right_level_1";
            this.dbl_right_level_1.Size = new System.Drawing.Size(121, 40);
            this.dbl_right_level_1.TabIndex = 2;
            // 
            // doubleBufferedPanel1
            // 
            this.doubleBufferedPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.doubleBufferedPanel1.Location = new System.Drawing.Point(0, 120);
            this.doubleBufferedPanel1.Name = "doubleBufferedPanel1";
            this.doubleBufferedPanel1.Size = new System.Drawing.Size(121, 40);
            this.doubleBufferedPanel1.TabIndex = 3;
            // 
            // doubleBufferedPanel2
            // 
            this.doubleBufferedPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.doubleBufferedPanel2.Location = new System.Drawing.Point(0, 80);
            this.doubleBufferedPanel2.Name = "doubleBufferedPanel2";
            this.doubleBufferedPanel2.Size = new System.Drawing.Size(121, 40);
            this.doubleBufferedPanel2.TabIndex = 4;
            // 
            // doubleBufferedPanel3
            // 
            this.doubleBufferedPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.doubleBufferedPanel3.Location = new System.Drawing.Point(0, 40);
            this.doubleBufferedPanel3.Name = "doubleBufferedPanel3";
            this.doubleBufferedPanel3.Size = new System.Drawing.Size(121, 40);
            this.doubleBufferedPanel3.TabIndex = 5;
            // 
            // doubleBufferedPanel4
            // 
            this.doubleBufferedPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.doubleBufferedPanel4.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferedPanel4.Name = "doubleBufferedPanel4";
            this.doubleBufferedPanel4.Size = new System.Drawing.Size(121, 40);
            this.doubleBufferedPanel4.TabIndex = 6;
            // 
            // dbl_left_level_1
            // 
            this.dbl_left_level_1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dbl_left_level_1.Location = new System.Drawing.Point(0, 172);
            this.dbl_left_level_1.Name = "dbl_left_level_1";
            this.dbl_left_level_1.Size = new System.Drawing.Size(121, 28);
            this.dbl_left_level_1.TabIndex = 3;
            // 
            // dbl_left_level_2
            // 
            this.dbl_left_level_2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dbl_left_level_2.Location = new System.Drawing.Point(0, 144);
            this.dbl_left_level_2.Name = "dbl_left_level_2";
            this.dbl_left_level_2.Size = new System.Drawing.Size(121, 28);
            this.dbl_left_level_2.TabIndex = 4;
            // 
            // dbl_left_level_3
            // 
            this.dbl_left_level_3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dbl_left_level_3.Location = new System.Drawing.Point(0, 116);
            this.dbl_left_level_3.Name = "dbl_left_level_3";
            this.dbl_left_level_3.Size = new System.Drawing.Size(121, 28);
            this.dbl_left_level_3.TabIndex = 5;
            // 
            // dbl_left_level_4
            // 
            this.dbl_left_level_4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dbl_left_level_4.Location = new System.Drawing.Point(0, 88);
            this.dbl_left_level_4.Name = "dbl_left_level_4";
            this.dbl_left_level_4.Size = new System.Drawing.Size(121, 28);
            this.dbl_left_level_4.TabIndex = 6;
            // 
            // dbl_left_level_5
            // 
            this.dbl_left_level_5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dbl_left_level_5.Location = new System.Drawing.Point(0, 60);
            this.dbl_left_level_5.Name = "dbl_left_level_5";
            this.dbl_left_level_5.Size = new System.Drawing.Size(121, 28);
            this.dbl_left_level_5.TabIndex = 7;
            // 
            // dbl_left_level_6
            // 
            this.dbl_left_level_6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dbl_left_level_6.Location = new System.Drawing.Point(0, 32);
            this.dbl_left_level_6.Name = "dbl_left_level_6";
            this.dbl_left_level_6.Size = new System.Drawing.Size(121, 28);
            this.dbl_left_level_6.TabIndex = 8;
            // 
            // dbl_left_level_7
            // 
            this.dbl_left_level_7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dbl_left_level_7.Location = new System.Drawing.Point(0, 4);
            this.dbl_left_level_7.Name = "dbl_left_level_7";
            this.dbl_left_level_7.Size = new System.Drawing.Size(121, 28);
            this.dbl_left_level_7.TabIndex = 9;
            // 
            // UC_Tank
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.dbl_main);
            this.Name = "UC_Tank";
            this.Size = new System.Drawing.Size(180, 200);
            this.dbl_main.ResumeLayout(false);
            this.dbl_right.ResumeLayout(false);
            this.dbl_left.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel dbl_main;
        private DoubleBufferedPanel dbl_left;
        private DoubleBufferedPanel dbl_left_level_7;
        private DoubleBufferedPanel dbl_left_level_6;
        private DoubleBufferedPanel dbl_left_level_5;
        private DoubleBufferedPanel dbl_left_level_4;
        private DoubleBufferedPanel dbl_left_level_3;
        private DoubleBufferedPanel dbl_left_level_2;
        private DoubleBufferedPanel dbl_left_level_1;
        private DoubleBufferedPanel dbl_right;
        private DoubleBufferedPanel doubleBufferedPanel4;
        private DoubleBufferedPanel doubleBufferedPanel3;
        private DoubleBufferedPanel doubleBufferedPanel2;
        private DoubleBufferedPanel doubleBufferedPanel1;
        private DoubleBufferedPanel dbl_right_level_1;
    }
}
