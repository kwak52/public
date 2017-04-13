using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.Functions
{
	public class CpFnP_SPLIT_STR_CP : CpTsShell, IP_SPLIT_STR_CP
    {
		protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            var tResult = TryFunc(() =>
            {
                PsCCSStdFnModuleP_SPLIT_STR_CP psModuleP_ZERLEGE_STR = this.Core as PsCCSStdFnModuleP_SPLIT_STR_CP;
                Debug.Assert(psModuleP_ZERLEGE_STR != null);

				string strDataDevID = psModuleP_ZERLEGE_STR.DeviceID;
                string strDataInput = psModuleP_ZERLEGE_STR.INPUT_STRING;
				string strDataStart = psModuleP_ZERLEGE_STR.ZEIGER_ANF;
                string strDataEnd = psModuleP_ZERLEGE_STR.ZEIGER_END;
                string strResult = psModuleP_ZERLEGE_STR.ERGEBNIS;
                string strError = psModuleP_ZERLEGE_STR.ERRORFLAG;

				if(iMngStation.MngTStep.GetMngGv().IsKindOfVariable(strDataInput))
					strDataInput = (string) iMngStation.MngTStep.GetMngGv().GetValue(strDataInput).RawValue;

				int nDataStart = Convert.ToInt32(strDataStart);
				int nDataEnd = Convert.ToInt32(strDataEnd);
								
				if(cpTsParent is IBLOCKINT_CP)
				{
					string strRxData = strDataInput;
					
					if(nDataEnd < 0)
						nDataEnd = strRxData.Length + nDataEnd;

					string strValueResult = strRxData.Substring(nDataStart, nDataEnd-nDataStart);

					Console.WriteLine("**RESOURCE MUTUAL EXCLUSION CRASH TEST CODE** Station ID: " + iMngStation.StationId.ToString() + ", Value: " + strValueResult);
					
					iMngStation.MngTStep.GetMngGv().SetValue(strResult, strValueResult, CpStringFormat.ASCII);
				}
                
                return TsResult.OK;
            });

            if (tResult.HasException)
            {
                this.ResultLog.TsActionResult = TsResult.ERROR;
                return TsResult.ERROR;				
            }

            return tResult.Result;
        }
    }
}
