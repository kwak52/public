using System;
using DevExpress.XtraBars;
using log4net.Appender;
using DevExpress.XtraBars.Docking;
using Dsu.PLCConverter.UI.AddressMapperLogics;
using Newtonsoft.Json;
using Dsu.PLCConvertor.Common.Util;
using System.IO;
using Dsu.PLCConverter.UI;
using System.Linq;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Windows.Forms;
using System.Collections.Generic;
using Dsu.PLCConvertor.Common;
using log4net;
using Dsu.PLCConvertor.Common.Internal;
using System.Configuration;
using System.Reflection;

namespace AddressMapper
{
    public partial class FormAddressMapper
        : DevExpress.XtraBars.Ribbon.RibbonForm
        , IAppender
    {
        /// <summary>
        /// 옴론 및 산전의 모든 H/W PLC type 정의
        /// </summary>
        PLCHWSpecs _plcs;
        PLCHWSpecs PLCHWSpecs
        {
            get { return _plcs; }
            set
            {
                _plcs = value;
                PLCHWSpecsChangeRequestSubject.OnNext(value);
            }
        }

        /// <summary>
        /// 선택된 옴론 및 산전 각 하나씩의 H/W PLC type 
        /// </summary>
        PLCMapping _mapping;

        /// <summary>
        /// 최종 mapping 결과들
        /// </summary>
        List<RangeMapping> _rangeMappings = new List<RangeMapping>();

        public static FormAddressMapper TheMainForm { get; private set; }
        private PLCMapping Mapping
        {
            get { return _mapping; }
            set
            {
                _mapping = value;
                Subjects.PLCMappingChangeRequestSubject.OnNext(value);
            }
        }

        ILog _logger => Dsu.PLCConverter.UI.Global.Logger;

        void WireDockPanelVisibility(Dsu.Common.Utilities.Actions.Action action, DockPanel dockPanel, BarCheckItem checkItem)
        {
            dockPanel.ClosingPanel += (s, e) =>
            {
                e.Cancel = true;
                dockPanel.Hide();
            };

            checkItem.CheckedChanged += (s, e) =>
            {
                if (checkItem.Checked)
                    dockPanel.Show();
                else
                    dockPanel.Hide();
            };
            action.Update += (s, e) => checkItem.Checked = dockPanel.Visibility == DockVisibility.Visible;
        }

        public FormAddressMapper()
        {
            InitializeComponent();
            TheMainForm = this;
        }

        private void FormAddressMapper_Load(object sender, EventArgs args)
        {
            Logger.Info("FormAddressMapper launched.");

            InitializeGrids();
            InitializeSubjects();

            WireDockPanelVisibility(action1, dockPanelMain, barCheckItemShowMain);
            WireDockPanelVisibility(action1, dockPanelLog, barCheckItemShowLog);
            WireDockPanelVisibility(action1, dockPanelGridRanged, barCheckItemGridRanged);
            WireDockPanelVisibility(action1, dockPanelGridOneToOne, barCheckItemGridOneToOne);

            ucMemoryBarOmron.Counterpart = ucMemoryBarXg5k;
            ucMemoryBarXg5k.Counterpart = ucMemoryBarOmron;

            var plcHardwareSettingFile = ConfigurationManager.AppSettings["plcHardwareSettingFile"];
            PLCHWSpecs = LoadPLCHardwareSetting(plcHardwareSettingFile);


            repositoryItemLookUpEditOmron.DisplayMember = "PLCType";
            repositoryItemLookUpEditXg5k.DisplayMember = "PLCType";
            repositoryItemLookUpEditOmron.EditValueChanged += (s, e) => PLCChanged(s, e, PLCVendor.Omron);
            repositoryItemLookUpEditXg5k.EditValueChanged += (s, e) => PLCChanged(s, e, PLCVendor.LSIS);

            lookUpEditOmronMemory.Properties.DisplayMember = "Name";
            lookUpEditXg5kMemory.Properties.DisplayMember = "Name";


            // 옴론 메모리 타입 변경
            lookUpEditOmronMemory.EditValueChanged += (s, e) =>
            {
                var memory = (OmronMemorySection)lookUpEditOmronMemory.EditValue;
                ucMemoryBarOmron.MemorySection = memory;
                AdjustRelativeBarSize();
            };
            //lookUpEditOmronMemory.EditValueChanging += (s, e) =>
            //{
            //    var old = (OmronMemorySection)e.OldValue;
            //    Console.WriteLine("");
            //};

            // 산전 메모리 타입 변경
            lookUpEditXg5kMemory.EditValueChanged += (s, e) =>
            {
                var memory = (Xg5kMemorySection)lookUpEditXg5kMemory.EditValue;
                ucMemoryBarXg5k.MemorySection = memory;
                AdjustRelativeBarSize();
            };

            dockPanelMain.SizeChanged += (s, e) => AdjustRelativeBarSize();

            ucMemoryBarOmron.PLCVendor = PLCVendor.Omron;
            ucMemoryBarXg5k.PLCVendor = PLCVendor.LSIS;

            gridControlRanged.Dock = DockStyle.Fill;
            gridControlOneToOne.Dock = DockStyle.Fill;

            Mapping = new PLCMapping(PLCHWSpecs.OmronPLCs[0], PLCHWSpecs.XG5000PLCs[0]);

            /// 옴론 / 산전 memory bar 두개를 상대적인 크기 반영
            void AdjustRelativeBarSize()
            {
                //Logger.Debug("AdjustRelativeBarSize called.");
                if (ucMemoryBarOmron.MemorySection == null || ucMemoryBarXg5k.MemorySection == null)
                    return;

                var o = ucMemoryBarOmron.MemorySection.Length;
                var x = ucMemoryBarXg5k.MemorySection.Length;
                var W = dockPanelMain.Width;
                var longer = o > x ? ucMemoryBarOmron : ucMemoryBarXg5k;
                var shorter = o > x ? ucMemoryBarXg5k : ucMemoryBarOmron;
                var lw = W - longer.Location.X - 10;
                longer.Width = lw;
                shorter.Width = (int)(lw * (Math.Min(o, x) / (float)Math.Max(o, x)));

                Console.WriteLine("");
            }
        }



        /// <summary>
        /// 선택된 range 간 mapping 수행
        /// </summary>
        private void btnAssign_Click(object sender, EventArgs e)
        {
            var o = ucMemoryBarOmron.ActiveRangeSelector.SelectedRange.ToMemoryRange();
            var x = ucMemoryBarXg5k. ActiveRangeSelector.SelectedRange.ToMemoryRange();
            var om = ucMemoryBarOmron.MemorySection;
            var xm = ucMemoryBarXg5k. MemorySection;
            _logger.Info($"Mapping: {om.Name}[{o.Start} - {o.End}] -> {xm.Name}[{x.Start} - {x.End}]");
            var oma = ucMemoryBarOmron.ActiveRangeAllocated();
            var xma = ucMemoryBarXg5k.ActiveRangeAllocated();
            oma.Counterpart = xma;
            xma.Counterpart = oma;

            var mapping = new RangeMapping(oma, xma);
            _rangeMappings.Add(mapping);
            gridControlRanged.DataSource = _rangeMappings;
        }

        private void btnExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (var om in _mapping.OmronPLC.Memories)
            {
                string oMemTypeName = om.Name;
                foreach (var omr in om.MemoryRanges.Cast<AllocatedMemoryRange>())
                {
                    var xmr = omr.Counterpart;
                    var xMemTypeName = xmr.Parent.Name;

                    _logger.Debug($"{oMemTypeName}[{omr.Start}:{omr.End}] => {xMemTypeName}[{xmr.Start}:{xmr.End}]");
                }
            }

            SerializeRules();
        }

