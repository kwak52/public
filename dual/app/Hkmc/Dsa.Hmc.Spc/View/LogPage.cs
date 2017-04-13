using DevExpress.XtraEditors;
using System.Data;
using System;
using System.IO;
using DevExpress.XtraGrid.Columns;
using System.Collections.Generic;
using Dsu.Common.Utilities;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using System.Diagnostics;

namespace Dsa.Hmc.Spc
{
    public partial class LogPage : XtraUserControl
    {
        public DataTable TableLog { get; set; }
        public int UserExportCount { get; set; } = 0;
        private TagDataSource _tagDataSet;

        public LogPage(TagDataSource tagDataSet, DataTable dt)
        {
            _tagDataSet = tagDataSet;
            InitializeComponent();
            ucGrid1.DataSource = dt;
        }

        private void ucGrid1_Load(object sender, System.EventArgs e)
        {
            ucGrid1.RowFont = new Font("Tahoma", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            ucGrid1.ColumnFont = new Font("Tahoma", 18F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            ucGrid1.Editable = false;
            ucGrid1.ShowAutoFilterRow = true;
            ucGrid1.GridView.OptionsView.EnableAppearanceOddRow = true;
            ucGrid1.UEventRowCellStyle += UcGrid1_UEventRowCellStyle;

            foreach (GridColumn gridColumn in ucGrid1.GridView.Columns)
            {
                if (gridColumn.ColumnType == typeof(DateTime))
                {
                    gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    gridColumn.DisplayFormat.FormatString = "yy-MM-dd HH:mm:ss";
                }
                else
                {
                    gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    if (gridColumn.FieldName == "차량")
                        gridColumn.DisplayFormat.FormatString = "X";
                    else if (gridColumn.FieldName == "NO")
                        gridColumn.DisplayFormat.FormatString = "0";
                    else
                        gridColumn.DisplayFormat.FormatString = "0.00";
                }
            }

            ucGrid1.GridView.BestFitColumns();
        }

        private void UcGrid1_UEventRowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.CellValue == null || e.CellValue == DBNull.Value)
                return;

            GridView View = sender as GridView;
            DataRow dr = View.GetDataRow(e.RowHandle);
            if (dr == null)
                return;
          //  StyleColor(e, e.Column.FieldName, dr["구분"].ToString());
        }

        private void StyleColor(RowCellStyleEventArgs e, string colName, string type)
        {
          
             //           e.Appearance.BackColor = Color.Tomato;
        }

        public void ExportExcel(string path)
        {
            FileStream stream;
            string filePath = "";
            if (UserExportCount == 0)
                filePath = path + "\\" + DateTime.Now.Date.ToShortDateString() + ".xlsx";
            else
                filePath = path + "\\" + DateTime.Now.Date.ToShortDateString() + "_" + UserExportCount.ToString() + ".xlsx";

            stream = new FileStream(filePath, FileMode.Create);
            ucGrid1.ExportExcel(stream);
            stream.Close();

            if (UserExportCount != 0)
                Process.Start(@filePath);
        }

        public void ClearData()
        {
            TableLog.Rows.Clear();
            UserExportCount = 0;
        }

    }
}

