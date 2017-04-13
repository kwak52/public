using CpTesterPlatform.CpApplication;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpSystem.Manager;
using CpTesterPlatform.CpTesterSs;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using PsKGaudi.Parser;
using PsKGaudi.Parser.PsCCSSTDFn.ControlFn;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CpTesterPlatform.DxUtility;
using System.Xml.Linq;

namespace CpTesterSs.UserControl
{
    public partial class userCtrMainView : DevExpress.XtraEditors.XtraUserControl
    {
        // managers.
        public CpStnManager MngStation { get; private set; }

        public CpApplicationManager MngApplication { get; private set; }
        public CpSystemManager MngSystem { get; private set; }
        public CpThreadManager MngTStepThread { get; set; }

        // user control views.
        public userCtrTStepStatus frmSgTStepStatus { get; private set; }
        public userCtrTStepInformation frmSgTStepInformation { get; private set; }
        public userCtrTStepList frmSgTStepList { get; private set; }

        public userCtrMainView()
        {   
            InitializeComponent();

            #region Load configure

            /// <summary> 
            /// Loading station manager.
            /// </summary>           

            // station manager
            MngStation = new CpStnManager();

            // system manager
            MngSystem = new CpSystemManager();

            MngApplication = new CpApplicationManager();

            #endregion

            #region child forms (user-control).

            /// <summary> 
            /// Loading sub forms.
            /// </summary>

            // load status view
            frmSgTStepStatus = new userCtrTStepStatus(this);
            panelControlSgTestStatus.Controls.Add(frmSgTStepStatus);
            panelControlSgTestStatus.Dock = DockStyle.Fill;

            // load information view                
            frmSgTStepInformation = new userCtrTStepInformation(this);
            panelControlSgTestInformation.Controls.Add(frmSgTStepInformation);
            panelControlSgTestInformation.Dock = DockStyle.Fill;

            // load test-list
            frmSgTStepList = new userCtrTStepList(this);
            panelControlSgTest.Controls.Add(frmSgTStepList);
            panelControlSgTest.Dock = DockStyle.Fill;

            // system manager
            //MngSystem = new CpSystemManager();

            #endregion
        }

        // function
        private async Task startSgTestByMainThread()
        {
            int nStartStep = 1000; //Convert.ToInt32(this.MngSystem.CnfSystem.TcConfigue.StartStep);
            MngApplication.MngTStep.setTsRange(nStartStep);

	        await TaskSgTest.taskSgTest(this);
        }

        // delegate
        private delegate void DelegateSetMainViewEnableButtons(bool bBtnStart, bool bBtnStop);
        public void InvokeSetMainViewEnableButtons(bool bBtnStart, bool bBtnStop)
        {
            if (InvokeRequired)
            {
                DelegateSetMainViewEnableButtons delSetButtons 
                    = new DelegateSetMainViewEnableButtons(InvokeSetMainViewEnableButtons);
                this.Invoke(delSetButtons, bBtnStart, bBtnStop);
            }
            else
            {
                barButtonItemStartSgTest.Enabled = bBtnStart;
                barButtonItemStopSgTest.Enabled = bBtnStop;
            }            
        }

        private delegate void DelegateUpdateGridWithStatusBarForPrintOutFn(int nCrtStepIndex, ClsCpTsShell cpTStep, CpSystemStatus eCpStatus, string strMsgFst = "", string strMsgScd = "");
        public void InvokeUpdateGridWithStatusBarForPrintOutFn(int nCrtStepIndex, ClsCpTsShell cpTStep, CpSystemStatus eCpStatus, string strMsgFst = "", string strMsgScd = "")
        {
            if (InvokeRequired)
            {
                DelegateUpdateGridWithStatusBarForPrintOutFn del = new DelegateUpdateGridWithStatusBarForPrintOutFn(InvokeUpdateGridWithStatusBarForPrintOutFn);
                this.Invoke(del, nCrtStepIndex, cpTStep, eCpStatus, strMsgFst, strMsgScd);
            }
            else
            {
                // Select Current Step & Focus on the Current Step (Scroll).
                frmSgTStepList.GridViewTestSteps.SelectRow(nCrtStepIndex);
                frmSgTStepList.GridViewTestSteps.FocusedRowHandle = nCrtStepIndex;

                // status bar test number
                frmSgTStepStatus.labelTsHeadNumber.Text = cpTStep.Core.StepNum.ToString();

                // print out
                if ((cpTStep.Core as PsCCSStdFnPrintOut) != null)
                {
                    string strPrintOutParm = (cpTStep.Core as PsCCSStdFnPrintOut).ArstdSerialParmsP.GetValueByIndex(CPTsIndex.INDEX_PRINT_OUT_DISPLAY_TEXT);
                    frmSgTStepStatus.labelControlMnCtrTsPrintOut.Text = ClsGlobalStringForUIDisplay.CP_MODULE_PRINT + strPrintOutParm;
                }
                else
                    frmSgTStepStatus.labelControlMnCtrTsPrintOut.Text = cpTStep.Core.STDBoschName;
            }
        }

