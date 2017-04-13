using System;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpCommon.ResultLog;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// DCV [V] (Us_low, Us_high)
	/// 
	/// - Description of Function : Integrating DC voltage measurement with AUTO triggering.
	/// - Parameters
	///     1. MaxSignalAmplitude
	///         Largest amplitude occurring on the signal. Can be used to establish the measurement range or to select a suppressor circuit.
	/// 
	///     2. IntegrationPeriod
	///         Period over which the signal is integrated. In the event of superimposed interference signals, 
	///         it is advisable to select a multiple of the interference signal period as the integration period.
	///         
	/// </summary>
	public class CpFnDCV : CpTsMacroMsDCShell, IDCV
	{
		public CpFnDCV()
		{
			this.STDFnMacroType = CpDefineTStepMacroType.ANL_MEASURE;
			this.ResultLog = new ClsRlMeasuring(TsResult.NONE, TsResult.NONE);
		}
	}
}
