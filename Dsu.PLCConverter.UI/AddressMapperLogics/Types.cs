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
        /// 기본적으로 null 값을 가짐.  null 이면 base.Name 값을 따름.  NonNull 로 지정되면 pattern 생성시 이 값을 이용
        /// </summary>
        public string PatternNameOverride { get; set; }

        /// <summary>
        /// 하나의 type 에 대해서 분할 mapping 된 조각들 정보
        /// </summary>
        [JsonIgnore]
        public List<MemoryRange> MemoryRanges = new List<MemoryRange>();

        [Browsable(false)]
        [JsonIgnore]
        public PLC PLC { get; set; }

        internal void Clear() => MemoryRanges.Clear();
    }

    /// <summary>
    /// 옴론 메모리 종류(W, H, D 등)에 따른 정보
    /// </summary>
    public class OmronMemorySection : MemorySection
    {
        // 추후에 추가 예정
        [JsonIgnore]
        /// <summary>
        /// Mapping 가능한 XG5000의 memory 영역 이름
        /// </summary>
        public List<string> MappableXg5kNames { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "Bit")]
        public bool BitAccessable { get; set; } = true;
        [JsonProperty(PropertyName = "Word")]
        public bool WordAccessable { get; set; } = true;

        public OmronMemorySection(string name, int start, int end, IEnumerable<string> mappableNames)
            : base(name, start, end)
        {
            MappableXg5kNames = mappableNames.ToList();
        }
        [JsonConstructor] OmronMemorySection() { }
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

    /// <summary>
    /// 할당된 메모리 구간
    /// </summary>
    public class AllocatedMemoryRange : MemoryRange
    {
        /// <summary>
        /// mapping 의 상대편 구간
        /// </summary>
        public AllocatedMemoryRange Counterpart { get; set; }
        public AllocatedMemoryRange(int start, int end, MemorySection parent)
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
}
