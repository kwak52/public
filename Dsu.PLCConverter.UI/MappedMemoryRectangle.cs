using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dsu.PLCConverter.UI.AddressMapperLogics;

namespace Dsu.PLCConverter.UI
{
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
        internal AllocatedMemoryRange AllocatedMemoryRange;
        public int Start { get; set; }
        public int End { get; set; }
        public int Length => End - Start;
        UcMemoryBar _parent;
        public MappedMemoryRectangle Counterpart
        {
            get
            {
                var logicalCounterpart = AllocatedMemoryRange.Counterpart;
                return 
                    _parent.Counterpart.RangeControls
                    .OfType<MappedMemoryRectangle>()
                    .FirstOrDefault(c => c.AllocatedMemoryRange.Start == logicalCounterpart.Start && c.AllocatedMemoryRange.End == logicalCounterpart.End)
                    ;
            }
        }

        void InitializeContextMenu()
        {
            var cms = new ContextMenuStrip();
            var items = cms.Items;
            items.Add(new ToolStripMenuItem("Remove mapping", Images.Clear, (o, a) =>
            {
                _parent.RemoveMemoryRange(this);
                _parent.Counterpart.RemoveMemoryRange(Counterpart);
            }));
            this.MouseClick += (s, e) => {
                if (e.Button == MouseButtons.Right)
                    cms.Show(MousePosition);
            };
        }

        async Task AnimateSelection()
        {
            var counterMemTypeName = AllocatedMemoryRange.Counterpart.Parent.Name;
            var counterBarMemTypeName = _parent.Counterpart.MemorySection.Name;

            // 사전에 mapping 한 memory type 이 선택되어 있지 않은 경우, combo 에서 맞는 memory type 을 선택
            if (counterMemTypeName != counterBarMemTypeName)
                Subjects.MemorySectionChangeRequestSubject.OnNext(Tuple.Create(_parent.Counterpart.PLCVendor, counterMemTypeName));

            await Task.Run(async () =>
            {
                var src = BackColor;
                var invert = Color.FromArgb(BackColor.ToArgb() ^ 0xffffff);
                for ( int i = 0; i < 10; i++ )
                {
                    BackColor = 
                    Counterpart.BackColor = src.InterpolateBetween(invert, (double)(i + 1) / 10);
                    await Task.Delay(70);
                }
                for (int i = 0; i < 10; i++)
                {
                    BackColor =
                    Counterpart.BackColor = invert.InterpolateBetween(src, (double)(i + 1) / 10);
                    await Task.Delay(70);
                }
            });
        }

        public MappedMemoryRectangle(UcMemoryBar parent, AllocatedMemoryRange r)
        {
            _parent = parent;
            AllocatedMemoryRange = r;

            Start = r.Start;
            End = r.End;
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.Aqua;

            InitializeContextMenu();

            Click += async (s, e) =>
            {
                _parent.ClearActiveRangeSelector();
                Global.Logger.Debug($"Mapped rectangle clicked.");
                await AnimateSelection();
            };
        }
    }
}
