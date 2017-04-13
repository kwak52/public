using System;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace Dsu.Common.Utilities.DX
{
    public class SplashScreenWaitor : IDisposable
    {
        protected SplashScreenManager _manager;

        private DXWaitForm _hotWaitForm;
        public DXWaitForm WaitForm => _hotWaitForm;

        public string ProgressCaption
        {
            get { return (_hotWaitForm == null) ? String.Empty : _hotWaitForm.ProgressCaption; }
            set { if (_hotWaitForm != null) _hotWaitForm.ProgressCaption = value; }
        }
        public string ProgressDescription
        {
            get { return (_hotWaitForm == null) ? String.Empty : _hotWaitForm.ProgressDescription; }
            set { if (_hotWaitForm != null) _hotWaitForm.ProgressDescription = value; }
        }
        public void Dispose()
        {
            _manager.CloseWaitForm();
        }

        public SplashScreenWaitor(Form parentForm, Type splashFormType = null, bool show=true)
        {
            _manager = parentForm.CreateSplashScreenManager(splashFormType, show);
            if (splashFormType == null || splashFormType == typeof (DXWaitForm))
            {
                _hotWaitForm = DXWaitForm.HotDXForm;
            }
        }
    }
}