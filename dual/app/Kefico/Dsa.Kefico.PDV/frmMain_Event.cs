using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using DevExpress.XtraSplashScreen;
using Dsa.Kefico.PDV.Enumeration;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Dsa.Kefico.PDV
{
    public partial class frmMain
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SplashScreenManager.CloseForm();
            if (File.Exists(Application.StartupPath + "\\DockingLayout.xml"))
                dockManager.RestoreFromXml(Application.StartupPath + "\\DockingLayout.xml");
        }

        private void OnExitButtonClick(object sender, EventArgs e)
        {
            if (PreClosingConfirmation() == DialogResult.Yes)
            {
                Dispose(true);
                Application.Exit();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PreClosingConfirmation() == DialogResult.Yes)
            {
                dockManager.SaveToXml(Application.StartupPath + "\\DockingLayout.xml");
                Dispose(true);
                Application.Exit();
            }
            else
                e.Cancel = true;
        }

        private void frmGrid_UEventRowCountChanged(object sender, int RowCount)
        {
            DisplayRecordCount(RowCount);
        }

        private void RepositoryItemEdit_SummaryChanged(object sender, EventArgs e)
        {
            filterMWS.SetFilter(TimeSummaryStart, TimeSummaryEnd, LineGroup);
        }

        private void frmGrid_UEventMouseClick(object sender, DataRow dataRow, string viewTables)
        {
            popupMenu_Pdv.ShowPopup(MousePosition);
        }

        private void frmGrid_UEventMouseDoubleClick(object sender, DataRow dataRow, string viewTables)
        {
            if (viewTables == ViewPDV.selectPdvGroup)
            {
                CreateEntryForm(false, dataRow);
            }
            else if (viewTables == ViewPDV.selectPdvTestList)
            {
                CreateTestListForm(false, dataRow);
            }
            else if (viewTables == ViewPDV.selectPdvView)
            {
                CreateReleaseForm(false, dataRow);
            }
        }

        private void barButtonItem_SelectEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(ViewPDV.selectPdvGroup);
        }

        private void barButtonItem_SelectTestList_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(ViewPDV.selectPdvTestList);
        }

        private void barButtonItem_SelectRelease_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(ViewPDV.selectPdvView);
        }

        private void FrmChart_UEventBoundDataChanged(object sender, int RecordCount)
        {
            DisplayRecordCount(RecordCount);
        }

        private DialogResult PreClosingConfirmation()
        {
            DialogResult dialogResult;
            dialogResult = XtraMessageBox.Show(this, "Exit Application?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return dialogResult;
        }
    }
}
