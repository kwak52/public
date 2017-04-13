using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// http://www.tigranetworks.co.uk/blogs/electricdreams/the-evil-of-mutable-structs/

namespace CSharp.Test
{
    internal struct MutableStruct
    {
        public int Value { get; set; }

        public void AssignValue(int newValue)
        {
            Value = newValue;
        }
    }

    [TestClass]
    public class UnitTestEvilStructMutability
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = new List<MutableStruct>
            {
                new MutableStruct {Value = 10}
            };

            foreach (var item in list)
            {
                item.AssignValue(30);

                //item.Value = 30;        // error CS1654: Cannot modify members of 'item' because it is a 'foreach iteration variable'
            }

            Assert.IsTrue(list[0].Value == 10);
            Assert.IsTrue(list[0].Value != 30);
        }
    }
}
