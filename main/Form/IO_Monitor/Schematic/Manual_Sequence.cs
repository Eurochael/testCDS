using System;
using static cds.Class_Concentration_CS600F;
using static cds.Config_IO;

namespace cds
{
    partial class frm_schematic
    {
        public int tmp_para_value = 0, tmp_para_value_1 = 1;
        public int default_ready = 100;
        public void Seq_Manual_Drain(tank_class.enum_tank_type call_selected_tank, tank_class.enum_seq_no_semi_auto seq_no, bool timer_stop_after_complete)
        {

            try
            {
                if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                {
                    Program.seq.semi_auto_tank_a.last_act_span = DateTime.Now - Program.seq.semi_auto_tank_a.last_act_time;
                }
                else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                {
                    Program.seq.semi_auto_tank_b.last_act_span = DateTime.Now - Program.seq.semi_auto_tank_b.last_act_time;
                }

                switch (seq_no)
                {
                    case tank_class.enum_seq_no_semi_auto.NONE:

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.INITIAL, "");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.INITIAL, "");
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.INITIAL:


                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.NONE;
                            Program.seq.semi_auto_tank_a.semi_auto_complete = false;
                            Program.seq.semi_auto_tank_a.semi_auto_run_count = 0;
                            Program.seq.semi_auto_tank_a.semi_auto_run_auto_flush_count = 0;
                            Program.seq.semi_auto_tank_a.semi_auto_run_diw_flush_count = 0;
                            Program.seq.semi_auto_tank_a.semi_auto_run_chemical_flush_count = 0;
                            Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, false);
                            Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.TANK_EMPTY_CHECK1_ONLY_ONCE, "");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.NONE;
                            Program.seq.semi_auto_tank_b.semi_auto_complete = false;
                            Program.seq.semi_auto_tank_b.semi_auto_run_count = 0;
                            Program.seq.semi_auto_tank_b.semi_auto_run_auto_flush_count = 0;
                            Program.seq.semi_auto_tank_b.semi_auto_run_diw_flush_count = 0;
                            Program.seq.semi_auto_tank_b.semi_auto_run_chemical_flush_count = 0;
                            Tank_Value_Clear(tank_class.enum_tank_type.TANK_B, false);
                            Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.TANK_EMPTY_CHECK1_ONLY_ONCE, "");
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_EMPTY_CHECK1_ONLY_ONCE:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            //LS-41
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                            {
                                //Empty일때
                                Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.TANK_DRAIN_END_ONLY_ONCE, "");
                            }
                            else
                            {
                                //Empty가 아닐 때
                                Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.TANK_DRAIN_START_2_ONLY_ONCE, "");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            //LS-41
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                            {
                                //Empty일때
                                Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.TANK_DRAIN_END_ONLY_ONCE, "");
                            }
                            else
                            {
                                //Empty가 아닐 때
                                Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.TANK_DRAIN_START_2_ONLY_ONCE, "");
                            }
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_DRAIN_START_2_ONLY_ONCE:

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            //TANK A 상태 Drain으로 변경 + Drain Valve ON AV-45
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.DRAIN;
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_drain = DateTime.Now;
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].use_drain_seq_by_semiauto = true;
                            //Cir Drain OR Tank Drain 판단은 Sequence에서 진행 Auto + Drain Status면 Drain 진행 여부 판단
                            //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, true);
                            Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.TANK_EMPTY_CHECK2_ONLY_ONCE, "WAIT LS-41");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            //TANK B 상태 Drain으로 변경 + Drain Valve ON AV-46
                            //Cir Drain OR Tank Drain 판단은 Sequence에서 진행 Auto + Drain Status면 Drain 진행 여부 판단
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.DRAIN;
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_drain = DateTime.Now;
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].use_drain_seq_by_semiauto = true;
                            //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, true);
                            Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.TANK_EMPTY_CHECK2_ONLY_ONCE, "WAIT LS-42");
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_EMPTY_CHECK2_ONLY_ONCE:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            //LS-41
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                            {
                                //Empty일때
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.DRAIN_WAIT;
                                Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.TANK_DRAIN_DELAY_BEFORE_END_ONLY_ONCE, "");
                            }
                            else
                            {
                                if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_drain).Seconds >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Drain_Time_Out))
                                {
                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Drain_Time_Over_Tank_A, "", true, false);
                                }
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            //LS-42
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                            {
                                //Empty일때
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.DRAIN_WAIT;
                                Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.TANK_DRAIN_DELAY_BEFORE_END_ONLY_ONCE, "");
                            }
                            else
                            {
                                if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_drain).Seconds >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Drain_Time_Out))
                                {
                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Drain_Time_Over_Tank_B, "", true, false);
                                }
                            }
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_DRAIN_DELAY_BEFORE_END_ONLY_ONCE:
                        tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Drain_Valve_Off_Time_Delay_Tank_Circulation);
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 500)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.TANK_DRAIN_END_ONLY_ONCE, "Delay " + tmp_para_value.ToString() + " Sec");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= 500)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.TANK_DRAIN_END_ONLY_ONCE, "Delay " + tmp_para_value.ToString() + " Sec");
                            }
                        }
                        break;
                    case tank_class.enum_seq_no_semi_auto.TANK_DRAIN_END_ONLY_ONCE:

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalSeconds >= (tmp_para_value))
                            {
                                //TANK A Drain Valve OFF AV-45
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.NONE;

                                TotalUsage_Reset(Program.tank[(int)tank_class.enum_tank_type.TANK_A]);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, false);
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == false)
                                {
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, false);
                                    CIRCULATION_PUMP_ON_OFF(false);
                                }

                                //DRAIN Semi-Auto 인경우 종료
                                if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DRAIN)
                                {
                                    if (timer_stop_after_complete == true)
                                    {
                                        Program.seq.semi_auto_tank_a.semi_auto_complete = true;
                                        Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE, "");
                                    }
                                }
                                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                                {
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT_AUTO_FLUSH, "");
                                }
                                else
                                {
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                                }

                            }
                            else
                            {
                                Program.seq.semi_auto_tank_a.memo_current = Convert.ToInt32(Program.seq.semi_auto_tank_a.last_act_span.TotalSeconds) + " / " + tmp_para_value.ToString() + " Sec";
                            }

                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalSeconds >= (tmp_para_value))
                            {
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.NONE;
                                //Tank 적산 초기화
                                TotalUsage_Reset(Program.tank[(int)tank_class.enum_tank_type.TANK_B]);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, false);
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == false)
                                {
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, false);
                                    CIRCULATION_PUMP_ON_OFF(false);
                                }

                                //DRAIN Semi-Auto 인경우 종료
                                if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DRAIN)
                                {
                                    if (timer_stop_after_complete == true)
                                    {
                                        Program.seq.semi_auto_tank_b.semi_auto_complete = true;
                                        Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE, "");
                                    }
                                }
                                else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                                {
                                    Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT_AUTO_FLUSH, "");
                                }
                                else
                                {
                                    Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                                }

                            }
                            else
                            {
                                Program.seq.semi_auto_tank_b.memo_current = Convert.ToInt32(Program.seq.semi_auto_tank_b.last_act_span.TotalSeconds) + " / " + tmp_para_value.ToString() + " Sec";
                            }
                        }
                        break;


                    case tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT_AUTO_FLUSH_READY:

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Program.seq.semi_auto_tank_a.semi_auto_run_count = 0;
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Program.seq.semi_auto_tank_b.semi_auto_run_count = 0;
                        }
                        break;


                    case tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT_AUTO_FLUSH:
                        //AUTO FLUSH만 진입
                        tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Auto_Flush_Count);

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.semi_auto_run_auto_flush_count >= tmp_para_value)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE, "");
                            }
                            else
                            {
                                if (Program.seq.semi_auto_tank_a.semi_auto_run_diw_flush_count >=
                                    Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count))
                                {
                                    if (Program.seq.semi_auto_tank_a.semi_auto_run_chemical_flush_count == 0)
                                    {
                                        Program.seq.semi_auto_tank_a.semi_auto_run_count = 0;
                                        Program.seq.semi_auto_tank_a.auto_flush_current_type = tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY;
                                        Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                                    }
                                    else if (Program.seq.semi_auto_tank_a.semi_auto_run_chemical_flush_count >=
                                    Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count))
                                    {
                                        Program.seq.semi_auto_tank_a.semi_auto_run_auto_flush_count = Program.seq.semi_auto_tank_a.semi_auto_run_auto_flush_count + 1;
                                        if (Program.seq.semi_auto_tank_a.semi_auto_run_auto_flush_count >= tmp_para_value)
                                        {
                                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE, "");
                                        }
                                        else
                                        {
                                            Program.seq.semi_auto_tank_a.semi_auto_run_count = 0;
                                            Program.seq.semi_auto_tank_a.semi_auto_run_diw_flush_count = 0;
                                            Program.seq.semi_auto_tank_a.semi_auto_run_chemical_flush_count = 0;
                                            Program.seq.semi_auto_tank_a.auto_flush_current_type = tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY;
                                            Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                                        }
                                    }
                                    else
                                    {

                                        Program.seq.semi_auto_tank_a.auto_flush_current_type = tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY;
                                        Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                                    }
                                }
                                else
                                {
                                    Program.seq.semi_auto_tank_a.auto_flush_current_type = tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY;
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                                }

                            }
                        }

                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.semi_auto_run_auto_flush_count >= tmp_para_value)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE, "");
                            }
                            else
                            {
                                if (Program.seq.semi_auto_tank_b.semi_auto_run_diw_flush_count >=
                                    Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count))
                                {
                                    if (Program.seq.semi_auto_tank_b.semi_auto_run_chemical_flush_count == 0)
                                    {
                                        Program.seq.semi_auto_tank_b.semi_auto_run_count = 0;
                                        Program.seq.semi_auto_tank_b.auto_flush_current_type = tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY;
                                        Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                                    }
                                    else if (Program.seq.semi_auto_tank_b.semi_auto_run_chemical_flush_count >=
                                    Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count))
                                    {
                                        Program.seq.semi_auto_tank_b.semi_auto_run_auto_flush_count = Program.seq.semi_auto_tank_b.semi_auto_run_auto_flush_count + 1;
                                        if (Program.seq.semi_auto_tank_b.semi_auto_run_auto_flush_count >= tmp_para_value)
                                        {
                                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE, "");
                                        }
                                        else
                                        {
                                            Program.seq.semi_auto_tank_b.semi_auto_run_count = 0;
                                            Program.seq.semi_auto_tank_b.semi_auto_run_diw_flush_count = 0;
                                            Program.seq.semi_auto_tank_b.semi_auto_run_chemical_flush_count = 0;
                                            Program.seq.semi_auto_tank_b.auto_flush_current_type = tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY;
                                            Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                                        }
                                    }
                                    else
                                    {
                                        Program.seq.semi_auto_tank_b.auto_flush_current_type = tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY;
                                        Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                                    }
                                }
                                else
                                {
                                    Program.seq.semi_auto_tank_b.auto_flush_current_type = tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY;
                                    Seq_Semi_Auto_Tank_B_Cur_To_Next((Program.seq.semi_auto_tank_b.no_cur), tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                                }

                            }
                        }

                        break;


                    case tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT:


                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            //semi_auto_type 적용은 Flush만, Flush And Supply는 상위 run_Count_auto_flush에서 진행
                            if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH)
                            {
                                Program.seq.semi_auto_tank_a.auto_flush_current_type = Program.seq.semi_auto_tank_a.semi_auto_type;
                                tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count);
                                //Flush And Supply Count는 End 시점에서 +
                                Program.seq.semi_auto_tank_a.semi_auto_run_diw_flush_count = Program.seq.semi_auto_tank_a.semi_auto_run_diw_flush_count + 1;
                            }
                            else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                            {
                                tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count);
                            }
                            else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH)
                            {
                                Program.seq.semi_auto_tank_a.auto_flush_current_type = Program.seq.semi_auto_tank_a.semi_auto_type;
                                tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count);
                                //Flush And Supply Count는 End 시점에서 +
                                Program.seq.semi_auto_tank_a.semi_auto_run_chemical_flush_count = Program.seq.semi_auto_tank_a.semi_auto_run_chemical_flush_count + 1;
                            }
                            else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                            {
                                tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count);
                            }
                            else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                            {

                                if (Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count);
                                }
                                else if (Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count);
                                }
                            }



                            if (Program.seq.semi_auto_tank_a.semi_auto_run_count >= tmp_para_value)
                            {
                                if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                                {
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT_AUTO_FLUSH, "");
                                }
                                else
                                {
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE, "");
                                }
                            }
                            else
                            {
                                Program.seq.semi_auto_tank_a.cur_mixing_index = -1;
                                Program.mixing_step_form.Setting_Mixing_Order(tank_class.enum_tank_type.TANK_A);
                                Program.seq.semi_auto_tank_a.input_request = true;
                                Program.seq.semi_auto_tank_a.cur_sametime_input_count = 0;
                                Tank_CCSS_Input_Complete_Flag_Clear(call_selected_tank, Program.seq.semi_auto_tank_a.mixing_order);
                                if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH ||
                                    Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                {
                                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                                    {
                                        Program.seq.semi_auto_tank_a.hdiw_check_start = DateTime.Now;
                                        Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_DIW, "");
                                    }
                                    else
                                    {
                                        Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_REQ, "");
                                    }

                                }
                                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH ||
                                    Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                {
                                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                                    {
                                        Program.seq.semi_auto_tank_a.hdiw_check_start = DateTime.Now;
                                        Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_CHEMICAL, "");
                                    }
                                    else
                                    {
                                        Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_REQ, "");
                                    }
                                }
                                else
                                {
                                    if (Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                    {
                                        if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                                        {
                                            Program.seq.semi_auto_tank_a.hdiw_check_start = DateTime.Now;
                                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_DIW, "");
                                        }
                                        else
                                        {
                                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_REQ, "");
                                        }
                                    }
                                    else if (Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                    {
                                        if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                                        {
                                            Program.seq.semi_auto_tank_a.hdiw_check_start = DateTime.Now;
                                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_CHEMICAL, "");
                                        }
                                        else
                                        {
                                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_REQ, "");
                                        }
                                    }
                                }

                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            //semi_auto_type 적용은 Flush만, Flush And Supply는 상위 run_Count_auto_flush에서 진행
                            if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH)
                            {
                                Program.seq.semi_auto_tank_b.auto_flush_current_type = Program.seq.semi_auto_tank_b.semi_auto_type;
                                tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count);
                                //Flush And Supply Count는 End 시점에서 +
                                Program.seq.semi_auto_tank_b.semi_auto_run_diw_flush_count = Program.seq.semi_auto_tank_b.semi_auto_run_diw_flush_count + 1;
                            }
                            else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                            {
                                tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count);
                            }
                            else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH)
                            {
                                Program.seq.semi_auto_tank_b.auto_flush_current_type = Program.seq.semi_auto_tank_b.semi_auto_type;
                                tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count);
                                //Flush And Supply Count는 End 시점에서 +
                                Program.seq.semi_auto_tank_b.semi_auto_run_chemical_flush_count = Program.seq.semi_auto_tank_b.semi_auto_run_chemical_flush_count + 1;
                            }
                            else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                            {
                                tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count);
                            }
                            else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                            {
                                if (Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Count);
                                }
                                else if (Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Count);
                                }
                            }

                            if (Program.seq.semi_auto_tank_b.semi_auto_run_count >= tmp_para_value)
                            {
                                if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                                {

                                    Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT_AUTO_FLUSH, "");
                                }
                                else
                                {
                                    Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE, "");
                                }
                            }
                            else
                            {
                                Program.seq.semi_auto_tank_b.cur_mixing_index = -1;
                                Program.mixing_step_form.Setting_Mixing_Order(tank_class.enum_tank_type.TANK_B);
                                Tank_CCSS_Input_Complete_Flag_Clear(call_selected_tank, Program.seq.semi_auto_tank_b.mixing_order);
                                Program.seq.semi_auto_tank_b.input_request = true;
                                Program.seq.semi_auto_tank_b.cur_sametime_input_count = 0;
                                if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH ||
                                    Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                {
                                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                                    {
                                        Program.seq.semi_auto_tank_b.hdiw_check_start = DateTime.Now;
                                        Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_DIW, "");
                                    }
                                    else
                                    {
                                        Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_REQ, "");
                                    }
                                }
                                else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH ||
                                    Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                {
                                    if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                                    {
                                        Program.seq.semi_auto_tank_b.hdiw_check_start = DateTime.Now;
                                        Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_CHEMICAL, "");
                                    }
                                    else
                                    {
                                        Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_REQ, "");
                                    }
                                }
                                else
                                {
                                    if (Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                    {
                                        if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                                        {
                                            Program.seq.semi_auto_tank_b.hdiw_check_start = DateTime.Now;
                                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_DIW, "");
                                        }
                                        else
                                        {
                                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_REQ, "");
                                        }

                                    }
                                    else if (Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                    {
                                        if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                                        {
                                            Program.seq.semi_auto_tank_b.hdiw_check_start = DateTime.Now;
                                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_CHEMICAL, "");
                                        }
                                        else
                                        {
                                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_REQ, "");
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_DIW:

                        //TS-09 온도 값 확인
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 100)
                            {
                                tmp_para_value = (int)(DateTime.Now - Program.seq.semi_auto_tank_a.hdiw_check_start).TotalSeconds;
                                if (HDIW_Temp_Check() == true)
                                {
                                    if (tmp_para_value >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.HDIW_Temp_Delay_Time))
                                    {
                                        Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_OK_BY_DIW, "");
                                    }
                                }
                                else
                                {
                                    Program.seq.semi_auto_tank_a.hdiw_check_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_DIW, "TEMP OK And Wait Delay " + tmp_para_value + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.HDIW_Temp_Delay_Time));
                                }

                            }

                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= 100)
                            {
                                tmp_para_value = (int)(DateTime.Now - Program.seq.semi_auto_tank_b.hdiw_check_start).TotalSeconds;
                                if (HDIW_Temp_Check() == true)
                                {
                                    if (tmp_para_value >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.HDIW_Temp_Delay_Time))
                                    {
                                        Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_OK_BY_DIW, "");
                                    }
                                }
                                else
                                {
                                    Program.seq.semi_auto_tank_b.hdiw_check_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_DIW, "TEMP OK And Wait Delay " + tmp_para_value + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.HDIW_Temp_Delay_Time));
                                }
                            }
                        }

                        break;

                    case tank_class.enum_seq_no_semi_auto.HDIW_TEMP_OK_BY_DIW:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_REQ, "");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_REQ, "");
                        }
                        break;

                    ///DIW INPUT
                    case tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_REQ:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.CHARGE;
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 500)
                            {
                                CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS4);
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_WAIT, "");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= 500)
                            {
                                CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS4);
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_WAIT, "");
                            }
                        }
                        break;




                    case tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_WAIT:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (CCSS_INPUT_END_BY_LEVEL_H(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS4) == true)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_COMPLETE, "");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (CCSS_INPUT_END_BY_LEVEL_H(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS4) == true)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_COMPLETE, "");
                            }
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_DIW_INPUT_COMPLETE:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY ||
                                Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START, "");
                            }
                            else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START, "");
                            }
                            else
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DRAIN_START, "");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY ||
                               Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START, ""); ;
                            }
                            else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START, "");
                            }
                            else
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DRAIN_START, "");
                            }
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_CHEMICAL:

                        //TS-09 온도 값 확인
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 100)
                            {
                                tmp_para_value = (int)(DateTime.Now - Program.seq.semi_auto_tank_a.hdiw_check_start).TotalSeconds;
                                if (HDIW_Temp_Check() == true)
                                {
                                    if (tmp_para_value >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.HDIW_Temp_Delay_Time))
                                    {
                                        Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_OK_BY_CHEMICAL, "");
                                    }
                                }
                                else
                                {
                                    Program.seq.semi_auto_tank_a.hdiw_check_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_CHEMICAL, "TEMP OK And Wait Delay " + tmp_para_value + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.HDIW_Temp_Delay_Time));
                                }

                            }

                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= 100)
                            {
                                tmp_para_value = (int)(DateTime.Now - Program.seq.semi_auto_tank_b.hdiw_check_start).TotalSeconds;
                                if (HDIW_Temp_Check() == true)
                                {
                                    if (tmp_para_value >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.HDIW_Temp_Delay_Time))
                                    {
                                        Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_OK_BY_CHEMICAL, "");
                                    }
                                }
                                else
                                {
                                    Program.seq.semi_auto_tank_b.hdiw_check_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.HDIW_TEMP_MONITORING_BY_CHEMICAL, "TEMP OK And Wait Delay " + tmp_para_value + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.HDIW_Temp_Delay_Time));
                                }
                            }
                        }

                        break;

                    case tank_class.enum_seq_no_semi_auto.HDIW_TEMP_OK_BY_CHEMICAL:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_REQ, "");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_REQ, "");
                        }
                        break;
                    ///CCSS INPUT
                    case tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_REQ:

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.CHARGE;
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.CHARGE;
                        }

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.input_request == true)
                            {
                                Program.seq.semi_auto_tank_a.input_request = false;
                                //Chemical Input CCSS별로 진행
                                //mixing order deque
                                if (Program.seq.semi_auto_tank_a.mixing_order.Count > 0 && Program.seq.semi_auto_tank_a.cur_sametime_input_count == 0)
                                {
                                    Program.seq.semi_auto_tank_a.cur_mixing = Program.seq.semi_auto_tank_a.mixing_order.Dequeue();
                                    Program.seq.semi_auto_tank_a.cur_mixing_index = Program.seq.semi_auto_tank_a.cur_mixing_index + 1;
                                    Program.seq.semi_auto_tank_a.cur_sametime_input_count = Program.seq.semi_auto_tank_a.cur_sametime_input_count + 1;
                                    CCSS_INPUT_Check(Program.seq.semi_auto_tank_a.cur_mixing.type, call_selected_tank);
                                }
                            }
                            else
                            {
                                //동시 투입 여부 결정
                                if (Program.seq.semi_auto_tank_a.mixing_order_list.Count - 1 == Program.seq.semi_auto_tank_a.cur_mixing_index)
                                {
                                    //마지막 순서일때는 무시한다.
                                }
                                else
                                {
                                    if (Program.seq.semi_auto_tank_a.mixing_order_list[Program.seq.semi_auto_tank_a.cur_mixing_index].ccss_row == Program.seq.semi_auto_tank_a.mixing_order_list[Program.seq.semi_auto_tank_a.cur_mixing_index + 1].ccss_row)
                                    {
                                        //다음 Step도 같은 행일 때 동시 투입한다.
                                        Program.seq.semi_auto_tank_a.cur_mixing = Program.seq.semi_auto_tank_a.mixing_order.Dequeue();
                                        Program.seq.semi_auto_tank_a.cur_mixing_index = Program.seq.semi_auto_tank_a.cur_mixing_index + 1;
                                        Program.seq.semi_auto_tank_a.cur_sametime_input_count = Program.seq.semi_auto_tank_a.cur_sametime_input_count + 1;
                                        CCSS_INPUT_Check(Program.seq.semi_auto_tank_a.cur_mixing.type, call_selected_tank);
                                    }
                                }
                            }

                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.input_request == true)
                            {
                                Program.seq.semi_auto_tank_b.input_request = false;
                                //Chemical Input CCSS별로 진행
                                //mixing order deque
                                if (Program.seq.semi_auto_tank_b.mixing_order.Count > 0 && Program.seq.semi_auto_tank_b.cur_sametime_input_count == 0)
                                {
                                    Program.seq.semi_auto_tank_b.cur_mixing = Program.seq.semi_auto_tank_b.mixing_order.Dequeue();
                                    Program.seq.semi_auto_tank_b.cur_mixing_index = Program.seq.semi_auto_tank_b.cur_mixing_index + 1;
                                    Program.seq.semi_auto_tank_b.cur_sametime_input_count = Program.seq.semi_auto_tank_b.cur_sametime_input_count + 1;
                                    CCSS_INPUT_Check(Program.seq.semi_auto_tank_b.cur_mixing.type, call_selected_tank);
                                }
                            }
                            else
                            {
                                //동시 투입 여부 결정
                                if (Program.seq.semi_auto_tank_b.mixing_order_list.Count - 1 == Program.seq.semi_auto_tank_b.cur_mixing_index)
                                {
                                    //마지막 순서일때는 무시한다.
                                }
                                else
                                {
                                    if (Program.seq.semi_auto_tank_b.mixing_order_list[Program.seq.semi_auto_tank_b.cur_mixing_index].ccss_row == Program.seq.semi_auto_tank_b.mixing_order_list[Program.seq.semi_auto_tank_b.cur_mixing_index + 1].ccss_row)
                                    {
                                        //다음 Step도 같은 행일 때 동시 투입한다.
                                        Program.seq.semi_auto_tank_b.cur_mixing = Program.seq.semi_auto_tank_b.mixing_order.Dequeue();
                                        Program.seq.semi_auto_tank_b.cur_mixing_index = Program.seq.semi_auto_tank_b.cur_mixing_index + 1;
                                        Program.seq.semi_auto_tank_b.cur_sametime_input_count = Program.seq.semi_auto_tank_b.cur_sametime_input_count + 1;
                                        CCSS_INPUT_Check(Program.seq.semi_auto_tank_b.cur_mixing.type, call_selected_tank);
                                    }
                                }
                            }

                        }
                        ///////////투입 체크
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(call_selected_tank, enum_ccss.CCSS1) == true)
                            {
                                Program.seq.semi_auto_tank_a.input_request = true; if (Program.seq.semi_auto_tank_a.cur_sametime_input_count > 0) { Program.seq.semi_auto_tank_a.cur_sametime_input_count = Program.seq.semi_auto_tank_a.cur_sametime_input_count - 1; }
                            }
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(call_selected_tank, enum_ccss.CCSS2) == true)
                            {
                                Program.seq.semi_auto_tank_a.input_request = true; if (Program.seq.semi_auto_tank_a.cur_sametime_input_count > 0) { Program.seq.semi_auto_tank_a.cur_sametime_input_count = Program.seq.semi_auto_tank_a.cur_sametime_input_count - 1; }
                            }
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(call_selected_tank, enum_ccss.CCSS3) == true)
                            {
                                Program.seq.semi_auto_tank_a.input_request = true; if (Program.seq.semi_auto_tank_a.cur_sametime_input_count > 0) { Program.seq.semi_auto_tank_a.cur_sametime_input_count = Program.seq.semi_auto_tank_a.cur_sametime_input_count - 1; }
                            }
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(call_selected_tank, enum_ccss.CCSS4) == true)
                            {
                                Program.seq.semi_auto_tank_a.input_request = true; if (Program.seq.semi_auto_tank_a.cur_sametime_input_count > 0) { Program.seq.semi_auto_tank_a.cur_sametime_input_count = Program.seq.semi_auto_tank_a.cur_sametime_input_count - 1; }
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(call_selected_tank, enum_ccss.CCSS1) == true)
                            {
                                Program.seq.semi_auto_tank_b.input_request = true; if (Program.seq.semi_auto_tank_b.cur_sametime_input_count > 0) { Program.seq.semi_auto_tank_b.cur_sametime_input_count = Program.seq.semi_auto_tank_b.cur_sametime_input_count - 1; }
                            }
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(call_selected_tank, enum_ccss.CCSS2) == true)
                            {
                                Program.seq.semi_auto_tank_b.input_request = true; if (Program.seq.semi_auto_tank_b.cur_sametime_input_count > 0) { Program.seq.semi_auto_tank_b.cur_sametime_input_count = Program.seq.semi_auto_tank_b.cur_sametime_input_count - 1; }
                            }
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(call_selected_tank, enum_ccss.CCSS3) == true)
                            {
                                Program.seq.semi_auto_tank_b.input_request = true; if (Program.seq.semi_auto_tank_b.cur_sametime_input_count > 0) { Program.seq.semi_auto_tank_b.cur_sametime_input_count = Program.seq.semi_auto_tank_b.cur_sametime_input_count - 1; }
                            }
                            if (Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(call_selected_tank, enum_ccss.CCSS4) == true)
                            {
                                Program.seq.semi_auto_tank_b.input_request = true; if (Program.seq.semi_auto_tank_b.cur_sametime_input_count > 0) { Program.seq.semi_auto_tank_b.cur_sametime_input_count = Program.seq.semi_auto_tank_b.cur_sametime_input_count - 1; }
                            }

                        }



                        //각 CCSS별로 Input + CIRCULATION ON 모두 완료되었을 때 
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Program.seq.semi_auto_tank_a.memo_current = call_selected_tank.ToString() + " Input Status / CCSS1 : " + Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete
                                + " / CCSS2 : " + Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete
                                + " / CCSS3 : " + Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete
                                + " / CCSS4 : " + Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete;
                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete == true &&
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete == true &&
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete == true &&
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete == true)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_COMPLETE, "");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Program.seq.semi_auto_tank_b.memo_current = call_selected_tank.ToString() + " Input Status / CCSS1 : " + Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete
                             + " / CCSS2 : " + Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete
                             + " / CCSS3 : " + Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete
                             + " / CCSS4 : " + Program.tank[(int)call_selected_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete;
                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete == true &&
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete == true &&
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete == true &&
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete == true)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_COMPLETE, "");
                            }
                        }
                        //CCSS 투입 중 HDIW 온도 이상 발생 시 Alarm 발생
                        if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                        {
                            if (HDIW_Temp_Check() == false)
                            {
                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.HDIW_Supply_Temp_Error, "", true, false);
                            }
                        }

                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_MONITORING_LEVEL:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == true)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_COMPLETE, "");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == true)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_COMPLETE, "");
                            }
                        }
                        break;



                    case tank_class.enum_seq_no_semi_auto.TANK_CHEMICAL_INPUT_COMPLETE:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DRAIN_START, "");
                            }
                            else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DRAIN_START, "");
                            }
                            if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START, "");
                            }
                            else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START, "");
                            }
                            else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START, "");
                            }

                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DRAIN_START, "");
                            }
                            else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DRAIN_START, "");
                            }
                            if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START, "");
                            }
                            else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START, "");
                            }
                            else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START, "");
                            }
                        }
                        break;

                    ///DRAIN
                    case tank_class.enum_seq_no_semi_auto.TANK_DRAIN_START:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.DRAIN;
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].use_drain_seq_by_semiauto = true;


                            //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, true);
                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_EMPTY_CHECK, "");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.DRAIN;
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].use_drain_seq_by_semiauto = true;
                            //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, true);
                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_EMPTY_CHECK, "");
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_EMPTY_CHECK:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                            {
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.DRAIN_WAIT;
                                //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, false);
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DRAIN_DELAY_BEFORE_END, "");
                            }

                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                            {
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.DRAIN_WAIT;
                                //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, false);
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DRAIN_DELAY_BEFORE_END, "");
                            }
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_DRAIN_DELAY_BEFORE_END:
                        tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Drain_Valve_Off_Time_Delay_Tank_Circulation);

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DRAIN_END, "");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_DRAIN_END, "");
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_DRAIN_END:

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {

                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= (tmp_para_value * 1000))
                            {
                                //Tank 적산 초기화
                                TotalUsage_Reset(Program.tank[(int)tank_class.enum_tank_type.TANK_A]);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, false);
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == false)
                                {
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, false);
                                    CIRCULATION_PUMP_ON_OFF(false);
                                }
                                Program.seq.semi_auto_tank_a.semi_auto_run_count = Program.seq.semi_auto_tank_a.semi_auto_run_count + 1;
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                            }
                            else
                            {
                                Program.seq.semi_auto_tank_a.memo_current = Convert.ToInt32(Program.seq.semi_auto_tank_a.last_act_span.TotalSeconds) + " / " + tmp_para_value.ToString() + " Sec";
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= (tmp_para_value * 1000))
                            {
                                //Tank 적산 초기화
                                TotalUsage_Reset(Program.tank[(int)tank_class.enum_tank_type.TANK_B]);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, false);
                                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == false)
                                {
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, false);
                                    CIRCULATION_PUMP_ON_OFF(false);
                                }
                                Program.seq.semi_auto_tank_b.semi_auto_run_count = Program.seq.semi_auto_tank_b.semi_auto_run_count + 1;
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                            }
                            else
                            {
                                Program.seq.semi_auto_tank_b.memo_current = Convert.ToInt32(Program.seq.semi_auto_tank_b.last_act_span.TotalSeconds) + " / " + tmp_para_value.ToString() + " Sec";
                            }
                        }
                        break;

                    ///SUPPLY
                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_START:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_VALVE_ON, "");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_VALVE_ON, "");
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_VALVE_ON:
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, true);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, true);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, true);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, true);
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, true);
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, true);
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, true);
                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_PUMP_ON, "");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_B, true);
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, true);
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, true);
                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_PUMP_ON, "");
                        }

                        break;




                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_PUMP_ON:


                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 500)
                            {
                                SUPPLY_A_PUMP_ON_OFF(true);
                                SUPPLY_B_PUMP_ON_OFF(true);
                                Program.seq.semi_auto_tank_a.dt_supply_start = DateTime.Now;
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.SUPPLY;
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_WAIT, "");
                            }

                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= 500)
                            {
                                SUPPLY_A_PUMP_ON_OFF(true);
                                SUPPLY_B_PUMP_ON_OFF(true);
                                Program.seq.semi_auto_tank_b.dt_supply_start = DateTime.Now;
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.SUPPLY;
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_WAIT, "");
                            }
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_WAIT:


                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 200)
                            {
                                if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Supply_Time);
                                    Program.seq.semi_auto_tank_a.dt_supply_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END, "");
                                }
                                else if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Supply_Time);
                                    Program.seq.semi_auto_tank_a.dt_supply_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END, "");
                                }
                                else if (Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Supply_Time);
                                    Program.seq.semi_auto_tank_a.dt_supply_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END, "");
                                }
                                else if (Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Supply_Time);
                                    Program.seq.semi_auto_tank_a.dt_supply_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END, "");
                                }
                            }
                        }

                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= 200)
                            {
                                if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Supply_Time);
                                    Program.seq.semi_auto_tank_b.dt_supply_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END, "");
                                }
                                else if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Supply_Time);
                                    Program.seq.semi_auto_tank_b.dt_supply_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END, "");
                                }
                                else if (Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_DIW_Flush_Supply_Time);
                                    Program.seq.semi_auto_tank_b.dt_supply_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END, "");
                                }
                                else if (Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                {
                                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Chemical_Flush_Supply_Time);
                                    Program.seq.semi_auto_tank_b.dt_supply_start = DateTime.Now;
                                    Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END, "");
                                }
                            }
                        }


                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END:

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            tmp_para_value_1 = (int)(DateTime.Now - Program.seq.semi_auto_tank_a.dt_supply_start).TotalSeconds;
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= tmp_para_value * 1000)
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_START, "");
                            }
                            else
                            {
                                //Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END, "" + tmp_para_value + " Sec / " + tmp_para_value_1 + " Sec");
                            }

                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            tmp_para_value_1 = (int)(DateTime.Now - Program.seq.semi_auto_tank_b.dt_supply_start).TotalSeconds;
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= tmp_para_value * 1000)
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_START, "");
                            }
                            else
                            {
                                //Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DELAY_END, "" + tmp_para_value + " Sec / " + tmp_para_value_1 + " Sec");
                            }

                        }


                        break;


                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_START:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 500)
                            {
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.DRAIN;
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].use_drain_seq_by_semiauto = false;
                                //SUPPLY Drain 시작
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_DRAIN, true);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);

                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);

                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.DRAIN;
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_WAIT, "");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= 500)
                            {
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.DRAIN;
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].use_drain_seq_by_semiauto = false;
                                //SUPPLY Drain 시작
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_DRAIN, true);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);

                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);

                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.DRAIN;
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_WAIT, "");
                            }
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_WAIT:
                        //TANK Empty 대기
                        tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Drain_Valve_Off_Time_Delay_Tank_Circulation);
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                            {
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.DRAIN_WAIT;
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_BEFORE_END, "");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                            {
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.DRAIN_WAIT;
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_BEFORE_END, "");
                            }
                        }

                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_BEFORE_END:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= (tmp_para_value * 1000))
                            {
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_END, "");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= (tmp_para_value * 1000))
                            {
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_END, "");

                            }

                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_DRAIN_END:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                            {
                                //적산 초기화
                                TotalUsage_Reset(Program.tank[(int)tank_class.enum_tank_type.TANK_A]);

                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_PUMP_OFF, "");
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                            {
                                //적산 초기화
                                TotalUsage_Reset(Program.tank[(int)tank_class.enum_tank_type.TANK_B]);
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_PUMP_OFF, "");
                            }
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_PUMP_OFF:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            SUPPLY_A_PUMP_ON_OFF(false);
                            SUPPLY_B_PUMP_ON_OFF(false);
                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_VALVE_OFF, "");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            SUPPLY_A_PUMP_ON_OFF(false);
                            SUPPLY_B_PUMP_ON_OFF(false);
                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_VALVE_OFF, "");
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_VALVE_OFF:

                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 500)
                            {
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_DRAIN, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);

                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                                Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_END, "");
                            }


                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (Program.seq.semi_auto_tank_b.last_act_span.TotalMilliseconds >= 500)
                            {
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_DRAIN, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);

                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_B, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, false);
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);
                                Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_END, "");
                            }

                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.TANK_FLUSH_SUPPLY_END:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {

                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.NONE;
                            if (Program.seq.semi_auto_tank_a.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                            {
                                if (Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                {
                                    Program.seq.semi_auto_tank_a.semi_auto_run_diw_flush_count = Program.seq.semi_auto_tank_a.semi_auto_run_diw_flush_count + 1;
                                }
                                else if (Program.seq.semi_auto_tank_a.auto_flush_current_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                {
                                    Program.seq.semi_auto_tank_a.semi_auto_run_chemical_flush_count = Program.seq.semi_auto_tank_a.semi_auto_run_chemical_flush_count + 1;
                                }
                            }
                            Program.seq.semi_auto_tank_a.semi_auto_run_count = Program.seq.semi_auto_tank_a.semi_auto_run_count + 1;
                            Seq_Semi_Auto_Tank_A_Cur_To_Next(Program.seq.semi_auto_tank_a.no_cur, tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.NONE;
                            if (Program.seq.semi_auto_tank_b.semi_auto_type == tank_class.enum_semi_auto.AUTO_FLUSH)
                            {
                                if (Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.DIW_FLUSH_AND_SUPPLY)
                                {
                                    Program.seq.semi_auto_tank_b.semi_auto_run_diw_flush_count = Program.seq.semi_auto_tank_b.semi_auto_run_diw_flush_count + 1;
                                }
                                else if (Program.seq.semi_auto_tank_b.auto_flush_current_type == tank_class.enum_semi_auto.CHEMICAL_FLUSH_AND_SUPPLY)
                                {
                                    Program.seq.semi_auto_tank_b.semi_auto_run_chemical_flush_count = Program.seq.semi_auto_tank_b.semi_auto_run_chemical_flush_count + 1;
                                }
                            }
                            Program.seq.semi_auto_tank_b.semi_auto_run_count = Program.seq.semi_auto_tank_b.semi_auto_run_count + 1;
                            Seq_Semi_Auto_Tank_B_Cur_To_Next(Program.seq.semi_auto_tank_b.no_cur, tank_class.enum_seq_no_semi_auto.MONITORING_RUN_COUNT, "");
                        }
                        break;


                    //End
                    case tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {
                            if (timer_stop_after_complete == true)
                            {
                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.NONE;
                                Program.seq.semi_auto_tank_a.semi_auto_complete = true;
                                timer_manual_sequence_tank_a.Enabled = false;
                                Program.main_md.Message_By_Application("Semi Auto - Tank A Running Complete", frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {
                            if (timer_stop_after_complete == true)
                            {
                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.NONE;
                                Program.seq.semi_auto_tank_b.semi_auto_complete = true;
                                timer_manual_sequence_tank_b.Enabled = false;
                                Program.main_md.Message_By_Application("Semi Auto - Tank B Running Complete", frm_messagebox.enum_apptype.Only_OK);
                            }
                        }
                        break;

                    case tank_class.enum_seq_no_semi_auto.ERROR_BY_ALARM:
                        if (call_selected_tank == tank_class.enum_tank_type.TANK_A)
                        {

                        }
                        else if (call_selected_tank == tank_class.enum_tank_type.TANK_B)
                        {

                        }
                        break;
                }

                //Sequnce Sub No가 변경될 때만 Seq 변경 로그 생성
                if (timer_manual_sequence_tank_a.Enabled == true)
                {
                    if (seq_no != Program.seq.semi_auto_tank_a.no_old)
                    {
                        Program.seq.semi_auto_tank_a.memo_current = "";
                        Program.log_md.LogWrite(Program.seq.semi_auto_tank_a.state_display + " : " + Program.seq.semi_auto_tank_a.memo_current, Module_Log.enumLog.SEQ_SEMI_AUTO_A, "", Module_Log.enumLevel.ALWAYS);
                    }
                    else
                    {
                        if (Program.seq.semi_auto_tank_a.memo_old != Program.seq.semi_auto_tank_a.memo_current)
                        {
                            if (Program.seq.semi_auto_tank_a.memo_current != "")
                            {
                                Program.log_md.LogWrite(Program.seq.semi_auto_tank_a.state_display + " : " + Program.seq.semi_auto_tank_a.memo_current, Module_Log.enumLog.SEQ_SEMI_AUTO_A, "", Module_Log.enumLevel.ALWAYS);
                            }
                        }
                    }
                    Program.seq.semi_auto_tank_a.no_old = seq_no;
                    Program.seq.semi_auto_tank_a.memo_old = Program.seq.semi_auto_tank_a.memo_current;

                }
                else if (timer_manual_sequence_tank_b.Enabled == true)
                {
                    if (seq_no != Program.seq.semi_auto_tank_b.no_old)
                    {
                        Program.seq.semi_auto_tank_b.memo_current = "";
                        Program.log_md.LogWrite(Program.seq.semi_auto_tank_b.state_display + " : " + Program.seq.semi_auto_tank_b.memo_current, Module_Log.enumLog.SEQ_SEMI_AUTO_B, "", Module_Log.enumLevel.ALWAYS);
                    }
                    else
                    {
                        if (Program.seq.semi_auto_tank_b.memo_old != Program.seq.semi_auto_tank_b.memo_current)
                        {
                            if (Program.seq.semi_auto_tank_b.memo_current != "")
                            {
                                Program.log_md.LogWrite(Program.seq.semi_auto_tank_b.state_display + " : " + Program.seq.semi_auto_tank_b.memo_current, Module_Log.enumLog.SEQ_SEMI_AUTO_B, "", Module_Log.enumLevel.ALWAYS);
                            }
                        }
                    }
                    Program.seq.semi_auto_tank_b.no_old = seq_no;
                    Program.seq.semi_auto_tank_b.memo_old = Program.seq.semi_auto_tank_b.memo_current;
                }

            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".Seq_Manual_Drain." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        public void Seq_Manual_Concentration_Calibration_By_LAL(tank_class.enum_tank_type call_selected_tank, tank_class.enum_seq_no_semi_auto seq_no, bool timer_stop_after_complete)
        {
            Program.seq.semi_auto_tank_a.last_act_span = DateTime.Now - Program.seq.semi_auto_tank_a.last_act_time;
            switch (seq_no)
            {
                case tank_class.enum_seq_no_semi_auto.CAL_NONE:
                    Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.CAL_INITIAL, "");
                    break;
                case tank_class.enum_seq_no_semi_auto.CAL_INITIAL:
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_FLUSHING_DIW, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);
                    Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.CAL_CM_MODE_CHANGE_SERIAL, "");
                    break;
                case tank_class.enum_seq_no_semi_auto.CAL_CM_MODE_CHANGE_SERIAL:
                    Program.CS600F.CM_SERIAL_PARALLEL_SWITCH(true, (int)enum_lal_serial_index.CONCENTRATION);
                    Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.CAL_CALIBRATION_START, "");
                    break;
                case tank_class.enum_seq_no_semi_auto.CAL_CALIBRATION_START:
                    if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 2000)
                    {
                        Program.CS600F.Message_Command_To_Byte_CS600F_TO_Send(Class_Concentration_CS600F.command_Write_CALIBRATION_START, (int)enum_lal_serial_index.CONCENTRATION);
                        Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.CAL_CALIBRATION_WAIT, "");
                    }
                    break;
                case tank_class.enum_seq_no_semi_auto.CAL_CALIBRATION_WAIT:
                    if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 1000)
                    {
                        if (Program.main_form.SerialData.CS600F.measure_status == enum_measure_status.during_background_correction_witch_is_successful_with_e07x || Program.main_form.SerialData.CS600F.measure_status == enum_measure_status.during_background_correction_witch_is_successful_without_e07x)
                        {
                            Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.CAL_CALIBRATION_END, "");
                        }
                    }
                    break;
                case tank_class.enum_seq_no_semi_auto.CAL_CALIBRATION_END:
                    if (Program.seq.semi_auto_tank_a.last_act_span.TotalMilliseconds >= 2000)
                    {
                        Program.CS600F.Message_Command_To_Byte_CS600F_TO_Send(Class_Concentration_CS600F.command_Write_CALIBRATION_END, (int)enum_lal_serial_index.CONCENTRATION);
                        Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.CAL_CM_MODE_CHANGE_PARALLEL, "");
                    }
                    break;
                case tank_class.enum_seq_no_semi_auto.CAL_CM_MODE_CHANGE_PARALLEL:
                    Program.CS600F.CM_SERIAL_PARALLEL_SWITCH(false, (int)enum_lal_serial_index.CONCENTRATION);

                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_FLUSHING_DIW, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);
                    Seq_Semi_Auto_Tank_A_Cur_To_Next((Program.seq.semi_auto_tank_a.no_cur), tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE, "");
                    break;
                //tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Drain_Valve_Off_Time_Delay_Tank_Circulation);
                //End
                case tank_class.enum_seq_no_semi_auto.SEMI_AUTO_COMPLETE:
                    if (timer_stop_after_complete == true)
                    {
                        Program.seq.semi_auto_tank_a.semi_auto_complete = true;
                        timer_manual_sequence_tank_a.Enabled = false;
                        Program.main_md.Message_By_Application("Semi Auto - Tank A Running Complete", frm_messagebox.enum_apptype.Only_OK);
                    }
                    break;

                case tank_class.enum_seq_no_semi_auto.ERROR_BY_ALARM:

                    break;
            }
            if (seq_no != Program.seq.semi_auto_tank_a.no_old)
            {
                Program.seq.semi_auto_tank_a.memo_current = "";
                Program.log_md.LogWrite(Program.seq.semi_auto_tank_a.state_display + " : " + Program.seq.semi_auto_tank_a.memo_current, Module_Log.enumLog.SEQ_SEMI_AUTO_A, "", Module_Log.enumLevel.ALWAYS);
            }
            else
            {
                if (Program.seq.semi_auto_tank_a.memo_old != Program.seq.semi_auto_tank_a.memo_current)
                {
                    if (Program.seq.semi_auto_tank_a.memo_current != "")
                    {
                        Program.log_md.LogWrite(Program.seq.semi_auto_tank_a.state_display + " : " + Program.seq.semi_auto_tank_a.memo_current, Module_Log.enumLog.SEQ_SEMI_AUTO_A, "", Module_Log.enumLevel.ALWAYS);
                    }
                }
            }
            Program.seq.semi_auto_tank_a.no_old = seq_no;
            Program.seq.semi_auto_tank_a.memo_old = Program.seq.semi_auto_tank_a.memo_current;
        }
    }
}
