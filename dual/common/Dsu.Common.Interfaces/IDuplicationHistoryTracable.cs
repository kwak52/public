using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// source 로 부터 복사 생성된 객체가 원본 source 에 대한 정보를 담고 있다.
    /// 복사 source 및 target 모두 IDuplicationHistoryTracable 이어야 함
    /// </summary>
	[Guid("A8E0F2A5-3E46-4ED0-8BC7-600672DC6B6C")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IDuplicationHistoryTracable
    {
        /// <summary>
        /// 누구한테서 복사되었나
        /// <br/> - 최초 originator 를 찾는 것은 Originator 를 null 일때까지 찾아 가면 됨.
        /// </summary>
        IDuplicationHistoryTracable Originator { get; }
    }
}
