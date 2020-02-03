namespace Dsu.PLCConverter.UI
{
    partial class UcMemoryRange
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.numericRangeControlClient1 = new DevExpress.XtraEditors.NumericRangeControlClient();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.customRangeControl1 = new Dsu.PLCConverter.UI.CustomRangeControl();
            ((System.ComponentModel.ISupportInitialize)(this.customRangeControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericRangeControlClient1
            // 
            this.numericRangeControlClient1.Maximum = 10240;
            this.numericRangeControlClient1.RangeControl = null;
            this.numericRangeControlClient1.RulerDelta = 1024;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // customRangeControl1
            // 
            this.customRangeControl1.Client = this.numericRangeControlClient1;
            this.customRangeControl1.Location = new System.Drawing.Point(0, 0);
            this.customRangeControl1.Margin = new System.Windows.Forms.Padding(2);
            this.customRangeControl1.Name = "customRangeControl1";
            this.customRangeControl1.Size = new System.Drawing.Size(688, 80);
            this.customRangeControl1.TabIndex = 0;
            this.customRangeControl1.Text = "customRangeControl1";
            // 
            // UcMemoryRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.customRangeControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UcMemoryRange";
            this.Size = new System.Drawing.Size(728, 104);
            this.Load += new System.EventHandler(this.UcMemoryRange_Load);
            ((System.ComponentModel.ISupportInitialize)(this.customRangeControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomRangeControl customRangeControl1;
        private DevExpress.XtraEditors.NumericRangeControlClient numericRangeControlClient1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
