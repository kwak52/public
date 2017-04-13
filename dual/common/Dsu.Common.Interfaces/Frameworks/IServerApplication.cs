using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Server application interface.
    /// </summary>
	[Guid("4BF3272A-56FA-484C-B40F-B211B6CAB4D6")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IServerApplication : IServer, IApplication
    {
    }
}