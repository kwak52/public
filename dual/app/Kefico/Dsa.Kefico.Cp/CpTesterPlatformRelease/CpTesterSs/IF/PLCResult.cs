using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTesterSs
{
    public class PLCResult
    {
        public double Data { get; set; }
        public bool NG { get; set; }
        public bool Skip { get; set; }
        public string DisplayName { get; set; }
        public string CPEditName { get; set; }
        public int Station { get; set; }
        public int PlcSaveIndex { get; set; }
        public int CPEditIndex { get; set; }

        public PLCResult(double data, bool ng, bool skip, string displayName, string cPEditName = "", int station = 0, int plcSaveIndex = 0, int cPEditIndex = 0)
        {
            Data = data;
            NG = ng;
            DisplayName = displayName;
            Skip = skip;
            CPEditName = cPEditName;
            Station = station;
            PlcSaveIndex = plcSaveIndex;
            CPEditIndex = cPEditIndex;
        }
    }
}
