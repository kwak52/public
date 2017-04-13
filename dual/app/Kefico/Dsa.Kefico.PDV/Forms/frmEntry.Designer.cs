namespace Dsa.Kefico.PDV.Forms
{
    partial class FrmEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEntry));
            this.wizardControl1 = new DevExpress.XtraWizard.WizardControl();
            this.wizardPage1 = new DevExpress.XtraWizard.WizardPage();
            this.textEdit_Comment = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit_Id = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_ProductModel = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_Group = new DevExpress.XtraEditors.ComboBoxEdit();
            this.completionWizardPage1 = new DevExpress.XtraWizard.CompletionWizardPage();
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
            this.wizardControl1.SuspendLayout();
            this.wizardPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Comment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_ProductModel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_Group.Properties)).BeginInit();
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
            this.wizardPage1.Controls.Add(this.textEdit_Comment);
            this.wizardPage1.Controls.Add(this.labelControl5);
            this.wizardPage1.Controls.Add(this.textEdit_Id);
            this.wizardPage1.Controls.Add(this.labelControl3);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_ProductModel);
            this.wizardPage1.Controls.Add(this.labelControl2);
            this.wizardPage1.Controls.Add(this.labelControl1);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_Group);
            this.wizardPage1.DescriptionText = "Wizard page subtitle: this should help the PDV Group Setting";
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(529, 175);
            this.wizardPage1.Text = "Wizard Entry";
            // 
            // textEdit_Comment
            // 
            this.textEdit_Comment.Location = new System.Drawing.Point(127, 103);
            this.textEdit_Comment.Name = "textEdit_Comment";
            this.textEdit_Comment.Size = new System.Drawing.Size(368, 20);
            this.textEdit_Comment.TabIndex = 3;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(61, 106);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(53, 14);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "Comment";
            // 
            // textEdit_Id
            // 
            this.textEdit_Id.Location = new System.Drawing.Point(127, 21);
            this.textEdit_Id.Name = "textEdit_Id";
            this.textEdit_Id.Properties.ReadOnly = true;
            this.textEdit_Id.Size = new System.Drawing.Size(368, 20);
            this.textEdit_Id.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(86, 24);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(30, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "PRNR";
            // 
            // comboBoxEdit_ProductModel
            // 
            this.comboBoxEdit_ProductModel.Location = new System.Drawing.Point(127, 77);
            this.comboBoxEdit_ProductModel.Name = "comboBoxEdit_ProductModel";
            this.comboBoxEdit_ProductModel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_ProductModel.Size = new System.Drawing.Size(368, 20);
            this.comboBoxEdit_ProductModel.TabIndex = 2;
            this.comboBoxEdit_ProductModel.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.comboBoxEdit_ProductModel_EditValueChanging);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(37, 80);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(79, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Product Model";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(36, 52);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(80, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Product Group";
            // 
            // comboBoxEdit_Group
            // 
            this.comboBoxEdit_Group.Location = new System.Drawing.Point(127, 49);
            this.comboBoxEdit_Group.Name = "comboBoxEdit_Group";
            this.comboBoxEdit_Group.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_Group.Size = new System.Drawing.Size(368, 20);
            this.comboBoxEdit_Group.TabIndex = 1;
            this.comboBoxEdit_Group.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.comboBoxEdit_Group_EditValueChanging);
            // 
            // completionWizardPage1
            // 
            this.completionWizardPage1.Name = "completionWizardPage1";
            this.completionWizardPage1.Size = new System.Drawing.Size(344, 187);
            // 
            // FrmEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 320);
            this.ControlBox = false;
            this.Controls.Add(this.wizardControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmEntry";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Wizard Entry";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmEntry_FormClosed);
            this.Load += new System.EventHandler(this.FrmEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).EndInit();
            this.wizardControl1.ResumeLayout(false);
            this.wizardPage1.ResumeLayout(false);
            this.wizardPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Comment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_ProductModel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_Group.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraWizard.WizardControl wizardControl1;
        private DevExpress.XtraWizard.CompletionWizardPage completionWizardPage1;
        private DevExpress.XtraWizard.WizardPage wizardPage1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_Group;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_ProductModel;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEdit_Id;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit textEdit_Comment;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}