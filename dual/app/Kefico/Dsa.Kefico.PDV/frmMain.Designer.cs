using System;
using DevExpress.XtraTab;
namespace Dsa.Kefico.PDV {
    partial class frmMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup1 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
            DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup2 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
            DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup3 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.mainRibbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barAndDockingController = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.bsView = new DevExpress.XtraBars.BarSubItem();
            this.bsTools = new DevExpress.XtraBars.BarSubItem();
            this.bsSkins = new DevExpress.XtraBars.BarSubItem();
            this.bsHelp = new DevExpress.XtraBars.BarSubItem();
            this.biAbout = new DevExpress.XtraBars.BarButtonItem();
            this.skinGalleryBarItem = new DevExpress.XtraBars.RibbonGalleryBarItem();
            this.biSaveImage = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem_RowCount = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem_Online = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem_StatusText = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItem_StartDay = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit_StartDay = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barEditItem_EndDay = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit_EndDay = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barEditItem_LineGroup = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox_LuneGroup = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barButtonItem_Summary_Apply = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem_TsvID = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox_TSV = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItem_QuickView = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemToggleSwitch_QuickView = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.barEditItem_MeasureID = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox_Measure = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItem_PositionID = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox_Position = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barButtonGroup1 = new DevExpress.XtraBars.BarButtonGroup();
            this.barEditItem_SelectTest = new DevExpress.XtraBars.BarEditItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_SelectRelease = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_SelectTestList = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_SelectEntry = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem_ChartScaleAuto = new DevExpress.XtraBars.BarEditItem();
            this.barButtonItem_NewEntry = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_NewRelease = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_NewTestList = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem_Ver = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem_EditEntry = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_EditRelease = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_Delete = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem_EditTestList = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_Copy = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem_Paste = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonMiniToolbar1 = new DevExpress.XtraBars.Ribbon.RibbonMiniToolbar(this.components);
            this.ribbonPageCategory_Theme = new DevExpress.XtraBars.Ribbon.RibbonPageCategory();
            this.ribbonPage_UI_Design = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.skinsPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup_Refresh = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup_FilterLine = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup_FilterDay = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup_Quick = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.repositoryItemToggleSwitch1 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.repositoryItemToggleSwitch2 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.popupControlContainer1 = new DevExpress.XtraBars.PopupControlContainer(this.components);
            this.exitButton = new DevExpress.XtraEditors.SimpleButton();
            this.defaultToolTipController1 = new DevExpress.Utils.DefaultToolTipController(this.components);
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.documentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.actionList1 = new Dsu.Common.Utilities.Actions.ActionList(this.components);
            this.action1 = new Dsu.Common.Utilities.Actions.Action(this.components);
            this.popupMenu_Pdv = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barToggleSwitchItem_LogLevel = new DevExpress.XtraBars.BarToggleSwitchItem();
            this.ribbonPageGroup_About = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.dockPanel1.SuspendLayout();
            this.controlContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainRibbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_StartDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_StartDay.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_EndDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_EndDay.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox_LuneGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox_TSV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch_QuickView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox_Measure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox_Position)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupControlContainer1)).BeginInit();
            this.popupControlContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu_Pdv)).BeginInit();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.controlContainer1);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.dockPanel1.FloatSize = new System.Drawing.Size(200, 528);
            this.dockPanel1.FloatVertical = true;
            this.dockPanel1.ID = new System.Guid("1a5d040e-8d04-4f3d-bfd7-d81cb0934982");
            this.dockPanel1.Location = new System.Drawing.Point(0, 661);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Options.ShowCloseButton = false;
            this.dockPanel1.OriginalSize = new System.Drawing.Size(264, 75);
            this.dockPanel1.Size = new System.Drawing.Size(1014, 75);
            this.dockPanel1.Text = "Output";
            // 
            // controlContainer1
            // 
            this.defaultToolTipController1.SetAllowHtmlText(this.controlContainer1, DevExpress.Utils.DefaultBoolean.Default);
            this.controlContainer1.Controls.Add(this.memoEdit1);
            this.controlContainer1.Location = new System.Drawing.Point(4, 24);
            this.controlContainer1.Name = "controlContainer1";
            this.controlContainer1.Size = new System.Drawing.Size(1006, 47);
            this.controlContainer1.TabIndex = 0;
            // 
            // memoEdit1
            // 
            this.memoEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEdit1.Location = new System.Drawing.Point(0, 0);
            this.memoEdit1.MenuManager = this.mainRibbon;
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.ReadOnly = true;
            this.memoEdit1.Size = new System.Drawing.Size(1006, 47);
            this.memoEdit1.TabIndex = 0;
            // 
            // mainRibbon
            // 
            this.mainRibbon.ApplicationButtonText = null;
            this.mainRibbon.Controller = this.barAndDockingController;
            this.mainRibbon.ExpandCollapseItem.Id = 0;
            this.mainRibbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.mainRibbon.ExpandCollapseItem,
            this.bsView,
            this.bsTools,
            this.bsHelp,
            this.biAbout,
            this.bsSkins,
            this.skinGalleryBarItem,
            this.biSaveImage,
            this.barStaticItem1,
            this.barStaticItem_RowCount,
            this.barButtonItem_Online,
            this.barStaticItem_StatusText,
            this.barEditItem_StartDay,
            this.barEditItem_EndDay,
            this.barEditItem_LineGroup,
            this.barButtonItem_Summary_Apply,
            this.barEditItem_TsvID,
            this.barEditItem_QuickView,
            this.barEditItem_MeasureID,
            this.barEditItem_PositionID,
            this.barButtonGroup1,
            this.barEditItem_SelectTest,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem_SelectRelease,
            this.barButtonItem_SelectTestList,
            this.barButtonItem_SelectEntry,
            this.barButtonItem6,
            this.barEditItem_ChartScaleAuto,
            this.barButtonItem_NewEntry,
            this.barButtonItem_NewRelease,
            this.barButtonItem_NewTestList,
            this.barStaticItem_Ver,
            this.barButtonItem_EditEntry,
            this.barButtonItem_EditRelease,
            this.barButtonItem_Delete,
            this.barEditItem1,
            this.barStaticItem2,
            this.barStaticItem3,
            this.barButtonItem_EditTestList,
            this.barButtonItem_Copy,
            this.barButtonItem_Paste,
            this.barToggleSwitchItem_LogLevel});
            this.mainRibbon.Location = new System.Drawing.Point(0, 0);
            this.mainRibbon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mainRibbon.MaxItemId = 2;
            this.mainRibbon.MdiMergeStyle = DevExpress.XtraBars.Ribbon.RibbonMdiMergeStyle.Always;
            this.mainRibbon.MiniToolbars.Add(this.ribbonMiniToolbar1);
            this.mainRibbon.Name = "mainRibbon";
            this.mainRibbon.OptionsTouch.ShowTouchUISelectorInQAT = true;
            this.mainRibbon.OptionsTouch.ShowTouchUISelectorVisibilityItemInQATMenu = true;
            this.mainRibbon.PageCategories.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageCategory[] {
            this.ribbonPageCategory_Theme});
            this.mainRibbon.PageCategoryAlignment = DevExpress.XtraBars.Ribbon.RibbonPageCategoryAlignment.Right;
            this.mainRibbon.PageHeaderItemLinks.Add(this.biAbout);
            this.mainRibbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1,
            this.ribbonPage2});
            this.mainRibbon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit_StartDay,
            this.repositoryItemDateEdit_EndDay,
            this.repositoryItemComboBox_LuneGroup,
            this.repositoryItemComboBox_TSV,
            this.repositoryItemToggleSwitch_QuickView,
            this.repositoryItemComboBox_Measure,
            this.repositoryItemComboBox_Position,
            this.repositoryItemToggleSwitch1,
            this.repositoryItemToggleSwitch2,
            this.repositoryItemTextEdit1});
            this.mainRibbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
            this.mainRibbon.Size = new System.Drawing.Size(1014, 147);
            this.mainRibbon.StatusBar = this.ribbonStatusBar1;
            this.mainRibbon.TransparentEditors = true;
            // 
            // barAndDockingController
            // 
            this.barAndDockingController.PropertiesBar.AllowLinkLighting = false;
            this.barAndDockingController.PropertiesBar.DefaultGlyphSize = new System.Drawing.Size(16, 16);
            this.barAndDockingController.PropertiesBar.DefaultLargeGlyphSize = new System.Drawing.Size(32, 32);
            // 
            // bsView
            // 
            this.bsView.Caption = "&View";
            this.bsView.Id = 2;
            this.bsView.Name = "bsView";
            // 
            // bsTools
            // 
            this.bsTools.Caption = "&Tools";
            this.bsTools.Id = 3;
            this.bsTools.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bsSkins)});
            this.bsTools.Name = "bsTools";
            // 
            // bsSkins
            // 
            this.bsSkins.Caption = "&Skins";
            this.bsSkins.Id = 7;
            this.bsSkins.Name = "bsSkins";
            // 
            // bsHelp
            // 
            this.bsHelp.Caption = "&Help";
            this.bsHelp.Id = 5;
            this.bsHelp.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.biAbout)});
            this.bsHelp.Name = "bsHelp";
            // 
            // biAbout
            // 
            this.biAbout.Caption = "&About";
            this.biAbout.Id = 6;
            this.biAbout.Name = "biAbout";
            // 
            // skinGalleryBarItem
            // 
            // 
            // 
            // 
            this.skinGalleryBarItem.Gallery.AllowHoverImages = true;
            this.skinGalleryBarItem.Gallery.FixedHoverImageSize = false;
            galleryItemGroup1.Caption = "Standard";
            galleryItemGroup2.Caption = "Bonus";
            galleryItemGroup2.Visible = false;
            galleryItemGroup3.Caption = "Office";
            galleryItemGroup3.Visible = false;
            this.skinGalleryBarItem.Gallery.Groups.AddRange(new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            galleryItemGroup1,
            galleryItemGroup2,
            galleryItemGroup3});
            this.skinGalleryBarItem.Gallery.ImageSize = new System.Drawing.Size(58, 43);
            this.skinGalleryBarItem.Gallery.ItemCheckMode = DevExpress.XtraBars.Ribbon.Gallery.ItemCheckMode.SingleRadio;
            this.skinGalleryBarItem.Id = 1;
            this.skinGalleryBarItem.Name = "skinGalleryBarItem";
            // 
            // biSaveImage
            // 
            this.biSaveImage.Caption = "Save";
            this.biSaveImage.Id = 14;
            this.biSaveImage.Name = "biSaveImage";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "barStaticItem1";
            this.barStaticItem1.Id = 1;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem_RowCount
            // 
            this.barStaticItem_RowCount.Caption = " Records: 0 ";
            this.barStaticItem_RowCount.Id = 2;
            this.barStaticItem_RowCount.Name = "barStaticItem_RowCount";
            this.barStaticItem_RowCount.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barButtonItem_Online
            // 
            this.barButtonItem_Online.Caption = "Offline";
            this.barButtonItem_Online.Glyph = global::Dsa.Kefico.PDV.Properties.Resources.show_32x32;
            this.barButtonItem_Online.Id = 3;
            this.barButtonItem_Online.LargeGlyph = global::Dsa.Kefico.PDV.Properties.Resources.hide_32x32;
            this.barButtonItem_Online.Name = "barButtonItem_Online";
            // 
            // barStaticItem_StatusText
            // 
            this.barStaticItem_StatusText.Caption = "Ready";
            this.barStaticItem_StatusText.Id = 4;
            this.barStaticItem_StatusText.Name = "barStaticItem_StatusText";
            this.barStaticItem_StatusText.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItem_StartDay
            // 
            this.barEditItem_StartDay.Caption = "Start ";
            this.barEditItem_StartDay.Description = "Start ";
            this.barEditItem_StartDay.Edit = this.repositoryItemDateEdit_StartDay;
            this.barEditItem_StartDay.EditWidth = 100;
            this.barEditItem_StartDay.Glyph = global::Dsa.Kefico.PDV.Properties.Resources.printstartdate_32x32;
            this.barEditItem_StartDay.Id = 2;
            this.barEditItem_StartDay.Name = "barEditItem_StartDay";
            // 
            // repositoryItemDateEdit_StartDay
            // 
            this.repositoryItemDateEdit_StartDay.AutoHeight = false;
            this.repositoryItemDateEdit_StartDay.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit_StartDay.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit_StartDay.Name = "repositoryItemDateEdit_StartDay";
            // 
            // barEditItem_EndDay
            // 
            this.barEditItem_EndDay.Caption = "End  ";
            this.barEditItem_EndDay.Description = "End";
            this.barEditItem_EndDay.Edit = this.repositoryItemDateEdit_EndDay;
            this.barEditItem_EndDay.EditWidth = 102;
            this.barEditItem_EndDay.Glyph = global::Dsa.Kefico.PDV.Properties.Resources.printstartdate_32x32;
            this.barEditItem_EndDay.Id = 3;
            this.barEditItem_EndDay.Name = "barEditItem_EndDay";
            // 
            // repositoryItemDateEdit_EndDay
            // 
            this.repositoryItemDateEdit_EndDay.AutoHeight = false;
            this.repositoryItemDateEdit_EndDay.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit_EndDay.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit_EndDay.Name = "repositoryItemDateEdit_EndDay";
            // 
            // barEditItem_LineGroup
            // 
            this.barEditItem_LineGroup.Caption = "Line Group";
            this.barEditItem_LineGroup.Edit = this.repositoryItemComboBox_LuneGroup;
            this.barEditItem_LineGroup.EditWidth = 100;
            this.barEditItem_LineGroup.Glyph = global::Dsa.Kefico.PDV.Properties.Resources.showproduct_16x16;
            this.barEditItem_LineGroup.Id = 1;
            this.barEditItem_LineGroup.Name = "barEditItem_LineGroup";
            // 
            // repositoryItemComboBox_LuneGroup
            // 
            this.repositoryItemComboBox_LuneGroup.AutoHeight = false;
            this.repositoryItemComboBox_LuneGroup.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox_LuneGroup.Name = "repositoryItemComboBox_LuneGroup";
            this.repositoryItemComboBox_LuneGroup.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // barButtonItem_Summary_Apply
            // 
            this.barButtonItem_Summary_Apply.Caption = "Reload";
            this.barButtonItem_Summary_Apply.Glyph = global::Dsa.Kefico.PDV.Properties.Resources.convert_32x32;
            this.barButtonItem_Summary_Apply.Id = 2;
            this.barButtonItem_Summary_Apply.LargeWidth = 100;
            this.barButtonItem_Summary_Apply.Name = "barButtonItem_Summary_Apply";
            this.barButtonItem_Summary_Apply.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.barButtonItem_Summary_Apply.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_Summary_Apply_ItemClick);
            // 
            // barEditItem_TsvID
            // 
            this.barEditItem_TsvID.Caption = "ID";
            this.barEditItem_TsvID.Edit = this.repositoryItemComboBox_TSV;
            this.barEditItem_TsvID.EditWidth = 100;
            this.barEditItem_TsvID.Enabled = false;
            this.barEditItem_TsvID.Id = 3;
            this.barEditItem_TsvID.Name = "barEditItem_TsvID";
            // 
            // repositoryItemComboBox_TSV
            // 
            this.repositoryItemComboBox_TSV.AllowFocused = false;
            this.repositoryItemComboBox_TSV.AutoHeight = false;
            this.repositoryItemComboBox_TSV.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox_TSV.Name = "repositoryItemComboBox_TSV";
            this.repositoryItemComboBox_TSV.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // barEditItem_QuickView
            // 
            this.barEditItem_QuickView.Caption = "Filter";
            this.barEditItem_QuickView.Edit = this.repositoryItemToggleSwitch_QuickView;
            this.barEditItem_QuickView.EditHeight = 25;
            this.barEditItem_QuickView.EditWidth = 74;
            this.barEditItem_QuickView.Glyph = ((System.Drawing.Image)(resources.GetObject("barEditItem_QuickView.Glyph")));
            this.barEditItem_QuickView.Id = 5;
            this.barEditItem_QuickView.Name = "barEditItem_QuickView";
            this.barEditItem_QuickView.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.barEditItem_QuickView.EditValueChanged += new System.EventHandler(this.barEditItem_QuickView_EditValueChanged);
            // 
            // repositoryItemToggleSwitch_QuickView
            // 
            this.repositoryItemToggleSwitch_QuickView.AutoHeight = false;
            this.repositoryItemToggleSwitch_QuickView.Name = "repositoryItemToggleSwitch_QuickView";
            this.repositoryItemToggleSwitch_QuickView.OffText = "Off";
            this.repositoryItemToggleSwitch_QuickView.OnText = "On";
            // 
            // barEditItem_MeasureID
            // 
            this.barEditItem_MeasureID.Caption = "ID";
            this.barEditItem_MeasureID.Edit = this.repositoryItemComboBox_Measure;
            this.barEditItem_MeasureID.EditWidth = 100;
            this.barEditItem_MeasureID.Enabled = false;
            this.barEditItem_MeasureID.Id = 8;
            this.barEditItem_MeasureID.Name = "barEditItem_MeasureID";
            // 
            // repositoryItemComboBox_Measure
            // 
            this.repositoryItemComboBox_Measure.AutoHeight = false;
            this.repositoryItemComboBox_Measure.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox_Measure.Name = "repositoryItemComboBox_Measure";
            this.repositoryItemComboBox_Measure.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // barEditItem_PositionID
            // 
            this.barEditItem_PositionID.Caption = "ID";
            this.barEditItem_PositionID.Edit = this.repositoryItemComboBox_Position;
            this.barEditItem_PositionID.EditWidth = 100;
            this.barEditItem_PositionID.Enabled = false;
            this.barEditItem_PositionID.Id = 9;
            this.barEditItem_PositionID.Name = "barEditItem_PositionID";
            // 
            // repositoryItemComboBox_Position
            // 
            this.repositoryItemComboBox_Position.AutoHeight = false;
            this.repositoryItemComboBox_Position.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox_Position.Name = "repositoryItemComboBox_Position";
            // 
            // barButtonGroup1
            // 
            this.barButtonGroup1.Caption = "barButtonGroup1";
            this.barButtonGroup1.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barButtonGroup1.Id = 10;
            this.barButtonGroup1.Name = "barButtonGroup1";
            // 
            // barEditItem_SelectTest
            // 
            this.barEditItem_SelectTest.Edit = null;
            this.barEditItem_SelectTest.Id = 22;
            this.barEditItem_SelectTest.Name = "barEditItem_SelectTest";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Id = 26;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Id = 27;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barButtonItem_SelectRelease
            // 
            this.barButtonItem_SelectRelease.Caption = "Select Release";
            this.barButtonItem_SelectRelease.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_SelectRelease.Glyph")));
            this.barButtonItem_SelectRelease.Id = 28;
            this.barButtonItem_SelectRelease.Name = "barButtonItem_SelectRelease";
            this.barButtonItem_SelectRelease.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_SelectRelease_ItemClick);
            // 
            // barButtonItem_SelectTestList
            // 
            this.barButtonItem_SelectTestList.Caption = "Select TestList";
            this.barButtonItem_SelectTestList.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_SelectTestList.Glyph")));
            this.barButtonItem_SelectTestList.Id = 29;
            this.barButtonItem_SelectTestList.Name = "barButtonItem_SelectTestList";
            this.barButtonItem_SelectTestList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_SelectTestList_ItemClick);
            // 
            // barButtonItem_SelectEntry
            // 
            this.barButtonItem_SelectEntry.Caption = "Select Entry";
            this.barButtonItem_SelectEntry.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_SelectEntry.Glyph")));
            this.barButtonItem_SelectEntry.Id = 30;
            this.barButtonItem_SelectEntry.Name = "barButtonItem_SelectEntry";
            this.barButtonItem_SelectEntry.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_SelectEntry_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "barButtonItem6";
            this.barButtonItem6.Id = 18;
            this.barButtonItem6.Name = "barButtonItem6";
            // 
            // barEditItem_ChartScaleAuto
            // 
            this.barEditItem_ChartScaleAuto.Edit = null;
            this.barEditItem_ChartScaleAuto.Id = 23;
            this.barEditItem_ChartScaleAuto.Name = "barEditItem_ChartScaleAuto";
            // 
            // barButtonItem_NewEntry
            // 
            this.barButtonItem_NewEntry.Caption = "New Entry";
            this.barButtonItem_NewEntry.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_NewEntry.Glyph")));
            this.barButtonItem_NewEntry.Id = 24;
            this.barButtonItem_NewEntry.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_NewEntry.LargeGlyph")));
            this.barButtonItem_NewEntry.LargeWidth = 80;
            this.barButtonItem_NewEntry.Name = "barButtonItem_NewEntry";
            this.barButtonItem_NewEntry.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem_NewEntry.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_NewEntry_ItemClick);
            // 
            // barButtonItem_NewRelease
            // 
            this.barButtonItem_NewRelease.Caption = "New Release";
            this.barButtonItem_NewRelease.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_NewRelease.Glyph")));
            this.barButtonItem_NewRelease.Id = 25;
            this.barButtonItem_NewRelease.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_NewRelease.LargeGlyph")));
            this.barButtonItem_NewRelease.LargeWidth = 80;
            this.barButtonItem_NewRelease.Name = "barButtonItem_NewRelease";
            this.barButtonItem_NewRelease.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem_NewRelease.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_NewRelease_ItemClick);
            // 
            // barButtonItem_NewTestList
            // 
            this.barButtonItem_NewTestList.Caption = "New TestList";
            this.barButtonItem_NewTestList.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_NewTestList.Glyph")));
            this.barButtonItem_NewTestList.Id = 33;
            this.barButtonItem_NewTestList.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_NewTestList.LargeGlyph")));
            this.barButtonItem_NewTestList.LargeWidth = 100;
            this.barButtonItem_NewTestList.Name = "barButtonItem_NewTestList";
            this.barButtonItem_NewTestList.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem_NewTestList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_TestList_ItemClick);
            // 
            // barStaticItem_Ver
            // 
            this.barStaticItem_Ver.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem_Ver.Caption = "V9.1.1 (2016.12.5)";
            this.barStaticItem_Ver.Id = 28;
            this.barStaticItem_Ver.Name = "barStaticItem_Ver";
            this.barStaticItem_Ver.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barButtonItem_EditEntry
            // 
            this.barButtonItem_EditEntry.Caption = "Edit Entry";
            this.barButtonItem_EditEntry.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_EditEntry.Glyph")));
            this.barButtonItem_EditEntry.Id = 29;
            this.barButtonItem_EditEntry.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_EditEntry.LargeGlyph")));
            this.barButtonItem_EditEntry.LargeWidth = 80;
            this.barButtonItem_EditEntry.Name = "barButtonItem_EditEntry";
            this.barButtonItem_EditEntry.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.barButtonItem_EditEntry.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_EditEntry_ItemClick);
            // 
            // barButtonItem_EditRelease
            // 
            this.barButtonItem_EditRelease.Caption = "Edit Release";
            this.barButtonItem_EditRelease.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_EditRelease.Glyph")));
            this.barButtonItem_EditRelease.Id = 30;
            this.barButtonItem_EditRelease.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_EditRelease.LargeGlyph")));
            this.barButtonItem_EditRelease.LargeWidth = 80;
            this.barButtonItem_EditRelease.Name = "barButtonItem_EditRelease";
            this.barButtonItem_EditRelease.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.barButtonItem_EditRelease.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_EditRelease_ItemClick);
            // 
            // barButtonItem_Delete
            // 
            this.barButtonItem_Delete.Caption = "Delete";
            this.barButtonItem_Delete.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Delete.Glyph")));
            this.barButtonItem_Delete.Id = 31;
            this.barButtonItem_Delete.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Delete.LargeGlyph")));
            this.barButtonItem_Delete.LargeWidth = 80;
            this.barButtonItem_Delete.Name = "barButtonItem_Delete";
            this.barButtonItem_Delete.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.barButtonItem_Delete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_Delete_ItemClick);
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "barEditItem1";
            this.barEditItem1.Edit = this.repositoryItemTextEdit1;
            this.barEditItem1.Id = 1;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "___________________";
            this.barStaticItem2.Id = 1;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "___________________";
            this.barStaticItem3.Id = 2;
            this.barStaticItem3.Name = "barStaticItem3";
            this.barStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barButtonItem_EditTestList
            // 
            this.barButtonItem_EditTestList.Caption = "Edit TestList";
            this.barButtonItem_EditTestList.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_EditTestList.Glyph")));
            this.barButtonItem_EditTestList.Id = 2;
            this.barButtonItem_EditTestList.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_EditTestList.LargeGlyph")));
            this.barButtonItem_EditTestList.Name = "barButtonItem_EditTestList";
            this.barButtonItem_EditTestList.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            this.barButtonItem_EditTestList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_EditTestList_ItemClick);
            // 
            // barButtonItem_Copy
            // 
            this.barButtonItem_Copy.Caption = "Copy";
            this.barButtonItem_Copy.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Copy.Glyph")));
            this.barButtonItem_Copy.Id = 1;
            this.barButtonItem_Copy.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Copy.LargeGlyph")));
            this.barButtonItem_Copy.Name = "barButtonItem_Copy";
            this.barButtonItem_Copy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem_Copy_ItemClick);
            // 
            // barButtonItem_Paste
            // 
            this.barButtonItem_Paste.Caption = "Paste";
            this.barButtonItem_Paste.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Paste.Glyph")));
            this.barButtonItem_Paste.Id = 2;
            this.barButtonItem_Paste.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem_Paste.LargeGlyph")));
            this.barButtonItem_Paste.Name = "barButtonItem_Paste";
            // 
            // ribbonMiniToolbar1
            // 
            this.ribbonMiniToolbar1.Alignment = System.Drawing.ContentAlignment.TopRight;
            this.ribbonMiniToolbar1.ParentControl = this;
            // 
            // ribbonPageCategory_Theme
            // 
            this.ribbonPageCategory_Theme.Name = "ribbonPageCategory_Theme";
            this.ribbonPageCategory_Theme.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage_UI_Design});
            this.ribbonPageCategory_Theme.Text = "Theme";
            // 
            // ribbonPage_UI_Design
            // 
            this.ribbonPage_UI_Design.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.skinsPageGroup,
            this.ribbonPageGroup_About});
            this.ribbonPage_UI_Design.Name = "ribbonPage_UI_Design";
            this.ribbonPage_UI_Design.Text = "UI Design";
            // 
            // skinsPageGroup
            // 
            this.skinsPageGroup.ItemLinks.Add(this.skinGalleryBarItem);
            this.skinsPageGroup.Name = "skinsPageGroup";
            this.skinsPageGroup.ShowCaptionButton = false;
            this.skinsPageGroup.Text = "Skins";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup_Refresh,
            this.ribbonPageGroup_FilterLine,
            this.ribbonPageGroup_FilterDay,
            this.ribbonPageGroup_Quick});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Home";
            // 
            // ribbonPageGroup_Refresh
            // 
            this.ribbonPageGroup_Refresh.ItemLinks.Add(this.barButtonItem_Summary_Apply);
            this.ribbonPageGroup_Refresh.Name = "ribbonPageGroup_Refresh";
            this.ribbonPageGroup_Refresh.Text = "Refresh";
            // 
            // ribbonPageGroup_FilterLine
            // 
            this.ribbonPageGroup_FilterLine.ItemLinks.Add(this.barEditItem_LineGroup);
            this.ribbonPageGroup_FilterLine.Name = "ribbonPageGroup_FilterLine";
            this.ribbonPageGroup_FilterLine.Text = "Filter Line";
            // 
            // ribbonPageGroup_FilterDay
            // 
            this.ribbonPageGroup_FilterDay.ItemLinks.Add(this.barEditItem_StartDay);
            this.ribbonPageGroup_FilterDay.ItemLinks.Add(this.barEditItem_EndDay);
            this.ribbonPageGroup_FilterDay.Name = "ribbonPageGroup_FilterDay";
            this.ribbonPageGroup_FilterDay.Text = "Filter Day";
            // 
            // ribbonPageGroup_Quick
            // 
            this.ribbonPageGroup_Quick.ItemLinks.Add(this.barEditItem_QuickView);
            this.ribbonPageGroup_Quick.Name = "ribbonPageGroup_Quick";
            this.ribbonPageGroup_Quick.Text = "Filter Quick";
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup4,
            this.ribbonPageGroup3});
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "Edit";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.AllowTextClipping = false;
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem_NewEntry);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem_SelectEntry);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Group";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItem_NewTestList);
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItem_SelectTestList);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "TestList";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.AllowTextClipping = false;
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem_NewRelease);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem_SelectRelease);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Pdv";
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
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.ItemLinks.Add(this.barButtonItem_Online);
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItem_RowCount, true);
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItem_StatusText, true);
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItem_Ver);
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 736);
            this.ribbonStatusBar1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.mainRibbon;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1014, 31);
            // 
            // popupControlContainer1
            // 
            this.defaultToolTipController1.SetAllowHtmlText(this.popupControlContainer1, DevExpress.Utils.DefaultBoolean.Default);
            this.popupControlContainer1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.popupControlContainer1.Appearance.Options.UseBackColor = true;
            this.popupControlContainer1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.popupControlContainer1.Controls.Add(this.exitButton);
            this.popupControlContainer1.Location = new System.Drawing.Point(1154, 76);
            this.popupControlContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.popupControlContainer1.Name = "popupControlContainer1";
            this.popupControlContainer1.Ribbon = this.mainRibbon;
            this.popupControlContainer1.Size = new System.Drawing.Size(92, 39);
            this.popupControlContainer1.TabIndex = 1;
            this.popupControlContainer1.Visible = false;
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(3, 4);
            this.exitButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(83, 30);
            this.exitButton.TabIndex = 0;
            this.exitButton.Text = "Exit";
            this.exitButton.Click += new System.EventHandler(this.OnExitButtonClick);
            // 
            // defaultToolTipController1
            // 
            // 
            // 
            // 
            this.defaultToolTipController1.DefaultController.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            // 
            // dockManager
            // 
            this.dockManager.Controller = this.barAndDockingController;
            this.dockManager.Form = this;
            this.dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // documentManager
            // 
            this.documentManager.BarAndDockingController = this.barAndDockingController;
            this.documentManager.MdiParent = this;
            this.documentManager.MenuManager = this.mainRibbon;
            this.documentManager.View = this.tabbedView;
            this.documentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView});
            // 
            // tabbedView
            // 
            this.tabbedView.DocumentGroupProperties.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.tabbedView.RootContainer.Element = null;
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "ribbonPageGroup2";
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
            this.action1.Text = "57651109";
            this.action1.Visible = true;
            this.action1.Update += new System.EventHandler(this.action1_Update);
            // 
            // popupMenu_Pdv
            // 
            this.popupMenu_Pdv.ItemLinks.Add(this.barButtonItem_Copy);
            this.popupMenu_Pdv.ItemLinks.Add(this.barButtonItem_Delete);
            this.popupMenu_Pdv.ItemLinks.Add(this.barStaticItem3);
            this.popupMenu_Pdv.ItemLinks.Add(this.barButtonItem_NewEntry);
            this.popupMenu_Pdv.ItemLinks.Add(this.barButtonItem_EditEntry);
            this.popupMenu_Pdv.ItemLinks.Add(this.barStaticItem3);
            this.popupMenu_Pdv.ItemLinks.Add(this.barButtonItem_NewTestList);
            this.popupMenu_Pdv.ItemLinks.Add(this.barButtonItem_EditTestList);
            this.popupMenu_Pdv.ItemLinks.Add(this.barStaticItem2);
            this.popupMenu_Pdv.ItemLinks.Add(this.barButtonItem_NewRelease);
            this.popupMenu_Pdv.ItemLinks.Add(this.barButtonItem_EditRelease);
            this.popupMenu_Pdv.Name = "popupMenu_Pdv";
            this.popupMenu_Pdv.Ribbon = this.mainRibbon;
            // 
            // barToggleSwitchItem_LogLevel
            // 
            this.barToggleSwitchItem_LogLevel.Caption = "Log Debug";
            this.barToggleSwitchItem_LogLevel.Id = 1;
            this.barToggleSwitchItem_LogLevel.Name = "barToggleSwitchItem_LogLevel";
            // 
            // ribbonPageGroup_About
            // 
            this.ribbonPageGroup_About.ItemLinks.Add(this.barToggleSwitchItem_LogLevel);
            this.ribbonPageGroup_About.Name = "ribbonPageGroup_About";
            this.ribbonPageGroup_About.Text = "Info";
            // 
            // frmMain
            // 
            this.defaultToolTipController1.SetAllowHtmlText(this, DevExpress.Utils.DefaultBoolean.Default);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 767);
            this.Controls.Add(this.popupControlContainer1);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.mainRibbon);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMain";
            this.Ribbon = this.mainRibbon;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "PDV";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.dockPanel1.ResumeLayout(false);
            this.controlContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainRibbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_StartDay.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_StartDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_EndDay.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_EndDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox_LuneGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox_TSV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch_QuickView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox_Measure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox_Position)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupControlContainer1)).EndInit();
            this.popupControlContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu_Pdv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

   
        #endregion

        private DevExpress.XtraBars.BarSubItem bsView;
        private DevExpress.XtraBars.BarSubItem bsTools;
        private DevExpress.XtraBars.BarSubItem bsHelp;
        private DevExpress.XtraBars.BarButtonItem biAbout;
        private DevExpress.XtraBars.BarSubItem bsSkins;
        private DevExpress.Utils.DefaultToolTipController defaultToolTipController1;
        private DevExpress.XtraBars.Ribbon.RibbonControl mainRibbon;
        private DevExpress.XtraBars.RibbonGalleryBarItem skinGalleryBarItem;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup skinsPageGroup;
        private DevExpress.XtraBars.PopupControlContainer popupControlContainer1;
        private DevExpress.XtraEditors.SimpleButton exitButton;
        private DevExpress.XtraBars.BarButtonItem biSaveImage;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.Ribbon.RibbonMiniToolbar ribbonMiniToolbar1;


        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem_RowCount;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_Online;
        private DevExpress.XtraBars.BarStaticItem barStaticItem_StatusText;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraBars.Ribbon.RibbonPageCategory ribbonPageCategory_Theme;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_FilterDay;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage_UI_Design;
        private DevExpress.XtraBars.BarEditItem barEditItem_StartDay;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit_StartDay;
        private DevExpress.XtraBars.BarEditItem barEditItem_EndDay;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit_EndDay;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_FilterLine;
        private DevExpress.XtraBars.BarEditItem barEditItem_LineGroup;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox_LuneGroup;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_Summary_Apply;
        private DevExpress.XtraBars.BarEditItem barEditItem_TsvID;
        private DevExpress.XtraBars.BarEditItem barEditItem_QuickView;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch_QuickView;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_Quick;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox_TSV;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox_Measure;
        private DevExpress.XtraBars.BarEditItem barEditItem_MeasureID;
        private DevExpress.XtraBars.BarEditItem barEditItem_PositionID;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox_Position;
        private DevExpress.XtraBars.BarButtonGroup barButtonGroup1;
        private DevExpress.XtraBars.BarEditItem barEditItem_SelectTest;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_SelectRelease;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_SelectTestList;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_SelectEntry;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarEditItem barEditItem_ChartScaleAuto;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_NewRelease;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_NewEntry;
        private Dsu.Common.Utilities.Actions.ActionList actionList1;
        private Dsu.Common.Utilities.Actions.Action action1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_Refresh;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_NewTestList;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarStaticItem barStaticItem_Ver;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_EditEntry;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_EditRelease;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_Delete;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.PopupMenu popupMenu_Pdv;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_EditTestList;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_Copy;
        private DevExpress.XtraBars.BarButtonItem barButtonItem_Paste;
        private DevExpress.XtraBars.BarToggleSwitchItem barToggleSwitchItem_LogLevel;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_About;
    }
}
