using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace CpTesterPlatform.CpDevices
{
	public class CpDevRS232_CommRS
	{
		private Object	thisLock			= new Object();

		SerialPort		m_portSerial		= null;
		string			m_strPortName		= string.Empty;
		int				m_nBaudRate 		= 0;
		int				m_nDataBits			= 0;
		StopBits		m_stopBits			= StopBits.One;
		Parity			m_parity			= Parity.None;
		bool			m_bLockFin			= false;
		
		// Set the read/write timeouts
	   int				m_nReadTimeOut		= 500;
	   int				m_nWriteTimeOut		= 500;

						
		public				CpDevRS232_CommRS(string strPortName, int nBaudRate, int nDataBits, StopBits stopbits = StopBits.One, Parity parity = Parity.None) 
		{
			m_strPortName		= strPortName;
			m_nBaudRate			= nBaudRate;
			m_nDataBits			= nDataBits;
			m_stopBits			= stopbits;
			m_parity			= parity;
		}

		public void			SetTimeOut(int nWriteTimeOut, int nReadTimeOut)
		{
			m_nReadTimeOut		= nReadTimeOut;
			m_nWriteTimeOut		= nWriteTimeOut;
		}

		public void			Initialize()
		{
			try
			{
				m_portSerial				= new SerialPort();

				m_portSerial.PortName		= m_strPortName;

				if(m_nBaudRate > 0)
					m_portSerial.BaudRate		= m_nBaudRate;

				m_portSerial.Parity			= m_parity;
				m_portSerial.DataBits		= m_nDataBits;
				m_portSerial.StopBits		= m_stopBits;
				m_portSerial.ReadTimeout	= m_nReadTimeOut;
				m_portSerial.WriteTimeout	= m_nWriteTimeOut;

				m_portSerial.Encoding		= System.Text.Encoding.GetEncoding(28591);				
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error in Initialization for serial port: " + ex.Message);
			}			
		}

		public bool			Open()
		{
			try
			{
				m_portSerial.Open();
				
				return m_portSerial.IsOpen;
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error during serial port opening: " + ex.Message);

				return false;
			}
		}

		public bool			Close()
		{
			try
			{
				m_portSerial.Close();

				return true;
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error during serial port opening: " + ex.Message);

				return false;
			}
		}

		public SerialPort	GetSerailPort()
		{
			return m_portSerial;
		}

		public bool			IsMyPort(SerialPort pPort)
		{
			return (m_portSerial == pPort) ? true : false;
		}

		public bool			ReadLine(out string strCmd)
		{
			strCmd			= string.Empty;

			try
			{
				string		message		= m_portSerial.ReadLine();

				Console.WriteLine("ReadLine - Received Message: " + message);

				strCmd		= message;

				return true;
			}
			catch (TimeoutException ex) 
			{ 
				Console.WriteLine("ReadLine - Received Message - Time Out: " + ex.Message);

				return false;
			}			 
		}

		public void			WriteLine(object objstrSrc)
		{
			try
			{
				string		strSrc		= objstrSrc.ToString();

				m_portSerial.WriteLine(strSrc);
			}
			catch (Exception ex)
			{
				Console.WriteLine("WriteLine - Sending Message - Time Out: " + ex.Message);
			}
		}

		public void			WriteData(object objData)
		{	
			byte [] aData		= (byte []) objData;

			WriteData(aData, 0, aData.Length);
		}

		public void			WriteData(byte [] aData, int nStart = 0, int nRange = -1)
		{
			try
			{
				int			nSendingRange			= aData.Length;

				if(nRange > 0)
					nSendingRange		= nRange;

				lock(thisLock)				
				{
					m_bLockFin			= true;
						
					for(int i=0; i<aData.Length; i++)
					{
						m_portSerial.Write(aData, i, 1);

						Thread.Sleep(2);
					}
						
					int			nTimeOut		= 0;
										
					while(true)
					{
						Thread.Sleep(1);

						nTimeOut++;

						if(m_bLockFin == false || nTimeOut > 300)
							break;
					}
				}
				
				Thread.Sleep(10);
			}
			catch (Exception ex)
			{
				Console.WriteLine("WriteData - Sending Message - Time Out: " + ex.Message);
			}
		}

		public bool			IsOpened()
		{
			try
			{
				return m_portSerial.IsOpen;
			}
			catch (Exception ex)
			{
				Console.WriteLine("WriteLine - Sending Message - Time Out: " + ex.Message);

				return false;
			}
		}

		public void			ReleaseLock()
		{
			m_bLockFin		= false;
		}

		public bool			GetLockState()
		{
			return m_bLockFin;
		}
	}
}
