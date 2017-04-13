using log4net;
using System;
using System.Windows.Forms;


namespace Dsu.Driver.UI
{
    public static class Logging
    {
        public static ILog Logger { get; set; }
    }
}

namespace Dsu.Driver.UI.NiDaq
{
    public static class EmControlCollection
    {
        public static void RemoveAll(this Control.ControlCollection colletion)
        {
            foreach (var c in colletion)
            {
                if (c is Form)
                    ((Form)c).Close();
                else if (c is IDisposable)
                    ((IDisposable)c).Dispose();
            }
            colletion.Clear();
        }
    }
}
