using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//
// Essential LINQ, pp.67
//
namespace CSharp.Test
{
    /// <summary>
    /// Partial method (MyPartialMethod) 가 선언되어 있으나, 정의되지 않은 경우
    /// </summary>
    public partial class PartialClassWithoutReimplementation
    {
        partial void MyPartialMethod();
        public void DoIt() {  MyPartialMethod(); }
    }

    /// <summary>
    /// Part1 : partial method 선언
    /// </summary>
    public partial class PartialClassWithReimplementation
    {
        partial void MyPartialMethod();
        public void DoIt() { MyPartialMethod(); }
    }

    /// <summary>
    /// Part2 : partial method 정의
    /// </summary>
    public partial class PartialClassWithReimplementation
    {
        partial void MyPartialMethod() {  Trace.WriteLine("Done!");}
    }

    [TestClass]
    public class UnitTestPartialMethod
    {
        [TestMethod]
        public void TestMethodPartialMethod()
        {
            new PartialClassWithoutReimplementation().DoIt();       // 정의되지 않은 partial method 는 compiler 에 의해서 무시된다.
            new PartialClassWithReimplementation().DoIt();          // 재정의된 partial method 가 존재하면 실행된다.
        }
    }
}
