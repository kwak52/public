using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.UI.XbarGantt
{
    public class BarPoint
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string Name { get; private set; }
        public Int64 SortId { get; set; }
        public int Id { get; set; }
        public string Value { get; set; }
        public string Type { get; private set; }

        public BarPoint() { }

        public BarPoint(int id, string name, string type, Int64 sortId, DateTime startTime, DateTime endTime, string value = "")
        {
            Id = id;
            Name = name;
            SortId = sortId;
            StartTime = startTime;
            EndTime = endTime;
            Type = type;
            Value = value;
        }

    }
}
