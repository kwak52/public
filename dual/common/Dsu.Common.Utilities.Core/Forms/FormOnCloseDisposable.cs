using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    /*
     * using() 구문을 사용하지 않더라도, form 이 닫힐 때에 Dispose() 를 가능하게 하는 Form 의 base.
     */
    public class FormOnCloseDisposable : Form
    {
        public FormOnCloseDisposable()
        {
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
        }


        protected virtual void OnClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }
    }
}
