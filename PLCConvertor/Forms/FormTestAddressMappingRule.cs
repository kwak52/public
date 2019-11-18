using Dsu.PLCConvertor.Common;
using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLCConvertor.Forms
{
    public partial class FormTestAddressMappingRule : Form
    {
        public FormTestAddressMappingRule()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var converted = ILSentence.AddressConvertorInstance.Convert(textEditAddress.Text);
            labelControl1.Text = $"Result: {converted}";
        }
    }
}
