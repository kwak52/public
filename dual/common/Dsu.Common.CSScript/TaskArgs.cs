using Dsu.Common.Interfaces;
using TimeUnit = System.Int64;

namespace Dsu.Common.Utilities
{
    public class TaskArgs : ITaskArgs
    {
        public object Tag { get; set; }
        public long StartClock { get; set; }
        public bool IsFinished { get; set; }
        public bool IsPaused { get; set; }
        public bool IsStopped { get; set; }
    }

    public class TaskArgsClock : TaskArgs
    {
        public TimeUnit WorldClock { get; private set; }
        public TaskArgsClock(TimeUnit worldClock) {  WorldClock = worldClock; }
    }
}
