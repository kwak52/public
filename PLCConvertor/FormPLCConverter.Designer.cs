namespace PLCConvertor
{
    partial class FormPLCConverter
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPLCConverter));
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer dockingContainer1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer();
            this.documentGroup1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup();
            this.document1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItemTestParse = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemCxtParse = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItemSource = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxSource = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemTarget = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBoxTarget = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barButtonItemAddressMapping = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemEditAddressMappingRule = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemEditPerferences = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemTestAddress = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemTranslate = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSetting = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemExit = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageDebugging = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupTest = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupOptions = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageProductRelease = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager();
            this.dockPanelLog = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.ucPanelLog1 = new PLCConvertor.UcPanelLog();
            this.dockPanelMain = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelMain = new System.Windows.Forms.Panel();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager();
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView();
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanelLog.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            this.dockPanelMain.SuspendLayout();
            this.controlContainer1.SuspendLayout();
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
            this.document1.Caption = "Main";
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
            this.barButtonItemTestParse,
            this.barButtonItemCxtParse,
            this.barEditItemSource,
            this.barEditItemTarget,
            this.barButtonItemAddressMapping,
            this.barButtonItemEditAddressMappingRule,
            this.barButtonItemEditPerferences,
            this.barButtonItemTestAddress,
            this.barButtonItemTranslate,
            this.barButtonItemSetting,
            this.barButtonItemExit});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.Margin = new System.Windows.Forms.Padding(2);
            this.ribbon.MaxItemId = 15;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageDebugging,
            this.ribbonPageProductRelease});
            this.ribbon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBoxSource,
            this.repositoryItemComboBoxTarget});
            this.ribbon.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(957, 183);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            // 
            // barButtonItemTestParse
            // 
            this.barButtonItemTestParse.Caption = "Test parse";
            this.barButtonItemTestParse.Id = 1;
            this.barButtonItemTestParse.Name = "barButtonItemTestParse";
            this.barButtonItemTestParse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarButtonItemTestParse_ItemClick);
            // 
            // barButtonItemCxtParse
            // 
            this.barButtonItemCxtParse.Caption = "CXT";
            this.barButtonItemCxtParse.Id = 2;
            this.barButtonItemCxtParse.Name = "barButtonItemCxtParse";
            this.barButtonItemCxtParse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarButtonItemCxtParse_ItemClick);
            // 
            // barEditItemSource
            // 
            this.barEditItemSource.Caption = "Source";
            this.barEditItemSource.Edit = this.repositoryItemComboBoxSource;
            this.barEditItemSource.Id = 3;
            this.barEditItemSource.Name = "barEditItemSource";
            // 
            // repositoryItemComboBoxSource
            // 
            this.repositoryItemComboBoxSource.AutoHeight = false;
            this.repositoryItemComboBoxSource.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxSource.Name = "repositoryItemComboBoxSource";
            // 
            // barEditItemTarget
            // 
            this.barEditItemTarget.Caption = "Target";
            this.barEditItemTarget.Edit = this.repositoryItemComboBoxTarget;
            this.barEditItemTarget.Id = 4;
            this.barEditItemTarget.Name = "barEditItemTarget";
            // 
            // repositoryItemComboBoxTarget
            // 
            this.repositoryItemComboBoxTarget.AutoHeight = false;
            this.repositoryItemComboBoxTarget.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxTarget.Name = "repositoryItemComboBoxTarget";
            // 
            // barButtonItemAddressMapping
            // 
            this.barButtonItemAddressMapping.Caption = "Address mapping";
            this.barButtonItemAddressMapping.Id = 6;
            this.barButtonItemAddressMapping.Name = "barButtonItemAddressMapping";
            this.barButtonItemAddressMapping.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAddressMapping_ItemClick);
            // 
            // barButtonItemEditAddressMappingRule
            // 
            this.barButtonItemEditAddressMappingRule.Caption = "Edit address mapping";
            this.barButtonItemEditAddressMappingRule.Id = 7;
            this.barButtonItemEditAddressMappingRule.Name = "barButtonItemEditAddressMappingRule";
            this.barButtonItemEditAddressMappingRule.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemEditAddressMappingRule_ItemClick);
            // 
            // barButtonItemEditPerferences
            // 
            this.barButtonItemEditPerferences.Caption = "Options";
            this.barButtonItemEditPerferences.Id = 8;
            this.barButtonItemEditPerferences.Name = "barButtonItemEditPerferences";
            this.barButtonItemEditPerferences.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemEditPerferences_ItemClick);
            // 
            // barButtonItemTestAddress
            // 
            this.barButtonItemTestAddress.Caption = "Test address";
            this.barButtonItemTestAddress.Id = 9;
            this.barButtonItemTestAddress.Name = "barButtonItemTestAddress";
            toolTipTitleItem1.Text = "Test Address Mapping";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Test conversion of device address.";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.barButtonItemTestAddress.SuperTip = superToolTip1;
            this.barButtonItemTestAddress.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemTestAddress_ItemClick);
            // 
            // barButtonItemTranslate
            // 
            this.barButtonItemTranslate.Caption = "변환";
            this.barButtonItemTranslate.Id = 11;
            this.barButtonItemTranslate.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItemTranslate.ImageOptions.Image")));
            this.barButtonItemTranslate.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItemTranslate.ImageOptions.LargeImage")));
            this.barButtonItemTranslate.Name = "barButtonItemTranslate";
            toolTipTitleItem2.Text = "PLC 변환";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "옴론 CX-Programmer text project 파일을 LS 산전 XG5000 용으로 변환합니다.\r\n\r\n- 결과 파일(*.qtx, *.txt" +
    ", *.review.cxt 등)은 경고없이 덮어쓰기 합니다.\r\n";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.barButtonItemTranslate.SuperTip = superToolTip2;
            this.barButtonItemTranslate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemTranslate_ItemClick);
            // 
            // barButtonItemSetting
            // 
            this.barButtonItemSetting.Caption = "설정";
            this.barButtonItemSetting.Id = 12;
            this.barButtonItemSetting.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItemSetting.ImageOptions.Image")));
            this.barButtonItemSetting.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItemSetting.ImageOptions.LargeImage")));
            this.barButtonItemSetting.Name = "barButtonItemSetting";
            toolTipItem3.Text = "변환 관련 설정을 변경합니다";
            superToolTip3.Items.Add(toolTipItem3);
            this.barButtonItemSetting.SuperTip = superToolTip3;
            this.barButtonItemSetting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSetting_ItemClick);
            // 
            // barButtonItemExit
            // 
            this.barButtonItemExit.Caption = "종료";
            this.barButtonItemExit.Id = 14;
            this.barButtonItemExit.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItemExit.ImageOptions.Image")));
            this.barButtonItemExit.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItemExit.ImageOptions.LargeImage")));
            this.barButtonItemExit.Name = "barButtonItemExit";
            this.barButtonItemExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemExit_ItemClick);
            // 
            // ribbonPageDebugging
            // 
            this.ribbonPageDebugging.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupTest,
            this.ribbonPageGroupOptions});
            this.ribbonPageDebugging.Name = "ribbonPageDebugging";
            this.ribbonPageDebugging.Text = "ribbonPage1";
            // 
            // ribbonPageGroupTest
            // 
            this.ribbonPageGroupTest.ItemLinks.Add(this.barButtonItemTestParse);
            this.ribbonPageGroupTest.ItemLinks.Add(this.barButtonItemCxtParse);
            this.ribbonPageGroupTest.ItemLinks.Add(this.barButtonItemAddressMapping);
            this.ribbonPageGroupTest.ItemLinks.Add(this.barButtonItemEditAddressMappingRule);
            this.ribbonPageGroupTest.ItemLinks.Add(this.barButtonItemTestAddress);
            this.ribbonPageGroupTest.Name = "ribbonPageGroupTest";
            this.ribbonPageGroupTest.Text = "Test";
            // 
            // ribbonPageGroupOptions
            // 
            this.ribbonPageGroupOptions.ItemLinks.Add(this.barEditItemSource);
            this.ribbonPageGroupOptions.ItemLinks.Add(this.barEditItemTarget);
            this.ribbonPageGroupOptions.ItemLinks.Add(this.barButtonItemEditPerferences);
            this.ribbonPageGroupOptions.Name = "ribbonPageGroupOptions";
            this.ribbonPageGroupOptions.Text = "Options";
            // 
            // ribbonPageProductRelease
            // 
            this.ribbonPageProductRelease.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPageProductRelease.Name = "ribbonPageProductRelease";
            this.ribbonPageProductRelease.Text = "메뉴";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemTranslate);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemSetting);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemExit);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "메인 메뉴";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 804);
            this.ribbonStatusBar.Margin = new System.Windows.Forms.Padding(2);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(957, 37);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanelLog,
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
            this.dockPanelLog.ID = new System.Guid("ec78d662-2e79-41d3-9997-6c98aac61503");
            this.dockPanelLog.Location = new System.Drawing.Point(0, 575);
            this.dockPanelLog.Margin = new System.Windows.Forms.Padding(2);
            this.dockPanelLog.Name = "dockPanelLog";
            this.dockPanelLog.OriginalSize = new System.Drawing.Size(200, 229);
            this.dockPanelLog.Size = new System.Drawing.Size(957, 229);
            this.dockPanelLog.Text = "Log";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.ucPanelLog1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(5, 29);
            this.dockPanel1_Container.Margin = new System.Windows.Forms.Padding(2);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(947, 195);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // ucPanelLog1
            // 
            this.ucPanelLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPanelLog1.Location = new System.Drawing.Point(0, 0);
            this.ucPanelLog1.Margin = new System.Windows.Forms.Padding(5);
            this.ucPanelLog1.Name = "ucPanelLog1";
            this.ucPanelLog1.SelectedIndex = -1;
            this.ucPanelLog1.Size = new System.Drawing.Size(947, 195);
            this.ucPanelLog1.TabIndex = 0;
            // 
            // dockPanelMain
            // 
            this.dockPanelMain.Controls.Add(this.controlContainer1);
            this.dockPanelMain.DockedAsTabbedDocument = true;
            this.dockPanelMain.ID = new System.Guid("a9cb4fad-4651-4699-96d7-74732f1ee9fc");
            this.dockPanelMain.Name = "dockPanelMain";
            this.dockPanelMain.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelMain.Text = "Main";
            // 
            // controlContainer1
            // 
            this.controlContainer1.Controls.Add(this.panelMain);
            this.controlContainer1.Location = new System.Drawing.Point(0, 0);
            this.controlContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.controlContainer1.Name = "controlContainer1";
            this.controlContainer1.Size = new System.Drawing.Size(951, 357);
            this.controlContainer1.TabIndex = 0;
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(951, 357);
            this.panelMain.TabIndex = 0;
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
            // FormPLCConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 841);
            this.Controls.Add(this.dockPanelLog);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "FormPLCConverter";
            this.Ribbon = this.ribbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "PLC 변환기";
            this.Load += new System.EventHandler(this.FormPLCConverter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanelLog.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.dockPanelMain.ResumeLayout(false);
            this.controlContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageDebugging;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupTest;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelLog;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private UcPanelLog ucPanelLog1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemTestParse;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelMain;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup documentGroup1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document document1;
        private System.Windows.Forms.Panel panelMain;
        private DevExpress.XtraBars.BarButtonItem barButtonItemCxtParse;
        private DevExpress.XtraBars.BarEditItem barEditItemSource;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxSource;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupOptions;
        private DevExpress.XtraBars.BarEditItem barEditItemTarget;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxTarget;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAddressMapping;
        private DevExpress.XtraBars.BarButtonItem barButtonItemEditAddressMappingRule;
        private DevExpress.XtraBars.BarButtonItem barButtonItemEditPerferences;
        private DevExpress.XtraBars.BarButtonItem barButtonItemTestAddress;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageProductRelease;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemTranslate;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSetting;
        private DevExpress.XtraBars.BarButtonItem barButtonItemExit;
    }
}