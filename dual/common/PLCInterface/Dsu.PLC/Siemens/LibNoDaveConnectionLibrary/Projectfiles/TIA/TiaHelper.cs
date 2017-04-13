﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNetSiemensPLCToolBoxLibrary.Projectfiles.TIA
{
    internal class TiaHelper
    {
        public static bool IsMarker(Stream s)
        {
            var arr = new byte[20];
            var r=s.Read(arr, 0, arr.Length);
            s.Seek(-1*arr.Length, SeekOrigin.Current);
            if (arr[1] == '#' && arr[2] == '#')
                return true;
            if (arr[1] == '$' && arr[2] == '$')
                return true;

            return false;
        }
    }
}
