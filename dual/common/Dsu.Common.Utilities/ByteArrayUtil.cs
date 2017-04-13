/*
 * See System.BitConverter
 *  BitConverter.GetBytes(f4)
 *  BitConverter.GetBytes(n2)
 *  BitConverter.ToSingle(bytes, nStartByte)
 *  BitConverter.ToInt32(bytes, nStartByte)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Dsu.Common.Utilities
{
    public static class ByteArrayUtil
    {
        public static byte[] ToBytes(object obj)
        {
            Type t = obj.GetType();
            if (t == typeof (byte))
                return new byte[] {(byte) obj};

            if (t == typeof(SByte))
                return new byte[] { (byte)(SByte)obj };

            if (t == typeof(Int16))
                return BitConverter.GetBytes((Int16)obj).ToArray();
            if (t == typeof(UInt16))
                return BitConverter.GetBytes((UInt16)obj).ToArray();
            if (t == typeof(Int32))
                return BitConverter.GetBytes((Int32)obj).ToArray();
            if (t == typeof(UInt32))
                return BitConverter.GetBytes((UInt32)obj).ToArray();
            if (t == typeof(Int64))
                return BitConverter.GetBytes((Int64)obj).ToArray();
            if (t == typeof(UInt64))
                return BitConverter.GetBytes((UInt64)obj).ToArray();
            if (t == typeof(float))
                return BitConverter.GetBytes((float)obj).ToArray();

            if (t == typeof (bool))
                return new byte[] {(byte) ((bool) obj ? 1 : 0)};

            if (t == typeof (string))
                return ToBytes((string) obj);

            if (t == typeof(byte[]))
            {
                byte[] objBytes = (byte[])obj;
                byte[] bytes = new byte[objBytes.Length];
                objBytes.CopyTo(bytes, 0);
                return bytes;
            }

            Debug.Assert(false);

            return null;
        }

        public static byte[] StringArrayToBytes(string[] strings, int nMaxBytesInAString)
        {
            int nItems = strings.Length;
            List<byte> lstBytesW = new List<byte>();
            foreach (var str in strings)
            {
                byte[] bytes = ToBytes(str).Take(nMaxBytesInAString).ToArray();
                lstBytesW.AddRange(bytes);
                int nFill = nMaxBytesInAString - bytes.Length;
                for (int i = 0; i < nFill; i++)
                    lstBytesW.Add(0);
            }

            byte[] bytesW = lstBytesW.ToArray();
            return bytesW;
        }


        public static byte[] ToBytes(IntPtr hBuffer, int length)
        {
            byte[] bytes = new byte[length];
            unsafe
            {
                Byte* pBuffer = (Byte*)hBuffer;
                for (int i = 0; i < length; i++)
                    bytes[i] = pBuffer[i];
            }
            return bytes;
        }

        public static byte[] ToBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }



        public static UInt16 ToUint16(byte[] bytes)
        {
            //return BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0);
            return (UInt16)(                  
                (bytes[0] << 8) +
                (bytes[1] << 0));
        }

        public static UInt32 ToUint32(byte[] bytes)
        {
            return (UInt32)(
                (bytes[0] << 24) +
                (bytes[1] << 16) +
                (bytes[2] << 8) +
                (bytes[3] << 0));
        }

        public static UInt64 ToUint64(byte[] bytes)
        {
            return (UInt64)(
                (bytes[0] << 56) +
                (bytes[1] << 48) +
                (bytes[2] << 40) +
                (bytes[3] << 32) +
                (bytes[4] << 24) +
                (bytes[5] << 16) +
                (bytes[6] << 8) +
                (bytes[7] << 0));
        }


        public static UInt16[] ToUint16s(byte[] bytes)
        {
            Debug.Assert(bytes.Length % 2 == 0);
            int nWords = bytes.Length / 2;
            UInt16[] vec = new UInt16[nWords];
            for (int i = 0; i < nWords; i++)
                vec[i] = (UInt16)(
                    (bytes[i * 2 + 1] << 8) + 
                    (bytes[i * 2 + 0] << 0) );
            return vec;
        }

        public static IntPtr FillBytePtrBuffer(IntPtr hBuffer, byte[] bytes)
        {
            unsafe
            {
                byte* pBuffer = (byte*)hBuffer;
                for (int i = 0; i < bytes.Length; i++)
                    pBuffer[i] = bytes[i];

                return IntPtrUtil.IncrementIntPtr(hBuffer, bytes.Length);
            }
        }

        public static IntPtr FillBytePtrBufferWithObject(IntPtr hBuffer, object obj)
        {
            byte[] bytes = ToBytes(obj);
            if (bytes == null)
                return (IntPtr)0;

            return FillBytePtrBuffer(hBuffer, bytes);
        }

        public static string ToString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        public static IntPtr FillBufferWithSizedString(IntPtr hBuffer, string str, int nLength)
        {
            byte[] bytes = ToBytes(str).Take(nLength+1).ToArray();
            bytes[nLength] = 0;
            return FillBytePtrBuffer(hBuffer, bytes);
        }
    }
}
