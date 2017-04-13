namespace Dsu.Driver.UI.NiDaq
{
    partial class FormGeneratePerChannelAO
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
            this.groupBoxWaveType = new System.Windows.Forms.GroupBox();
            this.enumEditorWaveType = new Dsu.Common.Utilities.EnumEditor();
            this.cbEnable = new System.Windows.Forms.CheckBox();
            this.labelDuty = new System.Windows.Forms.Label();
            this.textBoxAmplitude = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.daqChartCtrl1 = new Dsu.Driver.UI.NiDaq.DaqChartCtrl();
            this.numericTextBoxMax = new Dsu.Common.Utilities.NumericTextBox();
            this.numericTextBoxMin = new Dsu.Common.Utilities.NumericTextBox();
            this.numericTextBoxDuty = new Dsu.Common.Utilities.NumericTextBox();
            this.groupBoxWaveType.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxWaveType
            // 
            this.groupBoxWaveType.Controls.Add(this.enumEditorWaveType);
            this.groupBoxWaveType.Location = new System.Drawing.Point(20, 28);
            this.groupBoxWaveType.Name = "groupBoxWaveType";
            this.groupBoxWaveType.Size = new System.Drawing.Size(133, 54);
            this.groupBoxWaveType.TabIndex = 21;
            this.groupBoxWaveType.TabStop = false;
            this.groupBoxWaveType.Text = "Wave type";
            // 
            // enumEditorWaveType
            // 
            this.enumEditorWaveType.ControlSpacing = 20;
            this.enumEditorWaveType.EnumType = null;
            this.enumEditorWaveType.EnumValue = ((long)(0));
            this.enumEditorWaveType.LableFormat = "{0}";
            this.enumEditorWaveType.LayoutMode = Dsu.Common.Utilities.LayoutMode.Portrait;
            this.enumEditorWaveType.Location = new System.Drawing.Point(10, 12);
            this.enumEditorWaveType.Name = "enumEditorWaveType";
            this.enumEditorWaveType.Size = new System.Drawing.Size(110, 39);
            this.enumEditorWaveType.TabIndex = 13;
            // 
            // cbEnable
            // 
            this.cbEnable.AutoSize = true;
            this.cbEnable.Location = new System.Drawing.Point(20, 6);
            this.cbEnable.Name = "cbEnable";
            this.cbEnable.Size = new System.Drawing.Size(63, 16);
            this.cbEnable.TabIndex = 22;
            this.cbEnable.Text = "Enable";
            this.cbEnable.UseVisualStyleBackColor = true;
            // 
            // labelDuty
            // 
            this.labelDuty.AutoSize = true;
            this.labelDuty.Location = new System.Drawing.Point(339, 50);
            this.labelDuty.Name = "labelDuty";
            this.labelDuty.Size = new System.Drawing.Size(34, 12);
            this.labelDuty.TabIndex = 26;
            this.labelDuty.Text = "Duty:";
            // 
            // textBoxAmplitude
            // 
            this.textBoxAmplitude.Location = new System.Drawing.Point(401, 26);
            this.textBoxAmplitude.Name = "textBoxAmplitude";
            this.textBoxAmplitude.ReadOnly = true;
            this.textBoxAmplitude.Size = new System.Drawing.Size(61, 21);
            this.textBoxAmplitude.TabIndex = 24;
            this.textBoxAmplitude.Text = "+1.0";
            this.textBoxAmplitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(339, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "Amplitude:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 12);
            this.label2.TabIndex = 31;
            this.label2.Text = "Min:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(202, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 12);
            this.label1.TabIndex = 29;
            this.label1.Text = "Max:";
            // 
            // daqChartCtrl1
            // 
            this.daqChartCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.daqChartCtrl1.Channel = null;
            this.daqChartCtrl1.CollectNumberMultiplier = 1;
            this.daqChartCtrl1.IsRunning = false;
            this.daqChartCtrl1.Location = new System.Drawing.Point(3, 88);
            this.daqChartCtrl1.LowpassCutoffFrequency = null;
            this.daqChartCtrl1.Name = "daqChartCtrl1";
            this.daqChartCtrl1.NumSamplesPerBuffer = 0;
            this.daqChartCtrl1.RedrawChartProc = null;
            this.daqChartCtrl1.RedrawCounter = 0;
            this.daqChartCtrl1.RedrawPause = 100;
            this.daqChartCtrl1.SamplingRate = 0D;
            this.daqChartCtrl1.Size = new System.Drawing.Size(546, 212);
            this.daqChartCtrl1.TabIndex = 33;
            // 
            // numericTextBoxMax
            // 
            this.numericTextBoxMax.DecimalNumber = 4;
            this.numericTextBoxMax.Discriminant = ',';
            this.numericTextBoxMax.Dot = true;
            this.numericTextBoxMax.Exponent = true;
            this.numericTextBoxMax.Location = new System.Drawing.Point(233, 25);
            this.numericTextBoxMax.MaxCheck = false;
            this.numericTextBoxMax.MaxValue = 0D;
            this.numericTextBoxMax.MinCheck = false;
            this.numericTextBoxMax.MinValue = 0D;
            this.numericTextBoxMax.Name = "numericTextBoxMax";
            this.numericTextBoxMax.Negative = true;
            this.numericTextBoxMax.ShowBalloonTips = true;
            this.numericTextBoxMax.Size = new System.Drawing.Size(61, 21);
            this.numericTextBoxMax.TabIndex = 34;
            this.numericTextBoxMax.Text = "1.0";
            this.numericTextBoxMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericTextBoxMin
            // 
            this.numericTextBoxMin.DecimalNumber = 4;
            this.numericTextBoxMin.Discriminant = ',';
            this.numericTextBoxMin.Dot = true;
            this.numericTextBoxMin.Exponent = true;
            this.numericTextBoxMin.Location = new System.Drawing.Point(233, 52);
            this.numericTextBoxMin.MaxCheck = false;
            this.numericTextBoxMin.MaxValue = 0D;
            this.numericTextBoxMin.MinCheck = false;
            this.numericTextBoxMin.MinValue = 0D;
            this.numericTextBoxMin.Name = "numericTextBoxMin";
            this.numericTextBoxMin.Negative = true;
            this.numericTextBoxMin.ShowBalloonTips = true;
            this.numericTextBoxMin.Size = new System.Drawing.Size(61, 21);
            this.numericTextBoxMin.TabIndex = 35;
            this.numericTextBoxMin.Text = "-1.0";
            this.numericTextBoxMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericTextBoxDuty
            // 
            this.numericTextBoxDuty.DecimalNumber = 4;
            this.numericTextBoxDuty.Discriminant = ',';
            this.numericTextBoxDuty.Dot = true;
            this.numericTextBoxDuty.Exponent = true;
            this.numericTextBoxDuty.Location = new System.Drawing.Point(401, 47);
            this.numericTextBoxDuty.MaxCheck = false;
            this.numericTextBoxDuty.MaxValue = 0D;
            this.numericTextBoxDuty.MinCheck = false;
            this.numericTextBoxDuty.MinValue = 0D;
            this.numericTextBoxDuty.Name = "numericTextBoxDuty";
            this.numericTextBoxDuty.Negative = true;
            this.numericTextBoxDuty.ShowBalloonTips = true;
            this.numericTextBoxDuty.Size = new System.Drawing.Size(61, 21);
            this.numericTextBoxDuty.TabIndex = 36;
            this.numericTextBoxDuty.Text = "0.5";
            this.numericTextBoxDuty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // FormGeneratePerChannelAO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 302);
            this.Controls.Add(this.numericTextBoxDuty);
            this.Controls.Add(this.numericTextBoxMin);
            this.Controls.Add(this.numericTextBoxMax);
            this.Controls.Add(this.daqChartCtrl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelDuty);
            this.Controls.Add(this.textBoxAmplitude);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbEnable);
            this.Controls.Add(this.groupBoxWaveType);
            this.Name = "FormGeneratePerChannelAO";
            this.Text = "FormGeneratePerChannelAO";
            this.Load += new System.EventHandler(this.FormGeneratePerChannelAO_Load);
            this.groupBoxWaveType.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxWaveType;
        private Common.Utilities.EnumEditor enumEditorWaveType;
        private System.Windows.Forms.CheckBox cbEnable;
        private System.Windows.Forms.Label labelDuty;
        private System.Windows.Forms.TextBox textBoxAmplitude;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DaqChartCtrl daqChartCtrl1;
        private Common.Utilities.NumericTextBox numericTextBoxMax;
        private Common.Utilities.NumericTextBox numericTextBoxMin;
        private Common.Utilities.NumericTextBox numericTextBoxDuty;
    }
}