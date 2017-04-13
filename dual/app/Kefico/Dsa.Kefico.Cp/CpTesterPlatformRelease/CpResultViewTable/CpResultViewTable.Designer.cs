namespace CpTesterPlatform.CpResultViewTable
{
    partial class CpResultViewTable
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
            this.gridControlResult = new DevExpress.XtraGrid.GridControl();
            this.gridViewResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewResult)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlResult
            // 
            this.gridControlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlResult.Location = new System.Drawing.Point(0, 0);
            this.gridControlResult.MainView = this.gridViewResult;
            this.gridControlResult.Name = "gridControlResult";
            this.gridControlResult.Size = new System.Drawing.Size(890, 687);
            this.gridControlResult.TabIndex = 0;
            this.gridControlResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewResult});
            // 
            // gridViewResult
            // 
            this.gridViewResult.GridControl = this.gridControlResult;
            this.gridViewResult.Name = "gridViewResult";
            this.gridViewResult.OptionsBehavior.Editable = false;
            this.gridViewResult.OptionsBehavior.ReadOnly = true;
            this.gridViewResult.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewResult.OptionsView.ShowGroupPanel = false;
            this.gridViewResult.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridViewResult_CustomDrawCell);
            // 
            // CpResultViewTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlResult);
            this.Name = "CpResultViewTable";
            this.Size = new System.Drawing.Size(890, 687);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewResult;
    }
}
