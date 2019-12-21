using System;
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
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System.Threading.Tasks;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;

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

        /// <summary>
        /// Debugging mode : Shift key press 된 상태로 시작
        /// </summary>
        static bool IsDebuggingMode { get; set; }

        public FormPLCConverter()
        {
            InitializeComponent();
        }

        private async void FormPLCConverter_Load(object sender, EventArgs args)
        {
            Enabled = false;
            IsDebuggingMode = ModifierKeys == Keys.Shift;
            AdjustUI(IsDebuggingMode);

            Logger.Info("FormRibonApp launched.");
            Rung.Logger = Logger;
            Cx2Xg5kOption.LogLevel = LogLevel.INFO;

            using (var waitor = new SplashScreenWaitor("로딩", "변환기를 로딩합니다."))
            using (var subscription = Global.UIMessageSubject.Subscribe(m => SplashScreenManager.Default.SetWaitFormDescription(m)))
            {
                await Task.Run(() =>
                {
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
                    UserDefinedCommandMapper = UserDefinedCommandMapper.LoadFromJsonFile(commandMappingJsonFile, PLCVendor.Omron);

                    repositoryItemComboBoxSource.Items.AddRange(Enum.GetValues(typeof(PLCVendor)));
                    barEditItemSource.EditValue = PLCVendor.Omron;

                    repositoryItemComboBoxTarget.Items.AddRange(Enum.GetValues(typeof(PLCVendor)));
                    barEditItemTarget.EditValue = PLCVendor.LSIS;
                });

                Enabled = true;
            }



            void AdjustUI(bool isDebuggingMode)
            {
                dockPanelLog.Options.ShowCloseButton = false;

                if (!isDebuggingMode)
                {
                    Size = new Size(500, 300);
                    dockPanelMain.Close();
                    ribbonPageDebugging.Visible = false;
                    dockPanelLog.DockedAsTabbedDocument = true;
                }
            }
        }
        void TestConversion()
        {
            var cvtParam = new ConvertParams(PLCVendor.Omron, PLCVendor.LSIS);
            var inputs = MnemonicInput.Inputs[0].Input.SplitByLines();
            var rung = Rung.CreateRung(inputs, "TestRung", cvtParam);
            var graph = rung.GraphViz();
            var _pictureBox = new PictureBox() { Image = graph, Dock = DockStyle.Fill };
            var _formGraphviz = new Form() { Size = new Size(800, 500) };
            _formGraphviz.Controls.Add(_pictureBox);
            _formGraphviz.Show();
        }


        Form _lastEmbeddedForm;
        private void BarButtonItemTestParse_ItemClick(object s, ItemClickEventArgs e)
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

        private async void BarButtonItemCxtParse_ItemClick(object s, ItemClickEventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                var cxtPath = @"..\Documents\TestRung.cxt";
                ofd.Filter = "CXT file(*.cxt)|*.cxt|All files(*.*)|*.*";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;
                
                cxtPath = ofd.FileName;

                var stem = Path.GetFileNameWithoutExtension(cxtPath);
                string getPath(string f) => Path.Combine(Path.GetDirectoryName(cxtPath), f);
                var qtxFile = getPath($"{stem}.qtx");
                var msgFile = getPath($"{stem}.txt");
                var reviewFile = getPath($"{stem}-review.cxt");
                Logger?.Info($"Parsing {cxtPath}");

                var cvtParams = new ConvertParams(PLCVendor.Omron, PLCVendor.LSIS)
                {
                    SplitBySection = Cx2Xg5kOption.SplitBySection,
                };

                ConvertParams.Reset();
                    
                if (barCheckItemWithSymbols.Checked
                    && acceptSymbolsByUserPaste() != DialogResult.OK
                    && MsgBox.Ask("Ask", "변환을 계속하시겠습니까?") != DialogResult.Yes)
                {
                    return;
                }

                await Task.Run(() =>
                {
                    try
                    {
                        using (var waitor = new SplashScreenWaitor($"{stem}.cxt 변환중", $"{stem}.cxt 을 변환 중입니다."))
                        using (var subscription = Global.UIMessageSubject.Subscribe(m => SplashScreenManager.Default.SetWaitFormDescription(m)))
                        {
                            var cxtInfoRoot = Cx2Xg5k.Convert(cvtParams, cxtPath, qtxFile, "", reviewFile, msgFile);

                            var totalRungs =
                                from prog in cxtInfoRoot.Programs
                                from sec in prog.Sections
                                from rung in sec.Rungs
                                select rung
                                ;
                            //cxtInfoRoot
                            //    .Programs
                            //    .SelectMany(prog => prog.EnumerateType<CxtInfoSection>())
                            //    .SelectMany(sec => sec.EnumerateValidRungs())
                            //    .ToArray();

                            var numTotalRungs = totalRungs.Count();
                            var numFailed = cvtParams.ReviewProjectGenerator.FailedRungs.Count();
                            var message = $"{stem}.cxt 변환 완료!\r\n\r\n"
                                    + $"    총 rung 수 = {numTotalRungs}\r\n"
                                    + $"    실패한 rung 수 = {numFailed}";
                            Logger.Info(message);
                            MsgBox.Info("변환완료", message);
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error("에러", $"변환에 실패하였습니다.\r\n{ex.Message}");
                        throw;
                    }
                });



                // 사용자로부터 추가적인 symbol table 정보를 입력받는다.
                DialogResult acceptSymbolsByUserPaste()
                {
                    var form = new FormSymbolPaste();
                    var dialogResult = form.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        var map = ConvertParams.SourceVariableMap;
                        form.SymbolTableText
                            .SplitByLines(StringSplitOptions.RemoveEmptyEntries)
                            .Select(line => generatePlcVariable(line))
                            .Iter(v => {
                                if (map.ContainsKey(v.Device))
                                {
                                    var existingV = map[v.Device];
                                    if (v.Name.NonNullAny() && existingV.Name.IsNullOrEmpty())
                                        existingV.Name = v.Name;
                                    if (v.Variable.NonNullAny() && existingV.Variable.IsNullOrEmpty())
                                        existingV.Variable = v.Variable;
                                    if (v.Comment.NonNullAny() && existingV.Comment.IsNullOrEmpty())
                                        existingV.Comment = v.Comment;
                                }
                                else
                                    map.Add(v.Device, v);
                            });
                    }
                    return dialogResult;

                    // <TAB> 에 의해 구분되는 symbol table 의 하나의 line 을 분석하여 PLCVariable 로 반환
                    // line 구조 : 이름 <TAB> 데이터type <TAB> address <TAB> 주석
                    PLCVariable generatePlcVariable(string line)
                    {
                        var t = line.Split('\t').ToArray();
                        var name = t[0];
                        var typeStr = t[1];
                        var device = t[2];  // address
                        var comment = t[3];

                        var type =
                                (PLCVariable.DeviceType)Enum.Parse(
                                    typeof(PLCVariable.DeviceType), typeStr.Replace(" ", "_"), true);

                        return new PLCVariable(name, device, type, comment, "");
                    }
                }
            }
        }

        private void barButtonItemAddressMapping_ItemClick(object s, ItemClickEventArgs e)
        {
            new FormTestAddressMapping().Show();
        }

        private void barButtonItemEditAddressMappingRule_ItemClick(object s, ItemClickEventArgs e)
        {
            new FormEditAddressMappingRule().Show();
        }

        private void barButtonItemEditPerferences_ItemClick(object s, ItemClickEventArgs e)
        {
            new FormEditPreferences().Show();
        }

        private void barButtonItemTestAddress_ItemClick(object s, ItemClickEventArgs e)
        {
            new FormTestAddressMappingRule().Show();
        }

        private void barButtonItemTranslate_ItemClick(object s, ItemClickEventArgs e)
        {
            barButtonItemCxtParse.PerformClick();
        }

        private void barButtonItemSetting_ItemClick(object sender, ItemClickEventArgs e)
        {
            barButtonItemEditPerferences.PerformClick();
        }

        private void barButtonItemExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }
    }
}