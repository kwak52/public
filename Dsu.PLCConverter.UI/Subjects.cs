using Dsu.PLCConverter.UI.AddressMapperLogics;
using Dsu.PLCConvertor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConverter.UI
{
    public static class Subjects
    {
        public static Subject<Tuple<PLCVendor, string>> MemorySectionChangeRequestSubject = new Subject<Tuple<PLCVendor, string>>();
        public static Subject<PLCMapping> PLCMappingChangeRequestSubject = new Subject<PLCMapping>();        
    }
}
