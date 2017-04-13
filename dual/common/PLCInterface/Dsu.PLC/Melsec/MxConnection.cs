using System.Collections.Generic;
using System.Linq;
using Dsu.Common.Utilities;
using Dsu.PLC.Common;
using Dsu.PLC.Utilities;
using static LanguageExt.Prelude;


namespace Dsu.PLC.Melsec
{
    public class MxConnection : ConnectionBase
    {
        public McProtocolApp McProtocol { get; private set; }       // One of {McProtocolTcp, McProtocolUdp}

        private MxConnectionParameters _connectionParametersMelsec = null;
        public override IConnectionParameters ConnectionParameters
        {
            get { return _connectionParametersMelsec; }
            set { _connectionParametersMelsec = (MxConnectionParameters)value; }
        }

        public override ICpu Cpu { get { return McProtocol.Cpu; } }

        public MxConnection(MxConnectionParameters parameters)
            : base(parameters)
        {
            _connectionParametersMelsec = parameters;
            McProtocol = CreateConnect();
        }

        private McProtocolApp CreateConnect()
        {
            McProtocolApp conn;
            switch (_connectionParametersMelsec.TransportProtocol)
            {
                case TransportProtocol.Tcp:
                    conn = new McProtocolTcp(_connectionParametersMelsec.Ip, _connectionParametersMelsec.Port);
                    break;
                case TransportProtocol.Udp:
                    conn = new McProtocolUdp(_connectionParametersMelsec.Ip, _connectionParametersMelsec.Port);
                    break;
                default:
                    throw new UnexpectedCaseOccurredException("Unsupported transport protocol: " + _connectionParametersMelsec.TransportProtocol);
            }
            return conn;
        }

        public override bool Connect()
        {
                if (McProtocol == null)
                    McProtocol = CreateConnect();

                return McProtocol.Open() == 0;
            }
           

        protected override void Dispose(bool disposing)
        {
            if (McProtocol != null)
            {
                McProtocol.Close();
                McProtocol = null;
            }

            base.Dispose(disposing);
        }

	    public override TagBase CreateTag(string name) => MxTag.Create(this, name);

        public override void InvalidateMonitoringTargets()
        {
            // todo : 
            // throw new NotImplementedException();
        }

        public IEnumerable<MxTag> WordDevices { get { return Tags.Values.Cast<MxTag>().SelectWordDevices(); } }
        public IEnumerable<MxTag> DoubleWordDevices { get { return Tags.Values.Cast<MxTag>().SelectDoubleWordDevices(); } }


        public int MaxPoints { get; set; } = 192;
        internal override IEnumerable<ChannelRequestExecutor> Channelize(IEnumerable<TagBase> tags)
        {
            var partitions = PartitionTagGroups(tags.Cast<MxTag>(), MaxPoints).ToList();
            foreach (var partition in partitions)
            {
                var query = from t in partition
                             where t.BitwiseRange.IsSome
                             let k = t.BitwiseRange.GetValueUnsafe()
                             let nextDoubleWordAddress = (1 + t.Addreess / 32) * 32
                             let nextDoubleWordAddressString = t.IsHexDevice ? nextDoubleWordAddress.ToString("X") : nextDoubleWordAddress.ToString()
                             let newName0 = $"{t.DeviceType}{t.AddressString}"
                             let newName1 = $"{t.DeviceType}{nextDoubleWordAddressString}"
                             select new
                             {
                                 Original = t,
                                 Aliases = new MxTag[]
                                 {
                                     MxTag.Create(this, newName0, isHelperTag: true),
                                     MxTag.Create(this, newName1, isHelperTag: true),
                                 }
                             }
                    ;

                foreach (var pr in query)
                    pr.Original.Aliases.AddRange(pr.Aliases);

                var additionalTags = query.SelectMany(pr => pr.Aliases);

                var allTags = partition.Concat(additionalTags);
                yield return new MxChannelRequestExecutorRandom(this, allTags);

            }
        }

        private IEnumerable<IEnumerable<MxTag>> PartitionTagGroups(IEnumerable<MxTag> tags, int maxPoint)
        {
            var grouped =
                from t in tags
                //let byteLength = t.IsWordDevice ? 2 : 4
                group t by new 
                {
                    DeviceType = t.DeviceType,
                    PackStart = t.Addreess / (t.IsWordDevice ? 1 : 4),
                    NumPoints = match(t.BitwiseRange,
                            Some: k => (k * 4 > 32 - t.Addreess % 32) ? 2 : 1,
                            None: () => t.DeviceType == PlcDeviceType.ZR ? 2 : 1),
                    //PackLength = byteLength + match(t.BitwiseRange,
                    //        Some: k => (k * 4 > 32 - t.Addreess % 32) ? 4 : 0,
                    //        None: () => 0)
                }
                into g
                select new
                {
                    Tags = g.ToList(),
                    NumPoints = g.Key.NumPoints,
                    //DeviceType = g.Key.DeviceType,
                    //PackStart = g.Key.PackStart,
                    //PackLength = g.Key.PackLength,
                }
                ;


            int numPoint = 0;
            var partition = new List<MxTag>();
            foreach (var grp in grouped)
            {
                if (numPoint + grp.NumPoints >= maxPoint)
                {
                    yield return partition;
                    numPoint = 0;
                    partition = new List<MxTag>();
                }

                numPoint += grp.NumPoints;
                partition.AddRange(grp.Tags);
            }

            yield return partition;
        }


        public override object ReadATag(ITag tag) => null;
    }
}
