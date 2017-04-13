using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using Dsa.Kefico.MWS.Enumeration;
using Dsa.Kefico.MWS.EventHandler;
using Dsu.UI.Grid.GridOption;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Dsa.Kefico.MWS
{
    public partial class FrmGrid : Form
    {
        public UEventHandlerRowCountChanged UEventRowCountChanged;
        public UEventHandlerMouseDoubleClick UEventMouseDoubleClick;
        public UEventHandlerMouseClick UEventMouseClick;
        private EnumTable _viewTables;
        public bool LogDebug { get; set; }

        private DataRow _FitlerRowHandle = null;

        public FrmGrid(string Caption, bool bDebug)
        {
            InitializeComponent();
            this.Text = Caption;
            LogDebug = bDebug;
        }

        public DataRow RowHandleFilterSelect
        {
            get { return _FitlerRowHandle; }
            set { _FitlerRowHandle = value; }
        }

        public EnumTable ViewTable
        {
            get { return _viewTables; }
            set { _viewTables = value; }
        }

        public DataTable DataSource
        {
            get
            {
                DataTable dt = ucGrid1.DataSource as DataTable;
                if (dt == null)
                    return null;
                else
                    return dt;

            }
        }

        public void ShowGrid(DataTable dt)
        {
            ucGrid1.ShowGrid(dt);
            ucGrid1.UEventRowCountChanged += UcGrid1_UEventRowCountChanged;
            ucGrid1.UEventMouseDoubleClick += UcGrid1_UEventMouseDoubleClick;
            ucGrid1.UEventMouseClick += UcGrid1_UEventMouseClick;
            ucGrid1.UEventRowStyle += UcGrid1_UEventRowStyle;
            ucGrid1.UEventRowCellStyle += UcGrid1_UEventRowCellStyle;

                ucGrid1.UEventCustomColumnDisplayText += UcGrid1_UEventCustomColumnDisplayText;

            foreach (GridColumn gridColumn in ucGrid1.GridView.Columns)  //ahn test
            {
                if (gridColumn.FieldName.StartsWith("_"))
                    gridColumn.Visible = LogDebug;
                DisplayFormat.ViewDataTypePattern(gridColumn, true);
                DisplayFormat.ViewResultColor(gridColumn);
            }
        }

        public void ClearGrid()
        {
            ucGrid1.DataSource = null;
            ucGrid1.UEventRowCountChanged -= UcGrid1_UEventRowCountChanged;
            ucGrid1.UEventMouseDoubleClick -= UcGrid1_UEventMouseDoubleClick;
            ucGrid1.UEventMouseClick -= UcGrid1_UEventMouseClick;
            ucGrid1.UEventRowStyle -= UcGrid1_UEventRowStyle;
            ucGrid1.UEventRowCellStyle -= UcGrid1_UEventRowCellStyle;
            ucGrid1.UEventCustomColumnDisplayText -= UcGrid1_UEventCustomColumnDisplayText;
            ucGrid1.ClearGrid();
        }


        public void SetFilter(bool bFilterOn)
        {
            if (!bFilterOn)
                ucGrid1.GridView.ClearColumnsFilter();
            ucGrid1.ShowAutoFilterRow = bFilterOn;
        }


        public DataRow GetDataRow(string Field, int ID)
        {
            DataTable dt = ucGrid1.DataSource as DataTable;
            if (dt == null)
                return null;

            DataRow[] arrDr = dt.Select(string.Format("{0} = '{1}'", Field, ID));
            if (arrDr.Length == 0)
                return null;
            else
                return arrDr[0];
        }

        private void UcGrid1_UEventRowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            DataRow dr = View.GetDataRow(e.RowHandle);

            if (_FitlerRowHandle != null && _FitlerRowHandle == dr)
                e.Appearance.BackColor = Color.LightCyan;
            else
                e.Appearance.BackColor = Color.White;
        }

        private void UcGrid1_UEventRowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.CellValue == null || e.CellValue == DBNull.Value)
                return;

            GridView View = sender as GridView;
            StyleColor(e, e.Column.FieldName);
        }

        private void UcGrid1_UEventCustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == string.Empty)
                return;

            GridView View = sender as GridView;
            if (View.Columns.ColumnByFieldName(SchemaMWS.BV_displayType) != null)
                StyleFormat(View, e, e.Column.FieldName , SchemaMWS.BV_displayType);
            else if (View.Columns.ColumnByFieldName(SchemaMWS.TSV_pdvId) != null)
                StyleFormatDate(View, e, e.Column.FieldName);
        }

        private void StyleFormatDate(GridView View, CustomColumnDisplayTextEventArgs e, string colName)
        {
            switch (colName.ToUpper())
            {
                case "STARTTIME":
                case "ENDTIME":
                    if (e.Value is TimeSpan)
                        e.DisplayText = ((TimeSpan)e.Value).ToString("hh':'mm':'ss'.'ff"); break;
                default: break; 
            }
        }

        private void StyleFormat(GridView View, CustomColumnDisplayTextEventArgs e, string colName, string displayType)
        {
            string value = e.Value.ToString();
            switch (colName.ToUpper())
            {
                case "AVERAGE":
                case "VALUE":
                case "MAX":
                case "MIN":
                    string DisplayType = View.GetListSourceRowCellValue(e.ListSourceRowIndex, displayType).ToString();
                    if (DisplayType == "STR" || DisplayType == "BIN")
                        e.DisplayText = GaudiFileParserApi.Number2Ascii(GaudiFileParserApi.String2BigInt(value));
                    else if (DisplayType == "HEX")
                        e.DisplayText = GaudiFileParserApi.String2HexString(value);
//                     else if (DisplayType == "INT" && colName.ToUpper() != "AVERAGE" && value.EndsWith(".00000"))
//                         e.DisplayText = GaudiFileParserApi.String2BigInt(value).ToString();
                    else
                    {
                        // 일반 dimension 에서 소수점 이하 0으로 끝나는 부분을 제거함. e.g "15.00000" -> "15"
                        var match = Regex.Match(value, @".*(\.0+)$");
                        if (match.Groups.Count == 2)
                            e.DisplayText = Regex.Replace(value, @"\.0+", "");
                    }
                    break;
                default: break;
            }
        }

        private void StyleColor(RowCellStyleEventArgs e, string colName)
        {
            switch (colName.ToUpper())
            {
                case "CPK":
                    if (Convert.ToDouble(e.CellValue) < 1.33) ErrorColor(e); break;
                case "NG":
                    if (Convert.ToDouble(e.CellValue) > 0) ErrorColor(e); break;
                case "RESULT":
                    if (Convert.ToDouble(e.CellValue) == 0) ErrorColor(e); break;
                case "%GOOD":
                case "%GOOD100":
                    double d = Convert.ToDouble(e.CellValue);
                    if (d >= 95 && d < 99) WarningColor(e);
                    else if (d < 95) ErrorColor(e); break;
                case "INFO":
                    string value = e.CellValue.ToString();
                    if (value == "X F+" || value == "X F-") ErrorColor(e); break;
                default: break;
            }
        }

        private static void ErrorColor(RowCellStyleEventArgs e)
        {
            e.Appearance.BackColor = Color.Tomato;
            e.Appearance.ForeColor = Color.White;
        }

        private static void WarningColor(RowCellStyleEventArgs e)
        {
            e.Appearance.BackColor = Color.LightYellow;
        }

        private void UcGrid1_UEventMouseDoubleClick(object sender, int RowHandle)
        {
            if (UEventMouseDoubleClick != null)
            {
                GridView gridView = sender as GridView;

                _FitlerRowHandle = gridView.GetDataRow(RowHandle);
                if (_FitlerRowHandle == null)
                    return;

                gridView.RefreshData();

                UEventMouseDoubleClick(sender, gridView.GetDataRow(RowHandle), ViewTable);
            }
        }

        private void UcGrid1_UEventMouseClick(object sender, MouseEventArgs e, int rowHandle)
        {
            if (UEventMouseClick != null && e.Button == MouseButtons.Right)
            {
                GridView gridView = sender as GridView;

                _FitlerRowHandle = gridView.GetDataRow(rowHandle);
                if (_FitlerRowHandle == null)
                    return;

                gridView.RefreshData();

                UEventMouseClick(sender, gridView.GetDataRow(rowHandle), ViewTable);
            }
        }

        private void UcGrid1_UEventRowCountChanged(object sender, int RowCount)
        {
            UEventRowCountChanged?.Invoke(sender, RowCount);
        }
        private void frmGrid_Activated(object sender, EventArgs e)
        {
            UEventRowCountChanged?.Invoke(sender, ucGrid1.GridView.RowCount);
        }
    }
}
