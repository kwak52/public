using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Dsu.Common.Utilities.Core.ExtensionMethods;

namespace UtDsu.Common
{
    [TestClass]
    public class UtZip
    {
        [TestMethod]
        public void UtZipUnzip()
        {
            var str = "Hello, this is uncompressed original string.";
            Assert.AreEqual(str.Compress().Decompress(), str);

        }
    }
}
