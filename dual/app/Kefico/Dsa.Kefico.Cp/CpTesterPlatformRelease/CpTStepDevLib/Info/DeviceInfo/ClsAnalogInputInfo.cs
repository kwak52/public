using System;
using System.Xml;
using CpTesterPlatform.CpCommon;
using static CpCommon.ExceptionHandler;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Linq;
using System.Text.RegularExpressions;
using static Dsu.Driver.NiDaqMcAi;

namespace CpTesterPlatform.CpTStepDev
{
    public class ClsAnalogInputInfo : ClsDeviceInfoBase
    {
        public string AICoupling { get; set; } = "DC";
        public double LowpassCutoffFrequency { get; set; }
        public bool LowpassEnable { get; set; } = false;
        public double Max { get; set; } = 10;
        public double Min { get; set; } = -10;
        public string TerminalConfiguration { get; set; } = "Differential";
        public string COMMENT { get; set; }


        public ClsAnalogInputInfo(CpDeviceType insttype, XmlNode node)
            : base(insttype, node)
        {
            XmlNode attNode = node.SelectSingleNode("Attributes");

            COMMENT = attNode.Attributes["COMMENT"].Value;

            var attrVOffset = attNode.Attributes["VOLTAGE_OFFSET"];
            if (attrVOffset != null && attrVOffset.Value.NonNullAny())
            {
                var voltageOffset = Convert.ToDouble(attrVOffset.Value);
                var regex = Regex.Match(HwName, @"[^/].*/ai(\d+)", RegexOptions.IgnoreCase);
                var channelIndex = Int32.Parse(regex.Groups[1].ToString());
                DaqMcAiManager.SetChannelVoltageOffset(channelIndex, voltageOffset);
                System.Console.WriteLine("");
            }


            /*
            MinimumAIValue = Convert.ToDouble(attNode.Attributes["MIN_VAL"].Value);
            MaximumAIValue = Convert.ToDouble(attNode.Attributes["MAX_VAL"].Value);
            AITriggerSrc = attNode.Attributes["TRIGGER_SRC"].Value;
            AITriggerStartEdge = attNode.Attributes["TRIGGER_SRC"].Value;
            AITerminalCfg = attNode.Attributes["GROUND_TYPE"].Value;
            AISamplingRate = Convert.ToDouble(attNode.Attributes["SAMPLING_RATE"].Value);
            AISampleCount = Convert.ToInt32(attNode.Attributes["SAMPLE_COUNT"].Value);*/
        }
    }
}
