using System;
using System.Collections.Generic;
using System.Drawing;

namespace cds
{

    public class tank_class
    {
        public const int use_tank_cnt = 2; //3Tank 까지 사용 가능
        public const int ccss_cnt = 4; //ccss count는 4개 고정
        public string name = "TANK - A";
        public float concentration_ccss1;
        public float concentration_ccss2;
        public float concentration_ccss3;
        public float concentration_ccss4;


        public bool enable = false;

        //Life Time Over / Wafer Count Over
        public int wafer_cnt; // Process 후 wafer_cnt 계산
        public DateTime dt_start_process;// Process 후 Life_Time 계산
        public int life_time_to_minute; // Process 후 Life_Time 계산

        public string volume; //CCSS1 Volume/CCSS2 Volume/CCSS3 Volume/CCSS4 Volume
        public string volume_ratio; //CCSS1 Volume ratio/CCSS2 Volume ratio/CCSS3 Volume ratio/CCSS4 Volume ratio
        public string volume_name; // CCSS1 Name/CCSS2 Name/CCSS3 Name/CCSS4 Name

        public float total_volume;
        public ccss_data_info[] ccss_data;
        public enum_tank_status status = enum_tank_status.NONE;
        public bool use_drain_seq_by_semiauto = false;

        public bool circulation_processing = false;

        public DateTime dt_Start_drain;
        public DateTime dt_Start_charge;
        public DateTime dt_Start_ready;
        public DateTime dt_Start_cc_tank;
        public DateTime dt_Start_pump_run;
        public DateTime dt_Start_heater_run;
        public DateTime dt_ok_Circulation_Temp;
        public DateTime dt_ok_Process_Temp;
        public DateTime dt_start_Circulation_Heater_on;
        public DateTime dt_delay_Circulation_Temp_rising; //Delay Parameter 만큼 온도 유지 판단하기 위함
        public DateTime dt_delay_Supply_Temp_rising; //Delay Parameter 만큼 온도 유지 판단하기 위함

        public Color BackColor_Tank_By_Status(enum_tank_status status)
        {
            Color result = Color.Black;
            if (status == enum_tank_status.NONE)
            {
                result = Color.White;
            }
            else if (status == enum_tank_status.DRAIN)
            {
                result = Color.Red;
            }
            else if (status == enum_tank_status.DRAIN_WAIT)
            {
                result = Color.Yellow;
            }
            else if (status == enum_tank_status.CHARGE)
            {
                result = Color.AliceBlue;
            }
            else if (status == enum_tank_status.REFILL)
            {
                result = Color.AliceBlue;
            }
            else if (status == enum_tank_status.READY)
            {
                result = Color.LawnGreen;
            }
            else if (status == enum_tank_status.SUPPLY)
            {
                result = Color.LightBlue;
            }
            else if (status == enum_tank_status.STOP)
            {
                result = Color.Black;
            }
            return result;
        }
        public Color ForeColor_Tank_By_Status(enum_tank_status status)
        {
            Color result = Color.Black;

            if (status == enum_tank_status.NONE)
            {
                result = Color.Black;
            }
            else if (status == enum_tank_status.DRAIN)
            {
                result = Color.White;
            }
            else if (status == enum_tank_status.DRAIN_WAIT)
            {
                result = Color.Black;
            }
            else if (status == enum_tank_status.CHARGE)
            {
                result = Color.Black;
            }
            else if (status == enum_tank_status.REFILL)
            {
                result = Color.Black;
            }
            else if (status == enum_tank_status.READY)
            {
                result = Color.White;
            }
            else if (status == enum_tank_status.SUPPLY)
            {
                result = Color.White;
            }
            else if (status == enum_tank_status.STOP)
            {
                result = Color.White;
            }
            return result;
        }
        public class ccss_data_info
        {
            public double input_volume; //Parameter 비율에 따라 들어오는 양을 저장하는 변수
            public double input_volmue_real; //실제로 들어온 양을 저장 한다. simulation 사용
            public bool use;
            public bool input_complete;
            public bool flag_level_hh; //hh Level에 도달한 경우 적산양에 상관없이 Valve Close를 위함, HH off시 flag 초기화
            public DateTime dt_Start; //CCSS Input Start 시간 // Time Over시 알람 발생 필요 // 시작 시간 갱신 후 Valve Open
        }
        public enum enum_ccss
        {
            ccss1 = 0, ccss2 = 1, ccss3 = 2, ccss4 = 3
        }
        public enum enum_tank_type
        {
            TANK_A = 0,
            TANK_B = 1,
            TANK_C = 2,
            NONE = 9,
            TANK_ALL = 10,
        }
        public enum enum_seq_no
        {
            NONE = 0,
            INITIALIZE = 500,
            MODE_CHECK = 1000,
            MANUAL = 2000,
            AUTO = 3000,

