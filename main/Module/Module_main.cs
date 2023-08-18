using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace cds
{
    class Module_main
    {

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentProcessId();
        [DllImport("kernel32.dll")]
        public static extern int GetProcessWorkingSetSize(int hProcess, int lpMinimumWorkingSetSize, int lpMaximumWorkingSetSize);
        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern int SetProcessWorkingSetSize(int hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(int hObject);

        #region DUMP
        [DllImport("Dbghelp.dll")]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, int ProcessId, IntPtr hFile, int DumpType, ref MINIDUMP_EXCEPTION_INFORMATION ExceptionParam, IntPtr UserStreamParam, IntPtr CallbackParam);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MINIDUMP_EXCEPTION_INFORMATION
        {
            public uint ThreadId;
            public IntPtr ExceptionPointers;
            public int CientPointers;
        }

        const int MiniDumpNormal = 0x00000000;
        const int MiniDumpWithFullMemory = 0x00000002;
        #endregion DUMP

        //현재 로그인한 사용자 정보
        public Module_User.User_Info user_info;

        //프로그램에서 1회성으로 확인 필요한 Bool 변수

        //CCSS 배관 적산량 저장시 사용


        //현재 Idle 상태 Check 변수 자동 logOut
        //Used.dll GetTick 사용 금지, Mouse 사용 유무 판단
        public Point pt_idle_old;
        public DateTime dt_idle_keep;

        //frm_process_indicator Form에서 상세 진행 사항 공유를 위한 변수
        public string process_desciption = "";

        //App Page 관리
        private App_Page app_page = new App_Page();


        #region "App Page"
        public class App_Page
        {
            public DevExpress.XtraEditors.XtraForm page_old;
            public DevExpress.XtraEditors.XtraForm page_new;
        }
        #endregion

        public void FormShow(DevExpress.XtraEditors.XtraForm FormChild, System.Windows.Forms.Panel pnlMother)
        {
            try
            {
                if (pnlMother.Controls.Contains(FormChild) == false)
                {
                    FormChild.TopLevel = false;
                    pnlMother.Controls.Add(FormChild);
                }
                FormChild.SuspendLayout();
                FormChild.Dock = System.Windows.Forms.DockStyle.Fill;
                FormChild.Show();
                FormChild.Refresh();
                FormChild.BringToFront();

                app_page.page_old = app_page.page_new;
                app_page.page_new = FormChild;

                if (app_page.page_old != null) { FormActive_Change(app_page.page_old, false); }
                if (app_page.page_new != null) { FormActive_Change(app_page.page_new, true); }

            }
            catch (Exception ex) { Program.log_md.LogWrite("Module_main.FormShow." + FormChild.Name + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
            FormChild.ResumeLayout();
        }
        public void Occured_Alarm_FormShow(bool visible)
        {
            try
            {
                if (visible == true)
                {
                    Program.occured_alarm_form.Show();
                    Program.occured_alarm_form.Visible = true;
                    Program.occured_alarm_form.GridView_Colappsed();
                    Program.occured_alarm_form.BringToFront();
                }
                else
                {
                    Program.occured_alarm_form.Show();
                    Program.occured_alarm_form.Hide();
                }
            }
            catch (Exception ex) {
                Program.log_md.LogWrite("Module_main.Occured_Alarm_FormShow." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                Program.occured_alarm_form.Dispose();
                Program.occured_alarm_form = new frm_occured_alarm();
            }
            finally { }

        }
        public void FormActive_Change(DevExpress.XtraEditors.XtraForm form, Boolean active)
        {
            if (form == Program.alarm_form) { Program.alarm_form.actived = active; }
            else if (form == Program.mixing_step_form) { Program.mixing_step_form.actived = active; }
            else if (form == Program.parameter_form) { Program.parameter_form.actived = active; }
            else if (form == Program.io_monitor_form) { Program.io_monitor_form.actived = active; }
            //else if (form == Program.schematic_form_dhf) { Program.schematic_form_dhf.actived = active; }
            else if (form == Program.schematic_form) { Program.schematic_form.actived = active; }
            else if (form == Program.alarmlog_form) { Program.alarmlog_form.actived = active; }
            else if (form == Program.eventlog_form) { Program.eventlog_form.actived = active; }
            else if (form == Program.totalusagelog_form) { Program.totalusagelog_form.actived = active; }
            else if (form == Program.trendlog_form) { Program.trendlog_form.actived = active; }
            else if (form == Program.loading_appstart_form) { Program.loading_appstart_form.actived = active; }
            else if (form == Program.main_form) { Program.main_form.actived = active; }
            else if (form == Program.message_form) { Program.message_form.actived = active; }
            else if (form == Program.communications_form) { Program.communications_form.actived = active; }
            //else if (form == Program.process_indicator_form) { Program.process_indicator_form.actived = active; }
        }
        public bool Message_By_Application(string message, frm_messagebox.enum_apptype apptype)
        {
            Boolean result = false;
            try
            {
                Program.message_form.StartPosition = FormStartPosition.CenterScreen;
                Program.message_form.setting_initial(message, apptype);
                Program.message_form.ShowDialog();
                result = Program.message_form.ok_or_cancel;
            }
            catch (Exception ex)
            {
                result = false;
                Program.log_md.LogWrite("Module_main.Message_By_Application." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { }
            return result;
        }
        public void Set_WorkSetMemory()
        {
            object bret;
            int id;
            int ph;
            int wkmin = 0;
            int wkmax = 0;
            int dTemp;
            try
            {
                id = GetCurrentProcessId();
                ph = OpenProcess(256 + 1024, false, id);
                bret = GetProcessWorkingSetSize(ph, wkmin, wkmax);
                dTemp = 4;
                wkmin = dTemp * 1024 * 1024;
                wkmax = 30 * 1024 * 1024;
                bret = SetProcessWorkingSetSize(ph, wkmin, wkmax);
                bret = GetProcessWorkingSetSize(ph, wkmin, wkmax);
                bret = CloseHandle(ph);
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite("Module_main.Set_WorkSetMemory." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        public Version GetFileVersionInfo(string filename)
        {
            return Version.Parse(FileVersionInfo.GetVersionInfo(filename).FileVersion);
        }
        public double GetMemoryUsage(string ProcessName)
        {
            //GC.GetTotalMemory(true);
            double _Return = 0;
            foreach (Process _Process in Process.GetProcessesByName(ProcessName))
            {
                if (_Process.ToString().Remove(0, 27).ToLower() == "(" + ProcessName.ToLower() + ")")
                {
                    //_Return = (_Process.WorkingSet64 / 1024).ToString("0,000") + "K";
                    _Return = (_Process.WorkingSet64 / 1024);
                }
            }
            return _Return;
        }
        public long GetTotalFreeSpace(string driveName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == driveName)
                {
                    return (drive.TotalFreeSpace / 1024);
                    //KB
                    //return (drive.TotalFreeSpace / 1024);
                }
            }
            return -1;
        }
        public bool IsNumeric(string input)
        {
            Double number = 0;
            return Double.TryParse(input, out number);
        }
        public byte[] ObjectToByteArray_bySerialize(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public UInt32 Get_Structure_Size<T>(T structure)
        {
            UInt32 result;
            result = (UInt32)Marshal.SizeOf(structure);
            if (result == 1)
            {
                result = 0;
            }
            return result;
        }
        public byte[] ObjectToByteArray_byMashal(object obj)
        {
            //구조체 사이즈 
            int iSize = Marshal.SizeOf(obj);
            //사이즈 만큼 메모리 할당 받기
            byte[] arr = new byte[iSize];
            IntPtr ptr = Marshal.AllocHGlobal(iSize);
            //구조체 주소값 가져오기
            Marshal.StructureToPtr(obj, ptr, false);
            //메모리 복사 
            Marshal.Copy(ptr, arr, 0, iSize);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
        public delegate void Del_process_indicator_popup(string message, Boolean active, frm_process_indicator.enum_call_by call_by);
        public void process_indicator_popup(string message, Boolean active, frm_process_indicator.enum_call_by call_by)
        {
            try
            {
                if (active == true)
                {
                    if (Program.process_indicator_form.Modal == false)
                    {
                        Program.process_indicator_form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                        Program.process_indicator_form.Processing = true;
                        Program.process_indicator_form.call_by = call_by;
                        Program.process_indicator_form.SetCaption(message);
                        Program.process_indicator_form.BringToFront();
                        Program.process_indicator_form.Opacity = 100;
                        Program.process_indicator_form.ShowDialog();
                    }
                }
                else
                {
                    Program.process_indicator_form.Processing = false;
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite("Module_main.process_indicator_popup." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }

        }
        public void Save_ByteArray_Send_Log(string memo, byte[] byte_array)
        {
            try
            {
                string log = "";
                if (Program.cg_trace.socket_log == true)
                {
                    if (byte_array != null)
                    {
                        log = "{" + BitConverter.ToString(byte_array).Replace("-", " ") + "}";
                        if (Program.main_form.log_ctc_q_snd_data.Count > 10) { Program.main_form.log_ctc_q_snd_data.Dequeue(); }
                        Program.main_form.log_ctc_q_snd_data.Enqueue(memo + ", " + log);
                        //Program.log_md.LogWrite(memo + ", " + log, Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);
                        Program.log_md.LogWrite(memo, Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);
                    }
                    else
                    {
                        if (Program.main_form.log_ctc_q_snd_data.Count > 10) { Program.main_form.log_ctc_q_snd_data.Dequeue(); }
                        Program.main_form.log_ctc_q_snd_data.Enqueue(memo + ", " + "null");
                        //Program.log_md.LogWrite(memo + ", " + log, Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);
                        Program.log_md.LogWrite(memo, Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);

                    }
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite("Module_main.Save_ByteArray_Send_Log." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        public void Save_ByteArray_Rcv_Log(string memo, byte[] byte_array)
        {
            try
            {
                string log = "";
                if (Program.cg_trace.socket_log == true)
                {
                    if (byte_array != null)
                    {
                        log = "{" + BitConverter.ToString(byte_array).Replace("-", " ") + "}";
                        if (Program.main_form.log_ctc_q_rcv_data.Count > 10) { Program.main_form.log_ctc_q_rcv_data.Dequeue(); }
                        Program.main_form.log_ctc_q_rcv_data.Enqueue(memo + ", " + log);
                        //Program.log_md.LogWrite(memo + ", " + log, Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);
                        Program.log_md.LogWrite(memo, Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);
                    }
                    else
                    {
                        if (Program.main_form.log_ctc_q_rcv_data.Count > 10) { Program.main_form.log_ctc_q_rcv_data.Dequeue(); }
                        Program.main_form.log_ctc_q_rcv_data.Enqueue(memo + ", " + "null");
                        //Program.log_md.LogWrite(memo + ", " + log, Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);
                        Program.log_md.LogWrite(memo, Module_Log.enumLog.CTC_SOCKET_DATA, "", Module_Log.enumLevel.ALWAYS);

                    }
                }
            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite("Module_main.Save_ByteArray_Rcv_Log." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
        }
        public bool Login_Check(string user_id, string password)
        {
            bool result = false;
            try
            {

                if (user_id.ToUpper() == "")
                {
                    result = false;
                }
                //else if (user_id.ToUpper() == "DEVELOPER")
                //{
                //    if (password == "0")
                //    {
                //        Program.main_md.user_info.type = Module_User.User_Type.Develop;Program.main_md.user_info.id = user_id;Program.main_md.user_info.password = password;
                //        login(Program.main_md.user_info);
                //        result = true;
                //    }
                //    else
                //    {
                //        result = false;
                //    }

                //}
                else if (user_id.ToUpper() == "ADMIN")
                {
                    if (password == Program.cg_app_info.user_info.admin_password)
                    {
                        Program.main_md.user_info.type = Module_User.User_Type.Admin; Program.main_md.user_info.id = user_id; Program.main_md.user_info.password = password;
                        login(Program.main_md.user_info);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                }
                //else if (user_id.ToUpper() == "ENGINEER")
                //{
                //    if (password == "0")
                //    {
                //        Program.main_md.user_info.type = Module_User.User_Type.Engineer; Program.main_md.user_info.id = user_id; Program.main_md.user_info.password = password;
                //        login(Program.main_md.user_info);
                //        result = true;
                //    }
                //    else
                //    {
                //        result = false;
                //    }
                //}
                else if (user_id.ToUpper() == "USER")
                {
                    if (password == Program.cg_app_info.user_info.user_password)
                    {
                        Program.main_md.user_info.type = Module_User.User_Type.User; Program.main_md.user_info.id = user_id; Program.main_md.user_info.password = password;
                        login(Program.main_md.user_info);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                }

            }
            catch (Exception ex)
            {
                Program.log_md.LogWrite("Module_User.Login_Check." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                result = false;
            }
            return result;
        }
        public void login(Module_User.User_Info cur_user_info)
        {
            //xml_data = Module_XML.XML_Inidata_Read(Program.cg_main.path.xml + "/Setting.xml", "main/login_info/", "login_keep_time");



            if (cur_user_info.type == Module_User.User_Type.None)
            {
                //Logout과 동일함

                //access_ui
                cur_user_info.access_ui.schematic = true; cur_user_info.access_ui.io_monitor = true;
                cur_user_info.access_ui.alarm = true; cur_user_info.access_ui.parameter = true; cur_user_info.access_ui.mixing_step = true;
                cur_user_info.access_ui.trend_log = true; cur_user_info.access_ui.event_log = true; cur_user_info.access_ui.alarm_log = true; cur_user_info.access_ui.total_usage = true;

                //access_alarm
                cur_user_info.access_alarm.id = true; cur_user_info.access_alarm.name = true; cur_user_info.access_alarm.wdt = true;
                cur_user_info.access_alarm.level = true; cur_user_info.access_alarm.use = true; cur_user_info.access_alarm.comment = true;
                cur_user_info.access_alarm.host_send = true;

                //access_parameter
                cur_user_info.access_parameter.id = true; cur_user_info.access_parameter.name = true;
                cur_user_info.access_parameter.min = true; cur_user_info.access_parameter.set = true; cur_user_info.access_parameter.max = true;
                cur_user_info.access_parameter.comment = true; cur_user_info.access_parameter.ec_interlock = true;

                Program.alarm_form.btn_hostsend.Enabled = false;
                Program.parameter_form.btn_hostsend.Enabled = false;

                Program.popup_login.cmb_user_id.Enabled = true;
                Program.popup_login.Enabled = true;
                Program.popup_login.txt_user_password.Enabled = true;
                Program.popup_login.txt_user_password.Text = "";
                Program.popup_login.Text = "";

                Program.io_monitor_form.view_di.OptionsBehavior.Editable = false;
                Program.io_monitor_form.view_do.OptionsBehavior.Editable = false;
                Program.io_monitor_form.view_ai.OptionsBehavior.Editable = false;
                Program.io_monitor_form.view_ao.OptionsBehavior.Editable = false;

                Program.occured_alarm_form.btn_clear_all.Enabled = false;
                Program.main_form.btn_Configuration.Enabled = false;
                Program.main_form.btn_exit.Enabled = false;
                Program.eventlog_form.Insert_Event(cur_user_info.id + " Logout", (int)frm_eventlog.enum_event_type.Logout, (int)frm_eventlog.enum_occurred_type.USER, true);
                Program.main_form.Insert_System_Message("User Session Expired(Log Out)");
                Program.main_md.FormShow(Program.schematic_form, Program.main_form.pnl_body);

            }
            else if (cur_user_info.type == Module_User.User_Type.Admin)
            {
                //access_ui
                cur_user_info.access_ui.schematic = true; cur_user_info.access_ui.io_monitor = true;
                cur_user_info.access_ui.alarm = true; cur_user_info.access_ui.parameter = true; cur_user_info.access_ui.mixing_step = true;
                cur_user_info.access_ui.trend_log = true; cur_user_info.access_ui.event_log = true; cur_user_info.access_ui.alarm_log = true; cur_user_info.access_ui.total_usage = true;

                //access_alarm
                cur_user_info.access_alarm.id = false; cur_user_info.access_alarm.name = false; cur_user_info.access_alarm.wdt = true;
                cur_user_info.access_alarm.level = true; cur_user_info.access_alarm.use = true; cur_user_info.access_alarm.comment = false;
                cur_user_info.access_alarm.host_send = true;

                //access_parameter
                cur_user_info.access_parameter.id = false; cur_user_info.access_parameter.name = false;
                cur_user_info.access_parameter.min = true; cur_user_info.access_parameter.set = true; cur_user_info.access_parameter.max = true;
                cur_user_info.access_parameter.comment = false; cur_user_info.access_parameter.ec_interlock = true;

                Program.alarm_form.btn_hostsend.Enabled = true;
                Program.parameter_form.btn_hostsend.Enabled = true;
                Program.popup_login.Enabled = false;
                Program.popup_login.txt_user_password.Enabled = false;
                Program.main_form.btn_user_login.Text = "Logout" + System.Environment.NewLine + "(" + "Admin"+ ")";
                Program.popup_login.Text = "";

                Program.io_monitor_form.view_di.OptionsBehavior.Editable = true;
                Program.io_monitor_form.view_do.OptionsBehavior.Editable = true;
                Program.io_monitor_form.view_ai.OptionsBehavior.Editable = true;
                Program.io_monitor_form.view_ao.OptionsBehavior.Editable = true;

                Program.occured_alarm_form.btn_clear_all.Enabled = true;
                Program.main_form.btn_Configuration.Enabled = true;
                Program.main_form.btn_exit.Enabled = true;

                Program.eventlog_form.Insert_Event(cur_user_info.id + " Login", (int)frm_eventlog.enum_event_type.Login, (int)frm_eventlog.enum_occurred_type.USER, true);
                Program.main_form.Insert_System_Message("User Session Start(Log in)");
                //Program.main_md.FormShow(Program.schematic_form, Program.main_form.pnl_body);

            }
          
            else if (cur_user_info.type == Module_User.User_Type.User)
            {
                //access_ui
                cur_user_info.access_ui.schematic = true; cur_user_info.access_ui.io_monitor = false;
                cur_user_info.access_ui.alarm = false; cur_user_info.access_ui.parameter = false; cur_user_info.access_ui.mixing_step = false;
                cur_user_info.access_ui.trend_log = true; cur_user_info.access_ui.event_log = true; cur_user_info.access_ui.alarm_log = true; cur_user_info.access_ui.total_usage = true;

                //access_alarm
                cur_user_info.access_alarm.id = false; cur_user_info.access_alarm.name = false; cur_user_info.access_alarm.wdt = true;
                cur_user_info.access_alarm.level = true; cur_user_info.access_alarm.use = true; cur_user_info.access_alarm.comment = false;
                cur_user_info.access_alarm.host_send = false;

                //access_parameter
                cur_user_info.access_parameter.id = false; cur_user_info.access_parameter.name = false;
                cur_user_info.access_parameter.min = false; cur_user_info.access_parameter.set = true; cur_user_info.access_parameter.max = false;
                cur_user_info.access_parameter.comment = false; cur_user_info.access_parameter.ec_interlock = false;

                Program.alarm_form.btn_hostsend.Enabled = true;
                Program.parameter_form.btn_hostsend.Enabled = true;
                Program.popup_login.Enabled = false;
                Program.popup_login.txt_user_password.Enabled = false;
                Program.main_form.btn_user_login.Text = "Logout" + System.Environment.NewLine + "(" + "User" + ")";
                Program.popup_login.Text = "";


                Program.occured_alarm_form.btn_clear_all.Enabled = true;
                Program.main_form.btn_Configuration.Enabled = true;
                Program.main_form.btn_exit.Enabled = true;
                Program.eventlog_form.Insert_Event(cur_user_info.id + " Login", (int)frm_eventlog.enum_event_type.Login, (int)frm_eventlog.enum_occurred_type.USER, true);
                Program.main_form.Insert_System_Message("User Session Start(Log in)");
            }

        }
        public void logout(ref Module_User.User_Info cur_user_info)
        {
            cur_user_info.type = Module_User.User_Type.None; cur_user_info.id = ""; cur_user_info.password = "";
            //초기화 목적의 로그인 실제 로그인 안함
            login(cur_user_info);
            Program.main_form.btn_user_login.Text = "Login";
            Program.popup_login.Text = "";
        }
        public void Check_User_Login_Time_Expire()
        {
            if (pt_idle_old != Cursor.Position) { dt_idle_keep = DateTime.Now; }
            //Program.cg_app_info.login_expire_time 분 단위
            else if ((DateTime.Now - dt_idle_keep).TotalMinutes >= Program.cg_app_info.user_info.login_expire_time)
            {
                //Console.WriteLine(DateTime.Now.ToString("ss.fff") + "로그아웃");
                //로그아웃
                Program.main_md.logout(ref Program.main_md.user_info);
                Program.main_md.FormShow(Program.schematic_form, Program.main_form.pnl_body);
            }
            pt_idle_old = Cursor.Position;
        }

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string dirPath = Program.cg_main.path.log;
            string exeName = AppDomain.CurrentDomain.FriendlyName;

            MINIDUMP_EXCEPTION_INFORMATION info = new MINIDUMP_EXCEPTION_INFORMATION();
            info.CientPointers = 1;
            info.ExceptionPointers = Marshal.GetExceptionPointers();
            info.ThreadId = GetCurrentThreadId();

            {

                if (dirPath == null) { dirPath = @"D:\CDS\Log\"; }
                if (Directory.Exists(dirPath + "\\Log_Dump") == false)
                {
                    Directory.CreateDirectory(dirPath + "\\Log_Dump");
                }
                if (Directory.Exists(dirPath + "\\Log_Dump\\" + DateTime.Now.ToString("yyyy-MM-dd")) == false)
                {
                    Directory.CreateDirectory(dirPath + "\\Log_Dump\\" + DateTime.Now.ToString("yyyy-MM-dd"));
                }
                string dumpFileFullName = string.Format("{0}\\{1}_{2}.dmp", dirPath + "\\Log_Dump\\" + DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HHmmss_fff"), exeName);
                FileStream file = new FileStream(dumpFileFullName, FileMode.Create);
                MiniDumpWriteDump(GetCurrentProcess(), GetCurrentProcessId(), file.SafeFileHandle.DangerousGetHandle(), MiniDumpNormal, ref info, IntPtr.Zero, IntPtr.Zero);
                file.Close();
            }
        }


    }
}
