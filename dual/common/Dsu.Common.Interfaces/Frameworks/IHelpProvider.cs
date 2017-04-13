using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Context help 기능을 제공하려는 객체가 구현해야 할 interface
    /// </summary>
    /// 주로 form 이 대상이 되며, 다음과 같이 구현해 주면 된다.
    /// <c>
    ///    public HelpProvider ContextHelp { get { return CommonApplication.TheCommonApplication.ContextHelp; } }
    /// </c>
    [ComVisible(false)]
    public interface IHelpProvider
    {
        /// <summary> HelpProvider 객체 </summary>
        HelpProvider ContextHelp { get; }
    }

    /// <summary>
    /// Context help 기능을 제공하려는 top-level 객체가 구현해야 할 interface
    /// </summary>
    /// 주로 application form 이 구현한다.
    /// IHelpProvider.ContextHelp 는 application 의 모든 form 들이 사용할 HelpProvider 객체가 되어야 한다.
    /// FormAppCommon 에서 단 한번 생성한다.
    /// 
    /// Note:
    /// ContextHelp.HelpNamespace = HelpFilePath;
    [ComVisible(false)]
    public interface ITopLevelHelpProvider : IHelpProvider
    {
        /// <summary> Help(*.chm) 파일의 path </summary>
        string HelpFilePath { get; }
    }
}
