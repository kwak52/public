using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// 행위에 대한 reason 제공을 위한 interface
    /// </summary>
	[Guid("23F6D92B-C060-4958-8CEE-298E79EB37DF")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IReason
    {
        /// <summary> Reason </summary>
        string Reason { get; }
    }


    /// <summary>
    /// IReason interface 에 대한 slim implemetation
    /// </summary>
    public class ReasonSlim : IReason
    {
        /// <summary> Reason </summary>
        public string Reason { get; private set; }

        /// <summary> Constructor </summary>
        public ReasonSlim(string reason) { Reason = reason; }
    }
}
