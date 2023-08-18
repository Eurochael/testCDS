
namespace LogManager
{
    partial class uc_targetbox_log
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
			this.dbp_main = new LogManager.DoubleBufferedPanel();
			this.txt_date = new System.Windows.Forms.TextBox();
			this.pnl_padding3 = new LogManager.DoubleBufferedPanel();
			this.btn_loc = new System.Windows.Forms.Button();
			this.pnl_padding2 = new LogManager.DoubleBufferedPanel();
			this.txt_path = new System.Windows.Forms.TextBox();
			this.dlbl_name = new LogManager.DoubleBufferedLabel();
			this.pnl_padding1 = new LogManager.DoubleBufferedPanel();
			this.dbp_main.SuspendLayout();
			this.SuspendLayout();
			// 
			// dbp_main
			// 
			this.dbp_main.Controls.Add(this.txt_date);
			this.dbp_main.Controls.Add(this.pnl_padding3);
			this.dbp_main.Controls.Add(this.btn_loc);
			this.dbp_main.Controls.Add(this.pnl_padding2);
			this.dbp_main.Controls.Add(this.txt_path);
			this.dbp_main.Controls.Add(this.dlbl_name);
			this.dbp_main.Controls.Add(this.pnl_padding1);
			this.dbp_main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dbp_main.Location = new System.Drawing.Point(0, 0);
			this.dbp_main.Name = "dbp_main";
			this.dbp_main.Size = new System.Drawing.Size(529, 21);
			this.dbp_main.TabIndex = 0;
			// 
			// txt_date
			// 
			this.txt_date.Dock = System.Windows.Forms.DockStyle.Left;
			this.txt_date.Location = new System.Drawing.Point(458, 0);
			this.txt_date.Name = "txt_date";
			this.txt_date.Size = new System.Drawing.Size(41, 21);
			this.txt_date.TabIndex = 11;
			this.txt_date.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txt_date.TextChanged += new System.EventHandler(this.txt_date_TextChanged);
			this.txt_date.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_date_KeyPress);
			// 
			// pnl_padding3
			// 
			this.pnl_padding3.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnl_padding3.Location = new System.Drawing.Point(443, 0);
			this.pnl_padding3.Name = "pnl_padding3";
			this.pnl_padding3.Size = new System.Drawing.Size(15, 21);
			this.pnl_padding3.TabIndex = 10;
			// 
			// btn_loc
			// 
			this.btn_loc.Dock = System.Windows.Forms.DockStyle.Left;
			this.btn_loc.Location = new System.Drawing.Point(406, 0);
			this.btn_loc.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
			this.btn_loc.Name = "btn_loc";
			this.btn_loc.Size = new System.Drawing.Size(37, 21);
			this.btn_loc.TabIndex = 9;
			this.btn_loc.Text = "...";
			this.btn_loc.UseVisualStyleBackColor = true;
			this.btn_loc.Click += new System.EventHandler(this.btn_loc_Click);
			// 
			// pnl_padding2
			// 
			this.pnl_padding2.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnl_padding2.Location = new System.Drawing.Point(391, 0);
			this.pnl_padding2.Name = "pnl_padding2";
			this.pnl_padding2.Size = new System.Drawing.Size(15, 21);
			this.pnl_padding2.TabIndex = 8;
			// 
			// txt_path
			// 
			this.txt_path.Dock = System.Windows.Forms.DockStyle.Left;
			this.txt_path.Location = new System.Drawing.Point(103, 0);
			this.txt_path.Name = "txt_path";
			this.txt_path.Size = new System.Drawing.Size(288, 21);
			this.txt_path.TabIndex = 6;
			this.txt_path.TextChanged += new System.EventHandler(this.txt_path_TextChanged);
			// 
			// dlbl_name
			// 
			this.dlbl_name.Dock = System.Windows.Forms.DockStyle.Left;
			this.dlbl_name.Location = new System.Drawing.Point(13, 0);
			this.dlbl_name.Name = "dlbl_name";
			this.dlbl_name.Size = new System.Drawing.Size(90, 21);
			this.dlbl_name.TabIndex = 5;
			this.dlbl_name.Text = "Target01 Path :";
			this.dlbl_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pnl_padding1
			// 
			this.pnl_padding1.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnl_padding1.Location = new System.Drawing.Point(0, 0);
			this.pnl_padding1.Name = "pnl_padding1";
			this.pnl_padding1.Size = new System.Drawing.Size(13, 21);
			this.pnl_padding1.TabIndex = 4;
			// 
			// uc_targetbox_log
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dbp_main);
			this.Name = "uc_targetbox_log";
			this.Size = new System.Drawing.Size(529, 21);
			this.Load += new System.EventHandler(this.uc_targetbox_Load);
			this.dbp_main.ResumeLayout(false);
			this.dbp_main.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel dbp_main;
        private System.Windows.Forms.TextBox txt_path;
        private DoubleBufferedLabel dlbl_name;
        private DoubleBufferedPanel pnl_padding1;
        private System.Windows.Forms.Button btn_loc;
        private DoubleBufferedPanel pnl_padding2;
		private System.Windows.Forms.TextBox txt_date;
		private DoubleBufferedPanel pnl_padding3;
	}
}
