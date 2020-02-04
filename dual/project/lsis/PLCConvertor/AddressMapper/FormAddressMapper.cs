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
            lookUpEditOmronMemory.Properties.DataSource = _mapping.OmronPLC.Memories;
            lookUpEditOmronMemory.EditValue = _mapping.OmronPLC.Memories[0];
            lookUpEditXg5kMemory.Properties.DataSource = _mapping.Xg5kPLC.Memories;
            lookUpEditXg5kMemory.EditValue = _mapping.Xg5kPLC.Memories[0];

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

            //repositoryItemComboOmron.Items.AddRange(plcs.OmronPLCs);
            //repositoryItemComboXg5k.Items.AddRange(plcs.XG5000PLCs);
            repositoryItemLookUpEditOmron.DataSource = plcs.OmronPLCs;
            repositoryItemLookUpEditOmron.DisplayMember = "PLCType";
            repositoryItemLookUpEditXg5k.DataSource = plcs.XG5000PLCs;
            repositoryItemLookUpEditXg5k.DisplayMember = "PLCType";

            lookUpEditOmronMemory.Properties.DisplayMember = "Name";
            lookUpEditXg5kMemory.Properties.DisplayMember = "Name";

            Mapping = new PLCMapping(plcs.OmronPLCs[0], plcs.XG5000PLCs[0]);

            lookUpEditOmronMemory.EditValueChanged += (s, e) =>
            {
                var memory = (OmronMemorySection) lookUpEditOmronMemory.EditValue;
                ucMemoryBarOmron.MemorySection = memory;
            };

            lookUpEditXg5kMemory.EditValueChanged += (s, e) =>
            {
                var memory = (Xg5kMemorySection)lookUpEditXg5kMemory.EditValue;
                ucMemoryBarXg5k.MemorySection = memory;
            };


            dockPanelSource.Visibility = DockVisibility.Hidden;
            dockPanelTarget.Visibility = DockVisibility.Hidden;
        }

        private void FormAddressMapper_Load(object sender, EventArgs args)
        {
            Logger.Info("FormAddressMapper launched.");
            //WireDockPanelVisibility(action1, dockPanelMain, barCheckItemShowMain);
            //WireDockPanelVisibility(action1, dockPanelLog, barCheckItemShowLog);
            //WireDockPanelVisibility(action1, dockPanelSource, barCheckItemSource);
            //WireDockPanelVisibility(action1, dockPanelTarget, barCheckItemTarget);

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