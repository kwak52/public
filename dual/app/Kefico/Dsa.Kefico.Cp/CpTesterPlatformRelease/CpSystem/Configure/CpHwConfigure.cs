using System;
using System.IO;
using System.Reflection;
using System.Xml;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpSystem.Configure
{
    public class CpHwConfigure
    {
        public string Version { get; set; } = string.Empty;
        public DateTime LastBuild { get; set; } = new DateTime();
        public CpDevConfigure DevConfigue { get; private set; } = null;
        CpHwConfigure(CpDevConfigure devConfigue)
        {
            DevConfigue = devConfigue;
        }
        
        // xml file control 
        static public CpHwConfigure xmlLoadData(XmlDocument xmlDoc, string strConfigueNode)
        {
            var tResult = TryFunc(() =>
            {
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strConfigueNode);
				                               
                CpDevConfigure hwResultConfigue = CpDevConfigure.xmlLoadData(xmlNode, strConfigueNode);				
				
                return new CpHwConfigure(hwResultConfigue);
            });
            return tResult.Succeeded ? tResult.Result : null;
        }

        public void xmlSaveData(/*XmlDocument xmlDoc, XmlNode xmlNodePrt*/)
        {
            TryAction(() =>
            {
                /// root
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootNode = xmlDoc.CreateElement("ROOT");
                xmlDoc.AppendChild(rootNode);

                /// sub-root
                XmlNode xmlHwConfigureNode = xmlDoc.CreateNode("element", "HwConfigure", "");
                rootNode.AppendChild(xmlHwConfigureNode);

                /// items
                XmlNode xmlSystemVersion = xmlDoc.CreateNode("element", "HardwareVersion", "");
                xmlSystemVersion.InnerText = Version;
                xmlHwConfigureNode.AppendChild(xmlSystemVersion);

                XmlNode xmlLastBuildDate = xmlDoc.CreateNode("element", "LastBuildDate", "");
                DateTime dtLastBuild = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
                xmlLastBuildDate.InnerText = dtLastBuild.ToString(ClsGlobalStringForGeneral.FORMAT_DATE_TIME);
                xmlHwConfigureNode.AppendChild(xmlLastBuildDate);

                /// Device configuration.
                XmlNode xmlDevConfigueNode = xmlDoc.CreateElement("Devices");
                xmlHwConfigureNode.AppendChild(xmlDevConfigueNode);
                DevConfigue.xmlSaveData(xmlDoc, ref xmlDevConfigueNode);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                /// Save the document to a file and auto-indent the output.
                XmlWriter writer = XmlWriter.Create(ClsGlobalStringForGeneral.FILE_HARDWARE_CONFIGUE_NAME, settings);
                xmlDoc.Save(writer);
            });
        }
    }
   
}
