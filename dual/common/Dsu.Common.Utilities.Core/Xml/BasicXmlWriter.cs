using System;
using System.Collections.Generic;
using System.Xml;

namespace Dsu.Common.Utilities.Xml
{
    public class BasicXmlWriter : XmlBase, IBasicXmlWriter
    {
        public XmlElement DocumentRoot { get; private set; }

        public BasicXmlWriter(string filePath, IFormProgressbar progress, IEnumerable<string> validTags)
            : base(filePath, progress, validTags)
        {
            XmlDocument.AppendChild(XmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            DocumentRoot = XmlDocument.CreateElement("Document");
            XmlDocument.AppendChild(DocumentRoot);
            AddAttribute(DocumentRoot, "Version", Version.ToString());
        }

        public XmlElement CreateElement(string nodeName)
        {
            return XmlDocument.CreateElement(nodeName);
        }

        public override object CreateInstance(string tag)
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            base.Dispose();
            DocumentRoot = null;
        }

        public XmlNode AppendChild(XmlNode child) { return DocumentRoot.AppendChild(child); }
        public void Save() { XmlDocument.Save(FilePath); }
    }
}
