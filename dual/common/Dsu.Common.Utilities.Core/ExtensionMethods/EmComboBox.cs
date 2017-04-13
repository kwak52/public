using System;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    public static class EmComboBox
    {
        public static void SetEnumeration<T>(this ComboBox combo, T selected, Action<T> selectedAction )
        {
            combo.DataSource = Enum.GetValues(typeof (T));
            combo.SelectedItem = selected;
            combo.SelectedIndexChanged += (sender, args) => { selectedAction((T) combo.SelectedItem); };
        }
    }
}
