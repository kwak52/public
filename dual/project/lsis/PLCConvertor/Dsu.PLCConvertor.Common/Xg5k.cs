using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    public static class Xg5k
    {
        /// <summary>
        /// "XGRUNGSTART".  XG5000 의 Rung 단락 구분 명령.
        /// </summary>
        public static string XgRungStart => "XGRUNGSTART";

        /// <summary>
        /// "CMT".   XG5000 의 Rung comment 명령.  설명문
        /// </summary>
        public static string RungCommentCommand => "CMT";
    }
}
