using System;
using System.Windows.Forms;

namespace cds
{
    public partial class UC_CTC : UserControl
    {
        public int idx_serial = 0;
        public UC_CTC()
        {
            InitializeComponent();
        }

        private void btn_alarm_occurred_102_Click(object sender, EventArgs e)
        {
            if (idx_serial < 0) { return; }

            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {

                if (btn_event.Name == "btn_device_status_100")
                {
                    Program.CTC.Message_Device_Status_100();
                }
                else if (btn_event.Name == "btn_alarm_occurred_102")
                {
                    Program.CTC.Message_Alarm_Occurred_102(Convert.ToInt32(txt_alarm_code.Text), txt_remark.Text);
                }
                else if (btn_event.Name == "btn_fdc_data_send_106")
                {
                    Program.CTC.Message_FDC_Data_Send_106();
                }
                else if (btn_event.Name == "btn_database_changed_notice_107")
                {
                    Program.CTC.Message_Database_Changed_Notice_109(Program.parameter_list.Return_Object_by_ID(Convert.ToInt32(txt_parameter_id.Text)));
                }
                else if (btn_event.Name == "btn_cds_enable_event_450")
                {
                    Program.CTC.Message_CDS_Enable_Event_450();
                }
                else if (btn_event.Name == "btn_chemical_change_request_400")
                {
                    Program.CTC.Message_Chemical_Change_Request_400();
                }
                else if (btn_event.Name == "btn_cds_disable_event_451")
                {
                    Program.CTC.Message_CDS_Disable_Event_451(true);
                }
                else if (btn_event.Name == "btn_cds_cc_start_event_452")
                {
                    Program.CTC.Message_Chemical_Change_Start_Event_452(txt_chemical_change_start_event_supply_tank.Text);
                }
                else if (btn_event.Name == "btn_cds_cc_end_event_453")
                {
                    Program.CTC.Message_Chemical_Change_End_Event_453(txt_chemical_change_end_event_supply_tank.Text);
                }
            }

            Program.log_md.LogWrite(this.Name + " Tag Idx[" + idx_serial + "].btn_alarm_occurred_102_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.LEVEL_1);
        }

    }
}
