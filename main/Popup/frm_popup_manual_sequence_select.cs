using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;

namespace cds
{
    public partial class frm_popup_manual_sequence_select : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public tank_class.enum_semi_auto selected_semi_auto;
        public tank_class.enum_tank_type selected_tank_type;
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
        }
        private void frm_popup_page_FormClosed(object sender, FormClosedEventArgs e)
        {
            actived = false;
        }
        public frm_popup_manual_sequence_select()
        {
            InitializeComponent();
        }
        private void Btn_all(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                string text = "";
                if(selected_semi_auto == tank_class.enum_semi_auto.DRAIN)
                {
                    text = "Drain?";
                }
                else if (selected_semi_auto == tank_class.enum_semi_auto.DIW_FLUSH)
                {
                    text = "Diw Flush?";
                }
                else if (selected_semi_auto == tank_class.enum_semi_auto.CHEMICAL_FLUSH)
                {
                    text = "Chemical Flush?";
                }
                else if (selected_semi_auto == tank_class.enum_semi_auto.AUTO_FLUSH)
                {
                    text = "Auto Flush?";
                }

                if (btn_event.Name == "btn_all")
                {
                    if (Program.main_md.Message_By_Application("Are you sure Tank Both Operate" + text, frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                    {
                        selected_tank_type = tank_class.enum_tank_type.TANK_ALL;
                        this.Close();
                    }
                }
                else if (btn_event.Name == "btn_tanka")
                {
                    if (Program.main_md.Message_By_Application("Are you sure Tank-A Operate" + text, frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                    {
                        selected_tank_type = tank_class.enum_tank_type.TANK_A;
                        this.Close();
                    }
                }
                else if (btn_event.Name == "btn_tankb")
                {
                    if (Program.main_md.Message_By_Application("Are you sure Tank-B Operate" + text, frm_messagebox.enum_apptype.Ok_Or_Cancel) == true)
                    {
                        selected_tank_type = tank_class.enum_tank_type.TANK_B;
                        this.Close();
                    }
                }
                else if (btn_event.Name == "btn_cancel")
                {
                    selected_tank_type = tank_class.enum_tank_type.NONE;
                    this.Close();
                }
            }

            Program.log_md.LogWrite(this.Name + ".Btn_all." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);

        }
    }
}