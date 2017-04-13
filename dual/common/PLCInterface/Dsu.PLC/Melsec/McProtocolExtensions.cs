using System.Collections.Generic;
using System.Linq;

namespace Dsu.PLC.Melsec
{
    internal static class McProtocolExtensions
    {
        public static bool IsBitDevice(this PlcDeviceType type) => McProtocolApp.IsBitDevice(type);
        public static bool IsHexDevice(this PlcDeviceType type) => McProtocolApp.IsHexDevice(type);
        public static bool IsWordDevice(this PlcDeviceType type) => McProtocolApp.IsWordDevice(type);

        public static PlcDeviceType GetDeviceType(this string deviceName) => McProtocolApp.GetDeviceType(deviceName);

		public static IEnumerable<MxTag> SelectWordDevices(this IEnumerable<MxTag> tags) => tags.Where(t => t.IsWordDevice);
        public static IEnumerable<MxTag> SelectDoubleWordDevices(this IEnumerable<MxTag> tags) => tags.Where(t => ! t.IsWordDevice);
    }
}
