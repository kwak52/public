using CpTesterPlatform.CpTesterSs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CpTesterPlatform.CpTester
{
    

    public class CpInfo
    {
        public CpInfo() { }
        ///  a, b, c, d select line
        public static string MarkingLine { get; set; }
        public static int MarkingDaySkipHour { get; set; }
        public static List<PLCResult> PlcMap { get; set; }

    }
}
