using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Tag 를 갖는 객체가 구현해야 할 interface
    /// </summary>
	[Guid("8514CB4C-B045-456E-AF80-582BDF20B58F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITaggable
    {
        /// <summary> 일반적인 C# 의 Tag 와 같은 기능을 수행함.  name collision 방지를 위해서 UserTag 로 이름 지음. </summary>
        object UserTag { get; set; }
    }
}
