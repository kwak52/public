using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Test
{
    [TestClass]
    public class UnitTestFormatProvider
    {
        [TestMethod]
        public void TestMethodNumberFormat()
        {
        }

        [TestMethod]
        public void TestMethodSSN()
        {
            SocialSecurityNumber ssn = new SocialSecurityNumber("123-45-6789");
            Debug.WriteLine(ssn);

            Debug.WriteLine(string.Format("No Dashes: {0:nd}", ssn));

            Debug.WriteLine(string.Format(new MySSNFormatProvider(), "{0:secure}", ssn));
        }
    }

    /// <summary>
    /// http://codebetter.com/davidhayden/2006/03/12/open-closed-principle-iformattable-iformatprovider-icustomformatter/
    /// </summary>
    public class SocialSecurityNumber : IFormattable
    {
        private string _rawNumber;

        public SocialSecurityNumber(string number)
        {
            _rawNumber = number;
        }

        public override string ToString()
        {
            return ToString("G", null);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            SocialSecurityNumber ssn = obj as SocialSecurityNumber;

            if (ssn == null) return false;
            return (this._rawNumber.Equals(ssn._rawNumber));
        }

        public override int GetHashCode()
        {
            return _rawNumber.GetHashCode();
        }

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider != null)
            {
                ICustomFormatter formatter = formatProvider.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                    return formatter.Format(format, this, formatProvider);
            }

            if (format == null)
                format = "G";

            switch (format)
            {
                case "nd": return _rawNumber.Replace("-", "");
                case "G":
                default: return _rawNumber;
            }
        }

        #endregion IFormattable Members
    }

    public class MySSNFormatProvider : IFormatProvider, ICustomFormatter
    {
        #region IFormatProvider Members

        public object GetFormat(Type formatType)
        {
            return this;
        }

        #endregion IFormatProvider Members

        #region ICustomFormatter Members

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "secure":
                default: return "***-**-" + arg.ToString().Substring(7);
            }
        }

        #endregion ICustomFormatter Members
    }
}