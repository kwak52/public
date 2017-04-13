namespace DsuLicenseGenerator
{
    partial class FrmMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
			this.pnlMessage = new System.Windows.Forms.Panel();
			this.grpLicenseDemo = new System.Windows.Forms.GroupBox();
			this.txtActivationCodeDemo = new System.Windows.Forms.TextBox();
			this.lblActivationCodeDemo = new System.Windows.Forms.Label();
			this.txtHours = new System.Windows.Forms.TextBox();
			this.txtDays = new System.Windows.Forms.TextBox();
			this.txtActivationKeyDemo = new System.Windows.Forms.TextBox();
			this.lblHours = new System.Windows.Forms.Label();
			this.lblDays = new System.Windows.Forms.Label();
			this.lblPeriod = new System.Windows.Forms.Label();
			this.lblActivationDemo = new System.Windows.Forms.Label();
			this.grpLicenseFull = new System.Windows.Forms.GroupBox();
			this.txtActivationCodeFull = new System.Windows.Forms.TextBox();
			this.lblActivationCodeFull = new System.Windows.Forms.Label();
			this.txtActivationKeyFull = new System.Windows.Forms.TextBox();
			this.lblActivationFull = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.TextBox();
			this.lblAlertMessage = new System.Windows.Forms.Label();
			this.lblMessage2 = new System.Windows.Forms.Label();
			this.pnlImage = new System.Windows.Forms.Panel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.pnlMessage.SuspendLayout();
			this.grpLicenseDemo.SuspendLayout();
			this.grpLicenseFull.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlMessage
			// 
			this.pnlMessage.BackColor = System.Drawing.Color.White;
			this.pnlMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMessage.Controls.Add(this.grpLicenseDemo);
			this.pnlMessage.Controls.Add(this.grpLicenseFull);
			this.pnlMessage.Controls.Add(this.lblMessage);
			this.pnlMessage.Controls.Add(this.lblAlertMessage);
			this.pnlMessage.Controls.Add(this.lblMessage2);
			this.pnlMessage.Controls.Add(this.pnlImage);
			this.pnlMessage.Controls.Add(this.lblTitle);
			this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlMessage.Location = new System.Drawing.Point(0, 0);
			this.pnlMessage.Name = "pnlMessage";
			this.pnlMessage.Size = new System.Drawing.Size(361, 366);
			this.pnlMessage.TabIndex = 1;
			// 
			// grpLicenseDemo
			// 
			this.grpLicenseDemo.Controls.Add(this.txtActivationCodeDemo);
			this.grpLicenseDemo.Controls.Add(this.lblActivationCodeDemo);
			this.grpLicenseDemo.Controls.Add(this.txtHours);
			this.grpLicenseDemo.Controls.Add(this.txtDays);
			this.grpLicenseDemo.Controls.Add(this.txtActivationKeyDemo);
			this.grpLicenseDemo.Controls.Add(this.lblHours);
			this.grpLicenseDemo.Controls.Add(this.lblDays);
			this.grpLicenseDemo.Controls.Add(this.lblPeriod);
			this.grpLicenseDemo.Controls.Add(this.lblActivationDemo);
			this.grpLicenseDemo.Font = new System.Drawing.Font("Arial", 8.25F);
			this.grpLicenseDemo.Location = new System.Drawing.Point(24, 196);
			this.grpLicenseDemo.Name = "grpLicenseDemo";
			this.grpLicenseDemo.Size = new System.Drawing.Size(316, 112);
			this.grpLicenseDemo.TabIndex = 13;
			this.grpLicenseDemo.TabStop = false;
			this.grpLicenseDemo.Text = "Demo License";
			// 
			// txtActivationCodeDemo
			// 
			this.txtActivationCodeDemo.BackColor = System.Drawing.SystemColors.Control;
			this.txtActivationCodeDemo.Location = new System.Drawing.Point(104, 24);
			this.txtActivationCodeDemo.Name = "txtActivationCodeDemo";
			this.txtActivationCodeDemo.Size = new System.Drawing.Size(199, 20);
			this.txtActivationCodeDemo.TabIndex = 11;
			// 
			// lblActivationCodeDemo
			// 
			this.lblActivationCodeDemo.AutoSize = true;
			this.lblActivationCodeDemo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblActivationCodeDemo.Location = new System.Drawing.Point(9, 25);
			this.lblActivationCodeDemo.Name = "lblActivationCodeDemo";
			this.lblActivationCodeDemo.Size = new System.Drawing.Size(83, 14);
			this.lblActivationCodeDemo.TabIndex = 8;
			this.lblActivationCodeDemo.Text = "Activation Code";
			// 
			// txtHours
			// 
			this.txtHours.Enabled = false;
			this.txtHours.Location = new System.Drawing.Point(197, 78);
			this.txtHours.Name = "txtHours";
			this.txtHours.Size = new System.Drawing.Size(45, 20);
			this.txtHours.TabIndex = 11;
			this.txtHours.Text = "0";
			this.txtHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// txtDays
			// 
			this.txtDays.Location = new System.Drawing.Point(104, 78);
			this.txtDays.Name = "txtDays";
			this.txtDays.Size = new System.Drawing.Size(45, 20);
			this.txtDays.TabIndex = 11;
			this.txtDays.Text = "10";
			this.txtDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// txtActivationKeyDemo
			// 
			this.txtActivationKeyDemo.Location = new System.Drawing.Point(104, 50);
			this.txtActivationKeyDemo.Name = "txtActivationKeyDemo";
			this.txtActivationKeyDemo.Size = new System.Drawing.Size(199, 20);
			this.txtActivationKeyDemo.TabIndex = 11;
			// 
			// lblHours
			// 
			this.lblHours.AutoSize = true;
			this.lblHours.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHours.Location = new System.Drawing.Point(246, 81);
			this.lblHours.Name = "lblHours";
			this.lblHours.Size = new System.Drawing.Size(36, 14);
			this.lblHours.TabIndex = 10;
			this.lblHours.Text = "Hours";
			// 
			// lblDays
			// 
			this.lblDays.AutoSize = true;
			this.lblDays.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDays.Location = new System.Drawing.Point(153, 81);
			this.lblDays.Name = "lblDays";
			this.lblDays.Size = new System.Drawing.Size(32, 14);
			this.lblDays.TabIndex = 10;
			this.lblDays.Text = "Days";
			// 
			// lblPeriod
			// 
			this.lblPeriod.AutoSize = true;
			this.lblPeriod.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPeriod.Location = new System.Drawing.Point(9, 80);
			this.lblPeriod.Name = "lblPeriod";
			this.lblPeriod.Size = new System.Drawing.Size(88, 14);
			this.lblPeriod.TabIndex = 10;
			this.lblPeriod.Text = "Activation Period";
			// 
			// lblActivationDemo
			// 
			this.lblActivationDemo.AutoSize = true;
			this.lblActivationDemo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblActivationDemo.Location = new System.Drawing.Point(9, 52);
			this.lblActivationDemo.Name = "lblActivationDemo";
			this.lblActivationDemo.Size = new System.Drawing.Size(85, 14);
			this.lblActivationDemo.TabIndex = 10;
			this.lblActivationDemo.Text = "Activation Demo";
			// 
			// grpLicenseFull
			// 
			this.grpLicenseFull.Controls.Add(this.txtActivationCodeFull);
			this.grpLicenseFull.Controls.Add(this.lblActivationCodeFull);
			this.grpLicenseFull.Controls.Add(this.txtActivationKeyFull);
			this.grpLicenseFull.Controls.Add(this.lblActivationFull);
			this.grpLicenseFull.Font = new System.Drawing.Font("Arial", 8.25F);
			this.grpLicenseFull.Location = new System.Drawing.Point(24, 99);
			this.grpLicenseFull.Name = "grpLicenseFull";
			this.grpLicenseFull.Size = new System.Drawing.Size(316, 92);
			this.grpLicenseFull.TabIndex = 12;
			this.grpLicenseFull.TabStop = false;
			this.grpLicenseFull.Text = "Full License";
			// 
			// txtActivationCodeFull
			// 
			this.txtActivationCodeFull.BackColor = System.Drawing.SystemColors.Control;
			this.txtActivationCodeFull.Location = new System.Drawing.Point(104, 24);
			this.txtActivationCodeFull.Name = "txtActivationCodeFull";
			this.txtActivationCodeFull.Size = new System.Drawing.Size(199, 20);
			this.txtActivationCodeFull.TabIndex = 11;
			// 
			// lblActivationCodeFull
			// 
			this.lblActivationCodeFull.AutoSize = true;
			this.lblActivationCodeFull.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblActivationCodeFull.Location = new System.Drawing.Point(9, 25);
			this.lblActivationCodeFull.Name = "lblActivationCodeFull";
			this.lblActivationCodeFull.Size = new System.Drawing.Size(83, 14);
			this.lblActivationCodeFull.TabIndex = 8;
			this.lblActivationCodeFull.Text = "Activation Code";
			// 
			// txtActivationKeyFull
			// 
			this.txtActivationKeyFull.Location = new System.Drawing.Point(104, 50);
			this.txtActivationKeyFull.Name = "txtActivationKeyFull";
			this.txtActivationKeyFull.Size = new System.Drawing.Size(199, 20);
			this.txtActivationKeyFull.TabIndex = 11;
			// 
			// lblActivationFull
			// 
			this.lblActivationFull.AutoSize = true;
			this.lblActivationFull.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblActivationFull.Location = new System.Drawing.Point(9, 52);
			this.lblActivationFull.Name = "lblActivationFull";
			this.lblActivationFull.Size = new System.Drawing.Size(74, 14);
			this.lblActivationFull.TabIndex = 10;
			this.lblActivationFull.Text = "Activation Full";
			// 
			// lblMessage
			// 
			this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblMessage.Enabled = false;
			this.lblMessage.Font = new System.Drawing.Font("Arial", 8.25F);
			this.lblMessage.Location = new System.Drawing.Point(67, 57);
			this.lblMessage.Multiline = true;
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(273, 30);
			this.lblMessage.TabIndex = 11;
			this.lblMessage.Text = "Please enter the License Activation Code.";
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
			// lblMessage2
			// 
			this.lblMessage2.AutoSize = true;
			this.lblMessage2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMessage2.ForeColor = System.Drawing.Color.Black;
			this.lblMessage2.Location = new System.Drawing.Point(62, 331);
			this.lblMessage2.Name = "lblMessage2";
			this.lblMessage2.Size = new System.Drawing.Size(229, 14);
			this.lblMessage2.TabIndex = 3;
			this.lblMessage2.Text = "Press the OK button to Generate your license.";
			// 
			// pnlImage
			// 
			this.pnlImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlImage.BackgroundImage")));
			this.pnlImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pnlImage.Location = new System.Drawing.Point(27, 56);
			this.pnlImage.Name = "pnlImage";
			this.pnlImage.Size = new System.Drawing.Size(34, 32);
			this.pnlImage.TabIndex = 2;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
			this.lblTitle.Location = new System.Drawing.Point(11, 18);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(159, 16);
			this.lblTitle.TabIndex = 1;
			this.lblTitle.Text = "Generate Product License";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(289, 376);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(60, 22);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(208, 376);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(60, 22);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(361, 405);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.pnlMessage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmMain";
			this.Text = "Dsu License";
			this.Load += new System.EventHandler(this.FrmMain_Load);
			this.pnlMessage.ResumeLayout(false);
			this.pnlMessage.PerformLayout();
			this.grpLicenseDemo.ResumeLayout(false);
			this.grpLicenseDemo.PerformLayout();
			this.grpLicenseFull.ResumeLayout(false);
			this.grpLicenseFull.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMessage;
        private System.Windows.Forms.TextBox lblMessage;
        private System.Windows.Forms.TextBox txtActivationKeyFull;
        private System.Windows.Forms.Label lblActivationFull;
        private System.Windows.Forms.Label lblAlertMessage;
        private System.Windows.Forms.Label lblMessage2;
        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtActivationCodeFull;
        private System.Windows.Forms.Label lblActivationCodeFull;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox grpLicenseFull;
        private System.Windows.Forms.GroupBox grpLicenseDemo;
        private System.Windows.Forms.TextBox txtActivationCodeDemo;
        private System.Windows.Forms.Label lblActivationCodeDemo;
        private System.Windows.Forms.TextBox txtActivationKeyDemo;
        private System.Windows.Forms.Label lblActivationDemo;
        private System.Windows.Forms.Label lblDays;
        private System.Windows.Forms.TextBox txtDays;
        private System.Windows.Forms.TextBox txtHours;
        private System.Windows.Forms.Label lblHours;
        private System.Windows.Forms.Label lblPeriod;
    }
}

