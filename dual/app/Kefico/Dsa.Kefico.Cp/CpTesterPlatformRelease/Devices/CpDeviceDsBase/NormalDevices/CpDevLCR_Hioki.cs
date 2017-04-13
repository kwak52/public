using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev.Interface;
using static Dsu.Driver.Hioki;
using System.Threading;
using Dsu.Driver;
using System.Reflection;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevLCR_Hioki : CpDevNormalBase, ILCRMeter
    {
        public string CONTROLLER_ADDRESS { get; set; }
        public double L { get; set; }
        public double C { get; set; }
        public double R { get; set; }
        public int LOAD { get; set; } = 1;
        
        private LCRMeterManager lcr() => Hioki.manager();

        public override bool DevClose()
        {
            lcr().Close();
            return true;
        }

        public override bool DevOpen()
        {
            var oResult = TryFunc(() =>
            {
                Hioki.createManager(CONTROLLER_ADDRESS, 3500);
                DevReset();
                return true;
            });

            if (oResult.HasException || oResult.Result == false)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Open a LCR Control", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                return false;
            }

            return oResult.Result;
        }



        public override bool DevReset()
        {
            if (lcr() == null)
                return false;

            lcr().Level = "V";
            lcr().Mode = "LCR";
            lcr().Speed = "MEDIUM";
            lcr().Trigger = "INTERNAL";
            lcr().Format = "ASCii";
            lcr().Display1 = "CS";
            lcr().Display2 = "LS";
            lcr().Display3 = "RDC";
            lcr().Display4 = "OFF";
            lcr().MeasureItem = "72,64,0"; //MEASure 대상 설정 Default " Cs, Ls, Rdc" = "72,64,0"
            lcr().Reset = "RST";
            return true;
        }


        private bool UpdateLCR()
        {
            if (lcr() == null)
                return false;

            string rx = lcr().MEASure; //MeasureItem = "72,64,0" 일 경우 사용

            if (rx.Split(',').Count() == 3)
            {
                C = Convert.ToDouble(rx.Split(',')[0]);
                L = Convert.ToDouble(rx.Split(',')[1]);
                R = Convert.ToDouble(rx.Split(',')[2]);
            }
            else if (rx.Split(',').Count() == 2)  
            {
                C = Convert.ToDouble(rx.Split(',')[0]);
                R = Convert.ToDouble(rx.Split(',')[1]);
            }

            Console.WriteLine(string.Format("Cs {0}, Ls {1}, Rdc {2}", C, L, R));

            return true;
        }

        public bool LoadSettingFile(int LoadID)
        {
            lcr().Load = LoadID;
            return true;
        }

        public double GetInductance() { if (!UpdateLCR()) return 0.0; else return L; }
        public double GetCapicatance() { if (!UpdateLCR()) return 0.0; else return C; }
        public double GetResistance() { if (!UpdateLCR()) return 0.0; else return R; }

        public bool SetSettingFile(int LoadID)
        {
            LOAD = LoadID;
            lcr().Load = LoadID;
            return true;
        }

        public int GetSettingFile()
        {
            //test ahn 실제 장비 Load 데이터 취득    lcr().Load GET 구현 대기
            return LOAD;
        }
    }
}
