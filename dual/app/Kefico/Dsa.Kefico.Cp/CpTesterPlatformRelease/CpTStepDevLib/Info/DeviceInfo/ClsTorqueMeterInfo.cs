using System;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
	public class ClsTorqueMeterInfo : ClsDeviceInfoBase
	{
		public ClsTorqueMeterInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
		}	
	}
}