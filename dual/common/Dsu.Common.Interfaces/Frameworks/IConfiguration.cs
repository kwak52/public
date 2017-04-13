using System;
using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary> Configuration interface </summary>
	[Guid("8501C86A-61E7-4F92-B90A-1779CCF6AAF3")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IConfiguration : ICloneable
    {
        /// <summary> Save configuration to file </summary>
        void QSave(string fileName);
        /// <summary> Load configuration from file </summary>
        void QLoad(string fileName);
    }


    /// <summary> Configurable interface : IConfiguration 을 갖는 객체가 구현해야 할 interface </summary>
	[Guid("788DF347-09DA-46DF-B5A9-FE1DC4CBEBCF")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IConfigurable
    {
        /// <summary> Configuration </summary>
        IConfiguration Configuration { get; }
    }
}
