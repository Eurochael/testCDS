using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;

namespace cds
{
    public partial class frm_popup_login : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public Boolean actived
        {
            get { return _actived; }
            set
            {
                try
                {
                    _actived = value;
                    if (_actived == true)
                    {
                        this.Visible = false;
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + ".actived." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
            }
        }
        private void frm_popup_page_Load(object sender, EventArgs e)
        {
            actived = true;
            this.ActiveControl = txt_user_password;
        }
        private void frm_popup_page_FormClosed(object sender, FormClosedEventArgs e)
        {
            actived = false;
        }
        public frm_popup_login()
        {
            InitializeComponent();
        }
        private void Btn_login(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                if (btn_event.Name == "btn_login")
                {
                    if (Program.main_md.Login_Check(Program.popup_login.cmb_user_id.Text, Program.popup_login.txt_user_password.Text) == true)
                    {
                        this.Close();
                    }
                    else
                    {
                        Program.main_md.Message_By_Application("Login Fail", frm_messagebox.enum_apptype.Only_OK);
                    }
                }
                else if (btn_event.Name == "btn_cancel")
                {

                    this.Close();
                }
            }

        }

        private void txt_user_password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                btn_login.PerformClick();
            }
        }
    }
}