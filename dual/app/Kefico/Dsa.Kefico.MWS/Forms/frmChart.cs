using Dsa.Kefico.MWS.EventHandler;
using System;
using System.Data;
using System.Windows.Forms;

namespace Dsa.Kefico.MWS
{
    public partial class FrmChart : Form
    {
        public UEventHandlerBoundDataChanged UEventBoundDataChanged;


        public FrmChart(string Caption)
        {
            InitializeComponent();
            ucChartXbar1.TitleMain = "SPC";
            Text = Caption;
        }

        public void ShowChart(ChartSPC chartSPC, bool bScaleAuto)
        {
            ucChartXbar1.TitleSub = chartSPC.Title;
            ucChartXbar1.ShowChart(chartSPC.Source, chartSPC.Argement, chartSPC.Value, bScaleAuto, chartSPC.DisplayType, true, chartSPC.MinY, chartSPC.MaxY);
            ucChartXbar1.UEventBoundDataChanged += ucChartXbar1_UEventBoundDataChanged;
        }

        public void ClearChart()
        {
            ucChartXbar1.TitleSub = string.Empty;
            ucChartXbar1.UEventBoundDataChanged -= ucChartXbar1_UEventBoundDataChanged;
            ucChartXbar1.ClearChart();
        }
        public void AxisYRangeAuto(bool bAuto)
        {
            ucChartXbar1.AxisYRangeAuto(bAuto);
        }

        private void ucChartXbar1_UEventBoundDataChanged(object sender, int RecordCount)
        {
            UEventBoundDataChanged?.Invoke(sender, RecordCount);
        }

        private void frmChart_Activated(object sender, EventArgs e)
        {
            UEventBoundDataChanged?.Invoke(sender, ucChartXbar1.Points);
        }
    }
}