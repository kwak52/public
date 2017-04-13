using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ControlLogixNET;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Common;
using static LanguageExt.Prelude;


namespace Dsu.PLC.AB
{
    internal class AbChannelRequestExecutor : ChannelRequestExecutor
	{
		public AbConnection AbConnection { get { return (AbConnection) Connection; } }
        public AbChannelRequestExecutor(AbConnection connection, IEnumerable<TagBase> tags)
			: base(connection, tags)
		{
			Contract.Requires(tags.NonNullAny());
		}

		public override bool ExecuteRead()
		{
            var trial = LogixServices.TryReadLogixData(AbConnection);
            return match(trial,
                Succ: v => true,
                Fail: ex => false
               );
        }
	}
}
