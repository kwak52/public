using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Dsu.Common.Interfaces;
using TimeUnit = System.Int64;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Schedulable interface
    /// </summary>
    [ComVisible(false)]
    public interface ISchedulable
    {
        /// <summary> 실제 task 가 구동된 시간.  start time 을 지정하지 않더라도 scheduler 에 의해서 시작 될 때의 시간 </summary>
        TimeUnit? StartedTime { get; }
        /// <summary> 실제 task 가 종료된 시간. </summary>
        TimeUnit? FinishedTime { get; }

        /// <summary> 주어진 task 를 실행한다. </summary>
        /// <param name="worldClock"> 0 이면 worldClock 을 고려하지 않고 무조건 수행</param>
        TaskProcessResult Process(TimeUnit worldClock);
        Task<TaskProcessResult> ProcessAsync(TimeUnit worldClock);
    }


    [ComVisible(false)]
    public interface IScheduler
    {
        long Clock { get; }
    }


    [ComVisible(false)]
    public interface ITaskSchedulable : ITask, ISchedulable
    {
        IScheduler Scheduler { get; set; }
    }
}