            READY_TO_CTC_REPLY = 3100,
            TANK_STATUS_CHECK = 5000,
            TANK_MIXING_TYPE_CHECK = 6000,
            TANK_REFILL = 7000,

            TANK_EMPTY_CHECK1 = 10000,
            TANK_DRAIN_START = 11000,
            TANK_EMPTY_CHECK2 = 11100,

            TANK_DRAIN_MONITORING = 12000,
            TANK_DRAIN_DELAY_BEFORE_END = 13000,
            TANK_DRAIN_END = 14000,

            TANK_INPUT_ORDER_CHECK = 15000,
            TANK_INPUT_STANDBY = 15100,


            HDIW_TEMP_MONITORING = 15200,
            HDIW_TEMP_OK = 15300,
            TANK_SELECT = 16000,

            CCSS1_INPUT_READY_CHECK = 20000,
            CCSS1_INPUT_READY = 21000,
            CCSS1_INPUT_START = 22000,
            CCSS1_INPUT_MONITORING = 23000,
            CCSS1_INPUT_REQ_END = 25000,

            CCSS2_INPUT_READY_CHECK = 30000,
            CCSS2_INPUT_READY = 31000,
            CCSS2_INPUT_START = 32000,
            CCSS2_INPUT_MONITORING = 33000,
            CCSS2_INPUT_REQ_END = 35000,

            CCSS3_INPUT_READY_CHECK = 40000,
            CCSS3_INPUT_READY = 41000,
            CCSS3_INPUT_START = 42000,
            CCSS3_INPUT_MONITORING = 43000,
            CCSS3_INPUT_REQ_END = 45000,

            CIRCULATION_READY_CHECK = 60000,
            CIRCULATION_READY = 61000,
            CIRCULATION_PUMP_RUN = 61100,
            CIRCULATION_START = 62000,
            CIRCULATION_HEATER_RUN = 62100,
            CIRCULATION_MONITORING_LEVEL = 63000,
            CIRCULATION_CHECK = 63100,
            CIRCULATION_MONITORING_TEMP_START = 64000,
            CIRCULATION_MONITORING_TEMP_OK_WAIT_DELAY = 64010,

            CIRCULATION_MONITORING_CHECK_CONCENTRATION = 65000,
            CIRCULATION_MONITORING_CHECK_CONCENTRATION_CIR_VALVE_CLOSE = 65010,
            CIRCULATION_MONITORING_CHECK_CONCENTRATION_VALVE_OPEN = 65020,
            CIRCULATION_MONITORING_CHECK_CONCENTRATION_WAIT_DELAY1 = 65025,
            CIRCULATION_MONITORING_CHECK_CONCENTRATION_WAIT_DELAY2 = 65026,
            CIRCULATION_MONITORING_CHECK_CONCENTRATION_ACT = 65030,
            CIRCULATION_MONITORING_CHECK_CONCENTRATION_CIR_VALVE_OPEN = 65040,
            CIRCULATION_MONITORING_CHECK_CONCENTRATION_VALVE_CLOSE = 65050,
            CIRCULATION_MONITORING_CHECK_CONCENTRATION_OK = 65100,

