using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common;
using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PLCConvertor.Forms
{
    /// <summary>
    /// Address mapping 관련, 전체 rule set 을 편집하기 위한 form
    /// </summary>
    public partial class FormEditAddressMappingRule : Form
    {
        public FormEditAddressMappingRule()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Rule grid 에서 이미 선택된 rule
        /// </summary>
        static AddressConvertRule _oldSelectedRule = null;
        private void FormEditAddressMappingRule_Load(object sender, EventArgs args)
        {
            _oldSelectedRule = null;

            new[] { gridControlRule, gridControlSource, gridControlTarget }
            .Iter(gc =>
            {
                gc.Dock = DockStyle.Fill;
            });

            new[] { gridViewRule, gridViewSource, gridViewTarget }
            .Iter(gv =>
            {
                gv.OptionsView.ShowGroupPanel = false;
            });


            // Global 변수 참조
            gridControlRule.DataSource = ILSentence.AddressConvertorInstance.Rules;

            gridViewRule.FocusedRowChanged += (s, e) => {
                if (_oldSelectedRule != null)
                {
                    var src = gridControlSource.DataSource as SourceDetailWrapper;
                    src.Apply();

                    var tgt = gridControlTarget.DataSource as TargetDetailWrapper;
                    tgt.Apply();


                    btnDeleteSource.Enabled = gridViewSource.GetSelectedRows().Any();
                    btnDeleteTarget.Enabled = gridViewTarget.GetSelectedRows().Any();
                }



                AddressConvertRule withRangedRule = getAddressConvertRule();
                var selected = withRangedRule != null;
                if (selected)
                {
                    gridControlSource.DataSource = new SourceDetailWrapper(withRangedRule);
                    gridControlTarget.DataSource = new TargetDetailWrapper(withRangedRule);
                }

                dockPanelSource.Enabled = selected;
                dockPanelTarget.Enabled = selected;

                _oldSelectedRule = withRangedRule;
            };

            // Source 의 argument 행을 추가
            btnAddSource.Click += (s, e) =>
            {
                (gridControlSource.DataSource as SourceDetailWrapper).Add(new MinMaxRange(Tuple.Create(0, 0)));
                gridViewSource.RefreshData();
            };

            // Target 의 argument 행을 추가
            btnAddTarget.Click += (s, e) =>
            {
                (gridControlTarget.DataSource as TargetDetailWrapper).Add(new ExpressionHolder(""));
                gridViewTarget.RefreshData();
            };

            btnDeleteSource.Click += (s, e) => gridViewSource.DeleteSelectedRows();
            btnDeleteTarget.Click += (s, e) => gridViewTarget.DeleteSelectedRows();
            btnDeleteRule.Click += (s, e) => gridViewRule.DeleteSelectedRows();


            dockPanelSource.Enabled = 
            dockPanelTarget.Enabled = getAddressConvertRule() != null;

            new[] { btnSave, btnLoad, btnAddRule, btnDeleteRule }.Iter(btn => btn.Size = new Size(60, 20));


            AddressConvertRule getAddressConvertRule() => gridViewRule.GetRow(gridViewRule.GetSelectedRows().First()) as AddressConvertRule;
        }

        private void btnAddRule_Click(object sender, EventArgs e)
        {
            var form = new FormAddAddressMappingRule();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var rule = form.Rule;
                var newRuleSet = ILSentence.AddressConvertorInstance.Rules.Concat(new[] { rule }).ToList();
                ILSentence.AddressConvertorInstance.Rules = newRuleSet;
                gridControlRule.DataSource = ILSentence.AddressConvertorInstance.Rules;
                gridViewRule.RefreshData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var addressMappingJsonFile = ConfigurationManager.AppSettings["addressMappingRuleFile"];
            ILSentence.AddressConvertorInstance.SaveToJsonFile(addressMappingJsonFile);
            MessageBox.Show($"Saved to\r\n{addressMappingJsonFile}");
        }


        /// <summary>
        /// Json file 을 읽어들여서 새로운 rule set 을 구성한다.
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Json files|*.json|All files(*.*)|*.*";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var jsonFile = ofd.FileName;
                    ILSentence.AddressConvertorInstance = AddressConvertor.LoadFromJsonFile(jsonFile);
                    gridControlRule.DataSource = ILSentence.AddressConvertorInstance.Rules;
                    gridViewRule.RefreshData();
                    MessageBox.Show($"Sucessfully loaded\r\n{jsonFile}");
                }
            }
        }
    }
}
