using DevExpress.XtraEditors;
using Dsa.Kefico.PDV.EventHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Dsa.Kefico.PDV.Forms
{

    public partial class FrmRelease : DevExpress.XtraEditors.XtraForm
    {
        public UEventHandlerNewRelease UEventNewRelease;

        public bool New { get; private set; } = false;
        public bool Copy { get; private set; } = false;

        List<string> _lstGroup = new List<string>();
        List<string> _lstTestList = new List<string>();
        List<string> _lstPartNumber = new List<string>();
        List<string> _lstProductNumber = new List<string>();

        List<string> _lstPamGroup = new List<string>();
        List<string> _lstPamType = new List<string>();
        List<string> _lstDataConfig = new List<string>();
        List<string> _lstDataVariant = new List<string>();
        DataRow _drSelect;

        public int PdvId { get; set; }
        public int ProductGroupId { get; set; }
        public int TestListId { get; set; }
        public string PartNumber { get; set; }
        public string PamGroup { get; set; }
        public string PamType { get; set; }
        public string DataConfig { get; set; }
        public string DataVariant { get; set; }
        public string ChangeNumber { get; set; }
        public string Comment { get; set; }

        public FrmRelease(bool create, int id, List<string> lstGroup, List<string> lstTestList, List<string> lstProductNumber, List<string> lstPartNumber
            , List<string> lstPamGroup, List<string> lstPamType, List<string> lstDataConfig, List<string> lstDataVariant, DataRow drSelect = null)
        {
            New = create;
            if (create && drSelect != null)
                Copy = true;

            _lstGroup = lstGroup;
            _lstTestList = lstTestList;
            _lstProductNumber = lstProductNumber;
            _lstPartNumber = lstPartNumber;
            _lstPamGroup = lstPamGroup;
            _lstPamType = lstPamType;
            _lstDataConfig = lstDataConfig;
            _lstDataVariant = lstDataVariant;
            _drSelect = drSelect;

            PdvId = id;

            InitializeComponent();
        }

        public void UpdateEntry(List<string> lstGroup)
        {
            comboBoxEdit_GroupId.Properties.Items.Clear();
            comboBoxEdit_Group.Properties.Items.Clear();
            _lstGroup = lstGroup;
            foreach (string s in _lstGroup)
            {
                comboBoxEdit_GroupId.Properties.Items.Add(s.Split(':')[0]);
                comboBoxEdit_Group.Properties.Items.Add(s.Split(':')[1]);
            }
        }

        public void UpdateTestList(List<string> lstTestList, List<string> lstProductNumber, string ProductNumber)
        {
            if (New)
            {
                _lstTestList = lstTestList;
                _lstProductNumber = lstProductNumber;
                comboBoxEdit_TestListFilter.Properties.Items.Clear();
                comboBoxEdit_TestListFilter.Properties.Items.AddRange(_lstProductNumber);
                comboBoxEdit_TestListFilter.SelectedItem = ProductNumber;
            }
        }

        private void FrmEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
                this.DialogResult = DialogResult.Cancel;
        }

        private void wizardControl1_FinishClick(object sender, CancelEventArgs e)
        {
            if (comboBoxEdit_PartNumber.Text == ""
                || comboBoxEdit_TestList.Text == ""
                || comboBoxEdit_PamGroup.Text == ""
                || comboBoxEdit_PamType.Text == ""
                || comboBoxEdit_AddDataConfig.Text == ""
                || textEdit_ChangeNumber.Text == ""
                || comboBoxEdit_AddDataVar.Text == "")
            {
                e.Cancel = true;
                XtraMessageBox.Show(this, "Please enter all fields except comment", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBoxEdit_PartNumber.Text.Length != 10)
            {
                e.Cancel = true;
                XtraMessageBox.Show(this, "Please enter 10 characters PartNumber", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {


                this.DialogResult = DialogResult.OK;

                ProductGroupId = Convert.ToInt32(comboBoxEdit_GroupId.EditValue.ToString());
                TestListId = Convert.ToInt32(comboBoxEdit_TestListId.EditValue.ToString());

                PartNumber = comboBoxEdit_PartNumber.EditValue.ToString();
                PamGroup = comboBoxEdit_PamGroup.EditValue.ToString();
                PamType = comboBoxEdit_PamType.EditValue.ToString();
                DataConfig = comboBoxEdit_AddDataConfig.EditValue.ToString();
                DataVariant = comboBoxEdit_AddDataVar.EditValue.ToString();
                Comment = textEdit_Comment.Text;
                ChangeNumber = textEdit_ChangeNumber.Text;

                this.Close();
            }
        }

        private void wizardControl1_CancelClick(object sender, CancelEventArgs e)
        {
            this.Close();
        }

        private void FrmEntry_Load(object sender, EventArgs e)
        {
            foreach (string s in _lstGroup)
            {
                comboBoxEdit_GroupId.Properties.Items.Add(s.Split(':')[0]);
                comboBoxEdit_Group.Properties.Items.Add(s.Split(':')[1]);
            }

            comboBoxEdit_TestListFilter.Properties.Items.AddRange(_lstProductNumber);
            comboBoxEdit_TestListFilter.SelectedIndex = 0;
            comboBoxEdit_PartNumber.Properties.Items.AddRange(_lstPartNumber);
            comboBoxEdit_PamGroup.Properties.Items.AddRange(_lstPamGroup);
            comboBoxEdit_PamType.Properties.Items.AddRange(_lstPamType);
            comboBoxEdit_AddDataConfig.Properties.Items.AddRange(_lstDataConfig);
            comboBoxEdit_AddDataVar.Properties.Items.AddRange(_lstDataVariant);

            if (_drSelect != null)
                SelectRow(_drSelect);

            comboBoxEdit_PartNumber.ReadOnly = !New;
            comboBoxEdit_PamType.ReadOnly = !New;
            comboBoxEdit_TestListFilter.ReadOnly = !New;
            comboBoxEdit_TestList.ReadOnly = !New;

            if (Copy)
                wizardPage1.Text = "Copy Release";
            else if (New)
                wizardPage1.Text = "Create Release";
            else
                wizardPage1.Text = "Edit Release";
        }

        private void wizardControl1_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            completionWizardPage1.FinishText = "You have successfully completed the wizard";
            completionWizardPage1.FinishText += string.Format("\r\n\r\nGroup : {0}", comboBoxEdit_Group.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nTestList : {0}", comboBoxEdit_TestList.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nPartNumber : {0}", comboBoxEdit_PartNumber.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nPamGroup : {0}", comboBoxEdit_PamGroup.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nPamType : {0}", comboBoxEdit_PamType.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nAddDataConfig : {0}", comboBoxEdit_AddDataConfig.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nAddDataVariant : {0}", comboBoxEdit_AddDataVar.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nChangeNumber : {0}", textEdit_ChangeNumber.EditValue);
            completionWizardPage1.FinishText += string.Format("\r\nComment : {0}", textEdit_Comment.EditValue);
        }

        private void comboBoxEdit_PartNumber_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue.ToString().Length > 10)
                e.Cancel = true;
        }

        private void comboBoxEdit_PamType_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            e.NewValue = e.NewValue.ToString().ToUpper();
            if (e.NewValue.ToString().Length > 2)
                e.Cancel = true;
        }

        private void comboBoxEdit_TestListFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTestListComboBox();
        }

        private void UpdateTestListComboBox()
        {
            comboBoxEdit_TestList.Properties.Items.Clear();
            comboBoxEdit_TestListId.Properties.Items.Clear();
            foreach (string s in _lstTestList)
            {
                if (s.Split(':')[1].Split(' ')[0] == comboBoxEdit_TestListFilter.Text)
                {
                    comboBoxEdit_TestListId.Properties.Items.Add(s.Split(':')[0]);
                    comboBoxEdit_TestList.Properties.Items.Add(s.Split(':')[1]);
                }
            }
            comboBoxEdit_TestList.SelectedIndex = 0;
        }

        private void SelectRow(DataRow dr)
        {
            comboBoxEdit_PartNumber.SelectedItem = dr[SchemaPDV.PDVVIEW_partNumber];
            comboBoxEdit_Group.SelectedItem = dr[SchemaPDV.PDVVIEW_groupEntry];
            comboBoxEdit_PamGroup.SelectedItem = dr[SchemaPDV.PDVVIEW_pamGroup];
            comboBoxEdit_PamType.SelectedItem = dr[SchemaPDV.PDVVIEW_pamType];
            comboBoxEdit_AddDataConfig.SelectedItem = dr[SchemaPDV.PDVVIEW_dataConfig];
            comboBoxEdit_AddDataVar.SelectedItem = dr[SchemaPDV.PDVVIEW_dataVariant];
            comboBoxEdit_TestListFilter.EditValue = dr[SchemaPDV.PDVVIEW_testList].ToString().Split(' ')[0];
            UpdateTestListComboBox();
            comboBoxEdit_TestList.SelectedItem = dr[SchemaPDV.PDVVIEW_testList];
            textEdit_Comment.EditValue = dr[SchemaPDV.PDVVIEW_Comment];
            textEdit_ChangeNumber.EditValue = dr[SchemaPDV.PDVVIEW_ChangeNumber];
        }

        private void comboBoxEdit_Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit_GroupId.SelectedIndex = comboBoxEdit_Group.SelectedIndex;
        }

        private void comboBoxEdit_TestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxEdit_TestListId.SelectedIndex = comboBoxEdit_TestList.SelectedIndex;
        }

        private void simpleButton_NewTestList_Click(object sender, EventArgs e)
        {
            UEventNewRelease?.Invoke(sender);
        }
    }
}