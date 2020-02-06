using Dsu.PLCConverter.UI;
using Dsu.PLCConverter.UI.AddressMapperLogics;
using Dsu.PLCConvertor.Common.Internal;
using Dsu.PLCConvertor.Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AddressMapper
{
    public class RangeMapping
    {
        public string NameO => Omron.Parent.Name;
        public int StartO { get => Omron.Start; set => Omron.Start = value; }
        public int EndO { get => Omron.End; set => Omron.End = value; }
        public string NameX => Xg5k.Parent.Name;
        public int StartX { get => Xg5k.Start; set => Xg5k.Start = value; }
        public int EndX { get => Xg5k.End; set => Xg5k.End = value; }

        public bool Word => ((OmronMemorySection)Omron.Parent).WordAccessable;
        public bool Bit => ((OmronMemorySection)Omron.Parent).BitAccessable;

        public AllocatedMemoryRange Omron { get; private set; }
        public AllocatedMemoryRange Xg5k { get; private set; }
        public RangeMapping(AllocatedMemoryRange omron, AllocatedMemoryRange xg5k)
        {
            Omron = omron;
            Xg5k = xg5k;
        }
    }

    partial class FormAddressMapper
    {
        IEnumerable<IAddressConvertRule> GenerateRules()
        {
            foreach ( var m in _rangeMappings)
            {
                var o = m.Omron;
                var oms = (OmronMemorySection)o.Parent;
                var x = m.Xg5k;
                var srcName = o.Parent.PatternNameOverride ?? o.Parent.Name;
                if (oms.WordAccessable)
                {
                    var srcPattern = $"{srcName}(%d)";
                    var tgtPattern = $"{x.Parent.Name}(%d)";
                    var srcRange = new[] { Tuple.Create(o.Start, o.End) };
                    var tgtArgsRepr = new[] { "$0" };

                    var r = new AddressConvertRule(srcPattern, srcRange, tgtPattern, tgtArgsRepr);
                    yield return r;
                }

                if (oms.BitAccessable)
                {
                    var srcPattern = $"{srcName}(%d).(%d)";
                    var tgtPattern = $"{x.Parent.Name}(%d)(%x)";
                    var srcRange = new[] { Tuple.Create(o.Start, o.End), Tuple.Create(0, 15) };
                    var tgtArgsRepr = new[] { "$0", "$1" };

                    var r = new AddressConvertRule(srcPattern, srcRange, tgtPattern, tgtArgsRepr);
                    yield return r;
                }
            }
        }

        void SerializeRules()
        {
            var rules = GenerateRules();
            var serializer = new AddressConvertorSerializer(rules);
            var json = JsonConvert.SerializeObject(serializer, MyJsonSerializer.JsonSettingsSimple);
            var addressMappingJsonFile = ConfigurationManager.AppSettings["addressMappingRuleFile"];

            using (var sfd = new SaveFileDialog())
            {
                var folder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                sfd.Filter = "JSON file(*.json)|*.json|All files(*.*)|*.*";
                sfd.RestoreDirectory = true;
                var path = ConfigurationManager.AppSettings["addressMappingRuleFile"];
                sfd.InitialDirectory = Path.Combine(folder, Path.GetDirectoryName(path));
                sfd.FileName = Path.GetFileName(path);
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                File.WriteAllText(sfd.FileName, json);
                MsgBox.Info($"Saved to {sfd.FileName}");
            }

        }

        void InitializeGrids()
        {

        }
    }
}
