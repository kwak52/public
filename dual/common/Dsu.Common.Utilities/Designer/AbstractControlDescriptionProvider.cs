using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Dsu.Common.Utilities.Designer
{
    /// <summary>
    /// The designer must create an instance of type 'Dsu.Common.Utilities.DX.DXFormDockableLogBase' but it cannot because the type is declared as abstract. 
    /// http://stackoverflow.com/questions/1620847/how-can-i-get-visual-studio-2008-windows-forms-designer-to-render-a-form-that-imp
    /// </summary>
    /// <typeparam name="TAbstract"></typeparam>
    /// <typeparam name="TBase"></typeparam>
    [ComVisible(false)]
    public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public AbstractControlDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(TAbstract))
                return typeof(TBase);

            return base.GetReflectionType(objectType, instance);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
                objectType = typeof(TBase);

            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }
}
