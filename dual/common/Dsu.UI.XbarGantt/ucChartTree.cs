using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using DevExpress.Utils;
using Dsu.UI.XbarGantt.EventHandler;
using DevExpress.XtraTreeList.Nodes;
using System.Collections.Generic;

namespace Dsu.UI.XbarGantt
{
    public partial class ucChartTree : UserControl
    {
        public event UEventHandlerShowChanged UEventShowChanged;
        public event UEventHandlerMouseDoubleClick UEventMouseDoubleClick;
        public List<long> _showMemory = new List<long>();

        public ucChartTree()
        {
            InitializeComponent();
        }

        public void ShowTree(DataTable dt)
        {
            GanttTree.DataSource = dt;
          //  GanttTree.DataSource = TestTreeGeneration();

          //  GetShowNode();
        }

        private DataTable TestTreeGeneration()
        {
            //TEST
            List<int> lstId = new List<int>();
            DataTable dtt = new DataTable();

            dtt.Columns.Add(new DataColumn("memoryId", typeof(Int64)));
            dtt.Columns.Add(new DataColumn("ViewOrder", typeof(string)));

            Random r = new Random();
            for (int i = 0; i < 50; i++)
            {
                int sortId1 = r.Next(1, 50);
                if (sortId1 % 2 == 0)
                    dtt.Rows.Add(sortId1, "X" + sortId1.ToString("000"));
                else
                    dtt.Rows.Add(sortId1, "Y" + sortId1.ToString("000"));
            }

            return dtt;
        }

        private void GetShowNode()
        {
            _showMemory.Clear();
            foreach (TreeListNode node in GanttTree.Nodes)
            {
                if (node.Visible)
                    GetShowNodeCallback(node, _showMemory);
            }

            if (UEventShowChanged != null)
                UEventShowChanged(_showMemory);
        }

        private void GetShowNodeCallback(TreeListNode node, List<long> showMemory)
        {
            showMemory.Add(Convert.ToInt32(node["memoryId"]));
            foreach (TreeListNode nodeSub in node.Nodes)
            {
                if (node.Expanded)
                {
                    GetShowNodeCallback(nodeSub, showMemory);
                }
            }
        }

        private void GanttTree_LayoutUpdated(object sender, EventArgs e)
        {
            List<long> NewshowMemory = new List<long>();
            foreach (TreeListNode node in GanttTree.Nodes)
            {
                if (node.Visible)
                    GetShowNodeCallback(node, NewshowMemory);
            }

            if (NewshowMemory.Count != _showMemory.Count)
            {
                GetShowNode();
                return;
            }

            for (int i = 0; i < _showMemory.Count; i++)
            {
                if (_showMemory[i] != NewshowMemory[i])
                {
                    GetShowNode();
                    return;
                }
            }
        }

        private void GanttTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            UEventMouseDoubleClick?.Invoke(this, e);
        }
    }
}
