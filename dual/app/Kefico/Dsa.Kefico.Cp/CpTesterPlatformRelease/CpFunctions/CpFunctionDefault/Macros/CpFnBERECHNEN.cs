using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// Links variable (function results) and decimal constants in a mathematical formula (parameter term).
	/// In The Case of variable, the `Calculate 'function Operates Exclusively with pure numerical values Regardless of the units specified in the PAV.
	/// The units of the test result are therefore ignored by the calculating (Calculate) function.
	/// Example: The Reading of 7 V Obtained from a voltage measurement is assigned to the variable UB2.
	/// The function Calculate (UB2 + 2.0) is then invoked. Following that, UB2 is passed to a D / A converter as a stimulus value with the unit mV.
	/// Result of the calculation: 9.0 Many units can be specified for the computing function
	/// but They are Treated as simply Remarks Indicating to the reader how the results of the calculation are to be interpreted..    
	/// </summary>
	/// <param name="Term"> Mathematical term consisting of a combination of variables, constants, the operators +,-,*,/, and round brackets.</param>
	/// <param name="Rounding_Method"> Specifies whether and how the result is to be rounded..</param>    

	public class CpFnBERECHNEN : CpTsMacroMsShell, IBERECHNEN
    {
	    public double EqationResult { get; set; } = 0.0;

        public double getEquationResult()
        {
            return EqationResult;
        }

        public void setEquationResult(double dResult)
        {
            EqationResult = dResult;
        }

		public CpFnBERECHNEN()
		{
		}
    }
}
