module DeviceDriverInterfaces

type IPowerSupply =
    abstract GetVoltage: string -> double option        // channel -> value
    abstract GetVoltage: int -> double option           // channel -> value
    abstract SetVoltage: double -> bool
    abstract Id: string with get



