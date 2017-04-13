using DevExpress.XtraBars.Docking2010.Views;
using Dsa.Kefico.MWS.Enumeration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace Dsa.Kefico.MWS
{
    public partial class frmMain
    {
        private void OpenDocument(EnumTable viewTables)
        {

            BaseDocument document = null;
            string Key = docMWS.GetKey(viewTables);

            if (!docMWS.Creating)
            {
                string viewQuery = this.viewQuery.Query(filterMWS, viewTables);
                Trace.WriteLine($"QUERY: {viewQuery};");
                if (LogDebug) WriteOutput("Query", viewQuery);

                if (viewQuery == string.Empty)
                    return;

                if (!docMWS.ExistDoc(Key))
                    docMWS.Creating = true;

                if (viewTables == EnumTable.showPHVChart)
                    DrowChart(Key, viewTables);
                else
                    DrowGrid(Key, viewTables, viewQuery);

                SetFilterEnableControl(Key, false);

                docMWS.GetDocument(Key, out document);
            }
            if (document != null)
                tabbedView.Controller.Activate(document);

            barStaticItem_StatusText.Caption = EnumEventResult.Ready.ToString();
        }


        private void DrowGrid(string docKey, EnumTable viewTables, string ViewQuery)
        {
            try
            {
                FrmGrid frmGrid = CreateForm(docKey, viewTables) as FrmGrid;

                Stopwatch sw = Stopwatch.StartNew();
                DataTable dtView = mySqlMWS.GetDataFromDBView(ViewQuery);
                barStaticItem_StatusText.Caption = string.Format("Select : {0} ", sw.ElapsedMilliseconds.ToString("0s 000ms"));
                frmGrid.ViewTable = viewTables;
                frmGrid.MdiParent = this;
                frmGrid.Show();
                frmGrid.ShowGrid(dtView);
                frmGrid.UEventRowCountChanged += frmGrid_UEventRowCountChanged;
                frmGrid.UEventMouseDoubleClick += frmGrid_UEventMouseDoubleClick;
                frmGrid.UEventMouseClick += frmGrid_UEventMouseClick;

                CreateFilter(dtView, viewTables);

            }
            catch (System.Exception ex) { docMWS.Creating = false; WriteOutputException(ex, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }

     
        private void DrowChart(string docKey, EnumTable viewTables)
        {
            try
            {
                DataTable dtInfo = mySqlMWS.GetDataFromDBView(viewQuery.QueryInfo(filterMWS, EnumTable.showPHVChart));
                if (dtInfo.Rows[0][SchemaMWS.BV_displayTypeSpc].ToString() == "STR")
                    return;

                FrmChart frmChart = CreateForm(docKey, viewTables) as FrmChart;
                DataTable dtSpc = GetSpcTable();
            

                chartSPC.SetDatasource(dtSpc, dtInfo);
                frmChart.ShowChart(chartSPC, ChartScaleAuto);
                frmChart.UEventBoundDataChanged += FrmChart_UEventBoundDataChanged;
                frmChart.MdiParent = this;
                frmChart.Show();
            }
            catch (System.Exception ex) { docMWS.Creating = false; WriteOutputException(ex, System.Reflection.MethodBase.GetCurrentMethod().Name); }

        }

        private DataTable GetSpcTable()
        {
            string viewQuery = this.viewQuery.Query(filterMWS, EnumTable.showPHVChart);
            DataTable dtSpc = new DataTable();
            BaseDocument document = null;
            if (docMWS.GetDocument(docMWS.GetKey(EnumTable.showPHV), out document))
            {
                FrmGrid frmGrid = document.Form as FrmGrid;
                if (frmGrid != null && frmGrid.DataSource != null)
                    dtSpc = frmGrid.DataSource;
            }
            else
                dtSpc = mySqlMWS.GetDataFromDBView(viewQuery);

            return dtSpc;
        }

        private Form CreateForm(string docKey, EnumTable viewTables)
        {
            BaseDocument document = null;
            Form form = null;

            if (viewTables == EnumTable.showPHVChart)
            {
                if (docMWS.GetDocument(docKey, out document))
                {
                    form = document.Form as FrmChart;
                    ((FrmChart)form).ClearChart();
                    ((FrmChart)form).UEventBoundDataChanged -= FrmChart_UEventBoundDataChanged;
                }
                else
                {
                    form = new FrmChart(docKey);
                }
            }
            else
            {
                if (docMWS.GetDocument(docKey, out document))
                {
                    form = document.Form as FrmGrid;
                    ((FrmGrid)form).LogDebug = LogDebug;
                    ((FrmGrid)form).ClearGrid();
                    ((FrmGrid)form).UEventRowCountChanged -= frmGrid_UEventRowCountChanged;
                    ((FrmGrid)form).UEventMouseDoubleClick -= frmGrid_UEventMouseDoubleClick;
                    ((FrmGrid)form).UEventMouseClick -= frmGrid_UEventMouseClick;
                    if (viewTables != EnumTable.showPHV)
                        tabbedView.Controller.Activate(document);
                }
                else
                    form = new FrmGrid(docKey, LogDebug);
            }
            return form;
        }

        private void CreateFilter(DataTable dtView, EnumTable viewTables)
        {
            try
            {
                if (viewTables == EnumTable.showTotalSummaryView)
                    CreateSummaryFilter(dtView);
                else if (viewTables == EnumTable.showSPSV)
                    CreatePositionFilter(dtView);
                else if (viewTables == EnumTable.showPHV)
                    CreateMeasureFilter(dtView);
                else if (viewTables == EnumTable.showSTSV)
                    CreateSelectTestFilter(dtView);
                else if (viewTables == EnumTable.showBundle)
                    CreateTotalReportFilter(dtView);
            }
            catch (System.Exception ex) { WriteOutputException(ex, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }
        private void CreateSummaryFilter(DataTable dtView)
        {
            List<int> lstID = new List<int>();
            foreach (DataRow dr in dtView.Rows)
                lstID.Add(Convert.ToInt32(dr[SchemaMWS.TSV_id]));

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
                lstID.Add(Convert.ToInt32(dr[SchemaMWS.SSV_positionId]));
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
                lstID.Add(Convert.ToInt32(dr[SchemaMWS.PSV_measureid]));
            repositoryItemComboBox_Measure.Items.Clear();
            repositoryItemComboBox_Measure.Items.AddRange(lstID);
        }

        private void CreateSelectTestFilter(DataTable dtView)
        {
            List<int> lstID = new List<int>();
            foreach (DataRow dr in dtView.Rows)
                lstID.Add(Convert.ToInt32(dr[SchemaMWS.PSV_id]));
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
                lstID.Add(Convert.ToInt32(dr[SchemaMWS.BV_positionId]));

            repositoryItemComboBox_Position.Items.Clear();
            barEditItem_PositionID.EditValue = null;
            repositoryItemComboBox_Position.Items.AddRange(lstID);
        }

        private void SetFilterEnableControl(string docKey, bool bShow)
        {
            if (docKey == docMWS.GetKey(EnumTable.showTotalSummaryView))
            {
                barButtonItem_Summary_Apply.Enabled = true;
            }
        }

        private void DisplayRecordCount(int Record)
        {
            barStaticItem_RowCount.Caption = string.Format(" Records: {0} ", Record);
            barStaticItem_RowCount.Reset();
            Application.DoEvents();
        }
    }
}