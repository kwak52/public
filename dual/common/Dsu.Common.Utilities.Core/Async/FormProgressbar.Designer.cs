namespace Dsu.Common.Utilities
{
    partial class FormProgressbar
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.exProgressbar1 = new Dsu.Common.Utilities.ExProgressbar();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(197, 73);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Progressing...";
            // 
            // exProgressbar1
            // 
            this.exProgressbar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exProgressbar1.BarColor = System.Drawing.SystemColors.Highlight;
            this.exProgressbar1.CenterText = null;
            this.exProgressbar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exProgressbar1.Location = new System.Drawing.Point(4, 33);
            this.exProgressbar1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.exProgressbar1.Maximum = 100;
            this.exProgressbar1.Minimum = 0;
            this.exProgressbar1.Name = "exProgressbar1";
            this.exProgressbar1.ShowPercentage = false;
            this.exProgressbar1.Size = new System.Drawing.Size(268, 27);
            this.exProgressbar1.Step = 10;
            this.exProgressbar1.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
            this.exProgressbar1.TabIndex = 3;
            this.exProgressbar1.Value = 0;
            // 
            // FormProgressbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 102);
            this.Controls.Add(this.exProgressbar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Name = "FormProgressbar";
            this.Text = "FormProgressbarCancelable";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private ExProgressbar exProgressbar1;
    }
}