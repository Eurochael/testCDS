using System;
using System.Windows.Forms;

namespace cds
{
    public partial class UC_HeatExchanger : UserControl
    {
        public int idx_serial = 0;
        public UC_HeatExchanger()
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
                if (btn_event.Name == "btn_read_AllData")
                {
                    Program.Heat_Exchanger.Message_Command_To_Byte(Class_HeatExchanger.Read_AllData);
                }
                else if (btn_event.Name == "btn_sv_set")
                {
                    Program.Heat_Exchanger.Set_SV(Convert.ToInt32(txt_sv_value.Text));
                }
                else if (btn_event.Name == "btn_set_offset_chem_in")
                {
                    Program.Heat_Exchanger.Set_Offset_Chemical_in(Convert.ToInt32(txt_sv_value.Text));
                }
                else if (btn_event.Name == "btn_set_offset_chem_out")
                {
                    Program.Heat_Exchanger.Set_Offset_Chemical_Out(Convert.ToInt32(txt_sv_value.Text));
                }
                else if (btn_event.Name == "btn_heat_exchanger_on")
                {
                    Program.Heat_Exchanger.Heater_On_Off(true);
                }
                else if (btn_event.Name == "btn_heat_exchanger_off")
                {
                    Program.Heat_Exchanger.Heater_On_Off(false);
                }

            }

            Program.log_md.LogWrite(this.Name + " Tag Idx[" + idx_serial + "].btn_read_pv_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.LEVEL_1);
        }
    }
}
