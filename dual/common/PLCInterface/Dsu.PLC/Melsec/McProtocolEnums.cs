namespace Dsu.PLC.Melsec
{
    /// <summary>
    /// MELSEC Commnunication Protocol-sh080008w.pdf, pp. 465
    /// </summary>
    public enum DeviceAccessCommand
    {
        BatchRead = 0x0401,
        BatchWrite = 0x1401,
        RandomRead = 0x0403,
        RandomWrite = 0x1402,
        BatchReadBlocks = 0x0406,
        BatchWriteBlocks = 0x1406,
        RegisterMonitor = 0x0801,
        Monitor = 0x0802,


        RemoteRun = 0x1001,
        RemoteStop = 0x1002,
        RemotePause = 0x1003,
        RemoteLatchClear = 0x1005,
        RemoteReset = 0x1006,
        ReadCpuModelName = 0x0101,

        BufferMemoryBatchRead = 0x0613,
        BufferMemoryBatchWrite = 0x1613,
        IntelligentFunctionModuleBatchRead = 0x0601,
        IntelligentFunctionModuleBatchWrite = 0x1601,

        PasswordUnlock = 0x1630,
        PasswordLock = 0x1631,

        LoopbackTest = 0x0619,
        ClearErrorInformation = 0x1617,

        FileReadInformation = 0x1810,
        FileSearchInformation = 0x1811,
        FileCreateNew = 0x1820,
        FileDelete = 0x1822,
        FileCopy = 0x1824,
        FileModifyAttribute = 0x1825,
        FileModifyCreateDateTime = 0x1826,
        FileOpen = 0x1827,
        FileRead = 0x1828,
        FileWrite = 0x1829,
        FileClose = 0x182A,


        FileControlReadInformationWithoutHeader = 0x0201,
        FileControlReadInformationWithHeader = 0x0202,
        FileControlReadPresence = 0x0203,
        FileControlReadNumberUsageStatus = 0x0204,
        FileControlRead = 0x0206,
        FileControlLock = 0x0808,
        FileControlCreate = 0x1202,     // register filename
        FileControlWriteTo = 0x1203,
        FileControlModifyInformation = 0x1204,
        FileControlDelete = 0x1205,
        FileControlCopy = 0x1206,

    }

    public enum DeviceAccessSubcommand
    {
        WordDevice = 0x0000,
        C24 = 0x0001,
        BitDevice = 0x0002,
        ASCII = 0x0003,
        BINARY = 0x0004,
    }


    /// <summary>
    /// Device Code : QJ71C24_QJ71E71_Reference(SECURED).pdf, pp. 3-67,  example for pp. 3-103
    /// see Dsu.PLC.Melsec.PlcDeviceType enum
    /// </summary>
    public enum DeviceCode : byte
    {
        SpecialRelay = 0x91,
        SpecialRegister = 0xA9,
        Input = 0x9C,
        Output = 0x9D,
        InternalRelay = 0x90,
        LatchRelay = 0x92,
        Annunciator = 0x93,
        EdgeRelay = 0x94,
        LinkRelay = 0xA0,
        DataRegister = 0xA8,
        LinkRegister = 0xB4,


        TimerContact = 0xC1,
        TimerCoil = 0xC0,
        TimerCurrentValue = 0xC2,

        RetentiveTimerContact = 0xC7,
        RetentiveTimerCoil = 0xC6,
        RetentiveTimerCurrentValue = 0xC8,

        CounterContact = 0xC4,
        CounterCoil = 0xC3,
        CounterCurrentValue = 0xC5,

		LinkSpecialRelay = 0xA1,
        LinkSpecialRegister = 0xB5,
        StepRelay = 0x98,
        DirectInput = 0xA2,
        DirectOutput = 0xA3,

        IndexRegister = 0xCC,
        FileRegisterR = 0xAF,
        FileRegisterZR = 0xB0,
        ExtendedDataRegister = 0xA8,
        ExtendedLinkRegister = 0xB4,


		X = Input,
        Y = Output,
        M = InternalRelay,
        L = LatchRelay,
        F = Annunciator,
        V = EdgeRelay,
        B = LinkRelay,
        D = DataRegister,
        W = LinkRegister,


		S = StepRelay,
		SM = SpecialRelay,
		SD = SpecialRegister,
		SB = LinkSpecialRelay,
		SW = LinkSpecialRegister,
		SS = RetentiveTimerContact,
        SC = RetentiveTimerCoil,
        SN = RetentiveTimerCurrentValue,

        CS = CounterContact,
        CC = CounterCoil,
        CN = CounterCurrentValue,

        TS = TimerContact,
        TC = TimerCoil,
        TN = TimerCurrentValue,

        DX = DirectInput,
        DY = DirectOutput,

    }

    /// <summary>
    /// see Q Corresponding MELSEC Communication Protocol Reference Manual.pdf, pp. 3-151
    /// </summary>
    public enum Drive : byte
    {
        /// <summary> QCPU built-in program memory </summary>
        BuiltIn = 0x0,
        /// <summary> Memory card(RAM).  SRAM card </summary>
        MemoryRAM = 0x1,
        /// <summary> Memory card(ROM).  Flash card/ATA card </summary>
        MemoryROM = 0x02,
        /// <summary> QCPU built-in standard RAM </summary>
        StandardRAM = 0x03,
        /// <summary> QCPU built-in standard ROM </summary>
        StandardROM = 0x04,
    }


    /// <summary>
    /// see Q Corresponding MELSEC Communication Protocol Reference Manual.pdf, pp. 3-162
    /// </summary>
    public enum OpenMode : byte
    {
        OpenForRead = 0x0,
        OpenForWrite = 0x1,
    }

    /// <summary>
    /// see Q Corresponding MELSEC Communication Protocol Reference Manual.pdf, pp. 3-161
    /// </summary>
    public enum CloseType : byte
    {
        /// <summary>
        /// Close only the target files.
        /// </summary>
        NormalClose = 0x0,
        /// <summary>
        /// Forcefully close files, including other files that are opened by
        /// the modules/devices that opened the target file.
        /// </summary>
        ForceClose1 = 0x1,
        /// <summary>
        /// Forcefully close all the open files.
        /// </summary>
        ForceClose2 = 0x2,
    }

}
