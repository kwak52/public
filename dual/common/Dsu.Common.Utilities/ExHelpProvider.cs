using System;
using System.Linq;
using System.Windows.Forms;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// HelpProvider 기능 확장 class
    /// <br/> Toolstrip menu item 에 대한 context help 지원.  http://www.codeproject.com/Articles/27641/Context-Sensitive-Help-for-Toolstrip-Items
    /// </summary>
    public class ExHelpProvider : HelpProvider
    {
        private string GetHelpKeyword(ToolStrip ts)
        {
            if (ts == null)
                return null;

            foreach (var ti in ts.CollectItems<ToolStripItem>().Where(ti => ti.Selected))
            {
                string keyword = ti.Tag as string;
                if (keyword != null)
                    return keyword;
            }

            return null;
        }
        public override string GetHelpKeyword(Control ctl)
        {
            {
                var keyword = GetHelpKeyword(ctl as ToolStrip);
                if (keyword.NonNullAny())
                    return keyword;
            }


            Form form = ctl as Form;
            if (form != null)
            {
                foreach (var t in form.CollectChildren<MenuStrip>())        // ToolStrip
                {
                    var keyword = GetHelpKeyword(t);
                    if (keyword.NonNullAny())
                        return keyword;      
                }
            }


            return base.GetHelpKeyword(ctl);
        }
    }
}
