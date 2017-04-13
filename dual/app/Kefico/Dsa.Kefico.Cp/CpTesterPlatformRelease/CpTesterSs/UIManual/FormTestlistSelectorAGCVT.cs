using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpTester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CpTesterSs.UIManual
{
    public partial class FormTestlistSelectorAGCVT : Form
    {
        public static CpStnManager SelectedStation { get; private set; }
        List<CpStnManager> _stations;
        public FormTestlistSelectorAGCVT(IEnumerable<CpStnManager> stations)
        {
            InitializeComponent();
            _stations = stations.ToList();
        }

        private void FormTestlistSelector_Load(object sender, EventArgs arg)
        {
            if (_stations.Count > 0)
            {
                listBoxControl1.DataSource = new List<CpStnManager> { _stations[0] };
                listBoxControl1.CustomItemDisplayText += (s, e) =>
                {
                    e.DisplayText = ((CpStnManager)e.Item).Name;
                };

                listBoxControl1.ToolTip = "Double click item to select.";

                listBoxControl1.MouseDoubleClick += (s, e) =>
                {
                    SelectedStation = (CpStnManager)listBoxControl1.SelectedItem;
                    Close();
                };
            }
            if (_stations.Count > 1)
            {
                listBoxControl2.DataSource = new List<CpStnManager> { _stations[1] };
                listBoxControl2.CustomItemDisplayText += (s, e) =>
                {
                    e.DisplayText = ((CpStnManager)e.Item).Name;
                };

                listBoxControl2.ToolTip = "Double click item to select.";

                listBoxControl2.MouseDoubleClick += (s, e) =>
                {
                    SelectedStation = (CpStnManager)listBoxControl2.SelectedItem;
                    Close();
                };
            }

            if (_stations.Count > 2)
            {
                listBoxControl3.DataSource = new List<CpStnManager> { _stations[2] };
                listBoxControl3.CustomItemDisplayText += (s, e) =>
                {
                    e.DisplayText = ((CpStnManager)e.Item).Name;
                };

                listBoxControl3.ToolTip = "Double click item to select.";

                listBoxControl3.MouseDoubleClick += (s, e) =>
                {
                    SelectedStation = (CpStnManager)listBoxControl3.SelectedItem;
                    Close();
                };
            }
        }
    }
}
