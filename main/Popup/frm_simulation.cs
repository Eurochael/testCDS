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

namespace cds
{
    public partial class frm_simulation : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public bool returnValue = false;
        public bool ok_or_cancel = false;

        public string ok_text = "OK";
        public string cancel_text = "CANCEL";


        public enum enum_apptype
        {
            None = 0,
            Ok_Or_Cancel = 10,
            Only_OK = 20
        }
        public frm_simulation()
        {
            InitializeComponent();
            timer_Simulation_1.Enabled = true;

        }
        private void frm_simulation_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer_Simulation_1.Enabled = false;

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
                        xtraTabControl2.SelectedTabPageIndex = 0;
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
        private void frm_simulation_Load(object sender, EventArgs e)
        {
            Setting_initial();
        }
        public void Setting_initial()
        {
            try
            {
                uc_DIO_AIO_control1.Set_DIO_Setting();
                //this.StartPosition = FormStartPosition.Manual;
                //this.Location = new System.Drawing.Point(-2200, -500);
                //this.Location = new System.Drawing.Point(0, 0);
            }
            catch (Exception ex) { Program.log_md.LogWrite(this.Name + "." + ".Setting_Initial." + "." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }

        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                if (btn_event.Name == "btn_ok")
                {
                    ok_or_cancel = true;
                    this.Close();
                }
                else if (btn_event.Name == "btn_cancel")
                {
                    ok_or_cancel = false;
                    this.Close();
                }
            }
            Program.log_md.LogWrite(sender.ToString(), Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);
        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            lbox_cm_log.Items.Clear();
        }

        private void timer_Simulation_1_Tick(object sender, EventArgs e)
        {
            string cm_log = "";

        }

        public void CM_Log(string cm_log)
        {
            if(lbox_cm_log.Items.Count >= 100)
            {
                lbox_cm_log.Items.RemoveAt(lbox_cm_log.Items.Count-1);
            }
            lbox_cm_log.Items.Insert(0, cm_log);
        }
        private void btn_alarm_reset_103_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Alarm_Reset_103();
        }

        private void btn_database_change_notice_107_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Database_Changed_Notice_109(Program.parameter_list.Return_Object_by_ID(Convert.ToInt32(txt_parameter_id.Text)));
        }

        private void btn_cds_version_info_108_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_CDS_Version_Info_108();
        }

        private void btn_chemical_change_confirm_401_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Tank_A_Supply_End_Event_459();
        }

        private void btn_increase_wafer_count_403_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Increase_Wafer_Count_403();
        }

        private void btn_reclaim_enabled_404_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Reclaim_Enabled_404(Convert.ToBoolean(chk_reclaim_state.CheckState));
        }

        private void btn_manual_chmical_change_402_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Manual_Chemical_Change_402();
        }

        private void btn_alarm_occurred_102_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Alarm_Occurred_102(Convert.ToInt32(txt_alarm_code.Text), txt_remark.Text);
        }

        private void btn_chemical_change_request_400_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Chemical_Change_Request_400();
        }

        private void btn_cds_enable_event_450_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_CDS_Enable_Event_450();
        }

        private void btn_cds_disable_event_451_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_CDS_Disable_Event_451(false);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Chemical_Change_Start_Event_452(txt_chemical_change_start_event_supply_tank.Text);
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Chemical_Change_End_Event_453(txt_chemical_change_end_event_supply_tank.Text);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Device_Status_100();
        }
        private void btn_no_process_req_event_454_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_No_Process_Request_Event_454();
        }

        private void btn_no_process_req_cancel_event_454_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_No_Process_Request_Cancel_Event_455();
        }

        private void btn_auto_mode_event_456_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Auto_Mode_Event_456();
        }

        private void btn_manual_mode_event_457_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Manual_Mode_Event_457();
        }

        private void btn_tank_a_supply_start_458_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Tank_A_Supply_Start_Event_458();
        }

        private void btn_fdc_data_send_106_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_FDC_Data_Send_106();
        }

        private void btn_database_changed_notice_107_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Database_Changed_Notice_109(Program.parameter_list.Return_Object_by_ID(Convert.ToInt32(txt_parameter_id.Text)));
        }

        private void btn_stop_Supply_406_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Stop_Supply_406();
        }

        private void btn_device_status_100_Click(object sender, EventArgs e)
        {
            Program.CTC.Message_Device_Status_100();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            for(int idx= 0; idx < Convert.ToInt32(textEdit1.Text); idx++)
            {
                //Program.CTC.Message_Device_Status_100();
                Byte[] full_data;
                UInt16 send_flag;
                Module_Socket.Head send_head = new Module_Socket.Head();
                Module_Socket.Data_None send_Data = new Module_Socket.Data_None();
                send_flag = Convert.ToUInt16(idx);
                send_head = Program.CTC.Make_Packet_Head(Program.main_md.Get_Structure_Size(send_Data), send_flag, (UInt32)idx, 1, Program.cg_socket.ctc_network_no);
                full_data = Program.CTC.GetBytes_By_Packet(send_head, send_Data);
                Program.CTC.Message_Send(ref full_data, send_head, Module_Socket.TokenList.Device_Status_100);
                System.Threading.Thread.Sleep(Convert.ToInt32(textEdit2.Text));
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                string[] read_data;
                string query, err = "";
                read_data = System.IO.File.ReadAllLines(txt_para_data_path.Text);
                Config_Parameter para_object = new Config_Parameter();
                for (int idx = 1; idx < read_data.Length; idx++)
                {
                    para_object.comment = read_data[idx].Split(';')[1];
                    para_object.id = Convert.ToInt32(read_data[idx].Split(';')[0]);

                    query = "Update parameters";
                    query += " Set cds_parameter_comment = '" + para_object.comment.ToString() + "'";
                    query += " WHERE cds_parameter_id = '" + para_object.id + "'";
                    Program.database_md.MariaDB_MainQuery(Program.cg_main.db_local_cds.connection, query, ref err);

                }
                

            }
            catch (Exception ex)
            {

            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            try
            {

                string[] read_data;
                string query, err = "";
                read_data = System.IO.File.ReadAllLines(txt_alarm_data_path.Text);
                Config_Alarm alarm_object = new Config_Alarm();
                for (int idx = 1; idx < read_data.Length; idx++)
                {
                    alarm_object.comment = read_data[idx].Split(';')[1];
                    alarm_object.id = Convert.ToInt32(read_data[idx].Split(';')[0]);

                    query = "Update alarm_list";
                    query += " Set alarm_comment = '" + alarm_object.comment.ToString() + "'";
                    query += " WHERE alarm_id = '" + alarm_object.id + "'";
                    Program.database_md.MariaDB_MainQuery(Program.cg_main.db_local_cds.connection, query, ref err);

                }


            }
            catch (Exception ex)
            {

            }
        }

        private void chk_408_command_ok_ng_CheckedChanged(object sender, EventArgs e)
        {
            if(chk_408_command_ok_ng.Checked == true)
            {
               Program.schematic_form.Check_Availability_response_status = true;  
            }
            else
            {
                Program.schematic_form.Check_Availability_response_status = false;
            }
        }
    }
}