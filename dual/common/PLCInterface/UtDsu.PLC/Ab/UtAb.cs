using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ControlLogixNET;
using Dsu.Common.Utilities.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dsu.PLC.AB;

namespace UtDsu.PLC
{
    [TestClass]
    public class UtAb
    {
        private AbConnection _connection;
        private LogixProcessor _processor => _connection.LogixProcessor;
        [TestInitialize]
        public void Initialize()
        {
            _connection = new AbConnection(new AbConnectionParameters("192.168.0.104"));
            _connection.Connect();
        }

        [TestMethod]
        public void TestMethodAbListAllTags()
        {
            List<LogixTagInfo> tagInfo = _processor.EnumerateTags();
            foreach (var ti in tagInfo)
                Trace.WriteLine(ti);
        }

        [TestMethod]
        public void TestMethodAbUserData()
        {
            Trace.WriteLine("::User data");
            Trace.WriteLine(_processor.UserData);
            Trace.WriteLine("--");
        }


        private IEnumerable<string> GenerateTagNames()
        {
            yield return "TIMER";
            yield return "SINT";
            yield break;


            //yield return "SAFETY_MAT";
            //yield return "STRING";              // null??
            yield return "SINT";
            //yield return "Task:SafetyTask";         // null?
            yield return "DINT";
            ////yield return "Map:AB_TEST_Packet:Partner";
            yield return "CCCCCCCC";
            //yield return "AUX_VALVE_CONTROL";
            yield return "A";
            yield return "AAAAAAAA";
            //yield return "TIMER";
            //yield return "T1";
            yield return "INT";
            //yield return "COUNTER";   // null??
            yield return "ARRAY3";
            yield return "BOOL";
            //yield return "T2";
            //yield return "Program:MainProgram";
            //yield return "ALARM";
            //yield return "AXIS_GENERIC";
            //yield return "ARRAY_BOOL";        // null
            //yield return "ALARM_DIGITAL";
            yield return "BBBBBBBB";
            //yield return "CC";
            //yield return "CAM";
            yield return "ARRAY1";
            //yield return "Program:SafetyProgram";
            yield return "S";
            yield return "REAL";
            //yield return "AXIS_GENERIC_DRIVE";
            //yield return "Map:Local";
            //yield return "Task:MainTask";


            yield return "LINT";
            yield return "DDDDDDD";
            //yield return "Map:AHN_Ethernet";
            yield return "ARRAY2";
            //yield return "ALARM_ANALOG";
            //yield return "AXIS_CONSUMED";
            yield break;

            //return _processor.EnumerateTags()
            //        .Select(t => t.TagName)
            //        //.Where(t => !t.Contains(":"))
            //    ;
        }

        private IEnumerable<AbTag> GenerateTags() => _connection.CreateTags(GenerateTagNames()).OfType<AbTag>();

        [TestMethod]
        public void TestMethodReadWriteAB()
        {
            var tags = GenerateTags();
            _connection.SingleScan(prepare: true);
            tags.ForEach(t =>
            {
                Trace.WriteLine($"{t.Name} = {t.Value}");
            });
        }
    }
}
