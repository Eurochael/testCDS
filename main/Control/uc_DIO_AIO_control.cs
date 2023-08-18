using System;
using System.Drawing;
using System.Windows.Forms;

namespace cds
{
    public partial class uc_DIO_AIO_control : UserControl
    {
        public uc_DIO_AIO_control()
        {
            InitializeComponent();
            
            
        }
        public CheckBox[] chk_di;
        public CheckBox[] chk_do;

        private void btn_setdio_Click(object sender, EventArgs e)
        {
            Set_DIO_Setting();
        }
        public void Set_DIO_Setting()
        {


            fpnl_di.Controls.Clear();
            int idx = 0;
            int read_use_cnt = 0;
            //chk_di = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_di)).Length];
            chk_di = new CheckBox[Program.IO.DI.use_cnt];
            foreach (var temp in Enum.GetValues(typeof(Config_IO.enum_di)))
            {
                chk_di[idx] = new CheckBox();
                chk_di[idx].Tag = idx;
                if (Program.IO.DI.Tag[idx].use == true)
                {
                    //idx = Program.IO.DI.Tag[(int)temp].address;
                    //chk_di[idx].Tag = Program.IO.DI.Tag[(int)temp].address;
                    chk_di[idx].AutoSize = false;
                    chk_di[idx].Size = new Size(200, 20);
                    chk_di[idx].Text = temp.ToString();
                    chk_di[idx].CheckedChanged -= checkBox_CheckedChanged_DI; chk_di[idx].CheckedChanged += checkBox_CheckedChanged_DI;
                    fpnl_di.Controls.Add(chk_di[(idx)]);
                    read_use_cnt = read_use_cnt + 1;
                }
                idx = idx + 1;
            }
            groupControl1.Text = "DI (" + read_use_cnt + ")";
            idx = 0;
            read_use_cnt = 0;
            fpnl_do.Controls.Clear();
            //chk_do = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_do)).Length];
            chk_do = new CheckBox[Program.IO.DO.use_cnt];
            foreach (var temp in Enum.GetValues(typeof(Config_IO.enum_do)))
            {
                chk_do[idx] = new CheckBox();
                chk_do[idx].Tag = idx;//Program.IO.DO.Tag[(int)temp].address;
                if (Program.IO.DO.Tag[idx].use == true)
                {
                    //idx = Program.IO.DO.Tag[(int)temp].address;
                    chk_do[idx].AutoSize = false;
                    chk_do[idx].Size = new Size(200, 20);
                    chk_do[idx].Text = temp.ToString();
                    chk_do[idx].CheckedChanged -= checkBox_CheckedChanged_DO; chk_do[idx].CheckedChanged += checkBox_CheckedChanged_DO;
                    fpnl_do.Controls.Add(chk_do[idx]);
                    read_use_cnt = read_use_cnt + 1;
                }
                idx = idx + 1;
            }
            groupControl2.Text = "DO (" + read_use_cnt + ")";

            if (Program.cg_app_info.mode_simulation.use == true)
            {
                timer_uichange.Interval = 500;
                timer_uichange.Enabled = true;

                cmb_seq_move.Items.Clear();
                cmb_seq_move.DataSource = Enum.GetValues(typeof(tank_class.enum_seq_no));

                cmb_seq_supply_move.Items.Clear();
                cmb_seq_supply_move.DataSource = Enum.GetValues(typeof(tank_class.enum_seq_no_supply));
            }

            //N.C Sensor ON
            chk_tank_a_hh.Checked = true;
            chk_tank_b_hh.Checked = true;

            Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_HH].value = false;
            Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_HH].value = false;
            Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_OVERFLOW_CHECK].value = false;
            Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_OVERFLOW_CHECK].value = false;

        }
        private void checkBox_CheckedChanged_DI(object sender, System.EventArgs e)
        {
            int idx = 0;
            var checkbox = (CheckBox)sender;
            idx = (int)checkbox.Tag; //address
            Program.IO.DI.Tag[idx].value = checkbox.Checked;

        }
        private void checkBox_CheckedChanged_DO(object sender, System.EventArgs e)
        {
            int idx = 0;
            var checkbox = (CheckBox)sender;
            idx = (int)checkbox.Tag;
            Program.IO.DO.Tag[idx].value = checkbox.Checked;
        }
        private void timer_uichange_Tick(object sender, EventArgs e)
        {
            //CC Need State
            if (Program.seq.supply.c_c_need == true)
            {
                lbl_cc_need_state.BackColor = Color.Red;
                lbl_cc_need_state.Text = "CC Need : " + Program.seq.supply.c_c_need_text;
            }
            else
            {
                lbl_cc_need_state.BackColor = Color.Gray;
                lbl_cc_need_state.Text = "CC Not Need : ";
            }
            if (Program.seq.supply.req_c_c_start_cds_to_ctc == true)
            {
                lbl_need_ctc_resp_ok.BackColor = Color.Red;
                lbl_need_ctc_resp_ok.Text = "CTC Response Need";
            }
            else
            {
                lbl_need_ctc_resp_ok.BackColor = Color.Gray;
                lbl_need_ctc_resp_ok.Text = "CTC Response Not Need";
            }

            if (Program.seq.supply.ctc_supply_request == true)
            {
                lbl_ctc_supply_request.BackColor = Color.Green;
            }
            else
            {
                lbl_ctc_supply_request.BackColor = Color.Gray;
            }

            if (Program.cg_app_info.mode_simulation.use == false) { return; }

            this.SuspendLayout();
            Random rnd_tmp = new Random();
            //return;
            timer_uichange.Interval = (int)numeric_simulation_interval.Value;

            if (chk_di != null)
            {
                for (int idx = 0; idx < chk_di.Length; idx++)
                {
                    if (chk_di[idx] != null)
                    {
                        if (chk_di[idx] != null)
                        {

                            if (Program.IO.DI.Tag[idx].value == true)
                            {
                                chk_di[idx].ForeColor = Color.Green;
                            }
                            else
                            {
                                chk_di[idx].ForeColor = Color.Gray;
                            }
                        }

                    }


                }
            }

            if (chk_do != null)
            {
                for (int idx = 0; idx < chk_do.Length; idx++)
                {
                    if (chk_do[idx] != null)
                    {
                        if (chk_do[idx] != null)
                        {
                            if (Program.IO.DO.Tag[idx].value == true)
                            {
                                chk_do[idx].ForeColor = Color.Green;
                            }
                            else
                            {
                                chk_do[idx].ForeColor = Color.Gray;
                            }
                        }
                    }


                }
            }


            if (chk_autorun.Checked == true)
            {
                //Simulation Value Auto Refresh

                if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    //CCSS1
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow = Convert.ToDouble(numeric_rising_ccss1.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_IN_PRESS].value = 0.13;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_OUT_PRESS].value = 0.14;
                        Program.main_form.SerialData.FlowMeter_Sonotec.IPA_totalusage += Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_IN_PRESS].value = 0;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.IPA_SUPPLY_FILTER_OUT_PRESS].value = 0;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    //CCSS1
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow = Convert.ToDouble(numeric_rising_ccss1.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_IN_PRESS].value = 0.13;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_OUT_PRESS].value = 0.14;
                        Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage += Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_IN_PRESS].value = 0;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_OUT_PRESS].value = 0;
                    }
                    //CCSS2
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_USF500.NH4OH_flow = Convert.ToDouble(numeric_rising_ccss2.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.NH4OH_SUPPLY_FILTER_IN_PRESS].value = 0.13;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.NH4OH_SUPPLY_FILTER_OUT_PRESS].value = 0.14;
                        Program.main_form.SerialData.FlowMeter_USF500.NH4OH_totalusage += Program.main_form.SerialData.FlowMeter_USF500.NH4OH_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.NH4OH_SUPPLY_FILTER_IN_PRESS].value = 0;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.NH4OH_SUPPLY_FILTER_OUT_PRESS].value = 0;
                    }
                    //CCSS4
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HOT_DIW_BY_PASS].value == false)
                        {
                            Program.main_form.SerialData.FlowMeter_USF500.DIW_flow = Convert.ToDouble(numeric_rising_ccss4.Value);
                            Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value = 0.13;
                            Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage += Program.main_form.SerialData.FlowMeter_USF500.DIW_flow;
                        }
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value = 0;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    //CCSS1
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_Sonotec.DSP_flow = Convert.ToDouble(numeric_rising_ccss1.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DSP_SUPPLY_FILTER_IN_PRESS].value = 0.13;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DSP_SUPPLY_FILTER_OUT_PRESS].value = 0.14;
                        Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage += Program.main_form.SerialData.FlowMeter_Sonotec.DSP_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DSP_SUPPLY_FILTER_IN_PRESS].value = 0;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DSP_SUPPLY_FILTER_OUT_PRESS].value = 0;
                    }

                    //CCSS4
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow = Convert.ToDouble(numeric_rising_ccss4.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value = 0.13;
                        Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage += Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value = 0;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    //CCSS1
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_USF500.HF_flow = Convert.ToDouble(numeric_rising_ccss1.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS].value = 0.13;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS].value = 0.14;
                        Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage += Program.main_form.SerialData.FlowMeter_USF500.HF_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS].value = 0;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS].value = 0;
                    }
                    //CCSS4
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_USF500.DIW_flow = Convert.ToDouble(numeric_rising_ccss4.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value = 0.13;
                        Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage += Program.main_form.SerialData.FlowMeter_USF500.DIW_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value = 0;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    //CCSS1
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_Sonotec.LAL_flow = Convert.ToDouble(numeric_rising_ccss1.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.LAL_SUPPLY_FILTER_IN_PRESS].value = 0.13;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.LAL_SUPPLY_FILTER_OUT_PRESS].value = 0.14;
                        Program.main_form.SerialData.FlowMeter_Sonotec.LAL_totalusage += Program.main_form.SerialData.FlowMeter_Sonotec.LAL_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.LAL_SUPPLY_FILTER_IN_PRESS].value = 0;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.LAL_SUPPLY_FILTER_OUT_PRESS].value = 0;
                    }

                    //CCSS4
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow = Convert.ToDouble(numeric_rising_ccss4.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value = 0.13;
                        Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage += Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value = 0;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    //CCSS1
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow = Convert.ToDouble(numeric_rising_ccss1.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_IN_PRESS].value = 0.13;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_OUT_PRESS].value = 0.14;
                        Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage += Program.main_form.SerialData.FlowMeter_USF500.H2O2_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_IN_PRESS].value = 0;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2O2_SUPPLY_FILTER_OUT_PRESS].value = 0;
                    }
                    //CCSS2
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_USF500.H2SO4_flow = Convert.ToDouble(numeric_rising_ccss2.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2SO4_SUPPLY_FILTER_IN_PRESS].value = 0.13;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2SO4_SUPPLY_FILTER_OUT_PRESS].value = 0.14;
                        Program.main_form.SerialData.FlowMeter_USF500.H2SO4_totalusage += Program.main_form.SerialData.FlowMeter_USF500.H2SO4_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2SO4_SUPPLY_FILTER_IN_PRESS].value = 0;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.H2SO4_SUPPLY_FILTER_OUT_PRESS].value = 0;
                    }
                    //CCSS3
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_USF500.HF_flow = Convert.ToDouble(numeric_rising_ccss3.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS].value = 0.13;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS].value = 0.14;
                        Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage += Program.main_form.SerialData.FlowMeter_USF500.HF_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_IN_PRESS].value = 0;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.HF_SUPPLY_FILTER_OUT_PRESS].value = 0;
                    }
                    //CCSS4
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.FlowMeter_USF500.DIW_flow = Convert.ToDouble(numeric_rising_ccss4.Value);
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value = 0.13;
                        Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage += Program.main_form.SerialData.FlowMeter_USF500.DIW_flow;
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == false)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_PRESS].value = 0;
                    }
                }
                Level_Auto_Refresh_A();
                Level_Auto_Refresh_B();

                //HEATER CIR 1
                if (Program.Heat_Exchanger.run_state == true)
                {
                    if (Program.Heat_Exchanger.heater_on == true)
                    {
                        if (Program.Heat_Exchanger.pv_temp >
                            Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Set))
                        {
                            Program.Heat_Exchanger.pv_temp -= 1;
                        }
                    }
                    else
                    {
                        Program.Heat_Exchanger.pv_temp = 50;
                    }
                }

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIRCULATION1_HEATER_PWR_ON].value == true || Program.main_form.SerialData.Circulation_Thermostat.heater_on == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.CIRCULATION_THERMOSTAT_PWR_ON].value == true)
                {
                    if (Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv <
                        Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Set))
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv += 1;
                    }
                }
                else
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv = 5;
                }
                //HEATER CIR 2
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIRCULATION2_HEATER_PWR_ON].value == true)
                {
                    if (Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.pv <
                        Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Set))
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.pv += 1;
                    }
                }
                else
                {

                }
                //HEATER SUPPLY A
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_A_HEATER_PWR_ON].value == true || Program.main_form.SerialData.Supply_A_Thermostat.heater_on == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_A_THERMOSTAT_PWR_ON].value == true)
                {
                    if (Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv <
                        Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Set))
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv += 0.5;
                    }
                }
                else
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv = 15;
                }

                //HEATER SUPPLY B
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_B_HEATER_PWR_ON].value == true || Program.main_form.SerialData.Supply_B_Thermostat.heater_on == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_B_THERMOSTAT_PWR_ON].value == true)
                {
                    if (Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv <
                        Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_Set))
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv += 0.5;
                    }
                }
                else
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv = 15;
                }

                if (Program.Heat_Exchanger.heater_on == true)
                {
                    //HEAT Exchange가 모도 온도가 충족되면, Tank 온도 변경
                    if ((Program.Heat_Exchanger.pv_temp <=
                            Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Set))
                            && (Program.Heat_Exchanger.pv_temp <=
                            Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Set)))
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Set);
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Set);
                    }
                    else
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = Program.Heat_Exchanger.pv_temp;
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = Program.Heat_Exchanger.pv_temp;
                    }
                }
                else
                {
                    //HEATER CIR1 CIR2가 모도 온도가 충족되면, Tank 온도 변경
                    if ((Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv >=
                            Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Set))
                            && (Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater2.pv >=
                            Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Set)))
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Set);
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Set);
                    }
                    else
                    {
                        if (chk_temp_set_tank_a.Checked == true)
                        {
                            Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = (int)numeric_tank_temp_a.Value;
                        }
                        else
                        {
                            Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = 15;
                        }
                        if (chk_temp_set_tank_b.Checked == true)
                        {
                            Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = (int)numeric_tank_temp_b.Value;
                        }
                        else
                        {
                            Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = 15;
                        }
                    }
                }

                Program.main_form.SerialData.TEMP_CONTROLLER.supply_a.pv = Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.pv;
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_b.pv = Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.pv;
                Program.main_form.SerialData.TEMP_CONTROLLER.circulation.pv = Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.pv;

                //Pump Control

                if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value = 11.21;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value = 0.21;
                }
                else
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value = 0;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value = 0;
                }

                //IPA경우 Pump A만 존재하며, Supply A, B 둘 다 동시 공급
                if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value = 11.41;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS].value = 0.413;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_OUT_PRESS].value = 0.414;

                    if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value = 11.42;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_OUT_PRESS].value = 0.412;
                    }
                }
                else
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value = 0;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS].value = 0;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_OUT_PRESS].value = 0;
                    if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value = 0;
                        Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_OUT_PRESS].value = 0;
                    }
                }

                if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value = 11.42;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS].value = 0.423;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_OUT_PRESS].value = 0.424;
                }
                else
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value = 0;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS].value = 0;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_OUT_PRESS].value = 0;
                }

                //DRAIN PUMP
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.Drain_Pump_On].value == true)
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.DRAIN_FLOW].value = 11.61;
                }
                else
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.DRAIN_FLOW].value = 0;
                }


                //RETURN
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true)
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_A].value = 0.31;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_B].value = 0.32;
                }
                else
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_A].value = 0;
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.CHEMICAL_RETURN_B].value = 0;
                }

                //공통
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_PN2_PRESS].value = 0.61;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.TANKA_PN2_FLOW].value = 0.62;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.TANKB_PN2_FLOW].value = 0.63;

                Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_THERMOSTAT_PCW_FLOW].value = 0.13;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_THERMOSTAT_PCW_FLOW].value = 0.11;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_THERMOSTAT_PCW_FLOW].value = 0.12;

                Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA2_PRESS_SOL].value = 0.71;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA1_PRESS_PUMP].value = 0.72;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.MAIN_CDA3_PRESS_DRAIN].value = 0.73;

                Program.IO.AI.Tag[(int)Config_IO.enum_ai.CM_DIW_FLOW].value = 0.101;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.CM_DIW_PRESS].value = 0.101;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.CM_PUMP_PRESS].value = 0.104;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.CM_SAMPLING_FLOW].value = 0.102;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.CM_SAMPLING_PRESS].value = 0.102;
                Program.IO.AI.Tag[(int)Config_IO.enum_ai.CM_SOL_PRESS].value = 0.103;
                //DSP Mix

                Program.Heat_Exchanger.chemical_in = 50;
                Program.Heat_Exchanger.chemical_out = 50;
                Program.Heat_Exchanger.filter_in_press = 0.1;
                Program.Heat_Exchanger.filter_out_press = 0.2;
                Program.Heat_Exchanger.chemical_in_press = 0.3;



                //Circulation Pump Press


                //DRAIN PUMP
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value = 0.10;
                }
                else
                {
                    Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value = 0;
                }


                if (numeric_rising_wafercnt_tank_a.Value != 0)
                {
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY)
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt += (int)numeric_rising_wafercnt_tank_a.Value;
                    }
                    else
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt = 0;
                    }
                }
                if (numeric_rising_wafercnt_tank_b.Value != 0)
                {
                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY)
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt += (int)numeric_rising_wafercnt_tank_b.Value;
                    }
                    else
                    {
                        Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt = 0;
                    }
                }

                Concentration_Value_Change();

                this.ResumeLayout();
            }
        }
        public void Concentration_Value_Change()
        {
            Random rnd_temp = new Random();
            //APM
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (chk_concentration_1.Checked == true)
                {
                    Program.ABB.concentration_1 = (float)(numeric_concen_value_1.Value);
                    Program.ABB.concentration_1 = Program.ABB.concentration_1 + (float)(rnd_temp.Next(0, 9) * 0.01);
                }
                if (chk_concentration_2.Checked == true)
                {
                    Program.ABB.concentration_2 = (float)(numeric_concen_value_2.Value);
                    Program.ABB.concentration_2 = Program.ABB.concentration_2 + (float)(rnd_temp.Next(0, 9) * 0.01);
                }
            }
            //DSP
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                if (chk_concentration_1.Checked == true)
                {
                    Program.ABB.concentration_1 = (float)(numeric_concen_value_1.Value);
                    Program.ABB.concentration_1 = Program.ABB.concentration_1 + (float)(rnd_temp.Next(0, 9) * 0.01);
                }
            }
            //DHF
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                if (chk_concentration_1.Checked == true)
                {
                    Program.main_form.SerialData.CM210DC.Concentration = (float)(numeric_concen_value_1.Value);
                    Program.main_form.SerialData.CM210DC.Concentration = Program.main_form.SerialData.CM210DC.Concentration + (float)(rnd_temp.Next(0, 9) * 0.01);
                }
            }
            //LAL
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                if (chk_concentration_1.Checked == true)
                {
                    Program.main_form.SerialData.CS600F.concentration = (float)(numeric_concen_value_1.Value);
                    Program.main_form.SerialData.CS600F.concentration = Program.main_form.SerialData.CS600F.concentration + (float)(rnd_temp.Next(0, 9) * 0.01);
                }
            }
            //DSP MIX
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (chk_concentration_1.Checked == true)
                {
                    Program.main_form.SerialData.CS150C.h2o2_concentration = (float)(numeric_concen_value_1.Value);
                    Program.main_form.SerialData.CS150C.h2o2_concentration = Program.main_form.SerialData.CS150C.h2o2_concentration + (float)(rnd_temp.Next(0, 9) * 0.01);
                }
                if (chk_concentration_2.Checked == true)
                {
                    Program.main_form.SerialData.CS150C.h2so4_concentration = (float)(numeric_concen_value_2.Value);
                    Program.main_form.SerialData.CS150C.h2so4_concentration = Program.main_form.SerialData.CS150C.h2so4_concentration + (float)(rnd_temp.Next(0, 9) * 0.01);
                }
                if (chk_concentration_3.Checked == true)
                {
                    Program.main_form.SerialData.HF700.concentration = (float)(numeric_concen_value_3.Value);
                    Program.main_form.SerialData.HF700.concentration = Program.main_form.SerialData.HF700.concentration + (float)(rnd_temp.Next(0, 9) * 0.01);
                }
            }
        }
        public void Level_Auto_Refresh_A()
        {
            double ccss1_totalusage = Program.schematic_form.TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1);
            double ccss2_totalusage = Program.schematic_form.TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS2);
            double ccss3_totalusage = Program.schematic_form.TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS3);
            double ccss4_totalusage = Program.schematic_form.TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4);
            double tank_total_real = 0;

            ///CCSS1
            if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }
            ///CCSS2
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real;
                }
            }

            ///CCSS3
            if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS3].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS3].input_volmue_real;
                }
            }


            if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.dsp || Program.cg_app_info.eq_type == enum_eq_type.dhf || Program.cg_app_info.eq_type == enum_eq_type.lal || Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS4].input_volmue_real + ccss4_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS4].input_volmue_real;
                }
            }

            if (tank_total_real >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_H))
            {
                lbl_lv_tanka_h.BackColor = Color.Green;
                lbl_lv_tanka_m.BackColor = Color.Green;
                lbl_lv_tanka_l.BackColor = Color.Green;
                lbl_lv_tanka_ll.BackColor = Color.Green;

                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value = true;
            }
            else if (tank_total_real >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_M))
            {
                lbl_lv_tanka_h.BackColor = Color.White;
                lbl_lv_tanka_m.BackColor = Color.Green;
                lbl_lv_tanka_l.BackColor = Color.Green;
                lbl_lv_tanka_ll.BackColor = Color.Green;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value = true;
            }
            else if (tank_total_real >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_L))
            {
                lbl_lv_tanka_h.BackColor = Color.White;
                lbl_lv_tanka_m.BackColor = Color.White;
                lbl_lv_tanka_l.BackColor = Color.Green;
                lbl_lv_tanka_ll.BackColor = Color.Green;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value = true;
            }
            else if (tank_total_real >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_LL))
            {
                lbl_lv_tanka_h.BackColor = Color.White;
                lbl_lv_tanka_m.BackColor = Color.White;
                lbl_lv_tanka_l.BackColor = Color.White;
                lbl_lv_tanka_ll.BackColor = Color.Green;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value = true;
            }
            else
            {
                lbl_lv_tanka_h.BackColor = Color.White;
                lbl_lv_tanka_m.BackColor = Color.White;
                lbl_lv_tanka_l.BackColor = Color.White;
                lbl_lv_tanka_ll.BackColor = Color.White;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value = false;
            }

        }
        public void Level_Auto_Refresh_B()
        {
            double ccss1_totalusage = Program.schematic_form.TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1);
            double ccss2_totalusage = Program.schematic_form.TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS2);
            double ccss3_totalusage = Program.schematic_form.TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS3);
            double ccss4_totalusage = Program.schematic_form.TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4);
            double tank_total_real = 0;


            //CCSS1
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real + ccss1_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real;
                }
            }

            //CCSS2
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real + ccss2_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real + ccss2_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real;
                }
            }


            //CCSS3
            if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS3].input_volmue_real + ccss3_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS3].input_volmue_real;
                }
            }

            if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.dsp || Program.cg_app_info.eq_type == enum_eq_type.dhf || Program.cg_app_info.eq_type == enum_eq_type.lal || Program.cg_app_info.eq_type == enum_eq_type.dsp_mix) 
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS4].input_volmue_real + ccss4_totalusage;
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == false)
                {
                    tank_total_real = tank_total_real + Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS4].input_volmue_real;
                }
            }
            if (tank_total_real >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_B_Level_H))
            {
                lbl_lv_tankb_h.BackColor = Color.Green;
                lbl_lv_tankb_m.BackColor = Color.Green;
                lbl_lv_tankb_l.BackColor = Color.Green;
                lbl_lv_tankb_ll.BackColor = Color.Green;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value = true;
            }
            else if (tank_total_real >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_B_Level_M))
            {
                lbl_lv_tankb_h.BackColor = Color.White;
                lbl_lv_tankb_m.BackColor = Color.Green;
                lbl_lv_tankb_l.BackColor = Color.Green;
                lbl_lv_tankb_ll.BackColor = Color.Green;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value = true;
            }
            else if (tank_total_real >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_B_Level_L))
            {
                lbl_lv_tankb_h.BackColor = Color.White;
                lbl_lv_tankb_m.BackColor = Color.White;
                lbl_lv_tankb_l.BackColor = Color.Green;
                lbl_lv_tankb_ll.BackColor = Color.Green;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value = true;
            }
            else if (tank_total_real >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_B_Level_LL))
            {
                lbl_lv_tankb_h.BackColor = Color.White;
                lbl_lv_tankb_m.BackColor = Color.White;
                lbl_lv_tankb_l.BackColor = Color.White;
                lbl_lv_tankb_ll.BackColor = Color.Green;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value = true;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value = true;
            }
            else
            {
                lbl_lv_tankb_h.BackColor = Color.White;
                lbl_lv_tankb_m.BackColor = Color.White;
                lbl_lv_tankb_l.BackColor = Color.White;
                lbl_lv_tankb_ll.BackColor = Color.White;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value = false;
                Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value = false;

            }
        }
        private void chk_mixingtype_mixing_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox checkbox = sender as System.Windows.Forms.CheckBox;


            int idx_address = 0;
            if (chk_mixingtype_mixing.Checked == true)
            {
                Program.cg_mixing_step.mixing_use = true;
            }
            else
            {
                Program.cg_mixing_step.mixing_use = false;
            }

            if (chk_mixingtype_refiluse.Checked == true)
            {
                Program.cg_mixing_step.refill_use = true;
            }
            else
            {
                Program.cg_mixing_step.refill_use = false;
            }


            if (chk_autorun.Checked == true)
            {
                Program.cg_app_info.mode_simulation.use_seq_auto_run = true;
            }
            else
            {
                Program.cg_app_info.mode_simulation.use_seq_auto_run = false;
            }

        }
        private void btn_totalusage_set_Click(object sender, EventArgs e)
        {
            Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = (int)numeric_totalusage_ccss4.Value;
            Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage = (int)numeric_totalusage_ccss4.Value;
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage = (int)numeric_totalusage_ccss1.Value;
                Program.main_form.SerialData.FlowMeter_USF500.NH4OH_totalusage = (int)numeric_totalusage_ccss2.Value;
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                Program.main_form.SerialData.FlowMeter_Sonotec.IPA_totalusage = (int)numeric_totalusage_ccss1.Value;
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                Program.main_form.SerialData.FlowMeter_Sonotec.LAL_totalusage = (int)numeric_totalusage_ccss1.Value;
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage = (int)numeric_totalusage_ccss1.Value;
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage = (int)numeric_totalusage_ccss1.Value;
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage = (int)numeric_totalusage_ccss1.Value;
                Program.main_form.SerialData.FlowMeter_USF500.H2SO4_totalusage = (int)numeric_totalusage_ccss2.Value;
                Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage = (int)numeric_totalusage_ccss3.Value;
            }
        }
        private void btn_initial_Click(object sender, EventArgs e)
        {

        }
        private void btn_tank_temp_set_a_Click(object sender, EventArgs e)
        {
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
            }
            Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv = (int)numeric_tank_temp_a.Value;

        }
        private void btn_tank_temp_set_b_Click(object sender, EventArgs e)
        {
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
            }
            Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv = (int)numeric_tank_temp_b.Value;

        }

        private void btn_totalusage_clear_Click(object sender, EventArgs e)
        {
            numeric_totalusage_ccss1.Value = 0;
            numeric_totalusage_ccss2.Value = 0;
            numeric_totalusage_ccss3.Value = 0;
            numeric_totalusage_ccss4.Value = 0;
        }

        private void btn_tank_wafercount_set_a_Click(object sender, EventArgs e)
        {
            Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt = (int)numeric_tank_wafer_a.Value;
        }

        private void btn_tank_wafercount_set_b_Click(object sender, EventArgs e)
        {
            Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt = (int)numeric_tank_wafer_b.Value;
        }

        private void btn_tank_lifetime_set_a_Click(object sender, EventArgs e)
        {
            Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_process = Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_process.AddMinutes(-1 * (int)numeric_tank_lifetime_a.Value);
        }

        private void btn_tank_lifetime_set_b_Click(object sender, EventArgs e)
        {
            Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_start_process = Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_start_process.AddMinutes(-1 * (int)numeric_tank_lifetime_b.Value);
        }

        private void btn_seq_main_move_Click(object sender, EventArgs e)
        {
            tank_class.enum_seq_no status;
            Enum.TryParse<tank_class.enum_seq_no>(this.cmb_seq_move.SelectedValue.ToString(), out status);

            Program.schematic_form.Seq_Cur_To_Next(status, status, "");
        }

        private void btn_seq_supply_move_Click(object sender, EventArgs e)
        {
            tank_class.enum_seq_no_supply status;
            Enum.TryParse<tank_class.enum_seq_no_supply>(this.cmb_seq_supply_move.SelectedValue.ToString(), out status);

            Program.schematic_form.Seq_Supply_Cur_To_Next(status, status, "");
        }

        private void btn_ctc_supply_req_Click(object sender, EventArgs e)
        {
            if (Program.seq.supply.ctc_supply_request == true)
            {
                Program.seq.supply.ctc_supply_request = false;
            }
            else
            {
                Program.seq.supply.ctc_supply_request = true;
            }

        }

        private void btn_concentration_check_Click(object sender, EventArgs e)
        {
            Program.seq.supply.concentration_check_req_in_supply = true;
        }

        private void btn_ctc_cc_req_Click(object sender, EventArgs e)
        {
            Program.seq.supply.ctc_c_c_request = true;

        }

        private void btn_clear_Click(object sender, EventArgs e)
        {

        }

        private void btn_tank_a_level_sensor_off_Click(object sender, EventArgs e)
        {
            chk_tank_a_hh.Checked = true;
            rbtn_tank_a_ll.Checked = true;
            rbtn_tank_a_empty.Checked = true;
        }

        private void btn_tank_b_level_sensor_off_Click(object sender, EventArgs e)
        {
            chk_tank_b_hh.Checked = true;
            rbtn_tank_b_ll.Checked = true;
            rbtn_tank_b_empty.Checked = true;
        }

        private void btn_hdiw_temp_set_Click(object sender, EventArgs e)
        {
            Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.pv = (int)numeric_temp_set.Value;
        }

        private void btn_ctc_cc_rep_Click(object sender, EventArgs e)
        {
            //Program.seq.rep_c_c_start_cds_to_ctc = true;
            Program.seq.supply.rep_c_c_start_cds_to_ctc = true;
        }

        private void btn_totalusage_set_tank_a_Click(object sender, EventArgs e)
        {
            Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real = (int)numeric_totalusage_ccss1_tank_a.Value;
            Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real = (int)numeric_totalusage_ccss2_tank_a.Value;
            Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS3].input_volmue_real = (int)numeric_totalusage_ccss3_tank_a.Value;
            Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)enum_ccss.CCSS4].input_volmue_real = (int)numeric_totalusage_ccss4_tank_a.Value;
        }

        private void btn_totalusage_set_tank_b_Click(object sender, EventArgs e)
        {
            Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real = (int)numeric_totalusage_ccss1_tank_b.Value;
            Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real = (int)numeric_totalusage_ccss2_tank_b.Value;
            Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS3].input_volmue_real = (int)numeric_totalusage_ccss3_tank_b.Value;
            Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)enum_ccss.CCSS4].input_volmue_real = (int)numeric_totalusage_ccss4_tank_b.Value;
        }

        private void numeric_totalusage_ccss1_tank_a_ValueChanged(object sender, EventArgs e)
        {

        }


        private void chk_tank_a_hh_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKA_LEVEL_HH;
            if (rbtn_tank_a_h.Checked == true)
            {
                chk_di[idx_address].Checked = true;
            }
            else
            {
                chk_di[idx_address].Checked = false;
            }
        }

        private void chk_tank_b_hh_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKB_LEVEL_HH;
            if (rbtn_tank_a_h.Checked == true)
            {
                chk_di[idx_address].Checked = true;
            }
            else
            {
                chk_di[idx_address].Checked = false;
            }
        }
        public void Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type selected_tank, double value)
        {
            double tmp_value = 0;
            double Spare_value = 0;
            double sub_value = 0;
            Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real = 0;
            Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real = 0;
            Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS3].input_volmue_real = 0;
            Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS4].input_volmue_real = 0;
            for (int idx_level = 0; idx_level < value; idx_level++)
            {
                {
                    if (Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real < Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS1].input_volume)
                    {
                        Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS1].input_volmue_real += 1;
                    }
                    else if (Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real < Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS2].input_volume)
                    {
                        Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS2].input_volmue_real += 1;
                    }
                    else if (Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS3].input_volmue_real < Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS3].input_volume)
                    {
                        Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS3].input_volmue_real += 1;
                    }
                    else if (Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS4].input_volmue_real < Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS4].input_volume)
                    {
                        Program.tank[(int)selected_tank].ccss_data[(int)enum_ccss.CCSS4].input_volmue_real += 1;
                    }
                }
            }

        }

        private void rbtn_tank_a_h_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKA_LEVEL_H;
            if (rbtn_tank_a_h.Checked == true)
            {
                chk_di[idx_address].Checked = true;
                Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type.TANK_A, 60);
            }
            else
            {
                chk_di[idx_address].Checked = false;
                //rbtn_tank_a_m.Checked = true;
            }
        }


        private void rbtn_tank_a_m_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKA_LEVEL_M;
            if (rbtn_tank_a_m.Checked == true)
            {
                chk_di[idx_address].Checked = true;
                Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type.TANK_A, 40);
            }
            else
            {
                chk_di[idx_address].Checked = false;
                //rbtn_tank_a_l.Checked = true;
            }
        }

        private void rbtn_tank_a_l_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKA_LEVEL_L;
            if (rbtn_tank_a_l.Checked == true)
            {
                chk_di[idx_address].Checked = true;
                Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type.TANK_A, 20);
            }
            else
            {
                chk_di[idx_address].Checked = false;
                //rbtn_tank_a_ll.Checked = true;
            }
        }

        private void rbtn_tank_a_ll_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKA_LEVEL_LL;
            if (rbtn_tank_a_ll.Checked == true)
            {
                chk_di[idx_address].Checked = true;
                Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type.TANK_A, 10);
            }
            else
            {
                chk_di[idx_address].Checked = false;
                //rbtn_tank_a_empty.Checked = true;
            }
        }

        private void rbtn_tank_a_empty_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKA_EMPTY_CHECK;
            if (rbtn_tank_a_empty.Checked == true)
            {
                chk_di[idx_address].Checked = false;
                Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type.TANK_A, 0);
            }
            else
            {
                chk_di[idx_address].Checked = true;
            }
        }


        private void rbtn_tank_b_h_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKB_LEVEL_H;
            if (rbtn_tank_b_h.Checked == true)
            {
                chk_di[idx_address].Checked = true;
                Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type.TANK_B, 60);
            }
            else
            {
                chk_di[idx_address].Checked = false;
                //rbtn_tank_a_m.Checked = true;
            }
        }

        private void rbtn_tank_b_m_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKB_LEVEL_M;
            if (rbtn_tank_b_m.Checked == true)
            {
                chk_di[idx_address].Checked = true;
                Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type.TANK_B, 40);
            }
            else
            {
                chk_di[idx_address].Checked = false;
                //rbtn_tank_a_m.Checked = true;
            }
        }

        private void rbtn_tank_b_l_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKB_LEVEL_L;
            if (rbtn_tank_b_l.Checked == true)
            {
                chk_di[idx_address].Checked = true;
                Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type.TANK_B, 20);
            }
            else
            {
                chk_di[idx_address].Checked = false;
                //rbtn_tank_a_m.Checked = true;
            }
        }

        private void rbtn_tank_b_ll_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKB_LEVEL_LL;
            if (rbtn_tank_b_ll.Checked == true)
            {
                chk_di[idx_address].Checked = true;
                Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type.TANK_B, 10);
            }
            else
            {
                chk_di[idx_address].Checked = false;
                //rbtn_tank_a_m.Checked = true;
            }
        }

        private void rbtn_tank_b_e_CheckedChanged(object sender, EventArgs e)
        {
            int idx_address = (int)Config_IO.enum_di.TANKB_EMPTY_CHECK;
            if (rbtn_tank_b_empty.Checked == true)
            {
                chk_di[idx_address].Checked = false;
                Tank_Level_Adjust_By_Simulation(tank_class.enum_tank_type.TANK_B, 0);
            }
            else
            {
                chk_di[idx_address].Checked = true;
            }
        }

        private void chk_temp_set_tank_a_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
