using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    public static class EmSynchronizationContext
    {
        /// <summary>
        /// Control 객체로부터 SynchronizationContext 반환
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static SynchronizationContext GetSynchronizationContext(this Control control)
        {
            return control.DoGet<SynchronizationContext>(() => { return SynchronizationContext.Current; });
        }

        public static TaskScheduler GetTaskScheduler(this SynchronizationContext context)
        {
            TaskScheduler scheduler = null;
            context.Send(state =>
            {
                scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            }, null);

            return scheduler;
        }

        //public void DoSomeStuffOnBackgroundThread(SynchronizationContext synchronizationContext)
        //{
        //    // Do some stuff here
        //    // ...

        //    // Show the dialog on the UI thread
        //    var dialog = new SaveFileDialog();
        //    synchronizationContext.Send(() => dialog.Show());

        //    // Send is performed synchronously, thus this line of code only executes when the dialog was closed. You can extract the file name here
        //    var fileName = dialog.FileName;
        //}
    }
}
