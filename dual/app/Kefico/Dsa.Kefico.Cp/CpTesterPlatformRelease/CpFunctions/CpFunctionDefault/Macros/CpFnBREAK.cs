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
    /// Terminates the test if a measured value is outside the tolerance limits. This function does not return a value.    
    /// </summary>

    public class CpFnBREAK : CpTsMacroMsShell, IBREAK
    {
    }
}
