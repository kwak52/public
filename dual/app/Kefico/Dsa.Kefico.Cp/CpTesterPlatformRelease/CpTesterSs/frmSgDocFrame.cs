using System.Windows.Forms;
using CpTesterPlatform.CpApplication;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpTester;
using CpTesterSs.UserControl;
using CpTesterPlatform.CpTesterSs;

namespace CpTesterSs
{
    public partial class frmSgDocFrame : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Child Frame for User-Interface for a Station
        /// </summary>
        /// <param name="nStnIndex"></param>
        /// <param name="MDIPararent"></param>
        public frmSgDocFrame(int nStnIndex, Form MDIPararent, CpPlcIF PlcIF)
        {
            InitializeComponent();
			
            UcMainViewSs sgStationView = new UcMainViewSs(nStnIndex, (FormAppSs)MDIPararent, PlcIF)  { Dock = DockStyle.Fill };
            panelControlSgDocFrame.Controls.Add(sgStationView);
            panelControlSgDocFrame.Dock = DockStyle.Fill;
            MdiParent = MDIPararent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public UcMainViewSs GetMyViewFrm()
		{
			return (UcMainViewSs) panelControlSgDocFrame.Controls[0];
		}
    }
}