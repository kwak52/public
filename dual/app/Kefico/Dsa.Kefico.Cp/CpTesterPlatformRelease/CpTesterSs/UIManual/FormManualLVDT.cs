using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualLVDT : Form
    {
        private CpMngLVDT mngFD1; 
        private CpMngLVDT mngFD2; 
        private CpMngLVDT mngFD3;
        private Timer timer1 = new Timer();

        public FormManualLVDT(List<IDevManager> lstDevMgr)
        {
            InitializeComponent();

            InitializeUI(lstDevMgr);
        }

        private void InitializeUI(List<IDevManager> lstDevMgr)
        {
            layoutControlItem_GetFD1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem_GetFD2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem_GetFD3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItemFD1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItemFD2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItemFD3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            if (lstDevMgr.Count > 0)
            {
                mngFD1 = lstDevMgr[0] as CpMngLVDT;
                simpleButton_GetFD1.Text = string.Format("Get Data FD {0}", mngFD1.infoMotion.COMMENT);
                layoutControlItemFD1.Text = string.Format("Measuring Data FD {0}", mngFD1.infoMotion.COMMENT);
                layoutControlItem_GetFD1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemFD1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                if (lstDevMgr.Count > 1)
                {
                    mngFD2 = lstDevMgr[1] as CpMngLVDT;
                    simpleButton_GetFD2.Text = string.Format("Get Data FD {0}", mngFD2.infoMotion.COMMENT);
                    layoutControlItemFD2.Text = string.Format("Measuring Data FD {0}", mngFD2.infoMotion.COMMENT);
                    layoutControlItem_GetFD2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemFD2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;


                    if (lstDevMgr.Count > 2)
                    {
                        mngFD3 = lstDevMgr[2] as CpMngLVDT;
                        simpleButton_GetFD3.Text = string.Format("Get Data FD {0}", mngFD3.infoMotion.COMMENT);
                        layoutControlItemFD3.Text = string.Format("Measuring Data FD {0}", mngFD3.infoMotion.COMMENT);
                        layoutControlItem_GetFD3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlItemFD3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    }
                }
            }
        }

        private void ucManuLVDT_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
           // timer1.Start();
        }

        private void action1_Update(object sender, System.EventArgs e)
        {
            //simpleButton_GetFDIN.Enabled = mngIN.IsOpened;
        }

       
        private void simpleButton_GetFD_Click(object sender, EventArgs e)
        {
            if (mngFD1 == null || !mngFD1.ActiveHw)
                return;

            filterableTextBox_FDIN.Text = mngFD1.GetFuntionDimension().ToString();
        }

        private void simpleButton_GetFDMD_Click(object sender, EventArgs e)
        {
            if (mngFD2 == null || !mngFD2.ActiveHw)
                return;

            filterableTextBox_FDMD.Text = mngFD2.GetFuntionDimension().ToString();
        }

        private void simpleButton_GetFDOUT_Click(object sender, EventArgs e)
        {
            if (mngFD3 == null || !mngFD3.ActiveHw)
                return;

            filterableTextBox_FDOUT.Text = mngFD3.GetFuntionDimension().ToString();
        }

        private void frmManuLVDT_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

    }
}
