using System;
using DotNetSiemensPLCToolBoxLibrary.Communication;
using Dsu.PLC.Common;

namespace Dsu.PLC.Siemens
{
    public class S7Tag : TagBase
    {
        /// <summary>
        /// Siemens ToolBox library tag.
        /// </summary>
        internal PLCTag PlcTag { get; private set; }

        private S7Connection Connection => (S7Connection)ConnectionBase;

        //public int Addreess { get; private set; } = -1;

        public sealed override string Name
        {
            get { return PlcTag.ValueName; }
            set { PlcTag.ValueName = value; }
        }

        public override bool IsBitAddress { get { throw new NotImplementedException(); } protected internal set { throw new NotImplementedException(); } }

        public sealed override object Value
        {
            get { return PlcTag.Value; }
            set
            {
                PlcTag.Value = value;
                base.Value = value;
            }
        }


        public S7Tag(S7Connection connection, string name)
            : base(connection)
        {
            PlcTag = new PLCTag(name);
            Name = name;
            connection?.AddMonitoringTag(this);
        }
    }
}
