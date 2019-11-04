namespace PLCConvertor.Forms
{
    partial class FormTestAddressMapping
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSourcePattern = new System.Windows.Forms.TextBox();
            this.textBoxTargetPattern = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSourceArgs = new System.Windows.Forms.TextBox();
            this.textBoxTargetArgs = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source\r\npattern";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(370, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 36);
            this.label2.TabIndex = 1;
            this.label2.Text = "Target\r\npattern";
            // 
            // textBoxSourcePattern
            // 
            this.textBoxSourcePattern.Location = new System.Drawing.Point(93, 23);
            this.textBoxSourcePattern.Name = "textBoxSourcePattern";
            this.textBoxSourcePattern.Size = new System.Drawing.Size(194, 28);
            this.textBoxSourcePattern.TabIndex = 2;
            // 
            // textBoxTargetPattern
            // 
            this.textBoxTargetPattern.Location = new System.Drawing.Point(449, 20);
            this.textBoxTargetPattern.Name = "textBoxTargetPattern";
            this.textBoxTargetPattern.Size = new System.Drawing.Size(194, 28);
            this.textBoxTargetPattern.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 54);
            this.label3.TabIndex = 4;
            this.label3.Text = "Source\r\nargs\r\nrange";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(370, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 54);
            this.label4.TabIndex = 5;
            this.label4.Text = "Target \r\nargs\r\nrange";
            // 
            // textBoxSourceArgs
            // 
            this.textBoxSourceArgs.Location = new System.Drawing.Point(93, 93);
            this.textBoxSourceArgs.Multiline = true;
            this.textBoxSourceArgs.Name = "textBoxSourceArgs";
            this.textBoxSourceArgs.Size = new System.Drawing.Size(194, 143);
            this.textBoxSourceArgs.TabIndex = 6;
            // 
            // textBoxTargetArgs
            // 
            this.textBoxTargetArgs.Location = new System.Drawing.Point(449, 93);
            this.textBoxTargetArgs.Multiline = true;
            this.textBoxTargetArgs.Name = "textBoxTargetArgs";
            this.textBoxTargetArgs.Size = new System.Drawing.Size(194, 143);
            this.textBoxTargetArgs.TabIndex = 7;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(194, 291);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(93, 25);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // FormAddressMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 340);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.textBoxTargetArgs);
            this.Controls.Add(this.textBoxSourceArgs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTargetPattern);
            this.Controls.Add(this.textBoxSourcePattern);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddressMapping";
            this.Text = "Address Mapping";
            this.Load += new System.EventHandler(this.FormAddressMapping_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSourcePattern;
        private System.Windows.Forms.TextBox textBoxTargetPattern;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSourceArgs;
        private System.Windows.Forms.TextBox textBoxTargetArgs;
        private System.Windows.Forms.Button btnGenerate;
    }
}