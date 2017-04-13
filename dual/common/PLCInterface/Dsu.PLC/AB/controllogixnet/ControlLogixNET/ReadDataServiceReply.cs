using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Dsu.Common.Utilities;
using Dsu.PLC.AB.controllogixnet.LocalModification;
using EIPNET;
using EIPNET.EIP;
using EIPNET.CIP;

namespace ControlLogixNET
{
    internal class ReadDataServiceReply
    {
        public byte Service { get; internal set; }
        public byte Reserved { get; internal set; }
        public ushort Status { get; internal set; }
        public byte[] ByteStatus { get; internal set; }
        public ushort DataType { get; internal set; }
        public byte[] Data { get; internal set; }

        protected void BuildDataServiceReply(EncapsReply reply, bool newVersion)
        {
            //First we have to get the data item...
            EncapsRRData rrData = new EncapsRRData();

            CommonPacket cpf = new CommonPacket();
            int temp = 0;
            rrData.Expand(reply.EncapsData, 0, out temp);
            cpf = rrData.CPF;

            //The data item contains the information in an MR_Response
            MR_Response response = new MR_Response();
            response.Expand(cpf.DataItem.Data, newVersion ? 0 : 2, out temp);

            Service = response.ReplyService;
            DataType = 0x0000;

            Status = response.GeneralStatus;

            byte[] bbTemp = new byte[4];
            Buffer.BlockCopy(BitConverter.GetBytes(Status), 0, bbTemp, 0, 2);

            if (Status == 0xFF)
            {
                if (response.AdditionalStatus_Size > 0)
                    Buffer.BlockCopy(response.AdditionalStatus, 0, bbTemp, 2, 2);
            }

            ByteStatus = bbTemp;

            //Now check the response code...
            if (response.GeneralStatus != 0 && response.GeneralStatus != 0x06)
                return;

            if (response.ResponseData != null)
            {
                if (Service == (byte)ControlNetService.MultipleServicePacketResponse )
                {
                    Debug.Assert(this is ReadMultipleDataServiceReply);
                    Console.WriteLine("aaa");
                    // todo : multiple service packet 결과로부터 개별 결과 추출
                    Data = response.ResponseData;
                }
                else
                {
                    //Now we suck out the data type...
                    // CIPType : BOOL(0xC1), SINT(0xC2), INT(0xC3), DINT(0xC4), LINT(0xC5), REAL(0xCA), DWORD(0xD3)
                    DataType = BitConverter.ToUInt16(response.ResponseData, 0);
                    byte[] tempB = new byte[response.ResponseData.Length - 2];
                    Buffer.BlockCopy(response.ResponseData, 2, tempB, 0, tempB.Length);
                    Data = tempB;
                }
            }
        }


        public ReadDataServiceReply(EncapsReply reply, bool newVersion=false)
        {
            BuildDataServiceReply(reply, newVersion);
        }
    }
}
