using Dsu.Common.Utilities.ExtensionMethods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConverter.UI.AddressMapperLogics
{
    public interface IMemoryRange
    {
        int Start { get; }
        int End { get; }
        int Length { get; }
    }
    public interface IMemorySection { }

    [DebuggerDisplay("[{Start}:{End}]")]
    public class MemoryRangeBase : IMemoryRange
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Length => End - Start;
        internal MemoryRangeBase(int start, int end)
        {
            Start = start;
            End = end;
        }
        [JsonConstructor] protected MemoryRangeBase() {}
    }

    [DebuggerDisplay("{Name}=[{Start}:{End}]")]
    public class NamedMemoryRange : MemoryRangeBase
    {
        /// <summary>
        /// "PIO", "M", "T", ...
        /// </summary>
        public string Name { get; set; }
        public NamedMemoryRange(string name, int start, int end)
            : base(start, end)
        {
            Name = name;
        }
        [JsonConstructor] protected NamedMemoryRange() { }
    }


    /// <summary>
    /// 타입별 PLC 메모리.
    /// </summary>
    public class MemorySection : NamedMemoryRange, IMemorySection
    {
        public MemorySection(string name, int start, int end)
            : base(name, start, end)
        {
        }
        [JsonConstructor] protected MemorySection() {}

        /// <summary>
        /// 하나의 type 에 대해서 분할 mapping 된 조각들 정보
        /// </summary>
        [JsonIgnore]
        public List<MemoryRange> MemoryRanges = new List<MemoryRange>();
        internal void Clear() => MemoryRanges.Clear();
    }

    /// <summary>
    /// 옴론 메모리 종류(W, H, D 등)에 따른 정보
    /// </summary>
    public class OmronMemorySection : MemorySection
    {
        /// <summary>
        /// Mapping 가능한 XG5000의 memory 영역 이름
        /// </summary>
        public List<string> MappablesNames = new List<string>();
        public OmronMemorySection(string name, int start, int end, IEnumerable<string> mappableNames)
            : base(name, start, end)
        {
            MappablesNames = mappableNames.ToList();
        }
    }

    public class MemoryRange : MemoryRangeBase
    {
        public MemorySection Parent { get; set; }
        public MemoryRange(int start, int end, MemorySection parent)
            : base(start, end)
        {
            Parent = parent;
        }
    }
    public class AllocatedMemoryRange : MemoryRange
    {
        public AllocatedMemoryRange(int start, int end, MemorySection parent)
            : base(start, end, parent)
        {
        }
    }
    public class FreeMemoryRange : MemoryRange
    {
        public FreeMemoryRange(int start, int end, MemorySection parent)
            : base(start, end, parent)
        {
        }
    }



    public class Xg5kMemorySection : MemorySection
    {
        public Xg5kMemorySection(string name, int start, int end)
            : base(name, start, end)
        {
        }
        [JsonConstructor] Xg5kMemorySection() {}
    }

    /// <summary>
    /// 옴론 모든 PLC type 별 정의
    /// </summary>
    public class OmronPLC
    {
        List<OmronMemorySection> _memories = new List<OmronMemorySection>();
        public string PLCType { get; set; }
        [Browsable(false)]
        public OmronMemorySection[] Memories => _memories.ToArray();
        public OmronPLC(string plcType, IEnumerable<OmronMemorySection> memSections)
        {
            PLCType = plcType;
            _memories = memSections.ToList();
        }

        [JsonConstructor] OmronPLC() {}
        public void Clear() { Memories.Iter(m => m.Clear()); }
    }

    /// <summary>
    /// 산전 모든 PLC type 별 정의
    /// </summary>
    public class Xg5kPLC
    {
        List<Xg5kMemorySection> _memories = new List<Xg5kMemorySection>();
        public string PLCType { get; set; }
        [Browsable(false)]
        public Xg5kMemorySection[] Memories => _memories.ToArray();
        public Xg5kPLC(string plcType, IEnumerable<Xg5kMemorySection> memSections)
        {
            PLCType = plcType;
            _memories = memSections.ToList();
        }

        [JsonConstructor] Xg5kPLC() {}
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
        [JsonConstructor] PLCs() {}

        public static PLCs CreateSamplePLCs()
        {
            var omronPLCs = new[]
            {
                new OmronPLC("CJ1H",
                    new[] {
                        new OmronMemorySection("PIO",   0, 1024*10, new []{"P" }),
                        new OmronMemorySection("D",     0, 1024*10, new []{"M", "D" }),
                        new OmronMemorySection("J",     0, 1024*2,  new []{"M", "D" }),
                        new OmronMemorySection("K",     0, 512,     new []{"M", "D" }),
                    }),
                new OmronPLC("CJ2H",
                    new[] {
                        new OmronMemorySection("PIO",   0, 1024,    new []{"P" }),
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
