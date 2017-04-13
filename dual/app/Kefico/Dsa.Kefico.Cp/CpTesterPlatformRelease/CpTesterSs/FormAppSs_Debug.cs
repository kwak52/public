using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using CpApplication.Manager;
using CpTesterPlatform.CpCommon;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.Exceptions;
using Dsu.Common.Utilities.Forms;

namespace CpTesterPlatform.CpTester
{
	partial class FormAppSs
	{
		private void barButtonItemTriggerEventOnThread_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			Trace.WriteLine(CommonApplication.GetProfilePath());
			logger.Info($"LogFileName = {log4net.GlobalContext.Properties["LogFileName"]}");
			logger.Info("Triggering event on a thread.");
			Task.Run(() =>
			{
				Task.Delay(500);
				CptApplication.TheApplication.ApplicationSubject.OnNext(new MyEvent());
			});
		}

		private void barButtonItemExceptionStackTrace_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			try
			{
				throw new Exception("This is sample exception, intentionally generated..");
			}
			catch (Exception ex)
			{
				new FormExceptionStackTrace(ex) {/*Icon = DefaultIcon*/}.Show();
			}

		}

		private void barButtonItemGenerateUnhandledException_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			throw new Exception("this is sample unhandled exception.");
		}

		private void barButtonItemGenerateThreadedUnhandledException_ItemClick(object sender,
			DevExpress.XtraBars.ItemClickEventArgs e)
		{
			ExceptionSafeThreadStarter.Start("TestThread", this, () =>
			{
				throw new Exception("this is sample unhandled exception generated in a thread.");
			});
		}

		private void barButtonItemGenerateTaskUnhandledException_ItemClick(object sender,
			DevExpress.XtraBars.ItemClickEventArgs e)
		{
			ExceptionSafeTaskRunner.Run(() =>
			{
				ExceptionSafeTaskRunner.Run(() =>
				{
					throw new Exception("Inner exception");
				});

				//throw new Exception("Outer exception");
			}).Wait();



			// http://www.c-sharpcorner.com/uploadfile/cda5ba/exception-handling-in-parallel-task-library/
			try
			{
				Task t = Task.Factory.StartNew(() =>
				{
					Console.WriteLine("Digging is in progress");
					Task childtask = Task.Factory.StartNew(() =>
					{
						Console.WriteLine("Remove stones");

						throw new Exception("Something went wrong");

					}, TaskCreationOptions.AttachedToParent);
					throw new Exception("Tools crashed");
				});

				t.Wait();
			}
			catch (AggregateException ae)
			{
				foreach (var item in ae.Flatten().InnerExceptions)
				{
					Console.WriteLine("Stop the work and notify others :" + item.Message);
				}
			}




			var task = Task.Run(() =>
			{
				Task.Factory.StartNew(() =>
				{
					throw new Exception("this is deeper sample unhandled exception.");
				}, TaskCreationOptions.AttachedToParent);
			});

			try
			{
				task.Wait();
			}
			catch (AggregateException ex)
			{

				Console.WriteLine($"Got it : {ex}");
				throw;
			}

			Task.Run(() =>
			{
				try
				{
					Task.Run(() =>
					{
						throw new Exception("this is deeper sample unhandled exception.");
					});
					//var arr = new int[] {1};
					//int none = arr[2];
					//throw new Exception("this is sample unhandled exception.");
				}
				catch (Exception)
				{
					Console.WriteLine("Got it.");
					throw;
				}
			});
		}

		private Lazy<IActorRef> _sampleActorLazy = new Lazy<IActorRef>(() => CommonApplication.ActorSystem.ActorOf(Props.Create(() => new CpSampleActor()), "MySampleActor"));
		private IActorRef _sampleActor { get { return _sampleActorLazy.Value; } }
		private async void barButtonItemActor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			#region case 1 : actor 의 수행 종료를 기다리는 모델
			logger.Info("Sending messages to actor[sync].");
			var tasks1 = from n in Enumerable.Range(1, 10)
			select Task.Run(() => _sampleActor.Ask(n))
			;

			await Task.WhenAll(tasks1.ToArray());
			logger.Info("All messages processed.");
			#endregion



			#region case 2 : actor 의 수행 종료를 기다리지 않는 모델
			logger.Info("Sending messages to actor[async].");
			Parallel.For(0, 10, n => _sampleActor.Tell(n.ToString()));
			logger.Info("All messages sent.");
			#endregion
		}

	}
}
