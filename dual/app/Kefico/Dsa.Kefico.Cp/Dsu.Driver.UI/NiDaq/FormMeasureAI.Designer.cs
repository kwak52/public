namespace Dsu.Driver.UI.NiDaq
{
    partial class FormMeasureAI
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnCollect = new System.Windows.Forms.Button();
            this.comboBoxDevice = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControlChannels = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxLowpassFilter = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxEnableLowpassFilter = new System.Windows.Forms.CheckBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.cbAutoScale = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelMeasure = new System.Windows.Forms.Label();
            this.numericTextBoxMin = new Dsu.Common.Utilities.NumericTextBox();
            this.numericTextBoxMax = new Dsu.Common.Utilities.NumericTextBox();
            this.numericTextBoxTargetNumberOfSamples = new Dsu.Common.Utilities.NumericTextBox();
            this.numericTextBoxSamplingRate = new Dsu.Common.Utilities.NumericTextBox();
            this.numericTextBoxNumSamplesPerBuffer = new Dsu.Common.Utilities.NumericTextBox();
            this.numericTextBoxBufferDuration = new Dsu.Common.Utilities.NumericTextBox();
            this.tabControlChannels.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCollect
            // 
            this.btnCollect.Location = new System.Drawing.Point(219, 31);
            this.btnCollect.Name = "btnCollect";
            this.btnCollect.Size = new System.Drawing.Size(75, 65);
            this.btnCollect.TabIndex = 16;
            this.btnCollect.Text = "Collect";
            this.btnCollect.UseVisualStyleBackColor = true;
            this.btnCollect.Click += new System.EventHandler(this.btnCollect_Click);
            // 
            // comboBoxDevice
            // 
            this.comboBoxDevice.FormattingEnabled = true;
            this.comboBoxDevice.Location = new System.Drawing.Point(54, 7);
            this.comboBoxDevice.Name = "comboBoxDevice";
            this.comboBoxDevice.Size = new System.Drawing.Size(94, 21);
            this.comboBoxDevice.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Device:";
            // 
            // tabControlChannels
            // 
            this.tabControlChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlChannels.Controls.Add(this.tabPage1);
            this.tabControlChannels.Controls.Add(this.tabPage2);
            this.tabControlChannels.Location = new System.Drawing.Point(0, 117);
            this.tabControlChannels.Name = "tabControlChannels";
            this.tabControlChannels.SelectedIndex = 0;
            this.tabControlChannels.Size = new System.Drawing.Size(556, 244);
            this.tabControlChannels.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(548, 218);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(548, 218);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(360, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Sampling Rate:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(360, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Num samples / buffer:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(360, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Buffer duration(ms):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Target No. of Samplings:";
            // 
            // comboBoxLowpassFilter
            // 
            this.comboBoxLowpassFilter.FormattingEnabled = true;
            this.comboBoxLowpassFilter.Location = new System.Drawing.Point(83, 75);
            this.comboBoxLowpassFilter.Name = "comboBoxLowpassFilter";
            this.comboBoxLowpassFilter.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLowpassFilter.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Cut off freq.:";
            // 
            // checkBoxEnableLowpassFilter
            // 
            this.checkBoxEnableLowpassFilter.AutoSize = true;
            this.checkBoxEnableLowpassFilter.Location = new System.Drawing.Point(10, 57);
            this.checkBoxEnableLowpassFilter.Name = "checkBoxEnableLowpassFilter";
            this.checkBoxEnableLowpassFilter.Size = new System.Drawing.Size(125, 17);
            this.checkBoxEnableLowpassFilter.TabIndex = 30;
            this.checkBoxEnableLowpassFilter.Text = "Enable lowpass filter:";
            this.checkBoxEnableLowpassFilter.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(416, 364);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(64, 25);
            this.btnRun.TabIndex = 31;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(490, 363);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(64, 25);
            this.btnStop.TabIndex = 32;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // cbAutoScale
            // 
            this.cbAutoScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbAutoScale.AutoSize = true;
            this.cbAutoScale.Checked = true;
            this.cbAutoScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoScale.Location = new System.Drawing.Point(10, 368);
            this.cbAutoScale.Name = "cbAutoScale";
            this.cbAutoScale.Size = new System.Drawing.Size(76, 17);
            this.cbAutoScale.TabIndex = 33;
            this.cbAutoScale.Text = "Auto scale";
            this.cbAutoScale.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(92, 369);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "min:";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(166, 369);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "max:";
            // 
            // labelMeasure
            // 
            this.labelMeasure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMeasure.AutoSize = true;
            this.labelMeasure.Location = new System.Drawing.Point(323, 372);
            this.labelMeasure.Name = "labelMeasure";
            this.labelMeasure.Size = new System.Drawing.Size(35, 13);
            this.labelMeasure.TabIndex = 38;
            this.labelMeasure.Text = "label9";
            // 
            // numericTextBoxMin
            // 
            this.numericTextBoxMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericTextBoxMin.DecimalNumber = 4;
            this.numericTextBoxMin.Discriminant = ',';
            this.numericTextBoxMin.Dot = true;
            this.numericTextBoxMin.Enabled = false;
            this.numericTextBoxMin.Exponent = true;
            this.numericTextBoxMin.Location = new System.Drawing.Point(120, 366);
            this.numericTextBoxMin.MaxCheck = false;
            this.numericTextBoxMin.MaxValue = 0D;
            this.numericTextBoxMin.MinCheck = false;
            this.numericTextBoxMin.MinValue = 0D;
            this.numericTextBoxMin.Name = "numericTextBoxMin";
            this.numericTextBoxMin.Negative = true;
            this.numericTextBoxMin.ShowBalloonTips = true;
            this.numericTextBoxMin.Size = new System.Drawing.Size(39, 20);
            this.numericTextBoxMin.TabIndex = 39;
            this.numericTextBoxMin.Text = "-5";
            // 
            // numericTextBoxMax
            // 
            this.numericTextBoxMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericTextBoxMax.DecimalNumber = 4;
            this.numericTextBoxMax.Discriminant = ',';
            this.numericTextBoxMax.Dot = true;
            this.numericTextBoxMax.Enabled = false;
            this.numericTextBoxMax.Exponent = true;
            this.numericTextBoxMax.Location = new System.Drawing.Point(201, 367);
            this.numericTextBoxMax.MaxCheck = false;
            this.numericTextBoxMax.MaxValue = 0D;
            this.numericTextBoxMax.MinCheck = false;
            this.numericTextBoxMax.MinValue = 0D;
            this.numericTextBoxMax.Name = "numericTextBoxMax";
            this.numericTextBoxMax.Negative = true;
            this.numericTextBoxMax.ShowBalloonTips = true;
            this.numericTextBoxMax.Size = new System.Drawing.Size(39, 20);
            this.numericTextBoxMax.TabIndex = 40;
            this.numericTextBoxMax.Text = "5";
            // 
            // numericTextBoxTargetNumberOfSamples
            // 
            this.numericTextBoxTargetNumberOfSamples.DecimalNumber = 4;
            this.numericTextBoxTargetNumberOfSamples.Discriminant = ',';
            this.numericTextBoxTargetNumberOfSamples.Dot = true;
            this.numericTextBoxTargetNumberOfSamples.Exponent = true;
            this.numericTextBoxTargetNumberOfSamples.Location = new System.Drawing.Point(133, 30);
            this.numericTextBoxTargetNumberOfSamples.MaxCheck = false;
            this.numericTextBoxTargetNumberOfSamples.MaxValue = 0D;
            this.numericTextBoxTargetNumberOfSamples.MinCheck = false;
            this.numericTextBoxTargetNumberOfSamples.MinValue = 0D;
            this.numericTextBoxTargetNumberOfSamples.Name = "numericTextBoxTargetNumberOfSamples";
            this.numericTextBoxTargetNumberOfSamples.Negative = true;
            this.numericTextBoxTargetNumberOfSamples.ShowBalloonTips = true;
            this.numericTextBoxTargetNumberOfSamples.Size = new System.Drawing.Size(80, 20);
            this.numericTextBoxTargetNumberOfSamples.TabIndex = 41;
            this.numericTextBoxTargetNumberOfSamples.Text = "10,000";
            this.numericTextBoxTargetNumberOfSamples.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericTextBoxSamplingRate
            // 
            this.numericTextBoxSamplingRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericTextBoxSamplingRate.DecimalNumber = 4;
            this.numericTextBoxSamplingRate.Discriminant = ',';
            this.numericTextBoxSamplingRate.Dot = true;
            this.numericTextBoxSamplingRate.Exponent = true;
            this.numericTextBoxSamplingRate.Location = new System.Drawing.Point(445, 8);
            this.numericTextBoxSamplingRate.MaxCheck = false;
            this.numericTextBoxSamplingRate.MaxValue = 0D;
            this.numericTextBoxSamplingRate.MinCheck = false;
            this.numericTextBoxSamplingRate.MinValue = 0D;
            this.numericTextBoxSamplingRate.Name = "numericTextBoxSamplingRate";
            this.numericTextBoxSamplingRate.Negative = true;
            this.numericTextBoxSamplingRate.ShowBalloonTips = true;
            this.numericTextBoxSamplingRate.Size = new System.Drawing.Size(98, 20);
            this.numericTextBoxSamplingRate.TabIndex = 42;
            this.numericTextBoxSamplingRate.Text = "1,000,000";
            this.numericTextBoxSamplingRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericTextBoxNumSamplesPerBuffer
            // 
            this.numericTextBoxNumSamplesPerBuffer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericTextBoxNumSamplesPerBuffer.DecimalNumber = 4;
            this.numericTextBoxNumSamplesPerBuffer.Discriminant = ',';
            this.numericTextBoxNumSamplesPerBuffer.Dot = true;
            this.numericTextBoxNumSamplesPerBuffer.Exponent = true;
            this.numericTextBoxNumSamplesPerBuffer.Location = new System.Drawing.Point(477, 36);
            this.numericTextBoxNumSamplesPerBuffer.MaxCheck = false;
            this.numericTextBoxNumSamplesPerBuffer.MaxValue = 0D;
            this.numericTextBoxNumSamplesPerBuffer.MinCheck = false;
            this.numericTextBoxNumSamplesPerBuffer.MinValue = 0D;
            this.numericTextBoxNumSamplesPerBuffer.Name = "numericTextBoxNumSamplesPerBuffer";
            this.numericTextBoxNumSamplesPerBuffer.Negative = true;
            this.numericTextBoxNumSamplesPerBuffer.ShowBalloonTips = true;
            this.numericTextBoxNumSamplesPerBuffer.Size = new System.Drawing.Size(66, 20);
            this.numericTextBoxNumSamplesPerBuffer.TabIndex = 43;
            this.numericTextBoxNumSamplesPerBuffer.Text = "10,000";
            this.numericTextBoxNumSamplesPerBuffer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericTextBoxBufferDuration
            // 
            this.numericTextBoxBufferDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericTextBoxBufferDuration.DecimalNumber = 4;
            this.numericTextBoxBufferDuration.Discriminant = ',';
            this.numericTextBoxBufferDuration.Dot = true;
            this.numericTextBoxBufferDuration.Exponent = true;
            this.numericTextBoxBufferDuration.Location = new System.Drawing.Point(477, 62);
            this.numericTextBoxBufferDuration.MaxCheck = false;
            this.numericTextBoxBufferDuration.MaxValue = 0D;
            this.numericTextBoxBufferDuration.MinCheck = false;
            this.numericTextBoxBufferDuration.MinValue = 0D;
            this.numericTextBoxBufferDuration.Name = "numericTextBoxBufferDuration";
            this.numericTextBoxBufferDuration.Negative = true;
            this.numericTextBoxBufferDuration.ReadOnly = true;
            this.numericTextBoxBufferDuration.ShowBalloonTips = true;
            this.numericTextBoxBufferDuration.Size = new System.Drawing.Size(66, 20);
            this.numericTextBoxBufferDuration.TabIndex = 44;
            this.numericTextBoxBufferDuration.Text = "10,000";
            this.numericTextBoxBufferDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // FormMeasureAI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 393);
            this.Controls.Add(this.numericTextBoxBufferDuration);
            this.Controls.Add(this.numericTextBoxNumSamplesPerBuffer);
            this.Controls.Add(this.numericTextBoxSamplingRate);
            this.Controls.Add(this.numericTextBoxTargetNumberOfSamples);
            this.Controls.Add(this.numericTextBoxMax);
            this.Controls.Add(this.numericTextBoxMin);
            this.Controls.Add(this.labelMeasure);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbAutoScale);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.checkBoxEnableLowpassFilter);
            this.Controls.Add(this.comboBoxLowpassFilter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControlChannels);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxDevice);
            this.Controls.Add(this.btnCollect);
            this.MinimumSize = new System.Drawing.Size(525, 299);
            this.Name = "FormMeasureAI";
            this.Text = "Measure AI";
            this.Load += new System.EventHandler(this.FormMeasureAI_Load);
            this.tabControlChannels.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnCollect;
        private System.Windows.Forms.ComboBox comboBoxDevice;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabControl tabControlChannels;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxLowpassFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxEnableLowpassFilter;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox cbAutoScale;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelMeasure;
        private Common.Utilities.NumericTextBox numericTextBoxMin;
        private Common.Utilities.NumericTextBox numericTextBoxMax;
        private Common.Utilities.NumericTextBox numericTextBoxTargetNumberOfSamples;
        private Common.Utilities.NumericTextBox numericTextBoxSamplingRate;
        private Common.Utilities.NumericTextBox numericTextBoxNumSamplesPerBuffer;
        private Common.Utilities.NumericTextBox numericTextBoxBufferDuration;
    }
}