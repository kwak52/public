namespace Dsu.Common.Utilities.DX
{
    partial class DXFormLogController
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
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.enumEditorLevel = new Dsu.Common.Utilities.EnumEditor();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnLogger = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnSelected = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEditSelected = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditSelected)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckAll.Location = new System.Drawing.Point(297, 407);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(95, 23);
            this.btnCheckAll.TabIndex = 1;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUncheckAll.Location = new System.Drawing.Point(297, 436);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(95, 23);
            this.btnUncheckAll.TabIndex = 2;
            this.btnUncheckAll.Text = "Uncheck All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // enumEditorLevel
            // 
            this.enumEditorLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.enumEditorLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.enumEditorLevel.ControlSpacing = 20;
            this.enumEditorLevel.EnumType = null;
            this.enumEditorLevel.EnumValue = ((long)(0));
            this.enumEditorLevel.LableFormat = "{0}";
            this.enumEditorLevel.LayoutMode = Dsu.Common.Utilities.LayoutMode.Portrait;
            this.enumEditorLevel.Location = new System.Drawing.Point(8, 400);
            this.enumEditorLevel.Name = "enumEditorLevel";
            this.enumEditorLevel.Size = new System.Drawing.Size(150, 59);
            this.enumEditorLevel.TabIndex = 3;
            this.enumEditorLevel.Change += new System.EventHandler(this.enumEditorLevel_Change);
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEditSelected});
            this.gridControl1.Size = new System.Drawing.Size(397, 392);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnLogger,
            this.gridColumnSelected});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumnLogger
            // 
            this.gridColumnLogger.Caption = "Logger";
            this.gridColumnLogger.FieldName = "Name";
            this.gridColumnLogger.Name = "gridColumnLogger";
            this.gridColumnLogger.Visible = true;
            this.gridColumnLogger.VisibleIndex = 0;
            // 
            // gridColumnSelected
            // 
            this.gridColumnSelected.Caption = "Active";
            this.gridColumnSelected.ColumnEdit = this.repositoryItemCheckEditSelected;
            this.gridColumnSelected.FieldName = "IsEnabled";
            this.gridColumnSelected.MaxWidth = 50;
            this.gridColumnSelected.Name = "gridColumnSelected";
            this.gridColumnSelected.Visible = true;
            this.gridColumnSelected.VisibleIndex = 1;
            this.gridColumnSelected.Width = 40;
            // 
            // repositoryItemCheckEditSelected
            // 
            this.repositoryItemCheckEditSelected.AutoHeight = false;
            this.repositoryItemCheckEditSelected.Name = "repositoryItemCheckEditSelected";
            // 
            // DXFormLogController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 467);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.enumEditorLevel);
            this.Controls.Add(this.btnUncheckAll);
            this.Controls.Add(this.btnCheckAll);
            this.Name = "DXFormLogController";
            this.Text = "Logger Controller";
            this.Load += new System.EventHandler(this.DXFormLogController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditSelected)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnUncheckAll;
        private EnumEditor enumEditorLevel;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnLogger;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnSelected;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEditSelected;
    }
}