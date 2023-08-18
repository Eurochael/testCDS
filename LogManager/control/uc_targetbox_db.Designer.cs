
namespace LogManager
{
    partial class uc_targetbox_db
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
			this.pnl_padding2 = new LogManager.DoubleBufferedPanel();
			this.txt_field = new System.Windows.Forms.TextBox();
			this.doubleBufferedPanel2 = new LogManager.DoubleBufferedPanel();
			this.txt_table = new System.Windows.Forms.TextBox();
			this.doubleBufferedPanel1 = new LogManager.DoubleBufferedPanel();
			this.txt_db = new System.Windows.Forms.TextBox();
			this.dlbl_name = new LogManager.DoubleBufferedLabel();
			this.pnl_padding1 = new LogManager.DoubleBufferedPanel();
			this.dbp_main.SuspendLayout();
			this.SuspendLayout();
			// 
			// dbp_main
			// 
			this.dbp_main.Controls.Add(this.txt_date);
			this.dbp_main.Controls.Add(this.pnl_padding2);
			this.dbp_main.Controls.Add(this.txt_field);
			this.dbp_main.Controls.Add(this.doubleBufferedPanel2);
			this.dbp_main.Controls.Add(this.txt_table);
			this.dbp_main.Controls.Add(this.doubleBufferedPanel1);
			this.dbp_main.Controls.Add(this.txt_db);
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
			this.txt_date.TabIndex = 17;
			this.txt_date.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txt_date.TextChanged += new System.EventHandler(this.txt_date_TextChanged);
			this.txt_date.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_date_KeyPress);
			// 
			// pnl_padding2
			// 
			this.pnl_padding2.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnl_padding2.Location = new System.Drawing.Point(443, 0);
			this.pnl_padding2.Name = "pnl_padding2";
			this.pnl_padding2.Size = new System.Drawing.Size(15, 21);
			this.pnl_padding2.TabIndex = 16;
			// 
			// txt_field
			// 
			this.txt_field.Dock = System.Windows.Forms.DockStyle.Left;
			this.txt_field.Location = new System.Drawing.Point(320, 0);
			this.txt_field.Name = "txt_field";
			this.txt_field.Size = new System.Drawing.Size(123, 21);
			this.txt_field.TabIndex = 15;
			this.txt_field.TextChanged += new System.EventHandler(this.txt_field_TextChanged);
			// 
			// doubleBufferedPanel2
			// 
			this.doubleBufferedPanel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.doubleBufferedPanel2.Location = new System.Drawing.Point(305, 0);
			this.doubleBufferedPanel2.Name = "doubleBufferedPanel2";
			this.doubleBufferedPanel2.Size = new System.Drawing.Size(15, 21);
			this.doubleBufferedPanel2.TabIndex = 14;
			// 
			// txt_table
			// 
			this.txt_table.Dock = System.Windows.Forms.DockStyle.Left;
			this.txt_table.Location = new System.Drawing.Point(192, 0);
			this.txt_table.Name = "txt_table";
			this.txt_table.Size = new System.Drawing.Size(113, 21);
			this.txt_table.TabIndex = 13;
			this.txt_table.TextChanged += new System.EventHandler(this.txt_table_TextChanged);
			// 
			// doubleBufferedPanel1
			// 
			this.doubleBufferedPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.doubleBufferedPanel1.Location = new System.Drawing.Point(177, 0);
			this.doubleBufferedPanel1.Name = "doubleBufferedPanel1";
			this.doubleBufferedPanel1.Size = new System.Drawing.Size(15, 21);
			this.doubleBufferedPanel1.TabIndex = 12;
			// 
			// txt_db
			// 
			this.txt_db.Dock = System.Windows.Forms.DockStyle.Left;
			this.txt_db.Location = new System.Drawing.Point(103, 0);
			this.txt_db.Name = "txt_db";
			this.txt_db.Size = new System.Drawing.Size(74, 21);
			this.txt_db.TabIndex = 6;
			this.txt_db.TextChanged += new System.EventHandler(this.txt_db_TextChanged);
			// 
			// dlbl_name
			// 
			this.dlbl_name.Dock = System.Windows.Forms.DockStyle.Left;
			this.dlbl_name.Location = new System.Drawing.Point(4, 0);
			this.dlbl_name.Name = "dlbl_name";
			this.dlbl_name.Size = new System.Drawing.Size(99, 21);
			this.dlbl_name.TabIndex = 5;
			this.dlbl_name.Text = "Target01 Query :";
			this.dlbl_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pnl_padding1
			// 
			this.pnl_padding1.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnl_padding1.Location = new System.Drawing.Point(0, 0);
			this.pnl_padding1.Name = "pnl_padding1";
			this.pnl_padding1.Size = new System.Drawing.Size(4, 21);
			this.pnl_padding1.TabIndex = 4;
			// 
			// uc_targetbox_db
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dbp_main);
			this.Name = "uc_targetbox_db";
			this.Size = new System.Drawing.Size(529, 21);
			this.Load += new System.EventHandler(this.uc_targetbox_Load);
			this.dbp_main.ResumeLayout(false);
			this.dbp_main.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel dbp_main;
        private System.Windows.Forms.TextBox txt_db;
        private DoubleBufferedLabel dlbl_name;
        private DoubleBufferedPanel pnl_padding1;
		private System.Windows.Forms.TextBox txt_field;
		private DoubleBufferedPanel doubleBufferedPanel2;
		private System.Windows.Forms.TextBox txt_table;
		private DoubleBufferedPanel doubleBufferedPanel1;
		private System.Windows.Forms.TextBox txt_date;
		private DoubleBufferedPanel pnl_padding2;
	}
}
