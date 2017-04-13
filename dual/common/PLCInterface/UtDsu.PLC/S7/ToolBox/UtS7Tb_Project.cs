using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DotNetSiemensPLCToolBoxLibrary.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetSiemensPLCToolBoxLibrary.DataTypes.Blocks.Step7V5;
using DotNetSiemensPLCToolBoxLibrary.PLCs.S7_xxx.MC7;

namespace UtDsu.PLC.S7.ToolBox
{
    partial class UtS7Tb
    {
        [TestMethod]
        public void TestMethodBlockToAWL()
        {
            List<PLCBlockName> fbs = _connection.PLCListBlocks2(PLCBlockType.FB);
            Trace.WriteLine($"There are {fbs.Count} FB Blocks.");

            string fbName = fbs.First().ToString();
            byte[] mc7Code = _connection.PLCGetBlockInMC7(fbName);
            S7Block awlBlock = MC7Converter.GetAWLBlock(mc7Code, 0);
            Trace.WriteLine(awlBlock);
        }

    }
}
