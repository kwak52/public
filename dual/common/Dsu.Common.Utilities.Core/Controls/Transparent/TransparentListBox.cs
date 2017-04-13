// http://www.cprogramdevelop.com/1936262/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    public class TransparentListBox : ListBox
    {
        public TransparentListBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            //this.BackColor = Color.Transparent;            
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (/*this.Focused &&*/ this.SelectedItem != null)
            {
                Rectangle itemRect = this.GetItemRectangle(this.SelectedIndex);
                //e.Graphics.FillRectangle(Brushes.LightBlue, itemRect);
                using( var brush = new SolidBrush(Color.FromArgb(100, Color.LightBlue)))
                    e.Graphics.FillRectangle(brush, itemRect);
            }
            for (int i = 0; i < Items.Count; i++)
            {
                e.Graphics.DrawString(this.GetItemText(this.Items[i]), this.Font, new SolidBrush(this.ForeColor),
                    this.GetItemRectangle(i));
            }

            base.OnPaint(e);
        }
    }
}
