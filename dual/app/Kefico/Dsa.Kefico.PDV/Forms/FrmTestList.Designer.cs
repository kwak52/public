namespace Dsa.Kefico.PDV.Forms
{
    partial class FrmTestList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTestList));
            this.wizardControl1 = new DevExpress.XtraWizard.WizardControl();
            this.wizardPage1 = new DevExpress.XtraWizard.WizardPage();
            this.comboBoxEdit_FileStem = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit_Comment = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_ProductNumber = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboBoxEdit_Variant = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_ProductType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_Product = new DevExpress.XtraEditors.ComboBoxEdit();
            this.completionWizardPage1 = new DevExpress.XtraWizard.CompletionWizardPage();
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
            this.wizardControl1.SuspendLayout();
            this.wizardPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_FileStem.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Comment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_ProductNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_Variant.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_ProductType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_Product.Properties)).BeginInit();
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
            this.wizardPage1.Controls.Add(this.comboBoxEdit_FileStem);
            this.wizardPage1.Controls.Add(this.labelControl6);
            this.wizardPage1.Controls.Add(this.textEdit_Comment);
            this.wizardPage1.Controls.Add(this.labelControl5);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_ProductNumber);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_Variant);
            this.wizardPage1.Controls.Add(this.labelControl4);
            this.wizardPage1.Controls.Add(this.labelControl3);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_ProductType);
            this.wizardPage1.Controls.Add(this.labelControl2);
            this.wizardPage1.Controls.Add(this.labelControl1);
            this.wizardPage1.Controls.Add(this.comboBoxEdit_Product);
            this.wizardPage1.DescriptionText = "Wizard page subtitle: this should help the Test List Setting";
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(529, 175);
            this.wizardPage1.Text = "Wizard Test List";
            // 
            // comboBoxEdit_FileStem
            // 
            this.comboBoxEdit_FileStem.Location = new System.Drawing.Point(160, 123);
            this.comboBoxEdit_FileStem.Name = "comboBoxEdit_FileStem";
            this.comboBoxEdit_FileStem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_FileStem.Properties.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.comboBoxEdit1_Properties_EditValueChanging);
            this.comboBoxEdit_FileStem.Size = new System.Drawing.Size(350, 20);
            this.comboBoxEdit_FileStem.TabIndex = 16;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(128, 126);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(17, 14);
            this.labelControl6.TabIndex = 15;
            this.labelControl6.Text = "File";
            // 
            // textEdit_Comment
            // 
            this.textEdit_Comment.Location = new System.Drawing.Point(160, 149);
            this.textEdit_Comment.Name = "textEdit_Comment";
            this.textEdit_Comment.Size = new System.Drawing.Size(352, 20);
            this.textEdit_Comment.TabIndex = 12;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(92, 152);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(53, 14);
            this.labelControl5.TabIndex = 13;
            this.labelControl5.Text = "Comment";
            // 
            // comboBoxEdit_ProductNumber
            // 
            this.comboBoxEdit_ProductNumber.Location = new System.Drawing.Point(160, 19);
            this.comboBoxEdit_ProductNumber.Name = "comboBoxEdit_ProductNumber";
            this.comboBoxEdit_ProductNumber.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_ProductNumber.Size = new System.Drawing.Size(350, 20);
            this.comboBoxEdit_ProductNumber.TabIndex = 0;
            this.comboBoxEdit_ProductNumber.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.comboBoxEdit_ProductNumber_EditValueChanging);
            // 
            // comboBoxEdit_Variant
            // 
            this.comboBoxEdit_Variant.Location = new System.Drawing.Point(160, 97);
            this.comboBoxEdit_Variant.Name = "comboBoxEdit_Variant";
            this.comboBoxEdit_Variant.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_Variant.Size = new System.Drawing.Size(350, 20);
            this.comboBoxEdit_Variant.TabIndex = 3;
            this.comboBoxEdit_Variant.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.comboBoxEdit_Variant_EditValueChanging);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(60, 100);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(86, 14);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "Variant (P-VAR)";
            this.labelControl4.ToolTip = "File version (ex : *.v01)";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(5, 22);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(142, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "Product Number (PTTNR)";
            // 
            // comboBoxEdit_ProductType
            // 
            this.comboBoxEdit_ProductType.Location = new System.Drawing.Point(160, 71);
            this.comboBoxEdit_ProductType.Name = "comboBoxEdit_ProductType";
            this.comboBoxEdit_ProductType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_ProductType.Size = new System.Drawing.Size(350, 20);
            this.comboBoxEdit_ProductType.TabIndex = 2;
            this.comboBoxEdit_ProductType.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.comboBoxEdit_ProductType_EditValueChanging);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(19, 74);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(127, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Product Type (PTTYP)";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(43, 48);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(103, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Product (PTPROD)";
            // 
            // comboBoxEdit_Product
            // 
            this.comboBoxEdit_Product.Location = new System.Drawing.Point(160, 45);
            this.comboBoxEdit_Product.Name = "comboBoxEdit_Product";
            this.comboBoxEdit_Product.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_Product.Size = new System.Drawing.Size(350, 20);
            this.comboBoxEdit_Product.TabIndex = 1;
            this.comboBoxEdit_Product.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.comboBoxEdit_Product_EditValueChanging);
            // 
            // completionWizardPage1
            // 
            this.completionWizardPage1.Name = "completionWizardPage1";
            this.completionWizardPage1.Size = new System.Drawing.Size(344, 187);
            // 
            // FrmTestList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 320);
            this.ControlBox = false;
            this.Controls.Add(this.wizardControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmTestList";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Wizard Test List";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmEntry_FormClosed);
            this.Load += new System.EventHandler(this.FrmEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).EndInit();
            this.wizardControl1.ResumeLayout(false);
            this.wizardPage1.ResumeLayout(false);
            this.wizardPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_FileStem.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Comment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_ProductNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_Variant.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_ProductType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_Product.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraWizard.WizardControl wizardControl1;
        private DevExpress.XtraWizard.CompletionWizardPage completionWizardPage1;
        private DevExpress.XtraWizard.WizardPage wizardPage1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_Product;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_ProductType;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_ProductNumber;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_Variant;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit textEdit_Comment;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_FileStem;
    }
}