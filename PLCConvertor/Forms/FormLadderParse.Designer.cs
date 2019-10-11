﻿namespace PLCConvertor.Forms
{
    partial class FormLadderParse
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
            DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer dockingContainer2 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer();
            this.documentGroup1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup(this.components);
            this.document1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.cbRemoveAuxNode = new System.Windows.Forms.CheckBox();
            this.panelContainer1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanelMnemonics = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.listBoxControlMnemonics = new DevExpress.XtraEditors.ListBoxControl();
            this.dockPanelAnswer = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.listBoxControlAnswer = new DevExpress.XtraEditors.ListBoxControl();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.btnNewForm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelContainer1.SuspendLayout();
            this.dockPanelMnemonics.SuspendLayout();
            this.dockPanel2_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControlMnemonics)).BeginInit();
            this.dockPanelAnswer.SuspendLayout();
            this.controlContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControlAnswer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            this.SuspendLayout();
            // 
            // documentGroup1
            // 
            this.documentGroup1.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document[] {
            this.document1});
            // 
            // document1
            // 
            this.document1.Caption = "dockPanel1";
            this.document1.ControlName = "dockPanel1";
            this.document1.FloatLocation = new System.Drawing.Point(0, 0);
            this.document1.FloatSize = new System.Drawing.Size(200, 200);
            this.document1.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.True;
            this.document1.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.True;
            this.document1.Properties.AllowFloatOnDoubleClick = DevExpress.Utils.DefaultBoolean.True;
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1,
            this.panelContainer1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Float;
            this.dockPanel1.DockedAsTabbedDocument = true;
            this.dockPanel1.ID = new System.Guid("09582439-4e9d-44b9-b28a-a222cca5a781");
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel1.Size = new System.Drawing.Size(940, 610);
            this.dockPanel1.Text = "dockPanel1";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.btnNewForm);
            this.dockPanel1_Container.Controls.Add(this.pictureBox1);
            this.dockPanel1_Container.Controls.Add(this.btnEnd);
            this.dockPanel1_Container.Controls.Add(this.btnNext);
            this.dockPanel1_Container.Controls.Add(this.cbRemoveAuxNode);
            this.dockPanel1_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(940, 610);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(23, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(905, 562);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(118, 10);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(75, 23);
            this.btnEnd.TabIndex = 3;
            this.btnEnd.Text = ">>|";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.BtnEnd_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(23, 11);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // cbRemoveAuxNode
            // 
            this.cbRemoveAuxNode.AutoSize = true;
            this.cbRemoveAuxNode.Location = new System.Drawing.Point(373, 12);
            this.cbRemoveAuxNode.Name = "cbRemoveAuxNode";
            this.cbRemoveAuxNode.Size = new System.Drawing.Size(211, 22);
            this.cbRemoveAuxNode.TabIndex = 0;
            this.cbRemoveAuxNode.Text = "Remove internal node";
            this.cbRemoveAuxNode.UseVisualStyleBackColor = true;
            this.cbRemoveAuxNode.CheckedChanged += new System.EventHandler(this.CbRemoveAuxNode_CheckedChanged);
            // 
            // panelContainer1
            // 
            this.panelContainer1.Controls.Add(this.dockPanelMnemonics);
            this.panelContainer1.Controls.Add(this.dockPanelAnswer);
            this.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.panelContainer1.ID = new System.Guid("a7df967b-19c9-4a73-b541-8c07818eed2b");
            this.panelContainer1.Location = new System.Drawing.Point(946, 0);
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.OriginalSize = new System.Drawing.Size(200, 200);
            this.panelContainer1.Size = new System.Drawing.Size(200, 650);
            this.panelContainer1.Text = "panelContainer1";
            // 
            // dockPanelMnemonics
            // 
            this.dockPanelMnemonics.Controls.Add(this.dockPanel2_Container);
            this.dockPanelMnemonics.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanelMnemonics.ID = new System.Guid("3794a39f-7cdf-448d-810a-271ae1b9f907");
            this.dockPanelMnemonics.Location = new System.Drawing.Point(0, 0);
            this.dockPanelMnemonics.Name = "dockPanelMnemonics";
            this.dockPanelMnemonics.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelMnemonics.Size = new System.Drawing.Size(200, 325);
            this.dockPanelMnemonics.Text = "Mnemonics";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Controls.Add(this.listBoxControlMnemonics);
            this.dockPanel2_Container.Location = new System.Drawing.Point(9, 33);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(185, 283);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // listBoxControlMnemonics
            // 
            this.listBoxControlMnemonics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxControlMnemonics.Location = new System.Drawing.Point(0, 0);
            this.listBoxControlMnemonics.Name = "listBoxControlMnemonics";
            this.listBoxControlMnemonics.Size = new System.Drawing.Size(185, 283);
            this.listBoxControlMnemonics.TabIndex = 0;
            // 
            // dockPanelAnswer
            // 
            this.dockPanelAnswer.Controls.Add(this.controlContainer1);
            this.dockPanelAnswer.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanelAnswer.ID = new System.Guid("70309052-c6b7-4395-bab3-8b59c4987837");
            this.dockPanelAnswer.Location = new System.Drawing.Point(0, 325);
            this.dockPanelAnswer.Name = "dockPanelAnswer";
            this.dockPanelAnswer.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanelAnswer.Size = new System.Drawing.Size(200, 325);
            this.dockPanelAnswer.Text = "Answer";
            // 
            // controlContainer1
            // 
            this.controlContainer1.Controls.Add(this.listBoxControlAnswer);
            this.controlContainer1.Location = new System.Drawing.Point(9, 33);
            this.controlContainer1.Name = "controlContainer1";
            this.controlContainer1.Size = new System.Drawing.Size(185, 286);
            this.controlContainer1.TabIndex = 0;
            // 
            // listBoxControlAnswer
            // 
            this.listBoxControlAnswer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxControlAnswer.Location = new System.Drawing.Point(0, 0);
            this.listBoxControlAnswer.Name = "listBoxControlAnswer";
            this.listBoxControlAnswer.Size = new System.Drawing.Size(185, 286);
            this.listBoxControlAnswer.TabIndex = 0;
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
            this.document1});
            dockingContainer2.Element = this.documentGroup1;
            this.tabbedView1.RootContainer.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            dockingContainer2});
            // 
            // btnNewForm
            // 
            this.btnNewForm.Location = new System.Drawing.Point(213, 10);
            this.btnNewForm.Name = "btnNewForm";
            this.btnNewForm.Size = new System.Drawing.Size(75, 23);
            this.btnNewForm.TabIndex = 4;
            this.btnNewForm.Text = "+";
            this.btnNewForm.UseVisualStyleBackColor = true;
            this.btnNewForm.Click += new System.EventHandler(this.BtnNewForm_Click);
            // 
            // FormLadderParse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 650);
            this.Controls.Add(this.panelContainer1);
            this.Name = "FormLadderParse";
            this.Text = "FormLadderParse";
            this.Load += new System.EventHandler(this.FormLadderParse_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.dockPanel1_Container.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelContainer1.ResumeLayout(false);
            this.dockPanelMnemonics.ResumeLayout(false);
            this.dockPanel2_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControlMnemonics)).EndInit();
            this.dockPanelAnswer.ResumeLayout(false);
            this.controlContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControlAnswer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelMnemonics;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private System.Windows.Forms.CheckBox cbRemoveAuxNode;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup documentGroup1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document document1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.ListBoxControl listBoxControlMnemonics;
        private System.Windows.Forms.Button btnEnd;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelAnswer;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
        private DevExpress.XtraEditors.ListBoxControl listBoxControlAnswer;
        private System.Windows.Forms.Button btnNewForm;
    }
}