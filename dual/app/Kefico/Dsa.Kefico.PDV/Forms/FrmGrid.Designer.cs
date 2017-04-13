namespace Dsa.Kefico.PDV
{
    partial class FrmGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGrid));
            this.ucGrid1 = new Dsu.UI.Grid.ucGrid();
            this.SuspendLayout();
            // 
            // ucGrid1
            // 
            this.ucGrid1.ColumnFont = new System.Drawing.Font("Tahoma", 9F);
            this.ucGrid1.DataSource = null;
            this.ucGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGrid1.Editable = false;
            this.ucGrid1.Location = new System.Drawing.Point(0, 0);
            this.ucGrid1.MultiSelect = true;
            this.ucGrid1.Name = "ucGrid1";
            this.ucGrid1.RowFont = new System.Drawing.Font("Tahoma", 9F);
            this.ucGrid1.ShowAutoFilterRow = true;
            this.ucGrid1.ShowGroupPanel = false;
            this.ucGrid1.Size = new System.Drawing.Size(284, 262);
            this.ucGrid1.TabIndex = 0;
            // 
            // FrmGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.ucGrid1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGrid";
            this.Text = "Table";
            this.Activated += new System.EventHandler(this.frmGrid_Activated);
            this.ResumeLayout(false);

        }

        #endregion

        private Dsu.UI.Grid.ucGrid ucGrid1;
    }
}