using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DiscriminatedUnions;

namespace CSharp.Test
{
    [TestClass]
    public class UtFSharpInterop
    {
        [TestMethod]
        public void TestMethodDiscriminatedUnion()
        {
            var unionN = SuccessCode<int>.NewSome(1);
            var t = unionN.GetType();
            Assert.IsTrue(unionN.IsSome);
            int n = unionN.GetSome();
            Assert.AreEqual(n, 1);

            var unionS = SuccessCode<string>.NewSome("hello");
            Assert.IsTrue(unionS.IsSome);
            Assert.AreEqual(unionS.GetSome(), "hello");

            var message = "Failed to do something";
            var exception = SuccessCode<int>.NewException(new Exception(message));
            Assert.IsTrue(exception.IsException);
            var ex = exception.GetException();
            Assert.IsTrue(ex.Message == message);
        }
    }
}
