using Dsu.PLCConvertor.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLCConvertor.Forms
{
    public partial class FormLadderParse : Form
    {
        string[] _mnemonics;
        Rung4Parsing _rung4Parsing;
        IEnumerator<int> _parsingStages;

        public FormLadderParse()
        {
            InitializeComponent();
        }
        private void FormLadderParse_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        void Initialize()
        {
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

            _mnemonics = input4.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            _rung4Parsing = new Rung4Parsing(_mnemonics);
            _parsingStages = _rung4Parsing.CoRoutineRungParser().GetEnumerator();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            DrawCurrentBuildingLadder();

            //pictureBox1.Image = _rung4Parsing.GraphViz();

            if (!_parsingStages.MoveNext())
            {
                MessageBox.Show("Finished.");
                Initialize();
            }
        }

        private void CbRemoveAuxNode_CheckedChanged(object sender, EventArgs e)
        {
            DrawCurrentBuildingLadder();
        }

        void DrawCurrentBuildingLadder()
        {
            //var rung = _rung4Parsing.ToRung(cbRemoveAuxNode.Checked);
            //var graph = rung.GraphViz();
            //pictureBox1.Image = graph;

            pictureBox1.Image =
                cbRemoveAuxNode.Checked
                    ? _rung4Parsing.ToRung(true).GraphViz()
                    : _rung4Parsing.GraphViz()
                    ;

        }
    }
}
