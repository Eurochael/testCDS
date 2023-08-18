using System;
using System.Collections.Generic;

namespace cds
{
    partial class frm_schematic
    {
        public bool HDIW_Temp_Check()
        {
            bool result = false;
            //TS-09 온도 값 확인
            if (Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.pv >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.HDIW_Supply_Min_Temp)
                && Program.main_form.SerialData.TEMP_CONTROLLER.ts_09.pv <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.HDIW_Supply_Max_Temp))
            {
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HOT_DIW_BY_PASS, false);
                result = true;
            }
            else
            {
                if (Program.cg_app_info.ignore_hdiw_by_pass == true)
                {
                    //AK 내부 테스트 위함 제네러이터대용 강제 변환
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HOT_DIW_BY_PASS, false);
                    result = true;
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HOT_DIW_BY_PASS, true);
                    result = false;
                }


            }
            return result;
        }
        public double TotalUsage_Return_By_EQType_CCSS(enum_ccss ccss)
        {
            Double result = 0;
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (ccss == enum_ccss.CCSS1)
                {
                    result = Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage;
                }
                else if (ccss == enum_ccss.CCSS2)
                {
                    result = Program.main_form.SerialData.FlowMeter_USF500.NH4OH_totalusage;
                }
                else if (ccss == enum_ccss.CCSS4)
                {
                    result = Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                if (ccss == enum_ccss.CCSS1)
                {
                    result = Program.main_form.SerialData.FlowMeter_Sonotec.IPA_totalusage;
                }
                else if (ccss == enum_ccss.CCSS4)
                {
                    result = Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                if (ccss == enum_ccss.CCSS1)
                {
                    //Sonotec DSP의 경우 Software 적산 필요
                    result = Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage;
                }
                else if (ccss == enum_ccss.CCSS4)
                {
                    result = Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                if (ccss == enum_ccss.CCSS1)
                {
                    result = Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage;
                }
                else if (ccss == enum_ccss.CCSS4)
                {
                    result = Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                if (ccss == enum_ccss.CCSS1)
                {
                    //Sonotec LAL의 경우 Software 적산 필요
                    result = Program.main_form.SerialData.FlowMeter_Sonotec.LAL_totalusage;
                }
                else if (ccss == enum_ccss.CCSS4)
                {
                    result = Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (ccss == enum_ccss.CCSS1)
                {
                    result = Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage;
                }
                else if (ccss == enum_ccss.CCSS2)
                {
                    result = Program.main_form.SerialData.FlowMeter_USF500.H2SO4_totalusage;
                }
                else if (ccss == enum_ccss.CCSS3)
                {
                    result = Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage;
                }
                else if (ccss == enum_ccss.CCSS4)
                {
                    result = Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage;
                }
            }
            return result;
        }
        public void CCSS_Valve_Control_By_EQType_CCSS(enum_ccss ccss, tank_class.enum_tank_type call_selected_tank, bool state, bool totalusage_save)
        {
            if (ccss == enum_ccss.CCSS1)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_USF500.TotalUsage_Reset(enum_chemical.H2O2); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1);
                        }
                    }

                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B, state);
                    }

                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    if (Program.cg_app_info.ipa_ccss_ready_use == true)
                    {
                        if (state == true)
                        {
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.IPA_CCSS_Ready_Signal].value == true)
                            {
                                Program.FlowMeter_SONOTEC.TotalUsage_Reset(enum_chemical.IPA);
                                if(call_selected_tank == tank_class.enum_tank_type.TANK_A)
                                {
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.IPA_SUPPLY_TANK, state);
                                }
                            }
                        }
                        else
                        {
                            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.IPA_CCSS_Not_Ready_Signal, "", true, false);
                        }
                    }
                    else
                    {
                        //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                        if (state == true) { Program.FlowMeter_SONOTEC.TotalUsage_Reset(enum_chemical.IPA); }
                        else
                        {
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                            {
                                Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1));
                                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1);
                            }
                        }
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.IPA_SUPPLY_TANK, state);
                        }
                    }


                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_SONOTEC.TotalUsage_Reset(enum_chemical.DSP); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1);
                        }
                    }

                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DSP_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DSP_SUPPLY_TANK_B, state);
                    }

                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_USF500.TotalUsage_Reset(enum_chemical.HF); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1);
                        }
                    }

                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_B, state);
                    }

                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_SONOTEC.TotalUsage_Reset(enum_chemical.LAL); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1);
                        }
                    }

                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.LAL_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.LAL_SUPPLY_TANK_B, state);
                    }

                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_USF500.TotalUsage_Reset(enum_chemical.H2O2); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS1);
                        }
                    }

                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B, state);
                    }

                }
            }
            else if (ccss == enum_ccss.CCSS2)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_USF500.TotalUsage_Reset(enum_chemical.NH4OH); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS2));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS2);
                        }
                    }
                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B, state);
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_USF500.TotalUsage_Reset(enum_chemical.H2SO4); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS2));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS2);
                        }
                    }
                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B, state);
                    }
                }

            }
            else if (ccss == enum_ccss.CCSS3)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_USF500.TotalUsage_Reset(enum_chemical.HF); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS3));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS3);
                        }
                    }
                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_B, state);
                    }
                }
            }
            else if (ccss == enum_ccss.CCSS4)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_USF500.TotalUsage_Reset(enum_chemical.DIW); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4);
                        }
                    }
                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, state);
                    }
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HDIW_REMOTE_START, state);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_SONOTEC.TotalUsage_Reset(enum_chemical.DIW); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4);
                        }
                    }
                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, state);
                    }

                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_USF500.TotalUsage_Reset(enum_chemical.DIW); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4);
                        }
                    }

                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, state);
                    }

                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_SONOTEC.TotalUsage_Reset(enum_chemical.DIW); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4);
                        }
                    }
                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, state);
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    //Valve Open 시 적산 초기화 / Valve Close 시 적산 합산
                    if (state == true) { Program.FlowMeter_USF500.TotalUsage_Reset(enum_chemical.DIW); }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                        {
                            Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4));
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real += TotalUsage_Return_By_EQType_CCSS(enum_ccss.CCSS4);
                        }
                    }

                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, state);
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, state);
                    }

                }
            }

        }
        public bool CCSS_Valve_Use_Check_By_Mixing_Rate(enum_ccss ccss)
        {
            bool result = false;
            if (ccss == enum_ccss.CCSS1)
            {
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Mixing_Rate) != 0)
                {
                    result = true;
                }
            }
            else if (ccss == enum_ccss.CCSS2)
            {
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Mixing_Rate) != 0)
                {
                    result = true;
                }
            }
            else if (ccss == enum_ccss.CCSS3)
            {
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Mixing_Rate) != 0)
                {
                    result = true;
                }
            }
            else if (ccss == enum_ccss.CCSS4)
            {
                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Mixing_Rate) != 0)
                {
                    result = true;
                }
            }

            return result;

        }
        public void CCSS_USE_Signal_OUT()
        {
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, false);
                }

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS2, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS2, false);
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, false);
                }


            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, false);
                }


            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, false);
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, false);
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS1, false);
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS2, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS2, false);
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS3, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CCSS3, false);
                }
            }
        }
        public bool CCSS_INPUT_Status(tank_class.enum_tank_type call_selected_tank)
        {
            bool result = false;
            if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value
                        || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true)
                    {
                        result = true;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                    {
                        result = true;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == true)
                    {
                        result = true;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true)
                    {
                        result = true;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == true)
                    {
                        result = true;
                    }
                }
            }
            else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value
                        || Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                    {
                        result = true;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                    {
                        result = true;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == true)
                    {
                        result = true;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                    {
                        result = true;
                    }
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == true)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }
        public bool CCSS_INPUT_START(tank_class.enum_tank_type call_selected_tank, enum_ccss ccss)
        {
            bool result = false;
            double tmp_totalusage = 0;
            tmp_totalusage = TotalUsage_Return_By_EQType_CCSS(ccss);

            if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real + tmp_totalusage
                < Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volume)
            {
                //Input Start Time, 시간 갱신   
                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = false;
                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].dt_Start = DateTime.Now;
                CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, true, true);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public bool CCSS_INPUT_START_FORCE(tank_class.enum_tank_type call_selected_tank, enum_ccss ccss)
        {
            bool result = false;

            if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
            {
                //Input Start Time, 시간 갱신
                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = false;
                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].dt_Start = DateTime.Now;
                CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, true, true);
                result = true;
            }
            else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
            {
                //Input Start Time, 시간 갱신
                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = false;
                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].dt_Start = DateTime.Now;
                CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, true, true);
                result = true;
            }
            return result;
        }
        public bool CCSS_INPUT_END_FORCE(tank_class.enum_tank_type call_selected_tank, enum_ccss ccss)
        {
            bool result = false;
            if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
            {
                //Input Start Time, 시간 갱신
                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = false;
                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].dt_Start = DateTime.Now;
                CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                result = true;
            }
            else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
            {
                //Input Start Time, 시간 갱신
                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = false;
                Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].dt_Start = DateTime.Now;
                CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                result = true;
            }
            return result;
        }
        public bool CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type call_selected_tank, enum_ccss ccss)
        {
            bool complete = false;
            double tmp_totalusage = 0;
            tmp_totalusage = TotalUsage_Return_By_EQType_CCSS(ccss);


            if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == false)
                {
                    Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].flag_level_hh = false;
                }

                //Semi Auto DIW Flush 사용할 때는 Volume 무시
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == false
                    && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == false)
                {
                    if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volume != 0)
                    {
                        if (ccss == enum_ccss.CCSS4 && timer_manual_sequence_tank_a.Enabled == true &&
                            (Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH
                            || Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY
                            || Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.AUTO_FLUSH))
                        {
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real + tmp_totalusage
                                 >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_H))
                            {
                                if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete == false)
                                {
                                    Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = true;
                                    CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                                    //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                                    complete = true;
                                }
                            }
                        }
                        else
                        {
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real + tmp_totalusage
                                 >= Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volume)
                            {
                                if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete == false)
                                {
                                    Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = true;
                                    CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                                    //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                                    complete = true;
                                }
                            }
                        }
                    }
                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == true
                           && (timer_manual_sequence_tank_a.Enabled == true || Program.cg_app_info.eq_mode == enum_eq_mode.auto))
                {
                    if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real + tmp_totalusage
                                  >= Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volume)
                    {
                        if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete == false)
                        {
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = true;
                            CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                            //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                            complete = true;
                        }
                    }
                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == false)
                {
                    if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real + tmp_totalusage
                        >= Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volume)
                    {
                        if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete == false)
                        {
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = true;
                            CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                            //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                            complete = true;
                        }
                    }
                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == true && (timer_manual_sequence_tank_a.Enabled == false || Program.cg_app_info.eq_mode == enum_eq_mode.manual))
                {
                    //적산에 관계없이 H Level일때는 Valve Close
                    if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].flag_level_hh == false)
                    {
                        Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].flag_level_hh = true;
                        CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                        //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                    }
                    else
                    {
                        CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, false);
                    }
                    complete = true;
                }

            }
            else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == false)
                {
                    Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].flag_level_hh = false;
                }
                //Semi Auto DIW Flush 사용할 때는 Volume 무시
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == false
                    && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == false)
                {
                    if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volume != 0)
                    {
                        if (ccss == enum_ccss.CCSS4 && timer_manual_sequence_tank_b.Enabled == true &&
                            (Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH
                            || Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY
                            || Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.AUTO_FLUSH))
                        {
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real + tmp_totalusage
                                 >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_A_Level_H))
                            {
                                if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete == false)
                                {
                                    Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = true;
                                    CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                                    //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                                    complete = true;
                                }
                            }
                        }
                        else
                        {
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real + tmp_totalusage
                                 >= Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volume)
                            {
                                if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete == false)
                                {
                                    Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = true;
                                    CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                                    //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                                    complete = true;
                                }
                            }
                        }
                    }
                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == true
                    && (timer_manual_sequence_tank_b.Enabled == true || Program.cg_app_info.eq_mode == enum_eq_mode.auto))
                {
                    if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real + tmp_totalusage
                                  >= Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volume)
                    {
                        if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete == false)
                        {
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = true;
                            CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                            //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                            complete = true;
                        }
                    }
                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == false)
                {
                    if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real + tmp_totalusage
                        >= Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volume)
                    {
                        if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete == false)
                        {
                            Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = true;
                            CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                            //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                            complete = true;
                        }
                    }
                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == true && (timer_manual_sequence_tank_b.Enabled == false || Program.cg_app_info.eq_mode == enum_eq_mode.manual))
                {
                    //적산에 관계없이 H Level일때는 Valve Close
                    if (Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].flag_level_hh == false)
                    {
                        Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].flag_level_hh = true;
                        CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                        //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                    }
                    else
                    {
                        CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, false);
                    }
                    complete = true;
                }
            }
            return complete;
        }
        public bool CCSS_INPUT_END_BY_LEVEL_H(tank_class.enum_tank_type call_selected_tank, enum_ccss ccss)
        {
            bool complete = false;
            //AV-01
            if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == true)
                {
                    Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = true;
                    CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                    //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                    complete = true;
                }
            }
            else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
            {
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == true)
                {
                    Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_complete = true;
                    CCSS_Valve_Control_By_EQType_CCSS(ccss, call_selected_tank, false, true);
                    //Program.totalusagelog_form.Insert_Total_Usage(call_selected_tank, (int)ccss, Program.tank[(int)call_selected_tank].ccss_data[(int)ccss].input_volmue_real);
                    complete = true;
                }
            }
            return complete;
        }
        public void Tank_Value_Clear(tank_class.enum_tank_type cur_tank, bool all_clear)
        {
            //신호 초기화
            Program.seq.manual_exchange_req_by_user = false;
            Program.seq.manual_exchange_ack_by_ctc = false;
            Program.seq.input_request = true;

            Program.seq.cur_mixing_index = -1;

            if (all_clear == true)
            {
                Program.seq.supply.c_c_need = false;
                //Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state = false;
                Program.seq.main.no_cur = tank_class.enum_seq_no.NONE;
                Program.seq.supply.no_cur = tank_class.enum_seq_no_supply.TANK_READY_CHECK;
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, false);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_DRAIN, false);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, false);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RECLAIM_DRAIN, true);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_HE_UNIT, false);

                CIRCULATION_1_HEATER_ON_OFF(false);
                SUPPLY_A_HEATER_ON_OFF(false);
                SUPPLY_B_HEATER_ON_OFF(false);
                CIRCULATION_PUMP_ON_OFF(false);
                SUPPLY_A_PUMP_ON_OFF(false);
                SUPPLY_B_PUMP_ON_OFF(false);

                if (Program.cg_app_info.mode_simulation.use == true)
                {
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value = false;

                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value = false;
                    Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value = false;
                }
                else
                {
                }

            }
            else
            {
                if (cur_tank == tank_class.enum_tank_type.TANK_A)
                {
                    //TANK A Clear
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_RECLAIM_TO_TANK_A, false);

                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DSP_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.LAL_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.IPA_SUPPLY_TANK, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, false);

                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt = 0;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_process = DateTime.Now;

                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.NONE;

                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete = false;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete = false;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete = false;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete = false;





                }
                else if (cur_tank == tank_class.enum_tank_type.TANK_B)
                {
                    //TANK B Clear
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_RECLAIM_TO_TANK_B, false);

                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DSP_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.LAL_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.IPA_SUPPLY_TANK, false);

                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_B, false);

                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt = 0;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_start_process = DateTime.Now;

                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.NONE;

                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete = false;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete = false;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete = false;
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete = false;


                }
            }

        }
        public void Tank_CCSS_Input_Complete_Flag_Clear(tank_class.enum_tank_type call_selected_tank, Queue<CCSS_Info> mixing_order)
        {
            CCSS_Info[] mixing_use_check;
            mixing_use_check = mixing_order.ToArray();

            //하위에서 실제 사용하는 CCSS Use만 다시 Check 된다
            Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].use = false;
            Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].use = false;
            Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].use = false;
            Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].use = false;

            //하위에서 사용하지 않는 CCSS Input_Complete만 다시false된다. -> 실제 투입 후 true로 변환되어야함
            Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete = true;
            Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete = true;
            Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete = true;
            Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete = true;

            if (mixing_use_check != null)
            {
                for (int idx = 0; idx < mixing_use_check.Length; idx++)
                {
                    if (mixing_use_check[idx].type == enum_ccss.CCSS1) { Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].use = true; Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete = false; }
                    else if (mixing_use_check[idx].type == enum_ccss.CCSS2) { Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].use = true; Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete = false; }
                    else if (mixing_use_check[idx].type == enum_ccss.CCSS3) { Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].use = true; Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete = false; }
                    else if (mixing_use_check[idx].type == enum_ccss.CCSS4) { Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].use = true; Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete = false; }
                }
            }


        }
        public void Chemical_Change_Start(tank_class.enum_tank_type cur_tank)
        {
            if (cur_tank == tank_class.enum_tank_type.TANK_A)
            {
                Program.CTC.Message_Chemical_Change_Start_Event_452("1");
            }
            else if (cur_tank == tank_class.enum_tank_type.TANK_B)
            {
                Program.CTC.Message_Chemical_Change_Start_Event_452("2");
            }
        }
        public void Chemical_Change_End(tank_class.enum_tank_type cur_tank)
        {
            if (cur_tank == tank_class.enum_tank_type.TANK_A)
            {
                Program.CTC.Message_Chemical_Change_End_Event_453("1");
            }
            else if (cur_tank == tank_class.enum_tank_type.TANK_B)
            {
                Program.CTC.Message_Chemical_Change_End_Event_453("2");
            }
        }
        public void Tank_Supply_End(tank_class.enum_tank_type cur_tank)
        {
            Program.seq.supply.c_c_need = false;
            Program.seq.supply.concentration_measuring = false;
            Program.seq.supply.refill_run_state = false;

            //if (Program.seq.supply.cc_ctc_req_flag == true) { Program.CTC.Message_Chemical_Change_Start_Event_452("1"); }
            if (cur_tank == tank_class.enum_tank_type.TANK_A)
            {
                //TANK A Clear
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                    //System.Threading.Thread.Sleep(500);
                    SUPPLY_A_PUMP_ON_OFF(false);
                    SUPPLY_B_PUMP_ON_OFF(false);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.IPA_SUPPLY_TANK, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                    //System.Threading.Thread.Sleep(500);
                    SUPPLY_A_PUMP_ON_OFF(false);
                    SUPPLY_B_PUMP_ON_OFF(false);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_A, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                    //System.Threading.Thread.Sleep(500);
                    SUPPLY_A_PUMP_ON_OFF(false);
                    SUPPLY_B_PUMP_ON_OFF(false);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                    //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, false);
                    //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_A, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                    //System.Threading.Thread.Sleep(500);
                    SUPPLY_A_PUMP_ON_OFF(false);
                    SUPPLY_B_PUMP_ON_OFF(false);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                    //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, false);
                    //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.LAL_SUPPLY_TANK_A, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                    //System.Threading.Thread.Sleep(500);
                    SUPPLY_A_PUMP_ON_OFF(false);
                    SUPPLY_B_PUMP_ON_OFF(false);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                    //System.Threading.Thread.Sleep(500);
                    SUPPLY_A_PUMP_ON_OFF(false);
                    SUPPLY_B_PUMP_ON_OFF(false);
                }


                Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt = 0;
                Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_process = DateTime.Now;
                if (Program.seq.supply.CDS_enable_status_to_ctc == true) { Program.CTC.Message_CDS_Disable_Event_451(false); }
                if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.REFILL)
                {
                    Program.CTC.Message_Tank_A_Supply_End_Event_459();
                }
                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.NONE;

                Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete = false;
                Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete = false;
                Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete = false;
                Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete = false;
                Program.seq.supply.supply_status = false;
            }
            else if (cur_tank == tank_class.enum_tank_type.TANK_B)
            {
                //TANK B Clear
                if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DSP_SUPPLY_TANK_B, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                    //System.Threading.Thread.Sleep(500);
                    SUPPLY_A_PUMP_ON_OFF(false);
                    SUPPLY_B_PUMP_ON_OFF(false);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_B, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                    //System.Threading.Thread.Sleep(500);
                    SUPPLY_A_PUMP_ON_OFF(false);
                    SUPPLY_B_PUMP_ON_OFF(false);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.LAL_SUPPLY_TANK_B, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                    //System.Threading.Thread.Sleep(500);
                    SUPPLY_A_PUMP_ON_OFF(false);
                    SUPPLY_B_PUMP_ON_OFF(false);
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HF_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B, false);
                    SUPPLY_A_HEATER_ON_OFF(false);
                    SUPPLY_B_HEATER_ON_OFF(false);
                    //System.Threading.Thread.Sleep(500);
                    SUPPLY_A_PUMP_ON_OFF(false);
                    SUPPLY_B_PUMP_ON_OFF(false);
                }
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt = 0;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_start_process = DateTime.Now;
                if (Program.seq.supply.CDS_enable_status_to_ctc == true) { Program.CTC.Message_CDS_Disable_Event_451(false); }
                if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.REFILL)
                {
                    Program.CTC.Message_Tank_B_Supply_End_Event_461();
                }

                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.NONE;

                Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete = false;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete = false;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete = false;
                Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete = false;
                Program.seq.supply.supply_status = false;
            }
            Program.seq.supply.no_cur = tank_class.enum_seq_no_supply.TANK_READY_CHECK;
        }
        public void TotalUsage_Reset(tank_class selected_tank)
        {
            //적산 초기화
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_USF500.NH4OH_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_USF500.H2SO4_totalusage = 0;
            }
            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                Program.main_form.SerialData.FlowMeter_USF500.NH4OH_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage = 0;

                Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_USF500.ch1_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 1, Class_FlowMeter_USF500.ch1_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 2, Class_FlowMeter_USF500.ch2_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = 0;

                Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_USF500.ch1_totalusage_reset, (int)Config_IO.enum_dhf_serial_index.USF500_FLOWMETER);
                Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 2, Class_FlowMeter_USF500.ch2_totalusage_reset, (int)Config_IO.enum_dhf_serial_index.USF500_FLOWMETER);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage = 0;

                Program.FlowMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.totalusage_reset, (int)Config_IO.enum_dsp_serial_index.SONOTEC_FLOWMETER);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                Program.main_form.SerialData.FlowMeter_Sonotec.LAL_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage = 0;

                Program.FlowMeter_SONOTEC.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_Sonotec.totalusage_reset, (int)Config_IO.enum_lal_serial_index.SONOTEC_FLOWMETER);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                Program.main_form.SerialData.FlowMeter_USF500.HF_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_USF500.DIW_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_USF500.H2O2_totalusage = 0;
                Program.main_form.SerialData.FlowMeter_USF500.H2SO4_totalusage = 0;

                Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 1, Class_FlowMeter_USF500.ch1_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(1, 2, Class_FlowMeter_USF500.ch2_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 1, Class_FlowMeter_USF500.ch1_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
                Program.FlowMeter_USF500.Message_Command_Apply_CRC_TO_Send(2, 2, Class_FlowMeter_USF500.ch2_totalusage_reset, (int)Config_IO.enum_apm_serial_index.USF500_FLOWMETER);
            }
            selected_tank.ccss_data[0].input_volmue_real = 0;
            selected_tank.ccss_data[1].input_volmue_real = 0;
            selected_tank.ccss_data[2].input_volmue_real = 0;
            selected_tank.ccss_data[3].input_volmue_real = 0;
        }
        public void Concentration_Reset(tank_class.enum_tank_type selected_tank)
        {
            //농도 초기화
            Program.tank[(int)selected_tank].concentration_ccss1 = 0;
            Program.tank[(int)selected_tank].concentration_ccss2 = 0;
            Program.tank[(int)selected_tank].concentration_ccss3 = 0;
            Program.tank[(int)selected_tank].concentration_ccss4 = 0;
        }
        /// <summary>
        /// Manual상태에서 Input Valve 제어(Interlock)
        /// </summary>
        public void Manual_Valve_Close_By_TotalUsage_Over()
        {
            //Manual중 사용자가 직접 Valve 제어 시 적산량 만큼만 입력
            if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
            {

                //auto Mode이거나 Semi-Auto 모드에서는 Valve 강제로 OFF하지 않는다.
                if (timer_manual_sequence_tank_a.Enabled == false && timer_manual_sequence_tank_b.Enabled == false)
                {
                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS4);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS4);
                        }

                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS1);
                        }

                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS2);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.NH4OH_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS2);
                        }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1);
                        }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS4);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS4);
                        }

                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS1);
                        }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS4);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS4);
                        }

                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS1);
                        }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS4);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS4);
                        }

                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS1);
                        }
                    }
                    else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS4);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS4);
                        }

                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2O2_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS1);
                        }

                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS2);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.H2SO4_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS2);
                        }

                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_A].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS3);
                        }
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.HF_SUPPLY_TANK_B].value == true)
                        {
                            CCSS_INPUT_END_BY_TOTALUSAGE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS3);
                        }
                    }
                }


            }

        }
        public void CCSS_INPUT_Check(enum_ccss selected_ccss, tank_class.enum_tank_type selected_tank)
        {
            if (selected_tank == tank_class.enum_tank_type.TANK_A)
            {
                if (Program.cg_mixing_step.mixing_use == false && Program.cg_mixing_step.refill_use == true)
                {
                    CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_A, selected_ccss);
                }
                else
                {
                    CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_A, selected_ccss);
                }
            }
            else if (selected_tank == tank_class.enum_tank_type.TANK_B)
            {
                if (Program.cg_mixing_step.mixing_use == false && Program.cg_mixing_step.refill_use == true)
                {
                    CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_B, selected_ccss);
                }
                else
                {
                    CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_B, selected_ccss);
                }
            }

        }
        public bool Chemical_Original_Check()
        {
            bool result = false;
            int mixing_use_cnt = 0; //원액 Type의 경우 Parameter의 CCSS rate가 1개만 0 초과 여야만 한다.         

            if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Mixing_Rate) != 0)
            {
                mixing_use_cnt = mixing_use_cnt + 1;
            }
            if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Mixing_Rate) != 0)
            {
                mixing_use_cnt = mixing_use_cnt + 1;
            }
            if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Mixing_Rate) != 0)
            {
                mixing_use_cnt = mixing_use_cnt + 1;
            }
            if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Mixing_Rate) != 0)
            {
                mixing_use_cnt = mixing_use_cnt + 1;
            }


            if (mixing_use_cnt == 1 && Program.cg_mixing_step.refill_use == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public string SUPPLY_A_HEATER_ON_OFF(bool state)
        {
            string result = "";
            string log = "";
            if (state == true)
            {

                if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true && (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true))
                {
                    log = "SUPPLY_A PRESS : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS].value;
                    log = log + "SUPPLY_A PRESS HIGH : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Supply_A_IN);
                    log = log + "SUPPLY_A PRESS LOW : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Supply_A_IN);

                    log = log + "SUPPLY_A FLOW : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value;
                    log = log + "SUPPLY_A FLOW HIGH : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_A);
                    log = log + "SUPPLY_A FLOW LOW : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_A);

                    if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS].value >
                                                   Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Supply_A_IN)
                                                   && Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS].value
                                                   < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Supply_A_IN))
                    {
                        if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value >
                          Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_A)
                          && Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value
                          < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_A))
                        {
                            Program.main_form.Insert_System_Message("Supply A Heater On");
                            Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "Supply A Heater On" + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value, Module_Log.enumLevel.ALWAYS);
                        }
                        else
                        {
                            result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "SUPPLY_A_HEATER_ON_OFF : Cannot Start because Pump A, Status(flow)";
                        }
                    }
                    else
                    {
                        result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "SUPPLY_A_HEATER_ON_OFF : Cannot Start because Pump A, Status(press)";
                    }

                }
                else
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "SUPPLY_A_HEATER_ON_OFF : Cannot Start because Pump Status or Valve";
                }

                if (result == "")
                {
                    SUPPLY_A_HEATER_SET();
                    if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SCR1_RUN, state);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_A_HEATER_PWR_ON, state);
                        Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.supply_a, true);
                    }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_A_THERMOSTAT_PWR_ON].use == true)
                        {
                            if (Program.cg_app_info.mode_simulation.use == true) { Program.main_form.SerialData.Supply_A_Thermostat.heater_on = true; }
                            Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.Heater_ON, (int)Config_IO.enum_dsp_serial_index.SUPPLY_A_THERMOSTAT);
                        }
                    }
                }
                else if (result != "")
                {
                    Program.main_form.Insert_System_Message("Fail Supply A Heater On");
                    Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                }
            }
            else
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SCR1_RUN, state);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_A_HEATER_PWR_ON, state);
                    Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.supply_a, false);
                }
                else
                {
                    if (Program.cg_app_info.mode_simulation.use == true) { Program.main_form.SerialData.Supply_A_Thermostat.heater_on = false; }
                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.Heater_OFF, (int)Config_IO.enum_dsp_serial_index.SUPPLY_A_THERMOSTAT);
                }
            }
            return result;
        }
        public string SUPPLY_B_HEATER_ON_OFF(bool state)
        {
            string result = "";
            string log = "";
            if (state == true)
            {
                if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true && (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true))
                {
                    log = "SUPPLY_B PRESS : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS].value;
                    log = log + "SUPPLY_B PRESS HIGH : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Supply_B_IN);
                    log = log + "SUPPLY_B PRESS LOW : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Supply_B_IN);

                    log = log + "SUPPLY_B FLOW : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value;
                    log = log + "SUPPLY_B FLOW HIGH : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_B);
                    log = log + "SUPPLY_B FLOW LOW : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_B);

                    if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS].value >
                               Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Supply_B_IN)
                               && Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS].value
                               < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Supply_B_IN))
                    {
                        if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value >
                               Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_B)
                               && Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value
                               < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_B))
                        {
                            Program.main_form.Insert_System_Message("Supply B Heater On");
                            Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "Supply B Heater On" + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value, Module_Log.enumLevel.ALWAYS);
                        }
                        else
                        {
                            result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "SUPPLY_B_HEATER_ON_OFF : Cannot Start because Pump B, Status(flow)";
                        }
                    }
                    else
                    {
                        result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "SUPPLY_B_HEATER_ON_OFF : Cannot Start because Pump B, Status(press)";
                    }
                }
                else
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "SUPPLY_B_HEATER_ON_OFF : Cannot Start because Pump Status or Valve";
                }

                if (result == "")
                {
                    SUPPLY_B_HEATER_SET();
                    if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SCR2_RUN, state);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_B_HEATER_PWR_ON, state);
                        Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.supply_b, true);
                    }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_B_THERMOSTAT_PWR_ON].use == true)
                        {
                            if (Program.cg_app_info.mode_simulation.use == true) { Program.main_form.SerialData.Supply_B_Thermostat.heater_on = true; }
                            Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.Heater_ON, (int)Config_IO.enum_dsp_serial_index.SUPPLY_B_THERMOSTAT);
                        }
                    }
                }
                else if (result != "")
                {
                    Program.main_form.Insert_System_Message("Fail Supply B Heater On");
                    Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                }
            }
            else
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SCR2_RUN, state);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_B_HEATER_PWR_ON, state);
                    Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.supply_b, false);
                }
                else
                {
                    if (Program.cg_app_info.mode_simulation.use == true) { Program.main_form.SerialData.Supply_B_Thermostat.heater_on = false; }
                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.Heater_OFF, (int)Config_IO.enum_dsp_serial_index.SUPPLY_B_THERMOSTAT);
                }
            }
            return result;
        }
        public string CIRCULATION_1_HEATER_ON_OFF(bool state)
        {
            string result = "";
            string log = "";
            if (state == true)
            {
                log = "CIRCULATION_1 PRESS : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value;
                log = log + "CIRCULATION_1 PRESS HIGH : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Tank_Circulation);
                log = log + "CIRCULATION_1 PRESS LOW : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Tank_Circulation);

                log = log + "CIRCULATION_1 FLOW : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value;
                log = log + "CIRCULATION_1 FLOW HIGH : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Tank_Circulation);
                log = log + "CIRCULATION_1 FLOW LOW : " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Tank_Circulation);

                if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true && (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true))
                {

                    if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value >
                               Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Tank_Circulation)
                               && Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value
                               < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Tank_Circulation))
                    {
                        if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value >
                               Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Tank_Circulation)
                               && Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value
                               < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Tank_Circulation))
                        {

                            Program.main_form.Insert_System_Message("Circulation1 Heater On");
                            Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "Circulation1 Heater On" + Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value, Module_Log.enumLevel.ALWAYS);
                        }
                        else
                        {
                            result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "CIRCULATION_1_HEATER_ON_OFF : Cannot Start because Circulation Status(flow)";
                        }

                    }

                    else
                    {
                        result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "CIRCULATION_1_HEATER_ON_OFF : Cannot Start because Circulation Status(press)";
                    }
                }
                else
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "CIRCULATION_1_HEATER_ON_OFF : Cannot Start because Pump Status OR Valve";
                }

                if (result == "")
                {
                    CIRCULATION_1_HEATER_SET();
                    if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.ipa)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SCR3_RUN, state);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SCR4_RUN, state);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION1_HEATER_PWR_ON, state);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION2_HEATER_PWR_ON, state);


                        if (Program.cg_app_info.use_heating_output_mapping_tank == false)
                        {
                            Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.circulation, true);
                        }
                        else if (Program.cg_app_info.use_heating_output_mapping_tank == true)
                        {
                            //APM Target Temp -> Tank 승온 기능 활성화(기본 Circulation Line)
                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                            {
                                Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.tank_a, true);
                            }
                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                            {
                                Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.tank_b, true);
                            }
                        }
                    }
                    else
                    {
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIRCULATION_THERMOSTAT_PWR_ON].use == true)
                        {
                            if (Program.cg_app_info.mode_simulation.use == true) { Program.main_form.SerialData.Circulation_Thermostat.heater_on = true; }
                            if (Program.cg_app_info.he3320c_mode == enum_he3320c_mode.monitor_tank)
                            {
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                                {
                                    Program.ThermoStart_HE_3320C.Select_Tank(Class_ThermoStat_HE_3320C.he3320c_selected_tank.tank_b, false, (int)Config_IO.enum_dsp_mix_serial_index.CIRCULATION_THERMOSTAT);
                                    Program.ThermoStart_HE_3320C.Select_Tank(Class_ThermoStat_HE_3320C.he3320c_selected_tank.tank_a, true, (int)Config_IO.enum_dsp_mix_serial_index.CIRCULATION_THERMOSTAT);
                                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_mix_serial_index.CIRCULATION_THERMOSTAT);
                                }
                                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                                {
                                    Program.ThermoStart_HE_3320C.Select_Tank(Class_ThermoStat_HE_3320C.he3320c_selected_tank.tank_a, false, (int)Config_IO.enum_dsp_mix_serial_index.CIRCULATION_THERMOSTAT);
                                    Program.ThermoStart_HE_3320C.Select_Tank(Class_ThermoStat_HE_3320C.he3320c_selected_tank.tank_b, true, (int)Config_IO.enum_dsp_mix_serial_index.CIRCULATION_THERMOSTAT);
                                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_mix_serial_index.CIRCULATION_THERMOSTAT);
                                }
                            }
                            Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.Heater_ON, (int)Config_IO.enum_dsp_mix_serial_index.CIRCULATION_THERMOSTAT);
                        }
                    }

                }
                else if (result != "")
                {
                    Program.main_form.Insert_System_Message("Fail Cir1 Heater On");
                    Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                }
            }
            else
            {
                if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.ipa)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SCR3_RUN, state);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SCR4_RUN, state);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION1_HEATER_PWR_ON, state);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION2_HEATER_PWR_ON, state);
                    if (Program.cg_app_info.use_heating_output_mapping_tank == false)
                    {
                        Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.circulation, false);
                    }
                    else if (Program.cg_app_info.use_heating_output_mapping_tank == true)
                    {
                        //APM Target Temp -> Tank 승온 기능 활성화(기본 Circulation Line)
                        if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                        {
                            Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.tank_a, false);
                        }
                        else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                        {
                            Program.TempController_M74.M74_Run_And_Stop(Class_TempController_M74.enum_m74_type.tank_b, false);
                        }
                    }
                }
                else
                {
                    if (Program.cg_app_info.mode_simulation.use == true) { Program.main_form.SerialData.Circulation_Thermostat.heater_on = false; }
                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.Heater_OFF, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                }
            }
            return result;
        }
        public void SUPPLY_A_HEATER_SET()
        {
            float set_value = 0;
            tank_class.enum_tank_type call_selected_tank = tank_class.enum_tank_type.TANK_A;

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true)
            {
                call_selected_tank = tank_class.enum_tank_type.TANK_A;
                set_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Set);
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true)
            {
                call_selected_tank = tank_class.enum_tank_type.TANK_B;
                set_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_Set);
            }

            if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv = set_value;
                Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.SUPPLY_A_THERMOSTAT);
                Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.SUPPLY_A_THERMOSTAT);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv = set_value;
                Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.SUPPLY_A_THERMOSTAT);
                Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dhf_serial_index.SUPPLY_A_THERMOSTAT);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv = set_value;
                Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.SUPPLY_A_THERMOSTAT);
                Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dhf_serial_index.SUPPLY_A_THERMOSTAT);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_a.sv = set_value;
                Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.SUPPLY_A_THERMOSTAT);
                Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dhf_serial_index.SUPPLY_A_THERMOSTAT);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                Program.TempController_M74.M74_Temp_Set(Class_TempController_M74.enum_m74_type.supply_a, Convert.ToInt32(set_value * 10));
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                Program.TempController_M74.M74_Temp_Set(Class_TempController_M74.enum_m74_type.supply_a, Convert.ToInt32(set_value * 10));
            }
        }
        public void SUPPLY_B_HEATER_SET()
        {
            float set_value = 0;
            tank_class.enum_tank_type call_selected_tank = tank_class.enum_tank_type.TANK_A;

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true)
            {
                call_selected_tank = tank_class.enum_tank_type.TANK_A;
                set_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Set);
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true)
            {
                call_selected_tank = tank_class.enum_tank_type.TANK_B;
                set_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_Set);
            }

            if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.sv = set_value;
                Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.SUPPLY_B_THERMOSTAT);
                Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.SUPPLY_B_THERMOSTAT);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.sv = set_value;
                Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.SUPPLY_B_THERMOSTAT);
                Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dhf_serial_index.SUPPLY_B_THERMOSTAT);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.sv = set_value;
                Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.SUPPLY_B_THERMOSTAT);
                Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dhf_serial_index.SUPPLY_B_THERMOSTAT);
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                Program.TempController_M74.M74_Temp_Set(Class_TempController_M74.enum_m74_type.supply_b, Convert.ToInt32(set_value * 10));
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                Program.main_form.SerialData.TEMP_CONTROLLER.supply_heater_b.sv = set_value;
                Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.SUPPLY_B_THERMOSTAT);
                Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dhf_serial_index.SUPPLY_B_THERMOSTAT);
            }

            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                Program.TempController_M74.M74_Temp_Set(Class_TempController_M74.enum_m74_type.supply_b, Convert.ToInt32(set_value * 10));
            }
        }
        public void CIRCULATION_1_HEATER_SET()
        {
            float set_value = 0;
            tank_class.enum_tank_type call_selected_tank = tank_class.enum_tank_type.TANK_A;

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
            {
                call_selected_tank = tank_class.enum_tank_type.TANK_A;
                set_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Set);
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
            {
                call_selected_tank = tank_class.enum_tank_type.TANK_B;
                set_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Set);
            }

            if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                if (Program.cg_app_info.he3320c_mode == enum_he3320c_mode.monitor_tank)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.sv = set_value;
                        Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                        Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.sv = set_value;
                        Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                        Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    }
                }
                else
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = set_value;
                    Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dhf)
            {
                if (Program.cg_app_info.he3320c_mode == enum_he3320c_mode.monitor_tank)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.sv = set_value;
                        Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                        Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.sv = set_value;
                        Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                        Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    }
                }
                else
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = set_value;
                    Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                if (Program.cg_app_info.he3320c_mode == enum_he3320c_mode.monitor_tank)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.sv = set_value;
                        Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                        Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.sv = set_value;
                        Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                        Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    }
                }
                else
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = set_value;
                    Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
            {
                if (Program.cg_app_info.he3320c_mode == enum_he3320c_mode.monitor_tank)
                {
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.sv = set_value;
                        Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                        Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    }
                    else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                    {
                        Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.sv = set_value;
                        Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                        Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    }
                }
                else
                {
                    Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = set_value;
                    Program.ThermoStart_HE_3320C.Set_SV(Convert.ToInt32(set_value * 100), (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                    Program.ThermoStart_HE_3320C.Message_Command_Apply_CRC_TO_Send(Class_ThermoStat_HE_3320C.data_apply, (int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT);
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.apm)
            {
                if (Program.cg_app_info.use_heating_output_mapping_tank == false)
                {
                    Program.TempController_M74.M74_Temp_Set(Class_TempController_M74.enum_m74_type.circulation, Convert.ToInt32(set_value * 10));
                }
                else if (Program.cg_app_info.use_heating_output_mapping_tank == true)
                {
                    if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                    {
                        Program.TempController_M74.M74_Temp_Set(Class_TempController_M74.enum_m74_type.tank_a, Convert.ToInt32(set_value * 10));
                    }
                    else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                    {
                        Program.TempController_M74.M74_Temp_Set(Class_TempController_M74.enum_m74_type.tank_b, Convert.ToInt32(set_value * 10));
                    }
                }
            }
        }
        public void CIRCULATION_Heat_Exchanger_SET()
        {
            float set_value = 0;
            tank_class.enum_tank_type call_selected_tank = tank_class.enum_tank_type.TANK_A;

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
            {
                call_selected_tank = tank_class.enum_tank_type.TANK_A;
                set_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Set);
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
            {
                call_selected_tank = tank_class.enum_tank_type.TANK_B;
                set_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Set);
            }
            Program.main_form.SerialData.TEMP_CONTROLLER.circulation_heater1.sv = set_value;
            Program.Heat_Exchanger.Set_SV(Convert.ToInt32(set_value * 10));
        }
        public string CIRCULATION_Heat_Exchanger_ON_OFF(bool state)
        {
            string result = "";
            string log = "";
            if (state == true)
            {
                if (Program.Heat_Exchanger.run_state == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_HE_UNIT].value == true && (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true))
                {
                    if (Program.cg_app_info.mode_simulation.use == true) { Program.Heat_Exchanger.heater_on = true; }
                    Program.Heat_Exchanger.Heater_On_Off(true);
                    Program.main_form.Insert_System_Message("Heat Exchanger Heater On");
                    Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "Heat Exchanger Heater On", Module_Log.enumLevel.ALWAYS);
                }
                else
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "CIRCULATION_Heat_Exchanger_ON_OFF : Cannot Start because Pump Status OR Valve";
                }
                if (result == "")
                {
                    CIRCULATION_Heat_Exchanger_SET();
                }
                else if (result != "")
                {
                    Program.main_form.Insert_System_Message("Fail Heat Exchanger Heater On");
                    Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                }
            }
            else
            {
                if (Program.cg_app_info.mode_simulation.use == true) { Program.Heat_Exchanger.heater_on = false; }
                Program.Heat_Exchanger.Heater_On_Off(false);
            }
            return result;
        }
        public string CIRCULATION_PUMP_ON_OFF(bool state)
        {
            string result = "";
            if (state == true)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true
                    && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_DRAIN].value == false && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true
                    && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_DRAIN].value == false && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_A].value == true
                    && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_DRAIN].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_TO_TANK_B].value == true
                    && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_DRAIN].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == true)
                {
                }
                else
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "CIRCULATION_PUMP_ON_OFF : Cannot Start because Valve Status";
                    result = result + ", CIR_FROM_TANK_A" + ":" + Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value.ToString() + System.Environment.NewLine;
                    result = result + ", CIR_FROM_TANK_B" + ":" + Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value.ToString();
                    result = result + ", CIR_DRAIN" + ":" + Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_DRAIN].value.ToString() + System.Environment.NewLine;
                    result = result + ", TANKA_EMPTY_CHECK" + ":" + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value.ToString();
                    result = result + ", TANKB_EMPTY_CHECK" + ":" + Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value.ToString();
                }

                if (result == "") { Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state = state; }
                else if (result != "") { Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS); }
            }
            else
            {
                Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state = state;
            }
            //interlock Pump Close시 Heater 종료
            if (state == false)
            {
                CIRCULATION_1_HEATER_ON_OFF(false);
            }
            return result;
        }
        public string SUPPLY_A_PUMP_ON_OFF(bool state)
        {
            string result = "";
            if (state == true)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == false
                    && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == false
                    && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true
                    && Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true
                    && Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true
                   && Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true
                   && Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true
                    && (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true)
                    && Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true
                    && (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true)
                    && Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.ipa && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true
                    && (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_A].value == false && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == false)
                    && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "SUPPLY_A_PUMP_ON_OFF : Cannot Start because Valve Status";
                }
                if (result == "") { Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state = state; Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_PUMP_A_START, state); }
                else if (result != "") { Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS); }

            }
            else
            {
                Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state = state; Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_PUMP_A_START, state);
            }
            //interlock Pump Close시 Heater 종료
            if (state == false) { SUPPLY_A_HEATER_ON_OFF(false); }
            return result;
        }
        public string SUPPLY_B_PUMP_ON_OFF(bool state)
        {
            string result = "";
            if (state == true)
            {
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == false
                               && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == false
                    && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true
                    && Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true
                    && Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true
                   && Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                {
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_TO_MAIN_B].value == true
                   && Program.IO.DO.Tag[(int)Config_IO.enum_do.MAIN_RETURN_DRAIN].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == true)
                {

                }
                else
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "SUPPLY_B_PUMP_ON_OFF : Cannot Start because Valve Status";
                }
                if (result == "") { Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state = state; Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_PUMP_B_START, state); }
                else if (result != "") { Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS); }
            }
            else
            {
                Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state = state; Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_PUMP_B_START, state);
            }

            //interlock Pump Close시 Heater 종료
            if (state == false) { SUPPLY_B_HEATER_ON_OFF(false); }
            return result;
        }
        public void PCW_VALVE_ON_OFF(bool state)
        {
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_CIR].use == true) { Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_CIR, state); }
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_A].use == true) { Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_A, state); }
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_B].use == true) { Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.PCW_HEAT_CONTROLLER_B, state); }
        }
        public void Thermostat_Power_ON_OFF(bool state)
        {
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIRCULATION_THERMOSTAT_PWR_ON].use == true) { Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_THERMOSTAT_PWR_ON, state); }
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_A_THERMOSTAT_PWR_ON].use == true) { Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_A_THERMOSTAT_PWR_ON, state); }
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_B_THERMOSTAT_PWR_ON].use == true) { Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_B_THERMOSTAT_PWR_ON, state); }
        }
    }
}