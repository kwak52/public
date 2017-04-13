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
    /// This module is used for procurement of the SKM. 
    /// It prepares the Input data to a string on SKM understandable and causes the required actions.
    /// The answer is stored in shared memory, where it can, for example, from the module p_zerlege_string be retrieved and processed.
    /// </summary>    
    /// <param name="BEFEHL"> (ex. CANBLOCK) Command to the SKM.</param>
    /// <param name="DATUM"> .</param>
    /// <param name="PARA1"> .</param>
    /// <param name="PARA2"> .</param>
    /// <param name="PARA3"> .</param>
    /// <param name="PARA4"> .</param>    
    /// <param name="FEHLERFLAG"> (ex. &FEHLERFLAG) Set for errors that occurred.</param>
    /// <param name="ERGEBNIS"> (ex. &MEWE) Pointer to the response string of PSS / SKM.</param>    
    /// <param name="ERGEBNIS_FELD"> .</param>        
    /// </summary>    

    public class CpFnF_AUFTRAG : CpTsShell, IF_AUFTRAG
    {
    }
}
