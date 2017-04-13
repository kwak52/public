using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace CpSample
{
    public partial class frmManual : DevExpress.XtraEditors.XtraForm
    {
        public frmManual()
        {
            InitializeComponent();
            // Handling the QueryControl event that will populate all automatically generated Documents
            this.tabbedView1.QueryControl += tabbedView1_QueryControl;
        }

        // Assigning a required content for each auto generated Document
        void tabbedView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            if (e.Document == ucManuDAQDocument)
                e.Control = new CpSample.ucManuDAQ();
            if (e.Document == ucManuDCPowerDocument)
                e.Control = new CpSample.ucManuDCPower();
            if (e.Document == ucManuLCRDocument)
                e.Control = new CpSample.ucManuLCR();
            if (e.Document == ucManuLVDTDocument)
                e.Control = new CpSample.ucManuLVDT();
            if (e.Document == ucManuTempHumiDocument)
                e.Control = new CpSample.ucManuTempHumi();
            if (e.Document == ucManuPLCDocument)
                e.Control = new CpSample.ucManuPLC();
            if (e.Document == ucManuMotionDocument)
                e.Control = new CpSample.ucManuMotion();
            if (e.Document == ucManuDigitIODocument)
                e.Control = new CpSample.ucManuDigitIO();
            if (e.Control == null)
                e.Control = new System.Windows.Forms.Control();
        }
    }
}