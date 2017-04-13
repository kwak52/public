using System.IO.Ports;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface IRS232 : IDevice, IComm
    {
		string PortName {set; get;}
		int BaudRate {set; get;}
		int DataBits {set; get;}
		StopBits StopBits {set; get;}
		Parity Parity {set; get;}
		int TimeOutRead { set; get; }
		int TimeOutWrite { set; get; }
		bool UseCarrigeReturn { set; get; }
		bool UseLineFeed { set; get; }
    }
}
