// http://www.codeproject.com/Articles/24385/Have-a-Great-DesignTime-Experience-with-a-Powerful

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;

namespace Dsu.Common.Utilities.DesignSurfaceExt
{
    public class TabOrderHooker
    {
        private const string _Name_ = "TabOrderHooker";

        private object _tabOrder = null;

        //- Enables/Disables visual TabOrder on the view.
        //- internal override
        public void HookTabOrder(IDesignerHost host)
        {
            const string _signature_ = _Name_ + @"::ctor()";

            //- the TabOrder must be called AFTER the DesignSurface has been loaded
            //- therefore we do a little check
            if (null == host.RootComponent)
                throw new Exception(_signature_ + " - Exception: the TabOrder must be invoked after the DesignSurface has been loaded!");

            try
            {
                Assembly designAssembly = Assembly.Load("System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                Type tabOrderType = designAssembly.GetType("System.Windows.Forms.Design.TabOrder");
                if (_tabOrder == null)
                {
                    //- call the ctor passing the IDesignerHost taget object
                    _tabOrder = Activator.CreateInstance(tabOrderType, new object[] { host });
                }
                else
                {
                    DisposeTabOrder();
                }
            }//end_try
            catch (Exception exx)
            {
                Debug.WriteLine(exx.Message);
                if (null != exx.InnerException)
                    Debug.WriteLine(exx.InnerException.Message);

                throw;
            }//end_catch
        }



        //- Disposes the tab order
        public void DisposeTabOrder()
        {
            if (null == _tabOrder) return;
            try
            {
                Assembly designAssembly = Assembly.Load("System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                Type tabOrderType = designAssembly.GetType("System.Windows.Forms.Design.TabOrder");
                tabOrderType.InvokeMember("Dispose", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, _tabOrder, new object[] { true });
                _tabOrder = null;
            }
            catch (Exception exx)
            {
                Debug.WriteLine(exx.Message);
                if (null != exx.InnerException)
                    Debug.WriteLine(exx.InnerException.Message);

                throw;
            }//end_catch
        }


    }//end_class
}//end_namespace 
