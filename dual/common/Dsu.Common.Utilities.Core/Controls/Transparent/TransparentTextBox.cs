using System.Drawing;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    public class TransparentTextBox : TextBox
    {
        public TransparentTextBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.LightBlue)), ClientRectangle);

            //var offset = new Rectangle(ClientRectangle.X + 1, ClientRectangle.Y + 1, ClientRectangle.Width, ClientRectangle.Height);
            //e.Graphics.DrawString(Text, this.Font, new SolidBrush(Color.White), offset);
            e.Graphics.DrawString(Text, this.Font, new SolidBrush(this.ForeColor), ClientRectangle);
            base.OnPaint(e);
        }
    }
}