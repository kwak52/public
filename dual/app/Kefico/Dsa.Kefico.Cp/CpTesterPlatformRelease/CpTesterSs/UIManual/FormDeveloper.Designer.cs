namespace CpTesterPlatform.CpTester
{
    partial class FormDeveloper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeveloper));
            this.textEdit_Value = new DevExpress.XtraEditors.TextEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton_close = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_LayoutInitial = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Value.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textEdit_Value
            // 
            this.textEdit_Value.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textEdit_Value.Location = new System.Drawing.Point(175, 16);
            this.textEdit_Value.Name = "textEdit_Value";
            this.textEdit_Value.Properties.Appearance.Options.UseFont = true;
            this.textEdit_Value.Size = new System.Drawing.Size(103, 20);
            this.textEdit_Value.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.simpleButton_close);
            this.panel1.Controls.Add(this.simpleButton_LayoutInitial);
            this.panel1.Controls.Add(this.simpleButtonOK);
            this.panel1.Controls.Add(this.textEdit_Value);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 89);
            this.panel1.TabIndex = 14;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(30, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(133, 14);
            this.labelControl1.TabIndex = 15;
            this.labelControl1.Text = "MSA mode password:";
            // 
            // simpleButton_close
            // 
            this.simpleButton_close.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton_close.Image")));
            this.simpleButton_close.Location = new System.Drawing.Point(134, 53);
            this.simpleButton_close.Name = "simpleButton_close";
            this.simpleButton_close.Size = new System.Drawing.Size(92, 24);
            this.simpleButton_close.TabIndex = 14;
            this.simpleButton_close.Text = "Close";
            this.simpleButton_close.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton_LayoutInitial
            // 
            this.simpleButton_LayoutInitial.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton_LayoutInitial.Image")));
            this.simpleButton_LayoutInitial.Location = new System.Drawing.Point(236, 53);
            this.simpleButton_LayoutInitial.Name = "simpleButton_LayoutInitial";
            this.simpleButton_LayoutInitial.Size = new System.Drawing.Size(99, 24);
            this.simpleButton_LayoutInitial.TabIndex = 14;
            this.simpleButton_LayoutInitial.Text = "Layout initial";
            this.simpleButton_LayoutInitial.Click += new System.EventHandler(this.simpleButton_LayoutInitial_Click);
            // 
            // simpleButtonOK
            // 
            this.simpleButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonOK.Image")));
            this.simpleButtonOK.Location = new System.Drawing.Point(30, 53);
            this.simpleButtonOK.Name = "simpleButtonOK";
            this.simpleButtonOK.Size = new System.Drawing.Size(92, 24);
            this.simpleButtonOK.TabIndex = 14;
            this.simpleButtonOK.Text = "OK";
            this.simpleButtonOK.Click += new System.EventHandler(this.simpleButtonOK_Click);
            // 
            // FormDeveloper
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(347, 89);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDeveloper";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmDeveloper";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_Value.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textEdit_Value;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton_close;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOK;
        private DevExpress.XtraEditors.SimpleButton simpleButton_LayoutInitial;
    }
}