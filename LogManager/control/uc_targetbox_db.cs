using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogManager
{
    public partial class uc_targetbox_db : UserControl
    {
        public string str_name = "";
        public string str_db = "";
        public string str_table = "";
        public string str_field = "";
        public int int_date = 0;
        public uc_targetbox_db()
        {
            InitializeComponent();
        }

        private void uc_targetbox_Load(object sender, EventArgs e)
        {
            dlbl_name.Text = str_name;
            txt_db.Text = str_db;
            txt_table.Text = str_table;
            txt_field.Text = str_field;
            txt_date.Text = int_date.ToString();
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
        private void txt_db_TextChanged(object sender, EventArgs e)
		{
            str_db = txt_db.Text;
        }

		private void txt_table_TextChanged(object sender, EventArgs e)
		{
            str_table = txt_table.Text;
        }

		private void txt_field_TextChanged(object sender, EventArgs e)
		{
            str_field = txt_field.Text;
        }
        public void return_data()
		{
            txt_db.Text = str_db;
            txt_table.Text = str_table;
            txt_field.Text = str_field;
            txt_date.Text = int_date.ToString();
        }
	}
}
