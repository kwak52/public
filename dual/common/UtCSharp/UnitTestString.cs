using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    class Sentence
    {
        string[] words = "The quick brown fox".Split();
        public string this[int wordNum] // indexer
        {
            get { return words[wordNum]; }
            set { words[wordNum] = value; }
        }
    }
    
    
    [TestClass]
    public class UnitTestString
    {
        [TestMethod]
        public void TestMethod1()
        {
            foreach (char c in "beer") // c is the iteration variable
                Debug.WriteLine(c);
        }

        [TestMethod]
        public void TestStringIndexer()
        {
            string str = "hello";
            Assert.IsTrue(str[0] == 'h');
            Assert.IsTrue(str[3] == 'l');

            Sentence s = new Sentence();
            Assert.IsTrue(s[3] == "fox");
            s[3] = "kangaroo";
            Assert.IsTrue(s[3] == "kangaroo");
        }
    }


    // readonly vs const
    internal class Test
    {
        //private readonly int i = 0;
        //private const int j = 1;

//const int a

//    must be initialized
//    initialization must be at compile time

//readonly int a

//    can use default value, without initializing
//    initialization can be at run time
    }
}
