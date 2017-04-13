using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary> 고장날 수 있는 device 가 구현해야 할 interface.  Actuator 및 sensor 류 </summary>
	[Guid("BE8BCB9D-69DC-4B8C-9E79-579EB4B3DFFF")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IFaulty : INamed, IUniquelyNamed
    {
        /// <summary> 고장 상태 </summary>
        bool IsOutOfOrder { get; set; }
    }
}
