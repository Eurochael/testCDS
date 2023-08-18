using DevExpress.XtraWaitForm;
using System;
using System.Windows.Forms;

namespace cds
{
    public partial class frm_process_indicator : WaitForm
    {
        private Boolean _actived = false;
        public Boolean Processing = false;
        
        public enum_call_by call_by;
        public enum enum_call_by
        {
            none = 0
            , alarmlog = 10
            , trendlog = 20
            , evnetlog = 30
            , totalusagelog = 40
        }

        public frm_process_indicator()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
        }
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
        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            if(description != "")
            {
                base.SetDescription(description);
                this.progressPanel1.Description = description;
            }
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }

        private void frm_process_indicator_Load(object sender, EventArgs e)
        {
            actived = true;
            timer_process.Interval = 200; timer_process.Enabled = true;
        }
        private void frm_process_indicator_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private void frm_process_indicator_FormClosed(object sender, FormClosedEventArgs e)
        {
            actived = false;
        }
        private void timer_process_Tick(object sender, EventArgs e)
        {
            if (Processing == false)
            {
                timer_process.Enabled = false;
                //차트, 시트 조회 시 Cancel 요청이 왔을 때
                if (call_by == enum_call_by.alarmlog)
                {
                    Program.alarmlog_form.Load_Alarm_Log_call_cancel();
                }
                else if (call_by == enum_call_by.trendlog)
                {
                    Program.trendlog_form.Trend_Log_call_cancel();
                }

                this.Close();
            }
            else {
                SetDescription(Program.main_md.process_desciption);
            }
            //this.Visible = true;
            //this.BringToFront();
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Processing = false;
        }


    }
}