using System;
using System.Collections.Generic;

namespace Dsu.Common.Utilities.Xml
{
    public class BasicXmlReader : XmlBase, IBasicXmlReader
    {
        public BasicXmlReader(string filePath, IFormProgressbar progress, IEnumerable<string> validTags)
            : base(filePath, progress, validTags)
        {
            XmlDocument.Load(filePath);
        }

        public override object CreateInstance(string tag)
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
