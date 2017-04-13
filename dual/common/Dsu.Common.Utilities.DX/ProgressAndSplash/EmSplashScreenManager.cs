using System;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace Dsu.Common.Utilities.DX
{
    public static class EmSplashScreenManager
    {
        public static SplashScreenManager CreateSplashScreenManager(this Form parentForm, Type splashFormType=null, bool show=true)
        {
            if (splashFormType == null)
                splashFormType = typeof (DXWaitForm);

            var manager = new SplashScreenManager(parentForm, splashFormType, true, true);
            if ( show )
                manager.ShowWaitForm();

            return manager;
        }
    }
}