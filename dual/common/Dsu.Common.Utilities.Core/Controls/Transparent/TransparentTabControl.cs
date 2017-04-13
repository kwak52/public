using System.Drawing;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// NOT WORKING
    /// </summary>
    public class TransparentTabControl : TabControl
    {
        public TransparentTabControl()
        {
            //SetStyle(/*ControlStyles.UserPaint |*/ ControlStyles.SupportsTransparentBackColor |
            //             ControlStyles.OptimizedDoubleBuffer, true);
            //this.DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        // http://stackoverflow.com/questions/5338587/set-tabpage-header-color
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            //using (Brush br = new SolidBrush(TabColors[tabControl1.TabPages[e.Index]]))
            using (Brush br = new SolidBrush(Color.FromArgb(100, Color.LightBlue)))
            {
                e.Graphics.FillRectangle(br, e.Bounds);
                SizeF sz = e.Graphics.MeasureString(this.TabPages[e.Index].Text, e.Font);
                e.Graphics.DrawString(this.TabPages[e.Index].Text, e.Font, Brushes.Black,
                    e.Bounds.Left + (e.Bounds.Width - sz.Width)/2,
                    e.Bounds.Top + (e.Bounds.Height - sz.Height)/2 + 1);

                Rectangle rect = e.Bounds;
                rect.Offset(0, 1);
                rect.Inflate(0, -1);
                e.Graphics.DrawRectangle(Pens.DarkGray, rect);
                e.DrawFocusRectangle();
            }
        }

        Rectangle TabBoundary;
        RectangleF TabTextBoundary;
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            //g.FillRectangle(new SolidBrush(Color.Red), 0, 0, this.Size.Width, this.Size.Height);

            foreach (TabPage tp in this.TabPages)
            {
                //drawItem
                int index = this.TabPages.IndexOf(tp);

                this.TabBoundary = this.GetTabRect(index);
                this.TabTextBoundary = (RectangleF)this.GetTabRect(index);

                g.FillRectangle(new SolidBrush(Color.LightBlue), this.TabBoundary);
                g.DrawString("tabPage " + index.ToString(), this.Font, new SolidBrush(Color.Black), this.TabTextBoundary/*, format*/);
            }
        }
    }
}