            CIRCULATION_END = 69000,

            HEATER_READY_CHECK = 70000,
            HEATER_READY_CHECK_VALVE = 70300,
            HEATER_READY_CHECK_FM21 = 70500,
            HEATER_READY_CHECK_PS21 = 70700,
            HEATER_READY = 71000,
            HEATER_START = 72000,
            HEATER_MONITORING = 73000,
            HEATER_END = 75000,

            CHARGE_COMPLETE = 100000,

            ERROR_BY_ALARM = 900000,
            ERROR_BY_APP = 910000

        }
        public enum enum_seq_no_supply
        {

            TANK_READY_CHECK = 100000,
            READY_SUPPLY_BY_CTC = 200000,
            KEEP_SUPPLY_READY = 200100,
            KEEP_SUPPLY_START_VALVE_CHANGE = 200110,

            HEATER_STOP_CHECK_BEFORE_SUPPLY = 201000,
            CIRCULATION_STOP_BEFORE_SUPPLY = 202000,

            CIRCULATION_HEATER_OFF_REQ = 300500,
            CIRCULATION_HEATER_OFF_COMPLETE = 300600,
            CIRCULATION_PUPMP_OFF_REQ = 300700,
            CIRCULATION_PUPMP_OFF_COMPLETE = 300800,
            CIRCULATION_VALVE_OFF_REQ = 300900,

            CIRCULATION_VALVE_OFF_COMPLETE = 301000,
            SUPPLY_VALVE_ON_REQ = 302000,
            SUPPLY_VALVE_ON_COMPLETE = 302100,
            SUPPLY_PUMP_ON_REQ = 303000,
            SUPPLY_PUMP_ON_COMPLETE = 303100,
            KEEP_SUPPLY_WAITING_DELAY = 300120,
            KEEP_SUPPLY_END = 300130,
            KEEP_SUPPLY_END_VALVE_CHANGE = 300140,
            SUPPLY_CHECK_FLOW = 303500,
            SUPPLY_HEATER_ON_REQ = 304000,
            SUPPLY_HEATER_ON_COMPLETE = 304100,

            SUPPLY_LOOP_FLUSH = 310000,
            SUPPLY_START = 312000,
            SUPPLY_MONITORING = 320000,

            MONITORING_CHECK_CONCENTRATION = 65000,
            MONITORING_CHECK_CONCENTRATION_CIR_VALVE_CLOSE = 65010,
            MONITORING_CHECK_CONCENTRATION_VALVE_OPEN = 65020,
            MONITORING_CHECK_CONCENTRATION_WAIT_DELAY1 = 65025,
            MONITORING_CHECK_CONCENTRATION_WAIT_DELAY2 = 65026,
            MONITORING_CHECK_CONCENTRATION_ACT = 65030,
            MONITORING_CHECK_CONCENTRATION_CIR_VALVE_OPEN = 65040,
            MONITORING_CHECK_CONCENTRATION_VALVE_CLOSE = 65050,
            MONITORING_CHECK_CONCENTRATION_OK = 65100,


