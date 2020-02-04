using System;
using DevExpress.XtraBars;
using log4net.Appender;
using DevExpress.XtraBars.Docking;
using Dsu.PLCConverter.UI.AddressMapperLogics;
using Newtonsoft.Json;
using Dsu.PLCConvertor.Common.Util;
using System.IO;
using Dsu.PLCConverter.UI;

namespace AddressMapper
{
    public partial class FormAddressMapper
        : DevExpress.XtraBars.Ribbon.RibbonForm
        , IAppender
    {
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
            var omronPLCs = new[]
            {
                new OmronPLC("CJ1H",
                    new[] {
                        new OmronMemorySection("PIO",   0, 1024*10, new []{"P" }),
                        new OmronMemorySection("D",     0, 1024*10, new []{"M", "D" }),
                    }),
                new OmronPLC("CJ2H",
                    new[] {
                        new OmronMemorySection("PIO",   0, 1024, new []{"P" }),
                        new OmronMemorySection("D",     0, 1024, new []{"M", "D" }),
                    }),
            };

            var xg5kPLCs = new[]
            {
                new Xg5kPLC("Xg5k1H",
                    new[] {
                        new Xg5kMemorySection("P",   0, 1024*10),
                        new Xg5kMemorySection("M",   0, 1024*10),
                    }),
                new Xg5kPLC("Xg5k2H",
                    new[] {
                        new Xg5kMemorySection("P",   0, 1024),
                        new Xg5kMemorySection("M",   0, 1024),
                    }),
            };

            var plcs = new PLCs(omronPLCs, xg5kPLCs);


            var file = "OmronMemory.json";
            var json = JsonConvert.SerializeObject(plcs, MyJsonSerializer.JsonSettingsSimple);
            File.WriteAllText(file, json);
            MsgBox.Info("Info", $"File created: {file}");
        }
    }
}