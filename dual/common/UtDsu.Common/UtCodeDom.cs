using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace CSharp.Test
{
    /// <summary>
    /// http://www.codeproject.com/Articles/26312/Dynamic-Code-Integration-with-CodeDom
    /// </summary>
    [TestClass]
    public class UtCodeDom
    {
        [TestMethod]
        public void TestMethodCodeDom()
        {
            string code = @"
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    public class MyDisposable : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine(""XXX-th MyDisposable::Dispose() called."");
        }
    }
";

            for (int i = 0; i < 5; i++)
            {
                using (CodeDOMParser parser = new CodeDOMParser() { References = new string[] { "System.dll", "System.Windows.Forms.dll" } })
                {
                    var codeNth = code.Replace("XXX", i.ToString());
                    Assembly assembly = parser.CompileCode(codeNth);
                    Type myDisposableType = assembly.GetTypes()[0];
                    Type myDisposableType1 = assembly.GetType("MyDisposable");
                    Assert.IsTrue(myDisposableType == myDisposableType1);

                    Type disposableType = myDisposableType.GetInterface("System.IDisposable");
                    Debug.WriteLine(myDisposableType.ToString());

                    Assert.IsTrue(myDisposableType.FullName == "MyDisposable");
                    IDisposable disposable = (IDisposable)assembly.CreateInstance(myDisposableType.FullName);

                    disposable.Dispose();


                    foreach (var m in Reflection.GetMethods(myDisposableType,
                        BindingFlags.DeclaredOnly
                        | BindingFlags.Instance
                        | BindingFlags.Public
                        ))
                    {
                        Debug.WriteLine(m);
                    }
                }
            }
        }
    }
}
