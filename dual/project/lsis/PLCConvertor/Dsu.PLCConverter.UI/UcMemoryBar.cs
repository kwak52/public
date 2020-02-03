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

namespace Dsu.PLCConverter.UI
{
    public partial class UcMemoryBar : UserControl
    {
        public UcMemoryBar()
        {
            InitializeComponent();
        }


        bool _enableEditValueChanging = true;
        private void UcMemoryBar_Load(object sender, EventArgs args)
        {
            textEditStart.EditValue = 0;
            textEditEnd.EditValue = 0;

            void SetRange(RangeControlRange range)
            {
                textEditStart.EditValue = range.Minimum;
                textEditEnd.EditValue = range.Maximum;
            }
            customRangeControl1.RangeChanging += (s, e) => SetRange(e.Range);
            customRangeControl1.RangeChanged += (s, e) =>
            {
                SetRange(e.Range);
            };

            textEditStart.EditValueChanging += (s, e) =>
            {
                try
                {
                    if (_enableEditValueChanging)
                    {
                        _enableEditValueChanging = false;
                        var sr = customRangeControl1.SelectedRange;
                        int newValue = -1;
                        if (int.TryParse(textEditStart.Text, out newValue) && 0 <= newValue && newValue <= (int)sr.Maximum)
                            sr.Minimum = newValue;
                        else
                            e.Cancel = true;
                    }
                }
                catch(Exception ex)
                {
                    e.Cancel = true;
                }
                finally
                {
                    _enableEditValueChanging = true;
                }
            };
            textEditEnd.EditValueChanging += (s, e) =>
            {
                try
                {
                    if (_enableEditValueChanging)
                    {
                        _enableEditValueChanging = false;
                        var sr = customRangeControl1.SelectedRange;
                        int newValue = -1;
                        if (int.TryParse(textEditEnd.Text, out newValue) && (int)sr.Minimum <= newValue && newValue <= Maximum)
                            sr.Maximum = newValue;
                        else
                            e.Cancel = true;
                    }
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                }
                finally
                {
                    _enableEditValueChanging = true;
                }
            };
            //textEditStart.EditValueChanged += (s, e) => customRangeControl1.SelectedRange.Minimum = int.Parse(textEditStart.Text);
        }

        NumericRangeControlClient _client => (NumericRangeControlClient)customRangeControl1.Client;
        public int Maximum { get { return (int)_client.Maximum; } set { _client.Maximum = (int)value; } }
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
