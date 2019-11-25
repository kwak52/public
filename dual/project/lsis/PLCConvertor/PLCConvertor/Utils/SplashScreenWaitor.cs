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
        public static DialogResult Info(string title, string message)
            => XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        public static DialogResult Warn(string title, string message)
            => XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        public static DialogResult Error(string title, string message)
            => XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        public static DialogResult Ask(string title, string message)
            => XtraMessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }
}