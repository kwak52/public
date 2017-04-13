using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test.WithUI
{
    [TestClass]
    public class UtFormRadioSelector
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            // for Interaction.InputBox, needs System.Windows.Form and Microsoft.VisualBasic
            AutoClosingMessageBox.Show("this is essential", 30);
        }

        [TestMethod]
        public void TestMethodFormRadioSelector()
        {
            var frm = new FormRadioSelector(typeof(DialogResult));
            object result = frm.DoModalGetResult();
            if (result != null)
            {
                DialogResult eResult = (DialogResult)result;
                MessageBox.Show("You selected " + eResult);
            }
        }

        [TestMethod]
        public void TestMethodFormRadioSelector2()
        {
            var ary = new string[] {"hello", "world", "!!!"};
            var frm = new FormRadioSelector(ary);
            object result = frm.DoModalGetResult();
            if (result != null)
            {
                MessageBox.Show("You selected " + result);
            }
        }

    }
}
