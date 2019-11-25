using System;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// Well-known convertor exception
    /// </summary>
    internal class ConvertorException : Exception
    {
        public ConvertorException(string message)
            : base(message)
        {
        }
    }
}
