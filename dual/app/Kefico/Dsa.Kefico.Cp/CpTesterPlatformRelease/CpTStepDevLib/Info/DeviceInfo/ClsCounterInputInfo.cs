using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsCounterInputInfo : ClsDeviceInfoBase
    {
		public double MinimumValue { set; get; } = 0.0;
		public double MaximumValue { set; get; } = 1.0;			
		public double MinimumFrequency { set; get; } 
		public double MaximumFrequency { set; get; } 	
		public string InputSrc { set; get; } = string.Empty;

        public ClsCounterInputInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {	
			XmlNode attNode = node.SelectSingleNode("Attributes");

			MinimumValue = Convert.ToDouble(attNode.Attributes["MIN_VAL"].Value);
			MaximumValue = Convert.ToDouble(attNode.Attributes["MAX_VAL"].Value);
			MinimumFrequency = Convert.ToDouble(attNode.Attributes["MIN_FREQ"].Value);
			MaximumFrequency = Convert.ToDouble(attNode.Attributes["MAX_FREQ"].Value);
			InputSrc = attNode.Attributes["EDGE_CHANNEL"].Value;
        }
    }
}
