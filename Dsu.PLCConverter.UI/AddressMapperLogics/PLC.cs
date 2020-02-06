using Dsu.Common.Utilities.ExtensionMethods;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dsu.PLCConverter.UI.AddressMapperLogics
{
    public class PLC
    {
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
        List<OmronMemorySection> _memories = new List<OmronMemorySection>();
        public string PLCType { get; set; }
        [Browsable(false)]
        public OmronMemorySection[] Memories => _memories.ToArray();
        public OmronPLC(string plcType, IEnumerable<OmronMemorySection> memSections)
            : base(memSections)
        {
            PLCType = plcType;
            _memories = memSections.ToList();
        }

        [JsonConstructor] OmronPLC() { }
        public void Clear() { Memories.Iter(m => m.Clear()); }
    }

    /// <summary>
    /// 산전 모든 PLC type 별 정의
    /// </summary>
    public class Xg5kPLC : PLC
    {
        List<Xg5kMemorySection> _memories = new List<Xg5kMemorySection>();
        public string PLCType { get; set; }
        [Browsable(false)]
        public Xg5kMemorySection[] Memories => _memories.ToArray();

        public Xg5kPLC(string plcType, IEnumerable<Xg5kMemorySection> memSections)
            : base(memSections)
        {
            PLCType = plcType;
            _memories = memSections.ToList();
        }

        [JsonConstructor] Xg5kPLC() { }
        public void Clear() { Memories.Iter(m => m.Clear()); }
    }


    /// <summary>
    /// 옴론 및 산전 모든 PLC type 별 정의
    /// </summary>
    public class PLCs
    {
        List<OmronPLC> _omronPLCs = new List<OmronPLC>();
        List<Xg5kPLC> _xg5000PLCs = new List<Xg5kPLC>();

        public OmronPLC[] OmronPLCs => _omronPLCs.ToArray();
        public Xg5kPLC[] XG5000PLCs => _xg5000PLCs.ToArray();
        public PLCs(IEnumerable<OmronPLC> omronPLCs, IEnumerable<Xg5kPLC> xg5kPLCs)
        {
            _omronPLCs = omronPLCs.ToList();
            _xg5000PLCs = xg5kPLCs.ToList();
        }
        [JsonConstructor] PLCs() { }

        public static PLCs CreateSamplePLCs()
        {
            var omronPLCs = new[]
            {
                new OmronPLC("CJ1H",
                    new[] {
                        new OmronMemorySection("PIO",   0, 1024,     new []{"P" }),
                        new OmronMemorySection("D",     0, 1024*10, new []{"M", "D" }),
                        new OmronMemorySection("J",     0, 1024*2,  new []{"M", "D" }),
                        new OmronMemorySection("K",     0, 512,     new []{"M", "D" }),
                    }),
                new OmronPLC("CJ2H",
                    new[] {
                        new OmronMemorySection("PIO",   0, 1024*10, new []{"P" }),
                        new OmronMemorySection("D",     0, 1024,    new []{"M", "D" }),
                        new OmronMemorySection("J",     0, 1024*2,  new []{"M", "D" }),
                        new OmronMemorySection("K",     0, 512,     new []{"M", "D" }),
                    }),
            };

            var xg5kPLCs = new[]
            {
                new Xg5kPLC("Xg5k1H",
                    new[] {
                        new Xg5kMemorySection("P",   0, 1024*10),
                        new Xg5kMemorySection("M",   0, 1024*10),
                        new Xg5kMemorySection("L",   0, 1024),
                    }),
                new Xg5kPLC("Xg5k2H",
                    new[] {
                        new Xg5kMemorySection("P",   0, 1024),
                        new Xg5kMemorySection("M",   0, 1024),
                        new Xg5kMemorySection("X",   0, 10240),
                        new Xg5kMemorySection("Y",   0, 512),
                        new Xg5kMemorySection("A",   0, 16),
                    }),
            };

            var plcs = new PLCs(omronPLCs, xg5kPLCs);
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
