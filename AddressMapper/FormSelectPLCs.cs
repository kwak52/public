using Dsu.PLCConverter.UI.AddressMapperLogics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddressMapper
{
    public partial class FormSelectPLCs : Form
    {
        PLCHWSpecs _plcs;
        /// <summary>
        /// 선택된 옴론 및 산전 각 하나씩의 H/W PLC type 
        /// </summary>
        public PLCMapping Mapping { get; set; }

        public FormSelectPLCs(PLCHWSpecs plcs, PLCMapping mapping)
        {
            InitializeComponent();
            _plcs = plcs;
            Mapping = mapping;
        }

        private void FormSelectPLCs_Load(object sender, EventArgs e)
        {
            lookUpEditOmron.Properties.DisplayMember = "PLCType";
            lookUpEditXg5k.Properties.DisplayMember = "PLCType";

            lookUpEditOmron.Properties.DataSource = _plcs.OmronPLCs;
            lookUpEditXg5k.Properties.DataSource = _plcs.XG5000PLCs;
            lookUpEditOmron.EditValue = Mapping.OmronPLC;
            lookUpEditXg5k.EditValue = Mapping.Xg5kPLC;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Mapping = new PLCMapping((OmronPLC)lookUpEditOmron.EditValue, (Xg5kPLC)lookUpEditXg5k.EditValue);
            DialogResult = DialogResult.OK;
        }
    }
}
