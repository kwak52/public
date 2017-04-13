using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using log4net.Core;
using Dsu.Common.Interfaces;
using Dsu.Common.Resources;
using Dsu.Common.Utilities.Designer;
using Dsu.Common.Utilities.Exceptions;
using Dsu.Common.Utilities.ExtensionMethods;
using WeifenLuo.WinFormsUI.Docking;
using Images = Dsu.Common.Resources.Images;


namespace Dsu.Common.Utilities.DX
{
    /// <summary>
    /// Log 출력을 위한 docking window
    /// Subclasses : FormDockableLogViewS3, FormDockableLogViewSimulator, FormDockableLogViewController, DXFormDockableLog
    /// </summary>
    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<DXFormDockableLogBase, FormDockableBase>))]
    public abstract class DXFormDockableLogBase
        : FormDockableBase
		, ILogWindow
    {
        protected static DXGridViewOptionHolder _optionHolder;
        protected DateTime _lastRequestTime;

        protected abstract void ClearBindingRecordEntries();
        protected abstract object GetBindingRecords();

        protected virtual int GetMaxNumLogEntries() { return 1000; }

        private static bool _autoRollToEnd = true;
        protected static bool AutoRollToEnd { get { return _autoRollToEnd; } set { _autoRollToEnd = value; } }

        protected abstract GridView GridView { get; }
        protected abstract DevExpress.XtraGrid.GridControl GridControl { get; }

        protected readonly object _bindingsLock = new object();

        protected Dictionary<Level, Color> _levelColors = new Dictionary<Level, Color>()
        {
            {Level.Debug, Color.Gray},
            {Level.Error, Color.Red},
            {Level.Fatal, Color.Red},
            {Level.Info, Color.Black},
            {Level.Warn, Color.OrangeRed},
        };

        protected LogEntryManager _logManager;


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            LoggableApplication.LogWindow = null;
        }

        protected IEnumerable<LogRecord> CollectLogRecords()
        {
            for (int i = 0; i < _logManager.Count; i++)
            {
                LoggingEvent log = _logManager[i];
                yield return new LogRecord(log);
            }
        }

        protected abstract object CollectCustomLogRecords();
        protected abstract object ParseLogLine(string line);

        /// <summary> Context 메뉴 생성 및 표시.  http://stackoverflow.com/questions/6623672/how-to-put-an-icon-in-a-menuitem </summary>
        protected virtual void AppendContextMenu()
        {
            var items = GridControl.ContextMenuStrip.Items;

            items.Add(new ToolStripSeparator());
            items.Add(new ToolStripMenuItem("Clear", Images.Clear, (o, a) =>
            {
                GridView.TopRowIndex = 0;
                ClearBindingRecordEntries();
                _logManager.Clear();
                GridView.LayoutChanged();
            }));

            items.Add(new ToolStripMenuItem("Copy", Images.Copy, (o, a) =>
            {
                GridView.GetSelectedRows().ForEach(n =>
                {
                    var rec = (LogRecord)GridView.GetRow(n);
                    Clipboard.SetText(rec.ToString());
                });
            }));
            
            items.Add(new ToolStripMenuItem("Copy All", Images.Copy, (o, a) =>
            {
                var sb = new StringBuilder();
                CollectLogRecords().ForEach(r =>
                {
                    sb.Append(r.ToString() + "\r\n");
                });
                Clipboard.SetText(sb.ToString());                    
            }));

            items.Add(new ToolStripSeparator());
            items.Add(new ToolStripMenuItem("Open in new window", Images.NewWindow, (o, a) =>
            {
                new DXFormGridView(dataSource: CollectCustomLogRecords()).Show();
            }));

            items.Add(new ToolStripMenuItem("Open log file in new window...", Images.Log, (o, a) =>
            {
                using (var openFileDialog = new OpenFileDialog()
                {
                    Filter = "Log files(*.log)|*.log|All files(*.*)|*.*",
                    InitialDirectory = CommonApplication.GetProfilePath(),
                })
                {
                    if (DialogResult.OK == openFileDialog.ShowDialog(this))
                    {
                        var file = openFileDialog.FileName;

                        try
                        {
                            var records = File.ReadAllText(file)
                                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(line => ParseLogLine(line));

                            new DXFormGridView(dataSource: records) { Text = file }.Show();
                        }
                        catch (Exception ex)
                        {
                            CommonApplication.ShowError(String.Format("Failed to open {0}\r\n{1}", file, ex.Message));
                        }
                    }
                }

            }));


        }

        public override void Initialize(IApplication application, DockPanel dockpanel)
        {
            base.Initialize(application, dockpanel);
            Construct();

            GridView.EnableDefaultContextMenu(
                EmDXGridControl.DXGridViewContextMenuFlag.AutoFilterRow
                | EmDXGridControl.DXGridViewContextMenuFlag.GroupPanel
                | EmDXGridControl.DXGridViewContextMenuFlag.EnableShowAllColumns
                | EmDXGridControl.DXGridViewContextMenuFlag.BestFitColumn);
            AppendContextMenu();

            Icon = Images.Log.ToIcon();

            LoggableApplication.LogWindow = this;
            _logManager = LoggableApplication.LogEntryManager;
        }

        protected void Construct()
        {
            ContextHelp.SetHelp(this, "log-view.html");
            GridControl.DataSource = GetBindingRecords();

            GridView.BestFitMaxRowCount = 100;
            GridView.RowStyle += gridView1_RowStyle;
            GridView.OptionsBehavior.Editable = false;
            _lastRequestTime = DateTime.Now;


            /* Gridview 보기 설정 저장 */
            if (_optionHolder == null)
            {
                var registryLocation = CommonApplication.TheCommonApplication.RegistryLocation + @"\Configuration\ViewSetting";
                _optionHolder = new DXGridViewOptionHolder(this, GridView, registryLocation);
                Closing += (sender, args) =>
                {
                    _optionHolder.ToRegistry();
                };

                /* COM 호출을 경유해서 들어올 때, Form.Closing() 이 제대로 호출되지 않는 듯함. */
                AddSubscription(CommonApplication.TheCommonApplication.ApplicationSubject
                    .OfType<ObservableApplicationEvent>()
                    .Where(evt => evt.Type == ApplicationEventType.Closing)
                    .Subscribe(evt =>
                    {
                        _optionHolder.ToRegistry();
                    }));
            }
            else
                _optionHolder.SetGridView(this, GridView);            
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle < 0 || GridView.RowCount == 0)
                return;

            try
            {
                if (e.RowHandle == GridView.RowCount - 1)
                    e.Appearance.ForeColor = Color.DodgerBlue;
                else
                {
                    var record = (LogRecord)GridView.GetRow(e.RowHandle);
                    if ( record != null )
                        e.Appearance.ForeColor = _levelColors[record.Level];
                }
            }
            catch (Exception ex) { ExceptionHider.SwallowException(ex); }
        }


        /// <summary>
        /// see S3Application.AddLogEntry()
        /// </summary>
        /// <param name="size">size of virtual list view</param>
        public void Display(int size)
        {
            throw new NotImplementedException();
        }


        protected abstract int CountLogRows();      // _bindings.Count;
        protected abstract void RemoveFirstLogRows(int numRemoval);     // _bindings.RemoveRange(0, numRemoved);
        protected abstract void AppendLogRow(LoggingEvent logEntry);    // _bindings.Add(new LogRecordSim(logEntry));

        protected int _rowAdjustCount = 0;
        protected virtual void DelayedUpdateUI(bool autoRollEnd, int topRowAdjustCount)
        {
            this.DoAsync(() =>
            {
                try
                {
                    _rowAdjustCount += topRowAdjustCount;
                    var delay = CommonApplication.TheCommonConfiguration.UIUpdateDelayInMilliseconds;
                    SynchronizationContext.Current.Post(TimeSpan.FromMilliseconds(delay), () =>
                    {
                        TimeSpan diff = DateTime.Now - _lastRequestTime;
                        if (diff.Milliseconds >= delay)
                        {
                            if (autoRollEnd)
                            {
                                GridView.TopRowIndex = GridView.RowCount - 1;
                                GridView.LayoutChanged();
                            }
                            else
                                GridView.TopRowIndex = GridView.TopRowIndex + _rowAdjustCount;

                            _rowAdjustCount = 0;

                            _lastRequestTime = DateTime.Now;
                        }
                    });
                }
                catch (Exception ex) { ExceptionHider.SwallowException(ex); }
            });

        }

        //public abstract void AppendLog(LoggingEvent logEntry, int size);
        public virtual void AppendLog(LoggingEvent logEntry, int size)
        {
            int numRemoved = 0;
            int numBindings = 0;
            var max = GetMaxNumLogEntries();

            lock (_bindingsLock)
            {
                // 성능을 위해서 20% 초과하면, max size 로 reset
                var Max = (int)(max * 1.2);
                numBindings = CountLogRows();
                if (numBindings > Max)
                {
                    numRemoved = numBindings - (int)(max * 0.2);
                    RemoveFirstLogRows(numRemoved);
                    numBindings -= numRemoved;
                }

                AppendLogRow(logEntry);
            }

            int rowAdjust = numBindings < max ? 0 : -numRemoved - 1;
            DelayedUpdateUI(AutoRollToEnd, rowAdjust);
        }
    }
}

