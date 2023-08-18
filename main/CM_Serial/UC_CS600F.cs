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
    public partial class UC_CS600F : UserControl
    {
        public int idx_serial = 0;
        public UC_CS600F()
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
                if (Program.cg_app_info.eq_mode == enum_eq_mode.auto || Program.cg_app_info.semi_auto_state == true)
                {
                    Program.main_md.Message_By_Application("Cannot Operate in Auto or Semi-Auto Mode", frm_messagebox.enum_apptype.Only_OK); return;
                }
                else if (Program.main_md.user_info.type != Module_User.User_Type.Admin && Program.main_md.user_info.type != Module_User.User_Type.User)
                {
                    Program.main_md.Message_By_Application("Cannot Operate without Admin Authority", frm_messagebox.enum_apptype.Only_OK); return;
                }

                 if (btn_event.Name == "btn_read_data")
                {
                    Program.CS600F.Message_Command_To_Byte_CS600F_TO_Send(Class_Concentration_CS600F.command_Read_Data, idx_serial);
                }
                else if (btn_event.Name == "btn_cm_change_serial")
                {
                    Program.CS600F.CM_SERIAL_PARALLEL_SWITCH(true, idx_serial);
                }
                else if(btn_event.Name == "btn_cm_change_parallel")
                {
                    Program.CS600F.CM_SERIAL_PARALLEL_SWITCH(false, idx_serial);
                }
                else if (btn_event.Name == "btn_calibration_start")
                {
                    Program.CS600F.Message_Command_To_Byte_CS600F_TO_Send(Class_Concentration_CS600F.command_Write_CALIBRATION_START, idx_serial);
                }
                else if (btn_event.Name == "btn_calibration_end")
                {
                    Program.CS600F.Message_Command_To_Byte_CS600F_TO_Send(Class_Concentration_CS600F.command_Write_CALIBRATION_END, idx_serial);
                }
            }
            Program.log_md.LogWrite(this.Name + " Tag Idx[" + idx_serial + "].btn_read_pv_Click." + btn_event.Name, Module_Log.enumLog.Button, "", Module_Log.enumLevel.LEVEL_1);

        }

    }
}
