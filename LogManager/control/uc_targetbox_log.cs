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
    public partial class uc_targetbox_log : UserControl
    {
        public string str_name = "";
        public string str_path = "";
        public int int_date = 0;
        public uc_targetbox_log()
        {
            InitializeComponent();
        }

        private void uc_targetbox_Load(object sender, EventArgs e)
        {
            dlbl_name.Text = str_name;
            txt_path.Text = str_path;
            txt_date.Text = int_date.ToString();
        }

        private void btn_loc_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = str_path;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                str_path = folderBrowserDialog.SelectedPath;
            }
            txt_path.Text = str_path;
        }
		private void txt_date_KeyPress(object sender, KeyPressEventArgs e)
		{          
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            { 
                e.Handled = true;
            }
        }
        private void txt_date_TextChanged(object sender, EventArgs e)
        {
            int_date = Convert.ToInt32(txt_date.Text);
        }

        public void return_data()
		{
            txt_path.Text = str_path;
            txt_date.Text = int_date.ToString();
        }

		private void txt_path_TextChanged(object sender, EventArgs e)
		{
            str_path = txt_path.Text;
        }
	}
}
