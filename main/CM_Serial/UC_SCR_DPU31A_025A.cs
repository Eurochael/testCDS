using System;
using System.Windows.Forms;

namespace cds
{
    public partial class UC_SCR_DPU31A_025A : UserControl
    {
        public int idx_serial = 0;
        public UC_SCR_DPU31A_025A()
        {
            InitializeComponent();
        }

        private void UC_SCR_DPU31A_025A_Load(object sender, EventArgs e)
        {
            cmb_address_read.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            cmb_address_read.Properties.Items.Clear();
            cmb_address_read.Text = "";
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                cmb_address_read.Properties.Items.Add("SUPPLY A HEATER");
                cmb_address_read.Properties.Items.Add("SUPPLY B HEATER");
                cmb_address_read.Properties.Items.Add("CIRCULATION1 HEATER");
                cmb_address_read.Properties.Items.Add("CIRCULATION2 HEATER");
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                cmb_address_read.Properties.Items.Add("SUPPLY A HEATER");
            }
          
            if (cmb_address_read.Properties.Items.Count > 0)
            {
                cmb_address_read.SelectedIndex = 0;
            }
            cmb_address_read.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }
        private void btn_read_pv_Click(object sender, EventArgs e)
        {
            byte[] send_msg = null;
            if (idx_serial < 0) { return; }

            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto || Program.cg_app_info.semi_auto_state == true)
                {
                    Program.main_md.Message_By_Application("Cannot Operate in Auto or Semi-Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                }
                else if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
                {
                    Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
                }
                if (btn_event.Name == "btn_read_pv")
                {
                    if (cmb_address_read.SelectedIndex >=0) { Program.SCR_DPU31A_025A.Message_Command_Apply_CRC_TO_Send(Convert.ToByte(cmb_address_read.SelectedIndex + 1), Class_SCR_DPU31A_025A.read_operation, idx_serial); }
                    
                }

            }
            Program.log_md.LogWrite(this.Name + " Tag Idx[" + idx_serial + "].btn_read_pv_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.LEVEL_1);

        }

    }
}
