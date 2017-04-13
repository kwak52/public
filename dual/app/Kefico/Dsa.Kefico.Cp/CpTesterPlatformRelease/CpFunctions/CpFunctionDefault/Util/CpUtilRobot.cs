using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;
using Dsu.Driver.Math;
using System.IO;

namespace CpTesterPlatform.Functions
{
    public static class CpUtilRobot
    {
        public static double GetAirGapPulse(double value, bool Zaxis)
        {
            double AirGap = 0.0;

            // Z lead 5 => 2000 == 1mm
            // Y lead 10 => 1000 == 1mm

            if (Zaxis)
                AirGap = Math.Cos(7 * Math.PI / 180) * value * -2000;
            else
                AirGap = Math.Sin(7 * Math.PI / 180) * value * 1000;

            return AirGap;
        }

        public static double GetAirGapPulseUsingSingleAxis(double value)
        {
            // Z lead 5 => 2000 == 1mm
            return value * -2000;
        }

        public static double GetAirGapPulseUsingIAI(double value)
        {
            // Z  167 pulse == 1mm
            return value * -167;
        }
    }
}
