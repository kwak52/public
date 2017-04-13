namespace Dsu.Driver.UI.NiDaq
{
    partial class FormDaqChart
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
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.daqChartCtrl1 = new Dsu.Driver.UI.NiDaq.DaqChartCtrl();
            this.SuspendLayout();
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalyze.Location = new System.Drawing.Point(460, 3);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(87, 21);
            this.btnAnalyze.TabIndex = 29;
            this.btnAnalyze.Text = "Analyze...";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // daqChartCtrl1
            // 
            this.daqChartCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.daqChartCtrl1.Channel = null;
            this.daqChartCtrl1.CollectNumberMultiplier = 1;
            this.daqChartCtrl1.IsRunning = false;
            this.daqChartCtrl1.Location = new System.Drawing.Point(7, 30);
            this.daqChartCtrl1.LowpassCutoffFrequency = null;
            this.daqChartCtrl1.Name = "daqChartCtrl1";
            this.daqChartCtrl1.NumSamplesPerBuffer = 0;
            this.daqChartCtrl1.RedrawChartProc = null;
            this.daqChartCtrl1.RedrawCounter = 0;
            this.daqChartCtrl1.RedrawPause = 100;
            this.daqChartCtrl1.SamplingRate = 0D;
            this.daqChartCtrl1.Size = new System.Drawing.Size(540, 268);
            this.daqChartCtrl1.TabIndex = 30;
            // 
            // FormDaqChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 302);
            this.Controls.Add(this.daqChartCtrl1);
            this.Controls.Add(this.btnAnalyze);
            this.Name = "FormDaqChart";
            this.Text = "FormSwiftPlot";
            this.Load += new System.EventHandler(this.FormDaqChart_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAnalyze;
        private DaqChartCtrl daqChartCtrl1;
    }
}