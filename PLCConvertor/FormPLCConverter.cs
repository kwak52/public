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



        string input1 = @"LD A
AND B
OR C
OUT D
";

        string input2 = @"LD A
AND B
LD C
ORLD
OUT D
";

        string input3 = @"LD A
OUT TR0
AND B
OUT O1
LD TR0
AND C
OUT O2
";


        string input4 = @"LD 0.00
OUT TR0
AND 0.01
OUT 110.00
LD TR0
AND 110.00
OUT 102.10
";

        string input5 = @"LD 0.00
LD 0.01
OUT TR0
AND 0.02
ORLD
AND 0.03
OUT 102.11
LD TR0
AND 0.04
OUT 102.12
";

        string input6 = @"LD 0.00
LD 0.01
OUT TR0
AND 0.02
ORLD
AND 0.03
OUT 102.11
LD TR0
AND 0.04
OUT 102.12
";


        void TestConversion()
        {
            var inputs = input4.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
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
            var formILs = new FormSimpleEditor() { Title = "Enter Instruction Lists", Contents=input2 };
            if (formILs.ShowDialog() == DialogResult.OK)
            {
                if (_lastEmbeddedForm != null)
                    panelMain.Controls.Remove(_lastEmbeddedForm);
                _lastEmbeddedForm = new FormLadderParse(formILs.Contents);
                _lastEmbeddedForm.Show();
                _lastEmbeddedForm.EmbedToControl(panelMain);
            }
        }
    }
}