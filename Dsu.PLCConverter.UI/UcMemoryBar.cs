using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dsu.PLCConverter.UI.AddressMapperLogics;
using System.Diagnostics;
using Dsu.Common.Utilities.ExtensionMethods;
using log4net;

namespace Dsu.PLCConverter.UI
{
    /// <summary>
    /// 메모리 타입 관리용 UI.  내부에 여러개의 range 를 가짐
    /// </summary>
    public partial class UcMemoryBar
        : UserControl
        , IMemoryRange
    {
        MemorySection _memorySection;
        List<Control> _rangeControls = new List<Control>();
        ILog _logger => Global.Logger;

        /// <summary>
        /// 메모리 타입 자료 구조.  children 으로 MemoryRanges 를 가짐
        /// </summary>
        public MemorySection MemorySection
        {
            get { return _memorySection; }
            set {
                _memorySection = value;
                DrawRanges();
            }
        }
        List<MemoryRange> _ranges => MemorySection.MemoryRanges;
        public UcMemoryRange ActiveRangeSelector = null;
        public int Start => MemorySection.Start;
        public int End => MemorySection.End;
        public int Length => End - Start;
        public string Identifier { get; set; }

        public UcMemoryBar()
        {
            InitializeComponent();
        }

        public IMemoryRange SelectedRange { get; set; }

        private void UcMemoryBar_Load(object sender, EventArgs args)
        {
            SizeChanged += (s, e) => DrawRanges();
        }

        public void DrawRanges()
        {
            if (DesignMode)
                return;
            if (_memorySection == null)
                return;

            ActiveRangeSelector = null;
            _rangeControls.Iter(c => Controls.Remove(c));
            _rangeControls.Clear();
            (var w, var h) = (Width, Height);

            /// logical to physical
            int l2p(int n) => w * n / Length;
            MappedMemoryRectangle rect(MemoryRange r)
            {
                var width = l2p(r.End - r.Start);
                var loc = new Point(l2p(r.Start), 0);
                return new MappedMemoryRectangle(r) { Location = loc, Width = width, Height = h };
            }
            UcMemoryRange range(MemoryRangeBase r)
            {
                var width = l2p(r.End - r.Start);
                var loc = new Point(l2p(r.Start), 0);
                var ucRange = new UcMemoryRange() { Location = loc, Width = width, Height = h };
                ucRange.Minimum = r.Start;
                ucRange.Maximum = r.End;
                // default : 전체 range 선택
                ucRange.SelectedRange.Minimum = r.Start;
                ucRange.SelectedRange.Maximum = r.End;
                ActiveRangeSelector = ucRange;
                return ucRange;
            }
            var i = 0;
            if (_ranges.Any())
            {
                foreach (var r in _ranges)
                {
                    Debug.Assert(i <= r.Start);

                    //if (r.Start == i)
                    var rr = rect(r);
                    var msg = $"Adding rectangle for {Identifier} at location={rr.Location}, Width={rr.Width}, Height={rr.Height}";
                    _logger.Debug(msg);
                    _rangeControls.Add(rr);
                    rr.BringToFront();
                }
            }
            else
                _rangeControls.Add(range(MemorySection));

            Controls.AddRange(_rangeControls.ToArray());
        }

        /// <summary>
        /// 활성 range 구간을 allocation 으로 처리
        /// </summary>
        public void ActiveRangeAllocated()
        {
            _logger.Debug($"Allocating Ranges for {Identifier}");

            var r = ActiveRangeSelector.ActiveRange;
            _ranges.Add(new AllocatedMemoryRange(r.Start, r.End, MemorySection));
            _rangeControls.Remove(ActiveRangeSelector);
            Controls.Remove(ActiveRangeSelector);
            ActiveRangeSelector = null;

            DrawRanges();
        }

        public void DumpMemoryRanges()
        {
            _logger.Info($"Ranges for {Identifier}");
            _logger.Debug("Logical ranges");
            _logger.Debug($"{MemorySection.Name}");
            _ranges.Iter(r => _logger.Debug($"[{r.Start}:{r.End}]"));

            _logger.Debug("Physical ranges");
            _rangeControls.Iter(r => _logger.Debug($"Location={r.Location}, Width={r.Width}, Height={r.Height}"));
        }

        /// <summary>
        /// 아직 할당되지 않은 영역을 click 한 경우의 처리
        /// 이미 할당된 영역이거나 UcMemoryRange(active range selector) 가 영역은 click event 자체가 발생하지 않는다.
        /// </summary>
        private void UcMemoryBar_Click(object sender, EventArgs e)
        {
            /// physical to logical
            int p2l(int x) => (int)(Length * x / Width);
            var pt = PointToClient(MousePosition);

            _logger.Debug($"ActiveRangeSelector={ActiveRangeSelector}, Click Position={pt}, Logical position={p2l(pt.X)}");
        }
    }


    /// <summary>
    /// 이미 주소가 할당된 영역을 표시하는 rectangle
    /// </summary>
    public class MappedMemoryRectangle
        : PictureBox
        , IMemoryRange
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Length => End - Start;
        public MappedMemoryRectangle(IMemoryRange r)
        {
            Start = r.Start;
            End = r.End;
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.Aqua;
        }
    }
}
