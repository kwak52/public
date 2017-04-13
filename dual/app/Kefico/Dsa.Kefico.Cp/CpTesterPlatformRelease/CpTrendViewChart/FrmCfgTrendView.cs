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
    public partial class FrmCfgTrendView : Form
    {
        public Dictionary<PsCCSStdFnBase, bool> DicDisplayingObjects = new Dictionary<PsCCSStdFnBase, bool>();
        public Dictionary<PsCCSStdFnBase, Series> DicShowingSteps { get; set; }
        public int DisplayLimit { get; set; }
        public int CategoryLimit { get; set; } = 1;
        public FrmCfgTrendView()
        {
            InitializeComponent();
        }

        public void InitOptions()
        {
            if (DicDisplayingObjects == null)
                return;

            buttonEditDispLimit.Text = DisplayLimit.ToString();

            foreach (PsCCSStdFnBase stdFnBase in DicShowingSteps.Keys)
                checkedListBoxControlDO.Items.Add(new CheckedListBoxItem(stdFnBase.StepNum.ToString() + ":" + stdFnBase.GetMO(), DicDisplayingObjects[stdFnBase]));
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

        private void FrmCfgTrendView_Load(object sender, EventArgs e)
        {
            InitOptions();
        }

        private void checkedListBoxControlDO_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            string strValue = checkedListBoxControlDO.Items[e.Index].Value.ToString();
            string strStep = string.IsNullOrEmpty(strValue) ? string.Empty : strValue.Split(':')[0];
            int nStep = -1;

            checkedListBoxControlDO.Items[e.Index].CheckState = e.State;

            if (checkedListBoxControlDO.Items.Where(x => x.CheckState == CheckState.Checked).Count() > CategoryLimit)
                checkedListBoxControlDO.Items[e.Index].CheckState = e.State == CheckState.Unchecked ? CheckState.Unchecked : CheckState.Unchecked;

            if (!int.TryParse(strStep, out nStep))
                return;

            PsCCSStdFnBase stdFnbase = DicShowingSteps.Keys.Where(x => x.StepNum == nStep).FirstOrDefault();

            if (stdFnbase == null)
                return;

            DicDisplayingObjects[stdFnbase] = checkedListBoxControlDO.Items[e.Index].CheckState == (CheckState.Checked) ? true : false;
        }
    }
}
