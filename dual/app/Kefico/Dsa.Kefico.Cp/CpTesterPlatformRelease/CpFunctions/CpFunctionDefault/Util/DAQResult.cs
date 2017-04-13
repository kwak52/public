using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.Functions
{
    public class DAQResult
    {
        public double[] DATA { get; set; }
        public double MIN { get; set; }
        public double MAX { get; set; }
        public string Function { get; set; }
        public int StationIndex { get; set; }
        public DAQResult(int stationIndex, string function, double[] data, double min, double max)
        {
            DATA = data;
            MIN = min;
            MAX = max;
            Function = function;
            StationIndex = stationIndex;
        }
    }

    public class DaqChartEvent
    {
        public static Subject<DAQResult> DaqSubject = new Subject<DAQResult>();
    }
}
