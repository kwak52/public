using Dsu.Common.Utilities.ExtensionMethods;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dsu.PLCConverter.UI.AddressMapperLogics
{
    public class PLC
    {
        [JsonIgnore]
        public List<MemorySection> MemorySections = new List<MemorySection>();
        public PLC(IEnumerable<MemorySection> memorySections)
        {
            memorySections.Iter(s => s.PLC = this);
            MemorySections = memorySections.ToList();
        }
        [JsonConstructor] protected PLC() { }

    }


    /// <summary>
    /// 옴론 모든 PLC type 별 정의
    /// </summary>
    public class OmronPLC : PLC
    {
        [Browsable(false)]
        public List<OmronMemorySection> Memories { get; set; } = new List<OmronMemorySection>();
        public string PLCType { get; set; }

        public OmronPLC(string plcType, IEnumerable<OmronMemorySection> memSections)
            : base(memSections)
        {
            PLCType = plcType;
            Memories = memSections.ToList();
        }

        [JsonConstructor] OmronPLC() { }
        public void Clear() { Memories.Iter(m => m.Clear()); }
    }

    /// <summary>
    /// 산전 모든 PLC type 별 정의
    /// </summary>
    public class Xg5kPLC : PLC
    {
        [Browsable(false)]
        public List<Xg5kMemorySection> Memories { get; set; } = new List<Xg5kMemorySection>();
        public string PLCType { get; set; }

        public Xg5kPLC(string plcType, IEnumerable<Xg5kMemorySection> memSections)
            : base(memSections)
        {
            PLCType = plcType;
            Memories = memSections.ToList();
        }

        [JsonConstructor] Xg5kPLC() { }
        public void Clear() { Memories.Iter(m => m.Clear()); }
    }


    /// <summary>
    /// 옴론 및 산전 모든 PLC type 별 정의
    /// </summary>
    public class PLCHWSpecs
    {
        public List<OmronPLC> OmronPLCs { get; set; } = new List<OmronPLC>();
        public List<Xg5kPLC> XG5000PLCs { get; set; } = new List<Xg5kPLC>();

        public PLCHWSpecs(IEnumerable<OmronPLC> omronPLCs, IEnumerable<Xg5kPLC> xg5kPLCs)
        {
            OmronPLCs = omronPLCs.ToList();
            XG5000PLCs = xg5kPLCs.ToList();
        }
        [JsonConstructor] PLCHWSpecs() { }

        public static PLCHWSpecs CreateSamplePLCs()
        {
            var omronPLCs = new[]
            {
                new OmronPLC("CJ1H",
                    new[] {
                        new OmronMemorySection("PIO",   0, 1024-1,    new []{"P" }),
                        new OmronMemorySection("D",     0, 1024*10-1, new []{"M", "D" }),
                        new OmronMemorySection("J",     0, 1024*2-1,  new []{"M", "D" }),
                        new OmronMemorySection("K",     0, 512-1,     new []{"M", "D" }),
                    }),
                new OmronPLC("CJ2H",
                    new[] {
                        new OmronMemorySection("PIO",   0, 1024*10-1, new []{"P" }),
                        new OmronMemorySection("D",     0, 1024-1,    new []{"M", "D" }),
                        new OmronMemorySection("J",     0, 1024*2-1,  new []{"M", "D" }),
                        new OmronMemorySection("K",     0, 512-1,     new []{"M", "D" }),
                    }),
            };

            var xg5kPLCs = new[]
            {
                new Xg5kPLC("Xg5k1H",
                    new[] {
                        new Xg5kMemorySection("P",   0, 1024*10-1),
                        new Xg5kMemorySection("M",   0, 1024*10-1),
                        new Xg5kMemorySection("L",   0, 1024-1),
                    }),
                new Xg5kPLC("Xg5k2H",
                    new[] {
                        new Xg5kMemorySection("P",   0, 1024-1),
                        new Xg5kMemorySection("M",   0, 1024-1),
                        new Xg5kMemorySection("X",   0, 10240-1),
                        new Xg5kMemorySection("Y",   0, 512-1),
                        new Xg5kMemorySection("A",   0, 16-1),
                    }),
            };

            var plcs = new PLCHWSpecs(omronPLCs, xg5kPLCs);
            return plcs;
        }
    }

    /// <summary>
    /// 옴론 vs 산전 PLC type 별 mapping
    /// </summary>
    public class PLCMapping
    {
        public OmronPLC OmronPLC { get; private set; }
        public Xg5kPLC Xg5kPLC { get; private set; }
        public PLCMapping(OmronPLC omron, Xg5kPLC xg5k)
        {
            OmronPLC = omron;
            Xg5kPLC = xg5k;
        }
    }
}
