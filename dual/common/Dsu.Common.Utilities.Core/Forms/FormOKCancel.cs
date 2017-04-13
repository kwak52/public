using System;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    /*
     * OK, Cancel 을 지원하는 범용 form dialog base.
     * Step.
     * 1. Form 을 하나 생성하고(FormEx 라고 하자), OK, Cancel button 을 desginer 에서 생성한다.  각각 btnOK, btnCancel 이라고 부르자.
     * 2. FormEx 의 base 를 Form 에서 FormOKCancel 로 바꾼다.
     * 3. FormEx 의 생성자에서 base.InitializeFormOKCancel(btnOK, btnCacel) 를 호출한다.
     * --> FormEx.ShowDialog 결과를 DialogResult 로 받을 수 있다.
     * 4. 추가로 OK, Cancel 시에 작업을 더 필요로 하는 경우는 FormEx 에서 OnOK 및 OnCancel 을 override 한다.
     */
    public class FormOKCancel : Form
    {
        private Button m_btnOK = null;
        private Button m_btnCancel = null;

        public void InitializeFormOKCancel(Button btnOK, Button btnCancel)
        {
            m_btnOK = btnOK;
            m_btnCancel = btnCancel;

            if ( btnOK != null )
                m_btnOK.Click += new System.EventHandler(this.OnOK);

            if ( btnCancel != null )
                m_btnCancel.Click += new System.EventHandler(this.OnCancel);
            this.Load += new System.EventHandler(this.FormOKCancel_Load);
        }

        private void FormOKCancel_Load(object sender, EventArgs e)
        {
            if (m_btnOK != null)
                m_btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            if (m_btnCancel != null)
                m_btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }

        protected virtual void OnOK(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected virtual void OnCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
    
}
