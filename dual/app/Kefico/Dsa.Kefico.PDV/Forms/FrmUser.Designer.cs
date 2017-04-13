namespace Dsa.Kefico.PDV.Forms
{
    partial class FrmUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUser));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.textEdit_Password = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_User = new DevExpress.XtraEditors.ComboBoxEdit();
            this.simpleButton_Enter = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl_Result = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Password.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_User.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl_Result);
            this.panelControl1.Controls.Add(this.simpleButton_Enter);
            this.panelControl1.Controls.Add(this.textEdit_Password);
            this.panelControl1.Controls.Add(this.labelControl10);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.comboBoxEdit_User);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(343, 96);
            this.panelControl1.TabIndex = 0;
            // 
            // textEdit_Password
            // 
            this.textEdit_Password.EditValue = "";
            this.textEdit_Password.Location = new System.Drawing.Point(115, 48);
            this.textEdit_Password.Name = "textEdit_Password";
            this.textEdit_Password.Size = new System.Drawing.Size(148, 20);
            this.textEdit_Password.TabIndex = 34;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(58, 51);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(51, 14);
            this.labelControl10.TabIndex = 33;
            this.labelControl10.Text = "Password";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(51, 25);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(62, 14);
            this.labelControl6.TabIndex = 32;
            this.labelControl6.Text = "Select User";
            // 
            // comboBoxEdit_User
            // 
            this.comboBoxEdit_User.Location = new System.Drawing.Point(115, 22);
            this.comboBoxEdit_User.Name = "comboBoxEdit_User";
            this.comboBoxEdit_User.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_User.Size = new System.Drawing.Size(148, 20);
            this.comboBoxEdit_User.TabIndex = 31;
            // 
            // simpleButton_Enter
            // 
            this.simpleButton_Enter.Location = new System.Drawing.Point(275, 22);
            this.simpleButton_Enter.Name = "simpleButton_Enter";
            this.simpleButton_Enter.Size = new System.Drawing.Size(54, 46);
            this.simpleButton_Enter.TabIndex = 36;
            this.simpleButton_Enter.Text = "Enter";
            this.simpleButton_Enter.Click += new System.EventHandler(this.simpleButton_Enter_Click);
            // 
            // labelControl_Result
            // 
            this.labelControl_Result.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl_Result.Appearance.Options.UseForeColor = true;
            this.labelControl_Result.Location = new System.Drawing.Point(115, 74);
            this.labelControl_Result.Name = "labelControl_Result";
            this.labelControl_Result.Size = new System.Drawing.Size(0, 14);
            this.labelControl_Result.TabIndex = 37;
            // 
            // FrmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 96);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(359, 134);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(359, 134);
            this.Name = "FrmUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enter User";
            this.Load += new System.EventHandler(this.FrmUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Password.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_User.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit textEdit_Password;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_User;
        private DevExpress.XtraEditors.SimpleButton simpleButton_Enter;
        private DevExpress.XtraEditors.LabelControl labelControl_Result;
    }
}