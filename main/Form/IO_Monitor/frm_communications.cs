using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cds
{
    public partial class frm_communications : DevExpress.XtraEditors.XtraForm
    {
        private Boolean _actived = false;
        public frm_communications()
        {
            InitializeComponent();
        }
        public class Listbox_Object
        {
            public Listbox_Object()
            {
            }
            public int idx { get; set; }
            public string Value { get; set; }
            public bool enable { get; set; }
            public string Tag { get; set; }
        }
        private bool _initialized = false;
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
                        if (_initialized == false) { _initialized = true; Module_List_Setting(); }
                        timer_queue_viewer.Interval = 30; timer_queue_viewer.Enabled = true;
                        lbox_module_serial_list.SelectedIndex = 0;
                    }
                    else
                    {
                        timer_queue_viewer.Enabled = false;
                        Serial_Auto_Send_Active();
                    }
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + ".actived." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
            }
        }
        private void timer_queue_viewer_Tick(object sender, EventArgs e)
        {
            Queue_Viewer();
        }
        private void btn_send_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            Listbox_Object listbox_object = null;
            string[] OutStringSplit = null;
            string send_msg = "";
            char cSpace = ' ';
            byte[] send_data = null;
            int idx = 0;
            if (btn_event == null) { return; }

            else
            {

                if (btn_event.Name == "btn_send")
                {
                    if (Program.cg_app_info.eq_mode == enum_eq_mode.auto || Program.cg_app_info.semi_auto_state == true)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate in Auto or Semi-Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                    else if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
                    {
                        Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
                    }
                    else
                    {
                        if (lbox_module_serial_list.SelectedIndex < 0) { return; }
                        if (memo_manual_send.Text == "") { return; }

                        listbox_object = (Listbox_Object)lbox_module_serial_list.SelectedItem;


                        try
                        {
                            send_msg = memo_manual_send.Text;
                            if (rbtn_manual_send_none.Checked == true)
                            {

                            }
                            else if (rbtn_manual_send_cr.Checked == true)
                            {
                                send_msg = send_msg + " " + Convert.ToChar(0x0D);
                            }
                            else if (rbtn_manual_send_lf.Checked == true)
                            {
                                send_msg = send_msg + " " + Convert.ToChar(0x0A);
                            }
                            else if (rbtn_manual_send_crlf.Checked == true)
                            {
                                send_msg = send_msg + " " + Convert.ToChar(0x0D) + " " + Convert.ToChar(0x0A);
                            }

                            OutStringSplit = send_msg.Split(cSpace);

                            Array.Resize(ref send_data, OutStringSplit.Length);
                            foreach (string splitNumber in OutStringSplit)
                            {
                                if (rbtn_view_hex.Checked == true)
                                {
                                    send_data[idx] = Convert.ToByte(splitNumber, 16);
                                }
                                else if (rbtn_view_byte.Checked == true)
                                {
                                    send_data[idx] = Convert.ToByte(splitNumber);
                                }
                                else if (rbtn_view_string.Checked == true)
                                {
                                    send_data[idx] = Encoding.UTF8.GetBytes(splitNumber)[0];
                                }
                                idx++;
                            }
                        }
                        catch (Exception ex)
                        {
                            send_data = null;
                        }
                        if (send_data == null) { Program.main_md.Message_By_Application("Manual Data Packet Error", frm_messagebox.enum_apptype.Only_OK); return; }

                        //abb
                        if (listbox_object.Tag.IndexOf("ABB") >= 0)
                        {
                            if (send_data != null) { Program.main_form.abb_q_snd_data.Enqueue(send_data); }
                        }
                        else //heat exchanger
                        if (listbox_object.Tag.IndexOf("HEAT_EXCHANGER") >= 0)
                        {
                            if (send_data != null) { Program.main_form.heat_exchanger_q_snd_data.Enqueue(send_data); }
                        }
                        //serial
                        else if (lbox_module_serial_list.SelectedIndex >= 0)
                        {
                            if (send_data != null) { Program.main_form.serial_q_snd_data[listbox_object.idx].Enqueue(send_data); }
                        }


                    }

                }
                else if (btn_event.Name == "btn_log_clear_send")
                {
                    lbox_cm_send_log.Items.Clear();
                }
                else if (btn_event.Name == "btn_log_clear_rcv")
                {
                    lbox_cm_rcv_log.Items.Clear();
                }
                Program.log_md.LogWrite(this.Name + "btn_send_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.ALWAYS);

            }
        }
        public void Serial_Auto_Send_Active()
        {
            for (int idx = 0; idx < frm_main.serial_use_cnt; idx++)
            {
                Program.main_form.serial_auto_snd[lbox_module_serial_list.SelectedIndex] = true;
            }
            Program.main_form.abb_auto_snd = true;
            Program.main_form.heat_exchanger_auto_snd = true;
            memo_detail_send_log.Text = "";
            memo_detail_rcv_log.Text = "";
            lbox_module_serial_list.SelectedIndex = -1;

        }
        public void Module_List_Setting()
        {
            var page = xtraTabPage1;
            int idx_page = 0;
            try
            {
                tab_control_command_list.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
                lbox_module_serial_list.Items.Clear();
                for (int idx = 0; idx < Program.IO.SERIAL.use_cnt; idx++)
                {
                    if (Program.IO.SERIAL.Tag[idx].use == true)
                    {
                        idx_page = idx + 1;
                        Listbox_Object listbox_object = new Listbox_Object();
                        listbox_object.Value = Program.IO.SERIAL.Tag[idx].description;
                        listbox_object.enable = true;
                        listbox_object.idx = idx;
                        listbox_object.Tag = "";
                        lbox_module_serial_list.Items.Add(listbox_object);

                        if (idx == 0) { page = xtraTabPage1; }
                        else if (idx == 1) { page = xtraTabPage2; }
                        else if (idx == 2) { page = xtraTabPage3; }
                        else if (idx == 3) { page = xtraTabPage4; }
                        else if (idx == 4) { page = xtraTabPage5; }
                        else if (idx == 5) { page = xtraTabPage6; }
                        else if (idx == 6) { page = xtraTabPage7; }
                        else if (idx == 7) { page = xtraTabPage8; }
                        else if (idx == 8) { page = xtraTabPage9; }
                        else if (idx == 9) { page = xtraTabPage10; }
                        else if (idx == 10) { page = xtraTabPage11; }
                        else if (idx == 11) { page = xtraTabPage12; }

                        if (Program.IO.SERIAL.Tag[idx].unit == Config_IO.enum_unit.THERMOSTAT_HE_3320C)
                        {
                            UC_ThermoStat_HE_3320C ThermoStat_HE_3320C = new UC_ThermoStat_HE_3320C();
                            ThermoStat_HE_3320C.idx_serial = idx;
                            ThermoStat_HE_3320C.name = Program.IO.SERIAL.Tag[idx].description;
                            ThermoStat_HE_3320C.HideTagetMode();
                            page.Controls.Add(ThermoStat_HE_3320C);
                            ThermoStat_HE_3320C.Dock = System.Windows.Forms.DockStyle.Fill;
                        }
                        else if (Program.IO.SERIAL.Tag[idx].unit == Config_IO.enum_unit.TEMP_CONTROLLER_M74 || Program.IO.SERIAL.Tag[idx].unit == Config_IO.enum_unit.TEMP_CONTROLLER_M74R)
                        {
                            UC_TempController_M74 TempController_M74 = new UC_TempController_M74();
                            TempController_M74.idx_serial = idx;
                            page.Controls.Add(TempController_M74);
                            TempController_M74.Dock = System.Windows.Forms.DockStyle.Fill;
                        }
                        else if (Program.IO.SERIAL.Tag[idx].unit == Config_IO.enum_unit.CONCENTRATION_CM210A_DC)
                        {

                        }
                        else if (Program.IO.SERIAL.Tag[idx].unit == Config_IO.enum_unit.CONCENTRATION_CS600F)
                        {
                            UC_CS600F CS600F = new UC_CS600F();
                            CS600F.idx_serial = idx;
                            page.Controls.Add(CS600F);
                            CS600F.Dock = System.Windows.Forms.DockStyle.Fill;
                        }
                        else if (Program.IO.SERIAL.Tag[idx].unit == Config_IO.enum_unit.PUMP_CONTROLLER_PB12)
                        {
                            UC_PumpController_PB12 PumpController_PB12 = new UC_PumpController_PB12();
                            PumpController_PB12.idx_serial = idx;
                            page.Controls.Add(PumpController_PB12);
                            PumpController_PB12.Dock = System.Windows.Forms.DockStyle.Fill;
                        }
                        else if (Program.IO.SERIAL.Tag[idx].unit == Config_IO.enum_unit.FLOWMETER_SONOTEC)
                        {
                            UC_FlowMeter_Sonotec FlowMeter_Sonotec = new UC_FlowMeter_Sonotec();
                            FlowMeter_Sonotec.idx_serial = idx;
                            page.Controls.Add(FlowMeter_Sonotec);
                            FlowMeter_Sonotec.Dock = System.Windows.Forms.DockStyle.Fill;
                        }
                        else if (Program.IO.SERIAL.Tag[idx].unit == Config_IO.enum_unit.FLOWMETER_USF500)
                        {
                            UC_FlowMeter_USF500 FlowMeter_usf500 = new UC_FlowMeter_USF500();
                            FlowMeter_usf500.idx_serial = idx;
                            page.Controls.Add(FlowMeter_usf500);
                            FlowMeter_usf500.Dock = System.Windows.Forms.DockStyle.Fill;
                        }
                        else if (Program.IO.SERIAL.Tag[idx].unit == Config_IO.enum_unit.SCR_DPU31A_025A)
                        {
                            UC_SCR_DPU31A_025A SCR_DPU31A_025A = new UC_SCR_DPU31A_025A();
                            SCR_DPU31A_025A.idx_serial = idx;
                            page.Controls.Add(SCR_DPU31A_025A);
                            SCR_DPU31A_025A.Dock = System.Windows.Forms.DockStyle.Fill;
                        }
                    }
                    else
                    {

                    }

                }

                if (Program.cg_app_info.eq_type == enum_eq_type.apm || Program.cg_app_info.eq_type == enum_eq_type.dsp)
                {
                    idx_page = idx_page + 1;
                    if (idx_page == 0) { page = xtraTabPage1; }
                    else if (idx_page == 1) { page = xtraTabPage2; }
                    else if (idx_page == 2) { page = xtraTabPage3; }
                    else if (idx_page == 3) { page = xtraTabPage4; }
                    else if (idx_page == 4) { page = xtraTabPage5; }
                    else if (idx_page == 5) { page = xtraTabPage6; }
                    else if (idx_page == 6) { page = xtraTabPage7; }
                    else if (idx_page == 7) { page = xtraTabPage8; }
                    else if (idx_page == 8) { page = xtraTabPage9; }
                    else if (idx_page == 9) { page = xtraTabPage10; }
                    else if (idx_page == 10) { page = xtraTabPage11; }
                    else if (idx_page == 11) { page = xtraTabPage12; }

                    Listbox_Object listbox_object_abb = new Listbox_Object();
                    listbox_object_abb.Value = "ABB-ASP310";
                    listbox_object_abb.enable = false;
                    listbox_object_abb.idx = idx_page;
                    listbox_object_abb.Tag = "ABB";
                    lbox_module_serial_list.Items.Add(listbox_object_abb);
                    UC_ABB ABB = new UC_ABB();
                    ABB.idx_serial = 0;
                    page.Controls.Add(ABB);
                    ABB.Dock = System.Windows.Forms.DockStyle.Fill;
                }
                else if (Program.cg_app_info.eq_type == enum_eq_type.dsp_mix)
                {
                    idx_page = idx_page + 1;
                    if (idx_page == 0) { page = xtraTabPage1; }
                    else if (idx_page == 1) { page = xtraTabPage2; }
                    else if (idx_page == 2) { page = xtraTabPage3; }
                    else if (idx_page == 3) { page = xtraTabPage4; }
                    else if (idx_page == 4) { page = xtraTabPage5; }
                    else if (idx_page == 5) { page = xtraTabPage6; }
                    else if (idx_page == 6) { page = xtraTabPage7; }
                    else if (idx_page == 7) { page = xtraTabPage8; }
                    else if (idx_page == 8) { page = xtraTabPage9; }
                    else if (idx_page == 9) { page = xtraTabPage10; }
                    else if (idx_page == 10) { page = xtraTabPage11; }
                    else if (idx_page == 11) { page = xtraTabPage12; }

                    Listbox_Object listbox_object_heat_exchanger = new Listbox_Object();
                    listbox_object_heat_exchanger.Value = "HEAT EXCHANGER";
                    listbox_object_heat_exchanger.enable = false;
                    listbox_object_heat_exchanger.idx = idx_page;
                    listbox_object_heat_exchanger.Tag = "HEAT_EXCHANGER";
                    lbox_module_serial_list.Items.Add(listbox_object_heat_exchanger);
                    UC_HeatExchanger heat_exchanger = new UC_HeatExchanger();
                    heat_exchanger.idx_serial = 0;
                    page.Controls.Add(heat_exchanger);
                    heat_exchanger.Dock = System.Windows.Forms.DockStyle.Fill;
                }
                //CTC
                idx_page = idx_page + 1;

                if (idx_page == 0) { page = xtraTabPage1; }
                else if (idx_page == 1) { page = xtraTabPage2; }
                else if (idx_page == 2) { page = xtraTabPage3; }
                else if (idx_page == 3) { page = xtraTabPage4; }
                else if (idx_page == 4) { page = xtraTabPage5; }
                else if (idx_page == 5) { page = xtraTabPage6; }
                else if (idx_page == 6) { page = xtraTabPage7; }
                else if (idx_page == 7) { page = xtraTabPage8; }
                else if (idx_page == 8) { page = xtraTabPage9; }
                else if (idx_page == 9) { page = xtraTabPage10; }
                else if (idx_page == 10) { page = xtraTabPage11; }
                else if (idx_page == 11) { page = xtraTabPage12; }

                Listbox_Object listbox_object_ctc = new Listbox_Object();
                listbox_object_ctc.Value = "CTC";
                listbox_object_ctc.enable = false;
                listbox_object_ctc.idx = idx_page;
                listbox_object_ctc.Tag = "CTC";
                lbox_module_serial_list.Items.Add(listbox_object_ctc);
                UC_CTC CTC = new UC_CTC();
                CTC.idx_serial = 0;
                page.Controls.Add(CTC);
                CTC.Dock = System.Windows.Forms.DockStyle.Fill;

                lbox_module_serial_list.SelectedIndex = 0;

            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".Module_List_Setting." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        public void Queue_Viewer()
        {
            string data = "";
            Listbox_Object listbox_object = null;
            try
            {
                this.lbox_module_serial_list.SuspendLayout();

                listbox_object = (Listbox_Object)lbox_module_serial_list.SelectedItem;

                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto || Program.cg_app_info.semi_auto_state == true)
                {
                    gp_command_list.Enabled = false;
                }
                else
                {
                    gp_command_list.Enabled = true;
                }

                if (lbox_module_serial_list.SelectedIndex > -1)
                {

                    //ABB Log View

                    if (listbox_object.Tag.ToUpper().IndexOf("ABB") >= 0)
                    {
                        if (Program.main_form.log_abb_q_snd_data.Count > 0)
                        {
                            data = Program.main_form.log_abb_q_snd_data.Dequeue();
                            if (lbox_cm_send_log.Items.Count > 10) { lbox_cm_send_log.Items.RemoveAt(lbox_cm_send_log.Items.Count - 1); }
                            lbox_cm_send_log.Items.Insert(0, data);
                        }

                        if (Program.main_form.log_abb_q_rcv_data.Count > 0)
                        {
                            data = Program.main_form.log_abb_q_rcv_data.Dequeue();
                            if (lbox_cm_rcv_log.Items.Count > 10) { lbox_cm_rcv_log.Items.RemoveAt(lbox_cm_rcv_log.Items.Count - 1); }
                            lbox_cm_rcv_log.Items.Insert(0, data);
                        }

                    }
                    else if (listbox_object.Tag.ToUpper().IndexOf("HEAT") >= 0)
                    {
                        if (Program.main_form.log_heat_exchanger_q_snd_data.Count > 0)
                        {
                            data = Program.main_form.log_heat_exchanger_q_snd_data.Dequeue();
                            if (lbox_cm_send_log.Items.Count > 10) { lbox_cm_send_log.Items.RemoveAt(lbox_cm_send_log.Items.Count - 1); }
                            lbox_cm_send_log.Items.Insert(0, data);
                        }

                        if (Program.main_form.log_heat_exchanger_q_rcv_data.Count > 0)
                        {
                            data = Program.main_form.log_heat_exchanger_q_rcv_data.Dequeue();
                            if (lbox_cm_rcv_log.Items.Count > 10) { lbox_cm_rcv_log.Items.RemoveAt(lbox_cm_rcv_log.Items.Count - 1); }
                            lbox_cm_rcv_log.Items.Insert(0, data);
                        }

                    }
                    else if (listbox_object.Tag.ToUpper().IndexOf("CTC") >= 0)
                    {
                        if (Program.main_form.log_ctc_q_snd_data.Count > 0)
                        {
                            data = Program.main_form.log_ctc_q_snd_data.Dequeue();
                            if (lbox_cm_send_log.Items.Count > 10) { lbox_cm_send_log.Items.RemoveAt(lbox_cm_send_log.Items.Count - 1); }
                            lbox_cm_send_log.Items.Insert(0, data);
                        }

                        if (Program.main_form.log_ctc_q_rcv_data.Count > 0)
                        {
                            data = Program.main_form.log_ctc_q_rcv_data.Dequeue();
                            if (lbox_cm_rcv_log.Items.Count > 10) { lbox_cm_rcv_log.Items.RemoveAt(lbox_cm_rcv_log.Items.Count - 1); }
                            lbox_cm_rcv_log.Items.Insert(0, data);
                        }

                    }
                    else
                    {
                        //Serial Send log view 
                        for (int idx = 0; idx < Program.IO.SERIAL.use_cnt; idx++)
                        {
                            if (Program.main_form.log_serial_q_snd_data[listbox_object.idx].Count > 0)
                            {
                                data = Program.main_form.log_serial_q_snd_data[listbox_object.idx].Dequeue();
                                if (lbox_cm_send_log.Items.Count > 10) { lbox_cm_send_log.Items.RemoveAt(lbox_cm_send_log.Items.Count - 1); }
                                lbox_cm_send_log.Items.Insert(0, data);
                            }
                        }

                        //Serial Rcv log view 
                        for (int idx = 0; idx < Program.IO.SERIAL.use_cnt; idx++)
                        {
                            if (Program.main_form.log_serial_q_rcv_data[listbox_object.idx].Count > 0)
                            {
                                data = Program.main_form.log_serial_q_rcv_data[listbox_object.idx].Dequeue();
                                if (lbox_cm_rcv_log.Items.Count > 10) { lbox_cm_rcv_log.Items.RemoveAt(lbox_cm_rcv_log.Items.Count - 1); }
                                lbox_cm_rcv_log.Items.Insert(0, data);
                            }
                        }
                    }

                }



            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".Queue_Viewer." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally
            {
                this.lbox_module_serial_list.ResumeLayout();
            }
        }
        private void lbox_module_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            View_Config(lbox_module_serial_list.SelectedIndex);
        }
        public void View_Config(int idx_selected_item)
        {
            Listbox_Object listbox_object = null;

            try
            {
                if (lbox_module_serial_list.SelectedIndex < 0) { return; }

                listbox_object = (Listbox_Object)lbox_module_serial_list.SelectedItem;


                lbox_cm_send_log.Items.Clear();
                lbox_cm_rcv_log.Items.Clear();
                //Serial Command Select
                tab_control_command_list.SelectedTabPageIndex = listbox_object.idx;

                if (Program.main_form.serial_auto_snd[listbox_object.idx] == true)
                {
                    btn_auto_active.IsOn = true;
                }
                else
                {
                    btn_auto_active.IsOn = false;
                }

                //Configuration View
                if (listbox_object.Tag.ToUpper().IndexOf("ABB") >= 0)
                {
                    lbl_config_view.Text = "IP : " + Program.cg_socket.abb_ip + "\r\n";
                    lbl_config_view.Text += "PORT : " + Program.cg_socket.abb_port + "\r\n";
                }
                else if (listbox_object.Tag.ToUpper().IndexOf("HEAT") >= 0)
                {
                    lbl_config_view.Text = "IP : " + Program.cg_socket.heat_exchanger_ip + "\r\n";
                    lbl_config_view.Text += "PORT : " + Program.cg_socket.heat_exchanger_port + "\r\n";
                }
                else if (listbox_object.Tag.ToUpper().IndexOf("CTC") >= 0)
                {
                    lbl_config_view.Text = "IP : " + Program.cg_socket.ctc_ip + "\r\n";
                    lbl_config_view.Text += "PORT : " + Program.cg_socket.ctc_port + "\r\n";
                    lbl_config_view.Text += "Network No : " + Program.cg_socket.ctc_network_no + "\r\n";
                }
                else
                {
                    lbl_config_view.Text = "Mode : " + Program.IO.SERIAL.Tag[listbox_object.idx].type + "\r\n";
                    lbl_config_view.Text += "Baudrate : " + Program.IO.SERIAL.Tag[listbox_object.idx].buadrate.ToString().Replace("_", "") + "\r\n";
                    lbl_config_view.Text += "Stop Bit : " + Program.IO.SERIAL.Tag[listbox_object.idx].stopbit + "\r\n";
                    lbl_config_view.Text += "Data Bit : " + Program.IO.SERIAL.Tag[listbox_object.idx].databit + "\r\n";
                    lbl_config_view.Text += "Parity   : " + Program.IO.SERIAL.Tag[listbox_object.idx].parity + "\r\n";
                    lbl_config_view.Text += "Comport  : " + Program.IO.SERIAL.Tag[listbox_object.idx].comport + "\r\n";
                }

            }
            catch (Exception ex)
            { Program.log_md.LogWrite(this.Name + ".View_Config." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
            finally { }
        }
        private void btn_auto_active_Toggled(object sender, EventArgs e)
        {
            if (lbox_module_serial_list.SelectedIndex < 0) { return; }
            Listbox_Object listbox_object = null;
            listbox_object = (Listbox_Object)lbox_module_serial_list.SelectedItem;
            if (listbox_object.Tag.ToUpper().IndexOf("ABB") >= 0)
            {
                Program.main_form.abb_auto_snd = btn_auto_active.IsOn;
            }
            else if (listbox_object.Tag.ToUpper().IndexOf("HEAT") >= 0)
            {
                Program.main_form.heat_exchanger_auto_snd = btn_auto_active.IsOn;
            }
            else if (listbox_object.Tag.ToUpper().IndexOf("CTC") >= 0)
            {
                Program.main_form.ctc_auto_snd = btn_auto_active.IsOn;
            }
            else
            {
                Program.main_form.serial_auto_snd[listbox_object.idx] = btn_auto_active.IsOn;
            }



        }
        private void lbox_cm_send_log_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (lbox_cm_send_log.SelectedIndex >= 0)
            {
                memo_detail_send_log.Text = lbox_cm_send_log.SelectedItem.ToString();
            }
        }
        private void lbox_cm_rcv_log_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (lbox_cm_rcv_log.SelectedIndex >= 0)
            {
                memo_detail_rcv_log.Text = lbox_cm_rcv_log.SelectedItem.ToString();
            }
        }
        private void rbtn_view_hex_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_view_hex.Checked == true)
            {
                Program.cg_app_info.log_serial_view_type = enum_serial_view_type.HEX;
            }
            else if (rbtn_view_byte.Checked == true)
            {
                Program.cg_app_info.log_serial_view_type = enum_serial_view_type.BYTE;
            }
            else if (rbtn_view_string.Checked == true)
            {
                Program.cg_app_info.log_serial_view_type = enum_serial_view_type.STRING;
            }
        }
        private void lbox_module_list_DrawItem(object sender, DevExpress.XtraEditors.ListBoxDrawItemEventArgs e)
        {
            Brush backcolor = Brushes.White;
            Brush selectedItemBackBrush = Brushes.LightSteelBlue;
            Listbox_Object listbox_object = null;
            string itemText = (sender as ListBoxControl).GetItemText(e.Index);
            if (lbox_module_serial_list.SelectedIndex > -1)
            {
                listbox_object = (Listbox_Object)(sender as ListBoxControl).GetItem(e.Index);
                itemText = listbox_object.Value;
                if (listbox_object.Tag.ToUpper().IndexOf("ABB") >= 0)
                {
                    if (Program.ABB.run_state == true)
                    {
                        if ((int)e.State == 1 || (int)e.State == 17) //Selected || Selected foucs
                        {
                            e.Cache.FillRectangle(selectedItemBackBrush, e.Bounds);
                        }
                        else
                        {
                            e.Cache.FillRectangle(backcolor, e.Bounds);
                        }
                        using (Font f = new Font(e.Appearance.Font.Name, e.Appearance.Font.Size, FontStyle.Bold))
                        {
                            e.Cache.DrawString(itemText, f, Brushes.Lime, e.Bounds, e.Appearance.GetStringFormat());
                        }
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        if ((int)e.State == 1 || (int)e.State == 17) //Selected || Selected foucs
                        {
                            e.Cache.FillRectangle(selectedItemBackBrush, e.Bounds);
                        }
                        else
                        {
                            e.Cache.FillRectangle(backcolor, e.Bounds);
                        }

                        using (Font f = new Font(e.Appearance.Font.Name, e.Appearance.Font.Size, FontStyle.Bold))
                        {
                            e.Cache.DrawString(itemText, f, Brushes.Black, e.Bounds, e.Appearance.GetStringFormat());
                        }
                        e.Handled = true;
                        return;
                    }
                }
                else if (listbox_object.Tag.ToUpper().IndexOf("HEAT") >= 0)
                {
                    if (Program.Heat_Exchanger.run_state == true)
                    {
                        if ((int)e.State == 1 || (int)e.State == 17) //Selected || Selected foucs
                        {
                            e.Cache.FillRectangle(selectedItemBackBrush, e.Bounds);
                        }
                        else
                        {
                            e.Cache.FillRectangle(backcolor, e.Bounds);
                        }
                        using (Font f = new Font(e.Appearance.Font.Name, e.Appearance.Font.Size, FontStyle.Bold))
                        {
                            e.Cache.DrawString(itemText, f, Brushes.Lime, e.Bounds, e.Appearance.GetStringFormat());
                        }
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        if ((int)e.State == 1 || (int)e.State == 17) //Selected || Selected foucs
                        {
                            e.Cache.FillRectangle(selectedItemBackBrush, e.Bounds);
                        }
                        else
                        {
                            e.Cache.FillRectangle(backcolor, e.Bounds);
                        }

                        using (Font f = new Font(e.Appearance.Font.Name, e.Appearance.Font.Size, FontStyle.Bold))
                        {
                            e.Cache.DrawString(itemText, f, Brushes.Black, e.Bounds, e.Appearance.GetStringFormat());
                        }
                        e.Handled = true;
                        return;
                    }
                }
                else if (listbox_object.Tag.ToUpper().IndexOf("CTC") >= 0)
                {
                    if (Program.CTC.run_state == true)
                    {
                        if ((int)e.State == 1 || (int)e.State == 17) //Selected || Selected foucs
                        {
                            e.Cache.FillRectangle(selectedItemBackBrush, e.Bounds);
                        }
                        else
                        {
                            e.Cache.FillRectangle(backcolor, e.Bounds);
                        }
                        using (Font f = new Font(e.Appearance.Font.Name, e.Appearance.Font.Size, FontStyle.Bold))
                        {
                            e.Cache.DrawString(itemText, f, Brushes.Lime, e.Bounds, e.Appearance.GetStringFormat());
                        }
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        if ((int)e.State == 1 || (int)e.State == 17) //Selected || Selected foucs
                        {
                            e.Cache.FillRectangle(selectedItemBackBrush, e.Bounds);
                        }
                        else
                        {
                            e.Cache.FillRectangle(backcolor, e.Bounds);
                        }

                        using (Font f = new Font(e.Appearance.Font.Name, e.Appearance.Font.Size, FontStyle.Bold))
                        {
                            e.Cache.DrawString(itemText, f, Brushes.Black, e.Bounds, e.Appearance.GetStringFormat());
                        }
                        e.Handled = true;
                        return;
                    }
                }
                else
                {
                    if (Program.main_form.serial_port_state[listbox_object.idx] == true)
                    {
                        if ((int)e.State == 1 || (int)e.State == 17) //Selected || Selected foucs
                        {
                            e.Cache.FillRectangle(selectedItemBackBrush, e.Bounds);
                        }
                        else
                        {
                            e.Cache.FillRectangle(backcolor, e.Bounds);
                        }
                        using (Font f = new Font(e.Appearance.Font.Name, e.Appearance.Font.Size, FontStyle.Bold))
                        {
                            e.Cache.DrawString(itemText, f, Brushes.Lime, e.Bounds, e.Appearance.GetStringFormat());
                        }
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        if ((int)e.State == 1 || (int)e.State == 17) //Selected || Selected foucs
                        {
                            e.Cache.FillRectangle(selectedItemBackBrush, e.Bounds);
                        }
                        else
                        {
                            e.Cache.FillRectangle(backcolor, e.Bounds);
                        }

                        using (Font f = new Font(e.Appearance.Font.Name, e.Appearance.Font.Size, FontStyle.Bold))
                        {
                            e.Cache.DrawString(itemText, f, Brushes.Black, e.Bounds, e.Appearance.GetStringFormat());
                        }
                        e.Handled = true;
                        return;
                    }
                }

            }
        }
        private void lbox_module_serial_list_CustomItemDisplayText(object sender, DevExpress.XtraEditors.CustomItemDisplayTextEventArgs e)
        {
            Listbox_Object listbox_object = null;
            if (lbox_module_serial_list.SelectedIndex > -1)
            {
                listbox_object = (Listbox_Object)e.Item;
                e.DisplayText = listbox_object.Value;
            }
        }
    }
}