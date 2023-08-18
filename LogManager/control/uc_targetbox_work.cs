using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace LogManager
{
    public partial class uc_targetbox_work : UserControl
    {
        public string str_name = "";
        public int int_count = 0;
        public int int_delcount = 0;
        public string str_date = "";

        public uc_targetbox_work()
        {
            InitializeComponent();
        }

        private void uc_targetbox_Load(object sender, EventArgs e)
        {
            dlbl_name.Text = str_name;
            dlbl_count.Text = int_count.ToString();
            dlbl_delcount.Text = int_delcount.ToString();
            dlbl_deletedate.Text = str_date;
        }

        public void ui_refresh()
		{
            dlbl_count.Text = int_count.ToString();
            dlbl_delcount.Text = int_delcount.ToString();
            dlbl_deletedate.Text = str_date;
        }

    }
}
