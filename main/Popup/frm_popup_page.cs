using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace cds
{
    public partial class frm_popup_page : DevExpress.XtraEditors.XtraForm
    {
        protected override CreateParams CreateParams
        {
            //Form Alt+Tab 클릭 시에도 Window Handle 숨김
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        public enum enum_call_by
        {
            none
            , monitor
            , configuration
            , log
        }
        private Boolean _actived = false;
        public Boolean actived
        {
            get { return _actived; }
            set
            {
                try
                {
                    _actived = value;
                    if (_actived == true)
                    {
                        this.xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
                        this.Visible = false;
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + ".actived." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
            }
        }
        private void frm_popup_page_Load(object sender, EventArgs e)
        {
            actived = true;
        }
        private void frm_popup_page_FormClosed(object sender, FormClosedEventArgs e)
        {
            actived = false;
        }
        public frm_popup_page()
        {
            InitializeComponent();
        }
        public void DelayAction(int millisecond, Action action)
        {
            //일정 시간 후 함수 호출 대리자 사용함
            var timer = new DispatcherTimer();
            timer.Tick += delegate
            {
                action.Invoke();
                timer.Stop();
            };
            timer.Interval = TimeSpan.FromMilliseconds(millisecond);
            timer.Start();
        }
        public void Form_Acitve(bool active)
        {
            //일정 시간후 호출하기 위함 / 그래픽 부드럽게 처리 / DelayAction에서 호출함
            //호출하지 않을 경우 Page 3개 Form 생성 필요
            if (active == true){this.BringToFront(); this.Visible = true;}
            if (active == false) { this.Visible = false; }
        }
        public void setting_initial(enum_call_by call_by, Size size, Point point)
        {
            try
            {
                //page index
                //0 = monitor
                //1 = configuration
                //2 = log

                //2button height 145
                //3button height 215
                //4button height 285
                switch (call_by)
                {
                    case enum_call_by.none:
                        break;
                    case enum_call_by.monitor:
                        //Button Page 선택
                        xtraTabControl1.SelectedTabPageIndex = 0;
                        //Size  : 선택한 버튼의 Size (Width만 사용, Height 고정)
                        this.Size = new Size(150, 215);
                        //위치 X : 프로그램 윈도우 위치 + 선택한 버튼의 위치
                        //위치 Y : 프로그램 윈도우 위치 + 선택한 버튼의 위치 + 버튼보다 위로 위치 
                        this.Location = new Point(Program.main_form.Location.X + point.X, Program.main_form.Location.Y + point.Y - this.Size.Height - 10);
                        break;
                    case enum_call_by.configuration:
                        xtraTabControl1.SelectedTabPageIndex = 1;
                        this.Size = new Size(150, 215);
                        this.Location = new Point(Program.main_form.Location.X + point.X, Program.main_form.Location.Y + point.Y - this.Size.Height - 10);
                        break;
                    case enum_call_by.log:
                        xtraTabControl1.SelectedTabPageIndex = 2;
                        this.Size = new Size(150, 285);
                        this.Location = new Point(Program.main_form.Location.X + point.X, Program.main_form.Location.Y + point.Y - this.Size.Height - 10);
  
                        break;

                    //case enum_apptype.Ok_Or_Cancel:
                    //    btn_ok.Visible = true; btn_cancel.Visible = true;
                    //    btn_ok.Location = new Point(120, 120); btn_cancel.Location = new Point(330, 120);
                    //    break;
                    //case enum_apptype.Only_OK:
                    //    btn_ok.Visible = true; btn_cancel.Visible = false;
                    //    btn_ok.Location = new Point(230, 120); btn_cancel.Location = new Point(330, 120);
                    //    break;
                }

            }
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + ".Setting_Initial." + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }

        }
        private void frm_popup_page_Deactivate(object sender, EventArgs e)
        {
            //Console.WriteLine("DeActived");
        }

        private void btn_schematic_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                this.Visible = false;
                if (btn_event.Name == "btn_iomonitor")
                {
                    Program.main_md.FormShow(Program.io_monitor_form, Program.main_form.pnl_body);
                }
                else if (btn_event.Name == "btn_schematic")
                {
                    Program.main_md.FormShow(Program.schematic_form, Program.main_form.pnl_body);
                }
                else if(btn_event.Name == "btn_communication")
                {
                    Program.main_md.FormShow(Program.communications_form, Program.main_form.pnl_body);
                }
                //Configuration
                else if (btn_event.Name == "btn_mixingstep")
                {
                    Program.main_md.FormShow(Program.mixing_step_form, Program.main_form.pnl_body);
                }
                else if (btn_event.Name == "btn_alarm")
                {
                    Program.main_md.FormShow(Program.alarm_form, Program.main_form.pnl_body);
                }
                else if (btn_event.Name == "btn_parameter")
                {

                    Program.main_md.FormShow(Program.parameter_form, Program.main_form.pnl_body);
                }
                //Log
                else if (btn_event.Name == "btn_alarmlog")
                {
                    Program.main_md.FormShow(Program.alarmlog_form, Program.main_form.pnl_body);
                }
                else if (btn_event.Name == "btn_trendlog")
                {
                    Program.main_md.FormShow(Program.trendlog_form, Program.main_form.pnl_body);
                }
                else if (btn_event.Name == "btn_eventlog")
                {
                    Program.main_md.FormShow(Program.eventlog_form, Program.main_form.pnl_body);
                }
                else if (btn_event.Name == "btn_totalusagelog")
                {
                    Program.main_md.FormShow(Program.totalusagelog_form, Program.main_form.pnl_body);
                }
                Program.log_md.LogWrite(this.Name + ".btn_schematic_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
            }
                
        }

    }
}