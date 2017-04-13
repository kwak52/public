using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using System.Xml;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsRS232Info : ClsDeviceInfoBase
    {
		public int BAUDRATE { set; get; } = 9600;
		public int DATABITS { set; get; } = 8080;
		public bool CR { set; get; } = true;
		public bool LF { set; get; } = true;
		public int TIMEOUT { set; get; } = 500;

        public ClsRS232Info(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {		
            XmlNode attNode = node.SelectSingleNode("Attributes");

			BAUDRATE = Convert.ToInt32(attNode.Attributes["BAUDRATE"].Value);
			DATABITS = Convert.ToInt32(attNode.Attributes["DATABITS"].Value);
			CR = Convert.ToBoolean(attNode.Attributes["CR"].Value);
			LF = Convert.ToBoolean(attNode.Attributes["LF"].Value);
			TIMEOUT = Convert.ToInt32(attNode.Attributes["TIMEOUT"].Value);
        }
    }
}
