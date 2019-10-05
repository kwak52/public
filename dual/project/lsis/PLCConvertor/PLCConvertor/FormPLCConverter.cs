using System;
using System.Drawing;
using System.Windows.Forms;
using log4net.Appender;
using Dsu.PLCConvertor.Common;
using Dsu.Common.Utilities.Graph;
using PLCConvertor.Forms;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Forms;

namespace PLCConvertor
{
    public partial class FormPLCConverter
        : DevExpress.XtraBars.Ribbon.RibbonForm
        , IAppender
    {
        public FormPLCConverter()
        {
            InitializeComponent();
        }

        private void FormPLCConverter_Load(object sender, EventArgs e)
        {
            Logger.Info("FormRibonApp launched.");
            Rung.Logger = Logger;

            //TestCustomAppConfig();

            //void TestCustomAppConfig()
            //{
            //    Logger.Info("Custom configuration section test:");
            //    var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //    var config = appConfig.GetSection("exportPdv") as CustomConfigurationSection;
            //    Logger.Debug($"Source: folder={config.SourceFolderPrefix}");
            //    Logger.Debug($"Destination: folder={config.DestinationFolderPrefix}");
            //    Logger.Debug($"Destination server IP: {config.DestinationDBServerIp}");
            //}

        }
        void TestConversion()
        {
            var inputs = MnemonicInput.Inputs[0].Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var rung = Rung.CreateRung(inputs);
            var graph = rung.GraphViz();
            var _pictureBox = new PictureBox() { Image = graph, Dock = DockStyle.Fill };
            var _formGraphviz = new Form() { Size = new Size(800, 500) };
            _formGraphviz.Controls.Add(_pictureBox);
            _formGraphviz.Show();
        }


        Form _lastEmbeddedForm;
        private void BarButtonItemTestParse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var formILs = new FormInputSelector();
            if (formILs.ShowDialog() == DialogResult.OK)
            {
                if (_lastEmbeddedForm != null)
                    panelMain.Controls.Remove(_lastEmbeddedForm);
                _lastEmbeddedForm = new FormLadderParse(formILs.SelectedMnemonicInput);
                _lastEmbeddedForm.Show();
                _lastEmbeddedForm.EmbedToControl(panelMain);
            }
        }
    }
}