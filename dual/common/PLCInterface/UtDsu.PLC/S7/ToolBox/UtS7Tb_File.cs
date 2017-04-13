using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotNetSiemensPLCToolBoxLibrary.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetSiemensPLCToolBoxLibrary.DataTypes.Blocks.Step7V5;

namespace UtDsu.PLC.S7.ToolBox
{
    partial class UtS7Tb
    {
        [TestMethod]
        public void UtS7TbFileBrowseBlockInfo()
        {
#if false
            /*
             * Wireshark 으로 관찰하면, request 및 response 는 정상이다.
             * libNoDave 호출해서 reply 를 읽는 부분에 문제가 있다.
             * todo : libnodave 소스를 새로 컴파일해 보던가... => libdave compile 이 잘 안됨.
             * todo : socket 에서 받는 부분을 C# 으로 새로 코딩해 보던가..
             */
            Dictionary<PLCBlockType, int> blkInfo = _connection.PLCGetBlockCount();
            foreach (var bi in blkInfo)
            {
                Trace.WriteLine($"{bi.Key} = {bi.Value}");
            }
#endif

            List<PLCBlockName> dbBlocks2 = _connection.PLCListBlocks2(PLCBlockType.DB);
            Trace.WriteLine($"There are {dbBlocks2.Count} DB Blocks.");


            List<PLCBlockName> allBlocks = _connection.PLCListBlocks2(PLCBlockType.AllBlocks);
            Trace.WriteLine($"There are total {allBlocks.Count} Blocks.");


            List<string> dbBlocks = _connection.PLCListBlocks(PLCBlockType.DB);
            foreach (var blockName in dbBlocks)
            {
                S7Block dbBlock = _connection.PLCGetBlockHeader(blockName);
                Trace.WriteLine(
                    $"{dbBlock} : title={dbBlock.Title}, author={dbBlock.Author}, family={dbBlock.Family}, last-code-change={dbBlock.LastCodeChange}, codeSize = {dbBlock.CodeSize} ");
            }
            Trace.WriteLine($"There are {dbBlocks.Count} DB Blocks.");
        }

        [TestMethod]
        public void UtS7TbFileDownload()
        {
            // gets block names : blks = {OB, FC, FB, DB, SDB}n  e.g. {"OB1", ..., "FC1", ..., "FB7", ..., "DB1", ..., "SDB1", ...}
            var blks = _connection.PLCListBlocks(PLCBlockType.AllEditableBlocks);
            blks.ForEach(b => Trace.WriteLine(b));

            foreach (var blk in blks)
            {
                byte[] contents = _connection.PLCGetBlockInMC7(blk);
                File.WriteAllBytes($"{blk}.blk", contents);
            }


            // _connection.PLCPutBlockFromMC7toPLC(strBlockName, blockBytes);
            // _connection.PLCDeleteBlock(strBlockName);
        }
    }
}
