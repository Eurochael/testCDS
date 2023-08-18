
namespace cds
{
    partial class frm_io_monitor
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_io_monitor));
            this.gp_ao = new DevExpress.XtraEditors.GroupControl();
            this.grid_ao = new DevExpress.XtraGrid.GridControl();
            this.view_ao = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gp_do = new DevExpress.XtraEditors.GroupControl();
            this.grid_do = new DevExpress.XtraGrid.GridControl();
            this.view_do = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gp_ai = new DevExpress.XtraEditors.GroupControl();
            this.grid_ai = new DevExpress.XtraGrid.GridControl();
            this.view_ai = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gp_di = new DevExpress.XtraEditors.GroupControl();
            this.grid_di = new DevExpress.XtraGrid.GridControl();
            this.view_di = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnl_slider = new DevExpress.XtraEditors.PanelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btn_auto_tunning_gain = new DevExpress.XtraEditors.ToggleSwitch();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btn_view_raw = new DevExpress.XtraEditors.ToggleSwitch();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_change = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.timer_ui_change = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gp_ao)).BeginInit();
            this.gp_ao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_ao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.view_ao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gp_do)).BeginInit();
            this.gp_do.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_do)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.view_do)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gp_ai)).BeginInit();
            this.gp_ai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_ai)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.view_ai)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gp_di)).BeginInit();
            this.gp_di.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_di)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.view_di)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_slider)).BeginInit();
            this.pnl_slider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_auto_tunning_gain.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_view_raw.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gp_ao
            // 
            this.gp_ao.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.gp_ao.Appearance.Options.UseFont = true;
            this.gp_ao.Appearance.Options.UseTextOptions = true;
            this.gp_ao.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gp_ao.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 14F);
            this.gp_ao.AppearanceCaption.Options.UseFont = true;
            this.gp_ao.AppearanceCaption.Options.UseTextOptions = true;
            this.gp_ao.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gp_ao.Controls.Add(this.grid_ao);
            this.gp_ao.Location = new System.Drawing.Point(12, 668);
            this.gp_ao.Name = "gp_ao";
            this.gp_ao.Size = new System.Drawing.Size(1040, 210);
            this.gp_ao.TabIndex = 15;
            this.gp_ao.Text = "Analog Output";
            // 
            // grid_ao
            // 
            this.grid_ao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_ao.Location = new System.Drawing.Point(2, 24);
            this.grid_ao.MainView = this.view_ao;
            this.grid_ao.Name = "grid_ao";
            this.grid_ao.Size = new System.Drawing.Size(1036, 184);
            this.grid_ao.TabIndex = 2;
            this.grid_ao.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.view_ao});
            // 
            // view_ao
            // 
            this.view_ao.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.view_ao.GridControl = this.grid_ao;
            this.view_ao.Name = "view_ao";
            this.view_ao.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.view_ao.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.view_ao.OptionsView.ShowGroupPanel = false;
            this.view_ao.OptionsView.ShowIndicator = false;
            this.view_ao.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.view_ao.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.view_ao_CustomRowFilter);
            this.view_ao.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.view_di_MouseWheel);
            this.view_ao.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.view_ao_ValidatingEditor);
            // 
            // gp_do
            // 
            this.gp_do.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.gp_do.Appearance.Options.UseFont = true;
            this.gp_do.Appearance.Options.UseTextOptions = true;
            this.gp_do.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gp_do.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 14F);
            this.gp_do.AppearanceCaption.Options.UseFont = true;
            this.gp_do.AppearanceCaption.Options.UseTextOptions = true;
            this.gp_do.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gp_do.Controls.Add(this.grid_do);
            this.gp_do.Location = new System.Drawing.Point(12, 232);
            this.gp_do.Name = "gp_do";
            this.gp_do.Size = new System.Drawing.Size(1040, 210);
            this.gp_do.TabIndex = 14;
            this.gp_do.Text = "Digital Output";
            // 
            // grid_do
            // 
            this.grid_do.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_do.Location = new System.Drawing.Point(2, 24);
            this.grid_do.MainView = this.view_do;
            this.grid_do.Name = "grid_do";
            this.grid_do.Size = new System.Drawing.Size(1036, 184);
            this.grid_do.TabIndex = 2;
            this.grid_do.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.view_do});
            // 
            // view_do
            // 
            this.view_do.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.view_do.GridControl = this.grid_do;
            this.view_do.Name = "view_do";
            this.view_do.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.view_do.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.view_do.OptionsView.ShowGroupPanel = false;
            this.view_do.OptionsView.ShowIndicator = false;
            this.view_do.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.view_do.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.view_do_CustomRowFilter);
            this.view_do.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.view_di_MouseWheel);
            this.view_do.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.view_do_ValidatingEditor);
            // 
            // gp_ai
            // 
            this.gp_ai.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.gp_ai.Appearance.Options.UseFont = true;
            this.gp_ai.Appearance.Options.UseTextOptions = true;
            this.gp_ai.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gp_ai.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 14F);
            this.gp_ai.AppearanceCaption.Options.UseFont = true;
            this.gp_ai.AppearanceCaption.Options.UseTextOptions = true;
            this.gp_ai.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gp_ai.Controls.Add(this.grid_ai);
            this.gp_ai.Location = new System.Drawing.Point(12, 450);
            this.gp_ai.Name = "gp_ai";
            this.gp_ai.Size = new System.Drawing.Size(1040, 210);
            this.gp_ai.TabIndex = 12;
            this.gp_ai.Text = "Analog Input";
            // 
            // grid_ai
            // 
            this.grid_ai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_ai.Location = new System.Drawing.Point(2, 24);
            this.grid_ai.MainView = this.view_ai;
            this.grid_ai.Name = "grid_ai";
            this.grid_ai.Size = new System.Drawing.Size(1036, 184);
            this.grid_ai.TabIndex = 2;
            this.grid_ai.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.view_ai});
            // 
            // view_ai
            // 
            this.view_ai.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.view_ai.GridControl = this.grid_ai;
            this.view_ai.Name = "view_ai";
            this.view_ai.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.view_ai.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.view_ai.OptionsView.ShowGroupPanel = false;
            this.view_ai.OptionsView.ShowIndicator = false;
            this.view_ai.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.view_ai.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.view_ai_CustomRowFilter);
            this.view_ai.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.view_ai_CustomColumnDisplayText);
            this.view_ai.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.view_di_MouseWheel);
            this.view_ai.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.view_ai_ValidatingEditor);
            // 
            // gp_di
            // 
            this.gp_di.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.gp_di.Appearance.Options.UseFont = true;
            this.gp_di.Appearance.Options.UseTextOptions = true;
            this.gp_di.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gp_di.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 14F);
            this.gp_di.AppearanceCaption.Options.UseFont = true;
            this.gp_di.AppearanceCaption.Options.UseTextOptions = true;
            this.gp_di.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gp_di.Controls.Add(this.grid_di);
            this.gp_di.Location = new System.Drawing.Point(12, 14);
            this.gp_di.Name = "gp_di";
            this.gp_di.Size = new System.Drawing.Size(1040, 210);
            this.gp_di.TabIndex = 11;
            this.gp_di.Text = "Digital Input";
            // 
            // grid_di
            // 
            this.grid_di.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.grid_di.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.grid_di.Location = new System.Drawing.Point(2, 24);
            this.grid_di.MainView = this.view_di;
            this.grid_di.Name = "grid_di";
            this.grid_di.Size = new System.Drawing.Size(1036, 184);
            this.grid_di.TabIndex = 2;
            this.grid_di.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.view_di});
            // 
            // view_di
            // 
            this.view_di.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.view_di.GridControl = this.grid_di;
            this.view_di.Name = "view_di";
            this.view_di.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.view_di.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.view_di.OptionsView.ShowGroupPanel = false;
            this.view_di.OptionsView.ShowIndicator = false;
            this.view_di.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.view_di.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.view_di_CustomRowFilter);
            this.view_di.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.view_di_MouseWheel);
            this.view_di.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.view_di_ValidatingEditor);
            // 
            // pnl_slider
            // 
            this.pnl_slider.Controls.Add(this.labelControl2);
            this.pnl_slider.Controls.Add(this.btn_auto_tunning_gain);
            this.pnl_slider.Controls.Add(this.labelControl3);
            this.pnl_slider.Controls.Add(this.btn_view_raw);
            this.pnl_slider.Controls.Add(this.panelControl1);
            this.pnl_slider.Controls.Add(this.labelControl1);
            this.pnl_slider.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnl_slider.Location = new System.Drawing.Point(1060, 0);
            this.pnl_slider.Name = "pnl_slider";
            this.pnl_slider.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnl_slider.Size = new System.Drawing.Size(220, 890);
            this.pnl_slider.TabIndex = 16;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(12, 746);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(106, 27);
            this.labelControl2.TabIndex = 33;
            this.labelControl2.Text = "Auto Tunning : ";
            // 
            // btn_auto_tunning_gain
            // 
            this.btn_auto_tunning_gain.Location = new System.Drawing.Point(124, 744);
            this.btn_auto_tunning_gain.Name = "btn_auto_tunning_gain";
            this.btn_auto_tunning_gain.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.btn_auto_tunning_gain.Properties.Appearance.Options.UseFont = true;
            this.btn_auto_tunning_gain.Properties.OffText = "Auto Send";
            this.btn_auto_tunning_gain.Properties.OnText = "Auto Send";
            this.btn_auto_tunning_gain.Size = new System.Drawing.Size(87, 29);
            this.btn_auto_tunning_gain.TabIndex = 32;
            this.btn_auto_tunning_gain.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.btn_auto_tunning_gain_EditValueChanging);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Options.UseTextOptions = true;
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(12, 781);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(106, 27);
            this.labelControl3.TabIndex = 31;
            this.labelControl3.Text = "view raw data : ";
            // 
            // btn_view_raw
            // 
            this.btn_view_raw.Location = new System.Drawing.Point(124, 779);
            this.btn_view_raw.Name = "btn_view_raw";
            this.btn_view_raw.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.btn_view_raw.Properties.Appearance.Options.UseFont = true;
            this.btn_view_raw.Properties.OffText = "Auto Send";
            this.btn_view_raw.Properties.OnText = "Auto Send";
            this.btn_view_raw.Size = new System.Drawing.Size(87, 29);
            this.btn_view_raw.TabIndex = 30;
            this.btn_view_raw.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.btn_view_raw_EditValueChanging);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_cancel);
            this.panelControl1.Controls.Add(this.btn_change);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(2, 818);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(216, 70);
            this.panelControl1.TabIndex = 19;
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.Location = new System.Drawing.Point(5, 7);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(97, 60);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_change
            // 
            this.btn_change.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_change.ImageOptions.Image")));
            this.btn_change.Location = new System.Drawing.Point(112, 7);
            this.btn_change.Name = "btn_change";
            this.btn_change.Size = new System.Drawing.Size(97, 60);
            this.btn_change.TabIndex = 4;
            this.btn_change.Text = "Change";
            this.btn_change.Click += new System.EventHandler(this.btn_cancel_Click);
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
            this.labelControl1.Text = "IO - Monitor";
            // 
            // timer_ui_change
            // 
            this.timer_ui_change.Tick += new System.EventHandler(this.timer_ui_change_Tick);
            // 
            // frm_io_monitor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 890);
            this.Controls.Add(this.pnl_slider);
            this.Controls.Add(this.gp_ao);
            this.Controls.Add(this.gp_do);
            this.Controls.Add(this.gp_ai);
            this.Controls.Add(this.gp_di);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_io_monitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_io_monitor";
            ((System.ComponentModel.ISupportInitialize)(this.gp_ao)).EndInit();
            this.gp_ao.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_ao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.view_ao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gp_do)).EndInit();
            this.gp_do.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_do)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.view_do)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gp_ai)).EndInit();
            this.gp_ai.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_ai)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.view_ai)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gp_di)).EndInit();
            this.gp_di.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_di)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.view_di)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_slider)).EndInit();
            this.pnl_slider.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btn_auto_tunning_gain.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_view_raw.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gp_ao;
        private DevExpress.XtraGrid.GridControl grid_ao;
        private DevExpress.XtraEditors.GroupControl gp_do;
        private DevExpress.XtraGrid.GridControl grid_do;
        private DevExpress.XtraEditors.GroupControl gp_ai;
        private DevExpress.XtraGrid.GridControl grid_ai;
        private DevExpress.XtraEditors.GroupControl gp_di;
        private DevExpress.XtraGrid.GridControl grid_di;
        private DevExpress.XtraEditors.PanelControl pnl_slider;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_change;
        private System.Windows.Forms.Timer timer_ui_change;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ToggleSwitch btn_view_raw;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ToggleSwitch btn_auto_tunning_gain;
        public DevExpress.XtraGrid.Views.Grid.GridView view_ao;
        public DevExpress.XtraGrid.Views.Grid.GridView view_do;
        public DevExpress.XtraGrid.Views.Grid.GridView view_ai;
        public DevExpress.XtraGrid.Views.Grid.GridView view_di;
    }
}