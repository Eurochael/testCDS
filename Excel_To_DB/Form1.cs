using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Windows.Forms;

namespace Excel_To_DB
{
    public partial class Form1 : Form
    {
        public static DB_MARIA.MainModule DB_MARIA = new DB_MARIA.MainModule();
        public Local_cds db_local_cds = new Local_cds();
        public Form1()
        {
            InitializeComponent();
        }
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

            public Boolean display = false;
            public DateTime first_ontime;

        }

        public class Config_Parameter
        {
            public int id;
            public string name;
            public int value;
            public string unit;
            public string comment;
            public int value_min;
            public int value_max;
            public byte report_to_host;
            public byte visible;
        }

        public class Local_cds
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
        public string query = "", err = "";
        public int result_qry = 0;
        public string result = "";
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                db_local_cds.ip = txt_db_ip.Text;
                db_local_cds.password = txt_db_pass.Text;
                lbox_item.Items.Clear();

                IWorkbook workbook = spreadsheetControl1.Document;
                WorksheetCollection worksheets = workbook.Worksheets;
                Config_Alarm Alarm_object = new Config_Alarm();
                Worksheet worksheet = workbook.Worksheets[0];

                if (chk_selected_item_act.Checked == true)
                {

                    if (worksheet.GetCellValue(0, worksheet.SelectedCell.TopRowIndex).ToString() == "" || worksheet.GetCellValue(1, worksheet.SelectedCell.TopRowIndex).ToString() == "")
                    {
                        return;

                    }
                    //초기화
                    Alarm_object.id = 0;
                    Alarm_object.name = "";
                    Alarm_object.comment = "";
                    Alarm_object.visible = 1;
                    Alarm_object.enable = 1;

                    //Alarm No
                    if (IsNumeric(worksheet.GetCellValue(0, worksheet.SelectedCell.TopRowIndex).ToString()) == true)
                    {
                        Alarm_object.id = Convert.ToInt32(worksheet.GetCellValue(0, worksheet.SelectedCell.TopRowIndex).ToString());
                    }
                    else
                    {
                        Alarm_object.id = -1;
                    }
                    //Alarm Name
                    Alarm_object.name = worksheet.GetCellValue(1, worksheet.SelectedCell.TopRowIndex).ToString();
                    //Alarm Comment
                    Alarm_object.comment = worksheet.GetCellValue(2, worksheet.SelectedCell.TopRowIndex).ToString();

                    //Alarm Enable
                    if (worksheet.GetCellValue(4, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "0")
                    {
                        Alarm_object.enable = 0;
                    }
                    else if (worksheet.GetCellValue(4, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "1")
                    {
                        Alarm_object.enable = 1;
                    }

                    //Alarm WDT
                    if (IsNumeric(worksheet.GetCellValue(5, worksheet.SelectedCell.TopRowIndex).ToString()) == true)
                    {
                        Alarm_object.wdt = Convert.ToInt32(worksheet.GetCellValue(5, worksheet.SelectedCell.TopRowIndex).ToString());
                    }
                    else
                    {
                        Alarm_object.wdt = 0;
                    }

                    //Alarm Report To Host
                    if (worksheet.GetCellValue(6, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "0")
                    {
                        Alarm_object.report_to_host = 0;
                    }
                    else if (worksheet.GetCellValue(6, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "1")
                    {
                        Alarm_object.report_to_host = 1;
                    }

                    //Alarm Visible
                    if (worksheet.GetCellValue(7, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "0")
                    {
                        Alarm_object.visible = 0;
                    }
                    else if (worksheet.GetCellValue(7, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "1")
                    {
                        Alarm_object.visible = 1;
                    }

                    //Alarm Level
                    if (worksheet.GetCellValue(8, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "0")
                    {
                        Alarm_object.level = 0;
                    }
                    else if (worksheet.GetCellValue(8, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "1")
                    {
                        Alarm_object.level = 1;
                    }
                    else if (worksheet.GetCellValue(8, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "2")
                    {
                        Alarm_object.level = 2;
                    }
                    else
                    {
                        Alarm_object.level = 0;
                    }

                    //Alarm Unit
                    if (worksheet.GetCellValue(7, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "0")
                    {
                        Alarm_object.unit = 0;
                    }
                    else if (worksheet.GetCellValue(7, worksheet.SelectedCell.TopRowIndex).ToString().ToUpper() == "1")
                    {
                        Alarm_object.unit = 1;
                    }


                    if (Alarm_object.name != "")
                    {
                        Excel_To_Database(Alarm_object);
                    }

                    lbox_item.Items.Insert(0, "ROW : " + worksheet.SelectedCell.TopRowIndex + " / " + "OK");
                    return;
                }
                else
                {

                }

                if (chk_all_delete.Checked == true)
                {
                    query = "Delete From alarm_logs";
                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 0)
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                    }

                    query = "Delete From alarm_list";
                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 0)
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                    }
                }


                int for_idx_cnt = 0;//알람 NO 예비 번호로 부여
                for (int idx = 0; idx < 1; idx++)
                {

                    for (int idx2 = 1; idx2 < spinedit_row_maxcnt.Value; idx2++)
                    //제목 제외
                    {
                        if (worksheet.GetCellValue(0, idx2).ToString() == "" || worksheet.GetCellValue(1, idx2).ToString() == "")
                        {
                            return;

                        }
                        //초기화
                        Alarm_object.id = 0;
                        Alarm_object.name = "";
                        Alarm_object.comment = "";
                        Alarm_object.visible = 1;
                        Alarm_object.enable = 1;

                        //Alarm No
                        if (IsNumeric(worksheet.GetCellValue(0, idx2).ToString()) == true)
                        {
                            Alarm_object.id = Convert.ToInt32(worksheet.GetCellValue(0, idx2).ToString());
                        }
                        else
                        {
                            Alarm_object.id = for_idx_cnt;
                        }
                        //Alarm Name
                        Alarm_object.name = worksheet.GetCellValue(1, idx2).ToString();
                        //Alarm Comment
                        Alarm_object.comment = worksheet.GetCellValue(2, idx2).ToString();

                        //Alarm Enable
                        if (worksheet.GetCellValue(4, idx2).ToString().ToUpper() == "0")
                        {
                            Alarm_object.enable = 0;
                        }
                        else if (worksheet.GetCellValue(4, idx2).ToString().ToUpper() == "1")
                        {
                            Alarm_object.enable = 1;
                        }

                        //Alarm WDT
                        if (IsNumeric(worksheet.GetCellValue(5, idx2).ToString()) == true)
                        {
                            Alarm_object.wdt = Convert.ToInt32(worksheet.GetCellValue(5, idx2).ToString());
                        }
                        else
                        {
                            Alarm_object.wdt = 0;
                        }

                        //Alarm Report To Host
                        if (worksheet.GetCellValue(6, idx2).ToString().ToUpper() == "0")
                        {
                            Alarm_object.report_to_host = 0;
                        }
                        else if (worksheet.GetCellValue(6, idx2).ToString().ToUpper() == "1")
                        {
                            Alarm_object.report_to_host = 1;
                        }

                        //Alarm Visible
                        if (worksheet.GetCellValue(7, idx2).ToString().ToUpper() == "0")
                        {
                            Alarm_object.visible = 0;
                        }
                        else if (worksheet.GetCellValue(7, idx2).ToString().ToUpper() == "1")
                        {
                            Alarm_object.visible = 1;
                        }

                        //Alarm Level
                        if (worksheet.GetCellValue(8, idx2).ToString().ToUpper() == "0")
                        {
                            Alarm_object.level = 0;
                        }
                        else if (worksheet.GetCellValue(8, idx2).ToString().ToUpper() == "1")
                        {
                            Alarm_object.level = 1;
                        }
                        else if (worksheet.GetCellValue(8, idx2).ToString().ToUpper() == "2")
                        {
                            Alarm_object.level = 2;
                        }
                        else
                        {
                            Alarm_object.level = 0;
                        }

                        //Alarm Unit
                        if (worksheet.GetCellValue(7, idx2).ToString().ToUpper() == "0")
                        {
                            Alarm_object.unit = 0;
                        }
                        else if (worksheet.GetCellValue(7, idx2).ToString().ToUpper() == "1")
                        {
                            Alarm_object.unit = 1;
                        }


                        if (Alarm_object.name != "")
                        {
                            Excel_To_Database(Alarm_object);
                        }
                        for_idx_cnt++;
                        lbox_item.Items.Insert(0, "ROW : " + idx2 + " / " + "OK");
                    }

                }

            }
            catch (Exception ex)
            {
                lbox_item.Items.Insert(0, ex.ToString());
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            db_local_cds.ip = txt_db_ip.Text;
            db_local_cds.password = txt_db_pass.Text;
            lbox_item.Items.Clear();
            try
            {
                IWorkbook workbook = spreadsheetControl1.Document;
                WorksheetCollection worksheets = workbook.Worksheets;
                Config_Parameter para_object = new Config_Parameter();
                int for_idx_cnt = 0;//PARA NO 예비 번호로 부여
                Worksheet worksheet = workbook.Worksheets[0];

                if(chk_comment.Checked == true)
                {
                    para_object.comment = "";
                    para_object.id = Convert.ToInt32(worksheet.GetCellValue(0, worksheet.SelectedCell.TopRowIndex).ToString());
                    //comment
                    para_object.comment = worksheet.GetCellValue(4, worksheet.SelectedCell.TopRowIndex).ToString();

                    query = "Update parameters";
                    query += "    ,cds_parameter_comment = '" + para_object.comment.ToString() + "'";
                    query += "WHERE cds_parameter_id = '" + para_object.id + "'";

                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);

                }
                else if (chk_selected_item_act.Checked == true)
                {

                    if (worksheet.GetCellValue(0, worksheet.SelectedCell.TopRowIndex).ToString() == "" || worksheet.GetCellValue(1, worksheet.SelectedCell.TopRowIndex).ToString() == "")
                    {
                        return;
                    }
                    //초기화
                    para_object.id = 0;
                    para_object.name = "";
                    para_object.comment = "";
                    para_object.visible = 1;
                    para_object.report_to_host = 0;
                    //No
                    para_object.id = Convert.ToInt32(worksheet.GetCellValue(0, worksheet.SelectedCell.TopRowIndex).ToString());
                    //Name
                    para_object.name = worksheet.GetCellValue(1, worksheet.SelectedCell.TopRowIndex).ToString();

                    //max
                    if (IsNumeric(worksheet.GetCellValue(6, worksheet.SelectedCell.TopRowIndex).ToString()) == true)
                    {
                        para_object.value_max = Convert.ToInt32(worksheet.GetCellValue(6, worksheet.SelectedCell.TopRowIndex).ToString());
                    }
                    else
                    {
                        para_object.value_max = 0;
                    }
                    //value
                    if (IsNumeric(worksheet.GetCellValue(2, worksheet.SelectedCell.TopRowIndex).ToString()) == true)
                    {
                        para_object.value = Convert.ToInt32(worksheet.GetCellValue(2, worksheet.SelectedCell.TopRowIndex).ToString());
                    }
                    else
                    {
                        para_object.value = 0;
                    }
                    //min
                    if (IsNumeric(worksheet.GetCellValue(5, worksheet.SelectedCell.TopRowIndex).ToString()) == true)
                    {
                        para_object.value_min = Convert.ToInt32(worksheet.GetCellValue(5, worksheet.SelectedCell.TopRowIndex).ToString());
                    }
                    else
                    {
                        para_object.value_min = 0;
                    }
                    //unit
                    para_object.unit = worksheet.GetCellValue(3, worksheet.SelectedCell.TopRowIndex).ToString();

                    //comment
                    para_object.comment = worksheet.GetCellValue(4, worksheet.SelectedCell.TopRowIndex).ToString();

                    if (para_object.name != "" && para_object.comment != "")
                    {
                        Excel_To_Database(para_object);
                    }
                    for_idx_cnt++;

                    lbox_item.Items.Insert(0, "ROW : " + worksheet.SelectedCell.TopRowIndex + " / " + "OK");
                    return;
                }
                else
                {

                }

                if (chk_all_delete.Checked == true)
                {
                    query = "Delete From parameters";
                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 0)
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                    }

                }



                for (int idx = 0; idx < 1; idx++)
                {
                    for (int idx2 = 1; idx2 < spinedit_row_maxcnt.Value; idx2++)
                    //제목 제외
                    {
                        if(worksheet.GetCellValue(0, idx2).ToString() == "" || worksheet.GetCellValue(1, idx2).ToString() == "")
                        {
                            return;
                        }
                        //초기화
                        para_object.id = 0;
                        para_object.name = "";
                        para_object.comment = "";
                        para_object.visible = 1;
                        para_object.report_to_host = 0;
                        //No
                        para_object.id = Convert.ToInt32(worksheet.GetCellValue(0, idx2).ToString());
                        //Name
                        para_object.name = worksheet.GetCellValue(1, idx2).ToString();

                        //max
                        if (IsNumeric(worksheet.GetCellValue(6, idx2).ToString()) == true)
                        {
                            para_object.value_max = Convert.ToInt32(worksheet.GetCellValue(6, idx2).ToString());
                        }
                        else
                        {
                            para_object.value_max = 0;
                        }
                        //value
                        if (IsNumeric(worksheet.GetCellValue(2, idx2).ToString()) == true)
                        {
                            para_object.value = Convert.ToInt32(worksheet.GetCellValue(2, idx2).ToString());
                        }
                        else
                        {
                            para_object.value = 0;
                        }
                        //min
                        if (IsNumeric(worksheet.GetCellValue(5, idx2).ToString()) == true)
                        {
                            para_object.value_min = Convert.ToInt32(worksheet.GetCellValue(5, idx2).ToString());
                        }
                        else
                        {
                            para_object.value_min = 0;
                        }
                        //unit
                        para_object.unit = worksheet.GetCellValue(3, idx2).ToString();

                        //comment
                        para_object.comment = worksheet.GetCellValue(4, idx2).ToString();

                        if (para_object.name != "" && para_object.comment != "")
                        {
                            Excel_To_Database(para_object);
                        }
                        for_idx_cnt++;
                        lbox_item.Items.Insert(0, "ROW : " + idx2 + " / " + "OK");
                    }

                }
            }
            catch (Exception ex)
            {
                lbox_item.Items.Insert(0, ex.ToString());
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1;
            try
            {
                openFileDialog1 = new OpenFileDialog();
                //openFileDialog1.Filter = "*.xlsx";
                openFileDialog1 = new OpenFileDialog();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    spreadsheetControl1.LoadDocument(openFileDialog1.FileName, DocumentFormat.Xlsx);
                    MessageBox.Show("Load Success");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Excel_To_Database(Config_Alarm alarm_object)
        {
            string result = "", err = "", query = "";
            DataSet dataset = new DataSet();
            int result_qry = 0;
            try
            {
                if (chk_selected_item_act.Checked == true)
                {
                    query = "INSERT INTO alarm_list( alarm_id,alarm_name,alarm_comment,alarm_enabled,alarm_wdt,report_alarm_to_host,alarm_visible,alarm_level,alarm_unit )";
                    query += " VALUES (";
                    query += "'" + alarm_object.id + "'";
                    query += ",'" + alarm_object.name + "'";
                    query += ",'" + alarm_object.comment + "'";
                    query += ",'" + alarm_object.enable + "'";
                    query += ",'" + alarm_object.wdt + "'";
                    query += ",'" + alarm_object.report_to_host + "'";
                    query += ",'" + alarm_object.visible + "'";
                    query += ",'" + alarm_object.level + "'";
                    query += ",'" + alarm_object.unit + "'";
                    query += ")";


                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 0)
                    {
                        query = "Update alarm_list";
                        query += " Set alarm_name = '" + alarm_object.name.ToString() + "'";
                        query += "    ,alarm_comment = '" + alarm_object.comment.ToString() + "'";
                        query += "    ,alarm_enabled = '" + alarm_object.enable.ToString() + "'";
                        query += "    ,alarm_wdt = '" + alarm_object.wdt.ToString() + "'";
                        query += "    ,report_alarm_to_host = '" + alarm_object.report_to_host.ToString() + "'";
                        query += "    ,alarm_visible = '" + alarm_object.visible.ToString() + "'";
                        query += "    ,alarm_level = '" + alarm_object.level.ToString() + "'";
                        query += "    ,alarm_unit = '" + alarm_object.unit.ToString() + "'";
                        query += "WHERE alarm_id = '" + alarm_object.id + "'";

                        result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                        if (result_qry == 1)
                        {
                            lbox_item.Items.Insert(0, "Update : " + "OK");
                            //동일 데이타 업데이트 시 return 0
                            //Insert 또는 변동 데이터 업데이트 시 return 1
                            result = "";
                        }
                        else
                        {
                            lbox_item.Items.Insert(0, "Update : " + "FAIL");
                            result = err;
                        }

                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                        lbox_item.Items.Insert(0, "Insert : " + result);
                    }
                    return;
                }


                if (chk_update.Checked == true)
                {
                    query = "Update alarm_list";
                    query += " Set alarm_name = '" + alarm_object.name.ToString() + "'";
                    query += "    ,alarm_comment = '" + alarm_object.comment.ToString() + "'";
                    query += "WHERE alarm_id = '" + alarm_object.id + "'";

                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 1)
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                    }
                }
                else if (chk_allupdate.Checked == true)
                {
                    query = "Update alarm_list";
                    query += " Set alarm_name = '" + alarm_object.name.ToString() + "'";
                    query += "    ,alarm_comment = '" + alarm_object.comment.ToString() + "'";
                    query += "    ,alarm_enabled = '" + alarm_object.enable.ToString() + "'";
                    query += "    ,alarm_wdt = '" + alarm_object.wdt.ToString() + "'";
                    query += "    ,report_alarm_to_host = '" + alarm_object.report_to_host.ToString() + "'";
                    query += "    ,alarm_visible = '" + alarm_object.visible.ToString() + "'";
                    query += "    ,alarm_level = '" + alarm_object.level.ToString() + "'";
                    query += "    ,alarm_unit = '" + alarm_object.unit.ToString() + "'";
                    query += "WHERE alarm_id = '" + alarm_object.id + "'";

                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 1)
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                    }
                }
                else
                {
                    query = "INSERT INTO alarm_list( alarm_id,alarm_name,alarm_comment,alarm_enabled,alarm_wdt,report_alarm_to_host,alarm_visible,alarm_level,alarm_unit )";
                    query += " VALUES (";
                    query += "'" + alarm_object.id + "'";
                    query += ",'" + alarm_object.name + "'";
                    query += ",'" + alarm_object.comment + "'";
                    query += ",'" + alarm_object.enable + "'";
                    query += ",'" + alarm_object.wdt + "'";
                    query += ",'" + alarm_object.report_to_host + "'";
                    query += ",'" + alarm_object.visible + "'";
                    query += ",'" + alarm_object.level + "'";
                    query += ",'" + alarm_object.unit + "'";
                    query += ")";

                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 1)
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                    }
                }



            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
        }
        public void Excel_To_Database(Config_Parameter para_object)
        {
            string result = "", err = "", query = "";
            DataSet dataset = new DataSet();
            int result_qry = 0;
            try
            {

                if(chk_selected_item_act.Checked == true)
                {
                    query = "INSERT INTO parameters(cds_parameter_id,cds_parameter_name,cds_parameter_value,cds_parameter_unit,cds_parameter_comment,cds_parameter_minimum,cds_parameter_maximum,report_cds_parameter_to_host,cds_parameter_visible )";
                    query += " VALUES (";
                    query += "'" + para_object.id + "'";
                    query += ",'" + para_object.name + "'";
                    query += ",'" + para_object.value + "'";
                    query += ",'" + para_object.unit + "'";
                    query += ",'" + para_object.comment + "'";
                    query += ",'" + para_object.value_min + "'";
                    query += ",'" + para_object.value_max + "'";
                    query += ",'" + para_object.report_to_host + "'";
                    query += ",'" + para_object.visible + "'";
                    query += ")";

                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 0)
                    {
                        query = "Update parameters";
                        query += " Set cds_parameter_name = '" + para_object.name.ToString() + "'";
                        query += "    ,cds_parameter_value = '" + para_object.value.ToString() + "'";
                        query += "    ,cds_parameter_unit = '" + para_object.unit.ToString() + "'";
                        query += "    ,cds_parameter_comment = '" + para_object.comment.ToString() + "'";
                        query += "    ,cds_parameter_minimum = '" + para_object.value_min.ToString() + "'";
                        query += "    ,cds_parameter_maximum = '" + para_object.value_max.ToString() + "'";
                        query += "    ,report_cds_parameter_to_host = '" + para_object.report_to_host.ToString() + "'";
                        query += "    ,cds_parameter_visible = '" + para_object.visible.ToString() + "'";
                        query += "WHERE cds_parameter_id = '" + para_object.id + "'";

                        result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                        if (result_qry == 1)
                        {
                            lbox_item.Items.Insert(0, "Update : " + "OK");
                            //동일 데이타 업데이트 시 return 0
                            //Insert 또는 변동 데이터 업데이트 시 return 1
                            result = "";
                        }
                        else
                        {
                            lbox_item.Items.Insert(0, "Update : " + "FAIL");
                            result = err;
                        }

                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                        lbox_item.Items.Insert(0, "Insert : " + result);
                    }
                    return;
                }

                if (chk_update.Checked == true)
                {
                    query = "Update parameters";
                    query += " Set cds_parameter_name = '" + para_object.name.ToString() + "'";
                    query += "    ,cds_parameter_comment = '" + para_object.comment.ToString() + "'";
                    query += "WHERE cds_parameter_id = '" + para_object.id + "'";

                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 1)
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                    }
                }
                else if (chk_allupdate.Checked == true)
                {
                    query = "Update parameters";
                    query += " Set cds_parameter_name = '" + para_object.name.ToString() + "'";
                    query += "    ,cds_parameter_value = '" + para_object.value.ToString() + "'";
                    query += "    ,cds_parameter_unit = '" + para_object.unit.ToString() + "'";
                    query += "    ,cds_parameter_comment = '" + para_object.comment.ToString() + "'";
                    query += "    ,cds_parameter_minimum = '" + para_object.value_min.ToString() + "'";
                    query += "    ,cds_parameter_maximum = '" + para_object.value_max.ToString() + "'";
                    query += "    ,report_cds_parameter_to_host = '" + para_object.report_to_host.ToString() + "'";
                    query += "    ,cds_parameter_visible = '" + para_object.visible.ToString() + "'";
                    query += "WHERE cds_parameter_id = '" + para_object.id + "'";

                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 1)
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                    }
                }
                else
                {
                    query = "INSERT INTO parameters(cds_parameter_id,cds_parameter_name,cds_parameter_value,cds_parameter_unit,cds_parameter_comment,cds_parameter_minimum,cds_parameter_maximum,report_cds_parameter_to_host,cds_parameter_visible )";
                    query += " VALUES (";
                    query += "'" + para_object.id + "'";
                    query += ",'" + para_object.name + "'";
                    query += ",'" + para_object.value + "'";
                    query += ",'" + para_object.unit + "'";
                    query += ",'" + para_object.comment + "'";
                    query += ",'" + para_object.value_min + "'";
                    query += ",'" + para_object.value_max + "'";
                    query += ",'" + para_object.report_to_host + "'";
                    query += ",'" + para_object.visible + "'";
                    query += ")";

                    result_qry = MariaDB_MainQuery(db_local_cds.connection, query, ref err);
                    if (result_qry == 1)
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                    }
                }




            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
        }

        public int MariaDB_MainQuery(string connection, string str_Query, ref string str_Error)
        {
            int int_Query = 0;
            try
            {
                str_Error = DB_MARIA.ExecuteQuery(connection, str_Query, ref int_Query);
                return int_Query;
            }
            catch { return int_Query; }
        }
        public int MariaDB_MainQuery(string connection, string str_Query, string str_Key, Byte[] byte_Data, ref string str_Error)
        {
            int int_Query = 0;
            try
            {
                str_Error = DB_MARIA.ExecuteQuery(connection, str_Query, str_Key, byte_Data, ref int_Query);
                return int_Query;
            }
            catch { return int_Query; }
        }
        public bool MariaDB_MainDataSet(string connection, string str_Query, DataSet dataset_GedDataSet, ref string str_Error, string str_DataSetName = "DataSET")
        {
            try
            {
                if (str_Query == "") { return false; }
                str_Error = DB_MARIA.GetDataSet(connection, str_Query, dataset_GedDataSet, str_DataSetName);
                if (str_Error == "") { return true; }
                else { return false; }
            }

            catch { return false; }

        }

        private void chk_selected_item_act_CheckedChanged(object sender, EventArgs e)
        {
            if(chk_selected_item_act.Checked == true)
            {
                chk_all_delete.Checked = false;
                chk_update.Checked = false;
                chk_allupdate.Checked = false;
            }
        }

        public bool IsNumeric(string input)
        {
            Double number = 0;
            return Double.TryParse(input, out number);
        }
    }
}
