using System;
using System.Windows.Forms;

namespace cds
{
    public partial class UC_DHF : UserControl
    {
        public bool view_heater_interlock_value = false;
        public UC_DHF()
        {
            InitializeComponent();
        }


        public void UI_Change()
        {
            //Pipe Claer
            foreach (var x in this.pnl_pnid.Controls)
            {
                if (x is DoubleBufferedPanel)
                {
                    if (((DoubleBufferedPanel)x).Name.IndexOf("pnl_pip") >= 0)
                    {
                        ((DoubleBufferedPanel)x).BackColor = Program.schematic_form.Pipe_off;
                    }
                }
            }
            if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
            {
                chk_use_auto_drain.Checked = true;
                chk_use_auto_drain.Enabled = false;
            }
            else
            {
                chk_use_auto_drain.Enabled = true;
            }

            //CC가 필요할 때
            pnl_cc_need_tank_a.Visible = Program.schematic_form.Visible_CC_status(tank_class.enum_tank_type.TANK_A);
            pnl_cc_need_tank_b.Visible = Program.schematic_form.Visible_CC_status(tank_class.enum_tank_type.TANK_B);
            pnl_ctc_confirm_tank_a.Visible = Program.schematic_form.Visible_Confirm_status(tank_class.enum_tank_type.TANK_A);
            pnl_ctc_confirm_tank_b.Visible = Program.schematic_form.Visible_Confirm_status(tank_class.enum_tank_type.TANK_B);

            //Level
            lbl_lv_tanka_hh.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_A, frm_schematic.enum_tank_level.HH);
            lbl_lv_tanka_h.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_A, frm_schematic.enum_tank_level.H);
            lbl_lv_tanka_m.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_A, frm_schematic.enum_tank_level.M);
            lbl_lv_tanka_ll.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_A, frm_schematic.enum_tank_level.LL);
            lbl_lv_tanka_l.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_A, frm_schematic.enum_tank_level.L);

            lbl_lv_tankb_hh.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_B, frm_schematic.enum_tank_level.HH);
            lbl_lv_tankb_h.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_B, frm_schematic.enum_tank_level.H);
            lbl_lv_tankb_m.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_B, frm_schematic.enum_tank_level.M);
            lbl_lv_tankb_ll.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_B, frm_schematic.enum_tank_level.LL);
            lbl_lv_tankb_l.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_B, frm_schematic.enum_tank_level.L);




            if (Program.schematic_form.timer_manual_sequence_tank_a.Enabled == true)
            {
                pnl_semi_status_tank_a.Visible = true;
                if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH || Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                {
                    pnl_semi_status_tank_a.Text = Program.seq.semi_auto_tank_a.semi_auto_run_count.ToString() + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count).ToString();
                }
                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH || Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                {
                    pnl_semi_status_tank_a.Text = Program.seq.semi_auto_tank_a.semi_auto_run_count.ToString() + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count).ToString();
                }
                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                {
                    pnl_semi_status_tank_a.Text = Program.seq.semi_auto_tank_a.semi_auto_run_diw_flush_count.ToString() + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count).ToString();
                    pnl_semi_status_tank_a.Text = pnl_semi_status_tank_a.Text + "," + Program.seq.semi_auto_tank_a.semi_auto_run_chemical_flush_count.ToString() + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count).ToString();
                    pnl_semi_status_tank_a.Text = pnl_semi_status_tank_a.Text + "," + Program.seq.semi_auto_tank_a.auto_flush_current_type.ToString() + "/" + Program.seq.semi_auto_tank_a.semi_auto_run_count.ToString() + "/" + Program.seq.semi_auto_tank_a.semi_auto_run_auto_flush_count.ToString();
                }
            }
            else
            {
                pnl_semi_status_tank_a.Visible = false;
            }

            if (Program.schematic_form.timer_manual_sequence_tank_b.Enabled == true)
            {
                pnl_semi_status_tank_b.Visible = true;
                if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH || Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                {
                    pnl_semi_status_tank_b.Text = Program.seq.semi_auto_tank_b.semi_auto_run_count.ToString() + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count).ToString();
                }
                else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH || Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                {
                    pnl_semi_status_tank_b.Text = Program.seq.semi_auto_tank_b.semi_auto_run_count.ToString() + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count).ToString();
                }
                else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                {
                    pnl_semi_status_tank_b.Text = Program.seq.semi_auto_tank_b.semi_auto_run_diw_flush_count.ToString() + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count).ToString();
                    pnl_semi_status_tank_b.Text = pnl_semi_status_tank_b.Text + "," + Program.seq.semi_auto_tank_b.semi_auto_run_chemical_flush_count.ToString() + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count).ToString();
                    pnl_semi_status_tank_b.Text = pnl_semi_status_tank_b.Text + "," + Program.seq.semi_auto_tank_b.auto_flush_current_type.ToString() + "/" + Program.seq.semi_auto_tank_b.semi_auto_run_count.ToString() + "/" + Program.seq.semi_auto_tank_b.semi_auto_run_auto_flush_count.ToString();
                }
            }
            else
            {
                pnl_semi_status_tank_b.Visible = false;
            }
            lbl_mixing_volume_name_a.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_name.ToString();
            lbl_mixing_volume_ratio_a.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio.ToString();
            ///SERIAL DATA
            lbl_flow_fm01.Text = string.Format("{0:f2}", Program.main_form.SerialData.FlowMeter_USF500.DIW_flow) + "/" + string.Format("{0:f2}", Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage) + "(LPM)";
            lbl_flow_fm03.Text = string.Format("{0:f2}", Program.main_form.SerialData.FlowMeter_USF500.HF_flow) + "/" + string.Format("{0:f2}", Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage) + "(LPM)";
            lbl_tank_a_temp.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv) + "℃";
            lbl_tank_b_temp.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv) + "℃";

            lbl_temp_ts21.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.circulation.pv) + "℃";
            lbl_temp_ts41.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.pv) + "℃";
            lbl_temp_ts42.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.pv) + "℃";

            if (view_heater_interlock_value == false)
            {
                lbl_temp_heater_he41.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv) + "℃";
                lbl_temp_heater_he42.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv) + "℃";
                lbl_temp_heater_he21.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv) + "℃";
            }
            else
            {
                if (chk_offset_view.Checked == true)
                {
                    lbl_temp_heater_he41.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.offset) + "℃";
                    lbl_temp_heater_he42.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.offset) + "℃";
                    lbl_temp_heater_he21.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.offset) + "℃";
                }
            }

            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
            {
                pnl_body_tank_a.Enabled = false;
            }
            else
            {
                lbl_tank_a_volume.Text = Program.schematic_form.Tank_Volume_View(Program.tank[(int)tank_class.enum_tank_type.TANK_A]);

                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto && (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.REFILL))
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].life_time_to_minute = (int)(DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_process).TotalMinutes;
                }
                else
                {
                    //초기화는 Drain 완료 후 적용
                }

                lbl_tank_a_lifetime.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_A].life_time_to_minute.ToString() + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Life_Time_High) + "(Min)";
                lbl_tank_a_wafercount.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt.ToString() + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Wafer_Count_High);
                lbl_tank_a_concentrate.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_A].concentration_ccss1.ToString() + "wt%";
                lbl_state_tanka.BackColor = Program.tank[(int)tank_class.enum_tank_type.TANK_A].BackColor_Tank_By_Status(Program.tank[(int)tank_class.enum_tank_type.TANK_A].status);
                lbl_state_tanka.ForeColor = Program.tank[(int)tank_class.enum_tank_type.TANK_A].ForeColor_Tank_By_Status(Program.tank[(int)tank_class.enum_tank_type.TANK_A].status);
            }

            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == false)
            {
                pnl_body_tank_b.Enabled = false;
            }
            else
            {
                lbl_tank_b_volume.Text = Program.schematic_form.Tank_Volume_View(Program.tank[(int)tank_class.enum_tank_type.TANK_B]);
                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto && (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.REFILL))
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].life_time_to_minute = (int)(DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_start_process).TotalMinutes;
                }
                else
                {
                    //Program.tank[(int)tank_class.enum_tank_type.TANK_B].life_time_to_minute = 0;
                }
                lbl_tank_b_lifetime.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_B].life_time_to_minute.ToString() + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Life_Time_High) + "(Min)";
                lbl_tank_b_wafercount.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt.ToString() + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Wafer_Count_High);
                lbl_tank_b_concentrate.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_B].concentration_ccss1.ToString() + "wt%";
                lbl_state_tankb.BackColor = Program.tank[(int)tank_class.enum_tank_type.TANK_B].BackColor_Tank_By_Status(Program.tank[(int)tank_class.enum_tank_type.TANK_B].status);
                lbl_state_tankb.ForeColor = Program.tank[(int)tank_class.enum_tank_type.TANK_B].ForeColor_Tank_By_Status(Program.tank[(int)tank_class.enum_tank_type.TANK_B].status);
            }
            lbl_concentration.Text = string.Format("{0:f1}", Program.main_form.SerialData.CM210DC.Concentration) + " wt%";


            if (Program.seq.monitoring.ready_to_pump_off_Delay == true)
            {
                pnl_drainpump_off_delay_status.Visible = true;
                pnl_drainpump_off_delay_status.Text = Convert.ToInt32((DateTime.Now - Program.seq.monitoring.last_pump_on_level).TotalSeconds) + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Drain_Pump_Off_Time_Delay);
            }
            else
            {
                pnl_drainpump_off_delay_status.Visible = false;
            }

            //HEATER HE21
            if (Program.main_form.SerialData.Circulation_Thermostat.heater_on == true && Program.occured_alarm_form.most_occured_alarm_level != frm_alarm.enum_level.HEAVY && Program.main_form.serial_port_state[(int)Config_IO.enum_dhf_serial_index.CIRCULATION_THERMOSTAT] == true)
            {
                btn_heater_he21.ImageOptions.Image = Properties.Resources.heater_on;
            }
            else
            {
                btn_heater_he21.ImageOptions.Image = Properties.Resources.heater_off;
            }

            //HEATER HE41
            if (Program.main_form.SerialData.Supply_A_Thermostat.heater_on == true && Program.occured_alarm_form.most_occured_alarm_level != frm_alarm.enum_level.HEAVY && Program.main_form.serial_port_state[(int)Config_IO.enum_dhf_serial_index.SUPPLY_A_THERMOSTAT] == true)
            {
                btn_heater_he41.ImageOptions.Image = Properties.Resources.heater_on;
            }
            else
            {
                btn_heater_he41.ImageOptions.Image = Properties.Resources.heater_off;
            }

            //HEATER HE42
            if (Program.main_form.SerialData.Supply_B_Thermostat.heater_on == true && Program.occured_alarm_form.most_occured_alarm_level != frm_alarm.enum_level.HEAVY && Program.main_form.serial_port_state[(int)Config_IO.enum_dhf_serial_index.SUPPLY_B_THERMOSTAT] == true)
            {
                btn_heater_he42.ImageOptions.Image = Properties.Resources.heater_on;
            }
            else
            {
                btn_heater_he42.ImageOptions.Image = Properties.Resources.heater_off;
            }

            //BP21
            if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
            {
                btn_pump_bp21.ImageOptions.Image = Properties.Resources.pump_h_on;
            }
            else
            {
                btn_pump_bp21.ImageOptions.Image = Properties.Resources.pump_h_off;
            }

            //BP41
            if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
            {
                btn_pump_bp41.ImageOptions.Image = Properties.Resources.pump_h_on;
            }
            else
            {
                btn_pump_bp41.ImageOptions.Image = Properties.Resources.pump_h_off;
            }

            //BP42
            if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
            {
                btn_pump_bp42.ImageOptions.Image = Properties.Resources.pump_h_on;
            }
            else
            {
                btn_pump_bp42.ImageOptions.Image = Properties.Resources.pump_h_off;
            }

            //DI
            for (int idx = 0; idx < Program.IO.DI.use_cnt; idx++)
            {
                switch (idx)
                {
                    case (int)Config_IO.enum_di.Drain_Pump_On_Level: //Drain Pump On Level
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            pnl_lv_draintank_on.BackgroundImage = Properties.Resources.LED_GREEN_NONE;
                        }
                        else
                        {
                            pnl_lv_draintank_on.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                        }
                        break;
                    case (int)Config_IO.enum_di.BOTTOM_VAT_LEAK1: //bot leak
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            pnl_leak_bottom_vat.BackgroundImage = Properties.Resources.LED_RED_NONE;
                        }
                        else
                        {
                            pnl_leak_bottom_vat.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                        }
                        break;
                    case (int)Config_IO.enum_di.Drain_Tank_H: //drain Tank h
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            pnl_lv_draintank_h.BackgroundImage = Properties.Resources.LED_GREEN_NONE;
                        }
                        else
                        {
                            pnl_lv_draintank_h.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANK_VAT_LEAK: //vat leak
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            pnl_leak_tank_vat.BackgroundImage = Properties.Resources.LED_RED_NONE;
                        }
                        else
                        {
                            pnl_leak_tank_vat.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                        }
                        break;
                    case (int)Config_IO.enum_di.DIKE_LEAK: //dike leak
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            pnl_leak_dike.BackgroundImage = Properties.Resources.LED_RED_NONE;
                        }
                        else
                        {
                            pnl_leak_dike.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                        }
                        break;
                    case (int)Config_IO.enum_di.CIRCULATION_PUMP_LEAK: //circulation pump leak
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            pnl_leak_bp21.BackgroundImage = Properties.Resources.LED_RED_NONE;
                        }
                        else
                        {
                            pnl_leak_bp21.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                        }
                        break;

                    //TANK A LV
                    case (int)Config_IO.enum_di.TANKA_OVERFLOW_CHECK: //TANKA_LEVEL_OVERFLOW
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tanka_over.BackColor = Program.schematic_form.Level_on_HH_Over;
                        }
                        else
                        {
                            lbl_lv_tanka_over.BackColor = Program.schematic_form.Level_off;
                        }
                        break;

                    case (int)Config_IO.enum_di.TANKA_LEVEL_HH: //TANKA_LEVEL_HH
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tanka_hh.BackColor = Program.schematic_form.Level_on_HH_Over;
                        }
                        else
                        {
                            lbl_lv_tanka_hh.BackColor = Program.schematic_form.Level_off;
                        }
                        break;

                    case (int)Config_IO.enum_di.TANKA_LEVEL_H: //TANKA_LEVEL_H
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tanka_h.BackColor = Program.schematic_form.Level_on;
                        }
                        else
                        {
                            lbl_lv_tanka_h.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANKA_LEVEL_M: //TANKA_LEVEL_M
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tanka_m.BackColor = Program.schematic_form.Level_on;
                        }
                        else
                        {
                            lbl_lv_tanka_m.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANKA_LEVEL_L: //TANKA_LEVEL_L
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tanka_l.BackColor = Program.schematic_form.Level_on;
                        }
                        else
                        {
                            lbl_lv_tanka_l.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANKA_LEVEL_LL: //TANKA_LEVEL_LL
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tanka_ll.BackColor = Program.schematic_form.Level_on;
                        }
                        else
                        {
                            lbl_lv_tanka_ll.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANKA_EMPTY_CHECK: //TANKA_LEVEL_EMPTY
                        if (Program.IO.DI.Tag[idx].value == false)
                        {
                            lbl_lv_tanka_empty.BackColor = Program.schematic_form.Level_on;
                        }
                        else
                        {
                            lbl_lv_tanka_empty.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    //TANK B LV
                    case (int)Config_IO.enum_di.TANKB_OVERFLOW_CHECK: //TANKB_LEVEL_OVERFLOW
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tankb_over.BackColor = Program.schematic_form.Level_on_HH_Over;
                        }
                        else
                        {
                            lbl_lv_tankb_over.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANKB_LEVEL_HH: //TANKB_LEVEL_HH
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tankb_hh.BackColor = Program.schematic_form.Level_on_HH_Over;
                        }
                        else
                        {
                            lbl_lv_tankb_hh.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANKB_LEVEL_H: //TANKB_LEVEL_H
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tankb_h.BackColor = Program.schematic_form.Level_on;
                        }
                        else
                        {
                            lbl_lv_tankb_h.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANKB_LEVEL_M: //TANKB_LEVEL_M
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tankb_m.BackColor = Program.schematic_form.Level_on;
                        }
                        else
                        {
                            lbl_lv_tankb_m.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANKB_LEVEL_L: //TANKB_LEVEL_L
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tankb_l.BackColor = Program.schematic_form.Level_on;
                        }
                        else
                        {
                            lbl_lv_tankb_l.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANKB_LEVEL_LL: //TANKB_LEVEL_LL
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_lv_tankb_ll.BackColor = Program.schematic_form.Level_on;
                        }
                        else
                        {
                            lbl_lv_tankb_ll.BackColor = Program.schematic_form.Level_off;
                        }
                        break;
                    case (int)Config_IO.enum_di.TANKB_EMPTY_CHECK: //TANKB_LEVEL_EMPTY
                        if (Program.IO.DI.Tag[idx].value == false)
                        {
                            lbl_lv_tankb_empty.BackColor = Program.schematic_form.Level_on;
                        }
                        else
                        {
                            lbl_lv_tankb_empty.BackColor = Program.schematic_form.Level_off;
                        }
                        break;

                    //CIRCULATION PUMP STATUS
                    case (int)Config_IO.enum_di.CIRCULATION_PUMP_LEFT_ON:
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            pnl_circulation_pump_sensor_left.BackgroundImage = Properties.Resources.LED_GREEN_NONE;
                        }
                        else
                        {
                            pnl_circulation_pump_sensor_left.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                        }
                        break;

                    case (int)Config_IO.enum_di.CIRCULATION_PUMP_RIGHT_ON:
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            pnl_circulation_pump_sensor_right.BackgroundImage = Properties.Resources.LED_GREEN_NONE;
                        }
                        else
                        {
                            pnl_circulation_pump_sensor_right.BackgroundImage = Properties.Resources.LED_GRAY_NONE;
                        }
                        break;
                }
            }
            //DO
            for (int idx = 0; idx < Program.IO.DO.use_cnt; idx++)
            {
                switch (idx)
                {
                    //case (int)Config_IO.enum_do.SUPPLY_A_THERMOSTAT_PWR_ON: //sup a heater power on
                    //    if (Program.IO.DO.Tag[idx].value == true) { btn_heater_he41.ImageOptions.Image = Properties.Resources.heater_on; }
                    //    else { btn_heater_he41.ImageOptions.Image = Properties.Resources.heater_off; }
                    //    break;
                    //case (int)Config_IO.enum_do.SUPPLY_B_THERMOSTAT_PWR_ON: //sup b heater power on
                    //    if (Program.IO.DO.Tag[idx].value == true) { btn_heater_he42.ImageOptions.Image = Properties.Resources.heater_on; }
                    //    else { btn_heater_he42.ImageOptions.Image = Properties.Resources.heater_off; }
                    //    break;
                    //case (int)Config_IO.enum_do.CIRCULATION_THERMOSTAT_PWR_ON: //circulat1 heater power on
                    //    if (Program.IO.DO.Tag[idx].value == true) { btn_heater_he21.ImageOptions.Image = Properties.Resources.heater_on; }
                    //    else { btn_heater_he21.ImageOptions.Image = Properties.Resources.heater_off; }
                    //    break;

                    case (int)Config_IO.enum_do.Drain_Pump_On: //drain pump on
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            btn_pump_bp61.ImageOptions.Image = Properties.Resources.pump_h_on;
                        }
                        else
                        {
                            btn_pump_bp61.ImageOptions.Image = Properties.Resources.pump_h_off;
                        }
                        break;

                    //av
                    case (int)Config_IO.enum_do.DIW_SUPPLY_TANK_A: //diw-s tank a
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av01.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av01.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.DIW_SUPPLY_TANK_B: //diw-s tank b
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av02.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av02.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.HF_SUPPLY_TANK_A: //dsp-s tank a
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av03.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av03.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.HF_SUPPLY_TANK_B: //dsp-s tank b
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av04.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av04.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_A: // pcw h/e supply-a
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av11.ImageOptions.Image = Properties.Resources.valve_h_open; }
                        else { btn_valve_av11.ImageOptions.Image = Properties.Resources.valve_h_close; }
                        break;
                    case (int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_B: // pcw h/e supply-b
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av12.ImageOptions.Image = Properties.Resources.valve_h_open; }
                        else { btn_valve_av12.ImageOptions.Image = Properties.Resources.valve_h_close; }
                        break;
                    case (int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_CIR: // pcw h/e circulation
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av13.ImageOptions.Image = Properties.Resources.valve_h_open; }
                        else { btn_valve_av13.ImageOptions.Image = Properties.Resources.valve_h_close; }
                        break;
                    case (int)Config_IO.enum_do.CIR_FROM_TANK_A: //circuit tank a out
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av21.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av21.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.CIR_FROM_TANK_B: //circuit tank b out
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av22.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av22.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.CIR_TO_TANK_A: //circuit tank a in
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            btn_valve_av23.ImageOptions.Image = Properties.Resources.valve_v_open;
                        }
                        else
                        {
                            btn_valve_av23.ImageOptions.Image = Properties.Resources.valve_v_close;
                        }
                        break;
                    case (int)Config_IO.enum_do.CIR_TO_TANK_B: //circuit tank b in
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av24.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av24.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.CIR_DRAIN: //circuit drain (N/O drain N/C circuit
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av25.ImageOptions.Image = Properties.Resources.valve_3w_v_l_open; }
                        else { btn_valve_av25.ImageOptions.Image = Properties.Resources.valve_3w_v_l_close; }
                        break;
                    case (int)Config_IO.enum_do.MAIN_RETURN_DRAIN: //chemical return drain
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av31.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av31.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1: //chemical return drain
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av32.ImageOptions.Image = Properties.Resources.valve_3w_h_up_open; }
                        else { btn_valve_av32.ImageOptions.Image = Properties.Resources.valve_3w_h_up_close; }
                        break;
                    case (int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2: //chemical return drain
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av33.ImageOptions.Image = Properties.Resources.valve_3w_h_down_open; }
                        else { btn_valve_av33.ImageOptions.Image = Properties.Resources.valve_3w_h_down_close2; }
                        break;
                    case (int)Config_IO.enum_do.RETURN_TO_TANK_A: //chemical return tank a
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av35.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av35.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.RETURN_TO_TANK_B: //chemical return tank b
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av36.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av36.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A: //concentration return tank a
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av37.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av37.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B: //concentration return tank b
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av38.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av38.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;


                    case (int)Config_IO.enum_do.SUPPLY_FROM_TANK_A: //supply tank a out
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            btn_valve_av41.ImageOptions.Image = Properties.Resources.valve_h_open;
                        }
                        else
                        {
                            btn_valve_av41.ImageOptions.Image = Properties.Resources.valve_h_close;
                        }
                        break;
                    case (int)Config_IO.enum_do.SUPPLY_FROM_TANK_B: //supply tank b out
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av42.ImageOptions.Image = Properties.Resources.valve_h_open; }
                        else { btn_valve_av42.ImageOptions.Image = Properties.Resources.valve_h_close; }
                        break;
                    case (int)Config_IO.enum_do.SUPPLY_TO_MAIN_A: //supply tank a drain (N/O drain N/C supply)
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av43.ImageOptions.Image = Properties.Resources.valve_3w_v_r_open; }
                        else { btn_valve_av43.ImageOptions.Image = Properties.Resources.valve_3w_v_r_close; }
                        break;
                    case (int)Config_IO.enum_do.SUPPLY_TO_MAIN_B: //supply tank b drain (N/O drain N/C supply)
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av44.ImageOptions.Image = Properties.Resources.valve_3w_v_r_open; }
                        else { btn_valve_av44.ImageOptions.Image = Properties.Resources.valve_3w_v_r_close; }
                        break;
                    case (int)Config_IO.enum_do.TANK_A_DRAIN: //tank a drain
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av45.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av45.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.TANK_B_DRAIN: //tank b drain
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av46.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av46.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.SAMPLING: //sampling port
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av51.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av51.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.Drain_Tank_V_V_On: //drain tank out
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av61.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av61.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.Vet_V_V_On: //vat in?
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av62.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av62.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    //CIRCULATION PUMP SOL STATUS
                    case (int)Config_IO.enum_do.CIRCULATION_PUMP_LEFT_ON:
                        if (Program.IO.DO.Tag[idx].value == true) { pnl_circulation_pump_sol_left.BackgroundImage = Properties.Resources.LED_GREEN_NONE; }
                        else { pnl_circulation_pump_sol_left.BackgroundImage = Properties.Resources.LED_GRAY_NONE; }
                        break;
                    case (int)Config_IO.enum_do.CIRCULATION_PUMP_RIGHT_ON:
                        if (Program.IO.DO.Tag[idx].value == true) { pnl_circulation_pump_sol_right.BackgroundImage = Properties.Resources.LED_GREEN_NONE; }
                        else { pnl_circulation_pump_sol_right.BackgroundImage = Properties.Resources.LED_GRAY_NONE; }
                        break;
                }
            }
            //AI
            for (int idx = 0; idx < Program.IO.AI.use_cnt; idx++)
            {
                switch (idx)
                {
                    case (int)Config_IO.enum_ai.EXHAUST:
                        lbl_exhaust.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " Pa";
                        break;
                    case (int)Config_IO.enum_ai.CIRCULATION_PRESS:
                        lbl_press_ps21.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS:
                        lbl_press_ps41.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.SUPPLY_A_FILTER_OUT_PRESS:
                        lbl_press_ps43.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS:
                        lbl_press_ps42.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.SUPPLY_B_FILTER_OUT_PRESS:
                        lbl_press_ps44.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.SUPPLY_A_FLOW:
                        lbl_flow_fm41.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.SUPPLY_B_FLOW:
                        lbl_flow_fm42.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.CIRCULATION_FLOW:
                        lbl_flow_fm21.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.DIW_SUPPLY_PRESS:
                        lbl_press_ps01.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS:
                        lbl_press_ps03.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS:
                        lbl_press_ps04.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.CHEMICAL_RETURN_A:
                        lbl_press_ps31.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.CHEMICAL_RETURN_B:
                        lbl_press_ps32.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.TANKA_PN2_FLOW:
                        lbl_flow_fi11.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.TANKB_PN2_FLOW:
                        lbl_flow_fi12.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.MAIN_PN2_PRESS:
                        lbl_press_ps12.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.MAIN_CDA1_PRESS_PUMP:
                        lbl_press_ps14.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.MAIN_CDA2_PRESS_SOL:
                        lbl_press_ps13.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.MAIN_CDA3_PRESS_DRAIN:
                        lbl_press_ps15.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.DRAIN_FLOW:
                        lbl_flow_fm61.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.MAIN_PCW_PRESS:
                        lbl_press_ps11.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.SUPPLY_A_THERMOSTAT_PCW_FLOW:
                        lbl_flow_fm11.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.SUPPLY_B_THERMOSTAT_PCW_FLOW:
                        lbl_flow_fm12.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.CIRCULATION_THERMOSTAT_PCW_FLOW:
                        lbl_flow_fm13.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.CONCENTRATION_PRESS:
                        lbl_press_ps33.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                        //case (int)Config_IO.enum_ai.DIW_SUPPLY_FLOW:
                        //    lbl_flow_fm01.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        //    break;
                        //case (int)Config_IO.enum_ai.DSP_SUPPLY_FLOW:
                        //    lbl_flow_fm03.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        //    break;
                }
            }
            //DIW

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true ||
                Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
            {
                pnl_pip_diw01.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_diw02.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_diw02j.BackColor = Program.schematic_form.Pipe_on_ccss;
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true)
                {
                    pnl_pip_diw03.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_diw04.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_diw05.BackColor = Program.schematic_form.Pipe_on_ccss;
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                {
                    pnl_pip_diw06.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_diw07.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_diw08.BackColor = Program.schematic_form.Pipe_on_ccss;
                }

            }

            //DHF-S
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true ||
                Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
            {
                pnl_pip_hf01.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_hf02.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_hf02j.BackColor = Program.schematic_form.Pipe_on_ccss;
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true)
                {
                    pnl_pip_hf03.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_hf04.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_hf05.BackColor = Program.schematic_form.Pipe_on_ccss;
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                {
                    pnl_pip_hf06.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_hf07.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_hf08.BackColor = Program.schematic_form.Pipe_on_ccss;
                }
            }

            //LS Over
            //A
            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_OVERFLOW_CHECK].value == true)
            {
                pnl_pip_over01.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_over02.BackColor = Program.schematic_form.Pipe_on_ccss;
            }
            //B
            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_OVERFLOW_CHECK].value == true)
            {
                pnl_pip_over03.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_over04.BackColor = Program.schematic_form.Pipe_on_ccss;
            }

            //PN2-S
            // 조건문 필요 lbl_flow_fi11 lbl_flow_fi12 lbl_press_ps12 참조
            if (true ||
               true)
            {
                pnl_pip_pn01.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_pn01j.BackColor = Program.schematic_form.Pipe_on_ccss;
                //a
                if (true)
                {
                    pnl_pip_pn02.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_pn03.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_pn04.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_pn05.BackColor = Program.schematic_form.Pipe_on_ccss;
                }
                //b
                if (true)
                {
                    pnl_pip_pn06.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_pn07.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_pn08.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_pn09.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_pn10.BackColor = Program.schematic_form.Pipe_on_ccss;
                }
            }


            //CDA
            //조건문 필요

            if (true ||
                true ||
                true)
            {
                //SOL
                pnl_pip_cda01.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_cda01j.BackColor = Program.schematic_form.Pipe_on_ccss;
                if (true)
                {
                    pnl_pip_cda02.BackColor = Program.schematic_form.Pipe_on_ccss;
                }
                if (true || true)
                {
                    pnl_pip_cda03.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_cda03j.BackColor = Program.schematic_form.Pipe_on_ccss;

                }
                //PUMP
                if (true)
                {
                    pnl_pip_cda04.BackColor = Program.schematic_form.Pipe_on_ccss;
                }
                //DRAIN
                if (true)
                {
                    pnl_pip_cda05.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_cda06.BackColor = Program.schematic_form.Pipe_on_ccss;
                }

            }

            //WASTE DRAIN & VAT
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.Drain_Pump_On].value == true)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.Drain_Tank_V_V_On].value == true)
                {
                    pnl_pip_wastedr01.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_wastedr02.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_wastedr03.BackColor = Program.schematic_form.Pipe_on;
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.Vet_V_V_On].value == true)
                {
                    pnl_pip_wastedr04.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_wastedr05.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_wastedr06.BackColor = Program.schematic_form.Pipe_on;
                }

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.Drain_Tank_V_V_On].value == true ||
                   Program.IO.DO.Tag[(int)Config_IO.enum_do.Vet_V_V_On].value == true)
                {
                    pnl_pip_wastedr03j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_wastedr07.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_wastedr08.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_wastedr09.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_wastedr10.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_wastedr11.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_wastedr12.BackColor = Program.schematic_form.Pipe_on;

                }
            }

            //SUPPLY
            //av41
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true)
            {

                //pnl_pip_supply01.BackColor = Pipe_on;
                // pnl_pip_supply01j.BackColor = Pipe_on;
                pnl_pip_supply06.BackColor = Program.schematic_form.Pipe_on_supply;

                if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true ||
                   Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                {
                    pnl_pip_supply07.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply07j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply14.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply14j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply14j.BackColor = Program.schematic_form.Pipe_on;

                    if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                    {
                        pnl_pip_supply15_r.BackColor = Program.schematic_form.Pipe_on;
                    }
                    if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                    {
                        pnl_pip_supply23.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_supply24_r.BackColor = Program.schematic_form.Pipe_on;
                    }

                }
            }

            //AV42
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true)
            {
                pnl_pip_supply12.BackColor = Program.schematic_form.Pipe_on_supply;

                if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true ||
                   Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                {
                    pnl_pip_supply07j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply14.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply14j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply13.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply14j.BackColor = Program.schematic_form.Pipe_on;

                    if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                    {
                        pnl_pip_supply15_r.BackColor = Program.schematic_form.Pipe_on;
                    }
                    if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                    {
                        pnl_pip_supply23.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_supply24_r.BackColor = Program.schematic_form.Pipe_on;
                    }
                }
            }

            //av21 - 1
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
            {
                pnl_pip_supply01.BackColor = Program.schematic_form.Pipe_on_circulation;
                pnl_pip_supply01j.BackColor = Program.schematic_form.Pipe_on_circulation;
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_A_DRAIN].value == true)
            {
                pnl_pip_supply01.BackColor = Program.schematic_form.Pipe_on_drain;
                pnl_pip_supply01j.BackColor = Program.schematic_form.Pipe_on_drain;
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true)
            {
                pnl_pip_supply01.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_supply01j.BackColor = Program.schematic_form.Pipe_on_supply;
            }
            //av22 - 1
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
            {
                pnl_pip_supply08.BackColor = Program.schematic_form.Pipe_on_circulation;
                pnl_pip_supply08j.BackColor = Program.schematic_form.Pipe_on_circulation;
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_B_DRAIN].value == true)
            {
                pnl_pip_supply08.BackColor = Program.schematic_form.Pipe_on_drain;
                pnl_pip_supply08j.BackColor = Program.schematic_form.Pipe_on_drain;
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true)
            {
                pnl_pip_supply08.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_supply08j.BackColor = Program.schematic_form.Pipe_on_supply;
            }

            //av41
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_A_DRAIN].value == true)
            {
                pnl_pip_supply02.BackColor = Program.schematic_form.Pipe_on_drain;
                pnl_pip_supply03.BackColor = Program.schematic_form.Pipe_on_drain;
                pnl_pip_supply04.BackColor = Program.schematic_form.Pipe_on_drain;
            }

            //av21 - 2
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
            {
                pnl_pip_cir01.BackColor = Program.schematic_form.Pipe_on_circulation;
                //pnl_pip_cir02.BackColor = Pipe_on;
                //pnl_pip_cir03.BackColor = Pipe_on;
            }
            //av22 - 2
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
            {
                pnl_pip_cir04.BackColor = Program.schematic_form.Pipe_on_circulation;
                //pnl_pip_cir05.BackColor = Pipe_on;
                //pnl_pip_cir06.BackColor = Pipe_on;
            }
            //av42
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_B_DRAIN].value == true)
            {
                pnl_pip_supply09.BackColor = Program.schematic_form.Pipe_on_drain;
                pnl_pip_supply10.BackColor = Program.schematic_form.Pipe_on_drain;
                pnl_pip_supply11.BackColor = Program.schematic_form.Pipe_on_drain;
            }


            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true ||
                Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true)
            {

                //BP42
                if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                {
                    pnl_pip_supply15_l.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply16.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply17.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply18.BackColor = Program.schematic_form.Pipe_on;
                }

                //BP41
                if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                {
                    pnl_pip_supply24_l.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply25.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply26.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply27.BackColor = Program.schematic_form.Pipe_on;
                }

            }







            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true
            || Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true)
            {
                //supply a
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true)
                {
                    if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                    {
                        pnl_pip_return05.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return07.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return07j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_supply30.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_supply31.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return03.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return04.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;
                    }
                }
                else
                {
                    if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                    {
                        pnl_pip_supply28.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_supply29.BackColor = Program.schematic_form.Pipe_on;
                    }
                }
                //supply b
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true)
                {
                    if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                    {
                        pnl_pip_supply21.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_supply22.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return01.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return02.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return04.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return05.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return07.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return07j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                    }
                }
                else
                {
                    if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                    {

                        pnl_pip_supply19.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_supply20.BackColor = Program.schematic_form.Pipe_on;
                    }
                }
            }


            //supply 공통 
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true ||
                Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true)
            {
                //av32
                //pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                //SUPPLY a, b가 교차로 이뤄질때 
                if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true &&
                    Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == false)
                {
                    if (pnl_pip_supply21.BackColor == Program.schematic_form.Pipe_on)
                    {
                        pnl_pip_return05.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return07.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return07j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return04.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;

                    }

                }
                else if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true &&
                    Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == false)
                {
                    if (pnl_pip_supply31.BackColor == Program.schematic_form.Pipe_on)
                    {
                        pnl_pip_return05.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return07.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return07j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return04.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;
                    }
                }
                else if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true &&
                  Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true)
                {
                    if (pnl_pip_supply21.BackColor == Program.schematic_form.Pipe_on)
                    {
                        pnl_pip_return05.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return07.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return07j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return04.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;
                    }

                }

                //SUPPLY A라인이 모두 공급중일 떄 PIPE 전체 ON Refresh
                if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true &&
                    Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true &&
                    Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true)
                {
                    pnl_pip_return03.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return05.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return07.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return07j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return04.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;
                }

                //SUPPLY B라인이 모두 공급중일 떄 PIPE 전체 ON Refresh
                if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true &&
                    Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true &&
                    Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true)
                {

                    pnl_pip_return01.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return02.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return05.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return07.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return07j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return04.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;
                }
            }

            //CONCEN02
            //Supply -> Return에서 흐를때 변경
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1].value == true)
            {
                if (pnl_pip_return08.BackColor == Program.schematic_form.Pipe_on)
                {
                    pnl_pip_concen02.BackColor = Program.schematic_form.Pipe_on;
                }
            }
            else
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true ||
                Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                {
                    if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_DRAIN].value == true)
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true
                           || Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                            {
                                pnl_pip_concen02.BackColor = Program.schematic_form.Pipe_on;
                            }
                        }
                    }
                }
            }


            //av35,36 공통
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true
                    || Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true
                                    || Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true)
                {
                    pnl_pip_return09.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return10.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return11.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return11j.BackColor = Program.schematic_form.Pipe_on;
                }
                //av35
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true)
                {
                    pnl_pip_return12.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return13.BackColor = Program.schematic_form.Pipe_on;
                }
                //av36
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true)
                {
                    pnl_pip_return15.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return16.BackColor = Program.schematic_form.Pipe_on;
                }
                //av31
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true)
                {
                    pnl_pip_return06.BackColor = Program.schematic_form.Pipe_on;
                }
            }


            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true)
            {
                pnl_pip_return14.BackColor = Program.schematic_form.Pipe_on;
            }

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true)
            {
                pnl_pip_return17.BackColor = Program.schematic_form.Pipe_on;
            }


            //CIRCULATION PIPE
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true ||
                Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
            {


                if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                    {
                        pnl_pip_cir02.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_cir03.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_cir03j.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_cir07.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_cir08_r.BackColor = Program.schematic_form.Pipe_on_circulation;
                    }

                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                    {

                        pnl_pip_cir05.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_cir06.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_cir03j.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_cir07.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_cir08_r.BackColor = Program.schematic_form.Pipe_on_circulation;
                    }

                    pnl_pip_cir08_l.BackColor = Program.schematic_form.Pipe_on_circulation;
                    pnl_pip_cir09.BackColor = Program.schematic_form.Pipe_on_circulation;
                }
            }

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_DRAIN].value == true)
            {
                if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true
                       || Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                    {
                        pnl_pip_cir10.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_cir10j.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_concen01.BackColor = Program.schematic_form.Pipe_on_circulation;

                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_A].value == true
                && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_B].value == false)
                        {
                            pnl_pip_cir11.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir11j.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_sp01.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_sp02.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir13.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir14.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir12.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir12j.BackColor = Program.schematic_form.Pipe_on_circulation;

                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_B].value == true
                            && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_A].value == false)
                        {

                            pnl_pip_cir11.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir11j.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_sp01.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_sp02.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir12.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir12j.BackColor = Program.schematic_form.Pipe_on_circulation;

                            pnl_pip_cir15.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir16.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir17.BackColor = Program.schematic_form.Pipe_on_circulation;

                        }
                        else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_A].value == true
                            && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_B].value == true)
                        {

                            pnl_pip_cir11.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir11j.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_sp01.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_sp02.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir13.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir14.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir12.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir12j.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir15.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir16.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir17.BackColor = Program.schematic_form.Pipe_on_circulation;
                        }
                    }
                }


                //av23
                //av24

            }
            else
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true
                    || Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                {
                    if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                    {
                        pnl_pip_chedr_r01.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_chedr_r02.BackColor = Program.schematic_form.Pipe_on_circulation;
                        pnl_pip_chedr_r03.BackColor = Program.schematic_form.Pipe_on_circulation;
                    }
                }

            }
            //sampling

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true ||
                Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
            {
                if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_DRAIN].value == true)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SAMPLING].value == true)
                        {
                            pnl_pip_cir10.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir10j.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir11.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_cir11j.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_sp01.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_sp02.BackColor = Program.schematic_form.Pipe_on_circulation;
                            pnl_pip_sp03.BackColor = Program.schematic_form.Pipe_on_circulation;
                        }
                    }
                }
            }

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_A].value == true)
            {
                pnl_pip_cir14.BackColor = Program.schematic_form.Pipe_on_circulation;
            }

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_B].value == true)
            {
                pnl_pip_cir17.BackColor = Program.schematic_form.Pipe_on_circulation;
            }

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2].value == true)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A].value == true)
                {
                    pnl_pip_concen05.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_concen06.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_concen06j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_concen07.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_concen08.BackColor = Program.schematic_form.Pipe_on;
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B].value == true)
                {
                    pnl_pip_concen05.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_concen06.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_concen06j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_concen10.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_concen11.BackColor = Program.schematic_form.Pipe_on;
                }

            }
            else
            {
                if (pnl_pip_concen02.BackColor == Program.schematic_form.Pipe_on)
                {
                    pnl_pip_concen03.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_concen04.BackColor = Program.schematic_form.Pipe_on;
                }
            }

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A].value == true)
            {
                pnl_pip_concen09.BackColor = Program.schematic_form.Pipe_on;
            }

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B].value == true)
            {
                pnl_pip_concen12.BackColor = Program.schematic_form.Pipe_on;
            }

            //PCW-S
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_CIR].value == true ||
                Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_A].value == true ||
                Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_B].value == true)
            {
                pnl_pip_pcw01.BackColor = Program.schematic_form.Pipe_on;
                pnl_pip_pcw01j.BackColor = Program.schematic_form.Pipe_on;

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_CIR].value == true)
                {
                    pnl_pip_pcw02.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_pcw03.BackColor = Program.schematic_form.Pipe_on;
                }

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_A].value == true ||
                     Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_B].value == true)
                {
                    pnl_pip_pcw04.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_pcw04j.BackColor = Program.schematic_form.Pipe_on;

                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_A].value == true)
                    {
                        pnl_pip_pcw05.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_pcw06.BackColor = Program.schematic_form.Pipe_on;
                    }
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_B].value == true)
                    {
                        pnl_pip_pcw07.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_pcw08.BackColor = Program.schematic_form.Pipe_on;
                        pnl_pip_pcw09.BackColor = Program.schematic_form.Pipe_on;
                    }
                }
            }
        }


        private void btn_pump_bp21_Click(object sender, EventArgs e)
        {
            int idx;
            string result = "";
            DevExpress.XtraEditors.SimpleButton event_btn = (DevExpress.XtraEditors.SimpleButton)sender;
            try
            {
                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto || Program.cg_app_info.semi_auto_state == true)
                {
                    Program.main_md.Message_By_Application("Cannot Operate in Auto or Semi-Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                }
                else if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
                {
                    Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
                }
                else
                {

                }

                switch (event_btn.Name)
                {
                    case "btn_pump_bp21": // circulation pump
                        if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                        {
                            Program.schematic_form.CIRCULATION_1_HEATER_ON_OFF(false);
                            Program.schematic_form.CIRCULATION_PUMP_ON_OFF(false);
                        }
                        else
                        {
                            result = Program.schematic_form.CIRCULATION_PUMP_ON_OFF(true);
                            if (result != "")
                            {
                                Program.main_md.Message_By_Application(result, frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;

                    case "btn_pump_bp41": // supply a pump
                        if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                        {
                            Program.schematic_form.SUPPLY_A_HEATER_ON_OFF(false);
                            Program.schematic_form.SUPPLY_A_PUMP_ON_OFF(false);
                        }
                        else
                        {
                            result = Program.schematic_form.SUPPLY_A_PUMP_ON_OFF(true);
                            if (result != "")
                            {
                                Program.main_md.Message_By_Application(result, frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;

                    case "btn_pump_bp42": // supply b pump
                        if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                        {
                            Program.schematic_form.SUPPLY_B_HEATER_ON_OFF(false);
                            Program.schematic_form.SUPPLY_B_PUMP_ON_OFF(false);
                        }
                        else
                        {
                            result = Program.schematic_form.SUPPLY_B_PUMP_ON_OFF(true);
                            if (result != "")
                            {
                                Program.main_md.Message_By_Application(result, frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;

                    case "btn_pump_bp61": // drain pump
                        idx = (int)Config_IO.enum_do.Drain_Pump_On;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                        }
                        else
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.Drain_Tank_V_V_On].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.Vet_V_V_On].value == true)
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                                //if (Program.IO.DI.Tag[(int)Config_IO.enum_di.Drain_Pump_On_Level].value == true || Program.IO.DI.Tag[(int)Config_IO.enum_di.BOTTOM_VAT_LEAK1].value == true)
                                //{
                                //    Program.ethercat_md.DO_Write_Alone(idx, true);
                                //}
                                //else
                                //{
                                //    Program.main_md.Message_By_Application("Cannot Run Pump, Drain Valve is invalid", frm_messagebox.enum_apptype.Only_OK);
                                //}
                            }
                            else
                            {
                                Program.main_md.Message_By_Application("Cannot Run Pump, Drain Valve is invalid", frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".btn_pump_bp61_Click." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);

            }
        }

        private void btn_heater_he21_Click(object sender, EventArgs e)
        {
            int idx = 0;
            string result = "";

            try
            {
                DevExpress.XtraEditors.SimpleButton event_btn = (DevExpress.XtraEditors.SimpleButton)sender;

                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto || Program.cg_app_info.semi_auto_state == true)
                {
                    Program.main_md.Message_By_Application("Cannot Operate in Auto or Semi-Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                }
                else if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
                {
                    Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
                }
                else
                {

                }

                switch (event_btn.Name)
                {
                    case "btn_heater_he21": // circulation1 heater
                        idx = (int)Config_IO.enum_do.CIRCULATION_THERMOSTAT_PWR_ON;
                        if (Program.IO.DO.Tag[idx].value == true && Program.main_form.SerialData.Circulation_Thermostat.heater_on == true)
                        {
                            Program.schematic_form.CIRCULATION_1_HEATER_ON_OFF(false);
                        }
                        else if (Program.IO.DO.Tag[idx].value == true && Program.main_form.SerialData.Circulation_Thermostat.heater_on == false)
                        {

                            result = Program.schematic_form.CIRCULATION_1_HEATER_ON_OFF(true);
                            if (result != "")
                            {
                                Program.main_md.Message_By_Application(result, frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        else
                        {
                            result = "Circulation Thermostat Power Off";
                            if (result != "")
                            {
                                Program.main_md.Message_By_Application(result, frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                    case "btn_heater_he41": // supply a heater
                        idx = (int)Config_IO.enum_do.SUPPLY_A_THERMOSTAT_PWR_ON;
                        if (Program.IO.DO.Tag[idx].value == true && Program.main_form.SerialData.Supply_A_Thermostat.heater_on == true)
                        {
                            Program.schematic_form.SUPPLY_A_HEATER_ON_OFF(false);
                        }
                        else if (Program.IO.DO.Tag[idx].value == true && Program.main_form.SerialData.Supply_A_Thermostat.heater_on == false)
                        {

                            result = Program.schematic_form.SUPPLY_A_HEATER_ON_OFF(true);
                            if (result != "")
                            {
                                Program.main_md.Message_By_Application(result, frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        else
                        {
                            result = "Supply A Thermostat Power Off";
                            if (result != "")
                            {
                                Program.main_md.Message_By_Application(result, frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                    case "btn_heater_he42": // supply b heater
                        idx = (int)Config_IO.enum_do.SUPPLY_B_THERMOSTAT_PWR_ON;
                        if (Program.IO.DO.Tag[idx].value == true && Program.main_form.SerialData.Supply_B_Thermostat.heater_on == true)
                        {
                            Program.schematic_form.SUPPLY_B_HEATER_ON_OFF(false);
                        }
                        else if (Program.IO.DO.Tag[idx].value == true && Program.main_form.SerialData.Supply_B_Thermostat.heater_on == false)
                        {
                            result = Program.schematic_form.SUPPLY_B_HEATER_ON_OFF(true);
                            if (result != "")
                            {
                                Program.main_md.Message_By_Application(result, frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        else
                        {
                            result = "Supply B Thermostat Power Off";
                            if (result != "")
                            {
                                Program.main_md.Message_By_Application(result, frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".btn_heater_he21_Click." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }

        private void btn_valve_av01_Click(object sender, EventArgs e)
        {
            int idx;
            try
            {
                DevExpress.XtraEditors.SimpleButton event_btn = (DevExpress.XtraEditors.SimpleButton)sender;

                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto || Program.cg_app_info.semi_auto_state == true)
                {
                    Program.main_md.Message_By_Application("Cannot Operate in Auto or Semi-Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                }
                else if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
                {
                    Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
                }
                else
                {

                }
                switch (event_btn.Name)
                {
                    case "btn_valve_av01": //diw-s tank a
                        idx = (int)Config_IO.enum_do.DIW_SUPPLY_TANK_A;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.schematic_form.CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS4);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve(Level H)", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.schematic_form.CCSS_Valve_Use_Check_By_Mixing_Rate(enum_ccss.CCSS4) == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve(Check Mixing Rate)", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.schematic_form.CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS4) == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve", frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                    case "btn_valve_av02": //diw-s tank b
                        idx = (int)Config_IO.enum_do.DIW_SUPPLY_TANK_B;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.schematic_form.CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS4);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve(Level H)", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.schematic_form.CCSS_Valve_Use_Check_By_Mixing_Rate(enum_ccss.CCSS4) == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve(Check Mixing Rate)", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.schematic_form.CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS4) == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve", frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                    case "btn_valve_av03": //dsp-s tank a
                        idx = (int)Config_IO.enum_do.HF_SUPPLY_TANK_A;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.schematic_form.CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve(Level H)", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.schematic_form.CCSS_Valve_Use_Check_By_Mixing_Rate(enum_ccss.CCSS1) == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve(Check Mixing Rate)", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.schematic_form.CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1) == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve", frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                    case "btn_valve_av04": //dsp-s tank b
                        idx = (int)Config_IO.enum_do.HF_SUPPLY_TANK_B;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.schematic_form.CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS1);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve(Level H)", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.schematic_form.CCSS_Valve_Use_Check_By_Mixing_Rate(enum_ccss.CCSS1) == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve(Check Mixing Rate)", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.schematic_form.CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS1) == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve", frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                    case "btn_valve_av11": //pcw h/e supply-a
                        idx = (int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_A;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else { Program.ethercat_md.DO_Write_Alone(idx, true); }
                        break;
                    case "btn_valve_av12": //pcw h/e supply-b
                        idx = (int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_B;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else { Program.ethercat_md.DO_Write_Alone(idx, true); }
                        break;
                    case "btn_valve_av13": //pcw h/e circulation
                        idx = (int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_CIR;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else { Program.ethercat_md.DO_Write_Alone(idx, true); }
                        break;
                    case "btn_valve_av21": //circuit tank a out
                        idx = (int)Config_IO.enum_do.CIR_FROM_TANK_A;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                            Program.schematic_form.CIRCULATION_1_HEATER_ON_OFF(false);
                            Program.schematic_form.CIRCULATION_PUMP_ON_OFF(false);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank B Circulation Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_A_DRAIN].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Drain Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Supply Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);

                            }
                        }
                        break;
                    case "btn_valve_av22": //circuit tank b out
                        idx = (int)Config_IO.enum_do.CIR_FROM_TANK_B;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                            Program.schematic_form.CIRCULATION_1_HEATER_ON_OFF(false);
                            Program.schematic_form.CIRCULATION_PUMP_ON_OFF(false);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank A Circulation Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_B_DRAIN].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Drain Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Supply Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av23": //circuit tank a in
                        idx = (int)Config_IO.enum_do.CIR_TO_TANK_A;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_DRAIN].value == true)
                            {
                                Program.schematic_form.CIRCULATION_PUMP_ON_OFF(false);
                            }
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Supply Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av24": //circuit tank b in
                        idx = (int)Config_IO.enum_do.CIR_TO_TANK_B;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_DRAIN].value == true)
                            {
                                Program.schematic_form.CIRCULATION_PUMP_ON_OFF(false);
                            }
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Supply Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av25": //circuit drain (N/O drain N/C circuit
                        idx = (int)Config_IO.enum_do.CIR_DRAIN;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_A].value == true)
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_B].value == true)
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                            else
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank A Circulation Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                    case "btn_valve_av31": //chemical return drain
                        idx = (int)Config_IO.enum_do.MAIN_RETURN_DRAIN;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            if ((Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == false) && Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                            {
                                Program.schematic_form.SUPPLY_A_PUMP_ON_OFF(false);
                                Program.schematic_form.SUPPLY_B_PUMP_ON_OFF(false);
                            }
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                        }
                        else { Program.ethercat_md.DO_Write_Alone(idx, true); }
                        break;
                    case "btn_valve_av32": //chemical return concentration
                        idx = (int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else { Program.ethercat_md.DO_Write_Alone(idx, true); }
                        break;

                    case "btn_valve_av33": //concentration return tank
                        idx = (int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else { Program.ethercat_md.DO_Write_Alone(idx, true); }
                        break;
                    case "btn_valve_av35": //chemical return tank a
                        idx = (int)Config_IO.enum_do.RETURN_TO_TANK_A;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == false && Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                            {
                                Program.schematic_form.SUPPLY_A_PUMP_ON_OFF(false);
                            }
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == false && Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                            {
                                Program.schematic_form.SUPPLY_B_PUMP_ON_OFF(false);
                            }
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank Return To B Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av36": //chemical return tank b
                        idx = (int)Config_IO.enum_do.RETURN_TO_TANK_B;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == false && Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                            {
                                Program.schematic_form.SUPPLY_A_PUMP_ON_OFF(false);
                            }
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == false && Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                            {
                                Program.schematic_form.SUPPLY_B_PUMP_ON_OFF(false);
                            }
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank Return To A Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av37": //concentration return tank a
                        idx = (int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank Return Sample To B Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av38": //concentration return tank b
                        idx = (int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank Return Sample To A Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av41": //supply tank a out
                        idx = (int)Config_IO.enum_do.SUPPLY_FROM_TANK_A;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                            Program.schematic_form.SUPPLY_A_PUMP_ON_OFF(false);
                            Program.schematic_form.SUPPLY_B_PUMP_ON_OFF(false);
                            Program.schematic_form.SUPPLY_A_HEATER_ON_OFF(false);
                            Program.schematic_form.SUPPLY_B_HEATER_ON_OFF(false);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank B Supply Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_A_DRAIN].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Drain Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Circulation Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av42": //supply tank b out
                        idx = (int)Config_IO.enum_do.SUPPLY_FROM_TANK_B;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                            Program.schematic_form.SUPPLY_A_PUMP_ON_OFF(false);
                            Program.schematic_form.SUPPLY_B_PUMP_ON_OFF(false);
                            Program.schematic_form.SUPPLY_A_HEATER_ON_OFF(false);
                            Program.schematic_form.SUPPLY_B_HEATER_ON_OFF(false);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank A Supply Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_B_DRAIN].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Drain Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Circulation Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av43": //supply main A 
                        idx = (int)Config_IO.enum_do.SUPPLY_TO_MAIN_A;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == false)
                            {
                                Program.main_md.Message_By_Application("Supply Valve Chain is invalid", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == false)
                            {
                                Program.main_md.Message_By_Application("Supply Valve Chain is invalid", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true)
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true)
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true)
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                            else
                            {
                                Program.main_md.Message_By_Application("Supply Valve Chain is invalid", frm_messagebox.enum_apptype.Only_OK);
                            }

                        }
                        break;
                    case "btn_valve_av44": //supply main B
                        idx = (int)Config_IO.enum_do.SUPPLY_TO_MAIN_B;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == false)
                            {
                                Program.main_md.Message_By_Application("Supply Valve Chain is invalid", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == false)
                            {
                                Program.main_md.Message_By_Application("Supply Valve Chain is invalid", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true)
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true)
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true)
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                            else
                            {
                                Program.main_md.Message_By_Application("Supply Valve Chain is invalid", frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                    case "btn_valve_av45": //tank a drain
                        idx = (int)Config_IO.enum_do.TANK_A_DRAIN;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_B_DRAIN].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank B Drain Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Circulation Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Supply Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av46": //tank b drain
                        idx = (int)Config_IO.enum_do.TANK_B_DRAIN;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                        }
                        else
                        {
                            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_B].enable == false)
                            {
                                Program.main_md.Message_By_Application("Tank Disabled", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_A_DRAIN].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Tank A Drain Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Circulation Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Supply Valve Opened", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                            }
                        }
                        break;
                    case "btn_valve_av51": //sampling port
                        idx = (int)Config_IO.enum_do.SAMPLING;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                        }
                        else
                        {
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.C_DOOR_ALARM].value == false
                               && Program.IO.DI.Tag[(int)Config_IO.enum_di.ELEC_DOOR_OVERRIDE].value == false
                               && Program.IO.DI.Tag[(int)Config_IO.enum_di.E_RIGHT_DOOR].value == false)
                            {
                                Program.ethercat_md.DO_Write_Alone(idx, true);
                                Program.schematic_form.DelayAction_CC_Response(Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Sampling_Port_Use_Delay_Time) * 1000, new Action(() => { Program.schematic_form.Sampling_Port_Close_By_Application(); }));
                            }
                            else
                            {
                                Program.main_md.Message_By_Application("All Door not Closed", frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;
                    case "btn_valve_av61": //drain tank out
                        idx = (int)Config_IO.enum_do.Drain_Tank_V_V_On;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else { Program.ethercat_md.DO_Write_Alone(idx, true); }
                        break;
                    case "btn_valve_av62": //vat in?
                        idx = (int)Config_IO.enum_do.Vet_V_V_On;
                        if (Program.IO.DO.Tag[idx].value == true) { Program.ethercat_md.DO_Write_Alone(idx, false); }
                        else { Program.ethercat_md.DO_Write_Alone(idx, true); }
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".av_click." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                Program.schematic_form.Concentration_Check(tank_class.enum_tank_type.TANK_A, true);
                Program.schematic_form.Concentration_Check(tank_class.enum_tank_type.TANK_B, true);
            }
        }

        private void chk_use_auto_drain_CheckedChanged(object sender, EventArgs e)
        {
            Program.seq.monitoring.use_auto_drain = chk_use_auto_drain.Checked;
        }

        private void chk_use_auto_drain_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
            {
                e.Cancel = true;
                Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
            }
        }

        private void lbl_temp_heater_he41_Click(object sender, EventArgs e)
        {
            if (view_heater_interlock_value == false)
            {
                view_heater_interlock_value = true;
                chk_offset_view.Visible = true;
            }
            else
            {
                view_heater_interlock_value = false;
                chk_offset_view.Visible = false;
            }
        }
    }
}
