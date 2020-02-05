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
        public UcMemoryBar Counterpart { get; set; }
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
        public IMemoryRange SelectedRange { get; set; }

        public UcMemoryBar()
        {
            InitializeComponent();
        }


        private void UcMemoryBar_Load(object sender, EventArgs args)
        {
            SizeChanged += (s, e) => DrawRanges();
        }


        internal void ClearActiveRangeSelector()
        {
            _rangeControls.Remove(ActiveRangeSelector);
            Controls.Remove(ActiveRangeSelector);
            ActiveRangeSelector = null;
        }
        void CreateActiveRangeSelector(int start, int end)
        {
            var r = new MemoryRangeBase(start, end);
            var ucRange = range(r);
            _rangeControls.Add(ucRange);
            Controls.Add(ucRange);
            ActiveRangeSelector = ucRange;
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
        /// logical to physical
        int l2p(int n) => Width * n / Length;
        /// physical to logical
        int p2l(int x) => (int)(Length * x / Width);

        MappedMemoryRectangle rect(MemoryRange r)
        {
            var width = l2p(r.End - r.Start);
            var loc = new Point(l2p(r.Start), 0);
            return new MappedMemoryRectangle(this, r) { Location = loc, Width = width, Height = Height };
        }
        UcMemoryRange range(MemoryRangeBase r)
        {
            var width = l2p(r.End - r.Start);
            var loc = new Point(l2p(r.Start), 0);
            var ucRange = new UcMemoryRange() { Location = loc, Width = width, Height = Height };
            ucRange.Minimum = r.Start;
            ucRange.Maximum = r.End;
            // default : 전체 range 선택
            ucRange.SelectedRange.Minimum = r.Start;
            ucRange.SelectedRange.Maximum = r.End;
            ActiveRangeSelector = ucRange;
            return ucRange;
        }

        /// <summary>
        /// 활성 range 구간을 allocation 으로 처리
        /// </summary>
        public AllocatedMemoryRange ActiveRangeAllocated()
        {
            _logger.Debug($"Allocating Ranges for {Identifier}");

            var r = ActiveRangeSelector.ActiveRange;
            var allocatedRange = new AllocatedMemoryRange(r.Start, r.End, MemorySection);
            _ranges.Add(allocatedRange);
            ClearActiveRangeSelector();
            DrawRanges();
            return allocatedRange;
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
        private void UcMemoryBar_Click(object sender, EventArgs args)
        {
            var pt = PointToClient(MousePosition);

            var lpx = p2l(pt.X);
            _logger.Debug($"ActiveRangeSelector={ActiveRangeSelector}, Click Position={pt}, Logical position={lpx}");
            ClearActiveRangeSelector();

            Debug.Assert(_ranges.Any());
            var allS = _ranges.Select(r => r.Start).ToArray();
            var allE = _ranges.Select(r => r.End).ToArray();
            /// 클릭한 지점 이후에만 할당되어 있는 경우 : 맨 앞에 빈공간 클릭
            if (allS.ForAll(s => s > lpx))
            {
                var e = allS.Min() - 1;
                _logger.Debug($"Clicked range=[0, {e}]");
                CreateActiveRangeSelector(0, e);
            }
            /// 클릭한 지점 이전에만 할당된 경우 : 맨 뒤의 빈공간 클릭
            else if (allE.ForAll(e => e < lpx))
            {
                var s = allE.Max() + 1;
                _logger.Debug($"Clicked range=[{s}, {Length}]");
                CreateActiveRangeSelector(s, Length);
            }
            else
            {
                var rs = allE.Where(e => e < lpx).Max() + 1;
                var re = allS.Where(s => s > lpx).Min() - 1;
                _logger.Debug($"Clicked range=[{rs}, {re}]");
                CreateActiveRangeSelector(rs, re);
            }
        }

        /// <summary>
        /// 할당된 영역을 제거
        /// </summary>
        internal void RemoveMemoryRange(MappedMemoryRectangle m)
        {
            _rangeControls.Remove(m);
            Controls.Remove(m);
            _ranges.Remove(m.MemoryRange);
            DrawRanges();
        }
        /// <summary>
        /// 할당된 영역을 제거
        /// </summary>
        internal void RemoveMemoryRange(MemoryRange m)
        {
            var mappedMemoryRectange =
                _rangeControls
                    .OfType<MappedMemoryRectangle>()
                    .FirstOrDefault(c => c.MemoryRange.Start == m.Start && c.MemoryRange.End == m.End)
                    ;
            RemoveMemoryRange(mappedMemoryRectange);
        }
    }


    /// <summary>
    /// 이미 주소가 할당된 영역을 표시하는 rectangle
    /// </summary>
    public class MappedMemoryRectangle
        : PictureBox
        , IMemoryRange
    {
        /// <summary>
        /// Logical Memory Range
        /// </summary>
        internal MemoryRange MemoryRange;
        public int Start { get; set; }
        public int End { get; set; }
        public int Length => End - Start;
        UcMemoryBar _parent;
        ContextMenuStrip _contextMenuStrip;

        void InitializeContextMenu()
        {
            var _contextMenuStrip = new ContextMenuStrip();
            var items = _contextMenuStrip.Items;
            items.Add(new ToolStripMenuItem("Remove mapping", Images.Clear, (o, a) =>
            {
                _parent.RemoveMemoryRange(this);
                var countpartM = ((AllocatedMemoryRange)MemoryRange).Counterpart;
                _parent.Counterpart.RemoveMemoryRange(countpartM);
            }));
            this.MouseClick += (s, e) => {
                if (e.Button == MouseButtons.Right)
                    _contextMenuStrip.Show(MousePosition);
            };
        }

        public MappedMemoryRectangle(UcMemoryBar parent, MemoryRange r)
        {
            _parent = parent;
            MemoryRange = r;

            Start = r.Start;
            End = r.End;
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.Aqua;

            InitializeContextMenu();

            Click += (s, e) =>
            {
                _parent.ClearActiveRangeSelector();
                Global.Logger.Debug($"Mapped rectangle clicked.");
            };
        }
    }
}
