using System;
using System.Collections.Generic;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpUtility.TextString;
using System.Xml;
using System.Diagnostics;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsLoadInfo : ClsDeviceInfoBase
    {
        public int BoardID { set; get; } = 0;
        public CpInstLoadBoardType LoadModuleName { get; private set; } = CpInstLoadBoardType.NONE;
        public ClsShuntPinInfo PinInfo { get; private set; } = null;
        public CpHwMitechBoardType BoardType { set; get; } = CpHwMitechBoardType.NONE;
        public Dictionary<string, int> DicLoadUnit { get; set; } = null;

        public ClsLoadInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");

            BoardType = (CpHwMitechBoardType)Enum.Parse(typeof(CpHwMitechBoardType), node["Type"].InnerText);
            BoardID = Convert.ToInt16(attNode.Attributes["IPADDR"].Value);

            DicLoadUnit = new Dictionary<string, int>();
            PinInfo = new ClsShuntPinInfo();

            foreach (XmlNode inode in node.SelectSingleNode("Units").ChildNodes)
            {
                DicLoadUnit.Add(inode.Attributes["HARDWAREUNIT"].Value, Convert.ToInt32(inode.Attributes["CHNUM"].Value));
            }

            LoadModuleName = (CpInstLoadBoardType)Enum.Parse(typeof(CpInstLoadBoardType), node["HwName"].InnerText);

            if (BoardType != CpHwMitechBoardType.KAM)
                PinInfo = new ClsShuntPinInfo() { PositivePin = int.Parse(attNode.Attributes["SHUNT_P"].Value), NegativePin = int.Parse(attNode.Attributes["SHUNT_N"].Value) };
        }
    }

    public class ClsShuntPinInfo
    {
        private int m_nPositivePin = 0;
        public int PositivePin
        {
            get { return m_nPositivePin; }
            set { m_nPositivePin = value; }
        }
        private int m_nNegativePin = 0;
        public int NegativePin
        {
            get { return m_nNegativePin; }
            set { m_nNegativePin = value; }
        }
    }

    public class ClsLoadUdpDataInfo
    {
        private CpInstBoardDataType m_eCommandType = CpInstBoardDataType.NONE;
        public CpInstBoardDataType CommandType
        {
            get { return m_eCommandType; }
            set { m_eCommandType = value; }
        }

        private byte m_bDstPortNum = 0x0;
        public byte DstPortNum
        {
            get { return m_bDstPortNum; }
            set { m_bDstPortNum = value; }
        }

        private byte m_bChannelNum = 0x0;
        public byte ChannelNum
        {
            get { return m_bChannelNum; }
            set { m_bChannelNum = value; }
        }

        private byte m_bLoadValue = 0x0;
        public byte LoadValue
        {
            get { return m_bLoadValue; }
            set { m_bLoadValue = value; }
        }

        private byte m_bSrcPortNum = 0x0;
        public byte SrcPortNum
        {
            get { return m_bSrcPortNum; }
            set { m_bSrcPortNum = value; }
        }

        public static string getStringFromDataBytes(ClsLoadUdpDataInfo clsData)
        {
            List<byte> lstBytes = new List<byte>();
            lstBytes.Add(clsData.DstPortNum);
            lstBytes.Add(clsData.ChannelNum);
            lstBytes.Add(clsData.LoadValue);
            lstBytes.Add(clsData.SrcPortNum);

            return UtilTextConverter.convByteArryToHexString(lstBytes.ToArray());
        }

        private int m_nBoardID = 0;
        public int BoardID
        {
            get { return m_nBoardID; }
            set { m_nBoardID = value; }
        }
    }

    public class ClsLoadDataInfo
    {
        private CpInstLoadHardwareUnit m_eLoadUnit = CpInstLoadHardwareUnit.NONE;
        public CpInstLoadHardwareUnit LoadUnit
        {
            get { return m_eLoadUnit; }
            set { m_eLoadUnit = value; }
        }

        private CpInstBoardCommandType m_eCommandType = CpInstBoardCommandType.NONE;
        public CpInstBoardCommandType CommandType
        {
            get { return m_eCommandType; }
            set { m_eCommandType = value; }
        }

        private CpInstLoadConnectPinName m_ePortDstName = CpInstLoadConnectPinName.b00;
        public CpInstLoadConnectPinName PortDstName
        {
            get { return m_ePortDstName; }
            set { m_ePortDstName = value; }
        }

        private CpInstLoadValue m_eLoadValue = CpInstLoadValue.NONE;
        public CpInstLoadValue LoadValue
        {
            get { return m_eLoadValue; }
            set { m_eLoadValue = value; }
        }

        private CpInstLoadConnectPinName m_ePortSrcName = CpInstLoadConnectPinName.b00;
        public CpInstLoadConnectPinName PortSrcName
        {
            get { return m_ePortSrcName; }
            set { m_ePortSrcName = value; }
        }

        public string getLoadDataString(int nStep)
        {
            return string.Format("[{0}]CT:{1}, UNIT:{2}, DST:{3}, SRC:{4}, L:{5}",nStep ,CommandType.ToString(), m_eLoadUnit.ToString(), m_ePortDstName.ToString(), m_ePortSrcName.ToString(), m_eLoadValue.ToString());
        }
    }
     
    public class ClsLoadUnitInfo
    {
        private CpInstLoadHardwareUnit m_eLoadUnit = CpInstLoadHardwareUnit.NONE;
        public CpInstLoadHardwareUnit LoadUnit
        {
            get { return m_eLoadUnit; }
            set { m_eLoadUnit = value; }
        }

        private int m_nGroupID = 0; // same as nBoardIP
        public int GroupID
        {
            get { return m_nGroupID; }
            set { m_nGroupID = value; }
        }

        private string m_strChannelName = string.Empty;
        public string ChannelName
        {
            get { return m_strChannelName; }
            set { m_strChannelName = value; }
        }
    }

    public class ClsLoadConnectPinMap
    {
        private List<CpInstLoadValue> m_eLoadValueFamlily = new List<CpInstLoadValue>();
        public List<CpInstLoadValue> LoadValueFamlily
        {
            get { return m_eLoadValueFamlily; }
            set { m_eLoadValueFamlily = value; }
        }
        
        private List<ClsLoadConnectPinItem> m_lstPinMap = new List<ClsLoadConnectPinItem>();
        public List<ClsLoadConnectPinItem> LstPinMap
        {
            get { return m_lstPinMap; }
            set { m_lstPinMap = value; }
        }
    }

    public class ClsLoadConnectPinItem
    {
        private CpInstLoadConnectPinName m_eLoadPinName = CpInstLoadConnectPinName.b00;
        public CpInstLoadConnectPinName LoadPinName
        {
            get { return m_eLoadPinName; }
            set { m_eLoadPinName = value; }
        }
        
        private CpInstLoadConnectPortName m_ePortNumber = CpInstLoadConnectPortName.NONE;
        public CpInstLoadConnectPortName PortNumber
        {
            get { return m_ePortNumber; }
            set { m_ePortNumber = value; }
        }
    }
}
