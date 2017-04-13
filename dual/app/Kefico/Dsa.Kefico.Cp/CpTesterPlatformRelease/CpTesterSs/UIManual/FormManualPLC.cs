using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualPLC : Form
    {
        private List<CpMngPlc> DevMgrSet = new List<CpMngPlc>();
        private CpMngPlc SelectedMng;
        private Timer timer1 = new Timer();

        public FormManualPLC(List<IDevManager> lstDevMgr)
        {
            InitializeComponent();
            foreach (var mng in lstDevMgr)
                DevMgrSet.Add(mng as CpMngPlc);
        }

        private void ucManuPLC_Load(object sender, EventArgs e)
        {
            if (DevMgrSet.Count > 0)
                SelectedMng = DevMgrSet[0];

            ////this.action1.Update += action1_Update;    
            //foreach (CpMngPlc mng in DevMgrSet)
            //    comboBoxEdit_Device.Properties.Items.Add(mng.DeviceInfo.Device_ID);

            //propertyGridControl.Rows.Add(new EditorRow("Device_ID"));
            //propertyGridControl.GetLast().Properties.Caption = "Device ID";

            //propertyGridControl.Rows.Add(new EditorRow("HwName"));
            //propertyGridControl.GetLast().Properties.Caption = "H/W Info";

            //propertyGridControl.Rows.Add(new EditorRow("COMMENT"));
            //propertyGridControl.GetLast().Properties.Caption = "Comment";

            //propertyGridControl.Height = 16;

            //comboBoxEdit_Device.SelectedIndex = 0;

            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Start();
        }

        private void comboBoxEdit_Device_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedMng != null) SelectedMng.FuncEvtHndl.OnPLCReceive -= FuncEvtHndl_OnPLCReceive;
            SelectedMng = DevMgrSet.Where(w => w.DeviceInfo.Device_ID == comboBoxEdit_Device.SelectedItem.ToString()).FirstOrDefault();
            if (SelectedMng == null || !SelectedMng.ActiveHw)
                return;

            propertyGridControl.SelectedObject = SelectedMng.DeviceInfo;

            GetDataGrid.DataSource = SelectedMng.DicMonitor;
            SelectedMng.FuncEvtHndl.OnPLCReceive += FuncEvtHndl_OnPLCReceive;
        }

        private void FuncEvtHndl_OnPLCReceive(string data)
        {
            this.DoAsync(() =>
            {
                GetDataGrid.DataSource = null;
                GetDataGrid.DataSource = SelectedMng.DicMonitor;
            });
        }

        private void action1_Update(object sender, System.EventArgs e)
        {
            if (SelectedMng == null)
                return;
        }

        private void ucManuPLC_FormClosing(object sender, FormClosingEventArgs e)
        {

            timer1.Stop();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
    }
}
