using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtCrypto
    {
        [TestMethod]
        public void TestMethodCrypto()
        {
            const string key = "Crypto Key";
            string[] samples = new string[] { "Using C# function", "This is a sample text.", "Checking crypto-graph.", };

            try
            {
                foreach (string text in samples)
                {
                    // method test 1
                    string enc = Crypto.Encrypt(text, key);
                    string dec = Crypto.Decrypt(enc, key);

                    Assert.AreEqual(text, dec);

                    // method test 2
                    Assert.AreEqual(text, CryptoUtil.Decrypt(CryptoUtil.Encrypt(text)));
                }
            }
            catch (Exception ex)
            {
                Tools.ShowMessage("Exception occurred " + ex.ToString());
            }
        }
    }
}
