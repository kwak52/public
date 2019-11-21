using System;
using System.Windows.Forms;

namespace PLCConvertor.Forms
{
    /// <summary>
    /// 옴론 CXT 의 심볼 중 UI 상에는 존재하나, 
    /// CXT text file 에는 raw text 로 나타나지 않는 심볼을 처리하기 위해서
    /// UI 상에서 심볼 테이블 내용을 복사해서 붙여 넣는 방식을 지원하기 위한 form
    /// </summary>
    public partial class FormSymbolPaste : Form
    {
        public string SymbolTableText { get; private set; }
        public FormSymbolPaste()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SymbolTableText = textBoxSymbols.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
