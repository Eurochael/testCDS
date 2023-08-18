
namespace cds
{
    partial class frm_popup_login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_popup_login));
            this.cmb_user_id = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txt_user_password = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btn_login = new DevExpress.XtraEditors.SimpleButton();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_user_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_user_password.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmb_user_id
            // 
            this.cmb_user_id.Location = new System.Drawing.Point(134, 37);
            this.cmb_user_id.Name = "cmb_user_id";
            this.cmb_user_id.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.cmb_user_id.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.cmb_user_id.Properties.Appearance.Options.UseFont = true;
            this.cmb_user_id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_user_id.Properties.Items.AddRange(new object[] {
            "Admin",
            "User"});
            this.cmb_user_id.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb_user_id.Size = new System.Drawing.Size(144, 30);
            this.cmb_user_id.TabIndex = 20;
            // 
            // txt_user_password
            // 
            this.txt_user_password.EditValue = "";
            this.txt_user_password.Location = new System.Drawing.Point(134, 73);
            this.txt_user_password.Name = "txt_user_password";
            this.txt_user_password.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txt_user_password.Properties.Appearance.Options.UseFont = true;
            this.txt_user_password.Properties.PasswordChar = '*';
            this.txt_user_password.Size = new System.Drawing.Size(144, 30);
            this.txt_user_password.TabIndex = 21;
            this.txt_user_password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_user_password_KeyPress);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelControl2.Location = new System.Drawing.Point(9, 39);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(119, 24);
            this.labelControl2.TabIndex = 22;
            this.labelControl2.Tag = "";
            this.labelControl2.Text = "Authority : ";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseTextOptions = true;
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelControl3.Location = new System.Drawing.Point(9, 76);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(119, 24);
            this.labelControl3.TabIndex = 23;
            this.labelControl3.Tag = "";
            this.labelControl3.Text = "Password : ";
            // 
            // btn_login
            // 
            this.btn_login.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_login.Appearance.Options.UseFont = true;
            this.btn_login.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_login.ImageOptions.Image")));
            this.btn_login.Location = new System.Drawing.Point(38, 109);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(117, 50);
            this.btn_login.TabIndex = 24;
            this.btn_login.Text = "Login";
            this.btn_login.Click += new System.EventHandler(this.Btn_login);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_cancel.Appearance.Options.UseFont = true;
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.Location = new System.Drawing.Point(161, 109);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(117, 50);
            this.btn_cancel.TabIndex = 25;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Click += new System.EventHandler(this.Btn_login);
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.btn_cancel);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.btn_login);
            this.groupControl1.Controls.Add(this.txt_user_password);
            this.groupControl1.Controls.Add(this.cmb_user_id);
            this.groupControl1.Location = new System.Drawing.Point(6, 6);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(289, 170);
            this.groupControl1.TabIndex = 26;
            this.groupControl1.Text = "Login";
            // 
            // frm_popup_login
            // 
            this.Appearance.BackColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 184);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_popup_login";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Select Funtion";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_popup_page_FormClosed);
            this.Load += new System.EventHandler(this.frm_popup_page_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmb_user_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_user_password.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.ComboBoxEdit cmb_user_id;
        public DevExpress.XtraEditors.TextEdit txt_user_password;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btn_login;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}