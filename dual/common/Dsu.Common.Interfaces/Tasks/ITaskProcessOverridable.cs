using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces.LmInterop
{
    /// <summary>
    /// ITask.Process() 시에 script 로 action 을 제어할 수 있는 task
    /// </summary>
    [Guid("C27381B4-23C9-476F-9642-9A374F06C189")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface ITaskProcessOverridable : ITaskAsyncable
    {
        /// <summary>
        /// Overridable task 에서 실제 override 할 작업을 지정할 script
        /// </summary>
        ITaskScript OverrideScript { get; set; }
    }
}