        private void action1_Update(object sender, EventArgs e)
        {
            var o = ucMemoryBarOmron.ActiveRangeSelector;
            var x = ucMemoryBarXg5k.ActiveRangeSelector;
            if (o == null || x == null)
            {
                btnAssign.Enabled = false;
                return;
            }
            var ol = o.SelectedRange.ToMemoryRange()?.Length;
            var xl = x.SelectedRange.ToMemoryRange()?.Length;
            btnAssign.Enabled = xl.HasValue && ol.HasValue && xl.Value >= ol.Value;
        }

        private void btnTestRangeUI_ItemClick(object sender, ItemClickEventArgs e)
        {
            new FormTestRangeUI().Show();
        }


        private void btnGenerateJsonTemplate_ItemClick(object sender, ItemClickEventArgs e)
        {
            var plcs = PLCHWSpecs.CreateSamplePLCs();
            var file = "OmronMemory.json";
            var json = JsonConvert.SerializeObject(plcs, MyJsonSerializer.JsonSettingsSimple);
            File.WriteAllText(file, json);
            MsgBox.Info("Info", $"File created: {file}");
        }

        private void btnShowBarContents_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucMemoryBarOmron.DumpMemoryRanges();
            ucMemoryBarXg5k. DumpMemoryRanges();

            _mapping.OmronPLC.Memories.Iter(m => {

                m.MemoryRanges.OfType<AllocatedMemoryRange>().Iter(a =>
                {
                    var c = a.Counterpart;
                    _logger.Debug($"{m.Name}[{a.Start}:{a.End}] => {c.Parent.Name}[{c.Start}:{c.End}]");
                });
            });
        }

        private void btnSelectSampleRange_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucMemoryBarOmron.ActiveRangeSelector.SelectedRange.Minimum = 1024;
            ucMemoryBarOmron.ActiveRangeSelector.SelectedRange.Maximum = 1024*2 - 1;
            ucMemoryBarXg5k. ActiveRangeSelector.SelectedRange.Minimum = 1024;
            ucMemoryBarXg5k. ActiveRangeSelector.SelectedRange.Maximum = 1024*2 - 1;
        }

        private void btnLoadPLCSettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                var folder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                ofd.Filter = "JSON file(*.json)|*.json|All files(*.*)|*.*";
                ofd.RestoreDirectory = true;
                var path = ConfigurationManager.AppSettings["plcHardwareSettingFile"];
                ofd.InitialDirectory = Path.Combine(folder, Path.GetDirectoryName(path));
                ofd.FileName = Path.GetFileName(path);
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                var plcHardwareSettingFile = ofd.FileName;
                PLCHWSpecs = LoadPLCHardwareSetting(plcHardwareSettingFile);
            }
        }
    }
}