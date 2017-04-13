using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpUtility.TextString;
using System.Xml;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsDMMInfo : ClsDeviceInfoBase
    {
        public ClsDMMInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
        }

        // DMM info on NI side
        public class NIDMMPropertiesInfo : ClsMeasuringDevInfo
        {
            private int m_nFetchWaveformPoints = 100;
            public int FetchWaveformPoints
            {
                get { return m_nFetchWaveformPoints; }
                set { m_nFetchWaveformPoints = value; }
            }

            private int m_nPowerlineFrequency = 60; //Hz
            public int PowerlineFrequency
            {
                get { return m_nPowerlineFrequency; }
                set { m_nPowerlineFrequency = value; }
            }

            private double m_dSampleRate = 0.0;
            public double SampleRate
            {
                get { return m_dSampleRate; }
                set { m_dSampleRate = value; }
            }

            private double m_dAcqusitionTime = 0;
            public double AcqusitionTime
            {
                get { return m_dAcqusitionTime; }
                set { m_dAcqusitionTime = value; }
            }

            private int m_nMaxSampleNumberOnBuffer = 0;
            public int MaxSampleNumberOnBuffer
            {
                get { return m_nMaxSampleNumberOnBuffer; }
                set { m_nMaxSampleNumberOnBuffer = value; }
            }

            private int m_nTimeOutCycle = 0;
            public int TimeOutCycle
            {
                get { return m_nTimeOutCycle; }
                set { m_nTimeOutCycle = value; }
            }

            public NIDMMPropertiesInfo()
                : base()
            {
            }

            public NIDMMPropertiesInfo(double dRange, double dIntegrationTime, double dTimeOut,
                                       double dSampleRate,
                                       int nFetchWaveformPoints,
                                       double dDelayTime = 0.0, int nPowerlineFrequency = 60)
                : base(dRange, dIntegrationTime, dTimeOut, dDelayTime)
            {   
                m_nPowerlineFrequency = nPowerlineFrequency;
                m_dSampleRate = dSampleRate;
                m_nFetchWaveformPoints = nFetchWaveformPoints;

                m_dAcqusitionTime = m_dSampleRate / nFetchWaveformPoints;
                if (dTimeOut != 0)
                {
                    m_nMaxSampleNumberOnBuffer = Convert.ToInt32(dSampleRate / dTimeOut);
                    m_nTimeOutCycle = Convert.ToInt32(m_nMaxSampleNumberOnBuffer / nFetchWaveformPoints);
                }
            }

            public void printOutCnfInfo()
            {
                UtilTextMessageEdits.UtilTextMsgToConsole(
                   string.Format("DMM configure : Range = {0}, Sampling rate = {1}, waveform points = {2}, buffer point {3} : integration time(s) = {4}",
                   this.VerticalRange,
                   this.SampleRate,
                   this.FetchWaveformPoints,
                   this.MaxSampleNumberOnBuffer,
                   this.IntegrationTime));
            }
        }
    }
}
