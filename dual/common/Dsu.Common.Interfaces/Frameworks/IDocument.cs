using System;
using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary> 범용 문서 interface </summary>
	[Guid("B16FBE94-A498-44E0-8060-170D61D22E97")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IDocument : INamed, IDescribable, ISanityCheckable, IDisposable
    {
        /// <summary> 문서의 file 경로 </summary>
        string FilePath { get; }
        /// <summary> 문서의 GUID </summary>
        string GuidString { get; set; }

        /// <summary> Program generated notes.  e.g Note="Creator=GLComConverter1.0;Date=20150410;" </summary>
        string ProgramNote { get; set; }

        /// <summary> 문서 수정 여부 </summary>
        bool Dirty { get; set; }

        /// <summary> 저장 </summary>
        bool Save();
        /// <summary> 다른 이름으로 저장 </summary>
        bool SaveAs(string filePath);
    }    
}

