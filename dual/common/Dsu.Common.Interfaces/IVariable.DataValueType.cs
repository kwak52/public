using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// General Data value type.
    /// see ObjectValueParser
    /// </summary>
    [ComVisible(true)]
    public enum DataValueType
    {
        /// <summary> Bit type.  Same as Boolean </summary>
        [Description("Bit")] Bit,
        /// <summary> Byte </summary>
        [Description("Byte")] Byte,
        /// <summary> /// Signed two bytes int (short) /// </summary> 
        [Description("Word")] Word,
        /// <summary> /// Signed four bytes int (int) /// </summary> 
        [Description("DWord")] DWord,
        /// <summary> Signed Byte </summary>
        [Description("singed byte")] SByte,
        /// <summary> Unsigned short </summary>
        [Description("unsigned short")] UShort,
        /// <summary> Unsigned int </summary>
        [Description("unsigned int")] UInt,
        /// <summary> Long.  Int64</summary>
        [Description("long")] Long,
        /// <summary> Unsigned long. UInt64</summary>
        [Description("unsigned long")] ULong,
        /// <summary> float </summary>
        [Description("float")] Float,
        /// <summary> double </summary>
        [Description("double")] Double,
        /// <summary> string </summary>
        [Description("string")] String,
        /// <summary> user defined</summary>
        [Description("user defined")] UserDefined,
        /// <summary> Bit type.  Same as Boolean </summary>
        [Description("bool")] Boolean=Bit,
        /// <summary> Short </summary>
        [Description("short")] Short=Word,
        /// <summary> int </summary>
        [Description("int")] Int=DWord,
        /// <summary> unknown data type</summary>
        [Description("unknown")] Unknown,
    }


    /// <summary>
    /// DataValueType 에 대한 helper
    /// </summary>
    public static class EmDataValueTypeHelper
    {
        /// <summary>
        /// DataValueType 의 크기 반환.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetSizeInBytes(this DataValueType type)
        {
            switch (type)
            {
                case DataValueType.Bit:
                case DataValueType.Byte:
                case DataValueType.SByte:
                    return 1;
                case DataValueType.Short:
                case DataValueType.UShort:
                    return 2;

                case DataValueType.Int:
                case DataValueType.UInt:
                case DataValueType.Long:
                case DataValueType.ULong:
                case DataValueType.Float:
                case DataValueType.Double:
                    return 4;

                default:
                    return -1;
            }
        }

        /// <summary> DotNet type 을 DataValueType 으로 변환 </summary>
        public static DataValueType GetDataValueType(this Type t)
        {
            if (t == typeof(bool)) return DataValueType.Bit;
            if (t == typeof(byte)) return DataValueType.Byte;
            if (t == typeof(short)) return DataValueType.Word;
            if (t == typeof(int)) return DataValueType.DWord;
            if (t == typeof(sbyte)) return DataValueType.SByte;
            if (t == typeof(short)) return DataValueType.Short;
            if (t == typeof(int)) return DataValueType.Int;
            if (t == typeof(long)) return DataValueType.Long;
            if (t == typeof(ulong)) return DataValueType.ULong;
            if (t == typeof(float)) return DataValueType.Float;
            if (t == typeof(double)) return DataValueType.Double;
            if (t == typeof(string)) return DataValueType.String;

            throw new NotSupportedException(String.Format("Type {0} not supported for variable data", t.Name));
            // return DataValueType.None;
        }


        /// <summary> 주어진 DataValueType 에 맞는 default value instance 생성 </summary>
        public static object CreateObject(this DataValueType type)
        {
            switch (type)
            {
                case DataValueType.Bit: return new bool();
                case DataValueType.Byte: return new byte();
                case DataValueType.Word: return new short();
                case DataValueType.DWord: return new int();
                case DataValueType.SByte: return new sbyte();
                case DataValueType.UShort: return new ushort();
                case DataValueType.UInt: return new uint();
                case DataValueType.Long: return new long();
                case DataValueType.ULong: return new ulong();
                case DataValueType.Float: return new float();
                case DataValueType.Double: return new double();
                case DataValueType.String: return "";
                default:
                    throw new NotSupportedException(type.ToString());
            }
        }
    }
}