/// NI DAQ interface module for C#
module Dsu.Driver.NiDaq

open Dsu.Driver.Base
open NationalInstruments.DAQmx

/// NI DAQ MultiChannel manager 생성을 위한 parameter 생성 
/// <param name="channelPrefix">e.g "Dev5/ai"</param>
/// <param name="numChannel">channel 갯수 지정.  e.g 4 로 지정하면 {Dev5/ai0, Dev5/ai1, Dev5/ai2, Dev5/ai3} 의 channel 에 대해서 manage 함 </param>
let CreateDefaultMcParameter(channelNames) = NiDaqParams.createDefaultMcAiParameter(channelNames)
/// NI DAQ MultiChannel manager 를 생성
let CreateMcManager parameters = NiDaqMcAi.createManager parameters
/// NI DAQ MultiChannel manager 를 생성
/// <param name="channelPrefix">e.g "Dev5/ai"</param>
/// <param name="numChannel">channel 갯수 지정.  e.g 4 로 지정하면 {Dev5/ai0, Dev5/ai1, Dev5/ai2, Dev5/ai3} 의 channel 에 대해서 manage 함 </param>
let CreateManagerSimple(channelNames) = NiDaqMcAi.createManager (NiDaqParams.createDefaultMcAiParameter(channelNames))
/// 이미 생성된 NI DAQ MultiChannel manager instance 를 접근
let McManager() = NiDaqMcAi.manager()
/// NI DAQ SingleChannel manager 를 생성.  호출 전에 반드시 다채널 manager 가 생성되어 있어야 하며, channel 이 등록되어 있어야 한다.
let CreateScManager(channel) = new NiDaqScAi.DaqScAiManager(channel)
/// Get NationalInstruments.DAQmx.Device instance from deviceName.  e.g deviceName = "Dev1"
let GetDevice(deviceName) = DaqSystem.Local.LoadDevice(deviceName)
/// Perform reset on device.  e.g deviceName = "Dev1"
let ResetDevice(deviceName) = GetDevice(deviceName).Reset()
/// Perform self-test on device.  e.g deviceName = "Dev1"
let SelfTestDevice(deviceName) = GetDevice(deviceName).SelfTest()

let LogDeviceInfo(deviceName) =
    let d = GetDevice(deviceName)
    logInfo "NiDaq information for device: %s" deviceName
    //logInfo "\tAccessoryConnectionCount=%d" d.AccessoryConnectionCount
    PrintProperties(d)
