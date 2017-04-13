using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn;
using System;
using System.Reflection;

namespace CpTesterPlatform.Functions
{
    /// <summary>
    /// SK = STIMULI
    /// ZUSATZ = ADDITIONAL
    /// F_SK_ZUSATZ = additional board setting for the SK-RELAY-BOARD which control the relays of the stimulus switch of the product.
    /// [CCS Doc]
    /// Controlling the stimuli switching matrix relay option board.
    /// - turn on or off of the switching matrix board. (my opinion)
    /// </summary>    

    public class CpFnF_SK_ZUSATZ : CpTsShell, IF_SK_ZUSATZ
    {
    }
}