            ERROR_BY_ALARM = 900000,
            ERROR_BY_APP = 910000
        }
        public enum enum_seq_no_pump_control
        {
            READY = 500,
            INITIAL = 1000,
            CYCLE_START = 2000,
            LEFT_ON_ACT = 2100,
            RIGHT_ON_WAIT = 2200,
            RIGHT_ON_ACT = 2300,
            LEFT_ON_WAIT = 2400,
            CYCLE_COMPLETE = 3000,
            CYCLE_ERROR = 3100,
            ERROR_BY_ALARM = 900000,
            ERROR_BY_APP = 910000
        }
        public enum enum_seq_no_monitoring
        {

        }
        public enum enum_seq_no_drain_pump_control
        {
            READY = 500,
            INITIAL = 1000,
            CYCLE_START = 2000,
            LEFT_ON_ACT = 2100,
            RIGHT_ON_WAIT = 2200,
            RIGHT_ON_ACT = 2300,
            LEFT_ON_WAIT = 2400,
            CYCLE_COMPLETE = 3000,
            ERROR_BY_ALARM = 900000,
            ERROR_BY_APP = 910000
        }
        public enum enum_seq_no_semi_auto
        {
            NONE = 0,
            READY = 100,
            INITIAL = 200,
            MONITORING_RUN_COUNT_AUTO_FLUSH_READY = 210,
            MONITORING_RUN_COUNT_AUTO_FLUSH = 220,
            MONITORING_RUN_COUNT_AUTO_FLUSH_SUB = 221,
            MONITORING_RUN_COUNT = 230,
            TANK_DRAIN_START_1_ONLY_ONCE = 240,
            TANK_DRAIN_WAIT_ONLY_ONCE = 250,
            TANK_EMPTY_CHECK1_ONLY_ONCE = 300,
            TANK_DRAIN_START_2_ONLY_ONCE = 400,
            TANK_EMPTY_CHECK2_ONLY_ONCE = 500,
            TANK_DRAIN_DELAY_BEFORE_END_ONLY_ONCE = 600,
            TANK_DRAIN_END_ONLY_ONCE = 700,


            //TANK_DRAIN_START = 400,
            //TANK_EMPTY_CHECK2 = 500,
            //TANK_DRAIN_DELAY_BEFORE_END = 600,
            //TANK_DRAIN_END = 700,

            HDIW_TEMP_MONITORING_BY_DIW = 950,
            HDIW_TEMP_OK_BY_DIW = 960,
            TANK_DIW_INPUT_REQ = 1000,
            TANK_DIW_INPUT_WAIT = 1100,
            TANK_DIW_INPUT_COMPLETE = 1200,

            HDIW_TEMP_MONITORING_BY_CHEMICAL = 1950,
            HDIW_TEMP_OK_BY_CHEMICAL = 1960,
            TANK_CHEMICAL_INPUT_REQ = 2000,
            TANK_CHEMICAL_MONITORING_LEVEL = 2100,
            TANK_CHEMICAL_INPUT_COMPLETE = 2200,

            TANK_DRAIN_START = 3000,
            TANK_EMPTY_CHECK = 3100,
            TANK_DRAIN_DELAY_BEFORE_END = 3200,
            TANK_DRAIN_END = 3300,

            TANK_FLUSH_SUPPLY_START = 4000,
            TANK_FLUSH_SUPPLY_VALVE_ON = 4100,
            TANK_FLUSH_SUPPLY_PUMP_ON = 4200,
            TANK_FLUSH_SUPPLY_DELAY_WAIT = 4300,
            TANK_FLUSH_SUPPLY_DELAY_END = 4400,
            TANK_FLUSH_SUPPLY_DRAIN_START = 4500,
            TANK_FLUSH_SUPPLY_DRAIN_WAIT = 4600,
            TANK_FLUSH_SUPPLY_DRAIN_BEFORE_END = 4650,
            TANK_FLUSH_SUPPLY_DRAIN_END = 4700,
            TANK_FLUSH_SUPPLY_PUMP_OFF = 4800,
            TANK_FLUSH_SUPPLY_VALVE_OFF = 4900,
            TANK_FLUSH_SUPPLY_END = 5000,


            //LAL Calibration Semi Auto Add /20230410

            CAL_NONE = 7000,
            CAL_READY = 7100,
            CAL_INITIAL = 7200,
            CAL_CM_MODE_CHANGE_SERIAL = 7300,
            CAL_CALIBRATION_START = 7400,
            CAL_CALIBRATION_WAIT = 7500,
            CAL_CALIBRATION_END = 7600,
            CAL_CM_MODE_CHANGE_PARALLEL = 7900,
            SEMI_AUTO_COMPLETE = 10000,
            ERROR_BY_ALARM = 900000,
            ERROR_BY_APP = 910000
        }

