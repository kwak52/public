using System;
using System.Runtime.InteropServices;

namespace Dsu.Common.Interfaces
{
    /// <summary>
    /// Variable interface.  변수로 사용될 수 있는 객체가 구현해야 할 interface.
    /// </summary>
	[Guid("4F6CD752-7299-43A1-9DB3-BA135F865D19")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IVariable : INamed, IDescribable, ITaggable, IUniquelyNamed
    {
        /// <summary> Symbol 의 값 </summary>
        object Value { get; set; }

        /// <summary> variable 의 data type </summary>
        DataValueType DataType { get; set; }

        /// <summary> Variable 에 대한 value write request.
        /// <br/>  this.Value 를 이용하여 write 를 요청할 경우, thread 에 의해서 overwrite 될 수 있으므로, 내부 구현에서 write 요청시는 다음 field 를 이용한다.
        /// </summary>
        /// <param name="value">Write 요청 값.</param>
        /// <param name="reason">Write 요청한 원인에 대한 hint</param>
        /// <returns></returns>
        bool PostWriteRequest(object value, IReason reason);

        /// <summary> Use PostWriteRequest() instead. </summary>
        [Obsolete("Use PostWriteRequest() instead.")]
        object WriteRequestValue { set; }


        /// <summary> (PLC 로부터) readable 한지 여부.  S7 의 Input 인 경우 false </summary>
        bool Readable { get; }
        /// <summary> (PLC 로부터) writable 한지 여부.  S7 의 Output 인 경우 false </summary>
        bool Writable { get; }
    }
}
