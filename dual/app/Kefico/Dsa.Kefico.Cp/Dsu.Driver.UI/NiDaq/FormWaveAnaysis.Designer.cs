namespace Dsu.Driver.UI.NiDaq
{
    partial class FormWaveAnaysis
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
            this.waveNumericCtrl1 = new WaveNumericCtrl();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.btnWidthAnal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // waveNumericCtrl1
            // 
            this.waveNumericCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.waveNumericCtrl1.Data = null;
            this.waveNumericCtrl1.Location = new System.Drawing.Point(12, 12);
            this.waveNumericCtrl1.Name = "waveNumericCtrl1";
            this.waveNumericCtrl1.SamplingRate = 0D;
            this.waveNumericCtrl1.Size = new System.Drawing.Size(434, 390);
            this.waveNumericCtrl1.TabIndex = 0;
            // 
            // btnStatistics
            // 
            this.btnStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStatistics.Location = new System.Drawing.Point(371, 410);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(75, 23);
            this.btnStatistics.TabIndex = 1;
            this.btnStatistics.Text = "Statistics...";
            this.btnStatistics.UseVisualStyleBackColor = true;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnWidthAnal
            // 
            this.btnWidthAnal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWidthAnal.Location = new System.Drawing.Point(290, 410);
            this.btnWidthAnal.Name = "btnWidthAnal";
            this.btnWidthAnal.Size = new System.Drawing.Size(75, 23);
            this.btnWidthAnal.TabIndex = 2;
            this.btnWidthAnal.Text = "Width anal..";
            this.btnWidthAnal.UseVisualStyleBackColor = true;
            this.btnWidthAnal.Click += new System.EventHandler(this.btnWidthAnal_Click);
            // 
            // FormWaveAnaysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 445);
            this.Controls.Add(this.btnWidthAnal);
            this.Controls.Add(this.btnStatistics);
            this.Controls.Add(this.waveNumericCtrl1);
            this.Name = "FormWaveAnaysis";
            this.Text = "FormWaveAnaysis";
            this.ResumeLayout(false);

        }

        #endregion

        private WaveNumericCtrl waveNumericCtrl1;
        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Button btnWidthAnal;
    }
}