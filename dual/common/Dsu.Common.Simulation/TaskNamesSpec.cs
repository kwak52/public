using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Dsu.f3d.Interfaces;
using Dsu.Common.Interfaces;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Simulation
{
    [ComVisible(false)]
    public interface ITaskNameSpec
    {
        bool HasAny();
        string ToString();
        void Set(string taskNamesSpec);
        bool Contains(ITask task);
        void Add(ITask task);
        bool Remove(ITask task);
    }

    public class TaskNameSpec : ITaskNameSpec
    {
        public string TaskName3d { get; set; }
        public string TaskNameIo { get; set; }

        public string AnyTaskName { get { return TaskName3d.Any() ? TaskName3d : TaskNameIo; } }

        public void Clear()
        {
            TaskName3d = String.Empty;
            TaskNameIo = String.Empty;            
        }
        public bool Contains(ITask task)
        {
            if (task.Is3dTask)
                return TaskName3d == task.UniqueName;

            return TaskNameIo == task.UniqueName;
        }

        public void Add(ITask task)
        {
            Clear();
            if (task.Is3dTask)
                TaskName3d = task.UniqueName;
            else
                TaskNameIo = task.UniqueName;
        }

        public bool Remove(ITask task)
        {
            if (task.Is3dTask)
            {
                if (TaskName3d == task.UniqueName)
                {
                    TaskName3d = String.Empty;
                    return true;
                }
            }
            else
            {
                if (TaskNameIo == task.UniqueName)
                {
                    TaskNameIo = String.Empty;
                    return true;
                }                
            }            

            return false;
        }

        public override string ToString()
        {
            if (TaskName3d.NonNullAny() && TaskNameIo.NonNullAny())
                return String.Format("{0}@3d;{1}@Io", TaskName3d, TaskNameIo);
            if (TaskName3d.NonNullAny())
                return TaskName3d + "@3d";
            if (TaskNameIo.NonNullAny())
                return TaskNameIo + "@Io";

            return String.Empty;
        }

        public TaskNameSpec() { }

        public TaskNameSpec(string taskNamesSpec)
        {
            Set(taskNamesSpec);
        }

        public void Set(ITask task)
        {
            if (task.Is3dTask)
                TaskName3d = task.UniqueName;
            else
                TaskNameIo = task.UniqueName;
        }

        private string StripSuffix(string taskNameSpec)
        {
            if (taskNameSpec.EndsWith("@3d") || taskNameSpec.EndsWith("@Io"))
                return taskNameSpec.Substring(0, taskNameSpec.Length - 3);

            return taskNameSpec;
        }

        public void Set(string taskNameSpec)
        {
            Clear();

            if (taskNameSpec.IsNullOrEmpty())
                return;

            if ( taskNameSpec.EndsWith("@3d"))
                TaskName3d = StripSuffix(taskNameSpec);
            else if (taskNameSpec.EndsWith("@Io"))
                TaskNameIo = StripSuffix(taskNameSpec);
            else
                throw new UnexpectedCaseOccurredException("Not proper task spec : " + taskNameSpec);
        }

        public bool HasAny() {  return TaskName3d.Any() || TaskNameIo.Any(); }
    }


    /// <summary>
    /// Symbol 의 output binding 등에 사용되는 task 목록을 관리하기 위한 class.  OutputRising/OutputFalling, InputRising/InputFalling, ...
    /// @3d; 등의 구분자가 task 명에 사용되면 곤란...
    /// </summary>
    public class TaskNamesSpec : ITaskNameSpec
    {
        public HashSet<string> TaskNames3d { get { return _taskNames3d;} }
        public HashSet<string> TaskNamesIo { get { return _taskNamesIo; } }
        private HashSet<string> _taskNames3d = new HashSet<string>();
        private HashSet<string> _taskNamesIo = new HashSet<string>();

        public bool Contains(ITask task)
        {
            if ( task.Is3dTask )
                return TaskNames3d.Contains(task.UniqueName);

            return TaskNamesIo.Contains(task.UniqueName);
        }

		public bool Contains( bool b3d, string name )
		{
			if ( b3d )
				return TaskNames3d.Contains( name );

			return TaskNamesIo.Contains( name );
		}

        public void Add(ITask task)
        {
            var taskName = task.UniqueName;
            if (task.Is3dTask)
                TaskNames3d.Add(taskName);
            else
                TaskNamesIo.Add(taskName);
        }

		public void Add( bool b3d, string name )
		{
			if ( b3d )
				TaskNames3d.Add( name );
			else
				TaskNamesIo.Add( name );
		}

        public bool Remove(ITask task)
        {
            var taskName = task.UniqueName;
            if (task.Is3dTask)
                return TaskNames3d.Remove(taskName);

            return TaskNamesIo.Remove(taskName);
        }

		public bool Remove( bool b3d, string name )
		{
			if ( b3d )
				return TaskNames3d.Remove( name );

			return TaskNamesIo.Remove( name );
		}

        public override string ToString()
        {
            var sb = new StringBuilder();
            TaskNames3d.ForEach(t => sb.Append(t + "@3d;"));
            TaskNamesIo.ForEach(t => sb.Append(t + "@Io;"));
            return sb.ToString();
        }

        public TaskNamesSpec() { }

        public TaskNamesSpec(string taskNamesSpec)
        {
            Set(taskNamesSpec);
        }
        public void Set(string taskNamesSpec)
        {
            TaskNames3d.Clear();
            TaskNamesIo.Clear();

            var tuple = ParseNamesSpec(taskNamesSpec);
            tuple.Item1.ForEach(t => TaskNames3d.Add(t));
            tuple.Item2.ForEach(t => TaskNamesIo.Add(t));
        }

        /// <summary>
        /// </summary>
        /// <param name="taskNamesSpec">e.g "AttachRB1@3d;DoSomething@Io"</param>
        /// <returns></returns>
        public static Tuple<List<string>, List<String>> ParseNamesSpec(string taskNamesSpec)
        {
            var taskNames3d = new List<string>();
            var taskNamesIo = new List<string>();
            taskNamesSpec.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .ForEach(token =>
                {
                    if (token.Length >= 3 && token.EndsWith("@3d") || token.EndsWith("@Io"))
                    {
                        var task = token.Substring(0, token.Length - 3);
                        if (token.EndsWith("@3d"))
                            taskNames3d.Add(task);
                        else // if (token.EndsWith("@Io"))
                            taskNamesIo.Add(task);
                    }
                    else
                    {
                        throw new UnexpectedCaseOccurredException("Not proper task name : " + token);
                    }
                });

            return new Tuple<List<string>, List<string>>(taskNames3d, taskNamesIo);
        }


        public bool HasAny() {  return ! IsEmpty(); }
        public bool IsEmpty() { return TaskNames3d.IsNullOrEmpty() && TaskNamesIo.IsNullOrEmpty(); }
    }
}
