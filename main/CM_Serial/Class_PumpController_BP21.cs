using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cds
{
    public class Class_PumpController_BP21
    {
        public struct ST_PumpController_BP21
        {
            public int Alarm;
            public bool sol_control_error;
            public bool run_state;
        }

        public void Pump_ON_OFF(ref ST_PumpController_BP21 Pump,  bool state)
        {
            if (Program.cg_app_info.mode_simulation.use == true)
            {
                Pump.run_state = state;
            }
            else
            {
                Pump.run_state = state;
            }
        }
    }
}
