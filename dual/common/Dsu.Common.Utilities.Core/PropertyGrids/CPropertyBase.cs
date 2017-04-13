using System.ComponentModel;

namespace Dsu.Common.Utilities.PropertyGrids
{
    [TypeConverter(typeof(PropertySorter))]
    [DefaultProperty("Properties")]
    public class CPropertyBase
    {
    }
}
