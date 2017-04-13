using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// HUD(Head-Up Display) 를 포함하는 container(주로 view) 의 interface
    /// </summary>
    [ComVisible(false)]
    public interface IHudContainer
    {
        /// <summary> this 를 포함하는 container(주로 view) </summary>
        Control HudOwner { get; }

        /// <summary> Container 에 포함된 HDU panels </summary>
        IHudBar[] HudBars { get; }

        /// <summary> Hud container 에 hudbar 를 추가한다.  e.g GLView 에 hud bar 추가 </summary>
        void AddHudBar(IHudBar panel);

        /// <summary> Hud bar 삭제 </summary>
        bool RemoveHudBar(IHudBar panel);
    }

    /// <summary> IHudContainer 에 포함되는 HUD(Head-Up Display) item </summary>
    [ComVisible(false)]
    public interface IHudItem
    {
        /// <summary> Parent container </summary>
        IHudContainer HudContainer { get; }
        /// <summary> HUD item 을 구현하는 최상위 Control </summary>
        Control Control { get; }

        /// <summary> 사용자의 drag 에 의해서 bar 를 이동가능한지 여부 </summary>
        bool Movable { get; set; }
    }

    /// <summary>
    /// HUD(Head-Up Display) item.  ToolBar 형태로 표현
    /// </summary>
    [ComVisible(false)]
    public interface IHudBar : IHudItem, IDisposable
    {
        /// <summary> hudbar title </summary>
        string Title { get; set; }
        /// <summary> HUD container 에 표시될 때의 anchor 위치 </summary>
        AnchorStyles AnchorStyles { get; set; }
        /// <summary> bar 가 가로형인지 세로형인지의 여부 </summary>
        bool Horizontal { get; set; }
        /// <summary> Hud bar 에 close button 삽입 여부 </summary>
        bool Closable { get; set; }
        /// <summary> Hud bar 를 minimize/maximize 할 수 있는지의 여부 </summary>
        bool Minimizable { get; set; }
        /// <summary> Hud bar 에 속한 child controls </summary>
        Control[] Children { get; set; }

        /// <summary> Child control 추가 </summary>
        /// <param name="control"></param>
        void AddControl(Control control);
        /// <summary> Child control 삭제 </summary>
        bool RemoveControl(Control control);

        /// <summary> Layout 재배치 </summary>
        void RecalcLayout();
    }
}
