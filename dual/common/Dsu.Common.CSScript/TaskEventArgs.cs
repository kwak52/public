using System;
using Dsu.Common.Interfaces;
using Dsu.Common.Utilities.MVCs;
using TimeUnit = System.Int64;

namespace Dsu.Common.Utilities
{
    public class TaskEventArgs : EventArgs, IObservableDocumentEvent
    {
        public TimeUnit TimeStamp { get; set; }
        public ITask Task { get; private set; }
        public TaskEventArgs(ITask task, TimeUnit timeStamp = -1, IDocument docHint = null) { Task = task; TimeStamp = timeStamp; DocumentHint = docHint; }
        public IDocument DocumentHint { get; private set; }
    }

    public class ObservableEventTaskStarting : TaskEventArgs
    {
        public ObservableEventTaskStarting(ITask task, long timeStamp = -1, IDocument docHint = null) : base(task, timeStamp, docHint)
        {
        }
    }

    public class ObservableEventTaskFinished : TaskEventArgs
    {
        public ObservableEventTaskFinished(ITask task, long timeStamp = -1, IDocument docHint = null)
            : base(task, timeStamp, docHint)
        {
        }
    }

}
