#if COMPILED
[<AutoOpen>]
module Dsu.Driver.NiDaqHwProbe
#endif


#if INTERACTIVE
#r "../CpTesterPlatformRelease/ExtLib/x64/NationalInstruments.DAQmx.dll"
#endif

open NationalInstruments.DAQmx

let GetLocalDeviceNames() = DaqSystem.Local.Devices
let GetLocalDevices() = Array.map DaqSystem.Local.LoadDevice DaqSystem.Local.Devices
let GetLocalAOChannels() = 
    GetLocalDevices() |> Array.collect (fun d -> d.AOPhysicalChannels)

let GetLocalAIChannels() = 
    GetLocalDevices() |> Array.collect (fun d -> d.AIPhysicalChannels)



/// see http://zone.ni.com/reference/en-XX/help/370473H-01/TOC420.htm for Device properties
type NiDaqHwLocal() =
    /// NiDaqHwLocal PC(this computer) 에 연결된 NI DAQ device 이름 들
    static member DeviceNames = DaqSystem.Local.Devices
    /// NiDaqHwLocal PC에 연결된 NI DAQ device 들
    static member Devices = Array.map DaqSystem.Local.LoadDevice DaqSystem.Local.Devices
    /// NI DAQ device 의 Analog Output channel 이름들
    static member AOChannels = NiDaqHwLocal.Devices |> Array.collect (fun d -> d.AOPhysicalChannels)
    /// NI DAQ device 의 Analog Input channel 이름들
    static member AIChannels = NiDaqHwLocal.Devices |> Array.collect (fun d -> d.AIPhysicalChannels)

    /// "Dev5/ao0" 형태의 channel 이름을 parsing 하여 ("Dev5", "ao0") 형태의 tuple option 으로 반환한다. 
    static member ParseChannelName(channel) =
        match channel with
        | ChannelPattern (dev,ch) -> Some(dev, ch)
        | _ -> None

    /// device 이름을 이용한 NI DAQ device 반환
    static member GetDevice(deviceName) = DaqSystem.Local.LoadDevice(deviceName)
    /// device 이름들을 이용한 NI DAQ device 들을 반환
    static member GetDevices(deviceNames:string seq) = Seq.map DaqSystem.Local.LoadDevice deviceNames
    /// channel 이름 ("Dev1/ai0") 을 이용한 NI DAQ device 을 반환
    static member GetDeviceFromChannel(channel) = 
        let parsed = NiDaqHwLocal.ParseChannelName(channel)
        let deviceName = fst parsed.Value
        NiDaqHwLocal.GetDevice(deviceName)

    static member GetAOChannels(deviceName) = NiDaqHwLocal.GetDevice(deviceName).AOPhysicalChannels   // {"Dev5/ao0, "Dev5/ao1", "Dev5/ao2", "Dev5/ao3" }
    static member GetAOChannels(deviceNames:string seq) = NiDaqHwLocal.GetDevices(deviceNames) |> Seq.collect (fun d -> d.AOPhysicalChannels)   // {"Dev5/ao0, "Dev5/ao1", "Dev5/ao2", "Dev5/ao3" }
    static member GetAIChannels(deviceName) = NiDaqHwLocal.GetDevice(deviceName).AIPhysicalChannels   // {"Dev5/ai0, "Dev5/ai1" }
    static member GetAIChannels(deviceNames:string seq) = NiDaqHwLocal.GetDevices(deviceNames) |> Seq.collect (fun d -> d.AIPhysicalChannels)   // {"Dev5/ai0, "Dev5/ai1" }
    static member GetProductType(deviceName)        = NiDaqHwLocal.GetDevice(deviceName).ProductType
    static member GetAOMaximumRate(deviceName)      = NiDaqHwLocal.GetDevice(deviceName).AOMaximumRate      // 4000000.0
    static member GetAOMinmumRate(deviceName)       = NiDaqHwLocal.GetDevice(deviceName).AOMinimumRate      // 0.005960464478
    static member GetAOVoltageRanges(deviceName)    = NiDaqHwLocal.GetDevice(deviceName).AOVoltageRanges    // (-10.0, +10.0)
    static member GetAOSampleClockSupported(deviceName) = NiDaqHwLocal.GetDevice(deviceName).AOSampleClockSupported     // true for 6115
    static member GetAIMaximumMultiChannelRate(deviceName) = NiDaqHwLocal.GetDevice(deviceName).AIMaximumMultiChannelRate
    static member GetAIMaximumSingleChannelRate(deviceName) = NiDaqHwLocal.GetDevice(deviceName).AIMaximumSingleChannelRate
    static member GetAIMinimumRate(deviceName) = NiDaqHwLocal.GetDevice(deviceName).AIMinimumRate
    ///Indicates if the device supports simultaneous sampling.
    static member GetAISimultaneousSamplingSupported(deviceName) = NiDaqHwLocal.GetDevice(deviceName).AISimultaneousSamplingSupported
    /// 주어진 channel 이름들에서 현재 시스템에서 지원되지 않는 channel 이름을 추려서 반환한다.
    static member SelectInvalidChannels(channles:string seq) =
        let localChannelsLookup = buildSimpleLookup(Seq.append NiDaqHwLocal.AIChannels NiDaqHwLocal.AOChannels)
        channles |> Seq.filter(fun ch -> not (localChannelsLookup.Contains(ch)))

    static member IsLowpassCutoffSupported(device: Device) = 
        device.ProductType.ToString().Contains("6115")
    static member IsLowpassCutoffSupportedOnChannel(channel) = 
        let device = NiDaqHwLocal.GetDeviceFromChannel(channel)
        NiDaqHwLocal.IsLowpassCutoffSupported(device)
