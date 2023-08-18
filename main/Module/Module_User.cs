using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cds
{
    class Module_User
    {
        public struct User_Info
        {
            public string id;
            public string password;
            public DateTime check_in;
            public User_Type type;
            public DateTime start_time;
            public Access_UI access_ui;
            public Access_Modify_Alarm access_alarm;
            public Access_Modify_Parameter access_parameter;
        }
        public enum User_Type
        {//enumindex equal to grid array index
            None = 0,
            Develop = 1,
            Admin = 2,
            Engineer = 3,
            User = 4
        }


        public struct Access_UI
        {
            public bool schematic;
            public bool io_monitor;
            public bool alarm;
            public bool parameter;
            public bool mixing_step;
            public bool trend_log;
            public bool event_log;
            public bool alarm_log;
            public bool total_usage;
        }
        public struct Access_Modify_Alarm
        {
            public bool id;
            public bool name;
            public bool wdt;
            public bool level;
            public bool use;
            public bool comment;
            public bool host_send;
        }
        public struct Access_Modify_Parameter
        {
            public bool id;
            public bool name;
            public bool min;
            public bool set;
            public bool max;
            public bool comment;
            public bool ec_interlock;
        }

    }
}
