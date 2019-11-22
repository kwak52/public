using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;


namespace PLCConvertor
{
    public class SplashScreenWaitor : IDisposable
    {
        public void Dispose()
        {
            SplashScreenManager.CloseDefaultWaitForm();
        }

        public SplashScreenWaitor(string caption = null, string description = null)
        {
            SplashScreenManager.ShowDefaultWaitForm(caption, description);
        }
    }

    public static class MsgBox
    {
        public static DialogResult Info(string text, string caption)
            => XtraMessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        public static DialogResult Warn(string text, string caption)
            => XtraMessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        public static DialogResult Error(string text, string caption)
            => XtraMessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}