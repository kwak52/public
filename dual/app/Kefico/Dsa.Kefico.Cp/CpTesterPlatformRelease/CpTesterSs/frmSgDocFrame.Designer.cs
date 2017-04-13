namespace CpTesterSs
{
    partial class frmSgDocFrame
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
            this.panelControlSgDocFrame = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlSgDocFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlSgDocFrame
            // 
            this.panelControlSgDocFrame.AutoSize = true;
            this.panelControlSgDocFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlSgDocFrame.Location = new System.Drawing.Point(0, 0);
            this.panelControlSgDocFrame.Name = "panelControlSgDocFrame";
            this.panelControlSgDocFrame.Size = new System.Drawing.Size(621, 485);
            this.panelControlSgDocFrame.TabIndex = 0;
            // 
            // frmSgDocFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 485);
            this.Controls.Add(this.panelControlSgDocFrame);
            this.Name = "frmSgDocFrame";
            this.Text = "frmSgDocFrame";
            ((System.ComponentModel.ISupportInitialize)(this.panelControlSgDocFrame)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraEditors.PanelControl panelControlSgDocFrame;
    }
}