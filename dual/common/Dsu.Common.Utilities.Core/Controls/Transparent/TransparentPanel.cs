using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Creating a transparent Panel in .NET : http://www.fsmpi.uni-bayreuth.de/~dun3/archives/creating-a-transparent-panel-in-net/108.html
    /// http://bytes.com/topic/c-sharp/answers/248836-need-make-user-control-transparent
    /// </summary>
    public class TransparentPanel : Panel
    {
        //public TransparentPanel()
        //{
        //    SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        //    BackColor = Color.FromArgb(10, Color.Green);
        //}

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background.
        }
    }
}
