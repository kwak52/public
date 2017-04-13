using System.Drawing;
using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Icon holder interface.
    /// </summary>
    [ComVisible(false)]
    public interface IIconHolder
    {
        /// <summary> default icon of the holder object </summary>
        Icon DefaultIcon { get; }
    }
}
