using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities
{
    public interface INamedObject
    {
        string Name { get; set; }
    }

    public class NamedObject : INamedObject
    {
        public string Name { get; set; }

        public NamedObject(string name) { Name = name; }
    }

    public class NamedDescribable : INamed, IDescribable
    {
        public string Name { get; set; }
        public string Note { get; set; }
    }
}
