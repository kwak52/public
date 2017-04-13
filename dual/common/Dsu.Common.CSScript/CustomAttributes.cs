using System;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Scripting 으로 사용할 class 에 적용하는 attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ScriptClass : Attribute
    {
    }

    /// <summary>
    /// Scripting 으로 사용할 method 에 적용하는 attribute
    /// <para/> - [ScriptClass] attribute 가 적용된 class 에서만 의미를 갖는다.
    /// <para/> - http://www.dotnetperls.com/attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ScriptMethod : Attribute
    {
        public ScriptMethod() { }
        public ScriptMethod(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; private set; }
    }

    /// <summary>
    /// Dynamic feature 사용 등으로 method 이름이 변경되어서는 안되는 경우를 표시
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class NameCritical : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = true, Inherited = false)]
    public class MethodArgumentsCritical : Attribute
    {
    }


    /// <summary>
    /// NameCritical + MethodArgumentsCritical
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = true, Inherited = false)]
    public class MethodSignaturesCritical : Attribute
    {
    }

}
