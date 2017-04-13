namespace EIPNET.EIP
{
    /// <summary>
    /// Acromag_Intro_EthernetIP_747A.pdf, pp.19.
    /// 2 Bytes unsigned integer
    /// </summary>
    public enum EncapsCommand : ushort 
    {
        NOP = 0x0000,
        
        /* [0x0001.. 0x0003] : Reserved for legacy */

        /// <summary>
        /// May be sent via TCP or UDP
        /// </summary>
        ListServices = 0x0004,
        
        /* 0x0005 : Reserved for legacy */

        ListIdentity = 0x0063,
        ListInterfaces = 0x0064,
        RegisterSession = 0x0065,
        UnRegisterSession = 0x0066,
        /* [0x0067.. 0x006E] : Reserved for legacy */

        /// <summary>
        /// Sent only via TCP.
        /// </summary>
        SendRRData = 0x006F,
        /// <summary>
        /// Sent only via TCP.
        /// </summary>
        SendUnitData = 0x0070,
        /// <summary>
        /// Sent only via TCP.
        /// </summary>
        IndicateStatus = 0x0072,
        /// <summary>
        /// Sent only via TCP.
        /// </summary>
        Cancel = 0x0073

        /* [0x00C8.. 0xFFFF] : Reserved for future expansion - compliant products may not use command codes in this range. */
    }
}
