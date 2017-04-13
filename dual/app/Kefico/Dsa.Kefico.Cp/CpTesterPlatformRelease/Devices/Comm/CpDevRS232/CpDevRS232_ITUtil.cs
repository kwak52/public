using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace CpTesterPlatform.CpDevices
{
	static class CpDevRS232_ITUtil
	{
		public const int		HEX_VALUE			= 16;		
		public const int		BIN_VALUE			= 2;
		public const int		PACKET_SIZE			= 8;
		public const char		CARRIAGE_RETURN		= '\r';
		public const char		LINE_FEED			= '\n';

		public static string	MergeByte(byte [] acData, int nStart, int nRange)
		{
			string		strData		= string.Empty;

			for(int i=(nStart+nRange)-1; i>=nStart; i--)
				strData		+= CvByte2Hex(acData[i]);

			return strData;
		}

		public static byte[]	MergeByteArray(byte[] acDataP, byte[] acDataC)
		{	
			byte []		acResult	= new byte [acDataP.Length + acDataC.Length];
			int			nIdx		= 0;

			for(; nIdx<acDataP.Length; nIdx++)
				acResult[nIdx]		= acDataP[nIdx];

			for(; nIdx<acResult.Length; nIdx++)
				acResult[nIdx]		= acDataC[nIdx - acDataP.Length];

			return acResult;		
		}

		public static byte[]	SplitByte(int nVal, int nConSize)
		{
			byte []			acData		= new byte [nConSize];
			int				nExLen		= nConSize*2;
			string			strResult	= CvNum2Hex(nVal, nExLen);
			
			for(int i=0; i<nConSize; i++)
				acData[i]		= CvHex2Byte(strResult.ElementAt(nExLen - (2*i) - 2).ToString() + strResult.ElementAt(nExLen - (2*i) - 1).ToString());

			return acData;
		}

		public static int		CvStrBinVal2Dec(char [] acdata, bool bLE = true)
		{
			int			nStartIdx		= (bLE == true) ? acdata.Length - 1 : 0;
			int			nIncVal			= (bLE == true) ? -1 : 1;
			int			nEndIdx			= (bLE == true) ? 0 : acdata.Length - 1;
			int			nIdx			= nStartIdx;
			int			nResult			= 0;

			while(true)
			{
				char		cVal		= acdata[nIdx];
				int			nVal		= Convert.ToInt32(Math.Pow(2, nIdx)) * Convert.ToInt32(cVal.ToString());

				nResult		+= nVal;
				nIdx		+= nIncVal;

				if(nIdx == nEndIdx)
					break;
			}

			return nResult;			
		}

		public static string	CvByte2Hex(byte cVal)
		{
			return CvDec2Hex(cVal.ToString(), 2);
		}

		public static string	CvNum2Hex(int nDec)
		{
			return Convert.ToString(nDec, HEX_VALUE);
		}

		public static string	CvNum2Hex(long lVal)
		{
			return Convert.ToString(lVal, HEX_VALUE);
		}

		public static string	CvDec2Hex(string strDec)
		{
			return CvNum2Hex(Convert.ToInt32(strDec));
		}

		public static string	CvDec2Hex(string strDec, int nLen)
		{
			return CvNum2Hex(Convert.ToInt32(strDec)).PadLeft(nLen, '0');
		}

		public static string	CvNum2Hex(int nDec, int nLen)
		{
			return Convert.ToString(nDec, HEX_VALUE).PadLeft(nLen, '0');
		}
		
		public static string	CvDec2Bin(int nDec)
		{
			string		strBinVal		= Convert.ToString(Convert.ToInt32(nDec), BIN_VALUE);

			return strBinVal; 
		}

		public static string	CvDec2Bin(int nDec, int nLen)
		{
			string		strBinVal		= CvDec2Bin(Convert.ToInt32(nDec));
			string		strBinRst		= strBinVal.PadLeft(nLen, '0');

			return strBinRst; 
		}


		public static byte		CvHex2Byte(string strHex)
		{
			return Convert.ToByte(CvHex2Dec(strHex));
		}
		
		public static int		CvHex2Dec(string strHex)
		{
			return int.Parse(strHex, System.Globalization.NumberStyles.AllowHexSpecifier);
		}

		public static List<byte> CvStr2ByteArray(string strData)
		{
			List<byte> abResult = new List<byte>();
			
			foreach(char ch in strData.ToCharArray())
				abResult.Add(Convert.ToByte(ch));
				
			return abResult;
		}

		public static int		GetByteValue(int nValue, int nIdx)
		{
			//80a0	//32928 //10000000 10100000
			//a0: 0번째 바이트
			//80: 1번째 바이트

			return (nValue >> (8 * nIdx)) & 255;
		}
		
		public static string	SubstractString(string strSrc, int nStart, int nLength)
		{
			return strSrc.Substring(nStart, nLength);
		}

		public static string	InvertEndianSequence(string strSrc)
		{
			string		strResult		= string.Empty;

			for(int i = strSrc.Length-1; i>=0; i--)
				strResult		+= strSrc.ElementAt(i).ToString();
			
			return strResult;
		}

		public static bool		GetBinValue(byte cVal, int nIdx)
		{
			return ((cVal >> nIdx) & 1) == 0 ? false : true;
		}

		public static List<string>	AlignRxMsgs(string strMsg)
		{
			List<string>		vstrMsgs		= new List<string>();
			
			vstrMsgs		= strMsg.Split(CARRIAGE_RETURN).ToList();		
			
			return vstrMsgs;
		}

		public static string		CheckFinalMsgIntegrity(ref List<string> vstrMsgs)
		{
			string				strEndMsg		= vstrMsgs[vstrMsgs.Count - 1];
			bool				bResult			= false;
			
			if(strEndMsg.ElementAt(strEndMsg.Length - 1) != CARRIAGE_RETURN)
			{
				vstrMsgs.RemoveAt(vstrMsgs.Count - 1);

				bResult				= true;
			}
			
			return (bResult == true) ? strEndMsg : string.Empty;
		}
	}
}
