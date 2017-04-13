using System.Reflection;

namespace CpTesterPlatform.CpTester
{    partial class FormAppSs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAppSs));
            this.timerGlobalTask = new System.Windows.Forms.Timer(this.components);
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.m_cpDocManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.ribbonControlCPTester = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItemLoadCfgFrFile = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItemShowDeactivateStep = new DevExpress.XtraBars.BarCheckItem();
            this.skinRibbonGalleryBarItem1 = new DevExpress.XtraBars.SkinRibbonGalleryBarItem();
            this.barCheckItemShowColPosition = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemShowColVariant = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemShowColGate = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemShowColReturn = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemShowColParameter = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemShowColComment = new DevExpress.XtraBars.BarCheckItem();
            this.skinRibbonGalleryBarItem2 = new DevExpress.XtraBars.SkinRibbonGalleryBarItem();
            this.barButtonItemMnCtrStart = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemMnCtrStop = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemMnCtrTsConditon = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemMnCtrManualSignalAnalysis = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItemLogCANMsg = new DevExpress.XtraBars.BarCheckItem();
            this.barButtonItemMnCtrLogTStepAction = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemMnCtrLogTStepMeasuring = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItemLogSaveMeasuringLog = new DevExpress.XtraBars.BarCheckItem();
            this.barEditItemShowLogLevel = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barCheckItemMeasuringArray = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemSaveConsoleLog = new DevExpress.XtraBars.BarCheckItem();
            this.barButtonItemSaveLogPath = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemExit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemMnMain = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItemDebugConsoleOut = new DevExpress.XtraBars.BarCheckItem();
            this.barEditItem_PartID = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit5 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItem_SelectAuto = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_SelectManual = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_AdminDialogClear = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_LVDT = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_Motor1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_DigtalIO = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_AnalogIO = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_Robot = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_PLC = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_TempHumi = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_LCR = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_PowerSupply1 = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem_TemperatureUpper = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit6 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItem_TemperatureLower = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItem_HumidityUpper = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit8 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItem_HumidityLower = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit9 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItem_CurrentTemp = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit12 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItem_CurrentHumidity = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit13 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barCheckItemShowColMP = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItemShowColInf = new DevExpress.XtraBars.BarCheckItem();
            this.barButtonItem_LoadTestList = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem_AirGapMonitor = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit14 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItem_manualAirGap = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_Motor2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_Motor3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_PowerSupply2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_PowerSupply3 = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem_RPM = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit15 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItemDoorControl = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageMainControl = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupFileControl = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupTestCondition = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupMnCtrTesterControl = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageTsOption = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupShowSteps = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupShowColumn = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageManulControl = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupManualCtrComm = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup_Monitor = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageLog = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupMnCtrLogOptions = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup_id = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageEnvironmentSetup = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupEvnSetupTheme = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupDoor = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.repositoryItemImageEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.repositoryItemImageEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();
            this.repositoryItemPictureEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.repositoryItemProgressBar1 = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.repositoryItemMarqueeProgressBar1 = new DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar();
            this.repositoryItemRadioGroup1 = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemComboBox3 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemToggleSwitch1 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.repositoryItemToggleSwitch2 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.repositoryItemToggleSwitch_AutoManu = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.repositoryItemTextEdit10 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit11 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.ribbonStatusBarCPMainForm = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.barHeaderItemVersion = new DevExpress.XtraBars.BarHeaderItem();
            this.barStaticItemLoadedTestList = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.barStaticItemCurrentTime = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemRepeatTest = new DevExpress.XtraBars.BarStaticItem();
            this.tabbedView = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.barButtonItemTriggerEventOnThread = new DevExpress.XtraBars.BarButtonItem();
            this.actionList1 = new Dsu.Common.Utilities.Actions.ActionList(this.components);
            this.action1 = new Dsu.Common.Utilities.Actions.Action(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl_Message = new DevExpress.XtraEditors.LabelControl();
            this.labelControlTitle = new DevExpress.XtraEditors.LabelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.simpleButton_AutoManu = new DevExpress.XtraEditors.SimpleButton();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.panelContainer1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel_TestResult = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.ucTestMonitor1 = new CpTesterPlatform.CpTester.ucTestMonitor();
            this.dockPanel_Output = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.listBoxControl_Output = new DevExpress.XtraEditors.ListBoxControl();
            this.barButtonItem_RobotReadyMove = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.m_cpDocManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControlCPTester)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMarqueeProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch_AutoManu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            this.panelContainer1.SuspendLayout();
            this.dockPanel_TestResult.SuspendLayout();
            this.controlContainer1.SuspendLayout();
            this.dockPanel_Output.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl_Output)).BeginInit();
            this.SuspendLayout();
            // 
            // timerGlobalTask
            // 
            this.timerGlobalTask.Tick += new System.EventHandler(this.timerGlobalTask_Tick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.Glyph = ((System.Drawing.Image)(resources.GetObject("ribbonPageGroup2.Glyph")));
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Show Option";
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "Deactivation Step";
            this.barCheckItem1.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barCheckItem1.Glyph")));
            this.barCheckItem1.Id = 11;
            this.barCheckItem1.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barCheckItem1.LargeGlyph")));
            this.barCheckItem1.Name = "barCheckItem1";
            // 
            // m_cpDocManager
            // 
            this.m_cpDocManager.MdiParent = this;
            this.m_cpDocManager.MenuManager = this.ribbonControlCPTester;
            this.m_cpDocManager.View = this.tabbedView;
            this.m_cpDocManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView});
            // 
            // ribbonControlCPTester
            // 
            this.ribbonControlCPTester.ExpandCollapseItem.Id = 0;
            this.ribbonControlCPTester.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControlCPTester.ExpandCollapseItem,
            this.barButtonItemLoadCfgFrFile,
            this.barCheckItemShowDeactivateStep,
            this.skinRibbonGalleryBarItem1,
            this.barCheckItemShowColPosition,
            this.barCheckItemShowColVariant,
            this.barCheckItemShowColGate,
            this.barCheckItemShowColReturn,
            this.barCheckItemShowColParameter,
            this.barCheckItemShowColComment,
            this.skinRibbonGalleryBarItem2,
            this.barButtonItemMnCtrStart,
            this.barButtonItemMnCtrStop,
            this.barButtonItemMnCtrTsConditon,
            this.barButtonItemMnCtrManualSignalAnalysis,
            this.barCheckItemLogCANMsg,
            this.barButtonItemMnCtrLogTStepAction,
            this.barButtonItemMnCtrLogTStepMeasuring,
            this.barCheckItemLogSaveMeasuringLog,
            this.barEditItemShowLogLevel,
            this.barCheckItemMeasuringArray,
            this.barCheckItemSaveConsoleLog,
            this.barButtonItemSaveLogPath,
            this.barButtonItemExit,
            this.barButtonItemMnMain,
            this.barCheckItemDebugConsoleOut,
            this.barEditItem_PartID,
            this.barButtonItem_SelectAuto,
            this.barButtonItem_SelectManual,
            this.barButtonItem_AdminDialogClear,
            this.barButtonItem_LVDT,
            this.barButtonItem_Motor1,
            this.barButtonItem_DigtalIO,
            this.barButtonItem_AnalogIO,
            this.barButtonItem_Robot,
            this.barButtonItem_PLC,
            this.barButtonItem_TempHumi,
            this.barButtonItem_LCR,
            this.barButtonItem_PowerSupply1,
            this.barEditItem_TemperatureUpper,
            this.barEditItem_TemperatureLower,
            this.barEditItem_HumidityUpper,
            this.barEditItem_HumidityLower,
            this.barEditItem_CurrentTemp,
            this.barEditItem_CurrentHumidity,
            this.barCheckItemShowColMP,
            this.barCheckItemShowColInf,
            this.barButtonItem_LoadTestList,
            this.barEditItem_AirGapMonitor,
            this.barButtonItem_manualAirGap,
            this.barButtonItem_Motor2,
            this.barButtonItem_Motor3,
            this.barButtonItem_PowerSupply2,
            this.barButtonItem_PowerSupply3,
            this.barEditItem_RPM,
            this.barButtonItemDoorControl,
            this.barButtonItem_RobotReadyMove});
            this.ribbonControlCPTester.Location = new System.Drawing.Point(0, 0);
            this.ribbonControlCPTester.Margin = new System.Windows.Forms.Padding(2);
            this.ribbonControlCPTester.MaxItemId = 160;
            this.ribbonControlCPTester.Name = "ribbonControlCPTester";
            this.ribbonControlCPTester.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageMainControl,
            this.ribbonPageTsOption,
            this.ribbonPageManulControl,
            this.ribbonPageLog,
            this.ribbonPageEnvironmentSetup});
            this.ribbonControlCPTester.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemImageEdit1,
            this.repositoryItemPictureEdit1,
            this.repositoryItemImageEdit2,
            this.repositoryItemPictureEdit2,
            this.repositoryItemProgressBar1,
            this.repositoryItemMarqueeProgressBar1,
            this.repositoryItemRadioGroup1,
            this.repositoryItemComboBox1,
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemTextEdit3,
            this.repositoryItemComboBox2,
            this.repositoryItemTextEdit4,
            this.repositoryItemComboBox3,
            this.repositoryItemTextEdit5,
            this.repositoryItemToggleSwitch1,
            this.repositoryItemToggleSwitch2,
            this.repositoryItemToggleSwitch_AutoManu,
            this.repositoryItemTextEdit6,
            this.repositoryItemTextEdit7,
            this.repositoryItemTextEdit8,
            this.repositoryItemTextEdit9,
            this.repositoryItemTextEdit10,
            this.repositoryItemTextEdit11,
            this.repositoryItemTextEdit12,
            this.repositoryItemTextEdit13,
            this.repositoryItemTextEdit14,
            this.repositoryItemTextEdit15});
            this.ribbonControlCPTester.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.ribbonControlCPTester.Size = new System.Drawing.Size(1878, 143);
            this.ribbonControlCPTester.StatusBar = this.ribbonStatusBarCPMainForm;
            // 
            // barButtonItemLoadCfgFrFile
            // 
            this.barButtonItemLoadCfgFrFile.Caption = "Load Configuration From File";
            this.barButtonItemLoadCfgFrFile.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemLoadCfgFrFile.Glyph")));
            this.barButtonItemLoadCfgFrFile.Id = 1;
            this.barButtonItemLoadCfgFrFile.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemLoadCfgFrFile.LargeGlyph")));
            this.barButtonItemLoadCfgFrFile.LargeWidth = 120;
            this.barButtonItemLoadCfgFrFile.Name = "barButtonItemLoadCfgFrFile";
            this.barButtonItemLoadCfgFrFile.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItemLoadCfgFrFile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemLoadCfgFrFile_ItemClick);
            // 
            // barCheckItemShowDeactivateStep
            // 
            this.barCheckItemShowDeactivateStep.BindableChecked = true;
            this.barCheckItemShowDeactivateStep.Caption = "Deactivated Step";
            this.barCheckItemShowDeactivateStep.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowDeactivateStep.Checked = true;
            this.barCheckItemShowDeactivateStep.Id = 12;
            this.barCheckItemShowDeactivateStep.Name = "barCheckItemShowDeactivateStep";
            this.barCheckItemShowDeactivateStep.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemShowDeactivateStep_CheckedChanged);
            // 
            // skinRibbonGalleryBarItem1
            // 
            this.skinRibbonGalleryBarItem1.Caption = "skinRibbonGalleryBarItem1";
            this.skinRibbonGalleryBarItem1.Id = 13;
            this.skinRibbonGalleryBarItem1.Name = "skinRibbonGalleryBarItem1";
            // 
            // barCheckItemShowColPosition
            // 
            this.barCheckItemShowColPosition.BindableChecked = true;
            this.barCheckItemShowColPosition.Caption = "Position";
            this.barCheckItemShowColPosition.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowColPosition.Checked = true;
            this.barCheckItemShowColPosition.Id = 19;
            this.barCheckItemShowColPosition.Name = "barCheckItemShowColPosition";
            this.barCheckItemShowColPosition.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemShowColPosition_CheckedChanged);
            // 
            // barCheckItemShowColVariant
            // 
            this.barCheckItemShowColVariant.BindableChecked = true;
            this.barCheckItemShowColVariant.Caption = "Variant";
            this.barCheckItemShowColVariant.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowColVariant.Checked = true;
            this.barCheckItemShowColVariant.Id = 20;
            this.barCheckItemShowColVariant.Name = "barCheckItemShowColVariant";
            this.barCheckItemShowColVariant.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemShowColVariant_CheckedChanged);
            // 
            // barCheckItemShowColGate
            // 
            this.barCheckItemShowColGate.BindableChecked = true;
            this.barCheckItemShowColGate.Caption = "Gate";
            this.barCheckItemShowColGate.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowColGate.Checked = true;
            this.barCheckItemShowColGate.Id = 21;
            this.barCheckItemShowColGate.Name = "barCheckItemShowColGate";
            this.barCheckItemShowColGate.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemShowColGate_CheckedChanged);
            // 
            // barCheckItemShowColReturn
            // 
            this.barCheckItemShowColReturn.BindableChecked = true;
            this.barCheckItemShowColReturn.Caption = "Return";
            this.barCheckItemShowColReturn.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowColReturn.Checked = true;
            this.barCheckItemShowColReturn.Id = 22;
            this.barCheckItemShowColReturn.Name = "barCheckItemShowColReturn";
            this.barCheckItemShowColReturn.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemShowColReturn_CheckedChanged);
            // 
            // barCheckItemShowColParameter
            // 
            this.barCheckItemShowColParameter.BindableChecked = true;
            this.barCheckItemShowColParameter.Caption = "Parameter";
            this.barCheckItemShowColParameter.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowColParameter.Checked = true;
            this.barCheckItemShowColParameter.Id = 23;
            this.barCheckItemShowColParameter.Name = "barCheckItemShowColParameter";
            this.barCheckItemShowColParameter.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemShowColParameter_CheckedChanged);
            // 
            // barCheckItemShowColComment
            // 
            this.barCheckItemShowColComment.Caption = "Comment";
            this.barCheckItemShowColComment.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowColComment.Id = 24;
            this.barCheckItemShowColComment.Name = "barCheckItemShowColComment";
            this.barCheckItemShowColComment.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemShowColComment_CheckedChanged);
            // 
            // skinRibbonGalleryBarItem2
            // 
            this.skinRibbonGalleryBarItem2.Caption = "skinRibbonGalleryBarItem2";
            this.skinRibbonGalleryBarItem2.Id = 26;
            this.skinRibbonGalleryBarItem2.Name = "skinRibbonGalleryBarItem2";
            // 
            // barButtonItemMnCtrStart
            // 
            this.barButtonItemMnCtrStart.Caption = "Start (F1)";
            this.barButtonItemMnCtrStart.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrStart.Glyph")));
            this.barButtonItemMnCtrStart.Id = 35;
            this.barButtonItemMnCtrStart.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrStart.LargeGlyph")));
            this.barButtonItemMnCtrStart.LargeWidth = 120;
            this.barButtonItemMnCtrStart.Name = "barButtonItemMnCtrStart";
            this.barButtonItemMnCtrStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemMnCtrStart_ItemClick);
            // 
            // barButtonItemMnCtrStop
            // 
            this.barButtonItemMnCtrStop.Caption = " Stop (F4)";
            this.barButtonItemMnCtrStop.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrStop.Glyph")));
            this.barButtonItemMnCtrStop.Id = 36;
            this.barButtonItemMnCtrStop.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrStop.LargeGlyph")));
            this.barButtonItemMnCtrStop.LargeWidth = 120;
            this.barButtonItemMnCtrStop.Name = "barButtonItemMnCtrStop";
            this.barButtonItemMnCtrStop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemMnCtrStop_ItemClick);
            // 
            // barButtonItemMnCtrTsConditon
            // 
            this.barButtonItemMnCtrTsConditon.Caption = "Test Condition (F6)";
            this.barButtonItemMnCtrTsConditon.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrTsConditon.Glyph")));
            this.barButtonItemMnCtrTsConditon.Id = 53;
            this.barButtonItemMnCtrTsConditon.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrTsConditon.LargeGlyph")));
            this.barButtonItemMnCtrTsConditon.LargeWidth = 120;
            this.barButtonItemMnCtrTsConditon.Name = "barButtonItemMnCtrTsConditon";
            this.barButtonItemMnCtrTsConditon.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItemMnCtrTsConditon.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemMnCtrTsConditon_ItemClick);
            // 
            // barButtonItemMnCtrManualSignalAnalysis
            // 
            this.barButtonItemMnCtrManualSignalAnalysis.Caption = "Signal Analysis";
            this.barButtonItemMnCtrManualSignalAnalysis.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrManualSignalAnalysis.Glyph")));
            this.barButtonItemMnCtrManualSignalAnalysis.Id = 71;
            this.barButtonItemMnCtrManualSignalAnalysis.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrManualSignalAnalysis.LargeGlyph")));
            this.barButtonItemMnCtrManualSignalAnalysis.LargeWidth = 120;
            this.barButtonItemMnCtrManualSignalAnalysis.Name = "barButtonItemMnCtrManualSignalAnalysis";
            this.barButtonItemMnCtrManualSignalAnalysis.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemMnCtrManualSignalAnalysis_ItemClick);
            // 
            // barCheckItemLogCANMsg
            // 
            this.barCheckItemLogCANMsg.BindableChecked = true;
            this.barCheckItemLogCANMsg.Caption = "CAN Message";
            this.barCheckItemLogCANMsg.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemLogCANMsg.Checked = true;
            this.barCheckItemLogCANMsg.Id = 73;
            this.barCheckItemLogCANMsg.Name = "barCheckItemLogCANMsg";
            // 
            // barButtonItemMnCtrLogTStepAction
            // 
            this.barButtonItemMnCtrLogTStepAction.Caption = "Action Log";
            this.barButtonItemMnCtrLogTStepAction.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrLogTStepAction.Glyph")));
            this.barButtonItemMnCtrLogTStepAction.Id = 75;
            this.barButtonItemMnCtrLogTStepAction.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrLogTStepAction.LargeGlyph")));
            this.barButtonItemMnCtrLogTStepAction.LargeWidth = 100;
            this.barButtonItemMnCtrLogTStepAction.Name = "barButtonItemMnCtrLogTStepAction";
            this.barButtonItemMnCtrLogTStepAction.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemMnCtrLogTStepAction_ItemClick);
            // 
            // barButtonItemMnCtrLogTStepMeasuring
            // 
            this.barButtonItemMnCtrLogTStepMeasuring.Caption = "Measuring Log";
            this.barButtonItemMnCtrLogTStepMeasuring.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrLogTStepMeasuring.Glyph")));
            this.barButtonItemMnCtrLogTStepMeasuring.Id = 76;
            this.barButtonItemMnCtrLogTStepMeasuring.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnCtrLogTStepMeasuring.LargeGlyph")));
            this.barButtonItemMnCtrLogTStepMeasuring.LargeWidth = 100;
            this.barButtonItemMnCtrLogTStepMeasuring.Name = "barButtonItemMnCtrLogTStepMeasuring";
            this.barButtonItemMnCtrLogTStepMeasuring.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemMnCtrLogTStepMeasuring_ItemClick);
            // 
            // barCheckItemLogSaveMeasuringLog
            // 
            this.barCheckItemLogSaveMeasuringLog.BindableChecked = true;
            this.barCheckItemLogSaveMeasuringLog.Caption = "Save measuring full log";
            this.barCheckItemLogSaveMeasuringLog.Checked = true;
            this.barCheckItemLogSaveMeasuringLog.Glyph = ((System.Drawing.Image)(resources.GetObject("barCheckItemLogSaveMeasuringLog.Glyph")));
            this.barCheckItemLogSaveMeasuringLog.Id = 77;
            this.barCheckItemLogSaveMeasuringLog.LargeWidth = 100;
            this.barCheckItemLogSaveMeasuringLog.Name = "barCheckItemLogSaveMeasuringLog";
            this.barCheckItemLogSaveMeasuringLog.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.barCheckItemLogSaveMeasuringLog.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemLogSaveMeasuringLog_CheckedChanged);
            // 
            // barEditItemShowLogLevel
            // 
            this.barEditItemShowLogLevel.Caption = "Log Level  ";
            this.barEditItemShowLogLevel.Edit = this.repositoryItemComboBox2;
            this.barEditItemShowLogLevel.EditWidth = 80;
            this.barEditItemShowLogLevel.Id = 78;
            this.barEditItemShowLogLevel.Name = "barEditItemShowLogLevel";
            this.barEditItemShowLogLevel.EditValueChanged += new System.EventHandler(this.barEditItemShowLogLevel_EditValueChanged);
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            this.repositoryItemComboBox2.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // barCheckItemMeasuringArray
            // 
            this.barCheckItemMeasuringArray.Caption = "Save measuring array";
            this.barCheckItemMeasuringArray.Glyph = ((System.Drawing.Image)(resources.GetObject("barCheckItemMeasuringArray.Glyph")));
            this.barCheckItemMeasuringArray.Id = 79;
            this.barCheckItemMeasuringArray.LargeWidth = 100;
            this.barCheckItemMeasuringArray.Name = "barCheckItemMeasuringArray";
            this.barCheckItemMeasuringArray.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barCheckItemMeasuringArray.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemMeasuringArray_CheckedChanged);
            // 
            // barCheckItemSaveConsoleLog
            // 
            this.barCheckItemSaveConsoleLog.Caption = "Save to File logs";
            this.barCheckItemSaveConsoleLog.Glyph = ((System.Drawing.Image)(resources.GetObject("barCheckItemSaveConsoleLog.Glyph")));
            this.barCheckItemSaveConsoleLog.Id = 84;
            this.barCheckItemSaveConsoleLog.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barCheckItemSaveConsoleLog.LargeGlyph")));
            this.barCheckItemSaveConsoleLog.LargeWidth = 100;
            this.barCheckItemSaveConsoleLog.Name = "barCheckItemSaveConsoleLog";
            this.barCheckItemSaveConsoleLog.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemSaveConsoleLog_CheckedChanged);
            // 
            // barButtonItemSaveLogPath
            // 
            this.barButtonItemSaveLogPath.Caption = "Path to Save Logs";
            this.barButtonItemSaveLogPath.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemSaveLogPath.Glyph")));
            this.barButtonItemSaveLogPath.Id = 86;
            this.barButtonItemSaveLogPath.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemSaveLogPath.LargeGlyph")));
            this.barButtonItemSaveLogPath.LargeWidth = 100;
            this.barButtonItemSaveLogPath.Name = "barButtonItemSaveLogPath";
            this.barButtonItemSaveLogPath.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSaveLogPath_ItemClick);
            // 
            // barButtonItemExit
            // 
            this.barButtonItemExit.Caption = "Exit Application";
            this.barButtonItemExit.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemExit.Glyph")));
            this.barButtonItemExit.Id = 98;
            this.barButtonItemExit.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemExit.LargeGlyph")));
            this.barButtonItemExit.LargeWidth = 120;
            this.barButtonItemExit.Name = "barButtonItemExit";
            this.barButtonItemExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemExit_ItemClick);
            // 
            // barButtonItemMnMain
            // 
            this.barButtonItemMnMain.Caption = "Control";
            this.barButtonItemMnMain.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnMain.Glyph")));
            this.barButtonItemMnMain.Id = 106;
            this.barButtonItemMnMain.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemMnMain.LargeGlyph")));
            this.barButtonItemMnMain.LargeWidth = 120;
            this.barButtonItemMnMain.Name = "barButtonItemMnMain";
            this.barButtonItemMnMain.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemMnMain_ItemClick);
            // 
            // barCheckItemDebugConsoleOut
            // 
            this.barCheckItemDebugConsoleOut.BindableChecked = true;
            this.barCheckItemDebugConsoleOut.Caption = "Show Output";
            this.barCheckItemDebugConsoleOut.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemDebugConsoleOut.Checked = true;
            this.barCheckItemDebugConsoleOut.CheckStyle = DevExpress.XtraBars.BarCheckStyles.Radio;
            this.barCheckItemDebugConsoleOut.Glyph = ((System.Drawing.Image)(resources.GetObject("barCheckItemDebugConsoleOut.Glyph")));
            this.barCheckItemDebugConsoleOut.Id = 108;
            this.barCheckItemDebugConsoleOut.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barCheckItemDebugConsoleOut.LargeGlyph")));
            this.barCheckItemDebugConsoleOut.Name = "barCheckItemDebugConsoleOut";
            this.barCheckItemDebugConsoleOut.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemDebugConsoleOut_CheckedChanged);
            // 
            // barEditItem_PartID
            // 
            this.barEditItem_PartID.Caption = "Part ID";
            this.barEditItem_PartID.Edit = this.repositoryItemTextEdit5;
            this.barEditItem_PartID.EditWidth = 100;
            this.barEditItem_PartID.Id = 110;
            this.barEditItem_PartID.Name = "barEditItem_PartID";
            this.barEditItem_PartID.EditValueChanged += new System.EventHandler(this.barEditItem_PartID_EditValueChanged);
            // 
            // repositoryItemTextEdit5
            // 
            this.repositoryItemTextEdit5.AutoHeight = false;
            this.repositoryItemTextEdit5.Name = "repositoryItemTextEdit5";
            // 
            // barButtonItem_SelectAuto
            // 
            this.barButtonItem_SelectAuto.Caption = "Auto";
            this.barButtonItem_SelectAuto.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_SelectAuto.Glyph")));
            this.barButtonItem_SelectAuto.Id = 115;
            this.barButtonItem_SelectAuto.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_SelectAuto.LargeGlyph")));
            this.barButtonItem_SelectAuto.LargeWidth = 120;
            this.barButtonItem_SelectAuto.Name = "barButtonItem_SelectAuto";
            this.barButtonItem_SelectAuto.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_SelectAuto_ItemClick);
            // 
            // barButtonItem_SelectManual
            // 
            this.barButtonItem_SelectManual.Caption = "Manual";
            this.barButtonItem_SelectManual.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_SelectManual.Glyph")));
            this.barButtonItem_SelectManual.Id = 116;
            this.barButtonItem_SelectManual.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_SelectManual.LargeGlyph")));
            this.barButtonItem_SelectManual.LargeWidth = 120;
            this.barButtonItem_SelectManual.Name = "barButtonItem_SelectManual";
            this.barButtonItem_SelectManual.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_SelectManual_ItemClick);
            // 
            // barButtonItem_AdminDialogClear
            // 
            this.barButtonItem_AdminDialogClear.Caption = "Message Dialog Clear";
            this.barButtonItem_AdminDialogClear.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_AdminDialogClear.Glyph")));
            this.barButtonItem_AdminDialogClear.Id = 117;
            this.barButtonItem_AdminDialogClear.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_AdminDialogClear.LargeGlyph")));
            this.barButtonItem_AdminDialogClear.LargeWidth = 100;
            this.barButtonItem_AdminDialogClear.Name = "barButtonItem_AdminDialogClear";
            this.barButtonItem_AdminDialogClear.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_AdminDialogClear_ItemClick);
            // 
            // barButtonItem_LVDT
            // 
            this.barButtonItem_LVDT.Caption = "LVDT";
            this.barButtonItem_LVDT.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_LVDT.Glyph")));
            this.barButtonItem_LVDT.Id = 121;
            this.barButtonItem_LVDT.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_LVDT.LargeGlyph")));
            this.barButtonItem_LVDT.LargeWidth = 70;
            this.barButtonItem_LVDT.Name = "barButtonItem_LVDT";
            this.barButtonItem_LVDT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_LVDT_ItemClick);
            // 
            // barButtonItem_Motor1
            // 
            this.barButtonItem_Motor1.Caption = "Motor1";
            this.barButtonItem_Motor1.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Motor1.Glyph")));
            this.barButtonItem_Motor1.Id = 122;
            this.barButtonItem_Motor1.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Motor1.LargeGlyph")));
            this.barButtonItem_Motor1.LargeWidth = 70;
            this.barButtonItem_Motor1.Name = "barButtonItem_Motor1";
            this.barButtonItem_Motor1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_Motor_ItemClick);
            // 
            // barButtonItem_DigtalIO
            // 
            this.barButtonItem_DigtalIO.Caption = "Digtal IO";
            this.barButtonItem_DigtalIO.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_DigtalIO.Glyph")));
            this.barButtonItem_DigtalIO.Id = 123;
            this.barButtonItem_DigtalIO.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_DigtalIO.LargeGlyph")));
            this.barButtonItem_DigtalIO.LargeWidth = 70;
            this.barButtonItem_DigtalIO.Name = "barButtonItem_DigtalIO";
            this.barButtonItem_DigtalIO.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_DigtalIO_ItemClick);
            // 
            // barButtonItem_AnalogIO
            // 
            this.barButtonItem_AnalogIO.Caption = "Analog IO";
            this.barButtonItem_AnalogIO.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_AnalogIO.Glyph")));
            this.barButtonItem_AnalogIO.Id = 124;
            this.barButtonItem_AnalogIO.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_AnalogIO.LargeGlyph")));
            this.barButtonItem_AnalogIO.LargeWidth = 70;
            this.barButtonItem_AnalogIO.Name = "barButtonItem_AnalogIO";
            this.barButtonItem_AnalogIO.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_AnalogIO_ItemClick);
            // 
            // barButtonItem_Robot
            // 
            this.barButtonItem_Robot.Caption = "Robot";
            this.barButtonItem_Robot.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Robot.Glyph")));
            this.barButtonItem_Robot.Id = 125;
            this.barButtonItem_Robot.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Robot.LargeGlyph")));
            this.barButtonItem_Robot.LargeWidth = 70;
            this.barButtonItem_Robot.Name = "barButtonItem_Robot";
            this.barButtonItem_Robot.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_Robot_ItemClick);
            // 
            // barButtonItem_PLC
            // 
            this.barButtonItem_PLC.Caption = "PLC";
            this.barButtonItem_PLC.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_PLC.Glyph")));
            this.barButtonItem_PLC.Id = 126;
            this.barButtonItem_PLC.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_PLC.LargeGlyph")));
            this.barButtonItem_PLC.LargeWidth = 100;
            this.barButtonItem_PLC.Name = "barButtonItem_PLC";
            this.barButtonItem_PLC.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_PLC_ItemClick);
            // 
            // barButtonItem_TempHumi
            // 
            this.barButtonItem_TempHumi.Caption = "Temp && Humi";
            this.barButtonItem_TempHumi.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_TempHumi.Glyph")));
            this.barButtonItem_TempHumi.Id = 127;
            this.barButtonItem_TempHumi.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_TempHumi.LargeGlyph")));
            this.barButtonItem_TempHumi.LargeWidth = 100;
            this.barButtonItem_TempHumi.Name = "barButtonItem_TempHumi";
            this.barButtonItem_TempHumi.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_TempHumi_ItemClick);
            // 
            // barButtonItem_LCR
            // 
            this.barButtonItem_LCR.Caption = "LCR";
            this.barButtonItem_LCR.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_LCR.Glyph")));
            this.barButtonItem_LCR.Id = 128;
            this.barButtonItem_LCR.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_LCR.LargeGlyph")));
            this.barButtonItem_LCR.LargeWidth = 70;
            this.barButtonItem_LCR.Name = "barButtonItem_LCR";
            this.barButtonItem_LCR.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_LCR_ItemClick);
            // 
            // barButtonItem_PowerSupply1
            // 
            this.barButtonItem_PowerSupply1.Caption = "Power Supply1";
            this.barButtonItem_PowerSupply1.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_PowerSupply1.Glyph")));
            this.barButtonItem_PowerSupply1.Id = 129;
            this.barButtonItem_PowerSupply1.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_PowerSupply1.LargeGlyph")));
            this.barButtonItem_PowerSupply1.LargeWidth = 70;
            this.barButtonItem_PowerSupply1.Name = "barButtonItem_PowerSupply1";
            this.barButtonItem_PowerSupply1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_PowerSupply_ItemClick);
            // 
            // barEditItem_TemperatureUpper
            // 
            this.barEditItem_TemperatureUpper.Caption = "Temperature Upper";
            this.barEditItem_TemperatureUpper.Edit = this.repositoryItemTextEdit6;
            this.barEditItem_TemperatureUpper.EditValue = "40";
            this.barEditItem_TemperatureUpper.Id = 132;
            this.barEditItem_TemperatureUpper.Name = "barEditItem_TemperatureUpper";
            // 
            // repositoryItemTextEdit6
            // 
            this.repositoryItemTextEdit6.AutoHeight = false;
            this.repositoryItemTextEdit6.Name = "repositoryItemTextEdit6";
            // 
            // barEditItem_TemperatureLower
            // 
            this.barEditItem_TemperatureLower.Caption = "Temperature Lower";
            this.barEditItem_TemperatureLower.Edit = this.repositoryItemTextEdit7;
            this.barEditItem_TemperatureLower.EditValue = "1";
            this.barEditItem_TemperatureLower.Id = 133;
            this.barEditItem_TemperatureLower.Name = "barEditItem_TemperatureLower";
            // 
            // repositoryItemTextEdit7
            // 
            this.repositoryItemTextEdit7.AutoHeight = false;
            this.repositoryItemTextEdit7.Name = "repositoryItemTextEdit7";
            // 
            // barEditItem_HumidityUpper
            // 
            this.barEditItem_HumidityUpper.Caption = "Humidity Upper";
            this.barEditItem_HumidityUpper.Edit = this.repositoryItemTextEdit8;
            this.barEditItem_HumidityUpper.EditValue = "85";
            this.barEditItem_HumidityUpper.Id = 134;
            this.barEditItem_HumidityUpper.Name = "barEditItem_HumidityUpper";
            // 
            // repositoryItemTextEdit8
            // 
            this.repositoryItemTextEdit8.AutoHeight = false;
            this.repositoryItemTextEdit8.Name = "repositoryItemTextEdit8";
            // 
            // barEditItem_HumidityLower
            // 
            this.barEditItem_HumidityLower.Caption = "Humidity Lower";
            this.barEditItem_HumidityLower.Edit = this.repositoryItemTextEdit9;
            this.barEditItem_HumidityLower.EditValue = "1";
            this.barEditItem_HumidityLower.Id = 135;
            this.barEditItem_HumidityLower.Name = "barEditItem_HumidityLower";
            // 
            // repositoryItemTextEdit9
            // 
            this.repositoryItemTextEdit9.AutoHeight = false;
            this.repositoryItemTextEdit9.Name = "repositoryItemTextEdit9";
            // 
            // barEditItem_CurrentTemp
            // 
            this.barEditItem_CurrentTemp.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.False;
            this.barEditItem_CurrentTemp.Caption = "Current temperature ";
            this.barEditItem_CurrentTemp.Edit = this.repositoryItemTextEdit12;
            this.barEditItem_CurrentTemp.Enabled = false;
            this.barEditItem_CurrentTemp.Glyph = ((System.Drawing.Image)(resources.GetObject("barEditItem_CurrentTemp.Glyph")));
            this.barEditItem_CurrentTemp.Id = 138;
            this.barEditItem_CurrentTemp.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barEditItem_CurrentTemp.LargeGlyph")));
            this.barEditItem_CurrentTemp.Name = "barEditItem_CurrentTemp";
            // 
            // repositoryItemTextEdit12
            // 
            this.repositoryItemTextEdit12.AutoHeight = false;
            this.repositoryItemTextEdit12.Name = "repositoryItemTextEdit12";
            // 
            // barEditItem_CurrentHumidity
            // 
            this.barEditItem_CurrentHumidity.Caption = "Current  humidity       ";
            this.barEditItem_CurrentHumidity.Edit = this.repositoryItemTextEdit13;
            this.barEditItem_CurrentHumidity.Enabled = false;
            this.barEditItem_CurrentHumidity.Glyph = ((System.Drawing.Image)(resources.GetObject("barEditItem_CurrentHumidity.Glyph")));
            this.barEditItem_CurrentHumidity.Id = 139;
            this.barEditItem_CurrentHumidity.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barEditItem_CurrentHumidity.LargeGlyph")));
            this.barEditItem_CurrentHumidity.Name = "barEditItem_CurrentHumidity";
            this.barEditItem_CurrentHumidity.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            // 
            // repositoryItemTextEdit13
            // 
            this.repositoryItemTextEdit13.AutoHeight = false;
            this.repositoryItemTextEdit13.Name = "repositoryItemTextEdit13";
            // 
            // barCheckItemShowColMP
            // 
            this.barCheckItemShowColMP.BindableChecked = true;
            this.barCheckItemShowColMP.Caption = "Meas.Parmater";
            this.barCheckItemShowColMP.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowColMP.Checked = true;
            this.barCheckItemShowColMP.Id = 144;
            this.barCheckItemShowColMP.Name = "barCheckItemShowColMP";
            this.barCheckItemShowColMP.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemShowColMP_CheckedChanged);
            // 
            // barCheckItemShowColInf
            // 
            this.barCheckItemShowColInf.BindableChecked = true;
            this.barCheckItemShowColInf.Caption = "Info";
            this.barCheckItemShowColInf.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItemShowColInf.Checked = true;
            this.barCheckItemShowColInf.Id = 145;
            this.barCheckItemShowColInf.Name = "barCheckItemShowColInf";
            this.barCheckItemShowColInf.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItemInf_CheckedChanged);
            // 
            // barButtonItem_LoadTestList
            // 
            this.barButtonItem_LoadTestList.Caption = "Load TestList";
            this.barButtonItem_LoadTestList.Id = 148;
            this.barButtonItem_LoadTestList.ImageUri.Uri = "ListBullets";
            this.barButtonItem_LoadTestList.LargeWidth = 120;
            this.barButtonItem_LoadTestList.Name = "barButtonItem_LoadTestList";
            this.barButtonItem_LoadTestList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_LoadTestList_ItemClick);
            // 
            // barEditItem_AirGapMonitor
            // 
            this.barEditItem_AirGapMonitor.Caption = "Current Air Gap          ";
            this.barEditItem_AirGapMonitor.Edit = this.repositoryItemTextEdit14;
            this.barEditItem_AirGapMonitor.EditWidth = 70;
            this.barEditItem_AirGapMonitor.Enabled = false;
            this.barEditItem_AirGapMonitor.Glyph = ((System.Drawing.Image)(resources.GetObject("barEditItem_AirGapMonitor.Glyph")));
            this.barEditItem_AirGapMonitor.Id = 150;
            this.barEditItem_AirGapMonitor.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barEditItem_AirGapMonitor.LargeGlyph")));
            this.barEditItem_AirGapMonitor.Name = "barEditItem_AirGapMonitor";
            // 
            // repositoryItemTextEdit14
            // 
            this.repositoryItemTextEdit14.AutoHeight = false;
            this.repositoryItemTextEdit14.Name = "repositoryItemTextEdit14";
            // 
            // barButtonItem_manualAirGap
            // 
            this.barButtonItem_manualAirGap.Caption = "Setting Air Gap";
            this.barButtonItem_manualAirGap.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_manualAirGap.Glyph")));
            this.barButtonItem_manualAirGap.Id = 151;
            this.barButtonItem_manualAirGap.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_manualAirGap.LargeGlyph")));
            this.barButtonItem_manualAirGap.Name = "barButtonItem_manualAirGap";
            this.barButtonItem_manualAirGap.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_manualAirGap_ItemClick);
            // 
            // barButtonItem_Motor2
            // 
            this.barButtonItem_Motor2.Caption = "Motor2";
            this.barButtonItem_Motor2.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Motor2.Glyph")));
            this.barButtonItem_Motor2.Id = 153;
            this.barButtonItem_Motor2.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Motor2.LargeGlyph")));
            this.barButtonItem_Motor2.LargeWidth = 70;
            this.barButtonItem_Motor2.Name = "barButtonItem_Motor2";
            this.barButtonItem_Motor2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_Motor2_ItemClick);
            // 
            // barButtonItem_Motor3
            // 
            this.barButtonItem_Motor3.Caption = "Motor3";
            this.barButtonItem_Motor3.Id = 154;
            this.barButtonItem_Motor3.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Motor3.LargeGlyph")));
            this.barButtonItem_Motor3.LargeWidth = 70;
            this.barButtonItem_Motor3.Name = "barButtonItem_Motor3";
            this.barButtonItem_Motor3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_Motor3_ItemClick);
            // 
            // barButtonItem_PowerSupply2
            // 
            this.barButtonItem_PowerSupply2.Caption = "Power Supply2";
            this.barButtonItem_PowerSupply2.Id = 155;
            this.barButtonItem_PowerSupply2.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_PowerSupply2.LargeGlyph")));
            this.barButtonItem_PowerSupply2.LargeWidth = 70;
            this.barButtonItem_PowerSupply2.Name = "barButtonItem_PowerSupply2";
            this.barButtonItem_PowerSupply2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_PowerSupply2_ItemClick);
            // 
            // barButtonItem_PowerSupply3
            // 
            this.barButtonItem_PowerSupply3.Caption = "Power Supply3";
            this.barButtonItem_PowerSupply3.Id = 156;
            this.barButtonItem_PowerSupply3.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_PowerSupply3.LargeGlyph")));
            this.barButtonItem_PowerSupply3.LargeWidth = 70;
            this.barButtonItem_PowerSupply3.Name = "barButtonItem_PowerSupply3";
            this.barButtonItem_PowerSupply3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_PowerSupply3_ItemClick);
            // 
            // barEditItem_RPM
            // 
            this.barEditItem_RPM.Caption = "Currunt Motor RPM    ";
            this.barEditItem_RPM.Edit = this.repositoryItemTextEdit15;
            this.barEditItem_RPM.EditWidth = 70;
            this.barEditItem_RPM.Enabled = false;
            this.barEditItem_RPM.Glyph = ((System.Drawing.Image)(resources.GetObject("barEditItem_RPM.Glyph")));
            this.barEditItem_RPM.Id = 157;
            this.barEditItem_RPM.Name = "barEditItem_RPM";
            // 
            // repositoryItemTextEdit15
            // 
            this.repositoryItemTextEdit15.AutoHeight = false;
            this.repositoryItemTextEdit15.Name = "repositoryItemTextEdit15";
            // 
            // barButtonItemDoorControl
            // 
            this.barButtonItemDoorControl.Caption = "Door control";
            this.barButtonItemDoorControl.Id = 158;
            this.barButtonItemDoorControl.Name = "barButtonItemDoorControl";
            this.barButtonItemDoorControl.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDoorControl_ItemClick);
            // 
            // ribbonPageMainControl
            // 
            this.ribbonPageMainControl.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupFileControl,
            this.ribbonPageGroupTestCondition,
            this.ribbonPageGroup3,
            this.ribbonPageGroupMnCtrTesterControl,
            this.ribbonPageGroup5});
            this.ribbonPageMainControl.Name = "ribbonPageMainControl";
            this.ribbonPageMainControl.Text = "Main Control";
            // 
            // ribbonPageGroupFileControl
            // 
            this.ribbonPageGroupFileControl.ItemLinks.Add(this.barButtonItem_LoadTestList);
            this.ribbonPageGroupFileControl.ItemLinks.Add(this.barButtonItemLoadCfgFrFile);
            this.ribbonPageGroupFileControl.ItemLinks.Add(this.barButtonItemExit);
            this.ribbonPageGroupFileControl.Name = "ribbonPageGroupFileControl";
            this.ribbonPageGroupFileControl.Text = "File Control";
            // 
            // ribbonPageGroupTestCondition
            // 
            this.ribbonPageGroupTestCondition.ItemLinks.Add(this.barButtonItemMnCtrTsConditon);
            this.ribbonPageGroupTestCondition.Name = "ribbonPageGroupTestCondition";
            this.ribbonPageGroupTestCondition.Text = "Test Condition";
            this.ribbonPageGroupTestCondition.Visible = false;
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem_SelectAuto);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem_SelectManual);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Select Mode";
            // 
            // ribbonPageGroupMnCtrTesterControl
            // 
            this.ribbonPageGroupMnCtrTesterControl.ItemLinks.Add(this.barButtonItemMnCtrStart);
            this.ribbonPageGroupMnCtrTesterControl.ItemLinks.Add(this.barButtonItemMnCtrStop);
            this.ribbonPageGroupMnCtrTesterControl.ItemLinks.Add(this.barButtonItem_RobotReadyMove);
            this.ribbonPageGroupMnCtrTesterControl.Name = "ribbonPageGroupMnCtrTesterControl";
            this.ribbonPageGroupMnCtrTesterControl.Text = "Tester Control";
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.barEditItem_CurrentTemp);
            this.ribbonPageGroup5.ItemLinks.Add(this.barEditItem_CurrentHumidity);
            this.ribbonPageGroup5.ItemLinks.Add(this.barButtonItem_manualAirGap);
            this.ribbonPageGroup5.ItemLinks.Add(this.barEditItem_AirGapMonitor);
            this.ribbonPageGroup5.ItemLinks.Add(this.barEditItem_RPM);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.Text = "Environment";
            // 
            // ribbonPageTsOption
            // 
            this.ribbonPageTsOption.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupShowSteps,
            this.ribbonPageGroupShowColumn});
            this.ribbonPageTsOption.Name = "ribbonPageTsOption";
            this.ribbonPageTsOption.Text = "Test-List Option";
            // 
            // ribbonPageGroupShowSteps
            // 
            this.ribbonPageGroupShowSteps.Enabled = false;
            this.ribbonPageGroupShowSteps.ItemLinks.Add(this.barCheckItemShowDeactivateStep);
            this.ribbonPageGroupShowSteps.Name = "ribbonPageGroupShowSteps";
            this.ribbonPageGroupShowSteps.Text = "Show Step";
            // 
            // ribbonPageGroupShowColumn
            // 
            this.ribbonPageGroupShowColumn.ItemLinks.Add(this.barCheckItemShowColPosition);
            this.ribbonPageGroupShowColumn.ItemLinks.Add(this.barCheckItemShowColVariant);
            this.ribbonPageGroupShowColumn.ItemLinks.Add(this.barCheckItemShowColGate);
            this.ribbonPageGroupShowColumn.ItemLinks.Add(this.barCheckItemShowColMP);
            this.ribbonPageGroupShowColumn.ItemLinks.Add(this.barCheckItemShowColReturn);
            this.ribbonPageGroupShowColumn.ItemLinks.Add(this.barCheckItemShowColParameter);
            this.ribbonPageGroupShowColumn.ItemLinks.Add(this.barCheckItemShowColInf);
            this.ribbonPageGroupShowColumn.ItemLinks.Add(this.barCheckItemShowColComment);
            this.ribbonPageGroupShowColumn.Name = "ribbonPageGroupShowColumn";
            this.ribbonPageGroupShowColumn.Text = "Show Column";
            // 
            // ribbonPageManulControl
            // 
            this.ribbonPageManulControl.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupManualCtrComm,
            this.ribbonPageGroup_Monitor});
            this.ribbonPageManulControl.Name = "ribbonPageManulControl";
            this.ribbonPageManulControl.Text = "Manual Control";
            // 
            // ribbonPageGroupManualCtrComm
            // 
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_LCR);
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_DigtalIO);
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_AnalogIO);
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_Motor1);
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_Motor2);
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_Motor3);
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_PowerSupply1);
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_PowerSupply2);
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_PowerSupply3);
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_LVDT);
            this.ribbonPageGroupManualCtrComm.ItemLinks.Add(this.barButtonItem_Robot);
            this.ribbonPageGroupManualCtrComm.Name = "ribbonPageGroupManualCtrComm";
            this.ribbonPageGroupManualCtrComm.Text = "Manual";
            // 
            // ribbonPageGroup_Monitor
            // 
            this.ribbonPageGroup_Monitor.ItemLinks.Add(this.barButtonItem_TempHumi);
            this.ribbonPageGroup_Monitor.ItemLinks.Add(this.barButtonItem_PLC);
            this.ribbonPageGroup_Monitor.Name = "ribbonPageGroup_Monitor";
            this.ribbonPageGroup_Monitor.Text = "Monitor";
            this.ribbonPageGroup_Monitor.Visible = false;
            // 
            // ribbonPageLog
            // 
            this.ribbonPageLog.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupMnCtrLogOptions,
            this.ribbonPageGroup1,
            this.ribbonPageGroup_id});
            this.ribbonPageLog.Name = "ribbonPageLog";
            this.ribbonPageLog.Text = "Log";
            // 
            // ribbonPageGroupMnCtrLogOptions
            // 
            this.ribbonPageGroupMnCtrLogOptions.ItemLinks.Add(this.barCheckItemLogSaveMeasuringLog);
            this.ribbonPageGroupMnCtrLogOptions.ItemLinks.Add(this.barCheckItemMeasuringArray);
            this.ribbonPageGroupMnCtrLogOptions.ItemLinks.Add(this.barCheckItemSaveConsoleLog);
            this.ribbonPageGroupMnCtrLogOptions.ItemLinks.Add(this.barButtonItemSaveLogPath);
            this.ribbonPageGroupMnCtrLogOptions.Name = "ribbonPageGroupMnCtrLogOptions";
            this.ribbonPageGroupMnCtrLogOptions.Text = "Log Options";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemMnCtrLogTStepAction);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemMnCtrLogTStepMeasuring);
            this.ribbonPageGroup1.ItemLinks.Add(this.barCheckItemDebugConsoleOut);
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemShowLogLevel);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Log View";
            // 
            // ribbonPageGroup_id
            // 
            this.ribbonPageGroup_id.ItemLinks.Add(this.barEditItem_PartID);
            this.ribbonPageGroup_id.Name = "ribbonPageGroup_id";
            this.ribbonPageGroup_id.Text = "Log Information";
            // 
            // ribbonPageEnvironmentSetup
            // 
            this.ribbonPageEnvironmentSetup.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupEvnSetupTheme,
            this.ribbonPageGroup4,
            this.ribbonPageGroup6,
            this.ribbonPageGroupDoor});
            this.ribbonPageEnvironmentSetup.Name = "ribbonPageEnvironmentSetup";
            this.ribbonPageEnvironmentSetup.Text = "Environment Setup";
            // 
            // ribbonPageGroupEvnSetupTheme
            // 
            this.ribbonPageGroupEvnSetupTheme.ItemLinks.Add(this.skinRibbonGalleryBarItem2);
            this.ribbonPageGroupEvnSetupTheme.Name = "ribbonPageGroupEvnSetupTheme";
            this.ribbonPageGroupEvnSetupTheme.Text = "Theme";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.barEditItem_HumidityLower);
            this.ribbonPageGroup4.ItemLinks.Add(this.barEditItem_HumidityUpper);
            this.ribbonPageGroup4.ItemLinks.Add(this.barEditItem_TemperatureLower, true);
            this.ribbonPageGroup4.ItemLinks.Add(this.barEditItem_TemperatureUpper);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "Environment";
            // 
            // ribbonPageGroup6
            // 
            this.ribbonPageGroup6.ItemLinks.Add(this.barButtonItem_AdminDialogClear);
            this.ribbonPageGroup6.Name = "ribbonPageGroup6";
            this.ribbonPageGroup6.Text = "Administrator";
            this.ribbonPageGroup6.Visible = false;
            // 
            // ribbonPageGroupDoor
            // 
            this.ribbonPageGroupDoor.ItemLinks.Add(this.barButtonItemDoorControl);
            this.ribbonPageGroupDoor.Name = "ribbonPageGroupDoor";
            this.ribbonPageGroupDoor.Text = "Door control";
            // 
            // repositoryItemImageEdit1
            // 
            this.repositoryItemImageEdit1.AutoHeight = false;
            this.repositoryItemImageEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageEdit1.Name = "repositoryItemImageEdit1";
            this.repositoryItemImageEdit1.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("repositoryItemPictureEdit1.Appearance.Image")));
            this.repositoryItemPictureEdit1.Appearance.Options.UseImage = true;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.ZoomAccelerationFactor = 1D;
            this.repositoryItemPictureEdit1.ZoomPercent = 200D;
            // 
            // repositoryItemImageEdit2
            // 
            this.repositoryItemImageEdit2.AutoHeight = false;
            this.repositoryItemImageEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageEdit2.Name = "repositoryItemImageEdit2";
            // 
            // repositoryItemPictureEdit2
            // 
            this.repositoryItemPictureEdit2.Name = "repositoryItemPictureEdit2";
            this.repositoryItemPictureEdit2.ZoomAccelerationFactor = 1D;
            // 
            // repositoryItemProgressBar1
            // 
            this.repositoryItemProgressBar1.Name = "repositoryItemProgressBar1";
            // 
            // repositoryItemMarqueeProgressBar1
            // 
            this.repositoryItemMarqueeProgressBar1.Name = "repositoryItemMarqueeProgressBar1";
            this.repositoryItemMarqueeProgressBar1.ProgressAnimationMode = DevExpress.Utils.Drawing.ProgressAnimationMode.PingPong;
            // 
            // repositoryItemRadioGroup1
            // 
            this.repositoryItemRadioGroup1.Name = "repositoryItemRadioGroup1";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // repositoryItemTextEdit3
            // 
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            // 
            // repositoryItemTextEdit4
            // 
            this.repositoryItemTextEdit4.AutoHeight = false;
            this.repositoryItemTextEdit4.Name = "repositoryItemTextEdit4";
            // 
            // repositoryItemComboBox3
            // 
            this.repositoryItemComboBox3.AutoHeight = false;
            this.repositoryItemComboBox3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox3.Name = "repositoryItemComboBox3";
            this.repositoryItemComboBox3.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repositoryItemToggleSwitch1
            // 
            this.repositoryItemToggleSwitch1.AutoHeight = false;
            this.repositoryItemToggleSwitch1.Name = "repositoryItemToggleSwitch1";
            this.repositoryItemToggleSwitch1.OffText = "Off";
            this.repositoryItemToggleSwitch1.OnText = "On";
            // 
            // repositoryItemToggleSwitch2
            // 
            this.repositoryItemToggleSwitch2.AutoHeight = false;
            this.repositoryItemToggleSwitch2.Name = "repositoryItemToggleSwitch2";
            this.repositoryItemToggleSwitch2.OffText = "Off";
            this.repositoryItemToggleSwitch2.OnText = "On";
            // 
            // repositoryItemToggleSwitch_AutoManu
            // 
            this.repositoryItemToggleSwitch_AutoManu.AutoHeight = false;
            this.repositoryItemToggleSwitch_AutoManu.Name = "repositoryItemToggleSwitch_AutoManu";
            this.repositoryItemToggleSwitch_AutoManu.OffText = "Off";
            this.repositoryItemToggleSwitch_AutoManu.OnText = "On";
            // 
            // repositoryItemTextEdit10
            // 
            this.repositoryItemTextEdit10.AutoHeight = false;
            this.repositoryItemTextEdit10.Name = "repositoryItemTextEdit10";
            // 
            // repositoryItemTextEdit11
            // 
            this.repositoryItemTextEdit11.AutoHeight = false;
            this.repositoryItemTextEdit11.Name = "repositoryItemTextEdit11";
            // 
            // ribbonStatusBarCPMainForm
            // 
            this.ribbonStatusBarCPMainForm.ItemLinks.Add(this.barHeaderItemVersion);
            this.ribbonStatusBarCPMainForm.ItemLinks.Add(this.barStaticItemLoadedTestList);
            this.ribbonStatusBarCPMainForm.ItemLinks.Add(this.barEditItem1);
            this.ribbonStatusBarCPMainForm.ItemLinks.Add(this.barStaticItemCurrentTime);
            this.ribbonStatusBarCPMainForm.ItemLinks.Add(this.barStaticItemRepeatTest);
            this.ribbonStatusBarCPMainForm.Location = new System.Drawing.Point(0, 614);
            this.ribbonStatusBarCPMainForm.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.ribbonStatusBarCPMainForm.Name = "ribbonStatusBarCPMainForm";
            this.ribbonStatusBarCPMainForm.Ribbon = this.ribbonControlCPTester;
            this.ribbonStatusBarCPMainForm.Size = new System.Drawing.Size(1878, 31);
            // 
            // barHeaderItemVersion
            // 
            this.barHeaderItemVersion.Caption = "CP-Tester Version:14.0.0.0";
            this.barHeaderItemVersion.Id = 2;
            this.barHeaderItemVersion.Name = "barHeaderItemVersion";
            // 
            // barStaticItemLoadedTestList
            // 
            this.barStaticItemLoadedTestList.Id = 3;
            this.barStaticItemLoadedTestList.Name = "barStaticItemLoadedTestList";
            this.barStaticItemLoadedTestList.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItem1
            // 
            this.barEditItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barEditItem1.Edit = this.repositoryItemMarqueeProgressBar1;
            this.barEditItem1.EditWidth = 150;
            this.barEditItem1.Id = 41;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // barStaticItemCurrentTime
            // 
            this.barStaticItemCurrentTime.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItemCurrentTime.Caption = "YYYY/MM/DD/HH/MM/SS";
            this.barStaticItemCurrentTime.Glyph = ((System.Drawing.Image)(resources.GetObject("barStaticItemCurrentTime.Glyph")));
            this.barStaticItemCurrentTime.Id = 4;
            this.barStaticItemCurrentTime.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barStaticItemCurrentTime.LargeGlyph")));
            this.barStaticItemCurrentTime.Name = "barStaticItemCurrentTime";
            this.barStaticItemCurrentTime.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.barStaticItemCurrentTime.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItemRepeatTest
            // 
            this.barStaticItemRepeatTest.Id = 86;
            this.barStaticItemRepeatTest.Name = "barStaticItemRepeatTest";
            this.barStaticItemRepeatTest.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // tabbedView
            // 
            this.tabbedView.DocumentProperties.AllowFloat = false;
            this.tabbedView.RootContainer.Element = null;
            // 
            // barButtonItemTriggerEventOnThread
            // 
            this.barButtonItemTriggerEventOnThread.Id = -1;
            this.barButtonItemTriggerEventOnThread.Name = "barButtonItemTriggerEventOnThread";
            // 
            // actionList1
            // 
            this.actionList1.Actions.AddRange(new Dsu.Common.Utilities.Actions.Action[] {
            this.action1});
            this.actionList1.Count = 1;
            this.actionList1.ImageList = null;
            this.actionList1.ShowTextOnToolBar = false;
            this.actionList1.Tag = null;
            this.actionList1.UpdateCmdUISleepIntervalOnIdle = 0;
            // 
            // action1
            // 
            this.action1.Checked = false;
            this.action1.Enabled = true;
            this.action1.Hint = null;
            this.action1.Shortcut = System.Windows.Forms.Shortcut.None;
            this.action1.Tag = null;
            this.action1.Text = "49139655";
            this.action1.Visible = true;
            this.action1.Update += new System.EventHandler(this.action1_Update);
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl1.Appearance.BackColor2 = System.Drawing.Color.Gray;
            this.panelControl1.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.panelControl1.Appearance.ForeColor = System.Drawing.Color.Transparent;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Appearance.Options.UseBorderColor = true;
            this.panelControl1.Appearance.Options.UseForeColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.labelControl_Message);
            this.panelControl1.Controls.Add(this.labelControlTitle);
            this.panelControl1.Controls.Add(this.pictureBox1);
            this.panelControl1.Controls.Add(this.simpleButton_AutoManu);
            this.panelControl1.Controls.Add(this.pictureBox2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 143);
            this.panelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1878, 37);
            this.panelControl1.TabIndex = 3;
            // 
            // labelControl_Message
            // 
            this.labelControl_Message.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelControl_Message.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_Message.Appearance.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelControl_Message.Appearance.Options.UseFont = true;
            this.labelControl_Message.Appearance.Options.UseForeColor = true;
            this.labelControl_Message.Appearance.Options.UseTextOptions = true;
            this.labelControl_Message.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_Message.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl_Message.Location = new System.Drawing.Point(1587, 6);
            this.labelControl_Message.Name = "labelControl_Message";
            this.labelControl_Message.Size = new System.Drawing.Size(309, 24);
            this.labelControl_Message.TabIndex = 12;
            this.labelControl_Message.Text = "Audit";
            this.labelControl_Message.Visible = false;
            // 
            // labelControlTitle
            // 
            this.labelControlTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControlTitle.Appearance.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlTitle.Appearance.Options.UseFont = true;
            this.labelControlTitle.Appearance.Options.UseTextOptions = true;
            this.labelControlTitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControlTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControlTitle.Location = new System.Drawing.Point(396, 3);
            this.labelControlTitle.Name = "labelControlTitle";
            this.labelControlTitle.Size = new System.Drawing.Size(683, 31);
            this.labelControlTitle.TabIndex = 2;
            this.labelControlTitle.Text = "Title";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1741, -1);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(137, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // simpleButton_AutoManu
            // 
            this.simpleButton_AutoManu.Appearance.BackColor = System.Drawing.Color.Gray;
            this.simpleButton_AutoManu.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton_AutoManu.Appearance.ForeColor = System.Drawing.Color.White;
            this.simpleButton_AutoManu.Appearance.Options.UseBackColor = true;
            this.simpleButton_AutoManu.Appearance.Options.UseFont = true;
            this.simpleButton_AutoManu.Appearance.Options.UseForeColor = true;
            this.simpleButton_AutoManu.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.simpleButton_AutoManu.Location = new System.Drawing.Point(177, 0);
            this.simpleButton_AutoManu.Name = "simpleButton_AutoManu";
            this.simpleButton_AutoManu.Size = new System.Drawing.Size(176, 37);
            this.simpleButton_AutoManu.TabIndex = 11;
            this.simpleButton_AutoManu.Text = "  AUTO  ";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(171, 37);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // dockManager
            // 
            this.dockManager.Form = this;
            this.dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.panelContainer1});
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl"});
            // 
            // panelContainer1
            // 
            this.panelContainer1.ActiveChild = this.dockPanel_TestResult;
            this.panelContainer1.Controls.Add(this.dockPanel_TestResult);
            this.panelContainer1.Controls.Add(this.dockPanel_Output);
            this.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer1.ID = new System.Guid("25f82db3-651d-4e21-8992-104e4b650ba9");
            this.panelContainer1.Location = new System.Drawing.Point(0, 435);
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.OriginalSize = new System.Drawing.Size(233, 179);
            this.panelContainer1.Size = new System.Drawing.Size(1878, 179);
            this.panelContainer1.Tabbed = true;
            this.panelContainer1.Text = "panelContainer1";
            // 
            // dockPanel_TestResult
            // 
            this.dockPanel_TestResult.Controls.Add(this.controlContainer1);
            this.dockPanel_TestResult.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel_TestResult.ID = new System.Guid("3fe8f591-5c61-4cb3-aa75-14bcd86ea38e");
            this.dockPanel_TestResult.Location = new System.Drawing.Point(4, 24);
            this.dockPanel_TestResult.Name = "dockPanel_TestResult";
            this.dockPanel_TestResult.Options.ShowAutoHideButton = false;
            this.dockPanel_TestResult.Options.ShowCloseButton = false;
            this.dockPanel_TestResult.Options.ShowMaximizeButton = false;
            this.dockPanel_TestResult.OriginalSize = new System.Drawing.Size(1646, 124);
            this.dockPanel_TestResult.Size = new System.Drawing.Size(1870, 124);
            this.dockPanel_TestResult.Text = "Test Result";
            // 
            // controlContainer1
            // 
            this.controlContainer1.Controls.Add(this.ucTestMonitor1);
            this.controlContainer1.Location = new System.Drawing.Point(0, 0);
            this.controlContainer1.Name = "controlContainer1";
            this.controlContainer1.Size = new System.Drawing.Size(1870, 124);
            this.controlContainer1.TabIndex = 0;
            // 
            // ucTestMonitor1
            // 
            this.ucTestMonitor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTestMonitor1.Location = new System.Drawing.Point(0, 0);
            this.ucTestMonitor1.Name = "ucTestMonitor1";
            this.ucTestMonitor1.Size = new System.Drawing.Size(1870, 124);
            this.ucTestMonitor1.TabIndex = 0;
            // 
            // dockPanel_Output
            // 
            this.dockPanel_Output.Controls.Add(this.dockPanel1_Container);
            this.dockPanel_Output.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel_Output.ID = new System.Guid("7d40c2e2-3785-41b0-a8c8-d65616f1a380");
            this.dockPanel_Output.Location = new System.Drawing.Point(4, 24);
            this.dockPanel_Output.Name = "dockPanel_Output";
            this.dockPanel_Output.Options.AllowDockTop = false;
            this.dockPanel_Output.Options.FloatOnDblClick = false;
            this.dockPanel_Output.Options.ShowAutoHideButton = false;
            this.dockPanel_Output.Options.ShowCloseButton = false;
            this.dockPanel_Output.Options.ShowMaximizeButton = false;
            this.dockPanel_Output.OriginalSize = new System.Drawing.Size(1646, 124);
            this.dockPanel_Output.Size = new System.Drawing.Size(1870, 124);
            this.dockPanel_Output.Text = "Output";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.listBoxControl_Output);
            this.dockPanel1_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(1870, 124);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // listBoxControl_Output
            // 
            this.listBoxControl_Output.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.listBoxControl_Output.Cursor = System.Windows.Forms.Cursors.Default;
            this.listBoxControl_Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxControl_Output.Location = new System.Drawing.Point(0, 0);
            this.listBoxControl_Output.Name = "listBoxControl_Output";
            this.listBoxControl_Output.Size = new System.Drawing.Size(1870, 124);
            this.listBoxControl_Output.TabIndex = 1;
            // 
            // barButtonItem_RobotReadyMove
            // 
            this.barButtonItem_RobotReadyMove.Caption = "Robot Ready Move";
            this.barButtonItem_RobotReadyMove.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_RobotReadyMove.Glyph")));
            this.barButtonItem_RobotReadyMove.Id = 159;
            this.barButtonItem_RobotReadyMove.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_RobotReadyMove.LargeGlyph")));
            this.barButtonItem_RobotReadyMove.LargeWidth = 120;
            this.barButtonItem_RobotReadyMove.Name = "barButtonItem_RobotReadyMove";
            this.barButtonItem_RobotReadyMove.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_RobotReadyMove_ItemClick);
            // 
            // FormAppSs
            // 
            this.AllowFormGlass = DevExpress.Utils.DefaultBoolean.False;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1878, 645);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelContainer1);
            this.Controls.Add(this.ribbonStatusBarCPMainForm);
            this.Controls.Add(this.ribbonControlCPTester);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormAppSs";
            this.Ribbon = this.ribbonControlCPTester;
            this.StatusBar = this.ribbonStatusBarCPMainForm;
            this.Text = "CP-Tester ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAppSs_FormClosing);
            this.Load += new System.EventHandler(this.FormAppSs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_cpDocManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControlCPTester)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMarqueeProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch_AutoManu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            this.panelContainer1.ResumeLayout(false);
            this.dockPanel_TestResult.ResumeLayout(false);
            this.controlContainer1.ResumeLayout(false);
            this.dockPanel_Output.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl_Output)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerGlobalTask;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        public DevExpress.XtraBars.BarButtonItem BarButtonItemMnCtrTsConditon
        {
            get { return barButtonItemMnCtrTsConditon; }
            set { barButtonItemMnCtrTsConditon = value; }
        }
        private DevExpress.XtraBars.Docking2010.DocumentManager m_cpDocManager;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView;
		private Dsu.Common.Utilities.Actions.ActionList actionList1;
		private Dsu.Common.Utilities.Actions.Action action1;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBarCPMainForm;
		private DevExpress.XtraBars.BarHeaderItem barHeaderItemVersion;
		private DevExpress.XtraBars.BarStaticItem barStaticItemLoadedTestList;
		private DevExpress.XtraBars.BarEditItem barEditItem1;
		private DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar repositoryItemMarqueeProgressBar1;
		private DevExpress.XtraBars.BarStaticItem barStaticItemCurrentTime;
		private DevExpress.XtraBars.BarStaticItem barStaticItemRepeatTest;
		private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControlCPTester;
		private DevExpress.XtraBars.BarButtonItem barButtonItemLoadCfgFrFile;
		private DevExpress.XtraBars.BarCheckItem barCheckItemShowDeactivateStep;
		private DevExpress.XtraBars.SkinRibbonGalleryBarItem skinRibbonGalleryBarItem1;
		private DevExpress.XtraBars.BarCheckItem barCheckItemShowColPosition;
		private DevExpress.XtraBars.BarCheckItem barCheckItemShowColVariant;
		private DevExpress.XtraBars.BarCheckItem barCheckItemShowColGate;
		private DevExpress.XtraBars.BarCheckItem barCheckItemShowColReturn;
		private DevExpress.XtraBars.BarCheckItem barCheckItemShowColParameter;
		private DevExpress.XtraBars.BarCheckItem barCheckItemShowColComment;
		private DevExpress.XtraBars.SkinRibbonGalleryBarItem skinRibbonGalleryBarItem2;
		private DevExpress.XtraBars.BarButtonItem barButtonItemMnCtrStart;
		private DevExpress.XtraBars.BarButtonItem barButtonItemMnCtrStop;
		private DevExpress.XtraBars.BarButtonItem barButtonItemMnCtrTsConditon;
		private DevExpress.XtraBars.BarButtonItem barButtonItemMnCtrManualSignalAnalysis;
		private DevExpress.XtraBars.BarCheckItem barCheckItemLogCANMsg;
		private DevExpress.XtraBars.BarButtonItem barButtonItemMnCtrLogTStepAction;
		private DevExpress.XtraBars.BarButtonItem barButtonItemMnCtrLogTStepMeasuring;
		private DevExpress.XtraBars.BarCheckItem barCheckItemLogSaveMeasuringLog;
		private DevExpress.XtraBars.BarEditItem barEditItemShowLogLevel;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
		private DevExpress.XtraBars.BarCheckItem barCheckItemMeasuringArray;
		private DevExpress.XtraBars.BarCheckItem barCheckItemSaveConsoleLog;
		private DevExpress.XtraBars.BarButtonItem barButtonItemSaveLogPath;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit4;
		private DevExpress.XtraBars.BarButtonItem barButtonItemTriggerEventOnThread;
		private DevExpress.XtraBars.BarButtonItem barButtonItemExit;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageMainControl;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupFileControl;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupTestCondition;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupMnCtrTesterControl;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageTsOption;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupShowSteps;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupShowColumn;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageEnvironmentSetup;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupEvnSetupTheme;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupMnCtrLogOptions;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageManulControl;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupManualCtrComm;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageLog;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
		private DevExpress.XtraEditors.Repository.RepositoryItemImageEdit repositoryItemImageEdit1;
		private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
		private DevExpress.XtraEditors.Repository.RepositoryItemImageEdit repositoryItemImageEdit2;
		private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit2;
		private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar repositoryItemProgressBar1;
		private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup repositoryItemRadioGroup1;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
		private DevExpress.XtraEditors.PanelControl panelControl1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private DevExpress.XtraEditors.LabelControl labelControlTitle;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox3;
		private DevExpress.XtraBars.BarButtonItem barButtonItemMnMain;
        private DevExpress.XtraBars.BarCheckItem barCheckItemDebugConsoleOut;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel_Output;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private DevExpress.XtraEditors.ListBoxControl listBoxControl_Output;
        private DevExpress.XtraBars.BarEditItem barEditItem_PartID;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit5;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_id;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch1;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_SelectAuto;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_SelectManual;
        private DevExpress.XtraEditors.SimpleButton simpleButton_AutoManu;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_AdminDialogClear;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch_AutoManu;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_LVDT;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_Motor1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_DigtalIO;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_AnalogIO;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_Robot;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_PLC;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_TempHumi;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_LCR;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_PowerSupply1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_Monitor;
        private DevExpress.XtraBars.BarEditItem barEditItem_TemperatureUpper;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit6;
        private DevExpress.XtraBars.BarEditItem barEditItem_TemperatureLower;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit7;
        private DevExpress.XtraBars.BarEditItem barEditItem_HumidityUpper;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit8;
        private DevExpress.XtraBars.BarEditItem barEditItem_HumidityLower;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit9;
        private DevExpress.XtraBars.BarEditItem barEditItem_CurrentTemp;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit12;
        private DevExpress.XtraBars.BarEditItem barEditItem_CurrentHumidity;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit13;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit10;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit11;
        private DevExpress.XtraBars.BarCheckItem barCheckItemShowColMP;
        private DevExpress.XtraBars.BarCheckItem barCheckItemShowColInf;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_LoadTestList;
        private DevExpress.XtraBars.BarEditItem barEditItem_AirGapMonitor;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit14;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_manualAirGap;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_Motor2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_Motor3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_PowerSupply2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_PowerSupply3;
        private DevExpress.XtraEditors.LabelControl labelControl_Message;
        private DevExpress.XtraBars.BarEditItem barEditItem_RPM;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit15;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDoorControl;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupDoor;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel_TestResult;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
        private ucTestMonitor ucTestMonitor1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_RobotReadyMove;
    }
}

