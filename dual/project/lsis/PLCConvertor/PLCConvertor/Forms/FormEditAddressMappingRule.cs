using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common;
using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PLCConvertor.Forms
{
    public partial class FormEditAddressMappingRule : Form
    {
        public FormEditAddressMappingRule()
        {
            InitializeComponent();
        }

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

            btnAddSource.Click += (s, e) =>
            {
                (gridControlSource.DataSource as SourceDetailWrapper).Add(new MinMaxRange(Tuple.Create(0, 0)));
                gridViewSource.RefreshData();
            };

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
    }
}
