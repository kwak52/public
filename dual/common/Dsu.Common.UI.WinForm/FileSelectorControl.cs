using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dsu.Common.Resources;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.UI.WinForm
{
    public delegate void FileSelectorAddingEventHandler(object sender, List<string> files);

    /// <summary>
    /// 다중 File 을 선택하기 위한 user control
    /// </summary>
    public partial class FileSelectorControl : UserControl
    {
        private class Record
        {
            public string FileName { get; set; }
            public bool IsSelected { get; set; }
        }
        public IEnumerable<string> Files { get { return _records.Select(r => r.FileName); } }
        private List<Record> _records = new List<Record>();

        public string InitialFolder { get; set; }
        public string Filter { get { return _filter; } set { _filter = value; } }
        private string _filter = "All files (*.*)|*.*";

        /// <summary>
        /// OpenFileDialog 를 통해 선택된 file 을 confirm 받기 위한 event handler
        /// 선택된 file 에 대해서 첨삭할 수 있다.
        /// </summary>
        public event FileSelectorAddingEventHandler FileAddingHook;
        public bool IsButtonsVisible
        {
            get { return _isButtonsVisible; } 
            set
            {
                _isButtonsVisible = value;
                toolStrip1.Visible = _isButtonsVisible;
            }
        }
        private bool _isButtonsVisible = true;

        public bool IsSelectioinColumnVisible
        {
            get { return _isSelectionColumnVisible; }
            set
            {
                _isButtonsVisible = value;
                gridColumnSelected.Visible = _isSelectionColumnVisible;
            }
        }

        private bool _isSelectionColumnVisible = false;

        /// <summary>
        /// 사용자가 선택한 file 이 폴더일 경우, 폴더 아래의 모든 파일 항목(recursive)을 선택할 지 여부
        /// </summary>
        public bool IsExpandFolder { get; set; }

        public FileSelectorControl()
        {
            InitializeComponent();

            toolStripButtonAdd.Image = Images.ActionAdd;
            toolStripButtonClearAll.Image = Images.Clear;
            gridControl1.DataSource = _records;
        }

        public void AddFilter(string filter) { Filter = filter + "|" + Filter; }

        public void ClearAll() { _records.Clear(); }

        private void gridControl1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void AddFiles(IEnumerable<string> selectedFiles)
        {
            List<string> files = selectedFiles.ToList();
            if (FileAddingHook != null)
                FileAddingHook(this, files);

            _records.AddRange(files.Select(f => new Record() { FileName = f, IsSelected = true }));
            gridView1.LayoutChanged();
        }


        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            AddFiles(files);
        }

        private void toolStripButtonClearAll_Click(object sender, System.EventArgs e)
        {
            ClearAll();
        }

        private void toolStripButtonAdd_Click(object sender, System.EventArgs e)
        {
            openFileDialog1.InitialDirectory = InitialFolder.IsNullOrEmpty() ? Application.StartupPath : InitialFolder;
            openFileDialog1.Filter = Filter;
            if (DialogResult.OK == openFileDialog1.ShowDialog())
                AddFiles(openFileDialog1.FileNames);

        }
    }
}
