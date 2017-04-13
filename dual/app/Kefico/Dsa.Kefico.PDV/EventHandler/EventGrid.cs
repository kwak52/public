using Dsa.Kefico.PDV.Enumeration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dsa.Kefico.PDV.EventHandler
{
    public delegate void UEventHandlerRowCountChanged(object sender, int RowCount);
    public delegate void UEventHandlerMouseDoubleClick(object sender, DataRow dataRow, string viewTables);
    public delegate void UEventHandlerMouseClick(object sender, DataRow dataRow, string viewTables);



}
