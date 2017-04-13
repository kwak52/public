// http://www.codeproject.com/Articles/25510/Restore-Form-Position-and-Size-in-C

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace Dsu.Common.Utilities
{
    public class ComponentGeometrySerializer
    {
        public static void ApplyGeometryString(Component comp, string thisWindowGeometry)
        {
            if (string.IsNullOrEmpty(thisWindowGeometry))
                return;

            Control ctrl = comp as Control;
            Form frm = comp as Form;
            if (ctrl == null) return;

            string[] numbers = thisWindowGeometry.Split('|');
            string windowString = numbers[4];
            if (String.IsNullOrEmpty(windowString) || windowString == "Normal")
            {
                Point windowPoint = new Point(int.Parse(numbers[0]),
                    int.Parse(numbers[1]));
                Size windowSize = new Size(int.Parse(numbers[2]),
                    int.Parse(numbers[3]));

                bool locOkay = ValidGeometry_p(windowPoint, windowSize);
                bool sizeOkay = ValidGeometry_p(windowSize);

                if (locOkay == true && sizeOkay == true)
                {
                    ctrl.Location = windowPoint;
                    ctrl.Size = windowSize;
                    if (frm != null)
                    {
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.WindowState = FormWindowState.Normal;
                    }
                }
                else if (sizeOkay == true)
                {
                    ctrl.Size = windowSize;
                }
            }
            else if (windowString == "Maximized")
            {
                ctrl.Location = new Point(100, 100);
                if (frm != null)
                {
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.WindowState = FormWindowState.Maximized;
                }
            }
        }

        public static string GetGeometryString(Component comp)
        {
            Control ctrl = comp as Control;
            if (ctrl == null) return null;

            Form frm = ctrl as Form;
            return ctrl.Location.X.ToString() + "|" +
                ctrl.Location.Y.ToString() + "|" +
                ctrl.Size.Width.ToString() + "|" +
                ctrl.Size.Height.ToString() + "|" +
                (frm == null ? "" : frm.WindowState.ToString());
        }

        private static bool ValidGeometry_p(Point loc, Size size)
        {
            return loc.X >= 0 && loc.Y >= 0
                && loc.X + size.Width <= Screen.PrimaryScreen.WorkingArea.Width
                && loc.Y + size.Height <= Screen.PrimaryScreen.WorkingArea.Height;
        }

        private static bool ValidGeometry_p(Size size)
        {
            return (size.Height <= Screen.PrimaryScreen.WorkingArea.Height &&
                size.Width <= Screen.PrimaryScreen.WorkingArea.Width);
        }
    }
}
