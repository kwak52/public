using System;
using ControlLogixNET;
using ControlLogixNET.LogixType;
using Dsu.PLC.Common;

namespace Dsu.PLC.AB
{
    public class AbTag : TagBase
    {
        internal LogixTag UserTag { get; set; }
        internal LogixTagInfo TagInfo { get; set; }

        private AbConnection Connection => (AbConnection)ConnectionBase;
        private LogixProcessor Processor => Connection?.LogixProcessor;


        public sealed override string Name
        {
            get { return TagInfo.TagName; }
            set { TagInfo = Processor.GetTagInformation(value); }
        }

        public override bool IsBitAddress { get { throw new NotImplementedException(); } protected internal set { throw new NotImplementedException(); } }

        public AbTag(AbConnection connection, string name)
            : base(connection)
        {
            Name = name;
            UserTag = LogixTagFactory.CreateTag(name, connection.LogixProcessor);
            connection?.AddMonitoringTag(this);
        }
    }
}