        public enum enum_tank_status
        {
            NONE = 0,
            START = 10,
            STOP = 20,
            READY = 30,
            DRAIN = 40,
            DRAIN_WAIT = 51,
            EXCHANGE = 60,
            CHARGE = 65,
            REFILL = 70,
            DONE = 80,
            SUPPLY = 100
        }

        public enum enum_semi_auto
        {
            NONE = 0,
            DRAIN = 10,
            DIW_FLUSH = 20,
            DIW_FLUSH_AND_SUPPLY = 21,
            CHEMICAL_FLUSH = 30,
            CHEMICAL_FLUSH_AND_SUPPLY = 31,
            AUTO_FLUSH = 40,
            CALIBRATION_LAL = 50
        }

        public enum enum_seq_type
        {

            MAIN = 0,
            SEMI_AUTO_A = 10,
            SEMI_AUTO_B = 20,
            SUPPLY = 30,
            CIRCULATION = 40,
            DRAIN_VALVE_CONTROL = 50,
            NONE = 100,
        }

    }


    public class seq_info
    {
        public Boolean is_run;


        public DateTime starttime;
        public DateTime endtime;
        public DateTime hdiw_check_start;
        //main sequence
        public seq_type_main main;
        //Chemical Change 응답 확인 Sequence
        public seq_type_supply supply;
        //PumpControl Sequence
        public seq_type_pumpcontrol pumpcontrol;

        //Monitoring Sequnce
        public seq_type_monitoring monitoring;
        //Drain
        public seq_type_semi_auto semi_auto_tank_a;
        public seq_type_semi_auto semi_auto_tank_b;
        public seq_type_semi_auto semi_auto_tank_all;

        public bool circulation_on_req = false; //Circulation시 ON OFF
        public bool scr_supply_on_req = false; //Supply SCR ON OFF

        public bool input_request = false;//Mixing 순서대로 Input Check시 사용

        public bool cir_start = false;

        //Manual 사용 변수
        public bool manual_tank_a_cc_req_by_user = false;
        public bool manual_tank_b_cc_req_by_user = false;

        public ccss_variable[] ccss_variable; //ccss 정보 관리(CDS APP 내)

        public CCSS_Info cur_mixing;

        public int cur_mixing_index;

        public Queue<CCSS_Info> mixing_order;
        public int cur_sametime_input_count;
        public List<CCSS_Info> mixing_order_list; //동시 투입 확인 용

        public bool manual_exchange_req_by_user = false;
        public bool manual_exchange_ack_by_ctc = false;


    }
    public class ccss_variable
    {
        public bool changed_valve_a = false; //Valve Off 후 적산량 기록시 사용
        public bool changed_valve_b = false; //Valve Off 후 적산량 기록시 사용
    }
    public class seq_type_main
    {
        public tank_class.enum_tank_type cur_tank = tank_class.enum_tank_type.TANK_A;
        public tank_class.enum_seq_no no_old, no_cur;
        public bool concentration_measuring = false;
        public bool drain_start = false;
        public DateTime last_act_time;
        public TimeSpan last_act_span;
        public string state_display = "";
        public string state_display2 = "";
        public string memo_current = "";
        public string memo_old = "";
    }
    public class seq_type_supply
    {
        public tank_class.enum_tank_type cur_tank = tank_class.enum_tank_type.TANK_A;
        public tank_class.enum_seq_no_supply no_old, no_cur;
        public string CC_START_TANK = "";
        public bool CDS_enable_status_to_ctc = false;
        public bool supply_status = false;
        public bool concentration_measuring = false;
        public DateTime last_act_time;
        public TimeSpan last_act_span;
        public bool refill_run_state = false;
        public string state_display = "";
        public string state_display2 = "";


