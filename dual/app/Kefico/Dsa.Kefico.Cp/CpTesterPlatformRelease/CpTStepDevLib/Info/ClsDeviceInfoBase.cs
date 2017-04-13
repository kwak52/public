using System;
using System.Collections.Generic;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsDeviceInfoBase
    {
        public CpDeviceType DeviceType { get; private set; } = CpDeviceType.NONE;

        public string Device_ID { get; private set; } = string.Empty;

        public string HwName { get; private set; } = string.Empty;

		public string CommDevID { get; private set; } = string.Empty;

        public string DllName { get; private set; } = string.Empty;

        public bool VirtualDevice { get; private set; } = false;

        public ClsDeviceInfoBase(CpDeviceType insttype, XmlNode node = null)
        {
            if (node != null)
            {
                DeviceType = insttype;
                Device_ID = node["ID"].InnerText;
                HwName = node["HwName"].InnerText;
                DllName = node["DllName"].InnerText;				
				
				if(Enum.IsDefined(typeof(CpCommDeviceType), DeviceType.ToString()) == false)
				{
					VirtualDevice = Convert.ToBoolean(node["Virtual"].InnerText);
					CommDevID = node["CommDevID"].InnerText;
				}
					
            }
        }
    }
}
