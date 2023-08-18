using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogManager
{
    class Module_Main
    {
        public string[] md_string_log_path;
        public int md_max_log_count = 20;
        public string md_startup_path = Application.StartupPath.ToString() + @"\";
    }
}
