using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LogManager
{
    public partial class frm_main : Form
    {
        #region data_operate 사용 변수
        public structure_data_operate_para st_data_operate_para;
        public struct structure_data_operate_para
        {
            public int subtimer_count;//= 10;
            public TimeSpan[] tp_check_interval;//= new TimeSpan[ui_change_subtimer_count];
            public DateTime[] dt_check_last_act;//= new DateTime[ui_change_subtimer_count];
            public Boolean timer_initial;// = false;
        }
        #endregion

        uc_targetbox_log[] targetbox_log;
        uc_targetbox_db[] targetbox_db;
        uc_targetbox_work[] targetbox_work;
        public DateTime date_acttime;
        public Thread thd_data_operate;
        public Queue<string> q_log = new Queue<string>();
        public frm_main()
        {
            InitializeComponent();
        }
        private void frm_main_Load(object sender, EventArgs e)
        {
            Form_Visible(false);
            Initial_YAML();
            Initial();
        }

        private void Initial_YAML()
        {
            string result = "";
            result = Module_XML.XML_Inidata_Read(Application.StartupPath + "/Setting.xml", "main/path/", "main_config");
            try
            {
                if (result == "")
                {
                    Program.log_md.LogWrite(this.Name + ".Initial_YAML : " + " Path Error & Fail", Module_Log.enumLog.Process, "");
                }
                else
                {
                    Program.cg_main = Program.yaml_md.DeSerialize<Config_Main>(result, @"\logmanager_config_main.yaml");
                    Program.cg_appinfo = Program.yaml_md.DeSerialize<Config_App_Info>(result, @"\logmanager_config_app_info.yaml");
                    Program.cg_logpath = Program.yaml_md.DeSerialize<Config_log_path>(result, @"\logmanager_config_log_path.yaml");
                    Program.cg_dbpath = Program.yaml_md.DeSerialize<Config_db_path>(result, @"\logmanager_config_db_path.yaml");
                    Program.log_md.LogWrite(this.Name + ".Initial_YAML : " + "Config Load Complete", Module_Log.enumLog.Process, "");
                }
            }
            catch(Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".delete_log_file " + ex.Message, Module_Log.enumLog.Error, "");
            }
          
        }

        private void Initial()
        {
            dlbl_info.Text = @"※When removing a folder, it works as path\folder\deletefolder※";
            dlbl_info.ForeColor = Color.OrangeRed;

            st_data_operate_para.subtimer_count = 10;
            st_data_operate_para.timer_initial = false;
            st_data_operate_para.tp_check_interval = new TimeSpan[st_data_operate_para.subtimer_count];
            st_data_operate_para.dt_check_last_act = new DateTime[st_data_operate_para.subtimer_count];

            this.Text = "Log Manager" + "(" + Application.ProductVersion.ToString() + ")";

            //Target log initial
            targetbox_log = new uc_targetbox_log[Program.cg_appinfo.use_count_file];

            DoubleBufferedPanel[] dpnl_buff = new DoubleBufferedPanel[Program.cg_appinfo.use_count_file];
            for (int i = Program.cg_appinfo.use_count_file - 1; i > -1; i--)
            {
                dpnl_buff[i] = new DoubleBufferedPanel();
                dpnl_buff[i].Dock = DockStyle.Top;
                dpnl_buff[i].Height = 10;
                dpnl_location_log.Controls.Add(dpnl_buff[i]);

                targetbox_log[i] = new uc_targetbox_log();
                targetbox_log[i].str_name = "Path" + (i + 1).ToString("00") + " : ";
                targetbox_log[i].str_path = Program.cg_logpath.log[i].path;
                targetbox_log[i].int_date = Program.cg_logpath.log[i].date;
                targetbox_log[i].Dock = DockStyle.Top;
                dpnl_location_log.Controls.Add(targetbox_log[i]);
            }

            // Target db initial
            targetbox_db = new uc_targetbox_db[Program.cg_appinfo.use_count_db];

            DoubleBufferedPanel[] dpnl_buff2 = new DoubleBufferedPanel[Program.cg_appinfo.use_count_db];
            for (int i = Program.cg_appinfo.use_count_db - 1; i > -1; i--)
            {
                dpnl_buff2[i] = new DoubleBufferedPanel();
                dpnl_buff2[i].Dock = DockStyle.Top;
                dpnl_buff2[i].Height = 10;
                dpnl_location_db.Controls.Add(dpnl_buff2[i]);

                targetbox_db[i] = new uc_targetbox_db();
                targetbox_db[i].str_name = "Query" + (i + 1).ToString("00") + " : ";
                targetbox_db[i].str_db = Program.cg_dbpath.db[i].database;
                targetbox_db[i].str_table = Program.cg_dbpath.db[i].table;
                targetbox_db[i].str_field = Program.cg_dbpath.db[i].field;
                targetbox_db[i].int_date = Program.cg_dbpath.db[i].date;
                targetbox_db[i].Dock = DockStyle.Top;
                dpnl_location_db.Controls.Add(targetbox_db[i]);
            }

            // Target work initial
            targetbox_work = new uc_targetbox_work[Program.cg_appinfo.use_count_db + Program.cg_appinfo.use_count_file];
            DoubleBufferedPanel[] dpnl_buff3 = new DoubleBufferedPanel[Program.cg_appinfo.use_count_db + Program.cg_appinfo.use_count_file];
            for (int i = Program.cg_appinfo.use_count_db + Program.cg_appinfo.use_count_file - 1; i > -1; i--)
            {
                dpnl_buff3[i] = new DoubleBufferedPanel();
                dpnl_buff3[i].Dock = DockStyle.Top;
                dpnl_buff3[i].Height = 10;
                dpnl_work.Controls.Add(dpnl_buff3[i]);
                if (i > Program.cg_appinfo.use_count_file - 1)
                {
                    targetbox_work[i] = new uc_targetbox_work();
                    targetbox_work[i].str_name = "Query" + (i - 4).ToString("00") + " : ";

                }
                else
                {
                    targetbox_work[i] = new uc_targetbox_work();
                    targetbox_work[i].str_name = "Path" + (i + 1).ToString("00") + " : ";
                }
                targetbox_work[i].Dock = DockStyle.Top;
                dpnl_work.Controls.Add(targetbox_work[i]);
            }

            //Status initial
            dlbl_interval.Text = Program.cg_appinfo.interval.ToString();
            timer_uichange.Start();
            timer_checkthread.Start();
        }

        private void delete_log_file()
        {
            DirectoryInfo[] di;
            int delcount;
            string log = "";
            try
            {
                delcount = 0;
                di = new DirectoryInfo[Program.cg_appinfo.use_count_file];

                for (int i = 0; i < Program.cg_appinfo.use_count_file; i++)
                {
                    if (Directory.Exists(Program.cg_logpath.log[i].path) == true)
                    {
                        di[i] = new DirectoryInfo(Program.cg_logpath.log[i].path);

                        log = "";
                        foreach (FileInfo fileinfo in di[i].GetFiles("*", SearchOption.AllDirectories))
                        {
                            if (fileinfo.CreationTime < DateTime.Now.AddDays(-1 * Program.cg_logpath.log[i].date))
                            {
                                //Program.log_md.LogWrite(fileinfo.FullName, Module_Log.enumLog.DEBUG, "");
                                try
                                {
                                    fileinfo.Delete();
                                    log = "File Delete Success : " + fileinfo.FullName;
                                    Program.log_md.LogWrite(this.Name + ".delete_log_file :" + log, Module_Log.enumLog.Process, "");
                                    delcount += 1;
                                }
                                catch (Exception ex)
                                {
                                    log = "File Delete Fail : " + fileinfo.FullName + " / " + ex.ToString();
                                    Program.log_md.LogWrite(this.Name + ".delete_log_file :" + log, Module_Log.enumLog.Process, "");
                                }
                                finally
                                {
                                    queue_log_manager(log);
                                }

                                System.Threading.Thread.Sleep(20);
                            }

                        }
                        // 노드 Level 3 이하부터 제거(상위에서 파일 제거 후 남은 폴더만 제거를 위함)
                        // Ex 노드 
                        // Level1 - Log
                        // Level2 - IO, Log_AppInfo, Log_debug, Log_Seq 등
                        // Level3 - 2022-11-08 날짜
                        // Level4 - 
                        // Level5 - 
                        //D:\\CDS\\Log\\ IO \\ 2023-01-30 \\ Digital \\ In 
                        foreach (DirectoryInfo directoryinfo in di[i].GetDirectories("*", SearchOption.AllDirectories))
                        {
                            if (directoryinfo.Exists == true)
                            {
                                foreach (DirectoryInfo directoryInfo2 in directoryinfo.GetDirectories("*", SearchOption.AllDirectories))
                                {
                                    try
                                    {
                                        if (directoryInfo2.Exists == true)
                                        {
                                            foreach (DirectoryInfo directoryInfo3 in directoryInfo2.GetDirectories("*", SearchOption.AllDirectories))
                                            {
                                                try
                                                {
                                                    if (directoryInfo3.Exists == true)
                                                    {
                                                        foreach (DirectoryInfo directoryInfo4 in directoryInfo3.GetDirectories("*", SearchOption.AllDirectories))
                                                        {
                                                            try
                                                            {
                                                                if (directoryInfo4.Exists == true)
                                                                {
                                                                    foreach (DirectoryInfo directoryInfo5 in directoryInfo4.GetDirectories("*", SearchOption.AllDirectories))
                                                                    {
                                                                        if (directoryInfo5.CreationTime < DateTime.Now.AddDays(-1 * Program.cg_logpath.log[i].date))
                                                                        {
                                                                            Dir_Delete_Funtion(directoryInfo5);
                                                                            System.Threading.Thread.Sleep(10);
                                                                        }
                                                                    }
                                                                }
                                                                if (directoryInfo4.CreationTime < DateTime.Now.AddDays(-1 * Program.cg_logpath.log[i].date))
                                                                {
                                                                    Dir_Delete_Funtion(directoryInfo4);
                                                                    System.Threading.Thread.Sleep(10);
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }

                                                        }
                                                    }
                                                    if (directoryInfo3.CreationTime < DateTime.Now.AddDays(-1 * Program.cg_logpath.log[i].date))
                                                    {
                                                        Dir_Delete_Funtion(directoryInfo3);
                                                        System.Threading.Thread.Sleep(10);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                            }
                                        }
                                        if (directoryInfo2.CreationTime < DateTime.Now.AddDays(-1 * Program.cg_logpath.log[i].date))
                                        {
                                            Dir_Delete_Funtion(directoryInfo2);
                                            System.Threading.Thread.Sleep(10);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }
                            }
                        }
                        if (delcount > 0)
                        {
                            targetbox_work[i].str_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        targetbox_work[i].int_count = di[i].GetFiles("*", SearchOption.AllDirectories).Length;
                        targetbox_work[i].int_delcount = delcount;

                    }
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".delete_log_file " + ex.Message, Module_Log.enumLog.Error, "");
            }

        }
        public bool Dir_Delete_Funtion(DirectoryInfo dir_info)
        {
            string log = "";
            bool result = false;
            try
            {
                //True로 변경 금지, 변경 시 하위 폴더 전부 삭제됨
                //Ex Log/IO 하단의 모든 폴더 삭제
                Console.WriteLine(dir_info.FullName);
                dir_info.Delete(false);
                log = "Directory Delete Success : " + dir_info.FullName;
                Program.log_md.LogWrite(this.Name + ".delete_log_Directory :" + log, Module_Log.enumLog.Process, "");
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                //Directory가 비어있지 않으면, 기존 File이 제거되지 않음으로 간주
                //최상위 Driectroy는 삭제하지 않음
                //log = "Directory Delete Fail : " + directoryInfo2.FullName + " / " + ex.ToString();
                //Program.log_md.LogWrite(this.Name + ".delete_log_Directory :" + log, Module_Log.enumLog.Process, "");
            }
            finally
            {
                if (log != "") { queue_log_manager(log); }
            }
            return result;
        }
        public void queue_log_manager(string log)
        {
            if (log == "") { return; }
            if (q_log.Count >= 50) { q_log.Dequeue(); }
            q_log.Enqueue(log);
        }
        private void delete_log_db()
        {
            string qurey;
            string error;
            int count;
            string connection;
            string log = "";
            try
            {
                connection = "";
                error = "";
                for (int i = 0; i < Program.cg_appinfo.use_count_db; i++)
                {
                    for (int j = 0; j < Program.cg_main.db_target.Length; j++)
                    {
                        if (Program.cg_main.db_target[j].database == Program.cg_dbpath.db[i].database)
                        {
                            connection = Program.cg_main.db_target[j].connection;
                        }
                    }

                    if (connection != "")
                    {
                        if ((Program.cg_dbpath.db[i].table != null &&Program.cg_dbpath.db[i].table != "") || (Program.cg_dbpath.db[i].field != null && Program.cg_dbpath.db[i].field != ""))
                        {
                            qurey = "DELETE FROM " + Program.cg_dbpath.db[i].table
                         + " WHERE " + Program.cg_dbpath.db[i].field + " < '" + DateTime.Now.AddDays(-Program.cg_dbpath.db[i].date).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                            count = Program.database_md.MariaDB_MainQuery(connection, qurey, ref error);
                            if (error == "")
                            {
                                if (count > 0)
                                {
                                    log = "DB Delete Success : " + Program.cg_dbpath.db[i].table + " / " + Program.cg_dbpath.db[i].field + " / " + qurey; queue_log_manager(log);
                                    //Program.log_md.LogWrite(Program.cg_dbpath.db[i].database + "." + Program.cg_dbpath.db[i].table + " Delete Complete(" + count + ")", Module_Log.enumLog.DEBUG, "");
                                    targetbox_work[i + Program.cg_appinfo.use_count_file].int_delcount = count;
                                    targetbox_work[i + Program.cg_appinfo.use_count_file].str_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    Program.log_md.LogWrite(this.Name + ".delete_log_db :" + log, Module_Log.enumLog.Process, "");
                                }
                            }
                            else
                            {
                                log = "DB Delete Fail : " + error; 
                                Program.log_md.LogWrite(this.Name + ".delete_log_db :" + log, Module_Log.enumLog.Process, "");
                            }
                            if (log != ""){ queue_log_manager(log); }

                            qurey = "SELECT " + Program.cg_dbpath.db[i].field + " FROM " + Program.cg_dbpath.db[i].table;
                            count = Program.database_md.MariaDB_MainQuery(connection, qurey, ref error);
                            targetbox_work[i + Program.cg_appinfo.use_count_file].int_count = count;
                        }

                    }
                    System.Threading.Thread.Sleep(20);

                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".delete_log_db " + ex.Message, Module_Log.enumLog.Error, "");
            }
        }

        public void setting_dispose()
        {
            notifyIcon.Visible = false;
            Program.log_md.LogWrite(this.Name + ".setting_dispose : " + "App Close", Module_Log.enumLog.Process, "");
            System.Threading.Thread.Sleep(1000);
            timer_uichange.Enabled = false;
            timer_checkthread.Enabled = false;
            if (thd_data_operate != null) { thd_data_operate.Abort(); thd_data_operate = null; }
            if (Application.MessageLoop == true)
            {
                Application.ExitThread();
                Application.Exit();
                Environment.Exit(1);
            }
            else
            {
                Environment.Exit(1);
            }
        }

        private void timer_uichange_Tick(object sender, EventArgs e)
        {
            dlbl_acttime.Text = date_acttime.ToString("yyyy-MM-dd HH:mm:ss");

            if (Program.cg_appinfo.activate == true)
            {
                dlbl_act.Text = "true";
                dlbl_act.BackColor = Color.LimeGreen;
            }
            else
            {
                dlbl_act.Text = "false";
                dlbl_act.BackColor = Color.Red;
            }

            for (int i = 0; i < Program.cg_appinfo.use_count_db + Program.cg_appinfo.use_count_file; i++)
            {
                targetbox_work[i].ui_refresh();
            }

            if (q_log.Count > 0)
            {
                if (lbox_queue_log.Items.Count > 100)
                {
                    lbox_queue_log.Items.RemoveAt(0);
                }
                lbox_queue_log.Items.Insert(0, q_log.Dequeue().ToString());
            }


            if (thd_data_operate != null && thd_data_operate.IsAlive == true)
            {
                btn_runstatus.ForeColor = Color.Green;
            }
            else
            {
                btn_runstatus.ForeColor = Color.Red;
            }
        }

        private void timer_checkthread_Tick(object sender, EventArgs e)
        {
            check_thread();
        }

        public void check_thread()
        {
            try
            {
                if (thd_data_operate != null) { if (thd_data_operate.IsAlive == false) { thd_data_operate.Abort(); thd_data_operate = null; thd_data_operate = new Thread(data_operate); thd_data_operate.Start(); } }
                else { thd_data_operate = new Thread(data_operate); thd_data_operate.Start(); }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".check_thread " + ex.Message, Module_Log.enumLog.Error, "");
            }

        }

        public void data_operate()
        {
            try
            {
                if (st_data_operate_para.timer_initial == false)
                {
                    for (int index = 0; index < st_data_operate_para.subtimer_count; index++)
                    {
                        st_data_operate_para.tp_check_interval[index] = new TimeSpan(0); st_data_operate_para.dt_check_last_act[index] = new DateTime(0);
                    }
                    st_data_operate_para.timer_initial = true;
                }

                while (true)
                {
                    for (int index = 0; index < st_data_operate_para.subtimer_count; index++)
                    {
                        st_data_operate_para.tp_check_interval[index] = DateTime.Now - st_data_operate_para.dt_check_last_act[index];
                        if (Program.cg_appinfo.activate == true)
                        {
                            switch (index)
                            {
                                case 0: // interval 마다 로그 델리트 체크
                                    if (st_data_operate_para.tp_check_interval[index].TotalHours >= Program.cg_appinfo.interval)
                                    {
                                        st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                                        delete_log_file();
                                        delete_log_db();
                                        date_acttime = DateTime.Now;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            st_data_operate_para.dt_check_last_act[index] = DateTime.Now;
                        }
                    }
                    Thread.Sleep(100);
                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".data_operate " + ex.Message, Module_Log.enumLog.Error, "");
            }
        }
        private void btn_Click(object sender, EventArgs e)
        {
            Button btn_event = (Button)sender;
            int i;
            string result = "";
            try
            {
                Program.log_md.LogWrite(this.Name + ".btn_Click : " + btn_event.Name, Module_Log.enumLog.Process, "");
                switch (btn_event.Name)
                {
                    case "btn_delete":
                        if (Program.cg_appinfo.activate == true)
                        {
                            lbox_queue_log.Items.Clear();
                        }
                        break;
                    case "btn_force_act":
                        for (int index = 0; index < st_data_operate_para.subtimer_count; index++)
                        {
                            st_data_operate_para.dt_check_last_act[index] = DateTime.Now.AddDays(-1);
                        }

                        break;


                    case "btn_save":
                        if (MessageBox.Show("Are you sure you want to save?", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            result = Module_XML.XML_Inidata_Read(Application.StartupPath + "/setting.xml", "main/path/", "main_config");
                            for (i = 0; i < Program.cg_appinfo.use_count_file; i++)
                            {
                                Program.cg_logpath.log[i].path = targetbox_log[i].str_path;
                                Program.cg_logpath.log[i].date = targetbox_log[i].int_date;
                            }

                            Program.yaml_md.Serialize<Config_log_path>(result + @"\", "logmanager_config_log_path.yaml", Program.cg_logpath);

                            for (i = 0; i < Program.cg_appinfo.use_count_db; i++)
                            {
                                Program.cg_dbpath.db[i].database = targetbox_db[i].str_db;
                                Program.cg_dbpath.db[i].table = targetbox_db[i].str_table;
                                Program.cg_dbpath.db[i].field = targetbox_db[i].str_field;
                                Program.cg_dbpath.db[i].date = targetbox_db[i].int_date;
                            }

                            Program.yaml_md.Serialize<Config_db_path>(result + @"\", "logmanager_config_db_path.yaml", Program.cg_dbpath);

                            Program.yaml_md.Serialize<Config_App_Info>(result + @"\", "logmanager_config_app_info.yaml", Program.cg_appinfo);
                        }
                        break;
                    case "btn_return":
                        for (i = Program.cg_appinfo.use_count_file - 1; i > -1; i--)
                        {
                            targetbox_log[i].str_path = Program.cg_logpath.log[i].path;
                            targetbox_log[i].int_date = Program.cg_logpath.log[i].date;
                            targetbox_log[i].return_data();
                        }

                        for (i = Program.cg_appinfo.use_count_db - 1; i > -1; i--)
                        {
                            targetbox_db[i].str_db = Program.cg_dbpath.db[i].database;
                            targetbox_db[i].str_table = Program.cg_dbpath.db[i].table;
                            targetbox_db[i].str_field = Program.cg_dbpath.db[i].field;
                            targetbox_db[i].int_date = Program.cg_dbpath.db[i].date;
                            targetbox_db[i].return_data();
                        }
                        break;
                    case "btn_enable":
                        if (MessageBox.Show("Are you sure you want to change activation?", "change activation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            if (Program.cg_appinfo.activate == true)
                            {
                                Program.cg_appinfo.activate = false;
                            }
                            else
                            {
                                Program.cg_appinfo.activate = true;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite(this.Name + ".btn_Click " + ex.Message, Module_Log.enumLog.Error, "");
            }
            finally
            {
            }
        }

        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Form_Visible(false);
        }

        private void programEndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item_event = (ToolStripMenuItem)sender;
            switch (item_event.Text)
            {
                case "Show":
                    Form_Visible(true);
                    break;
                case "End":
                    setting_dispose();
                    break;
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Form_Visible(true);
        }
        private void Form_Visible(bool show)
        {
            if (show == true)
            {
                dbp_main.SuspendLayout();
                dpnl_location_log.SuspendLayout();
                dpnl_location_db.SuspendLayout();
                dpnl_work.SuspendLayout();
                for (int i = Program.cg_appinfo.use_count_file - 1; i > -1; i--)
                {
                    targetbox_log[i].SuspendLayout();
                }
                for (int i = Program.cg_appinfo.use_count_db - 1; i > -1; i--)
                {
                    targetbox_db[i].SuspendLayout();
                }
                this.Visible = true;
                this.ShowInTaskbar = true;
                notifyIcon.Visible = false;


                dbp_main.ResumeLayout();
                dpnl_location_log.ResumeLayout();
                dpnl_location_db.ResumeLayout();
                dpnl_work.ResumeLayout();
                for (int i = Program.cg_appinfo.use_count_file - 1; i > -1; i--)
                {
                    targetbox_log[i].ResumeLayout();
                }
                for (int i = Program.cg_appinfo.use_count_db - 1; i > -1; i--)
                {
                    targetbox_db[i].ResumeLayout();
                }
            }
            else
            {
                this.Hide();
                this.Visible = false;
                this.ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }

        }

        private void btn_runstatus_Click(object sender, EventArgs e)
        {

        }
    }
}
