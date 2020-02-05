﻿using System;
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

namespace AddressMapper
{
    public partial class FormAddressMapper
        : DevExpress.XtraBars.Ribbon.RibbonForm
        , IAppender
    {
        PLCs _plcs;
        PLCMapping _mapping;
        public static FormAddressMapper TheMainForm { get; private set; }
        private PLCMapping Mapping
        {
            get { return _mapping; }
            set {_mapping = value; PLCMappingChanged(); }
        }
        /// <summary>
        /// 두 PLC 간 기종 변경시 호출 됨
        /// </summary>
        void PLCMappingChanged()
        {
            Global.Logger.Info("PLC types changed.");
            Clear();

            lookUpEditOmronMemory.Properties.DataSource = null;
            lookUpEditOmronMemory.EditValue = null;
            lookUpEditXg5kMemory.Properties.DataSource = null;
            lookUpEditXg5kMemory.EditValue = null;

            lookUpEditOmronMemory.Properties.DataSource = _mapping.OmronPLC.Memories;
            lookUpEditOmronMemory.EditValue = _mapping.OmronPLC.Memories[0];
            lookUpEditXg5kMemory. Properties.DataSource = _mapping.Xg5kPLC.Memories;
            lookUpEditXg5kMemory. EditValue = _mapping.Xg5kPLC.Memories[0];

            barEditItemOmronPLC.EditValue = _mapping.OmronPLC;
            barEditItemXg5kPLC.EditValue = _mapping.Xg5kPLC;
        }

        void Clear()
        {
            _mapping.OmronPLC.Clear();
            _mapping.Xg5kPLC.Clear();
            ucMemoryBarOmron.DrawRanges();
            ucMemoryBarXg5k.DrawRanges();
        }

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

        public FormAddressMapper(PLCs plcs)
        {
            InitializeComponent();
            _plcs = plcs;
            TheMainForm = this;
        }

        private void FormAddressMapper_Load(object sender, EventArgs args)
        {
            Logger.Info("FormAddressMapper launched.");
            WireDockPanelVisibility(action1, dockPanelMain, barCheckItemShowMain);
            WireDockPanelVisibility(action1, dockPanelLog, barCheckItemShowLog);

            ucMemoryBarOmron.Counterpart = ucMemoryBarXg5k;
            ucMemoryBarXg5k.Counterpart = ucMemoryBarOmron;


            repositoryItemLookUpEditOmron.DataSource = _plcs.OmronPLCs;
            repositoryItemLookUpEditOmron.DisplayMember = "PLCType";
            repositoryItemLookUpEditXg5k.DataSource = _plcs.XG5000PLCs;
            repositoryItemLookUpEditXg5k.DisplayMember = "PLCType";
            repositoryItemLookUpEditOmron.EditValueChanged += PLCChanged;
            repositoryItemLookUpEditXg5k.EditValueChanged += PLCChanged;

            lookUpEditOmronMemory.Properties.DisplayMember = "Name";
            lookUpEditXg5kMemory.Properties.DisplayMember = "Name";


            // 메모리 타입 변경
            lookUpEditOmronMemory.EditValueChanged += (s, e) =>
            {
                var memory = (OmronMemorySection)lookUpEditOmronMemory.EditValue;
                ucMemoryBarOmron.MemorySection = memory;
                AdjustRelativeBarSize();
            };

            lookUpEditXg5kMemory.EditValueChanged += (s, e) =>
            {
                var memory = (Xg5kMemorySection)lookUpEditXg5kMemory.EditValue;
                ucMemoryBarXg5k.MemorySection = memory;
                AdjustRelativeBarSize();
            };

            dockPanelMain.SizeChanged += (s, e) => AdjustRelativeBarSize();

            Mapping = new PLCMapping(_plcs.OmronPLCs[0], _plcs.XG5000PLCs[0]);


            ucMemoryBarOmron.Identifier = "OMRON";
            ucMemoryBarXg5k.Identifier = "Xg5k";

            gridControlRanged.Dock = DockStyle.Fill;
            gridControlOneToOne.Dock = DockStyle.Fill;

            Subjects.MemorySectionChangeRequestSubject.Subscribe(tpl =>
            {
                var ucMemoryBar = tpl.Item1;
                var memTypeName = tpl.Item2;
                if (ucMemoryBar == ucMemoryBarOmron)
                {
                    lookUpEditOmronMemory.EditValue = _mapping.OmronPLC.Memories.FirstOrDefault(m => m.Name == memTypeName);
                }
                else if (ucMemoryBar == ucMemoryBarXg5k)
                {
                    lookUpEditXg5kMemory.EditValue = _mapping.Xg5kPLC.Memories.FirstOrDefault(m => m.Name == memTypeName);
                }
            });


            /// 옴론 / 산전 memory bar 두개를 상대적인 크기 반영
            void AdjustRelativeBarSize()
            {
                Logger.Debug("AdjustRelativeBarSize called.");
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

            /// <summary>
            /// 기종 변경
            /// </summary>
            void PLCChanged(object sender1, EventArgs args1)
            {
                var omron = (OmronPLC)barEditItemOmronPLC.EditValue;
                var xg5k  = (Xg5kPLC) barEditItemXg5kPLC. EditValue;
                Mapping = new PLCMapping(omron, xg5k);
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
            Global.Logger.Info($"Mapping: {om.Name}[{o.Start} - {o.End}] -> {xm.Name}[{x.Start} - {x.End}]");
            var oma = ucMemoryBarOmron.ActiveRangeAllocated();
            var xma = ucMemoryBarXg5k.ActiveRangeAllocated();
            oma.Counterpart = xma;
            xma.Counterpart = oma;
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

                    Global.Logger.Debug($"{oMemTypeName}[{omr.Start}:{omr.End}] => {xMemTypeName}[{xmr.Start}:{xmr.End}]");
                }
            }
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
            var plcs = PLCs.CreateSamplePLCs();
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
                    Global.Logger.Debug($"{m.Name}[{a.Start}:{a.End}] => {c.Parent.Name}[{c.Start}:{c.End}]");
                });
            });
        }

        private void barSelectSampleRange_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucMemoryBarOmron.ActiveRangeSelector.SelectedRange.Minimum = 1024;
            ucMemoryBarOmron.ActiveRangeSelector.SelectedRange.Maximum = 1024*2 - 1;
            ucMemoryBarXg5k. ActiveRangeSelector.SelectedRange.Minimum = 1024;
            ucMemoryBarXg5k. ActiveRangeSelector.SelectedRange.Maximum = 1024*2 - 1;
        }

    }
}