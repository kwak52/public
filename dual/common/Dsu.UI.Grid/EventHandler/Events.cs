using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dsu.UI.Grid.EventHandler
{
    public delegate void UEventHandlerRowCountChanged(object sender, int RowCount);
    public delegate void UEventHandlerMouseDoubleClick(object sender, int RowHandle);
    public delegate void UEventHandlerMouseClick(object sender, MouseEventArgs e, int RowHandle);
    public delegate void UEventHandlerRowStyle(object sender, RowStyleEventArgs e);
    public delegate void UEventHandlerRowCellStyle(object sender, RowCellStyleEventArgs e);
    public delegate void UEventHandlerCustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e);
    public delegate void UEventHandlerDataSourceChanged(object sender, EventArgs e);
    public delegate void UEventHandlerCellValueChanged(object sender, CellValueChangedEventArgs e);

    
}
