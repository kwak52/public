using System.Xml;

namespace Dsu.Common.Utilities.Xml
{
    public interface IBasicXmlSerializable
    {
        string GetXmlNodeName();

        /* XML Writing : Object instance --> XML node */
        XmlNode ToXml(IBasicXmlWriter writer);

        /* XML Reading : XML node --> Object instance */
        void FromXml(IBasicXmlReader reader, XmlNode node);     // virtual, base.call
    }
}
