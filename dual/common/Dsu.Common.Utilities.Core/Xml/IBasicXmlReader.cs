using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dsu.Common.Utilities.Xml
{
    public interface IBasicXmlReader : IDisposable
    {
        object CreateInstance(string tag);
        object CreateInstance(XmlNode node);
    }
}
