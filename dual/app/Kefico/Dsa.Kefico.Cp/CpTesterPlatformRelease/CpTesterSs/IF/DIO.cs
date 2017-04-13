using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTesterSs
{
    public class DIO
    {
        public int Index { get; set; }
        public bool On { get; set; }
        public string Name { get; set; }
        public DIO(int index, bool on, string name)
        {
            Index = index;
            On = on;
            Name = name;
        }
    }
}
