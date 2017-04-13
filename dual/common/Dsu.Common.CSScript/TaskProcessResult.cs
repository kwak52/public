using Dsu.Common.Interfaces;
using Dsu.Common.Utilities.ExtensionMethods;
using TimeUnit = System.Int64;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Task 처리 결과를 표현하는 class
    /// null 반환시에는 Finished == true, NeedViewUpdate == false 와 동일하게 해석한다.
    /// </summary>
    public class TaskProcessResult
    {
        public ITask Task { get; private set; }
        public bool Succeeded { get { return ErrorDescription.IsNullOrEmpty(); } }
        /// <summary> 에러 있을 시에만 설정 </summary>
        public string ErrorDescription { get; set; }

        /// <summary> Task 종료 여부 </summary>
        public bool Finished { get; set; }
        public bool NeedsViewUpdate { get; set; }

        public TaskProcessResult(ITask task, bool succeeded = true)
        {
            Task = task;
            if (!succeeded)
                ErrorDescription = "Some unknonw error";
        }
        public TaskProcessResult(ITask task, string error)
        {
            Task = task;
            ErrorDescription = error;
        }
    }


    public class TaskProcessParameters
    {
        public ITask Task { get; set; }
        public TimeUnit WorldClock { get; set; }
        public IScheduler Scheduler { get; set; }
    }
}