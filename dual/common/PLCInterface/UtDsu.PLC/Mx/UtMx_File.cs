using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Dsu.PLC.Melsec;
using Dsu.PLC.Melsec.Dualsoft;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileInfo = Dsu.PLC.Melsec.Dualsoft.FileInfo;

namespace UtDsu.PLC
{
    partial class UtMx
    {
        private void ForceCloseAllFiles() { McProtocol.CloseFile(0, CloseType.ForceClose2); }


        [TestMethod]
        public void UtMxFileWrite()
        {
            ForceCloseAllFiles();
            //McProtocol.WriteFile(@"W:\solutions\trunk\common\PLCInterface\UtDsu.PLC\bin\Debug\MAIN.QPG");
            //return;

            McProtocol.RemoteStop();

            var files = new[] {"COMMENT.QCD", "MAIN.QPG", "PARAM.QPA", "SRCINF1I.CAB", "SRCINF2I.CAB"};
            var path = @"W:\solutions\trunk\common\PLCInterface\UtDsu.PLC\bin\Debug\";
            foreach (var f in files)
                McProtocol.WriteFile(path + f);

            //McProtocol.WriteFile(@"W:\solutions\trunk\common\PLCInterface\UtDsu.PLC\bin\Debug\X.CAB");
            McProtocol.WriteFile(@"W:\solutions\trunk\common\PLCInterface\UtDsu.PLC\bin\Debug\MAIN.QPG");

            McProtocol.RemoteRun();
        }

        [TestMethod]
        public void UtMxFileDelete()
        {
            McProtocol.DeleteFile("VERSION.TXT");
        }

        [TestMethod]
        public void UtMxFileModifyTime()
        {
            //McProtocol.ModifyFileTime("X.CAB", new DateTime(2002, 1, 1, 1, 1, 1));
            //McProtocol.ModifyFileTime("X.CAB", new DateTime(2002, 2, 2, 2, 2, 2));
            McProtocol.ModifyFileTime("X.CAB", new DateTime(2002, 12, 31, 23, 59, 59));
        }

        [TestMethod]
        public void UtMxFileReadDirectoryInformation()
        {
            ForceCloseAllFiles();
            IEnumerable<FileInfo> fileInfos = McProtocol.ReadDirectoryInformation();
            foreach (var fi in fileInfos)
            {
                var fp = McProtocol.GetFilePosition(fi.Name);
                Trace.WriteLine(fi + $", Fp={fp?.ToString()}");
            }
        }

        [TestMethod]
        public void UtMxFileReleaseLock()
        {
            ForceCloseAllFiles();
        }


        /// <summary>
        /// 권한이 허용된 file 을 읽어서 disk 저장
        /// </summary>
        [TestMethod]
        public void UtMxFileRead()
        {
            ForceCloseAllFiles();
            IEnumerable<FileInfo> fileInfos = McProtocol.ReadDirectoryInformation().Where(fi => fi.Readable);
            foreach (var fi in fileInfos)
            {
                var bytes = McProtocol.ReadFile(fi.Name);
                File.WriteAllBytes(fi.Name, bytes);
            }
            ForceCloseAllFiles();
        }

        [TestMethod]
        public void UtMxFileCreateSnapshot()
        {
            Snapshot snapshot = Snapshot.CreateSnapshot(McProtocol);
            snapshot.Serialize(@"W:\tmp");
        }
    }
}