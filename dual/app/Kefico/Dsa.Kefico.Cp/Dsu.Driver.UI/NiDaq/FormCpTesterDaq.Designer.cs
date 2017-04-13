namespace Dsu.Driver.UI.NiDaq
{
    partial class FormCpTesterDaq
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
            this.numericTextBoxMax = new Dsu.Common.Utilities.NumericTextBox();
            this.numericTextBoxMin = new Dsu.Common.Utilities.NumericTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbAutoScale = new System.Windows.Forms.CheckBox();
            this.numericTextBoxTargetNumberOfSamples = new Dsu.Common.Utilities.NumericTextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCollect = new System.Windows.Forms.Button();
            this.textBoxAnalysis = new System.Windows.Forms.TextBox();
            this.daqChartCtrl1 = new Dsu.Driver.UI.NiDaq.DaqChartCtrl();
            this.SuspendLayout();
            // 
            // numericTextBoxMax
            // 
            this.numericTextBoxMax.DecimalNumber = 4;
            this.numericTextBoxMax.Discriminant = ',';
            this.numericTextBoxMax.Dot = true;
            this.numericTextBoxMax.Enabled = false;
            this.numericTextBoxMax.Exponent = true;
            this.numericTextBoxMax.Location = new System.Drawing.Point(413, 13);
            this.numericTextBoxMax.MaxCheck = false;
            this.numericTextBoxMax.MaxValue = 0D;
            this.numericTextBoxMax.MinCheck = false;
            this.numericTextBoxMax.MinValue = 0D;
            this.numericTextBoxMax.Name = "numericTextBoxMax";
            this.numericTextBoxMax.Negative = true;
            this.numericTextBoxMax.ShowBalloonTips = true;
            this.numericTextBoxMax.Size = new System.Drawing.Size(39, 20);
            this.numericTextBoxMax.TabIndex = 45;
            this.numericTextBoxMax.Text = "5";
            // 
            // numericTextBoxMin
            // 
            this.numericTextBoxMin.DecimalNumber = 4;
            this.numericTextBoxMin.Discriminant = ',';
            this.numericTextBoxMin.Dot = true;
            this.numericTextBoxMin.Enabled = false;
            this.numericTextBoxMin.Exponent = true;
            this.numericTextBoxMin.Location = new System.Drawing.Point(332, 12);
            this.numericTextBoxMin.MaxCheck = false;
            this.numericTextBoxMin.MaxValue = 0D;
            this.numericTextBoxMin.MinCheck = false;
            this.numericTextBoxMin.MinValue = 0D;
            this.numericTextBoxMin.Name = "numericTextBoxMin";
            this.numericTextBoxMin.Negative = true;
            this.numericTextBoxMin.ShowBalloonTips = true;
            this.numericTextBoxMin.Size = new System.Drawing.Size(39, 20);
            this.numericTextBoxMin.TabIndex = 44;
            this.numericTextBoxMin.Text = "-5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(378, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 43;
            this.label8.Text = "max:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(304, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "min:";
            // 
            // cbAutoScale
            // 
            this.cbAutoScale.AutoSize = true;
            this.cbAutoScale.Checked = true;
            this.cbAutoScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoScale.Location = new System.Drawing.Point(222, 14);
            this.cbAutoScale.Name = "cbAutoScale";
            this.cbAutoScale.Size = new System.Drawing.Size(76, 17);
            this.cbAutoScale.TabIndex = 41;
            this.cbAutoScale.Text = "Auto scale";
            this.cbAutoScale.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxTargetNumberOfSamples
            // 
            this.numericTextBoxTargetNumberOfSamples.DecimalNumber = 4;
            this.numericTextBoxTargetNumberOfSamples.Discriminant = ',';
            this.numericTextBoxTargetNumberOfSamples.Dot = true;
            this.numericTextBoxTargetNumberOfSamples.Exponent = true;
            this.numericTextBoxTargetNumberOfSamples.Location = new System.Drawing.Point(131, 9);
            this.numericTextBoxTargetNumberOfSamples.MaxCheck = false;
            this.numericTextBoxTargetNumberOfSamples.MaxValue = 0D;
            this.numericTextBoxTargetNumberOfSamples.MinCheck = false;
            this.numericTextBoxTargetNumberOfSamples.MinValue = 0D;
            this.numericTextBoxTargetNumberOfSamples.Name = "numericTextBoxTargetNumberOfSamples";
            this.numericTextBoxTargetNumberOfSamples.Negative = true;
            this.numericTextBoxTargetNumberOfSamples.ShowBalloonTips = true;
            this.numericTextBoxTargetNumberOfSamples.Size = new System.Drawing.Size(47, 20);
            this.numericTextBoxTargetNumberOfSamples.TabIndex = 50;
            this.numericTextBoxTargetNumberOfSamples.Text = "10,000";
            this.numericTextBoxTargetNumberOfSamples.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(414, 363);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(39, 25);
            this.btnStop.TabIndex = 49;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(369, 363);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(39, 25);
            this.btnRun.TabIndex = 48;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 47;
            this.label4.Text = "Target No. of Samplings:";
            // 
            // btnCollect
            // 
            this.btnCollect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollect.Location = new System.Drawing.Point(369, 304);
            this.btnCollect.Name = "btnCollect";
            this.btnCollect.Size = new System.Drawing.Size(83, 53);
            this.btnCollect.TabIndex = 46;
            this.btnCollect.Text = "Collect and Analysis";
            this.btnCollect.UseVisualStyleBackColor = true;
            this.btnCollect.Click += new System.EventHandler(this.btnCollect_Click);
            // 
            // textBoxAnalysis
            // 
            this.textBoxAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAnalysis.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAnalysis.Location = new System.Drawing.Point(4, 304);
            this.textBoxAnalysis.Multiline = true;
            this.textBoxAnalysis.Name = "textBoxAnalysis";
            this.textBoxAnalysis.ReadOnly = true;
            this.textBoxAnalysis.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxAnalysis.Size = new System.Drawing.Size(359, 84);
            this.textBoxAnalysis.TabIndex = 51;
            // 
            // daqChartCtrl1
            // 
            this.daqChartCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.daqChartCtrl1.Location = new System.Drawing.Point(4, 35);
            this.daqChartCtrl1.Name = "daqChartCtrl1";
            this.daqChartCtrl1.Size = new System.Drawing.Size(449, 263);
            this.daqChartCtrl1.TabIndex = 52;
            // 
            // FormCpTesterDaq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 392);
            this.Controls.Add(this.daqChartCtrl1);
            this.Controls.Add(this.textBoxAnalysis);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnCollect);
            this.Controls.Add(this.numericTextBoxTargetNumberOfSamples);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericTextBoxMax);
            this.Controls.Add(this.numericTextBoxMin);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbAutoScale);
            this.MinimumSize = new System.Drawing.Size(470, 430);
            this.Name = "FormCpTesterDaq";
            this.Text = "FormCpTesterDaq";
            this.Load += new System.EventHandler(this.FormCpTesterDaq_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Utilities.NumericTextBox numericTextBoxMax;
        private Common.Utilities.NumericTextBox numericTextBoxMin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbAutoScale;
        private Common.Utilities.NumericTextBox numericTextBoxTargetNumberOfSamples;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCollect;
        private System.Windows.Forms.TextBox textBoxAnalysis;
        private DaqChartCtrl daqChartCtrl1;
    }
}