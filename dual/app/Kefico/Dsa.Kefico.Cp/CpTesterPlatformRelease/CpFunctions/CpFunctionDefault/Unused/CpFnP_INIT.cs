using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using System;
using System.Diagnostics;

namespace CpTesterPlatform.Functions
{
    public class CpFnP_INIT : CpTsShell, IP_INIT
    {
        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleP_INIT psModuleP_INIT = this.Core as PsCCSStdFnModuleP_INIT;
            Debug.Assert(psModuleP_INIT != null);

            // assign the properties of the CAN.
            ICAN2000INI cpTsCAN2000INI = cpTsParent as ICAN2000INI;
            if (cpTsCAN2000INI != null)
            {
                //clsCpSgCANPort crtCANActivatedPort = cpMngSystem.MngHardware.MngCAN.getCrtActivatedCANPort();
                //Debug.Assert(crtCANActivatedPort != null);

                string strValue = psModuleP_INIT.ADRESSE;
                string strData = psModuleP_INIT.DATUM;
                this.ResultLog.TsActionResult = TsResult.OK;

                switch (strValue)
                {
                    case "CAN_T_SEND":
                        {
                            int nTxTimeOut = Convert.ToInt32(strData);
                            //crtCANActivatedPort.Property.TimeOutTx = nTxTimeOut;
                        }
                        break;
                    case "CAN_T_READ":
                        {
                            int nRxTimeOut = Convert.ToInt32(strData);
                            //crtCANActivatedPort.Property.TimeOutRx = nRxTimeOut;
                        }
                        break;
                    case "CAN_DRIVER":
                        {
                            CpCANDriverType eCANDriver = (CpCANDriverType)Enum.Parse(typeof(CpCANDriverType), strData);
                            //crtCANActivatedPort.Property.DriverType = eCANDriver;
                        }
                        break;
                    case "BAUDRATE":
                        {
                            uint nBaudrate = Convert.ToUInt32(strData);
                            //crtCANActivatedPort.Property.Baudrate = nBaudrate;
                        }
                        break;
                    case "CAN_TAST":
                        {
                            int nTast = Convert.ToInt32(strData);
                            //crtCANActivatedPort.Property.Tast = nTast;
                        }
                        break;
                    case "CAN_WIDER":
                        {
                            int nResist = Convert.ToInt32(strData);
                            //crtCANActivatedPort.Property.Resist = nResist;
                        }
                        break;
                    case "NA_ARCHITEKTUR":
                        {
                            // store result of the hole module steps.
                        }
                        break;
                    default:
                        {
                            Debug.Assert(false);
                            this.ResultLog.TsActionResult = TsResult.NG;
                        }
                        break;
                }
            }

            return TsResult.OK;
        }
    }
}
