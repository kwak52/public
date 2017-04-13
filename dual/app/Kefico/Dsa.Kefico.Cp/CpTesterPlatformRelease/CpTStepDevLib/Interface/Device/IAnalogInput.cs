using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
	public interface IAnalogInput : IDAQIO
	{
		//
		// 요약:
		//     AI coupling. default = AICoupling.DC
		string AICoupling { get; set; }   
		//
		// 요약:
		//     Low pass cut-off frequency. default = 500K Hz
		double LowpassCutoffFrequency { get; set; }
		//
		// 요약:
		//     Low pass filter enable. default = false
		bool LowpassEnable { get; set; }
		//
		// 요약:
		//     Max value. default = +10.0
		double Max { get; set; }
		//
		// 요약:
		//     Min value for CreateVoltageChannel. default = -10.0
		double Min { get; set; }
		//
		// 요약:
		//     Terminal configuration. default = -1
		string TerminalConfiguration { get; set; }
        //
        // 요약:
        //     Sample count limit  per sec each Channel 
        double SampleRateLimit { get; set; }
	   
		double GetInstantV();
		Task<double []> GetPeriodicV(double nPeriodSec, double dSamplingRate);
        void StartDataCollecting(int nPeriodSec, double dSamplingRate);
        List<double> EndDataCollecting();
        bool ReturnBuffer(double[] vresult);

    }
}
