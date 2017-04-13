using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.Common.Utilities;

namespace GeneralPurpose.Test
{
    //sample test class
    public class CDayOfWeekSample
    {
        public enum EnumDayOfWeekE
        {
            SUN, MON, TUE, WED, THR, FRI, SAT,
        }

        public enum EnumDayOfWeekK
        {
            일, 월, 화, 수, 목, 금, 토,
        }
    }


    [TestClass]
    public class UtEnumerationExtractor
    {
        [TestMethod]
        public void TestMethodEnumeration_Extractor()
        {
            List<Type> lstTypes = Tools.EnumerableToList(EnumerationExtractor.GetInnerEnumTypes(typeof (CDayOfWeekSample)));
            foreach( Type typeEnum in lstTypes )
            {
                string strTypeName = typeEnum.ToString();
                Array a = Enum.GetValues(typeEnum);
                object[] objs = a.Cast<object>().ToArray();

                /*
                 * CDayOfWeekSample 의 내부 enum 을 미리 알지 못하는 경우, Reflection 을 통해서 가져와서 print = EnumerationExtractor.GetInnerEnumTypes()
                 */
                DEBUG.WriteLine("CDayOfWeekSample has enum type : {0}", strTypeName);
                foreach (var day in a)
                    DEBUG.WriteLine("\t{0}", day.ToString());

                List<CDayOfWeekSample.EnumDayOfWeekE> daysE;
                List<CDayOfWeekSample.EnumDayOfWeekK> daysK;

                if (typeEnum == typeof (CDayOfWeekSample.EnumDayOfWeekE))
                {
                    daysE = a.Cast<CDayOfWeekSample.EnumDayOfWeekE>().ToList();
                    Assert.AreEqual(daysE[0], CDayOfWeekSample.EnumDayOfWeekE.SUN);
                }
                else if (typeEnum == typeof (CDayOfWeekSample.EnumDayOfWeekK))
                {
                    daysK = a.Cast<CDayOfWeekSample.EnumDayOfWeekK>().ToList();
                    Assert.AreEqual(daysK[0], CDayOfWeekSample.EnumDayOfWeekK.일);
                }
            }

            {
                /*
                 * CDayOfWeekSample 의 내부 enum 을 미리 안다고 가정할 경우.
                 */
                string[] daysE = Enum.GetNames(typeof(CDayOfWeekSample.EnumDayOfWeekE));
                string[] daysK = Enum.GetNames(typeof(CDayOfWeekSample.EnumDayOfWeekK));
                Assert.AreEqual(daysE[0], CDayOfWeekSample.EnumDayOfWeekE.SUN.ToString());
                Assert.AreEqual(daysK[0], CDayOfWeekSample.EnumDayOfWeekK.일.ToString());
            }
        }
    }
}