        // task
        private async Task<bool> LoadSgTestList(string strSelFileName)
        {   
            if (MngApplication.MngTStep != null)
                MngApplication.MngTStep = null;

            return await WaitLoadingTestList(MngApplication, MngTStepThread, strSelFileName);
        }
        
        public static Task<bool> WaitLoadingTestList(CpApplicationManager MngApplication, CpThreadManager MngTStepThread, string strSelFileName)
        {   
            if (MngApplication.MngTStep != null)
                MngApplication.MngTStep.GaudiReadData.Clear();

            PsCCS.PsCCSGaudiFile psGaudiReadData = PsKGaudi.PsKGaudiInf.OpenGaudiFile(strSelFileName, UtilTextMessageEdits.PrintLogLevel.ToString());

            // create a set of managers from here.
            MngApplication.MngTStep = new CpTsManager(psGaudiReadData);
            MngTStepThread = new CpThreadManager();
            return Task.FromResult(true);
        }

        // event
        private async void barButtonItemOpenFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // open file dialog.
            using (OpenFileDialog opFileDlg = new OpenFileDialog())
            {
                if (opFileDlg.ShowDialog() == DialogResult.OK)
                {
	                await Task.Run(async () =>
	                {
						// resource enable.
						InvokeSetMainViewEnableButtons(false /*start*/, false /*stop*/);

						// show current status.
						frmSgTStepStatus.InvokeChangeStatus(CpSystemStatus.LOADING);

						// load a file.
						bool result = await LoadSgTestList(opFileDlg.FileName);
						if (!result)
						{
							string strMsg = MngSystem.MngResx.GetString("StringFailedToLoadFile") + opFileDlg.FileName;
							throw new FileLoadException(strMsg);
						}

						// display test-list information.
						barStaticItemLoadedTestList.Caption = opFileDlg.FileName;

						// show test list in the grid.
						// fill up with the given data.
						userCtrTStepList.AsyncUpdateInitialGridViewAll(MngApplication.MngTStep, frmSgTStepList);

						// complete message.
						frmSgTStepStatus.InvokeChangeStatus(CpSystemStatus.READY);

						InvokeSetMainViewEnableButtons(true/*start*/, false/*stop*/);
					});

                }
            }
        }
        private async void barButtonItemStartSgTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // create a sequencer.
            await startSgTestByMainThread();
        }

        private void barButtonItemProperty_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // read xml file.
            // CpStnConfigure cnfStation = CpStnConfigure.loadStationConfig();

            // XDocument xmlDoc = new XDocument();
            string sFilePath = Directory.GetCurrentDirectory();
            if (!File.Exists(sFilePath + "\\" + ClsGlobalStringForGeneral.FILE_STATION_CONFIGUE_NAME))
            {
                MessageBox.Show("System Configuration file loading Error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //             DataSet dtSet = new DataSet();
            //             dtSet.ReadXml(sFilePath + "\\" + ClsGlobalStringForGeneral.FILE_STATION_CONFIGUE_NAME);  \

            XDocument xmlDoc = XDocument.Load(sFilePath + "\\" + ClsGlobalStringForGeneral.FILE_STATION_CONFIGUE_NAME);

            // make a data set.

            // open form for editing properties.

            UiCpConfigure frmConfig = new UiCpConfigure(xmlDoc, ClsGlobalStringForGeneral.FILE_STATION_CONFIGUE_NAME);
            frmConfig.Show();
        }
    }
}
