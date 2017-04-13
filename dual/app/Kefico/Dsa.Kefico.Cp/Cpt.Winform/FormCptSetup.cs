using System;
using System.Windows.Forms;
using static CSharpInterop;

namespace Cpt.Winform
{
    public partial class FormCptSetup : Form
    {
        private CptModule.CptHostConfig _config;

        public FormCptSetup(CptModule.CptHostConfig config)
        {
            InitializeComponent();
            _config = config;
            textBoxHost.Text = valueFromOption(_config.Host).ToString();
            textBoxSection.Text = valueFromOption(_config.Section);
            textBoxFixture.Text = valueFromOption(_config.Fixture);
            textBoxBatch.Text = valueFromOption(_config.Batch);
            cbEnableDebug.Checked = MwsConfig.mwsEnableDebug;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            _config.Host = createSome(Int32.Parse(textBoxHost.Text));
            _config.Section = createSome(textBoxSection.Text);
            _config.Fixture = createSome(textBoxFixture.Text);
            _config.Batch = createSome(textBoxBatch.Text);
            Close();
        }

        private void cbEnableDebug_CheckedChanged(object sender, EventArgs e)
        {
            MwsConfig.mwsEnableDebug = cbEnableDebug.Checked;
        }
    }
}
