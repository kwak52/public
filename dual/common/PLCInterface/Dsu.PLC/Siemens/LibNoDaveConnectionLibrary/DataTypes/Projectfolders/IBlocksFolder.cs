﻿using System.Collections.Generic;
using DotNetSiemensPLCToolBoxLibrary.DataTypes.Blocks;

namespace DotNetSiemensPLCToolBoxLibrary.DataTypes.Projectfolders
{
    internal interface IBlocksFolder
    {
        List<ProjectBlockInfo> readPlcBlocksList();

        List<ProjectBlockInfo> BlockInfos { get; }

        Block GetBlock(string BlockName);
        Block GetBlock(ProjectBlockInfo blkInfo);
    }
}
