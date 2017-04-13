namespace CpTesterSs.UserControl
{
	partial class UcTResultChart
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
			DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
			DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
			this.chartControlResultData = new DevExpress.XtraCharts.ChartControl();
			((System.ComponentModel.ISupportInitialize)(this.chartControlResultData)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
			this.SuspendLayout();
			// 
			// chartControlResultData
			// 
			this.chartControlResultData.DataBindings = null;
			xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
			xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
			this.chartControlResultData.Diagram = xyDiagram1;
			this.chartControlResultData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chartControlResultData.Legend.Name = "Default Legend";
			this.chartControlResultData.Location = new System.Drawing.Point(0, 0);
			this.chartControlResultData.Name = "chartControlResultData";
			series1.Name = "Demo";
			series1.View = lineSeriesView1;
			this.chartControlResultData.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
			this.chartControlResultData.Size = new System.Drawing.Size(995, 585);
			this.chartControlResultData.TabIndex = 0;
			// 
			// UcTResultChart
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chartControlResultData);
			this.Name = "UcTResultChart";
			this.Size = new System.Drawing.Size(995, 585);
			((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartControlResultData)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraCharts.ChartControl chartControlResultData;
	}
}
