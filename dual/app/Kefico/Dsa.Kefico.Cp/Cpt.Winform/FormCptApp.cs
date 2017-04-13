using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dsu.Common.Utilities;
using Dsu.Common.Utilities.ExtensionMethods;
using static Dsu.Common.Utilities.FS.ActorMessagesBase;
using log4net;
using static ActorMessages;
using static CSharpInterop;
using static CptManagerModule;

using static CptModule;
using PsCommon;

namespace Cpt.Winform
{
    /// <summary>
    /// Sample CP Tester application
    /// </summary>
    public partial class FormCptApp : Form
    {
        private CptManager _cptManager;
        private List<Step.Step> _testLists;
        private CptHostConfig _configHost;
        private ProductConfig _configProduct;
        private uint _pdvId;

        private TestConfig _configTest;
        private ILog _logger;

        private string GaudiFile => textBoxGaudiFile.Text;

        private void ShowErrorWhileLogging(string msg, string formTitle="")
        {
            LogError(msg);
            MessageBox.Show(msg, formTitle);
        }
        private void ShowInfoWhileLogging(string msg, string formTitle = "")
        {
            LogInfo(msg);
            MessageBox.Show(msg, formTitle);
        }

        private void LogError(string msg) => _logger.Error("UI: " + msg);
        private void LogInfo(string msg) => _logger.Info("UI: " + msg);


        public FormCptApp(ILog logger, CptManager manager)
        {
            InitializeComponent();
            _logger = logger;

            CptModule.loadFromAppConfig();
            _cptManager = manager;
            _configHost = manager?.ConfigHost ?? new CptHostConfig(CptModule.cptHostId, CptModule.cptSection, CptModule.cptFixture, CptModule.cptBatch, CptModule.cptTestListPathPrefix);
            _configProduct = manager?.ConfigProduct ?? new ProductConfig(CptModule.cptEcuId, CptModule.cptEeprom);
            _configTest = manager?.ConfigTest ?? new TestConfig(CptModule.cptIsProduction, CptModule.cptGate, CptModule.cptPartNumber, CptModule.cptFileVersion);

            // 추후 수정 예제
            _configProduct.EcuId = "ECUIDZ0000";
            _configProduct.Eprom = "FFFFFFFFFFXXXXXXXXXX";

            var details = _cptManager.GetCpTestInformationDetails();
            _pdvId = details.PdvId;
        }

        private void OnFatalException(Exception ex)
        {
            if (ex is TypeInitializationException && ex.Message.Contains("Actor"))
            {
                var msg = $"FAILED to initialize actor system.\nCheck whether MWS server is running.\n\n\n-----------------\nException {ex.ToString()}";
                ShowErrorWhileLogging(msg, "FormCptActor.OnFatalException");
            }
            else
            {
                LogError($"Exception occurred!\n{ex}");
                //MessageBox.Show($"Exception occurred!\n{ex.Message}", "FormCptActor.OnFatalException");
            }

            //this.Do(() =>
            //{
            //    Close();
            //});
            //Application.Exit();
        }

