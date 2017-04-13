namespace CpTesterPlatform.CpTrendView
{
    partial class CpTrendViewChart
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CpTrendViewChart));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.chartControlTrend = new DevExpress.XtraCharts.ChartControl();
            this.layoutControlGroupTC = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemChart = new DevExpress.XtraLayout.LayoutControlItem();
            this.popupMenuTrendChart = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barButtonItemClear = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemCfgDisplay = new DevExpress.XtraBars.BarButtonItem();
            this.barManagerTC = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlTrend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupTC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuTrendChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerTC)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.chartControlTrend);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroupTC;
            this.layoutControl1.Size = new System.Drawing.Size(1332, 713);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // chartControlTrend
            // 
            this.chartControlTrend.DataBindings = null;
            this.chartControlTrend.Legend.Name = "Default Legend";
            this.chartControlTrend.Location = new System.Drawing.Point(24, 24);
            this.chartControlTrend.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.chartControlTrend.Name = "chartControlTrend";
            this.chartControlTrend.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartControlTrend.Size = new System.Drawing.Size(1284, 665);
            this.chartControlTrend.TabIndex = 4;
            this.chartControlTrend.CustomDrawSeries += new DevExpress.XtraCharts.CustomDrawSeriesEventHandler(this.chartControlTrend_CustomDrawSeries);
            this.chartControlTrend.CustomDrawSeriesPoint += new DevExpress.XtraCharts.CustomDrawSeriesPointEventHandler(this.chartControlTrend_CustomDrawSeriesPoint);
            this.chartControlTrend.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chartControlTrend_MouseClick);
            // 
            // layoutControlGroupTC
            // 
            this.layoutControlGroupTC.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupTC.GroupBordersVisible = false;
            this.layoutControlGroupTC.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemChart});
            this.layoutControlGroupTC.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupTC.Name = "layoutControlGroupTC";
            this.layoutControlGroupTC.Size = new System.Drawing.Size(1332, 713);
            this.layoutControlGroupTC.TextVisible = false;
            // 
            // layoutControlItemChart
            // 
            this.layoutControlItemChart.Control = this.chartControlTrend;
            this.layoutControlItemChart.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemChart.Name = "layoutControlItemChart";
            this.layoutControlItemChart.Size = new System.Drawing.Size(1292, 673);
            this.layoutControlItemChart.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemChart.TextVisible = false;
            // 
            // popupMenuTrendChart
            // 
            this.popupMenuTrendChart.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemClear),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemCfgDisplay)});
            this.popupMenuTrendChart.Manager = this.barManagerTC;
            this.popupMenuTrendChart.Name = "popupMenuTrendChart";
            // 
            // barButtonItemClear
            // 
            this.barButtonItemClear.Caption = "Clear Data";
            this.barButtonItemClear.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemClear.Glyph")));
            this.barButtonItemClear.Id = 1;
            this.barButtonItemClear.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemClear.LargeGlyph")));
            this.barButtonItemClear.Name = "barButtonItemClear";
            this.barButtonItemClear.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemClear_ItemClick);
            // 
            // barButtonItemCfgDisplay
            // 
            this.barButtonItemCfgDisplay.Caption = "Display Option";
            this.barButtonItemCfgDisplay.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemCfgDisplay.Glyph")));
            this.barButtonItemCfgDisplay.Id = 2;
            this.barButtonItemCfgDisplay.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemCfgDisplay.LargeGlyph")));
            this.barButtonItemCfgDisplay.Name = "barButtonItemCfgDisplay";
            this.barButtonItemCfgDisplay.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemCfgDisplay_ItemClick);
            // 
            // barManagerTC
            // 
            this.barManagerTC.DockControls.Add(this.barDockControlTop);
            this.barManagerTC.DockControls.Add(this.barDockControlBottom);
            this.barManagerTC.DockControls.Add(this.barDockControlLeft);
            this.barManagerTC.DockControls.Add(this.barDockControlRight);
            this.barManagerTC.Form = this;
            this.barManagerTC.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemClear,
            this.barButtonItemCfgDisplay});
            this.barManagerTC.MaxItemId = 3;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1332, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 713);
            this.barDockControlBottom.Size = new System.Drawing.Size(1332, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 713);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1332, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 713);
            // 
            // CpTrendViewChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "CpTrendViewChart";
            this.Size = new System.Drawing.Size(1332, 713);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartControlTrend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupTC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuTrendChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerTC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraCharts.ChartControl chartControlTrend;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupTC;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemChart;
        private DevExpress.XtraBars.PopupMenu popupMenuTrendChart;
        private DevExpress.XtraBars.BarButtonItem barButtonItemClear;
        private DevExpress.XtraBars.BarButtonItem barButtonItemCfgDisplay;
        private DevExpress.XtraBars.BarManager barManagerTC;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}
