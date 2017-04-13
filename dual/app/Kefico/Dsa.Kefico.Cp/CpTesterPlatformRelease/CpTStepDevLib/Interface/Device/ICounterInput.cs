using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface ICounterInput : IDAQIO
    {				
		double MinimumValue { set; get; } 
		double MaximumValue { set; get; } 
		double MinimumFrequency { set; get; } 
		double MaximumFrequency { set; get; } 
		string CounterChannel  { set; get; } 
		int SampleCount { set; get; } 
		
		double GetFrequency(int nSampleCount);	
		double GetDuty(int nSampleCount);		
    }
}
