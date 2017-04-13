using System;
using System.Threading;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.DX
{
    public class SplashScreenProgressWaitor : SplashScreenWaitor, IFormProgressbar
    {
        private DXWaitForm _waitForm;
        public SplashScreenProgressWaitor(Form parentForm, Type splashFormType = null)
            : base(parentForm, splashFormType, false)
        {
        }

        public new string ProgressCaption { get { return _waitForm.ProgressCaption; } set { _waitForm.ProgressCaption = value; } }
        public new string ProgressDescription { get { return _waitForm.ProgressDescription; } set { _waitForm.ProgressDescription = value; } }
        public CancellationToken CancellationToken { get { return _waitForm.CancellationToken; } }
        public int ProgressTotal { get { return _waitForm == null ? 0 : _waitForm.ProgressTotal; } set { _waitForm.ProgressTotal = value; } }
        public int ProgressPortion { get { return _waitForm == null ? 0 : _waitForm.ProgressPortion; } set { _waitForm.ProgressPortion = value; } }

        public void AddProgressPortion(int portion) { _waitForm.AddProgressPortion(portion); } 

        
        public void StartProgressbar()
        {
            if (!_manager.IsSplashFormVisible )
                _manager.ShowWaitForm();

            _waitForm = DXWaitForm.HotDXForm;
        }

        public void FinishProgressbar()
        {
            if (_manager.IsSplashFormVisible )
                _manager.CloseWaitForm();
            _waitForm = null;
        }
    }
}