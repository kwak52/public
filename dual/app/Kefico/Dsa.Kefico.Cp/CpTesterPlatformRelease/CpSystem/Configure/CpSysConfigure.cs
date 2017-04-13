using System;
using System.IO;
using System.Reflection;
using System.Xml;
using CpTesterPlatform.CpCommon;
using System.Xml.Linq;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpSystem.Configure
{
    /// <summary>
    /// Properties for the system configuration.
    /// </summary>
	public class CpSysConfigure
    {
        /// <summary>
        /// user control settings(enable/disable) for hardwares.
        /// </summary>
        public CpHaConfigure HardwareActivation { get; set; }

        public CpSysConfigure(CpHaConfigure HaApp)
        {
            HardwareActivation = HaApp;
        }       
				
		static public CpSysConfigure loadCpSystemConfig(string fileNameWithPath)
        {
			XDocument xDoc = XDocument.Load(fileNameWithPath);
			XElement xelemAppRoot = xDoc.Element("ROOT").Element("SystemConfigure");

            CpSysConfigure systemConfigue = xmlLoadData(xelemAppRoot);
            if (systemConfigue == null)
            {
                UtilTextMessageBox.UIMessageBoxForWarning("System Configuration file loading Error", "Failed to load system config file [CPTesterSystemConfigue.xml].");
                return null;
            }
            return systemConfigue;
        }       
		
        // xml file control 
		static public CpSysConfigure xmlLoadData(XElement xElem)
        {
            // Hardware application configuration.
            CpHaConfigure HaApp = CpHaConfigure.xmlLoadData(xElem.Element("HaConfigure"));           
			if (HaApp == null)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to get configuration of 'HaConfigure'", ConsoleColor.Red);
                return null;
            }
            return new CpSysConfigure(HaApp);
        }
		
        public void xmlSaveData(/*XmlDocument xmlDoc, XmlNode xmlNodePrt*/)
        {
            TryAction(() =>
            {
                // root
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootNode = xmlDoc.CreateElement("ROOT");
                xmlDoc.AppendChild(rootNode);

                // sub-root
                XmlNode xmlSysConfigureNode = xmlDoc.CreateNode("element", "SystemConfigure", "");
                rootNode.AppendChild(xmlSysConfigureNode);

                XmlNode xmlLastBuildDate = xmlDoc.CreateNode("element", "LastBuildDate", "");
                DateTime dtLastBuild = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
                xmlLastBuildDate.InnerText = dtLastBuild.ToString(ClsGlobalStringForGeneral.FORMAT_DATE_TIME);
                xmlSysConfigureNode.AppendChild(xmlLastBuildDate);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                // Save the document to a file and auto-indent the output.
                XmlWriter writer = XmlWriter.Create(ClsGlobalStringForGeneral.FILE_SYSTEM_CONFIGUE_NAME, settings);
                xmlDoc.Save(writer);
            });
        }
    }
}
