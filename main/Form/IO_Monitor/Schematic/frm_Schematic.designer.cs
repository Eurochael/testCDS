
namespace cds
{
    partial class frm_schematic
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer_uichange = new System.Windows.Forms.Timer(this.components);
            this.timer_check_thread = new System.Windows.Forms.Timer(this.components);
            this.timer_manual_sequence_tank_a = new System.Windows.Forms.Timer(this.components);
            this.timer_manual_sequence_tank_b = new System.Windows.Forms.Timer(this.components);
            this.pnl_body = new cds.DoubleBufferedPanel();
            this.fpnl_seq_monitor = new System.Windows.Forms.FlowLayoutPanel();
            this.doubleBufferedPanel2 = new cds.DoubleBufferedPanel();
            this.txt_seq_main = new System.Windows.Forms.TextBox();
            this.lbl_seq_main = new cds.DoubleBufferedLabel();
            this.doubleBufferedPanel3 = new cds.DoubleBufferedPanel();
            this.txt_seq_supply = new System.Windows.Forms.TextBox();
            this.doubleBufferedLabel1 = new cds.DoubleBufferedLabel();
            this.doubleBufferedPanel5 = new cds.DoubleBufferedPanel();
            this.txt_seq_semi_auto_tank_a = new System.Windows.Forms.TextBox();
            this.doubleBufferedLabel2 = new cds.DoubleBufferedLabel();
            this.doubleBufferedPanel6 = new cds.DoubleBufferedPanel();
            this.txt_seq_semi_auto_tank_b = new System.Windows.Forms.TextBox();
            this.doubleBufferedLabel3 = new cds.DoubleBufferedLabel();
            this.doubleBufferedPanel7 = new cds.DoubleBufferedPanel();
            this.txt_seq_semi_auto_tank_all = new System.Windows.Forms.TextBox();
            this.doubleBufferedLabel4 = new cds.DoubleBufferedLabel();
            this.btn_seq_viewer = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.doubleBufferedPanel4 = new cds.DoubleBufferedPanel();
            this.doubleFlowLayoutPanel1 = new cds.DoubleFlowLayoutPanel();
            this.gp_common = new DevExpress.XtraEditors.GroupControl();
            this.btn_calibration = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_common_split_level_3 = new System.Windows.Forms.Panel();
            this.btn_exchange = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_common_split_level_2 = new System.Windows.Forms.Panel();
            this.btn_mode_change = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_common_split_level_1 = new System.Windows.Forms.Panel();
            this.btn_all_stop = new DevExpress.XtraEditors.SimpleButton();
            this.gp_tank_a = new DevExpress.XtraEditors.GroupControl();
            this.btn_tank_auto_flush_a = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_split_level_5 = new System.Windows.Forms.Panel();
            this.btn_tank_chem_flush_supply_a = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_split_level_4 = new System.Windows.Forms.Panel();
            this.btn_tank_diw_flush_supply_a = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_split_level_3 = new System.Windows.Forms.Panel();
            this.btn_tank_chem_flush_a = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_split_level_2 = new System.Windows.Forms.Panel();
            this.btn_tank_diw_flush_a = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_split_level_1 = new System.Windows.Forms.Panel();
            this.btn_tank_drain_a = new DevExpress.XtraEditors.SimpleButton();
            this.gp_tank_b = new DevExpress.XtraEditors.GroupControl();
            this.btn_tank_auto_flush_b = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_tank_chem_flush_supply_b = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_tank_diw_flush_supply_b = new DevExpress.XtraEditors.SimpleButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btn_tank_chem_flush_b = new DevExpress.XtraEditors.SimpleButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btn_tank_diw_flush_b = new DevExpress.XtraEditors.SimpleButton();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btn_tank_drain_b = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_body.SuspendLayout();
            this.fpnl_seq_monitor.SuspendLayout();
            this.doubleBufferedPanel2.SuspendLayout();
            this.doubleBufferedPanel3.SuspendLayout();
            this.doubleBufferedPanel5.SuspendLayout();
            this.doubleBufferedPanel6.SuspendLayout();
            this.doubleBufferedPanel7.SuspendLayout();
            this.doubleBufferedPanel4.SuspendLayout();
            this.doubleFlowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gp_common)).BeginInit();
            this.gp_common.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gp_tank_a)).BeginInit();
            this.gp_tank_a.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gp_tank_b)).BeginInit();
            this.gp_tank_b.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer_uichange
            // 
            this.timer_uichange.Tick += new System.EventHandler(this.timer_main_Tick);
            // 
            // timer_check_thread
            // 
            this.timer_check_thread.Interval = 1000;
            this.timer_check_thread.Tick += new System.EventHandler(this.timer_check_thread_Tick);
            // 
            // timer_manual_sequence_tank_a
            // 
            this.timer_manual_sequence_tank_a.Tick += new System.EventHandler(this.timer_manual_sequence_tank_a_Tick);
            // 
            // timer_manual_sequence_tank_b
            // 
            this.timer_manual_sequence_tank_b.Tick += new System.EventHandler(this.timer_manual_sequence_tank_b_Tick);
            // 
            // pnl_body
            // 
            this.pnl_body.Controls.Add(this.fpnl_seq_monitor);
            this.pnl_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_body.Location = new System.Drawing.Point(0, 0);
            this.pnl_body.Name = "pnl_body";
            this.pnl_body.Size = new System.Drawing.Size(1152, 882);
            this.pnl_body.TabIndex = 360;
            // 
            // fpnl_seq_monitor
            // 
            this.fpnl_seq_monitor.AutoScroll = true;
            this.fpnl_seq_monitor.Controls.Add(this.doubleBufferedPanel2);
            this.fpnl_seq_monitor.Controls.Add(this.doubleBufferedPanel3);
            this.fpnl_seq_monitor.Controls.Add(this.doubleBufferedPanel5);
            this.fpnl_seq_monitor.Controls.Add(this.doubleBufferedPanel6);
            this.fpnl_seq_monitor.Controls.Add(this.doubleBufferedPanel7);
            this.fpnl_seq_monitor.Controls.Add(this.btn_seq_viewer);
            this.fpnl_seq_monitor.Controls.Add(this.button1);
            this.fpnl_seq_monitor.Location = new System.Drawing.Point(3, 565);
            this.fpnl_seq_monitor.Name = "fpnl_seq_monitor";
            this.fpnl_seq_monitor.Size = new System.Drawing.Size(314, 314);
            this.fpnl_seq_monitor.TabIndex = 1327;
            // 
            // doubleBufferedPanel2
            // 
            this.doubleBufferedPanel2.Controls.Add(this.txt_seq_main);
            this.doubleBufferedPanel2.Controls.Add(this.lbl_seq_main);
            this.doubleBufferedPanel2.Location = new System.Drawing.Point(3, 3);
            this.doubleBufferedPanel2.Name = "doubleBufferedPanel2";
            this.doubleBufferedPanel2.Size = new System.Drawing.Size(287, 50);
            this.doubleBufferedPanel2.TabIndex = 0;
            // 
            // txt_seq_main
            // 
            this.txt_seq_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_seq_main.Font = new System.Drawing.Font("Tahoma", 7F);
            this.txt_seq_main.Location = new System.Drawing.Point(0, 12);
            this.txt_seq_main.Multiline = true;
            this.txt_seq_main.Name = "txt_seq_main";
            this.txt_seq_main.Size = new System.Drawing.Size(287, 38);
            this.txt_seq_main.TabIndex = 944;
            // 
            // lbl_seq_main
            // 
            this.lbl_seq_main.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_seq_main.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lbl_seq_main.Location = new System.Drawing.Point(0, 0);
            this.lbl_seq_main.Name = "lbl_seq_main";
            this.lbl_seq_main.Size = new System.Drawing.Size(287, 12);
            this.lbl_seq_main.TabIndex = 935;
            this.lbl_seq_main.Text = "Seq Main";
            // 
            // doubleBufferedPanel3
            // 
            this.doubleBufferedPanel3.Controls.Add(this.txt_seq_supply);
            this.doubleBufferedPanel3.Controls.Add(this.doubleBufferedLabel1);
            this.doubleBufferedPanel3.Location = new System.Drawing.Point(3, 59);
            this.doubleBufferedPanel3.Name = "doubleBufferedPanel3";
            this.doubleBufferedPanel3.Size = new System.Drawing.Size(287, 50);
            this.doubleBufferedPanel3.TabIndex = 1;
            // 
            // txt_seq_supply
            // 
            this.txt_seq_supply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_seq_supply.Font = new System.Drawing.Font("Tahoma", 7F);
            this.txt_seq_supply.Location = new System.Drawing.Point(0, 12);
            this.txt_seq_supply.Multiline = true;
            this.txt_seq_supply.Name = "txt_seq_supply";
            this.txt_seq_supply.Size = new System.Drawing.Size(287, 38);
            this.txt_seq_supply.TabIndex = 948;
            // 
            // doubleBufferedLabel1
            // 
            this.doubleBufferedLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.doubleBufferedLabel1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.doubleBufferedLabel1.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferedLabel1.Name = "doubleBufferedLabel1";
            this.doubleBufferedLabel1.Size = new System.Drawing.Size(287, 12);
            this.doubleBufferedLabel1.TabIndex = 947;
            this.doubleBufferedLabel1.Text = "Seq Supply";
            // 
            // doubleBufferedPanel5
            // 
            this.doubleBufferedPanel5.Controls.Add(this.txt_seq_semi_auto_tank_a);
            this.doubleBufferedPanel5.Controls.Add(this.doubleBufferedLabel2);
            this.doubleBufferedPanel5.Location = new System.Drawing.Point(3, 115);
            this.doubleBufferedPanel5.Name = "doubleBufferedPanel5";
            this.doubleBufferedPanel5.Size = new System.Drawing.Size(287, 50);
            this.doubleBufferedPanel5.TabIndex = 2;
            // 
            // txt_seq_semi_auto_tank_a
            // 
            this.txt_seq_semi_auto_tank_a.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_seq_semi_auto_tank_a.Font = new System.Drawing.Font("Tahoma", 7F);
            this.txt_seq_semi_auto_tank_a.Location = new System.Drawing.Point(0, 12);
            this.txt_seq_semi_auto_tank_a.Multiline = true;
            this.txt_seq_semi_auto_tank_a.Name = "txt_seq_semi_auto_tank_a";
            this.txt_seq_semi_auto_tank_a.Size = new System.Drawing.Size(287, 38);
            this.txt_seq_semi_auto_tank_a.TabIndex = 950;
            // 
            // doubleBufferedLabel2
            // 
            this.doubleBufferedLabel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.doubleBufferedLabel2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.doubleBufferedLabel2.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferedLabel2.Name = "doubleBufferedLabel2";
            this.doubleBufferedLabel2.Size = new System.Drawing.Size(287, 12);
            this.doubleBufferedLabel2.TabIndex = 949;
            this.doubleBufferedLabel2.Text = "Seq Semi Auto - Tank A";
            // 
            // doubleBufferedPanel6
            // 
            this.doubleBufferedPanel6.Controls.Add(this.txt_seq_semi_auto_tank_b);
            this.doubleBufferedPanel6.Controls.Add(this.doubleBufferedLabel3);
            this.doubleBufferedPanel6.Location = new System.Drawing.Point(3, 171);
            this.doubleBufferedPanel6.Name = "doubleBufferedPanel6";
            this.doubleBufferedPanel6.Size = new System.Drawing.Size(287, 50);
            this.doubleBufferedPanel6.TabIndex = 3;
            // 
            // txt_seq_semi_auto_tank_b
            // 
            this.txt_seq_semi_auto_tank_b.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_seq_semi_auto_tank_b.Font = new System.Drawing.Font("Tahoma", 7F);
            this.txt_seq_semi_auto_tank_b.Location = new System.Drawing.Point(0, 12);
            this.txt_seq_semi_auto_tank_b.Multiline = true;
            this.txt_seq_semi_auto_tank_b.Name = "txt_seq_semi_auto_tank_b";
            this.txt_seq_semi_auto_tank_b.Size = new System.Drawing.Size(287, 38);
            this.txt_seq_semi_auto_tank_b.TabIndex = 950;
            // 
            // doubleBufferedLabel3
            // 
            this.doubleBufferedLabel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.doubleBufferedLabel3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.doubleBufferedLabel3.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferedLabel3.Name = "doubleBufferedLabel3";
            this.doubleBufferedLabel3.Size = new System.Drawing.Size(287, 12);
            this.doubleBufferedLabel3.TabIndex = 949;
            this.doubleBufferedLabel3.Text = "Seq Semi Auto - Tank B";
            // 
            // doubleBufferedPanel7
            // 
            this.doubleBufferedPanel7.Controls.Add(this.txt_seq_semi_auto_tank_all);
            this.doubleBufferedPanel7.Controls.Add(this.doubleBufferedLabel4);
            this.doubleBufferedPanel7.Location = new System.Drawing.Point(3, 227);
            this.doubleBufferedPanel7.Name = "doubleBufferedPanel7";
            this.doubleBufferedPanel7.Size = new System.Drawing.Size(287, 50);
            this.doubleBufferedPanel7.TabIndex = 4;
            // 
            // txt_seq_semi_auto_tank_all
            // 
            this.txt_seq_semi_auto_tank_all.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_seq_semi_auto_tank_all.Font = new System.Drawing.Font("Tahoma", 7F);
            this.txt_seq_semi_auto_tank_all.Location = new System.Drawing.Point(0, 12);
            this.txt_seq_semi_auto_tank_all.Multiline = true;
            this.txt_seq_semi_auto_tank_all.Name = "txt_seq_semi_auto_tank_all";
            this.txt_seq_semi_auto_tank_all.Size = new System.Drawing.Size(287, 38);
            this.txt_seq_semi_auto_tank_all.TabIndex = 950;
            // 
            // doubleBufferedLabel4
            // 
            this.doubleBufferedLabel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.doubleBufferedLabel4.Font = new System.Drawing.Font("Tahoma", 8F);
            this.doubleBufferedLabel4.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferedLabel4.Name = "doubleBufferedLabel4";
            this.doubleBufferedLabel4.Size = new System.Drawing.Size(287, 12);
            this.doubleBufferedLabel4.TabIndex = 949;
            this.doubleBufferedLabel4.Text = "Seq Semi Auto - Tank ALL";
            // 
            // btn_seq_viewer
            // 
            this.btn_seq_viewer.Location = new System.Drawing.Point(3, 283);
            this.btn_seq_viewer.Name = "btn_seq_viewer";
            this.btn_seq_viewer.Size = new System.Drawing.Size(75, 23);
            this.btn_seq_viewer.TabIndex = 5;
            this.btn_seq_viewer.Text = "hide";
            this.btn_seq_viewer.UseVisualStyleBackColor = true;
            this.btn_seq_viewer.Click += new System.EventHandler(this.btn_seq_viewer_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(84, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "show";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // doubleBufferedPanel4
            // 
            this.doubleBufferedPanel4.Controls.Add(this.doubleFlowLayoutPanel1);
            this.doubleBufferedPanel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.doubleBufferedPanel4.Location = new System.Drawing.Point(1152, 0);
            this.doubleBufferedPanel4.Name = "doubleBufferedPanel4";
            this.doubleBufferedPanel4.Size = new System.Drawing.Size(126, 882);
            this.doubleBufferedPanel4.TabIndex = 359;
            // 
            // doubleFlowLayoutPanel1
            // 
            this.doubleFlowLayoutPanel1.Controls.Add(this.gp_common);
            this.doubleFlowLayoutPanel1.Controls.Add(this.gp_tank_a);
            this.doubleFlowLayoutPanel1.Controls.Add(this.gp_tank_b);
            this.doubleFlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doubleFlowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.doubleFlowLayoutPanel1.Name = "doubleFlowLayoutPanel1";
            this.doubleFlowLayoutPanel1.Size = new System.Drawing.Size(126, 882);
            this.doubleFlowLayoutPanel1.TabIndex = 0;
            // 
            // gp_common
            // 
            this.gp_common.Controls.Add(this.btn_calibration);
            this.gp_common.Controls.Add(this.pnl_common_split_level_3);
            this.gp_common.Controls.Add(this.btn_exchange);
            this.gp_common.Controls.Add(this.pnl_common_split_level_2);
            this.gp_common.Controls.Add(this.btn_mode_change);
            this.gp_common.Controls.Add(this.pnl_common_split_level_1);
            this.gp_common.Controls.Add(this.btn_all_stop);
            this.gp_common.Dock = System.Windows.Forms.DockStyle.Top;
            this.gp_common.Location = new System.Drawing.Point(3, 3);
            this.gp_common.Name = "gp_common";
            this.gp_common.Padding = new System.Windows.Forms.Padding(3);
            this.gp_common.Size = new System.Drawing.Size(120, 204);
            this.gp_common.TabIndex = 14;
            this.gp_common.Text = "Common";
            // 
            // btn_calibration
            // 
            this.btn_calibration.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_calibration.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_calibration.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_calibration.Appearance.Options.UseBackColor = true;
            this.btn_calibration.Appearance.Options.UseBorderColor = true;
            this.btn_calibration.Appearance.Options.UseFont = true;
            this.btn_calibration.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_calibration.Location = new System.Drawing.Point(5, 176);
            this.btn_calibration.Name = "btn_calibration";
            this.btn_calibration.Size = new System.Drawing.Size(110, 23);
            this.btn_calibration.TabIndex = 9;
            this.btn_calibration.Text = "Calibration";
            this.btn_calibration.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // pnl_common_split_level_3
            // 
            this.pnl_common_split_level_3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_common_split_level_3.Location = new System.Drawing.Point(5, 166);
            this.pnl_common_split_level_3.Name = "pnl_common_split_level_3";
            this.pnl_common_split_level_3.Size = new System.Drawing.Size(110, 10);
            this.pnl_common_split_level_3.TabIndex = 8;
            // 
            // btn_exchange
            // 
            this.btn_exchange.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_exchange.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_exchange.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_exchange.Appearance.Options.UseBackColor = true;
            this.btn_exchange.Appearance.Options.UseBorderColor = true;
            this.btn_exchange.Appearance.Options.UseFont = true;
            this.btn_exchange.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_exchange.Location = new System.Drawing.Point(5, 126);
            this.btn_exchange.Name = "btn_exchange";
            this.btn_exchange.Size = new System.Drawing.Size(110, 40);
            this.btn_exchange.TabIndex = 7;
            this.btn_exchange.Text = "Exchange";
            this.btn_exchange.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // pnl_common_split_level_2
            // 
            this.pnl_common_split_level_2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_common_split_level_2.Location = new System.Drawing.Point(5, 116);
            this.pnl_common_split_level_2.Name = "pnl_common_split_level_2";
            this.pnl_common_split_level_2.Size = new System.Drawing.Size(110, 10);
            this.pnl_common_split_level_2.TabIndex = 6;
            // 
            // btn_mode_change
            // 
            this.btn_mode_change.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_mode_change.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_mode_change.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_mode_change.Appearance.Options.UseBackColor = true;
            this.btn_mode_change.Appearance.Options.UseBorderColor = true;
            this.btn_mode_change.Appearance.Options.UseFont = true;
            this.btn_mode_change.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_mode_change.Location = new System.Drawing.Point(5, 76);
            this.btn_mode_change.Name = "btn_mode_change";
            this.btn_mode_change.Size = new System.Drawing.Size(110, 40);
            this.btn_mode_change.TabIndex = 5;
            this.btn_mode_change.Text = "Mode Change";
            this.btn_mode_change.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // pnl_common_split_level_1
            // 
            this.pnl_common_split_level_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_common_split_level_1.Location = new System.Drawing.Point(5, 66);
            this.pnl_common_split_level_1.Name = "pnl_common_split_level_1";
            this.pnl_common_split_level_1.Size = new System.Drawing.Size(110, 10);
            this.pnl_common_split_level_1.TabIndex = 4;
            // 
            // btn_all_stop
            // 
            this.btn_all_stop.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_all_stop.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_all_stop.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_all_stop.Appearance.Options.UseBackColor = true;
            this.btn_all_stop.Appearance.Options.UseBorderColor = true;
            this.btn_all_stop.Appearance.Options.UseFont = true;
            this.btn_all_stop.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_all_stop.Location = new System.Drawing.Point(5, 26);
            this.btn_all_stop.Name = "btn_all_stop";
            this.btn_all_stop.Size = new System.Drawing.Size(110, 40);
            this.btn_all_stop.TabIndex = 1;
            this.btn_all_stop.Text = "All Stop";
            this.btn_all_stop.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // gp_tank_a
            // 
            this.gp_tank_a.Controls.Add(this.btn_tank_auto_flush_a);
            this.gp_tank_a.Controls.Add(this.pnl_split_level_5);
            this.gp_tank_a.Controls.Add(this.btn_tank_chem_flush_supply_a);
            this.gp_tank_a.Controls.Add(this.pnl_split_level_4);
            this.gp_tank_a.Controls.Add(this.btn_tank_diw_flush_supply_a);
            this.gp_tank_a.Controls.Add(this.pnl_split_level_3);
            this.gp_tank_a.Controls.Add(this.btn_tank_chem_flush_a);
            this.gp_tank_a.Controls.Add(this.pnl_split_level_2);
            this.gp_tank_a.Controls.Add(this.btn_tank_diw_flush_a);
            this.gp_tank_a.Controls.Add(this.pnl_split_level_1);
            this.gp_tank_a.Controls.Add(this.btn_tank_drain_a);
            this.gp_tank_a.Dock = System.Windows.Forms.DockStyle.Top;
            this.gp_tank_a.Location = new System.Drawing.Point(3, 213);
            this.gp_tank_a.Name = "gp_tank_a";
            this.gp_tank_a.Padding = new System.Windows.Forms.Padding(3);
            this.gp_tank_a.Size = new System.Drawing.Size(120, 326);
            this.gp_tank_a.TabIndex = 12;
            this.gp_tank_a.Text = "Tank A";
            // 
            // btn_tank_auto_flush_a
            // 
            this.btn_tank_auto_flush_a.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_auto_flush_a.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_auto_flush_a.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_auto_flush_a.Appearance.Options.UseBackColor = true;
            this.btn_tank_auto_flush_a.Appearance.Options.UseBorderColor = true;
            this.btn_tank_auto_flush_a.Appearance.Options.UseFont = true;
            this.btn_tank_auto_flush_a.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_auto_flush_a.Location = new System.Drawing.Point(5, 281);
            this.btn_tank_auto_flush_a.Name = "btn_tank_auto_flush_a";
            this.btn_tank_auto_flush_a.Size = new System.Drawing.Size(110, 40);
            this.btn_tank_auto_flush_a.TabIndex = 28;
            this.btn_tank_auto_flush_a.Text = "Auto Flush";
            this.btn_tank_auto_flush_a.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // pnl_split_level_5
            // 
            this.pnl_split_level_5.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_split_level_5.Location = new System.Drawing.Point(5, 271);
            this.pnl_split_level_5.Name = "pnl_split_level_5";
            this.pnl_split_level_5.Size = new System.Drawing.Size(110, 10);
            this.pnl_split_level_5.TabIndex = 27;
            // 
            // btn_tank_chem_flush_supply_a
            // 
            this.btn_tank_chem_flush_supply_a.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_chem_flush_supply_a.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_chem_flush_supply_a.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_chem_flush_supply_a.Appearance.Options.UseBackColor = true;
            this.btn_tank_chem_flush_supply_a.Appearance.Options.UseBorderColor = true;
            this.btn_tank_chem_flush_supply_a.Appearance.Options.UseFont = true;
            this.btn_tank_chem_flush_supply_a.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_chem_flush_supply_a.Location = new System.Drawing.Point(5, 231);
            this.btn_tank_chem_flush_supply_a.Name = "btn_tank_chem_flush_supply_a";
            this.btn_tank_chem_flush_supply_a.Size = new System.Drawing.Size(110, 40);
            this.btn_tank_chem_flush_supply_a.TabIndex = 26;
            this.btn_tank_chem_flush_supply_a.Text = "Chem Flush\r\n( Supply)";
            this.btn_tank_chem_flush_supply_a.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // pnl_split_level_4
            // 
            this.pnl_split_level_4.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_split_level_4.Location = new System.Drawing.Point(5, 221);
            this.pnl_split_level_4.Name = "pnl_split_level_4";
            this.pnl_split_level_4.Size = new System.Drawing.Size(110, 10);
            this.pnl_split_level_4.TabIndex = 25;
            // 
            // btn_tank_diw_flush_supply_a
            // 
            this.btn_tank_diw_flush_supply_a.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_diw_flush_supply_a.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_diw_flush_supply_a.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_diw_flush_supply_a.Appearance.Options.UseBackColor = true;
            this.btn_tank_diw_flush_supply_a.Appearance.Options.UseBorderColor = true;
            this.btn_tank_diw_flush_supply_a.Appearance.Options.UseFont = true;
            this.btn_tank_diw_flush_supply_a.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_diw_flush_supply_a.Location = new System.Drawing.Point(5, 176);
            this.btn_tank_diw_flush_supply_a.Name = "btn_tank_diw_flush_supply_a";
            this.btn_tank_diw_flush_supply_a.Size = new System.Drawing.Size(110, 45);
            this.btn_tank_diw_flush_supply_a.TabIndex = 24;
            this.btn_tank_diw_flush_supply_a.Text = "DIW Flush\r\n(Supply)";
            this.btn_tank_diw_flush_supply_a.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // pnl_split_level_3
            // 
            this.pnl_split_level_3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_split_level_3.Location = new System.Drawing.Point(5, 166);
            this.pnl_split_level_3.Name = "pnl_split_level_3";
            this.pnl_split_level_3.Size = new System.Drawing.Size(110, 10);
            this.pnl_split_level_3.TabIndex = 23;
            // 
            // btn_tank_chem_flush_a
            // 
            this.btn_tank_chem_flush_a.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_chem_flush_a.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_chem_flush_a.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_chem_flush_a.Appearance.Options.UseBackColor = true;
            this.btn_tank_chem_flush_a.Appearance.Options.UseBorderColor = true;
            this.btn_tank_chem_flush_a.Appearance.Options.UseFont = true;
            this.btn_tank_chem_flush_a.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_chem_flush_a.Location = new System.Drawing.Point(5, 126);
            this.btn_tank_chem_flush_a.Name = "btn_tank_chem_flush_a";
            this.btn_tank_chem_flush_a.Size = new System.Drawing.Size(110, 40);
            this.btn_tank_chem_flush_a.TabIndex = 22;
            this.btn_tank_chem_flush_a.Text = "Chem Flush";
            this.btn_tank_chem_flush_a.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // pnl_split_level_2
            // 
            this.pnl_split_level_2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_split_level_2.Location = new System.Drawing.Point(5, 116);
            this.pnl_split_level_2.Name = "pnl_split_level_2";
            this.pnl_split_level_2.Size = new System.Drawing.Size(110, 10);
            this.pnl_split_level_2.TabIndex = 21;
            // 
            // btn_tank_diw_flush_a
            // 
            this.btn_tank_diw_flush_a.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_diw_flush_a.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_diw_flush_a.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_diw_flush_a.Appearance.Options.UseBackColor = true;
            this.btn_tank_diw_flush_a.Appearance.Options.UseBorderColor = true;
            this.btn_tank_diw_flush_a.Appearance.Options.UseFont = true;
            this.btn_tank_diw_flush_a.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_diw_flush_a.Location = new System.Drawing.Point(5, 76);
            this.btn_tank_diw_flush_a.Name = "btn_tank_diw_flush_a";
            this.btn_tank_diw_flush_a.Size = new System.Drawing.Size(110, 40);
            this.btn_tank_diw_flush_a.TabIndex = 20;
            this.btn_tank_diw_flush_a.Text = "DIW Flush";
            this.btn_tank_diw_flush_a.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // pnl_split_level_1
            // 
            this.pnl_split_level_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_split_level_1.Location = new System.Drawing.Point(5, 66);
            this.pnl_split_level_1.Name = "pnl_split_level_1";
            this.pnl_split_level_1.Size = new System.Drawing.Size(110, 10);
            this.pnl_split_level_1.TabIndex = 19;
            // 
            // btn_tank_drain_a
            // 
            this.btn_tank_drain_a.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_drain_a.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_drain_a.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_drain_a.Appearance.Options.UseBackColor = true;
            this.btn_tank_drain_a.Appearance.Options.UseBorderColor = true;
            this.btn_tank_drain_a.Appearance.Options.UseFont = true;
            this.btn_tank_drain_a.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_drain_a.Location = new System.Drawing.Point(5, 26);
            this.btn_tank_drain_a.Name = "btn_tank_drain_a";
            this.btn_tank_drain_a.Size = new System.Drawing.Size(110, 40);
            this.btn_tank_drain_a.TabIndex = 18;
            this.btn_tank_drain_a.Text = "Drain";
            this.btn_tank_drain_a.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // gp_tank_b
            // 
            this.gp_tank_b.Controls.Add(this.btn_tank_auto_flush_b);
            this.gp_tank_b.Controls.Add(this.panel1);
            this.gp_tank_b.Controls.Add(this.btn_tank_chem_flush_supply_b);
            this.gp_tank_b.Controls.Add(this.panel2);
            this.gp_tank_b.Controls.Add(this.btn_tank_diw_flush_supply_b);
            this.gp_tank_b.Controls.Add(this.panel3);
            this.gp_tank_b.Controls.Add(this.btn_tank_chem_flush_b);
            this.gp_tank_b.Controls.Add(this.panel4);
            this.gp_tank_b.Controls.Add(this.btn_tank_diw_flush_b);
            this.gp_tank_b.Controls.Add(this.panel7);
            this.gp_tank_b.Controls.Add(this.btn_tank_drain_b);
            this.gp_tank_b.Dock = System.Windows.Forms.DockStyle.Top;
            this.gp_tank_b.Location = new System.Drawing.Point(3, 545);
            this.gp_tank_b.Name = "gp_tank_b";
            this.gp_tank_b.Padding = new System.Windows.Forms.Padding(3);
            this.gp_tank_b.Size = new System.Drawing.Size(120, 334);
            this.gp_tank_b.TabIndex = 13;
            this.gp_tank_b.Text = "Tank B";
            // 
            // btn_tank_auto_flush_b
            // 
            this.btn_tank_auto_flush_b.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_auto_flush_b.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_auto_flush_b.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_auto_flush_b.Appearance.Options.UseBackColor = true;
            this.btn_tank_auto_flush_b.Appearance.Options.UseBorderColor = true;
            this.btn_tank_auto_flush_b.Appearance.Options.UseFont = true;
            this.btn_tank_auto_flush_b.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_auto_flush_b.Location = new System.Drawing.Point(5, 286);
            this.btn_tank_auto_flush_b.Name = "btn_tank_auto_flush_b";
            this.btn_tank_auto_flush_b.Size = new System.Drawing.Size(110, 40);
            this.btn_tank_auto_flush_b.TabIndex = 39;
            this.btn_tank_auto_flush_b.Text = "Auto Flush";
            this.btn_tank_auto_flush_b.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 276);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(110, 10);
            this.panel1.TabIndex = 38;
            // 
            // btn_tank_chem_flush_supply_b
            // 
            this.btn_tank_chem_flush_supply_b.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_chem_flush_supply_b.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_chem_flush_supply_b.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_chem_flush_supply_b.Appearance.Options.UseBackColor = true;
            this.btn_tank_chem_flush_supply_b.Appearance.Options.UseBorderColor = true;
            this.btn_tank_chem_flush_supply_b.Appearance.Options.UseFont = true;
            this.btn_tank_chem_flush_supply_b.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_chem_flush_supply_b.Location = new System.Drawing.Point(5, 231);
            this.btn_tank_chem_flush_supply_b.Name = "btn_tank_chem_flush_supply_b";
            this.btn_tank_chem_flush_supply_b.Size = new System.Drawing.Size(110, 45);
            this.btn_tank_chem_flush_supply_b.TabIndex = 37;
            this.btn_tank_chem_flush_supply_b.Text = "Chem Flush\r\n( Supply)";
            this.btn_tank_chem_flush_supply_b.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 221);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(110, 10);
            this.panel2.TabIndex = 36;
            // 
            // btn_tank_diw_flush_supply_b
            // 
            this.btn_tank_diw_flush_supply_b.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_diw_flush_supply_b.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_diw_flush_supply_b.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_diw_flush_supply_b.Appearance.Options.UseBackColor = true;
            this.btn_tank_diw_flush_supply_b.Appearance.Options.UseBorderColor = true;
            this.btn_tank_diw_flush_supply_b.Appearance.Options.UseFont = true;
            this.btn_tank_diw_flush_supply_b.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_diw_flush_supply_b.Location = new System.Drawing.Point(5, 176);
            this.btn_tank_diw_flush_supply_b.Name = "btn_tank_diw_flush_supply_b";
            this.btn_tank_diw_flush_supply_b.Size = new System.Drawing.Size(110, 45);
            this.btn_tank_diw_flush_supply_b.TabIndex = 35;
            this.btn_tank_diw_flush_supply_b.Text = "DIW Flush\r\n(Supply)";
            this.btn_tank_diw_flush_supply_b.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 166);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(110, 10);
            this.panel3.TabIndex = 34;
            // 
            // btn_tank_chem_flush_b
            // 
            this.btn_tank_chem_flush_b.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_chem_flush_b.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_chem_flush_b.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_chem_flush_b.Appearance.Options.UseBackColor = true;
            this.btn_tank_chem_flush_b.Appearance.Options.UseBorderColor = true;
            this.btn_tank_chem_flush_b.Appearance.Options.UseFont = true;
            this.btn_tank_chem_flush_b.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_chem_flush_b.Location = new System.Drawing.Point(5, 126);
            this.btn_tank_chem_flush_b.Name = "btn_tank_chem_flush_b";
            this.btn_tank_chem_flush_b.Size = new System.Drawing.Size(110, 40);
            this.btn_tank_chem_flush_b.TabIndex = 33;
            this.btn_tank_chem_flush_b.Text = "Chem Flush";
            this.btn_tank_chem_flush_b.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(5, 116);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(110, 10);
            this.panel4.TabIndex = 32;
            // 
            // btn_tank_diw_flush_b
            // 
            this.btn_tank_diw_flush_b.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_diw_flush_b.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_diw_flush_b.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_diw_flush_b.Appearance.Options.UseBackColor = true;
            this.btn_tank_diw_flush_b.Appearance.Options.UseBorderColor = true;
            this.btn_tank_diw_flush_b.Appearance.Options.UseFont = true;
            this.btn_tank_diw_flush_b.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_diw_flush_b.Location = new System.Drawing.Point(5, 76);
            this.btn_tank_diw_flush_b.Name = "btn_tank_diw_flush_b";
            this.btn_tank_diw_flush_b.Size = new System.Drawing.Size(110, 40);
            this.btn_tank_diw_flush_b.TabIndex = 31;
            this.btn_tank_diw_flush_b.Text = "DIW Flush";
            this.btn_tank_diw_flush_b.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(5, 66);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(110, 10);
            this.panel7.TabIndex = 30;
            // 
            // btn_tank_drain_b
            // 
            this.btn_tank_drain_b.Appearance.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_tank_drain_b.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btn_tank_drain_b.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btn_tank_drain_b.Appearance.Options.UseBackColor = true;
            this.btn_tank_drain_b.Appearance.Options.UseBorderColor = true;
            this.btn_tank_drain_b.Appearance.Options.UseFont = true;
            this.btn_tank_drain_b.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_tank_drain_b.Location = new System.Drawing.Point(5, 26);
            this.btn_tank_drain_b.Name = "btn_tank_drain_b";
            this.btn_tank_drain_b.Size = new System.Drawing.Size(110, 40);
            this.btn_tank_drain_b.TabIndex = 29;
            this.btn_tank_drain_b.Text = "Drain";
            this.btn_tank_drain_b.Click += new System.EventHandler(this.btn_all_stop_Click);
            // 
            // frm_schematic
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1278, 882);
            this.Controls.Add(this.pnl_body);
            this.Controls.Add(this.doubleBufferedPanel4);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_schematic";
            this.Text = "frm_Schematic";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_schematic_FormClosed);
            this.Load += new System.EventHandler(this.frm_schematic_Load);
            this.pnl_body.ResumeLayout(false);
            this.fpnl_seq_monitor.ResumeLayout(false);
            this.doubleBufferedPanel2.ResumeLayout(false);
            this.doubleBufferedPanel2.PerformLayout();
            this.doubleBufferedPanel3.ResumeLayout(false);
            this.doubleBufferedPanel3.PerformLayout();
            this.doubleBufferedPanel5.ResumeLayout(false);
            this.doubleBufferedPanel5.PerformLayout();
            this.doubleBufferedPanel6.ResumeLayout(false);
            this.doubleBufferedPanel6.PerformLayout();
            this.doubleBufferedPanel7.ResumeLayout(false);
            this.doubleBufferedPanel7.PerformLayout();
            this.doubleBufferedPanel4.ResumeLayout(false);
            this.doubleFlowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gp_common)).EndInit();
            this.gp_common.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gp_tank_a)).EndInit();
            this.gp_tank_a.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gp_tank_b)).EndInit();
            this.gp_tank_b.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DoubleBufferedPanel doubleBufferedPanel4;
        private DoubleFlowLayoutPanel doubleFlowLayoutPanel1;
        private System.Windows.Forms.Timer timer_uichange;
        private DevExpress.XtraEditors.GroupControl gp_tank_a;
        private DevExpress.XtraEditors.GroupControl gp_tank_b;
        private System.Windows.Forms.Timer timer_check_thread;
        private DevExpress.XtraEditors.SimpleButton btn_tank_chem_flush_a;
        private System.Windows.Forms.Panel pnl_split_level_2;
        private DevExpress.XtraEditors.SimpleButton btn_tank_diw_flush_a;
        private System.Windows.Forms.Panel pnl_split_level_1;
        private DevExpress.XtraEditors.SimpleButton btn_tank_drain_a;
        private DevExpress.XtraEditors.SimpleButton btn_tank_auto_flush_a;
        private System.Windows.Forms.Panel pnl_split_level_5;
        private DevExpress.XtraEditors.SimpleButton btn_tank_chem_flush_supply_a;
        private System.Windows.Forms.Panel pnl_split_level_4;
        private DevExpress.XtraEditors.SimpleButton btn_tank_diw_flush_supply_a;
        private System.Windows.Forms.Panel pnl_split_level_3;
        private DevExpress.XtraEditors.SimpleButton btn_tank_auto_flush_b;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btn_tank_chem_flush_supply_b;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton btn_tank_diw_flush_supply_b;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.SimpleButton btn_tank_chem_flush_b;
        private System.Windows.Forms.Panel panel4;
        private DevExpress.XtraEditors.SimpleButton btn_tank_diw_flush_b;
        private System.Windows.Forms.Panel panel7;
        private DevExpress.XtraEditors.SimpleButton btn_tank_drain_b;
        private DoubleBufferedPanel pnl_body;
        private DoubleBufferedPanel doubleBufferedPanel2;
        private System.Windows.Forms.TextBox txt_seq_main;
        private DoubleBufferedLabel lbl_seq_main;
        private DoubleBufferedPanel doubleBufferedPanel3;
        private System.Windows.Forms.TextBox txt_seq_supply;
        private DoubleBufferedLabel doubleBufferedLabel1;
        private DoubleBufferedPanel doubleBufferedPanel5;
        private System.Windows.Forms.TextBox txt_seq_semi_auto_tank_a;
        private DoubleBufferedLabel doubleBufferedLabel2;
        private DoubleBufferedPanel doubleBufferedPanel6;
        private System.Windows.Forms.TextBox txt_seq_semi_auto_tank_b;
        private DoubleBufferedLabel doubleBufferedLabel3;
        private DoubleBufferedPanel doubleBufferedPanel7;
        private System.Windows.Forms.TextBox txt_seq_semi_auto_tank_all;
        private DoubleBufferedLabel doubleBufferedLabel4;
        public System.Windows.Forms.FlowLayoutPanel fpnl_seq_monitor;
        public System.Windows.Forms.Button btn_seq_viewer;
        public System.Windows.Forms.Timer timer_manual_sequence_tank_a;
        public System.Windows.Forms.Timer timer_manual_sequence_tank_b;
        public DevExpress.XtraEditors.SimpleButton btn_exchange;
        public DevExpress.XtraEditors.GroupControl gp_common;
        public DevExpress.XtraEditors.SimpleButton btn_all_stop;
        public DevExpress.XtraEditors.SimpleButton btn_mode_change;
        public System.Windows.Forms.Panel pnl_common_split_level_1;
        public System.Windows.Forms.Panel pnl_common_split_level_2;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Panel pnl_common_split_level_3;
        public DevExpress.XtraEditors.SimpleButton btn_calibration;
    }
}