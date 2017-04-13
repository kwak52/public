using System;
using System.Text;
using System.Security.Cryptography;

namespace Dsu.LicenseKeyChecker
{
    internal static class CLicenseActivator
	{

		#region Member Variables

        private static string _PassWord = "Dual2016";

		#endregion


		#region Public Methods

		public static string GenerateActivationCode(string sProduct)
		{
			CHardwareInfo cMachine = new CHardwareInfo();

			string sMachineID = sProduct
								+ cMachine.GetProcessorID() 
								+ cMachine.GetHardDiskID(0);

			string sCode = DateTime.Now.ToString("yyMMdd") + sMachineID;
            sCode = GetMD5Encryt(sCode);

			return sCode;
		}

        public static CLicenseInfo AnalyseKey(string sKey)
        {

            CLicenseInfo cInfo = new CLicenseInfo();

            string sDecKey = Decrypt(_PassWord, sKey);

            string[] KeyInfo = sDecKey.Split('_');

            try
            {

                if (sDecKey.EndsWith("X"))
                {

                    cInfo.IsDemo = true;
                    cInfo.DemoDays = int.Parse(KeyInfo[1]);

                }

                cInfo.ActivationCode = KeyInfo[0];
                cInfo.ActivationKey = KeyInfo[0];
            }
            catch (System.Exception ex)
            {
                ex.Data.Clear();
            }

            return cInfo;
        }

		#endregion


		#region Private Methods

		private static string GetMD5Encryt(string sKey)
		{
            HashAlgorithm algorithm = MD5.Create();

            byte[] GetHash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(sKey));

            StringBuilder hex = new StringBuilder(GetHash.Length);
            for (int i = 0; i < GetHash.Length /4; i++)
                hex.Append(GetHash[i].ToString());

            return hex.ToString();
		}

		private static string Decrypt(string sKey, string sText)
		{
            string sValue = string.Empty;

			try
			{
				RijndaelManaged rijndaelCipher = new RijndaelManaged();
				rijndaelCipher.Mode = CipherMode.CBC;
				rijndaelCipher.Padding = PaddingMode.PKCS7;

				rijndaelCipher.KeySize = 128;
				rijndaelCipher.BlockSize = 128;
                byte[] encryptedData = Convert.FromBase64String(HexToString(sText));
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
				byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
				
				sValue =  Encoding.UTF8.GetString(plainText);
			}
			catch(System.Exception ex)
			{
				ex.Data.Clear();
			}

            return sValue;
		}

        private static string MoreDecypt(string sKey,string PassWord)
        {




            return sKey;
        }

        private static string MoreEncypt(string sKey, string PassWord)
        {



            return sKey;
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
