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
    /// TRIGGER = TRIGGER OF THE BOARD
    /// INIT = INITIALIZATION
    /// E_TRIGGER_INIT = the trigger of the board should be initialized.
    /// This module is for the pre-assignment of the trigger card with channel-specific Zustndig parameters.
    /// The channel-specific parameters in the module as Parameters displayed.
    /// </summary>

    public class CpFnE_TRIGGER_INIT : CpTsShell, IE_TRIGGER_INIT
    {
    }
}
