using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.Common.Interfaces.Database
{
    public interface IDbConnection : IDisposable
    {
        string ConnectionString { get; set; }
    }
}
