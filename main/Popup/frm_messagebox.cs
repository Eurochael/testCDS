using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cds
{
    public partial class frm_messagebox : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public bool returnValue = false;
        public bool ok_or_cancel = false;
        public bool ok = false;
        public bool ok2 = false;

        public string ok_text = "OK";
        public string cancel_text = "CANCEL";

        public enum enum_apptype
        {
            None = 0,
            Ok_Or_Cancel = 10,
            OK1_OK2_Cancel = 11,
            Only_OK = 20
        }

        public frm_messagebox()
        {
            InitializeComponent();
        }
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
        private void frm_messagebox_Load(object sender, EventArgs e)
        {
            
        }
        public void setting_initial(String message, enum_apptype apptype)
        {
            try
            {
                ok = false;
                ok2 = false;
                ok_or_cancel = false;

                message = message.Replace(" : ", "\r\n");
                lbl_title.Text = message; btn_ok.Text = ok_text; btn_cancel.Text = cancel_text;
                switch (apptype)
                {
                    case enum_apptype.None:
                        break;

                    case enum_apptype.Ok_Or_Cancel:
                        btn_ok.Visible = true; btn_ok2.Visible = false; btn_cancel.Visible = true;
                        btn_ok.Text = "OK"; btn_cancel.Text = "Cancel";
                        btn_ok.Location = new Point(120, 120); btn_cancel.Location = new Point(330, 120);
                        break;

                    case enum_apptype.OK1_OK2_Cancel:
                        btn_ok.Visible = true; btn_ok2.Visible = true; btn_cancel.Visible = true;
                        btn_ok.Text = "Tank A"; btn_ok2.Text = "Tank B"; btn_cancel.Text = "Cancel";
                        btn_ok.Location = new Point(80, 120); btn_ok2.Location = new Point(240, 120); btn_cancel.Location = new Point(420, 120);
                        break;

                    case enum_apptype.Only_OK:
                        btn_ok.Visible = true; btn_ok2.Visible = false; btn_cancel.Visible = false;
                        btn_ok.Text = "OK";
                        btn_ok.Location = new Point(230, 120); btn_cancel.Location = new Point(330, 120);
                        break;
                }
              
            }
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".Setting_Initial." + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                if (btn_event.Name == "btn_ok")
                {
                    ok_or_cancel = true;
                    ok = true;
                    ok2 = false;
                    this.Close();
                }
                if (btn_event.Name == "btn_ok2")
                {
                    ok_or_cancel = true;
                    ok = false;
                    ok2 = true;
                    this.Close();
                }
                else if (btn_event.Name == "btn_cancel")
                {
                    ok_or_cancel = false;
                    this.Close();
                }
            }
            Program.log_md.LogWrite(sender.ToString(), Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }
    }
}