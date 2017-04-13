using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
/*-*/    [Obsolete("Use DXFormLogController instead.")]
/*-*/    [System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]   // TIPS : dotfucscator
/*-*/    public partial class FormLoggerController : Form
/*-*/    {
/*-*/        private List<LogProxy> _loggers;
/*-*/
/*-*/        private enum LevelType { Debug, Warn, Info }
/*-*/
/*-*/        private LevelType _level = LevelType.Debug;
/*-*/
/*-*/        public FormLoggerController(IEnumerable<LogProxy> loggers)
/*-*/        {
/*-*/            InitializeComponent();
/*-*/            _loggers = loggers.ToList();
/*-*/        }
/*-*/
/*-*/        private void FormLoggerController_Load(object sender, EventArgs e)
/*-*/        {
/*-*/            enumEditorLevel.BorderStyle = BorderStyle.None;           
/*-*/            enumEditorLevel.EnumType = typeof (LevelType);
/*-*/            enumEditorLevel.EnumValue = (long) _level;
/*-*/
/*-*/            RedrawListView();
/*-*/        }
/*-*/
/*-*/        private void RedrawListView()
/*-*/        {
/*-*/            listView1.Items.Clear();
/*-*/            foreach (var logProxy in _loggers)
/*-*/            {
#if ! DEBUG && SCRIPT
                /*
                 * Release version 에서의 debug logging control 은, 사용자 script 에 한해서 제어할 수 있도록 한다.
                 * 나머지는 release version 에서 debug logging 은 자동으로 disable 된다.
                 */
                if ( _level == LevelType.Debug && ! logProxy.Type.GetCustomAttributes(false).Cast<Attribute>().OfType<ScriptClass>().Any())
                    continue;
#endif
/*-*/                var lvi = new ListViewItem(logProxy.Name) { Tag = logProxy };
/*-*/                listView1.Items.Add(lvi);
/*-*/                switch (_level)
/*-*/                {    
/*-*/                    case LevelType.Debug: lvi.Checked = logProxy.IsEnableDebug; break;
/*-*/                    case LevelType.Warn: lvi.Checked = logProxy.IsEnableWarn; break;
/*-*/                    case LevelType.Info: lvi.Checked = logProxy.IsEnableInfo; break;
/*-*/                    default:
/*-*/                        throw new InvalidOperationException("Unknown level " + _level);
/*-*/                }
/*-*/            }            
/*-*/        }
/*-*/
            /// <summary>
            /// listview 의 check item 처리는 ItemChecked 가 아닌 ItemCheck 에서 처리해야 한다.
            /// ItemChecked 는 item check 상태가 이미 변한 후에 호출된다.
            /// 
            /// http://stackoverflow.com/questions/7506588/why-does-listview-fire-event-indicating-that-checked-item-is-unchecked-after-add
            /// </summary>
/*-*/        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
/*-*/        {
/*-*/            Contract.Requires(e.NewValue.IsOneOf(CheckState.Checked, CheckState.Unchecked));
/*-*/            var proxy = (LogProxy) listView1.Items[e.Index].Tag;
/*-*/            bool check = e.NewValue == CheckState.Checked;
/*-*/            switch (_level)
/*-*/            {
/*-*/                case LevelType.Debug: proxy.IsEnableDebug = check; break;
/*-*/                case LevelType.Warn: proxy.IsEnableWarn = check; break;
/*-*/                case LevelType.Info: proxy.IsEnableInfo = check; break;
/*-*/                default:
/*-*/                    throw new InvalidOperationException("Unknown level " + _level);
/*-*/            }
/*-*/        }
/*-*/
/*-*/        private void btnCheckAll_Click(object sender, EventArgs e)
/*-*/        {
/*-*/            foreach (var item in listView1.Items)
/*-*/                ((ListViewItem) item).Checked = true;
/*-*/        }
/*-*/
/*-*/        private void btnUncheckAll_Click(object sender, EventArgs e)
/*-*/        {
/*-*/            foreach (var item in listView1.Items)
/*-*/                ((ListViewItem)item).Checked = false;
/*-*/        }
/*-*/
/*-*/        private void enumEditorLevel_Change(object sender, EventArgs e)
/*-*/        {
/*-*/            _level = (LevelType)enumEditorLevel.EnumValue;
/*-*/            RedrawListView();
/*-*/        }
/*-*/    }
}
