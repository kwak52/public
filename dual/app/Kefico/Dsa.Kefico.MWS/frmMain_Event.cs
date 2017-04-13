using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using DevExpress.XtraSplashScreen;
using Dsa.Kefico.MWS.Enumeration;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Dsa.Kefico.MWS
{
    public partial class frmMain
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SplashScreenManager.CloseForm();
            if(File.Exists(Application.StartupPath + "\\DockingLayout.xml"))
                dockManager.RestoreFromXml(Application.StartupPath + "\\DockingLayout.xml");
        }

        private void OnLinkClicked(object sender, NavBarLinkEventArgs e)
        {
            OpenDocument((EnumTable)e.Link.Item.Tag);
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
            //             filterMWS.SetFilterSummary(TimeSummaryStart, TimeSummaryEnd, LineGroup, QuickView);
            //             SetFilterEnableControl(docMWS.GetKey(EnumTable.showTotalSummaryView), true);

            FilterOn(docMWS.GetKey(EnumTable.showTotalSummaryView));
            FilterOn(docMWS.GetKey(EnumTable.showSPSV));
            FilterOn(docMWS.GetKey(EnumTable.showPHV));
            FilterOn(docMWS.GetKey(EnumTable.showBundle));
        }

        private void FilterOn(string doc)
        {
            BaseDocument document = null;
            if (docMWS.GetDocument(doc, out document))
            {
                FrmGrid frmGrid = document.Form as FrmGrid;
                frmGrid.SetFilter((bool)barEditItem_QuickView.EditValue);
            }
        }

        private void frmGrid_UEventMouseDoubleClick(object sender, DataRow dataRow, EnumTable viewTables)
        {
            try
            {
                BaseDocument doc;
                if (viewTables == EnumTable.showTotalSummaryView)
                {
                    doc = UdateRowSelect(EnumTable.showTotalSummaryView, SchemaMWS.TSV_id, dataRow);
                    if (SelectTest)
                        OpenDocument(EnumTable.showSTSV);
                    else
                        OpenDocument(EnumTable.showSPSV);
                }
                else if (viewTables == EnumTable.showSPSV)
                {
                    doc = UdateRowSelect(EnumTable.showSPSV, SchemaMWS.SSV_step, dataRow);
                    OpenDocument(EnumTable.showPHV);
                    OpenDocument(EnumTable.showPHVChart);
                }
                else if (viewTables == EnumTable.showPHV)
                {
                    doc = UdateRowSelect(EnumTable.showPHV, SchemaMWS.PSV_measureid, dataRow);
                    OpenDocument(EnumTable.showBundle);
                }
                else if (viewTables == EnumTable.showSTSV)
                {
                    doc = UdateRowSelect(EnumTable.showSTSV, SchemaMWS.PSV_id, dataRow);
                    OpenDocument(EnumTable.showBundle);
                }
                else if (viewTables == EnumTable.showBundle)
                {
                    doc = UdateRowSelect(EnumTable.showBundle, SchemaMWS.BV_positionId, dataRow);
                    OpenDocument(EnumTable.showPHV);
                    OpenDocument(EnumTable.showPHVChart);
                }

            }
            catch (System.Exception ex) { WriteOutputException(ex, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }

        private void FrmChart_UEventBoundDataChanged(object sender, int RecordCount)
        {
            DisplayRecordCount(RecordCount);
        }
  
        private void OnAboutItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
       
        }

    
   
        private void barButtonItem_Summary_Apply_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument( EnumTable.showTotalSummaryView);
            SetFilterEnableControl(docMWS.GetKey( EnumTable.showTotalSummaryView), false);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(EnumTable.showRT);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(EnumTable.showTCT);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(EnumTable.showSPEDV);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(EnumTable.showSOV);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            // OpenDocument(EnumTable.showSPEV);
        }

        private void mainRibbon_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem_spcChart_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(EnumTable.showPHVChart);
        }

        private void barButtonItem_spcTable_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(EnumTable.showPHV);
        }

        private void barButtonItem_summary_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(EnumTable.showTotalSummaryView);
        }

        private void barButtonItem_testdata_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(EnumTable.showBundle);
        }

        private void barButtonItem_selectTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(EnumTable.showSTSV);
        }

        private void barButtonItem_measure_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(EnumTable.showSPSV);
        }

        private void frmGrid_UEventMouseClick(object sender, DataRow dataRow, EnumTable viewTables)
        {
            BaseDocument doc;
            barButtonItem_summary.Enabled = true;
            barButtonItem_measure.Enabled = false;
            barButtonItem_spcTable.Enabled = false;
            barButtonItem_spcChart.Enabled = false;
            barButtonItem_testdata.Enabled = false;
            barButtonItem_selectTest.Enabled = false;

            if (viewTables == EnumTable.showTotalSummaryView)
            {
                doc = UdateRowSelect(EnumTable.showTotalSummaryView, SchemaMWS.TSV_id, dataRow);
                barButtonItem_measure.Enabled = true;
                barButtonItem_selectTest.Enabled = true;
            }
            else if (viewTables == EnumTable.showSPSV)
            {
                doc = UdateRowSelect(EnumTable.showSPSV, SchemaMWS.SSV_step, dataRow);
                barButtonItem_spcTable.Enabled = true;
                barButtonItem_spcChart.Enabled = true;
            }
            else if (viewTables == EnumTable.showPHV)
            {
                doc = UdateRowSelect(EnumTable.showPHV, SchemaMWS.PSV_measureid, dataRow);
                barButtonItem_testdata.Enabled = true;
            }
            else if (viewTables == EnumTable.showSTSV)
            {
                doc = UdateRowSelect(EnumTable.showSTSV, SchemaMWS.PSV_id, dataRow);
                barButtonItem_testdata.Enabled = true;
            }
            else if (viewTables == EnumTable.showBundle)
            {
                doc = UdateRowSelect(EnumTable.showBundle, SchemaMWS.BV_positionId, dataRow);
                barButtonItem_spcTable.Enabled = true;
                barButtonItem_spcChart.Enabled = true;
            }

            popupMenu_MWS.ShowPopup(MousePosition); //test ahn
        }


        #region Event_Function
        private DialogResult PreClosingConfirmation()
        {
            DialogResult dialogResult;
            dialogResult = XtraMessageBox.Show(this, "Exit Application?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return dialogResult;
        }
      
        private BaseDocument UdateRowSelect(EnumTable ViewTables, string Field, DataRow dataRow)
        {
            string docKey = docMWS.GetKey(ViewTables);
            BaseDocument document = null;
            if (docMWS.GetDocument(docKey, out document))
            {
                int EditValue = Convert.ToInt32(dataRow[Field]);
                FrmGrid frmGrid = document.Form as FrmGrid;
                DataRow dr = frmGrid.GetDataRow(Field, EditValue);
                if (dr == null)
                    return null;

                frmGrid.RowHandleFilterSelect = dr;
                if (ViewTables == EnumTable.showTotalSummaryView)
                {
                    SummaryID = EditValue;
                    SummaryID_Text = dataRow["partNr"].ToString();
                    filterMWS.SetFilterProduct(dr);
                }
                else if (ViewTables == EnumTable.showSPSV)
                {
                    PositionID = EditValue;
                    PositionID_Text = string.Format("Step {0}", EditValue);
                    filterMWS.SetFilterStep(dr);
                }
                else if (ViewTables == EnumTable.showSTSV)
                {
                    MeasureID = EditValue;
                    TimeSpan timeSpan = (TimeSpan)dataRow["time"];
                    MeasureID_Text = timeSpan.ToString("hh':'mm':'ss");
                    filterMWS.SetFilterSelectTest(dr);
                }
                else if (ViewTables == EnumTable.showPHV)
                {
                    MeasureID = EditValue;
                    TimeSpan timeSpan = (TimeSpan)dataRow["time"];
                    MeasureID_Text = timeSpan.ToString("hh':'mm':'ss");
                    filterMWS.SetFilterSpcTable(dr);
                }
                else if (ViewTables == EnumTable.showBundle)
                {
                    PositionID = EditValue;
                    filterMWS.SetFilterTotalReport(dr);
                }
            }
            return document;
        }
        private void mainRibbon_SelectedPageChanged(object sender, EventArgs e)
        {
            RibbonControl rc = sender as RibbonControl;
            if (rc == null)
                return;
            
            BaseDocument document = null;

            EnumRibbon enumRibbon = RibbonMWS.GetType(rc.SelectedPage.Text);
            EnumTable enumTable = ViewMWS.GetType(tabbedView.ActiveDocument.Caption);

            if (enumRibbon == EnumRibbon.History)
            {
                if (SelectTest == true)
                    enumTable = EnumTable.showSTSV;
                else
                    enumTable = EnumTable.showSPSV;
            }else if (enumRibbon == EnumRibbon.Overview)
            {
                enumTable = EnumTable.showTotalSummaryView;
            }

            if (docMWS.GetDocument(docMWS.GetKey(enumTable), out document))
                tabbedView.Controller.Activate(document);
        }
        private void barEditItem_ChartScaleAuto_EditValueChanged(object sender, EventArgs e)
        {
            BaseDocument document = null;
            if (docMWS.GetDocument(docMWS.GetKey(EnumTable.showPHVChart), out document))
            {
                FrmChart frmChart = document.Form as FrmChart;
                frmChart.AxisYRangeAuto(ChartScaleAuto);
            }
        }

        #endregion
    }
}
