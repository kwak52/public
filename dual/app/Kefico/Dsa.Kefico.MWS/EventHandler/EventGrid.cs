using Dsa.Kefico.MWS.Enumeration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Dsa.Kefico.MWS.EventHandler
{
    public delegate void UEventHandlerRowCountChanged(object sender, int RowCount);
    public delegate void UEventHandlerMouseDoubleClick(object sender, DataRow dataRow, EnumTable viewTables);
    public delegate void UEventHandlerMouseClick(object sender, DataRow dataRow, EnumTable viewTables);


}
