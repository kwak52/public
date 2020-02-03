using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConverter.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dsu.PLCConverter.UI
{
    public partial class FormRange : Form
    {
        UcMemoryBar _memBar;
        public FormRange(UcMemoryBar memBar)
        {
            InitializeComponent();
            _memBar = memBar;
        }

        private void FormRange_Load(object sender, EventArgs e)
        {

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            _memBar.SelectedRange.Minimum = int.Parse(textEditStart.Text);
            _memBar.SelectedRange.Maximum = int.Parse(textEditEnd.Text);
        }

        private void textEditStart_Validating(object sender, CancelEventArgs e)
        {
            var min = textEditStart.Text.TryParseInt();
            var max = textEditEnd.Text.TryParseInt();
            if (    min == null || max == null
                || !min.Value.InClosedRange(0, _memBar.Maximum)
                || !max.Value.InClosedRange(min.Value, _memBar.Maximum))
            {
                e.Cancel = true;
            }
            Console.WriteLine("");
        }
    }
}
