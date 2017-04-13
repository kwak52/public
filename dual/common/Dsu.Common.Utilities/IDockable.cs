using System.Runtime.InteropServices;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// DockPanelSuite 의 Docking 기능을 가지는 interface
    /// </summary>
    [ComVisible(false)]
    public interface IDockable
    {
        void SaveDockLayout(string xml);
        void LoadDockLayout(string xml);
        void RemoveNamedDockContent(string dockName);
    }
}