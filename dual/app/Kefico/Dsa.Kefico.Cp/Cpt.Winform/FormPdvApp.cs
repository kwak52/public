using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ActorMessages;
using static PdvManagerModule;

namespace Cpt.Winform
{
    public partial class FormPdvApp : Form
    {
        private PdvManager _pdvManager;
        public FormPdvApp()
        {
            InitializeComponent();

            _pdvManager = new PdvManager(new PdvClientConfig());
            var response = _pdvManager.Ask(new AmRequestCreateFolder("1234567890", "MM", "XX"));
            MessageBox.Show(response.ToString());
        }
    }
}
