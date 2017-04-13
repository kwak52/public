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
    /// L = LOAD
    /// KAM = power source board
    /// ZUSATZ = ADDITIONAL
    /// L_KAM_ZUSATZ = additional board setting for the load board which interconnects between stimuli and the product.
    /// [CCS Doc]
    /// Necessary setting for surveillance / measurement parameter.
    /// This module - as well as the 'F_ANSTEUER' module the interface from the downstream hardware GAUDI Drivers are.
    /// The hardware driver for the KAM, both through education eventual call of 
    /// 'F_ANSTEUER' module (with the appropriate, in the 'hw_units _.... CNF ' file given Ansteuerunits) are served, as well as additionally with this module.
    /// This module is only for special features in the PAV no mention (Sensing , measuring channels) order to work.
    /// Its use is limited to such PAM's, with new hardware.    
    /// </summary>
    
    public class CpFnL_KAM_ZUSATZ : CpTsShell, IL_KAM_ZURATZ
    {
    }
}
