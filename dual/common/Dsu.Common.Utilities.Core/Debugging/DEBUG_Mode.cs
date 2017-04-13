// http://www.codeproject.com/Tips/323212/Accurate-way-to-tell-if-an-assembly-is-compiled-in


namespace Dsu.Common.Utilities
{
    partial class DEBUG
    {
        public static bool DebugMode_p(string FileName) { return DebugMode_p(FileName, false); }
        public static bool DebugMode_p(string FileName, bool bAssemlbyName/*=false*/)
        {
            System.Reflection.Assembly assembly;
            if (bAssemlbyName)
                assembly = System.Reflection.Assembly.Load(FileName);
            else
                assembly = System.Reflection.Assembly.LoadFile(FileName);
            return DebugMode_p(assembly);
        }

        public static bool DebugMode_p(System.Reflection.Assembly Assembly)
        {
            var attributes = Assembly.GetCustomAttributes(typeof(System.Diagnostics.DebuggableAttribute), false);
            if (attributes.Length > 0)
            {
                var debuggable = attributes[0] as System.Diagnostics.DebuggableAttribute;
                if (debuggable != null)
                    return (debuggable.DebuggingFlags & System.Diagnostics.DebuggableAttribute.DebuggingModes.Default) == System.Diagnostics.DebuggableAttribute.DebuggingModes.Default;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
