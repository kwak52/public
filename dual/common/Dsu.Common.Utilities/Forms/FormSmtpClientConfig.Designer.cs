namespace Dsu.Common.Utilities.Forms
{
    partial class FormSmtpClientConfig
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxServer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericTextBoxPort = new Dsu.Common.Utilities.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxUserEmail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.checkBoxEnableSSL = new System.Windows.Forms.CheckBox();
            this.checkBoxSave = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.actionList1 = new Dsu.Common.Utilities.Actions.ActionList(this.components);
            this.action1 = new Dsu.Common.Utilities.Actions.Action(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            // 
            // comboBoxServer
            // 
            this.comboBoxServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxServer.FormattingEnabled = true;
            this.comboBoxServer.Location = new System.Drawing.Point(81, 28);
            this.comboBoxServer.Name = "comboBoxServer";
            this.comboBoxServer.Size = new System.Drawing.Size(161, 20);
            this.comboBoxServer.TabIndex = 1;
            this.toolTip1.SetToolTip(this.comboBoxServer, "Specifiy SMTP server.\\r\\nYou can directly type servername or pick from combo list" +
        "s.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            // 
            // numericTextBoxPort
            // 
            this.numericTextBoxPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericTextBoxPort.Location = new System.Drawing.Point(81, 54);
            this.numericTextBoxPort.Name = "numericTextBoxPort";
            this.numericTextBoxPort.Size = new System.Drawing.Size(161, 21);
            this.numericTextBoxPort.TabIndex = 3;
            this.toolTip1.SetToolTip(this.numericTextBoxPort, "SMTP server port");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "User:";
            // 
            // textBoxUserEmail
            // 
            this.textBoxUserEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUserEmail.Location = new System.Drawing.Point(81, 80);
            this.textBoxUserEmail.Name = "textBoxUserEmail";
            this.textBoxUserEmail.Size = new System.Drawing.Size(161, 21);
            this.textBoxUserEmail.TabIndex = 5;
            this.toolTip1.SetToolTip(this.textBoxUserEmail, "Your email address on SMTP server");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPassword.Location = new System.Drawing.Point(81, 107);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(161, 21);
            this.textBoxPassword.TabIndex = 7;
            this.toolTip1.SetToolTip(this.textBoxPassword, "password for email address");
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // checkBoxEnableSSL
            // 
            this.checkBoxEnableSSL.AutoSize = true;
            this.checkBoxEnableSSL.Checked = true;
            this.checkBoxEnableSSL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableSSL.Location = new System.Drawing.Point(14, 144);
            this.checkBoxEnableSSL.Name = "checkBoxEnableSSL";
            this.checkBoxEnableSSL.Size = new System.Drawing.Size(90, 16);
            this.checkBoxEnableSSL.TabIndex = 8;
            this.checkBoxEnableSSL.Text = "Enable SSL";
            this.checkBoxEnableSSL.UseVisualStyleBackColor = true;
            // 
            // checkBoxSave
            // 
            this.checkBoxSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxSave.AutoSize = true;
            this.checkBoxSave.Checked = true;
            this.checkBoxSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSave.Location = new System.Drawing.Point(106, 210);
            this.checkBoxSave.Name = "checkBoxSave";
            this.checkBoxSave.Size = new System.Drawing.Size(136, 16);
            this.checkBoxSave.TabIndex = 9;
            this.checkBoxSave.Text = "Save current setting";
            this.toolTip1.SetToolTip(this.checkBoxSave, "Save this information to registry for later use.\\r\\nPassword is stored after encr" +
        "yption.");
            this.checkBoxSave.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(167, 232);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(86, 232);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // actionList1
            // 
            this.actionList1.Actions.AddRange(new Dsu.Common.Utilities.Actions.Action[] {
            this.action1});
            this.actionList1.ImageList = null;
            this.actionList1.ShowTextOnToolBar = false;
            this.actionList1.Tag = null;
            // 
            // action1
            // 
            this.action1.Checked = false;
            this.action1.Enabled = true;
            this.action1.Hint = null;
            this.action1.Shortcut = System.Windows.Forms.Shortcut.None;
            this.action1.Tag = null;
            this.action1.Text = "action1";
            this.action1.Visible = true;
            this.action1.Update += new System.EventHandler(this.action1_Update);
            // 
            // FormSmtpClientConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 267);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.checkBoxSave);
            this.Controls.Add(this.checkBoxEnableSSL);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxUserEmail);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericTextBoxPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxServer);
            this.Controls.Add(this.label1);
            this.Name = "FormSmtpClientConfig";
            this.Text = "FormSmtpClientConfig";
            this.Load += new System.EventHandler(this.FormSmtpClientConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxServer;
        private System.Windows.Forms.Label label2;
        private Dsu.Common.Utilities.NumericTextBox numericTextBoxPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxUserEmail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.CheckBox checkBoxEnableSSL;
        private System.Windows.Forms.CheckBox checkBoxSave;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private Dsu.Common.Utilities.Actions.ActionList actionList1;
        private Dsu.Common.Utilities.Actions.Action action1;
    }
}