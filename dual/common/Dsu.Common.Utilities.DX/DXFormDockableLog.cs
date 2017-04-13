using System.Collections.Generic;
using System.Runtime.InteropServices;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using log4net.Core;

namespace Dsu.Common.Utilities.DX
{
    /// <summary>
    /// Log 출력을 위한 docking window
    /// SharpStudio 가 아닌 일반 applicatino 용
    /// <para/> - Log4Net
    /// <para/> - ListView : virtual mode : http://www.codeproject.com/Articles/42229/Virtual-Mode-ListView
    /// </summary>
    [Guid("BD757DAA-08E1-4564-8341-1AD5481B6608")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [ProgId("Dsu.Common.Utilities.DX.DXFormDockableLog")]
    public partial class DXFormDockableLog : DXFormDockableLogBase
    {
        protected override GridControl GridControl { get { return gridControl1; }}
        protected override GridView GridView { get { return gridView1; } }

        private List<LogRecord> _bindings = new List<LogRecord>();

        protected override void ClearBindingRecordEntries() { _bindings.Clear(); }
        protected override object GetBindingRecords() { return _bindings; }


        public DXFormDockableLog()
        {
            InitializeComponent();
            base.Construct();
        }


        protected override object CollectCustomLogRecords() { return base.CollectLogRecords(); }
        protected override object ParseLogLine(string line) { throw new System.NotImplementedException(); } 
        protected override int CountLogRows() { return _bindings.Count; }
        protected override void RemoveFirstLogRows(int numRemoval) { _bindings.RemoveRange(0, numRemoval); }
        protected override void AppendLogRow(LoggingEvent logEntry) { _bindings.Add(new LogRecord(logEntry)); } 
    }
}
