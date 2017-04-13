using System;
using System.ComponentModel;
using System.Xml;
using Dsu.Common.Interfaces;
using Dsu.Common.Utilities.Xml;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Dualsoft 의 모든 application 에서 공통으로 사용가능한 configuration 항목들
    /// </summary>
    public abstract class CommonConfiguration : IConfiguration, IRegistrySerializable, IBasicXmlSerializable
    {
        public static CommonConfiguration TheCommonConfiguration { get; protected set; }
        public bool PreferDialogTopMost { get; set; }
        [Description("Open last file, when program starts.")]
        public bool IsOpenLastFileWhenStartUp { get; set; }
        public bool PreferHudDialog { get; set; }

        [Description("Enable script feature.")]
        public bool IsEnableScript { get; set; }

        /// <summary> 사용자 script 에서의 log 사용에 대해 기본적으로 enable 시킬 것인지의 여부 </summary>
        [Description("Automatic enable logging for user script.")]
        public bool IsAutomaticEnableLogForUserScript { get; set; }

        public virtual string ScriptProjectFileName { get; set; }


        [Description("Enable application idle time processing.")]
        public bool IsEnableApplicationIdleProcessing { get; set; }

        public bool IsEnableDebug { get; set; }


        public string LastOpenFolder { get; set; }

        [Description("Maximum number of visible rows in log window.")]
        public int MaxNumLogEntries { get { return _maxNumLogEntries; } set { _maxNumLogEntries = Math.Max(1, value); } }

        private int _maxNumLogEntries = 1000;


        [Description("UI update delay allowed.  recommend 300.")]
        public int UIUpdateDelayInMilliseconds { get; set; }

        [Description("Language setting for user interfaces.")]
        public SupportedCultures Language { get { return _language; } set { _language = value; InvokeChanged("Language"); } }

        private SupportedCultures _language;

        public enum FileOpenLocationType
        {
            CurrentDocument,
            Last,
        }
        public FileOpenLocationType FileOpenInitialFolder { get; set; }

        public event EventHandler<ConfigurationChangedEventArgs> ConfigurationChangedHook;

        protected void InvokeChanged(string propertyName)
        {
            ConfigurationChangedHook.Handle(this, new ConfigurationChangedEventArgs(propertyName));
        }

        public CommonConfiguration()
        {
            PreferHudDialog = true;
            PreferDialogTopMost = false;
            UIUpdateDelayInMilliseconds = 300;
            IsEnableDebug = false;
            IsEnableApplicationIdleProcessing = true;
        }
        public abstract void QSave(string fileName);

        public abstract void QLoad(string fileName);

        public abstract string GetXmlNodeName();

        public virtual XmlNode ToXml(IBasicXmlWriter writer)
        {
            XmlNode config = writer.CreateElement(GetXmlNodeName());
            return config;
        }

        public virtual void FromXml(IBasicXmlReader reader, XmlNode node)
        {
        }

        /// <summary>
        /// http://stackoverflow.com/questions/21116554/proper-way-to-implement-icloneable
        /// </summary>
        public object Clone() { return this.MemberwiseClone(); }
    }


    public class ConfigurationChangedEventArgs : EventArgs
    {
        public string ChangedPropertyName { get; private set; }

        public ConfigurationChangedEventArgs(string changedPropertyName)
        {
            ChangedPropertyName = changedPropertyName;
        }
    }



}
