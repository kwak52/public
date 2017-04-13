using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using CpTesterPlatform.CpCommon;
using System.Xml;
using PsCommon;
//using NationalInstruments.ModularInstruments.NIScope;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsOscilloInfo : ClsDeviceInfoBase
    {
        public ClsOscilloInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            ChannelNum = node.Attributes["CHNUM"].Value;
            ProbeDivider = Convert.ToInt32(node.Attributes["PROBEDIV"].Value);
        }

        public int ProbeDivider { private set; get; } = 1;
        public string ChannelNum { private set; get; } = string.Empty;
    }

    // Scope & DMM info on NI side    
    public class NIScopePropertiesInfo : ClsMeasuringDevInfo
    {
        //Configure the vertical parameters.
        //private ScopeVerticalCoupling m_eVerticalCoupling = ScopeVerticalCoupling.DC;
        /*public NationalInstruments.ModularInstruments.NIScope.ScopeVerticalCoupling VerticalCoupling
        {
            get { return m_eVerticalCoupling; }
            set { m_eVerticalCoupling = value; }
        }*/

        private double m_dVerticalOffset = 0.0;
        public double VerticalOffset
        {
            get { return m_dVerticalOffset; }
            set { m_dVerticalOffset = value; }
        }

        private double m_dProbeAttenuation = 0.0;
        public double ProbeAttenuation
        {
            get { return m_dProbeAttenuation; }
            set { m_dProbeAttenuation = value; }
        }

        // Configure the horizontal parameters.
        private double m_dSampleRateMin = 0;
        public double SampleRateMin
        {
            get { return m_dSampleRateMin; }
            set { m_dSampleRateMin = value; }
        }

        private int m_nRecordLengthMin = 0;
        public int RecordLengthMin
        {
            get { return m_nRecordLengthMin; }
            set { m_nRecordLengthMin = value; }
        }

        private double m_dReferencePosition = 50.0;
        public double ReferencePosition
        {
            get { return m_dReferencePosition; }
            set { m_dReferencePosition = value; }
        }

        private int m_nNumberOfRecords = 0;
        public int NumberOfRecords
        {
            get { return m_nNumberOfRecords; }
            set { m_nNumberOfRecords = value; }
        }

        private bool m_bEnforceRealTime = true;
        public bool EnforceRealTime
        {
            get { return m_bEnforceRealTime; }
            set { m_bEnforceRealTime = value; }
        }

        // Configure the measuring parameters.
        private long m_lRecordLength = 0;
        public long RecordLength
        {
            get { return m_lRecordLength; }
            set { m_lRecordLength = value; }
        }

        private bool m_bAutoRange = false;
        public bool AutoRange
        {
            get { return m_bAutoRange; }
            set { m_bAutoRange = value; }
        }

        //Configure the Trigger
       /* private ScopeTriggerType m_eTriggerType = ScopeTriggerType.Immediate;
        public NationalInstruments.ModularInstruments.NIScope.ScopeTriggerType TriggerType
        {
            get { return m_eTriggerType; }
            set { m_eTriggerType = value; }
        }
		*/
        public NIScopePropertiesInfo()
            : base()
        {
        }

        public NIScopePropertiesInfo(double dVerticalRange, double dIntegrationTime, double dTimeout, double dDelay)
            : base(dVerticalRange, dIntegrationTime, dTimeout, dDelay)
        {
        }
    }

    public class ClsRefTIMEAInfo
    {
        private NIScopePropertiesInfo m_clsScopeInfo = null;
        public NIScopePropertiesInfo ClsScopeInfo
        {
            get { return m_clsScopeInfo; }
            set { m_clsScopeInfo = value; }
        }

        private NIDevTriggerInfo m_clsTriggerStartInfo = null;
        public NIDevTriggerInfo ClsTriggerStartInfo
        {
            get { return m_clsTriggerStartInfo; }
            set { m_clsTriggerStartInfo = value; }
        }        

        private NIDevTriggerInfo m_clsTriggerStopInfo = null;
        public NIDevTriggerInfo ClsTriggerStopInfo
        {
            get { return m_clsTriggerStopInfo; }
            set { m_clsTriggerStopInfo = value; }
        }        

        private CpSpecDimension m_eDimension = CpSpecDimension.S;
        public CpSpecDimension Dimension
        {
            get { return m_eDimension; }
            set { m_eDimension = value; }
        }
    }
}
