using System;

namespace Dsu.Common.Utilities
{    
    public static class BooleanParser
    {
        public static bool? Parse(string strBool)
        {
            bool boolResult;
            int intResult;
            if (Boolean.TryParse(strBool, out boolResult))
                return boolResult;
            if (Int32.TryParse(strBool, out intResult))
                return intResult != 0;

            return null;
        }
    }

    /// <summary>
    /// see BooleanHolder/BooleanSetter for safe implementation
    /// </summary>
    unsafe public class CBooleanEnabler : IDisposable
    {
        protected bool* m_pb = null;

        public CBooleanEnabler(bool* pb)
        {
            if (*pb == false)
            {
                *pb = true;
                m_pb = pb;
            }
        }

        public void Dispose()
        {
            if ( m_pb != null )
                *m_pb = false;
        }
    }

    unsafe public class CBooleanDisabler : IDisposable
    {
        protected bool* m_pb = null;

        public CBooleanDisabler(bool* pb)
        {
            if (*pb == true)
            {
                *pb = false;
                m_pb = pb;
            }
        }

        public void Dispose()
        {
            if (m_pb != null)
                *m_pb = true;
        }
    }

    unsafe public class CBooleanToggler : IDisposable
    {
        protected bool* m_pb = null;
        protected bool m_bV = false;

        public CBooleanToggler(bool* pb)
        {
            m_pb = pb;
            m_bV = *pb;
            *pb = !*pb;
        }

        public void Dispose()
        {
            *m_pb = m_bV;
        }
    }
}
