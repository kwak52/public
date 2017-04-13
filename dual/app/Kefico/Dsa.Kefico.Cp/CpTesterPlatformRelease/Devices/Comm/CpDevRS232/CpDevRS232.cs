using CpTesterPlatform.CpTStepDev.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using System.IO.Ports;
using static CpCommon.ExceptionHandler;
using System.Reflection;

namespace CpTesterPlatform.CpDevices
{
    /// <summary>
    /// Example to represent how to build a communication device.
    /// A Communication Device is constructed in run-time environment by a communication device manager.
    /// Communication Device is an inherited class from a corresponding interface.
    /// </summary>
	public class CpDevRS232 : IRS232
	{
		public CpFunctionEventHandler FuncEvtHndl { get; set; }
		public string DeviceID { get; set; }
		
		public string PortName {set; get;}
		public int BaudRate {set; get;}
		public int DataBits {set; get;}
		public StopBits StopBits {set; get;} = StopBits.One;
		public Parity Parity {set; get;}	 = Parity.None;		
		public int TimeOutRead { set; get; } = 500;
		public int TimeOutWrite { set; get; } = 500;
		public bool UseCarrigeReturn { set; get; } = false;
		public bool UseLineFeed { set; get; } = false;

		CpDevRS232_CommRS RS_COMM = null;

		public string GetModuleName()
		{
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
		}

		public bool DevClose()
		{
			if(RS_COMM != null)
				return RS_COMM.Close();

			return true;
		}

		public bool DevOpen()
		{
            var oResult = TryFunc(() =>
            {
                List<string> vstrPort = CpDevRS232_CommUtil.GetAvailablePorts();

                if (!vstrPort.Contains(PortName))
                    return false;

                RS_COMM = new CpDevRS232_CommRS(PortName, BaudRate, DataBits, StopBits, Parity);

                RS_COMM.SetTimeOut(TimeOutWrite, TimeOutRead);
                RS_COMM.Initialize();

                return RS_COMM.Open();
            });

            if (oResult.HasException)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to Device Open RS232.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

                return false;
            }

            return oResult.Result;
        }

		public bool DevReset()
		{
			return false;
		}

		public bool ReadData(ref byte[] readBuffer)
		{
			return false;
		}

		public bool WriteData(byte[] writeBuffer)
		{
			return true;
		}

		public bool WriteData(string writeData)
		{
            var oResult = TryFunc(() =>
            {
				List<byte> abWritingData = CpDevRS232_ITUtil.CvStr2ByteArray(writeData);

				if(UseCarrigeReturn == true)
					abWritingData.Add(Convert.ToByte(CpDevRS232_ITUtil.CARRIAGE_RETURN));

				if(UseLineFeed == true)
					abWritingData.Add(Convert.ToByte(CpDevRS232_ITUtil.LINE_FEED));				

				RS_COMM.WriteData(abWritingData.ToArray());

				return true;
			});

            if (oResult.HasException)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("RS-232 Writting Error: ", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);

                return false;
            }

            return oResult.Result;
		}

		public bool ReadData(ref string readData)
		{
            string strReadData = string.Empty;

            var oResult = TryFunc(() =>
            {
				return RS_COMM.ReadLine(out strReadData);;
			});

            if (oResult.HasException)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("RS-232 Reading Error: ", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\tReason : " + oResult.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

                return false;
            }

            readData = strReadData;

            return oResult.Result;
		}
	}
}
