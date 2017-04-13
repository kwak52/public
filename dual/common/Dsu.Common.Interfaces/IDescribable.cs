using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Description 을 갖는 객체가 구현해야 할 interface
    /// </summary>
	[Guid("4090748C-2912-4BD2-8E70-6900E20F594F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IDescribable
    {
        /// <summary> Note.  주로 사용자 메모 </summary>
        string Note { get; set; }
    }
}
