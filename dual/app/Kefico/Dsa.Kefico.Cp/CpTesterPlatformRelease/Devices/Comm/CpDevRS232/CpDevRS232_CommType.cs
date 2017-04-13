using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace CpTesterPlatform.CpDevices
{
	public enum BAUD_RATE
	{
//		Bps_112		= 300,
//		Bps_600		= 600,
//		Bps_1200	= 1200,
//		Bps_2400	= 2400,
//		Bps_9600	= 9600,
//		Bps_14K		= 14400,
		Bps_18K		= 19200,
//		Bps_37K		= 38400,
//		Bps_56K		= 57600,
//		Bps_112K	= 115200,
//		Bps_250K	= 256000,
//		Bps_3M		= 3072000,
//		RS422		= 57600000
	}

	static class CpDevRS232_CommUtil
	{
		public const string			NO_AVAILABLE		= "No Port Available";

		public static RegistryKey   GetRegKeyPortAddr()
		{
			return Registry.LocalMachine.OpenSubKey("HARDWARE").OpenSubKey("DEVICEMAP").OpenSubKey("SERIALCOMM");
		}
		 
		public static List<string>	GetAvailablePorts()
		{
			SerialPort		pComPort		= new SerialPort();
			RegistryKey		regKey			= GetRegKeyPortAddr();
			List<string>	vstrPortName	= new List<string>();

			if(regKey == null)
			{
				vstrPortName.Add(NO_AVAILABLE);

				return vstrPortName;
			}				

			foreach(string strRegName in regKey.GetValueNames())
			{
				try
				{
					string		strPort		= (string) regKey.GetValue(strRegName);

					pComPort.PortName		= strPort;

					if(pComPort.IsOpen != true)
					{
						try
						{
							pComPort.Open();

							vstrPortName.Add(strPort);
						}
						catch (Exception){	throw;					}
						finally {			pComPort.Close();		}
					}								
				}
				catch (Exception){	throw;					}						
			}

			return vstrPortName;
		}		
	}
	
}
