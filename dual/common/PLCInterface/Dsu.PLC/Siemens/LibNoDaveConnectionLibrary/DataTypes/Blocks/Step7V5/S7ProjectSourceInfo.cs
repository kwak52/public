﻿namespace DotNetSiemensPLCToolBoxLibrary.DataTypes.Blocks.Step7V5
{
    internal class S7ProjectSourceInfo : ProjectBlockInfo
    {
        public string Filename { get; set; }

        public override string ToString()
        {
            if (Deleted)
                return "$$_" + Name;
            return Name;
        }
    }
}
