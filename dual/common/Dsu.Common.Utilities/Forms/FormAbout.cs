using System;
using System.Diagnostics;
using System.Windows.Forms;
using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities
{
    public partial class FormAbout : FormHelper
    {
        public FormAbout(IApplication application, string aboutText)
        {
            InitializeComponent();

            var iconHolder = application as IIconHolder;
            if (iconHolder != null)
                Icon = iconHolder.DefaultIcon;
            Text = "About " + application.ApplicationName;
            textBoxAbout.Text = aboutText;
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            textBoxAbout.Select(0, 0);
            linkLabelHomePage.Focus();
        }

        private void linkLabelHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabelHomePage.Text);
        }
    }
}
