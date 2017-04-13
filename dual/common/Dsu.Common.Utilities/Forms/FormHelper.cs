using System.ComponentModel;
using System.Windows.Forms;
using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Help 를 제공하는 form 의 base
    /// <br/> - 생성자나 Load 에서 ContextHelp.SetHelp(this, "robot-jog.html"); 등과 같이 호출
    /// </summary>
    public class FormHelper : Form, IHelpProvider
    {
        [Browsable(false)]
        public virtual HelpProvider ContextHelp
        {
            get
            {
                if (DesignMode)
                    return new HelpProvider();

                return CommonApplication.TheCommonApplication.ContextHelp;
            }
        }
    }
}
