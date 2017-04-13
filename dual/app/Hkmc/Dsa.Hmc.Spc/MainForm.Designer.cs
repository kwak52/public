namespace Dsa.Hmc.Spc
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.documentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.windowsUIView = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.WindowsUIView(this.components);
            this.mainTileContainer = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.TileContainer(this.components);
            this.document1Tile = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile(this.components);
            this.document_Main = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsUIView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainTileContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1Tile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // documentManager
            // 
            this.documentManager.ContainerControl = this;
            this.documentManager.View = this.windowsUIView;
            this.documentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.windowsUIView});
            // 
            // windowsUIView
            // 
            this.windowsUIView.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowsUIView.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.windowsUIView.AppearanceCaption.ForeColor = System.Drawing.Color.PaleTurquoise;
            this.windowsUIView.AppearanceCaption.Options.UseFont = true;
            this.windowsUIView.AppearanceCaption.Options.UseForeColor = true;
            this.windowsUIView.ContentContainers.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.IContentContainer[] {
            this.mainTileContainer});
            this.windowsUIView.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
            this.document_Main});
            this.windowsUIView.Tiles.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.BaseTile[] {
            this.document1Tile});
            // 
            // mainTileContainer
            // 
            this.mainTileContainer.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("MONITOR", ((System.Drawing.Image)(resources.GetObject("mainTileContainer.Buttons")))),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("SPC", ((System.Drawing.Image)(resources.GetObject("mainTileContainer.Buttons1")))),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("DB", ((System.Drawing.Image)(resources.GetObject("mainTileContainer.Buttons2")))),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("SETTING", ((System.Drawing.Image)(resources.GetObject("mainTileContainer.Buttons3"))))});
            this.mainTileContainer.Caption = "FENDER APRON & FRTSIDE MBR COMPL SPC";
            this.mainTileContainer.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.BaseTile[] {
            this.document1Tile});
            this.mainTileContainer.Name = "mainTileContainer";
            this.mainTileContainer.Properties.ItemSize = 850;
            // 
            // document1Tile
            // 
            this.document1Tile.BackgroundImage = global::Dsa.Hmc.Spc.Properties.Resources.MAIN_SPC;
            this.document1Tile.Document = this.document_Main;
            this.document1Tile.Name = "document1Tile";
            this.document1Tile.Properties.BackgroundImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Stretch;
            // 
            // document_Main
            // 
            this.document_Main.Caption = "Main";
            this.document_Main.ControlName = "document_Main";
            this.document_Main.Properties.AllowActivate = DevExpress.Utils.DefaultBoolean.False;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 456);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "SPC";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsUIView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainTileContainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1Tile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document_Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.WindowsUIView windowsUIView;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.TileContainer mainTileContainer;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile document1Tile;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document document_Main;
    }
}