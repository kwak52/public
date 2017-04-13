using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.PLC.Melsec
{
    partial class McProtocolApp
    {
        internal IEnumerable<byte> GenerateRandomRequestPacket(MxConnection connection, IEnumerable<MxTag> wds, IEnumerable<MxTag> dwds, bool packAlign=false)
        {
			yield return (byte)wds.Count();
            yield return (byte)dwds.Count();

            foreach (var wd in wds)
            {
                foreach (var b in wd.GetDeviceDesignation(packAlign ? 16 : 0))
                    yield return b;
            }

            foreach (var dwd in dwds)
            {
                foreach (var b in dwd.GetDeviceDesignation(packAlign ? 32 : 0))
                    yield return b;
            }
        }


   //     /// <summary>
   //     /// Read random devices.  see MELSEC Communication protocol-sh0800008w.pdf, pp. 99
   //     /// </summary>
   //     public void ReadDeviceRandom(MxConnection connection, IEnumerable<MxTag> tags = null)
   //     {
   //         var data = GenerateRandomRequestPacket(connection, tags);
   //         var response = ExecuteCommand(DeviceAccessCommand.RandomRead, data.ToArray());
			//Trace.WriteLine(response.Length);
   //     }
    }
}
