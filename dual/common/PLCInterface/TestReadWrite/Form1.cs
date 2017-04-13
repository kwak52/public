using Dsu.PLC;
using Dsu.PLC.Common;
using Dsu.PLC.Melsec;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reactive.Linq;
using System.Windows.Forms;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Threading;

namespace TestPLC
{
    public partial class Form1 : Form
    {
        MxConnection _connSET;
        MxConnection _conn;
        DataTable dtSet;
        DataTable dtGet;
        private int PLCDelay = 100;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            _conn = new MxConnection(new MxConnectionParameters("192.168.8.10", Convert.ToUInt16(10000), TransportProtocol.Udp));
          //  _connSET = new MxConnection(new MxConnectionParameters("192.168.8.10", Convert.ToUInt16(10001), TransportProtocol.Tcp));

            dtSet = new DataTable();
            dtSet.Columns.Add(new DataColumn("Address", typeof(string)));
            dtSet.Columns.Add(new DataColumn("Type", typeof(string)));
            dtSet.Columns.Add(new DataColumn("Data", typeof(int)));
            dtSet.Columns.Add(new DataColumn("Count", typeof(int)));

            for (int i = 7000; i < 7999; i++)
                dtSet.Rows.Add(string.Format("M{0}", i), "BIT", 1, 0);

            for (int i = 7000; i < 7999; i++)
                dtSet.Rows.Add(string.Format("D{0}", i), "WORD", 1, 0);

            ucGrid_Set.DataSource = dtSet;

            dtGet = new DataTable();
            dtGet.Columns.Add(new DataColumn("Address", typeof(string)));
            dtGet.Columns.Add(new DataColumn("Type", typeof(string)));
            dtGet.Columns.Add(new DataColumn("Data", typeof(int)));
            dtGet.Columns.Add(new DataColumn("Count", typeof(int)));

            for (int i = 7000; i < 7999; i++)
                dtGet.Rows.Add(string.Format("M{0}", i), "BIT", 0, 0);

            for (int i = 7000; i < 7999; i++)
                 dtGet.Rows.Add(string.Format("D{0}", i), "WORD", 0, 0);

            ucGrid_Get.DataSource = dtGet;

            OpenPLC();

        }

        private IDisposable _subscription;
        private async void OpenPLC()
        {
            _conn.PerRequestDelay = PLCDelay;
          //  _connSET.Connect();
            if (_conn.Connect())
                _conn.CreateTags(GenerateTagNames());

            _conn.Subject.OfType<TagValueChangedEvent>().Subscribe(evt => EventTag((MxTag)evt.Tag));
            await _conn.StartDataExchangeLoopAsync();
            _conn.Disconnect();
        }


        private void EventTag(MxTag tagEvt)
        {
            this.Do(() =>
            {
                Console.WriteLine(tagEvt.Name + " : " + tagEvt.Value);
                DataRow[] dr = dtGet.Select(string.Format("Address = '{0}'", tagEvt.Name));
                foreach (var ab in dr)
                {
                    ab["data"] = tagEvt.Value;
                }
            });
        }

        private IEnumerable<string> GenerateTagNames()
        {
            foreach (DataRow dr in dtSet.Rows)
                yield return dr["Address"].ToString();
            foreach (DataRow dr in dtGet.Rows)
                yield return dr["Address"].ToString();
        }

        private void ucGrid_Set_UEventMouseDoubleClick(object sender, int RowHandle)
        {
            DataRowView dr = (DataRowView)ucGrid_Set.GridView.GetRow(RowHandle);
            if (dr == null)
                return;

            //if (dr == null)
            //{
            //    for (int i = 30000; i < 30100; i++)
            //    {
            //        _connSET.McProtocol.SetDevice(string.Format("D{0}", i), 777);
            //        Thread.Sleep(10);
            //    }
            //    return;
            //}

            int[] arrData = new int[1];
            arrData[0] = Convert.ToInt32(dr["data"]);

            if (dr["type"].ToString() == "BIT")
                _connSET.McProtocol.SetBitDevice(dr["Address"].ToString(), 1, arrData);
            else
                _connSET.McProtocol.SetDevice(dr["Address"].ToString(), Convert.ToInt32(dr["data"]));
        }
    }
}
