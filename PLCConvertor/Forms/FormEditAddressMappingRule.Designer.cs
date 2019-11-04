namespace PLCConvertor.Forms
{
    partial class FormEditAddressMappingRule
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
            DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer dockingContainer4 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer();
            this.documentGroup1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup(this.components);
            this.document1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanelRule = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.gridControlRule = new DevExpress.XtraGrid.GridControl();
            this.gridViewRule = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panelContainer1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanelSource = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.gridControlSource = new DevExpress.XtraGrid.GridControl();
            this.gridViewSource = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dockPanelTarget = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel3_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.gridControlTarget = new DevExpress.XtraGrid.GridControl();
            this.gridViewTarget = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.btnAddRule = new System.Windows.Forms.Button();
            this.btnDeleteRule = new System.Windows.Forms.Button();
            this.btnDeleteSource = new System.Windows.Forms.Button();
            this.btnAddSource = new System.Windows.Forms.Button();
            this.btnDeleteTarget = new System.Windows.Forms.Button();
            this.btnAddTarget = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanelRule.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewRule)).BeginInit();
            this.panelContainer1.SuspendLayout();
            this.dockPanelSource.SuspendLayout();
            this.dockPanel2_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSource)).BeginInit();
            this.dockPanelTarget.SuspendLayout();
            this.dockPanel3_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            this.SuspendLayout();
            // 
            // documentGroup1
            // 
            this.documentGroup1.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document[] {
            this.document1});
            // 
            // document1
            // 
            this.document1.Caption = "Rules";
            this.document1.ControlName = "dockPanelRule";
            this.document1.FloatLocation = new System.Drawing.Point(0, 0);
            this.document1.FloatSize = new System.Drawing.Size(200, 200);
            this.document1.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.True;
            this.document1.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.True;
            this.document1.Properties.AllowFloatOnDoubleClick = DevExpress.Utils.DefaultBoolean.True;
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanelRule,
            this.panelContainer1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl"});
            // 
            // dockPanelRule
            // 
            this.dockPanelRule.Controls.Add(this.dockPanel1_Container);
            this.dockPanelRule.DockedAsTabbedDocument = true;
            this.dockPanelRule.ID = new System.Guid("55b1121a-be74-4105-a894-25c722bb43af");
            this.dockPanelRule.Name = "dockPanelRule";
            this.dockPanelRule.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelRule.Text = "Rules";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.btnDeleteRule);
            this.dockPanel1_Container.Controls.Add(this.btnAddRule);
            this.dockPanel1_Container.Controls.Add(this.gridControlRule);
            this.dockPanel1_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(451, 561);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // gridControlRule
            // 
            this.gridControlRule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlRule.Location = new System.Drawing.Point(3, 3);
            this.gridControlRule.MainView = this.gridViewRule;
            this.gridControlRule.Name = "gridControlRule";
            this.gridControlRule.Size = new System.Drawing.Size(445, 508);
            this.gridControlRule.TabIndex = 0;
            this.gridControlRule.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewRule});
            // 
            // gridViewRule
            // 
            this.gridViewRule.GridControl = this.gridControlRule;
            this.gridViewRule.Name = "gridViewRule";
            // 
            // panelContainer1
            // 
            this.panelContainer1.Controls.Add(this.dockPanelSource);
            this.panelContainer1.Controls.Add(this.dockPanelTarget);
            this.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.panelContainer1.ID = new System.Guid("7d5b06a7-a2ba-4d96-a8e9-46c8fbde66aa");
            this.panelContainer1.Location = new System.Drawing.Point(457, 0);
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.OriginalSize = new System.Drawing.Size(347, 200);
            this.panelContainer1.Size = new System.Drawing.Size(347, 601);
            this.panelContainer1.Text = "panelContainer1";
            // 
            // dockPanelSource
            // 
            this.dockPanelSource.Controls.Add(this.dockPanel2_Container);
            this.dockPanelSource.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanelSource.ID = new System.Guid("24ec8e8d-3b44-495a-ad64-1e765f8565c7");
            this.dockPanelSource.Location = new System.Drawing.Point(0, 0);
            this.dockPanelSource.Name = "dockPanelSource";
            this.dockPanelSource.OriginalSize = new System.Drawing.Size(517, 400);
            this.dockPanelSource.Size = new System.Drawing.Size(347, 301);
            this.dockPanelSource.Text = "Source";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Controls.Add(this.btnDeleteSource);
            this.dockPanel2_Container.Controls.Add(this.btnAddSource);
            this.dockPanel2_Container.Controls.Add(this.gridControlSource);
            this.dockPanel2_Container.Location = new System.Drawing.Point(9, 33);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(332, 259);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // gridControlSource
            // 
            this.gridControlSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlSource.Location = new System.Drawing.Point(3, 3);
            this.gridControlSource.MainView = this.gridViewSource;
            this.gridControlSource.Name = "gridControlSource";
            this.gridControlSource.Size = new System.Drawing.Size(323, 199);
            this.gridControlSource.TabIndex = 0;
            this.gridControlSource.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewSource});
            // 
            // gridViewSource
            // 
            this.gridViewSource.GridControl = this.gridControlSource;
            this.gridViewSource.Name = "gridViewSource";
            // 
            // dockPanelTarget
            // 
            this.dockPanelTarget.Controls.Add(this.dockPanel3_Container);
            this.dockPanelTarget.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanelTarget.ID = new System.Guid("2f1e6fd7-7917-487b-aea9-ca673900628e");
            this.dockPanelTarget.Location = new System.Drawing.Point(0, 301);
            this.dockPanelTarget.Name = "dockPanelTarget";
            this.dockPanelTarget.OriginalSize = new System.Drawing.Size(517, 399);
            this.dockPanelTarget.Size = new System.Drawing.Size(347, 300);
            this.dockPanelTarget.Text = "Target";
            // 
            // dockPanel3_Container
            // 
            this.dockPanel3_Container.Controls.Add(this.btnDeleteTarget);
            this.dockPanel3_Container.Controls.Add(this.gridControlTarget);
            this.dockPanel3_Container.Controls.Add(this.btnAddTarget);
            this.dockPanel3_Container.Location = new System.Drawing.Point(9, 33);
            this.dockPanel3_Container.Name = "dockPanel3_Container";
            this.dockPanel3_Container.Size = new System.Drawing.Size(332, 261);
            this.dockPanel3_Container.TabIndex = 0;
            // 
            // gridControlTarget
            // 
            this.gridControlTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlTarget.Location = new System.Drawing.Point(3, 3);
            this.gridControlTarget.MainView = this.gridViewTarget;
            this.gridControlTarget.Name = "gridControlTarget";
            this.gridControlTarget.Size = new System.Drawing.Size(323, 211);
            this.gridControlTarget.TabIndex = 0;
            this.gridControlTarget.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTarget});
            // 
            // gridViewTarget
            // 
            this.gridViewTarget.GridControl = this.gridControlTarget;
            this.gridViewTarget.Name = "gridViewTarget";
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.View = this.tabbedView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // tabbedView1
            // 
            this.tabbedView1.DocumentGroups.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup[] {
            this.documentGroup1});
            this.tabbedView1.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
            this.document1});
            dockingContainer4.Element = this.documentGroup1;
            this.tabbedView1.RootContainer.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            dockingContainer4});
            // 
            // btnAddRule
            // 
            this.btnAddRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRule.Location = new System.Drawing.Point(260, 516);
            this.btnAddRule.Name = "btnAddRule";
            this.btnAddRule.Size = new System.Drawing.Size(91, 36);
            this.btnAddRule.TabIndex = 1;
            this.btnAddRule.Text = "Add";
            this.btnAddRule.UseVisualStyleBackColor = true;
            this.btnAddRule.Click += new System.EventHandler(this.btnAddRule_Click);
            // 
            // btnDeleteRule
            // 
            this.btnDeleteRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteRule.Location = new System.Drawing.Point(357, 517);
            this.btnDeleteRule.Name = "btnDeleteRule";
            this.btnDeleteRule.Size = new System.Drawing.Size(91, 36);
            this.btnDeleteRule.TabIndex = 2;
            this.btnDeleteRule.Text = "Delete";
            this.btnDeleteRule.UseVisualStyleBackColor = true;
            // 
            // btnDeleteSource
            // 
            this.btnDeleteSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteSource.Location = new System.Drawing.Point(235, 208);
            this.btnDeleteSource.Name = "btnDeleteSource";
            this.btnDeleteSource.Size = new System.Drawing.Size(91, 36);
            this.btnDeleteSource.TabIndex = 4;
            this.btnDeleteSource.Text = "Delete";
            this.btnDeleteSource.UseVisualStyleBackColor = true;
            // 
            // btnAddSource
            // 
            this.btnAddSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSource.Location = new System.Drawing.Point(138, 209);
            this.btnAddSource.Name = "btnAddSource";
            this.btnAddSource.Size = new System.Drawing.Size(91, 36);
            this.btnAddSource.TabIndex = 3;
            this.btnAddSource.Text = "Add";
            this.btnAddSource.UseVisualStyleBackColor = true;
            // 
            // btnDeleteTarget
            // 
            this.btnDeleteTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTarget.Location = new System.Drawing.Point(235, 219);
            this.btnDeleteTarget.Name = "btnDeleteTarget";
            this.btnDeleteTarget.Size = new System.Drawing.Size(91, 36);
            this.btnDeleteTarget.TabIndex = 4;
            this.btnDeleteTarget.Text = "Delete";
            this.btnDeleteTarget.UseVisualStyleBackColor = true;
            // 
            // btnAddTarget
            // 
            this.btnAddTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTarget.Location = new System.Drawing.Point(138, 220);
            this.btnAddTarget.Name = "btnAddTarget";
            this.btnAddTarget.Size = new System.Drawing.Size(91, 36);
            this.btnAddTarget.TabIndex = 3;
            this.btnAddTarget.Text = "Add";
            this.btnAddTarget.UseVisualStyleBackColor = true;
            // 
            // FormEditAddressMappingRule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 601);
            this.Controls.Add(this.panelContainer1);
            this.Name = "FormEditAddressMappingRule";
            this.Text = "FormEditAddressMappingRule";
            this.Load += new System.EventHandler(this.FormEditAddressMappingRule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanelRule.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewRule)).EndInit();
            this.panelContainer1.ResumeLayout(false);
            this.dockPanelSource.ResumeLayout(false);
            this.dockPanel2_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSource)).EndInit();
            this.dockPanelTarget.ResumeLayout(false);
            this.dockPanel3_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelSource;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelTarget;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel3_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelRule;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraGrid.GridControl gridControlRule;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewRule;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup documentGroup1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document document1;
        private DevExpress.XtraGrid.GridControl gridControlSource;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewSource;
        private DevExpress.XtraGrid.GridControl gridControlTarget;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTarget;
        private System.Windows.Forms.Button btnDeleteSource;
        private System.Windows.Forms.Button btnAddSource;
        private System.Windows.Forms.Button btnDeleteTarget;
        private System.Windows.Forms.Button btnAddTarget;
        private System.Windows.Forms.Button btnDeleteRule;
        private System.Windows.Forms.Button btnAddRule;
    }
}