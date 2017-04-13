using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Common;
using Dsu.PLC.Fuji;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UtDsu.PLC.Fj
{
    [TestClass]
    public class UtFj
    {
        private FjConnection _connection;
        private FjProtocol Protocol => _connection.FjProtocol;

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void Initialize()
        {
            try
            {
                _connection = new FjConnection(new FjConnectionParameters("../ioarea-sample.ini", "192.168.0.103"));
                _connection.Connect();
            }
            catch (Exception)
            {
                _connection.Dispose();
            }
        }

        [TestCleanup()]
        public void CleanUp()
        {
            _connection.Dispose();
        }

        private FjPacketResponse Read(MemoryType type, uint startAddress, ushort requestByteLength, bool check = true)
        {
            var request = Protocol.GetReadRequestPacket(type, startAddress, requestByteLength).ToArray();
            var responsePacket = Protocol.Execute(request);
            var response = FjPacketResponse.Analyze(responsePacket, request);

            if (check)
            {
                Assert.IsTrue(response.IsOK);
                Assert.IsTrue(response.OperationStatus == OperationStatusType.Success);
                Assert.IsTrue(response.CommandType == CommandType.Read);
                Assert.IsTrue(response.MemoryType == type);
                Assert.IsTrue(response.ModeType == ModeType.Read);
                Assert.IsTrue(response.Data.Length == requestByteLength);
            }

            return response;

        }

        [TestMethod]
        public void UtFjReadIO()
        {
            ushort readWordLength = 254;
            var response = Read(MemoryType.InputOutputMemory, 0, readWordLength);
            Trace.WriteLine("Connecting...");
        }

        [TestMethod]
        public void UtFjReadStandard()
        {
            ushort readWordLength = 16;
            var response = Read(MemoryType.StandardMemory, 0, readWordLength);

            Trace.WriteLine("Connecting...");
        }



        /// <summary>
        /// 254 Words(=508 bytes) 까지는 OK.   255 Word 부터 crash.
        /// </summary>
        [TestMethod]
        public void UtFjLimitReadIO()
        {
			EmLinq.MinMaxRange(500, 2, 508).ForEach(n =>
			{
				var res = Read(MemoryType.InputOutputMemory, 0, (ushort)n);
				Assert.IsTrue(res.IsOK);
				Trace.WriteLine($"{n} bytes read: succeeded.");
			});

            ushort readByteLength = 512;
            var response = Read(MemoryType.InputOutputMemory, 0, readByteLength, check: false);
            Assert.IsFalse(response.IsOK);

            Trace.WriteLine("Connecting...");
        }

		/// <summary>
		/// 254 Words(=508 bytes) 까지는 OK.   255 Word 부터 crash.
		/// </summary>
		[TestMethod]
        public void UtFjLimitReadStandard()
        {
	        EmLinq.MinMaxRange(500, 2, 508).ForEach(n =>
	        {
		        var res = Read(MemoryType.StandardMemory, 0, (ushort)n);
		        Assert.IsTrue(res.IsOK);
		        Trace.WriteLine($"{n} bytes read: succeeded.");
	        });

			ushort readByteLength = 512;
			var response = Read(MemoryType.StandardMemory, 0, readByteLength, check: false);
            Assert.IsFalse(response.IsOK);

            Trace.WriteLine("Connecting...");
        }

        [TestMethod]
        public void UtFjParse()
        {
            var tagMX = _connection.CreateTag("%MX01.10.3.1") as FjTagSRS;
            Assert.IsTrue(tagMX is FjTagSRS);
            Assert.IsTrue(tagMX.IsBitAddress);
            Assert.IsTrue(tagMX.CpuNumber == 1);
            Assert.IsTrue(tagMX.ByteOffset == 6);
            Assert.IsTrue(tagMX.BitOffset.IsSome);
            Assert.IsTrue(tagMX.BitOffset == 1);

            var tagMW = _connection.CreateTag("%MW01.3.3") as FjTagSRS;
            Assert.IsTrue(tagMW is FjTagSRS);
            Assert.IsFalse(tagMW.IsBitAddress);
            Assert.IsTrue(tagMW.CpuNumber == 1);
            Assert.IsTrue(tagMW.ByteOffset == 6);
            Assert.IsTrue(tagMW.BitOffset.IsNone);


            var tags = new[] {"%MX01.3.3.1", /*"%MW01.3.3",*/ "%IW07.14", "%IX07.14.2", "%IW07.14.2"};
            foreach (var t in tags)
            {
                var mytag = _connection.CreateTag(t);
                Trace.WriteLine(mytag);
            }

            Trace.WriteLine("Done.");

        }

        [TestMethod]
        public void UtFjTagParseInvalidFormat()
        {
            var tags = new[]
            {
                "%MW01.2.3.1",      // MW{X}.{Y}.{Z} at most.
                "%MW13.2.3",        // 13 not allowed. [0..7]
                "%MW7.2.3",         // 2 not allowed. {1, 3, 10}
                "%MX7.1.2.16",      // 16 not allowed. [0..15]
            };

            foreach (var t in tags)
            {
                Exception exception = null;
                try
                {
                    var tag = _connection.CreateTag(t);
                    Trace.WriteLine(tag);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Trace.WriteLine($"{t}: {ex.Message}");
                }

                Assert.IsTrue(exception != null);
				Assert.IsTrue(exception is PlcExceptionTag || exception.GetType().Name == "ContractException");
            }

            Trace.WriteLine("Done.");
        }



        private IEnumerable<string> GenerateTagNames()
        {
//             int size = 1;
//             foreach (var n in EmLinq.HexRange(0x2000 - 32 * size, 32 * size))
//                 yield return $"X{n}";
//             foreach (var n in Enumerable.Range(0, 32 * size))
//                 yield return $"M{n}";



            yield return "%MX1.1.0";
            yield return "%MX1.2.0";

            yield return "%IX3.0.1";
            yield return "%IX3.0.2";
            yield return "%IX3.0.3";
            yield return "%IX3.0.4";
            yield return "%IX3.0.5";
            yield return "%IX3.0.6";

            yield return "%IX6.0.0.1";
            yield return "%IX6.0.0.2";
            yield return "%IX6.0.0.3";
            yield return "%IX6.0.0.4";
            yield return "%IX6.0.0.5";
            yield return "%IX6.0.0.6";


            foreach (var ioName in _connection.EnumerateValidInputOutputTags())
                yield return ioName;
        }

        private IEnumerable<FjTag> GenerateTags() => _connection.CreateTags(GenerateTagNames()).OfType<FjTag>();


        private IEnumerable<FjTagIO> CreateValidInputOutputTags()
	    {
			var availableTagNames = _connection.EnumerateValidInputOutputTags();
			return availableTagNames.Select(t => _connection.CreateTag(t)).OfType<FjTagIO>();
		}

		[TestMethod]
	    public void UtFjTagEnumerateValidTags()
	    {
		    _connection.EnumerateValidInputOutputTags().ForEach(t =>
		    {
			    Trace.WriteLine(t);
		    });
	    }



        [TestMethod]
        public void UtFjTagChannelize()
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
