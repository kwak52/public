using DevExpress.XtraBars.Docking2010.Views;
using Dsa.Kefico.PDV.Enumeration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace Dsa.Kefico.PDV
{
    public partial class frmMain
    {
        private void OpenDocument(string viewTables, bool bNew = true)
        {
            BaseDocument document = null;
            string Key = viewTables;

            if (!docMWS.Creating)
            {
                string viewQuery = this.viewQuery.Query(filterMWS, viewTables);
                if (LogDebug)
                    WriteOutput("Query", viewQuery);

                if (viewQuery == string.Empty)
                    return;

                if (!docMWS.ExistDoc(Key))
                    docMWS.Creating = true;

                DrowGrid(Key, viewTables, viewQuery);

            }
            else if (docMWS.GetDocument(Key, out document))
            {
                tabbedView.Controller.Activate(document);
                barStaticItem_StatusText.Caption = EnumEventResult.Ready.ToString();
            }
        }

        private void UpdateDocument(string table, string column, int id)
        {
            BaseDocument document = null;
            if (docMWS.GetDocument(table, out document))
            {
                OpenDocument(table);
                FrmGrid frmGrid = document.Form as FrmGrid;
                foreach (DataRow dr in frmGrid.DataSource.Rows)
                {
                    if (Convert.ToInt32(dr[column]) == id)
                        frmGrid.RowHandleFilterSelect = dr;
                }
            }
        }

        private void DrowGrid(string docKey, string viewTables, string ViewQuery)
        {
            try
            {
                FrmGrid frmGrid = CreateForm(docKey, viewTables) as FrmGrid;
                frmGrid.ControlBox = false;
                frmGrid.FormClosing += FrmGrid_FormClosing;

                Stopwatch sw = Stopwatch.StartNew();
                DataTable dtView = mySqlMWS.GetDataFromDBView(ViewQuery, viewTables);
                dtView.TableName = viewTables;
                barStaticItem_StatusText.Caption = string.Format("Select : {0} ", sw.ElapsedMilliseconds.ToString("0s 000ms"));
                frmGrid.ViewTable = viewTables;
                frmGrid.MdiParent = this;
                frmGrid.Show();
                frmGrid.ShowGrid(dtView);
                frmGrid.UEventRowCountChanged += frmGrid_UEventRowCountChanged;
                frmGrid.UEventMouseDoubleClick += frmGrid_UEventMouseDoubleClick;
                frmGrid.UEventMouseClick += frmGrid_UEventMouseClick;
            }
            catch (System.Exception ex) { docMWS.Creating = false; WriteOutputException(ex, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }

        private void FrmGrid_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private DataTable GetDataTable(string table)
        {
            DataTable dtSpc = new DataTable();
            BaseDocument document = null;
            if (docMWS.GetDocument(table, out document))
            {
                FrmGrid frmGrid = document.Form as FrmGrid;
                if (frmGrid != null && frmGrid.DataSource != null)
                    dtSpc = frmGrid.DataSource;
            }

            return dtSpc;
        }

        private List<string> GetTestList(DataTable dt)
        {
            List<string> lstTestList = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                lstTestList.Add(string.Format("{0}:{1} {2} {3} ({4})"
                    , dr[SchemaPDV.PDVTESTLIST_id]
                    , dr[SchemaPDV.PDVTESTLIST_productNumber]
                    , dr[SchemaPDV.PDVTESTLIST_product]
                    , dr[SchemaPDV.PDVTESTLIST_productType]
                    , dr[SchemaPDV.PDVTESTLIST_variant]
                    ));
            }

            return lstTestList;
        }


        private List<string> GetGroup(DataTable dt)
        {
            List<string> lst = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(string.Format("{0}:{1} {2}"
                    , dr[SchemaPDV.PDVGROUP_id]
                    , dr[SchemaPDV.PDVGROUP_ProductGroup]
                    , dr[SchemaPDV.PDVGROUP_ProductModel]
                    ));
            }

            return lst;
        }

        private List<string> GetDataList(DataTable dt, string colName)
        {
            List<string> lstData = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                if (!lstData.Contains(dr[colName].ToString()))
                    lstData.Add(dr[colName].ToString());
            }

            return lstData;
        }

        private object GetData(DataTable dt, string colName, int id)
        {
            DataRow[] arrDr = dt.Select(string.Format("id = '{0}'", id));
            if (arrDr.Length == 0)
                return null;
            else
                return arrDr[0][colName];
        }

        private List<int> GetDataListInt(DataTable dt, string colName)
        {
            List<int> lstData = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                if (!lstData.Contains(Convert.ToInt32(dr[colName])))
                    lstData.Add(Convert.ToInt32(dr[colName]));
            }

            return lstData;
        }


        private int GetLastId(DataTable dt, string column)
        {
            int max = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (max < Convert.ToInt32(dr[column]))
                    max = Convert.ToInt32(dr[column]);
            }
            return max;
        }

        private Form CreateForm(string docKey, string viewTables)
        {
            BaseDocument document = null;
            Form form = null;

            if (docMWS.GetDocument(docKey, out document))
            {
                form = document.Form as FrmGrid;
                ((FrmGrid)form).LogDebug = LogDebug;    
                ((FrmGrid)form).ClearGrid();
                ((FrmGrid)form).UEventRowCountChanged -= frmGrid_UEventRowCountChanged;
                ((FrmGrid)form).UEventMouseDoubleClick -= frmGrid_UEventMouseDoubleClick;
                ((FrmGrid)form).UEventMouseClick -= frmGrid_UEventMouseClick;
                tabbedView.Controller.Activate(document);
            }
            else
                form = new FrmGrid(docKey, LogDebug);
            return form;
        }

        private void CreateSummaryFilter(DataTable dtView)
        {
            List<int> lstID = new List<int>();
            foreach (DataRow dr in dtView.Rows)
                lstID.Add(Convert.ToInt32(dr[SchemaPDV.TSV_id]));

            repositoryItemComboBox_TSV.Items.Clear();
            repositoryItemComboBox_Measure.Items.Clear();
            repositoryItemComboBox_Position.Items.Clear();

            barEditItem_TsvID.EditValue = null;
            barEditItem_MeasureID.EditValue = null;
            barEditItem_PositionID.EditValue = null;

            repositoryItemComboBox_TSV.Items.AddRange(lstID);
        }
        private void CreatePositionFilter(DataTable dtView)
        {
            List<int> lstID = new List<int>();
            foreach (DataRow dr in dtView.Rows)
                lstID.Add(Convert.ToInt32(dr[SchemaPDV.SSV_positionId]));
            repositoryItemComboBox_Measure.Items.Clear();
            repositoryItemComboBox_Position.Items.Clear();

            barEditItem_MeasureID.EditValue = null;
            barEditItem_PositionID.EditValue = null;
            repositoryItemComboBox_Position.Items.AddRange(lstID);
        }
        private void CreateMeasureFilter(DataTable dtView)
        {
            List<int> lstID = new List<int>();
            foreach (DataRow dr in dtView.Rows)
                lstID.Add(Convert.ToInt32(dr[SchemaPDV.PSV_measureid]));
            repositoryItemComboBox_Measure.Items.Clear();
            repositoryItemComboBox_Measure.Items.AddRange(lstID);
        }

        private void CreateSelectTestFilter(DataTable dtView)
        {
            List<int> lstID = new List<int>();
            foreach (DataRow dr in dtView.Rows)
                lstID.Add(Convert.ToInt32(dr[SchemaPDV.PSV_id]));
            repositoryItemComboBox_Measure.Items.Clear();
            repositoryItemComboBox_Position.Items.Clear();

            barEditItem_MeasureID.EditValue = null;
            barEditItem_PositionID.EditValue = null;
            repositoryItemComboBox_Measure.Items.AddRange(lstID);
        }
        private void CreateTotalReportFilter(DataTable dtView)
        {
            List<int> lstID = new List<int>();
            foreach (DataRow dr in dtView.Rows)
                lstID.Add(Convert.ToInt32(dr[SchemaPDV.BV_positionId]));

            repositoryItemComboBox_Position.Items.Clear();
            barEditItem_PositionID.EditValue = null;
            repositoryItemComboBox_Position.Items.AddRange(lstID);
        }

        private void DisplayRecordCount(int Record)
        {
            barStaticItem_RowCount.Caption = string.Format(" Records: {0} ", Record);
            barStaticItem_RowCount.Reset();
            Application.DoEvents();
        }

        private FrmGrid GetGrid(string table)
        {
            BaseDocument document = null;
            if (docMWS.GetDocument(table, out document))
                return document.Form as FrmGrid;
            else
                return null;
        }
    }
}
