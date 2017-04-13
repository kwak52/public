using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    internal class CTestClass
    {
        protected int m_nCounter = 0;
        protected string m_strName = null;

        public CTestClass(string name, int nCounter)
        {
            m_strName = name;
            m_nCounter = nCounter;
        }

        public string Name { get { return m_strName; } set { m_strName = value; } }

        static public implicit operator CTestClass(string strRep)
        {
            return new CTestClass(strRep, -1);
        }

        static public implicit operator string(CTestClass product)
        {
            return product.m_strName;
        }

        public override string ToString()
        {
            return m_strName;
        }
    }

    [TestClass]
    public class UtStringComparer
    {
        [TestMethod]
        public void TestMethodStringComparer()
        {
            CTestClass[] products = new CTestClass[]{
                    new CTestClass("one", 1),
                    new CTestClass("two", 2),
                    new CTestClass("three", 3),
                    new CTestClass("four", 4),
                };

            // testing as an array object
            bool b1 = products.Contains(new CTestClass("one", 0), new StringEqualityComparer<CTestClass>());
            Assert.IsTrue(b1);
            bool b2 = products.Contains(new CTestClass("two", 1));
            Assert.IsTrue(!b2);

            // build hashset
            HashSet<CTestClass> setProducts = new HashSet<CTestClass>(products, new StringEqualityComparer<CTestClass>());
            Assert.IsTrue(setProducts.Contains("one"));

            bool b3 = setProducts.Add(new CTestClass("one", 1));
            Assert.IsTrue(!b3);
            Assert.IsTrue(!setProducts.Contains("five"));
            bool b4 = setProducts.Add(new CTestClass("five", 5));
            Assert.IsTrue(b4);
            var vQuery = setProducts.Where(o => { return o.Name == "one"; });
            List<CTestClass> lstQuery = vQuery.ToList();
            var vdict = vQuery.ToDictionary(p => p.Name, p => p);

            //setProducts.Select((p, ))
            int nCount = setProducts.Count;
            bool b5 = setProducts.Remove("one");
            Assert.AreEqual(setProducts.Count, nCount - 1);
        }
    }
}
