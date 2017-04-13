using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace CpTesterPlatform
{
    public partial class SplashScreenGHI1 : SplashScreen
    {
        public SplashScreenGHI1()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);

			SplashScreenCommand command = (SplashScreenCommand)cmd;
            if (command == SplashScreenCommand.BringToFront)
            {
                this.BringToFront();
            }
        }

        #endregion

        public enum SplashScreenCommand
        {
            BringToFront
        }       
    }
}