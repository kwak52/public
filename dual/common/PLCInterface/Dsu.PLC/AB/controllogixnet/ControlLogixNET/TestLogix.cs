using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EIPNET;
using EIPNET.EIP;
using EIPNET.CIP;

namespace ControlLogixNET
{
    internal class TestLogix
    {
        public static IEnumerable<byte> ConcatMultipleServicePacket(IEnumerable<byte[]> IOIs)
        {
            if (IOIs == null || ! IOIs.Any())
                yield break;

            // Number of services
            int nos = (ushort)IOIs.Count();
            foreach (var b in BitConverter.GetBytes((ushort)nos))
                yield return b;

            // 각 service 의 offset : Array of byte offsets to the start of each embedded service in the Service List.
            int offset = 2 + nos * 2;
            foreach (var b in BitConverter.GetBytes((ushort)offset))
                yield return b;
            foreach (var IOI in IOIs.Take(nos-1))
            {
                offset += 4;        // for offset and {0x4c, length/2}
                offset += (ushort)IOI.Count();
                foreach (var b in BitConverter.GetBytes((ushort)offset))
                    yield return b;
            }

            foreach (var IOI in IOIs)
            {
                yield return (byte)ControlNetService.CIP_ReadData;      // 0x4C
                yield return (byte)(IOI.Length / 2);

                foreach (var b in IOI)
                    yield return b;

                // command specific data : 0x01, 0x00
                yield return 0x01;
                yield return 0x00;
            }
        }

        public static void BuildLogixReadDataRequest(LogixProcessor processor, IEnumerable<string> tagAddresses)
        {

            List<byte[]> mspInfos = new List<byte[]>();  // multiple service packets
            foreach (var tagAddress in tagAddresses)
                mspInfos.Add(IOI.BuildIOI(tagAddress));

            var body = ConcatMultipleServicePacket(mspInfos);

            ReadDataServiceRequest request = new ReadDataServiceRequest();
            UnconnectedSend ucmm = new UnconnectedSend();
            ucmm.RequestPath = CommonPaths.ConnectionManager;
            ucmm.RoutePath = LogixProcessor.TheInstance.Path;
            ucmm.Priority_TimeTick = 0x05;  // SOME random :  si.ConnectionParameters.PriorityAndTick;
            ucmm.Timeout_Ticks = 0x99;      // SOME random :  si.ConnectionParameters.ConnectionTimeoutTicks;
            ucmm.MessageRequest = new MR_Request();
            ucmm.MessageRequest.Service = (byte)ControlNetService.MultipleServicePacket;       // 0x0A;
            ucmm.MessageRequest.Request_Path = CommonPaths.Router;                              // { 0x20, 0x02, 0x24, 0x01 };
            ucmm.MessageRequest.Request_Data = body.ToArray();

            EncapsRRData rrData = new EncapsRRData();
            rrData.CPF = new CommonPacket();
            rrData.CPF.AddressItem = CommonPacketItem.GetNullAddressItem();
            rrData.CPF.DataItem = CommonPacketItem.GetUnconnectedDataItem(ucmm.Pack());

            SessionInfo si = processor.SessionInfo;
            EncapsReply reply = si.SendRRData(rrData.CPF.AddressItem, rrData.CPF.DataItem);

            if (reply == null)
                throw new Exception("ERROR");

            if (reply.Status != 0 && reply.Status != 0x06)
            {
                //si.LastSessionError = (int)reply.Status;
                throw new Exception("ERROR");
            }

            var xx = new ReadDataServiceReply(reply, newVersion: true);




            var y = ucmm.Pack();
            var x = body;
        }
    }
}
