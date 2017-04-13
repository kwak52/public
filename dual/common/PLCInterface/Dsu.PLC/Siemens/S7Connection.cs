using System;
using System.Collections.Generic;
using System.Linq;
using DotNetSiemensPLCToolBoxLibrary.Communication;
using Dsu.PLC.Common;


namespace Dsu.PLC.Siemens
{
    public class S7Connection : ConnectionBase
    {
        private PLCConnection _connection;
        internal PLCConnection PLCConnection => _connection;
        internal IEnumerable<PLCTag> PLCTags => Tags.Values.Cast<S7Tag>().Select(t => t.PlcTag);


        private S7ConnectionParameters _connectionParametersSiemens = null;

        public override IConnectionParameters ConnectionParameters
        {
            get { return _connectionParametersSiemens; }
            set { _connectionParametersSiemens = (S7ConnectionParameters) value; }
        }

        private S7Cpu _cpu;

        public override ICpu Cpu
        {
            get { return _cpu; }
        }



        public S7Connection(S7ConnectionParameters parameters)
            : base(parameters)
        {
            _connectionParametersSiemens = parameters;
            _connection = new PLCConnection(parameters.Ip, parameters.Ip);
        }

        public bool IsConnected { get; private set; }

        public override bool Connect()
        {
            if (IsConnected)
                return true;

            try
            {
                _connection.Connect();
                IsConnected = true;
                _cpu = new S7Cpu(_connection);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (_connection != null)
            {
                _connection.Disconnect();
                _connection = null;
            }

            base.Dispose(disposing);
        }


        public override TagBase CreateTag(string name) => new S7Tag(this, name);

        public override void InvalidateMonitoringTargets()
        {
            //throw new NotImplementedException();
        }


        internal override IEnumerable<ChannelRequestExecutor> Channelize(IEnumerable<TagBase> tags)
        {
            var channel = new S7ChannelRequestExecutor(this, tags);
            yield return channel;
        }

        public override object ReadATag(ITag tag) => null;
    }
}
