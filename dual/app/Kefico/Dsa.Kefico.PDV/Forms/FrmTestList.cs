using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Dsa.Kefico.PDV.Forms
{

    public partial class FrmTestList : DevExpress.XtraEditors.XtraForm
    {
        public int PdvTestListId { get; set; }

        public bool New { get; private set; } = false;
        public bool Copy { get; private set; } = false;

        List<string> _lstProductNumber = new List<string>();
        List<int> _lstvariant = new List<int>();
        List<string> _lstproductType = new List<string>();
        List<string> _lstproduct = new List<string>();
        List<string> _lstfileStem = new List<string>();

        public string ProductNumber { get; set; }
        public string ProductType { get; set; }
        public string Product { get; set; }
        public int Variant { get; set; }

        public string FileStem { get; set; }
        public string Comment { get; set; }
        DataRow _drSelect;

        public FrmTestList(bool create, int id, List<string> lstProductNumber, List<string> lstproduct, List<string> lstproductType, List<int> lstvariant, List<string> lstfileStem, DataRow drSelect = null)
        {
            New = create;
            if (create && drSelect != null)
                Copy = true;

            PdvTestListId = id;
            _lstProductNumber = lstProductNumber;
            _lstvariant = lstvariant;
            _lstproduct = lstproduct;
            _lstproductType = lstproductType;
            _lstfileStem = lstfileStem;

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
            if (comboBoxEdit_ProductNumber.Text == ""
                || comboBoxEdit_Product.Text == ""
                || comboBoxEdit_ProductType.Text == ""
                || comboBoxEdit_Variant.Text == ""
                || comboBoxEdit_FileStem.Text == "")
            {
                e.Cancel = true;
                XtraMessageBox.Show(this, "Please enter all fields", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBoxEdit_Product.Text.Length != 2
                || comboBoxEdit_ProductType.Text.Length != 2)
            {
                e.Cancel = true;
                XtraMessageBox.Show(this, "Please enter [Product or ProductType] two characters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBoxEdit_ProductNumber.Text.Length != 10)
            {
                e.Cancel = true;
                XtraMessageBox.Show(this, "Please enter 10 characters ProductNumber", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBoxEdit_FileStem.Text.Length != 11)
            {
                e.Cancel = true;
                XtraMessageBox.Show(this, "Please enter 11 characters file name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.DialogResult = DialogResult.OK;

                ProductNumber = comboBoxEdit_ProductNumber.EditValue.ToString();
                Product = comboBoxEdit_Product.EditValue.ToString();
                ProductType = comboBoxEdit_ProductType.EditValue.ToString();
                FileStem = comboBoxEdit_FileStem.EditValue.ToString();
                Variant = Convert.ToInt32(comboBoxEdit_Variant.EditValue);
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
            comboBoxEdit_ProductNumber.Properties.Items.AddRange(_lstProductNumber);
            comboBoxEdit_Product.Properties.Items.AddRange(_lstproduct);
            comboBoxEdit_ProductType.Properties.Items.AddRange(_lstproductType);
            comboBoxEdit_Variant.Properties.Items.AddRange(_lstvariant);
            comboBoxEdit_FileStem.Properties.Items.AddRange(_lstfileStem);

            comboBoxEdit_ProductNumber.ReadOnly = !New;
            comboBoxEdit_Product.ReadOnly = !New;
            comboBoxEdit_ProductType.ReadOnly = !New;

            if (_drSelect != null) SelectRow(_drSelect);

            if (Copy)
                wizardPage1.Text = "Copy TestList";
            else if (New)
                wizardPage1.Text = "Create TestList";
            else
                wizardPage1.Text = "Edit TestList";
        }

        private void SelectRow(DataRow dr)
        {
            comboBoxEdit_ProductNumber.SelectedItem = dr[SchemaPDV.PDVTESTLIST_productNumber];
            comboBoxEdit_Product.SelectedItem = dr[SchemaPDV.PDVTESTLIST_product];
            comboBoxEdit_ProductType.SelectedItem = dr[SchemaPDV.PDVTESTLIST_productType];
            comboBoxEdit_Variant.SelectedItem = dr[SchemaPDV.PDVTESTLIST_variant];
            comboBoxEdit_FileStem.SelectedItem = dr[SchemaPDV.PDVTESTLIST_fileStem];
            textEdit_Comment.EditValue = dr[SchemaPDV.PDVTESTLIST_Comment];
        }

        private void wizardControl1_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            completionWizardPage1.FinishText = "You have successfully completed the wizard";
            completionWizardPage1.FinishText += string.Format("\r\n\r\nPartNumber : {0}", comboBoxEdit_ProductNumber.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nProduct : {0}", comboBoxEdit_Product.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nProductType : {0}", comboBoxEdit_ProductType.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nVariant : {0}", comboBoxEdit_Variant.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nFile : {0}", comboBoxEdit_FileStem.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nComment : {0}", textEdit_Comment.EditValue);
        }

        private void comboBoxEdit_ProductNumber_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue.ToString().Length > 10)
                e.Cancel = true;
        }

        private void comboBoxEdit_Variant_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            int intTEST;
            if (!Int32.TryParse(e.NewValue.ToString(), out intTEST))
                e.Cancel = true;
            if (e.NewValue.ToString().Length > 2)
                e.Cancel = true;
        }

        private void comboBoxEdit_Product_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            e.NewValue = e.NewValue.ToString().ToUpper();
            if (e.NewValue.ToString().Length > 2)
                e.Cancel = true;
        }

        private void comboBoxEdit_ProductType_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            e.NewValue = e.NewValue.ToString().ToUpper();
            if (e.NewValue.ToString().Length > 2)
                e.Cancel = true;
        }

        private void comboBoxEdit1_Properties_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue.ToString().Length > 11)
                e.Cancel = true;
        }
    }
}