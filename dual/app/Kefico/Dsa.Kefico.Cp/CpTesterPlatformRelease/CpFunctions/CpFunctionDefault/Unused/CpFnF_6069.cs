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
    /// F = Function Unit
    /// 6069 = specific board in the FU3.    /// 
    /// F_6069 = specific board in the FU3 module name of '6069' should be set to be controlled.
    /// [CCS Doc]
    /// Initialization module for 6069 FU 3 slot.
    /// </summary>    

    public class CpFnF_6069 : CpTsShell, IF_6069
    {
    }
}
