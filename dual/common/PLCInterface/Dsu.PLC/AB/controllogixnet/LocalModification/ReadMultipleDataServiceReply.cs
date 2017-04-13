using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ControlLogixNET;
using EIPNET.CIP;
using EIPNET.EIP;

namespace Dsu.PLC.AB.controllogixnet.LocalModification
{
    internal class ReadMultipleDataServiceReply : ReadDataServiceReply
    {
        /// <summary>
        /// Multiple service reply 에 포함된 개별 reply 하나를 표현하기 위한 class
        /// </summary>
        internal class EachReply
        {
            public byte Service { get; }
            public object Value { get; }
            public ushort SuccessCode { get; }      // should be zero on success
            public ushort AdditionalSize { get; }

            public EachReply(byte service, ushort successCode, ushort additionalSize, object value)
            {
                Service = service;
                SuccessCode = successCode;
                Value = value;
                AdditionalSize = additionalSize;
            }
        }

        public EachReply this[int nthReply] => Replies[nthReply];

        public List<EachReply> Replies { get; }
        public int FailedCount = 0;

        public ReadMultipleDataServiceReply(EncapsReply reply)
            : base(reply, newVersion:true)
        {
            int offset = 0;
            int numRequest = BitConverter.ToInt16(Data, offset);
            Replies = new List<EachReply>();

            // request 갯수 만큼 2 byte 로 저장된 offset 값들을 읽어 낸다.
            List<short> lstOffsets = (from i in Enumerable.Range(0, numRequest)
                    select BitConverter.ToInt16(Data, 2 + 2*i))
                .ToList();

            Debug.Assert(numRequest == lstOffsets.Count());

            // offset 값들 만큼 skip
            offset += 2 + 2*numRequest;
            for ( int i = 0; i < numRequest; i++ )
            {
                Debug.Assert(offset == lstOffsets[i]);
                ushort service = BitConverter.ToUInt16(Data, offset);
                offset += 2;
                Debug.Assert( (service & 0x0080) != 0);     // MSB 1 for response, 0 for request
                service &= 0xFF7F;

                ushort successCode = Data[offset++];
                ushort additionalSize = Data[offset++];
                CIPType type = (CIPType) BitConverter.ToUInt16(Data, offset);
                offset += 2;

                object readValue = null;
                switch (type)
                {
                        case CIPType.BOOL:
                        readValue = Data[offset++] != 0;
                        break;

                        case CIPType.SINT:
                        readValue = Data[offset++];
                        break;

                    case CIPType.INT:
                        readValue = BitConverter.ToInt16(Data, offset);
                        offset += 2;
                        break;
                    case CIPType.DINT:
                        readValue = BitConverter.ToInt32(Data, offset);
                        offset += 4;
                        break;
                    case CIPType.LINT:
                        readValue = BitConverter.ToInt64(Data, offset);
                        offset += 8;
                        break;
                    case CIPType.REAL:
                        readValue = BitConverter.ToSingle(Data, offset);
                        offset += 4;
                        break;
                }

                Replies.Add(new EachReply((byte)service, successCode, additionalSize, readValue));
            };
        }
    }
}
