using Dsu.PLCConverter.UI.AddressMapperLogics;
using Dsu.PLCConvertor.Common.Util;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;

namespace AddressMapper
{
    partial class FormAddressMapper
    {
        PLCHWSpecs LoadPLCHardwareSetting(string plcHardwareSettingFile)
        {
            var json = File.ReadAllText(plcHardwareSettingFile);
            var plcs = JsonConvert.DeserializeObject<PLCHWSpecs>(json, MyJsonSerializer.JsonSettingsSimple);
            return plcs;
        }
    }
}
