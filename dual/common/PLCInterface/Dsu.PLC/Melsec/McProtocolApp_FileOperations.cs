using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using Dsu.Common.Utilities.Core.ExtensionMethods;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Melsec.Dualsoft;
using FileInfo = Dsu.PLC.Melsec.Dualsoft.FileInfo;

namespace Dsu.PLC.Melsec
{
    partial class McProtocolApp
    {
        internal IEnumerable<FileInfo> ReadDirectoryInformation(Drive drive, int head, int numRequest)
        {
            Contract.Requires(head.InRange(1, 256));
                // see Q Corresponding MELSEC Communication Protocol Reference Manual.pdf, pp. 3-189
            Contract.Requires(numRequest.InRange(1, 36));

            // Q Corresponding MELSEC Communication Protocol Reference Manual.pdf, pp. 3-173
            var data = new byte[]
            {
                0x30, 0x30, 0x30, 0x30, // Specific value
                (byte) drive, 0x00, // Drive NO.    [0..??]
                (byte) head, 0x00, // Head file No.    [1..256]    정보 추출 최초 파일 위치
                (byte) numRequest, 0x00, // Number of file request   [1..36]  HEAD 로부터 시작해서 몇개의 파일을 읽어 올지 갯수
                0x00, 0x00, // Number of directory characters
            };

            byte[] rtData = ExecuteCommand(DeviceAccessCommand.FileReadInformation, data.ToArray());
            int offset = 0;
            byte[] btNofi = rtData.BlockCopy(ref offset, 2);
            var nofi = BitConverter.ToInt16(rtData, 0); // number of file information

            for (int i = 0; i < nofi; i++)
            {
                byte[] btFileStem = rtData.BlockCopy(ref offset, 8);
                byte[] btExtension = rtData.BlockCopy(ref offset, 3);
                byte[] btAttribute = rtData.BlockCopy(ref offset, 2);
                byte[] btSpare12 = rtData.BlockCopy(ref offset, 9);
                DateTime dt = rtData.GetFileDateTime(offset);
                offset += 4;
                byte[] btSpare3 = rtData.BlockCopy(ref offset, 2);
                byte[] btFileSize = rtData.BlockCopy(ref offset, 4);

                var attribute = BitConverter.ToInt16(btAttribute, 0);

                var stem = System.Text.ASCIIEncoding.ASCII.GetString(btFileStem, 0, 8).Trim();
                var ext = System.Text.ASCIIEncoding.ASCII.GetString(btExtension, 0, 3).Trim();
                var fileName = $"{stem}.{ext}";
                var fileSize = BitConverter.ToInt32(btFileSize, 0);

                Trace.WriteLine($"{fileName} : date={dt}");
                bool readable = btAttribute[0] == 0x01 || btAttribute[0] == 0x20;
                bool writable = btAttribute[0] == 0x20;

                yield return
                    new FileInfo(fileName) {CreatedTime = dt, Size = fileSize, Readable = readable, Writable = writable}
                    ;
            }
        }


        internal IEnumerable<FileInfo> ReadDirectoryInformation(Drive drive = Drive.BuiltIn)
        {
            int head = 1;
            const int numRequest = 36;
            while (true)
            {
                var fileInfos = ReadDirectoryInformation(drive, head, numRequest);
                if (fileInfos.IsNullOrEmpty())
                    yield break;

                foreach (var fi in fileInfos)
                {
                    yield return fi;
                }

                int read = fileInfos.Count();
                if (read <= numRequest)
                    yield break;

                head += read;
            }
        }

        public UInt16? GetFilePosition(string fileName, Drive drive = Drive.BuiltIn)
        {
            // Q Corresponding.pdf, pp. 3-175
            var data = BitConverter.GetBytes((UInt32) Password)
                    .Concat(new byte[]
                    {
                        (byte) drive, 0x00, // Drive NO.
                        0x00, 0x00, // Number of directory name character
                        (byte) fileName.Length, 0x00, // Number of file name character
                    })
                    .Concat(Encoding.ASCII.GetBytes(fileName))
                ;

            try
            {
                byte[] rtData = ExecuteCommand(DeviceAccessCommand.FileSearchInformation, data.ToArray());
                return BitConverter.ToUInt16(rtData, 0);
            }
            catch (Exception)
            {
                return null;
            }
        }


        private void CreateFile(string fileName, UInt32 fileSize, Drive drive = Drive.BuiltIn)
        {
            var data = BitConverter.GetBytes((UInt32)Password)
                .Concat(BitConverter.GetBytes((UInt16)drive))
                .Concat(BitConverter.GetBytes((UInt32)fileSize))
                .Concat(BitConverter.GetBytes((UInt16)fileName.Length))
                .Concat(Encoding.ASCII.GetBytes(fileName))
                ;

            ExecuteCommand(DeviceAccessCommand.FileCreateNew, data.ToArray());
        }

        private UInt16? OpenFile(OpenMode openMode, string fileName, Drive drive = Drive.BuiltIn)
        {
            if (openMode == OpenMode.OpenForWrite)
            {
                var posi = GetFilePosition(fileName);
                if (!posi.HasValue)
                {
                    // write mode 에서 해당 파일이 존재하지 않는 경우, 먼저 생성??
                    Trace.WriteLine("Trying to open non existing file");

                    CreateFile(fileName, 0);
                }
            }

            var data = BitConverter.GetBytes((UInt32)Password)
                .Concat(new byte[] {
                    0x00, (byte)openMode,       // == BitConverter.GetBytes((UInt16)openMode).Reverse()
                    (byte) drive, 0x00, // Drive NO.
                    (byte) fileName.Length, 0x00, // Number of file name character
                }).Concat(Encoding.ASCII.GetBytes(fileName))
                ;

            byte[] rtData = ExecuteCommand(DeviceAccessCommand.FileOpen, data.ToArray());
            return BitConverter.ToUInt16(rtData, 0);
        }

