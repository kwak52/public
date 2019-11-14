﻿using System;
using System.Drawing;
using System.Windows.Forms;
using log4net.Appender;
using Dsu.PLCConvertor.Common;
using Dsu.Common.Utilities.Graph;
using PLCConvertor.Forms;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Forms;
using Dsu.PLCConvertor.Common.Internal;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Configuration;

namespace PLCConvertor
{
    public partial class FormPLCConverter
        : DevExpress.XtraBars.Ribbon.RibbonForm
        , IAppender
    {
        static AddressConvertor _addressConvertor;
        static AddressConvertor AddressConvertor
        {
            get { return _addressConvertor; }
            set {
                _addressConvertor = value;
                ILSentence.AddressConvertorInstance = value;
            }
        }

        static UserDefinedCommandMapper _userDefinedCommandMapper;
        static UserDefinedCommandMapper UserDefinedCommandMapper
        {
            get { return _userDefinedCommandMapper; }
            set
            {
                _userDefinedCommandMapper = value;
                IL.UserDefinedCommandMapper = value;
            }
        }

        public FormPLCConverter()
        {
            InitializeComponent();
        }

        private void FormPLCConverter_Load(object sender, EventArgs e)
        {
            Logger.Info("FormRibonApp launched.");
            Rung.Logger = Logger;
            Cx2Xg5kOption.LogLevel = LogLevel.WARN;


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


            var addressMappingJsonFile = ConfigurationManager.AppSettings["addressMappingRuleFile"];
            AddressConvertor = AddressConvertor.LoadFromJsonFile(addressMappingJsonFile);
            var commandMappingJsonFile = ConfigurationManager.AppSettings["userDefinedCommandMappingFile"];
            UserDefinedCommandMapper = UserDefinedCommandMapper.LoadFromJsonFile(commandMappingJsonFile);

            repositoryItemComboBoxSource.Items.AddRange(Enum.GetValues(typeof(PLCVendor)));
            barEditItemSource.EditValue = PLCVendor.Omron;

            repositoryItemComboBoxTarget.Items.AddRange(Enum.GetValues(typeof(PLCVendor)));
            barEditItemTarget.EditValue = PLCVendor.LSIS;
        }
        void TestConversion()
        {
            var cvtParam = new ConvertParams(PLCVendor.Omron, PLCVendor.LSIS);
            var inputs = MnemonicInput.Inputs[0].Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var rung = Rung.CreateRung(inputs, "TestRung", cvtParam);
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
                // dockPanelMain 이 embeding control 이 잘 안되서 panelMain 을 dockPanelMain 에 삽입
                if (_lastEmbeddedForm != null)
                    panelMain.Controls.Remove(_lastEmbeddedForm);

                var source = (PLCVendor)barEditItemSource.EditValue;
                var target = (PLCVendor)barEditItemTarget.EditValue;

                _lastEmbeddedForm = new FormLadderParse(formILs.SelectedMnemonicInput, source, target);
                _lastEmbeddedForm.Show();
                _lastEmbeddedForm.EmbedToControl(panelMain);

                dockPanelMain.Text = formILs.SelectedMnemonicInput.Comment;
            }
        }

        private void BarButtonItemCxtParse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                var cxtPath = @"..\Documents\TestRung.cxt";
                ofd.Filter = "CXT file(*.cxt)|*.cxt|All files(*.*)|*.*";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                    cxtPath = ofd.FileName;

                var stem = Path.GetFileNameWithoutExtension(cxtPath);
                string getPath(string f) => Path.Combine(Path.GetDirectoryName(cxtPath), f);
                var qtxFile = getPath($"{stem}.qtx");
                var msgFile = getPath($"{stem}.txt");
                Logger?.Info($"Parsing {cxtPath}");

                var cvtParams = new ConvertParams(PLCVendor.Omron, PLCVendor.LSIS)
                {
                    SplitBySection = barCheckItemSplitBySection.Checked,
                };
                Cx2Xg5k.Convert(cvtParams, cxtPath, qtxFile, "", msgFile);
            }
        }

        private void barButtonItemAddressMapping_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FormTestAddressMapping().Show();
        }

        private void barButtonItemEditAddressMappingRule_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FormEditAddressMappingRule().Show();
        }

        private void barButtonItemEditPerferences_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FormEditPreferences().Show();
        }
    }
}