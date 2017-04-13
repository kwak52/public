using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    public static class EmMenu
    {
        /// <summary>
        /// http://stackoverflow.com/questions/15926377/change-the-backcolor-of-the-toolstripseparator-control
        /// </summary>
        public static void ChangeProperty(this ToolStripSeparator separator, Nullable<Color> foreColor = null, Nullable<Color> backColor = null, int? width = null, int? thickness = null)
        {
            separator.Paint += (sender, e) =>
            {
                foreColor = foreColor ?? Color.DarkKhaki;
                backColor = backColor ?? Color.Empty;

                // Get the separator's width and height.
                ToolStripSeparator toolStripSeparator = (ToolStripSeparator)sender;
                width = width ?? toolStripSeparator.Width;
                int height = toolStripSeparator.Height;
                thickness = thickness ?? 2;

                // Fill the background.
                e.Graphics.FillRectangle(new SolidBrush(backColor.Value), 0, 0, width.Value, height);

                // Draw the line.
                e.Graphics.DrawLine(new Pen(foreColor.Value) { Width = thickness.Value }, 4, height / 2, width.Value - 4, height / 2);
            };
        }

        public static bool IsSelected(this ToolStripItem item)
        {
            return item.Selected && item.Enabled;
        }

        public static IEnumerable<T> CollectItems<T>(this ToolStripMenuItem mi, bool includeMe=true) where T : ToolStripItem
        {
            if (includeMe && mi is T)
                yield return mi as T;

            foreach (var i in mi.DropDownItems)
            {
                if ( i is ToolStripMenuItem)
                {
                    foreach (var si in ((ToolStripMenuItem)i).CollectItems<T>(true))
                        yield return si;
                }                
            }            
        }
        public static IEnumerable<T> CollectItems<T>(this ToolStrip toolStrip) where T : ToolStripItem
        {
            foreach (var ti in toolStrip.Items)
            {
                var tmi = ti as ToolStripMenuItem;
                if ( tmi != null )
                {
                    foreach (var mi in tmi.CollectItems<T>())
                        yield return mi;
                }
            }
        }
    }
}
