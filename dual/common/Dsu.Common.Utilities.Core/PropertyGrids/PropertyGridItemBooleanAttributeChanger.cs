/*
 * http://www.codeproject.com/Articles/152945/Enabling-disabling-properties-at-runtime-in-the-Pr
 *      Enabling/disabling properties at runtime in the PropertyGrid
 * http://www.codeproject.com/Articles/4448/Customized-display-of-collection-data-in-a-Propert
 * http://www.codeproject.com/Articles/6611/Ordering-Items-in-the-Property-Grid
 * http://www.codeproject.com/Articles/189521/Dynamic-Properties-for-PropertyGrid
 * 
 * Property grid 에서 item A 의 값 변경에 따라 item B 의 상태(e.g enabled/disabled, show/hide 등)가 
 * 동적으로 변경되어야 하는 경우를 처리하기 위함.
 * 
 * property item A 의 attribute 에 다음 attribute 지정
 *  - [RefreshProperties(System.ComponentModel.RefreshProperties.All)]
 * property item B 의 attribute 에 다음 중 적절한 attribute 지정
 *  - [Browsable(true)]
 *  - [ReadOnly(false)]
 */


using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Dsu.Common.Utilities.PropertyGrids
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T">ReadOnlyAttribute, BrowsableAttribute</typeparam>
    [ComVisible(false)]
    public class PropertyGridItemBooleanAttributeChanger<T> where T : Attribute
    {
        protected T _attribute = null;
        protected System.Reflection.FieldInfo _fieldToChange = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">property 를 포함하는 class 의 type</param>
        /// <param name="strItemName">변경하려는 property 의 이름</param>
        /// <param name="internalFieldName">변경하려는 property 의 attribute filed 명.  e.g "isReadOnly"</param>
        public PropertyGridItemBooleanAttributeChanger(Type type, string strItemName, string internalFieldName)
        {
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(type)[strItemName];
            _attribute = (T)descriptor.Attributes[typeof(T)];

            _fieldToChange = _attribute.GetType().GetField(internalFieldName,
                                             System.Reflection.BindingFlags.NonPublic
                                           | System.Reflection.BindingFlags.Instance
                                             );
        }

        public bool Value { get { return (bool)_fieldToChange.GetValue(_attribute); } set { _fieldToChange.SetValue(_attribute, value); } }
    }


    /// <summary> Property item 의 enable/disable 속성을 변경 (read-only) </summary>
    [ComVisible(false)]
    public class PropertyGridItemReadOnlyChanger : PropertyGridItemBooleanAttributeChanger<ReadOnlyAttribute>
    {
        public PropertyGridItemReadOnlyChanger(Type type, string strItemName)
            : base(type, strItemName, "isReadOnly")
        { }
        public bool ReadOnly { get { return Value; } set { Value = value; } }
    }

    /// <summary> Property item 의 visible 속성을 변경 </summary>
    [ComVisible(false)]
    public class PropertyGridItemVisibleChanger : PropertyGridItemBooleanAttributeChanger<BrowsableAttribute>
    {
        public PropertyGridItemVisibleChanger(Type type, string strItemName)
            : base(type, strItemName, "browsable")
        { }
        public bool Visible { get { return Value; } set { Value = value; } }
    }
}

