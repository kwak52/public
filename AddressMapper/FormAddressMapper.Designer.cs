﻿namespace AddressMapper
{
    partial class FormAddressMapper
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
            DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer dockingContainer1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer();
            this.documentGroup1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup(this.components);
            this.document1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barCheckItemShowLog = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemSource = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemTarget = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemShowMain = new DevExpress.XtraBars.BarCheckItem();
            this.barComboOmron = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barComboXG5000 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupTemplates = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupView = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanelLog = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.ucPanelLog1 = new AddressMapper.UcPanelLog();
            this.dockPanelSource = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.checkedComboBoxEdit1 = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.dockPanelTarget = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockPanelMain = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer2 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.rangeControl1 = new AddressMapper.CustomRangeControl();
            this.numericRangeControlClient1 = new DevExpress.XtraEditors.NumericRangeControlClient();
            this.checkedComboBoxEdit2 = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.rangeControlNormal = new DevExpress.XtraEditors.RangeControl();
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanelLog.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            this.dockPanelSource.SuspendLayout();
            this.controlContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit1.Properties)).BeginInit();
            this.dockPanelTarget.SuspendLayout();
            this.dockPanelMain.SuspendLayout();
            this.controlContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeControlNormal)).BeginInit();
            this.SuspendLayout();
            // 
            // documentGroup1
            // 
            this.documentGroup1.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document[] {
            this.document1});
            // 
            // document1
            // 
            this.document1.Caption = "Address map";
            this.document1.ControlName = "dockPanelMain";
            this.document1.FloatLocation = new System.Drawing.Point(0, 0);
            this.document1.FloatSize = new System.Drawing.Size(200, 200);
            this.document1.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.True;
            this.document1.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.True;
            this.document1.Properties.AllowFloatOnDoubleClick = DevExpress.Utils.DefaultBoolean.True;
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.barCheckItemShowLog,
            this.barCheckItemSource,
            this.barCheckItemTarget,
            this.barCheckItemShowMain,
            this.barComboOmron,
            this.barComboXG5000});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 8;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemCheckEdit1,
            this.repositoryItemComboBox2});
            this.ribbon.Size = new System.Drawing.Size(1178, 217);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // barCheckItemShowLog
            // 
            this.barCheckItemShowLog.Caption = "Log";
            this.barCheckItemShowLog.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowLog.Id = 1;
            this.barCheckItemShowLog.Name = "barCheckItemShowLog";
            // 
            // barCheckItemSource
            // 
            this.barCheckItemSource.Caption = "Omron";
            this.barCheckItemSource.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemSource.Id = 2;
            this.barCheckItemSource.Name = "barCheckItemSource";
            // 
            // barCheckItemTarget
            // 
            this.barCheckItemTarget.Caption = "XG5000";
            this.barCheckItemTarget.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemTarget.Id = 3;
            this.barCheckItemTarget.Name = "barCheckItemTarget";
            // 
            // barCheckItemShowMain
            // 
            this.barCheckItemShowMain.Caption = "Main";
            this.barCheckItemShowMain.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowMain.Id = 4;
            this.barCheckItemShowMain.Name = "barCheckItemShowMain";
            // 
            // barComboOmron
            // 
            this.barComboOmron.Caption = "Omron";
            this.barComboOmron.Edit = this.repositoryItemComboBox1;
            this.barComboOmron.Id = 5;
            this.barComboOmron.Name = "barComboOmron";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // barComboXG5000
            // 
            this.barComboXG5000.Caption = "XG5000";
            this.barComboXG5000.Edit = this.repositoryItemComboBox2;
            this.barComboXG5000.Id = 7;
            this.barComboXG5000.Name = "barComboXG5000";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupTemplates,
            this.ribbonPageGroupView});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroupTemplates
            // 
            this.ribbonPageGroupTemplates.ItemLinks.Add(this.barComboOmron);
            this.ribbonPageGroupTemplates.ItemLinks.Add(this.barComboXG5000);
            this.ribbonPageGroupTemplates.Name = "ribbonPageGroupTemplates";
            this.ribbonPageGroupTemplates.Text = "Tempaltes";
            // 
            // ribbonPageGroupView
            // 
            this.ribbonPageGroupView.ItemLinks.Add(this.barCheckItemShowMain);
            this.ribbonPageGroupView.ItemLinks.Add(this.barCheckItemShowLog);
            this.ribbonPageGroupView.ItemLinks.Add(this.barCheckItemSource);
            this.ribbonPageGroupView.ItemLinks.Add(this.barCheckItemTarget);
            this.ribbonPageGroupView.Name = "ribbonPageGroupView";
            this.ribbonPageGroupView.Text = "View";
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 835);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1178, 41);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanelLog,
            this.dockPanelSource,
            this.dockPanelTarget,
            this.dockPanelMain});
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
            // dockPanelLog
            // 
            this.dockPanelLog.Controls.Add(this.dockPanel1_Container);
            this.dockPanelLog.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.dockPanelLog.ID = new System.Guid("4be5f6b3-5971-4327-9cd9-0efe0817a1bd");
            this.dockPanelLog.Location = new System.Drawing.Point(0, 635);
            this.dockPanelLog.Name = "dockPanelLog";
            this.dockPanelLog.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelLog.Size = new System.Drawing.Size(1178, 200);
            this.dockPanelLog.Text = "Log";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.ucPanelLog1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(6, 36);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(1166, 158);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // ucPanelLog1
            // 
            this.ucPanelLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPanelLog1.Location = new System.Drawing.Point(0, 0);
            this.ucPanelLog1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucPanelLog1.Name = "ucPanelLog1";
            this.ucPanelLog1.SelectedIndex = -1;
            this.ucPanelLog1.Size = new System.Drawing.Size(1166, 158);
            this.ucPanelLog1.TabIndex = 0;
            // 
            // dockPanelSource
            // 
            this.dockPanelSource.Controls.Add(this.controlContainer1);
            this.dockPanelSource.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanelSource.ID = new System.Guid("e9c28f34-66a8-463d-bed0-25df2c0c5782");
            this.dockPanelSource.Location = new System.Drawing.Point(0, 217);
            this.dockPanelSource.Name = "dockPanelSource";
            this.dockPanelSource.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelSource.Size = new System.Drawing.Size(200, 418);
            this.dockPanelSource.Text = "Omron";
            // 
            // controlContainer1
            // 
            this.controlContainer1.Controls.Add(this.checkedComboBoxEdit1);
            this.controlContainer1.Location = new System.Drawing.Point(6, 33);
            this.controlContainer1.Name = "controlContainer1";
            this.controlContainer1.Size = new System.Drawing.Size(185, 379);
            this.controlContainer1.TabIndex = 0;
            // 
            // checkedComboBoxEdit1
            // 
            this.checkedComboBoxEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedComboBoxEdit1.Location = new System.Drawing.Point(7, 13);
            this.checkedComboBoxEdit1.MenuManager = this.ribbon;
            this.checkedComboBoxEdit1.Name = "checkedComboBoxEdit1";
            this.checkedComboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.checkedComboBoxEdit1.Size = new System.Drawing.Size(175, 30);
            this.checkedComboBoxEdit1.TabIndex = 0;
            // 
            // dockPanelTarget
            // 
            this.dockPanelTarget.Controls.Add(this.dockPanel2_Container);
            this.dockPanelTarget.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanelTarget.ID = new System.Guid("ee0c92d3-2a4b-4315-91a1-2bcd5f8935da");
            this.dockPanelTarget.Location = new System.Drawing.Point(200, 217);
            this.dockPanelTarget.Name = "dockPanelTarget";
            this.dockPanelTarget.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelTarget.Size = new System.Drawing.Size(200, 418);
            this.dockPanelTarget.Text = "XG5000";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Location = new System.Drawing.Point(6, 33);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(185, 379);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // dockPanelMain
            // 
            this.dockPanelMain.Controls.Add(this.controlContainer2);
            this.dockPanelMain.DockedAsTabbedDocument = true;
            this.dockPanelMain.ID = new System.Guid("6e76eaf6-85ee-42b7-8766-5b84857b094c");
            this.dockPanelMain.Name = "dockPanelMain";
            this.dockPanelMain.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelMain.Text = "Address map";
            // 
            // controlContainer2
            // 
            this.controlContainer2.Controls.Add(this.rangeControlNormal);
            this.controlContainer2.Controls.Add(this.rangeControl1);
            this.controlContainer2.Location = new System.Drawing.Point(0, 0);
            this.controlContainer2.Name = "controlContainer2";
            this.controlContainer2.Size = new System.Drawing.Size(772, 378);
            this.controlContainer2.TabIndex = 0;
            // 
            // rangeControl1
            // 
            this.rangeControl1.Client = this.numericRangeControlClient1;
            this.rangeControl1.Location = new System.Drawing.Point(177, 71);
            this.rangeControl1.Name = "rangeControl1";
            this.rangeControl1.Size = new System.Drawing.Size(420, 90);
            this.rangeControl1.TabIndex = 0;
            this.rangeControl1.Text = "rangeControl1";
            // 
            // numericRangeControlClient1
            // 
            this.numericRangeControlClient1.RangeControl = null;
            // 
            // checkedComboBoxEdit2
            // 
            this.checkedComboBoxEdit2.Location = new System.Drawing.Point(74, 33);
            this.checkedComboBoxEdit2.MenuManager = this.ribbon;
            this.checkedComboBoxEdit2.Name = "checkedComboBoxEdit2";
            this.checkedComboBoxEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.checkedComboBoxEdit2.Size = new System.Drawing.Size(150, 30);
            this.checkedComboBoxEdit2.TabIndex = 0;
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.MenuManager = this.ribbon;
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
            dockingContainer1.Element = this.documentGroup1;
            this.tabbedView1.RootContainer.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            dockingContainer1});
            // 
            // rangeControlNormal
            // 
            this.rangeControlNormal.Client = this.numericRangeControlClient1;
            this.rangeControlNormal.Location = new System.Drawing.Point(146, 239);
            this.rangeControlNormal.Name = "rangeControlNormal";
            this.rangeControlNormal.Size = new System.Drawing.Size(420, 90);
            this.rangeControlNormal.TabIndex = 1;
            this.rangeControlNormal.Text = "rangeControl2";
            // 
            // FormAddressMapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 876);
            this.Controls.Add(this.dockPanelTarget);
            this.Controls.Add(this.dockPanelSource);
            this.Controls.Add(this.dockPanelLog);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Name = "FormAddressMapper";
            this.Ribbon = this.ribbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "FormAddressMapper";
            this.Load += new System.EventHandler(this.FormAddressMapper_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanelLog.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.dockPanelSource.ResumeLayout(false);
            this.controlContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit1.Properties)).EndInit();
            this.dockPanelTarget.ResumeLayout(false);
            this.dockPanelMain.ResumeLayout(false);
            this.controlContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rangeControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeControlNormal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupView;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelLog;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private UcPanelLog ucPanelLog1;
        private DevExpress.XtraBars.BarCheckItem barCheckItemShowLog;
        private DevExpress.XtraBars.BarCheckItem barCheckItemSource;
        private DevExpress.XtraBars.BarCheckItem barCheckItemTarget;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelTarget;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelSource;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelMain;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer2;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup documentGroup1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document document1;
        private DevExpress.XtraBars.BarCheckItem barCheckItemShowMain;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupTemplates;
        private DevExpress.XtraBars.BarEditItem barComboOmron;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarEditItem barComboXG5000;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit checkedComboBoxEdit1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit checkedComboBoxEdit2;
        private DevExpress.XtraEditors.NumericRangeControlClient numericRangeControlClient1;
        private CustomRangeControl rangeControl1;
        private DevExpress.XtraEditors.RangeControl rangeControlNormal;
    }
}