using DevExpress.XtraDiagram.Base;
using System;
using System.Drawing;
using System.Windows.Forms;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

namespace cds
{
    public partial class UC_IPA : UserControl
    {
        public bool view_heater_interlock_value = false;
        public UC_IPA()
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
            //CC가 필요할 때
            pnl_cc_need_tank_a.Visible = Program.schematic_form.Visible_CC_status(tank_class.enum_tank_type.TANK_A);
            pnl_ctc_confirm_tank_a.Visible = Program.schematic_form.Visible_Confirm_status(tank_class.enum_tank_type.TANK_A);

            lbl_mixing_volume_name_a.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_name.ToString();
            lbl_mixing_volume_ratio_a.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_A].volume_ratio.ToString();
            ///SERIAL DATA
            lbl_flow_fm03.Text = string.Format("{0:f2}", Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow) + "/" + string.Format("{0:f2}", Program.main_form.SerialData.FlowMeter_Sonotec.IPA_totalusage) + "(LPM)";
            lbl_tank_a_temp.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv) + "℃";

            lbl_temp_ts41.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.pv) + "℃";
           

            if (view_heater_interlock_value == false)
            {
                lbl_temp_heater_he41.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv) + "℃";
            }
            else
            {
                if (chk_mv_view.Checked == true)
                {
                    lbl_temp_heater_he41.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.mv) + "%";
                }
                else if (chk_offset_view.Checked == true)
                {
                    lbl_temp_heater_he41.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.offset) + "℃";
                }
                else
                {
                    //SV
                    lbl_temp_heater_he41.Text = string.Format("{0:f1}", Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv) + "℃";
                }
            }
            lbl_he41.BackColor = Program.schematic_form.BackColor_Tank_By_TempController_status(ref Program.main_form.SerialData.TEMP_CONTROLLER.supply_a);

            //Level
            lbl_lv_tanka_hh.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_A, frm_schematic.enum_tank_level.HH);
            lbl_lv_tanka_h.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_A, frm_schematic.enum_tank_level.H);
            lbl_lv_tanka_m.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_A, frm_schematic.enum_tank_level.M);
            lbl_lv_tanka_ll.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_A, frm_schematic.enum_tank_level.LL);
            lbl_lv_tanka_l.Text = Program.schematic_form.Label_Tank_Level_status(tank_class.enum_tank_type.TANK_A, frm_schematic.enum_tank_level.L);


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


            if (Program.cg_app_info.tank_info[(int)tank_class.enum_tank_type.TANK_A].enable == false)
            {
                pnl_body_tank_a.Enabled = false;
            }
            else
            {
                lbl_tank_a_volume.Text = Program.schematic_form.Tank_Volume_View(Program.tank[(int)tank_class.enum_tank_type.TANK_A]);

                if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY)
                {
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].life_time_to_minute = (int)(DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_process).TotalMinutes;
                }
                else
                {
                    //초기화는 Drain 완료 후 적용
                }
                lbl_tank_a_lifetime.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_A].life_time_to_minute.ToString() + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Life_Time_High) + "(Min)";
                lbl_tank_a_wafercount.Text = Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt.ToString() + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Wafer_Count_High);

                lbl_state_tanka.BackColor = Program.tank[(int)tank_class.enum_tank_type.TANK_A].BackColor_Tank_By_Status(Program.tank[(int)tank_class.enum_tank_type.TANK_A].status);
                lbl_state_tanka.ForeColor = Program.tank[(int)tank_class.enum_tank_type.TANK_A].ForeColor_Tank_By_Status(Program.tank[(int)tank_class.enum_tank_type.TANK_A].status);

            }

            //DI
            for (int idx = 0; idx < Program.IO.DI.use_cnt; idx++)
            {
                switch (idx)
                {

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
                    case (int)Config_IO.enum_di.Purge_Unit_Alarm: //Purge Unit Alarm
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_purge_alarm.BackColor = Color.Red;
                        }
                        else
                        {
                            lbl_purge_alarm.BackColor = Color.Lime;
                        }
                        break;
                    case (int)Config_IO.enum_di.Gas_Detec_Alarm: //Gas_Detec_Alarm
                        if (Program.IO.DI.Tag[idx].value == true)
                        {
                            lbl_gas_detect_alarm.BackColor = Color.Red;
                        }
                        else
                        {
                            lbl_gas_detect_alarm.BackColor = Color.Lime;
                        }
                        break;
                    case (int)Config_IO.enum_di.Purge_Unit_OK: //Purge Unit OK
                        if (Program.IO.DI.Tag[idx].value == false)
                        {
                            lbl_purge_ok.BackColor = Color.Red;
                        }
                        else
                        {
                            lbl_purge_ok.BackColor = Color.Lime;
                        }
                        break;
                }
            }

            //DO
            for (int idx = 0; idx < Program.IO.DO.use_cnt; idx++)
            {
                switch (idx)
                {
                    case (int)Config_IO.enum_do.SUPPLY_A_HEATER_PWR_ON: //sup a heater power on
                        if (Program.IO.DO.Tag[idx].value == true) { btn_heater_he41.ImageOptions.Image = Properties.Resources.heater_on; }
                        else { btn_heater_he41.ImageOptions.Image = Properties.Resources.heater_off; }
                        break;
                    case (int)Config_IO.enum_do.IPA_SUPPLY_TANK: //IPA tank a
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av03.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av03.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.MAIN_RETURN_DRAIN: //chemical return drain
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av31.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av31.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.RETURN_TO_TANK_A: //chemical return tank a
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av35.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av35.ImageOptions.Image = Properties.Resources.valve_v_close; }
                        break;
                    case (int)Config_IO.enum_do.TANK_A_DRAIN: //chemical return tank a
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av45.ImageOptions.Image = Properties.Resources.valve_h_open; ; }
                        else { btn_valve_av45.ImageOptions.Image = Properties.Resources.valve_h_close; ; }
                        break;
                    case (int)Config_IO.enum_do.SUPPLY_FROM_TANK_A: //supply tank a out
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            btn_valve_av41.ImageOptions.Image = Properties.Resources.valve_v_open;
                        }
                        else
                        {
                            btn_valve_av41.ImageOptions.Image = Properties.Resources.valve_v_close;
                        }
                        break;
                    case (int)Config_IO.enum_do.SUPPLY_TO_MAIN_A: //supply tank a drain (N/O drain N/C supply)
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av43.ImageOptions.Image = Properties.Resources.valve_3w_h_down_close; }
                        else { btn_valve_av43.ImageOptions.Image = Properties.Resources.valve_3w_h_down_close2; }
                        break;
                    case (int)Config_IO.enum_do.SUPPLY_TO_MAIN_B: //supply tank b drain (N/O drain N/C supply)
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av44.ImageOptions.Image = Properties.Resources.valve_3w_h_down_close; }
                        else { btn_valve_av44.ImageOptions.Image = Properties.Resources.valve_3w_h_down_close2; }
                        break;
                    case (int)Config_IO.enum_do.SAMPLING: //sampling port
                        if (Program.IO.DO.Tag[idx].value == true) { btn_valve_av51.ImageOptions.Image = Properties.Resources.valve_v_open; }
                        else { btn_valve_av51.ImageOptions.Image = Properties.Resources.valve_v_close; }
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
                    case (int)Config_IO.enum_ai.HEATER_N2_PRESS:
                        lbl_press_drain.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " Pa";
                        break;
                    case (int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_IN_PRESS:
                        lbl_press_ps03.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_OUT_PRESS:
                        lbl_press_ps04.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS:
                        lbl_press_ps41.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.SUPPLY_A_FILTER_OUT_PRESS:
                        lbl_press_ps43.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
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
                    case (int)Config_IO.enum_ai.CHEMICAL_RETURN_A:
                        lbl_press_ps31.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.CHEMICAL_RETURN_B:
                        lbl_press_ps32.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.TANKA_PN2_FLOW:
                        lbl_flow_fi11.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.HEATER_PN2_FLOW:
                        lbl_flow_fi12.Text = string.Format("{0:f1}", Program.IO.AI.Tag[idx].value) + " LPM";
                        break;
                    case (int)Config_IO.enum_ai.MAIN_CDA2_PRESS_SOL:
                        lbl_press_ps13.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.MAIN_CDA1_PRESS_PUMP:
                        lbl_press_ps14.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                    case (int)Config_IO.enum_ai.MAIN_PN2_PRESS:
                        lbl_press_ps63.Text = string.Format("{0:f3}", Program.IO.AI.Tag[idx].value) + " Mpa";
                        break;
                }
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

            ///PIPE LINE
            ///****************************************************************************************
            ///****************************************************************************************
            ///****************************************************************************************
            ///****************************************************************************************
            ///****************************************************************************************
            ///PIPE LINE
            ///

            //CCSS

            //IPA
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
            {
                pnl_pip_ipa01.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_ipa02.BackColor = Program.schematic_form.Pipe_on_ccss;
            }

            //LS Over
            //A
            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_OVERFLOW_CHECK].value == true)
            {
                pnl_pip_over01.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_over02.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_drain02j.BackColor = Program.schematic_form.Pipe_on_ccss;
                pnl_pip_drain03.BackColor = Program.schematic_form.Pipe_on_ccss;
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
                    pnl_pip_pn_n2_01.BackColor = Program.schematic_form.Pipe_on_ccss;
                    pnl_pip_pn_n2_02.BackColor = Program.schematic_form.Pipe_on_ccss;
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

                }
                //PUMP
                if (true)
                {
                    pnl_pip_cda04.BackColor = Program.schematic_form.Pipe_on_ccss;
                }


            }

            //SUPPLY
            //av41
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true)
            {
                pnl_pip_supply01.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_sp01j.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_supply02j.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_supply02_01.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_supply02.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_supply03.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_supply04.BackColor = Program.schematic_form.Pipe_on_supply;

                if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                {
                    pnl_pip_supply05.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply06.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply06j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply07.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply11.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply12.BackColor = Program.schematic_form.Pipe_on;

                }
            }

            //Drain
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_A_DRAIN].value == true)
            {
                pnl_pip_supply01.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_sp01j.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_supply02j.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_supply02_01.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_drain01.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_drain02.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_drain02j.BackColor = Program.schematic_form.Pipe_on_supply;
                pnl_pip_drain03.BackColor = Program.schematic_form.Pipe_on_supply;
            }
            //Return

            if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true &&
                (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true ||
                (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true)))
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true)
                {
                    pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return05.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return06.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return06j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return07.BackColor = Program.schematic_form.Pipe_on;          
                    pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return08j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return11.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return12.BackColor = Program.schematic_form.Pipe_on;
                }

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true)
                {
                    pnl_pip_return01.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return02.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return03.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return04.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return06.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return06j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return08j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return11.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return12.BackColor = Program.schematic_form.Pipe_on;
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true)
                {
                    pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return05.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return06.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return06j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return07.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return08j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return09.BackColor = Program.schematic_form.Pipe_on;
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true)
                {
                    pnl_pip_return01.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return02.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return02j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return03.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return04.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return04j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return06.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return06j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return08.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return08j.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_return09.BackColor = Program.schematic_form.Pipe_on;
                }
            }


            //Valve

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true)
            {
                pnl_pip_return10.BackColor = Program.schematic_form.Pipe_on;
            }
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true)
            {
                pnl_pip_return13.BackColor = Program.schematic_form.Pipe_on;
            }
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
            {
                pnl_pip_ipa03.BackColor = Program.schematic_form.Pipe_on_ccss;
            }
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true)
            {
                pnl_pip_supply01.BackColor = Program.schematic_form.Pipe_on;
                pnl_pip_supply02j.BackColor = Program.schematic_form.Pipe_on;
                pnl_pip_supply02.BackColor = Program.schematic_form.Pipe_on;
            }

            if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state  == true)
            {

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true)
                {
                    pnl_pip_supply08.BackColor = Program.schematic_form.Pipe_on;
                }
                else
                {
                    pnl_pip_supply09.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply10.BackColor = Program.schematic_form.Pipe_on;
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true)
                {
                    pnl_pip_supply13.BackColor = Program.schematic_form.Pipe_on;
                }
                else
                {
                    pnl_pip_supply14.BackColor = Program.schematic_form.Pipe_on;
                    pnl_pip_supply15.BackColor = Program.schematic_form.Pipe_on;
                }

            }

            
            //samping
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SAMPLING].value == true)
            {
                pnl_pip_supply01.BackColor = Program.schematic_form.Pipe_on;
                pnl_pip_sp01j.BackColor = Program.schematic_form.Pipe_on;
                pnl_pip_sp01.BackColor = Program.schematic_form.Pipe_on;
                pnl_pip_sp02.BackColor = Program.schematic_form.Pipe_on;
                pnl_pip_sp03.BackColor = Program.schematic_form.Pipe_on;
            }
        }

        private void chk_use_auto_drain_CheckedChanged(object sender, EventArgs e)
        {

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

                    case "btn_valve_av03": //IPA tank a
                        idx = (int)Config_IO.enum_do.IPA_SUPPLY_TANK;
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
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.schematic_form.CCSS_Valve_Use_Check_By_Mixing_Rate(enum_ccss.CCSS1) == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve(Check Mixing Rate)", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.cg_app_info.ipa_ccss_ready_use == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.IPA_CCSS_Ready_Signal].value == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Check IPA CCSS Ready Signal", frm_messagebox.enum_apptype.Only_OK);
                            }
                            else if (Program.schematic_form.CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1) == false)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve", frm_messagebox.enum_apptype.Only_OK);
                            }

                            //if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == false)
                            //{
                            //    Program.schematic_form.CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1);
                            //}
                        }
                        break;

                    case "btn_valve_av31": //chemical return drain
                        idx = (int)Config_IO.enum_do.MAIN_RETURN_DRAIN;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {

                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == false && Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                            {
                                Program.schematic_form.SUPPLY_A_PUMP_ON_OFF(false);
                                Program.schematic_form.SUPPLY_B_PUMP_ON_OFF(false);
                            }
                            Program.ethercat_md.DO_Write_Alone(idx, false);
                        }
                        else { Program.ethercat_md.DO_Write_Alone(idx, true); }
                        break;

                    case "btn_valve_av35": //chemical return tank a
                        idx = (int)Config_IO.enum_do.RETURN_TO_TANK_A;
                        if (Program.IO.DO.Tag[idx].value == true)
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == false && Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                            {
                                Program.schematic_form.SUPPLY_A_PUMP_ON_OFF(false);
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
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_A_DRAIN].value == true)
                            {
                                Program.main_md.Message_By_Application("Cannot Open Valve, Already Used Drain Valve Opened", frm_messagebox.enum_apptype.Only_OK);
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
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true)
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
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true)
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
                            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa &&
                                Program.IO.DI.Tag[(int)Config_IO.enum_di.C_DOOR_ALARM].value == false && Program.IO.DI.Tag[(int)Config_IO.enum_di.E_RIGHT_DOOR].value == false)
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

                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".av_click." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);

            }
        }

        private void btn_heater_he41_Click(object sender, EventArgs e)
        {
            int idx;
            string result;
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
                    case "btn_heater_he41": // supply a heater
                        idx = (int)Config_IO.enum_do.SUPPLY_A_HEATER_PWR_ON;
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_A_HEATER_PWR_ON].value == true)
                        {
                            Program.schematic_form.SUPPLY_A_HEATER_ON_OFF(false);
                        }
                        else
                        {
                            result = Program.schematic_form.SUPPLY_A_HEATER_ON_OFF(true);
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
                Program.log_md.LogWrite(this.Name + ".btn_heater_he41_Click." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }

        private void btn_pump_bp41_Click(object sender, EventArgs e)
        {
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

                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".btn_pump_bp41_Click." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real = Convert.ToInt32(textBox1.Text);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (Program.schematic_form.EMPTY_FORCE_OFF == true)
            {
                Program.schematic_form.EMPTY_FORCE_OFF = false;
                Program.schematic_form.SUPPLY_A_HEATER_SET();
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SCR1_RUN, true);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_A_HEATER_PWR_ON, true);
                Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.supply_a, true);
            }
            else
            {
                Program.schematic_form.EMPTY_FORCE_OFF = true;
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SCR1_RUN, false);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_A_HEATER_PWR_ON, false);
                Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.supply_a, false);
            }
        }

        private void lbl_temp_heater_he41_Click(object sender, EventArgs e)
        {
            if (view_heater_interlock_value == false)
            {
                view_heater_interlock_value = true;
                chk_mv_view.Visible = true;
                chk_offset_view.Visible = true;
            }
            else
            {
                view_heater_interlock_value = false;
                chk_mv_view.Visible = false;
                chk_offset_view.Visible = false;
            }
        }
    }
}
