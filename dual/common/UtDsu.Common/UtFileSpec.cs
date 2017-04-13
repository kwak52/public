using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    [TestClass]
    public class UtFileSpec
    {
        [TestMethod]
        public void TestMethodFileSpec()
        {
            DEBUG.WriteLine(FileSpec.Absolute2Relative(@"c:\test\hello\world.cs", @"c:\test\hello\"));

            Assert.AreEqual(FileSpec.Absolute2Relative(@"c:\test\hello\world.cs", @"c:\test\hello\"), "world.cs");
            string s = FileSpec.Absolute2Relative(@"c:\test\hello2\world.cs", @"c:\test\hello\");
            Assert.AreEqual(s, "../hello2/world.cs");

            string s2 = FileSpec.Absolute2Relative(@"c:\test\hello2\world.cs", @"c:\xxx\");
            Assert.AreEqual(s2, "../test/hello2/world.cs");

            string s3 = FileSpec.Absolute2Relative(@"c:\test\hello2\world.cs", @"d:\");
            Assert.AreEqual(s3, "c:/test/hello2/world.cs");

            string strPath = @"c:\hello.test.ext";
            FileInfo fi = new FileInfo(strPath);
            Assert.AreEqual(fi.Extension.ToLower(), ".ext");
            Assert.AreEqual(fi.Extension, Path.GetExtension(strPath));


            Assert.AreEqual(FileSpec.Absolute2Relative(@"x:\abc\def", @"x:\PQR\STU"), "../../abc/def");
            Assert.IsTrue(Path.IsPathRooted(@"x:\abc"));
            Assert.IsTrue(Path.IsPathRooted("x:"));
            Assert.IsTrue(Path.IsPathRooted("\\"));

            Assert.AreEqual(FileSpec.ChangeReferenceFolder("./abc", "./DEF"), "../abc");
            Assert.AreEqual(FileSpec.ChangeReferenceFolder("./abc/def", "./DEF"), "../abc/def");
            Assert.AreEqual(FileSpec.ChangeReferenceFolder("./abc/def", "./DEF/GHI"), "../../abc/def");

            Assert.AreEqual(FileSpec.ChangeReferenceFolder("abc", "./DEF"), "../abc");
            Assert.AreEqual(FileSpec.ChangeReferenceFolder("abc/def", "./DEF"), "../abc/def");
            Assert.AreEqual(FileSpec.ChangeReferenceFolder("abc/def", "./DEF/GHI"), "../../abc/def");

            Assert.AreEqual(FileSpec.ChangeReferenceFolder("abc", "DEF"), "../abc");
            Assert.AreEqual(FileSpec.ChangeReferenceFolder("abc/def", "DEF"), "../abc/def");
            Assert.AreEqual(FileSpec.ChangeReferenceFolder("abc/def", "DEF/GHI"), "../../abc/def");
        }
    }
}
