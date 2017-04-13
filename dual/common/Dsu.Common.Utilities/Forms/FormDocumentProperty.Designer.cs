namespace Dsu.Common.Utilities.Forms
{
    partial class FormDocumentProperty
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.btnNewGuid = new System.Windows.Forms.Button();
            this.textBoxName = new Dsu.Common.Utilities.FilterableTextBox();
            this.textBoxGuid = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageDescription = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.tabPageNote = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxProgramNote = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageDescription.SuspendLayout();
            this.tabPageNote.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageDescription);
            this.tabControl1.Controls.Add(this.tabPageNote);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(385, 262);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.btnNewGuid);
            this.tabPageGeneral.Controls.Add(this.textBoxName);
            this.tabPageGeneral.Controls.Add(this.textBoxGuid);
            this.tabPageGeneral.Controls.Add(this.label2);
            this.tabPageGeneral.Controls.Add(this.label1);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(377, 236);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // btnNewGuid
            // 
            this.btnNewGuid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewGuid.Location = new System.Drawing.Point(294, 36);
            this.btnNewGuid.Name = "btnNewGuid";
            this.btnNewGuid.Size = new System.Drawing.Size(75, 23);
            this.btnNewGuid.TabIndex = 5;
            this.btnNewGuid.Text = "New guid";
            this.btnNewGuid.UseVisualStyleBackColor = true;
            this.btnNewGuid.Click += new System.EventHandler(this.btnNewGuid_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.AllowedCharacterSet = null;
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.ForbiddenCharacterSet = null;
            this.textBoxName.Location = new System.Drawing.Point(57, 11);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ShowBalloonTips = true;
            this.textBoxName.Size = new System.Drawing.Size(312, 21);
            this.textBoxName.TabIndex = 4;
            // 
            // textBoxGuid
            // 
            this.textBoxGuid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGuid.Location = new System.Drawing.Point(57, 38);
            this.textBoxGuid.Name = "textBoxGuid";
            this.textBoxGuid.Size = new System.Drawing.Size(227, 21);
            this.textBoxGuid.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Guid:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // tabPageDescription
            // 
            this.tabPageDescription.Controls.Add(this.label3);
            this.tabPageDescription.Controls.Add(this.textBoxNote);
            this.tabPageDescription.Location = new System.Drawing.Point(4, 22);
            this.tabPageDescription.Name = "tabPageDescription";
            this.tabPageDescription.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDescription.Size = new System.Drawing.Size(377, 236);
            this.tabPageDescription.TabIndex = 1;
            this.tabPageDescription.Text = "User Note";
            this.tabPageDescription.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(275, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "Add free-form text description fot the document.";
            // 
            // textBoxNote
            // 
            this.textBoxNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNote.Location = new System.Drawing.Point(6, 31);
            this.textBoxNote.Multiline = true;
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxNote.Size = new System.Drawing.Size(363, 197);
            this.textBoxNote.TabIndex = 0;
            // 
            // tabPageNote
            // 
            this.tabPageNote.Controls.Add(this.label4);
            this.tabPageNote.Controls.Add(this.textBoxProgramNote);
            this.tabPageNote.Location = new System.Drawing.Point(4, 22);
            this.tabPageNote.Name = "tabPageNote";
            this.tabPageNote.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNote.Size = new System.Drawing.Size(377, 236);
            this.tabPageNote.TabIndex = 2;
            this.tabPageNote.Text = "Program Note";
            this.tabPageNote.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(198, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Program generated comment text.";
            // 
            // textBoxProgramNote
            // 
            this.textBoxProgramNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProgramNote.Location = new System.Drawing.Point(6, 31);
            this.textBoxProgramNote.Multiline = true;
            this.textBoxProgramNote.Name = "textBoxProgramNote";
            this.textBoxProgramNote.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxProgramNote.Size = new System.Drawing.Size(363, 197);
            this.textBoxProgramNote.TabIndex = 0;
            // 
            // FormDocumentProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 262);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormDocumentProperty";
            this.Text = "Document Property";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDocumentProperty_FormClosing);
            this.Load += new System.EventHandler(this.FormDocumentProperty_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageDescription.ResumeLayout(false);
            this.tabPageDescription.PerformLayout();
            this.tabPageNote.ResumeLayout(false);
            this.tabPageNote.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TextBox textBoxGuid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPageDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.TabPage tabPageNote;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxProgramNote;
        private System.Windows.Forms.Button btnNewGuid;
        private FilterableTextBox textBoxName;
    }
}