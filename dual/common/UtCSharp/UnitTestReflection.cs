using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestReflection
    {
        private async void PrivateDoAsync()
        {
            await Task.Delay(1000);
        }
        public async Task PublicDoAsync()
        {
            Trace.WriteLine("PublicDoAsync");
            await Task.Delay(1000);
        }

        public void Sync()
        {
            
        }

        [TestMethod]
        public async Task TestMethodIsAsync()
        {
            Assert.IsTrue(this.GetType().IsAsyncMethod("PrivateDoAsync"));
            Assert.IsTrue(this.GetType().IsAsyncMethod("PublicDoAsync"));
            Assert.IsFalse(this.GetType().IsAsyncMethod("Sync"));

            MethodInfo mi = this.GetType().GetMethod("PublicDoAsync");
            await (Task)mi.Invoke(this, null);
            Trace.WriteLine("Awaited.");
        }
    }
}
