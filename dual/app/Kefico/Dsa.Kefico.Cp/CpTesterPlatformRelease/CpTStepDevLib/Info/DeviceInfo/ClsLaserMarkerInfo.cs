using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
	public class ClsLaserMarkerInfo : ClsDeviceInfoBase 
	{
		public ClsLaserMarkerInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
        }
	}
}
