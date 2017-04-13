using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CpTesterPlatform.CpCommon;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsSwitchInfo : ClsDeviceInfoBase
    {
        public ClsSwitchInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");

            m_strTopologyName = attNode.Attributes["TOPOLOGY"].Value;
            m_strRowName = attNode.Attributes["ROWNAME"].Value;
            m_strRowMain = attNode.Attributes["ROWMAIN"].Value;
            m_strColName = attNode.Attributes["COLNAME"].Value;
            m_nCardNum = Convert.ToInt16(attNode.Attributes["CARDNUM"].Value);
            m_nColNum = Convert.ToInt16(attNode.Attributes["COLNUM"].Value);
            m_nRowNum = Convert.ToInt16(attNode.Attributes["ROWNUM"].Value);
            m_bMultiSwitch = Convert.ToBoolean(attNode.Attributes["MULTISWITCH"].Value);
        }

        private bool m_bMultiSwitch = false;
        public bool MultiSwitch
        {
            get { return m_bMultiSwitch; }
            set { m_bMultiSwitch = value; }
        }
        private string m_strTopologyName = string.Empty;
        public string TopologyName
        {
            get { return m_strTopologyName; }
            set { m_strTopologyName = value; }
        }

        private string m_strColName = string.Empty;
        public string ColName
        {
            get { return m_strColName; }
            set { m_strColName = value; }
        }

        private string m_strRowName = string.Empty;
        public string RowName
        {
            get { return m_strRowName; }
            set { m_strRowName = value; }
        }

        private string m_strRowMain = string.Empty;
        public string RowMain
        {
            get { return m_strRowMain; }
            set { m_strRowMain = value; }
        }

        private int m_nCardNum = 0;
        public int CardNum
        {
            get { return m_nCardNum; }
            set { m_nCardNum = value; }
        }

        private int m_nColNum = 0;
        public int ColNum
        {
            get { return m_nColNum; }
            set { m_nColNum = value; }
        }

        private int m_nRowNum = 0;
        public int RowNum
        {
            get { return m_nRowNum; }
            set { m_nRowNum = value; }
        }
    }

    public class ClsConnectionInfo
    {
        private string m_strRowName = string.Empty;
        public string RowName
        {
            get { return m_strRowName; }
            set { m_strRowName = value; }
        }
        private string m_strColName = string.Empty;
        public string ColName
        {
            get { return m_strColName; }
            set { m_strColName = value; }
        }
        private CpInstSwitchType m_eSwitchType = CpInstSwitchType.SK_SWITCH;
        public CpInstSwitchType SwitchType
        {
            get { return m_eSwitchType; }
            set { m_eSwitchType = value; }
        }
        private bool m_bConnect = false;
        public bool Connect
        {
            get { return m_bConnect; }
            set { m_bConnect = value; }
        }
        private int m_nSwitchNum = 0;
        public int SwitchNum
        {
            get { return m_nSwitchNum; }
            set { m_nSwitchNum = value; }
        }
    }
}
