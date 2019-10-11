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
    public partial class FormInputSelector : Form
    {
        public FormInputSelector()
        {
            InitializeComponent();
        }

        public string Contents => textBox1.Text;

        static int _selectedIndex;
        public MnemonicInput SelectedMnemonicInput;
        private void FormInputSelector_Load(object sender, EventArgs args)
        {
            comboBox1.DataSource = MnemonicInput.Inputs;
            comboBox1.DisplayMember = "Comment";
            comboBox1.SelectedIndex = _selectedIndex;
            comboBox1.SelectedIndexChanged += (s, e) => {
                textBox1.TextChanged -= TextChanged;
                _selectedIndex = comboBox1.SelectedIndex;
                UpdateTextArea();
                textBox1.TextChanged += TextChanged;
            };
            cbAllowEdit.CheckedChanged += (s, e) => textBox1.ReadOnly = !cbAllowEdit.Checked;
            btnOK.Click += (s, e) => DialogResult = DialogResult.OK;

            UpdateTextArea();
            textBox1.TextChanged += TextChanged;

            void UpdateTextArea()
            {
                SelectedMnemonicInput = MnemonicInput.Inputs[comboBox1.SelectedIndex];
                textBox1.Text = MnemonicInput.CommentOutMultiple(SelectedMnemonicInput.Input);
            }

            void TextChanged(object s, EventArgs e) => SelectedMnemonicInput = new MnemonicInput("Arbitary", textBox1.Text);
        }
    }


}