        private UInt16? OpenFileForRead(string fileName, Drive drive = Drive.BuiltIn) => OpenFile(OpenMode.OpenForRead, fileName, drive);
        private UInt16? OpenFileForWrite(string fileName, Drive drive = Drive.BuiltIn) => OpenFile(OpenMode.OpenForWrite, fileName, drive);




        public void CloseFile(UInt16 filePointer, CloseType closeType=CloseType.NormalClose)
        {
            var data = BitConverter.GetBytes((UInt16)filePointer)
                .Concat(BitConverter.GetBytes((UInt16) closeType));

            byte[] rtData = ExecuteCommand(DeviceAccessCommand.FileClose, data.ToArray());
            Debug.Assert(rtData.IsNullOrEmpty());
        }



        #region Read files
        private IEnumerable<byte> ReadFilePartial(UInt16 filePointer, UInt32 offset, UInt16 numRead)
        {
            var data = BitConverter.GetBytes((UInt16)filePointer)
                .Concat(BitConverter.GetBytes((UInt32)offset))      // Offset address
                .Concat(BitConverter.GetBytes((UInt16)numRead))     // Number of bytes read
                ;

            byte[] rtData = ExecuteCommand(DeviceAccessCommand.FileRead, data.ToArray());
            var numByteRead = BitConverter.ToUInt16(rtData, 0);
            return rtData.Skip(2);
        }

        private IEnumerable<byte> ReadFile(UInt16 filePointer, int targetReadSize)
        {
            UInt16 bytesRead = 0;
            while (bytesRead < targetReadSize)
            {
                var currentTargetSize = Math.Min(1024, targetReadSize - bytesRead);
                var bytes = ReadFilePartial(filePointer, bytesRead, (UInt16)currentTargetSize);
                foreach (var b in bytes)
                {
                    bytesRead++;
                    yield return b;
                }
            }
        }

        public byte[] ReadFile(string fileName, Drive drive = Drive.BuiltIn)
        {
            var pair = ReadFiles(new[] {fileName}, drive).First();
            Debug.Assert(pair.Key == fileName);
            return pair.Value;
        }

        public IEnumerable<KeyValuePair<string, byte[]>> ReadFiles(IEnumerable<string> fileNames, Drive drive = Drive.BuiltIn)
        {
            IEnumerable<FileInfo> fileInfos = ReadDirectoryInformation(drive).Where(fi => fileNames.Contains(fi.Name));
            foreach (var fi in fileInfos)
            {
                var handle = OpenFileForRead(fi.Name, drive);
                // todo : error handling 조건 명세
                //if (!handle.HasValue)
                //    return null;

                var bytes = ReadFile(handle.Value, fi.Size).ToArray();
                Trace.WriteLine($"Read {bytes.Length} bytes.");
                foreach (var b in bytes)
                {
                    yield return new KeyValuePair<string, byte[]>(fi.Name, bytes);
                }

            }
        }
        #endregion


        private UInt16 WritePartialFile(UInt16 filePointer, byte[] contents, UInt32 offset, UInt16 numWrite)
        {
            var data = BitConverter.GetBytes(filePointer)      // File pointer number
                .Concat(BitConverter.GetBytes((UInt32)offset))
                .Concat(BitConverter.GetBytes((UInt16)numWrite))       // Number of directory name characters
                .Concat(contents.Skip((int)offset).Take(numWrite))
                ;

            byte[] rtData = ExecuteCommand(DeviceAccessCommand.FileWrite, data.ToArray());
            var numWrittenBytes = BitConverter.ToUInt16(rtData, 0);
            return numWrittenBytes;
        }

        public bool WriteFile(string filePath, Drive drive = Drive.BuiltIn)
        {
            var fileName = Path.GetFileName(filePath);
            var handle = OpenFileForWrite(fileName, drive);
            if (!handle.HasValue)
                return false;

            UInt16 bytesWritten = 0;
            byte[] contents = File.ReadAllBytes(filePath);
            while (bytesWritten < contents.Length)
            {
                var currentTargetSize = Math.Min(1024, contents.Length - bytesWritten);
                bytesWritten += WritePartialFile(handle.Value, contents, bytesWritten, (UInt16)currentTargetSize);
            }

            CloseFile(handle.Value);
            return true;
        }


        public void ModifyFileTime(string fileName, DateTime time, Drive drive = Drive.BuiltIn)
        {
            var data = BitConverter.GetBytes((UInt32) 0)            // fixed value
                    .Concat(BitConverter.GetBytes((UInt16) drive))  // Drive NO.
                    .Concat(time.GetFileDateTime())                             // date & time
                    .Concat(BitConverter.GetBytes((UInt16) fileName.Length))    // Number of file name character
                    .Concat(Encoding.ASCII.GetBytes(fileName))
                ;

            byte[] rtData = ExecuteCommand(DeviceAccessCommand.FileModifyCreateDateTime, data.ToArray());
        }


        public void DeleteFile(string fileName, Drive drive = Drive.BuiltIn)
        {
            var data = BitConverter.GetBytes((UInt32)Password)
                    .Concat(BitConverter.GetBytes((UInt16)drive))  // Drive NO.
                    .Concat(BitConverter.GetBytes((UInt16)fileName.Length))    // Number of file name character
                    .Concat(Encoding.ASCII.GetBytes(fileName))
                ;

            byte[] rtData = ExecuteCommand(DeviceAccessCommand.FileDelete, data.ToArray());
        }

    }
}
