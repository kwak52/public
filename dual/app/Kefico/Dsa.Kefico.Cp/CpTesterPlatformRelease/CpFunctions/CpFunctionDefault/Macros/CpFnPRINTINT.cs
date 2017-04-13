using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpCommon.ResultLog;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// Joins up multiple individual strings to form a single complete string.    
	/// </summary>    

	public class CpFnPRINTINT : CpTsMacroMsShell, IPRINTINT
    {
        public CpFnPRINTINT()
        {
            this.STDFnMacroType = CpDefineTStepMacroType.ANL_MEASURE;
            this.ResultLog = new ClsRlMeasuring(TsResult.NONE, TsResult.NONE);
		
			 
        }
    }
}
