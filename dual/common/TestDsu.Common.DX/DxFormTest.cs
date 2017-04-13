using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.XtraPrinting.Native;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.DX;
using static Dsu.Common.Utilities.Exceptions.ExceptionHandler;

namespace Test.Dsu.Common.DX
{
	public partial class DxFormTest : DevExpress.XtraEditors.XtraForm
	{
		public DxFormTest()
		{
			InitializeComponent();
		}

		private async void btnAsyncActions_Click(object sender, System.EventArgs e)
		{
            //Trace.WriteLine($"Before={DateTime.Now}");
            //var tResult = await TryFuncAsync<bool>(async () =>
            //{
            //    Trace.WriteLine($"\tInner Before={DateTime.Now}");
            //    await Task.Delay(2000);
            //    Trace.WriteLine($"\tInner Arter={DateTime.Now}");
            //    return false;
            //});
            //Trace.WriteLine($"After={DateTime.Now}");
            //Trace.WriteLine($"Result={tResult.Result}");


            Trace.WriteLine($"Before={DateTime.Now}");
            var tResult = await TryActionAsync(async () =>
            {
                Trace.WriteLine($"\tInner Before={DateTime.Now}");
                await Task.Delay(2000);
                Trace.WriteLine($"\tInner Arter={DateTime.Now}");
            });
            Trace.WriteLine($"After={DateTime.Now}");


            var tasks = new Task[]
			{
				Task.Run(async () =>
				{
					Console.Write("Hello, ");
					await Task.Delay(3000);
					Console.WriteLine("World");
				}),
				Task.Run(async () =>
				{
					Console.Write("Good ");
					await Task.Delay(300);
					Console.WriteLine("Morning");
				}),
				Task.Run(async () =>
				{
					await Task.Delay(300);
					Console.Write("How ");
					await Task.Delay(1000);
					Console.WriteLine("are you?");
				}),
			};
			
			await Task.WhenAll(tasks);

			Console.WriteLine("Finished everything.");
		}

		private void btnWrapControlIntoForm_Click(object sender, System.EventArgs e)
		{
			new DxUcTest().WrapIntoForm().Show();
		}

		private readonly int _counter = 1000;
		private readonly int _sleepInterval = 30;
		private readonly int _checkInterval = 10;


		private async void btnSleep_Click(object sender, EventArgs e)
		{
			await ActionStopWatch.MeasureAsync(Task.Run(() =>
			{
				foreach (var n in Enumerable.Range(1, _counter))
				{
					Thread.Sleep(_sleepInterval);
					if (n % _checkInterval == 0)
					{
						label1.Do(() => { label1.Text = n.ToString(); });
					}
				}
			}),
			span =>
			{
				Trace.WriteLine($"Sleep Spent {span.Milliseconds}ms");
			})
			;
		}

		private async void btnDelay_Click(object sender, EventArgs e)
		{
			await ActionStopWatch.MeasureAsync(Task.Run(async () =>
			{
				foreach (var n in Enumerable.Range(1, _counter))
				{
					await Task.Delay(_sleepInterval);
					if (n % _checkInterval == 0)
					{
						label1.Do(() => { label1.Text = n.ToString(); });
					}
				}
			}),
			span =>
			{
				Trace.WriteLine($"Delay Spent {span.Milliseconds}ms");
			})
			;
		}
	}
}