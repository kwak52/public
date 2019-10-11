using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PLCConvertor.Forms
{
    public partial class FormLadderParse : Form
    {
        MnemonicInput _mnemonicInput;
        string[] _mnemonics;
        Rung4Parsing _rung4Parsing;
        IEnumerator<int> _parsingStages;
        ILog Logger = Global.Logger;

        public FormLadderParse(MnemonicInput mnemonicInput)
        {
            InitializeComponent();
            Text = mnemonicInput.Comment;
            _mnemonicInput = mnemonicInput;
            _mnemonics = m2a(mnemonicInput.Input);
            listBoxControlMnemonics.DataSource = _mnemonics;
            listBoxControlMnemonics.SelectedIndex = 0;

            if (mnemonicInput.DesiredOutputs.NonNullAny())
                listBoxControlAnswer.DataSource = m2a(mnemonicInput.DesiredOutputs[0]);

            string[] m2a(string input) => MnemonicInput.MultilineString2Array(input);
        }
        private void FormLadderParse_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        void Initialize()
        {
            _rung4Parsing = new Rung4Parsing(_mnemonics);
            _parsingStages = _rung4Parsing.CoRoutineRungParser().GetEnumerator();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (!_parsingStages.MoveNext())
            {
                MessageBox.Show("Finished.");
                Initialize();
            }

            pictureBox1.Image = DrawCurrentBuildingLadder();

            listBoxControlMnemonics.SelectedIndex = _rung4Parsing.CurrentMnemonicIndex;
        }
        private void BtnEnd_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = GetFinalLadderImage();
        }
        private void BtnNewForm_Click(object sender, EventArgs e)
        {
            new FormScrollableImage(GetFinalLadderImage()).Show();
        }

        Image GetFinalLadderImage()
        {
            while (_parsingStages.MoveNext())
                ;

            var image = DrawCurrentBuildingLadder();
            listBoxControlMnemonics.SelectedIndex = _rung4Parsing.CurrentMnemonicIndex;


            var mnemonics = Rung2ILConvertor.Convert(_rung4Parsing.ToRung(false));
            var converted = String.Join("\r\n", mnemonics);
            Logger?.Info($"Reversely converted mnemonics for {_mnemonicInput.Comment}:\r\n{converted}");


            converted = MnemonicInput.CommentOutMultiple(converted);
            var input = MnemonicInput.CommentOutMultiple(_mnemonicInput.Input);
            var answer = input;
            bool correct = input == converted;
            if (!correct && _mnemonicInput.DesiredOutputs != null)
            {
                var answers = _mnemonicInput.DesiredOutputs.Select(o => MnemonicInput.CommentOutMultiple(o)).ToArray();
                if (answers.NonNullAny())
                {
                    correct = answers.Any(o => o == converted);
                    answer = answers[0];
                }
            }
            if (!correct)
                Logger?.Error($"Convert mismatch.  Desired output:\r\n{answer}");

            return image;
        }

        private void CbRemoveAuxNode_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = DrawCurrentBuildingLadder();
        }

        Image DrawCurrentBuildingLadder()
        {
            //var rung = _rung4Parsing.ToRung(cbRemoveAuxNode.Checked);
            //var graph = rung.GraphViz();
            //pictureBox1.Image = graph;
            return
            //pictureBox1.Image =
                cbRemoveAuxNode.Checked
                    ? _rung4Parsing.ToRung(true).GraphViz()
                    : _rung4Parsing.GraphViz()
                    ;

        }

    }
}
