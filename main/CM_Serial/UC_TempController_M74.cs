using System;
using System.Windows.Forms;

namespace cds
{
    public partial class UC_TempController_M74 : UserControl
    {
        public int idx_serial = 0;
        public UC_TempController_M74()
        {
            InitializeComponent();
        }

        private void UC_TempController_M74_Load(object sender, EventArgs e)
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
                if (btn_event.Name == "btn_read_pv")
                {
                    Program.TempController_M74.Message_Command_To_Byte_M74_TO_Send(Convert.ToByte(cmb_address_read.SelectedIndex + 1), Class_TempController_M74.command_CH1_To_CH4_Data_Read_SIMPLE, idx_serial);
                }
                else if (btn_event.Name == "btn_set_offset")
                {
                    if (cmb_ch.SelectedIndex < 0) { return; }
                    if(txt_sv_value.Text == "") { return; }
                    if (cmb_ch.SelectedIndex == 0)
                    {
                        Program.TempController_M74.OffSet_CH1_SV(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt16(txt_sv_value.Text), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 1)
                    {
                        Program.TempController_M74.OffSet_CH2_SV(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt16(txt_sv_value.Text), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 2)
                    {
                        Program.TempController_M74.OffSet_CH3_SV(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt16(txt_sv_value.Text), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 3)
                    {
                        Program.TempController_M74.OffSet_CH4_SV(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt16(txt_sv_value.Text), idx_serial);
                    }
                    Program.cg_offset.total_cnt = 12;
                    Program.cg_offset.temp_offset[(cmb_address_write.SelectedIndex * 4) + cmb_ch.SelectedIndex].name = cmb_ch.Text;
                    Program.cg_offset.temp_offset[(cmb_address_write.SelectedIndex * 4) + cmb_ch.SelectedIndex].offset = Convert.ToInt16(txt_sv_value.Text) * 0.1;
                    Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_parameter, "");
                }
                else if (btn_event.Name == "btn_read_data")
                {
                    if (cmb_ch.SelectedIndex < 0) { return; }
                    if (txt_sv_value.Text == "") { return; }
                    if (cmb_ch.SelectedIndex == 0)
                    {
                        Program.TempController_M74.Message_Command_To_Byte_M74_TO_Send(Convert.ToByte(cmb_address_read.SelectedIndex + 1), Class_TempController_M74.command_CH1_Data_Read_PV_SV_STATUS, idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 1)
                    {
                        Program.TempController_M74.Message_Command_To_Byte_M74_TO_Send(Convert.ToByte(cmb_address_read.SelectedIndex + 1), Class_TempController_M74.command_CH2_Data_Read_PV_SV_STATUS, idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 2)
                    {
                        Program.TempController_M74.Message_Command_To_Byte_M74_TO_Send(Convert.ToByte(cmb_address_read.SelectedIndex + 1), Class_TempController_M74.command_CH3_Data_Read_PV_SV_STATUS, idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 3)
                    {
                        Program.TempController_M74.Message_Command_To_Byte_M74_TO_Send(Convert.ToByte(cmb_address_read.SelectedIndex + 1), Class_TempController_M74.command_CH4_Data_Read_PV_SV_STATUS, idx_serial);
                    }

                }
                else if (btn_event.Name == "btn_set_value")
                {
                    if (cmb_ch.SelectedIndex < 0) { return; }
                    if (txt_sv_value.Text == "") { return; }
                    if (cmb_ch.SelectedIndex == 0)
                    {
                        Program.TempController_M74.Set_CH1_SV(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt32(txt_sv_value.Text), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 1)
                    {
                        Program.TempController_M74.Set_CH2_SV(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt32(txt_sv_value.Text), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 2)
                    {
                        Program.TempController_M74.Set_CH3_SV(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt32(txt_sv_value.Text), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 3)
                    {
                        Program.TempController_M74.Set_CH4_SV(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt32(txt_sv_value.Text), idx_serial);
                    }

                }
                else if (btn_event.Name == "btn_set_output")
                {
                    if (cmb_ch.SelectedIndex < 0) { return; }
                    if (txt_sv_value.Text == "") { return; }
                    if (cmb_ch.SelectedIndex == 0)
                    {
                        Program.TempController_M74.Set_CH1_OUTPORT(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt32(txt_sv_value.Text), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 1)
                    {
                        Program.TempController_M74.Set_CH2_OUTPORT(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt32(txt_sv_value.Text), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 2)
                    {
                        Program.TempController_M74.Set_CH3_OUTPORT(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt32(txt_sv_value.Text), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 3)
                    {
                        Program.TempController_M74.Set_CH4_OUTPORT(Convert.ToByte(cmb_address_write.SelectedIndex + 1), Convert.ToInt32(txt_sv_value.Text), idx_serial);
                    }

                }

                else if (btn_event.Name == "btn_run")
                {
                    if (cmb_ch.SelectedIndex < 0) { return; }
                    if (cmb_ch.SelectedIndex == 0)
                    {
                        Program.TempController_M74.RUN_CH1(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 1)
                    {
                        Program.TempController_M74.RUN_CH2(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 2)
                    {
                        Program.TempController_M74.RUN_CH3(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 3)
                    {
                        Program.TempController_M74.RUN_CH4(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }

                }
                else if (btn_event.Name == "btn_stop")
                {
                    if (cmb_ch.SelectedIndex < 0) { return; }

                    if (cmb_ch.SelectedIndex == 0)
                    {
                        Program.TempController_M74.STOP_CH1(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 1)
                    {
                        Program.TempController_M74.STOP_CH2(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 2)
                    {
                        Program.TempController_M74.STOP_CH3(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 3)
                    {
                        Program.TempController_M74.STOP_CH4(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }

                }
                else if (btn_event.Name == "btn_at_run")
                {
                    if (cmb_ch.SelectedIndex < 0) { return; }
                    if (cmb_ch.SelectedIndex == 0)
                    {
                        Program.TempController_M74.AT_RUN_CH1(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 1)
                    {
                        Program.TempController_M74.AT_RUN_CH2(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 2)
                    {
                        Program.TempController_M74.AT_RUN_CH3(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 3)
                    {
                        Program.TempController_M74.AT_RUN_CH4(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }

                }
                else if (btn_event.Name == "btn_at_stop")
                {
                    if (cmb_ch.SelectedIndex < 0) { return; }
                    if (cmb_ch.SelectedIndex == 0)
                    {
                        Program.TempController_M74.AT_STOP_CH1(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 1)
                    {
                        Program.TempController_M74.AT_STOP_CH2(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 2)
                    {
                        Program.TempController_M74.AT_STOP_CH3(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }
                    else if (cmb_ch.SelectedIndex == 3)
                    {
                        Program.TempController_M74.AT_STOP_CH4(Convert.ToByte(cmb_address_write.SelectedIndex + 1), idx_serial);
                    }

                }
            }
            Program.log_md.LogWrite(this.Name + " Tag Idx[" + idx_serial + "].btn_read_pv_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.LEVEL_1);
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
                    cmb_ch.Properties.Items.Add("Tank A");
                    cmb_ch.Properties.Items.Add("Tank B");
                    cmb_ch.Properties.Items.Add("TS-09");
                }
                else if (cmb_address_write.SelectedIndex == 1)
                {
                    cmb_ch.Properties.Items.Add("Supply A");
                    cmb_ch.Properties.Items.Add("Supply B");
                    cmb_ch.Properties.Items.Add("Circulation");
                }
                else if (cmb_address_write.SelectedIndex == 2)
                {
                    cmb_ch.Properties.Items.Add("Supply Heater A");
                    cmb_ch.Properties.Items.Add("Supply Heater B");
                    cmb_ch.Properties.Items.Add("Circulation Heater 1");
                    cmb_ch.Properties.Items.Add("Circulation Heater 2");
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                if (cmb_address_write.SelectedIndex == 0)
                {
                    cmb_ch.Properties.Items.Add("Tank A");
                    cmb_ch.Properties.Items.Add("Supply A");
                    cmb_ch.Properties.Items.Add("Supply Heater A");
                }
            }
            else
            {
                if (cmb_address_write.SelectedIndex == 0)
                {
                    cmb_ch.Properties.Items.Add("Tank A");
                    cmb_ch.Properties.Items.Add("Tank B");
                }
            }
            cmb_ch.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

        private void cmb_address_write_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_ch.Text = "";
        }
    }
}
