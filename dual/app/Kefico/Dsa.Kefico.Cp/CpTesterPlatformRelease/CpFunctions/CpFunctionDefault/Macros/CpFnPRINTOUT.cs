using System;
using System.Reflection;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using static CpCommon.ExceptionHandler;
using CpTesterPlatform.CpCommon.ResultLog;

namespace CpTesterPlatform.Functions
{
    public class CpFnPRINTOUT : CpTsMacroShell, IPRINTOUT
    {
        private string m_strPrintoutString = string.Empty;
        public string PrintoutString
        {
            get { return m_strPrintoutString; }
            set { m_strPrintoutString = value; }
        }

        public CpFnPRINTOUT()
        {

        }

        public override string[] CreateMeasuringLogString()
        {
            try
            {
                string[] arstrLog = new string[Enum.GetValues(typeof(UiCpDefineEnumGridViewTStepMeasuringLog)).Length];
                arstrLog[(int)(UiCpDefineEnumGridViewTStepMeasuringLog.STEP)] = Core.StepNum.ToString();
                arstrLog[(int)(UiCpDefineEnumGridViewTStepMeasuringLog.POSITION)] = Core.Position.ToString();
                arstrLog[(int)(UiCpDefineEnumGridViewTStepMeasuringLog.MO)] = Core.GetMO();
                arstrLog[(int)(UiCpDefineEnumGridViewTStepMeasuringLog.FNC_NAME)] = Core.STDBoschName;
                arstrLog[(int)(UiCpDefineEnumGridViewTStepMeasuringLog.MEASURE)] = m_strPrintoutString;
                arstrLog[(int)(UiCpDefineEnumGridViewTStepMeasuringLog.CHECK)] = TsResult.NONE.ToString();
                return arstrLog;
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to create a string represented as log format in " + MethodBase.GetCurrentMethod().DeclaringType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
            return null;
        }

        public void setPrintoutString(string strPrint)
        {
            PrintoutString = strPrint;
        }
    }
}
