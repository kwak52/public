using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dsu.Common.Utilities.Exceptions
{
	public class ExceptionSafeTaskRunner
	{
		public static Task Run(Action action)
		{
			return Task.Factory.StartNew(() => { action(); }, TaskCreationOptions.AttachedToParent);
		}


		//public static Task Run(Func<Task> function);
		//public static Task Run(Func<Task> function, CancellationToken cancellationToken);
		//public static Task Run(Action action, CancellationToken cancellationToken);
		//public static Task<TResult> Run<TResult>(Func<Task<TResult>> function);
		//public static Task<TResult> Run<TResult>(Func<TResult> function);
		//public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken);
		//public static Task<TResult> Run<TResult>(Func<Task<TResult>> function, CancellationToken cancellationToken);
	}
}
