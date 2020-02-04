using System;
using System.Drawing;
using System.Windows.Forms;
using log4net.Appender;
using Dsu.PLCConvertor.Common;
using PLCConvertor.Forms;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using System.Linq;
using System.IO;
using System.Configuration;
using DevExpress.XtraSplashScreen;
using System.Threading.Tasks;
using DevExpress.XtraBars;
using System.Reflection;
using System.Text.RegularExpressions;
using Dsu.PLCConverter.UI;

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
            using (var subscription = Dsu.PLCConvertor.Common.Global.UIMessageSubject.Subscribe(m => SplashScreenManager.Default.SetWaitFormDescription(m)))
            {
                await Task.Run(() =>
                {
                    try
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
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"{ex}");
                        MsgBox.Error("에러", $"로딩에 실패하였습니다.\r\n{ex.Message}");
                    }
                });

                Enabled = true;
            }



            void AdjustUI(bool isDebuggingMode)
            {
                var ver = Assembly.GetEntryAssembly().GetName().Version;
                Text = $"{Text} v{ver}";
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
                var reviewFile = getPath($"{stem}.review.cxt");
                Logger?.Info($"Parsing {cxtPath}");

                var cvtParams = new ConvertParams(PLCVendor.Omron, PLCVendor.LSIS)
                {
                    SplitBySection = Cx2Xg5kOption.SplitBySection,
                };

                ConvertParams.Reset();

                await Task.Run(() =>
                {
                    try
                    {
                        using (var waitor = new SplashScreenWaitor($"{stem}.cxt 변환중", $"{stem}.cxt 을 변환 중입니다."))
                        using (var subscription = Dsu.PLCConvertor.Common.Global.UIMessageSubject.Subscribe(m => SplashScreenManager.Default.SetWaitFormDescription(m)))
                        {
                            var cxtInfoRoot = Cx2Xg5k.Convert(cvtParams, cxtPath, qtxFile, "", reviewFile, msgFile);

                            var totalRungs =
                                from prog in cxtInfoRoot.Programs
                                from sec in prog.Sections
                                from rung in sec.Rungs
                                select rung
                                ;

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

        private void btnRemoveLineNumber_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "QTX file(*.qtx)|*.qtx|All files(*.*)|*.*";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                var qtxFile = ofd.FileName;
                var removed =
                    File.ReadAllLines(qtxFile)
                        .Select(l =>
                        {
                            var match = Regex.Match(l, @"\d+\t(.*)");
                            var g = match.Groups.Cast<Group>().Select(gr => gr.ToString()).ToArray();
                            if (g.Length == 2)
                                return g[1];
                            else
                                return l;
                        });

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "QTX file(*.qtx)|*.qtx|All files(*.*)|*.*";
                    sfd.RestoreDirectory = true;
                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;
                    File.WriteAllLines(sfd.FileName, removed);
                }
            }
        }
    }
}