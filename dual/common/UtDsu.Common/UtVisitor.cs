using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    internal class CVisitorPrint : CReflectionVisitorBase
    {
        public void Visit(DateTime dt)
        {
            DEBUG.WriteLine("Visitor(DateTime dt) : {0}", dt);
        }

        public void Visit(string str)
        {
            DEBUG.WriteLine("Visitor(string str) : {0}", str);
        }

        public void Visit(string[] astr)
        {
            DEBUG.WriteLine("Visitor(string[] astr) : {0}", string.Join(" ", astr));            
        }

        /* 다음 함수는 존재하면 reflection 기반 visiting 이 불가능하다. */
        //public void Visit(object o)
        //{
        //    DEBUG.WriteLine("Visitor : {0}", o);
        //}
    }

    // 대조군
    internal class CVisitorContrast
    {
        public void Visit(DateTime str)
        {
            DEBUG.WriteLine("Visitor Object: {0}", str);
        }

        public void Visit(string str)
        {
            DEBUG.WriteLine("Visitor : {0}", str);
        }
    }


    [TestClass]
    public class UtVisitor
    {
        [TestMethod]
        public void TestMethodVisitor()
        {
            CVisitorPrint visitor = new CVisitorPrint();

            object[] visitables = { DateTime.Now, "hello", DateTime.Now, "world", new CVisitorPrint(), new string[]{"my", "name"} };
            foreach (object ovisitable in visitables)
                visitor.Visit(ovisitable);

            /*
             * 다음 코드는 컴파일 오류 발생
             */
            //CVisitorContrast visitorOther = new CVisitorContrast();
            //foreach (object ovisitable in visitables)
            //    visitorOther.Visit(ovisitable);      // error CS1503: Argument 1: cannot convert from 'object' to 'System.DateTime'


            DateTime dt = DateTime.Now;
            string s = "hello";
            visitor.Visit(s);
            visitor.Visit(s);
            visitor.Visit(dt as object);
            visitor.Visit(dt as object);
        }
    }
}
