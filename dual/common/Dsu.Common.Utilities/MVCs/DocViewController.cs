using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities.MVCs
{
    [ComVisible(false)]
    public class DocViewController<TDoc, TView>
        : MvcControlBase<TDoc, TView>
        where TDoc : DocBase
        where TView : IView
    {
        public static readonly LogProxy logger = LogProxy.CreateLoggerProxy(MethodBase.GetCurrentMethod().DeclaringType);

        public DocViewController(TDoc doc, TView view)
            : base(doc, view)
        {
            Contract.Requires(doc != null);
            logger.DebugFormat("Initializing controller: model={0}, view={1}", doc.FilePath, view.ToString());
        }

        private IDisposable _documentChangeSubscription;
        protected override void WireEvents()
        {
            logger.Debug("Controller: Wiring events");
            _documentChangeSubscription = Model.DocumentChangeSubject
                //.Where(evt => evt is ModelChangedEventArgs)
                .OnceInTimeWindow(TimeSpan.FromMilliseconds(40))
                .Subscribe(OnDocumentChanged);
        }
        protected override void UnwireEvents()
        {
            logger.Debug("Controller: Unwiring events");
            _documentChangeSubscription.DisposeSafely();
            _documentChangeSubscription = null;
        }

        private void OnDocumentChanged(IObservableDocumentEvent evt)
        {
            logger.Debug("Controller: Model changed.  views need to be updated.");
            UpdateViews();
        }

        protected override void UpdateViews()
        {
            logger.Debug("Controller: Updating views.");
            foreach (var view in m_views)
                view.QInvalidate();
        }



        public void RemoveView(TView view)
        {
            m_views.Remove(view);
        }

        public void RemoveAllViews()
        {
            m_views.ToList().ForEach(v => RemoveView(v));
        }

        public void AddView(TView view)
        {
            m_views.Add(view);
        }

    }
}
