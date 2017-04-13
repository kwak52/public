using System;
using System.Collections.Generic;
using Dsu.PLC.Melsec;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLC.Utilities;
using static LanguageExt.Prelude;

namespace UtDsu.PLC
{
    [TestClass]
    public partial class UtMx
    {
        private MxConnection _connection;

        private McProtocolApp McProtocol
        {
            get { return _connection.McProtocol; }
        }


        [TestInitialize]
        public void Initialize()
        {
            _connection = new MxConnection(new MxConnectionParameters("192.168.0.10", 10001, Dsu.PLC.TransportProtocol.Tcp));
            _connection.Connect();
        }

	    [TestMethod]
	    public void UtMxTagCheckDeviceType()
	    {
			var tagM1111 = MxTag.Create(_connection, "M1111");
			Assert.IsTrue(tagM1111.IsBitDevice);

			var tagD0 = MxTag.Create(_connection, "D0");
			Assert.IsFalse(tagD0.IsBitDevice);
		    Assert.IsTrue(tagD0.ByteOffset.GetValueUnsafe() == 0);

			var tagD1 = MxTag.Create(_connection, "D1");
			Assert.IsFalse(tagD1.IsBitDevice);
			Assert.IsTrue(tagD1.ByteOffset.GetValueUnsafe() == 2);

			var tagD2 = MxTag.Create(_connection, "D2");
			Assert.IsFalse(tagD2.IsBitDevice);
			Assert.IsTrue(tagD2.ByteOffset.GetValueUnsafe() == 4);
		}

	    [TestMethod]
	    public void UtMxTagCreate()
	    {
		    var tagNames = new[]
		    {
			    "B", "M", "X", "Y", "L", "F", "V", "W", "R",
				"D", "DX", "DY",
				// "S", "T", "C", "Z"
				"ZR",
				//"FX", "FY", "FD", 
				"SM", "SD", "SB", "SW", "SS", "SC", "SN",	// "ST", 
			};
		    foreach (var tagName in tagNames)
		    {
			    var tag = MxTag.Create(_connection, $"{tagName}0");
				Assert.IsTrue(tag.ByteOffset.IsSome);
				Assert.IsTrue(tag.ByteOffset.GetValueUnsafe() == 0);
				Trace.WriteLine($"Tag {tag} has type {tag.DeviceType}");
			}
		}

		[TestMethod]
        public void UtMxTagDeviceDesignation()
        {
            var tagM1111 = MxTag.Create(_connection, "M1111");				// 1111 = 0x0457
            var designation = tagM1111.GetDeviceDesignation();
            Assert.IsTrue(designation[0] == 0x57);
            Assert.IsTrue(designation[1] == 0x04);
            Assert.IsTrue(designation[2] == 0x00);
            Assert.IsTrue(designation[3] == (byte) PlcDeviceType.M);

            var tagD0 = MxTag.Create(_connection, "D0");
            designation = tagD0.GetDeviceDesignation();
            Assert.IsTrue(designation[0] == 0x00);
            Assert.IsTrue(designation[1] == 0x00);
            Assert.IsTrue(designation[2] == 0x00);
            Assert.IsTrue(designation[3] == (byte) PlcDeviceType.D);

            var tagY160 = MxTag.Create(_connection, "Y160");
            designation = tagY160.GetDeviceDesignation();
            Assert.IsTrue(designation[0] == 0x60);
            Assert.IsTrue(designation[1] == 0x01);
            Assert.IsTrue(designation[2] == 0x00);
            Assert.IsTrue(designation[3] == (byte) PlcDeviceType.Y);


	        foreach (var x in EmLinq.HexRange(0, 17).Select(n => "X" + n))
	        {
		        var desig = MxTag.Create(_connection, x).GetDeviceDesignation();
				var x0 = String.Join(" ", desig.Select(n => n.ToString()));
				Trace.WriteLine($"Desig1 : {x} = {x0}");
			}
			foreach (var x in EmLinq.HexRange(0, 17).Select(n => "Y" + n))
			{
				var desig = MxTag.Create(_connection, x).GetDeviceDesignation(packAlignBit: 32);
				var x0 = String.Join(" ", desig.Select(n => n.ToString()));
				Trace.WriteLine($"Align: {x} = {x0}");
			}
		}



