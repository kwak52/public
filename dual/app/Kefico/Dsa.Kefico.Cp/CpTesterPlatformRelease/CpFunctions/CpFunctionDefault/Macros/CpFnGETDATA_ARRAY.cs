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
    public class CpFnGETDATA_ARRAY : CpTsMacroShell, IGETDATA_ARRAY
    {
        public CpFnGETDATA_ARRAY()
        {
            this.STDFnMacroType = CpDefineTStepMacroType.ANL_MEASURE;
            this.ResultLog = new ClsRlMeasuring(TsResult.NONE, TsResult.NONE);
        }
    }
}
