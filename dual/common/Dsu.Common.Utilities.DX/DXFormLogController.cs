using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.DX
{
    [Obfuscation(Exclude = true, Feature = "renaming")]   // TIPS : dotfucscator
    public partial class DXFormLogController : Form
    {
        private class Record
        {
            [Browsable(false)]
            public static DXFormLogController ParentForm { get; set; }
            private LogProxy _logProxy;

            public Record(LogProxy logProxy)
            {
                _logProxy = logProxy;
            }

            public bool IsEnabled
            {
                get
                {
                    switch (ParentForm.Level)
                    {
                        case LevelType.Debug: return _logProxy.IsEnableDebug;
                        case LevelType.Warn: return _logProxy.IsEnableWarn;
                        case LevelType.Info: return _logProxy.IsEnableInfo;
                        default:
                            throw new InvalidOperationException("Unknown level " + ParentForm.Level);
                    }                    
                }
                set
                {
                    switch (ParentForm.Level)
                    {
                        case LevelType.Debug: _logProxy.IsEnableDebug = value; break;
                        case LevelType.Warn: _logProxy.IsEnableWarn = value; break;
                        case LevelType.Info: _logProxy.IsEnableInfo = value; break;
                        default:
                            throw new InvalidOperationException("Unknown level " + ParentForm.Level);
                    }                                        
                }
            }

            public string Name { get { return _logProxy.Name; } }
        }


        private List<Record> _records = new List<Record>();          // wrapped logger record
        private List<LogProxy> _loggers;        // original logger data

        internal enum LevelType { Debug, Warn, Info }
        internal LevelType Level { get {  return _level; } }
        private LevelType _level = LevelType.Debug;

        public DXFormLogController(IEnumerable<LogProxy> loggers)
        {
            InitializeComponent();
            _loggers = loggers.ToList();

            gridControl1.DataSource = _records;
            RedrawUI();

            if ( Record.ParentForm != null )
                throw new UnexpectedCaseOccurredException("Already exists log control form.   Abroted.");

            Record.ParentForm = this;
            Closed += (sender, args) => { Record.ParentForm = null; };
        }

        private void DXFormLogController_Load(object sender, EventArgs e)
        {
            enumEditorLevel.BorderStyle = BorderStyle.None;           
            enumEditorLevel.EnumType = typeof (LevelType);
            enumEditorLevel.EnumValue = (long) _level;
        }

        private void RedrawUI()
        {
            _records.Clear();

            List<LogProxy> filteredLoggers = new List<LogProxy>();
            foreach( var logProxy in _loggers)
            {
#if !DEBUG && SCRIPT
                /*
                 * Release version 에서의 debug logging control 은, 사용자 script 에 한해서 제어할 수 있도록 한다.
                 * 나머지는 release version 에서 debug logging 은 자동으로 disable 된다.
                 */
                if (_level == LevelType.Debug && !logProxy.Type.GetCustomAttributes(false).Cast<Attribute>().OfType<ScriptClass>().Any())
                    continue;
#endif
				filteredLoggers.Add(logProxy);                
            }

            _records.AddRange(filteredLoggers.Select(l => new Record(l)));
            
            gridView1.LayoutChanged();
        }


        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            _records.ForEach(r => r.IsEnabled = true);
            gridView1.LayoutChanged();
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            _records.ForEach(r => r.IsEnabled = false);
            gridView1.LayoutChanged();
        }

        private void enumEditorLevel_Change(object sender, EventArgs e)
        {
            _level = (LevelType)enumEditorLevel.EnumValue;
            RedrawUI();
        }
    }
}
