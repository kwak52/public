namespace AddressMapper
{
    /// <summary>
    /// https://www.devexpress.com/Support/Center/Question/Details/T547214/custom-painting-for-rangecontrol
    /// </summary>
    partial class FormTestRangeUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTestRangeUI));
            this.numericRangeControlClient1 = new DevExpress.XtraEditors.NumericRangeControlClient();
            this.rangeControl1 = new DevExpress.XtraEditors.RangeControl();
            ((System.ComponentModel.ISupportInitialize)(this.rangeControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericRangeControlClient1
            // 
            this.numericRangeControlClient1.Maximum = 200;
            this.numericRangeControlClient1.Minimum = 100;
            this.numericRangeControlClient1.RangeControl = null;
            this.numericRangeControlClient1.RulerDelta = 10;
            // 
            // rangeControl1
            // 
            this.rangeControl1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.rangeControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.rangeControl1.Appearance.Options.UseBackColor = true;
            this.rangeControl1.Appearance.Options.UseForeColor = true;
            this.rangeControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rangeControl1.BackgroundImage")));
            this.rangeControl1.Client = this.numericRangeControlClient1;
            this.rangeControl1.Location = new System.Drawing.Point(89, 136);
            this.rangeControl1.Name = "rangeControl1";
            this.rangeControl1.SelectionType = DevExpress.XtraEditors.RangeControlSelectionType.ThumbAndFlag;
            this.rangeControl1.Size = new System.Drawing.Size(570, 107);
            this.rangeControl1.TabIndex = 0;
            this.rangeControl1.Text = "rangeControl1";
            // 
            // FormTestRangeUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rangeControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormTestRangeUI";
            this.Text = "FormTestRangeUI";
            ((System.ComponentModel.ISupportInitialize)(this.rangeControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.NumericRangeControlClient numericRangeControlClient1;
        private DevExpress.XtraEditors.RangeControl rangeControl1;
    }
}