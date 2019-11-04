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
            this.labelSource = new System.Windows.Forms.Label();
            this.labelTarget = new System.Windows.Forms.Label();
            this.textBoxSource = new System.Windows.Forms.TextBox();
            this.textBoxTarget = new System.Windows.Forms.TextBox();
            this.textBoxNumArgs = new System.Windows.Forms.TextBox();
            this.labelNumArgs = new System.Windows.Forms.Label();
            this.textBoxSourceArg0 = new System.Windows.Forms.TextBox();
            this.labelArg0 = new System.Windows.Forms.Label();
            this.textBoxSourceArg1 = new System.Windows.Forms.TextBox();
            this.labelArg1 = new System.Windows.Forms.Label();
            this.textBoxSourceArg2 = new System.Windows.Forms.TextBox();
            this.labelArg2 = new System.Windows.Forms.Label();
            this.textBoxSourceArg3 = new System.Windows.Forms.TextBox();
            this.labelArg3 = new System.Windows.Forms.Label();
            this.textBoxTargetArg3 = new System.Windows.Forms.TextBox();
            this.textBoxTargetArg2 = new System.Windows.Forms.TextBox();
            this.textBoxTargetArg1 = new System.Windows.Forms.TextBox();
            this.textBoxTargetArg0 = new System.Windows.Forms.TextBox();
            this.textBoxRuleName = new System.Windows.Forms.TextBox();
            this.labelRuleName = new System.Windows.Forms.Label();
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
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(429, 112);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(72, 18);
            this.labelSource.TabIndex = 2;
            this.labelSource.Text = "Source:";
            // 
            // labelTarget
            // 
            this.labelTarget.AutoSize = true;
            this.labelTarget.Location = new System.Drawing.Point(596, 112);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(65, 18);
            this.labelTarget.TabIndex = 3;
            this.labelTarget.Text = "Target:";
            // 
            // textBoxSource
            // 
            this.textBoxSource.Location = new System.Drawing.Point(432, 148);
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.Size = new System.Drawing.Size(129, 28);
            this.textBoxSource.TabIndex = 4;
            // 
            // textBoxTarget
            // 
            this.textBoxTarget.Location = new System.Drawing.Point(599, 148);
            this.textBoxTarget.Name = "textBoxTarget";
            this.textBoxTarget.Size = new System.Drawing.Size(131, 28);
            this.textBoxTarget.TabIndex = 5;
            // 
            // textBoxNumArgs
            // 
            this.textBoxNumArgs.Location = new System.Drawing.Point(446, 25);
            this.textBoxNumArgs.Name = "textBoxNumArgs";
            this.textBoxNumArgs.Size = new System.Drawing.Size(39, 28);
            this.textBoxNumArgs.TabIndex = 7;
            // 
            // labelNumArgs
            // 
            this.labelNumArgs.AutoSize = true;
            this.labelNumArgs.Location = new System.Drawing.Point(341, 28);
            this.labelNumArgs.Name = "labelNumArgs";
            this.labelNumArgs.Size = new System.Drawing.Size(99, 18);
            this.labelNumArgs.TabIndex = 6;
            this.labelNumArgs.Text = "Num. args:";
            // 
            // textBoxSourceArg0
            // 
            this.textBoxSourceArg0.Location = new System.Drawing.Point(432, 194);
            this.textBoxSourceArg0.Name = "textBoxSourceArg0";
            this.textBoxSourceArg0.Size = new System.Drawing.Size(74, 28);
            this.textBoxSourceArg0.TabIndex = 9;
            // 
            // labelArg0
            // 
            this.labelArg0.AutoSize = true;
            this.labelArg0.Location = new System.Drawing.Point(374, 197);
            this.labelArg0.Name = "labelArg0";
            this.labelArg0.Size = new System.Drawing.Size(35, 18);
            this.labelArg0.TabIndex = 8;
            this.labelArg0.Text = "$0:";
            // 
            // textBoxSourceArg1
            // 
            this.textBoxSourceArg1.Location = new System.Drawing.Point(432, 228);
            this.textBoxSourceArg1.Name = "textBoxSourceArg1";
            this.textBoxSourceArg1.Size = new System.Drawing.Size(74, 28);
            this.textBoxSourceArg1.TabIndex = 11;
            // 
            // labelArg1
            // 
            this.labelArg1.AutoSize = true;
            this.labelArg1.Location = new System.Drawing.Point(374, 231);
            this.labelArg1.Name = "labelArg1";
            this.labelArg1.Size = new System.Drawing.Size(35, 18);
            this.labelArg1.TabIndex = 10;
            this.labelArg1.Text = "$1:";
            // 
            // textBoxSourceArg2
            // 
            this.textBoxSourceArg2.Location = new System.Drawing.Point(432, 262);
            this.textBoxSourceArg2.Name = "textBoxSourceArg2";
            this.textBoxSourceArg2.Size = new System.Drawing.Size(74, 28);
            this.textBoxSourceArg2.TabIndex = 13;
            // 
            // labelArg2
            // 
            this.labelArg2.AutoSize = true;
            this.labelArg2.Location = new System.Drawing.Point(374, 265);
            this.labelArg2.Name = "labelArg2";
            this.labelArg2.Size = new System.Drawing.Size(35, 18);
            this.labelArg2.TabIndex = 12;
            this.labelArg2.Text = "$2:";
            // 
            // textBoxSourceArg3
            // 
            this.textBoxSourceArg3.Location = new System.Drawing.Point(432, 296);
            this.textBoxSourceArg3.Name = "textBoxSourceArg3";
            this.textBoxSourceArg3.Size = new System.Drawing.Size(74, 28);
            this.textBoxSourceArg3.TabIndex = 15;
            // 
            // labelArg3
            // 
            this.labelArg3.AutoSize = true;
            this.labelArg3.Location = new System.Drawing.Point(374, 299);
            this.labelArg3.Name = "labelArg3";
            this.labelArg3.Size = new System.Drawing.Size(29, 18);
            this.labelArg3.TabIndex = 14;
            this.labelArg3.Text = "$3";
            // 
            // textBoxTargetArg3
            // 
            this.textBoxTargetArg3.Location = new System.Drawing.Point(600, 296);
            this.textBoxTargetArg3.Name = "textBoxTargetArg3";
            this.textBoxTargetArg3.Size = new System.Drawing.Size(130, 28);
            this.textBoxTargetArg3.TabIndex = 19;
            // 
            // textBoxTargetArg2
            // 
            this.textBoxTargetArg2.Location = new System.Drawing.Point(600, 262);
            this.textBoxTargetArg2.Name = "textBoxTargetArg2";
            this.textBoxTargetArg2.Size = new System.Drawing.Size(130, 28);
            this.textBoxTargetArg2.TabIndex = 18;
            // 
            // textBoxTargetArg1
            // 
            this.textBoxTargetArg1.Location = new System.Drawing.Point(600, 228);
            this.textBoxTargetArg1.Name = "textBoxTargetArg1";
            this.textBoxTargetArg1.Size = new System.Drawing.Size(130, 28);
            this.textBoxTargetArg1.TabIndex = 17;
            // 
            // textBoxTargetArg0
            // 
            this.textBoxTargetArg0.Location = new System.Drawing.Point(600, 194);
            this.textBoxTargetArg0.Name = "textBoxTargetArg0";
            this.textBoxTargetArg0.Size = new System.Drawing.Size(130, 28);
            this.textBoxTargetArg0.TabIndex = 16;
            // 
            // textBoxRuleName
            // 
            this.textBoxRuleName.Location = new System.Drawing.Point(432, 71);
            this.textBoxRuleName.Name = "textBoxRuleName";
            this.textBoxRuleName.Size = new System.Drawing.Size(74, 28);
            this.textBoxRuleName.TabIndex = 21;
            // 
            // labelRuleName
            // 
            this.labelRuleName.AutoSize = true;
            this.labelRuleName.Location = new System.Drawing.Point(374, 74);
            this.labelRuleName.Name = "labelRuleName";
            this.labelRuleName.Size = new System.Drawing.Size(61, 18);
            this.labelRuleName.TabIndex = 20;
            this.labelRuleName.Text = "Name:";
            // 
            // FormAddAddressMappingRule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxRuleName);
            this.Controls.Add(this.labelRuleName);
            this.Controls.Add(this.textBoxTargetArg3);
            this.Controls.Add(this.textBoxTargetArg2);
            this.Controls.Add(this.textBoxTargetArg1);
            this.Controls.Add(this.textBoxTargetArg0);
            this.Controls.Add(this.textBoxSourceArg3);
            this.Controls.Add(this.labelArg3);
            this.Controls.Add(this.textBoxSourceArg2);
            this.Controls.Add(this.labelArg2);
            this.Controls.Add(this.textBoxSourceArg1);
            this.Controls.Add(this.labelArg1);
            this.Controls.Add(this.textBoxSourceArg0);
            this.Controls.Add(this.labelArg0);
            this.Controls.Add(this.textBoxNumArgs);
            this.Controls.Add(this.labelNumArgs);
            this.Controls.Add(this.textBoxTarget);
            this.Controls.Add(this.textBoxSource);
            this.Controls.Add(this.labelTarget);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.listBoxRuleType);
            this.Name = "FormAddAddressMappingRule";
            this.Text = "FormAddAddressMappingRule";
            this.Load += new System.EventHandler(this.FormAddAddressMappingRule_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxRuleType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelTarget;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.TextBox textBoxTarget;
        private System.Windows.Forms.TextBox textBoxNumArgs;
        private System.Windows.Forms.Label labelNumArgs;
        private System.Windows.Forms.TextBox textBoxSourceArg0;
        private System.Windows.Forms.Label labelArg0;
        private System.Windows.Forms.TextBox textBoxSourceArg1;
        private System.Windows.Forms.Label labelArg1;
        private System.Windows.Forms.TextBox textBoxSourceArg2;
        private System.Windows.Forms.Label labelArg2;
        private System.Windows.Forms.TextBox textBoxSourceArg3;
        private System.Windows.Forms.Label labelArg3;
        private System.Windows.Forms.TextBox textBoxTargetArg3;
        private System.Windows.Forms.TextBox textBoxTargetArg2;
        private System.Windows.Forms.TextBox textBoxTargetArg1;
        private System.Windows.Forms.TextBox textBoxTargetArg0;
        private System.Windows.Forms.TextBox textBoxRuleName;
        private System.Windows.Forms.Label labelRuleName;
    }
}