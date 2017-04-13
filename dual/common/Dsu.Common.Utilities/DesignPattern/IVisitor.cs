using System;
using System.Reflection;

namespace Dsu.Common.Utilities
{
    public interface IVisitor
    {
        void Visit(object visitable);
    }

    // http://weblogs.asp.net/cazzu/pages/25488.aspx
    public abstract class CReflectionVisitorBase : IVisitor
    {
        private MethodInfo _lastmethod = null;
        private object _lastvisitable = null;
        public void Visit(object visitable)
        {
            try
            {
                MethodInfo method = this.GetType().GetMethod("Visit",
                               BindingFlags.ExactBinding | BindingFlags.Public | BindingFlags.Instance,
                               Type.DefaultBinder, new Type[] { visitable.GetType() }, new ParameterModifier[0]);
                if (method != null)
                {
                    // Avoid StackOverflow exceptions by executing only if the method and visitable  
                    // are different from the last parameters used.
                    if (method != _lastmethod || visitable != _lastvisitable)
                    {
                        _lastmethod = method;
                        _lastvisitable = visitable;
                        method.Invoke(this, new object[] { visitable });
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw ex;
            }
        }
    }
}
