using System;


namespace Dsu.Common.Utilities
{
    public partial class CGuid
    {
        // GetGuidFromInterface(typeof(IFoo));
        public static Guid GetGuidFromInterface(Type t)
        {
            return System.Runtime.InteropServices.Marshal.GenerateGuidForType(t);
        }

        public static Guid NewGuid() { return Guid.NewGuid();  }
    }
}