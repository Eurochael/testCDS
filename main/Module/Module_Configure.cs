using cds;
using System;
using System.Collections.Generic;
using static cds.frm_alarm;
using static cds.frm_trendlog;
//1.0.0.0
namespace cds
{
    #region "CDS Main Config"
    public class Config_Main
    {
        public App_Path path;
        public MariaDB.Server db_server;
        public MariaDB.Local_cds db_local_cds;
        public MariaDB.Local_fdc db_Local_fdc;
    }
    #endregion

    #region "Config_App_Info -> Server Sync Client"
    public class Config_App_Info
    {
        private string _version;
        public string version
        {
            get
            {
                _version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return _version;
            }
            set { }
        }
        public Mode_Simulation mode_simulation;
        public System_Internal_Info internal_info;
        public Tank_Info[] tank_info;
        public enum_eq_type eq_type;
        public enum_eq_mode eq_mode;
        public enum_pump_mode circulation_pump_mode;
        public enum_he3320c_mode he3320c_mode;
        public bool ipa_ccss_ready_use;
        public bool keep_supply_and_cir_off_delay_by_change = false;
        public bool ignore_ctc_config_sync = false;
        public bool ignore_hdiw_by_pass = false;
        public bool supply_enable_force = false;
        public bool supply_empty_level_to_stop = false;
        public bool use_concentration_check = false;
        public bool use_heating_output_mapping_tank = false;
        public bool temp_pid_scheduler_use = false;
        //public bool use_chemical_drain = false;
        public bool semi_auto_state;
        //적분 Intergral a -> b Slice Interval
        public int totalusage_slice_interval;
        //User 정보
        public User_Info user_info;

        //Log & Time
        public int log_save_level_min;
        public int serial_connect_time;
        public int abb_connect_time;
        public int ctc_connect_time;
        public enum_serial_view_type log_serial_view_type;

    }
    public enum enum_serial_view_type
    {
        HEX = 0, BYTE = 1, STRING = 2
    }
    public class App_Mode
    {
        public int mode = 0;
    }
    public class User_Info
    {
        public int login_expire_time;
        public string user_password;
        public string admin_password;
    }
    public class Mode_Simulation
    {
        public bool use = false;
        public bool use_ethercat_real = false;
        public bool use_seq_auto_run = false;
    }
    public class System_Internal_Info
    {
        public bool log_manager_run = false;
        public Double usage_app = 0;
        public Double usage_drive_c = 0;
        public Double usage_drive_d = 0;
    }
    /// <summary>
    /// 0 : Tank A, 1 : Tank B, 2 : Tank C
    /// </summary>
    public class Tank_Info
    {
        public string name = "";
        public bool enable = true;

    }
    #endregion

