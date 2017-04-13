using System;
using System.Linq;
using System.Collections.Generic;
using EIPNET;
using EIPNET.EIP;
using EIPNET.CIP;

namespace ControlLogixNET
{
    internal class LogixServices
    {
        public static ReadDataServiceRequest BuildLogixReadDataRequest(LogixTag tag, ushort number, out int requestSize)
        {
            if (tag.TagInfo != null && tag.TagInfo.IsStructure == false && tag.TagInfo.Dimensions == 0)
            {
                //Short version
                ushort size = 0;
                switch ((CIPType)tag.TagInfo.DataType)
                {
                    case CIPType.BITS:
                    case CIPType.BOOL:
                        size = 1; break;
                    case CIPType.DINT:
                        size = 4; break;
                    case CIPType.INT:
                        size = 2; break;
                    case CIPType.LINT:
                        size = 8; break;
                    case CIPType.REAL:
                        size = 4; break;
                    case CIPType.SINT:
                        size = 1; break;
                    default:
                        return BuildLogixReadDataRequest(tag.Address, number, out requestSize);
                }

                return BuildShortReadDataRequest(tag.TagInfo.MemoryAddress, number, size, out requestSize);
            }
            else
            {
                //Long version
                return BuildLogixReadDataRequest(tag.Address, number, out requestSize);
            }
        }

        public static ReadDataServiceRequest BuildLogixReadDataRequest(string tagAddress, ushort number, out int requestSize)
        {
            byte[] newIOI = IOI.BuildIOI(tagAddress);

            int pathLen = newIOI.Length;// IOI.BuildIOI(null, tagAddress);
            ReadDataServiceRequest request = new ReadDataServiceRequest();
            request.Service = (byte)ControlNetService.CIP_ReadData;
            request.PathSize = (byte)(pathLen / 2);
            byte[] path = new byte[pathLen];
            Buffer.BlockCopy(newIOI, 0, path, 0, newIOI.Length);
            //IOI.BuildIOI(path, tagAddress);
            request.Path = path;
            request.Elements = number;

            requestSize = request.Size;

            return request;
        }

        public static ReadDataServiceRequest BuildShortReadDataRequest(uint tagInstance, ushort number, ushort size, out int requestSize)
        {
            byte[] newIOI = IOI.BuildIOI(tagInstance);

            int pathLen = newIOI.Length;
            ReadDataServiceRequest request = new ReadDataServiceRequest();
            request.Service = (byte)ControlNetService.CIP_ModifyWriteTag;   // 0x4E;// (byte)ControlNetService.CIP_ReadData;
            request.PathSize = 0x03;
            byte[] path = new byte[] { 0x20, 0xB2, 0x25, 0x00, 0x21, 0x00 };
            request.Path = path;
            request.Elements = number;

            byte[] addData = new byte[10 + newIOI.Length];
            addData[0] = 0x02; addData[1] = 0x00; addData[2] = 0x01; addData[3] = 0x01; addData[4] = 0x01;
            addData[5] = (byte)(size & 0xFF); addData[6] = (byte)((size & 0xFF00) >> 8);
            addData[7] = 0x03; addData[8] = 0x20; addData[9] = 0x6B;
            Buffer.BlockCopy(newIOI, 0, addData, 10, newIOI.Length);
            request.AddtlData = addData;

            requestSize = request.Size;

            return request;
        }

        public static ReadDataServiceRequest BuildFragmentedReadDataRequest(string tagAddress, ushort number, uint offset, out int requestSize)
        {
            byte[] newIOI = IOI.BuildIOI(tagAddress);

            int pathLen = newIOI.Length;// IOI.BuildIOI(null, tagAddress);
            ReadDataServiceRequest request = new ReadDataServiceRequest();
            request.Service = (byte)ControlNetService.CIP_ReadDataFragmented;
            request.PathSize = (byte)(pathLen / 2);
            request.IsFragmented = true;
            request.DataOffset = offset;
            byte[] path = new byte[pathLen];
            Buffer.BlockCopy(newIOI, 0, path, 0, newIOI.Length);
            //IOI.BuildIOI(path, tagAddress);
            request.Path = path;
            request.Elements = number;

            requestSize = request.Size;

            return request;
        }


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

        /// <summary>
        /// Multiple Service Packet 을 이용해서 한꺼번에 여러개의 tag 를 읽는다.
        /// </summary>
        /// <param name="si"></param>
        /// <param name="tagAddress"></param>
        /// <returns></returns>
        public static ReadDataServiceReply ReadLogixData(SessionInfo si, IEnumerable<string> tagAddresses)
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


