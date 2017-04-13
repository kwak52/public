using System.Diagnostics;
using CpTesterPlatform.CpCommon;
using PsCommon;
using PsKGaudi.Parser;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsStimuliInfo
    {

        private CpDeviceType m_eHwType = CpDeviceType.NONE;
        public CpDeviceType HwType
        {
            get { return m_eHwType; }
            set { m_eHwType = value; }
        }

        private string m_strUnitName = string.Empty;
        public string UnitName
        {
            get { return m_strUnitName; }
            set { m_strUnitName = value; }
        }
        private string m_strPinAlias = string.Empty;
        public string PinAlias
        {
            get { return m_strPinAlias; }
            set { m_strPinAlias = value; }
        }
        private string m_strDevice_ID = string.Empty;
        public string Device_ID
        {
            get { return m_strDevice_ID; }
            set { m_strDevice_ID = value; }
        }
        private int m_nChannelNum = 0;
        public int ChannelNum
        {
            get { return m_nChannelNum; }
            set { m_nChannelNum = value; }
        }
        private double m_dValue = 0.0;
        public double Value
        {
            get { return m_dValue; }
            set { m_dValue = value; }
        }
        private CpSpecDimension m_eDimension = CpSpecDimension.NONE;
        public CpSpecDimension Dimension
        {
            get { return m_eDimension; }
            set { m_eDimension = value; }
        }

        public static int GetDeviceNumber(ClsStimuliInfo info)
        {
            if (info.ChannelNum == 1)
            {
                return 0;
            }
            else if(info.ChannelNum == 2)
            {
                return 0;
            }
            else
            {
                Debug.Assert(false);
            }

            return 0;
        }

        public static string GetChannelNumber(ClsStimuliInfo info, int nTotalChannel)
        {
            return (info.ChannelNum - 1).ToString();
        }
    }
    
}
