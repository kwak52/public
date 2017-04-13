namespace Dsu.Common.Utilities
{
    partial class FormStackTrace
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
            this.lbStackTrace = new System.Windows.Forms.ListBox();
            this.editMessage = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbSkipSimiliar = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lbStackTrace
            // 
            this.lbStackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStackTrace.FormattingEnabled = true;
            this.lbStackTrace.ItemHeight = 12;
            this.lbStackTrace.Location = new System.Drawing.Point(13, 153);
            this.lbStackTrace.Name = "lbStackTrace";
            this.lbStackTrace.Size = new System.Drawing.Size(344, 148);
            this.lbStackTrace.TabIndex = 0;
            // 
            // editMessage
            // 
            this.editMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.editMessage.Location = new System.Drawing.Point(13, 53);
            this.editMessage.Multiline = true;
            this.editMessage.Name = "editMessage";
            this.editMessage.ReadOnly = true;
            this.editMessage.Size = new System.Drawing.Size(344, 85);
            this.editMessage.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(282, 358);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // cbSkipSimiliar
            // 
            this.cbSkipSimiliar.AutoSize = true;
            this.cbSkipSimiliar.Location = new System.Drawing.Point(16, 316);
            this.cbSkipSimiliar.Name = "cbSkipSimiliar";
            this.cbSkipSimiliar.Size = new System.Drawing.Size(292, 16);
            this.cbSkipSimiliar.TabIndex = 3;
            this.cbSkipSimiliar.Text = "Skip these kind of exceptions/messages, later.";
            this.cbSkipSimiliar.UseVisualStyleBackColor = true;
            // 
            // FormStackTrace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 393);
            this.Controls.Add(this.cbSkipSimiliar);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.editMessage);
            this.Controls.Add(this.lbStackTrace);
            this.Name = "FormStackTrace";
            this.Text = "FormStackTrace";
            this.Load += new System.EventHandler(this.FormStackTrace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbStackTrace;
        private System.Windows.Forms.TextBox editMessage;
        private System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.CheckBox cbSkipSimiliar;
    }
}