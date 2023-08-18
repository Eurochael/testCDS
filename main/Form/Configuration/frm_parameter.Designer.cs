
namespace cds
{
    partial class frm_parameter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_parameter));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txt_max = new DevExpress.XtraEditors.TextEdit();
            this.txt_set = new DevExpress.XtraEditors.TextEdit();
            this.txt_min = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.btn_hostsend = new DevExpress.XtraEditors.ToggleSwitch();
            this.txt_comment = new DevExpress.XtraEditors.MemoEdit();
            this.txt_name = new DevExpress.XtraEditors.TextEdit();
            this.txt_id = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.btn_clear = new DevExpress.XtraEditors.SimpleButton();
            this.txt_token = new DevExpress.XtraEditors.TokenEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Export = new DevExpress.XtraEditors.SimpleButton();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_change = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gp_parameter_config = new DevExpress.XtraEditors.GroupControl();
            this.grid_parameter_config = new DevExpress.XtraGrid.GridControl();
            this.gridview_parameterconfig = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnl_body = new cds.DoubleBufferedPanel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer_change_check = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_max.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_set.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_min.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_hostsend.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_comment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_token.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gp_parameter_config)).BeginInit();
            this.gp_parameter_config.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_parameter_config)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridview_parameterconfig)).BeginInit();
            this.pnl_body.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txt_max);
            this.panelControl1.Controls.Add(this.txt_set);
            this.panelControl1.Controls.Add(this.txt_min);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelControl7);
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.btn_hostsend);
            this.panelControl1.Controls.Add(this.txt_comment);
            this.panelControl1.Controls.Add(this.txt_name);
            this.panelControl1.Controls.Add(this.txt_id);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.btn_search);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.btn_clear);
            this.panelControl1.Controls.Add(this.txt_token);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl1.Location = new System.Drawing.Point(1060, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelControl1.Size = new System.Drawing.Size(220, 890);
            this.panelControl1.TabIndex = 17;
            // 
            // txt_max
            // 
            this.txt_max.Location = new System.Drawing.Point(85, 284);
            this.txt_max.Name = "txt_max";
            this.txt_max.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txt_max.Properties.MaskSettings.Set("mask", "d");
            this.txt_max.Properties.UseMaskAsDisplayFormat = true;
            this.txt_max.Size = new System.Drawing.Size(118, 20);
            this.txt_max.TabIndex = 52;
            // 
            // txt_set
            // 
            this.txt_set.Location = new System.Drawing.Point(85, 255);
            this.txt_set.Name = "txt_set";
            this.txt_set.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txt_set.Properties.MaskSettings.Set("mask", "f0");
            this.txt_set.Properties.UseMaskAsDisplayFormat = true;
            this.txt_set.Size = new System.Drawing.Size(118, 20);
            this.txt_set.TabIndex = 51;
            // 
            // txt_min
            // 
            this.txt_min.Location = new System.Drawing.Point(85, 226);
            this.txt_min.Name = "txt_min";
            this.txt_min.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txt_min.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
            this.txt_min.Properties.MaskSettings.Set("mask", "d");
            this.txt_min.Size = new System.Drawing.Size(118, 20);
            this.txt_min.TabIndex = 50;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Options.UseTextOptions = true;
            this.labelControl5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl5.Location = new System.Drawing.Point(2, 281);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(77, 23);
            this.labelControl5.TabIndex = 49;
            this.labelControl5.Text = "Max : ";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Options.UseTextOptions = true;
            this.labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.Location = new System.Drawing.Point(2, 252);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(77, 23);
            this.labelControl4.TabIndex = 48;
            this.labelControl4.Text = "Set : ";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Options.UseTextOptions = true;
            this.labelControl7.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl7.Location = new System.Drawing.Point(2, 223);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(77, 23);
            this.labelControl7.TabIndex = 47;
            this.labelControl7.Text = "Min : ";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Options.UseTextOptions = true;
            this.labelControl8.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl8.Location = new System.Drawing.Point(2, 314);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(77, 23);
            this.labelControl8.TabIndex = 43;
            this.labelControl8.Text = "Host Send : ";
            // 
            // btn_hostsend
            // 
            this.btn_hostsend.Enabled = false;
            this.btn_hostsend.Location = new System.Drawing.Point(85, 314);
            this.btn_hostsend.Name = "btn_hostsend";
            this.btn_hostsend.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btn_hostsend.Properties.Appearance.Options.UseFont = true;
            this.btn_hostsend.Properties.OffText = "Off";
            this.btn_hostsend.Properties.OnText = "On";
            this.btn_hostsend.Properties.ShowText = false;
            this.btn_hostsend.Size = new System.Drawing.Size(71, 24);
            this.btn_hostsend.TabIndex = 42;
            // 
            // txt_comment
            // 
            this.txt_comment.Location = new System.Drawing.Point(24, 369);
            this.txt_comment.Name = "txt_comment";
            this.txt_comment.Size = new System.Drawing.Size(179, 180);
            this.txt_comment.TabIndex = 39;
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(85, 200);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(118, 20);
            this.txt_name.TabIndex = 36;
            // 
            // txt_id
            // 
            this.txt_id.Enabled = false;
            this.txt_id.Location = new System.Drawing.Point(85, 171);
            this.txt_id.Name = "txt_id";
            this.txt_id.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txt_id.Properties.MaskSettings.Set("mask", "d");
            this.txt_id.Properties.UseMaskAsDisplayFormat = true;
            this.txt_id.Size = new System.Drawing.Size(118, 20);
            this.txt_id.TabIndex = 35;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Options.UseTextOptions = true;
            this.labelControl6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl6.Location = new System.Drawing.Point(2, 341);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(77, 23);
            this.labelControl6.TabIndex = 34;
            this.labelControl6.Text = "Comment : ";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Options.UseTextOptions = true;
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(2, 197);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(77, 23);
            this.labelControl3.TabIndex = 31;
            this.labelControl3.Text = "Name : ";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(2, 168);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(77, 23);
            this.labelControl2.TabIndex = 30;
            this.labelControl2.Text = "ID : ";
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.Location = new System.Drawing.Point(6, 129);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(98, 35);
            this.btn_search.TabIndex = 29;
            this.btn_search.Text = "Search";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Options.UseTextOptions = true;
            this.labelControl9.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControl9.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl9.Location = new System.Drawing.Point(6, 75);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(197, 23);
            this.labelControl9.TabIndex = 28;
            this.labelControl9.Text = "Search Option( Token : \',\')";
            // 
            // btn_clear
            // 
            this.btn_clear.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_clear.ImageOptions.Image")));
            this.btn_clear.Location = new System.Drawing.Point(105, 129);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(98, 35);
            this.btn_clear.TabIndex = 27;
            this.btn_clear.Text = "Clear";
            this.btn_clear.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // txt_token
            // 
            this.txt_token.Location = new System.Drawing.Point(6, 101);
            this.txt_token.Name = "txt_token";
            this.txt_token.Properties.AutoHeightMode = DevExpress.XtraEditors.TokenEditAutoHeightMode.RestrictedExpand;
            this.txt_token.Properties.DropDownShowMode = DevExpress.XtraEditors.TokenEditDropDownShowMode.Regular;
            this.txt_token.Properties.EditMode = DevExpress.XtraEditors.TokenEditMode.Manual;
            this.txt_token.Properties.MaxExpandLines = 6;
            this.txt_token.Properties.Separators.AddRange(new string[] {
            ","});
            this.txt_token.Properties.EditValueChanged += new System.EventHandler(this.txt_token_Properties_EditValueChanged);
            this.txt_token.Size = new System.Drawing.Size(197, 20);
            this.txt_token.TabIndex = 26;
            this.txt_token.ValidateToken += new DevExpress.XtraEditors.TokenEditValidateTokenEventHandler(this.txt_token_ValidateToken);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btn_Export);
            this.panelControl2.Controls.Add(this.btn_cancel);
            this.panelControl2.Controls.Add(this.btn_change);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(2, 754);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(216, 134);
            this.panelControl2.TabIndex = 18;
            // 
            // btn_Export
            // 
            this.btn_Export.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Export.ImageOptions.Image")));
            this.btn_Export.Location = new System.Drawing.Point(6, 69);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(97, 60);
            this.btn_Export.TabIndex = 6;
            this.btn_Export.Text = "Export";
            this.btn_Export.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.Location = new System.Drawing.Point(6, 5);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(97, 60);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_change
            // 
            this.btn_change.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_change.ImageOptions.Image")));
            this.btn_change.Location = new System.Drawing.Point(113, 5);
            this.btn_change.Name = "btn_change";
            this.btn_change.Size = new System.Drawing.Size(97, 60);
            this.btn_change.TabIndex = 4;
            this.btn_change.Text = "Change";
            this.btn_change.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl1.Location = new System.Drawing.Point(2, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(216, 33);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Parameter Config";
            // 
            // gp_parameter_config
            // 
            this.gp_parameter_config.Controls.Add(this.grid_parameter_config);
            this.gp_parameter_config.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_parameter_config.Location = new System.Drawing.Point(20, 20);
            this.gp_parameter_config.Name = "gp_parameter_config";
            this.gp_parameter_config.Size = new System.Drawing.Size(1020, 850);
            this.gp_parameter_config.TabIndex = 18;
            this.gp_parameter_config.Text = "alarm search result = 0";
            // 
            // grid_parameter_config
            // 
            this.grid_parameter_config.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_parameter_config.Location = new System.Drawing.Point(2, 23);
            this.grid_parameter_config.MainView = this.gridview_parameterconfig;
            this.grid_parameter_config.Name = "grid_parameter_config";
            this.grid_parameter_config.Size = new System.Drawing.Size(1016, 825);
            this.grid_parameter_config.TabIndex = 1;
            this.grid_parameter_config.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridview_parameterconfig});
            // 
            // gridview_parameterconfig
            // 
            this.gridview_parameterconfig.GridControl = this.grid_parameter_config;
            this.gridview_parameterconfig.Name = "gridview_parameterconfig";
            this.gridview_parameterconfig.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridview_alarmconfig_FocusedRowChanged);
            this.gridview_parameterconfig.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.gridview_parameterconfig_CustomRowFilter);
            // 
            // pnl_body
            // 
            this.pnl_body.Controls.Add(this.gp_parameter_config);
            this.pnl_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_body.Location = new System.Drawing.Point(0, 0);
            this.pnl_body.Name = "pnl_body";
            this.pnl_body.Padding = new System.Windows.Forms.Padding(20);
            this.pnl_body.Size = new System.Drawing.Size(1060, 890);
            this.pnl_body.TabIndex = 19;
            // 
            // timer_change_check
            // 
            this.timer_change_check.Tick += new System.EventHandler(this.timer_change_check_Tick);
            // 
            // frm_parameter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 890);
            this.Controls.Add(this.pnl_body);
            this.Controls.Add(this.panelControl1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_parameter";
            this.Text = "frm_mixing_step";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_parameter_FormClosed);
            this.Load += new System.EventHandler(this.frm_parameter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_max.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_set.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_min.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_hostsend.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_comment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_token.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gp_parameter_config)).EndInit();
            this.gp_parameter_config.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_parameter_config)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridview_parameterconfig)).EndInit();
            this.pnl_body.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl gp_parameter_config;
        private DevExpress.XtraGrid.GridControl grid_parameter_config;
        private DevExpress.XtraGrid.Views.Grid.GridView gridview_parameterconfig;
        private DoubleBufferedPanel pnl_body;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer_change_check;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        public DevExpress.XtraEditors.ToggleSwitch btn_hostsend;
        private DevExpress.XtraEditors.MemoEdit txt_comment;
        private DevExpress.XtraEditors.TextEdit txt_name;
        private DevExpress.XtraEditors.TextEdit txt_id;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton btn_clear;
        private DevExpress.XtraEditors.TokenEdit txt_token;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_change;
        private DevExpress.XtraEditors.TextEdit txt_max;
        private DevExpress.XtraEditors.TextEdit txt_set;
        private DevExpress.XtraEditors.TextEdit txt_min;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SimpleButton btn_Export;
    }
}