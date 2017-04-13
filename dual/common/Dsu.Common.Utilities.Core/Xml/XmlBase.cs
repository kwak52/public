using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Xml;

namespace Dsu.Common.Utilities.Xml
{
    public abstract class XmlBase : IDisposable
    {

        /// <summary> 최종 희망 XML 저장 format version.   see FileFormatVersion-XXX.txt </summary>
        public virtual int LastFileFormatVersion { get { return 1; } }

        /// <summary> 현재 XML 저장 format version.   see FileFormatVersion-XXX.txt </summary>
        public int Version { get { return _version; } protected set { _version = value; } }
        protected int _version = 1;

        public string FilePath { get; private set; }
        public XmlDocument XmlDocument { get; private set; }

        private string[] _validTags = null;
        public string[] ValidTags { get {return _validTags; } set { _validTags = value; }}

        public IFormProgressbar Progress { get; set; }

        protected XmlBase(string filePath, IFormProgressbar progress, IEnumerable<string> validTags)
        {
            XmlDocument = new XmlDocument();
            FilePath = filePath;
            ValidTags = validTags.ToArray();
            Progress = progress;
        }

        public virtual bool ValidTag_p(string tag) { return !String.IsNullOrEmpty(tag) && ValidTags.Contains(tag); }


        public abstract object CreateInstance(string tag);

        public virtual object CreateInstance(XmlNode node)
        {
            return CreateInstance(node.Name);
        }


        public static XmlNode SelectSingleNodeWithSameName(XmlNode node, IEnumerable<string> names)
        {
            Contract.Requires(node != null && names != null && names.Count() > 0);
            var query = new StringBuilder(String.Format("*[local-name()='{0}'", names.First()));
            foreach (var name in names.Skip(1))
                query.Append(String.Format(" or local-name()='{0}'", name));
            query.Append("]");

            return node.SelectSingleNode(query.ToString());
        }
        public static XmlNode SelectSingleNodeContainingName(XmlNode node, IEnumerable<string> names)
        {
            Contract.Requires(node != null && names != null && names.Count() > 0);
            var query = new StringBuilder(String.Format("*[contains(local-name(), '{0}')", names.First()));
            foreach (var name in names.Skip(1))
                query.Append(String.Format(" or contains(local-name(), '{0}')", name));
            query.Append("]");

            return node.SelectSingleNode(query.ToString());
        }

        public XmlAttribute AddAttribute(XmlNode node, string attrName, string value)
        {
            XmlAttribute attr = XmlDocument.CreateAttribute(attrName);
            attr.Value = value;

            return node.Attributes.Append(attr);
        }

        public XmlAttribute AddAttributeIf(bool condition, XmlNode node, string attrName, string value)
        {
            if (!condition)
                return null;

            return AddAttribute(node, attrName, value);
        }


        public XmlAttribute AddAttributeIfNonNull(XmlNode node, string attrName, string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            return AddAttribute(node, attrName, value);
        }
        public XmlAttribute AddAttributeIfTrue(XmlNode node, string attrName, bool value)
        {
            if (! value)
                return null;

            return AddAttribute(node, attrName, value.ToString());
        }


        public XmlAttribute AddAttributeIfNonZero<T>(XmlNode node, string attrName, T value)
        {
            /* type T 의 default value(numeric 이면 zero가 될 것임) 와 value 를 비교 */
            if (value.Equals(Activator.CreateInstance(typeof(T))))
                return null;

            return AddAttribute(node, attrName, value.ToString());
        }


        public virtual void Dispose()
        {
            XmlDocument = null;
        }
    }


    public static class XmlQueryExtension
    {
        public static XmlNodeList SelectNodesContainingName(this XmlNode node, string name)
        {
            return node.SelectNodes(String.Format("*[contains(local-name(), '{0}')]", name));
        }

        public static XmlNode SelectSingleNodeContainingName(this XmlNode node, string name)
        {
            return node.SelectSingleNode(String.Format("*[contains(local-name(), '{0}')]", name));
        }

    }
}
