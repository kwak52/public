using System;
using System.Threading;
using System.Windows.Forms;
using Dsu.Common.Utilities.Exceptions;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Popup tooltip class.
    /// <br/> - 확인 버튼 등을 누를 때, action 수행 결과를 해당 버튼에서 사용자에게 보여 주기 위한 tooltip
    /// </summary>
    /// 한번 설정하면, 나중에 해당 button 을 hover 할 때, 다시 동일 tooltip 이 보여지므로, 이를 없애기 위해서 인위적으로 Hide 수행
    public class PopupToolTip : ToolTip
    {
        public string Title { get { return ToolTipTitle; } set { ToolTipTitle = value; } }

        public PopupToolTip(string title)
        {
            ToolTipIcon = ToolTipIcon.Info;
            Title = title;
            AutoPopDelay = 0;
            AutomaticDelay = 0;
            InitialDelay = 0;
            ShowAlways = false;
            ReshowDelay = Int32.MaxValue;
        }

        /// <summary>
        /// Popup ToolTip 을 표시한다.
        /// </summary>
        /// <param name="text">Tooltip 에 보여줄 메시지</param>
        /// <param name="window">Tooltip 을 소유할 control.  보통 누르는 button</param>
        /// <param name="duration">Tooltip 을 보여줄 시간.  milliseconds</param>
        public void Show(string text, Control window, int duration = 2000)
        {
            base.Show(text, window, duration);
            ExceptionSafeThreadStarter.Start("PopupToolTip", window, async () =>
            {
                Thread.Sleep(duration);
                await window.DoAsync(() => Hide(window));
            });
        }
    }
}
