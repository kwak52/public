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

        private void FormInputSelector_Load(object sender, EventArgs args)
        {
            comboBox1.DataSource = MnemonicInput.Inputs;
            comboBox1.DisplayMember = "Comment";
            comboBox1.SelectedIndexChanged += (s, e) => UpdateTextArea();
            cbAllowEdit.CheckedChanged += (s, e) => textBox1.ReadOnly = !cbAllowEdit.Checked;
            btnOK.Click += (s, e) => DialogResult = DialogResult.OK;

            UpdateTextArea();

            void UpdateTextArea() => textBox1.Text = MnemonicInput.Inputs[comboBox1.SelectedIndex].Input;
        }
    }


}