        public static ReadDataServiceReply ReadLogixDataFragmented(SessionInfo si, string tagAddress, ushort elementCount, uint byteOffset)
        {
            int requestSize = 0;
            ReadDataServiceRequest request = BuildFragmentedReadDataRequest(tagAddress, elementCount, byteOffset, out requestSize);

            EncapsRRData rrData = new EncapsRRData();
            rrData.CPF = new CommonPacket();
            rrData.CPF.AddressItem = CommonPacketItem.GetConnectedAddressItem(si.ConnectionParameters.O2T_CID);
            rrData.CPF.DataItem = CommonPacketItem.GetConnectedDataItem(request.Pack(), SequenceNumberGenerator.SequenceNumber);
            rrData.Timeout = 2000;

            EncapsReply reply = si.SendUnitData(rrData.CPF.AddressItem, rrData.CPF.DataItem);

            if (reply == null)
                return null;

            if (reply.Status != 0 && reply.Status != 0x06)
            {
                //si.LastSessionError = (int)reply.Status;
                return null;
            }

            return new ReadDataServiceReply(reply);
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
            ucmm.MessageRequest.Request_Data = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00};

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

            return new ReadDataServiceReply(reply, newVersion:true);
        }
		
#if MONO
		public static WriteDataServiceRequest BuildLogixWriteDataRequest(string tagAddress, ushort dataType, ushort elementCount, byte[] data)
		{
			return BuildLogixWriteDataRequest(tagAddress, dataType, elementCount, data, 0x0000);
		}
#endif 
#if MONO
		public static WriteDataServiceRequest BuildLogixWriteDataRequest(string tagAddress, ushort dataType, ushort elementCount, byte[] data, ushort structHandle)
#else
        public static WriteDataServiceRequest BuildLogixWriteDataRequest(string tagAddress, ushort dataType, ushort elementCount, byte[] data, ushort structHandle = 0x0000)
#endif
		{
            byte[] newIOI = IOI.BuildIOI(tagAddress);

            int pathLen = newIOI.Length;
            WriteDataServiceRequest request = new WriteDataServiceRequest();
            request.Service = (byte)ControlNetService.CIP_WriteData;
            request.PathSize = (byte)(pathLen / 2);
            byte[] path = new byte[pathLen];
            Buffer.BlockCopy(newIOI, 0, path, 0, newIOI.Length);
            request.Path = path;
            request.Elements = elementCount;
            request.DataType = dataType;
            request.StructHandle = structHandle;
            request.Data = data;

            return request;
        }
		
#if MONO
		public static WriteDataServiceRequest BuildFragmentedWriteRequest(string tagAddress, ushort dataType, ushort elementCount, uint offset, byte[] data)
		{
			return BuildFragmentedWriteRequest(tagAddress, dataType, elementCount, offset, data, 0x0000);
		}
#endif
#if MONO
		public static WriteDataServiceRequest BuildFragmentedWriteRequest(string tagAddress, ushort dataType, ushort elementCount, uint offset, byte[] data, ushort structHandle)
#else
        public static WriteDataServiceRequest BuildFragmentedWriteRequest(string tagAddress, ushort dataType, ushort elementCount, uint offset, byte[] data, ushort structHandle = 0x0000)
#endif
		{
            byte[] newIOI = IOI.BuildIOI(tagAddress);

            int pathLen = newIOI.Length;
            WriteDataServiceRequest request = new WriteDataServiceRequest();
            request.Service = (byte)ControlNetService.CIP_WriteDataFragmented;
            request.PathSize = (byte)(pathLen / 2);
            byte[] path = new byte[pathLen];
            Buffer.BlockCopy(newIOI, 0, path, 0, newIOI.Length);
            request.Path = path;
            request.Elements = elementCount;
            request.DataType = dataType;
            request.StructHandle = structHandle;
            request.IsFragmented = true;
            request.Offset = offset;
            request.Data = data;

            return request;
        }
		
#if MONO
		public static WriteDataServiceReply WriteLogixData(SessionInfo si, string tagAddress, ushort dataType, ushort elementCount, byte[] data)
		{
			return WriteLogixData(si, tagAddress, dataType, elementCount, data, 0x0000);
		}
#endif
#if MONO
		public static WriteDataServiceReply WriteLogixData(SessionInfo si, string tagAddress, ushort dataType, ushort elementCount, byte[] data, ushort structHandle)
#else
        public static WriteDataServiceReply WriteLogixData(SessionInfo si, string tagAddress, ushort dataType, ushort elementCount, byte[] data, ushort structHandle = 0x0000)
#endif
		{
            WriteDataServiceRequest request = BuildLogixWriteDataRequest(tagAddress, dataType, elementCount, data, structHandle);

            EncapsRRData rrData = new EncapsRRData();
            rrData.CPF = new CommonPacket();
            rrData.CPF.AddressItem = CommonPacketItem.GetConnectedAddressItem(si.ConnectionParameters.O2T_CID);
            rrData.CPF.DataItem = CommonPacketItem.GetConnectedDataItem(request.Pack(), SequenceNumberGenerator.SequenceNumber);
            rrData.Timeout = 2000;

            EncapsReply reply = si.SendUnitData(rrData.CPF.AddressItem, rrData.CPF.DataItem);

            if (reply == null)
                return null;

            if (reply.Status != 0)
            {
                return null;
            }

            return new WriteDataServiceReply(reply);
        }

    }
}
