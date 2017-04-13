// http://rextester.com/discussion/LMCV31603/Gzip-string-and-back

using System;
using System.Text;
using System.IO;
using System.IO.Compression;


namespace Dsu.Common.Utilities.Core.ExtensionMethods
{
    public static class EmZip
    {
        public static byte[] ZippedBytesFromFile(string path)
        {
            var bytes = System.IO.File.ReadAllBytes(path);
            return Compress(bytes);
        }

        public static void ZippedBytesToFile(string path, byte[] gzBuffer)
        {
            var bytes = Decompress(gzBuffer);
            System.IO.File.WriteAllBytes(path, bytes);
        }
        public static byte[] Compress(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            return Compress(buffer);
        }
        public static byte[] Compress(this byte[] buffer)
        {
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return gzBuffer;
        }

        public static string ToBase64String(byte[] gzBuffer) => Convert.ToBase64String(gzBuffer);
        public static byte[] FromBase64String(string compressedText) => Convert.FromBase64String(compressedText);

        public static byte[] Decompress(this byte[] gzBuffer)
        {
            if (gzBuffer == null || gzBuffer.Length == 0)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return buffer;

                //return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}
