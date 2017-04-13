namespace Dsu.LicenseKeyChecker
{
    partial class FrmLicense
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLicense));
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.txtActivationCode = new System.Windows.Forms.TextBox();
            this.txtActivationKey = new System.Windows.Forms.TextBox();
            this.lblActivationKey = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblActivationCode = new System.Windows.Forms.Label();
            this.lblAlertMessage = new System.Windows.Forms.Label();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.pnlMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMessage
            // 
            this.pnlMessage.BackColor = System.Drawing.Color.White;
            this.pnlMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMessage.Controls.Add(this.btnCancel);
            this.pnlMessage.Controls.Add(this.lblMessage);
            this.pnlMessage.Controls.Add(this.btnOK);
            this.pnlMessage.Controls.Add(this.txtProduct);
            this.pnlMessage.Controls.Add(this.txtActivationCode);
            this.pnlMessage.Controls.Add(this.txtActivationKey);
            this.pnlMessage.Controls.Add(this.lblActivationKey);
            this.pnlMessage.Controls.Add(this.lblProduct);
            this.pnlMessage.Controls.Add(this.lblActivationCode);
            this.pnlMessage.Controls.Add(this.lblAlertMessage);
            this.pnlMessage.Controls.Add(this.pnlImage);
            this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMessage.Location = new System.Drawing.Point(0, 0);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(367, 197);
            this.pnlMessage.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(279, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 27);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblMessage.Enabled = false;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 8.25F);
            this.lblMessage.Location = new System.Drawing.Point(67, 15);
            this.lblMessage.Multiline = true;
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(273, 30);
            this.lblMessage.TabIndex = 11;
            this.lblMessage.Text = "To request a license, please contact ahn@dualsoft.co.kr";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(191, 154);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 27);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtProduct
            // 
            this.txtProduct.BackColor = System.Drawing.SystemColors.Control;
            this.txtProduct.Location = new System.Drawing.Point(106, 64);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.ReadOnly = true;
            this.txtProduct.Size = new System.Drawing.Size(246, 21);
            this.txtProduct.TabIndex = 11;
            // 
            // txtActivationCode
            // 
            this.txtActivationCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtActivationCode.Location = new System.Drawing.Point(106, 91);
            this.txtActivationCode.Name = "txtActivationCode";
            this.txtActivationCode.ReadOnly = true;
            this.txtActivationCode.Size = new System.Drawing.Size(246, 21);
            this.txtActivationCode.TabIndex = 11;
            // 
            // txtActivationKey
            // 
            this.txtActivationKey.Location = new System.Drawing.Point(106, 118);
            this.txtActivationKey.Name = "txtActivationKey";
            this.txtActivationKey.Size = new System.Drawing.Size(246, 21);
            this.txtActivationKey.TabIndex = 11;
            this.txtActivationKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtActivationKey_KeyDown);
            // 
            // lblActivationKey
            // 
            this.lblActivationKey.AutoSize = true;
            this.lblActivationKey.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivationKey.Location = new System.Drawing.Point(18, 120);
            this.lblActivationKey.Name = "lblActivationKey";
            this.lblActivationKey.Size = new System.Drawing.Size(67, 14);
            this.lblActivationKey.TabIndex = 10;
            this.lblActivationKey.Text = "License Key";
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProduct.Location = new System.Drawing.Point(19, 66);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(44, 14);
            this.lblProduct.TabIndex = 8;
            this.lblProduct.Text = "Product";
            // 
            // lblActivationCode
            // 
            this.lblActivationCode.AutoSize = true;
            this.lblActivationCode.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivationCode.Location = new System.Drawing.Point(18, 93);
            this.lblActivationCode.Name = "lblActivationCode";
            this.lblActivationCode.Size = new System.Drawing.Size(59, 14);
            this.lblActivationCode.TabIndex = 8;
            this.lblActivationCode.Text = "Machine ID";
            // 
            // lblAlertMessage
            // 
            this.lblAlertMessage.AutoSize = true;
            this.lblAlertMessage.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlertMessage.ForeColor = System.Drawing.Color.Red;
            this.lblAlertMessage.Location = new System.Drawing.Point(107, 194);
            this.lblAlertMessage.Name = "lblAlertMessage";
            this.lblAlertMessage.Size = new System.Drawing.Size(0, 14);
            this.lblAlertMessage.TabIndex = 3;
            // 
            // pnlImage
            // 
            this.pnlImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlImage.BackgroundImage")));
            this.pnlImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlImage.Location = new System.Drawing.Point(27, 14);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(34, 32);
            this.pnlImage.TabIndex = 2;
            // 
            // FrmLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 197);
            this.ControlBox = false;
            this.Controls.Add(this.pnlMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLicense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dsu License";
            this.Load += new System.EventHandler(this.FrmLicense_Load);
            this.pnlMessage.ResumeLayout(false);
            this.pnlMessage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMessage;
        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.Label lblActivationKey;
        private System.Windows.Forms.Label lblActivationCode;
        private System.Windows.Forms.TextBox txtActivationCode;
        private System.Windows.Forms.TextBox txtActivationKey;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblAlertMessage;
        private System.Windows.Forms.TextBox lblMessage;
    }
}