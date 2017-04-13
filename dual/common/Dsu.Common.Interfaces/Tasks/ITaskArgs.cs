using System.Runtime.InteropServices;
using TimeUnit = System.Int64;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// ITask 수행시 넘겨 질 arguments interface
    /// </summary>
    [Guid("E705E3F5-69ED-47E1-BC69-D7B9D4B542D2")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITaskArgs
    {
    }


    /// <summary>
    /// 수행(Execute() 호출)후, 제어가 필요한 task 의 argument interface
    /// </summary>
    [Guid("E3DCBEBD-DB84-41CF-9B2E-F525760DC4B5")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IControllableTaskArgs : ITaskArgs
    {
        /// <summary>
        /// Client 의 simulation clock
        /// </summary>
        TimeUnit StartClock { get; set; }


        /// <summary>
        /// task 종료 여부.  
        /// <br/> - caller 에서 false 로 설정하고 ITask.Execute 를 호출한다.
        /// <br/> - server 에서 task 가 종료되면 true 로 설정한다.
        /// <br/> - caller 에서 sync mode 로 task 종료를 검사하려면, 해당 값이 true 로 설정되는지를 wait 해서 검사한다.
        /// </summary>
        bool IsFinished { get; set; }


        /// <summary>
        /// pause 여부.  추후 resume 될 수 있어야 한다.
        /// <br/> - caller 에서 false 로 설정하고 ITask.Execute 를 호출한다.
        /// <br/> - caller 에서 호출 후, 잠시 task 를 pause 시킬 필요가 있을 때에 true 로 설정할 수도 있다.
        /// <br/> - server 에서는 task 수행 중에 true 로 설정되는 지를 주기적으로 검사하다가 설정되면 pause 해야 한다.
        ///         pause 되면 해당 task 는 server 의 paused task list 에서 관리된다.   추후 resume 가능해야 한다.
        /// </summary>
        bool IsPaused { get; set; }


        /// <summary>
        /// stop 여부.  추후 resume 될 가능성은 없다.
        /// <br/> - caller 에서 false 로 설정하고 ITask.Execute 를 호출한다.
        /// <br/> - caller 에서 Execute 호출 후, 완전히 task 를 취소시킬 필요가 있을 때에 true 로 설정할 수도 있다.
        /// <br/> - server 에서는 task 수행 중에 true 로 설정되는 지를 주기적으로 검사하다가 설정되면 stop 해야 한다.
        /// </summary>
        bool IsStopped { get; set; }
    }



}
