using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtReflection
    {
        [TestMethod]
        public void TestMethodReflection_CallStack()
        {
            Assert.AreEqual(System.Reflection.MethodBase.GetCurrentMethod().Name, "TestMethodReflection_CallStack");
            Assert.AreEqual(DEBUG.CallStackGetNthName(0), "TestMethodReflection_CallStack");
            foreach (string f in DEBUG.CallStackGetAllFramesInfo())
                DEBUG.WriteLine(f);

            /* This will bug you due to dialog box */
            //Debugging.StackTrace("Test stack trace");
        }


        [TestMethod]
        public void TestMethodReflection()
        {
            Type t = typeof(Form);

            Debug.WriteLine(Reflection.GetTypeInfo(t));

            Debug.WriteLine("------ Methods ------");
            foreach (string mi in Reflection.GetMethods(t, BindingFlags.Public | BindingFlags.Static))
                Debug.WriteLine(mi);

            Debug.WriteLine("------ Members ------");
            foreach (string mi in Reflection.GetMembers(t))
                Debug.WriteLine(mi);

            Debug.WriteLine("------ Fields ------");
            foreach (string mi in Reflection.GetFields(t))
                Debug.WriteLine(mi);

            Debug.WriteLine("------ Properties ------");
            foreach (string mi in Reflection.GetProperties(t))
                Debug.WriteLine(mi);

            Debug.WriteLine("------ Interfaces ------");
            foreach (string mi in Reflection.GetInterfaces(t))
                Debug.WriteLine(mi);

            Debug.WriteLine("------ Constructors ------");
            foreach (string mi in Reflection.GetConstructors(t))
                Debug.WriteLine(mi);
        }

    }




}
