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
    /// L = LOARD
    /// KAM = power source board
    /// KONFIG = CONFIGURATION
    /// L_KAM_KONFIG = an information for the KAM board configuration.
    /// [CCS Doc]
    /// Defining the basic configuration for KAM.
    /// This module - as well as the ' F_ANSTEUER' module - the interface from the downstream hardware GAUDI Drivers are.
    /// The hardware driver for the KAM, both through education eventual call of 
    /// 'F_ANSTEUER' module (with the appropriate, in the 'hw_units _....CNF ' file given Ansteuerunits ) are served, as well as additionally with this module.
    /// This module is the only for special features in PAV no mention of ( one / two - Constant - operation, Cascading) is required.
    /// Its use is limited to such PAM 's , with new hardware ( 6278 ) are equipped!    
    /// </summary>    

    public class CpFnL_KAM_KONFIG : CpTsShell, IL_KAM_KONFIG
    {
    }
}
