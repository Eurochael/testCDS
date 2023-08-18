using System;
using System.Windows.Forms;

namespace cds
{
    public partial class UC_ThermoStat_HE_3320C : UserControl
    {
        public int idx_serial = 0;
        public string name = "";
        public UC_ThermoStat_HE_3320C()
        {
            InitializeComponent();

        }
        public void HideTagetMode()
        {
            if(name.ToUpper().IndexOf("CIR") >= 0)
            {
                pnl_tank_a_func.Visible = true;
                pnl_tank_b_func.Visible = true;
            }
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
                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.read_supply_pv, idx_serial);
                }
                else if (btn_event.Name == "btn_write_alarm_reset")
                {
                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.alarm_reset, idx_serial);
                }
                else if (btn_event.Name == "btn_write_sv_apply")
                {
                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, idx_serial);
                }
                else if (btn_event.Name == "btn_set_output")
                {
                    if (txt_sv_value.Text == "") { return; }
                    Program.ThermoStart_HE_3320C.Set_Offset(Convert.ToInt16(txt_sv_value.Text), idx_serial);
                    Program.cg_offset.total_cnt = 12;
                    Program.cg_offset.temp_offset[idx_serial].name = this.name;
                    Program.cg_offset.temp_offset[idx_serial].offset = Convert.ToInt16(txt_sv_value.Text) * 0.01;
                    Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_parameter, "");
                }
                else if (btn_event.Name == "btn_sv_set")
                {
                    if (txt_sv_value.Text == "") { return; }
                    Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(txt_sv_value.Text), idx_serial);
                }
                else if (btn_event.Name == "btn_heater_on")
                {
                    if (txt_sv_value.Text == "") { return; }
                    Program.ThermoStart_HE_3320C.Heater_ON_OFF(true, idx_serial);
                }
                else if (btn_event.Name == "btn_heater_off")
                {
                    if (txt_sv_value.Text == "") { return; }
                    Program.ThermoStart_HE_3320C.Heater_ON_OFF(false, idx_serial);
                }
                else if (btn_event.Name == "btn_tank_a_on")
                {
                    Program.ThermoStart_HE_3320C.Select_Tank(Class_ThermoStat_HE_3320C.he3320c_selected_tank.tank_a, true, idx_serial);
                }
                else if (btn_event.Name == "btn_tank_b_on")
                {
                    Program.ThermoStart_HE_3320C.Select_Tank(Class_ThermoStat_HE_3320C.he3320c_selected_tank.tank_b, true, idx_serial);
                }
                else if (btn_event.Name == "btn_tank_a_off")
                {
                    Program.ThermoStart_HE_3320C.Select_Tank(Class_ThermoStat_HE_3320C.he3320c_selected_tank.tank_a, false, idx_serial);
                }
                else if (btn_event.Name == "btn_tank_b_off")
                {
                    Program.ThermoStart_HE_3320C.Select_Tank(Class_ThermoStat_HE_3320C.he3320c_selected_tank.tank_b, false, idx_serial);
                }
            }
            Program.log_md.LogWrite(this.Name + " Tag Idx[" + idx_serial + "].btn_read_pv_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.LEVEL_1);
        }

    }
}
