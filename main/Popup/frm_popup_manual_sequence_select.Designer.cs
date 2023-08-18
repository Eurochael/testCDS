
namespace cds
{
    partial class frm_popup_manual_sequence_select
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_popup_manual_sequence_select));
            this.btn_all = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_tankb = new DevExpress.XtraEditors.SimpleButton();
            this.btn_tanka = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_all
            // 
            this.btn_all.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_all.Appearance.Options.UseFont = true;
            this.btn_all.Location = new System.Drawing.Point(22, 48);
            this.btn_all.Name = "btn_all";
            this.btn_all.Size = new System.Drawing.Size(117, 74);
            this.btn_all.TabIndex = 24;
            this.btn_all.Text = "All";
            this.btn_all.Click += new System.EventHandler(this.Btn_all);
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.btn_cancel);
            this.groupControl1.Controls.Add(this.btn_tankb);
            this.groupControl1.Controls.Add(this.btn_tanka);
            this.groupControl1.Controls.Add(this.btn_all);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(5, 5);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(407, 216);
            this.groupControl1.TabIndex = 26;
            this.groupControl1.Text = "Flush Tank Select";
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.Location = new System.Drawing.Point(145, 154);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(117, 56);
            this.btn_cancel.TabIndex = 27;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Click += new System.EventHandler(this.Btn_all);
            // 
            // btn_tankb
            // 
            this.btn_tankb.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_tankb.Appearance.Options.UseFont = true;
            this.btn_tankb.Location = new System.Drawing.Point(268, 48);
            this.btn_tankb.Name = "btn_tankb";
            this.btn_tankb.Size = new System.Drawing.Size(117, 74);
            this.btn_tankb.TabIndex = 26;
            this.btn_tankb.Text = "Tank B";
            this.btn_tankb.Click += new System.EventHandler(this.Btn_all);
            // 
            // btn_tanka
            // 
            this.btn_tanka.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_tanka.Appearance.Options.UseFont = true;
            this.btn_tanka.Location = new System.Drawing.Point(145, 48);
            this.btn_tanka.Name = "btn_tanka";
            this.btn_tanka.Size = new System.Drawing.Size(117, 74);
            this.btn_tanka.TabIndex = 25;
            this.btn_tanka.Text = "Tank A";
            this.btn_tanka.Click += new System.EventHandler(this.Btn_all);
            // 
            // frm_popup_manual_sequence_select
            // 
            this.Appearance.BackColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 226);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_popup_manual_sequence_select";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Funtion";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_popup_page_FormClosed);
            this.Load += new System.EventHandler(this.frm_popup_page_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btn_all;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btn_tankb;
        private DevExpress.XtraEditors.SimpleButton btn_tanka;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
    }
}