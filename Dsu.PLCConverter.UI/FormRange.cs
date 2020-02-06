using DevExpress.XtraEditors;
using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Dsu.PLCConverter.UI
{
    /// <summary>
    /// Range 결정 UI.  수치로 정확히 입력.  Target 측(Xg5k)의 memory 영역 지정에 사용
    /// </summary>
    public partial class FormRange : Form
    {
        UcMemoryRange _memBar;
        Nullable<int> _length = null;

        
        public FormRange(UcMemoryRange memBar, Nullable<int> length)
        {
            InitializeComponent();
            _memBar = memBar;
            _length = length;
        }
        public FormRange(UcMemoryRange memBar)
            : this(memBar, null)
        { }

        private void FormRange_Load(object sender, EventArgs args)
        {
            var min = (int)_memBar.SelectedRange.Minimum;
            textEditStart.EditValue = min;
            textEditEnd.EditValue   = _memBar.SelectedRange.Maximum;
            layoutControlItemLength.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            if (_length.HasValue)
            {
                var len = _length.Value;
                layoutControlItemLength.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                textEditLength.EditValue = len;
                textEditEnd.EditValue = min + len;

                // 주어진 길이는 source 측에서 결정된 길이를 따름.
                // Start 의 값을 변경할 때, 주어진 길이를 유지하도록 End 값을 자동수정.
                // End 값 수정시 Start 값 수정, also
                int counter = 0;
                textEditStart.EditValueChanging += (s, e) =>
                {
                    try
                    {
                        if (Interlocked.Increment(ref counter) == 1)
                        {
                            var val = e.NewValue.ToString().TryParseInt();
                            if (val.HasValue)
                                textEditEnd.EditValue = val.Value + len;
                        }
                    }
                    finally
                    {
                        Interlocked.Decrement(ref counter);
                    }
                };
                textEditEnd.EditValueChanging += (s, e) =>
                {
                    try
                    {
                        if (Interlocked.Increment(ref counter) == 1)
                        {
                            var val = e.NewValue.ToString().TryParseInt();
                            if (val.HasValue)
                                textEditStart.EditValue = val.Value - len;
                        }
                    }
                    finally
                    {
                        Interlocked.Decrement(ref counter);
                    }
                };
            }
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