    #region "App Path"
    public class App_Path
    {
        private string _yaml;
        public string yaml
        {
            //get { return _yaml = System.Windows.Forms.Application.StartupPath + @"\Yaml\"; }
            get { return _yaml; }
            set { _yaml = value; }
        }
        private string _xml;
        public string xml
        {
            get { return _xml = System.Windows.Forms.Application.StartupPath + @"\XML\"; }
            //get { return _xml; }
            set { _xml = value; }
        }
        private string _log;
        public string log
        {
            //get { return _log = System.Windows.Forms.Application.StartupPath + @"\Log\"; }
            get { return _log; }
            set { _log = value; }
        }
        private string _log_trend;
        public string log_trend
        {
            //get { return _log_trend = System.Windows.Forms.Application.StartupPath + @"\Log\Log_TrendData\"; }
            get { return _log_trend; }
            set { _log_trend = value; }
        }
        private string _log_io;
        public string log_io
        {
            //get { return _log_trend = System.Windows.Forms.Application.StartupPath + @"\Log\Log_TrendData\"; }
            get { return _log_io; }
            set { _log_io = value; }
        }

        private string _mixing_step_save_folder_name;
        public string mixing_step_save_folder_name
        {
            //get { return _log_trend = System.Windows.Forms.Application.StartupPath + @"\Log\Log_TrendData\"; }
            get { return _mixing_step_save_folder_name; }
            set { _mixing_step_save_folder_name = value; }
        }
    }

}
#endregion

#region "MariaDB"
public class MariaDB
{
    public class Server
    {
        public string ip = "127.0.0.1";
        public int port = 3306;
        public string driver = "MariaDB ODBC 3.1 Driver";
        public string database = "ctc";
        public string id = "root";
        public string password = "express2345";
        private string _connection;
        public string connection
        {
            get { return _connection = @"DRIVER={" + driver + "};SERVER=" + ip + ";PORT=" + port + ";USER=" + id + ";PASSWORD=" + password + ";DATABASE=" + database + ";"; }
            //get { return _connection; }
            set { _connection = value; }
        }
    }
    public class Local_cds
    {
        public string ip = "127.0.0.1";
        public int port = 3306;
        public string driver = "MariaDB ODBC 3.1 Driver";
        public string database = "cds";
        public string id = "root";
        public string password = "express2345";
        private string _connection;
        public string connection
        {
            get { return _connection = @"DRIVER={" + driver + "};SERVER=" + ip + ";PORT=" + port + ";USER=" + id + ";PASSWORD=" + password + ";DATABASE=" + database + ";"; }
            //get { return _connection; }
            set { _connection = value; }
        }
    }
    public class Local_fdc
    {
        public string ip = "127.0.0.1";
        public int port = 3306;
        public string driver = "MariaDB ODBC 3.1 Driver";
        public string database = "fdc";
        public string id = "root";
        public string password = "express2345";
        private string _connection;
        public string connection
        {
            get { return _connection = @"DRIVER={" + driver + "};SERVER=" + ip + ";PORT=" + port + ";USER=" + id + ";PASSWORD=" + password + ";DATABASE=" + database + ";"; }
            //get { return _connection; }
            set { _connection = value; }
        }
    }
}

#endregion

#region "App Start Para / not use yaml"
public class Config_AppLoading
{
    public Boolean load_complete = false;
    public int seq_no_cur = 0;
    public int seq_no_error_occurred = 0;
    public int seq_no_old = -1;
    public const int seq_no_error = 999;
    public const int seq_no_max = 1000;

    public string request_text;
    public string complete_text;
    public string result;
    public string simulation_debug = "";
    public string simulation_debug_path = "";
    public string xml_main_path;

    public Boolean init_variable = false;
    public Boolean Config_Sync_Complete = false;
    public Boolean CTC_DB_Fail = false;
    public string CTC_DB_Fail_Text = "";
    public int seq_interval_default = 100;
    public TimeSpan tp_elapse = new TimeSpan();
    public DateTime dt_last_seq_run = new DateTime();

}
#endregion

#region "Alarm Structure"

public class Alarm_List
{
    public int total_cnt;
    public Dictionary<int, int> index_id_mapping = new Dictionary<int, int>();
    public Config_Alarm[] contents;
    public Config_Alarm Return_Object_by_ID(int alarm_id)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(alarm_id, out idx_ref_address) == true)
        {
            if (contents[idx_ref_address] != null)
            {
                return contents[idx_ref_address];
            }
            else
            {
                return null;
            }
        }
        else
        {
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Database_Alarm_ID_Not_Register, "ID : " + alarm_id.ToString(), true, false);
            return null;
        }
    }
    public void Alarm_Reset_Request_by_ID(int alarm_id, enum_cleared_by cleared_by)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(alarm_id, out idx_ref_address) == true)
        {
            Program.alarm_list.contents[idx_ref_address].cleared_by = (int)cleared_by;
            Program.alarm_list.contents[idx_ref_address].clear_request = true;
        }
        else
        {
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Database_Alarm_ID_Not_Register, "ID : " + alarm_id.ToString(), true, false);
        }

    }
    public void Alarm_Thread_Call_by_ID(int alarm_id, string remark, bool auto_thread_call_keep_alarm_disable, bool alarm_alive)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(alarm_id, out idx_ref_address) == true)
        {
            Program.alarm_list.contents[idx_ref_address].auto_thread_call_keep_alarm_disable = auto_thread_call_keep_alarm_disable;
            Program.alarm_list.contents[idx_ref_address].alarm_alive = alarm_alive;
            if (remark != "") { Program.alarm_list.contents[idx_ref_address].remark = remark; }
            Program.alarm_list.contents[idx_ref_address].thread_call = true;
        }
        else
        {

        }


    }
    public int Return_Enable_by_ID(int alarm_id)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(alarm_id, out idx_ref_address) == true)
        {
            return (int)Program.alarm_list.contents[idx_ref_address].enable;
        }
        else
        {
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Database_Alarm_ID_Not_Register, "ID : " + alarm_id.ToString(), true, false);
            return 0;
        }
    }
    public int Return_WDT_by_ID(int alarm_id)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(alarm_id, out idx_ref_address) == true)
        {
            return Program.alarm_list.contents[idx_ref_address].wdt;
        }
        else
        {
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Database_Alarm_ID_Not_Register, "ID : " + alarm_id.ToString(), true, false);
            return 0;
        }
    }
    public int Return_Level_by_ID(int alarm_id)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(alarm_id, out idx_ref_address) == true)
        {
            return Program.alarm_list.contents[idx_ref_address].level;
        }
        else
        {
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Database_Alarm_ID_Not_Register, "ID : " + alarm_id.ToString(), true, false);
            return 0;
        }
    }

}
/// <summary>
/// Alarm ON OFF 관리는 Main Form - Alarm Check에서만 진행한다. // 다른 Function에서 Call시 thread_call true로 활성화
/// </summary>
public class Config_Alarm
{
    //DB 공용 정보
    public int id;
    public string name;
    public string comment;
    public string remark;
    public byte enable;
    public int wdt;
    public byte report_to_host;
    public byte visible;
    public byte level;
    public byte unit;

