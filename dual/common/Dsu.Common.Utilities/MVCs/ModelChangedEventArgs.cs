// see PropertyChangeRequestEventArgs.cs in MVC-PV-2005

using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities.MVCs
{
    public class AppDocView
    {
        public IApplication Application { get; set; }
        public IDocument Document { get; set; }
        public IView View { get; set; }        
    }


    public class ModelChangedEventArgs : ObservableDocumentEvent
    {
        public bool RefreshSensors { get; set; }

        public ModelChangedEventArgs(IDocument docHint=null, string reason=null, bool refreshSensors = true)
            : base(docHint, reason)
        {
            RefreshSensors = refreshSensors;
        }
    }
}
