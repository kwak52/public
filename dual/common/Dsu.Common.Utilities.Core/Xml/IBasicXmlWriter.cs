using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dsu.Common.Utilities.Xml
{
    public interface IBasicXmlWriter : IDisposable
    {
        XmlElement DocumentRoot { get; }

        XmlNode AppendChild(XmlNode child);
        XmlElement CreateElement(string nodeName);
        object CreateInstance(string tag);
        void Save();

        XmlAttribute AddAttribute(XmlNode node, string attrName, string value);
        XmlAttribute AddAttributeIfNonNull(XmlNode node, string attrName, string value);
    }


}
