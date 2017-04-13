using System;
using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Client interface for Server connection.
    /// </summary>
	[Guid("B2DCBCF4-B33A-4164-BF09-228D26BB441D")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IClient
    {
        /// <summary> Name of the client </summary>
        string ClientName { get; }

        /// <summary> Version of the client </summary>
        string VersionString { get; }

        /// <summary> Process id of the client </summary>
        int PID { get; }

        /// <summary> Server-side 에서 연결된 connection 을 강제로 끊을 때에, client 에게 최종 공지하기 위함. </summary>
        /// <param name="server"></param>
        void DisconnectedByServer(IServer server);

        /// <summary> test </summary>
        string Hello(string greeting);

        /// <summary> test </summary>
        void EchoCallback(string greeting);
    }
}
