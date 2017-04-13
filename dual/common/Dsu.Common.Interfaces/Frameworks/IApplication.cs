using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Application interface
    /// </summary>
    [Guid("D6268C82-0609-40C9-B416-91409A07AD46")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IApplication
    {
        /// <summary> Name of the application </summary>
        string ApplicationName { get; }

        /// <summary> HKCU 하부의 위치 </summary>
        string RegistryLocation { get; }

        /// <summary> Application 의 status label 에 주어진 text 를 write 한다. </summary>
        string StatusBarText { get; set; }
    }


    /// <summary>
    /// Form 을 갖는 application
    /// </summary>
    [ComVisible(false)]
    public interface IFormApplication : IApplication
    {
        /// <summary> Application 을 구성하는 main form </summary>
        Form TheApplicationForm { get; }

        /// <summary> 주어진 action 을 background 로 실행 </summary>
        void RunBackgroundAction(string actionName, Action action, bool withProgressUI);
    }

    /// <summary>
    /// Application interface
    /// </summary>
	[Guid("34C88331-A922-4338-920B-93A9585D4E00")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IApplicationWithDocuments : IApplication
    {
        /// <summary> Active document getter </summary>
        IDocument ActiveDocument { get; }

        /// <summary> Active view getter </summary>
        IView ActiveView { get; }

        /// <summary> Create new document </summary>
        IDocument NewDocument(string documentType);

        /// <summary> Open document whose name given with filename </summary>
        IDocument OpenDocument(string filename);

        /// <summary> Closes document </summary>
        bool CloseDocument(IDocument document);

        /// <summary> Save active document </summary>
        bool Save();
        /// <summary> Save active document as given filename</summary>
        bool SaveAs(string filePath);
    }
}
