namespace Dsa.Kefico.MWS
{
    partial class FrmChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChart));
            this.ucChartXbar1 = new Dsu.UI.XbarChart.ucChartXbar();
            this.SuspendLayout();
            // 
            // ucChartXbar1
            // 
            this.ucChartXbar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucChartXbar1.Location = new System.Drawing.Point(0, 0);
            this.ucChartXbar1.Name = "ucChartXbar1";
            this.ucChartXbar1.Size = new System.Drawing.Size(284, 262);
            this.ucChartXbar1.TabIndex = 0;
            this.ucChartXbar1.TitleMain = "Sub Title";
            this.ucChartXbar1.TitleSub = "Sub Title";
            // 
            // FrmChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.ucChartXbar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmChart";
            this.Text = "Chart";
            this.Activated += new System.EventHandler(this.frmChart_Activated);
            this.ResumeLayout(false);

        }

        #endregion

        private Dsu.UI.XbarChart.ucChartXbar ucChartXbar1;
    }
}