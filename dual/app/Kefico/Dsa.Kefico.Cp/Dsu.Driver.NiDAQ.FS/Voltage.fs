[<AutoOpen>]
module Dsu.Driver.Voltage

open NationalInstruments.DAQmx


/// <summary>
/// Reads single sample.  returns Measure type (Value, OutOfRange, Exception)
/// </summary>
/// <param name="physicalChannelName">Channgel name</param>
/// <param name="terminalConfiguration"></param>
/// <param name="min"></param>
/// <param name="max"></param>
/// <param name="units"></param>
let readSingleSample physicalChannelName terminalConfiguration (min, max) (units:AIVoltageUnits) : Measure =
    try
        use task = new Task()
        let channel = task.AIChannels.CreateVoltageChannel(physicalChannelName, "", terminalConfiguration, min, max, units)
        let reader = new AnalogSingleChannelReader(task.Stream)

        // Verify the task
        task.Control(TaskAction.Verify);

        let value = reader.ReadSingleSample()
        if inClosedRange value (min, max) then
            Measure(value)
        else
            OutOfRange
    with exn ->
        Exception(exn)

let writeSingleSample physicalChannelName (min, max) value (units:AOVoltageUnits) =
    use task = new Task()
    let channel = task.AOChannels.CreateVoltageChannel(physicalChannelName, "", min, max, units)
    let writer = new AnalogSingleChannelWriter(task.Stream)
    writer.WriteSingleSample(true, value)


let loopbackTest() =
    let range = 0.0, 5.0
    let writeValue = 2.2
    writeSingleSample "dev1/ao0" range writeValue AOVoltageUnits.Volts
    let readValue = readSingleSample "dev1/ai0" AITerminalConfiguration.Differential range AIVoltageUnits.Volts
    match readValue with
    | Measure(v) -> v = writeValue
    | _ -> false

