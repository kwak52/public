namespace Dsu.PLCConverter.UI
{
    partial class UcMemoryBar
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
            this.customRangeControl1 = new Dsu.PLCConverter.UI.CustomRangeControl();
            this.numericRangeControlClient1 = new DevExpress.XtraEditors.NumericRangeControlClient();
            this.textEditStart = new DevExpress.XtraEditors.TextEdit();
            this.textEditEnd = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.customRangeControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditEnd.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // customRangeControl1
            // 
            this.customRangeControl1.Client = this.numericRangeControlClient1;
            this.customRangeControl1.Location = new System.Drawing.Point(3, 3);
            this.customRangeControl1.Name = "customRangeControl1";
            this.customRangeControl1.Size = new System.Drawing.Size(677, 90);
            this.customRangeControl1.TabIndex = 0;
            this.customRangeControl1.Text = "customRangeControl1";
            // 
            // numericRangeControlClient1
            // 
            this.numericRangeControlClient1.RangeControl = null;
            // 
            // textEditStart
            // 
            this.textEditStart.Location = new System.Drawing.Point(198, 113);
            this.textEditStart.Name = "textEditStart";
            this.textEditStart.Size = new System.Drawing.Size(150, 30);
            this.textEditStart.TabIndex = 1;
            // 
            // textEditEnd
            // 
            this.textEditEnd.Location = new System.Drawing.Point(432, 113);
            this.textEditEnd.Name = "textEditEnd";
            this.textEditEnd.Size = new System.Drawing.Size(150, 30);
            this.textEditEnd.TabIndex = 2;
            // 
            // UcMemoryBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textEditEnd);
            this.Controls.Add(this.textEditStart);
            this.Controls.Add(this.customRangeControl1);
            this.Name = "UcMemoryBar";
            this.Size = new System.Drawing.Size(683, 173);
            this.Load += new System.EventHandler(this.UcMemoryBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.customRangeControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditEnd.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomRangeControl customRangeControl1;
        private DevExpress.XtraEditors.NumericRangeControlClient numericRangeControlClient1;
        private DevExpress.XtraEditors.TextEdit textEditStart;
        private DevExpress.XtraEditors.TextEdit textEditEnd;
    }
}
