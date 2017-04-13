using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn;
using PsKGaudi.Parser.PsCCSSTDFn.Module;
using System;
using System.Diagnostics;

namespace CpTesterPlatform.Functions
{
    /// <summary>
    /// This module is used for procurement of the SKM. 
    /// It prepares the Input data to a string on SKM understandable and causes the required actions.
    /// The answer is stored in shared memory, where it can, for example, from the module p_zerlege_string be retrieved and processed.
    /// </summary>    
    /// <param name="BEFEHL"> (ex. CANBLOCK) Command to the SKM.</param>
    /// <param name="ID_SENDEN"> (ex. 7E0) Transmission identifier to the SG: Identifier in HEX to that SG. If no message is sent, so no value is entered.</param>    
    /// <param name="ID_SENDEN_LEN"> (ex. 11) Here, the bit number of the transmit identifier is entered.</param>
    /// <param name="ID_LESEN"> (ex. 7E8) Reading Identifier: Specifies the identifier for the object on which the response of the SG is expected. If no response from the SG to be read, so nothing is entered here.</param>
    /// <param name="ID_LESEN_LEN"> (ex. 11) Here, the bit number of the read identifier is entered.</param>
    /// <param name="MODUS_STD"> (ex. 11) Specifies how the message should be handled.</param>
    /// <param name="ERGEBNIS"> (ex. INITPOR) Here are the data to be sent to SG (ID_SENDEN) entered.</param>
    /// <param name="PERIODE_SEND"> (ex. 0) Specifies whether a broadcast area transmitted once or cyclically.</param>
    /// <param name="TIMEOUT_SKM"> (ex. -) If a request does not answer within this time, so an error is generated.</param>
    /// <param name="FEHLERFLAG"> (ex. &FEHLERFLAG) Set for errors that occurred.</param>
    /// <param name="ERGEBNIS"> (ex. &MEWE) Pointer to the response string of PSS / SKM.</param>    
    /// </summary>    

    public class CpFnP_CANSTD : CpTsShell, IP_CANSTD
    {


        protected override TsResult ExecuteMain(CpSystemManager cpMngSystem, IStnManager iMngStation, CpTsShell cpTsParent = null)
        {
            PsCCSStdFnModuleP_CANSTD psModuleP_CANSTD = this.Core as PsCCSStdFnModuleP_CANSTD;
            Debug.Assert(psModuleP_CANSTD != null);

            ICAN2000INI cpTsCAN2000INI = cpTsParent as ICAN2000INI;
            if (cpTsCAN2000INI != null)
            {
                //clsCpSgCANPort crtCANActivatedPort = cpMngSystem.MngHardware.MngCAN.getCrtActivatedCANPort();
                //Debug.Assert(crtCANActivatedPort != null);

                //string strTxID = psModuleP_CANSTD.LstParameterWithValue[(int)(STDFuncParmsP_CANSTD.ID_SENDEN)].Value;
                //Debug.Assert(strTxID != string.Empty);
                //crtCANActivatedPort.Property.IdTx = strTxID;

                //string strTxLength = psModuleP_CANSTD.LstParameterWithValue[(int)(STDFuncParmsP_CANSTD.ID_SENDEN_LEN)].Value;
                //Debug.Assert(strTxLength != string.Empty);
                //crtCANActivatedPort.Property.TxLength = Convert.ToInt32(strTxLength);

                //string strRxID = psModuleP_CANSTD.LstParameterWithValue[(int)(STDFuncParmsP_CANSTD.ID_LESEN)].Value;
                //Debug.Assert(strRxID != string.Empty);
                //crtCANActivatedPort.Property.IdRx = strRxID;

                //string strRxLength = psModuleP_CANSTD.LstParameterWithValue[(int)(STDFuncParmsP_CANSTD.ID_LESEN_LEN)].Value;
                //Debug.Assert(strRxLength != string.Empty);
                //crtCANActivatedPort.Property.RxLength = Convert.ToInt32(strRxLength);

                //string strMode = psModuleP_CANSTD.LstParameterWithValue[(int)(STDFuncParmsP_CANSTD.MODUS_STD)].Value;

                //switch (strMode)
                //{
                //    case "INITPROT":
                //        {
                //            if (cpMngSystem.CnfSystem.HaConfigure.ActiveHwFncCAN)
                //            {
                //                if (!cpMngSystem.MngHardware.MngCAN.openSelectedCANPort(crtCANActivatedPort.PortID))
                //                    Debug.Assert(false);
                //            }
                //        }
                //        break;
                //    default:
                //        {
                //            Debug.Assert(false);
                //            this.ResultLog.TsActionResult = TsResult.NG;
                //        }
                //        break;
                //}

                this.ResultLog.TsActionResult = TsResult.OK;
                return TsResult.OK;
            }

            this.ResultLog.TsActionResult = TsResult.NONE;
            return TsResult.ERROR;
        }
    }




}
