using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    public abstract class AppScriptBase
    {
        public static void WaitUntil(Func<bool> predicate, int checkIntervalMili = 200)
        {
            while (!predicate())
                Thread.Sleep(checkIntervalMili);
        }

        protected abstract LogProxy Logger { get; }

        protected abstract Form GetApplicationForm();
        protected abstract void SimulationPause();
        protected abstract void SimulationResume();
        protected abstract bool IsSimulationRunning();
        public void ShowBlockedMessage(string message)
        {
            GetApplicationForm().Do(() =>
            {
                bool needPauseResume = IsSimulationRunning();
                if (needPauseResume)
                    SimulationPause();

                Task.Run(() =>
                {
                    Logger.Error(message);
                    if (needPauseResume)
                    {
                        message += "\r\n\r\nDo you want to resume the scheduler?";
                        if (DialogResult.Yes == MessageBox.Show(message, "Blocked message", MessageBoxButtons.YesNo))
                            SimulationResume();
                    }
                    else
                        MessageBox.Show(message, "Blocked message");
                });
            });
        }

        public bool CheckNonNulls(string message, params object[] objects)
        {
            if (objects.NonNullAny(o => o == null))
            {
                ShowBlockedMessage(message);
                return false;
            }

            return true;
        }

        public void Assert(bool expression, string message)
        {
            if (!expression)
                ShowBlockedMessage(message);
        }
    }
}
