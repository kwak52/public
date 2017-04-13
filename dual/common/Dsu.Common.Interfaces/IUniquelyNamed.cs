using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Uniq한 이름을 갖는 객체가 구현해야 할 interface
    /// <br/> - 실제 객체 구현에 있어서 INamed 와 IUniquelyNamed 를 둘다 상속 받을 수 있다.
    /// </summary>
	[Guid("857C2546-5308-45B0-A2A2-1CD0B05C9C35")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IUniquelyNamed
    {
        /// <summary> unique 한 객체의 이름 </summary>
        string UniqueName { get; set; }
    }


    //[ComVisible(false)]
    //public interface IUniquelyNamedEntityContainer
    //{
    //    bool ContainsUniqueName(string name);
    //}


    /// <summary>
    /// 동일 클래스 내에서 Unique id 를 가지는 객체.
    /// unique id 는 runtime 에 유일하게 결정되고, serialize 되지 않음.   자동으로 결정되므로 setter 가 없음.
    /// </summary>
	[Guid("8F613342-0D82-4FF5-BC8F-7A04E5C370D3")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IUniquelyIdentifiable
    {
        /// <summary> unique 한 객체의 Id </summary>
        int UniqueId { get; }
    }
    /*
     * Sample Code
    class X
    {
        private int _uniqueId = -1;
        private static int _counter = 0;
        public int UniqueId { get { return _uniqueId; } }
        public X() { _uniqueId = ++_counter; }
    }
     */


    /// <summary>
    /// Program 내에서 unique 하게 identifable
    /// </summary>
    [ComVisible(false)]
    public interface IIntraProgramUniquelyIdentifiable
    {
        /// <summary> Intra program unique Id </summary>
        long PUID { get; }
    }

    /// <summary>
    /// 단위 program 내에서 unique id 를 제공하기 위한 static class
    /// </summary>
    [ComVisible(false)]
    public static class IntraProgramUniquelyIdentifiable
    {
        private static long _counter = 0L;

        /// <summary> 단위 program 내에서 unique 한 id 를 생성해서 반환 </summary>
        public static long AllocatePUID()
        {
            return Interlocked.Increment(ref _counter);
        }
    }
    /*
        public long PUID { get; private set; }
        PUID = IntraProgramUniquelyIdentifiable.AllocatePUID();
     */
}
