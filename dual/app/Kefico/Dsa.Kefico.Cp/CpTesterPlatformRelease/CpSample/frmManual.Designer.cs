namespace CpSample
{
    partial class frmManual
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
            DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer dockingContainer1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer();
            this.documentGroup1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup(this.components);
            this.ucManuDAQDocument = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.ucManuDCPowerDocument = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.ucManuLCRDocument = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.ucManuLVDTDocument = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.ucManuTempHumiDocument = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.ucManuPLCDocument = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.ucManuMotionDocument = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.ucManuDigitIODocument = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuDAQDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuDCPowerDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuLCRDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuLVDTDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuTempHumiDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuPLCDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuMotionDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuDigitIODocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            this.SuspendLayout();
            // 
            // documentGroup1
            // 
            this.documentGroup1.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document[] {
            this.ucManuDAQDocument,
            this.ucManuDCPowerDocument,
            this.ucManuLCRDocument,
            this.ucManuLVDTDocument,
            this.ucManuTempHumiDocument,
            this.ucManuPLCDocument,
            this.ucManuMotionDocument,
            this.ucManuDigitIODocument});
            // 
            // ucManuDAQDocument
            // 
            this.ucManuDAQDocument.Caption = "DAQ";
            this.ucManuDAQDocument.ControlName = "ucManuDAQ";
            this.ucManuDAQDocument.ControlTypeName = "CpSample.ucManuDAQ";
            // 
            // ucManuDCPowerDocument
            // 
            this.ucManuDCPowerDocument.Caption = "DC Power";
            this.ucManuDCPowerDocument.ControlName = "ucManuDCPower";
            this.ucManuDCPowerDocument.ControlTypeName = "CpSample.ucManuDCPower";
            // 
            // ucManuLCRDocument
            // 
            this.ucManuLCRDocument.Caption = "LCR";
            this.ucManuLCRDocument.ControlName = "ucManuLCR";
            this.ucManuLCRDocument.ControlTypeName = "CpSample.ucManuLCR";
            // 
            // ucManuLVDTDocument
            // 
            this.ucManuLVDTDocument.Caption = "LVDT";
            this.ucManuLVDTDocument.ControlName = "ucManuLVDT";
            this.ucManuLVDTDocument.ControlTypeName = "CpSample.ucManuLVDT";
            // 
            // ucManuTempHumiDocument
            // 
            this.ucManuTempHumiDocument.Caption = "Temp & Humi";
            this.ucManuTempHumiDocument.ControlName = "ucManuTempHumi";
            this.ucManuTempHumiDocument.ControlTypeName = "CpSample.ucManuTempHumi";
            // 
            // ucManuPLCDocument
            // 
            this.ucManuPLCDocument.Caption = "PLC";
            this.ucManuPLCDocument.ControlName = "ucManuPLC";
            this.ucManuPLCDocument.ControlTypeName = "CpSample.ucManuPLC";
            // 
            // ucManuMotionDocument
            // 
            this.ucManuMotionDocument.Caption = "Motion";
            this.ucManuMotionDocument.ControlName = "ucManuMotion";
            this.ucManuMotionDocument.ControlTypeName = "CpSample.ucManuMotion";
            // 
            // ucManuDigitIODocument
            // 
            this.ucManuDigitIODocument.Caption = "Digit IO";
            this.ucManuDigitIODocument.ControlName = "ucManuDigitIO";
            this.ucManuDigitIODocument.ControlTypeName = "CpSample.ucManuDigitIO";
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.View = this.tabbedView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // tabbedView1
            // 
            this.tabbedView1.DocumentGroups.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup[] {
            this.documentGroup1});
            this.tabbedView1.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
            this.ucManuDAQDocument,
            this.ucManuDCPowerDocument,
            this.ucManuLCRDocument,
            this.ucManuLVDTDocument,
            this.ucManuTempHumiDocument,
            this.ucManuPLCDocument,
            this.ucManuMotionDocument,
            this.ucManuDigitIODocument});
            this.tabbedView1.RootContainer.Element = null;
            dockingContainer1.Element = this.documentGroup1;
            this.tabbedView1.RootContainer.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            dockingContainer1});
            // 
            // frmManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 811);
            this.Name = "frmManual";
            this.Text = "CP Manual";
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuDAQDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuDCPowerDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuLCRDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuLVDTDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuTempHumiDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuPLCDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuMotionDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucManuDigitIODocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup documentGroup1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document ucManuDAQDocument;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document ucManuDCPowerDocument;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document ucManuLCRDocument;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document ucManuLVDTDocument;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document ucManuTempHumiDocument;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document ucManuPLCDocument;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document ucManuMotionDocument;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document ucManuDigitIODocument;
    }
}