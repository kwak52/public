using System;

namespace Dsu.Common.Utilities
{
    public static class IntPtrUtil
    {
        public static IntPtr GetIntPtr(byte[] bytes)
        {
            unsafe
            {
                // TIPS : getting pointer from C# array
                // http://stackoverflow.com/questions/537573/how-to-get-intptr-from-byte-in-c-sharp
                fixed (byte* pByte = bytes)
                {
                    IntPtr hBuffer = (IntPtr)pByte;
                    return hBuffer;
                }
            }
        }


        public static IntPtr IncrementIntPtr(IntPtr oldptr, int nIncrement)
        {
            return new IntPtr(oldptr.ToInt64() + nIncrement);            
        }

    }
}
