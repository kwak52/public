using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpTester;
using Dsu.Driver.Util.Emergency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CpTesterSs.UIManual
{
    public partial class FormTestlistSelectorA78 : Form
    {
        public static CpStnManager SelectedStation { get; private set; }
        List<CpStnManager> _stations;
        public FormTestlistSelectorA78(IEnumerable<CpStnManager> stations)
        {
            InitializeComponent();
            _stations = stations.ToList();
        }

        private void FormTestlistSelector_Load(object sender, EventArgs arg)
        {
            //var parsedSignal = CpSignalManager.GetParsedSignal(SignalEnum.UPart8);
            //var partSpecs = parsedSignal.Message;   // e.g "9024180021=BLACK;9024180022=YELLOW;9024180023=GRAY;9024180024=RED"
            //_stations.ForEach(f =>  //test ahn
            //{
            //    var matchSpec = partSpecs.Split(';').FirstOrDefault(s => s.Contains(f.MngTStep.GaudiReadData.TestListInfo.PartNum);
            //});


            listBoxControl1.DataSource = _stations.Take(4);
            listBoxControl1.CustomItemDisplayText += (s, e) =>
            {
                e.DisplayText = ((CpStnManager)e.Item).Name;
            };

            listBoxControl1.ToolTip = "Double click item to select.";

            listBoxControl1.MouseDoubleClick += (s, e) =>
            {
                SelectedStation = (CpStnManager)listBoxControl1.SelectedItem;
                Close();

                if (ModifierKeys == Keys.Shift)
                    FormDeveloper.DoModal();
            };

            if (_stations.Count > 4)
            {
                List<CpStnManager> stations7Dct = new List<CpStnManager>();
                for (int i = 4; i < _stations.Count; i++)
                    stations7Dct.Add(_stations[i]);

                listBoxControl2.DataSource = stations7Dct;
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
        }
    }
}
