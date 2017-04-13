using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Dsu.PLC.Melsec.Dualsoft
{
    /// <summary>
    /// MC protocol 을 통해 가져온 file 정보에서 date time 분석.
    /// see Q Corresponding MELSEC Communication Protocol Reference Manual.pdf, pp. 3-153 ~ 3-154
    /// </summary>
    internal static class FileInfoExtractor
    {
        #region Date time
        public static byte[] GetFileDateTime(this DateTime dt)
        {
            UInt16 y = (UInt16)((dt.Year - 1980) << 9);
            UInt16 m = (UInt16)(dt.Month << 5);
            UInt16 d = (UInt16)(dt.Day << 0);

            UInt16 H = (UInt16)(dt.Hour << 11);
            UInt16 M = (UInt16)(dt.Minute << 5);
            UInt16 S = (UInt16)(dt.Second/2);

            return BitConverter.GetBytes((UInt16)(y | m | d))
                .Concat(BitConverter.GetBytes((UInt16)(H | M | S)))
                .ToArray();
        }

        public static DateTime GetFileDateTime(this byte[] dateTime, int startIndex = 0)
        {
            Contract.Requires(dateTime.Length == 4);
            var time = GetFileTime(dateTime, startIndex + 0);
            var date = GetFileDate(dateTime, startIndex + 2);
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        public static DateTime GetFileDate(byte[] date, int startIndex=0)
        {
            Contract.Requires(date.Length == 2);
            return GetFileDate(BitConverter.ToUInt16(date, startIndex));
        }

        public static DateTime GetFileDate(ushort date)
        {
            int y = ((date & 0xFE00) >> 9) + 1980;      // 7 bit
            int m = (date & 0x01E0) >> 5;
            int d = (date & 0x001F) >> 0;
            return new DateTime(y, m, d);
        }

        public static DateTime GetFileTime(byte[] time, int startIndex = 0)
        {
            Contract.Requires(time.Length == 2);
            return GetFileTime(BitConverter.ToUInt16(time, startIndex));
        }

        public static DateTime GetFileTime(ushort time)
        {
            int h = (time & 0xF800) >> 11;
            int m = (time & 0x07E0) >> 5;
            int s = (time & 0x001F) * 2;
            var dt = DateTime.Now;
            return new DateTime(dt.Year, dt.Month, dt.Day, h, m, s);
        }
        #endregion
    }
}
