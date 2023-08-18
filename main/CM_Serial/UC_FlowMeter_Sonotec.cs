using System;
using System.Windows.Forms;

namespace cds
{
    public partial class UC_FlowMeter_Sonotec : UserControl
    {
        public int idx_serial = 0;
        public UC_FlowMeter_Sonotec()
        {
            InitializeComponent();
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
               
                if (btn_event.Name == "btn_read_data")
                {
                    Program.FlowMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.read_status_flow_totalusage, idx_serial);
                }
                else if (btn_event.Name == "btn_zeroset")
                {
                    Program.FlowMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.zeroset, idx_serial);
                }
                else if (btn_event.Name == "btn_totalusage_zeroset")
                {
                    Program.FlowMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.totalusage_reset, idx_serial);

                }
            }
            Program.log_md.LogWrite(this.Name + " Tag Idx[" + idx_serial + "].btn_read_pv_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }
    }
}
