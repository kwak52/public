namespace EIPNET.EIP
{
    /// <summary>
    /// CIP Vol2_1.4.pdf, Table 2-6.3
    /// </summary>
    public enum CommonPacketTypeId : ushort
    {
        NULL = 0x0000,
        ListIdentityResponse = 0x000C,
        ConnectionBased = 0x00A1,
        ConnectedTransportPacket = 0x00B1,
        UnconnectedMessage = 0x00B2,
        /// <summary>
        /// Sockaddr Info, originator-to-target
        /// </summary>
        SocketAddrInfo_O2T = 0x8000,
        /// <summary>
        /// Sockaddr Info, target-to-originator
        /// </summary>
        SocketAddrInfo_T2O = 0x8001,

        ListServiceResponse = 0x0100,

        /// <summary>
        /// Sequenced Address iteme
        /// </summary>
        SequencedAddressItem = 0x8002
    }
}
