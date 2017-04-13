using DevExpress.XtraCharts;
using DevExpress.XtraEditors.Controls;
using PsKGaudi.Parser.PsCCSSTDFn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpTesterPlatform.CpTrendView
{
    // see FrmCfgTrendView
    public partial class FormDaqDisplaySelector : Form
    {
        //public Dictionary<PsCCSStdFnBase, bool> DicDisplayingObjects = new Dictionary<PsCCSStdFnBase, bool>();
        //public Dictionary<PsCCSStdFnBase, Series> DicShowingSteps { get; set; }
        //public int DisplayLimit { get; set; }
        //public int CategoryLimit { get; set; } = 1;
        public FormDaqDisplaySelector()
        {
            InitializeComponent();
        }

        public void InitOptions()
        {
            //if (DicDisplayingObjects == null)
            //    return;

            foreach ( var s in new[] { "Input", "Output", "Middle" })
            {
                checkedListBoxControlDO.Items.Add(new CheckedListBoxItem(s));
            }
        }

        private void simpleButtonSet_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormDaqDisplaySelector_Load(object sender, EventArgs e)
        {
            InitOptions();
        }

        private void checkedListBoxControlDO_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
        }
    }
}
