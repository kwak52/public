using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// 이름을 갖는 객체가 구현해야 할 interface
    /// </summary>
	[Guid("04C5219F-5E58-4094-B05C-A6D66D33B5D4")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface INamed
    {
        /// <summary> 객체의 이름 </summary>
        string Name { get; set; }
    }
}
