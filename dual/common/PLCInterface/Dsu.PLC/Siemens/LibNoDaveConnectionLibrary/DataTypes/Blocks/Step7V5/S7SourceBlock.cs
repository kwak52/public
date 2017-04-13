﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetSiemensPLCToolBoxLibrary.DataTypes.Blocks.Step7V5
{
    internal class S7SourceBlock:S7Block
    {
        public string Text { get; set; }

        public string Filename { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
