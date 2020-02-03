using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using log4net.Appender;
using System.Configuration;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace AddressMapper
{
    public partial class FormAddressMapper
        : DevExpress.XtraBars.Ribbon.RibbonForm
        , IAppender
    {
        void WireDockPanelVisibility(Dsu.Common.Utilities.Actions.Action action, DockPanel dockPanel, BarCheckItem checkItem)
        {
            dockPanel.ClosingPanel += (s, e) =>
            {
                e.Cancel = true;
                dockPanel.Hide();
            };

            checkItem.CheckedChanged += (s, e) =>
            {
                if (checkItem.Checked)
                    dockPanel.Show();
                else
                    dockPanel.Hide();
            };
            action.Update += (s, e) => checkItem.Checked = dockPanel.Visibility == DockVisibility.Visible;
        }

        //NumericRangeControlClient numericRangeControlClient1;
        //void CreateNumericRangeClient()
        //{
        //    this.numericRangeControlClient1 = new DevExpress.XtraEditors.NumericRangeControlClient();
        //    //customRangeControl1.Client = this.numericRangeControlClient1;
        //    this.numericRangeControlClient1.Maximum = 10000;
        //    //this.numericRangeControlClient1.RangeControl = this.customRangeControl1;
        //    this.numericRangeControlClient1.RulerDelta = 16;
        //}
        public FormAddressMapper()
        {
            InitializeComponent();
        }

        private void FormAddressMapper_Load(object sender, EventArgs args)
        {
            Logger.Info("FormAddressMapper launched.");
            //WireDockPanelVisibility(action1, dockPanelMain, barCheckItemShowMain);
            //WireDockPanelVisibility(action1, dockPanelLog, barCheckItemShowLog);
            //WireDockPanelVisibility(action1, dockPanelSource, barCheckItemSource);
            //WireDockPanelVisibility(action1, dockPanelTarget, barCheckItemTarget);


            rangeControl1.RangeChanging += (s, e) =>
            {
                var r = e.Range;
                Trace.WriteLine($"Range changing : {r.Minimum} ~ {r.Maximum}");
            };
            rangeControl1.RangeChanged += (s, e) =>
            {
                var r = e.Range;
                Trace.WriteLine($"Range changed. : {r.Minimum} ~ {r.Maximum}");
            };

            //rangeControlNormal.RangeChanging += (s, e) => Trace.WriteLine("Range changing...");
            rangeControlNormal.RangeChanged += (s, e) =>
            {
                var r = rangeControlNormal.SelectedRange;
                Trace.WriteLine($"Range changed : {r.Minimum} ~ {r.Maximum}");
            };
        }
    }

    /// <summary>
    /// Custom range control : https://www.devexpress.com/Support/Center/Question/Details/T397835/rangecontrol-where-is-the-rangechanging-event
    /// </summary>
    public class CustomRangeControl : RangeControl
    {
        public delegate void RangeChangingEventHandler(object sender, RangeControlRangeEventArgs e);
        public event RangeChangingEventHandler RangeChanging;

        public CustomRangeControl() : base() { }
        protected override RangeControlHandler CreateHandler()
        {
            return new CustomRangeControlHandler(this);
        }
        internal void RaiseChangingEvent()
        {
            //var r = SelectedRange;
            //Trace.WriteLine($"Range changing event : {r.Maximum} ~ {r.Minimum}");
            if (RangeChanging != null)
            {
                var d1 = Client.GetValue(RangeViewInfo.RangeMinimum);
                var d2 = Client.GetValue(RangeViewInfo.RangeMaximum);
                var range = new RangeControlRange(d1, d2);
                
                Trace.WriteLine($"{d1} -- {d2}");
                RangeChanging(this, new RangeControlRangeEventArgs() { Range = range });
            }
        }
    }
    public class CustomRangeControlHandler : RangeControlHandler
    {
        public CustomRangeControlHandler(RangeControl rangeControl) : base(rangeControl) { }
        void RaiseRangeChangeEvent(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (ViewInfo.HotInfo.HitTest == RangeControlHitTest.MinRangeThumb
                    || ViewInfo.HotInfo.HitTest == RangeControlHitTest.MaxRangeThumb
                    || ViewInfo.HotInfo.HitTest == RangeControlHitTest.LeftScaleThumb
                    || ViewInfo.HotInfo.HitTest == RangeControlHitTest.RightScaleThumb

                    || ViewInfo.PressedInfo.HitTest == RangeControlHitTest.MinRangeThumb
                    || ViewInfo.PressedInfo.HitTest == RangeControlHitTest.MaxRangeThumb
                    || ViewInfo.PressedInfo.HitTest == RangeControlHitTest.LeftScaleThumb
                    || ViewInfo.PressedInfo.HitTest == RangeControlHitTest.RightScaleThumb
                    )
                {
                    (RangeControl as CustomRangeControl).RaiseChangingEvent();
                }
            }
        }
        public override void OnMouseMove(MouseEventArgs e)
        {
            //Trace.Write("M");
            base.OnMouseMove(e);
            RaiseRangeChangeEvent(e);
        }
        public override void OnMouseUp(MouseEventArgs e)
        {
            //Trace.Write("U");
            base.OnMouseUp(e);
            RaiseRangeChangeEvent(e);
        }
        public override void OnMouseDown(MouseEventArgs e)
        {
            //Trace.Write("D");
            base.OnMouseDown(e);
            RaiseRangeChangeEvent(e);
        }

    }
}