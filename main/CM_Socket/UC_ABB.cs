using System;
using System.Windows.Forms;

namespace cds
{
    public partial class UC_ABB : UserControl
    {
        public int idx_serial = 0;
        public UC_ABB()
        {
            InitializeComponent();
        }

        private void btn_read_pv_Click(object sender, EventArgs e)
        {
            byte[] send_msg = null;
            if (idx_serial < 0) { return; }

            DevExpress.XtraEditors.SimpleButton btn_event = sender as DevExpress.XtraEditors.SimpleButton;
            if (btn_event == null) { return; }
            else
            {
                if (btn_event.Name == "btn_read_data")
                {
                    Program.ABB.Message_Command_To_Byte(Class_ABB.read_property_value1_to_4);
                }
                else if (btn_event.Name == "btn_read_do_status")
                {
                    Program.ABB.Message_Command_To_Byte(Class_ABB.read_DO_Status);
                }
                else if (btn_event.Name == "btn_chemistry_ch1_select")
                {
                    Program.ABB.Message_Command_To_Byte(Class_ABB.write_chemistry_01_selected);
                }
                else if (btn_event.Name == "btn_chemistry_ch2_select")
                {
                    Program.ABB.Message_Command_To_Byte(Class_ABB.write_chemistry_02_selected);
                }
                else if (btn_event.Name == "btn_online")
                {
                    Program.ABB.Message_Command_To_Byte(Class_ABB.write_online);
                }
                else if (btn_event.Name == "btn_offline")
                {
                    Program.ABB.Message_Command_To_Byte(Class_ABB.write_offline);
                }
                else if (btn_event.Name == "btn_reference_trigger_on")
                {
                    Program.ABB.Message_Command_To_Byte(Class_ABB.write_reference_trigger_on);
                }
                else if (btn_event.Name == "btn_reference_trigger_off")
                {
                    Program.ABB.Message_Command_To_Byte(Class_ABB.write_reference_trigger_off);
                }

            }

            Program.log_md.LogWrite(this.Name + " Tag Idx[" + idx_serial + "].btn_read_pv_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.LEVEL_1);
        }
    }
}
