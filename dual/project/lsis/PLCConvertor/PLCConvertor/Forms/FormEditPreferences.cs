using Dsu.PLCConvertor.Common;
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
    public partial class FormEditPreferences : Form
    {
        public FormEditPreferences()
        {
            InitializeComponent();
        }

        private void FormEditPreferences_Load(object sender, EventArgs args)
        {
            checkEditSplitBySection.Checked = Cx2Xg5kOption.SplitBySection;
            checkEditForceSplitRung.Checked = Cx2Xg5kOption.ForceRungSplit;

            checkEditSplitBySection.CheckedChanged += (s, e) => Cx2Xg5kOption.SplitBySection = checkEditSplitBySection.Checked;
            checkEditForceSplitRung.CheckedChanged += (s, e) => Cx2Xg5kOption.ForceRungSplit = checkEditForceSplitRung.Checked;
        }
    }
}
