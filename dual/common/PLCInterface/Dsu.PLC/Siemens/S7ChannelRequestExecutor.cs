using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using DotNetSiemensPLCToolBoxLibrary.Communication;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Common;

namespace Dsu.PLC.Siemens
{
    internal class S7ChannelRequestExecutor : ChannelRequestExecutor
	{
		public S7Connection S7Connection { get { return (S7Connection) Connection; } }
        internal IEnumerable<PLCTag> PLCTags => Tags.Cast<S7Tag>().Select(t => t.PlcTag);
        public S7ChannelRequestExecutor(S7Connection connection, IEnumerable<TagBase> tags)
			: base(connection, tags)
		{
			Contract.Requires(tags.NonNullAny());
		}

		public override bool ExecuteRead()
		{
            S7Connection.PLCConnection.ReadValues(PLCTags);
            return true;
		}
	}
}
