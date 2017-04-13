using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Script 변경 내역(script reload event)을 처리하기 위한 event
    /// </summary>
    public class ScriptChangeEvent : IObservableEvent
    {
        public IScriptingHost ScriptingHost { get; private set; }
        public string ScriptFile { get; private set; }

        public ScriptChangeEvent(IScriptingHost scriptingHostImpl, string scriptFile)
        {
            ScriptingHost = scriptingHostImpl;
            ScriptFile = scriptFile;
        }
    }

    public class ScriptReloadedEvent : ScriptChangeEvent
    {
        public ScriptReloadedEvent(IScriptingHost scriptingHostImpl, string scriptFile) : base(scriptingHostImpl, scriptFile)
        {
        }
    }
    /// <summary>
    /// Script hosting 을 지원하려는 class 가 구현해야 할 interface
    /// </summary>
    [ComVisible(false)]
    public interface IScriptingHost : IIconHolder
    {
        string ScriptProjectFileName { get; set; }
        string[] TaskScriptNames { get; }
        string[] EventScriptNames { get; }

        string[] DefaultReferences { get; }

        string GetScriptComment(string scriptName);

        string[] DefinableEventScriptNames { get; }

        /// <summary> 단위 task 실행 </summary>
        /// <param name="taskName">task script 이름</param>
        /// <param name="arg">task script 의 argument </param>
        object ExecuteTaskScript(string taskName, object arg);

        /// <summary> async mode 단위 task 실행 </summary>
        Task<object> ExecuteTaskScriptAsync(string taskName, object arg);

        /// <summary> 복수 task 실행 </summary>
        /// <param name="taskNames">task script 이름들</param>
        /// <param name="arg">task script 의 argument </param>
        /// <param name="startParallel">복수 task 를 동시에 시작할지, 순차적으로 하나 끝나면 다음 실행할지의 여부</param>
        /// <returns></returns>
        object ExecuteTaskScripts(string[] taskNames, object arg, bool startParallel);

        Task<object> ExecuteTaskScriptsAsync(string[] taskNames, object arg, bool startParallel);
        
        object ExecuteEventScript(string taskName, object arg);

        bool CompileScriptProject();

        ScriptingTargetProject ScriptingTargetProject { get;  }
        //string[] References { get; set; }
        //void AddReference(string dll);
        //void RemoveReference(string dll);


        Subject<IObservableEvent> ScriptChangeSubject { get; }
    }
}
