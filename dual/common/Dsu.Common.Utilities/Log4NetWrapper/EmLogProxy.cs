using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.Common.Utilities
{
    public static class EmLogProxy
    {
        public static void EnableAll(this LogProxy p, bool enable=true)
        {
            p.IsEnableDebug = p.IsEnableWarn = p.IsEnableInfo = enable;
        }
        public static void EnableWarn(this LogProxy p, bool enable = true)
        {
            p.IsEnableWarn = enable;
        }

        public static void DisableAll(this LogProxy p)
        {
            p.IsEnableDebug = p.IsEnableWarn = p.IsEnableInfo = false;
        }
    }
}
