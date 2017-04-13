using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;
using PsCommon;
using PsCommon.Enum;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsMeasuringDevInfo
    {
        private double m_dVerticalRange = 0;
        public double VerticalRange
        {
            get { return m_dVerticalRange; }
            set { m_dVerticalRange = value; }
        }

        private double m_dTimeout = 0.0;
        public double Timeout
        {
            get { return m_dTimeout; }
            set { m_dTimeout = value; }
        }

        private double m_dIntegrationTime = 0; //DMM_APER_TIME : Integration time into the following integration times can be set
        public double IntegrationTime
        {
            get { return m_dIntegrationTime; }
            set { m_dIntegrationTime = value; }
        }

        private double m_dDelay = 0;
        public double Delay
        {
            get { return m_dDelay; }
            set { m_dDelay = value; }
        }

        public ClsMeasuringDevInfo()
        {
        }

        public ClsMeasuringDevInfo(double dVerticalRange, double dIntegrationTime, double dTimeout, double dDelay)
        {
            m_dVerticalRange = dVerticalRange;
            m_dIntegrationTime = dIntegrationTime;
            m_dTimeout = dTimeout;
            m_dDelay = dDelay;
        }
    }

    public class NIDevTriggerInfo // with DMM
    {
        private double m_dMaxLevel = 0.0;
        public double MaxLevel
        {
            get { return m_dMaxLevel; }
            set { m_dMaxLevel = value; }
        }

        private double m_dLevel = 0.0;
        public double Level
        {
            get { return m_dLevel; }
            set { m_dLevel = value; }
        }

        private double m_dTrgDelay = 0.0;
        public double TrgDelay
        {
            get { return m_dTrgDelay; }
            set { m_dTrgDelay = value; }
        }

        private eFLANKE m_eSlope = eFLANKE.P;
        public eFLANKE Slope
        {
            get { return m_eSlope; }
            set { m_eSlope = value; }
        }

        private string m_strTrgName = string.Empty;
        public string TrgName
        {
            get { return m_strTrgName; }
            set { m_strTrgName = value; }
        }

        private string m_strScanUnit = string.Empty;
        public string ScanUnit
        {
            get { return m_strScanUnit; }
            set { m_strScanUnit = value; }
        }

        private string m_strVxiDevice = string.Empty;
        public string VxiDevice
        {
            get { return m_strVxiDevice; }
            set { m_strVxiDevice = value; }
        }

        public static string getScannerUnitFromTrgInfo(Dictionary<string, NIDevTriggerInfo> dicTrgInfo, string strScanUnit)
        {
            var tResult = TryFunc(() =>
            {
                NIDevTriggerInfo tinfo = dicTrgInfo.Where(trginfo => trginfo.Value.m_strScanUnit == strScanUnit).FirstOrDefault().Value;

                if (tinfo.VxiDevice.Equals("TRIG_01") || tinfo.VxiDevice.Equals("DVM_01") || tinfo.VxiDevice.Equals("COUN_01"))                
                    return MESS_TYPE_ID.DVM_01_MESS.ToString();
                if (tinfo.VxiDevice.Equals("TRIG_02") || tinfo.VxiDevice.Equals("DVM_02") || tinfo.VxiDevice.Equals("COUN_02"))                
                    return MESS_TYPE_ID.DVM_02_MESS.ToString();

                Debug.Assert(false);
                throw new Exception("Unhandled case in getScannerUnitFromTrgInfo()");
            });
            return tResult.Succeeded ? tResult.Result : string.Empty;
        }

        public static NIDevTriggerInfo getTriggerInfoByTrgName(Dictionary<string, NIDevTriggerInfo> dicTrgInfo, string scanunit)
        {
            var tResult = TryFunc(() =>
            {
                NIDevTriggerInfo trginfo = dicTrgInfo.Where(trg => trg.Value.ScanUnit.Equals(scanunit)).FirstOrDefault().Value;
                return trginfo;
            });

            return tResult.Succeeded ? tResult.Result : null;
        }
    }
}
