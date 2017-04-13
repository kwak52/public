namespace Dsu.PLC.Fuji
{
    public enum MemoryType : byte
    {
        /// <summary>
        /// Input/Output memory 를 동시에 읽어 온다.
        /// </summary>
        InputOutputMemory = 0x01,

        /// <summary>
        /// Memory 영역
        /// </summary>
        StandardMemory = 0x02,

        /// <summary>
        /// Latch Memory ?
        /// </summary>
        RetainMemory = 0x04,

        /// <summary>
        /// read-only
        /// </summary>
        SystemMemory = 0x08,

        // 0x06, 0x07, 0xFF : 뭔지 모르겠으나, 결과는 날아온다.
        // 0x10, 0x11 : InputOutputMemory 와 동일한 결과 반환

        PE_LinkBroadcastArea = 0xFF,
        P_LinkBroadcastArea = 0xFF,
        FL_LinkCommonMemory = 0xFF,
    }

    public enum ConnectionMode : byte
    {
        Cpu0 = 0x7A,
        /// <summary>
        /// Cpu1 to Cpu7, P/PE-link, FL-net
        /// </summary>
        NonCpu0 = 0x7B,
    }


    public enum CommandType : byte
    {
        Read = 0x00,
        Write = 0x01,
        Cpu = 0x04,
    }


    public enum ModeType : byte
    {
        Read = 0x00,
        Write = 0x00,

        CpuBatchStart = 0x00,
        CpuBatchInitialize = 0x01,
        CpuBatchStop = 0x02,
        CpuBatchReset = 0x03,
        CpuIndividualStart = 0x04,
        CpuIndividualInitializeAndStart = 0x05,
        CpuIndividualStop = 0x06,
        CpuIndividualReset = 0x07,
    }


    public enum OperationStatusType : byte
    {
        /// <summary>
        /// Ended normally The processing of command is completed successfully.
        /// </summary>
        Success = 0x00,

        /// <summary>
        /// Command cannot be executed because an abnormality occurred on the CPU.
        /// </summary>
        CpuError = 0x10,

        /// <summary>
        /// Command cannot be executed because the CPU is running.
        /// </summary>
        CpuRunning = 0x11,

        /// <summary>
        /// Command cannot be executed due to the key switch condition of the CPU.
        /// </summary>
        CommandUnexecutable = 0x12,

        /// <summary>
        /// CPU received undefined command or mode.
        /// </summary>
        UndefinedCommand = 0x20,

        /// <summary>
        /// Setting error was found in command header part.
        /// </summary>
        ParameterError = 0x22,

        /// <summary>
        /// Transmission is interlocked by a command from other device.
        /// </summary>
        TransmissionInterlocked = 0x23,

        /// <summary>
        /// Requested command cannot be executed because other command is now being executed.
        /// </summary>
        ProcessingACommand = 0x28,

        /// <summary>
        /// Requested command cannot be executed because D300win loader is now processing.
        /// </summary>
        RemoteLoaderNowProcessing = 0x2B,

        /// <summary>
        /// Requested command cannot be executed because the system is now being initialized.
        /// </summary>
        InitializationNotCompeted = 0x2F,

        /// <summary>
        /// Invalid data type or number was specified
        /// </summary>
        DataSettingError = 0x40,

        /// <summary>
        ///  Specified data cannot be found
        /// </summary>
        InexistentData = 0x41,

        /// <summary>
        /// Specified address exceeds the valid range.
        /// </summary>
        MemoryAddressSettingError = 0x44, 

        /// <summary>
        /// Address + the number of read/write words exceed the valid range.
        /// </summary>
        MemorySizeOver = 0x45, 

        /// <summary>
        /// No module exists at specified destination station number.
        /// </summary>
        CommandSendDestinationSettingError = 0xA0, 

        /// <summary>
        /// No response data is returned from the remote module.
        /// </summary>
        NoResponseToCommand = 0xA2, 

        /// <summary>
        /// Command cannot be communicated because an abnormality occurred on the SX bus.
        /// </summary>
        SXBusSendError = 0xA4, 

        /// <summary>
        /// Command cannot be communicated because NAK occurred while sending data via SX bus.
        /// </summary>
        SXBusSendNAK = 0xA5, 

        /// <summary>
        /// "Operation status" must be set to FF when issuing a request command.
        /// </summary>
        SpecificationAtSendingRequestCommand = 0xFF, 
        Send = SpecificationAtSendingRequestCommand,
    }
}
