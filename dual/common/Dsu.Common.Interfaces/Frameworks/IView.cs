using System;
using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// View interface
    /// </summary>
	[Guid("69828838-8BAB-4BA7-8ED6-8D0EF17CA495")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IView : IDisposable
    {
        /// <summary> Document of the view </summary>
        IDocument Document { get; }

        /// <summary> View invalidating </summary>
        void QInvalidate();

        /// <summary> Update model(document) </summary>
        /// <param name="reason"></param>
        void UpdateModel(string reason);
    }   
}
