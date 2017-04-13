

namespace Dsu.PacketAnalysis
open PcapDotNet.Core

module Devices =
    let NetworkDevices = LivePacketDevice.AllLocalMachine;

    let deviceAddress (da : DeviceAddress) = 
        (da.Address.Family, da.Broadcast.Family, da.Destination.Family, da.Netmask.Family)

    let DeviceInfo (device : LivePacketDevice) = 
        let daInfo = device.Addresses |> Seq.map(fun da -> deviceAddress da)

        let da = deviceAddress 
        (device.Name,
         daInfo,
         device.Attributes,
         device.Description)


type MonitoringTarget(ip:string) =
    //let ip = ip
    member x.Ip = ip


