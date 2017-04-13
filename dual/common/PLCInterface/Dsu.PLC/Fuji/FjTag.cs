using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Common;
using LanguageExt;

namespace Dsu.PLC.Fuji
{
	/// <summary>
	/// Fuji PLC Tag
	/// </summary>
    public class FjTag : TagBase
    {
        internal FjConnection Connection => (FjConnection)ConnectionBase;

	    public override Option<int> ByteOffset { get {return 2 * Address; } }
        public override bool IsBitAddress { get; protected internal set; }
        public int Address { get; protected set; }

		public virtual Option<int> UnitNumber { get { throw new NotReimplementedException("Cpu or Bus number not re-implemented.");} }

		public MemoryType MemoryType { get; internal set; }

        public override string ToString() => Name;


        protected FjTag(FjConnection connection, string name)
            : base(connection)
        {
			Contract.Requires(name.StartsWith("%"));
            Name = name;
            connection?.AddMonitoringTag(this);
        }

		/// <summary>
		/// override to perform actions after concrete class construction
		/// </summary>
        protected virtual void ConstructionEpilogue() { }

        /// <summary>
        /// Factory method
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="name">e.g. "%IW.3.0"</param>
        /// <returns></returns>
        public static FjTag Create(FjConnection connection, string name)
        {
            if (!name.StartsWith("%"))
                throw new PlcExceptionTag("Address not begins with '%'");

            var classifier = name[1];
            var wx = name[2];
            Debug.Assert(classifier.IsOneOf('I', 'Q', 'M'));
            Debug.Assert(wx.IsOneOf('W', 'X'));

            FjTag tag = null;
            if ( classifier == 'M')
                tag = new FjTagSRS(connection, name);
            else
                tag = new FjTagIO(connection, name);

            tag.IsBitAddress = wx == 'X';

            tag.ConstructionEpilogue();
            return tag;
        }

	    public static Try<FjTag> TryCreate(FjConnection connection, string name) => () => Create(connection, name);


		/// <summary>
		/// Read request 를 위해서 한꺼번에 읽을 수 있는 tag 들끼리 grouping 한다. 
		/// </summary>
		public static IEnumerable<IEnumerable<FjTag>> ChannelizeTags(IEnumerable<FjTag> tags)
		{
			var ioGroup = tags.OfType<FjTagIO>().GroupBy(t => t.BusUnitNumber);
			foreach (var g in ioGroup)
				yield return g.ToList();

			// 2단 grouping : http://stackoverflow.com/questions/7325278/group-by-in-linq
			var srsGroup = from tg in tags.OfType<FjTagSRS>()
						   group tg by new { tg.CpuNumber, tg.MemoryClassificationNumber } into g
						   select g;

			foreach (var g in srsGroup)
				yield return g.ToList();
	    }
    }
}
