using System;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsCommon;
using PsCommon.Enum;

namespace CpTesterPlatform.Functions
{
	public class CpFnCONTROL : CpTsMacroShell, ICONTROL
    {        
        public eVERZWEIGUNG BranchType { set; get; }
        public int PairControlFnStepNum { set; get; } = -1;

		public CpFnCONTROL()
		{
			 
		}
    }
}
