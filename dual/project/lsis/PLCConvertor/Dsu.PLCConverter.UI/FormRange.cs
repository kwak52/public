using DevExpress.XtraEditors;
using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dsu.PLCConverter.UI
{
    public partial class FormRange : Form
    {
        UcMemoryRange _memBar;
        public FormRange(UcMemoryRange memBar)
        {
            InitializeComponent();
            _memBar = memBar;
        }

        private void FormRange_Load(object sender, EventArgs e)
        {
            textEditStart.EditValue = _memBar.SelectedRange.Minimum;
            textEditEnd.EditValue   = _memBar.SelectedRange.Maximum;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            _memBar.SelectedRange.Minimum = int.Parse(textEditStart.Text);
            _memBar.SelectedRange.Maximum = int.Parse(textEditEnd.Text);
        }

        private void textEditRange_Validating(object sender, CancelEventArgs e)
        {
            var te = sender as TextEdit;
            var valOpt = te.Text.TryParseInt();
            if (valOpt == null)
            {
                e.Cancel = true;
                Global.Logger.Warn($"Invalid value : {te.Text}");
                return;
            }

            var val = valOpt.Value;
            if (te == textEditStart)
            {
                var max = textEditEnd.Text.TryParseInt().Value;
                if (!val.InClosedRange(0, max))
                {
                    e.Cancel = true;
                    Global.Logger.Warn($"Invalid value : {te.Text}");
                    return;
                }
            }
            else if (te == textEditEnd)
            {
                var min = textEditStart.Text.TryParseInt().Value;
                if (!val.InClosedRange(min, _memBar.Maximum))
                {
                    e.Cancel = true;
                    Global.Logger.Warn($"Invalid value : {te.Text}");
                    return;
                }
            }

            Console.WriteLine("");
        }
    }
}
