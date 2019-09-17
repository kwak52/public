using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dsu.PLCConvertor.Common;

namespace PLCConvertor
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
        }

        string input1 = @"LD A
AND B
OR C
OUT D
";

        string input2 = @"LD A
OUT TR0
AND B
OUT O1
LD TR0
AND C
OUT O2
";


        string input3 = @"LD 0.00
OUT TR0
AND 0.01
OUT 110.00
LD TR0
AND 110.00
OUT 102.10
";

        string input4 = @"LD 0.00
LD 0.01
OUT TR0
AND 0.02
ORLD
AND 0.03
OUT 102.11
LD TR0
AND 0.04
OUT 102.12
";


        private void btnTest_Click(object sender, EventArgs e)
        {
            var inputs = input4.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var rung = Rung.CreateRung(inputs);
            var graph = rung.GraphViz();
            var _pictureBox = new PictureBox() { Image = graph, Dock = DockStyle.Fill };
            var _formGraphviz = new Form() { Size = new Size(800, 500) };
            _formGraphviz.Controls.Add(_pictureBox);
            _formGraphviz.Show();
        }
    }
}
