using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Dsu.Common.Interfaces;
using Dsu.Common.Utilities.Exceptions;
using WeifenLuo.WinFormsUI.Docking;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Docking 지원하는 base class
    /// </summary>
    public abstract class FormDockableBase
        : DockContent
        , IHelpProvider
        , ISubscribable
    {
        public HelpProvider ContextHelp { get { return CommonApplication.TheCommonApplication.ContextHelp; } }

        protected IApplication _application;
        protected ILoggable LoggableApplication { get {  return _application as ILoggable;} }
        //protected DockPanel DockPanel { get { return DockHandler.DockPanel; } set { DockHandler.DockPanel = value; } }

        protected CommonApplication CommonApplication { get {  return _application as CommonApplication; } }

        private List<IDisposable> _subscriptions = new List<IDisposable>();

        public void AddSubscription(IDisposable subscription)
        {
            if ( _subscriptions == null )
                throw new ArgumentNullException("Null subscription");

            _subscriptions.Add(subscription);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subscriptions.ForEach(s => s.Dispose());   
            }

            base.Dispose(disposing);
        }
        public virtual void Initialize(IApplication application, DockPanel dockpanel)
        {
            _application = application;
            DockPanel = dockpanel;
        }

        public new void Show()
        {
            try
            {
                if (DockPanel != null)
                    base.Show(DockPanel, DockState.DockLeft);
                else
                    base.Show();
            }
            catch (Exception ex)
            {                
                ExceptionHider.SwallowException(ex);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            /* todo : DockPanelSuite 2.9 로 upgrade 후 지원되지 않는 기능... 어떻게??? */
            //TabPageContextMenu = new ContextMenu(new[]
            //    {
            //        new MenuItem("Undock", (o, args) => { Undock(); }),
            //        new MenuItem("Dock", (o, args) => { Redock();}),
            //    });
        }


        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_application is IDockable)
                ((IDockable)_application).RemoveNamedDockContent(GetType().FullName);
        }
    }
}
