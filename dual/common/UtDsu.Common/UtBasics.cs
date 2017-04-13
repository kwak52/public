using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

// TIPS : namespace abbreviation
using GP = Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtBasics
    {
        // TIPS : Nullable argument
        static bool NullableArg(Nullable<bool> b)
        {
            if (b.HasValue)
                return b.Value;
            else
            {
                DEBUG.WriteLine("NULL state bool");
                return false;
            }
        }

        [TestMethod]
        public void TestMethodNullableArg()
        {
            Assert.IsTrue(NullableArg(true));
            Assert.IsFalse(NullableArg(false));
            Assert.IsFalse(NullableArg(null));
        }

        [TestMethod]
        public void TestMethodStringSplitJoin()
        {
            string str1 = "ONE\t\tTwo\t\tThree";
            string[] items = str1.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(items.Length, 3);
            Assert.AreEqual(items[0], "ONE");
            Assert.AreEqual(items[1], "Two");
            Assert.AreEqual(items[2], "Three");
            Assert.AreEqual(string.Join(".", items), "ONE.Two.Three");

            items = str1.Split(new char[] { '\t' });
            Assert.AreEqual(items.Length, 5);
            Assert.AreEqual(items[0], "ONE");
            Assert.AreEqual(items[2], "Two");
            Assert.AreEqual(items[4], "Three");
        }

        [TestMethod]
        public void TestMethodTextFileParser()
        {
            var hosts = from line in TextFileParser.ReadLinesFromFile(@"C:\Windows\System32\drivers\etc\hosts")
                        where !String.IsNullOrEmpty(line) && line[0] != '#' && line[0] != '\r' && line[0] != '\n'
                        let item = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        select new
                        {
                            IP = item[0],
                            FQDN = item[1],
                            DN = item.Length >= 3 ? item[2] : ""
                        };
            foreach (var h in hosts)
            {
                DEBUG.WriteLine("IP={0}, FQDN={1}, DN={2}", h.IP, h.FQDN, h.DN);
            }
        }

        [TestMethod]
        public void TestMethodBasicsSnippet()
        {
            // TIPS : foreach using enumeration, like C-shell
            foreach (var s in new string[] { "a", "b", "c" })
                DEBUG.WriteLine(s);

            // TIPS : object[], string[], Array, List<T> type conversion
            string[] strs = new string[] { "a", "b", "c" };
            object[] objs = new string[] { "a", "b", "c" };
            Array astrs = new string[] { "a", "b", "c" };

            {
                string[] strs1 = strs;
                object[] objs1 = objs;
                object[] objs2 = strs;      // string[] --> object[] : OK
                //string[] strs2 = objs;      // object[] --> string[] : Compile error
                string[] strs3 = Tools.ToObjects<string>(objs);      // object[] --> string[] : OK
                Debug.Assert(Tools.AllEqual_p(strs1.Length, objs1.Length, objs2.Length, strs3.Length, 3));
            }

            {
                //string[] strs1 = astrs;     // Array --> string[] : Compile error
                //object[] objs1 = astrs;     // Array --> object[] : Compile error
                object[] objs2 = Tools.ToObjects(astrs);     // Array --> object[] : OK
                string[] strs2 = Tools.ToObjects<string>(objs2);   // object[] --> string[]
                Debug.Assert(Tools.AllEqual_p(objs2.Length, strs2.Length, 3));
            }

            {
                List<string> strs1 = strs.ToList();
                List<object> objs1 = objs.ToList(); // object[] --> List<object> : OK
                //List<object> objs1 = strs.ToList(); // string[] --> List<object> : Compile error
                List<object> objs2 = Tools.ToObjects(strs).ToList(); // string[] --> object[] --> List<object> : OK
                List<string> strs2 = Tools.ToList<string>(objs);    // object[] --> List<T> : OK

                Debug.Assert(Tools.AllEqual_p(strs1.Count, strs2.Count, objs1.Count, objs2.Count, 3));
            }
        }
    }
}
