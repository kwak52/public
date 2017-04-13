using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Dsu.Common.Interfaces.LmInterop
{
    /// <summary>
    /// Task 를 가지는 객체가 구현해야 할 interface.
    /// <para/> - Sharp3d 에서는 DocCell 에서 구현 (Shar3d application 에서는 wrapper 로 구현)
    /// <para/> - DMWorks 라면 application level 에서 구현될 듯...???
    /// <br/> see also Sharp3d.SceneGraph.TaskProviderImpl
    /// </summary>
    [Guid("BCE3E055-6AA6-4A30-87C7-0929AAD8ADD8")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITaskProvider
    {
        /// <summary> 모든 task 를 반환 </summary>
        ITask[] Tasks { get; } 

        /// <summary>
        ///  주어진 이름을 가진 task 를 반환
        /// </summary>
        /// <param name="taskname"></param>
        /// <returns></returns>
        ITask FindTask(string taskname);

        /// <summary> 단위 task 실행 </summary>
        /// 주어진 taskname 의 이름을 갖는 task 를 찾아서 수행한다.  Task start/finish notification 을 받을 수 있다.
        /// ITask.Execute() 를 직접 호출하는 경우는 notification 을 받을 수 없다.
        object ExecuteNamedTask(string taskname, object arg);

        /// <summary> return value should be casted to Task&lt; bool &gt;</summary>
        void ExecuteNamedTaskAsync(string taskname, object arg);


        /// <summary>
        /// 복수 task 실행 (Sync mode)
        /// 
        /// startParallel=true : 가장 긴 실행 시간을 갖는 task 만큼 후에 return
        /// startParallel=false : 모든 task 의 실행 시간 총합 만큼 후에 return
        /// </summary> 
        /// <param name="tasknames">실행할 task 목록</param>
        /// <param name="arg">각 task 에 공통적으로 적용할 argument </param>
        /// <param name="startParallel">각 task 동시 실행 여부.  false 이면 하나의 task 를 호출하고, 해당 task 종료 후 다음 task 를 호출한다. </param>
        object[] ExecuteNamedTasks(string[] tasknames, object arg, bool startParallel);

        /// <summary>
        /// Async mode 로 복수 task 실행.  바로 return
        /// startParallel=true : 각 task 는 동시 실행됨.
        /// startParallel=false : 각 task 는 순차적으로 하나씩 실행됨.
        /// </summary>
        void ExecuteNamedTasksAsync(string[] tasknames, object arg, bool startParallel);
    }
}
