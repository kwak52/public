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
    /// E = ???
    /// MESSAGES = A SET OF MESSAGES
    /// E_MESSAGES = a set of messages could be declare to use in the process.
    /// [CCS Doc]
    /// Dissemination of messages to a process.    
    /// </summary>    

    public class CpFnE_MESSAGES : CpTsShell, IE_MESSAGES
    {
    }
}
