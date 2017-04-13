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
    public class CpFnDCITRG : CpTsMacroMsDCTrgShell, IDCITRG
    {
        private CpInstLoadValue m_eShuntValue = CpInstLoadValue.NONE;
        public CpInstLoadValue ShuntValue
        {
            get { return m_eShuntValue; }
            set { m_eShuntValue = value; }
        }

        public CpFnDCITRG()
        {
            this.STDFnMacroType = CpDefineTStepMacroType.ANL_MEASURE;
            this.ResultLog = new ClsRlMeasuring(TsResult.NONE, TsResult.NONE);
        }

        public CpInstLoadValue getShuntValue()
        {
            return ShuntValue;
        }

        public void setShuntValue(CpInstLoadValue eLoadValue)
        {
            ShuntValue = eLoadValue;
        }
    }
}
