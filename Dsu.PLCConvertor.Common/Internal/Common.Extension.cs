using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    public static class CommonExtension
    {
        public static void PushMultiples<T>(this Stack<T> stack, IEnumerable<T> elements)
        {
            elements.Reverse().Iter(e => stack.Push(e));
        }
    }
}
