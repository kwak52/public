namespace Cpt.Winform
{
    partial class FormLauncher
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
            this.btnNewRequest = new System.Windows.Forms.Button();
            this.btnPdvTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNewRequest
            // 
            this.btnNewRequest.Location = new System.Drawing.Point(12, 41);
            this.btnNewRequest.Name = "btnNewRequest";
            this.btnNewRequest.Size = new System.Drawing.Size(187, 23);
            this.btnNewRequest.TabIndex = 3;
            this.btnNewRequest.Text = "New tester";
            this.btnNewRequest.UseVisualStyleBackColor = true;
            // 
            // btnPdvTest
            // 
            this.btnPdvTest.Location = new System.Drawing.Point(12, 70);
            this.btnPdvTest.Name = "btnPdvTest";
            this.btnPdvTest.Size = new System.Drawing.Size(187, 23);
            this.btnPdvTest.TabIndex = 4;
            this.btnPdvTest.Text = "PDV test";
            this.btnPdvTest.UseVisualStyleBackColor = true;
            // 
            // FormLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 104);
            this.Controls.Add(this.btnPdvTest);
            this.Controls.Add(this.btnNewRequest);
            this.Name = "FormLauncher";
            this.Text = "Launcher";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnNewRequest;
        private System.Windows.Forms.Button btnPdvTest;
    }
}