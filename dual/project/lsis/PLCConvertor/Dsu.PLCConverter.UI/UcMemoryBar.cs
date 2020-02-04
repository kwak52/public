using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dsu.PLCConverter.UI
{
    public partial class UcMemoryBar : UserControl
    {
        public int Start { get; set; } = 0;
        public int End { get; set; } = 0;
        public List<Tuple<int, int>> Ranges = new List<Tuple<int, int>>();

        public UcMemoryBar(int start, int end)
        {
            InitializeComponent();
        }

        private void UcMemoryBar_Load(object sender, EventArgs e)
        {

        }
    }
}
