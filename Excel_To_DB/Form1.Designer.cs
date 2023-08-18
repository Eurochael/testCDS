namespace Excel_To_DB
{
    partial class Form1
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
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.spinedit_row_maxcnt = new DevExpress.XtraEditors.SpinEdit();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.spinedit_sheetcnt = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.lbox_item = new System.Windows.Forms.ListBox();
            this.spreadsheetControl1 = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chk_selected_item_act = new System.Windows.Forms.CheckBox();
            this.chk_allupdate = new System.Windows.Forms.CheckBox();
            this.txt_db_pass = new System.Windows.Forms.TextBox();
            this.txt_db_ip = new System.Windows.Forms.TextBox();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.chk_update = new System.Windows.Forms.CheckBox();
            this.chk_all_delete = new System.Windows.Forms.CheckBox();
            this.chk_comment = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.spinedit_row_maxcnt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinedit_sheetcnt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // simpleButton6
            // 
            this.simpleButton6.Location = new System.Drawing.Point(1060, 53);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(201, 41);
            this.simpleButton6.TabIndex = 21;
            this.simpleButton6.Text = "sheet To DB(para)";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // spinedit_row_maxcnt
            // 
            this.spinedit_row_maxcnt.EditValue = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.spinedit_row_maxcnt.Location = new System.Drawing.Point(526, 53);
            this.spinedit_row_maxcnt.Name = "spinedit_row_maxcnt";
            this.spinedit_row_maxcnt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinedit_row_maxcnt.Size = new System.Drawing.Size(100, 20);
            this.spinedit_row_maxcnt.TabIndex = 20;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(1060, 6);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(201, 41);
            this.simpleButton4.TabIndex = 15;
            this.simpleButton4.Text = "sheet To DB(Alarm)";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(457, 53);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(69, 14);
            this.labelControl2.TabIndex = 19;
            this.labelControl2.Text = "row count : ";
            // 
            // spinedit_sheetcnt
            // 
            this.spinedit_sheetcnt.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinedit_sheetcnt.Location = new System.Drawing.Point(526, 27);
            this.spinedit_sheetcnt.Name = "spinedit_sheetcnt";
            this.spinedit_sheetcnt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinedit_sheetcnt.Size = new System.Drawing.Size(100, 20);
            this.spinedit_sheetcnt.TabIndex = 18;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(447, 30);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(79, 14);
            this.labelControl1.TabIndex = 17;
            this.labelControl1.Text = "sheet count : ";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.lbox_item);
            this.groupControl2.Controls.Add(this.spreadsheetControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1264, 649);
            this.groupControl2.TabIndex = 16;
            this.groupControl2.Text = "excel To Database";
            // 
            // lbox_item
            // 
            this.lbox_item.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbox_item.FormattingEnabled = true;
            this.lbox_item.ItemHeight = 14;
            this.lbox_item.Location = new System.Drawing.Point(1131, 23);
            this.lbox_item.Name = "lbox_item";
            this.lbox_item.Size = new System.Drawing.Size(131, 624);
            this.lbox_item.TabIndex = 7;
            // 
            // spreadsheetControl1
            // 
            this.spreadsheetControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.spreadsheetControl1.Location = new System.Drawing.Point(2, 23);
            this.spreadsheetControl1.Name = "spreadsheetControl1";
            this.spreadsheetControl1.Size = new System.Drawing.Size(1129, 624);
            this.spreadsheetControl1.TabIndex = 6;
            this.spreadsheetControl1.Text = "spreadsheetControl1";
            // 
            // simpleButton5
            // 
            this.simpleButton5.Dock = System.Windows.Forms.DockStyle.Left;
            this.simpleButton5.Location = new System.Drawing.Point(0, 0);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(224, 100);
            this.simpleButton5.TabIndex = 5;
            this.simpleButton5.Text = "Load Excel(Para OR Alarm)";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chk_comment);
            this.panel1.Controls.Add(this.chk_selected_item_act);
            this.panel1.Controls.Add(this.chk_allupdate);
            this.panel1.Controls.Add(this.txt_db_pass);
            this.panel1.Controls.Add(this.txt_db_ip);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.chk_update);
            this.panel1.Controls.Add(this.chk_all_delete);
            this.panel1.Controls.Add(this.simpleButton5);
            this.panel1.Controls.Add(this.simpleButton6);
            this.panel1.Controls.Add(this.simpleButton4);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.spinedit_row_maxcnt);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.spinedit_sheetcnt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 649);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1264, 100);
            this.panel1.TabIndex = 22;
            // 
            // chk_selected_item_act
            // 
            this.chk_selected_item_act.AutoSize = true;
            this.chk_selected_item_act.Location = new System.Drawing.Point(679, 73);
            this.chk_selected_item_act.Name = "chk_selected_item_act";
            this.chk_selected_item_act.Size = new System.Drawing.Size(196, 16);
            this.chk_selected_item_act.TabIndex = 29;
            this.chk_selected_item_act.Text = "Selected Item Insert Or Update";
            this.chk_selected_item_act.UseVisualStyleBackColor = true;
            this.chk_selected_item_act.CheckedChanged += new System.EventHandler(this.chk_selected_item_act_CheckedChanged);
            // 
            // chk_allupdate
            // 
            this.chk_allupdate.AutoSize = true;
            this.chk_allupdate.Location = new System.Drawing.Point(679, 51);
            this.chk_allupdate.Name = "chk_allupdate";
            this.chk_allupdate.Size = new System.Drawing.Size(154, 16);
            this.chk_allupdate.TabIndex = 28;
            this.chk_allupdate.Text = "DB ALL Update By No?";
            this.chk_allupdate.UseVisualStyleBackColor = true;
            // 
            // txt_db_pass
            // 
            this.txt_db_pass.Location = new System.Drawing.Point(296, 30);
            this.txt_db_pass.Name = "txt_db_pass";
            this.txt_db_pass.Size = new System.Drawing.Size(135, 21);
            this.txt_db_pass.TabIndex = 27;
            this.txt_db_pass.Text = "1234";
            // 
            // txt_db_ip
            // 
            this.txt_db_ip.Location = new System.Drawing.Point(296, 3);
            this.txt_db_ip.Name = "txt_db_ip";
            this.txt_db_ip.Size = new System.Drawing.Size(135, 21);
            this.txt_db_ip.TabIndex = 26;
            this.txt_db_ip.Text = "127.0.0.1";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(248, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(42, 14);
            this.labelControl3.TabIndex = 24;
            this.labelControl3.Text = "DB IP : ";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(230, 31);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 25;
            this.labelControl4.Text = "DB PASS : ";
            // 
            // chk_update
            // 
            this.chk_update.AutoSize = true;
            this.chk_update.Location = new System.Drawing.Point(679, 29);
            this.chk_update.Name = "chk_update";
            this.chk_update.Size = new System.Drawing.Size(223, 16);
            this.chk_update.TabIndex = 23;
            this.chk_update.Text = "DB Name,comment Update By No?";
            this.chk_update.UseVisualStyleBackColor = true;
            // 
            // chk_all_delete
            // 
            this.chk_all_delete.AutoSize = true;
            this.chk_all_delete.Location = new System.Drawing.Point(679, 8);
            this.chk_all_delete.Name = "chk_all_delete";
            this.chk_all_delete.Size = new System.Drawing.Size(105, 16);
            this.chk_all_delete.TabIndex = 22;
            this.chk_all_delete.Text = "DB ALL Delete";
            this.chk_all_delete.UseVisualStyleBackColor = true;
            // 
            // chk_comment
            // 
            this.chk_comment.AutoSize = true;
            this.chk_comment.Location = new System.Drawing.Point(921, 8);
            this.chk_comment.Name = "chk_comment";
            this.chk_comment.Size = new System.Drawing.Size(122, 16);
            this.chk_comment.TabIndex = 30;
            this.chk_comment.Text = "Comment Update";
            this.chk_comment.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 750);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupControl2);
            this.Name = "Form1";
            this.Text = "Alarm, Parameter Insert";
            ((System.ComponentModel.ISupportInitialize)(this.spinedit_row_maxcnt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinedit_sheetcnt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SpinEdit spinedit_row_maxcnt;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit spinedit_sheetcnt;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraSpreadsheet.SpreadsheetControl spreadsheetControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chk_update;
        private System.Windows.Forms.CheckBox chk_all_delete;
        private System.Windows.Forms.TextBox txt_db_pass;
        private System.Windows.Forms.TextBox txt_db_ip;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.CheckBox chk_allupdate;
        private System.Windows.Forms.ListBox lbox_item;
        private System.Windows.Forms.CheckBox chk_selected_item_act;
        private System.Windows.Forms.CheckBox chk_comment;
    }
}

