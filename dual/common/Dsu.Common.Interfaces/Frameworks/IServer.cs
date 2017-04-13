using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Server interface
    /// </summary>
	[Guid("689598CD-3717-4C1F-B79D-9F9CF8BAF1B3")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IServer
    {
        /// <summary> Server name getter </summary>
        string ServerName { get; }     // Do *NOT* rename to "Name".  충돌함.

        /// <summary> Server version getter </summary>
        string Version { get; }

        /// <summary> Server PID(Procss id) getter </summary>
        int PID { get; }

        /// <summary> Echo back for test </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        string Echo(string message);

        /// <summary> Echo back to the given client, for test </summary>
        void EchoBack(IClient client, string message);

        /// <summary> Say hello for test </summary>
        string Hello(string greeting);

        /// <summary> Client 로부터 접속 요청을 처리 </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        bool Connect(IClient client);

        /// <summary> 연결된 Client 로부터 접속 해지 요청을 처리 </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        bool Disconnect(IClient client);
    }
}
