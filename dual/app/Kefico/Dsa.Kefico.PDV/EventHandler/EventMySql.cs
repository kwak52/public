using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dsa.Kefico.PDV.EventHandler
{
    public delegate void UEventHandlerSqlReadRecordPosition(object sender, int Position);
    public delegate void UEventHandlerSqlReadRecordCount(object sender, int RowCount);
    public delegate void UEventHandlerSqlException(MySqlException err);
}
