// http://www.codeproject.com/Articles/24710/Multiple-Inheritance-in-C

using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    internal class AttributeAge : Attribute
    {
        public int Age { get; set; }
    }

    internal class AttributeName : Attribute, INamedObject
    {
        // TIPS: Setting a default value for C# Auto-implemented properties.
        // http://www.codeproject.com/Tips/310476/Setting-a-default-value-for-Csharp-Auto-implemente
        [DefaultValue("")]
        public string Name { get; set; }
    }

    /*
     * CPerson 은 AttributeName 및 AttributeAge 에서 다중 상속 받은 class
     */
    [AttributeName, AttributeAge]
    internal class CPerson : CDynamicClassBase
    {
    }

    internal class CUnNamedClassTest : CDynamicClassBase
    {
    }

    internal class AttributeTest
    {
        public static void Check()
        {
            CDynamicClassBase n1 = new CPerson();
            Assert.IsTrue(n1.Check<AttributeName>());
            n1.Use<AttributeName>().Name = "hello";
            n1.Use<AttributeAge>().Age = 18;
            Assert.AreEqual(n1.Use<AttributeName>().Name, "hello");
            Assert.AreEqual(n1.Use<AttributeAge>().Age, 18);

            CDynamicClassBase n2 = new CUnNamedClassTest();
            Assert.IsTrue(!n2.Check<AttributeName>());

            Assert.IsTrue(n1.Is<CPerson>());
            Assert.IsTrue(n2.Is<CUnNamedClassTest>());
        }
    }


    [TestClass]
    public class UtAttribute
    {
        [TestMethod]
        public void TestMethodAttribute()
        {
            AttributeTest.Check();
        }
    }
}
