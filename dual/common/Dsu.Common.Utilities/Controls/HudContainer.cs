using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities.Controls
{
    public class HudContainer : IHudContainer, IDisposable
    {
        public Control HudOwner { get; private set; }
        public IHudBar[] HudBars { get { return _hudBars.ToArray(); } }
        private List<IHudBar> _hudBars = new List<IHudBar>();
        public void AddHudBar(IHudBar panel)
        {
            _hudBars.Add(panel);

            HudOwner.Controls.Add(panel.Control);
            HudOwner.Controls.SetChildIndex(panel.Control, 0);
        }

        public bool RemoveHudBar(IHudBar panel)
        {
            HudOwner.Controls.Remove(panel.Control);
            return _hudBars.Remove(panel);
        }

        public HudContainer(Control hudOwner)
        {
            Contract.Requires(hudOwner is IHudContainer);
            HudOwner = hudOwner;
        }

       public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

       ~HudContainer() { Dispose(false); } 
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (HudOwner != null)
                {
                    HudOwner.Dispose();
                    HudOwner = null;
                }

                _hudBars.ForEach(hb => hb.Dispose());
                _hudBars.Clear();
            }

            _disposed = true;
        }
    }
}
