using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    public class ValueContainer
    {
        public string Value { get; set; }
    }

    public static class GenericConverter
    {

        public static double GetDoubleValue(this ValueContainer container)
        {
            double value = 0;
            if (container == null)
                return 0;
            if (!Double.TryParse(container.Value, out value))
                return 0;

            return value;
        }
    }

    [TestClass]
    public class UnitTestXml
    {
        [TestMethod]
        public void TestMethod1()
        {
            var container = new ValueContainer() {Value = "1.1"};
            Assert.IsTrue(container.GetDoubleValue() == 1.1);
            container = null;
            Assert.IsTrue(container.GetDoubleValue() == 0);

        }
    }
}
