using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNetSiemensPLCToolBoxLibrary.Projectfiles.TIA
{
    internal static class BinaryReaderExtensions
    {
        public static TiaObjectId ReadTiaObjectId(this BinaryReader rd)
        {
            return new TiaObjectId(rd.ReadInt32(), rd.ReadInt64());
        }
    }
}