        //CTC 통신 변수
        public bool ctc_supply_request = false; //Supply 전 신호를 받아야 동작하며, 받은 후 Flag 초기화 Supply A 완료 후 Exchange 시 Supply B 바로 전환을 위함
        public bool ctc_c_c_request = false;
        public bool ctc_reclaim_request = false;
        public bool ctc_reclaim_enabled = false;
        public bool ready_flag = false;
        public bool ready_flag_in_req_send = false;
        //CC flag 변수
        public bool cc_ctc_req_flag = false;
        public bool cc_level_low_flag = false;
        public bool cc_wafer_cnt_over_flag = false;
        public bool cc_lifetime_flag = false;
        public bool cc_manual_exchange = false;

        public bool c_c_need; //Life Time, Wafer Count, CTC Req등 C.C 필요한지 판단.
        public string c_c_need_text;

        public bool req_c_c_start_cds_to_ctc; //c.c전 CTC로 부터 컨펌 후 진행
        public bool rep_c_c_start_cds_to_ctc; //c.c전 CTC로 부터 컨펌 후 진행

        public bool concentration_check_req_in_supply = false;

        public DateTime dt_last_concentration_check;
        public DateTime dt_start_cc_start_req_cds_to_ctc;

    }
    public class seq_type_pumpcontrol
    {
        public tank_class.enum_tank_type cur_tank = tank_class.enum_tank_type.TANK_A;
        public tank_class.enum_seq_no_pump_control no_old, no_cur;
        public DateTime last_act_time;
        public TimeSpan last_act_span;
        public string state_display = "";
        public string state_display2 = "";
    }
    public class seq_type_monitoring
    {
        //Drain Tank
        public bool use_auto_drain = true;
        public string pump_off_delay_Status = "";
        public bool ready_to_pump_off_Delay = false;
        public DateTime last_pump_on_level;
        public DateTime last_act_time;
        public TimeSpan last_act_span;
        public string state_display = "";
        public string state_display2 = "";

        //CM Drain Tank
        public bool cm_use_auto_drain = true;
        public string cm_pump_off_delay_Status = "";
        public bool cm_ready_to_pump_off_Delay = false;
        public DateTime cm_last_pump_on_level;
        public DateTime cm_last_act_time;
        public TimeSpan cm_last_act_span;
        public string cm_state_display = "";
        public string cm_state_display2 = "";
    }
    public class seq_type_semi_auto
    {
        public tank_class.enum_tank_type cur_tank = tank_class.enum_tank_type.TANK_A;
        public tank_class.enum_tank_type selected_tank_type = tank_class.enum_tank_type.TANK_A;
        public tank_class.enum_seq_no_semi_auto no_old, no_cur;
        public tank_class.enum_semi_auto semi_auto_type;
        public tank_class.enum_semi_auto auto_flush_current_type;

        //GUI 색상 반전 사용
        public DateTime dt_btn_state_change_drain;
        public DateTime dt_btn_state_change_diw_flush;
        public DateTime dt_btn_state_change_chem_flush;
        public DateTime dt_btn_state_change_calibration;


        //Supply A, Supply B Delay Time 계산
        public DateTime dt_supply_start;
        public DateTime hdiw_check_start;
        public int semi_auto_run_auto_flush_count = 0;
        public int semi_auto_run_diw_flush_count = 0;
        public int semi_auto_run_chemical_flush_count = 0;
        public int semi_auto_run_count = 0;
        public bool semi_auto_complete = false;
        public bool drain_complete = false;
        public bool diw_flush_and_supply_complete = false;
        public bool chemical_flush_and_supply_auto_complete = false;
        public DateTime last_act_time;
        public TimeSpan last_act_span;
        public string state_display = "";
        public string state_display2 = "";
        public string memo_current = "";
        public string memo_old = "";

        public CCSS_Info cur_mixing;
        public int cur_mixing_index;
        public Queue<CCSS_Info> mixing_order;

        public int cur_sametime_input_count;
        public List<CCSS_Info> mixing_order_list; //동시 투입 확인 용
        public bool input_request = true;

    }


}