        private void FormCptApp_Load(object sender, EventArgs args)
        {
            textBoxPdvId.Text = _pdvId.ToString();

            //new FormCptSetup(_configHost).ShowDialog();

            CptManagerModule.CptManager.NotifyPowerOn(_configHost);

            cbHotTest.Checked = true;
            var cwd = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            using (new CwdChanger(cwd))
            {
                var details = _cptManager.GetCpTestInformationDetails();
                //var path = 
                //    FileSpec.Relative2Absolute(
                //        $@"..\pruef_ccs\pruef_s_mmxx\p{_configProduct.ProductNumber}.v{_configProduct.VariantString}{_configTest
                //            .TestModePrefix}", cwd);


                // e.g "W:\solutions\trunk\app\Kefico\Dsa.Kefico.Cp\pruef_ccs\pruef_s_mmxx\p9001270003.CpXv01e"
                textBoxGaudiFile.Text = details.TestListFilePath;
                tbProductNumber.Text = details.ProductNumber;
            }

            try
            {
                LogInfo("CP tester system launching....");
                if (_cptManager == null )
                    _cptManager = CptManagerModule.CptManager.Create(_configHost, _configProduct, _configTest);

                LogInfo($"Ping MWS server....");
                var response = _cptManager.Ask(new AmPing(), TimeSpan.FromSeconds(5.0));
                LogInfo($"Got response: {response}");

                CptActor.CptActorSubject
                    .Subscribe(m =>
                    {
                        var fatalError = m as AmError;
                        var stepReply = m as AmReplySteps;
                        if (fatalError != null)
                            OnFatalException(new Exception(fatalError.Message));
                        else if (stepReply != null)
                        {
                            LogInfo($"Client got AmReplySteps with {stepReply.Steps.Length} steps.");
                            foreach (var s in stepReply.Steps)
                            {
                                _logger.Debug($"id={s.id}, pdvId={s.pdvId}, step={s.stepNumber}, position={s.positionNumber}, revision={s.revisionNumber}, "
                                                + $"min={s.GetMin()}, max={s.GetMax()}"
                                );
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                OnFatalException(ex);
            }

            gridView1.CustomDrawCell += GridCustomDraw.CustomDrawMinMaxValue;
        }

        private void Terminate(bool closeForm = true)
        {
            if (_cptManager != null)
            {
                try
                {
                    _cptManager.NotifyPowerOff();
                    _cptManager.Dispose();
                }
                catch (Exception ex)
                {
                    PrintRed(ex.Message);
                }
                _cptManager = null;
            }

            if (closeForm)
                Close();
        }


        private string GetGate()
        {
            if (cbHotTest.Checked) return "H";
            if (cbRoomTest.Checked) return "R";
            if (cbTotalTest.Checked) return "T";
            throw new Exception("No gate information");
        }

        private Step.Step[] SendStepRequest()
        {
            LogInfo("Sending test list request......");
            //return null;
            var relativePath = FileSpec.Absolute2Relative(GaudiFile,
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            try
            {
                var steps = _cptManager.LoadTestData(_pdvId);
                _testLists = steps.ToList();
                return steps;
            }
            catch (Exception ex)
            {
                LogError($"Exception : {ex}");
                //System.Windows.Forms.MessageBox.Show($"Exception : {ex.Message}", "FormCptActor.SendStepRequest");
                return null;
            }
        }

        private async void send_Click(object sender, EventArgs e)
        {
            send.Enabled = false;
            //dataGridView1.DataSource = UploadStepFromGaudiFile(_conn, GaudiFile);
            await Task.Run(() =>
            {
                try
                {
                    var steps = SendStepRequest();
                    if (steps == null)
                        ShowErrorWhileLogging($"Failed to get test steps from server.");
                    else
                    {
                        this.Do(() =>
                        {
                            gridControl1.DataSource = Step.toDataTable(steps, false, false);
                            LogInfo($"Got {steps.Length} steps");
                            gridView1.LayoutChanged();
                        });
                    }
                }
                finally
                {
                    this.Do(() => send.Enabled = true);
                }
            });
        }

        //private void btnSearch_Click(object sender, EventArgs e)
        //{
        //    using (var openFileDialog = new OpenFileDialogEx()
        //    {
        //        Filter = "Gaudi files|*.v*e;*.v*f;*.v*s|All files(*.*)|*.*",
        //        AddExtension = true,
        //        InitialDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
        //    })
        //    {
        //        if (DialogResult.OK == openFileDialog.ShowDialog(this))
        //        {
        //            textBoxGaudiFile.Text = openFileDialog.FileName;
        //        }
        //    }
        //}

        private async void sendStepRequestRepeatedlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    SendStepRequest();
                    Trace.WriteLine($"Finished {i}-th request.");
                }
            });
        }
        private bool Confirm(string msg) => DialogResult.Yes == MessageBox.Show(msg, "Confirm.", MessageBoxButtons.YesNo);

        private void poisonPillToServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Confirm("This test will stop the actor system, and it will cause system crash.\nDo you want to continue"))
                Terminate();
        }


        private void cPTSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormCptSetup(_configHost).ShowDialog();
        }

        private void powerONToolStripMenuItem_Click(object sender, EventArgs e)
            => CptManagerModule.CptManager.NotifyPowerOn(_configHost);

        private void powerOFFToolStripMenuItem_Click(object sender, EventArgs e) => _cptManager.NotifyPowerOff();

        private async void pingMwsServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    var reply = _cptManager.Ask(new AmPing() {Message = "Hello"}, TimeSpan.FromSeconds(2));
                    ShowInfoWhileLogging(reply.ToString());
                }
                catch (Exception ex)
                {
                    OnFatalException(ex);
                }
            });
        }

        private async void crashServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Confirm("This test will crash the server actor.\nDo you want to continue"))
                return;

            await Task.Run(() =>
            {
                try
                {
                    var reply = _cptManager.Ask(new AmRequestCrash("I will kill you!"), TimeSpan.FromSeconds(2));
                    ShowInfoWhileLogging(reply.ToString());
                }
                catch (Exception ex)
                {
                    OnFatalException(ex);
                }
            });
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Terminate();
        private void uploadCpXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CpXml files(*.CpXv*)|*.CpXv*|All files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                _cptManager.ApiUploadCpXml(_pdvId, openFileDialog1.FileName);
        }

        private void generateTestResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormCptTestResult(_cptManager, _testLists).Show();
        }

        private void simulateTestResultEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormCptSimulateTestListEdit(_cptManager, _testLists).Show();
        }

        private void pingSQLServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var time = _cptManager.GetSqlServerTime();
                ShowInfoWhileLogging($"Sucessfully got server time : {time}");

            }
            catch (Exception ex)
            {
                ShowErrorWhileLogging($"Failed to connect to server: {ex.Message}");
            }
        }
    }
}