    public bool occurred; //알람 발생인지, Reset인지 구분 queue에서 사용
    public bool thread_call; //Sequence 또는 Thread에서 알람 발생 시 처리를 위함
    public bool clear_request;

    public DateTime occurred_time;
    public int occurred_by;
    public DateTime cleared_time;
    public int cleared_by;

    public DateTime last_off_time;

    public bool display = false;
    public bool auto_thread_call_keep_alarm_disable = false;
    public bool alarm_alive = false;
    public DateTime first_ontime;



}
#endregion

#region "Parameter Structure"
public class Parameter_List
{
    public int total_cnt;
    public Dictionary<int, int> index_id_mapping = new Dictionary<int, int>();
    public Config_Parameter[] contents;
    public Config_Parameter Return_Object_by_ID(int para_id)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(para_id, out idx_ref_address) == true)
        {
            if (contents[idx_ref_address] != null)
            {
                return contents[idx_ref_address];
            }
            else
            {
                return null;
            }
        }
        else
        {
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Database_Parameter_ID_Not_Register, "ID : " + para_id.ToString(), true, false);
            return null;
        }
    }
    public float Return_Value_To_Float_by_ID(int para_id)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(para_id, out idx_ref_address) == true)
        {
            return Program.parameter_list.contents[idx_ref_address].value;
        }
        else
        {
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Database_Parameter_ID_Not_Register, "ID : " + para_id.ToString(), true, false);
            return 0;
        }
    }
    public int Return_Value_To_int_by_ID(int para_id)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(para_id, out idx_ref_address) == true)
        {
            return Convert.ToInt32(Program.parameter_list.contents[idx_ref_address].value);
        }
        else
        {
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Database_Parameter_ID_Not_Register, "ID : " + para_id.ToString(), true, false);
            return 0;
        }
    }
    public int Return_Value_Min_by_ID(int para_id)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(para_id, out idx_ref_address) == true)
        {
            return Convert.ToInt32(Program.parameter_list.contents[idx_ref_address].value_min);
        }
        else
        {
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Database_Parameter_ID_Not_Register, "ID : " + para_id.ToString(), true, false);
            return 0;
        }
    }
    public int Return_Value_Max_by_ID(int para_id)
    {
        int idx_ref_address = 0;
        if (index_id_mapping.TryGetValue(para_id, out idx_ref_address) == true)
        {
            return Convert.ToInt32(Program.parameter_list.contents[idx_ref_address].value_max);
        }
        else
        {
            Program.alarm_list.Alarm_Thread_Call_by_ID((int)frm_alarm.enum_alarm.Database_Parameter_ID_Not_Register, "ID : " + para_id.ToString(), true, false);
            return 0;
        }
    }
}
public class Config_Parameter
{
    public int id;
    public string name;
    public float value;
    public string unit;
    public string comment;
    public int value_min;
    public int value_max;
    public byte report_to_host;
    public byte visible;
}
#endregion
#region "Config : Config_Trace"
public class Config_Trace
{
    public bool socket_log = true;
}
#endregion
#region "DB Query_Parameter"
public class Query_Parameter
{
    public DateTime start;
    public DateTime end;
    public List<string> token = new List<string>();
}
#endregion

