﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// http://www.gamlor.info/wordpress/2010/10/c-5-0-async-feature-be-aware-of-the-synchronization-context/
    /// </summary>
    public class SynchronizationContextEx : SynchronizationContext
    {
        private readonly Queue<Action> messagesToProcess = new Queue<Action>();
        private readonly object syncHandle = new object();
        private bool isRunning = true;

        public override void Send(SendOrPostCallback codeToRun, object state) { throw new NotImplementedException(); } 
        public override void Post(SendOrPostCallback codeToRun, object state)
        {
            lock (syncHandle)
            {
                messagesToProcess.Enqueue(() => codeToRun(state));
                SignalContinue();
            }
        }

        public void RunMessagePump()
        {
            while (CanContinue())
            {
                Action nextToRun = GrabItem();
                nextToRun();
            }
        }

        private Action GrabItem()
        {
            lock (syncHandle)
            {
                while (CanContinue() && messagesToProcess.Count == 0)
                {
                    Monitor.Wait(syncHandle);
                }
                return messagesToProcess.Dequeue();
            }
        }

        private bool CanContinue()
        {
            lock (syncHandle)
            {
                return isRunning;
            }
        }

        public void Cancel()
        {
            lock (syncHandle)
            {
                isRunning = false;
                SignalContinue();
            }
        }

        private void SignalContinue()
        {
            Monitor.Pulse(syncHandle);
        }
    }
}
