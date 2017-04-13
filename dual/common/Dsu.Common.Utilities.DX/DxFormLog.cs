using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using Dsu.Common.Interfaces;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.DX;
using Dsu.Common.Utilities.Exceptions;
using Dsu.Common.Utilities.ExtensionMethods;
using log4net.Core;
using Images=Dsu.Common.Resources.Images;

namespace Dsu.Common.Utilities.DX
{
	/// <summary>
	/// Log4Net 을 이용한 DevXpress log form.
	/// WeifenLuo.WinFormsUI.Docking 을 이용한 doxkable form 은 DXFormDockableLog 참고.
	/// </summary>
	public partial class DxFormLog
		: DevExpress.XtraEditors.XtraForm
		, ILogWindow
	{
		protected IApplication _application;
		protected ILoggable LoggableApplication { get { return _application as ILoggable; } }

		private List<LogRecord> _bindings = new List<LogRecord>();
		protected DateTime _lastRequestTime;
		private static bool _autoRollToEnd = true;
		protected readonly object _bindingsLock = new object();
		protected int MaxNumLogEntries { get; } = 1000;
		private int UIUpdateDelayInMilliseconds { get; } = 300;

		protected Dictionary<Level, Color> _levelColors = new Dictionary<Level, Color>()
		{
			{Level.Debug, Color.Gray},
			{Level.Error, Color.Red},
			{Level.Fatal, Color.Red},
			{Level.Info, Color.Black},
			{Level.Warn, Color.OrangeRed},
		};

		protected LogEntryManager _logManager;

		protected void ClearBindingRecordEntries() { _bindings.Clear(); }
		protected object GetBindingRecords() { return _bindings; }

		public DxFormLog()
		{
			InitializeComponent();
			Construct();
		}

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

		/// <summary> Context 메뉴 생성 및 표시.  http://stackoverflow.com/questions/6623672/how-to-put-an-icon-in-a-menuitem </summary>
		protected virtual void AppendContextMenu()
		{
			var items = gridControl1.ContextMenuStrip.Items;

			items.Add(new ToolStripSeparator());
			items.Add(new ToolStripMenuItem("Clear", Images.Clear, (o, a) =>
			{
				gridView1.TopRowIndex = 0;
				ClearBindingRecordEntries();
				_logManager.Clear();
				gridView1.LayoutChanged();
			}));

			items.Add(new ToolStripMenuItem("Copy", Images.Copy, (o, a) =>
			{
				gridView1.GetSelectedRows().ForEach(n =>
				{
					var rec = (LogRecord)gridView1.GetRow(n);
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
				new DXFormGridView(dataSource: CollectLogRecords()).Show();
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

		protected object ParseLogLine(string line) { throw new System.NotImplementedException(); }
		protected void RemoveFirstLogRows(int numRemoval) { _bindings.RemoveRange(0, numRemoval); }
		protected void AppendLogRow(LoggingEvent logEntry) { _bindings.Add(new LogRecord(logEntry)); }


		public void Initialize(IApplication application/*, DockPanel dockpanel*/)
		{
			Construct();

			gridView1.EnableDefaultContextMenu(
				EmDXGridControl.DXGridViewContextMenuFlag.AutoFilterRow
				| EmDXGridControl.DXGridViewContextMenuFlag.GroupPanel
				| EmDXGridControl.DXGridViewContextMenuFlag.EnableShowAllColumns
				| EmDXGridControl.DXGridViewContextMenuFlag.BestFitColumn);
			AppendContextMenu();

			Icon = Images.Log.ToIcon();

			_application = application;
			LoggableApplication.LogWindow = this;
			_logManager = LoggableApplication.LogEntryManager;
		}

		protected void Construct()
		{
			//ContextHelp.SetHelp(this, "log-view.html");
			gridControl1.DataSource = GetBindingRecords();

			gridView1.BestFitMaxRowCount = 100;
			gridView1.RowStyle += gridView1_RowStyle;
			gridView1.OptionsBehavior.Editable = false;
			_lastRequestTime = DateTime.Now;


			///* Gridview 보기 설정 저장 */
			//if (_optionHolder == null)
			//{
			//	var registryLocation = CommonApplication.TheCommonApplication.RegistryLocation + @"\Configuration\ViewSetting";
			//	_optionHolder = new DXGridViewOptionHolder(this, gridView1, registryLocation);
			//	Closing += (sender, args) =>
			//	{
			//		_optionHolder.ToRegistry();
			//	};

			//	/* COM 호출을 경유해서 들어올 때, Form.Closing() 이 제대로 호출되지 않는 듯함. */
			//	AddSubscription(CommonApplication.TheCommonApplication.ApplicationSubject
			//		.OfType<ObservableApplicationEvent>()
			//		.Where(evt => evt.Type == ApplicationEventType.Closing)
			//		.Subscribe(evt =>
			//		{
			//			_optionHolder.ToRegistry();
			//		}));
			//}
			//else
			//	_optionHolder.SetGridView(this, gridView1);
		}

		private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
		{
			if (e.RowHandle < 0 || gridView1.RowCount == 0)
				return;

			try
			{
				if (e.RowHandle == gridView1.RowCount - 1)
					e.Appearance.ForeColor = Color.DodgerBlue;
				else
				{
					var record = (LogRecord)gridView1.GetRow(e.RowHandle);
					if (record != null)
						e.Appearance.ForeColor = _levelColors[record.Level];
				}
			}
			catch (Exception ex) { ExceptionHider.SwallowException(ex); }
		}

		protected int _rowAdjustCount = 0;
		protected virtual void DelayedUpdateUI(bool autoRollEnd, int topRowAdjustCount)
		{
			this.DoAsync(() =>
			{
				try
				{
					_rowAdjustCount += topRowAdjustCount;
					//var delay = CommonApplication.TheCommonConfiguration.UIUpdateDelayInMilliseconds;
					var delay = UIUpdateDelayInMilliseconds;
					SynchronizationContext.Current.Post(TimeSpan.FromMilliseconds(delay), () =>
					{
						TimeSpan diff = DateTime.Now - _lastRequestTime;
						if (diff.Milliseconds >= delay)
						{
							if (autoRollEnd)
							{
								gridView1.TopRowIndex = gridView1.RowCount - 1;
								gridView1.LayoutChanged();
							}
							else
								gridView1.TopRowIndex = gridView1.TopRowIndex + _rowAdjustCount;

							_rowAdjustCount = 0;

							_lastRequestTime = DateTime.Now;
						}
					});
				}
				catch (Exception ex) { ExceptionHider.SwallowException(ex); }
			});

		}

		public void AppendLog(LoggingEvent logEntry, int size)
		{
			int numRemoved = 0;
			int numBindings = 0;
			var max = MaxNumLogEntries;

			lock (_bindingsLock)
			{
				// 성능을 위해서 20% 초과하면, max size 로 reset
				var Max = (int)(max * 1.2);
				numBindings = _bindings.Count;
				if (numBindings > Max)
				{
					numRemoved = numBindings - (int)(max * 0.2);
					RemoveFirstLogRows(numRemoved);
					numBindings -= numRemoved;
				}

				AppendLogRow(logEntry);
			}

			int rowAdjust = numBindings < max ? 0 : -numRemoved - 1;
			DelayedUpdateUI(_autoRollToEnd, rowAdjust);
		}


		public void Display(int size)
		{
			throw new NotImplementedException();
		}

	}
}