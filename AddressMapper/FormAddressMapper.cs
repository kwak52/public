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

namespace AddressMapper
{
    public partial class FormAddressMapper
        : DevExpress.XtraBars.Ribbon.RibbonForm
        , IAppender
    {
        PLCs _plcs;
        PLCMapping _mapping;
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
            lookUpEditOmronMemory.Properties.DataSource = _mapping.OmronPLC.Memories;
            lookUpEditOmronMemory.EditValue = _mapping.OmronPLC.Memories[0];
            lookUpEditXg5kMemory.Properties.DataSource = _mapping.Xg5kPLC.Memories;
            lookUpEditXg5kMemory.EditValue = _mapping.Xg5kPLC.Memories[0];

            //lookUpEditXg5kMemory.EditValueChanged();

            barEditItemOmronPLC.EditValue = _mapping.OmronPLC;
            barEditItemXg5kPLC.EditValue = _mapping.Xg5kPLC;
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
        }

        private void FormAddressMapper_Load(object sender, EventArgs args)
        {
            Logger.Info("FormAddressMapper launched.");
            //WireDockPanelVisibility(action1, dockPanelMain, barCheckItemShowMain);
            //WireDockPanelVisibility(action1, dockPanelLog, barCheckItemShowLog);
            //WireDockPanelVisibility(action1, dockPanelSource, barCheckItemSource);
            //WireDockPanelVisibility(action1, dockPanelTarget, barCheckItemTarget);


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


            //PLCMappingChanged();
            //AdjustRelativeBarSize();

            dockPanelSource.Visibility = DockVisibility.Hidden;
            dockPanelTarget.Visibility = DockVisibility.Hidden;


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
                var xg5k = (Xg5kPLC)barEditItemXg5kPLC.EditValue;
                Mapping = new PLCMapping(omron, xg5k);
            }
        }



        private void btnGenerateJsonTemplate_ItemClick(object sender, ItemClickEventArgs e)
        {
            var plcs = PLCs.CreateSamplePLCs();
            var file = "OmronMemory.json";
            var json = JsonConvert.SerializeObject(plcs, MyJsonSerializer.JsonSettingsSimple);
            File.WriteAllText(file, json);
            MsgBox.Info("Info", $"File created: {file}");
        }
    }
}