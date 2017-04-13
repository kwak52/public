namespace DXAppCPTester
{
    partial class userCtrMainView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(userCtrMainView));
            this.m_splitCntCtrlHorizon = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridCtrTestSteps = new DevExpress.XtraGrid.GridControl();
            this.gridViewTestSteps = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridCtrModuleSteps = new DevExpress.XtraGrid.GridControl();
            this.gridViewModuleSteps = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.popupMenuTStepView = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barEditItemSearchStep = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSearchControl2 = new DevExpress.XtraEditors.Repository.RepositoryItemSearchControl();
            this.barButtonItemUncheckAllSteps = new DevExpress.XtraBars.BarButtonItem();
            this.barManagerTStepView = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.repositoryItemSearchControl1 = new DevExpress.XtraEditors.Repository.RepositoryItemSearchControl();
            ((System.ComponentModel.ISupportInitialize)(this.m_splitCntCtrlHorizon)).BeginInit();
            this.m_splitCntCtrlHorizon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrTestSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTestSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrModuleSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewModuleSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuTStepView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerTStepView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // m_splitCntCtrlHorizon
            // 
            this.m_splitCntCtrlHorizon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_splitCntCtrlHorizon.Horizontal = false;
            this.m_splitCntCtrlHorizon.Location = new System.Drawing.Point(0, 0);
            this.m_splitCntCtrlHorizon.Margin = new System.Windows.Forms.Padding(8);
            this.m_splitCntCtrlHorizon.Name = "m_splitCntCtrlHorizon";
            this.m_splitCntCtrlHorizon.Panel1.Controls.Add(this.gridCtrTestSteps);
            this.m_splitCntCtrlHorizon.Panel1.Text = "Panel1";
            this.m_splitCntCtrlHorizon.Panel2.Controls.Add(this.gridCtrModuleSteps);
            this.m_splitCntCtrlHorizon.Panel2.Text = "Panel2";
            this.m_splitCntCtrlHorizon.Size = new System.Drawing.Size(1028, 528);
            this.m_splitCntCtrlHorizon.SplitterPosition = 458;
            this.m_splitCntCtrlHorizon.TabIndex = 23;
            this.m_splitCntCtrlHorizon.Text = "splitContainerControlHorizon";
            // 
            // gridCtrTestSteps
            // 
            this.gridCtrTestSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCtrTestSteps.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(8);
            this.gridCtrTestSteps.Location = new System.Drawing.Point(0, 0);
            this.gridCtrTestSteps.MainView = this.gridViewTestSteps;
            this.gridCtrTestSteps.Margin = new System.Windows.Forms.Padding(8);
            this.gridCtrTestSteps.Name = "gridCtrTestSteps";
            this.gridCtrTestSteps.Size = new System.Drawing.Size(1028, 458);
            this.gridCtrTestSteps.TabIndex = 2;
            this.gridCtrTestSteps.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTestSteps});
            // 
            // gridViewTestSteps
            // 
            this.gridViewTestSteps.GridControl = this.gridCtrTestSteps;
            this.gridViewTestSteps.Name = "gridViewTestSteps";
            this.gridViewTestSteps.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridViewTestSteps_CustomDrawCell);
            this.gridViewTestSteps.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewTestSteps_FocusedRowChanged);
            this.gridViewTestSteps.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridViewTestSteps_CustomUnboundColumnData);
            this.gridViewTestSteps.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridViewTestSteps_MouseUp);
            // 
            // gridCtrModuleSteps
            // 
            this.gridCtrModuleSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCtrModuleSteps.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(8);
            this.gridCtrModuleSteps.Location = new System.Drawing.Point(0, 0);
            this.gridCtrModuleSteps.LookAndFeel.SkinMaskColor = System.Drawing.Color.White;
            this.gridCtrModuleSteps.MainView = this.gridViewModuleSteps;
            this.gridCtrModuleSteps.Margin = new System.Windows.Forms.Padding(8);
            this.gridCtrModuleSteps.Name = "gridCtrModuleSteps";
            this.gridCtrModuleSteps.Size = new System.Drawing.Size(1028, 65);
            this.gridCtrModuleSteps.TabIndex = 3;
            this.gridCtrModuleSteps.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewModuleSteps});
            // 
            // gridViewModuleSteps
            // 
            this.gridViewModuleSteps.GridControl = this.gridCtrModuleSteps;
            this.gridViewModuleSteps.Name = "gridViewModuleSteps";
            this.gridViewModuleSteps.OptionsView.ShowGroupPanel = false;
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(8);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 528);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1028, 0);
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(8);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 528);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 528);
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(8);
            this.barDockControlBottom.Size = new System.Drawing.Size(1028, 0);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(8);
            this.barDockControlTop.Size = new System.Drawing.Size(1028, 0);
            // 
            // popupMenuTStepView
            // 
            this.popupMenuTStepView.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barEditItemSearchStep),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemUncheckAllSteps, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.popupMenuTStepView.Manager = this.barManagerTStepView;
            this.popupMenuTStepView.Name = "popupMenuTStepView";
            // 
            // barEditItemSearchStep
            // 
            this.barEditItemSearchStep.Caption = "Search Step";
            this.barEditItemSearchStep.Edit = this.repositoryItemSearchControl2;
            this.barEditItemSearchStep.Glyph = ((System.Drawing.Image)(resources.GetObject("barEditItemSearchStep.Glyph")));
            this.barEditItemSearchStep.Id = 2;
            this.barEditItemSearchStep.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barEditItemSearchStep.LargeGlyph")));
            this.barEditItemSearchStep.Name = "barEditItemSearchStep";
            this.barEditItemSearchStep.Width = 200;
            //this.barEditItemSearchStep.EditValueChanged += new System.EventHandler(this.barEditItemSearchStep_EditValueChanged);            
            // 
            // repositoryItemSearchControl2
            // 
            this.repositoryItemSearchControl2.AutoHeight = false;
            this.repositoryItemSearchControl2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.repositoryItemSearchControl2.Name = "repositoryItemSearchControl2";
            this.repositoryItemSearchControl2.KeyDown += new System.Windows.Forms.KeyEventHandler(repositoryItemSearchControl2_KeyDown);
            this.repositoryItemSearchControl2.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(repositoryItemSearchControl2_EditValueChanging);
            // 
            // barButtonItemUncheckAllSteps
            // 
            this.barButtonItemUncheckAllSteps.Caption = "Uncheck all step";
            this.barButtonItemUncheckAllSteps.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemUncheckAllSteps.Glyph")));
            this.barButtonItemUncheckAllSteps.Id = 3;
            this.barButtonItemUncheckAllSteps.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemUncheckAllSteps.LargeGlyph")));
            this.barButtonItemUncheckAllSteps.Name = "barButtonItemUncheckAllSteps";
            this.barButtonItemUncheckAllSteps.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemUncheckAllSteps_ItemClick);
            // 
            // barManagerTStepView
            // 
            this.barManagerTStepView.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManagerTStepView.DockControls.Add(this.barDockControl1);
            this.barManagerTStepView.DockControls.Add(this.barDockControl2);
            this.barManagerTStepView.DockControls.Add(this.barDockControl3);
            this.barManagerTStepView.DockControls.Add(this.barDockControl4);
            this.barManagerTStepView.Form = this;
            this.barManagerTStepView.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barEditItemSearchStep,
            this.barButtonItemUncheckAllSteps});
            this.barManagerTStepView.MainMenu = this.bar1;
            this.barManagerTStepView.MaxItemId = 4;
            this.barManagerTStepView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSearchControl1,
            this.repositoryItemSearchControl2});
            // 
            // bar1
            // 
            this.bar1.BarName = "CustomRClickBar";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.FloatLocation = new System.Drawing.Point(444, 667);
            this.bar1.HideWhenMerging = DevExpress.Utils.DefaultBoolean.True;
            this.bar1.OptionsBar.AllowRename = true;
            this.bar1.Text = "CustomRClickBar";
            this.bar1.Visible = false;
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
            this.barDockControl1.Size = new System.Drawing.Size(1028, 0);
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl2.Location = new System.Drawing.Point(0, 528);
            this.barDockControl2.Size = new System.Drawing.Size(1028, 20);
            // 
            // barDockControl3
            // 
            this.barDockControl3.CausesValidation = false;
            this.barDockControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl3.Location = new System.Drawing.Point(0, 0);
            this.barDockControl3.Size = new System.Drawing.Size(0, 528);
            // 
            // barDockControl4
            // 
            this.barDockControl4.CausesValidation = false;
            this.barDockControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl4.Location = new System.Drawing.Point(1028, 0);
            this.barDockControl4.Size = new System.Drawing.Size(0, 528);
            // 
            // repositoryItemSearchControl1
            // 
            this.repositoryItemSearchControl1.AutoHeight = false;
            this.repositoryItemSearchControl1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.repositoryItemSearchControl1.Name = "repositoryItemSearchControl1";
            // 
            // userCtrMainView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.m_splitCntCtrlHorizon);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Controls.Add(this.barDockControl3);
            this.Controls.Add(this.barDockControl4);
            this.Controls.Add(this.barDockControl2);
            this.Controls.Add(this.barDockControl1);
            this.Margin = new System.Windows.Forms.Padding(8);
            this.Name = "userCtrMainView";
            this.Size = new System.Drawing.Size(1028, 548);
            ((System.ComponentModel.ISupportInitialize)(this.m_splitCntCtrlHorizon)).EndInit();
            this.m_splitCntCtrlHorizon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrTestSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTestSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrModuleSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewModuleSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuTStepView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerTStepView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl m_splitCntCtrlHorizon;
        private DevExpress.XtraGrid.GridControl gridCtrTestSteps;
        public DevExpress.XtraGrid.GridControl GridCtrTestSteps
        {
            get { return gridCtrTestSteps; }
            set { gridCtrTestSteps = value; }
        }
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTestSteps;
        public DevExpress.XtraGrid.Views.Grid.GridView GridViewTestSteps
        {
            get { return gridViewTestSteps; }
            set { gridViewTestSteps = value; }
        }
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraGrid.GridControl gridCtrModuleSteps;
        public DevExpress.XtraGrid.GridControl GridCtrModuleSteps
        {
            get { return gridCtrModuleSteps; }
            set { gridCtrModuleSteps = value; }
        }
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewModuleSteps;
        private DevExpress.XtraBars.PopupMenu popupMenuTStepView;
        private DevExpress.XtraBars.BarManager barManagerTStepView;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraBars.BarDockControl barDockControl3;
        private DevExpress.XtraBars.BarDockControl barDockControl4;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchControl repositoryItemSearchControl1;
        private DevExpress.XtraBars.BarEditItem barEditItemSearchStep;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchControl repositoryItemSearchControl2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemUncheckAllSteps;
    
        public DevExpress.XtraGrid.Views.Grid.GridView GridViewModuleSteps
        {
            get { return gridViewModuleSteps; }
            set { gridViewModuleSteps = value; }
        }
    }
}
