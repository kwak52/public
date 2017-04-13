namespace Dsa.Kefico.PDV.Forms
{
    partial class FrmRelease
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRelease));
            this.wizardControl1 = new DevExpress.XtraWizard.WizardControl();
            this.wizardPage1 = new DevExpress.XtraWizard.WizardPage();
            this.simpleButton_NewTestList = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit_ChangeNumber = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_TestListId = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboBoxEdit_GroupId = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboBoxEdit_TestListFilter = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_TestList = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit_Comment = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_AddDataVar = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_AddDataConfig = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_PamGroup = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_PamType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_Group = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboBoxEdit_PartNumber = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.completionWizardPage1 = new DevExpress.XtraWizard.CompletionWizardPage();
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
            this.wizardControl1.SuspendLayout();
            this.wizardPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_ChangeNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_TestListId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_GroupId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_TestListFilter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_TestList.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Comment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_AddDataVar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_AddDataConfig.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_PamGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_PamType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_Group.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_PartNumber.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.Controls.Add(this.wizardPage1);
            this.wizardControl1.Controls.Add(this.completionWizardPage1);
            this.wizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl1.Location = new System.Drawing.Point(0, 0);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.Pages.AddRange(new DevExpress.XtraWizard.BaseWizardPage[] {
            this.wizardPage1,
            this.completionWizardPage1});
            this.wizardControl1.Size = new System.Drawing.Size(561, 320);
            this.wizardControl1.CancelClick += new System.ComponentModel.CancelEventHandler(this.wizardControl1_CancelClick);
            this.wizardControl1.FinishClick += new System.ComponentModel.CancelEventHandler(this.wizardControl1_FinishClick);
            this.wizardControl1.NextClick += new DevExpress.XtraWizard.WizardCommandButtonClickEventHandler(this.wizardControl1_NextClick);
            // 
            // wizardPage1
            // 
            this.wizardPage1.Controls.Add(this.simpleButton_NewTestList);
            this.wizardPage1.Controls.Add(this.textEdit_ChangeNumber);
            this.wizardPage1.Controls.Add(this.labelControl10);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_TestListId);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_GroupId);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_TestListFilter);
            this.wizardPage1.Controls.Add(this.labelControl8);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_TestList);
            this.wizardPage1.Controls.Add(this.labelControl2);
            this.wizardPage1.Controls.Add(this.textEdit_Comment);
            this.wizardPage1.Controls.Add(this.labelControl1);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_AddDataVar);
            this.wizardPage1.Controls.Add(this.labelControl7);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_AddDataConfig);
            this.wizardPage1.Controls.Add(this.labelControl4);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_PamGroup);
            this.wizardPage1.Controls.Add(this.labelControl5);
            this.wizardPage1.Controls.Add(this.labelControl6);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_PamType);
            this.wizardPage1.Controls.Add(this.labelControl9);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_Group);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_PartNumber);
            this.wizardPage1.Controls.Add(this.labelControl3);
            this.wizardPage1.DescriptionText = "Wizard page subtitle: this should help the PDV Release Setting";
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(529, 175);
            this.wizardPage1.Text = "Wizard Release PartNumber";
            // 
            // simpleButton_NewTestList
            // 
            this.simpleButton_NewTestList.Location = new System.Drawing.Point(6, 38);
            this.simpleButton_NewTestList.Name = "simpleButton_NewTestList";
            this.simpleButton_NewTestList.Size = new System.Drawing.Size(39, 18);
            this.simpleButton_NewTestList.TabIndex = 8;
            this.simpleButton_NewTestList.Text = "New";
            this.simpleButton_NewTestList.Click += new System.EventHandler(this.simpleButton_NewTestList_Click);
            // 
            // textEdit_ChangeNumber
            // 
            this.textEdit_ChangeNumber.EditValue = "XXSAXXXX";
            this.textEdit_ChangeNumber.Location = new System.Drawing.Point(125, 141);
            this.textEdit_ChangeNumber.Name = "textEdit_ChangeNumber";
            this.textEdit_ChangeNumber.Size = new System.Drawing.Size(132, 20);
            this.textEdit_ChangeNumber.TabIndex = 30;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(30, 144);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(88, 14);
            this.labelControl10.TabIndex = 29;
            this.labelControl10.Text = "Change Number";
            // 
            // comboBoxEdit_TestListId
            // 
            this.comboBoxEdit_TestListId.Location = new System.Drawing.Point(450, 29);
            this.comboBoxEdit_TestListId.Name = "comboBoxEdit_TestListId";
            this.comboBoxEdit_TestListId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_TestListId.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEdit_TestListId.Size = new System.Drawing.Size(50, 20);
            this.comboBoxEdit_TestListId.TabIndex = 28;
            this.comboBoxEdit_TestListId.Visible = false;
            // 
            // comboBoxEdit_GroupId
            // 
            this.comboBoxEdit_GroupId.Location = new System.Drawing.Point(450, 3);
            this.comboBoxEdit_GroupId.Name = "comboBoxEdit_GroupId";
            this.comboBoxEdit_GroupId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_GroupId.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEdit_GroupId.Size = new System.Drawing.Size(50, 20);
            this.comboBoxEdit_GroupId.TabIndex = 27;
            this.comboBoxEdit_GroupId.Visible = false;
            // 
            // comboBoxEdit_TestListFilter
            // 
            this.comboBoxEdit_TestListFilter.Location = new System.Drawing.Point(125, 37);
            this.comboBoxEdit_TestListFilter.Name = "comboBoxEdit_TestListFilter";
            this.comboBoxEdit_TestListFilter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_TestListFilter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEdit_TestListFilter.Size = new System.Drawing.Size(132, 20);
            this.comboBoxEdit_TestListFilter.TabIndex = 1;
            this.comboBoxEdit_TestListFilter.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit_TestListFilter_SelectedIndexChanged);
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(267, 40);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(43, 14);
            this.labelControl8.TabIndex = 25;
            this.labelControl8.Text = "TestList";
            // 
            // comboBoxEdit_TestList
            // 
            this.comboBoxEdit_TestList.Location = new System.Drawing.Point(316, 37);
            this.comboBoxEdit_TestList.Name = "comboBoxEdit_TestList";
            this.comboBoxEdit_TestList.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_TestList.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEdit_TestList.Size = new System.Drawing.Size(199, 20);
            this.comboBoxEdit_TestList.TabIndex = 2;
            this.comboBoxEdit_TestList.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit_TestList_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(48, 40);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(71, 14);
            this.labelControl2.TabIndex = 23;
            this.labelControl2.Text = "TestList filter";
            // 
            // textEdit_Comment
            // 
            this.textEdit_Comment.Location = new System.Drawing.Point(383, 141);
            this.textEdit_Comment.Name = "textEdit_Comment";
            this.textEdit_Comment.Size = new System.Drawing.Size(132, 20);
            this.textEdit_Comment.TabIndex = 9;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(324, 144);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(53, 14);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "Comment";
            // 
            // comboBoxEdit_AddDataVar
            // 
            this.comboBoxEdit_AddDataVar.Location = new System.Drawing.Point(383, 115);
            this.comboBoxEdit_AddDataVar.Name = "comboBoxEdit_AddDataVar";
            this.comboBoxEdit_AddDataVar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_AddDataVar.Size = new System.Drawing.Size(132, 20);
            this.comboBoxEdit_AddDataVar.TabIndex = 8;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(302, 118);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(75, 14);
            this.labelControl7.TabIndex = 19;
            this.labelControl7.Text = "Additional Var";
            // 
            // comboBoxEdit_AddDataConfig
            // 
            this.comboBoxEdit_AddDataConfig.Location = new System.Drawing.Point(383, 89);
            this.comboBoxEdit_AddDataConfig.Name = "comboBoxEdit_AddDataConfig";
            this.comboBoxEdit_AddDataConfig.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_AddDataConfig.Size = new System.Drawing.Size(132, 20);
            this.comboBoxEdit_AddDataConfig.TabIndex = 7;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(286, 92);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(91, 14);
            this.labelControl4.TabIndex = 17;
            this.labelControl4.Text = "Additional Config";
            // 
            // comboBoxEdit_PamGroup
            // 
            this.comboBoxEdit_PamGroup.Location = new System.Drawing.Point(125, 115);
            this.comboBoxEdit_PamGroup.Name = "comboBoxEdit_PamGroup";
            this.comboBoxEdit_PamGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_PamGroup.Size = new System.Drawing.Size(132, 20);
            this.comboBoxEdit_PamGroup.TabIndex = 5;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(58, 118);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(61, 14);
            this.labelControl5.TabIndex = 15;
            this.labelControl5.Text = "PAM Group";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(62, 92);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(56, 14);
            this.labelControl6.TabIndex = 14;
            this.labelControl6.Text = "PAM Type";
            // 
            // comboBoxEdit_PamType
            // 
            this.comboBoxEdit_PamType.Location = new System.Drawing.Point(125, 89);
            this.comboBoxEdit_PamType.Name = "comboBoxEdit_PamType";
            this.comboBoxEdit_PamType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_PamType.Size = new System.Drawing.Size(132, 20);
            this.comboBoxEdit_PamType.TabIndex = 4;
            this.comboBoxEdit_PamType.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.comboBoxEdit_PamType_EditValueChanging);
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(39, 14);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(80, 14);
            this.labelControl9.TabIndex = 12;
            this.labelControl9.Text = "Product Group";
            // 
            // comboBoxEdit_Group
            // 
            this.comboBoxEdit_Group.Location = new System.Drawing.Point(125, 11);
            this.comboBoxEdit_Group.Name = "comboBoxEdit_Group";
            this.comboBoxEdit_Group.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_Group.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEdit_Group.Size = new System.Drawing.Size(390, 20);
            this.comboBoxEdit_Group.TabIndex = 0;
            this.comboBoxEdit_Group.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit_Group_SelectedIndexChanged);
            // 
            // comboBoxEdit_PartNumber
            // 
            this.comboBoxEdit_PartNumber.Location = new System.Drawing.Point(125, 63);
            this.comboBoxEdit_PartNumber.Name = "comboBoxEdit_PartNumber";
            this.comboBoxEdit_PartNumber.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.comboBoxEdit_PartNumber.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_PartNumber.Size = new System.Drawing.Size(390, 20);
            this.comboBoxEdit_PartNumber.TabIndex = 3;
            this.comboBoxEdit_PartNumber.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.comboBoxEdit_PartNumber_EditValueChanging);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(3, 66);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(116, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "Part Number(FTTNR)";
            // 
            // completionWizardPage1
            // 
            this.completionWizardPage1.Name = "completionWizardPage1";
            this.completionWizardPage1.Size = new System.Drawing.Size(344, 187);
            // 
            // FrmRelease
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 320);
            this.ControlBox = false;
            this.Controls.Add(this.wizardControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmRelease";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Wizard Release";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmEntry_FormClosed);
            this.Load += new System.EventHandler(this.FrmEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).EndInit();
            this.wizardControl1.ResumeLayout(false);
            this.wizardPage1.ResumeLayout(false);
            this.wizardPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_ChangeNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_TestListId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_GroupId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_TestListFilter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_TestList.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Comment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_AddDataVar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_AddDataConfig.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_PamGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_PamType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_Group.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_PartNumber.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraWizard.WizardControl wizardControl1;
        private DevExpress.XtraWizard.CompletionWizardPage completionWizardPage1;
        private DevExpress.XtraWizard.WizardPage wizardPage1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_PartNumber;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_Group;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_AddDataVar;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_AddDataConfig;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_PamGroup;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_PamType;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_TestListFilter;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_TestList;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEdit_Comment;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_TestListId;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_GroupId;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.TextEdit textEdit_ChangeNumber;
        private DevExpress.XtraEditors.SimpleButton simpleButton_NewTestList;
    }
}