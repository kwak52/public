using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC;
using Dsu.PLC.Common;
using Dsu.PLC.Melsec;
using Dsu.UI.XbarGantt;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace ChartGantt_Form
{
    public partial class Form1 : Form
    {
        DataTable _dtInput = new DataTable();
        DataTable _dtLog = new DataTable();
        MxConnection _conn;
        ucChartTree CharTree;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _conn = new MxConnection(new MxConnectionParameters("192.168.8.10", Convert.ToUInt16(10002), TransportProtocol.Udp));
          //  _conn = new MxConnection(new MxConnectionParameters("192.168.0.99", Convert.ToUInt16(5001), TransportProtocol.Udp));

            LoadingSymbol();
            //LoadingSymbolforPLC();
            CreateLogTable();

            ucChartGantt1.ShowChart(_dtInput, _dtLog);

            ucChartTree1.ShowTree(_dtInput);
            ucChartTree1.UEventMouseDoubleClick += UcChartTree1_UEventMouseDoubleClick;
            ucChartTree1.UEventShowChanged += ucChartTree1_UEventShowChanged;

            OpenPLC();

            Timer t = new Timer();
            t.Interval = 500;
            t.Tick += T_Tick;
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if (toggleSwitch1.IsOn)
                this.Do(() =>
                {
                    // ucChartGantt1.ShowChart(_dtInput, _dtLog);
                    ucChartGantt1.BoundDataChanged(ucChartTree1._showMemory);
                });
        }

        private void CreateLogTable()
        {
            _dtLog.Columns.Add("Address");
            _dtLog.Columns.Add(new DataColumn("AddressWord", typeof(bool)));
            _dtLog.Columns.Add(new DataColumn("Value", typeof(int)));
            _dtLog.Columns.Add(new DataColumn("Time", typeof(DateTime)));
        }

        private void LoadingSymbol()
        {
            string directory = @"D:\solutions\app\Kefico\Dsa.Kefico.Cp\bin\Configure\HwConfig";
            string search = "CPTester - Copy.cnf";
            List<string> lstPath = Directory.EnumerateFiles(directory, search, SearchOption.TopDirectoryOnly).ToList();

            _dtInput.Columns.Add(new DataColumn("Id", typeof(int)));
            _dtInput.Columns.Add("Address");
            _dtInput.Columns.Add("Symbol");
            _dtInput.Columns.Add("Type");
            _dtInput.Columns.Add(new DataColumn("memoryId", typeof(Int64)));

            foreach (var file in lstPath)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                            break;

                        string[] arrData = line.Split(';');
                        if (arrData.Length == 4)     //PLC_READ;M7050;0:PLC_START;
                        {
                            if (arrData[0] == "PLC_READ")
                                _dtInput.Rows.Add(_dtInput.Rows.Count, arrData[1], string.Format("[{0}] {1}", arrData[1], arrData[2].Replace(":", "_")), "I", _dtInput.Rows.Count);
                            else
                                _dtInput.Rows.Add(_dtInput.Rows.Count , arrData[1], string.Format("[{0}] {1}", arrData[1], arrData[2].Replace(":", "_")), "Q", _dtInput.Rows.Count);
                        }
                    }
                }
            }
        }

        private void LoadingSymbolforPLC()
        {
            TagDataSource tags = new TagDataSource();

            _dtInput.Columns.Add(new DataColumn("Id", typeof(int)));
            _dtInput.Columns.Add("Address");
            _dtInput.Columns.Add("Symbol");
            _dtInput.Columns.Add("Type");
            _dtInput.Columns.Add(new DataColumn("memoryId", typeof(Int64)));

            foreach (var tag in tags.Items)
            {
                if (tag.EventType == EmEventType.DataDB)
                    _dtInput.Rows.Add(_dtInput.Rows.Count, tag.Address, tag.Name, "I", _dtInput.Rows.Count);
                else
                    _dtInput.Rows.Add(_dtInput.Rows.Count, tag.Address, tag.Name, "Q", _dtInput.Rows.Count);
            }
        }

        private void ucChartTree1_UEventShowChanged(List<long> ShowMemory)
        {
            this.Do(() =>
            {
                ucChartGantt1.BoundDataChanged(ShowMemory);
            });
        }

        private void UcChartTree1_UEventMouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Do(() =>
            {
                ucChartGantt1.ShowChart(_dtInput, _dtLog);
                ucChartGantt1.BoundDataChanged(ucChartTree1._showMemory);
            });
        }

        private async void OpenPLC()
        {
            if (_conn.Connect())
                _conn.CreateTags(GenerateTagNames());
            _conn.PerRequestDelay = 10;
            _conn.Subject.OfType<TagValueChangedEvent>().Subscribe(evt => EventTag((MxTag)evt.Tag));
            await _conn.StartDataExchangeLoopAsync();
            _conn.Disconnect();
        }

        private IEnumerable<string> GenerateTagNames()
        {
            foreach (DataRow dr in _dtInput.Rows)
                yield return dr["Address"].ToString();
        }

        private void EventTag(MxTag tagEvt)
        {
            this.Do(() =>
            {
                if (_dtLog.Rows.Count > 10000)
                {
                    for (int i = 0; i < 1000; i++)
                        _dtLog.Rows.RemoveAt(i);
                }

                UpdateChart(tagEvt.Name, Convert.ToInt32(tagEvt.Value), DateTime.Now);
                _dtLog.Rows.Add(tagEvt.Name, tagEvt.IsWordDevice, Convert.ToInt32(tagEvt.Value), DateTime.Now);
            });
        }

        private void UpdateChart(string Address, int value, DateTime dt)
        {
            DataRow TagRow = _dtInput.Select(string.Format("Address = '{0}'", Address)).FirstOrDefault();
            int TagID = (int)TagRow["Id"];
            string Symbol = (string)TagRow["Symbol"];
            string Type = (string)TagRow["Type"];

            DataRow SortedRow = _dtLog.Select(string.Format("Address = '{0}'", Address)).LastOrDefault();
           // DataRow SortedRow = _dtLog.Select(string.Format("Address = '{0}' and time < '{1}' ", Address, dt)).LastOrDefault();
            if (SortedRow != null)
            {
                if (value == 0 || (bool)SortedRow["AddressWord"])
                {
                    string Comment = string.Format("{0}-{1}", SortedRow["Value"], value);

                    BarPoint point = new BarPoint(TagID, Symbol, Type, TagID, (DateTime)SortedRow["Time"], dt, Comment);
                    ucChartGantt1._lstPoint.Add(point);
                }
            }
        }
    }
}
