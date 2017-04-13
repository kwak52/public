using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterSs.UserControl;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CpTesterSs
{
	/// <summary>
	/// Display Test Status: Color, OK/NG Counts, and Etc,.
	/// </summary>
	public partial class UcTStepStatus : DevExpress.XtraEditors.XtraUserControl
	{
		private UcMainViewSs userCtrMainView;
        private CpSystemStatus m_StnStatus = CpSystemStatus.FINISH;
        public CpSystemStatus Status { get { return m_StnStatus; } }

        public UcTStepStatus()
		{
			InitializeComponent();
		}
		public UcTStepStatus(UcMainViewSs userCtrMainView)
		{
			InitializeComponent();
			this.userCtrMainView = userCtrMainView;
			this.Dock = DockStyle.Fill;
		}


		private static Dictionary<CpSystemStatus, Color> _statusColorMap = new Dictionary<CpSystemStatus, Color>()
		{
			[CpSystemStatus.LOADING] = Color.Orange,
			[CpSystemStatus.READY] = Color.LightGray,
			[CpSystemStatus.RUN] = Color.Yellow,
			[CpSystemStatus.FINISH] = Color.Red,
			[CpSystemStatus.OK] = Color.Lime,
			[CpSystemStatus.NG] = Color.Red,
			[CpSystemStatus.PAUSE] = Color.Aqua,
			[CpSystemStatus.SKIP] = Color.Purple,
			//[CpSystemStatus.PRE_TASK_RUN] = Color.Beige,
			//[CpSystemStatus.STOP] = Color.Pink, ///Stop displays a last test status.
		};					 
				
		/// <summary>
		/// Change the status of the main view using a given parameter. 
		/// </summary>
		/// <param name="eSystemStatus"> a set of system states which has own color.</param>
		/// <param name="strSgStatus"> a message in the first row.</param>
		/// <param name="strPrintOut"> a message in the second row.</param>
		public void ChangeStatus(CpSystemStatus eSystemStatus, string strSgStatus = "", string strPrintOut = "")
        {
            m_StnStatus = eSystemStatus;

            if ( _statusColorMap.ContainsKey(eSystemStatus) )
				ChangeStatusColor(_statusColorMap[eSystemStatus]);

			ChangeStatusText(eSystemStatus.ToString(), strSgStatus, strPrintOut);
		}


		/// <summary>
		/// Change the color which indicates the status of the system. 
		/// </summary>
		/// <param name="colorStatus"> a color indicates the status of the system.</param>
		public void ChangeStatusColor(Color colorStatus)
		{
			this.DoAsync(() => { layoutControlGroupMnStatus.AppearanceGroup.BackColor = colorStatus; });
		}
		/// <summary>
		/// Change the status of the main view using a given parameter. 
		/// </summary>
		/// <param name="strSystemStatusWithResult"> a message for the system status.</param>
		/// <param name="strSgStatus"> a message in the status text.</param>
		/// <param name="strPrintOut"> a message in the 'print-out' text.</param>
		public void ChangeStatusText(string strSystemStatusWithResult, string strSgStatus, string strPrintOut)
		{
			this.DoAsync(() =>
			{
				labelControlMnCtrTsResult.Text = strSystemStatusWithResult;
				labelControlMnCtrTsStatus.Text = strSgStatus;
				labelControlMnCtrTsPrintOut.Text = strPrintOut;
			});
		}

		public void UpdateProgressFinished()
		{
			this.DoAsync(() =>
			{				
				progressBarControlSgTest.Position = 100;
			});
		}

		public void UpdateProgress(int nCrtStepIndex, int nTsRangeByIndex, int nTsHeadNumber = -1)
		{
			this.DoAsync(() =>
			{
				if (nTsHeadNumber != -1)
					labelTsHeadNumber.Text = nTsHeadNumber.ToString();
				progressBarControlSgTest.Position = Convert.ToInt32(nCrtStepIndex / (double)(nTsRangeByIndex) * 100);
			});
		}

		public void UpdateSuceededResult()
		{
            this.DoAsync(() =>
            {
                textEditTotalTest.Text = userCtrMainView.MngStation.TsCount.ToString();
            });
		}

		public void UpdateFailedResult()
		{
            this.DoAsync(() =>
            {
                textEditNgTest.Text = userCtrMainView.MngStation.NgCount.ToString();
            });
        }
    }
}
