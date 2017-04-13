using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpCommon.ResultLog;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;
using PsKGaudi.Parser.PsCCSSTDFn;
using System;
using System.Reflection;

namespace CpTesterPlatform.Functions
{
    /// <summary>
    /// DCI [A] 
    /// 
    /// - Description of Function : 
    ///     Indirect measurement of current by means of integrating DC voltage measurement with AUTO triggering on a defined shunt.
    ///     This function is always used when a load with integrated shunt is connected to a control unit pin and the current is to be measured on the integrated shunt. 
    ///     Currents flowing into the control unit register as positive.
    ///     
    /// - Parameters
    ///     1. MaxSignalAmplitude
    ///         Largest amplitude occurring on the signal. Can be used to establish the measurement range or to select a suppressor circuit.
    /// 
    ///     2. IntegrationPeriod
    ///         Period over which the signal is integrated.
    ///         In the event of superimposed interference signals, it is advisable to select a multiple of the interference signal period as the integration period.
    ///         Standard value: period duration of supply voltage.
    ///         
    ///     3. Shuntresistor
    ///         For selection of resistor value see MSL Catalogue, Implementation of Modules (Simulated Loads).
    ///         
    /// </summary>

    public class CpFnDCI : CpTsMacroMsDCIShell, IDCI
    {
        public CpFnDCI()
        {
            this.STDFnMacroType = CpDefineTStepMacroType.ANL_MEASURE;
            this.ResultLog = new ClsRlMeasuring(TsResult.NONE, TsResult.NONE);
        }
    }
}
