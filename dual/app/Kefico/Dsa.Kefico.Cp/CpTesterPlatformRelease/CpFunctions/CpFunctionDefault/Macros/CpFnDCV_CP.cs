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
    public class CpFnDCV_CP : CpTsMacroMsDCShell, IDCV_CP
    {
        public CpFnDCV_CP()
        {
            this.STDFnMacroType = CpDefineTStepMacroType.ANL_MEASURE;
            this.ResultLog = new ClsRlMeasuring(TsResult.NONE, TsResult.NONE);
        }
    }
}