#region "Config Trend Data"
public class Config_Trend_DataList
{
    public int total_cnt;
    public int fifo_max_minute;
    public Trend_Data[] trend_data;
}
public class Trend_Data
{
    public string name;
    public bool use;
    public double value;
    public enum_trend_type type;
}
#endregion "Trend Data"

#region "Config Sokcet - Server"
public class Config_Socket
{
    public int server_port = 5000;
    public string server_name = "CDS-SERVER";
    public Boolean server_port_auto_create = false; //프로그램 실행 시 true 활성화 시 Network No에 따라 소켓 생성 18 -> 5000 / 19 -> 5001
    public Boolean use_response_check = false;
    public int message_retry_cnt = 3;
    public int message_retry_interval = 500;
    public int rcv_timeout = 5; //5초 이상 Ack 수신하지 못할 시 알람 발생
    public ushort ctc_network_no = 0;
    public string abb_ip = "abb_ip";
    public int abb_port = 502;
    public string heat_exchanger_ip = "192.168.0.100";
    public int heat_exchanger_port = 502;
    public bool ctc_ip_use_send = true;
    public string ctc_ip = "abb_ip";
    public int ctc_port = 5000;
    public int trace_send_interval = 1000;

}

#endregion "Config Sokcet - Server"

#region "Config : Config_Mixing_Step"
public class Config_Mixing_Step
{
    //Max Step은 CCSS의 수와 같음, Mixing Type 장비에서만 접근 가능
    //Row = CCSS Step + 1(미사용) Not Use로 Setp +1
    public int step_cnt;
    public int ccss_cnt;
    public bool mixing_use;
    public string matching_file;
    public bool refill_use;
    public enum_ccss refill_ccss;
    public CCSS_Info[] ccss_info;
    public STEP_Info[] step_info;
    public int[] mixing_data;
}
public class CCSS_Info
{
    public string name;
    public bool use;
    public int ccss_row; //같은 레벨일 경우 동시 투입, 다른 레벨일 경우 앞 레벨 투입 완료까지 대기
    public enum_ccss type;
    public enum_chemical chemical;
}
public class STEP_Info
{
    public string name;
    public bool use;
}
public enum enum_ccss
{
    CCSS1 = 0,
    CCSS2 = 1,
    CCSS3 = 2,
    CCSS4 = 3,
}
public enum enum_chemical
{
    NONE = 0,
    DIW = 10,
    HOT_DIW = 20,
    H2O2 = 30,
    H2SO4 = 31,
    NH4OH = 40,
    HF = 50,
    DSP = 60,
    LAL = 70,
    IPA = 80
}
#endregion

#region "Config Unit IO"
public class Config_Unit_Io
{
    //35개 안쓰는 영역은 사용안함 실제 사용 개수는 23개 IO Map 참조
    public int total_cnt;
    public Unit_IO[] unit_io;
}
public class Unit_IO
{
    public enum_unit_type unit_type;
    public Int32 index;
    public string name;
    public Int32 no;
}
public enum enum_unit_type
{
    ctc,
    pmc,
    cds,
    ctc_io,
    spare
}

public enum enum_eq_type
{
    apm, dhf, dsp, dsp_mix, ipa, lal, none
}
public enum enum_eq_mode
{
    none, manual, auto
}
/// <summary>
/// none : 기존 Circulation Pump Mode / 파라메타로 Sensor 또는 Interval Mode 변경 가능
/// type1 : DSP Circulation Pump DO Mode 추가 반영 Type DO Program.ethercat_md.DO_Write_Alone((int)Config_IO.enum_do.CIRCULATION_PUMP_START, true); 로 제어함
/// </summary>
public enum enum_pump_mode
{
    none, type1, type2, type3, type4
}
public enum enum_he3320c_mode
{
    none, monitor_tank
}
public enum enum_supply_mode
{
    all, a, b
}
#endregion "Config Unit IO"

#region
public class Config_Offset_Parameter
{
    public int total_cnt;
    public Temp_Offset[] temp_offset;
}
public class Temp_Offset
{
    public string name = "";
    public double offset;
}

#endregion

