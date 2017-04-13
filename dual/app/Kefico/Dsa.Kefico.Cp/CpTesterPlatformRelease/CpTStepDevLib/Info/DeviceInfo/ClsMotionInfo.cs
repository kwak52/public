using System.Xml;
using CpTesterPlatform.CpCommon;
using System;

namespace CpTesterPlatform.CpTStepDev
{
	public class ClsMotionInfo : ClsDeviceInfoBase 
	{
        public string AXIS_ID { set; get; } = "0";
        public string AXIS_DIRECTION { set; get; } = "CW";
        public string COMMENT { set; get; }
        public double CFG_ACC_RPM_RER_SEC { set; get; }
        public double CFG_DEC_RPM_RER_SEC { set; get; }
        public double CFG_MAX_RPM { set; get; }
        public double CFG_DISTANCE_PER_REVOLUTION { get; set; }
        public bool AXIS_ROBOT { get; set; }
        
        public ClsMotionInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");
            
            AXIS_ID = attNode.Attributes["AXIS_ID"].Value;
            AXIS_DIRECTION = attNode.Attributes["AXIS_DIRECTION"].Value;
            CFG_ACC_RPM_RER_SEC = Convert.ToDouble(attNode.Attributes["CFG_ACC_RPM_RER_SEC"].Value);
            CFG_DEC_RPM_RER_SEC = Convert.ToDouble(attNode.Attributes["CFG_DEC_RPM_RER_SEC"].Value);
            CFG_MAX_RPM = Convert.ToDouble(attNode.Attributes["CFG_MAX_RPM"].Value);
            CFG_DISTANCE_PER_REVOLUTION = Convert.ToDouble(attNode.Attributes["CFG_DISTANCE_PER_REVOLUTION"].Value);
            AXIS_ROBOT = Convert.ToBoolean(attNode.Attributes["AXIS_ROBOT"].Value); 
            COMMENT = attNode.Attributes["COMMENT"].Value;

        }
    }
}
