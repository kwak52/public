using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dsu.Common.Utilities.MVCs
{
    /// <summary>
    /// Document 하나당 controller 가 생성됨.  해당 document 와 연결된 모든 view 를 하나의 controller 에서 manange 함.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TView"></typeparam>
    [ComVisible(false)]
    public abstract class MvcControlBase<TModel, TView> : IDisposable
        where TModel : IDisposable
        where TView : IDisposable
    {
        #region Member Variables

        protected List<TView> m_views = new List<TView>();
        protected TModel m_model;

        #endregion

        #region Abstract Methods

        protected abstract void WireEvents();
        protected abstract void UnwireEvents();
        protected abstract void UpdateViews();

        #endregion

        #region Methods

        protected MvcControlBase(TModel model, TView view)
        {
            if (m_model != null || m_views.Count > 0)
                UnwireEvents();

            m_model = model;
            m_views.Add(view);

            UpdateViews();
            WireEvents();
        }

        #endregion

        #region Properties

        public TModel Model { get { return m_model; } }

        public IEnumerable<TView> Views
        {
            get { return m_views; } 
            set
            {
                if ( value == null )
                    m_views.Clear();
                else
                    m_views = value.ToList();
            }
        }

        public TView View { get { return m_views.FirstOrDefault(); } }

        #endregion


        
        
        #region IDisposable Members

        private bool _disposed;

        ~MvcControlBase()
        {
            Dispose(false);     // false : 암시적 dispose 호출
        }
        public void Dispose()
        {
            Dispose(true);      // true : 명시적 dispose 호출
            GC.SuppressFinalize(this);  // 사용자에 의해서 명시적으로 dispose 되었으므로, GC 는 이 객체에 대해서 손대지 말것을 알림.
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (m_model != null)
                {
                    UnwireEvents();
                    m_model.Dispose();
                    m_model = default(TModel);
                }

                m_views.ToList().ForEach(v => v.Dispose());            // .ToList(). : 컬렉션이 수정되었습니다. 열거 작업이 실행되지 않을 수도 있습니다.
                m_views.Clear();
            }

            /*
             * release any unmanaged objects
             * set the object references to null
             */

            _disposed = true;
        }

        #endregion
    }
}
