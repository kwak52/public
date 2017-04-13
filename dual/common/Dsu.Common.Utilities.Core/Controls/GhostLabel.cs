using System;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// http://stackoverflow.com/questions/547172/pass-through-mouse-events-to-parent-control
    /// </summary>
    public class GhostLabel : Label
    {
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);

            if (m.Msg == WM_NCHITTEST)
            {
                m.Result = (IntPtr)HTTRANSPARENT;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
