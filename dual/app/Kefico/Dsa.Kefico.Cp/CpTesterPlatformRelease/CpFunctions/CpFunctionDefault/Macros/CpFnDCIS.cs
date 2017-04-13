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
    /// DCIS [A] 
    /// 
    /// - Description of Function : 
    ///     Indirect measurement of current by means of integrating DC voltage measurement with AUTO triggering on a defined measurement resistor.
    ///     This function is always used when a current measurement resistor has to be inserted in a line to the control unit (e.g. UB) for the duration of the measurement. 
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
    ///         For selection of resistor level see MSL Catalogue, Current Measurement Using Series Resistor.
    ///         The maximum permissible testing impedance is specified as the parameter.
    ///         
    /// </summary>

    public class CpFnDCIS : CpTsMacroMsDCIShell, IDCIS
    {
        private double m_dEqationResult = 0.0;
        public double EqationResult
        {
            get { return m_dEqationResult; }
            set { m_dEqationResult = value; }
        }

        public double getEquationResult()
        {
            return EqationResult;
        }

        public void setEquationResult(double dResult)
        {
            EqationResult = dResult;
        }

        public CpFnDCIS()
        {
            this.STDFnMacroType = CpDefineTStepMacroType.ANL_MEASURE;
            this.ResultLog = new ClsRlMeasuring(TsResult.NONE, TsResult.NONE);
        }
    }
}
