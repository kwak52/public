using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dsu.PLC.Common;
using Dsu.PLC.Utilities;


namespace Dsu.PLC.Fuji
{
    public class FjConnection : ConnectionBase
    {
        public FjProtocol FjProtocol { get; private set; }       // One of {FjProtocolTcp, FjProtocolUdp}

        private FjConnectionParameters _connectionParametersFuji = null;
        public override IConnectionParameters ConnectionParameters
        {
            get { return _connectionParametersFuji; }
            set { _connectionParametersFuji = (FjConnectionParameters)value; }
        }

		internal Config Config { get { return _connectionParametersFuji.Config; } }

        /// <summary>
        /// Config 에 의해서 결정되는, 모든 가능한 i/o 접점을 나열한다.
        /// </summary>
        /// <returns></returns>
	    public IEnumerable<string> EnumerateValidInputOutputTags() => Config.EnumerateValidInputOutputTags();

		public override ICpu Cpu { get { return FjProtocol.Cpu; } }

        public FjConnection(FjConnectionParameters parameters)
            : base(parameters)
        {
            _connectionParametersFuji = parameters;
            Debug.Assert(parameters.TransportProtocol == TransportProtocol.Tcp);
            FjProtocol = new FjProtocol(parameters.Ip, parameters.Port);
        }

        public override bool Connect()
        {
            if (FjProtocol == null)
                FjProtocol = new FjProtocol(_connectionParametersFuji.Ip, _connectionParametersFuji.Port);

            return FjProtocol.Open() == 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (FjProtocol != null)
            {
                FjProtocol.Close();
                FjProtocol = null;
            }

            base.Dispose(disposing);
        }

	    public override TagBase CreateTag(string name) => FjTag.Create(this, name);

	    public override void InvalidateMonitoringTargets()
        {
            // todo : 
            // throw new NotImplementedException();
        }

        //public IEnumerable<TagFuji> WordDevices { get { return Tags.Values.Cast<TagFuji>().Where(t => !t.IsHexDevice); } }
        //public IEnumerable<TagFuji> DoubleWordDevices { get { return Tags.Values.Cast<TagFuji>().Where(t => t.IsHexDevice); } }

        public override object ReadATag(ITag tag) => null;

        internal override IEnumerable<ChannelRequestExecutor> Channelize(IEnumerable<TagBase> tagbases)
        {
            var tags = tagbases.OfType<FjTag>();
            const int maxAllowedBytes = 492;
            int maxRequestBytes = maxAllowedBytes - 6 - 4;      // pp. App.1-2

            var grps =
                    from t in tags
                    group t by
                    new {MemoryType = t.MemoryType, Channel = t.ByteOffset.GetValueUnsafe() / maxRequestBytes}
                    into g
                    select new
                    {
                        Channel = g.Key.Channel,
                        MemoryType = g.Key.MemoryType,
                        Tags = g.ToList()
                    }
                ;
            foreach (var grp in grps)
            {
                var iots = grp.Tags;
                var memoryType = grp.MemoryType;
                var s = iots.Min(t => t.ByteOffset.GetValueUnsafe());
                var e = iots.Max(t => t.ByteOffset.GetValueUnsafe());
                yield return new FjChannelRequestExecutor(this, memoryType, s, e + 4, iots);
            }
        }
    }
}
