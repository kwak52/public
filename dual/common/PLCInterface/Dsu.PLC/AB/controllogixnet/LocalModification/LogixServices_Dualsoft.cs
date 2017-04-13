using System;
using System.Collections.Generic;
using System.Linq;
using Dsu.PLC.AB;
using Dsu.PLC.AB.controllogixnet.LocalModification;
using Dsu.PLC.Common;
using EIPNET;
using EIPNET.CIP;
using EIPNET.EIP;
using LanguageExt;

namespace ControlLogixNET
{
    internal partial class LogixServices
    {
        private static IEnumerable<byte> ConcatMultipleServicePacket(IEnumerable<byte[]> IOIs)
        {
            if (IOIs == null || !IOIs.Any())
                yield break;

            // Number of services
            int nos = (ushort)IOIs.Count();
            foreach (var b in BitConverter.GetBytes((ushort)nos))
                yield return b;

            // 각 service 의 offset : Array of byte offsets to the start of each embedded service in the Service List.
            int offset = 2 + nos * 2;
            foreach (var b in BitConverter.GetBytes((ushort)offset))
                yield return b;

            foreach (var IOI in IOIs.Take(nos - 1))
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


        public static ReadDataServiceReply ReadLogixData(SessionInfo si, string tagAddress, ushort elementCount)
        {
            UnconnectedSend ucmm = new UnconnectedSend();
            ucmm.RequestPath = CommonPaths.ConnectionManager;
            ucmm.RoutePath = LogixProcessor.TheInstance.Path;
            ucmm.Priority_TimeTick = si.ConnectionParameters.PriorityAndTick;
            ucmm.Timeout_Ticks = si.ConnectionParameters.ConnectionTimeoutTicks;
            ucmm.MessageRequest = new MR_Request();
            ucmm.MessageRequest.Service = (byte)ControlNetService.CIP_ReadDataFragmented;       // 0x52;        ControlNetService.CIP_ReadData also works
            ucmm.MessageRequest.Request_Path = IOI.BuildIOI(tagAddress); // == new byte[] { 0x91, LengthObBytes, <bytes representation for address> }
            ucmm.MessageRequest.Request_Data = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };

            EncapsRRData rrData = new EncapsRRData();
            rrData.CPF = new CommonPacket();
            rrData.CPF.AddressItem = CommonPacketItem.GetNullAddressItem();
            rrData.CPF.DataItem = CommonPacketItem.GetUnconnectedDataItem(ucmm.Pack());

            EncapsReply reply = si.SendRRData(rrData.CPF.AddressItem, rrData.CPF.DataItem);


            if (reply == null)
                return null;

            if (reply.Status != 0 && reply.Status != 0x06)
            {
                //si.LastSessionError = (int)reply.Status;
                return null;
            }

            return new ReadDataServiceReply(reply, newVersion: true);
        }

		public static Try<ReadMultipleDataServiceReply> TryReadLogixData(AbConnection connection) => () => ReadLogixData(connection);

		/// <summary>
		/// Multiple Service Packet 을 이용해서 한꺼번에 여러개의 tag 를 읽는다.
		/// </summary>
		/// <param name="si"></param>
		/// <param name="tagAddress"></param>
		/// <returns></returns>
		public static ReadMultipleDataServiceReply ReadLogixData(AbConnection connection)
        {
            var tags = connection.Tags;
            lock(tags)   // key/value pair 를 순서 고정시키기 위해서 사용
            {
                SessionInfo si = connection.LogixProcessor.SessionInfo;
                List<byte[]> mspInfos = new List<byte[]>();  // multiple service packets
                foreach (var tagAddress in tags.Keys)
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

                EncapsReply reply = si.SendRRData(rrData.CPF.AddressItem, rrData.CPF.DataItem);

                if (reply == null)
	                throw new PlcExceptionRead("No reply from PLC");

                if (reply.Status != 0 && reply.Status != 0x06)
                {
					//si.LastSessionError = (int)reply.Status;
					throw new PlcExceptionRead($"Unexpected status code : {reply.Status}");
				}

				var multiReply = new ReadMultipleDataServiceReply(reply);

                int i = 0;
                foreach (var k in tags.Keys)
                {
                    var tag = (AbTag)tags[k];
                    tag.Timestamp = DateTime.Now;
                    if ( ! multiReply[i].Value.Equals(tag.Value))
                    {
                        var x = tag.TagInfo.CIPType;
                        tag.Value = multiReply[i].Value;
                    }


                    i++;
                }

                return multiReply;
            }
        }
    }
}