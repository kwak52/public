using Dsu.Driver.Util.Emergency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reactive.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using CpTesterPlatform.CpTester;
using System.Drawing;
using System.Threading.Tasks;
using CpTesterPlatform.Functions;

namespace CpTesterSs.Event
{
    public partial class FormNotify : Form
    {
        public static FormNotify TheForm;
        private string _message;
        public FormNotify(string message)
        {
            InitializeComponent();
            TheForm = this;
            _message = message;
        }


        private void FormNotify_Load(object sender, EventArgs e)
        {
            Location = new Point(Location.X, Location.Y + 300);
            labelDescription.Text = _message;
        }
    }

    public class CpNotifyMessageDisplayer : CpUtil.NotifyMessageDisplayer
    {
        public CpNotifyMessageDisplayer(string message) : base(message)
        {
            CpFunctionDefault.Util.CpLog4netLogging.LogInfo(message);
        }
    }
}
