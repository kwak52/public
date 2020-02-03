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
            this.components = new System.ComponentModel.Container();
            this.numericRangeControlClient1 = new DevExpress.XtraEditors.NumericRangeControlClient();
            this.textEditStart = new DevExpress.XtraEditors.TextEdit();
            this.textEditEnd = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.customRangeControl1 = new Dsu.PLCConverter.UI.CustomRangeControl();
            ((System.ComponentModel.ISupportInitialize)(this.textEditStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customRangeControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericRangeControlClient1
            // 
            this.numericRangeControlClient1.RangeControl = null;
            // 
            // textEditStart
            // 
            this.textEditStart.Location = new System.Drawing.Point(158, 94);
            this.textEditStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textEditStart.Name = "textEditStart";
            this.textEditStart.Size = new System.Drawing.Size(120, 24);
            this.textEditStart.TabIndex = 1;
            // 
            // textEditEnd
            // 
            this.textEditEnd.Location = new System.Drawing.Point(346, 94);
            this.textEditEnd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textEditEnd.Name = "textEditEnd";
            this.textEditEnd.Size = new System.Drawing.Size(120, 24);
            this.textEditEnd.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 32);
            // 
            // customRangeControl1
            // 
            this.customRangeControl1.Client = this.numericRangeControlClient1;
            this.customRangeControl1.Location = new System.Drawing.Point(2, 2);
            this.customRangeControl1.Margin = new System.Windows.Forms.Padding(2);
            this.customRangeControl1.Name = "customRangeControl1";
            this.customRangeControl1.Size = new System.Drawing.Size(578, 75);
            this.customRangeControl1.TabIndex = 0;
            this.customRangeControl1.Text = "customRangeControl1";
            // 
            // UcMemoryBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.textEditEnd);
            this.Controls.Add(this.textEditStart);
            this.Controls.Add(this.customRangeControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "UcMemoryBar";
            this.Size = new System.Drawing.Size(582, 144);
            this.Load += new System.EventHandler(this.UcMemoryBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEditStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customRangeControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomRangeControl customRangeControl1;
        private DevExpress.XtraEditors.NumericRangeControlClient numericRangeControlClient1;
        private DevExpress.XtraEditors.TextEdit textEditStart;
        private DevExpress.XtraEditors.TextEdit textEditEnd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
