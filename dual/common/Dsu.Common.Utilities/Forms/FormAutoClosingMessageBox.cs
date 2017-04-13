using System;
using System.Threading;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    public partial class FormAutoClosingMessageBox : Form
    {
        public string Message { get { return textBoxMessage.Text; } set { textBoxMessage.Text = value; } }
        public string Title { get { return Text; } set { Text = value; } }
        public int ShowTime { get { return _remainingSeconds; } set { _remainingSeconds = value; } }
        private int _remainingSeconds;


        public FormAutoClosingMessageBox(string message, int showTimeSeconds)
        {
            InitializeComponent();

            textBoxMessage.Text = message;
            _remainingSeconds = showTimeSeconds;
            textBoxRemainingTime.Text = _remainingSeconds.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (--_remainingSeconds < 0)
                Close();

            textBoxRemainingTime.Text = _remainingSeconds.ToString();
        }
    }


    // http://stackoverflow.com/questions/14522540/close-a-messagebox-after-several-seconds/14522952#14522952
    //[Obsolete("Use PopupToolTip class instead.")]
    public class AutoClosingMessageBox
    {
        static private void CloseIt(int showTimeMilli)
        {
            Thread.Sleep(showTimeMilli);
            Microsoft.VisualBasic.Interaction.AppActivate(
                 System.Diagnostics.Process.GetCurrentProcess().Id);
            SendKeys.SendWait(" ");
        }

        static public void Show(string message, int showTimeMilli, string title = null)
        {
            //new Thread(() => CloseIt(showTimeMilli)).Start();
            //MessageBox.Show(message, title);

            new FormAutoClosingMessageBox(message, showTimeMilli / 1000) { Title = title }.Show();
        }
    }

    
    

}
