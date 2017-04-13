using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtGUID
    {
        [TestMethod]
        public void TestMethodGUID()
        {
            DEBUG.WriteLine(Guid.NewGuid().ToString());

            Guid iid = CGuid.GetGuidFromInterface(typeof(INamedObject));
            DEBUG.WriteLine(iid.ToString());
        }
    }
}
