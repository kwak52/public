using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dsu.PLCConverter.UI.AddressMapperLogics;
using Dsu.Common.Utilities.Core.ExtensionMethods;
using System.Diagnostics;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.PLCConverter.UI
{
    /// <summary>
    /// 메모리 타입 관리용 UI.  내부에 여러개의 range 를 가짐
    /// </summary>
    public partial class UcMemoryBar : UserControl
    {
        MemorySection _memorySection;
        List<Control> _rangeControls = new List<Control>();
        public MemorySection MemorySection
        {
            get { return _memorySection; }
            set {
                _memorySection = value;
                if (_memorySection != null)
                    DrawRanges();
            }
        }
        List<MemoryRange> _ranges => MemorySection.MemoryRanges;
        public int Start => MemorySection.Start;
        public int End => MemorySection.End;

        public UcMemoryBar()
        {
            InitializeComponent();
        }

        private void UcMemoryBar_Load(object sender, EventArgs args)
        {
            SizeChanged += (s, e) => DrawRanges();
        }

        void DrawRanges()
        {
            _rangeControls.Iter(c => Controls.Remove(c));
            _rangeControls.Clear();
            (var x, var y) = (Location.X, Location.Y);
            (var w, var h) = (Width, Height);
            var totalLength = End - Start;

            /// logical to physical
            int l2p(int n) => w * n / totalLength;
            PictureBox rect(MemoryRange r)
            {
                var width = l2p(r.End - r.Start);
                var loc = new Point(x + l2p(r.Start), y);
                return new PictureBox() { Location = loc, Width = width, Height = h };
            }
            UcMemoryRange range(RangedMemory r)
            {
                var width = l2p(r.End - r.Start);
                var loc = new Point(l2p(r.Start), 0);
                var ucRange = new UcMemoryRange() { Location = loc, Width = width, Height = h };
                ucRange.Minimum = r.Start;
                ucRange.Maximum = r.End;
                return ucRange;
            }
            var i = 0;
            if (_ranges.Any())
            {
                foreach (var r in _ranges)
                {
                    Debug.Assert(i <= r.Start);

                    //if (r.Start == i)
                    _rangeControls.Add(rect(r));
                }
            }
            else
                _rangeControls.Add(range(MemorySection));

            Controls.AddRange(_rangeControls.ToArray());
        }
    }
}
