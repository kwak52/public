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

namespace Dsu.PLCConverter.UI
{
    public partial class UcMemoryBar : UserControl
    {
        MemorySection _memorySection;
        public int Start => _memorySection.Start;
        public int End => _memorySection.End;
        public List<Tuple<int, int>> Ranges = new List<Tuple<int, int>>();

        public UcMemoryBar(MemorySection memorySection)
        {
            InitializeComponent();
            _memorySection = memorySection;
        }

        private void UcMemoryBar_Load(object sender, EventArgs e)
        {

        }
    }
}
