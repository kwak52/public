using System;
using System.Diagnostics;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;

namespace CpTesterPlatform.Functions
{
    public class CpFnP_STEUER : CpTsShell, IP_STEUER
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleP_STEUER psModuleP_STEUER = this.Core as PsCCSStdFnModuleP_STEUER;
            Debug.Assert(psModuleP_STEUER != null);

            string strCANPortID = psModuleP_STEUER.ANSTEUER_UNIT;

            //clsCpSgCANPort crtSelCANPort = cpMngSystem.MngHardware.MngCAN.getCANPort(strCANPortID);

            //this.ResultLog.TsActionResult = (crtSelCANPort != null ? TsResult.OK : TsResult.NG);

            //if (crtSelCANPort != null)
            //{
            //    ICAN2000INI cpTsCAN2000INI = cpTsParent as ICAN2000INI;
            //    if (cpTsCAN2000INI != null)
            //        cpMngSystem.MngHardware.MngCAN.setCrtCANPortID(strCANPortID);

            //    base.ResultLog.TsActionResult = TsResult.OK;
            //    return TsResult.OK;
            //}

            base.ResultLog.TsActionResult = TsResult.NONE;
            return TsResult.ERROR;
        }
    }
}
