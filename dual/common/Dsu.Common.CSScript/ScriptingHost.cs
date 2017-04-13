using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using csscript;
using CSScriptLibrary;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Scripting Host 구현을 위한 class.
    /// <para/> - 실제 script 기능을 필요로 하는 class (==> this.Container)는
    ///         IScriptingHost 에서 상속받고, script 기능의 실제 구현은 본 클래스 ScriptingHost 에게 위임한다.
    /// </summary>
    public partial class ScriptingHost : IScriptingHost, IDisposable
    {
        public static readonly LogProxy logger = LogProxy.CreateLoggerProxy(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Scripting host implementation(this) 을 포함하는 IScriptingHost 객체.
		/// </summary>
		public IScriptingHost Container { get; private set; }

        protected Dictionary<string, TaskScript> _taskScripts = new Dictionary<string, TaskScript>();
        protected Dictionary<string, TaskScript> _eventScripts = new Dictionary<string, TaskScript>();

        public Dictionary<string, TaskScript> TaskScripts { get { return _taskScripts; } }


        public virtual Icon DefaultIcon { get { return null; } }




        #region IScriptingHost implemetation
        private Subject<IObservableEvent> _scriptChangeSubject = new Subject<IObservableEvent>();
        public Subject<IObservableEvent> ScriptChangeSubject { get { return _scriptChangeSubject; } }
        #endregion


        public ScriptingHost(IScriptingHost container)
        {
            Container = container;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ScriptingHost() { Dispose(false); } 
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Assembly = null;
                Container = null;
            }

            _disposed = true;
        }


        public string[] TaskScriptNames
        {
            [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
            get { return _taskScripts.Keys.ToArray(); }
        }

        public string[] EventScriptNames
        {
            [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
            get { return _eventScripts.Keys.ToArray(); }
        }

        /// <summary> {task, event} script 함수 모두를 대상으로 script 이름을 검색해서, 해당 script 의 comment 를 반환한다.  </summary>
        /// <param name="scriptName">검색 대상 script 명</param>
        /// <returns></returns>
        public string GetScriptComment(string scriptName)
        {
            if (_taskScripts.Keys.Contains(scriptName))
                return _taskScripts[scriptName].Comment;
            if (_eventScripts.Keys.Contains(scriptName))
                return _eventScripts[scriptName].Comment;

            return null;
        }

        public bool HasScriptMethod(string scriptName)
        {
            return _eventScripts.Keys.Contains(scriptName) || _taskScripts.Keys.Contains(scriptName);
        }

        protected virtual string[] GetPredefinedScriptName()
        {
            return null;
        }

        /// <summary> 아직 사용자가 정의하지 않은 event script 까지 포함하여, 정의 가능한 모든 event script 명을 반환 </summary>
        public string[] DefinableEventScriptNames
        {
            [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
            get { return GetPredefinedScriptName(); }
        }

        public object ExecuteTaskScripts(string[] taskNames, object arg, bool startParallel)
        {
            return ExecuteTaskScriptsAsync(taskNames, arg, startParallel).Result;
        }

        public async Task<object> ExecuteTaskScriptsAsync(string[] taskNames, object arg, bool startParallel)
        {
            if (taskNames.IsNullOrEmpty())
                throw new ArgumentNullException("No task names specified for execution.");

            List<Task> tasks = new List<Task>();
            if (startParallel)
                await Task.WhenAll(taskNames.Select(t => ExecuteTaskScriptAsync(t, arg)));
            else
            {
                foreach (var t in taskNames)
                    await ExecuteTaskScriptAsync(t, arg);
            }

            return true;
        }


        public virtual object ExecuteTaskScript(string taskName, object arg)
        {
            return ExecuteTaskScript(_taskScripts[taskName], arg);
        }

        public async virtual Task<object> ExecuteTaskScriptAsync(string taskName, object arg)
        {
            logger.InfoFormat("Executing task script {0}({1}) in sync mode", taskName, arg);
            return await ExecuteTaskScriptHelperAsync(_taskScripts[taskName], arg);
        }

        public virtual object ExecuteTaskScript(TaskScript taskScript, object arg)
        {
            logger.InfoFormat("Executing task script {0}({1}) in sync mode", taskScript.Name, arg);
            return ExecuteTaskScriptAsync(taskScript, arg).Result;
        }

        public async virtual Task<object> ExecuteTaskScriptAsync(TaskScript taskScript, object arg)
        {
            logger.InfoFormat("Executing task script {0}({1}) in async mode", taskScript.Name, arg);
            return await ExecuteTaskScriptHelperAsync(taskScript, arg);
        }

        private async Task<object> ExecuteTaskScriptHelperAsync(TaskScript taskScript, object arg)
        {
            ScriptChangeSubject.OnNext(new ObservableEventTaskStarting(taskScript));

            var result = await taskScript.ExecuteAsync(arg);

            ScriptChangeSubject.OnNext(new ObservableEventTaskFinished(taskScript));

            return result;
        }

        public virtual object ExecuteEventScript(string taskName, object arg)
        {
            try
            {
                if ( _eventScripts.ContainsKey(taskName) )
                    return _eventScripts[taskName].Execute(arg);
                return false;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Failed to executing task script {0}({1}) : {2}", taskName, arg.ToString(), ex.Message);
                throw new ScriptTaskException("Script exception : " + ex.Message);
            }
        }



        public ScriptingTargetProject ScriptingTargetProject { get; private set; }
        public string[] References { get { return ScriptingTargetProject.References; } }

        private string _scriptProjFile;
        public string ScriptProjectFileName
        {
            get { return _scriptProjFile; }
            set
            {
                if (_scriptProjFile != value)
                {
                    try
                    {
                        Assembly = null;
                        _scriptProjFile = value;
                        CompileScriptProject();

                        if (CommonApplication.TheCommonConfiguration != null && CommonApplication.TheCommonConfiguration.IsAutomaticEnableLogForUserScript)
                        {
                            LogProxy.CurrentLoggers
                                .Where(l => l.Type.GetCustomAttributes(false).Cast<Attribute>().OfType<ScriptClass>().Any())
                                .ForEach(l => l.EnableAll());
                        }
                    }
                    finally
                    {
                        // script 컴파일이 실패하더라도 script file name 은 계속 간직하고 있어야 한다.
                        _scriptProjFile = value;
                    }
                }
            }
        }



        public virtual bool CompileScriptProject()
        {
            if (ScriptProjectFileName.IsNullOrEmpty())
                return false;

            if (!File.Exists(ScriptProjectFileName))
                throw new FileNotFoundException(String.Format("Script file {0} not found", ScriptProjectFileName));

            try
            {
                InitializeCompiler();

                _taskScripts.Clear();
                _eventScripts.Clear();

                /*
                 * compile 이전에 필요한 작업을 수행한다.
                 */
                if (!AssemblyLoading())
                    return false;

                Assembly = CompileAssemblyFromFile(ScriptingTargetProject.AbsolutePathSourceFiles);

                /*
                 * compile 된 코드를 기반으로 필요한 작업을 수행한다.
                 */
                AssemblyLoaded();

                var args = new ScriptReloadedEvent(this, ScriptProjectFileName);
                if ( Container != null )
                    Container.ScriptChangeSubject.OnNext(args);

                ScriptChangeSubject.OnNext(args);

                return true;
            }
            catch (CompilerException ex)
            {
                if ( DialogResult.Yes == MessageBox.Show(String.Format("Compile error : {0}\n{1}\r\n=========================\r\nDo you want to clear script file reference?", ScriptProjectFileName, ex.Message), "Script Compile Error!"))
                {
                    Assembly = null;
                    _scriptProjFile = null;
                }

                return false;
            }
        }


        /// <summary>
        /// class name - class instance map : Non-static class 의 method instance 를 접근하기 위해서 필요
        /// </summary>
        private Dictionary<string, object> _instanceMap = new Dictionary<string, object>();

        protected MethodDelegate GetMethod(string fullMethodName, object[] paramsList)
        {
            var hierachialNames = fullMethodName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            var methodName = hierachialNames.Last();
            var className = String.Join(".", hierachialNames.Take(hierachialNames.Length - 1));
            return GetMethod(className, methodName, new object[] { });
        }

        protected MethodDelegate GetMethod(string namespaceName, string className, string methodName, object[] paramsList)
        {
            Contract.Requires(!string.IsNullOrEmpty(namespaceName));
            return GetMethod(namespaceName + "." + className, methodName, paramsList);
        }

        protected MethodDelegate GetMethod(string className, string methodName, object[] paramsList)
        {
            Contract.Requires(!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(methodName));
            object instance;
            if (_instanceMap.ContainsKey(className))
                instance = _instanceMap[className];
            else
            {
                instance = Assembly.CreateInstance(className);
                _instanceMap.Add(className, instance);
            }

			return new AsmHelper(Assembly).GetMethod(instance, methodName, paramsList);
        }

        protected MethodDelegate GetMethod(MethodInfo mi, object[] paramsList)
        {
            if (mi == null)
                return null;

            if (mi.IsStatic)
                return new AsmHelper(Assembly).GetStaticMethod(mi.Name, paramsList);

            var className = mi.ReflectedType.FullName;
            return GetMethod(className, mi.Name, paramsList);
        }


        protected virtual bool AssemblyLoaded()
        {
            /*
             * Assembly 에서 제공하는 type 들 중에서
             *      - [ScriptClass] attribute 를 가지며
             *      - public default constructor 를 제공하며
             *      - default constructor 가 abstract 가 아닌
             *  모든 class 들에서 public 함수를 골라내어
             *      - [ScriptMethod] attribute 를 가지며
             *      - return type : object or Task<object>
             *      - parameter 는 1 개
             *      
             * 
             *  모든 메소드들을 검색
             *  
             */
            List<MethodInfo> filteredMethods = Assembly
                .GetTypes()
                    .Where(w => w.GetCustomAttributes().OfType<ScriptClass>().Any())
                    .Where(w => w.GetConstructor(Type.EmptyTypes).IsPublic)
                    .Where(w => !w.GetConstructor(Type.EmptyTypes).IsAbstract)
                .SelectMany(x => x.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
                    .Where(y => y.GetCustomAttributes().OfType<ScriptMethod>().Any())
                    .Where(y => y.ReturnType == typeof(object) || y.ReturnType == typeof(Task<object>))
                    .Where(y => y.GetParameters().Length == 1)
                    .Where(y => y.GetParameters().First().ParameterType == typeof(object))
                //  .Where(y => y.IsPublic)
                .ToList()
                ;

            var parameters = new[] { new object() };

            /*
             * method 중에서 사용자 정의 method(=> 사전 정의된 method 이름인 method)들을 추출
             */
            foreach (var m in filteredMethods.Where(f => GetPredefinedScriptName().Contains(f.Name)))
            {
                var attr = m.GetCustomAttributes().OfType<ScriptMethod>().FirstOrDefault();
                string comment = attr == null ? "" : attr.Comment;

                var method = GetMethod(m.ReflectedType.FullName, m.Name, parameters);
                _eventScripts.Add(m.Name, new TaskScript(m.Name, method, comment));
            }

            /*
             * method 중에서 사용자 정의 method(=> 사전 정의된 method 이름이 아닌 method)들을 추출
             */
            foreach (var m in filteredMethods.Where(f => !GetPredefinedScriptName().Contains(f.Name)))
            {
                var attr = m.GetCustomAttributes().OfType<ScriptMethod>().FirstOrDefault();
                string comment = attr == null ? "" : attr.Comment;

                var method = GetMethod(m.ReflectedType.FullName, m.Name, parameters);
                _taskScripts.Add(m.Name, new TaskScript(m.Name, method, comment));
            }

            return true;
        }

        public void CompileScript(string scriptFile)
        {
            ScriptProjectFileName = scriptFile;
        }

    }
}
