using DevExpress.LookAndFeel;
using DevExpress.Spreadsheet;
using DevExpress.XtraBars;
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
    public partial class frm_skin : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Boolean _actived = false;
        public frm_skin()
        {
            InitializeComponent();
        }
        public Boolean actived
        {
            get { return _actived; }
            set
            {
                try
                {
                    _actived = value;
                    if (_actived == true)
                    {

                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                { Program.log_md.LogWrite(this.Name + ".actived." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS); }
                finally { }
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
           // UserLookAndFeel.Default.SkinName = "My Office 2019 Black";
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_main, "");
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_sync, "");
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_trend, "");
        }
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_socket, "");
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_trace, "");
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_app_info, "");
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            Program.cg_mixing_step.ccss_cnt = 4;
            Program.cg_mixing_step.step_cnt = 4;
            Program.cg_mixing_step.ccss_info = new CCSS_Info[Program.cg_mixing_step.ccss_cnt];
            Program.cg_mixing_step.step_info = new STEP_Info[Program.cg_mixing_step.step_cnt];
            Program.cg_mixing_step.mixing_data = new int[Program.cg_mixing_step.step_cnt * Program.cg_mixing_step.ccss_cnt];
            for (int idx = 0; idx < Program.cg_mixing_step.ccss_cnt; idx++)
            {
                Program.cg_mixing_step.ccss_info[idx] = new CCSS_Info();
                Program.cg_mixing_step.ccss_info[idx].name = "chem" + (idx + 1);
            }
            for (int idx = 0; idx < Program.cg_mixing_step.step_cnt; idx++)
            {
                Program.cg_mixing_step.step_info[idx] = new STEP_Info();
                Program.cg_mixing_step.step_info[idx].name = "Step " + (idx + 1);
            }
            Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_mixing_step, "");
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            
            Program.cg_unit_no.total_cnt = 35;
            Program.cg_unit_no.unit_io = new Unit_IO[Program.cg_unit_no.total_cnt];
            for (int idx = 0; idx < Program.cg_unit_no.total_cnt; idx++)
            {
                Program.cg_unit_no.unit_io[idx] = new Unit_IO();
               switch (idx)
                {
                    case 0:
                        //ctc
                        Program.cg_unit_no.unit_io[idx].unit_type = enum_unit_type.ctc;
                        Program.cg_unit_no.unit_io[idx].name = "uCTC";
                        Program.cg_unit_no.unit_io[idx].index = idx;
                        Program.cg_unit_no.unit_io[idx].no = 0;
                        break;
                    case int n when(1 <= n && n <= 12):
                        //pmc
                        Program.cg_unit_no.unit_io[idx].unit_type = enum_unit_type.pmc;
                        Program.cg_unit_no.unit_io[idx].name = "uPMC" + n;
                        Program.cg_unit_no.unit_io[idx].index = idx;
                        Program.cg_unit_no.unit_io[idx].no = n;
                        break;
                    case int n when (13 <= n && n <= 14):
                        //spare
                        Program.cg_unit_no.unit_io[idx].unit_type = enum_unit_type.spare;
                        Program.cg_unit_no.unit_io[idx].name = "spare" + n;
                        Program.cg_unit_no.unit_io[idx].index = idx;
                        Program.cg_unit_no.unit_io[idx].no = n;
                        break;
                    case int n when (15 <= n && n <= 20):
                        //cds
                        Program.cg_unit_no.unit_io[idx].unit_type = enum_unit_type.cds;
                        Program.cg_unit_no.unit_io[idx].name = "uCDS" + n;
                        Program.cg_unit_no.unit_io[idx].index = idx;
                        Program.cg_unit_no.unit_io[idx].no = n;
                        break;
                    case int n when (21 <= n && n <= 27):
                        //spare
                        Program.cg_unit_no.unit_io[idx].unit_type = enum_unit_type.spare;
                        Program.cg_unit_no.unit_io[idx].name = "spare" + n;
                        Program.cg_unit_no.unit_io[idx].index = idx;
                        Program.cg_unit_no.unit_io[idx].no = n;
                        break;
                    case 28:
                        //ctc io
                        Program.cg_unit_no.unit_io[idx].unit_type = enum_unit_type.ctc_io;
                        Program.cg_unit_no.unit_io[idx].name = "uDI";
                        Program.cg_unit_no.unit_io[idx].index = idx;
                        Program.cg_unit_no.unit_io[idx].no = 28;
                        break;
                    case 29:
                        //ctc io
                        Program.cg_unit_no.unit_io[idx].unit_type = enum_unit_type.ctc_io;
                        Program.cg_unit_no.unit_io[idx].name = "uDO";
                        Program.cg_unit_no.unit_io[idx].index = idx;
                        Program.cg_unit_no.unit_io[idx].no = 29;
                        break;
                    case 30:
                        //ctc io
                        Program.cg_unit_no.unit_io[idx].unit_type = enum_unit_type.ctc_io;
                        Program.cg_unit_no.unit_io[idx].name = "uAI";
                        Program.cg_unit_no.unit_io[idx].index = idx;
                        Program.cg_unit_no.unit_io[idx].no = 30;
                        break;
                    case 31:
                        //ctc io
                        Program.cg_unit_no.unit_io[idx].unit_type = enum_unit_type.ctc_io;
                        Program.cg_unit_no.unit_io[idx].name = "uAO";
                        Program.cg_unit_no.unit_io[idx].index = idx;
                        Program.cg_unit_no.unit_io[idx].no = 31;
                        break;
                    case int n when (32 <= n):
                        //spare
                        Program.cg_unit_no.unit_io[idx].unit_type = enum_unit_type.spare;
                        Program.cg_unit_no.unit_io[idx].name = "spare" + n;
                        Program.cg_unit_no.unit_io[idx].index = idx;
                        Program.cg_unit_no.unit_io[idx].no = n;
                        break;
                }
            }
            Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_unit_io, "");
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            //digital In Config Create
            try
            {
                int idx_class = 0;
                IWorkbook workbook = spreadsheetControl2.Document;
                WorksheetCollection worksheets = workbook.Worksheets;
                Worksheet worksheet = workbook.Worksheets[1];
                
                Program.IO.DI.use_cnt = Convert.ToInt32(txt_real_use_cnt.Text);
                Program.IO.DI.Tag = new Config_IO.Config_Digial_Tag[Program.IO.DI.use_cnt];
                if (txt_start_address_1.Text != "")
                {
                    for (int idx = Convert.ToInt32(txt_start_address_1.Text); idx < Convert.ToInt32(txt_address_use_cnt_1.Text) + Convert.ToInt32(txt_start_address_1.Text); idx++)
                    {
                        Program.IO.DI.Tag[idx_class] = new Config_IO.Config_Digial_Tag();
                        Program.IO.DI.Tag[idx_class].address = Convert.ToInt32(worksheet.GetCellValue(6, idx-1).ToString());
                        Program.IO.DI.Tag[idx_class].unit = worksheet.GetCellValue(7, idx - 1).ToString();
                        Program.IO.DI.Tag[idx_class].name = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.DI.Tag[idx_class].description = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.DI.Tag[idx_class].etc = worksheet.GetCellValue(9, idx - 1).ToString();

                        Program.IO.DI.Tag[idx_class].use = true;
                        Program.IO.DI.Tag[idx_class].gui_display = true;
                        
                        idx_class = idx_class + 1;
                    }


                }

                Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_di, "");


                //analog Out Config Create




                //serial Config Create

            }
            catch (Exception ex)
            {

            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1;
            try
            {
                openFileDialog1 = new OpenFileDialog();
                //openFileDialog1.Filter = "*.xlsx";
                openFileDialog1 = new OpenFileDialog();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    spreadsheetControl2.LoadDocument(openFileDialog1.FileName, DocumentFormat.Xlsx);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            //digital Out Config Create
            try
            {
                int idx_class = 0;
                IWorkbook workbook = spreadsheetControl2.Document;
                WorksheetCollection worksheets = workbook.Worksheets;
                Worksheet worksheet = workbook.Worksheets[1];
                //digital In Config Create

                Program.IO.DO.use_cnt = Convert.ToInt32(txt_real_use_cnt.Text);
                Program.IO.DO.Tag = new Config_IO.Config_Digial_Tag[Program.IO.DO.use_cnt];
                if (txt_start_address_1.Text != "")
                {
                    for (int idx = Convert.ToInt32(txt_start_address_1.Text); idx < Convert.ToInt32(txt_address_use_cnt_1.Text) + Convert.ToInt32(txt_start_address_1.Text); idx++)
                    {
                        Program.IO.DO.Tag[idx_class] = new Config_IO.Config_Digial_Tag();
                        Program.IO.DO.Tag[idx_class].address = idx_class;//Convert.ToInt32(worksheet.GetCellValue(6, idx - 1).ToString());
                        Program.IO.DO.Tag[idx_class].unit = worksheet.GetCellValue(7, idx - 1).ToString();
                        Program.IO.DO.Tag[idx_class].name = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.DO.Tag[idx_class].description = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.DO.Tag[idx_class].etc = worksheet.GetCellValue(9, idx - 1).ToString();

                        Program.IO.DO.Tag[idx_class].use = true;
                        Program.IO.DO.Tag[idx_class].gui_display = true;

                        idx_class = idx_class + 1;
                    }
                }
                if (txt_start_address_2.Text != "")
                {
                    for (int idx = Convert.ToInt32(txt_start_address_2.Text); idx < Convert.ToInt32(txt_address_use_cnt_2.Text) + Convert.ToInt32(txt_start_address_2.Text); idx++)
                    {
                        Program.IO.DO.Tag[idx_class] = new Config_IO.Config_Digial_Tag();
                        Program.IO.DO.Tag[idx_class].address = idx_class;// idx_class + Convert.ToInt32(worksheet.GetCellValue(6, idx - 1).ToString());
                        Program.IO.DO.Tag[idx_class].unit = worksheet.GetCellValue(7, idx - 1).ToString();
                        Program.IO.DO.Tag[idx_class].name = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.DO.Tag[idx_class].description = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.DO.Tag[idx_class].etc = worksheet.GetCellValue(9, idx - 1).ToString();

                        Program.IO.DO.Tag[idx_class].use = true;
                        Program.IO.DO.Tag[idx_class].gui_display = true;

                        idx_class = idx_class + 1;
                    }
                }
                if (txt_start_address_3.Text != "")
                {
                    for (int idx = Convert.ToInt32(txt_start_address_3.Text); idx < Convert.ToInt32(txt_address_use_cnt_3.Text) + Convert.ToInt32(txt_start_address_3.Text); idx++)
                    {
                        Program.IO.DO.Tag[idx_class] = new Config_IO.Config_Digial_Tag();
                        Program.IO.DO.Tag[idx_class].address = idx_class;// Convert.ToInt32(worksheet.GetCellValue(6, idx - 1).ToString());
                        Program.IO.DO.Tag[idx_class].unit = worksheet.GetCellValue(7, idx - 1).ToString();
                        Program.IO.DO.Tag[idx_class].name = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.DO.Tag[idx_class].description = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.DO.Tag[idx_class].etc = worksheet.GetCellValue(9, idx - 1).ToString();

                        Program.IO.DO.Tag[idx_class].use = true;
                        Program.IO.DO.Tag[idx_class].gui_display = true;

                        idx_class = idx_class + 1;
                    }
                }

                Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_do, "");


                //analog Out Config Create

                //serial Config Create

            }
            catch (Exception ex)
            {

            }
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            //Analog In Config Create
            try
            {
                int idx_class = 0;
                IWorkbook workbook = spreadsheetControl2.Document;
                WorksheetCollection worksheets = workbook.Worksheets;
                Worksheet worksheet = workbook.Worksheets[1];
                //digital In Config Create

                Program.IO.AI.use_cnt = Convert.ToInt32(txt_real_use_cnt.Text);
                Program.IO.AI.Tag = new Config_IO.Config_Analog_Tag[Program.IO.AI.use_cnt];
                if (txt_start_address_1.Text != "")
                {
                    for (int idx = Convert.ToInt32(txt_start_address_1.Text); idx < Convert.ToInt32(txt_address_use_cnt_1.Text) + Convert.ToInt32(txt_start_address_1.Text); idx++)
                    {
                        Program.IO.AI.Tag[idx_class] = new Config_IO.Config_Analog_Tag();
                        Program.IO.AI.Tag[idx_class].address = idx_class;//Convert.ToInt32(worksheet.GetCellValue(6, idx - 1).ToString());
                        Program.IO.AI.Tag[idx_class].unit = worksheet.GetCellValue(7, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].name = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].description = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].etc = worksheet.GetCellValue(9, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].range = Config_IO.enum_aes_cbc_analog_range._4_to_20_ma;
                        Program.IO.AI.Tag[idx_class].use = true;
                        Program.IO.AI.Tag[idx_class].gui_display = true;

                        idx_class = idx_class + 1;
                    }
                }
                if (txt_start_address_2.Text != "")
                {
                    for (int idx = Convert.ToInt32(txt_start_address_2.Text); idx < Convert.ToInt32(txt_address_use_cnt_2.Text) + Convert.ToInt32(txt_start_address_2.Text); idx++)
                    {
                        Program.IO.AI.Tag[idx_class] = new Config_IO.Config_Analog_Tag();
                        Program.IO.AI.Tag[idx_class].address = idx_class;// idx_class + Convert.ToInt32(worksheet.GetCellValue(6, idx - 1).ToString());
                        Program.IO.AI.Tag[idx_class].unit = worksheet.GetCellValue(7, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].name = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].description = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].etc = worksheet.GetCellValue(9, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].range = Config_IO.enum_aes_cbc_analog_range._4_to_20_ma;
                        Program.IO.AI.Tag[idx_class].use = true;
                        Program.IO.AI.Tag[idx_class].gui_display = true;

                        idx_class = idx_class + 1;
                    }
                }
                if (txt_start_address_3.Text != "")
                {
                    for (int idx = Convert.ToInt32(txt_start_address_3.Text); idx < Convert.ToInt32(txt_address_use_cnt_3.Text) + Convert.ToInt32(txt_start_address_3.Text); idx++)
                    {
                        Program.IO.AI.Tag[idx_class] = new Config_IO.Config_Analog_Tag();
                        Program.IO.AI.Tag[idx_class].address = idx_class;// Convert.ToInt32(worksheet.GetCellValue(6, idx - 1).ToString());
                        Program.IO.AI.Tag[idx_class].unit = worksheet.GetCellValue(7, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].name = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].description = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].etc = worksheet.GetCellValue(9, idx - 1).ToString();
                        Program.IO.AI.Tag[idx_class].range = Config_IO.enum_aes_cbc_analog_range._4_to_20_ma;
                        Program.IO.AI.Tag[idx_class].use = true;
                        Program.IO.AI.Tag[idx_class].gui_display = true;

                        idx_class = idx_class + 1;
                    }
                }

                Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_ai, "");


                //analog Out Config Create

                //serial Config Create

            }
            catch (Exception ex)
            {

            }
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            //Analog In Config Create
            try
            {
                int idx_class = 0;
                IWorkbook workbook = spreadsheetControl2.Document;
                WorksheetCollection worksheets = workbook.Worksheets;
                Worksheet worksheet = workbook.Worksheets[1];
                //digital In Config Create

                Program.IO.AO.use_cnt = Convert.ToInt32(txt_real_use_cnt.Text);
                Program.IO.AO.Tag = new Config_IO.Config_Analog_Tag[Program.IO.AO.use_cnt];
                if (txt_start_address_1.Text != "")
                {
                    for (int idx = Convert.ToInt32(txt_start_address_1.Text); idx < Convert.ToInt32(txt_address_use_cnt_1.Text) + Convert.ToInt32(txt_start_address_1.Text); idx++)
                    {
                        Program.IO.AO.Tag[idx_class] = new Config_IO.Config_Analog_Tag();
                        Program.IO.AO.Tag[idx_class].address = idx_class;//Convert.ToInt32(worksheet.GetCellValue(6, idx - 1).ToString());
                        Program.IO.AO.Tag[idx_class].unit = worksheet.GetCellValue(7, idx - 1).ToString();
                        Program.IO.AO.Tag[idx_class].name = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AO.Tag[idx_class].description = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AO.Tag[idx_class].etc = worksheet.GetCellValue(9, idx - 1).ToString();

                        Program.IO.AO.Tag[idx_class].use = true;
                        Program.IO.AO.Tag[idx_class].gui_display = true;

                        idx_class = idx_class + 1;
                    }
                }
                if (txt_start_address_2.Text != "")
                {
                    for (int idx = Convert.ToInt32(txt_start_address_2.Text); idx < Convert.ToInt32(txt_address_use_cnt_2.Text) + Convert.ToInt32(txt_start_address_2.Text); idx++)
                    {
                        Program.IO.AO.Tag[idx_class] = new Config_IO.Config_Analog_Tag();
                        Program.IO.AO.Tag[idx_class].address = idx_class;// idx_class + Convert.ToInt32(worksheet.GetCellValue(6, idx - 1).ToString());
                        Program.IO.AO.Tag[idx_class].unit = worksheet.GetCellValue(7, idx - 1).ToString();
                        Program.IO.AO.Tag[idx_class].name = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AO.Tag[idx_class].description = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AO.Tag[idx_class].etc = worksheet.GetCellValue(9, idx - 1).ToString();

                        Program.IO.AO.Tag[idx_class].use = true;
                        Program.IO.AO.Tag[idx_class].gui_display = true;

                        idx_class = idx_class + 1;
                    }
                }
                if (txt_start_address_3.Text != "")
                {
                    for (int idx = Convert.ToInt32(txt_start_address_3.Text); idx < Convert.ToInt32(txt_address_use_cnt_3.Text) + Convert.ToInt32(txt_start_address_3.Text); idx++)
                    {
                        Program.IO.AO.Tag[idx_class] = new Config_IO.Config_Analog_Tag();
                        Program.IO.AO.Tag[idx_class].address = idx_class;// Convert.ToInt32(worksheet.GetCellValue(6, idx - 1).ToString());
                        Program.IO.AO.Tag[idx_class].unit = worksheet.GetCellValue(7, idx - 1).ToString();
                        Program.IO.AO.Tag[idx_class].name = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AO.Tag[idx_class].description = worksheet.GetCellValue(8, idx - 1).ToString();
                        Program.IO.AO.Tag[idx_class].etc = worksheet.GetCellValue(9, idx - 1).ToString();

                        Program.IO.AO.Tag[idx_class].use = true;
                        Program.IO.AO.Tag[idx_class].gui_display = true;

                        idx_class = idx_class + 1;
                    }
                }

                Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_ao, "");

                //analog Out Config Create
                //serial Config Create

            }
            catch (Exception ex)
            {

            }
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            try
            {
                int idx_tmp = 0;
                //serial data
                Program.IO.SERIAL.use_cnt = 8;
                Program.IO.SERIAL.Tag = new Config_IO.Config_Serial_Tag[Program.IO.SERIAL.use_cnt];

                //DHF 기준
                for(int idx =0; idx < 8; idx++)
                {
                    Program.IO.SERIAL.Tag[idx] = new Config_IO.Config_Serial_Tag();
                }
                idx_tmp = 0;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = true;
                Program.IO.SERIAL.Tag[idx_tmp].description = "SUPPLY A THERMOSTAT";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.THERMOSTAT_HE_3320C;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 1;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = true;
                Program.IO.SERIAL.Tag[idx_tmp].description = "SYPPLY B THERMOSTAT";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.THERMOSTAT_HE_3320C;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 2;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = true;
                Program.IO.SERIAL.Tag[idx_tmp].description = "CIRCULATION THERMOSTAT";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.THERMOSTAT_HE_3320C;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 3;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = true;
                Program.IO.SERIAL.Tag[idx_tmp].description = "USF-500 FLOWMETER";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.FLOWMETER_USF500;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 4;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = false;
                Program.IO.SERIAL.Tag[idx_tmp].description = "SUPPLY A PUMP CONTROLLER";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.PUMP_CONTROLLER_PB12;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 5;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = false;
                Program.IO.SERIAL.Tag[idx_tmp].description = "SUPPLY B PUMP CONTROLLER";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.PUMP_CONTROLLER_PB12;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 6;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = false;
                Program.IO.SERIAL.Tag[idx_tmp].description = "CONCENTRATION";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.CONCENTRATION_CM210A_DC;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 7;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].description = "TEMP CONTROLLER";
                Program.IO.SERIAL.Tag[idx_tmp].use = true;
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.TEMP_CONTROLLER_M74;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._19200;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.even;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;


                Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_serial, "");

            }
            catch (Exception ex)
            {

            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            try
            {
                int idx_tmp = 0;
                //serial data
                Program.IO.SERIAL.use_cnt = 8;
                Program.IO.SERIAL.Tag = new Config_IO.Config_Serial_Tag[Program.IO.SERIAL.use_cnt];

                //DHF 기준
                for (int idx = 0; idx < 8; idx++)
                {
                    Program.IO.SERIAL.Tag[idx] = new Config_IO.Config_Serial_Tag();
                }
                idx_tmp = 0;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = true;
                Program.IO.SERIAL.Tag[idx_tmp].description = "SCR";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.SCR_DPU31A_025A;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._38400;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.even;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 1;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = true;
                Program.IO.SERIAL.Tag[idx_tmp].description = "TEMP LIMIT";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.TEMP_CONTROLLER_M74R;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._19200;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.even;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 2;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = false;
                Program.IO.SERIAL.Tag[idx_tmp].description = "Spare";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.THERMOSTAT_HE_3320C;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 3;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = true;
                Program.IO.SERIAL.Tag[idx_tmp].description = "USF-500 FLOWMETER";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.FLOWMETER_USF500;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 4;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = false;
                Program.IO.SERIAL.Tag[idx_tmp].description = "SUPPLY A PUMP CONTROLLER";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.PUMP_CONTROLLER_PB12;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 5;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = false;
                Program.IO.SERIAL.Tag[idx_tmp].description = "SUPPLY B PUMP CONTROLLER";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.PUMP_CONTROLLER_PB12;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 6;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].use = false;
                Program.IO.SERIAL.Tag[idx_tmp].description = "SPARE";
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.CONCENTRATION_CM210A_DC;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._9600;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.none;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;

                idx_tmp = 7;
                Program.IO.SERIAL.Tag[idx_tmp].address = idx_tmp;
                Program.IO.SERIAL.Tag[idx_tmp].description = "TEMP CONTROLLER";
                Program.IO.SERIAL.Tag[idx_tmp].use = true;
                Program.IO.SERIAL.Tag[idx_tmp].unit = Config_IO.enum_unit.TEMP_CONTROLLER_M74;
                Program.IO.SERIAL.Tag[idx_tmp].buadrate = Config_IO.enum_aes_cbc_serial_baudrate._19200;
                Program.IO.SERIAL.Tag[idx_tmp].parity = Config_IO.enum_aes_cbc_serial_parity.even;
                Program.IO.SERIAL.Tag[idx_tmp].databit = Config_IO.enum_aes_cbc_serial_databit.bit8;
                Program.IO.SERIAL.Tag[idx_tmp].stopbit = Config_IO.enum_aes_cbc_serial_stopbit.bit1;


                Program.sub_md.Config_Class_To_Yaml(Module_sub.Config_type.cg_serial, "");

            }
            catch (Exception ex)
            {

            }
        }
    }
}