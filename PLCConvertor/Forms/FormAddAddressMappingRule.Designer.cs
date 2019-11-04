namespace PLCConvertor.Forms
{
    partial class FormAddAddressMappingRule
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
            this.listBoxRuleType = new System.Windows.Forms.ListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxRuleType
            // 
            this.listBoxRuleType.FormattingEnabled = true;
            this.listBoxRuleType.ItemHeight = 18;
            this.listBoxRuleType.Location = new System.Drawing.Point(22, 124);
            this.listBoxRuleType.Name = "listBoxRuleType";
            this.listBoxRuleType.Size = new System.Drawing.Size(209, 202);
            this.listBoxRuleType.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(593, 400);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "button1";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FormAddAddressMappingRule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.listBoxRuleType);
            this.Name = "FormAddAddressMappingRule";
            this.Text = "FormAddAddressMappingRule";
            this.Load += new System.EventHandler(this.FormAddAddressMappingRule_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxRuleType;
        private System.Windows.Forms.Button btnOK;
    }
}