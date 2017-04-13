using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DsuLicenseGenerator
{
	internal static class CLicenseGenerator
	{

		#region Member Variables


        private static string _PassWord = "Dual2016";
	

		#endregion


		#region Public Methods

		public static string GenerateActivationKeyDemo(string sCode, int iDays, int iHours)
		{
            string days = iDays.ToString();

            string sKey = sCode + "_" + days + "_" + "X";

            sKey = Encrypt(_PassWord, sKey);

			return sKey;
		}

		public static string GenerateActivationKeyFull(string sCode)
		{
            string sKey = sCode + "_" + "o";

            sKey = Encrypt(_PassWord, sKey);

			return sKey;
		}

		#endregion


		#region Private Methods

		private static string Encrypt(string sKey, string sText)
		{
			RijndaelManaged rijndaelCipher = new RijndaelManaged();
			rijndaelCipher.Mode = CipherMode.CBC;
			rijndaelCipher.Padding = PaddingMode.PKCS7;

			rijndaelCipher.KeySize = 128;
			rijndaelCipher.BlockSize = 128;
			byte[] pwdBytes = Encoding.UTF8.GetBytes(sKey);
			byte[] keyBytes = new byte[16];
			int len = pwdBytes.Length;
			if (len > keyBytes.Length)
			{
				len = keyBytes.Length;
			}
			Array.Copy(pwdBytes, keyBytes, len);
			rijndaelCipher.Key = keyBytes;
			rijndaelCipher.IV = keyBytes;
			ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
			byte[] plainText = Encoding.UTF8.GetBytes(sText);

            return stringToHex(Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length)));
		}

        private static string stringToHex(string input)
        {
            string resultHex = string.Empty;
            byte[] arr_byteStr = Encoding.Default.GetBytes(input);

            foreach (byte byteStr in arr_byteStr)
                resultHex += string.Format("{0:X2}", byteStr);

            return resultHex;

        }

        private static string HexToString(string input)
        {
            string resultString = string.Empty;
            while (input.Length > 0)
            {
                resultString += System.Convert.ToChar(System.Convert.ToUInt32(input.Substring(0, 2), 16)).ToString();
                input = input.Substring(2, input.Length - 2);
            }

            return resultString;

        }

		#endregion
	}
}
