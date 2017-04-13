using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestTypeConversion
    {
        [TestMethod]
        public void TestMethodTypeConversion()
        {
            int int1 = 1;
            var xInt1 = ObjectValueParser.Convert(typeof(short), int1);
            Assert.IsTrue(xInt1.GetType() == typeof(short));
            Assert.IsTrue((short)xInt1 == 1);

            bool bool1 = true;
            var xBool1 = ObjectValueParser.Convert(typeof(bool), bool1);
            Assert.IsTrue(xBool1.GetType() == typeof(bool));
            Assert.IsTrue((bool)xBool1 == true);
        }
    }

}
