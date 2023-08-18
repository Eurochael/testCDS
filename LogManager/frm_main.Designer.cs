
namespace LogManager
{
    partial class frm_main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_main));
            this.timer_uichange = new System.Windows.Forms.Timer(this.components);
            this.btn_save = new System.Windows.Forms.Button();
            this.timer_checkthread = new System.Windows.Forms.Timer(this.components);
            this.btn_return = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.programEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbox_queue_log = new System.Windows.Forms.ListBox();
            this.btn_force_act = new System.Windows.Forms.Button();
            this.dbp_main = new LogManager.DoubleBufferedPanel();
            this.gbox_log = new System.Windows.Forms.GroupBox();
            this.dpnl_work = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedPanel7 = new LogManager.DoubleBufferedPanel();
            this.dlbl_deletedate = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedPanel8 = new LogManager.DoubleBufferedPanel();
            this.dlbl_delcount = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedPanel9 = new LogManager.DoubleBufferedPanel();
            this.dlbl_count = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedPanel10 = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedLabel13 = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedPanel11 = new LogManager.DoubleBufferedPanel();
            this.gbox_status = new System.Windows.Forms.GroupBox();
            this.dpnl_status = new LogManager.DoubleBufferedPanel();
            this.btn_runstatus = new System.Windows.Forms.Button();
            this.doubleBufferedLabel4 = new LogManager.DoubleBufferedLabel();
            this.btn_enable = new System.Windows.Forms.Button();
            this.dlbl_interval = new LogManager.DoubleBufferedLabel();
            this.dlbl_act = new LogManager.DoubleBufferedLabel();
            this.dlbl_acttime = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedLabel3 = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedLabel2 = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedLabel1 = new LogManager.DoubleBufferedLabel();
            this.gbox_info_db = new System.Windows.Forms.GroupBox();
            this.dpnl_location_db = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedPanel1 = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedLabel5 = new LogManager.DoubleBufferedLabel();
            this.pnl_padding2 = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedLabel6 = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedPanel2 = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedLabel7 = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedPanel3 = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedLabel8 = new LogManager.DoubleBufferedLabel();
            this.dlbl_name = new LogManager.DoubleBufferedLabel();
            this.pnl_padding1 = new LogManager.DoubleBufferedPanel();
            this.gbox_info_file = new System.Windows.Forms.GroupBox();
            this.dpnl_location_log = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedPanel4 = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedLabel10 = new LogManager.DoubleBufferedLabel();
            this.pnl_padding3 = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedLabel11 = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedPanel5 = new LogManager.DoubleBufferedPanel();
            this.doubleBufferedLabel12 = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedLabel9 = new LogManager.DoubleBufferedLabel();
            this.doubleBufferedPanel6 = new LogManager.DoubleBufferedPanel();
            this.dpnl_info = new LogManager.DoubleBufferedPanel();
            this.dlbl_info = new LogManager.DoubleBufferedLabel();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.dbp_main.SuspendLayout();
            this.gbox_log.SuspendLayout();
            this.doubleBufferedPanel7.SuspendLayout();
            this.gbox_status.SuspendLayout();
            this.dpnl_status.SuspendLayout();
            this.gbox_info_db.SuspendLayout();
            this.doubleBufferedPanel1.SuspendLayout();
            this.gbox_info_file.SuspendLayout();
            this.doubleBufferedPanel4.SuspendLayout();
            this.dpnl_info.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer_uichange
            // 
            this.timer_uichange.Tick += new System.EventHandler(this.timer_uichange_Tick);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(435, 716);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(112, 24);
            this.btn_save.TabIndex = 7;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_Click);
            // 
            // timer_checkthread
            // 
            this.timer_checkthread.Tick += new System.EventHandler(this.timer_checkthread_Tick);
            // 
            // btn_return
            // 
            this.btn_return.Location = new System.Drawing.Point(317, 716);
            this.btn_return.Name = "btn_return";
            this.btn_return.Size = new System.Drawing.Size(112, 24);
            this.btn_return.TabIndex = 8;
            this.btn_return.Text = "Setting Load";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(556, 716);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(112, 24);
            this.btn_delete.TabIndex = 9;
            this.btn_delete.Text = "Clear";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Log Manager";
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.programEndToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(105, 48);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.programEndToolStripMenuItem_Click);
            // 
            // programEndToolStripMenuItem
            // 
            this.programEndToolStripMenuItem.Name = "programEndToolStripMenuItem";
            this.programEndToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.programEndToolStripMenuItem.Text = "End";
            this.programEndToolStripMenuItem.Click += new System.EventHandler(this.programEndToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbox_queue_log);
            this.groupBox1.Location = new System.Drawing.Point(553, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 696);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Log View";
            // 
            // lbox_queue_log
            // 
            this.lbox_queue_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbox_queue_log.FormattingEnabled = true;
            this.lbox_queue_log.HorizontalScrollbar = true;
            this.lbox_queue_log.ItemHeight = 12;
            this.lbox_queue_log.Location = new System.Drawing.Point(3, 17);
            this.lbox_queue_log.Name = "lbox_queue_log";
            this.lbox_queue_log.ScrollAlwaysVisible = true;
            this.lbox_queue_log.Size = new System.Drawing.Size(408, 676);
            this.lbox_queue_log.TabIndex = 9;
            // 
            // btn_force_act
            // 
            this.btn_force_act.Location = new System.Drawing.Point(199, 716);
            this.btn_force_act.Name = "btn_force_act";
            this.btn_force_act.Size = new System.Drawing.Size(112, 24);
            this.btn_force_act.TabIndex = 12;
            this.btn_force_act.Text = "Force Act";
            this.btn_force_act.UseVisualStyleBackColor = true;
            this.btn_force_act.Click += new System.EventHandler(this.btn_Click);
            // 
            // dbp_main
            // 
            this.dbp_main.Controls.Add(this.gbox_log);
            this.dbp_main.Controls.Add(this.gbox_status);
            this.dbp_main.Controls.Add(this.gbox_info_db);
            this.dbp_main.Controls.Add(this.gbox_info_file);
            this.dbp_main.Location = new System.Drawing.Point(12, 12);
            this.dbp_main.Name = "dbp_main";
            this.dbp_main.Size = new System.Drawing.Size(535, 696);
            this.dbp_main.TabIndex = 0;
            // 
            // gbox_log
            // 
            this.gbox_log.Controls.Add(this.dpnl_work);
            this.gbox_log.Controls.Add(this.doubleBufferedPanel7);
            this.gbox_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbox_log.Location = new System.Drawing.Point(0, 413);
            this.gbox_log.Name = "gbox_log";
            this.gbox_log.Size = new System.Drawing.Size(535, 283);
            this.gbox_log.TabIndex = 5;
            this.gbox_log.TabStop = false;
            this.gbox_log.Text = "Working Information";
            // 
            // dpnl_work
            // 
            this.dpnl_work.AutoScroll = true;
            this.dpnl_work.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpnl_work.Location = new System.Drawing.Point(3, 38);
            this.dpnl_work.Name = "dpnl_work";
            this.dpnl_work.Size = new System.Drawing.Size(529, 242);
            this.dpnl_work.TabIndex = 3;
            // 
            // doubleBufferedPanel7
            // 
            this.doubleBufferedPanel7.Controls.Add(this.dlbl_deletedate);
            this.doubleBufferedPanel7.Controls.Add(this.doubleBufferedPanel8);
            this.doubleBufferedPanel7.Controls.Add(this.dlbl_delcount);
            this.doubleBufferedPanel7.Controls.Add(this.doubleBufferedPanel9);
            this.doubleBufferedPanel7.Controls.Add(this.dlbl_count);
            this.doubleBufferedPanel7.Controls.Add(this.doubleBufferedPanel10);
            this.doubleBufferedPanel7.Controls.Add(this.doubleBufferedLabel13);
            this.doubleBufferedPanel7.Controls.Add(this.doubleBufferedPanel11);
            this.doubleBufferedPanel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.doubleBufferedPanel7.Location = new System.Drawing.Point(3, 17);
            this.doubleBufferedPanel7.Name = "doubleBufferedPanel7";
            this.doubleBufferedPanel7.Size = new System.Drawing.Size(529, 21);
            this.doubleBufferedPanel7.TabIndex = 2;
            // 
            // dlbl_deletedate
            // 
            this.dlbl_deletedate.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbl_deletedate.Location = new System.Drawing.Point(355, 0);
            this.dlbl_deletedate.Name = "dlbl_deletedate";
            this.dlbl_deletedate.Size = new System.Drawing.Size(147, 21);
            this.dlbl_deletedate.TabIndex = 15;
            this.dlbl_deletedate.Text = "delete date";
            this.dlbl_deletedate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // doubleBufferedPanel8
            // 
            this.doubleBufferedPanel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedPanel8.Location = new System.Drawing.Point(340, 0);
            this.doubleBufferedPanel8.Name = "doubleBufferedPanel8";
            this.doubleBufferedPanel8.Size = new System.Drawing.Size(15, 21);
            this.doubleBufferedPanel8.TabIndex = 14;
            // 
            // dlbl_delcount
            // 
            this.dlbl_delcount.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbl_delcount.Location = new System.Drawing.Point(238, 0);
            this.dlbl_delcount.Name = "dlbl_delcount";
            this.dlbl_delcount.Size = new System.Drawing.Size(102, 21);
            this.dlbl_delcount.TabIndex = 13;
            this.dlbl_delcount.Text = "delete log count";
            this.dlbl_delcount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // doubleBufferedPanel9
            // 
            this.doubleBufferedPanel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedPanel9.Location = new System.Drawing.Point(223, 0);
            this.doubleBufferedPanel9.Name = "doubleBufferedPanel9";
            this.doubleBufferedPanel9.Size = new System.Drawing.Size(15, 21);
            this.doubleBufferedPanel9.TabIndex = 12;
            // 
            // dlbl_count
            // 
            this.dlbl_count.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbl_count.Location = new System.Drawing.Point(118, 0);
            this.dlbl_count.Name = "dlbl_count";
            this.dlbl_count.Size = new System.Drawing.Size(105, 21);
            this.dlbl_count.TabIndex = 11;
            this.dlbl_count.Text = "log count";
            this.dlbl_count.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // doubleBufferedPanel10
            // 
            this.doubleBufferedPanel10.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedPanel10.Location = new System.Drawing.Point(103, 0);
            this.doubleBufferedPanel10.Name = "doubleBufferedPanel10";
            this.doubleBufferedPanel10.Size = new System.Drawing.Size(15, 21);
            this.doubleBufferedPanel10.TabIndex = 8;
            // 
            // doubleBufferedLabel13
            // 
            this.doubleBufferedLabel13.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedLabel13.Location = new System.Drawing.Point(13, 0);
            this.doubleBufferedLabel13.Name = "doubleBufferedLabel13";
            this.doubleBufferedLabel13.Size = new System.Drawing.Size(90, 21);
            this.doubleBufferedLabel13.TabIndex = 5;
            this.doubleBufferedLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // doubleBufferedPanel11
            // 
            this.doubleBufferedPanel11.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedPanel11.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferedPanel11.Name = "doubleBufferedPanel11";
            this.doubleBufferedPanel11.Size = new System.Drawing.Size(13, 21);
            this.doubleBufferedPanel11.TabIndex = 4;
            // 
            // gbox_status
            // 
            this.gbox_status.Controls.Add(this.dpnl_status);
            this.gbox_status.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbox_status.Location = new System.Drawing.Point(0, 306);
            this.gbox_status.Name = "gbox_status";
            this.gbox_status.Size = new System.Drawing.Size(535, 107);
            this.gbox_status.TabIndex = 4;
            this.gbox_status.TabStop = false;
            this.gbox_status.Text = "Status";
            // 
            // dpnl_status
            // 
            this.dpnl_status.AutoScroll = true;
            this.dpnl_status.Controls.Add(this.btn_runstatus);
            this.dpnl_status.Controls.Add(this.doubleBufferedLabel4);
            this.dpnl_status.Controls.Add(this.btn_enable);
            this.dpnl_status.Controls.Add(this.dlbl_interval);
            this.dpnl_status.Controls.Add(this.dlbl_act);
            this.dpnl_status.Controls.Add(this.dlbl_acttime);
            this.dpnl_status.Controls.Add(this.doubleBufferedLabel3);
            this.dpnl_status.Controls.Add(this.doubleBufferedLabel2);
            this.dpnl_status.Controls.Add(this.doubleBufferedLabel1);
            this.dpnl_status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpnl_status.Location = new System.Drawing.Point(3, 17);
            this.dpnl_status.Name = "dpnl_status";
            this.dpnl_status.Size = new System.Drawing.Size(529, 87);
            this.dpnl_status.TabIndex = 1;
            // 
            // btn_runstatus
            // 
            this.btn_runstatus.Location = new System.Drawing.Point(410, 6);
            this.btn_runstatus.Name = "btn_runstatus";
            this.btn_runstatus.Size = new System.Drawing.Size(112, 24);
            this.btn_runstatus.TabIndex = 8;
            this.btn_runstatus.Text = "Run Status";
            this.btn_runstatus.UseVisualStyleBackColor = true;
            this.btn_runstatus.Visible = false;
            this.btn_runstatus.Click += new System.EventHandler(this.btn_runstatus_Click);
            // 
            // doubleBufferedLabel4
            // 
            this.doubleBufferedLabel4.AutoSize = true;
            this.doubleBufferedLabel4.Location = new System.Drawing.Point(128, 60);
            this.doubleBufferedLabel4.Name = "doubleBufferedLabel4";
            this.doubleBufferedLabel4.Size = new System.Drawing.Size(41, 12);
            this.doubleBufferedLabel4.TabIndex = 7;
            this.doubleBufferedLabel4.Text = "(Hour)";
            // 
            // btn_enable
            // 
            this.btn_enable.Location = new System.Drawing.Point(410, 54);
            this.btn_enable.Name = "btn_enable";
            this.btn_enable.Size = new System.Drawing.Size(112, 24);
            this.btn_enable.TabIndex = 6;
            this.btn_enable.Text = "Active Change";
            this.btn_enable.UseVisualStyleBackColor = true;
            this.btn_enable.Click += new System.EventHandler(this.btn_Click);
            // 
            // dlbl_interval
            // 
            this.dlbl_interval.AutoSize = true;
            this.dlbl_interval.Location = new System.Drawing.Point(111, 60);
            this.dlbl_interval.Name = "dlbl_interval";
            this.dlbl_interval.Size = new System.Drawing.Size(11, 12);
            this.dlbl_interval.TabIndex = 5;
            this.dlbl_interval.Text = "0";
            // 
            // dlbl_act
            // 
            this.dlbl_act.AutoSize = true;
            this.dlbl_act.BackColor = System.Drawing.Color.LawnGreen;
            this.dlbl_act.Location = new System.Drawing.Point(111, 36);
            this.dlbl_act.Name = "dlbl_act";
            this.dlbl_act.Size = new System.Drawing.Size(22, 12);
            this.dlbl_act.TabIndex = 4;
            this.dlbl_act.Text = "act";
            // 
            // dlbl_acttime
            // 
            this.dlbl_acttime.AutoSize = true;
            this.dlbl_acttime.Location = new System.Drawing.Point(111, 12);
            this.dlbl_acttime.Name = "dlbl_acttime";
            this.dlbl_acttime.Size = new System.Drawing.Size(143, 12);
            this.dlbl_acttime.TabIndex = 3;
            this.dlbl_acttime.Text = "yyyy-MM-dd hh:mm:ss";
            // 
            // doubleBufferedLabel3
            // 
            this.doubleBufferedLabel3.AutoSize = true;
            this.doubleBufferedLabel3.Location = new System.Drawing.Point(52, 60);
            this.doubleBufferedLabel3.Name = "doubleBufferedLabel3";
            this.doubleBufferedLabel3.Size = new System.Drawing.Size(53, 12);
            this.doubleBufferedLabel3.TabIndex = 2;
            this.doubleBufferedLabel3.Text = "Interval :";
            // 
            // doubleBufferedLabel2
            // 
            this.doubleBufferedLabel2.AutoSize = true;
            this.doubleBufferedLabel2.Location = new System.Drawing.Point(20, 36);
            this.doubleBufferedLabel2.Name = "doubleBufferedLabel2";
            this.doubleBufferedLabel2.Size = new System.Drawing.Size(85, 12);
            this.doubleBufferedLabel2.TabIndex = 1;
            this.doubleBufferedLabel2.Text = "Manager Act :";
            // 
            // doubleBufferedLabel1
            // 
            this.doubleBufferedLabel1.AutoSize = true;
            this.doubleBufferedLabel1.Location = new System.Drawing.Point(13, 12);
            this.doubleBufferedLabel1.Name = "doubleBufferedLabel1";
            this.doubleBufferedLabel1.Size = new System.Drawing.Size(92, 12);
            this.doubleBufferedLabel1.TabIndex = 0;
            this.doubleBufferedLabel1.Text = "Last Act Time :";
            // 
            // gbox_info_db
            // 
            this.gbox_info_db.Controls.Add(this.dpnl_location_db);
            this.gbox_info_db.Controls.Add(this.doubleBufferedPanel1);
            this.gbox_info_db.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbox_info_db.Location = new System.Drawing.Point(0, 153);
            this.gbox_info_db.Name = "gbox_info_db";
            this.gbox_info_db.Size = new System.Drawing.Size(535, 153);
            this.gbox_info_db.TabIndex = 3;
            this.gbox_info_db.TabStop = false;
            this.gbox_info_db.Text = "Target Information - DB";
            // 
            // dpnl_location_db
            // 
            this.dpnl_location_db.AutoScroll = true;
            this.dpnl_location_db.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpnl_location_db.Location = new System.Drawing.Point(3, 38);
            this.dpnl_location_db.Name = "dpnl_location_db";
            this.dpnl_location_db.Size = new System.Drawing.Size(529, 112);
            this.dpnl_location_db.TabIndex = 3;
            // 
            // doubleBufferedPanel1
            // 
            this.doubleBufferedPanel1.Controls.Add(this.doubleBufferedLabel5);
            this.doubleBufferedPanel1.Controls.Add(this.pnl_padding2);
            this.doubleBufferedPanel1.Controls.Add(this.doubleBufferedLabel6);
            this.doubleBufferedPanel1.Controls.Add(this.doubleBufferedPanel2);
            this.doubleBufferedPanel1.Controls.Add(this.doubleBufferedLabel7);
            this.doubleBufferedPanel1.Controls.Add(this.doubleBufferedPanel3);
            this.doubleBufferedPanel1.Controls.Add(this.doubleBufferedLabel8);
            this.doubleBufferedPanel1.Controls.Add(this.dlbl_name);
            this.doubleBufferedPanel1.Controls.Add(this.pnl_padding1);
            this.doubleBufferedPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.doubleBufferedPanel1.Location = new System.Drawing.Point(3, 17);
            this.doubleBufferedPanel1.Name = "doubleBufferedPanel1";
            this.doubleBufferedPanel1.Size = new System.Drawing.Size(529, 21);
            this.doubleBufferedPanel1.TabIndex = 2;
            // 
            // doubleBufferedLabel5
            // 
            this.doubleBufferedLabel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedLabel5.Location = new System.Drawing.Point(458, 0);
            this.doubleBufferedLabel5.Name = "doubleBufferedLabel5";
            this.doubleBufferedLabel5.Size = new System.Drawing.Size(41, 21);
            this.doubleBufferedLabel5.TabIndex = 33;
            this.doubleBufferedLabel5.Text = "day";
            this.doubleBufferedLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_padding2
            // 
            this.pnl_padding2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl_padding2.Location = new System.Drawing.Point(443, 0);
            this.pnl_padding2.Name = "pnl_padding2";
            this.pnl_padding2.Size = new System.Drawing.Size(15, 21);
            this.pnl_padding2.TabIndex = 32;
            // 
            // doubleBufferedLabel6
            // 
            this.doubleBufferedLabel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedLabel6.Location = new System.Drawing.Point(320, 0);
            this.doubleBufferedLabel6.Name = "doubleBufferedLabel6";
            this.doubleBufferedLabel6.Size = new System.Drawing.Size(123, 21);
            this.doubleBufferedLabel6.TabIndex = 31;
            this.doubleBufferedLabel6.Text = "field";
            this.doubleBufferedLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // doubleBufferedPanel2
            // 
            this.doubleBufferedPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedPanel2.Location = new System.Drawing.Point(305, 0);
            this.doubleBufferedPanel2.Name = "doubleBufferedPanel2";
            this.doubleBufferedPanel2.Size = new System.Drawing.Size(15, 21);
            this.doubleBufferedPanel2.TabIndex = 28;
            // 
            // doubleBufferedLabel7
            // 
            this.doubleBufferedLabel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedLabel7.Location = new System.Drawing.Point(192, 0);
            this.doubleBufferedLabel7.Name = "doubleBufferedLabel7";
            this.doubleBufferedLabel7.Size = new System.Drawing.Size(113, 21);
            this.doubleBufferedLabel7.TabIndex = 27;
            this.doubleBufferedLabel7.Text = "table";
            this.doubleBufferedLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // doubleBufferedPanel3
            // 
            this.doubleBufferedPanel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedPanel3.Location = new System.Drawing.Point(177, 0);
            this.doubleBufferedPanel3.Name = "doubleBufferedPanel3";
            this.doubleBufferedPanel3.Size = new System.Drawing.Size(15, 21);
            this.doubleBufferedPanel3.TabIndex = 22;
            // 
            // doubleBufferedLabel8
            // 
            this.doubleBufferedLabel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedLabel8.Location = new System.Drawing.Point(103, 0);
            this.doubleBufferedLabel8.Name = "doubleBufferedLabel8";
            this.doubleBufferedLabel8.Size = new System.Drawing.Size(74, 21);
            this.doubleBufferedLabel8.TabIndex = 21;
            this.doubleBufferedLabel8.Text = "database";
            this.doubleBufferedLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dlbl_name
            // 
            this.dlbl_name.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbl_name.Location = new System.Drawing.Point(4, 0);
            this.dlbl_name.Name = "dlbl_name";
            this.dlbl_name.Size = new System.Drawing.Size(99, 21);
            this.dlbl_name.TabIndex = 5;
            this.dlbl_name.Text = "name";
            this.dlbl_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_padding1
            // 
            this.pnl_padding1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl_padding1.Location = new System.Drawing.Point(0, 0);
            this.pnl_padding1.Name = "pnl_padding1";
            this.pnl_padding1.Size = new System.Drawing.Size(4, 21);
            this.pnl_padding1.TabIndex = 4;
            // 
            // gbox_info_file
            // 
            this.gbox_info_file.Controls.Add(this.dpnl_location_log);
            this.gbox_info_file.Controls.Add(this.doubleBufferedPanel4);
            this.gbox_info_file.Controls.Add(this.dpnl_info);
            this.gbox_info_file.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbox_info_file.Location = new System.Drawing.Point(0, 0);
            this.gbox_info_file.Name = "gbox_info_file";
            this.gbox_info_file.Size = new System.Drawing.Size(535, 153);
            this.gbox_info_file.TabIndex = 0;
            this.gbox_info_file.TabStop = false;
            this.gbox_info_file.Text = "Target Information - File";
            // 
            // dpnl_location_log
            // 
            this.dpnl_location_log.AutoScroll = true;
            this.dpnl_location_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpnl_location_log.Location = new System.Drawing.Point(3, 56);
            this.dpnl_location_log.Name = "dpnl_location_log";
            this.dpnl_location_log.Size = new System.Drawing.Size(529, 94);
            this.dpnl_location_log.TabIndex = 6;
            // 
            // doubleBufferedPanel4
            // 
            this.doubleBufferedPanel4.Controls.Add(this.doubleBufferedLabel10);
            this.doubleBufferedPanel4.Controls.Add(this.pnl_padding3);
            this.doubleBufferedPanel4.Controls.Add(this.doubleBufferedLabel11);
            this.doubleBufferedPanel4.Controls.Add(this.doubleBufferedPanel5);
            this.doubleBufferedPanel4.Controls.Add(this.doubleBufferedLabel12);
            this.doubleBufferedPanel4.Controls.Add(this.doubleBufferedLabel9);
            this.doubleBufferedPanel4.Controls.Add(this.doubleBufferedPanel6);
            this.doubleBufferedPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.doubleBufferedPanel4.Location = new System.Drawing.Point(3, 35);
            this.doubleBufferedPanel4.Name = "doubleBufferedPanel4";
            this.doubleBufferedPanel4.Size = new System.Drawing.Size(529, 21);
            this.doubleBufferedPanel4.TabIndex = 5;
            // 
            // doubleBufferedLabel10
            // 
            this.doubleBufferedLabel10.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedLabel10.Location = new System.Drawing.Point(458, 0);
            this.doubleBufferedLabel10.Name = "doubleBufferedLabel10";
            this.doubleBufferedLabel10.Size = new System.Drawing.Size(41, 21);
            this.doubleBufferedLabel10.TabIndex = 20;
            this.doubleBufferedLabel10.Text = "day";
            this.doubleBufferedLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_padding3
            // 
            this.pnl_padding3.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl_padding3.Location = new System.Drawing.Point(448, 0);
            this.pnl_padding3.Name = "pnl_padding3";
            this.pnl_padding3.Size = new System.Drawing.Size(10, 21);
            this.pnl_padding3.TabIndex = 19;
            // 
            // doubleBufferedLabel11
            // 
            this.doubleBufferedLabel11.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedLabel11.Location = new System.Drawing.Point(401, 0);
            this.doubleBufferedLabel11.Name = "doubleBufferedLabel11";
            this.doubleBufferedLabel11.Size = new System.Drawing.Size(47, 21);
            this.doubleBufferedLabel11.TabIndex = 18;
            this.doubleBufferedLabel11.Text = "change";
            this.doubleBufferedLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // doubleBufferedPanel5
            // 
            this.doubleBufferedPanel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedPanel5.Location = new System.Drawing.Point(391, 0);
            this.doubleBufferedPanel5.Name = "doubleBufferedPanel5";
            this.doubleBufferedPanel5.Size = new System.Drawing.Size(10, 21);
            this.doubleBufferedPanel5.TabIndex = 15;
            // 
            // doubleBufferedLabel12
            // 
            this.doubleBufferedLabel12.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedLabel12.Location = new System.Drawing.Point(103, 0);
            this.doubleBufferedLabel12.Name = "doubleBufferedLabel12";
            this.doubleBufferedLabel12.Size = new System.Drawing.Size(288, 21);
            this.doubleBufferedLabel12.TabIndex = 14;
            this.doubleBufferedLabel12.Text = "path";
            this.doubleBufferedLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // doubleBufferedLabel9
            // 
            this.doubleBufferedLabel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedLabel9.Location = new System.Drawing.Point(13, 0);
            this.doubleBufferedLabel9.Name = "doubleBufferedLabel9";
            this.doubleBufferedLabel9.Size = new System.Drawing.Size(90, 21);
            this.doubleBufferedLabel9.TabIndex = 5;
            this.doubleBufferedLabel9.Text = "name";
            this.doubleBufferedLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // doubleBufferedPanel6
            // 
            this.doubleBufferedPanel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.doubleBufferedPanel6.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferedPanel6.Name = "doubleBufferedPanel6";
            this.doubleBufferedPanel6.Size = new System.Drawing.Size(13, 21);
            this.doubleBufferedPanel6.TabIndex = 4;
            // 
            // dpnl_info
            // 
            this.dpnl_info.Controls.Add(this.dlbl_info);
            this.dpnl_info.Dock = System.Windows.Forms.DockStyle.Top;
            this.dpnl_info.Location = new System.Drawing.Point(3, 17);
            this.dpnl_info.Name = "dpnl_info";
            this.dpnl_info.Size = new System.Drawing.Size(529, 18);
            this.dpnl_info.TabIndex = 4;
            // 
            // dlbl_info
            // 
            this.dlbl_info.AutoSize = true;
            this.dlbl_info.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbl_info.Location = new System.Drawing.Point(0, 0);
            this.dlbl_info.Name = "dlbl_info";
            this.dlbl_info.Size = new System.Drawing.Size(89, 12);
            this.dlbl_info.TabIndex = 0;
            this.dlbl_info.Text = "INFORMATION";
            this.dlbl_info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 749);
            this.Controls.Add(this.btn_force_act);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.btn_return);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.dbp_main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_main_FormClosing);
            this.Load += new System.EventHandler(this.frm_main_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.dbp_main.ResumeLayout(false);
            this.gbox_log.ResumeLayout(false);
            this.doubleBufferedPanel7.ResumeLayout(false);
            this.gbox_status.ResumeLayout(false);
            this.dpnl_status.ResumeLayout(false);
            this.dpnl_status.PerformLayout();
            this.gbox_info_db.ResumeLayout(false);
            this.doubleBufferedPanel1.ResumeLayout(false);
            this.gbox_info_file.ResumeLayout(false);
            this.doubleBufferedPanel4.ResumeLayout(false);
            this.dpnl_info.ResumeLayout(false);
            this.dpnl_info.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel dbp_main;
        private System.Windows.Forms.GroupBox gbox_info_file;
        private System.Windows.Forms.Timer timer_uichange;
        private System.Windows.Forms.GroupBox gbox_log;
        private System.Windows.Forms.GroupBox gbox_status;
        private DoubleBufferedPanel dpnl_status;
        private DoubleBufferedLabel doubleBufferedLabel4;
        private System.Windows.Forms.Button btn_enable;
        private DoubleBufferedLabel dlbl_interval;
        private DoubleBufferedLabel dlbl_act;
        private DoubleBufferedLabel dlbl_acttime;
        private DoubleBufferedLabel doubleBufferedLabel3;
        private DoubleBufferedLabel doubleBufferedLabel2;
        private DoubleBufferedLabel doubleBufferedLabel1;
        private System.Windows.Forms.GroupBox gbox_info_db;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Timer timer_checkthread;
		private DoubleBufferedPanel dpnl_location_db;
		private DoubleBufferedPanel doubleBufferedPanel1;
		private DoubleBufferedLabel doubleBufferedLabel5;
		private DoubleBufferedPanel pnl_padding2;
		private DoubleBufferedLabel doubleBufferedLabel6;
		private DoubleBufferedPanel doubleBufferedPanel2;
		private DoubleBufferedLabel doubleBufferedLabel7;
		private DoubleBufferedPanel doubleBufferedPanel3;
		private DoubleBufferedLabel doubleBufferedLabel8;
		private DoubleBufferedLabel dlbl_name;
		private DoubleBufferedPanel pnl_padding1;
		private System.Windows.Forms.Button btn_return;
		private System.Windows.Forms.Button btn_delete;
		private DoubleBufferedPanel dpnl_work;
		private DoubleBufferedPanel doubleBufferedPanel7;
		private DoubleBufferedLabel dlbl_deletedate;
		private DoubleBufferedPanel doubleBufferedPanel8;
		private DoubleBufferedLabel dlbl_delcount;
		private DoubleBufferedPanel doubleBufferedPanel9;
		private DoubleBufferedLabel dlbl_count;
		private DoubleBufferedPanel doubleBufferedPanel10;
		private DoubleBufferedLabel doubleBufferedLabel13;
		private DoubleBufferedPanel doubleBufferedPanel11;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem programEndToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
		private DoubleBufferedPanel dpnl_location_log;
		private DoubleBufferedPanel doubleBufferedPanel4;
		private DoubleBufferedLabel doubleBufferedLabel10;
		private DoubleBufferedPanel pnl_padding3;
		private DoubleBufferedLabel doubleBufferedLabel11;
		private DoubleBufferedPanel doubleBufferedPanel5;
		private DoubleBufferedLabel doubleBufferedLabel12;
		private DoubleBufferedLabel doubleBufferedLabel9;
		private DoubleBufferedPanel doubleBufferedPanel6;
		private DoubleBufferedPanel dpnl_info;
		private DoubleBufferedLabel dlbl_info;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbox_queue_log;
        private System.Windows.Forms.Button btn_force_act;
        private System.Windows.Forms.Button btn_runstatus;
    }
}

