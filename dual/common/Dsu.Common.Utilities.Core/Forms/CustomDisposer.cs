using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Dispose 처리가 필요한 Form 이나 UserControl 이 구현해야할 interface.
    /// Form 이나 UserControl 생성자에 다음 코드를 추가한다.
    /// 
    ///         if ( this.components == null )
    ///             this.components = new System.ComponentModel.Container();
    ///         components.Add(new CustomDisposer(OnCustomDispose));
    /// 
    /// </summary>
    public interface ICustomDisposer
    {
        void OnCustomDispose(bool disposing);
    }
    /// <summary>
    /// http://blogs.msdn.com/b/ploeh/archive/2006/08/10/howtodisposemembersfromforms.aspx
    /// </summary>
    public class CustomDisposer : Component
    {
        private Action<bool> _dispose;
        public CustomDisposer(Action<bool> disposeCallback)
        {
            _dispose = disposeCallback;
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _dispose(disposing);
        }
    }
}
