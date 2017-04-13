using Dsu.Common.Interfaces;
using Dsu.Common.Utilities.ExtensionMethods;
using TimeUnit = System.Int64;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Task ó�� ����� ǥ���ϴ� class
    /// null ��ȯ�ÿ��� Finished == true, NeedViewUpdate == false �� �����ϰ� �ؼ��Ѵ�.
    /// </summary>
    public class TaskProcessResult
    {
        public ITask Task { get; private set; }
        public bool Succeeded { get { return ErrorDescription.IsNullOrEmpty(); } }
        /// <summary> ���� ���� �ÿ��� ���� </summary>
        public string ErrorDescription { get; set; }

        /// <summary> Task ���� ���� </summary>
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