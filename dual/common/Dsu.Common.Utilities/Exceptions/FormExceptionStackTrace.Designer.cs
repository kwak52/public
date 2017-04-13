namespace Dsu.Common.Utilities.Forms
{
    partial class FormExceptionStackTrace
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
            this.textBoxExceptionSummary = new System.Windows.Forms.TextBox();
            this.textBoxFrameInfo = new System.Windows.Forms.TextBox();
            this.listViewExceptionStackTrace1 = new Dsu.Common.Utilities.ListViewExceptionStackTrace();
            this.SuspendLayout();
            // 
            // textBoxExceptionSummary
            // 
            this.textBoxExceptionSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExceptionSummary.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxExceptionSummary.ForeColor = System.Drawing.Color.Black;
            this.textBoxExceptionSummary.Location = new System.Drawing.Point(17, 12);
            this.textBoxExceptionSummary.Multiline = true;
            this.textBoxExceptionSummary.Name = "textBoxExceptionSummary";
            this.textBoxExceptionSummary.ReadOnly = true;
            this.textBoxExceptionSummary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExceptionSummary.Size = new System.Drawing.Size(521, 74);
            this.textBoxExceptionSummary.TabIndex = 1;
            // 
            // textBoxFrameInfo
            // 
            this.textBoxFrameInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFrameInfo.Location = new System.Drawing.Point(17, 330);
            this.textBoxFrameInfo.Multiline = true;
            this.textBoxFrameInfo.Name = "textBoxFrameInfo";
            this.textBoxFrameInfo.ReadOnly = true;
            this.textBoxFrameInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxFrameInfo.Size = new System.Drawing.Size(521, 76);
            this.textBoxFrameInfo.TabIndex = 2;
            // 
            // listViewExceptionStackTrace1
            // 
            this.listViewExceptionStackTrace1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewExceptionStackTrace1.FullRowSelect = true;
            this.listViewExceptionStackTrace1.Location = new System.Drawing.Point(17, 92);
            this.listViewExceptionStackTrace1.Name = "listViewExceptionStackTrace1";
            this.listViewExceptionStackTrace1.OpenFileOnDoubleClick = false;
            this.listViewExceptionStackTrace1.Size = new System.Drawing.Size(521, 232);
            this.listViewExceptionStackTrace1.TabIndex = 0;
            this.listViewExceptionStackTrace1.UseCompatibleStateImageBehavior = false;
            this.listViewExceptionStackTrace1.UseVisualStudioIDEIfApplicable = false;
            this.listViewExceptionStackTrace1.View = System.Windows.Forms.View.Details;
            this.listViewExceptionStackTrace1.SelectedIndexChanged += new System.EventHandler(this.listViewExceptionStackTrace1_SelectedIndexChanged);
            // 
            // FormExceptionStackTrace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 418);
            this.Controls.Add(this.textBoxFrameInfo);
            this.Controls.Add(this.textBoxExceptionSummary);
            this.Controls.Add(this.listViewExceptionStackTrace1);
            this.Name = "FormExceptionStackTrace";
            this.Text = "FormExceptionStackTrace";
            this.Load += new System.EventHandler(this.FormExceptionStackTrace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListViewExceptionStackTrace listViewExceptionStackTrace1;
        private System.Windows.Forms.TextBox textBoxExceptionSummary;
        private System.Windows.Forms.TextBox textBoxFrameInfo;
    }
}