using System.Windows.Forms;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    public static class EmHelpProvider
    {
        public static void SetHelp(this HelpProvider helpProvider, Control control, string keyword, HelpNavigator navigator = HelpNavigator.Topic)
        {
            helpProvider.SetShowHelp(control, true);
            helpProvider.SetHelpKeyword(control, keyword);
            helpProvider.SetHelpNavigator(control, navigator);
        }
    }
}
