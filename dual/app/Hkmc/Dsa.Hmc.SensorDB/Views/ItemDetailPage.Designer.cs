namespace Dsa.Hmc.SensorDB
{
    partial class ItemDetailPage
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
            this.winLayoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ucChartXbar1 = new Dsu.UI.XbarChart.ucChartXbar();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.winLayoutControl1)).BeginInit();
            this.winLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // winLayoutControl1
            // 
            this.winLayoutControl1.Controls.Add(this.ucChartXbar1);
            this.winLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.winLayoutControl1.Location = new System.Drawing.Point(58, 0);
            this.winLayoutControl1.Name = "winLayoutControl1";
            this.winLayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1093, 194, 450, 350);
            this.winLayoutControl1.Root = this.layoutControlGroup1;
            this.winLayoutControl1.Size = new System.Drawing.Size(823, 546);
            this.winLayoutControl1.TabIndex = 0;
            this.winLayoutControl1.Text = "winLayoutControl1";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(823, 546);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // ucChartXbar1
            // 
            this.ucChartXbar1.Location = new System.Drawing.Point(12, 12);
            this.ucChartXbar1.Name = "ucChartXbar1";
            this.ucChartXbar1.Size = new System.Drawing.Size(799, 522);
            this.ucChartXbar1.TabIndex = 4;
            this.ucChartXbar1.TitleMain = "Title";
            this.ucChartXbar1.TitleSub = "Sub Title";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.ucChartXbar1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(803, 526);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // ItemDetailPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.winLayoutControl1);
            this.Name = "ItemDetailPage";
            this.Padding = new System.Windows.Forms.Padding(58, 0, 58, 0);
            this.Size = new System.Drawing.Size(939, 546);
            ((System.ComponentModel.ISupportInitialize)(this.winLayoutControl1)).EndInit();
            this.winLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl winLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private Dsu.UI.XbarChart.ucChartXbar ucChartXbar1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}
