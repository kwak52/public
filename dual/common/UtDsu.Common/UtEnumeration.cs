using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.Core.ExtensionMethods;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtEnumeration
    {
        [TestMethod]
        public void TestMethodEnumeration()
        {
			{
				int i = 0;
				List<DialogResult> lst = Tools.GetEnumerationList<DialogResult>();
				Assert.IsTrue(lst != null && lst.Count == 8);
				Assert.AreEqual(lst[i++], DialogResult.None);
				Assert.AreEqual(lst[i++], DialogResult.OK);
				Assert.AreEqual(lst[i++], DialogResult.Cancel);
				Assert.AreEqual(lst[i++], DialogResult.Abort);
				Assert.AreEqual(lst[i++], DialogResult.Retry);
				Assert.AreEqual(lst[i++], DialogResult.Ignore);
				Assert.AreEqual(lst[i++], DialogResult.Yes);
				Assert.AreEqual(lst[i++], DialogResult.No);
			}

	        {
				int i = 0;
				List<DialogResult> lst = EmEnum.GetValues<DialogResult>().ToList();
				Assert.IsTrue(lst != null && lst.Count == 8);
				Assert.AreEqual(lst[i++], DialogResult.None);
				Assert.AreEqual(lst[i++], DialogResult.OK);
				Assert.AreEqual(lst[i++], DialogResult.Cancel);
				Assert.AreEqual(lst[i++], DialogResult.Abort);
				Assert.AreEqual(lst[i++], DialogResult.Retry);
				Assert.AreEqual(lst[i++], DialogResult.Ignore);
				Assert.AreEqual(lst[i++], DialogResult.Yes);
				Assert.AreEqual(lst[i++], DialogResult.No);
			}


			Array dialogs = Enum.GetValues(typeof (DialogResult));

            Assert.AreEqual(DialogResult.None, EnumerationConverter.GetEnumValue<DialogResult>("None"));
            Assert.AreEqual(DialogResult.OK, EnumerationConverter.GetEnumValue<DialogResult>("OK"));
            Assert.AreEqual(DialogResult.Cancel, EnumerationConverter.GetEnumValue<DialogResult>("Cancel"));
            Assert.AreEqual(DialogResult.Abort, EnumerationConverter.GetEnumValue<DialogResult>("Abort"));
        }
    }
}
