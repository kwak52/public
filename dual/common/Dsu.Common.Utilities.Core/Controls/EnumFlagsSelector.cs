// http://www.codeproject.com/Articles/386634/EnumFlagsSelector-select-multiple-enum-values

// Author : kobi.bo (kobi@zebrot.com)
// License : CPOL

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// <c>EnumFlagsSelector</c> control allows to select multiple enum values from all
    /// possible values of enum <typeparamref name="T"/>. To Use the control, You
    /// should inherit from <c>EnumFlagsSelector</c> with enum type which is decorated
    /// with Flags attribute. Each enum value can be checked to be included in the
    /// selection and unchecked to be removed from the selection.
    /// <para>
    /// The <c>Selected</c> property holds the current selected values. This property
    /// is always synchronized with the user interface. In other words, When the user
    /// add enum value or remove enum value, the property value is updated to new selection
    /// immediately and when the property is updated in the program - the user interface
    /// is updated to reflect the change.
    /// </para>
    /// <para>
    /// When the user change its selcetion by checking or unchecking enum values ,
    /// <c>SelectionChanged</c> event is raised. You can use the <c>Selected</c>
    /// property of the <c>EnumFlagsSelector</c> which raised this event, to find
    /// out the new selected value.
    /// </para>
    /// <para>
    /// By default, the names of the enum values will be displayed in the user interface.
    /// While this is very convenient, sometimes this is not suitable (For example, a more detailed
    /// name should be displayed, the jargon used to name the values is unfamiliar to the target
    /// audience or the values needs to be translated to another language). In this case, You can
    /// set <c>AllowFormat</c> property and override <c>OnFormat</c> method. This method accepts the enum value
    /// to be displayed and should return a string representation of the value which will be displayed
    /// on the user interface.
    /// </para>
    /// <typeparam name="T">The enum type</typeparam>
    /// </summary>

    [ComVisible(false)]
    public partial class EnumFlagsSelector<T> : UserControl
    {
        /// <summary>
        /// Create a new <c>FlagsSelector</c>
        /// </summary>
        public EnumFlagsSelector()
        {
            InitializeComponent();
            foreach (object oo in Enum.GetValues(typeof(T)))
            {
                winCheckList.Items.Add(oo, false);
            }
        }
        /// <summary>
        /// Create a new <c>FlagsSelector</c> and select <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value</param>
        public EnumFlagsSelector(T value)
            : this()
        {
            Selected = value;
        }


        public EnumFlagsSelector(long maskedValue)
        {
            InitializeComponent();
            SetDisplayLists(maskedValue);
        }

        public void SetDisplayLists(long maskedValue)
        {
            winCheckList.Items.Clear();
            foreach (object oo in Enum.GetValues(typeof(T)))
            {
                if ((maskedValue & (int)oo) != 0)
                    winCheckList.Items.Add(oo, false);
            }
        }


        /// <summary>
        /// The selected value
        /// </summary>
        public T Selected
        {
            get
            {
                return (T)((object)_selected);
            }
            set
            {
                int valueInt = (int)((object)value);
                if (_selected == valueInt)
                {
                    return;
                }
                _selected = valueInt;
                for (int ii = 0; ii < winCheckList.Items.Count; ii++)
                {
                    winCheckList.SetItemChecked(ii, FlagsHelper.IsSet(_selected, (int)winCheckList.Items[ii]));
                }
            }
        }
        private int _selected = 0;
        /// <summary>
        /// Whether the <c>OnFormat</c> method should be used to determine what will
        /// be display string for each enum value. If false, The display value will
        /// be the enum value name.
        /// </summary>
        public bool AllowFormat
        {
            get
            {
                return _allowFormat;
            }
            set
            {
                if (value == _allowFormat)
                {
                    return;
                }
                _allowFormat = value;
                if (value)
                {
                    winCheckList.Format += winCheckList_Format;
                }
                else
                {
                    winCheckList.Format -= winCheckList_Format;
                }
            }
        }
        private bool _allowFormat;


        /// <summary>
        /// Find the display string of the item
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The display string</returns>
        public virtual string OnFormat(T value)
        {
            return value.ToString();
        }

        #region Events
        /// <summary>
        /// Raise when the selectoion changed
        /// </summary>
        public event EventHandler SelectionChanged;
        #endregion


        
        private void winCheckList_ItemCheck(object sender, ItemCheckEventArgs ee)
        {
            int changedFlag = (int)winCheckList.Items[ee.Index];
            if (ee.NewValue == CheckState.Checked)
            {
                FlagsHelper.Set(ref _selected, changedFlag);
            }
            else if (ee.NewValue == CheckState.Unchecked)
            {
                FlagsHelper.Clear(ref _selected, changedFlag);
            }
            if (SelectionChanged != null)
            {
                SelectionChanged(sender, ee);
            }
        }

        private void winCheckList_Format(object sender, ListControlConvertEventArgs ee)
        {
            ee.Value = OnFormat((T)ee.ListItem);
        }
    }

    /// <summary>
    /// Collection of actions for integer bit manipulation
    /// </summary>
    public static class FlagsHelper
    {
        /// <summary>
        /// Find whether the setted bits in <paramref name="checkBits"/> are also
        /// setted in <paramref name="flags"/> variable
        /// </summary>
        /// <param name="flags">The current variable to check</param>
        /// <param name="checkBits">The bits to check</param>
        /// <returns>True if the bits are setted, otherwise false</returns>
        public static bool IsSet(this int flags, int checkBits)
        {
            return ((flags & checkBits) == checkBits);
        }

        /// <summary>
        /// Set the setted bits in <paramref name="setBits"/> in <paramref name="flags"/> variable
        /// </summary>
        /// <param name="flags">The current variable to update</param>
        /// <param name="setBits">The bits to check</param>
        public static void Set(ref int flags, int setBits)
        {
            flags |= setBits;
        }

        /// <summary>
        /// Clear the setted bits <paramref name="clearBits"/> in <paramref name="flags"/> variable
        /// </summary>
        /// <param name="flags">The current variable to update</param>
        /// <param name="clearBits">The bits to clear</param>
        public static void Clear(ref int flags, int clearBits)
        {
            flags &= (~clearBits);
        }

        /// <summary>
        /// Clear all bits in <paramref name="flags"/> variable
        /// </summary>
        /// <param name="flags">The current variable to update</param>
        public static void Clear(ref int flags)
        {
            flags = 0;
        }
    }
}
