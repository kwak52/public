using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using ControlLogixNET;
using Dsu.PLC.Common;


namespace Dsu.PLC.AB
{
    public class AbConnection : ConnectionBase
    {
        public LogixProcessor LogixProcessor { get; set; }

        private AbConnectionParameters _connectionParametersAB = null;
        public override IConnectionParameters ConnectionParameters
        {
            get { return _connectionParametersAB; }
            set { _connectionParametersAB = (AbConnectionParameters)value; }
        }

        private AbCpu _cpu;
        public override ICpu Cpu { get { return _cpu; } }

        public AbConnection(AbConnectionParameters parameters)
            : base(parameters)
        {
            _connectionParametersAB = parameters;
        }

        public override bool Connect()
        {
            LogixProcessor = new LogixProcessor(_connectionParametersAB.Ip, _connectionParametersAB.Path, _connectionParametersAB.Port);
            bool result = LogixProcessor.Connect();
            _cpu = new AbCpu(LogixProcessor);
            return result;
        }

        public override bool Disconnect()
        {
            try
            {
                if (LogixProcessor != null)
                    return LogixProcessor.Disconnect();
            }
            finally
            {
                LogixProcessor = null;
            }

            return false;
        }

	    public override TagBase CreateTag(string name) => new AbTag(this, name);

	    public override void InvalidateMonitoringTargets()
        {
            //throw new NotImplementedException();
        }

        internal override IEnumerable<ChannelRequestExecutor> Channelize(IEnumerable<TagBase> tags)
        {
            var channel = new AbChannelRequestExecutor(this, tags);
            yield return channel;
        }


        public object ReadATag(string address) => ReadATag(new AbTag(this, address));
        public override object ReadATag(ITag tag) => ReadATag(new AbTag(this, tag.Name));

        public object ReadATag(AbTag tag)
        {
            if (!LogixProcessor.ReadTag(tag.UserTag))
                throw new Exception("Failed");

            return tag.UserTag.Value();
        }

    }
}
