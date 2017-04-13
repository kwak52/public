using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    public static class EmDrawing
    {
        /// <summary> Inverts a color </summary>
        /// http://stackoverflow.com/questions/1165107/how-do-i-invert-a-colour-color-c-net
        public static Color Invert(this Color cr)
        {
            return Color.FromArgb(cr.ToArgb() ^ 0xFFFFFF);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSize">e.g "{Width=200, Height=40}"</param>
        /// Size 가 class 가 아니라 structure 이므로, extension method 를 사용할 경우, 구조체가 복사되어 역할을 하지 못함.
        public static System.Drawing.Size SizeFromString(string strSize)
        {
            var match = Regex.Match(strSize, @"{Width=(\d+),\s*Height=(\d+)}");
            if ( match.Groups.Count != 3 )
                throw new UnexpectedCaseOccurredException(String.Format("Invalid format for size :'{0}'", strSize));

            var w = Int32.Parse(match.Groups[1].ToString());
            var h = Int32.Parse(match.Groups[2].ToString());

            return new Size(w, h);
        }


        public static System.Drawing.Point PointFromString(string strPoint)
        {
            var match = Regex.Match(strPoint, @"{X=(\d+),\s*Y=(\d+)}");
            if (match.Groups.Count != 3)
                throw new UnexpectedCaseOccurredException(String.Format("Invalid format for point :'{0}'", strPoint));

            var x = Int32.Parse(match.Groups[1].ToString());
            var y = Int32.Parse(match.Groups[2].ToString());
            return new Point(x, y);
        }

        public static string ColorToString(System.Drawing.Color cr)
        {
            return cr.ToArgb().ToString("X8");
        }

        public static System.Drawing.Color ColorFromString(string strColor)
        {
            return Color.FromArgb(int.Parse(strColor, NumberStyles.HexNumber));
        }

    }
}
