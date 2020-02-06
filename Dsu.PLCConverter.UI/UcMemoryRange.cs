using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;
using Dsu.PLCConverter.UI.AddressMapperLogics;
using Dsu.PLCConvertor.Common;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.PLCConverter.UI
{
    /// <summary>
    /// Custom Range Control 을 포함하는 메모리 영역 지정을 위한 사용자 control
    /// </summary>
    public partial class UcMemoryRange
        : UserControl
        , IMemoryRange
    {
        CustomRangeControl _crc => customRangeControl1;
        NumericRangeControlClient _client => (NumericRangeControlClient)customRangeControl1.Client;

        public UcMemoryRange()
        {
            InitializeComponent();
        }


        //bool _enableEditValueChanging = true;
        private void UcMemoryRange_Load(object sender, EventArgs args)
        {
            void SetRange(RangeControlRange range)
            {
                //textEditStart.EditValue = range.Minimum;
                //textEditEnd.EditValue = range.Maximum;
            }
            int GetSourceRangeLength() => ((UcMemoryBar)Parent).Counterpart.ActiveRangeSelector.SelectedRange.GetLength();

            _crc.RangeChanging += (s, e) => SetRange(e.Range);
            _crc.RangeChanged += (s, e) =>
            {
                SetRange(e.Range);
            };

            ForeColor = Color.Maroon;
            SelectionType = RangeControlSelectionType.ThumbAndFlag;

            var omronMemBar = ((UcMemoryBar)Parent).Counterpart;
            var cms = new ContextMenuStrip();
            var items = cms.Items;
            switch(((UcMemoryBar)Parent).PLCVendor)
            {
                case PLCVendor.LSIS:
                    items.Add(new ToolStripMenuItem("숫자로 정확히 입력", Images.T, (o, a) =>
                    {
                        var srcLength = GetSourceRangeLength();
                        new FormRange(this, srcLength).ShowDialog();
                    }));
                    items.Add(new ToolStripMenuItem("시작 기준 맞추기", Images.T, (o, a) =>
                    {
                        var srcLength = GetSourceRangeLength();
                        SelectedRange.Minimum = Minimum;
                        SelectedRange.Maximum = srcLength - 1;
                    }));
                    items.Add(new ToolStripMenuItem("끝 기준 맞추기", Images.T, (o, a) =>
                    {
                        var srcLength = GetSourceRangeLength();
                        SelectedRange.Maximum = Maximum;
                        SelectedRange.Minimum = Maximum - srcLength;
                    }));
                    break;
                case PLCVendor.Omron:
                    items.Add(new ToolStripMenuItem("숫자로 정확히 입력", Images.T, (o, a) =>
                    {
                        new FormRange(this).ShowDialog();
                    }));
                    break;
                default:
                    throw new Exception("Unknown case.");
            }

            _crc.MouseClick += (s, e) => {
                if (e.Button == MouseButtons.Right)
                    cms.Show(MousePosition);
            };

            _crc.Dock = DockStyle.Fill;
        }

        public int Maximum { get { return (int)_client.Maximum; } set { _client.Maximum = (int)value; } }
        public int Minimum { get { return (int)_client.Minimum; } set { _client.Minimum = (int)value; } }
        public int RulerDelta { get { return (int)_client.RulerDelta; } set { _client.RulerDelta = (int)value; } }
        public Color ForeColor { get { return _crc.Appearance.ForeColor; } set { _crc.Appearance.ForeColor = value; } }
        public RangeControlSelectionType SelectionType { get { return _crc.SelectionType; } set { _crc.SelectionType = value; } }
        public RangeControlRange SelectedRange { get { return _crc.SelectedRange; } set { _crc.SelectedRange = value; } }
        public IMemoryRange ActiveRange => SelectedRange.ToMemoryRange();

        public int Start => Minimum;
        public int End => Maximum;
        public int Length => End - Start;
    }
    public static class EmRangeControl
    {
        public static IMemoryRange ToMemoryRange(this RangeControlRange selectedRange)
        {
            if (selectedRange.Minimum == null || selectedRange.Maximum == null)
                return null;
            return new MemoryRangeBase((int)selectedRange.Minimum, (int)selectedRange.Maximum);
        }

        public static int GetLength(this RangeControlRange range) => (int)range.Maximum - (int)range.Minimum;

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
                if (   ViewInfo.PressedInfo.HitTest == RangeControlHitTest.MinRangeThumb
                    || ViewInfo.PressedInfo.HitTest == RangeControlHitTest.MaxRangeThumb
                    || ViewInfo.PressedInfo.HitTest == RangeControlHitTest.RangeBox
                    )
                {
                    (RangeControl as CustomRangeControl).RaiseChangingEvent();
                }
            }
        }
        public override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            RaiseRangeChangeEvent(e);
        }
    }
}
