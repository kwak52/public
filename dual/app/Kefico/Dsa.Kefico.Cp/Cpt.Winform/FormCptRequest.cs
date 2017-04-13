using System;
using System.Windows.Forms;
using log4net;
using static CptModule;
using System.Configuration;
using System.Collections.Generic;
using PsCommon;
using Dsu.Common.Utilities.Core.FSharpInterOp;
using System.Linq;
using System.Text;

namespace Cpt.Winform
{
    public partial class FormCptRequest : Form
    {
        /// CP tester 와 서버 연동을 위한 manager
        private CptManagerModule.CptManager _cptManager;
        /// 시험기 고유 정보
        private CptHostConfig _configHost;
        /// 제품 정보
        private ProductConfig _configProduct;
        /// 시험 정보
        private TestConfig _configTest;
        private ILog _logger;


        public FormCptRequest(ILog logger)
        {
            MwsConfig.mwsServer = ConfigurationManager.AppSettings["mwsServer"];
            _logger = logger;
            InitializeComponent();

            // 고유 설정 정보 로딩 : {시험기 정보, 제품 정보, 테스트 정보}
            CptModule.loadFromAppConfig();
            _configHost = new CptHostConfig(CptModule.cptHostId, CptModule.cptSection, CptModule.cptFixture, CptModule.cptBatch, CptModule.cptTestListPathPrefix);
            _configProduct = new ProductConfig(CptModule.cptEcuId, CptModule.cptEeprom);
            _configTest = new TestConfig(CptModule.cptIsProduction, CptModule.cptGate, CptModule.cptPartNumber, CptModule.cptFileVersion);

            _cptManager = CptManagerModule.CptManager.Create(_configHost, _configProduct, _configTest);

            FormClosed += (o, args) => { Terminate(closeForm: false); };
        }
        private void FormCptRequest_Load(object sender, EventArgs e)
        {
            int nResult = 0;
            //textEdit_cptHostId.TextChanged += (o, args) =>
            //    _configHost.Host =
            //        Int32.TryParse(textEdit_cptHostId.Text, out nResult)
            //            ? FSharpOption<int>.Some(nResult)
            //            : FSharpOption<int>.None;

            //textEdit_cptSection.TextChanged +=
            //    (o, args) => _configHost.Section = FSharpOption<string>.Some(textEdit_cptSection.Text);
            //textEdit_cptFixture.TextChanged +=
            //    (o, args) => _configHost.Fixture = FSharpOption<string>.Some(textEdit_cptFixture.Text);
            //textEdit_cptBatch.TextChanged +=
            //    (o, args) => _configHost.Batch = FSharpOption<string>.Some(textEdit_cptBatch.Text);

            //textEdit_cptProductNumber.TextChanged +=
            //    (o, args) => _configProduct.ProductNumber = textEdit_cptProductNumber.Text;


            textEdit_cptPartNumber.TextChanged +=
                (o, args) => _configTest.PartNumber = textEdit_cptPartNumber.Text;
            textEdit_cptGate.TextChanged +=
                (o, args) => _configTest.Gate = textEdit_cptGate.Text;
            textEdit_cptFileVersion.TextChanged += (o, args) =>
                _configTest.FileVersion =
                    Int32.TryParse(textEdit_cptFileVersion.Text, out nResult) ? nResult : 1;
            cbIsProduction.CheckedChanged += (o, args) => _configTest.IsProduction = cbIsProduction.Checked;
        }

        /// 시험기 종료 절차 수행
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
                    CSharpInterop.PrintRed(ex.Message);
                }
                _cptManager = null;
            }

            if (closeForm)
                Close();
        }

        /// 시험기 고유 정보 로딩
        private void button_LoadDefault_Click(object sender, EventArgs e)
        {
            textEdit_cptHostId.EditValue = CptModule.cptHostId;
            textEdit_cptSection.EditValue = CptModule.cptSection;
            textEdit_cptFixture.EditValue = CptModule.cptFixture;
            textEdit_cptBatch.EditValue = CptModule.cptBatch;

            textEdit_cptProductNumber.EditValue = string.Empty;
            textEdit_cptServerFileVersion.EditValue = string.Empty;
            textEdit_cptEcuId.EditValue = CptModule.cptEcuId;
            textEdit_cptEeprom.EditValue = CptModule.cptEeprom;

            cbIsProduction.Checked = CptModule.cptIsProduction;
            textEdit_cptGate.EditValue = CptModule.cptGate;
            textEdit_cptPartNumber.EditValue = CptModule.cptPartNumber;
            textEdit_cptProductType.EditValue = string.Empty;
            textEdit_pdvPathTestList.EditValue = string.Empty;
            textEdit_cptPdvId.EditValue = string.Empty;
        }

        private int ToInt(object obj)
        {
            if (obj is string)
                return Int32.Parse((string)obj);
            return (int)obj;
        }
        /// 시험기 고유 정보를 서버에 전달하여 시험에 관한 상세 정보 획득
        private void button_Request_Click(object sender, EventArgs e)
        {
            _configTest.ProductType = textEdit_cptProductType.Text;

            try
            {
                var details = _cptManager.GetCpTestInformationDetails();

                textEdit_pdvPathTestList.EditValue = details.TestListFilePath;
                textEdit_cptProductNumber.Text = details.ProductNumber;
                textEdit_cptProductType.Text = details.Product + details.ProductType;
                textEdit_cptServerFileVersion.Text = details.Version.ToString();
                textEdit_cptPdvId.Text = details.PdvId.ToString();


                _configProduct.ProductNumber = details.ProductNumber;
                _configProduct.Version = details.Version.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Fatal Exception:");
            }
        }

        ///  상세 정보를 바탕으로 추후 작업을 위한 UI form 실행
        private void btnNext_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            var form = new FormCptApp(_logger, _cptManager);
            //form.Closed += (o, args) => { Close(); };
            form.ShowDialog();
        }



        /// API 기준 시험기 연동 예제
        private void btnApiTest_Click(object sender, EventArgs e)
        {
            // 서버에 test 에 대한 세부 정보를 요청함
            var details = _cptManager.ApiGetCpTestInformationDetails(textEdit_cptPartNumber.Text, textEdit_cptGate.Text, cbIsProduction.Checked);

            // 서버로부터 받은 test list file path.
            var cpxml = details.TestListFilePath;

            // 서버에 step parsing 을 요청함
            var steps = _cptManager.ApiRequestTestStep(details);

            // cp xml 을 시험기에서 자체 parsing 하고, 시험을 실시
            // .... parse cpxml file...
            // .... do test ...

            // 서버로부터 받은 measure step 의 value 와 OK/NG 판정을 채워서 uploading 용 step 을 만듦
            List<Step.UploadStep> uploadSteps =
                steps.Select(s =>
                {
                    var isOK = true;
                    if (s.min.IsSome() && s.max.IsSome())
                        return new Step.UploadStep(s, isOK, value:Decimal.Parse(s.min.Value.ToString()));
                    else
                        return new Step.UploadStep(s, isOK, "msg");
                })
                .ToList();

            // 이번 시험에 대한 summary 정보 작성
            CptModule.TestSummary testSummary = new TestSummary()
            {
                StartTime = DateTime.Now - TimeSpan.FromSeconds(1),
                Duration = 0.2M,
                Temparature = 20.0,
            };

            // 서버에 upload
            _cptManager.UploadTestResult(testSummary, uploadSteps);

        }
    }
}