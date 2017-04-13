using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using Dsu.UI.Grid.EventHandler;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.IO;
using DevExpress.XtraPrinting;

namespace Dsu.UI.Grid
{
    public partial class ucGrid : UserControl
    {
        public event UEventHandlerRowCountChanged UEventRowCountChanged;
        public event UEventHandlerMouseDoubleClick UEventMouseDoubleClick;
        public event UEventHandlerMouseClick UEventMouseClick;
        public event UEventHandlerRowStyle UEventRowStyle;
        public event UEventHandlerRowCellStyle UEventRowCellStyle;
        public event UEventHandlerCustomColumnDisplayText UEventCustomColumnDisplayText;
        public event UEventHandlerDataSourceChanged UEventDataSourceChanged;
        public event UEventHandlerCellValueChanged UEventCellValueChanged;
        public ucGrid()
        {
            InitializeComponent();
            InitializeGrid();
        }
        private void InitializeGrid()
        {
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsView.ShowAutoFilterRow = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsClipboard.CopyColumnHeaders = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            gridView1.RowStyle += GridView1_RowStyle;
            gridView1.RowCellStyle += GridView1_RowCellStyle;
            gridView1.CustomColumnDisplayText += GridView1_CustomColumnDisplayText;
            gridControl1.DataSourceChanged += GridControl1_DataSourceChanged;
        }

        public void ClearGrid()
        {
        }

        private void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (UEventCellValueChanged != null)
                UEventCellValueChanged(sender, e);
        }

        private void GridControl1_DataSourceChanged(object sender, EventArgs e)
        {
            if (UEventDataSourceChanged != null)
                UEventDataSourceChanged(sender, e);
        }
        private void GridView1_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (UEventCustomColumnDisplayText != null)
                UEventCustomColumnDisplayText(sender, e);
        }
    
        private void GridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (UEventRowStyle != null)
                UEventRowStyle(sender, e);
        }
        private void GridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (UEventRowCellStyle != null)
                UEventRowCellStyle(sender, e);
        }

        public System.Drawing.Font RowFont
        {
            get { return gridView1.Appearance.Row.Font; }
            set
            {
                gridView1.Appearance.Row.Font = value;
                gridView1.Appearance.Row.Options.UseFont = true;
            }
        }
        public System.Drawing.Font ColumnFont
        {
            get { return gridView1.Appearance.HeaderPanel.Font; }
            set { gridView1.Appearance.HeaderPanel.Font = value; }
        }
        public bool Editable
        {
            get { return gridView1.OptionsBehavior.Editable; }
            set { gridView1.OptionsBehavior.Editable = value; }
        }
        public bool MultiSelect
        {
            get { return gridView1.OptionsSelection.MultiSelect; }
            set { gridView1.OptionsSelection.MultiSelect = value; }
        }
        public bool ShowAutoFilterRow
        {
            get { return gridView1.OptionsView.ShowAutoFilterRow; }
            set { gridView1.OptionsView.ShowAutoFilterRow = value; }
        }
        public bool ShowGroupPanel
        {
            get { return gridView1.OptionsView.ShowGroupPanel; }
            set { gridView1.OptionsView.ShowGroupPanel = value; }
        }
        public GridView GridView
        {
            get { return gridView1; }
        }
        public object DataSource
        {
            get { return gridControl1.DataSource; }
            set { gridControl1.DataSource = value; }
        }
        public void ShowGrid(DataTable dt)
        {
            gridControl1.DataSource = dt;
        }
        public void ExportCSV(string path)
        {
            gridControl1.ExportToCsv(path);
        }
        public void ExportExcel(Stream stream)
        {
            //  gridControl1.ExportToXls(stream);
            XlsxExportOptions x = new XlsxExportOptions();
            x.ExportMode = XlsxExportMode.SingleFilePageByPage;
            gridControl1.ExportToXlsx(stream);
        }
        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            if (UEventRowCountChanged != null)
                UEventRowCountChanged(sender, gridView1.RowCount);
        }
        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridHitInfo hitInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
            if ((hitInfo.InRow) && (!gridView1.IsGroupRow(hitInfo.RowHandle)))
                UEventMouseDoubleClick?.Invoke(gridView1, hitInfo.RowHandle);
        }
        private void gridControl1_MouseClick(object sender, MouseEventArgs e)
        {
            GridHitInfo hitInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
            UEventMouseClick?.Invoke(gridView1, e, hitInfo.RowHandle);
        }
    }
}
