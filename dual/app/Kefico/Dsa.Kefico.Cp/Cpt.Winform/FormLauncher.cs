using System.Windows.Forms;
using log4net;

namespace Cpt.Winform
{
    public partial class FormLauncher : Form
    {
        private void Launch(Form form)
        {
            this.Visible = false;
            form.Show();
            form.Closed += (sender, args) => { this.Close(); };
        }
        public FormLauncher(ILog logger)
        {
            InitializeComponent();

            btnNewRequest.Click += (sender, args) => { Launch(new FormCptRequest(logger)); };
            btnPdvTest.Click += (sender, args) => { Launch(new FormPdvApp()); };
        }
    }
}
