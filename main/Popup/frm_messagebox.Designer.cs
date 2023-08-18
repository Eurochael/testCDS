
namespace cds
{
    partial class frm_messagebox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_messagebox));
            this.pnl_main = new cds.DoubleBufferedPanel();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ok = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_title = new DevExpress.XtraEditors.LabelControl();
            this.btn_ok2 = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_main
            // 
            this.pnl_main.BackColor = System.Drawing.Color.White;
            this.pnl_main.Controls.Add(this.btn_ok2);
            this.pnl_main.Controls.Add(this.btn_cancel);
            this.pnl_main.Controls.Add(this.btn_ok);
            this.pnl_main.Controls.Add(this.lbl_title);
            this.pnl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_main.Location = new System.Drawing.Point(10, 10);
            this.pnl_main.Name = "pnl_main";
            this.pnl_main.Size = new System.Drawing.Size(578, 188);
            this.pnl_main.TabIndex = 0;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btn_cancel.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btn_cancel.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_cancel.Appearance.Options.UseBackColor = true;
            this.btn_cancel.Appearance.Options.UseFont = true;
            this.btn_cancel.Location = new System.Drawing.Point(400, 120);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(120, 50);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btn_ok.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btn_ok.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_ok.Appearance.Options.UseBackColor = true;
            this.btn_ok.Appearance.Options.UseFont = true;
            this.btn_ok.Location = new System.Drawing.Point(80, 120);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(120, 50);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "OK";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // lbl_title
            // 
            this.lbl_title.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.lbl_title.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lbl_title.Appearance.Options.UseFont = true;
            this.lbl_title.Appearance.Options.UseForeColor = true;
            this.lbl_title.Appearance.Options.UseTextOptions = true;
            this.lbl_title.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lbl_title.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbl_title.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_title.Location = new System.Drawing.Point(0, 0);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(578, 96);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "are you sure Application End?";
            // 
            // btn_ok2
            // 
            this.btn_ok2.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btn_ok2.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.btn_ok2.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_ok2.Appearance.Options.UseBackColor = true;
            this.btn_ok2.Appearance.Options.UseFont = true;
            this.btn_ok2.Location = new System.Drawing.Point(240, 120);
            this.btn_ok2.Name = "btn_ok2";
            this.btn_ok2.Size = new System.Drawing.Size(120, 50);
            this.btn_ok2.TabIndex = 3;
            this.btn_ok2.Text = "OK";
            this.btn_ok2.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // frm_messagebox
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(598, 208);
            this.ControlBox = false;
            this.Controls.Add(this.pnl_main);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("frm_messagebox.IconOptions.LargeImage")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_messagebox";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Message";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frm_messagebox_Load);
            this.pnl_main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel pnl_main;
        private DevExpress.XtraEditors.LabelControl lbl_title;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_ok;
        private DevExpress.XtraEditors.SimpleButton btn_ok2;
    }
}