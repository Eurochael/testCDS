
namespace LogManager
{
    partial class uc_targetbox_work
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
			this.pnl_padding2 = new LogManager.DoubleBufferedPanel();
			this.dlbl_name = new LogManager.DoubleBufferedLabel();
			this.pnl_padding1 = new LogManager.DoubleBufferedPanel();
			this.dlbl_count = new LogManager.DoubleBufferedLabel();
			this.pnl_padding3 = new LogManager.DoubleBufferedPanel();
			this.dlbl_delcount = new LogManager.DoubleBufferedLabel();
			this.dlbl_deletedate = new LogManager.DoubleBufferedLabel();
			this.doubleBufferedPanel1 = new LogManager.DoubleBufferedPanel();
			this.dbp_main.SuspendLayout();
			this.SuspendLayout();
			// 
			// dbp_main
			// 
			this.dbp_main.Controls.Add(this.dlbl_deletedate);
			this.dbp_main.Controls.Add(this.doubleBufferedPanel1);
			this.dbp_main.Controls.Add(this.dlbl_delcount);
			this.dbp_main.Controls.Add(this.pnl_padding3);
			this.dbp_main.Controls.Add(this.dlbl_count);
			this.dbp_main.Controls.Add(this.pnl_padding2);
			this.dbp_main.Controls.Add(this.dlbl_name);
			this.dbp_main.Controls.Add(this.pnl_padding1);
			this.dbp_main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dbp_main.Location = new System.Drawing.Point(0, 0);
			this.dbp_main.Name = "dbp_main";
			this.dbp_main.Size = new System.Drawing.Size(529, 21);
			this.dbp_main.TabIndex = 0;
			// 
			// pnl_padding2
			// 
			this.pnl_padding2.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnl_padding2.Location = new System.Drawing.Point(103, 0);
			this.pnl_padding2.Name = "pnl_padding2";
			this.pnl_padding2.Size = new System.Drawing.Size(15, 21);
			this.pnl_padding2.TabIndex = 8;
			// 
			// dlbl_name
			// 
			this.dlbl_name.Dock = System.Windows.Forms.DockStyle.Left;
			this.dlbl_name.Location = new System.Drawing.Point(13, 0);
			this.dlbl_name.Name = "dlbl_name";
			this.dlbl_name.Size = new System.Drawing.Size(90, 21);
			this.dlbl_name.TabIndex = 5;
			this.dlbl_name.Text = "Target log01 :";
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
			// dlbl_count
			// 
			this.dlbl_count.Dock = System.Windows.Forms.DockStyle.Left;
			this.dlbl_count.Location = new System.Drawing.Point(118, 0);
			this.dlbl_count.Name = "dlbl_count";
			this.dlbl_count.Size = new System.Drawing.Size(105, 21);
			this.dlbl_count.TabIndex = 11;
			this.dlbl_count.Text = "log_count";
			this.dlbl_count.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnl_padding3
			// 
			this.pnl_padding3.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnl_padding3.Location = new System.Drawing.Point(223, 0);
			this.pnl_padding3.Name = "pnl_padding3";
			this.pnl_padding3.Size = new System.Drawing.Size(15, 21);
			this.pnl_padding3.TabIndex = 12;
			// 
			// dlbl_delcount
			// 
			this.dlbl_delcount.Dock = System.Windows.Forms.DockStyle.Left;
			this.dlbl_delcount.Location = new System.Drawing.Point(238, 0);
			this.dlbl_delcount.Name = "dlbl_delcount";
			this.dlbl_delcount.Size = new System.Drawing.Size(102, 21);
			this.dlbl_delcount.TabIndex = 13;
			this.dlbl_delcount.Text = "delete_log_count";
			this.dlbl_delcount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// dlbl_deletedate
			// 
			this.dlbl_deletedate.Dock = System.Windows.Forms.DockStyle.Left;
			this.dlbl_deletedate.Location = new System.Drawing.Point(355, 0);
			this.dlbl_deletedate.Name = "dlbl_deletedate";
			this.dlbl_deletedate.Size = new System.Drawing.Size(147, 21);
			this.dlbl_deletedate.TabIndex = 15;
			this.dlbl_deletedate.Text = "delete_date";
			this.dlbl_deletedate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// doubleBufferedPanel1
			// 
			this.doubleBufferedPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.doubleBufferedPanel1.Location = new System.Drawing.Point(340, 0);
			this.doubleBufferedPanel1.Name = "doubleBufferedPanel1";
			this.doubleBufferedPanel1.Size = new System.Drawing.Size(15, 21);
			this.doubleBufferedPanel1.TabIndex = 14;
			// 
			// uc_targetbox_work
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dbp_main);
			this.Name = "uc_targetbox_work";
			this.Size = new System.Drawing.Size(529, 21);
			this.Load += new System.EventHandler(this.uc_targetbox_Load);
			this.dbp_main.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel dbp_main;
        private DoubleBufferedLabel dlbl_name;
        private DoubleBufferedPanel pnl_padding1;
        private DoubleBufferedPanel pnl_padding2;
		private DoubleBufferedLabel dlbl_delcount;
		private DoubleBufferedPanel pnl_padding3;
		private DoubleBufferedLabel dlbl_count;
		private DoubleBufferedLabel dlbl_deletedate;
		private DoubleBufferedPanel doubleBufferedPanel1;
	}
}
