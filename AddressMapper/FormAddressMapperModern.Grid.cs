﻿using Dsu.PLCConverter.UI;
using Dsu.PLCConverter.UI.AddressMapperLogics;
using Dsu.PLCConvertor.Common.Internal;
using Dsu.PLCConvertor.Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static Dsu.PLCConvertor.Common.Internal.AddressConvertorSerializer;

namespace AddressMapper
{
    partial class FormAddressMapperModern
    {
        IEnumerable<IAddressConvertRule> GenerateRules()
        {
            foreach (var m in _oneToOneMappings)
                yield return m.ToNormalRule();

            foreach (var m in _rangeMappings)
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

        public class Sample
        {
            public string Name { get; set; }
            public Sample(string name)
            {
                Name = name;
            }
            public Sample() { }
        }
        /// <summary>
        /// 최종 mapping 결과들
        /// </summary>
        BindingList<RangeMapping> _rangeMappings = new BindingList<RangeMapping>();
        BindingList<OneToOneRule> _oneToOneMappings = new BindingList<OneToOneRule>();
        void InitializeGrids()
        {
            gridControlRanged.DataSource = _rangeMappings;
            gridControlOneToOne.DataSource = _oneToOneMappings;

            //gridControlOneToOne.EmbeddedNavigator.ButtonClick += (s, e) =>
            //{
            //    gridViewOneToOne.AddNewRow();
            //    e.Handled = true;
            //};
            ////gridViewOneToOne.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            //gridViewOneToOne.InitNewRow += (s, e) =>
            //{
            //    gridViewOneToOne.AddNewRow();
            //};
        }
    }
}
