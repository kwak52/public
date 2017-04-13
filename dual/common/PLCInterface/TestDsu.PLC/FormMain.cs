using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using Dsu.PLC.AB;
using Dsu.PLC.Common;
using Dsu.PLC.Fuji;
using Dsu.PLC.Melsec;
using Dsu.PLC.Siemens;

namespace TestDsu.PLC
{
    public enum PlcType
    {
        Siemens, Melsec, Fuji, Logix,
    }
    public partial class FormMain : Form
    {
        private S7Connection _connectionS7;
        private MxConnection _connectionMx;
        private AbConnection _connectionAb;
        private FjConnection _connectionFj;

        private ConnectionBase Connection
        {
            get
            {
                switch ((PlcType)enumEditorPLCType.EnumValue)
                {
                    case PlcType.Melsec: return _connectionMx;
                    case PlcType.Fuji: return _connectionFj;
                    case PlcType.Logix: return _connectionAb;
                    case PlcType.Siemens: return _connectionS7;
                    default:
                        throw new Exception("Unknown PLC type.");
                }
            }
        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            enumEditorPLCType.EnumType = typeof(PlcType);
            enumEditorPLCType.EnumValue = (long)PlcType.Melsec;
        }

        private IEnumerable<string> GenerateTagNames()
        {
            yield return "X1";
            yield return "TN0";
            yield return "TN1";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            switch ((PlcType)enumEditorPLCType.EnumValue)
            {
                case PlcType.Melsec:
                    _connectionMx = new MxConnection(new MxConnectionParameters("192.168.0.102"));
                    _connectionMx.Connect();
                    break;
                case PlcType.Fuji:
                    _connectionFj = new FjConnection(new FjConnectionParameters("../ioarea-sample.ini", "192.168.0.103"));
                    _connectionFj.Connect();
                    break;
                case PlcType.Logix:
                    _connectionAb = new AbConnection(new AbConnectionParameters("192.168.0.104"));
                    _connectionAb.Connect();
                    break;
                case PlcType.Siemens:
                    _connectionS7 = new S7Connection(new S7ConnectionParameters("192.168.0.101"));
                    _connectionS7.Connect();
                    break;
            }
        }

        private IDisposable _subscription;
        private async void btnMonitor_Click(object sender, EventArgs e)
        {
            Connection.CreateTags(GenerateTagNames()).Count();

            _subscription = Connection.Subject
                .OfType<TagValueChangedEvent>()
                .Subscribe(evt =>
            {
                var tag = (MxTag) evt.Tag;
                Trace.WriteLine($"{tag} value changed {tag.OldValue} => {tag.Value}");
            });

            await Connection.StartDataExchangeLoopAsync();
        }

        private void action1_Update(object sender, EventArgs e)
        {
//             btnConnect.Enabled = Connection == null;
//             btnDisconnect.Enabled = Connection != null;
//             btnMonitor.Enabled = Connection != null;
        }

        private void btnStopMonitor_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("Stopping monitor...");
            _subscription.Dispose();
            _subscription = null;
            Connection.StopDataExchangeLoop();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            switch ((PlcType)enumEditorPLCType.EnumValue)
            {
                case PlcType.Melsec:
                    _connectionMx.Disconnect();
                    break;
                case PlcType.Fuji:
                    _connectionFj.Disconnect();
                    break;
                case PlcType.Logix:
                    _connectionAb.Disconnect();
                    break;
                case PlcType.Siemens:
                    _connectionS7.Disconnect();
                    break;
            }
        }
    }
}
