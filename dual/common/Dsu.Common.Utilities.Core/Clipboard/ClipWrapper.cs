﻿using System.Runtime.InteropServices;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// 객체 복사를 위한 wrapper class
    /// </summary>
    [ComVisible(false)]
    public class ClipWrapper<T> : IIntraProgramClipWrapper where T: class
    {
        /// <summary>
        /// 실제의 node data 를 갖는다.
        /// </summary>
        public object ClipData { get; private set; }

        public ClipWrapper(T node)
        {
            ClipData = node;
        }
    }
}
