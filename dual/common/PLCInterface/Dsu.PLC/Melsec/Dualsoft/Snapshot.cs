using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dsu.Common.Utilities;

namespace Dsu.PLC.Melsec.Dualsoft
{
    /// <summary>
    /// PLC snapshot : 사용자 파일만 포함하고, 시스템 파일은 제외된다.
    /// </summary>
    internal class Snapshot
    {
        public List<FileSnapshot> FileSnapshots { get; } = new List<FileSnapshot>();

        /// <summary>
        /// 현재 PLC 의 snapshot 을 생성한다.
        /// </summary>
        /// <param name="connection"> PLC connection </param>
        /// <returns></returns>
        public static Snapshot CreateSnapshot(McProtocolApp connection)
        {
            Snapshot snapshot = new Snapshot();
            connection.CloseFile(0, CloseType.ForceClose2);
            IEnumerable<FileInfo> fileInfos = connection.ReadDirectoryInformation().Where(fi => fi.Readable);
            foreach (var fi in fileInfos)
            {
                var bytes = connection.ReadFile(fi.Name);
                snapshot.FileSnapshots.Add(new FileSnapshot(fi, bytes));
            }
            connection.CloseFile(0, CloseType.ForceClose2);

            return snapshot;
        }

        public void Serialize(string dirPath)
        {
            using (new CwdChanger(dirPath))
            {
                foreach (var ss in FileSnapshots)
                {
                    var path = Path.Combine(dirPath, ss.Name);
                    File.WriteAllBytes(path, ss.FileContents);
                    File.SetCreationTime(path, ss.CreatedTime);
                    File.SetLastWriteTime(path, ss.CreatedTime);
                }
            }
        }
    }
}
