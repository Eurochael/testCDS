using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cds;
using YamlDotNet.Serialization;

namespace Config_Create
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //DI
        cds.Config_IO.Config_DI di = new cds.Config_IO.Config_DI();
        public Panel[] pnl_di;
        public CheckBox[] chk_di;
        public CheckBox[] no_nc_di;
        public TextBox[] txt_no_di;
        public TextBox[] txt_address_di;

        //DO
        cds.Config_IO.Config_DO doo = new cds.Config_IO.Config_DO();
        public Panel[] pnl_do;
        public CheckBox[] chk_do;
        public CheckBox[] no_nc_do;
        public TextBox[] txt_no_do;
        public TextBox[] txt_address_do;

        //AI
        cds.Config_IO.Config_AI AI = new cds.Config_IO.Config_AI();
        public Panel[] pnl_ai;
        public CheckBox[] chk_ai;
        public CheckBox[] no_nc_ai;
        public TextBox[] txt_no_ai;
        public TextBox[] txt_address_ai;
        public TextBox[] txt_min_ai;
        public TextBox[] txt_max_ao;
        private void btn_Initialize_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            Set_DI_Setting();
            Set_DO_Setting();
            Set_AI_Setting();
            this.ResumeLayout();
            timer1.Interval = 500;
            timer1.Enabled = true;
        }
        public void Set_DI_Setting()
        {
            fpnl_di.Controls.Clear();
            int idx = 0;
            //chk_di = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_di)).Length];
            pnl_di = new Panel[Enum.GetValues(typeof(Config_IO.enum_di)).Length];
            chk_di = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_di)).Length];
            no_nc_di = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_di)).Length];
            txt_address_di = new TextBox[Enum.GetValues(typeof(Config_IO.enum_di)).Length];
            txt_no_di = new TextBox[Enum.GetValues(typeof(Config_IO.enum_di)).Length];

            di.use_cnt = Enum.GetValues(typeof(Config_IO.enum_di)).Length;
            di.Tag = new Config_IO.Config_Digial_Tag[di.use_cnt];
            foreach (var temp in Enum.GetValues(typeof(Config_IO.enum_di)))
            {
                pnl_di[idx] = new Panel();
                pnl_di[idx].Size = new Size(1200, 30);
                pnl_di[idx].BorderStyle = BorderStyle.Fixed3D;
                pnl_di[idx].Font = new Font("arial", 18);
                chk_di[idx] = new CheckBox();
                chk_di[idx].Size = new Size(900, 30);
                chk_di[idx].Text = temp.ToString();
                no_nc_di[idx] = new CheckBox();
                no_nc_di[idx].Text = idx + " N.O";
                no_nc_di[idx].Size = new Size(120, 30);

                txt_address_di[idx] = new TextBox();
                txt_address_di[idx].Size = new Size(100,30);

                chk_di[idx].Tag = idx;

                chk_di[(idx)].Parent = pnl_di[idx]; chk_di[(idx)].Dock = DockStyle.Left;
                no_nc_di[(idx)].Parent = pnl_di[idx]; no_nc_di[(idx)].Dock = DockStyle.Left;
                txt_address_di[(idx)].Parent = pnl_di[idx]; txt_address_di[(idx)].Dock = DockStyle.Left;

                txt_address_di[idx].Text = idx.ToString();
                fpnl_di.Controls.Add(pnl_di[idx]);
                idx = idx + 1;
            }
            groupBox1.Text = "DI (" + idx + ")";

        }
        public void Set_DO_Setting()
        {
            fpnl_do.Controls.Clear();
            int idx = 0;
            //chk_do = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_do)).Length];
            pnl_do = new Panel[Enum.GetValues(typeof(Config_IO.enum_do)).Length];
            chk_do = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_do)).Length];
            no_nc_do = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_do)).Length];
            txt_address_do = new TextBox[Enum.GetValues(typeof(Config_IO.enum_do)).Length];
            txt_no_do = new TextBox[Enum.GetValues(typeof(Config_IO.enum_do)).Length];

            doo.use_cnt = Enum.GetValues(typeof(Config_IO.enum_do)).Length;
            doo.Tag = new Config_IO.Config_Digial_Tag[doo.use_cnt];
            foreach (var temp in Enum.GetValues(typeof(Config_IO.enum_do)))
            {
                pnl_do[idx] = new Panel();
                pnl_do[idx].Size = new Size(1200, 30);
                pnl_do[idx].Font = new Font("arial", 18);
                pnl_do[idx].BorderStyle = BorderStyle.Fixed3D;
                chk_do[idx] = new CheckBox();
                chk_do[idx].Size = new Size(900, 30);
                chk_do[idx].Text = temp.ToString();
                no_nc_do[idx] = new CheckBox();
                no_nc_do[idx].Text = idx + " N.O";
                no_nc_do[idx].Size = new Size(120, 30);
                no_nc_do[idx].Checked = true;

                txt_address_do[idx] = new TextBox();
                txt_address_do[idx].Size = new Size(100, 30);

                chk_do[idx].Tag = idx;
                //chk_do[idx].Checked = true;

                chk_do[(idx)].Parent = pnl_do[idx]; chk_do[(idx)].Dock = DockStyle.Left;
                no_nc_do[(idx)].Parent = pnl_do[idx]; no_nc_do[(idx)].Dock = DockStyle.Left;
                txt_address_do[(idx)].Parent = pnl_do[idx]; txt_address_do[(idx)].Dock = DockStyle.Left;

                txt_address_do[idx].Text = idx.ToString();
                fpnl_do.Controls.Add(pnl_do[idx]);
                idx = idx + 1;
            }
            groupBox2.Text = "DO (" + idx + ")";

        }
        public void Set_AI_Setting()
        {
            fpnl_ai.Controls.Clear();
            int idx = 0;
            //chk_ai = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_ai)).Length];
            pnl_ai = new Panel[Enum.GetValues(typeof(Config_IO.enum_ai)).Length];
            chk_ai = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_ai)).Length];
            no_nc_ai = new CheckBox[Enum.GetValues(typeof(Config_IO.enum_ai)).Length];
            txt_address_ai = new TextBox[Enum.GetValues(typeof(Config_IO.enum_ai)).Length];
            txt_no_ai = new TextBox[Enum.GetValues(typeof(Config_IO.enum_ai)).Length];
            txt_min_ai = new TextBox[Enum.GetValues(typeof(Config_IO.enum_ai)).Length];
            txt_max_ao = new TextBox[Enum.GetValues(typeof(Config_IO.enum_ai)).Length];

            AI.use_cnt = Enum.GetValues(typeof(Config_IO.enum_ai)).Length;
            AI.Tag = new Config_IO.Config_Analog_Tag[AI.use_cnt];
            foreach (var temp in Enum.GetValues(typeof(Config_IO.enum_ai)))
            {
                pnl_ai[idx] = new Panel();
                pnl_ai[idx].Size = new Size(1200, 30);
                pnl_ai[idx].Font = new Font("arial", 18);
                pnl_ai[idx].BorderStyle = BorderStyle.Fixed3D;
                chk_ai[idx] = new CheckBox();
                chk_ai[idx].Size = new Size(900, 30);
                chk_ai[idx].Text = temp.ToString();
                no_nc_ai[idx] = new CheckBox();
                no_nc_ai[idx].Text = idx + "";
                no_nc_ai[idx].Size = new Size(120, 30);
                //no_nc_ai[idx].Enabled = false;

                txt_address_ai[idx] = new TextBox();
                txt_address_ai[idx].Size = new Size(100, 30);
                chk_ai[idx].Tag = idx;

                chk_ai[(idx)].Parent = pnl_ai[idx]; chk_ai[(idx)].Dock = DockStyle.Left;
                no_nc_ai[(idx)].Parent = pnl_ai[idx]; no_nc_ai[(idx)].Dock = DockStyle.Left;
                txt_address_ai[(idx)].Parent = pnl_ai[idx]; txt_address_ai[(idx)].Dock = DockStyle.Left;

                txt_address_ai[idx].Text = idx.ToString();
                fpnl_ai.Controls.Add(pnl_ai[idx]);
                idx = idx + 1;
            }
            groupBox3.Text = "AI (" + idx + ")";

        }
        public string Serialize<T>(string path, string fileName, T value)
        {
            String result = "";
            try
            {
                if (Directory.Exists(path) == false) { Directory.CreateDirectory(path); }
                var builder = new SerializerBuilder().Build();
                using (var stream = File.Create(path + @"\" + fileName))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        builder.Serialize(writer, value);
                    }
                }
                result = "";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        private void btn_create_di_Click(object sender, EventArgs e)
        {
            try
            {
                int idx = 0;
                foreach (var temp in Enum.GetValues(typeof(Config_IO.enum_di)))
                {
                    di.Tag[idx] = new cds.Config_IO.Config_Digial_Tag();
                    di.Tag[idx].no = idx;
                    di.Tag[idx].use = chk_di[idx].Checked;
                    di.Tag[idx].gui_display = chk_di[idx].Checked;
                    di.Tag[idx].name = chk_di[idx].Text;
                    di.Tag[idx].description = chk_di[idx].Text;
                    di.Tag[idx].address = Convert.ToInt32(txt_address_di[idx].Text);
                    if (no_nc_di[idx].Checked == true)
                    {
                        di.Tag[idx].unit = "N.C";
                    }
                    else
                    {
                        di.Tag[idx].unit = "N.O";
                    }
                    idx = idx + 1;
                    //if (di.Tag[idx].name.ToUpper().IndexOf("PRESS") >= 0 )
                    //{
                    //}
                }
                Serialize(Application.StartupPath, "config_di.yaml", di);
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btn_create_do_Click(object sender, EventArgs e)
        {
            try
            {
                int idx = 0;
                foreach (var temp in Enum.GetValues(typeof(Config_IO.enum_do)))
                {
                    doo.Tag[idx] = new cds.Config_IO.Config_Digial_Tag();
                    doo.Tag[idx].no = idx;
                    doo.Tag[idx].use = chk_do[idx].Checked;
                    doo.Tag[idx].gui_display = chk_do[idx].Checked;
                    doo.Tag[idx].name = chk_do[idx].Text;
                    doo.Tag[idx].description = chk_do[idx].Text;
                    doo.Tag[idx].address = Convert.ToInt32(txt_address_do[idx].Text);
                    if (no_nc_do[idx].Checked == true)
                    {
                        doo.Tag[idx].unit = "N.C";
                    }
                    else
                    {
                        doo.Tag[idx].unit = "N.O";
                    }
                    idx = idx + 1;
                    //if (di.Tag[idx].name.ToUpper().IndexOf("PRESS") >= 0 )
                    //{
                    //}
                }
                Serialize(Application.StartupPath, "config_do.yaml", doo);
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btn_create_ai_Click(object sender, EventArgs e)
        {
            try
            {
                int idx = 0;
                foreach (var temp in Enum.GetValues(typeof(Config_IO.enum_ai)))
                {
                    AI.Tag[idx] = new cds.Config_IO.Config_Analog_Tag();
                    AI.Tag[idx].no = idx;
                    AI.Tag[idx].use = chk_ai[idx].Checked;
                    AI.Tag[idx].gui_display = chk_ai[idx].Checked;
                    AI.Tag[idx].name = chk_ai[idx].Text;
                    AI.Tag[idx].description = chk_ai[idx].Text;
                    AI.Tag[idx].address = Convert.ToInt32(txt_address_ai[idx].Text);
                    if (AI.Tag[idx].name.ToUpper().IndexOf("PRESS") >= 0)
                    {
                        AI.Tag[idx].min = 0;
                        AI.Tag[idx].max = 20;
                        AI.Tag[idx].gain = 31.25;
                        AI.Tag[idx].offset = -125;
                    }
                    else if (AI.Tag[idx].name.ToUpper().IndexOf("FLOW") >= 0)
                    {
                        AI.Tag[idx].min = 0;
                        AI.Tag[idx].max = 40;
                        AI.Tag[idx].gain = 0.5;
                        AI.Tag[idx].offset = -2;
                    }
                    else if (AI.Tag[idx].name.ToUpper().IndexOf("EXHAUST") >= 0)
                    {
                        AI.Tag[idx].min = 0;
                        AI.Tag[idx].max = 20;
                        AI.Tag[idx].gain = 31.25;
                        AI.Tag[idx].offset = -125;
                    }
                    AI.Tag[idx].unit = "4~20mA";
                    AI.Tag[idx].range = Config_IO.enum_aes_cbc_analog_range._4_to_20_ma;
                    idx = idx + 1;
                   
                }
                Serialize(Application.StartupPath, "config_ai.yaml", AI);
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btn_create_ao_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for(int idx = 0; idx < chk_di.Length; idx++)
            {
                if(chk_di[idx].Checked == true)
                {
                    chk_di[idx].BackColor = Color.Lime;
                }
                else
                {
                    chk_di[idx].BackColor = Color.White;
                }
            }

            for (int idx = 0; idx < chk_do.Length; idx++)
            {
                if (chk_do[idx].Checked == true)
                {
                    chk_do[idx].BackColor = Color.Lime;
                }
                else
                {
                    chk_do[idx].BackColor = Color.White;
                }
            }

            for (int idx = 0; idx < chk_ai.Length; idx++)
            {
                if (chk_ai[idx].Checked == true)
                {
                    chk_ai[idx].BackColor = Color.Lime;
                }
                else
                {
                    chk_ai[idx].BackColor = Color.White;
                }
            }
        }
    }
}
