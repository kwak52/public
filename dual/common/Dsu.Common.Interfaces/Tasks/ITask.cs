using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TimeUnit = System.Int64;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Task type enumeration
    /// </summary>
    [Guid("CC9A9D53-3949-445B-B410-125D7D3F221A")]
    [ComVisible(true)]
    public enum TaskType
    {
        /// <summary> Undefined.  error case. </summary>
        Undefined,
        /// <summary> Script task </summary>
        Script,
        /// <summary> Joint motion task </summary>
        JointMotion,
        /// <summary> Robot IK path task </summary>
        RobotIKPath,
        /// <summary> Robot FK path task </summary>
        RobotFKPath,
        /// <summary> Node Slerp task </summary>
        NodeSlerp,
        /// <summary> parallel and/or serial tasks collection </summary>
        Collection,

        /// <summary> Part source </summary>
        PartSource,
        /// <summary> View refreshing task </summary>
        ViewRefresher,

		/// <summary> Mhs Junction Control task </summary>
		MhsJunction,
		/// <summary> Mhs Unit Start task </summary>
		MhsUnitStart,
		/// <summary> Mhs Unit Stop task </summary>
		MhsUnitStop,
		/// <summary> Mhs Unit Set Max Velocity task </summary>
		MhsUnitSetSpeed,
		/// <summary> Mhs Transfer Unit Forward task </summary>
		MhsUnitForward,
		/// <summary> Mhs Transfer Unit Backward task </summary>
		MhsUnitBackward,
		/// <summary> Mhs Moving Unit Attach task </summary>
		MhsMovingUnitAttach,
		/// <summary> Mhs Moving Unit Detach task </summary>
		MhsMovingUnitDetach,
    }


    /// <summary>
    /// 범용 task interface
    /// </summary>
    [Guid("8D2707B1-B85D-494E-93EE-7777A268F527")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITask : INamed, IUniquelyNamed, IDescribable, ITaggable
    {
        /// <summary> Task owner device.  Nullable. </summary>
        //IDevice Device { get; }

        /// <summary> Type of task </summary>
        TaskType TaskType { get; }

        /// <summary> 3d task 여부 </summary>
        bool Is3dTask { get; }



        /// <summary>
        /// (sync mode) 주어진 task 를 실행.  task start/finish notification 은 제공하지 않는다.
        /// <br/> task start/finish notification 을 받으려 한다면, ITaskProvider.ExecuteNamedTask() 를 이용한다.
        /// </summary>
        object Execute(object arg);
    }

    /// <summary>
    /// 비동기 task 
    /// </summary>
    [ComVisible(false)]
    public interface ITaskAsyncable : ITask
    {
        /// <summary>
        /// (async mode) 주어진 task 를 실행.
        /// </summary>
        /// <br/> async 에서는 호출하자 마자 control 이 바로 넘어 온다.  이때 task 는 background 에서 실행됨.
        Task<object> ExecuteAsync(object arg);        
    }


    /// <summary>
    /// Tasks(복수 task) interface
    /// </summary>
    [Guid("4983EEA6-6C2B-4506-99E7-5E5A0E6AE208")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITasks : ITaskAsyncable
    {
        /// <summary> Collection 을 구성하는 sub tasks </summary>
        ITask[] SubTasks { get; }
    }


    /// <summary>
    /// 시작 시점과 종료 시점을 갖는 task
    /// </summary>
    [Guid("4150F313-B0DD-47DA-952B-EDE3C64869F0")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITaskDurationSpecified : ITaskAsyncable
    {
        /// <summary>
        /// zero(0) means invalid duration
        /// </summary>
        TimeUnit Duration { get; }
    }




    /// <summary>
    /// 시작 시점을 갖는 task 의 interface
    /// </summary>
    [Guid("BE16ED2A-62C9-4D5B-842C-A57DA33877C1")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITaskStartTimeSpecified : ITaskAsyncable
    {
        /// <summary> 시작 시간 </summary>
        TimeUnit StartTime { get; }
    }

    /// <summary>
    /// 종료 시점을 갖는 task 의 interface
    /// </summary>
    [Guid("264D54E3-6092-40F6-853E-8F47085A67BA")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITaskFinishTimeSpecified : ITaskAsyncable
    {
        /// <summary> 종료 시간 </summary>
        TimeUnit FinishTime { get; }
    }



    /// <summary>
    /// 시작/종료 시점을 계산할 수 있는 task
    /// </summary>
    [Guid("93F6221B-AE2E-4D57-BAEF-C85B3A33331B")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITaskDurationCalculable : ITaskStartTimeSpecified, ITaskFinishTimeSpecified, ITaskDurationSpecified
    {
    }


    /// <summary>
    /// Script task interface
    /// </summary>
    [Guid("645098AA-29A5-449C-A6D9-2DB78B89D284")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITaskScript : ITaskAsyncable
    {
    }

    /// <summary>
    /// Inner task implementation interface
    /// </summary>
    [ComVisible(false)]
    public interface ITaskImpl
    {
        /// <summary> Task implementation 의 실제 owner task </summary>
        ITask DecoratorTask { get; set; }
    }
}
