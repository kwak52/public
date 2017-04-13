using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraBars.Docking2010;
using CpTesterPlatform.CpCommon;
using Dsu.Common.Utilities.ExtensionMethods;
using CpTesterPlatform.CpTesterSs;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.IO;
using static CpBase.CpLog4netLogging;

namespace CpTesterPlatform.CpTester
{

    public partial class FormManualDigitIO : Form
    {
        private List<CpMngDIOControl> DevMgrSet = new List<CpMngDIOControl>();
        private CpMngDIOControl SelectedMng;
        private Dictionary<string, List<CpAdtCnf>> DicPin;
        private Timer timer1 = new Timer();
        DataTable dtIn = new DataTable();
        DataTable dtOut = new DataTable();
        private List<DIO> lstOut1 = new List<DIO>();
        private List<DIO> lstOut2 = new List<DIO>();
        private List<DIO> lstOut3 = new List<DIO>();

        private List<DIO> lstIn1 = new List<DIO>();
        private List<DIO> lstIn2 = new List<DIO>();
        private List<DIO> lstIn3 = new List<DIO>();

        private Image imgOn;
        private Image imgOff;

        public FormManualDigitIO(List<IDevManager> lstDevMgr, Dictionary<string, List<CpAdtCnf>> dicPin)
        {
            InitializeComponent();
            DicPin = dicPin;
            foreach (var mng in lstDevMgr)
                DevMgrSet.Add(mng as CpMngDIOControl);

            imgOn = Image.FromFile(Directory.GetCurrentDirectory() + "\\IMG\\LedOn.ico");
            imgOff = Image.FromFile(Directory.GetCurrentDirectory() + "\\IMG\\LedOff.ico");
        }

        private void ucManuDigitIO_Load(object sender, EventArgs e)
        {
            if (DevMgrSet.Count > 0)
                SelectedMng = DevMgrSet[0];

            CreateDIO();
            SelectedMng.FuncEvtHndl.OnTcpIpReceive += FuncEvtHndl_OnTcpIpReceive;
            SetDO();
            SetDI();

            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Start();
        }

        private void SetDI()
        {
            if (lstIn1.Count == 0)
                return;

            if (lstIn2.Count == 0)
            {
                dtIn.Columns.Add("Solt1", typeof(DIO));
                for (int i = 0; i < 16; i++)
                    dtIn.Rows.Add(lstIn1[i]);

            }
            else if (lstIn3.Count == 0)
            {
                dtIn.Columns.Add("Solt1", typeof(DIO));
                dtIn.Columns.Add("Solt2", typeof(DIO));

                for (int i = 0; i < 16; i++)
                    dtIn.Rows.Add(lstIn1[i], lstIn2[i]);

            }
            else
            {
                dtIn.Columns.Add("Solt1", typeof(DIO));
                dtIn.Columns.Add("Solt2", typeof(DIO));
                dtIn.Columns.Add("Solt3", typeof(DIO));

                for (int i = 0; i < 16; i++)
                    dtIn.Rows.Add(lstIn1[i], lstIn2[i], lstIn3[i]);
            }

            gridControl_IN.DataSource = dtIn;
            gridView2.RowCellStyle += GridView1_RowCellStyle;
        }


        private void SetDO()
        {
            if (lstOut1.Count == 0)
                return;

            if (lstOut2.Count == 0)
            {
                dtOut.Columns.Add("Solt1", typeof(DIO));
                for (int i = 0; i < 16; i++)
                    dtOut.Rows.Add(lstOut1[i]);

            }
            else if (lstOut3.Count == 0)
            {
                dtOut.Columns.Add("Solt1", typeof(DIO));
                dtOut.Columns.Add("Solt2", typeof(DIO));

                for (int i = 0; i < 16; i++)
                    dtOut.Rows.Add(lstOut1[i], lstOut2[i]);

            }
            else
            {
                dtOut.Columns.Add("Solt1", typeof(DIO));
                dtOut.Columns.Add("Solt2", typeof(DIO));
                dtOut.Columns.Add("Solt3", typeof(DIO));

                for (int i = 0; i < 16; i++)
                    dtOut.Rows.Add(lstOut1[i], lstOut2[i], lstOut3[i]);
            }

            gridControl_OUT.DataSource = dtOut;
            gridView1.RowCellStyle += GridView1_RowCellStyle;
        }

