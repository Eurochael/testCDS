using System;
using System.Windows.Forms;

namespace cds
{
    public partial class UC_FlowMeter_USF500 : UserControl
    {
        public int idx_serial = 0;
        public UC_FlowMeter_USF500()
        {
            InitializeComponent();
        }

        private void UC_FlowMeter_USF500_Load(object sender, EventArgs e)
        {

            //if (cmb_address_read.Properties.Items.Count > 0)
            //{
            //    cmb_address_read.SelectedIndex = 0;
            //}

            //if (cmb_address_write.Properties.Items.Count > 0)
            //{
            //    cmb_address_write.SelectedIndex = 0;
            //}

            //if (cmb_ch.Properties.Items.Count > 0)
            //{
            //    cmb_ch.SelectedIndex = 0;
            //}

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
                if (btn_event.Name == "btn_read_main_status")
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(Convert.ToByte(cmb_address_read.SelectedIndex + 1), 0, Class_FlowMeter_USF500.read_Status, idx_serial);
                }
                else if (btn_event.Name == "btn_read_ch1_data")
                {
                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(Convert.ToByte(cmb_address_read.SelectedIndex + 1), Convert.ToByte(cmb_ch.SelectedIndex + 1), Class_FlowMeter_USF500.read_ch1_flow_Status, idx_serial);
                }
                else if (btn_event.Name == "btn_read_ch2_data")
                {

                    Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(Convert.ToByte(cmb_address_read.SelectedIndex + 1), Convert.ToByte(cmb_ch.SelectedIndex + 1), Class_FlowMeter_USF500.read_ch2_flow_Status, idx_serial);
                }
                else if (btn_event.Name == "btn_zeroset")
                {
                    if (cmb_ch.SelectedIndex < 0) { return; }
                    if (Convert.ToByte(cmb_ch.SelectedIndex + 1) == 1)
                    {
                        Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToByte(cmb_ch.SelectedIndex + 1), Class_FlowMeter_USF500.ch1_zeroset, idx_serial);
                    }
                    else if (Convert.ToByte(cmb_ch.SelectedIndex + 1) == 2)
                    {
                        Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToByte(cmb_ch.SelectedIndex + 1), Class_FlowMeter_USF500.ch2_zeroset, idx_serial);
                    }

                }
                else if (btn_event.Name == "btn_totalusage_zeroset")
                {
                    if (cmb_ch.SelectedIndex < 0) { return; }
                    if (Convert.ToByte(cmb_ch.SelectedIndex + 1) == 1)
                    {
                        Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToByte(cmb_ch.SelectedIndex + 1), Class_FlowMeter_USF500.ch1_totalusage_reset, idx_serial);
                    }
                    else if (Convert.ToByte(cmb_ch.SelectedIndex + 1) == 2)
                    {
                        Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToByte(cmb_ch.SelectedIndex + 1), Class_FlowMeter_USF500.ch2_totalusage_reset, idx_serial);
                    }


                }

            }
            Program.log_md.LogWrite(this.Name + " Tag Idx[" + idx_serial + "].btn_read_pv_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.LEVEL_1);

        }

        private void cmb_address_wrtie_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_ch.Text = "";
        }

        private void cmb_ch_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmb_ch.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            cmb_ch.Properties.Items.Clear();
            cmb_ch.Text = "";
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (cmb_address_write.SelectedIndex == 0)
                {
                    cmb_ch.Properties.Items.Add("NH4OH");
                }
                else if (cmb_address_write.SelectedIndex == 1)
                {
                    cmb_ch.Properties.Items.Add("HDIW");
                    cmb_ch.Properties.Items.Add("H2O2");
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                if (cmb_address_write.SelectedIndex == 0)
                {
                    cmb_ch.Properties.Items.Add("DIW");
                    cmb_ch.Properties.Items.Add("HF");
                }
               
            }
            cmb_ch.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

        private void cmb_address_read_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (cmb_address_read.SelectedIndex == 0)
                {
                    btn_read_ch1_data.Text = "NH4OH";
                    btn_read_ch2_data.Text = "";
                }
                else if (cmb_address_read.SelectedIndex == 1)
                {
                    btn_read_ch1_data.Text = "HDIW";
                    btn_read_ch2_data.Text = "H2O2";
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                if (cmb_address_read.SelectedIndex == 0)
                {
                    btn_read_ch1_data.Text = "DIW";
                    btn_read_ch2_data.Text = "HF";
                }
                else
                {
                    btn_read_ch1_data.Text = "";
                    btn_read_ch2_data.Text = "";
                }
            }
        }
    }
}
