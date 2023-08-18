using System;
using System.Collections.Generic;
//1.0.0.0
namespace LogManager
{
    public class Config_Main
    {
        public App_Path path;
        public MariaDB.Target[] db_target;
    }

    #region "App Path"
    public class App_Path
    {
        private string _yaml;
        private string _log;

        public string yaml
        {
            //get { return _yaml = System.Windows.Forms.Application.StartupPath + @"\yaml\"; }
            get { return _yaml; }
            set { _yaml = value; }
        }
        public string log
        {
            //get { return _yaml = System.Windows.Forms.Application.StartupPath + @"\yaml\"; }
            get { return _log; }
            set { _log = value; }
        }
    }
    #endregion

    public class Config_App_Info
    {
        public bool activate = true;
        public int interval = 1; // hour
        public int use_count_file = 5; // maximun 20
        public int use_count_db = 5; // maximun 20
    }

    public class Config_log_path
    {
        public log_path[] log;
    }
    public class Config_db_path
    {
        public db_path[] db;
    }
    public class log_path
    {
        public string path = "";
        public int date = 90;
    }

    public class db_path
    {
        public string database = "cds";
        public string table = "alarm_logs";
        public string field = "alarm_occurred_time";
        public int date = 90;
    }

}



#region "MariaDB"
public class MariaDB
{
    public class Target
    {
        public string ip = "127.0.0.1";
        public int port = 3306;
        public string driver = "MariaDB ODBC 3.1 Driver";
        public string database = "cds";
        public string id = "root";
        public string password = "1234";
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

#region "DB Query_Parameter"
public class Query_Parameter
{
    public DateTime start;
    public DateTime end;
    public List<string> token = new List<string>();
}
#endregion
