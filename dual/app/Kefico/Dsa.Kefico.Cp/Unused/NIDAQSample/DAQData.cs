using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NationalInstruments.Examples.ContAcqVoltageSamples_IntClk
{
    public struct DAQData
    {
        DateTime _timeStamp;
        double _d0, _d1, _d2, _d3;

        public DAQData(DateTime time, double d0, double d1, double d2, double d3)
        {
            _timeStamp = time;
            _d0 = d0;
            _d1 = d1;
            _d2 = d2;
            _d3 = d3;
        }
    }
}
