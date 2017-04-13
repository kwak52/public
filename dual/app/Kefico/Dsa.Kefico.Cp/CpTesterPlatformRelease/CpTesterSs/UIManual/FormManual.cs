using System.Collections.Generic;
using System.Linq;
using CpTesterPlatform.CpSystem;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpApplication;
using CpTesterPlatform.CpApplication.Manager;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManual : DevExpress.XtraEditors.XtraForm
    {
        private CpSystemManager MngSystem;
        private Dictionary<string, List<CpAdtCnf>> dicPin = new Dictionary<string, List<CpAdtCnf>>();

        public FormManual(CpSystemManager mngSystem, CpApplicationManager mngApplication)
        {
            InitializeComponent();
            MngSystem = mngSystem;
            foreach (CpStnManager stn in mngApplication.Station)
            {
                if (dicPin.ContainsKey(stn.Name))
                    continue;
                dicPin.Add(stn.Name, new List<CpAdtCnf>());
                foreach (CpAdtCnf AdapterCnf in stn.MngTStep.MngControlBlock.LstLoadedAdapterCnf)
                {
                    dicPin.Last().Value.Add(AdapterCnf);
                }
            }

            this.tabbedView1.QueryControl += tabbedView1_QueryControl;
            // Handling the QueryControl event that will populate all automatically generated Documents
        }

        // Assigning a required content for each auto generated Document
        private void tabbedView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            //if (e.Document == ucManuDAQDocument)
            //    e.Control = new frmManuDAQ(CpUtil.GetManagerDevices(MngSystem, CpDeviceType.ANALOG_INPUT));
            //if (e.Document == ucManuMotionDocument)
            //    e.Control = new frmManuMotion(CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(MngSystem, CpDeviceType.MOTION), false));
            //if (e.Document == ucManuDigitIODocument)
            //    e.Control = new frmManuDigitIO(CpUtil.GetManagerDevices(MngSystem, CpDeviceType.DIGITAL_IO), dicPin);
            //if (e.Document == ucManuDCPowerDocument)
            //    e.Control = new frmManuDCPower(CpUtil.GetManagerDevices(MngSystem, CpDeviceType.POWER_SUPPLY));
            //if (e.Document == ucManuLCRDocument)
            //    e.Control = new frmManuLCR(CpUtil.GetManagerDevices(MngSystem, CpDeviceType.LCRMETER));
            //if (e.Document == ucManuLVDTDocument)
            //    e.Control = new frmManuLVDT(CpUtil.GetManagerDevices(MngSystem, CpDeviceType.LVDT));
            //if (e.Document == ucManuPLCDocument)
            //    e.Control = new frmManuPLC(CpUtil.GetManagerDevices(MngSystem, CpDeviceType.PLC));
            ////if (e.Document == ucManuRobotDocument)
            ////    e.Control = new frmManuRobot(CpUtil.GetRobotDevice(CpUtil.GetManagerDevices(MngSystem, CpDeviceType.MOTION), true));
            //if (e.Control == null)
            //    e.Control = new System.Windows.Forms.Control();
        }
    }
}