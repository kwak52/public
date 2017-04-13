using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Dsa.Kefico.PDV.Forms
{

    public partial class FrmEntry : DevExpress.XtraEditors.XtraForm
    {

        public int PdvGroupId { get; set; }
        List<string> _lstProductGroup = new List<string>();
        List<string> _lstProduct = new List<string>();

        public string ProductGroup { get; set; }
        public string Product { get; set; }
        public bool New { get; private set; } = false;
        public bool Copy { get; private set; } = false;
        public string Comment { get; set; }
        DataRow _drSelect;

        public FrmEntry(bool create, int id, List<string> lstProductGroup, List<string> lstProduct,  DataRow drSelect = null)
        {
            New = create;
            if (create && drSelect != null)
                Copy = true;

            PdvGroupId = id;
            _lstProductGroup = lstProductGroup;
            _lstProduct = lstProduct;
            _drSelect = drSelect;

            InitializeComponent();
        }

        private void FrmEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
                this.DialogResult = DialogResult.Cancel;
        }

        private void wizardControl1_FinishClick(object sender, CancelEventArgs e)
        {
            if (comboBoxEdit_Group.Text == ""
               || comboBoxEdit_ProductModel.Text == "")
            {
                e.Cancel = true;
                XtraMessageBox.Show(this, "Please enter all fields except comment", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.DialogResult = DialogResult.OK;

                PdvGroupId = Convert.ToInt32(textEdit_Id.EditValue);
                ProductGroup = comboBoxEdit_Group.EditValue.ToString();
                Product = comboBoxEdit_ProductModel.EditValue.ToString();
                Comment = textEdit_Comment.Text;

                this.Close();
            }
        }

        private void wizardControl1_CancelClick(object sender, CancelEventArgs e)
        {
            this.Close();
        }

        private void FrmEntry_Load(object sender, EventArgs e)
        {
            comboBoxEdit_Group.Properties.Items.AddRange(_lstProductGroup);
            comboBoxEdit_ProductModel.Properties.Items.AddRange(_lstProduct);
            textEdit_Id.EditValue = PdvGroupId;

            if (_drSelect != null) SelectRow(_drSelect);

            if (Copy)
                wizardPage1.Text = "Copy Entry";
            else if (New)
                wizardPage1.Text = "Create Entry";
            else
                wizardPage1.Text = "Edit Entry";
        }

        private void SelectRow(DataRow dr)
        {
            comboBoxEdit_Group.SelectedItem = dr[SchemaPDV.PDVGROUP_ProductGroup];
            comboBoxEdit_ProductModel.SelectedItem = dr[SchemaPDV.PDVGROUP_ProductModel];
            textEdit_Comment.EditValue  = dr[SchemaPDV.PDVGROUP_Comment];
        }

        private void wizardControl1_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            completionWizardPage1.FinishText = "You have successfully completed the wizard";
            completionWizardPage1.FinishText += string.Format("\r\n\r\nPRNR : {0}", textEdit_Id.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nProduct Group : {0}", comboBoxEdit_Group.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nProduct : {0}", comboBoxEdit_ProductModel.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nComment : {0}", textEdit_Comment.EditValue);
        }

        private void comboBoxEdit_Group_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            e.NewValue = e.NewValue.ToString().ToUpper();
        }

        private void comboBoxEdit_ProductModel_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            e.NewValue = e.NewValue.ToString().ToUpper();
        }
    }
}