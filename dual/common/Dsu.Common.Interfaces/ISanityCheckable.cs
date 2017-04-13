using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// 정합성 checking 이 필요한 객체가 구현해야할 interface
    /// </summary>
    [ComVisible(false)]
    public interface ISanityCheckable
    {
        /// <summary> 정합성 check </summary>
        /// <param name="stopOnFirstError"> 최초 error 발생시 stop 할지 진행할지 여부</param>
        /// <returns>정합성 문제 있는 부분을 문자열로 반환</returns>
        string CheckSanity(bool stopOnFirstError);
        /// <summary> 하위 checkable items  </summary>
        IEnumerable<ISanityCheckable> SanityCheckables { get; }
    }


    /// <summary>
    /// 정합성 checking 기본 구현 class
    /// </summary>
    public class SanityCheckableImpl
    {
        private ISanityCheckable _checkable;

        /// <summary> 생성자 </summary>
        public SanityCheckableImpl(ISanityCheckable checkable)
        {
            _checkable = checkable;
        }

        /// <summary> 정합성 check </summary>
        public virtual string CheckSanity(bool stopOnFirstError = true)
        {
            string error = String.Empty;
            foreach (var c in _checkable.SanityCheckables)
            {
                error += c.CheckSanity(stopOnFirstError);
                if (error.Any())
                {
                    if (stopOnFirstError)
                        return error;
                }
            }

            return error;
        }
        
    }
}
