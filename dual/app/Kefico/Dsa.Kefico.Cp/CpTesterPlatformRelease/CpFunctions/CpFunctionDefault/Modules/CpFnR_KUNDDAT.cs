using System.Collections.Generic;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;

namespace CpTesterPlatform.Functions
{
	public class CpFnR_KUNDDAT : CpTsShell, IR_KUNDDAT
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleR_KUNDDAT psModuleR_KUNDDAT = this.Core as PsCCSStdFnModuleR_KUNDDAT;
            Debug.Assert(psModuleR_KUNDDAT != null);

			string strResult = string.Empty;

            IPRINTOUT cpTsPRINTOUT = cpTsParent as IPRINTOUT;
            if (cpTsPRINTOUT != null)
            {
                strResult = GetLogString(iMngStation, psModuleR_KUNDDAT.DATENSTRING1);

                cpTsPRINTOUT.setPrintoutString(strResult);

                base.ResultLog.TsActionResult = TsResult.OK;
                return TsResult.OK;
            }

            this.ResultLog.TsActionResult = TsResult.NONE;
            return TsResult.NONE;
        }

		string GetLogString(IStnManager iMngStation, string strInput)
		{
			string strResult = string.Empty;
			string [] vstrSgStreamVars = strInput.Split(new char [] { ' ', '_' });
						
			for(int i=0; i<vstrSgStreamVars.Length; i++)
			{
				if (iMngStation.MngTStep.GetMngGv().IsKindOfVariable(vstrSgStreamVars[i]))
				{
					CpGlbVarBase retValue = iMngStation.MngTStep.GetMngGv().GetValue(vstrSgStreamVars[i]);
					strResult += retValue.RawValue.ToString();
				}
				else
					strResult += vstrSgStreamVars[i];
				
				strResult += (i == vstrSgStreamVars.Length-1) ? "" : " ";
			}

			return strResult;
		}
    }
}
