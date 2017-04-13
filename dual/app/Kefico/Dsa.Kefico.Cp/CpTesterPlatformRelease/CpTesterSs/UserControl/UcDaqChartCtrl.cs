using CpTesterPlatform.Functions;
using DevExpress.XtraCharts;
using Dsu.Common.Resources;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Driver.UI.NiDaq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CpBase.CpLog4netLogging;

namespace CpTesterSs.UserControl
{
    public class UcDaqChartCtrl : DaqChartCtrl
    {
        public static Subject<double[]> TestDaqDataSubject = new Subject<double[]>();

        /// whether dynamic chart, or static chart
        public bool IsDynamic { get; set; }
        public UcMainViewSs MainView { get; private set; }

        private HashSet<string> _displayChannel = new HashSet<string>();
        private string _selectedChannel = "";


        private static Font _legendFont = new Font("Times New Roman", 14.0f, FontStyle.Bold);
        public UcDaqChartCtrl(UcMainViewSs mainView, bool isDynamic)
        {
            IsDynamic = isDynamic;
            MainView = mainView;
            //ActivateDynamicDraw = false;    // CP tester 에서의 기본값은 false 로 설정한다.

            mainView.DaqChartItem.ForEach(ch => _displayChannel.Add(ch));
            _selectedChannel = _displayChannel.FirstOrDefault();

            var random = new Random();
            var subscription =
                DaqChartEvent.DaqSubject
                .Where(r => r.StationIndex == MainView.MngStation.StationId && r.Function == _selectedChannel)
                .Subscribe(async r =>
            {
                try
                {
                    if (!IsDynamic && Diagram != null)
                    {
                        var data = r.DATA;
                        var min = r.MIN;
                        var max = r.MAX;

                        SetMinMaxGuideLine(min, max);
                        var channel = r.Function;

                        // Main UI 에서 수행된다.
                        await RedrawChart(data);

                        _chart.Series[0].Name = channel;
                        _chart.Series[0].ShowInLegend = true;
                        _chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
                        _chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;

                        _chart.Legend.Font = _legendFont;
                        _chart.ShowLegend();
                    }
                }
                catch (Exception ex)
                {
                    LogError($"Exception on DaqChart: {ex}");
                }
            });

            TestDaqDataSubject.Subscribe(data =>
            {
                if (!IsDynamic)
                {
                    //var data = evt.DATA.Take(1000).ToArray();
                    //var r = evt.MAX - evt.MIN;
                    //var data = Enumerable.Range(0, 1000).Select(n => random.NextDouble() / r).ToArray();
                    SetMinMaxGuideLine(0.1, 1.45);

                    RedrawChart(data);
                }
            });

            Globals.ApplicationExitSubject.Subscribe(s =>
            {
                subscription.Dispose();
            });
        }

        protected override void RegisterEventHandler()
        {
            base.RegisterEventHandler();
            _chart.MouseClick += (sndr, e) =>
            {
                if (e.Button == MouseButtons.Right && ModifierKeys != Keys.ControlKey)
                {
                    var menu = new ContextMenuStrip();

                    if (IsDynamic)
                    {
                        //menu.Items.Add(new ToolStripMenuItem(ActivateDynamicDraw ? "Dectivate" : "Activate", null, (obj, a) =>
                        //{
                        //    // 주어진 시간 한도(60초) 내에서만 dynamic 을 display 한다.
                        //    ActivateDynamicDraw = !ActivateDynamicDraw;
                        //    if (ActivateDynamicDraw)
                        //    {
                        //        Task.Run(async () =>
                        //        {
                        //            await Task.Delay(1000 * 60);
                        //            ActivateDynamicDraw = false;
                        //            Clear();
                        //        });
                        //    }
                        //    else
                        //        Clear();
                        //})
                        //{
                        //    Checked = ActivateDynamicDraw,
                        //    Image = ActivateDynamicDraw ? Images.LightGreen : Images.ActionAdd,
                        //});
                    }
                    else
                    {
                        foreach(var ch in _displayChannel)
                        {
                            bool selected = _selectedChannel == ch;
                            menu.Items.Add(new ToolStripMenuItem(ch, null, (obj, a) =>
                            {
                                Clear();
                                _selectedChannel = ch;
                            })
                            {
                                Checked = selected,
                                Image = selected ? Images.LightGreen : Images.ActionAdd,
                            });
                        }
                    }

                    menu.Show(this, this.PointToClient(MousePosition));

                }
            };
        }

    }
}
