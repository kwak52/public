namespace PLCConvertor.Forms
{
    partial class FormLadderParse
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
            this.btnNext = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbRemoveAuxNode = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(32, 28);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(78, 40);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(12, 133);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1122, 505);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // cbRemoveAuxNode
            // 
            this.cbRemoveAuxNode.AutoSize = true;
            this.cbRemoveAuxNode.Location = new System.Drawing.Point(157, 38);
            this.cbRemoveAuxNode.Name = "cbRemoveAuxNode";
            this.cbRemoveAuxNode.Size = new System.Drawing.Size(223, 22);
            this.cbRemoveAuxNode.TabIndex = 2;
            this.cbRemoveAuxNode.Text = "Remove Internal Nodes";
            this.cbRemoveAuxNode.UseVisualStyleBackColor = true;
            this.cbRemoveAuxNode.CheckedChanged += new System.EventHandler(this.CbRemoveAuxNode_CheckedChanged);
            // 
            // FormLadderParse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 650);
            this.Controls.Add(this.cbRemoveAuxNode);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnNext);
            this.Name = "FormLadderParse";
            this.Text = "FormLadderParse";
            this.Load += new System.EventHandler(this.FormLadderParse_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox cbRemoveAuxNode;
    }
}