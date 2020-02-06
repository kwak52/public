using Dsu.PLCConverter.UI.AddressMapperLogics;

namespace AddressMapper
{
    public class RangeMapping
    {
        public string NameO => Omron.Parent.Name;
        public int StartO { get => Omron.Start; set => Omron.Start = value; }
        public int EndO { get => Omron.End; set => Omron.End = value; }
        public string NameX => Xg5k.Parent.Name;
        public int StartX { get => Xg5k.Start; set => Xg5k.Start = value; }
        public int EndX { get => Xg5k.End; set => Xg5k.End = value; }

        public bool Word => ((OmronMemorySection)Omron.Parent).WordAccessable;
        public bool Bit => ((OmronMemorySection)Omron.Parent).BitAccessable;

        public AllocatedMemoryRange Omron { get; private set; }
        public AllocatedMemoryRange Xg5k { get; private set; }
        public RangeMapping(AllocatedMemoryRange omron, AllocatedMemoryRange xg5k)
        {
            Omron = omron;
            Xg5k = xg5k;
        }
    }
}
