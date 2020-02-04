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
        Mapping _mapping;
        private Mapping Mapping
        {
            get { return _mapping; }
            set {_mapping = value; MappingChanged(); }
        }
        void MappingChanged()
        {
            comboOmronMemory.Properties.Items.Clear();
            comboOmronMemory.Properties.Items.AddRange(_mapping.OmronPLC.Memories.Select(m => m.Name).ToArray());

            comboXg5kMemory.Properties.Items.Clear();
            comboXg5kMemory.Properties.Items.AddRange(_mapping.Xg5kPLC.Memories.Select(m => m.Name).ToArray());
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

            //repositoryItemComboOmron.Items.AddRange(plcs.OmronPLCs.Select(o => o.PLCType).ToArray());
            //repositoryItemComboXg5k.Items.AddRange(plcs.XG5000PLCs.Select(o => o.PLCType).ToArray());
            repositoryItemComboOmron.Items.AddRange(plcs.OmronPLCs);
            repositoryItemComboXg5k.Items.AddRange(plcs.XG5000PLCs);
            Mapping = new Mapping(plcs.OmronPLCs[0], plcs.XG5000PLCs[0]);

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