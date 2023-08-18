using System;

namespace cds
{
    partial class frm_schematic
    {
        public void Seq_Main()
        {
            string result = "";
            int tmp_para_value = 0, tmp_para_value_1 = 0, tmp_para_value_2 = 0;
            while (true)
            {
                try
                {
                    if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY)
                    {

                    }
                    else
                    {
                        if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                        {
                            Program.seq.main.last_act_span = DateTime.Now - Program.seq.main.last_act_time;
                            switch (Program.seq.main.no_cur)
                            {

                                //Sequence 초기화
                                case tank_class.enum_seq_no.NONE:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.INITIALIZE, "");
                                    }
                                    break;
                                case tank_class.enum_seq_no.INITIALIZE:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        //Mixing Queue 초기화
                                        if (Program.seq.mixing_order == null) { Program.seq.mixing_order = new System.Collections.Generic.Queue<CCSS_Info>(); }
                                        //Mixxing Index 초기화
                                        Program.seq.cur_mixing_index = -1;
                                        Program.seq.mixing_order.Clear();
                                        Program.seq.main.concentration_measuring = false;
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.MODE_CHECK, "Mixing Queue Initial OK");
                                        Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString(), (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                    }
                                    break;

                                case tank_class.enum_seq_no.MODE_CHECK:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                                        {
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.AUTO, "");
                                        }
                                        else
                                        {
                                            // Manual 상태에서 Tank Drain -> Charge -> Ready 상태로 변경
                                            // Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.MANUAL, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.MANUAL:
                                    //Manual 상태일때는 Manual Exchange 버튼 클릭 +  CTC Req 대기
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        if (Program.seq.manual_exchange_ack_by_ctc == true)
                                        {
                                            ////CTC에게 REQ 전달
                                            //if (Program.seq.req_c_c_start_cds_to_ctc == false)
                                            //{
                                            //    Program.seq.req_c_c_start_cds_to_ctc = true;
                                            //    Program.seq.rep_c_c_start_cds_to_ctc = false;
                                            //}
                                            ////CTC RESPONSE 수신
                                            //if (Program.seq.rep_c_c_start_cds_to_ctc == true)
                                            //{


                                            //}
                                            //Manual 상태에서 Exchange 버튼 클릭 후 / CTC OK 수신 시 Tank Excahnge 진행
                                            Program.seq.manual_exchange_ack_by_ctc = false;
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_SELECT, "");
                                        }
                                        else
                                        {
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.MODE_CHECK, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.AUTO:
                                    //Auto 상태일때는 Exchange Sequence 바로 진입
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        //System Botting 후 항온조 및 PCW ON
                                        Thermostat_Power_ON_OFF(true);
                                        PCW_VALVE_ON_OFF(true);
                                        if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.DIW_TO_CM, true);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_FLW_V_V, true);
                                        }
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_SELECT, "Auto Mode Start");
                                    }
                                    break;

                                case tank_class.enum_seq_no.TANK_SELECT:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        //Chemical Change 순서 체크
                                        if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.NONE &&
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].enable == true)
                                        {
                                            Program.seq.main.cur_tank = tank_class.enum_tank_type.TANK_A;
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.START;
                                            Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, false);
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_MIXING_TYPE_CHECK, "Tank A Charge Start");
                                            Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> Tank A", (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        }
                                        else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.NONE &&
                                             Program.tank[(int)tank_class.enum_tank_type.TANK_B].enable == true)
                                        {
                                            Program.seq.main.cur_tank = tank_class.enum_tank_type.TANK_B;
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.START;
                                            Tank_Value_Clear(tank_class.enum_tank_type.TANK_B, false);
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_MIXING_TYPE_CHECK, "Tank B Charge Start");
                                            Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> Tank B", (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        }
                                        else
                                        {
                                            //Error
                                            Program.seq.main.memo_current = "Tank A, B cannot Run";
                                        }

                                    }
                                    break;
                                ///---Drain Check---
                                case tank_class.enum_seq_no.TANK_MIXING_TYPE_CHECK:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        // Exchange는 무조건 Drain 후 진행
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_EMPTY_CHECK1, "WAIT LS-41 ON");
                                    }
                                    break;

                                case tank_class.enum_seq_no.TANK_EMPTY_CHECK1:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {

                                        if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            //LS-41
                                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                                            {
                                                //Empty일때
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_DRAIN_END, "A DRAIN OK");
                                            }
                                            else
                                            {
                                                //Empty가 아닐 때
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_DRAIN_START, "");
                                            }
                                        }
                                        else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            //LS-42
                                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                                            {
                                                //Empty일때
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_DRAIN_END, "B DRAIN OK");
                                            }
                                            else
                                            {
                                                //Empty가 아닐 때
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_DRAIN_START, "");
                                            }
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.TANK_DRAIN_START:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        //TANK A 상태 Drain으로 변경 + Drain Valve ON AV-45
                                        if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.DRAIN;
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_drain = DateTime.Now;
                                            //Cir Drain OR Tank Drain 판단은 Sequence에서 진행 Auto + Drain Status면 Drain 진행 여부 판단
                                            //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, true);
                                            Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> Tank A", (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_EMPTY_CHECK2, "WAIT LS-41");
                                        }
                                        //TANK B 상태 Drain으로 변경 + Drain Valve ON AV-45
                                        else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.DRAIN;
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_drain = DateTime.Now;
                                            //Cir Drain OR Tank Drain 판단은 Sequence에서 진행 Auto + Drain Status면 Drain 진행 여부 판단
                                            //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, true);
                                            Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> Tank B", (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_EMPTY_CHECK2, "WAIT LS-42");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.TANK_EMPTY_CHECK2:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            //LS-41 Empty일때
                                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                                            {
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.DRAIN_WAIT;
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_DRAIN_DELAY_BEFORE_END, "A Wait Delay");
                                            }
                                            else
                                            {
                                                if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_drain).Seconds >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Drain_Time_Out))
                                                {
                                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Drain_Time_Over_Tank_A, "", true, false);
                                                }
                                            }
                                        }
                                        else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            //LS-42 Empty일때
                                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                                            {
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.DRAIN_WAIT;
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_DRAIN_DELAY_BEFORE_END, "B Wait Delay");
                                            }
                                            else
                                            {
                                                if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_drain).Seconds >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Drain_Time_Out))
                                                {
                                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Drain_Time_Over_Tank_B, "", true, false);
                                                }
                                            }
                                        }

                                    }
                                    break;

                                case tank_class.enum_seq_no.TANK_DRAIN_DELAY_BEFORE_END:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Drain_Valve_Off_Time_Delay_Tank_Circulation);
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_DRAIN_END, "Drain Valve Off Time Parameter Delay " + tmp_para_value.ToString() + " Sec");
                                    }
                                    break;

                                case tank_class.enum_seq_no.TANK_DRAIN_END:
                                    //AV-45 OFF
                                    //Drain 완료 후 Parameter 시간 만큼 일정 Delay 대기

                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= (tmp_para_value * 1000))
                                    {

                                        //TANK A Drain Valve OFF AV-45
                                        if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            //Tank 적산 초기화
                                            TotalUsage_Reset(Program.tank[(int)tank_class.enum_tank_type.TANK_A]);
                                            //농도 초기화
                                            Concentration_Reset(tank_class.enum_tank_type.TANK_A);
                                            //Life Time 초기화
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].life_time_to_minute = 0;
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, false);
                                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == false)
                                            {
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, false);
                                                CIRCULATION_PUMP_ON_OFF(false);
                                            }

                                        }
                                        //TANK B Drain Valve OFF AV-46
                                        else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            //Tank 적산 초기화
                                            TotalUsage_Reset(Program.tank[(int)tank_class.enum_tank_type.TANK_B]);
                                            //농도 초기화
                                            Concentration_Reset(tank_class.enum_tank_type.TANK_B);
                                            //Life Time 초기화
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].life_time_to_minute = 0;
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, false);
                                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == false)
                                            {
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, false);
                                                CIRCULATION_PUMP_ON_OFF(false);
                                            }
                                        }
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_INPUT_ORDER_CHECK, "Drain End");
                                    }
                                    else
                                    {


                                        Program.seq.main.memo_current = Convert.ToInt32(Program.seq.main.last_act_span.TotalSeconds) + " / " + tmp_para_value.ToString() + " Sec";
                                    }
                                    break;


                                ///---------------------------------------CHARGE OR REFILL---------------------------------------
                                case tank_class.enum_seq_no.TANK_INPUT_ORDER_CHECK:
                                    //Mixing Order 확인
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {

                                        Program.seq.input_request = true;
                                        Program.seq.cur_sametime_input_count = 0;
                                        Program.mixing_step_form.Setting_Mixing_Order(tank_class.enum_tank_type.TANK_ALL);
                                        Tank_CCSS_Input_Complete_Flag_Clear(Program.seq.main.cur_tank, Program.seq.mixing_order);
                                        if (Program.seq.mixing_order.Count > 0)
                                        {
                                            if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                            {
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.CHARGE;
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_charge = DateTime.Now;
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].circulation_processing = false;

                                            }
                                            else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                            {
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.CHARGE;
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_charge = DateTime.Now;
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].circulation_processing = false;

                                            }
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_INPUT_STANDBY, "");
                                        }
                                        else
                                        {
                                            //mixing Order Error
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.ERROR_BY_ALARM, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.TANK_INPUT_STANDBY:
                                    //DIW, CHEMICAL INPUT전 순서 확인 후 분기
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        //mixing order queue가 순차적으로 Supply 수행
                                        //Ex DIW -> H2O2 -> NH4OH 일 경우
                                        //DIW -> H2O2 -> NH4OH 수행
                                        Program.seq.circulation_on_req = true;
                                        Program.seq.cir_start = false;
                                        Program.seq.main.concentration_measuring = false;
                                        Program.seq.cur_sametime_input_count = 0;
                                        if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                                        {
                                            Program.seq.hdiw_check_start = DateTime.Now;
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.HDIW_TEMP_MONITORING, "");
                                        }
                                        else
                                        {
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_READY, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.HDIW_TEMP_MONITORING:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        //TS-09 온도 값 확인
                                        tmp_para_value = (int)(DateTime.Now - Program.seq.hdiw_check_start).TotalSeconds;
                                        if (HDIW_Temp_Check() == true)
                                        {
                                            if (tmp_para_value >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.HDIW_Temp_Delay_Time))
                                            {
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.HDIW_TEMP_OK, "");
                                            }
                                        }
                                        else
                                        {
                                            Program.seq.hdiw_check_start = DateTime.Now;
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.HDIW_TEMP_MONITORING, "TEMP OK And Wait Delay " + tmp_para_value + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.HDIW_Temp_Delay_Time));
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.HDIW_TEMP_OK:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.HOT_DIW_BY_PASS, false);
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_READY, "");
                                    }
                                    break;

                                ///////////////////////////////////////////////Circurlation///////////////////////////////////////////////

                                case tank_class.enum_seq_no.CIRCULATION_READY:

                                    //Tank_Circulation_Start_Level
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {

                                        //Supply가 Refill 중 일 때는 대기한다.
                                        if (Program.seq.supply.refill_run_state == true)
                                        {
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.TANK_INPUT_ORDER_CHECK, "");
                                        }
                                        else
                                        {
                                            if (Program.seq.input_request == true)
                                            {
                                                Program.seq.input_request = false;
                                                //if (Program.seq.cur_sametime_input_count > 0){Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count - 1;}
                                                //동시 진행중일 때 첫번째 Chemical이 먼저 True로 변경되면 Pass 후 두번째 Chemical이 True로 변경시 다음 Chemical 투입한다.
                                                //동시 투입한 Chemical 완료 횟수까지 Chemical Input 대기
                                                //CCSS1, CCSS2 동시 투입 시 input_count = 1
                                                //CCSS1 또는 CCSS2 완료 시 input_cout = 0;
                                                //CCSS2 완료 후 CCSS3 진입
                                                if (Program.seq.mixing_order.Count > 0 && Program.seq.cur_sametime_input_count == 0)
                                                {

                                                    Program.seq.cur_mixing = Program.seq.mixing_order.Dequeue();
                                                    Program.seq.cur_mixing_index = Program.seq.cur_mixing_index + 1;
                                                    Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count + 1;
                                                    CCSS_INPUT_Check(Program.seq.cur_mixing.type, Program.seq.main.cur_tank);
                                                    Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + Program.seq.cur_mixing.chemical.ToString()
                                                    , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                    //CCSS 적산 Reset 대기 시간
                                                    System.Threading.Thread.Sleep(1000);
                                                }
                                            }
                                            else
                                            {
                                                //동시 투입 여부 결정
                                                if (Program.seq.mixing_order_list.Count - 1 == Program.seq.cur_mixing_index)
                                                {
                                                    //마지막 순서일때는 무시한다.
                                                }
                                                else
                                                {
                                                    if (Program.seq.mixing_order_list.Count > Program.seq.cur_mixing_index)
                                                    {
                                                        if (Program.seq.mixing_order_list[Program.seq.cur_mixing_index].ccss_row == Program.seq.mixing_order_list[Program.seq.cur_mixing_index + 1].ccss_row)
                                                        {
                                                            //다음 Step도 같은 행일 때 동시 투입한다.
                                                            Program.seq.cur_mixing = Program.seq.mixing_order.Dequeue();
                                                            Program.seq.cur_mixing_index = Program.seq.cur_mixing_index + 1;
                                                            Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count + 1;
                                                            CCSS_INPUT_Check(Program.seq.cur_mixing.type, Program.seq.main.cur_tank);
                                                            Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + Program.seq.cur_mixing.chemical.ToString()
                                                            , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                            //CCSS 적산 Reset 대기 시간
                                                            System.Threading.Thread.Sleep(1000);
                                                        }
                                                    }

                                                }

                                            }
                                            //CCSS1 Input Start 후 적산 체크
                                            //유량계 적산 값 확인(FM-01) // 파라메타 유량만큼 들어갔을 때
                                            //CCSS 개별 Input 시 각 CCSS가 Input 완료되었을 때 flag 변경, 변경해야 다음 Order Queue 진행 Program.seq.input_request = true

                                            if (Program.cg_mixing_step.mixing_use == true)
                                            {
                                                if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(Program.seq.main.cur_tank, enum_ccss.CCSS1) == true)
                                                {
                                                    Program.seq.input_request = true; if (Program.seq.cur_sametime_input_count > 0) { Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count - 1; }
                                                }
                                                if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(Program.seq.main.cur_tank, enum_ccss.CCSS2) == true)
                                                {
                                                    Program.seq.input_request = true; if (Program.seq.cur_sametime_input_count > 0) { Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count - 1; }
                                                }
                                                if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(Program.seq.main.cur_tank, enum_ccss.CCSS3) == true)
                                                {
                                                    Program.seq.input_request = true; if (Program.seq.cur_sametime_input_count > 0) { Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count - 1; }
                                                }
                                                if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].use == true && CCSS_INPUT_END_BY_TOTALUSAGE(Program.seq.main.cur_tank, enum_ccss.CCSS4) == true)
                                                {
                                                    Program.seq.input_request = true; if (Program.seq.cur_sametime_input_count > 0) { Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count - 1; }
                                                }
                                            }
                                            else
                                            {
                                                if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].use == true && CCSS_INPUT_END_BY_LEVEL_H(Program.seq.main.cur_tank, enum_ccss.CCSS1) == true)
                                                {
                                                    Program.seq.input_request = true; if (Program.seq.cur_sametime_input_count > 0) { Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count - 1; }
                                                }
                                                if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].use == true && CCSS_INPUT_END_BY_LEVEL_H(Program.seq.main.cur_tank, enum_ccss.CCSS2) == true)
                                                {
                                                    Program.seq.input_request = true; if (Program.seq.cur_sametime_input_count > 0) { Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count - 1; }
                                                }
                                                if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].use == true && CCSS_INPUT_END_BY_LEVEL_H(Program.seq.main.cur_tank, enum_ccss.CCSS3) == true)
                                                {
                                                    Program.seq.input_request = true; if (Program.seq.cur_sametime_input_count > 0) { Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count - 1; }
                                                }
                                                if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].use == true && CCSS_INPUT_END_BY_LEVEL_H(Program.seq.main.cur_tank, enum_ccss.CCSS4) == true)
                                                {
                                                    Program.seq.input_request = true; if (Program.seq.cur_sametime_input_count > 0) { Program.seq.cur_sametime_input_count = Program.seq.cur_sametime_input_count - 1; }
                                                }
                                            }

                                            //안쓰는 Chemical은 완료처리 되어있음

                                            //각 CCSS별로 Input + CIRCULATION ON 모두 완료되었을 때 
                                            Program.seq.main.memo_current = Program.seq.main.cur_tank.ToString() + " Input Status / CCSS1 : " + Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete
                                                 + " / CCSS2 : " + Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete
                                                 + " / CCSS3 : " + Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete
                                                 + " / CCSS4 : " + Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete;

                                            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete == true &&
                                                Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete == true &&
                                                Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete == true &&
                                                Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete == true)
                                            {
                                                Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + "Input Complete"
                                                    , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_LEVEL, "");
                                            }
                                            else
                                            {
                                            }

                                            CCSS_Input_TimeOut_Check_By_Tank_Charge();
                                            //CCSS 투입 중 HDIW 온도 이상 발생 시 Alarm 발생
                                            if (Program.cg_app_info.eq_type == enum_eq_type.apm)
                                            {
                                                if (HDIW_Temp_Check() == false)
                                                {
                                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.HDIW_Supply_Temp_Error, "", true, false);
                                                }
                                            }
                                        }



                                    }
                                    break;

                                ///적산양 만큼 완료 후 CIRCULATION_MONITORING_LEVEL로 이동되며, Level H(M)가 되었는지 확인함
                                case tank_class.enum_seq_no.CIRCULATION_MONITORING_LEVEL:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 1000)
                                    {
                                        if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                                        {
                                            if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                            {
                                                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value == true)
                                                {
                                                    Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + "Tank A Level H"
                                                        , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                    Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_CHECK, "Target : TANKA_LEVEL_H");
                                                }
                                            }
                                            else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                            {
                                                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value == true)
                                                {
                                                    Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + "Tank A Level H"
                                                        , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                    Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_CHECK, "Target : TANKB_LEVEL_H");
                                                }
                                            }

                                        }
                                        else
                                        {
                                            //Tank Level M?
                                            if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == false)
                                            {
                                                if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].circulation_processing == true)
                                                {
                                                    System.Threading.Thread.Sleep(Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Heater_On_Dleay_Time) * 1000);
                                                    if (Program.cg_app_info.eq_type != enum_eq_type.dsp_mix)
                                                    {
                                                        CIRCULATION_1_HEATER_ON_OFF(true);
                                                    }
                                                    else
                                                    {
                                                        CIRCULATION_Heat_Exchanger_ON_OFF(true);
                                                    }
                                                    if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value == true)
                                                    {
                                                        Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + "Tank A Level H"
                                                            , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_CHECK, "");
                                                    }
                                                    else
                                                    {
                                                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Tank_Level_Not_HighUsing_Mixing_Tank_A, "Target : TANKA_LEVEL_M", true, false);
                                                    }
                                                }
                                            }
                                            else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == false)
                                            {
                                                if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].circulation_processing == true)
                                                {
                                                    System.Threading.Thread.Sleep(Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Heater_On_Dleay_Time) * 1000);
                                                    if (Program.cg_app_info.eq_type != enum_eq_type.dsp_mix)
                                                    {
                                                        CIRCULATION_1_HEATER_ON_OFF(true);
                                                    }
                                                    else
                                                    {
                                                        CIRCULATION_Heat_Exchanger_ON_OFF(true);
                                                    }
                                                    if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value == true)
                                                    {
                                                        Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + "Tank B Level H"
                                                            , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_CHECK, "");
                                                    }
                                                    else
                                                    {
                                                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Tank_Level_Not_HighUsing_Mixing_Tank_B, "Target : TANKB_LEVEL_M", true, false);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.CIRCULATION_CHECK:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                                        {
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_END, "");
                                        }
                                        else
                                        {
                                            if (Program.seq.cir_start == true)
                                            {
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_TEMP_START, "");
                                            }
                                            else
                                            {
                                                //순환이 끝났을 때 High Level이면 다음 Sequence로 이동
                                                //CIRCULATION_MONITORING_LEVEL과 같은 내용이나, Abnormal 상황에서 조치를 위함
                                                //Tank Level H(M)?
                                                if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                                {
                                                    if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value == true)
                                                    {
                                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_TEMP_START, "");
                                                    }
                                                    else
                                                    {
                                                        //Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Tank_Level_Not_HighUsing_Mixing_Tank_A, "", true, false);
                                                    }
                                                }
                                                else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                                {
                                                    if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value == true)
                                                    {
                                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_TEMP_START, "");
                                                    }
                                                    else
                                                    {
                                                        //Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Tank_Level_Not_HighUsing_Mixing_Tank_B, "", true, false);
                                                    }
                                                }
                                            }
                                        }


                                    }
                                    break;

                                case tank_class.enum_seq_no.CIRCULATION_MONITORING_TEMP_START:

                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        //Circulation 온도 체크 TS-21
                                        if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {

                                            if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Low)
                                                && Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_High))
                                            {
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_delay_Circulation_Temp_rising = DateTime.Now;
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_TEMP_OK_WAIT_DELAY, "PV : " + Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv.ToString() + " / " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Set));
                                            }
                                            else
                                            {
                                                if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_Circulation_Heater_on).Seconds > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Heating_Time_Out))
                                                {
                                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Heating_Time_Over_Temp_Controll_Tank_A, "", true, false);
                                                }
                                            }
                                            //Tank 상태가 Charge중이며,  CIRCULATION_MONITORING_TEMP_START가 되었으나, Circulation이 동작중이지 않을 때는 Abnormal 상황으로 판단.
                                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.CHARGE && Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == false)
                                            {
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Circulation_Pump_Error_In_Charge, "", true, false);
                                            }

                                        }
                                        else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Low)
                                                && Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_High))
                                            {
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_delay_Circulation_Temp_rising = DateTime.Now;
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_TEMP_OK_WAIT_DELAY, "PV : " + Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv.ToString() + " / " + Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Set));
                                            }
                                            else
                                            {
                                                if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_start_Circulation_Heater_on).Seconds > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Heating_Time_Out))
                                                {
                                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Heating_Time_Over_Temp_Controll_Tank_B, "", true, false);
                                                }
                                            }

                                            //Tank 상태가 Charge중이며,  CIRCULATION_MONITORING_TEMP_START가 되었으나, Circulation이 동작중이지 않을 때는 Abnormal 상황으로 판단.
                                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.CHARGE && Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == false)
                                            {
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Circulation_Pump_Error_In_Charge, "", true, false);
                                            }
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.CIRCULATION_MONITORING_TEMP_OK_WAIT_DELAY:

                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_Low)
                                                && Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Temp_High))
                                            {
                                                tmp_para_value_2 = (int)(DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_delay_Circulation_Temp_rising).TotalSeconds;
                                                if (tmp_para_value_2 >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Temp_OK_Time_Delay))
                                                {
                                                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_ok_Circulation_Temp = DateTime.Now;
                                                    Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + "Tank A Temp OK(" + Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv + ")"
                                                    , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);

                                                    //Heat Exchanger -> 내부 순환 변경
                                                    if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                                                    {
                                                        CIRCULATION_Heat_Exchanger_ON_OFF(false);
                                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_HE_UNIT, false);
                                                        CIRCULATION_1_HEATER_ON_OFF(true);
                                                    }
                                                    if (Program.cg_app_info.use_concentration_check == false)
                                                    {
                                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CHARGE_COMPLETE, "");
                                                    }
                                                    else
                                                    {
                                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION, "");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_delay_Circulation_Temp_rising = DateTime.Now;
                                                Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_TEMP_OK_WAIT_DELAY, "TEMP OK And Wait Delay " + tmp_para_value_2 + " / " + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Temp_OK_Time_Delay));
                                            }
                                        }
                                        else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_Low)
                                               && Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Temp_High))
                                                tmp_para_value_2 = (int)(DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_delay_Circulation_Temp_rising).TotalSeconds;
                                            if (tmp_para_value_2 >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Temp_OK_Time_Delay))
                                            {
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_ok_Circulation_Temp = DateTime.Now;
                                                Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + "Tank B Temp OK(" + Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv + ")"
                                                , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                //Heat Exchanger -> 내부 순환 변경
                                                if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                                                {
                                                    CIRCULATION_Heat_Exchanger_ON_OFF(false);
                                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_HE_UNIT, false);
                                                    CIRCULATION_1_HEATER_ON_OFF(true);
                                                }
                                                if (Program.cg_app_info.use_concentration_check == false)
                                                {
                                                    Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CHARGE_COMPLETE, "");
                                                }
                                                else
                                                {
                                                    Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION, "");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_delay_Circulation_Temp_rising = DateTime.Now;
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_TEMP_OK_WAIT_DELAY, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        //Supply Tank에서 농도 측정 중이 아닐 때
                                        if (Program.seq.supply.concentration_measuring == false ||
                                            (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status != tank_class.enum_tank_status.SUPPLY && Program.tank[(int)tank_class.enum_tank_type.TANK_B].status != tank_class.enum_tank_status.SUPPLY))
                                        {
                                            Program.seq.main.concentration_measuring = true;
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_VALVE_OPEN, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_VALVE_OPEN:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        //농도체크 시 일정 시간 Drain 유지 후 농도 체크 수행
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, false);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);
                                        //Drain Delay Parameter
                                        tmp_para_value = Convert.ToInt32(Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CM_Check_Drain_Time));
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_WAIT_DELAY1, "WAIT DRAIN Delay :" + tmp_para_value + " Sec");
                                    }
                                    break;
                                case tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_WAIT_DELAY1:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= tmp_para_value * 1000)
                                    {
                                        // Drain 완료 후 Tank로 Return
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, true);
                                        if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.dsp)
                                        {
                                            Program.ABB.Message_Command_To_Byte(Class_ABB.read_property_value1_to_4);
                                        }

                                        if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, true);
                                        }
                                        else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, true);
                                        }
                                        //Drain Delay Parameter
                                        tmp_para_value = Convert.ToInt32(Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CM_Check_Time_Delay));
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_WAIT_DELAY2, "WAIT Flow Delay :" + tmp_para_value + " Sec");
                                    }
                                    break;
                                case tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_WAIT_DELAY2:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= tmp_para_value * 1000)
                                    {
                                        //Drain 완료 후 일정시간 Tank로 흐른 후 농도 체크 진행
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_ACT, "");
                                    }
                                    break;
                                case tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_ACT:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        //농도 체크
                                        tmp_para_value_1 = Concentration_Check(Program.seq.main.cur_tank, true);
                                        if (tmp_para_value_1 == 0)
                                        {
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_VALVE_CLOSE, "");
                                        }
                                        else
                                        {
                                            if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                            {
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CM_Fail_Prep_Tank_A, "", true, false);
                                            }
                                            else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                            {
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CM_Fail_Prep_Tank_B, "", true, false);
                                            }

                                            //Seq_Cur_To_Next((Program.seq.main.no_cur), tank_class.enum_seq_no.ERROR_BY_ALARM, "");
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_VALVE_CLOSE, "");
                                        }
                                    }
                                    break;


                                case tank_class.enum_seq_no.CIRCULATION_MONITORING_CHECK_CONCENTRATION_VALVE_CLOSE:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, true);
                                        //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);
                                        if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                                            //농도 체크 시 Supply가 이미 사용중이면 사용 배관을 원상태로 만든다.
                                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.REFILL)
                                            {
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, true);
                                            }
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_END, "");
                                        }
                                        else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);
                                            //농도 체크 시 Supply가 이미 사용중이면 사용 배관을 원상태로 만든다.
                                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.REFILL)
                                            {
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, true);
                                            }
                                            Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CIRCULATION_END, "");
                                        }
                                        Program.seq.main.concentration_measuring = false;
                                    }
                                    break;
                                case tank_class.enum_seq_no.CIRCULATION_END:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        //Charge + Circulation Complete
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.CHARGE_COMPLETE, "");

                                    }
                                    break;

                                case tank_class.enum_seq_no.CHARGE_COMPLETE:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            //Ready로 변경
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.READY;
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_ready = DateTime.Now;
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_ok_Circulation_Temp = DateTime.Now;
                                            Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + " Tank A REDAY"
                                                , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);


                                        }
                                        else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.READY;
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_ready = DateTime.Now;
                                            Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + " Tank B REDAY"
                                                , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);

                                        }
                                        Seq_Cur_To_Next(Program.seq.main.no_cur, tank_class.enum_seq_no.MODE_CHECK, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no.ERROR_BY_ALARM:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        Program.seq.main.last_act_time = DateTime.Now;
                                        //Program.seq.main.no_cur = tank_class.enum_seq_no.Read;
                                    }
                                    break;

                                case tank_class.enum_seq_no.ERROR_BY_APP:
                                    if (Program.seq.main.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        Program.seq.main.last_act_time = DateTime.Now;
                                        //Program.seq.main.no_cur = tank_class.enum_seq_no.READY_TO_AUTO;
                                    }
                                    break;

                            }
                        }
                    }

                    //Tank Ready된 시간 계산 Alarm 발생을 위함
                    Tank_Ready_TimeOut_Check();


                    //Sequnce Main No, 또는 Meom가 변경될 때만 Seq 변경 로그 생성 
                    if (Program.seq.main.no_old != Program.seq.main.no_cur)
                    {
                        Program.seq.main.memo_current = "";
                        Program.log_md.LogWrite(Program.seq.main.state_display + " : " + Program.seq.main.memo_current, Module_Log.enumLog.SEQ_MAIN, "", Module_Log.enumLevel.ALWAYS);
                    }
                    else
                    {
                        if (Program.seq.main.memo_old != Program.seq.main.memo_current)
                        {
                            Program.log_md.LogWrite(Program.seq.main.state_display + " : " + Program.seq.main.memo_current, Module_Log.enumLog.SEQ_MAIN, "", Module_Log.enumLevel.ALWAYS);
                        }
                    }
                    Program.seq.main.no_old = Program.seq.main.no_cur;
                    Program.seq.main.memo_old = Program.seq.main.memo_current;
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + "." + "Seq_Main" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally
                {

                }
                System.Threading.Thread.Sleep(100);
            }

        }
        public void Seq_Supply()
        {
            string result = "";
            bool tmp_dio = false;
            int tmp_para_value = 0, tmp_para_value_1 = 0, tmp_para_value_2 = 0, tmp_refill_start, tmp_refill_end, tmp_keep_delay = 0;
            int tank_suppply_end_check = 0;
            while (true)
            {
                try
                {
                    Program.seq.supply.last_act_span = DateTime.Now - Program.seq.supply.last_act_time;

                    if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY)
                    {

                    }
                    else if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
                    {

                    }
                    else
                    {

                        if (timer_manual_sequence_tank_a.Enabled == false && timer_manual_sequence_tank_b.Enabled == false)
                        {
                            //Semi Auto 중에는 Circulation Seqeuence 중지
                            switch (Program.seq.supply.no_cur)
                            {

                                case tank_class.enum_seq_no_supply.TANK_READY_CHECK:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 50)
                                    {
                                        //Tank가 Ready 상태로 전환됬을 때 CC Request 전송(Chemical Change Request 400)
                                        if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.READY)
                                        {
                                            Program.seq.supply.dt_start_cc_start_req_cds_to_ctc = DateTime.Now;
                                            Program.seq.supply.concentration_measuring = false;

                                            if (Program.seq.supply.CC_START_TANK == "")
                                            {
                                                //CC가 아닌 Auto Mode 시작 후 Tank Ready 일떄 Falg 설정(Request Send 요청 Flag)
                                                Program.seq.supply.ready_flag = true;
                                                Program.seq.supply.ready_flag_in_req_send = false;
                                            }
                                            Program.seq.supply.ctc_supply_request = false;
                                            Program.seq.supply.req_c_c_start_cds_to_ctc = false; Program.seq.supply.rep_c_c_start_cds_to_ctc = false;
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.READY_SUPPLY_BY_CTC, "");
                                        }
                                        else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.READY)
                                        {
                                            Program.seq.supply.dt_start_cc_start_req_cds_to_ctc = DateTime.Now;
                                            Program.seq.supply.concentration_measuring = false;

                                            if (Program.seq.supply.CC_START_TANK == "")
                                            {
                                                //CC가 아닌 Auto Mode 시작 후 Tank Ready 일떄 Falg 설정(Request Send 요청 Flag)
                                                Program.seq.supply.ready_flag = true;
                                                Program.seq.supply.ready_flag_in_req_send = false;
                                            }
                                            Program.seq.supply.ctc_supply_request = false;
                                            Program.seq.supply.req_c_c_start_cds_to_ctc = false; Program.seq.supply.rep_c_c_start_cds_to_ctc = false;
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.READY_SUPPLY_BY_CTC, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.READY_SUPPLY_BY_CTC:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        //CTC Supply Start Request 대기
                                        //Tank A OR Tank B 선택

                                        //Refil 상태 변수 초기화 사용 중에 Abnormal 종료되었을 때 Valve Close
                                        if (Program.seq.supply.refill_run_state == true)
                                        {
                                            Program.seq.supply.refill_run_state = false;
                                            CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1);
                                            CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS1);
                                        }

                                        if (Program.seq.supply.ctc_supply_request == true || Program.seq.supply.CC_START_TANK != "")
                                        {
                                            //Program.seq.supply.CC_START_TANK = Chemical Change Sequence에서 시작됬을 때
                                            Program.seq.supply.ctc_supply_request = false;
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "START", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);

                                            //Process Start 후 Supply로 상태 변경
                                            //Wafer Count, Process Start TIme 초기화

                                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.READY)
                                            {
                                                if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY && Program.cg_app_info.keep_supply_and_cir_off_delay_by_change == false)
                                                {
                                                    Tank_Supply_End(tank_class.enum_tank_type.TANK_B);
                                                    Tank_Value_Clear(tank_class.enum_tank_type.TANK_B, false);
                                                }
                                                else
                                                {
                                                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_start_process = DateTime.Now;
                                                }
                                                Program.seq.supply.cur_tank = tank_class.enum_tank_type.TANK_A;
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.SUPPLY;
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt = 0;
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_process = DateTime.Now;
                                                Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.CIRCULATION_HEATER_OFF_REQ, "");
                                            }
                                            else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.READY)
                                            {
                                                if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY && Program.cg_app_info.keep_supply_and_cir_off_delay_by_change == false)
                                                {
                                                    Tank_Supply_End(tank_class.enum_tank_type.TANK_A);
                                                    Tank_Value_Clear(tank_class.enum_tank_type.TANK_A, false);
                                                }
                                                else
                                                {
                                                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_process = DateTime.Now;
                                                }
                                                Program.seq.supply.cur_tank = tank_class.enum_tank_type.TANK_B;
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.SUPPLY;
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt = 0;
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_start_process = DateTime.Now;
                                                Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.CIRCULATION_HEATER_OFF_REQ, "");
                                            }
                                            else
                                            {
                                                //A 또는 B가 Ready가 아닌데 CTC Requst가 오면 알람 발생
                                            }
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.CIRCULATION_HEATER_OFF_REQ:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Circulation Heater OFF", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        CIRCULATION_1_HEATER_ON_OFF(false);
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.CIRCULATION_HEATER_OFF_COMPLETE, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.CIRCULATION_HEATER_OFF_COMPLETE:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.CIRCULATION_PUPMP_OFF_REQ, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.CIRCULATION_PUPMP_OFF_REQ:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        //BP-21 STOP
                                        CIRCULATION_PUMP_ON_OFF(false);
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.CIRCULATION_VALVE_OFF_REQ, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.CIRCULATION_VALVE_OFF_REQ:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        //Valve OFF Tank A : AV-21, AV-23
                                        //Valve OFF Tank B : AV-22, AV-24
                                        //Valve ON AV-25(3Way)

                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, false);
                                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_A, false);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_TANK_A, false);
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Circulation Valve OFF -> " + "Tank A", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        }
                                        else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_B, false);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_TANK_B, false);
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Circulation Valve OFF -> " + "Tank B", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        }
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.CIRCULATION_VALVE_OFF_COMPLETE, "");
                                    }
                                    break;


                                case tank_class.enum_seq_no_supply.CIRCULATION_VALVE_OFF_COMPLETE:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        if (Program.cg_app_info.keep_supply_and_cir_off_delay_by_change == false)
                                        {
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_VALVE_ON_REQ, "");
                                        }
                                        else
                                        {
                                            if (Program.seq.supply.CC_START_TANK == "")
                                            {
                                                Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_VALVE_ON_REQ, "");
                                            }
                                            else
                                            {
                                                Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.KEEP_SUPPLY_READY, "");
                                            }
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.KEEP_SUPPLY_READY:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.KEEP_SUPPLY_START_VALVE_CHANGE, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.KEEP_SUPPLY_START_VALVE_CHANGE:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        if (Program.seq.supply.CC_START_TANK == "1" && Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY)
                                        {
                                            //Tank A Ready -> Supply
                                            //Tank B Return Valve Close
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, false);
                                        }
                                        else if (Program.seq.supply.CC_START_TANK == "2" && Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY)
                                        {
                                            //Tank B Ready -> Supply
                                            //Tank A Return Valve Close
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, false);
                                        }
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_VALVE_ON_REQ, "");

                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.SUPPLY_VALVE_ON_REQ:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        //Tank A + Supply A + SUpply B + Return A
                                        //AV-41, AV-43, AV-44, AV-35
                                        //Tank B + Supply B + Return B
                                        //AV-42, AV-43, AV-44, AV-36

                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_A, true);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_TO_MAIN_B, true);

                                        if (Program.cg_app_info.use_concentration_check == true)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, true);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, true);
                                        }
                                        else
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, false);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);
                                        }

                                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            Program.CTC.Message_Tank_A_Supply_Start_Event_458();
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, true);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, true);
                                            if (Program.cg_app_info.use_concentration_check == true)
                                            {
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, true);
                                            }
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Supply Valve On -> " + "Tank A", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        }
                                        else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            Program.CTC.Message_Tank_B_Supply_Start_Event_460();
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_B, true);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, true);
                                            if (Program.cg_app_info.use_concentration_check == true)
                                            {
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, true);
                                            }
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Supply Valve On -> " + "Tank B", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        }
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_VALVE_ON_COMPLETE, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.SUPPLY_VALVE_ON_COMPLETE:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_PUMP_ON_REQ, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.SUPPLY_PUMP_ON_REQ:

                                    //Valve Open 후 일정 시간 Delay 및 Valve Open 확인(Interlock)
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true &&
                                                Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_A].value == true)
                                            {
                                                //BP-41, BP-42 RUN
                                                SUPPLY_A_PUMP_ON_OFF(true); SUPPLY_B_PUMP_ON_OFF(true);
                                                Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Pump On  -> " + "Tank A", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_PUMP_ON_COMPLETE, "");
                                            }
                                        }
                                        else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true &&
                                                Program.IO.DO.Tag[(int)Config_IO.enum_do.RETURN_TO_TANK_B].value == true)
                                            {
                                                //BP-41, BP-42 RUN
                                                SUPPLY_A_PUMP_ON_OFF(true); SUPPLY_B_PUMP_ON_OFF(true);
                                                Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Pump On  -> " + "Tank B", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_PUMP_ON_COMPLETE, "");
                                            }

                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.SUPPLY_PUMP_ON_COMPLETE:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        tmp_keep_delay = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Keep_Supply_Time_Chemical_Changed);
                                        if (Program.cg_app_info.keep_supply_and_cir_off_delay_by_change == true)
                                        {
                                            if (Program.seq.supply.CC_START_TANK != "")
                                            {
                                                Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.KEEP_SUPPLY_WAITING_DELAY, "");
                                            }
                                            else
                                            {
                                                Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_CHECK_FLOW, "");
                                            }
                                        }
                                        else
                                        {
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_CHECK_FLOW, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.KEEP_SUPPLY_WAITING_DELAY:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= tmp_keep_delay * 1000)
                                    {
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.KEEP_SUPPLY_END, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.KEEP_SUPPLY_END:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.KEEP_SUPPLY_END_VALVE_CHANGE, "");
                                    }
                                    break;
                                case tank_class.enum_seq_no_supply.KEEP_SUPPLY_END_VALVE_CHANGE:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        if (Program.seq.supply.CC_START_TANK == "1" && Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY)
                                        {
                                            //Tank A Ready -> Supply
                                            //Tank B Supply Valve Close
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.NONE;
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_A, false);
                                        }
                                        else if (Program.seq.supply.CC_START_TANK == "2" && Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY)
                                        {
                                            //Tank B Ready -> Supply
                                            //Tank A Supply Valve Close
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.NONE;
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_FROM_TANK_B, false);
                                        }
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_CHECK_FLOW, "");
                                    }
                                    break;


                                case tank_class.enum_seq_no_supply.SUPPLY_CHECK_FLOW:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        //FM-41 FM-42 OK?
                                        //각 Flow가 정상일때 tmp_para_value +1 => tmp_para_value = 2
                                        tmp_para_value = 0;
                                        if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_A))
                                        {
                                            tmp_para_value += 1;
                                        }

                                        if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_B))
                                        {
                                            tmp_para_value += 1;
                                        }

                                        if (Program.cg_app_info.mode_simulation.use == false)
                                        {
                                            if (tmp_para_value == 2)
                                            {
                                                Program.seq.scr_supply_on_req = true;
                                                Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_HEATER_ON_REQ, "");
                                            }
                                        }
                                        else
                                        {
                                            Program.seq.scr_supply_on_req = true;
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_HEATER_ON_REQ, "");
                                        }

                                    }

                                    break;

                                case tank_class.enum_seq_no_supply.SUPPLY_HEATER_ON_REQ:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Heater_On_Dleay_Time) * 1000)
                                    {
                                        if (Program.seq.scr_supply_on_req == true)
                                        {

                                            SUPPLY_A_HEATER_ON_OFF(true);

                                            if (Program.cg_app_info.eq_type != enum_eq_type.ipa)
                                            {
                                                SUPPLY_B_HEATER_ON_OFF(true);
                                            }
                                        }
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Heater On" + "", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_HEATER_ON_COMPLETE, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.SUPPLY_HEATER_ON_COMPLETE:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_LOOP_FLUSH, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.SUPPLY_LOOP_FLUSH:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        //LAL Reclaim Valve ON
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_START, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.SUPPLY_START:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 20)
                                    {
                                        if (Program.seq.supply.CC_START_TANK != "")
                                        {
                                            if (Program.seq.supply.CC_START_TANK == "1")
                                            {
                                                Program.CTC.Message_Chemical_Change_End_Event_453("1");
                                            }
                                            else if (Program.seq.supply.CC_START_TANK == "2")
                                            {
                                                Program.CTC.Message_Chemical_Change_End_Event_453("2");
                                            }
                                        }
                                        //Supply 완료 후 Enable 전송을 위한 Trigger 초기화
                                        supply_enable_send = false;
                                        Program.seq.supply.cc_ctc_req_flag = false;
                                        Program.seq.supply.cc_level_low_flag = false;
                                        Program.seq.supply.cc_lifetime_flag = false;
                                        Program.seq.supply.cc_manual_exchange = false;
                                        Program.seq.supply.cc_wafer_cnt_over_flag = false;

                                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_delay_Supply_Temp_rising = DateTime.Now;
                                        Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_delay_Supply_Temp_rising = DateTime.Now;
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Enable -> true" + "", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_MONITORING, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.SUPPLY_MONITORING:

                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        if (Program.cg_app_info.use_concentration_check == false)
                                        {

                                        }
                                        else
                                        {
                                            //Concentration Measure Delay Parameter

                                            //Main Seq에서 농도 측정 사용 시 완료 시까지 대기 한다.
                                            if (Program.seq.main.concentration_measuring == false)
                                            {

                                                tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Concentration_Measure_Interval);
                                                if (tmp_para_value == 0)
                                                {

                                                }
                                                else
                                                {
                                                    if ((DateTime.Now - Program.seq.supply.dt_last_concentration_check).TotalSeconds >= tmp_para_value)
                                                    {
                                                        Program.seq.supply.dt_last_concentration_check = DateTime.Now;
                                                        Program.seq.supply.concentration_check_req_in_supply = true;
                                                    }

                                                    if (Program.seq.supply.concentration_check_req_in_supply == true)
                                                    {
                                                        Program.seq.supply.concentration_check_req_in_supply = false;
                                                        Program.seq.supply.concentration_measuring = true;
                                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION, "");
                                                    }
                                                }

                                            }

                                        }

                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_VALVE_OPEN, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_VALVE_OPEN:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, true);
                                        //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, false);

                                        //Drain Delay Parameter
                                        tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CM_Check_Drain_Time);
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_WAIT_DELAY1, "");

                                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, false);

                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_WAIT_DELAY1, "");
                                        }
                                        else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, false);
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_WAIT_DELAY1, "");
                                        }
                                    }
                                    break;
                                case tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_WAIT_DELAY1:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= tmp_para_value * 1000)
                                    {
                                        // Drain 완료 후 Tank로 Return
                                        //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, true);

                                        if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.dsp)
                                        {
                                            Program.ABB.Message_Command_To_Byte(Class_ABB.read_property_value1_to_4);
                                        }

                                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, true);
                                        }
                                        else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, true);
                                        }

                                        //Drain Delay Parameter

                                        //Check Interval Parameter는 Tank Change시에만 적용
                                        //tmp_para_value = 1;
                                        //농도 체크에 시간이 필요한 농도계로 인해 Parameter로 참조
                                        tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CM_Check_Time_Delay);
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_WAIT_DELAY2, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_WAIT_DELAY2:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= tmp_para_value * 1000)
                                    {
                                        //Drain 완료 후 일정시간 Tank로 흐른 후 농도 체크 진행
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_ACT, "");
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_ACT:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        //농도 체크
                                        tmp_para_value_1 = 0; tmp_para_value_1 = Concentration_Check(Program.seq.supply.cur_tank, true);
                                        tank_suppply_end_check += tmp_para_value_1;
                                        if (tmp_para_value_1 == 0)
                                        {
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_CIR_VALVE_OPEN, "");
                                        }
                                        else
                                        {
                                            if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                            {
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Concentration_Fail_Tank_A, "", true, false);
                                            }
                                            else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                            {
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Concentration_Fail_Tank_B, "", true, false);
                                            }
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_CIR_VALVE_OPEN, "");
                                            //Seq_Supply_Cur_To_Next((Program.seq.supply.no_cur), tank_class.enum_seq_no_supply.ERROR_BY_ALARM, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_CIR_VALVE_OPEN:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_A, true);
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_VALVE_CLOSE, "");
                                        }
                                        else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_TO_TANK_B, true);
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_VALVE_CLOSE, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_VALVE_CLOSE:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, true);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_2, true);
                                        if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_A, true);
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_OK, "");
                                        }
                                        else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.RETURN_SAMPLE_TO_TANK_B, true);
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_OK, "");
                                        }
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.MONITORING_CHECK_CONCENTRATION_OK:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 100)
                                    {
                                        Program.seq.supply.concentration_measuring = false;
                                        //Charge + Circulation Complete
                                        Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.SUPPLY_MONITORING, "");
                                    }
                                    break;


                                case tank_class.enum_seq_no_supply.ERROR_BY_ALARM:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        //Program.seq.supply.no_cur = tank_class.enum_seq_no_supply.Read;
                                    }
                                    break;

                                case tank_class.enum_seq_no_supply.ERROR_BY_APP:
                                    if (Program.seq.supply.last_act_span.TotalMilliseconds >= 500)
                                    {
                                        //Program.seq.supply.no_cur = tank_class.enum_seq_no_supply.READY_TO_AUTO;
                                    }
                                    break;
                            }

                            //////////////////////Supply Seq에 상관 없이 항상 수행//////////////////////

                            //Tank Level LifeTime 등 Check
                            tank_suppply_end_check = 0;
                            Program.seq.supply.c_c_need_text = "";
                            //Chemical Change 조건 확인 충족 시 tank_suppply_end_check +1 / tank_suppply_end_check >= 1 이상일 때 C.C조건 수행
                            if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A && (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.REFILL))
                            {
                                //Supply 진행 후 Tank 온도 확인
                                if (Program.cg_app_info.supply_enable_force == true)
                                {
                                    if (supply_enable_send == false)
                                    {
                                        supply_enable_send = true;
                                        Program.CTC.Message_CDS_Enable_Event_450();
                                    }
                                }
                                else
                                {
                                    if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Low)
                                                                       && Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_High))
                                    {
                                        tmp_para_value_2 = (int)(DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_delay_Supply_Temp_rising).TotalSeconds;
                                        if (tmp_para_value_2 >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Supply_Temp_OK_Time_Delay))
                                        {
                                            if (supply_enable_send == false)
                                            {
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_ok_Process_Temp = DateTime.Now;
                                                Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + "Tank A Supply Temp OK(" + Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv + ")"
                                                , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                supply_enable_send = true;
                                                supply_disable_send = false;
                                                Program.CTC.Message_CDS_Enable_Event_450();

                                            }
                                        }

                                    }
                                    else
                                    {
                                        Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_delay_Supply_Temp_rising = DateTime.Now;
                                        if (supply_enable_send == true)
                                        {
                                            if (supply_disable_send == false)
                                            {
                                                supply_disable_send = true;
                                                if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_Low))
                                                {
                                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.High_Abnormal_Temp_In_Tank_A, "", true, false);
                                                }
                                                else if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_a.pv > Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Temp_High))
                                                {
                                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Low_Abnormal_Temp_In_Tank_A, "", true, false);
                                                }
                                                Program.CTC.Message_CDS_Disable_Event_451(false);
                                            }
                                        }
                                    }
                                }

                                //CTC Change Req는 우선 순위로 먼저 적용한다. OR 조건으로 묶여있기 때문에 상관 없음
                                if (Program.seq.supply.ctc_c_c_request == true)
                                {
                                    Program.seq.supply.ctc_c_c_request = false;
                                    tank_suppply_end_check += 1;
                                    if (Program.seq.supply.cc_ctc_req_flag == false)
                                    {
                                        Program.seq.supply.cc_ctc_req_flag = true;
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "(TANK A CTC REQ)", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                    }
                                }

                                //Level Low 확인
                                if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                {
                                    tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value; //L
                                    if (tmp_dio == false) { tank_suppply_end_check += 1; Program.seq.supply.c_c_need_text = "TANK A LEVEL Low"; }
                                    if (Program.seq.supply.cc_level_low_flag == false)
                                    {
                                        Program.seq.supply.cc_level_low_flag = true;
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "(TANK A LEVEL L OFF)", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                    }
                                }

                                //농도 이상 확인
                                //tank_suppply_end_check += Concentration_Check();

                                //Wafer Count Over Check
                                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Wafer_Count_High) != 0)
                                {
                                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].wafer_cnt >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Wafer_Count_High))
                                    {
                                        tank_suppply_end_check += 1; Program.seq.supply.c_c_need_text = "TANK A WAFER COUNT OVER";
                                        if (Program.seq.supply.cc_wafer_cnt_over_flag == false)
                                        {
                                            Program.seq.supply.cc_wafer_cnt_over_flag = true;
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "(TANK A WAFER CNT OVER)", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        }
                                    }
                                }

                                //Life Time Over Check
                                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Life_Time_High) != 0)
                                {
                                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].life_time_to_minute = (int)(DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_process).TotalMinutes;
                                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].life_time_to_minute >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Life_Time_High))
                                    {
                                        tank_suppply_end_check += 1; Program.seq.supply.c_c_need_text = "TANK A LIFE TIME OVER";
                                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Life_Time_Over_Tank_A, "", true, false);
                                        if (Program.seq.supply.cc_lifetime_flag == false)
                                        {
                                            Program.seq.supply.cc_lifetime_flag = true;
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "(TANK A LIFE TIME OVER)", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        }
                                    }
                                }

                                //User Manual Exchange Request
                                if (Manual_Exchange_Req_By_User == true)
                                {
                                    Manual_Exchange_Req_By_User = false;
                                    if (Program.seq.supply.cc_manual_exchange == false)
                                    {
                                        Program.seq.supply.cc_manual_exchange = true;
                                        tank_suppply_end_check += 1; Program.seq.supply.c_c_need_text = "MANUAL EXCHANGE BY USER";
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "(Manual Exchange By User)", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                    }
                                }

                                if (tank_suppply_end_check >= 1)
                                {
                                    Program.seq.supply.c_c_need = true;
                                }

                                //Supply 도중 LL 도달 시 전체 중지
                                //Level Low 확인
                                if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                {
                                    if (Program.cg_app_info.supply_empty_level_to_stop == true)
                                    {
                                        tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value; //EMPTY
                                        if (tmp_dio == false)
                                        {
                                            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Level_Low_Low_Tank_A, "", true, false);
                                            Tank_Supply_End(Program.seq.supply.cur_tank);
                                        }
                                    }
                                    else
                                    {
                                        tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value; //LL
                                        if (tmp_dio == false)
                                        {
                                            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Level_Low_Low_Tank_A, "", true, false);
                                            Tank_Supply_End(Program.seq.supply.cur_tank);
                                        }
                                    }

                                }

                            }
                            else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B && (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.REFILL))
                            {
                                //Supply 진행 후 Tank 온도 확인
                                if (Program.cg_app_info.supply_enable_force == true)
                                {
                                    if (supply_enable_send == false)
                                    {
                                        supply_enable_send = true;
                                        Program.CTC.Message_CDS_Enable_Event_450();
                                    }
                                }
                                else
                                {
                                    if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv >= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_Low)
                                                                       && Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_High))
                                    {
                                        tmp_para_value_2 = (int)(DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_delay_Supply_Temp_rising).TotalSeconds;
                                        if (tmp_para_value_2 >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Supply_Temp_OK_Time_Delay))
                                        {

                                            if (supply_enable_send == false)
                                            {
                                                Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_ok_Process_Temp = DateTime.Now;
                                                Program.eventlog_form.Insert_Event("SEQ : " + Program.seq.main.no_cur.ToString() + " -> " + "Tank A Supply Temp OK(" + Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv + ")"
                                                , (int)frm_eventlog.enum_event_type.SEQ, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                supply_enable_send = true;
                                                supply_disable_send = false;
                                                Program.CTC.Message_CDS_Enable_Event_450();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_delay_Supply_Temp_rising = DateTime.Now;
                                        if (supply_enable_send == true)
                                        {
                                            if (supply_disable_send == false)
                                            {
                                                supply_disable_send = true;
                                                if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_Low))
                                                {
                                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.High_Abnormal_Temp_In_Tank_B, "", true, false);
                                                }
                                                else if (Program.main_form.SerialData.TEMP_CONTROLLER.tank_b.pv > Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Temp_High))
                                                {
                                                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Low_Abnormal_Temp_In_Tank_B, "", true, false);
                                                }
                                                Program.CTC.Message_CDS_Disable_Event_451(false);
                                            }
                                        }
                                    }
                                }

                                //CTC Change Req는 우선 순위로 먼저 적용한다. OR 조건으로 묶여있기 떄문에 상관 없음
                                if (Program.seq.supply.ctc_c_c_request == true)
                                {
                                    Program.seq.supply.ctc_c_c_request = false;
                                    tank_suppply_end_check += 1;
                                    if (Program.seq.supply.cc_ctc_req_flag == false)
                                    {
                                        Program.seq.supply.cc_ctc_req_flag = true;
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "(TANK B CTC REQ)", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                    }
                                }

                                //Level Low 확인
                                if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                {
                                    tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value;//L
                                    if (tmp_dio == false) { tank_suppply_end_check += 1; Program.seq.supply.c_c_need_text = "TANK B LEVEL Low"; }
                                    if (Program.seq.supply.cc_level_low_flag == false)
                                    {
                                        Program.seq.supply.cc_level_low_flag = true;
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "(TANK B LEVEL L OFF)", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                    }
                                }

                                //농도 이상 확인
                                //tank_suppply_end_check += Concentration_Check();

                                //Wafer Count Over Check
                                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Wafer_Count_High) != 0)
                                {
                                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].wafer_cnt >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Wafer_Count_High))
                                    {
                                        tank_suppply_end_check += 1; Program.seq.supply.c_c_need_text = "TANK B WAFER COUNT OVER";
                                        if (Program.seq.supply.cc_wafer_cnt_over_flag == false)
                                        {
                                            Program.seq.supply.cc_wafer_cnt_over_flag = true;
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "(TANK B WAFER CNT OVER)", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        }
                                    }
                                }
                                //Life Time Over Check
                                if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Life_Time_High) != 0)
                                {
                                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].life_time_to_minute = (int)(DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_start_process).TotalMinutes;
                                    if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].life_time_to_minute >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CC_Life_Time_High))
                                    {
                                        tank_suppply_end_check += 1; Program.seq.supply.c_c_need_text = "TANK B LIFE TIME OVER";
                                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Life_Time_Over_Tank_B, "", true, false);
                                        if (Program.seq.supply.cc_lifetime_flag == false)
                                        {
                                            Program.seq.supply.cc_lifetime_flag = true;
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "(TANK B LIFE TIME OVER)", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        }
                                    }
                                }

                                //User Manual Exchange Request
                                if (Manual_Exchange_Req_By_User == true)
                                {
                                    Manual_Exchange_Req_By_User = false;
                                    if (Program.seq.supply.cc_manual_exchange == false)
                                    {
                                        Program.seq.supply.cc_manual_exchange = true;
                                        tank_suppply_end_check += 1; Program.seq.supply.c_c_need_text = "MANUAL EXCHANGE BY USER";
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "(Manual Exchange By User)", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                    }
                                }

                                if (tank_suppply_end_check >= 1)
                                {
                                    Program.seq.supply.c_c_need = true;
                                }
                                //Supply 도중 LL 도달 시 전체 중지
                                //Level Low 확인
                                if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                {
                                    if (Program.cg_app_info.supply_empty_level_to_stop == true)
                                    {
                                        tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value;//EMPTY
                                        if (tmp_dio == false)
                                        {
                                            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Level_Low_Low_Tank_B, "", true, false);
                                            Tank_Supply_End(Program.seq.supply.cur_tank);
                                        }
                                    }
                                    else
                                    {
                                        tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value;//LL
                                        if (tmp_dio == false)
                                        {
                                            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Level_Low_Low_Tank_B, "", true, false);
                                            Tank_Supply_End(Program.seq.supply.cur_tank);
                                        }
                                    }

                                }

                            }

                            //Tank CC Need Check
                            if (Program.seq.supply.c_c_need == true)
                            {
                                if (Program.seq.supply.concentration_measuring == false)
                                {
                                    //농도 측정 중이 아닐 때만 CC 시도
                                    if (Program.seq.supply.req_c_c_start_cds_to_ctc == false)
                                    {
                                        //각 Supply당 1회만 전송한다.
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        Program.seq.supply.dt_start_cc_start_req_cds_to_ctc = DateTime.Now;
                                        Program.CTC.Message_Chemical_Change_Request_400();
                                    }
                                }

                                if (Program.seq.supply.req_c_c_start_cds_to_ctc == true && Program.seq.supply.rep_c_c_start_cds_to_ctc == false)
                                {
                                    if ((DateTime.Now - Program.seq.supply.dt_start_cc_start_req_cds_to_ctc).TotalSeconds >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chemical_Change_Confirm_Time_Out))
                                    {
                                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Over_Time_Chemical_Change_Request, "", true, false);
                                    }
                                }
                                if (Program.seq.supply.rep_c_c_start_cds_to_ctc == true)
                                {
                                    Program.seq.supply.c_c_need = false;
                                    Program.seq.supply.req_c_c_start_cds_to_ctc = false; Program.seq.supply.rep_c_c_start_cds_to_ctc = false;
                                    Program.seq.supply.ready_flag = false;

                                    if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A)
                                    {
                                        Chemical_Change_Start(tank_class.enum_tank_type.TANK_A);
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Process -> " + "Tank A", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        //Tank B가 Ready 상태이면 Tank B로 Chage
                                        //CHARGE_COMPLETE에서 자동으로 넘어온다. Main -> Supply
                                        if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].enable == true)
                                        {
                                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.READY)
                                            {
                                                //SUPPLY_END
                                                if (Program.cg_app_info.keep_supply_and_cir_off_delay_by_change == false)
                                                {
                                                    Tank_Supply_End(tank_class.enum_tank_type.TANK_A);
                                                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_cc_tank = DateTime.Now;
                                                    Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.READY_SUPPLY_BY_CTC, "");
                                                }
                                                else
                                                {
                                                    Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.READY_SUPPLY_BY_CTC, "");
                                                }
                                            }
                                            else
                                            {
                                                //사전 준비된 Tank 없음
                                                //Alarm 발생
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Not_Ready_Tank_B, "", true, false);
                                            }
                                        }
                                        else
                                        {
                                            //SUPPLY_END
                                            Tank_Supply_End(tank_class.enum_tank_type.TANK_A);
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_cc_tank = DateTime.Now;
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.TANK_READY_CHECK, "");
                                        }

                                    }
                                    else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B)
                                    {
                                        Chemical_Change_Start(tank_class.enum_tank_type.TANK_B);
                                        Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Process -> " + "Tank B", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                        //Tank A가 Ready 상태이면 Tank A로 Chage
                                        //CHARGE_COMPLETE에서 자동으로 넘어온다. Main -> Supply
                                        if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].enable == true)
                                        {
                                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.READY)
                                            {
                                                //SUPPLY_END
                                                if (Program.cg_app_info.keep_supply_and_cir_off_delay_by_change == false)
                                                {
                                                    Tank_Supply_End(tank_class.enum_tank_type.TANK_B);
                                                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_cc_tank = DateTime.Now;
                                                    Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.READY_SUPPLY_BY_CTC, "");
                                                }
                                                else
                                                {
                                                    Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.READY_SUPPLY_BY_CTC, "");
                                                }
                                            }
                                            else
                                            {
                                                //사전 준비된 Tank 없음
                                                //Alarm 발생
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Not_Ready_Tank_A, "", true, false);
                                            }
                                        }
                                        else
                                        {
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_cc_tank = DateTime.Now;
                                            //SUPPLY_END
                                            Tank_Supply_End(tank_class.enum_tank_type.TANK_B);
                                            Seq_Supply_Cur_To_Next(Program.seq.supply.no_cur, tank_class.enum_seq_no_supply.TANK_READY_CHECK, "");
                                        }

                                    }

                                }

                            }
                            else if (Program.seq.supply.ready_flag == true && Program.seq.supply.c_c_need == false)
                            {
                                if (Program.seq.supply.rep_c_c_start_cds_to_ctc == true)
                                {
                                    Program.seq.supply.req_c_c_start_cds_to_ctc = false; Program.seq.supply.rep_c_c_start_cds_to_ctc = false;
                                    Program.seq.supply.ready_flag = false;
                                }
                                else
                                {
                                    if (Program.seq.supply.ready_flag_in_req_send == false)
                                    {
                                        if (Program.seq.supply.concentration_measuring == false)
                                        {
                                            //농도 측정 중이 아닐 때만 CC 시도
                                            if (Program.seq.supply.req_c_c_start_cds_to_ctc == false)
                                            {
                                                //각 Supply당 1회만 전송한다.
                                                if ((DateTime.Now - Program.seq.supply.dt_start_cc_start_req_cds_to_ctc).TotalSeconds >= 3)
                                                {
                                                    Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "C.C Need" + "", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                                    Program.seq.supply.dt_start_cc_start_req_cds_to_ctc = DateTime.Now;
                                                    //Program.CTC.Message_Chemical_Change_Request_400();
                                                    Program.CTC.Message_Check_Availability_408();
                                                    Program.seq.supply.ready_flag_in_req_send = true;
                                                }
                                            }
                                        }
                                    }

                                    if (Program.seq.supply.req_c_c_start_cds_to_ctc == true && Program.seq.supply.rep_c_c_start_cds_to_ctc == false)
                                    {
                                        if ((DateTime.Now - Program.seq.supply.dt_start_cc_start_req_cds_to_ctc).TotalSeconds >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chemical_Change_Confirm_Time_Out))
                                        {
                                            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Over_Time_Chemical_Change_Request, "", true, false);
                                        }
                                    }
                                }

                            }
                            else
                            {

                            }

                            ///원액 Refill Check
                            if (Chemical_Original_Check() == true)
                            {
                                if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_A &&
                                    (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.REFILL))
                                {

                                    tmp_refill_start = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Refill_Start_Level);
                                    tmp_refill_end = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Refill_End_Level);

                                    //원액 Type일 때 Refil 체크 Start Level -> End Level 까지
                                    if (tmp_refill_start == 0) { tmp_refill_start = 3; }
                                    if (tmp_refill_start == 1) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value; }//LL
                                    else if (tmp_refill_start == 2) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value; }//L
                                    else if (tmp_refill_start == 3) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value; }//M
                                    else if (tmp_refill_start == 4) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value; }//H
                                    //else if (tmp_refill_start == 5) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_HH].value; }//HH

                                    if (tmp_dio == false)
                                    {
                                        if (Program.seq.supply.refill_run_state == false)
                                        {
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Refill Start -> " + "Tank A", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.REFILL;
                                            Program.seq.supply.refill_run_state = true;
                                            //Tank B가 Charge중 일 때 일시 중지
                                            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete == false)
                                            {
                                                CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS1);
                                            }
                                            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete == false)
                                            {
                                                CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS2);
                                            }
                                            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete == false)
                                            {
                                                CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS3);
                                            }
                                            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete == false)
                                            {
                                                CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_B, enum_ccss.CCSS4);
                                            }
                                            System.Threading.Thread.Sleep(2000);
                                            //일정 시간 Delay 후 Refill 시작

                                            //2022-12-15
                                            //원액 Test를 위해 강제로 CCSS1 -> CCSS4 임시 변경
                                            CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_A, Program.cg_mixing_step.refill_ccss);
                                        }
                                    }


                                    if (tmp_refill_end == 0) { tmp_refill_end = 3; }
                                    if (tmp_refill_end == 1) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value; }//LL
                                    else if (tmp_refill_end == 2) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value; }//L
                                    else if (tmp_refill_end == 3) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value; }//M
                                    else if (tmp_refill_end == 4) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value; }//H
                                    //else if (tmp_refill_end == 5) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_HH].value; }//HH

                                    if (tmp_dio == true)
                                    {
                                        if (Program.seq.supply.refill_run_state == true)
                                        {
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].status = tank_class.enum_tank_status.SUPPLY;
                                            Program.seq.supply.refill_run_state = false;
                                            CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_A, Program.cg_mixing_step.refill_ccss);
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Refill Stop -> " + "Tank A", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                            System.Threading.Thread.Sleep(2000);
                                        }
                                    }


                                }
                                else if (Program.seq.supply.cur_tank == tank_class.enum_tank_type.TANK_B &&
                                    (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.REFILL))
                                {
                                    tmp_refill_start = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Refill_Start_Level);
                                    tmp_refill_end = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Refill_End_Level);
                                    //원액 Type일 때 Refil 체크 Start Level -> End Level 까지
                                    if (tmp_refill_start == 0) { tmp_refill_start = 3; }
                                    if (tmp_refill_start == 1) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value; }//LL
                                    else if (tmp_refill_start == 2) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value; }//L
                                    else if (tmp_refill_start == 3) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value; }//M
                                    else if (tmp_refill_start == 4) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value; }//H
                                    //else if (tmp_refill_start == 5) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_HH].value; }//HH

                                    if (tmp_dio == false)
                                    {
                                        if (Program.seq.supply.refill_run_state == false)
                                        {
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Refill Start -> " + "Tank B", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.REFILL;
                                            Program.seq.supply.refill_run_state = true;
                                            //Tank B가 Charge중 일 때 일시 중지
                                            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete == false)
                                            {
                                                CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS1);
                                            }
                                            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete == false)
                                            {
                                                CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS2);
                                            }
                                            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete == false)
                                            {
                                                CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS3);
                                            }
                                            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete == false)
                                            {
                                                CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_A, enum_ccss.CCSS4);
                                            }
                                            System.Threading.Thread.Sleep(2000);
                                            //일정 시간 Delay 후 Refill 시작
                                            CCSS_INPUT_START_FORCE(tank_class.enum_tank_type.TANK_B, Program.cg_mixing_step.refill_ccss);
                                        }
                                    }

                                    if (tmp_refill_end == 0) { tmp_refill_end = 3; }
                                    if (tmp_refill_end == 1) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value; }//LL
                                    else if (tmp_refill_end == 2) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value; }//L
                                    else if (tmp_refill_end == 3) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value; }//M
                                    else if (tmp_refill_end == 4) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value; }//H
                                    //else if (tmp_refill_end == 5) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_HH].value; }//HH

                                    if (tmp_dio == true)
                                    {
                                        if (Program.seq.supply.refill_run_state == true)
                                        {
                                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].status = tank_class.enum_tank_status.SUPPLY;
                                            Program.seq.supply.refill_run_state = false;
                                            CCSS_INPUT_END_FORCE(tank_class.enum_tank_type.TANK_B, Program.cg_mixing_step.refill_ccss);
                                            Program.eventlog_form.Insert_Event("SEQ SUPPLY : " + "Refill Stop -> " + "Tank B", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                                            System.Threading.Thread.Sleep(2000);

                                        }
                                    }


                                }

                            }
                        }

                        //Tank Supply된 후 최대 사용 시간 Check
                        Tank_Use_TimeOut_Check();

                        if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.REFILL
                            || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.REFILL)
                        {
                        }
                        else
                        {
                            //Tank A, Tank B Supply가 아닐 때 CTC Supply 신호 대기 
                            if (Program.seq.supply.ready_flag == false)
                            {
                                //Seq_Supply_Cur_To_Next((Program.seq.supply.no_cur), tank_class.enum_seq_no_supply.TANK_READY_CHECK, "");
                            }
                        }
                        //Sequnce Sub No가 변경될 때만 Seq 변경 로그 생성
                        if (Program.seq.supply.no_cur != Program.seq.supply.no_old)
                        {
                            Program.log_md.LogWrite(Program.seq.supply.state_display + " : " + Program.seq.supply.state_display2, Module_Log.enumLog.SEQ_SUPPLY, "", Module_Log.enumLevel.ALWAYS);
                        }
                        Program.seq.supply.no_old = Program.seq.supply.no_cur;
                    }

                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + "." + "Seq_Supply" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
                System.Threading.Thread.Sleep(20);
            }
        }
        public void Circulation_Need()
        {
            int tmp_para_value = 0;
            bool tmp_dio = false;
            //Circulation Level 조건이 되었을 때 순환 요청
            tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Tank_Circulation_Level);

            if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A &&
                (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.CHARGE))
            {
                if (tmp_para_value == 0) { tmp_para_value = 2; }
                if (tmp_para_value == 1) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value; }//LL
                else if (tmp_para_value == 2) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value; }//L
                else if (tmp_para_value == 3) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value; }//M
                else if (tmp_para_value == 4) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value; }//H
                else if (tmp_para_value == 5) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_HH].value; }//HH

                //Valve ON AV-21, AV-23
                //Valve OFF AV-25(3Way)
                if (tmp_dio == true)
                {
                    if (Program.seq.cir_start == false)
                    {
                        if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == false)
                        {

                            System.Threading.Thread.Sleep(3000);
                            //순환 요청 전 Circulation Line을 사용하고 있는지 확인 후 Circulation 수행
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].circulation_processing = true;
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].circulation_processing = false;
                            Program.seq.cir_start = true;
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_A, true);
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, true);
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_TANK_A, true);
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, true);
                            ///Dsp Mix의 내부 순환 및 Heating은 Heat Exchanger에서 수행한다.
                            if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                            {
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_HE_UNIT, true);
                            }
                            System.Threading.Thread.Sleep(1000);
                            //PUMP BP-21 RUN
                            CIRCULATION_PUMP_ON_OFF(true);
                            Program.eventlog_form.Insert_Event("SEQ : " + "Circulation Tank A Start", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                            Sequence_Log_Add(tank_class.enum_seq_type.NONE, "Thread : Circulation_Need -> Cir Start", "Tank A - Circulation Start");
                        }
                        else
                        {
                            //Seq_Cur_To_Next((Program.seq.main.no_cur), tank_class.enum_seq_no.CIRCULATION_READY, "Another Tank Used Circulation");
                        }
                    }
                    //CIR PUMP가 동작 후 Heater ON
                    if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                    {
                        if (Program.seq.circulation_on_req == true)
                        {
                            //1회만 수행
                            Program.seq.circulation_on_req = false;
                            System.Threading.Thread.Sleep(Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Heater_On_Dleay_Time_Tank_Circulation) * 1000);
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_start_Circulation_Heater_on = DateTime.Now;
                            Program.eventlog_form.Insert_Event("SEQ : " + "Circulation Tank A Heater ON REQ", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                            ///Dsp Mix의 내부 순환 및 Heating은 Heat Exchanger에서 수행한다.
                            if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                            {
                                CIRCULATION_Heat_Exchanger_ON_OFF(true);
                                Sequence_Log_Add(tank_class.enum_seq_type.NONE, "Thread : Heat Exchanger Circulation_Need -> Heater On", "Tank A - Circulation Start");
                            }
                            else
                            {
                                CIRCULATION_1_HEATER_ON_OFF(true);
                                Sequence_Log_Add(tank_class.enum_seq_type.NONE, "Thread : Circulation_Need -> Heater On", "Tank A - Circulation Heater1,2 Start");
                            }

                        }
                    }
                }

            }
            else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B &&
                (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.CHARGE))
            {
                if (tmp_para_value == 0) { tmp_para_value = 2; }
                if (tmp_para_value == 1) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value; }//LL
                else if (tmp_para_value == 2) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value; }//L
                else if (tmp_para_value == 3) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value; }//M
                else if (tmp_para_value == 4) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value; }//H
                else if (tmp_para_value == 5) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_HH].value; }//HH

                //Valve ON AV-22, AV-24
                //Valve OFF AV-25(3Way)
                if (tmp_dio == true)
                {
                    if (Program.seq.cir_start == false)
                    {
                        if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == false)
                        {
                            System.Threading.Thread.Sleep(3000);
                            //순환 요청 전 Circulation Line을 사용하고 있는지  확인 후 Circulation 수행
                            Program.tank[(int)tank_class.enum_tank_type.TANK_A].circulation_processing = false;
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].circulation_processing = true;
                            Program.seq.cir_start = true;
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_B, true);
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, true);
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_TANK_B, true);
                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.MAIN_RETURN_SAMPLE_1, true);
                            ///Dsp Mix의 내부 순환 및 Heating은 Heat Exchanger에서 수행한다.
                            if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                            {
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_TO_HE_UNIT, true);
                            }
                            System.Threading.Thread.Sleep(1000);
                            //PUMP BP-21 RUN
                            CIRCULATION_PUMP_ON_OFF(true);
                            Program.eventlog_form.Insert_Event("SEQ : " + "Circulation Tank B Start", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                            Sequence_Log_Add(tank_class.enum_seq_type.NONE, "Thread : Circulation_Need -> Cir Start", "Tank B - Circulation Start");
                        }
                        else
                        {
                        }
                    }
                    //CIR PUMP가 동작 후 Heater ON
                    if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                    {
                        if (Program.seq.circulation_on_req == true)
                        {
                            //1회만 수행
                            Program.seq.circulation_on_req = false;
                            System.Threading.Thread.Sleep(Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Heater_On_Dleay_Time_Tank_Circulation) * 1000);
                            Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_start_Circulation_Heater_on = DateTime.Now;
                            Program.eventlog_form.Insert_Event("SEQ : " + "Circulation Tank B Heater ON REQ", (int)frm_eventlog.enum_event_type.SEQ_SUB, (int)frm_eventlog.enum_occurred_type.SYSTEM, true);
                            ///Dsp Mix의 내부 순환 및 Heating은 Heat Exchanger에서 수행한다.
                            if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                            {
                                CIRCULATION_Heat_Exchanger_ON_OFF(true);
                                Sequence_Log_Add(tank_class.enum_seq_type.NONE, "Thread : Heat Exchanger Circulation_Need -> Heater On", "Tank B - Circulation Start");
                            }
                            else
                            {
                                CIRCULATION_1_HEATER_ON_OFF(true);
                                Sequence_Log_Add(tank_class.enum_seq_type.NONE, "Thread : Circulation_Need -> Heater On", "Tank B - Circulation Heater1,2 Start");
                            }

                        }
                    }
                }



            }

        }
        public void Seq_Circulation_Pump_Control()
        {
            int default_sleep = 100;
            int left_sensor_on_cnt = 0, right_sensor_on_cnt = 0;
            int sol_wait_cnt = 0, sol_wait_time = 0;

            try
            {
                while (true)
                {
                    Program.seq.pumpcontrol.last_act_span = DateTime.Now - Program.seq.pumpcontrol.last_act_time;

                    if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY)
                    {

                    }
                    else
                    {
                        if (Program.cg_app_info.circulation_pump_mode == enum_pump_mode.none)
                        {
                            if (Program.schematic_form.circulation_pump_run_interval != 0)
                            {
                                if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                                {
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_LEFT_ON, true);
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_RIGHT_ON, false);
                                    System.Threading.Thread.Sleep(circulation_pump_run_interval);
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_LEFT_ON, false);
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_RIGHT_ON, true);
                                    System.Threading.Thread.Sleep(circulation_pump_run_interval);
                                }
                            }
                            else
                            {
                                if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                                {
                                    switch (Program.seq.pumpcontrol.no_cur)
                                    {
                                        case tank_class.enum_seq_no_pump_control.READY:
                                            if (Program.seq.pumpcontrol.last_act_span.TotalMilliseconds >= default_sleep)
                                            {
                                                Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.INITIAL, "");

                                            }
                                            break;
                                        case tank_class.enum_seq_no_pump_control.INITIAL:

                                            if (Program.seq.pumpcontrol.last_act_span.TotalMilliseconds >= default_sleep)
                                            {
                                                //Sol Control Error 확인 변수
                                                Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.sol_control_error = false;
                                                //Sol 제어 변수 파라메터 대입
                                                sol_wait_time = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Pump_Sensor_Wait_Delay_Tank_Circulation) * 1000;
                                                sol_wait_cnt = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Pump_Sensor_Wait_Count);

                                                left_sensor_on_cnt = 0;
                                                right_sensor_on_cnt = 0;
                                                Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.CYCLE_START, "");
                                            }
                                            break;

                                        case tank_class.enum_seq_no_pump_control.CYCLE_START:

                                            if (Program.seq.pumpcontrol.last_act_span.TotalMilliseconds >= default_sleep)
                                            {
                                                Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.LEFT_ON_ACT, "");

                                            }
                                            break;

                                        case tank_class.enum_seq_no_pump_control.LEFT_ON_ACT:

                                            if (Program.seq.pumpcontrol.last_act_span.TotalMilliseconds >= default_sleep)
                                            {
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_LEFT_ON, true);
                                                Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.RIGHT_ON_WAIT, "");
                                            }
                                            break;

                                        case tank_class.enum_seq_no_pump_control.RIGHT_ON_WAIT:

                                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_RIGHT_ON].value == true)
                                            {
                                                right_sensor_on_cnt = 0;
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_LEFT_ON, false);
                                                Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.RIGHT_ON_ACT, "");
                                                if (Program.cg_app_info.mode_simulation.use == true)
                                                {
                                                    System.Threading.Thread.Sleep(1000);
                                                    Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_RIGHT_ON].value = false;
                                                }
                                            }
                                            else
                                            {
                                                if (Program.cg_app_info.mode_simulation.use == true)
                                                {
                                                    System.Threading.Thread.Sleep(1000);
                                                    Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_RIGHT_ON].value = true;
                                                }
                                                else
                                                {
                                                    if (Program.seq.pumpcontrol.last_act_span.TotalMilliseconds >= sol_wait_time)
                                                    {
                                                        right_sensor_on_cnt = right_sensor_on_cnt + 1;
                                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_LEFT_ON, false);
                                                        Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.RIGHT_ON_ACT, "");
                                                    }
                                                }
                                            }


                                            break;

                                        case tank_class.enum_seq_no_pump_control.RIGHT_ON_ACT:
                                            if (Program.seq.pumpcontrol.last_act_span.TotalMilliseconds >= default_sleep)
                                            {
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_RIGHT_ON, true);
                                                Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.LEFT_ON_WAIT, "");
                                            }
                                            break;

                                        case tank_class.enum_seq_no_pump_control.LEFT_ON_WAIT:

                                            if (Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_LEFT_ON].value == true)
                                            {
                                                left_sensor_on_cnt = 0;
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_RIGHT_ON, false);
                                                Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.CYCLE_COMPLETE, "");
                                                if (Program.cg_app_info.mode_simulation.use == true)
                                                {
                                                    System.Threading.Thread.Sleep(1000);
                                                    Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_LEFT_ON].value = false;
                                                }

                                            }
                                            else
                                            {
                                                if (Program.cg_app_info.mode_simulation.use == true)
                                                {
                                                    System.Threading.Thread.Sleep(1000);
                                                    Program.IO.DI.Tag[(int)Config_IO.enum_di.CIRCULATION_PUMP_LEFT_ON].value = true;
                                                }
                                                else
                                                {
                                                    if (Program.seq.pumpcontrol.last_act_span.TotalMilliseconds >= sol_wait_time)
                                                    {
                                                        left_sensor_on_cnt = left_sensor_on_cnt + 1;
                                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_RIGHT_ON, false);
                                                        Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.CYCLE_COMPLETE, "");
                                                    }
                                                }


                                            }

                                            break;

                                        case tank_class.enum_seq_no_pump_control.CYCLE_COMPLETE:

                                            if (Program.seq.pumpcontrol.last_act_span.TotalMilliseconds >= default_sleep)
                                            {
                                                if ((left_sensor_on_cnt >= sol_wait_cnt) || (right_sensor_on_cnt >= sol_wait_cnt))
                                                {
                                                    //SOL 동작 이상
                                                    Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.sol_control_error = true;
                                                    Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.CYCLE_ERROR, "");
                                                }
                                                else
                                                {
                                                    Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.CYCLE_START, "");
                                                }
                                            }
                                            break;

                                        case tank_class.enum_seq_no_pump_control.CYCLE_ERROR:
                                            if (Program.seq.pumpcontrol.last_act_span.TotalMilliseconds >= default_sleep)
                                            {
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Prep_Pump_Operation_Error, "", true, false);
                                                //Seq_Pump_Control_Cur_To_Next((Program.seq.pumpcontrol.no_cur), tank_class.enum_seq_no_pump_control.INITIAL, "");
                                            }
                                            break;

                                        case tank_class.enum_seq_no_pump_control.ERROR_BY_ALARM:
                                            if (Program.seq.pumpcontrol.last_act_span.TotalMilliseconds >= default_sleep)
                                            {

                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_LEFT_ON, false);
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_RIGHT_ON, false);
                                    Seq_Pump_Control_Cur_To_Next(Program.seq.pumpcontrol.no_cur, tank_class.enum_seq_no_pump_control.READY, "");
                                }
                            }
                        }
                        else if (Program.cg_app_info.circulation_pump_mode == enum_pump_mode.type1)
                        {
                            if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
                            {
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_START, true);
                            }
                            else
                            {
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_START, false);
                            }
                        }

                    }

                    //Sequnce Sub No가 변경될 때만 Seq 변경 로그 생성
                    if (Program.seq.pumpcontrol.no_cur != Program.seq.pumpcontrol.no_old)
                    {
                        Program.log_md.LogWrite(Program.seq.pumpcontrol.state_display + " : " + Program.seq.pumpcontrol.state_display2, Module_Log.enumLog.SEQ_PUMP_CONTROL, "", Module_Log.enumLevel.ALWAYS);
                    }
                    Program.seq.pumpcontrol.no_old = Program.seq.pumpcontrol.no_cur;

                    System.Threading.Thread.Sleep(20);
                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Seq_Pump_Control" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }


        }
        public void Seq_Circulation_Monitoring()
        {
            try
            {
                while (true)
                {

                    for (int idx_monitor = 0; idx_monitor < 1; idx_monitor++)
                    {
                        switch (idx_monitor)
                        {
                            case 0:
                                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
                                {
                                    if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY)
                                    {

                                    }
                                    else
                                    {
                                        if (timer_manual_sequence_tank_a.Enabled == false && timer_manual_sequence_tank_b.Enabled == false)
                                        {
                                            if (Program.cg_app_info.eq_type != enum_eq_type.ipa)
                                            {
                                                Circulation_Need();
                                            }
                                        }
                                    }

                                }
                                break;
                        }
                    }

                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Seq_Circulation_Monitoring" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        public void Seq_Drain_Valve_Monitoring()
        {
            int tmp_para_value = 0;
            bool tmp_dio = false;

            try
            {
                while (true)
                {
                    tmp_para_value = Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chemical_Drain_Start_Level);

                    //Semi Auto에서 Supply Drain이 아닌 일반 Drain Mode(DIW Flush, Chem Flush)에서만 동작

                    if (timer_manual_sequence_tank_a.Enabled == true)
                    {
                        if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].use_drain_seq_by_semiauto == false)
                        {
                            return;
                        }
                    }
                    else if (timer_manual_sequence_tank_b.Enabled == true)
                    {
                        if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].use_drain_seq_by_semiauto == false)
                        {
                            return;
                        }
                    }
                    if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY)
                    {

                    }
                    else
                    {
                        if (Program.cg_app_info.eq_mode == enum_eq_mode.auto || timer_manual_sequence_tank_a.Enabled == true || timer_manual_sequence_tank_b.Enabled == true)
                        {


                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.DRAIN || Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.DRAIN_WAIT)
                            {
                                //약액 투입중에는 Drain하지 않는다. Interlock
                                if (CCSS_INPUT_Status(tank_class.enum_tank_type.TANK_A) == false)
                                {
                                    if (tmp_para_value == 0) { tmp_para_value = 3; }
                                    if (tmp_para_value == 1) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value; }//LL
                                    else if (tmp_para_value == 2) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value; }//L
                                    else if (tmp_para_value == 3) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_M].value; }//M
                                    else if (tmp_para_value == 4) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_H].value; }//H
                                    else if (tmp_para_value == 5) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_HH].value; }//HH

                                    if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_LL].value == false)
                                    {
                                        if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == true)
                                        {
                                            if ((DateTime.Now - dt_start_tank_a_level_lowlow).TotalSeconds > Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Tank_A_Empty_Sensor_Monitoring_Value))
                                            {
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Level_Empty_Fail_Tank_A, "", true, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dt_start_tank_a_level_lowlow = DateTime.Now;
                                    }
                                    //Empty일 경우 Drain 중지
                                    if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                                    {
                                        Seq_Drain_Valve_Monitoring_tank_a_log_cur = "Tank A Drain Stop";
                                        //Empty일때
                                        CIRCULATION_PUMP_ON_OFF(false);
                                        System.Threading.Thread.Sleep(1000);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_A, false);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, true);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, false);
                                    }
                                    else
                                    {
                                        Tank_Drain_Start_tank_a = true;
                                        if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
                                        {
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_A, false);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, true);
                                            Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, true);
                                            System.Threading.Thread.Sleep(1000);
                                            CIRCULATION_PUMP_ON_OFF(false);
                                        }
                                        else
                                        {
                                            //수위가 Cir Drain Level이며, 다른 Tank의 Circulation Line이 사용 중이면 대기한다.
                                            if (tmp_dio == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true)
                                            {
                                                Seq_Drain_Valve_Monitoring_tank_a_log_cur = "Tank A Drain Ready";
                                                //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_A, false);
                                                //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, true);
                                                //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, true);
                                                //System.Threading.Thread.Sleep(1000);
                                                //CIRCULATION_PUMP_ON_OFF(false);
                                            }
                                            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == false)
                                            {
                                                //System.Threading.Thread.Sleep(500);
                                                //Prameter 수위보다 높을 때 Cir Drain 진행

                                                if (tmp_dio == true)
                                                {
                                                    Seq_Drain_Valve_Monitoring_tank_a_log_cur = "Tank A Drain Start(Chemical Drain)";
                                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_A, true);
                                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, false);
                                                    System.Threading.Thread.Sleep(1000);
                                                    CIRCULATION_PUMP_ON_OFF(true);
                                                    chemical_drain_on = true;
                                                }
                                                else
                                                {
                                                    Seq_Drain_Valve_Monitoring_tank_a_log_cur = "Tank A Drain Start(Tank Drain)";
                                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_A, false);
                                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, true);
                                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, true);
                                                    System.Threading.Thread.Sleep(1000);
                                                    CIRCULATION_PUMP_ON_OFF(false);
                                                    chemical_drain_off = false;

                                                }

                                            }
                                        }


                                    }
                                }
                                else
                                {
                                    Seq_Drain_Valve_Monitoring_tank_a_log_cur = "Tank A Drain Stop(CCSS INPUT Processing)";
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, false);
                                }

                            }
                            //else if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.CHARGE)
                            else
                            {
                                Seq_Drain_Valve_Monitoring_tank_a_log_cur = "Tank A Drain Stop";
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_A_DRAIN, false);
                            }

                            if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.DRAIN || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.DRAIN_WAIT)
                            {
                                //약액 투입중에는 Drain하지 않는다. Interlock
                                if (CCSS_INPUT_Status(tank_class.enum_tank_type.TANK_B) == false)
                                {
                                    if (tmp_para_value == 0) { tmp_para_value = 3; }
                                    if (tmp_para_value == 1) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value; }//LL
                                    else if (tmp_para_value == 2) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value; }//L
                                    else if (tmp_para_value == 3) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_M].value; }//M
                                    else if (tmp_para_value == 4) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_H].value; }//H
                                    else if (tmp_para_value == 5) { tmp_dio = Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_HH].value; }//HH

                                    if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_LL].value == false)
                                    {
                                        if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == true)
                                        {
                                            if ((DateTime.Now - dt_start_tank_b_level_lowlow).TotalSeconds > Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Tank_B_Empty_Sensor_Monitoring_Value))
                                            {
                                                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Level_Empty_Fail_Tank_B, "", true, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dt_start_tank_b_level_lowlow = DateTime.Now;
                                    }

                                    //Empty일 경우 Drain 중지
                                    if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                                    {
                                        //Empty일때
                                        Seq_Drain_Valve_Monitoring_tank_b_log_cur = "Tank B Drain Stop";
                                        CIRCULATION_PUMP_ON_OFF(false);
                                        System.Threading.Thread.Sleep(1000);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_B, false);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, true);
                                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, false);
                                    }
                                    else
                                    {
                                        Tank_Drain_Start_tank_b = true;                                    //Empty가 아닐 때
                                                                                                           //수위가 Cir Drain Level이며, 다른 Tank의 Circulation Line이 사용 중이면 대기한다.
                                        if (tmp_dio == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true)
                                        {
                                            Seq_Drain_Valve_Monitoring_tank_b_log_cur = "Tank B Drain Ready";
                                        }
                                        else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == false)
                                        {
                                            //System.Threading.Thread.Sleep(500);
                                            //Prameter 수위보다 높을 때 Cir Drain 진행
                                            if (tmp_dio == true)
                                            {
                                                Seq_Drain_Valve_Monitoring_tank_b_log_cur = "Tank B Drain Start(Chemical Drain)";
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_B, true);
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, false);
                                                System.Threading.Thread.Sleep(1000);
                                                CIRCULATION_PUMP_ON_OFF(true);
                                            }
                                            else
                                            {
                                                Seq_Drain_Valve_Monitoring_tank_b_log_cur = "Tank B Drain Start(Tank Drain)";
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_FROM_TANK_B, false);
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIR_DRAIN, true);
                                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, true);
                                                System.Threading.Thread.Sleep(1000);
                                                CIRCULATION_PUMP_ON_OFF(false);

                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    Seq_Drain_Valve_Monitoring_tank_b_log_cur = "Tank B Drain Stop";
                                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, false);
                                }

                            }
                            //else if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.CHARGE)
                            else
                            {
                                Seq_Drain_Valve_Monitoring_tank_b_log_cur = "Tank B Drain Stop(CCSS INPUT Processing)";
                                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TANK_B_DRAIN, false);
                            }

                        }
                        else
                        {
                            Seq_Drain_Valve_Monitoring_tank_a_log_cur = "Status Manual(Drain Seq Stop)";
                            Seq_Drain_Valve_Monitoring_tank_b_log_cur = "Status Manual(Drain Seq Stop)";
                        }


                    }
                    if (Seq_Drain_Valve_Monitoring_tank_a_log_cur != Seq_Drain_Valve_Monitoring_tank_a_log_old)
                    {
                        Program.log_md.LogWrite(Seq_Drain_Valve_Monitoring_tank_a_log_cur, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                    }
                    if (Seq_Drain_Valve_Monitoring_tank_b_log_cur != Seq_Drain_Valve_Monitoring_tank_b_log_old)
                    {
                        Program.log_md.LogWrite(Seq_Drain_Valve_Monitoring_tank_b_log_cur, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS);
                    }
                    Seq_Drain_Valve_Monitoring_tank_a_log_old = Seq_Drain_Valve_Monitoring_tank_a_log_cur;
                    Seq_Drain_Valve_Monitoring_tank_b_log_old = Seq_Drain_Valve_Monitoring_tank_b_log_cur;
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Seq_Circulation_Monitoring" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }


        }
        /// <summary>
        /// Interlock, Alarm Signal 출력 등 모니터링 Sequence
        /// </summary>
        public void Seq_Monitoring()
        {
            int last_act_idx = 0;
            try
            {
                while (true)
                {
                    for (int idx_monitor = 0; idx_monitor < 15; idx_monitor++)
                    {
                        last_act_idx = idx_monitor;
                        switch (idx_monitor)
                        {
                            case 0:
                                if (Program.cg_app_info.eq_type != enum_eq_type.ipa) { Drain_Tank_Monitoring(); }
                                if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix) { CM_Drain_Tank_Monitoring(); }
                                break;
                            case 1:
                                Manual_Valve_Close_By_TotalUsage_Over();
                                break;
                            case 2:
                                TotalUsage_Calculation_By_Software();
                                TotalUsage_Calculation_Sonotec_By_Software();

                                break;

                            case 3:
                                Circulation_Heater_Interlock_And_Run_Req();
                                Supply_A_Heater_Interlock_And_Run_Req();
                                Supply_B_Heater_Interlock_And_Run_Req();
                                break;
                            case 4:
                                Circulation_Pump_Interlock();
                                Supply_A_Pump_Interlock();
                                Supply_B_Pump_Interlock();
                                break;
                            case 5:
                                //LL Touch 후 전체 Pump Heater 동작 중지 L Sensor 이상 감지 해야 Flag 복구됨
                                Tank_Low_Level_Check();
                                break;
                            case 6:
                                HotDiw_Manage();
                                CCSS_USE_Signal_OUT();
                                break;
                            case 7:
                                Alarm_Level_Check_AND_TOWER_LAMP_Interlock_By_Level_Heavy();
                                break;
                            case 8:
                                Interlock_Check();
                                break;
                            case 9:
                                Tank_Can_Not_Use_Status_Check();
                                break;
                            case 10:
                                Empty_Check_And_TotalUsage_Reset();
                                break;
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + "." + "Seq_Monitoring No(" + last_act_idx + ")" + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        public void Circulation_Heater_Interlock_And_Run_Req()
        {
            string result = "";

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIRCULATION_THERMOSTAT_PWR_ON].use == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.CIRCULATION_THERMOSTAT_PWR_ON].value == false)
            {
                Program.main_form.SerialData.Circulation_Thermostat.heater_on = false;
                Program.main_form.SerialData.Circulation_Thermostat.run_state = false;
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIRCULATION_THERMOSTAT_PWR_ON].use == true && Program.main_form.SerialData.Circulation_Thermostat.heater_on == true)
            {

                if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value >=
                           Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Tank_Circulation)
                           && Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value
                           <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Tank_Circulation))
                {
                    if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value >=
                           Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Tank_Circulation)
                           && Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value
                           <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Tank_Circulation))
                    {

                        if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.CIRCULATION_THERMOSTAT] == true)
                        {

                        }

                    }
                    else
                    {
                        //Heater Interlock
                        result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "CIRCULATION Heater Stop Interlock : because Circulation Status(flow : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value + ")";
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Heater_Interlock_Circulation, result, true, false);
                        //CIRCULATION_1_HEATER_ON_OFF(false); CIRCULATION_2_HEATER_ON_OFF(false);
                    }
                }

                else
                {
                    //Heater Interlock
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "CIRCULATION Heater Stop Interlock : because Circulation Status(Press : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_PRESS].value + ")";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Heater_Interlock_Circulation, "", true, false);
                    //CIRCULATION_1_HEATER_ON_OFF(false); CIRCULATION_2_HEATER_ON_OFF(false);
                }

                if (result != "") { Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS); }
            }
            else
            {
                Program.main_form.SerialData.Circulation_Thermostat.flag_run_req = false;
                //result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "CIRCULATION_1_HEATER_ON_OFF : Cannot Start because Pump Status OR Valve / ";
            }

        }
        public void Supply_A_Heater_Interlock_And_Run_Req()
        {
            string result = "";

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_A_THERMOSTAT_PWR_ON].use == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_A_THERMOSTAT_PWR_ON].value == false)
            {
                Program.main_form.SerialData.Supply_A_Thermostat.heater_on = false;
                Program.main_form.SerialData.Supply_A_Thermostat.run_state = false;
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_A_THERMOSTAT_PWR_ON].use == true && Program.main_form.SerialData.Supply_A_Thermostat.heater_on == true)
            {

                if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS].value >=
                           Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Supply_A_IN)
                           && Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FILTER_IN_PRESS].value
                           <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Supply_A_IN))
                {
                    if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value >=
                           Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_A)
                           && Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value
                           <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_A))
                    {
                        if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.SUPPLY_A_THERMOSTAT] == true)
                        {

                        }
                    }
                    else
                    {
                        //Heater Interlock
                        result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "Supply A Heater Stop Interlock : because Circulation Status(flow : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value + ")";
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Heater_Interlock_Tank_A, "", true, false);
                        //SUPPLY_A_HEATER_ON_OFF(false);
                    }
                }

                else
                {
                    //Heater Interlock
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "Supply A Heater Stop Interlock : because Circulation Status(Press : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value + ")";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Heater_Interlock_Tank_A, "", true, false);
                    //SUPPLY_A_HEATER_ON_OFF(false);
                }

                if (result != "") { Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS); }
            }
            else
            {
                Program.main_form.SerialData.Supply_A_Thermostat.flag_run_req = false;
                //result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "CIRCULATION_1_HEATER_ON_OFF : Cannot Start because Pump Status OR Valve / ";
            }

        }
        public void Supply_B_Heater_Interlock_And_Run_Req()
        {
            string result = "";

            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_B_THERMOSTAT_PWR_ON].use == true && Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_B_THERMOSTAT_PWR_ON].value == false)
            {
                Program.main_form.SerialData.Supply_B_Thermostat.heater_on = false;
                Program.main_form.SerialData.Supply_B_Thermostat.run_state = false;
            }
            else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_B_THERMOSTAT_PWR_ON].use == true && Program.main_form.SerialData.Supply_B_Thermostat.heater_on == true)
            {

                if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS].value >=
                           Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_Low_Supply_B_IN)
                           && Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FILTER_IN_PRESS].value
                           <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Pressure_High_Supply_B_IN))
                {
                    if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value >=
                           Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_B)
                           && Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value
                           <= Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_B))
                    {

                        if (Program.main_form.serial_port_state[(int)Config_IO.enum_dsp_serial_index.SUPPLY_B_THERMOSTAT] == true)
                        {

                        }

                    }
                    else
                    {
                        //Heater Interlock
                        result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "Supply B Heater Stop Interlock : because Circulation Status(flow : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value + ")";
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Heater_Interlock_Tank_B, "", true, false);
                        //SUPPLY_B_HEATER_ON_OFF(false);
                    }
                }

                else
                {
                    //Heater Interlock
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "Supply B Heater Stop Interlock : because Circulation Status(Press : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value + ")";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Heater_Interlock_Tank_B, "", true, false);
                    //SUPPLY_B_HEATER_ON_OFF(false);
                }

                if (result != "") { Program.log_md.LogWrite(result, Module_Log.enumLog.DEBUG, "", Module_Log.enumLevel.ALWAYS); }
            }
            else
            {
                Program.main_form.SerialData.Supply_B_Thermostat.flag_run_req = false;
                //result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "CIRCULATION_1_HEATER_ON_OFF : Cannot Start because Pump Status OR Valve / ";
            }

        }
        public void Circulation_Pump_Interlock()
        {
            if (Program.main_form.SerialData.CIRCULATION_PUMP_CONTROLLER.run_state == true)
            {
                string result = $"EQ Mode {Program.cg_app_info.eq_mode} - CIRCULATION Pump Stop Interlock : because Circulation";
                if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value >
                        Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Tank_Circulation)
                        && Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value
                        < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Tank_Circulation))
                {
                }
                else
                {
                    result += $" Status(flow : {Program.IO.AI.Tag[(int)Config_IO.enum_ai.CIRCULATION_FLOW].value})";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Pump_Empty_Trouble_Tank_Circulation_Pump, result, true, false);
                }

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                {
                    result += " Tank A Empty";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Pump_Empty_Trouble_Tank_Circulation_Pump, result, true, false);
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                {
                    result += " Tank B Empty";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Pump_Empty_Trouble_Tank_Circulation_Pump, result, true, false);
                }
            }

        }
        public void Supply_A_Pump_Interlock()
        {
            string result = "";
            if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == true)
            {
                if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value >
                        Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_A)
                        && Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value
                        < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_A))
                {
                }
                else
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "Supply A Pump Stop Interlock : because Circulation Status(flow : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_A_FLOW].value + ")";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Pump_Empty_Trouble_Supply_A_Pump, result, true, false);
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "Supply A Pump Stop Interlock : because Circulation Tank A Empty";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Pump_Empty_Trouble_Supply_A_Pump, result, true, false);
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "Supply A Pump Stop Interlock : because Circulation Tank B Empty";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Pump_Empty_Trouble_Supply_A_Pump, result, true, false);
                }
            }

        }
        public void Supply_B_Pump_Interlock()
        {
            string result = "";
            if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == true)
            {
                if (Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value >
                        Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_Low_Supply_B)
                        && Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value
                        < Program.parameter_list.Return_Value_To_Float_by_ID((int)frm_parameter.enum_parmater.Flowrate_High_Supply_B))
                {
                }
                else
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "Supply B Pump Stop Interlock : because Circulation Status(flow : " + Program.IO.AI.Tag[(int)Config_IO.enum_ai.SUPPLY_B_FLOW].value + ")";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Pump_Empty_Trouble_Supply_B_Pump, result, true, false);
                }

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "Supply B Pump Stop Interlock : because Circulation Tank A Empty";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Pump_Empty_Trouble_Supply_B_Pump, result, true, false);
                }
                else if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                {
                    result = "EQ Mode " + Program.cg_app_info.eq_mode.ToString() + " - " + "Supply B Pump Stop Interlock : because Circulation Tank B Empty";
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Pump_Empty_Trouble_Supply_B_Pump, result, true, false);
                }
            }

        }
        public void TotalUsage_Calculation_By_Software()
        {
            double cur_flow_by_Seconds = 0;


            if (Program.cg_app_info.eq_type == enum_eq_type.dsp || Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                //cur_flow = string.Format("{0:f1}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_FLOW].value) + " Lpm";
                // Min -> Sec 유속 구한 후 Valve Open ~ Close까지의 시간만큼 누적한다.
                if (Program.cg_app_info.mode_simulation.use == false)
                {
                    Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow = Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_FLOW].value;
                }
                if (Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow == 0)
                {
                    cur_flow_by_Seconds = 0;
                }
                else
                {
                    cur_flow_by_Seconds = Program.main_form.SerialData.FlowMeter_Sonotec.DIW_flow / 60 * (Program.cg_app_info.totalusage_slice_interval * 0.001);
                }

                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DIW_SUPPLY_TANK_B].value == true)
                {
                    if ((DateTime.Now - dt_start_totalusage_diw_by_sonotec).TotalMilliseconds >= Program.cg_app_info.totalusage_slice_interval)
                    {
                        dt_start_totalusage_diw_by_sonotec = DateTime.Now;
                        Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage = Program.main_form.SerialData.FlowMeter_Sonotec.DIW_totalusage + Math.Round(cur_flow_by_Seconds, 3);
                        //Console.WriteLine(DateTime.Now.ToString("ss.fff : ") + cur_flow_by_Seconds);
                    }
                }
                else
                {
                    dt_start_totalusage_diw_by_sonotec = DateTime.Now;
                }
            }
        }
        public void TotalUsage_Calculation_Sonotec_By_Software()
        {
            double cur_flow_by_Seconds = 0;

            if (Program.cg_app_info.eq_type == enum_eq_type.dsp)
            {
                //cur_flow = string.Format("{0:f1}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_FLOW].value) + " Lpm";
                // Min -> Sec 유속 구한 후 Valve Open ~ Close까지의 시간만큼 누적한다.
                if (Program.cg_app_info.mode_simulation.use == false)
                {

                }
                if (Program.main_form.SerialData.FlowMeter_Sonotec.DSP_flow == 0)
                {
                    cur_flow_by_Seconds = 0;
                }
                else
                {
                    cur_flow_by_Seconds = Program.main_form.SerialData.FlowMeter_Sonotec.DSP_flow / 60 * (Program.cg_app_info.totalusage_slice_interval * 0.001);
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.DSP_SUPPLY_TANK_B].value == true)
                {
                    if ((DateTime.Now - dt_start_totalusage_dsp_by_sonotec).TotalMilliseconds >= Program.cg_app_info.totalusage_slice_interval)
                    {
                        dt_start_totalusage_dsp_by_sonotec = DateTime.Now;
                        Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage = Program.main_form.SerialData.FlowMeter_Sonotec.DSP_totalusage + Math.Round(cur_flow_by_Seconds, 3);
                        //Console.WriteLine(DateTime.Now.ToString("ss.fff : ") + cur_flow_by_Seconds);
                    }
                }
                else
                {
                    dt_start_totalusage_dsp_by_sonotec = DateTime.Now;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.ipa)
            {
                //cur_flow = string.Format("{0:f1}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_FLOW].value) + " Lpm";
                // Min -> Sec 유속 구한 후 Valve Open ~ Close까지의 시간만큼 누적한다.
                if (Program.cg_app_info.mode_simulation.use == false)
                {

                }
                if (Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow == 0)
                {
                    cur_flow_by_Seconds = 0;
                }
                else
                {
                    cur_flow_by_Seconds = Program.main_form.SerialData.FlowMeter_Sonotec.IPA_flow / 60 * (Program.cg_app_info.totalusage_slice_interval * 0.001);
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.IPA_SUPPLY_TANK].value == true)
                {
                    if ((DateTime.Now - dt_start_totalusage_ipa_by_sonotec).TotalMilliseconds >= Program.cg_app_info.totalusage_slice_interval)
                    {
                        dt_start_totalusage_ipa_by_sonotec = DateTime.Now;
                        Program.main_form.SerialData.FlowMeter_Sonotec.IPA_totalusage = Program.main_form.SerialData.FlowMeter_Sonotec.IPA_totalusage + Math.Round(cur_flow_by_Seconds, 3);
                        //Console.WriteLine(DateTime.Now.ToString("ss.fff : ") + cur_flow_by_Seconds);
                    }
                }
                else
                {
                    dt_start_totalusage_ipa_by_sonotec = DateTime.Now;
                }
            }
            else if (Program.cg_app_info.eq_type == enum_eq_type.lal)
            {
                //cur_flow = string.Format("{0:f1}", Program.IO.AI.Tag[(int)Config_IO.enum_ai.DIW_SUPPLY_FLOW].value) + " Lpm";
                // Min -> Sec 유속 구한 후 Valve Open ~ Close까지의 시간만큼 누적한다.
                if (Program.cg_app_info.mode_simulation.use == false)
                {

                }
                if (Program.main_form.SerialData.FlowMeter_Sonotec.LAL_flow == 0)
                {
                    cur_flow_by_Seconds = 0;
                }
                else
                {
                    cur_flow_by_Seconds = Program.main_form.SerialData.FlowMeter_Sonotec.LAL_flow / 60 * (Program.cg_app_info.totalusage_slice_interval * 0.001);
                }
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.LAL_SUPPLY_TANK_B].value == true)
                {
                    if ((DateTime.Now - dt_start_totalusage_lal_by_sonotec).TotalMilliseconds >= Program.cg_app_info.totalusage_slice_interval)
                    {
                        dt_start_totalusage_lal_by_sonotec = DateTime.Now;
                        Program.main_form.SerialData.FlowMeter_Sonotec.LAL_totalusage = Program.main_form.SerialData.FlowMeter_Sonotec.LAL_totalusage + Math.Round(cur_flow_by_Seconds, 3);
                        //Console.WriteLine(DateTime.Now.ToString("ss.fff : ") + cur_flow_by_Seconds);
                    }
                }
                else
                {
                    dt_start_totalusage_lal_by_sonotec = DateTime.Now;
                }
            }
        }
        public void Interlock_Check()
        {
            if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
            {
                if (Program.main_form.SerialData.SUPPLY_A_PUMP_CONTROLLER.run_state == false)
                {
                    //Runstate가 False이나 출력은 나가고 있을 때 로그 생성 후 종료
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_PUMP_A_START].value == true)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_PUMP_A_START, false);


                    }
                }
                if (Program.main_form.SerialData.SUPPLY_B_PUMP_CONTROLLER.run_state == false)
                {
                    //Runstate가 False이나 출력은 나가고 있을 때 로그 생성 후 종료
                    if (Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_PUMP_B_START].value == true)
                    {
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.SUPPLY_PUMP_B_START, false);


                    }
                }
            }
        }
        public void Alarm_Level_Check_AND_TOWER_LAMP_Interlock_By_Level_Heavy()
        {
            if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.HEAVY)
            {
                //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.BUZZER, true);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TOWER_LAMP_RED, true);
                no_process_cancel = true;

                //Heavy일때 Flag이용하여, 설비 전체 Stop //Heavy가 조치되어야 Interlock 재활성화됨
                if (Seq_Stop_By_Interlock == false)
                {
                    Seq_Stop_By_Interlock = true;
                    All_Stop();
                    //Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.CDS_Alive_Interlock, "", true, false);
                    //EQ Interlock
                }
                //if (no_process_req == true)
                //{
                //    no_process_req = false;
                //    Program.CTC.Message_No_Process_Request_Event_454();
                //}
            }
            else if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.LIGHT)
            {
                //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.BUZZER, true);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TOWER_LAMP_RED, true);
                no_process_cancel = true;
                Seq_Stop_By_Interlock = false;
                //if (no_process_req == true)
                //{
                //    no_process_req = false;
                //    Program.CTC.Message_No_Process_Request_Event_454();
                //}
            }
            else if (Program.occured_alarm_form.most_occured_alarm_level == frm_alarm.enum_level.WARNING)
            {
                no_process_req = true;
                if (Program.occured_alarm_form.cnt_occured_alarm_total != 0)
                {
                    //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.BUZZER, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TOWER_LAMP_RED, true);
                }
                else
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.BUZZER, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TOWER_LAMP_RED, false);
                }
                Seq_Stop_By_Interlock = false;

                //if (no_process_cancel == true)
                //{
                //    no_process_cancel = false;
                //    Program.CTC.Message_No_Process_Request_Cancel_Event_455();
                //}
            }



            if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
            {
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TOWER_LAMP_GRN, true);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TOWER_LAMP_YEL, false);
                if (auto_mode_on_trigger == true)
                {
                    auto_mode_on_trigger = false;
                    Program.CTC.Message_Auto_Mode_Event_456();
                }
                manual_mode_on_trigger = true;
            }
            else if (Program.cg_app_info.eq_mode == enum_eq_mode.manual)
            {
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TOWER_LAMP_GRN, false);
                Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.TOWER_LAMP_YEL, true);
                if (manual_mode_on_trigger == true)
                {
                    manual_mode_on_trigger = false;
                    Program.CTC.Message_Manual_Mode_Event_457();

                }
                auto_mode_on_trigger = true;
            }

        }
        public void Tank_Low_Level_Check()
        {

            if (Program.cg_app_info.eq_mode == enum_eq_mode.manual && timer_manual_sequence_tank_a.Enabled == false && timer_manual_sequence_tank_b.Enabled == false)
            {
                //auto Mode이거나 Semi-Auto 모드에서는 Sequence Pass
                //Auto 또는 Semi-Auto에서 긴급 중단 후 전체 멈춤
                //Empty일 때 긴급 멈춤

                //Tank Low Level일때는 Pump Heater All Stop

                //Tank A Line이 사용 중이나, Tank Level이 Empty일 때

                //Tank A
                if ((Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_A].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_A].value == true) && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                {
                    if (Tank_Low_Level_Touch_tank_a == false)
                    {
                        Tank_Low_Level_Touch_tank_a = true;
                        CIRCULATION_1_HEATER_ON_OFF(false);
                        SUPPLY_A_HEATER_ON_OFF(false);
                        SUPPLY_B_HEATER_ON_OFF(false);
                        SUPPLY_A_PUMP_ON_OFF(false);
                        SUPPLY_B_PUMP_ON_OFF(false);
                        CIRCULATION_PUMP_ON_OFF(false);
                        Sequence_Log_Add(tank_class.enum_seq_type.NONE, "Thread : Tank_Low_Level_Check", "Tank A - Stop");
                    }
                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_LEVEL_L].value == true)
                {
                    Tank_Low_Level_Touch_tank_a = false;
                }

                //Tank B
                if ((Program.IO.DO.Tag[(int)Config_IO.enum_do.SUPPLY_FROM_TANK_B].value == true || Program.IO.DO.Tag[(int)Config_IO.enum_do.CIR_FROM_TANK_B].value == true) && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
                {
                    if (Tank_Low_Level_Touch_tank_b == false)
                    {
                        Tank_Low_Level_Touch_tank_b = true;
                        CIRCULATION_1_HEATER_ON_OFF(false);
                        SUPPLY_A_HEATER_ON_OFF(false);
                        SUPPLY_B_HEATER_ON_OFF(false);
                        SUPPLY_A_PUMP_ON_OFF(false);
                        SUPPLY_B_PUMP_ON_OFF(false);
                        CIRCULATION_PUMP_ON_OFF(false);
                        Sequence_Log_Add(tank_class.enum_seq_type.NONE, "Thread : Tank_Low_Level_Check", "Tank B - Stop");
                    }
                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_LEVEL_L].value == true)
                {
                    Tank_Low_Level_Touch_tank_b = false;
                }
            }
            else
            {
                Tank_Low_Level_Touch_tank_b = false;
            }

        }
        /// <summary>
        /// Auto 또는 Semi Auto에서 Drain명령 시 Level에 따라 Valve 조절
        /// </summary>
        public void Drain_Tank_Monitoring()
        {
            if (Program.seq.monitoring.use_auto_drain == true)
            {
                //Drain Tank 
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.Drain_Tank_H].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Pump_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Tank_V_V_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Vet_V_V_On, false);
                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.BOTTOM_VAT_LEAK1].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.Drain_Pump_On_Level].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Pump_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Tank_V_V_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Vet_V_V_On, false);
                    //Last_pump On Level 감지 후 Drain시 Delay 유지 위함
                    Program.seq.monitoring.last_pump_on_level = DateTime.Now;

                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.BOTTOM_VAT_LEAK1].value == false && Program.IO.DI.Tag[(int)Config_IO.enum_di.Drain_Pump_On_Level].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Pump_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Tank_V_V_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Vet_V_V_On, false);
                    //Last_pump On Level 감지 후 Drain시 Delay 유지 위함
                    Program.seq.monitoring.last_pump_on_level = DateTime.Now;

                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.BOTTOM_VAT_LEAK1].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.Drain_Pump_On_Level].value == false)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Pump_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Tank_V_V_On, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Vet_V_V_On, true);
                }
                else
                {
                    if ((DateTime.Now - Program.seq.monitoring.last_pump_on_level).TotalSeconds >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Drain_Pump_Off_Time_Delay))
                    {
                        Program.seq.monitoring.ready_to_pump_off_Delay = false;
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Pump_On, false);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Tank_V_V_On, false);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Vet_V_V_On, false);
                    }
                    else
                    {
                        Program.seq.monitoring.ready_to_pump_off_Delay = true;
                    }

                }
            }
            else
            {
                //Auto Mode가 아닐때도 Interlock 동작
                //BOTTOM_VAT_LEAK1 Interlock이 정상(ture)고, Drain tank Level On Sensor가 꺼져있으면, Pump OFF한다.
                //Program.seq.monitoring.ready_to_pump_off_Delay = false;
                //if (Program.IO.DI.Tag[(int)Config_IO.enum_di.BOTTOM_VAT_LEAK1].value == false && Program.IO.DI.Tag[(int)Config_IO.enum_di.Drain_Pump_On_Level].value == false)
                //{
                //    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Pump_On, false);
                //    //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Drain_Tank_V_V_On, false);
                //    //Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.Vet_V_V_On, false);
                //}
            }
        }
        public void CM_Drain_Tank_Monitoring()
        {
            if (Program.seq.monitoring.cm_use_auto_drain == true)
            {
                //Drain Tank 
                if (Program.IO.DI.Tag[(int)Config_IO.enum_di.CM_DRAIN_TANK_H].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_Drain_Pump_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_Drain_Tank_V_V_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_VAT_V_V_On, false);
                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.CM_VAT_LEAK].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.CM_DRAIN_PUMP_ON_LEVEL].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_Drain_Pump_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_Drain_Tank_V_V_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_VAT_V_V_On, false);
                    //Last_pump On Level 감지 후 Drain시 Delay 유지 위함
                    Program.seq.monitoring.cm_last_pump_on_level = DateTime.Now;

                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.CM_VAT_LEAK].value == false && Program.IO.DI.Tag[(int)Config_IO.enum_di.CM_DRAIN_PUMP_ON_LEVEL].value == true)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_Drain_Pump_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_Drain_Tank_V_V_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_VAT_V_V_On, false);
                    //Last_pump On Level 감지 후 Drain시 Delay 유지 위함
                    Program.seq.monitoring.cm_last_pump_on_level = DateTime.Now;

                }
                else if (Program.IO.DI.Tag[(int)Config_IO.enum_di.CM_VAT_LEAK].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.CM_DRAIN_PUMP_ON_LEVEL].value == false)
                {
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_Drain_Pump_On, true);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_Drain_Tank_V_V_On, false);
                    Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_VAT_V_V_On, true);
                }
                else
                {
                    if ((DateTime.Now - Program.seq.monitoring.cm_last_pump_on_level).TotalSeconds >= Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.CM_Drain_Pump_Off_Time_Delay))
                    {
                        Program.seq.monitoring.cm_ready_to_pump_off_Delay = false;
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_Drain_Pump_On, false);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_Drain_Tank_V_V_On, false);
                        Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CM_VAT_V_V_On, false);
                    }
                    else
                    {
                        Program.seq.monitoring.cm_ready_to_pump_off_Delay = true;
                    }

                }
            }
            else
            {

            }
        }
        public void Tank_Ready_TimeOut_Check()
        {
            Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_ready = DateTime.Now;

            if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.DRAIN || Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.CHARGE)
            {
                if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_ready).Minutes > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_A_Ready_Time_Out))
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Ready_Time_Over_Tank_A, "", true, false);
                }
            }

            if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.DRAIN || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.CHARGE)
            {
                if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_ready).Minutes > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Circulation_Tank_B_Ready_Time_Out))
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Ready_Time_Over_Tank_B, "", true, false);
                }
            }
        }
        public void Tank_Use_TimeOut_Check()
        {
            if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Use_Time_Out) != 0)
            {
                if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.REFILL)
                {
                    if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_cc_tank).Minutes > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_A_Use_Time_Out))
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Use_Time_Over_Tank_A, "", true, false);
                    }
                }
            }

            if (Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Use_Time_Out) != 0)
            {
                if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY || Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.REFILL)
                {
                    if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_cc_tank).Minutes > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Supply_Tank_B_Use_Time_Out))
                    {
                        Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Use_Time_Over_Tank_B, "", true, false);
                    }
                }
            }
        }
        public void CCSS_Input_TimeOut_Check_By_Tank_Charge()
        {
            string remark = "";
            //Charge Time Over Check
            if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
            {
                if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_charge).Seconds > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Charge_Time_Delay))
                {
                    remark = (DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_A].dt_Start_charge).Seconds + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Charge_Time_Delay);
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Charge_Time_Over_Tank_A, remark, true, false);
                }
            }
            else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
            {
                if ((DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_charge).Seconds > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Charge_Time_Delay))
                {
                    remark = (DateTime.Now - Program.tank[(int)tank_class.enum_tank_type.TANK_B].dt_Start_charge).Seconds + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Charge_Time_Delay);
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Charge_Time_Over_Tank_B, remark, true, false);
                }
            }

            //Supply 진행 중 Time Out Alarm Check
            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].use == true &&
                (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].dt_Start).Seconds
                > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Supply_Time_Max))
            {
                remark = (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].dt_Start).Seconds + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Supply_Time_Max);
                if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_A_Chem1, remark, true, false);
                }
                else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_B_Chem1, remark, true, false);
                }
            }
            else if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].input_complete == true &&
                (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].dt_Start).Seconds
                < Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Supply_Time_Min))
            {
                remark = (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss1].dt_Start).Seconds + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem1_Supply_Time_Min);
                if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_A_Chem1, remark, true, false);
                }
                else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_B_Chem1, remark, true, false);
                }
            }

            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].use == true &&
                (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].dt_Start).Seconds
                > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Supply_Time_Max))
            {
                remark = (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].dt_Start).Seconds + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Supply_Time_Max);
                if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_A_Chem2, remark, true, false);
                }
                else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_B_Chem2, remark, true, false);
                }
            }
            else if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].input_complete == true &&
                (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].dt_Start).Seconds
                < Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Supply_Time_Min))
            {
                remark = (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss2].dt_Start).Seconds + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem2_Supply_Time_Min);

                if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_A_Chem2, remark, true, false);
                }
                else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_B_Chem2, remark, true, false);
                }
            }

            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].use == true &&
               (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].dt_Start).Seconds
               > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Supply_Time_Max))
            {
                remark = (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].dt_Start).Seconds + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Supply_Time_Max);
                if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_A_Chem3, remark, true, false);
                }
                else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_B_Chem3, remark, true, false);
                }
            }
            else if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].input_complete == true &&
                (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].dt_Start).Seconds
                < Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Supply_Time_Min))
            {
                remark = (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss3].dt_Start).Seconds + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem3_Supply_Time_Min);
                if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_A_Chem3, remark, true, false);
                }
                else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_B_Chem3, remark, true, false);
                }
            }

            if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].use == true &&
               (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].dt_Start).Seconds
               > Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Supply_Time_Max))
            {
                remark = (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].dt_Start).Seconds + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Supply_Time_Max);
                if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_A_Chem4, remark, true, false);
                }
                else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Max_Time_Over_Tank_B_Chem4, remark, true, false);
                }
            }
            else if (Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].use == true && Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].input_complete == true &&
                (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].dt_Start).Seconds
                < Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Supply_Time_Min))
            {
                remark = (DateTime.Now - Program.tank[(int)Program.seq.main.cur_tank].ccss_data[(int)tank_class.enum_ccss.ccss4].dt_Start).Seconds + "/" + Program.parameter_list.Return_Value_To_int_by_ID((int)frm_parameter.enum_parmater.Chem4_Supply_Time_Min);
                if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_A)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_A_Chem4, remark, true, false);
                }
                else if (Program.seq.main.cur_tank == tank_class.enum_tank_type.TANK_B)
                {
                    Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Supply_Min_Time_Over_Tank_B_Chem4, remark, true, false);
                }
            }
        }
        public void Tank_Can_Not_Use_Status_Check()
        {
            bool tank_a_enable = false;
            bool tank_b_enable = false;
            if (Program.cg_app_info.eq_mode == enum_eq_mode.auto)
            {
                if (Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.READY ||
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.REFILL ||
                    Program.tank[(int)tank_class.enum_tank_type.TANK_A].status == tank_class.enum_tank_status.SUPPLY)
                {
                    tank_a_enable = true;
                }

                if (Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.READY ||
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.REFILL ||
                    Program.tank[(int)tank_class.enum_tank_type.TANK_B].status == tank_class.enum_tank_status.SUPPLY)
                {
                    tank_b_enable = true;
                }
            }

            if (tank_a_enable == false && tank_b_enable == false)
            {
                Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Can_Not_Use_Tank, "", true, false);
            }
        }
        public void Empty_Check_And_TotalUsage_Reset()
        {
            if (Program.cg_app_info.eq_mode == enum_eq_mode.manual && Program.schematic_form.timer_manual_sequence_tank_a.Enabled == false && Program.schematic_form.timer_manual_sequence_tank_b.Enabled == false)
                if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_A_DRAIN].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKA_EMPTY_CHECK].value == false)
                {
                    TotalUsage_Reset(Program.tank[(int)tank_class.enum_tank_type.TANK_A]);
                }
            if (Program.IO.DO.Tag[(int)Config_IO.enum_do.TANK_B_DRAIN].value == true && Program.IO.DI.Tag[(int)Config_IO.enum_di.TANKB_EMPTY_CHECK].value == false)
            {
                TotalUsage_Reset(Program.tank[(int)tank_class.enum_tank_type.TANK_B]);
            }
        }
    }
}