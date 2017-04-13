using CpTesterPlatform.CpTesterSs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTester
{
    public delegate void StopEventHandler(bool bStop);
    public delegate void UnloadingResultEventHandler(List<PLCResult> result, bool nNg, string Message, int NgStation, int StationIndex);
    public delegate void UnloadingCloseEventHandler(bool bClose);
    public delegate void UnloadingPassEventHandler(bool bPassMode);
    public delegate void UnloadingDialogEventHandler(bool bShowDialog);
    public delegate void FdMasterEventHandler(bool bStart);

    
}
