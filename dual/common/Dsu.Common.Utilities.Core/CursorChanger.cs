using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.Core
{
    public class CursorChanger : IDisposable
    {
        protected Cursor _cursorBackup = null;

        public CursorChanger(Cursor newCursor)
        {
            _cursorBackup = Cursor.Current;
            Cursor.Current = newCursor;
        }
        public void Dispose()
        {
            Cursor.Current = _cursorBackup;
        }
    }


    /*
        using (new WaitCursor())
        {
        }
     */
    public class WaitCursor : CursorChanger
    {
        public WaitCursor() : base(Cursors.WaitCursor) { }
    }

    // http://tech.pro/tutorial/732/csharp-tutorial-how-to-use-custom-cursors
    public class TextCursor : IDisposable
    {
        private Cursor _cursorBackup = null;

        public TextCursor(string text)
        {
            _cursorBackup = Cursor.Current;
            using(Bitmap bitmap = new Bitmap(200, 40))
            {
                Graphics g = Graphics.FromImage(bitmap);

                using (Font f = new Font(FontFamily.GenericSerif, 18))
                    g.DrawString(text, f, Brushes.Green, 0, 0);

                Cursor newCursor = CreateCursor(bitmap, 3, 3);
                Cursor.Current = newCursor;
            }
        }

        public void Dispose()
        {
            Cursor.Current = _cursorBackup;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IntPtr ptr = bmp.GetHicon();
            IconInfo tmp = new IconInfo();
            GetIconInfo(ptr, ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            ptr = CreateIconIndirect(ref tmp);
            return new Cursor(ptr);
        }

        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }
    }
}
