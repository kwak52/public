using System;
using System.Windows.Forms;
using static Dsu.Driver.NiDaqHwProbe;

namespace Dsu.Driver.UI.NiDaq
{
    public partial class DeviceTreeCtrl : UserControl
    {
        private TreeNode _rootNode;

        public TreeNodeMouseClickEventHandler MouseClickEventHandler;
        //public void AddNodeMouseClick(TreeNodeMouseClickEventHandler handler)
        //{
        //    treeView1.NodeMouseClick += handler;
        //}

        public DeviceTreeCtrl()
        {
            InitializeComponent();
        }

        private void DeviceTreeCtrl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            _rootNode = treeView1.Nodes.Add("This Computer");
            foreach (var deviceName in GetLocalDeviceNames())
            {
                var device = NiDaqHwLocal.GetDevice(deviceName);
                var deviceNode = _rootNode.Nodes.Add($"{deviceName} [{device.ProductType}]");
                deviceNode.Tag = device;

                var AONode = deviceNode.Nodes.Add("AO channels");
                foreach (var ch in device.AOPhysicalChannels)
                    AONode.Nodes.Add(ch).Tag = ch;

                var AINode = deviceNode.Nodes.Add("AI channels");
                foreach (var ch in device.AIPhysicalChannels)
                    AINode.Nodes.Add(ch).Tag = ch;
            }

            treeView1.ExpandAll();

            // treeView1.AfterSelect += (o, args) => MessageBox.Show($"You selected {treeView1.SelectedNode.Text} node.");
            treeView1.NodeMouseClick += (o, args) =>
            {
                MouseClickEventHandler(o, args);
                //if (args.Button == MouseButtons.Right)
                //    MessageBox.Show($"You R-clicked {treeView1.SelectedNode.Text} node.");
            };
        }
    }
}