        /// <summary>
        /// Test 에 사용할 MX tag 이름 생성.  e.g "[X,Y][0..15, 17, 25, 33, 99, 81]", "M1111", "D0", "Y160"
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GenerateTagNames()
        {
            //             int size = 3;
            // 
            //             foreach (var n in EmLinq.HexRange(0, 32 * size))
            //                 yield return $"B{n}";
            //             foreach (var n in EmLinq.HexRange(0x2000 - 32 * size, 32 * size))
            //                 yield return $"B{n}";
            //             foreach (var n in EmLinq.HexRange(0, 32 * size))
            //                 yield return $"Y{n}";
            //             foreach (var n in EmLinq.HexRange(0x2000 - 32 * size, 32 * size))
            //                 yield return $"Y{n}";
            //             foreach (var n in EmLinq.HexRange(0, 32 * 5))
            //                 yield return $"X{n}";
            //             foreach (var n in EmLinq.HexRange(0x2000 - 32 * size, 32 * size))
            //                 yield return $"X{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"M{n}";
            //             foreach (var n in Enumerable.Range(8192 - 32 * size, 32 * size))
            //                 yield return $"M{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"L{n}";
            //             foreach (var n in Enumerable.Range(8192 - 32 * size, 32 * size))
            //                 yield return $"L{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"SM{n}";
            //             foreach (var n in Enumerable.Range(2048 - 32 * size, 32 * size))
            //                 yield return $"SM{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"F{n}";
            //             foreach (var n in Enumerable.Range(2048 - 32 * size, 32 * size))
            //                 yield return $"F{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"V{n}";
            //             foreach (var n in Enumerable.Range(2048 - 32 * size, 32 * size))
            //                 yield return $"V{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"TS{n}";
            //             foreach (var n in Enumerable.Range(2048 - 32 * size, 32 * size))
            //                 yield return $"TS{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"TC{n}";
            //             foreach (var n in Enumerable.Range(2048 - 32 * size, 32 * size))
            //                 yield return $"TC{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"TN{n}";
            //             foreach (var n in Enumerable.Range(2048 - 32 * size, 32 * size))
            //                 yield return $"TN{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"CN{n}";
            //             foreach (var n in Enumerable.Range(1024 - 32 * size, 32 * size))
            //                 yield return $"CN{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"CS{n}";
            //             foreach (var n in Enumerable.Range(1024 - 32 * size, 32 * size))
            //                 yield return $"CS{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"CC{n}";
            //             foreach (var n in Enumerable.Range(1024 - 32 * size, 32 * size))
            //                 yield return $"CC{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"D{n}";
            //             foreach (var n in Enumerable.Range(12288 - 32 * size, 32 * size))
            //                 yield return $"D{n}";
            //             foreach (var n in EmLinq.HexRange(0, 32 * size))
            //                 yield return $"W{n}";
            //             foreach (var n in EmLinq.HexRange(0x2000 - 32 * size, 32 * size))
            //                 yield return $"W{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size - 1))
            //                 yield return $"ZR{n}";
            //             foreach (var n in Enumerable.Range(1000 - 32 * size, 32 * size))
            //                 yield return $"ZR{n}";
            //             foreach (var n in Enumerable.Range(0, 32 * size))
            //                 yield return $"R{n}";
            //             foreach (var n in Enumerable.Range(1000 - 32 * size, 32 * size))
            //                 yield return $"R{n}";
            //             foreach (var n in EmLinq.HexRange(0, 32 * size))
            //                 yield return $"SW{n}";
            //             foreach (var n in EmLinq.HexRange(0x800 - 32 * size, 32 * size))
            //                 yield return $"SW{n}";
            //             foreach (var n in Enumerable.Range(0, 1 * size))
            //                 yield return $"Z{n}";
            //             foreach (var n in Enumerable.Range(16 - 1 * size, 1 * size))
            //                 yield return $"Z{n}";
            //             foreach (var n in EmLinq.HexRange(0, 32 * size))
            //                 yield return $"SB{n}";
            //             foreach (var n in EmLinq.HexRange(800 - 32 * size, 32 * size))
            //                 yield return $"SB{n}";
            //       
            yield return "X0";
            yield return "X1";
            yield break;

            yield return "K1M100";
            yield return "K1X0";
            yield return "K1L0";
            yield return "K1L0";
            yield return "K1L10";
            yield return "K1L20";
            yield return "K2L0";
            yield return "K7L10";
            yield return "K4L32";
            yield return "K4L50";
            yield return "K7L50";
            yield return "K1M0";
            yield return "K1M1";


            yield return "K1X200";
            yield return "K2X200";
            yield return "K3X200";
            yield return "K4X200";
            yield return "K5X200";
            yield return "K8X10";
            yield return "K8X10";
            yield return "X0";
            yield return "X3";
            yield return "X4";
            yield return "X5";
            yield return "X6";


            yield return "W1FFF";
            yield return "W1FFE";
            yield return "SW7FE";
            yield return "SW7FF";

            // DX tags
            foreach (var n in EmLinq.HexRange(0, 32))
                yield return $"DX{n}";

            // D tags
            foreach (var n in Enumerable.Range(0, 5))
                yield return $"D{n}";


            var mTags = from n in Enumerable.Range(0, 31)
                        .Concat(Enumerable.Range(110, 5))
                        .Concat(Enumerable.Range(100, 6))
                        .Concat(Enumerable.Range(200, 4))
                        select $"M{n}";

            var ioTags = from xy in new[] { "X", "Y" }
                         from hex in EmLinq.HexRange(0, 1024).Concat(new[] { "31", "111", "112", "113", "114" })
                         select $"{xy}{hex}";

            foreach (var io in ioTags.Concat(mTags))
                yield return io;

            foreach (var t in new[] { "D0", "D100" })
                yield return t;
        }

        private IEnumerable<MxTag> GenerateTags() => _connection.CreateTags(GenerateTagNames()).OfType<MxTag>();

		[TestMethod]
        public void UtMxTagRW()
        {
	        var tags = GenerateTags();
            _connection.SingleScan(prepare: true);

            McProtocol.SetDevice("D7000", 444);
            McProtocol.SetDevice("D7001", 445);

            int a = 0;
            McProtocol.GetDevice("D7000", out a);
            Console.WriteLine(a);

            //McProtocol.SetDevice("x0", 0);
            //McProtocol.SetDevice("x0", 1);
            //McProtocol.SetDevice("x0", 0);
            //McProtocol.SetDevice("x0", 1);
            //McProtocol.SetDevice("x0", 1);
            //McProtocol.SetDevice("D0", 111);

            //tags.ForEach(t =>
            //{
            //    var value = t.Value;
            //    string hex = "";
            //    if (value is ulong)
            //        hex = ((ulong)value).ToString("X");
            //    else if (value is uint)
            //        hex = ((uint)value).ToString("X");
            //    else if (value is ushort)
            //        hex = ((ushort)value).ToString("X");
            //    else if (value is byte)
            //        hex = ((byte)value).ToString("X");
            //    if (hex.NonNullAny())
            //        hex = $"= 0X{hex}";

            //    Trace.WriteLine($"{t.Name} = {t.Value}{hex}");
            //});
        }
	}
}
