using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Dsa.Kefico.PDV.Enumeration;
using Dsa.Kefico.PDV.EventHandler;
using Dsu.UI.Grid.GridOption;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Dsa.Kefico.PDV
{
    public partial class FrmGrid : Form
    {
        public UEventHandlerRowCountChanged UEventRowCountChanged;
        public UEventHandlerMouseDoubleClick UEventMouseDoubleClick;
        public UEventHandlerMouseClick UEventMouseClick;
        private string _viewTables;
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

        public string ViewTable
        {
            get { return _viewTables; }
            set { _viewTables = value; }
        }

        public void SetFilter(bool bFilterOn)
        {
            if (!bFilterOn)
                ucGrid1.GridView.ClearColumnsFilter();
            ucGrid1.ShowAutoFilterRow = bFilterOn;
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
            set { ucGrid1.DataSource = value; }
        }


        public void ShowGrid(DataTable dt)
        {
            
            ucGrid1.ShowGrid(dt);
            ucGrid1.UEventRowCountChanged += UcGrid1_UEventRowCountChanged;
            ucGrid1.UEventMouseDoubleClick += UcGrid1_UEventMouseDoubleClick;
            ucGrid1.UEventMouseClick += UcGrid1_UEventMouseClick;
            ucGrid1.UEventRowStyle += UcGrid1_UEventRowStyle;
            ucGrid1.GridView.BestFitColumns();

            foreach (GridColumn gridColumn in ucGrid1.GridView.Columns)
            {
                if (gridColumn.FieldName.StartsWith("_"))
                    gridColumn.Visible = LogDebug;

                DisplayFormat.ViewDataTypePattern(gridColumn, false);
                DisplayFormat.ViewResultColor(gridColumn);
            }

            if (dt.TableName == ViewPDV.selectPdvGroup)
            {
                foreach (GridColumn gridColumn in ucGrid1.GridView.Columns)  
                {
                        if (gridColumn.FieldName == "id")
                            gridColumn.Caption = "PRNR";
                        if (gridColumn.FieldName == "partNumberFamily" || gridColumn.FieldName == "changeNumber")
                            gridColumn.Visible = LogDebug;
                }
            }

            if (dt.TableName == ViewPDV.selectPdvTestList)
            {
                foreach (GridColumn gridColumn in ucGrid1.GridView.Columns)  
                {
                        if (gridColumn.FieldName == "id")
                            gridColumn.Visible = LogDebug;
                }
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

        public void ClearGrid()
        {
            ucGrid1.DataSource = null;
            ucGrid1.UEventRowCountChanged -= UcGrid1_UEventRowCountChanged;
            ucGrid1.UEventMouseDoubleClick -= UcGrid1_UEventMouseDoubleClick;
            ucGrid1.UEventMouseClick -= UcGrid1_UEventMouseClick;
            ucGrid1.UEventRowStyle -= UcGrid1_UEventRowStyle;
            ucGrid1.ClearGrid();
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
