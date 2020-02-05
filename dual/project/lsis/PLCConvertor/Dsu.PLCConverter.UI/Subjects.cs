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
        public static Subject<Tuple<UcMemoryBar, string>> MemorySectionChangeRequestSubject = new Subject<Tuple<UcMemoryBar, string>>();
    }
}