        private void GridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (((DIO)e.CellValue).On)
            {
                e.Appearance.BackColor = Color.LightGreen;
                e.Appearance.ForeColor = Color.Black;
            }
            else
            {
                e.Appearance.BackColor = Color.LightGray;
                e.Appearance.ForeColor = Color.DimGray;
            }
        }




        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            e.DisplayText = ((DIO)e.Value).Name;
        }

        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (((DIO)e.CellValue).On)
                e.Graphics.DrawImage(imgOn, e.Bounds.X, e.Bounds.Y);
            else
                e.Graphics.DrawImage(imgOff, e.Bounds.X, e.Bounds.Y);

            e.Handled = false;
        }


        private void gridView2_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            e.DisplayText = ((DIO)e.Value).Name;
        }

        private void FuncEvtHndl_OnTcpIpReceive(string strData)
        {
            if (strData.Split(';').Length != 4)
                return;

            int bIndex = Convert.ToInt32(strData.Split(';')[1]);
            bool bOn = Convert.ToBoolean(strData.Split(';')[2]);

            if (strData.Split(';')[0] == "DO")
            {
                this.DoAsync(() =>
                {
                    if (bIndex < 16) lstOut1[bIndex].On = bOn;
                    else if (bIndex < 32) lstOut2[bIndex - 16].On = bOn;
                    else if (bIndex < 48) lstOut3[bIndex - 32].On = bOn;

                    gridControl_OUT.RefreshDataSource();
                });
            }
            else if (strData.Split(';')[0] == "DI")
            {
                this.DoAsync(() =>
                {
                    if (bIndex < 16) lstIn1[bIndex].On = bOn;
                    else if (bIndex < 32) lstIn2[bIndex - 16].On = bOn;
                    else if (bIndex < 48) lstIn3[bIndex - 32].On = bOn;

                    gridControl_IN.RefreshDataSource();
                });
            }
        }

        private void CreateDIO()
        {
            if (SelectedMng == null || !SelectedMng.ActiveHw)
                return;

            List<bool> lstDI = SelectedMng.GetDInState();
            List<bool> lstDO = SelectedMng.GetDOutState();

            List<CpAdtCnf> lstAdt = DicPin.Values.Last().Where(w => w.CtrBlockProperty == ControBlockProperty.MUX).ToList();

            for (int i = 0; i < lstDO.Count; i++)
            {
                if (lstAdt.Count == i)
                    break;

                int IndexDO = 0;
                DIO dio;
                CpAdtCnf adtCnf = lstAdt.Where(ab => ab.AdtBpAddress.RawString.Replace("DO", "") == i.ToString("00")).FirstOrDefault();
                if (adtCnf == null)
                    IndexDO = i;
                else
                    IndexDO = Convert.ToInt32(adtCnf.AdtBpAddress.RawString.Replace("DO", ""));
                dio = new DIO(IndexDO, lstDO[IndexDO], string.Format("DO{0:00}: {1}", IndexDO, adtCnf == null ? "" : adtCnf.AdtPinName.RawString));

                if (i < 16) lstOut1.Add(dio);
                else if (i < 32) lstOut2.Add(dio);
                else if (i < 48) lstOut3.Add(dio);
            }


            for (int i = 0; i < lstDI.Count; i++)
            {
                if (lstAdt.Count == i)
                    break;

                int IndexDI = 0;
                DIO dio;

                CpAdtCnf adtCnf = lstAdt.Where(ab => ab.AdtBpAddress.RawString.Replace("DI", "") == i.ToString("00")).FirstOrDefault();
                if (adtCnf == null)
                    IndexDI = i;
                else
                    IndexDI = Convert.ToInt32(adtCnf.AdtBpAddress.RawString.Replace("DI", ""));

                dio = new DIO(IndexDI, lstDI[IndexDI], string.Format("DI{0:00}: {1}", IndexDI, adtCnf == null ? "" : adtCnf.AdtPinName.RawString));
                if (i < 16) lstIn1.Add(dio);
                else if (i < 32) lstIn2.Add(dio);
                else if (i < 48) lstIn3.Add(dio);

            }
        }

        private void ClearButtons()
        {

        }

        private void action1_Update(object sender, System.EventArgs e)
        {
            if (SelectedMng == null)
                return;

        }

        private void simpleButton_Open_Click(object sender, EventArgs e)
        {
            if (SelectedMng != null) SelectedMng.FuncEvtHndl.OnTcpIpReceive -= FuncEvtHndl_OnTcpIpReceive;
            SelectedMng.OpenDevice();
            SelectedMng.FuncEvtHndl.OnTcpIpReceive += FuncEvtHndl_OnTcpIpReceive;
        }

        private void simpleButton_Close_Click(object sender, EventArgs e)
        {
            SelectedMng.CloseDevice();
        }

        private void windowsUIButtonPanel_OUT_ButtonChecked(object sender, ButtonEventArgs e)
        {
            SelectedMng.SetDOutState(Convert.ToInt32(e.Button.Properties.Tag), e.Button.Properties.Checked);
        }

        private void frmManuDigitIO_FormClosing(object sender, FormClosingEventArgs e)
        {
            SelectedMng.FuncEvtHndl.OnTcpIpReceive -= FuncEvtHndl_OnTcpIpReceive;
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void gridControl_OUT_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (SelectedMng.DioControl.Disconnected)
            {
                ShowMessageBox("UDIO disconnected!");
                return;
            }
            
            GridHitInfo hitInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
            if ((hitInfo.InRow) && (!gridView1.IsGroupRow(hitInfo.RowHandle)) && gridView1.IsCellSelect)
            {
                var dio = gridView1.GetRowCellValue(hitInfo.RowHandle, gridView1.GetSelectedCells().First().Column) as DIO;
                SelectedMng.SetDOutState(Convert.ToInt32(dio.Index), !dio.On);
            }

            gridControl_OUT.RefreshDataSource();
        }
    }
}
