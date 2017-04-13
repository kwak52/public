using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Dsu.Common.Utilities.Core;
using Dsu.Common.Utilities.Core.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    /// <summary>
    /// http://www.devx.com/vb2themax/Tip/18767
    /// </summary>
    [TestClass]
    public class UtBitmask
    {
        [TestMethod]
        public void UtBitUInt64()
        {
            ulong value = 0x123456789ABCDE;
            Assert.IsTrue(value.GetBitsValue(0, 7) == 0xDE);
            Assert.IsTrue(value.GetBitsValue(0, 3) == 0xE);
            Assert.IsTrue(value.GetBitsValue(0, 11) == 0xCDE);

            Assert.IsTrue(value.GetBitsValue(4, 7) == 0xD);
            Assert.IsTrue(value.GetBitsValue(4, 11) == 0xCD);
            Assert.IsTrue(value.GetBitsValue(0, 63) == 0x123456789ABCDE);


            Assert.IsTrue(value.GetBits(0, 7) == 0xDE);
            Assert.IsTrue(value.GetBits(0, 3) == 0xE);
            Assert.IsTrue(value.GetBits(0, 11) == 0xCDE);

            Assert.IsTrue(value.GetBits(4, 7) == 0xD0);
            Assert.IsTrue(value.GetBits(4, 11) == 0xCD0);
            Assert.IsTrue(value.GetBits(0, 63) == 0x123456789ABCDE);


            Assert.IsFalse(value.GetBit(0));
            Assert.IsTrue(value.GetBit(1));
        }

        [TestMethod]
        public void UtBitUInt32()
        {
            uint value = 0x13579ACE;
            Assert.IsTrue(value.GetBitsValue(0, 7) == 0xCE);
            Assert.IsTrue(value.GetBitsValue(0, 3) == 0xE);
            Assert.IsTrue(value.GetBitsValue(0, 11) == 0xACE);

            Assert.IsTrue(value.GetBitsValue(4, 7) == 0xC);
            Assert.IsTrue(value.GetBitsValue(4, 11) == 0xAC);
            Assert.IsTrue(value.GetBitsValue(0, 63) == 0x13579ACE);


            Assert.IsTrue(value.GetBits(0, 7) == 0xCE);
            Assert.IsTrue(value.GetBits(0, 3) == 0xE);
            Assert.IsTrue(value.GetBits(0, 11) == 0xACE);

            Assert.IsTrue(value.GetBits(4, 7) == 0xC0);
            Assert.IsTrue(value.GetBits(4, 11) == 0xAC0);
            Assert.IsTrue(value.GetBits(0, 63) == 0x13579ACE);


            Assert.IsFalse(value.GetBit(0));
            Assert.IsTrue(value.GetBit(1));
        }


        [TestMethod]
        public void UtBitConverter()
        {
            var bytes = new byte[] {0xfe, 0xef, 0xfe, 0xef};
            uint value = BitConverter.ToUInt32(bytes, 0);
            Assert.IsTrue(value == 0xeffeeffe);
        }

        [TestMethod]
        public void UtBitBigInteger()
        {
            var hugeDecimal = BigInteger.Parse("123456789012345678901234567890123456789012345678901234567890");
            var hugeHex = BigInteger.Parse("00FF00FF00FF00FF00FF00FF00FF00FF00FF00FF00FF00FF00FF00FF00FF00FF", NumberStyles.AllowHexSpecifier);

            var bigNumber = new BigInteger(Enumerable.Range(0, 31).Select(n => (byte)n).ToArray());
            dynamic bits = bigNumber.GetNonPublicField("_bits");
            foreach (var bit in bits)
                Trace.WriteLine($"0X{((uint)bit).ToString("X")}");
        }
    }
}
