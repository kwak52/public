using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// 주어진 문자열 set 에서 하나의 문자열을 고르기 위한 combobox class
    /// </summary>
    public class ComboBoxStringSelector : ComboBox
    {
        private List<string> _strings = new List<string>();

        /// <summary>
        /// ComboBox 를 초기화.
        /// </summary>
        /// <param name="strings">ComboBox 에 채울 문자열 set</param>
        /// <param name="selText">초기 선택 문자열</param>
        /// <param name="action">ComboBox 에서 문자열이 선택되었을 경우에 수행할 action</param>
        public void Initialize(IEnumerable<string> strings, string selText=null, Action action=null)
        {
            _strings = strings.ToList();
            DataSource = _strings;
            if (!String.IsNullOrEmpty(selText) && strings.Contains(selText))
                SelectedIndex = strings.ToList().FindIndex(s => s == selText);

            if ( action != null )
            {
                SelectedIndexChanged += (sender, args) => { action(); };
            }
        }

        public string GetSelectedItemText()
        {
            return _strings[SelectedIndex];
        }
    }


    /// <summary>
    /// Enum value 를 선택하기 위한 combobox selector
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
    public class ComboBoxEnumSelector : ComboBox
    {
        private string[] _names;
        private List<int> _values;

        private List<int> _omitValues = new List<int>();

        /// <summary> Enumeration type 중에서 combo 에서 제거해야 할 item 이 존재하면 해당 enum value 를 int 변환해서 추가함. </summary>
        public IEnumerable<int> OmitValues { get { return _omitValues; } set { _omitValues = value.ToList(); } }

        public void Initialize(Type enumType, int? selEnum = null, Action action = null)
        {
            _values = Enum.GetValues(enumType).Cast<int>().Except(OmitValues).ToList();
            _names = _values.Select(v => Enum.GetName(enumType, v)).ToArray();

            DataSource = _names;
            if (selEnum != null)
                SelectEnumValue(selEnum.Value);

            if (action != null)
            {
                /* SelectedIndexChanged 에 적용하면, combobox drop list 를 navigation 하는 중에도 event handler 가 호출된다. */
                SelectionChangeCommitted += (sender, args) => { action(); };
            }
        }

        public int GetSelectedItemEnumValue()
        {
            return _values[SelectedIndex];
        }

        public void SelectEnumValue(int enumValue)
        {
            SelectedIndex = _values.FindIndex(v => v == enumValue);
        }
        
    }
}
