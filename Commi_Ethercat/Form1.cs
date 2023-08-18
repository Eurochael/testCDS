using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comi_Ethercat
{
    public partial class Form1 : Form
    {
        Comi_Ethercat.Class_Main class_main = new Comi_Ethercat.Class_Main();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            class_main.MasterDeviceInit();
        }
    }
}
