using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestFileInfo
    {
        [TestMethod]
        public void TestMethodFileInfo()
        {
            var fi = new FileInfo(@"C:\temp\temp2\foo\bar.baz.txt");

            Trace.WriteLine("DirectoryName = {0}", fi.DirectoryName); // C:\temp\temp2\foo
            Trace.WriteLine("Extension = {0}", fi.Extension); // .txt
            Trace.WriteLine("FullName = {0}", fi.FullName); // C:\temp\temp2\foo\bar.baz.txt
            Trace.WriteLine("Name = {0}", fi.Name); // bar.baz.txt

            DirectoryInfo di = fi.Directory;
            Trace.WriteLine("Directory.Name = {0}", di.Name); // foo
            Trace.WriteLine("Directory.Parent.Name = {0}", di.Parent.Name); // temp2
            Trace.WriteLine("Directory.Parent.Parent.Name = {0}", di.Parent.Parent.Name); // temp
            Trace.WriteLine("Directory.Parent.Parent.Parent.Name = {0}", di.Parent.Parent.Parent.Name); // C:\
            //Trace.WriteLine("Directory.Parent.Parent.Parent.Parent.Name = {0}", di.Parent.Parent.Parent.Parent.Name);     // Null exception
            Trace.WriteLine("Diectory.Root = {0}", di.Root.ToString()); // C:\


            Trace.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            Trace.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
        }


        [TestMethod]
        public void TestMethodFileInfoRelative()
        {

            var fi = new FileInfo(@".\temp\temp2\foo\bar.baz.txt");

            DEBUG.WriteLine("DirectoryName = {0}", fi.DirectoryName);           // C:\temp\temp2\foo
            DEBUG.WriteLine("Extension = {0}", fi.Extension);                   // .txt
            DEBUG.WriteLine("FullName = {0}", fi.FullName);                     // C:\temp\temp2\foo\bar.baz.txt
            DEBUG.WriteLine("Name = {0}", fi.Name);                             // bar.baz.txt

            DirectoryInfo di = fi.Directory;
            DEBUG.WriteLine("Directory.Name = {0}", di.Name);                   // foo
            DEBUG.WriteLine("Directory.Parent.Name = {0}", di.Parent.Name);     // temp2
            DEBUG.WriteLine("Directory.Parent.Parent.Name = {0}", di.Parent.Parent.Name);     // temp
            DEBUG.WriteLine("Directory.Parent.Parent.Parent.Name = {0}", di.Parent.Parent.Parent.Name);     // C:\
            //DEBUG.WriteLine("Directory.Parent.Parent.Parent.Parent.Name = {0}", di.Parent.Parent.Parent.Parent.Name);     // Null exception
            DEBUG.WriteLine("Diectory.Root = {0}", di.Root);                    // C:\
                       
        }

        [TestMethod]
        public void TestMethodPath()
        {
            DEBUG.WriteLine("GetPathRoot = {0}", Path.GetPathRoot("./a/b/c.txt"));
            DEBUG.WriteLine("GetPathRoot = {0}", Path.GetPathRoot("../a/b/c.txt"));

            DEBUG.WriteLine("GetPathRoot = {0}", Path.GetPathRoot("/a/b/c.txt"));
            DEBUG.WriteLine("GetPathRoot = {0}", Path.GetPathRoot("a/b/c.txt"));
        }
    }
}
