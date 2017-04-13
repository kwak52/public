namespace Dsu.Driver.UI.NiDaq
{
    partial class FormGenerateAO
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
            this.textBoxBufferDuration = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.comboBoxDevice = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControlChannels = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericTextBoxSamplingRate = new Dsu.Common.Utilities.NumericTextBox();
            this.numericTextBoxNumSamplesPerBuffer = new Dsu.Common.Utilities.NumericTextBox();
            this.tabControlChannels.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxBufferDuration
            // 
            this.textBoxBufferDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBufferDuration.Location = new System.Drawing.Point(438, 62);
            this.textBoxBufferDuration.Name = "textBoxBufferDuration";
            this.textBoxBufferDuration.ReadOnly = true;
            this.textBoxBufferDuration.Size = new System.Drawing.Size(62, 20);
            this.textBoxBufferDuration.TabIndex = 12;
            this.textBoxBufferDuration.Text = "1000";
            this.textBoxBufferDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(326, 457);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 16;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(426, 457);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 17;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // comboBoxDevice
            // 
            this.comboBoxDevice.FormattingEnabled = true;
            this.comboBoxDevice.Location = new System.Drawing.Point(54, 6);
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
            this.tabControlChannels.Location = new System.Drawing.Point(14, 88);
            this.tabControlChannels.Name = "tabControlChannels";
            this.tabControlChannels.SelectedIndex = 0;
            this.tabControlChannels.Size = new System.Drawing.Size(490, 353);
            this.tabControlChannels.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(482, 327);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(482, 327);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(317, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Sampling Rate:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(317, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Num samples / buffer:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(317, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Buffer duration(ms):";
            // 
            // numericTextBoxSamplingRate
            // 
            this.numericTextBoxSamplingRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericTextBoxSamplingRate.DecimalNumber = 4;
            this.numericTextBoxSamplingRate.Discriminant = ',';
            this.numericTextBoxSamplingRate.Dot = true;
            this.numericTextBoxSamplingRate.Exponent = true;
            this.numericTextBoxSamplingRate.Location = new System.Drawing.Point(403, 7);
            this.numericTextBoxSamplingRate.MaxCheck = false;
            this.numericTextBoxSamplingRate.MaxValue = 0D;
            this.numericTextBoxSamplingRate.MinCheck = false;
            this.numericTextBoxSamplingRate.MinValue = 0D;
            this.numericTextBoxSamplingRate.Name = "numericTextBoxSamplingRate";
            this.numericTextBoxSamplingRate.Negative = true;
            this.numericTextBoxSamplingRate.ShowBalloonTips = true;
            this.numericTextBoxSamplingRate.Size = new System.Drawing.Size(98, 20);
            this.numericTextBoxSamplingRate.TabIndex = 43;
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
            this.numericTextBoxNumSamplesPerBuffer.Location = new System.Drawing.Point(435, 33);
            this.numericTextBoxNumSamplesPerBuffer.MaxCheck = false;
            this.numericTextBoxNumSamplesPerBuffer.MaxValue = 0D;
            this.numericTextBoxNumSamplesPerBuffer.MinCheck = false;
            this.numericTextBoxNumSamplesPerBuffer.MinValue = 0D;
            this.numericTextBoxNumSamplesPerBuffer.Name = "numericTextBoxNumSamplesPerBuffer";
            this.numericTextBoxNumSamplesPerBuffer.Negative = true;
            this.numericTextBoxNumSamplesPerBuffer.ShowBalloonTips = true;
            this.numericTextBoxNumSamplesPerBuffer.Size = new System.Drawing.Size(66, 20);
            this.numericTextBoxNumSamplesPerBuffer.TabIndex = 44;
            this.numericTextBoxNumSamplesPerBuffer.Text = "10,000";
            this.numericTextBoxNumSamplesPerBuffer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // FormGenerateAO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 492);
            this.Controls.Add(this.numericTextBoxNumSamplesPerBuffer);
            this.Controls.Add(this.numericTextBoxSamplingRate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControlChannels);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxDevice);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.textBoxBufferDuration);
            this.MinimumSize = new System.Drawing.Size(524, 300);
            this.Name = "FormGenerateAO";
            this.Text = "Generate AO waves";
            this.Load += new System.EventHandler(this.FormGenerateAO_Load);
            this.tabControlChannels.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBoxBufferDuration;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ComboBox comboBoxDevice;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabControl tabControlChannels;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Common.Utilities.NumericTextBox numericTextBoxSamplingRate;
        private Common.Utilities.NumericTextBox numericTextBoxNumSamplesPerBuffer;
    }
}