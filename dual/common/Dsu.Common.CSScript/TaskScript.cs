using System;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Threading.Tasks;
using CSScriptLibrary;
using Dsu.Common.Interfaces;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.Exceptions;
using Dsu.f3d.Interfaces;
using TimeUnit = System.Int64;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// 사용자의 script file 에서 추출한 ScriptMethod attribute 를 가지는 method 를 task 화 한 class.
    /// </summary>
    public class TaskScript : ITaskSchedulable, ITaskScript
    {
        public static readonly LogProxy logger = LogProxy.CreateLoggerProxy(MethodBase.GetCurrentMethod().DeclaringType);
        public string Name { get; set; }
        public string UniqueName { get { return Name; } set { Name = value; } }

        /// <summary>
        /// [ScriptMethod("사용자 comment")] 형태의 attribute 에서 추출한 comment
        /// </summary>
        public string Comment { get; set; }
        public object UserTag { get; set; }

        public TimeUnit? StartedTime { get; internal set; }
        public TimeUnit? FinishedTime { get; private set; }
        private MethodDelegate _method;

        /// <summary> Container scheduler </summary>
        public IScheduler Scheduler { get; set; }

        public TaskProcessResult Process(long worldClock)
        {
            return new TaskProcessResult(this)
            {
                NeedsViewUpdate = Execute(new TaskArgsClock(worldClock)) != null,
                Finished = true,
            };
        }
        public virtual async Task<TaskProcessResult> ProcessAsync(TimeUnit worldClock)
        {
            return await Task.Run(() => Process(worldClock));
        }

        public TaskScript(string name, MethodDelegate method, string comment)
        {
            Name = name;
            _method = method;
            Comment = comment;
        }

        public IDevice Device { get { throw new NotImplementedException();} }
        public string Note { get { return Comment; } set { Comment = value; } }
        public TaskType TaskType { get { return TaskType.Script;} }
        public bool Is3dTask { get ; set; }
        public object Execute(object arg)
        {
            try { return _method(arg); }
            catch (Exception ex)
            {
                logger.ErrorFormat("Failed to execute task script: {0}({1}) : {2}", Name, arg, ex.Message);
                return false;
            }
        }

        public async Task<object> ExecuteAsync(object arg)
        {
             return await Task.Run(() => Execute(arg));
        }

    }
}
