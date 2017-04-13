using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpCommon.ResultLog;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn;
using System;
using System.Reflection;

namespace CpTesterPlatform.Functions
{
    public class CpFnGETDATA_PULSE : CpTsMacroMsShell, IGETDATA_PULSE
    {
        public CpFnGETDATA_PULSE()
        {
            this.STDFnMacroType = CpDefineTStepMacroType.ANL_MEASURE;
            this.ResultLog = new ClsRlMeasuring(TsResult.NONE, TsResult.NONE);
        }
    }
}
