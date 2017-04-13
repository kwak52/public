using System;
using System.Drawing;

namespace Dsa.Hmc.SensorDB
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
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.windowsUIView = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.WindowsUIView(this.components);
            this.tileContainer = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.TileContainer(this.components);
            this.actionList1 = new Dsu.Common.Utilities.Actions.ActionList(this.components);
            this.action1 = new Dsu.Common.Utilities.Actions.Action(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsUIView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileContainer)).BeginInit();
            this.SuspendLayout();
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.View = this.windowsUIView;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.windowsUIView});
            // 
            // windowsUIView
            // 
            this.windowsUIView.AddTileWhenCreatingDocument = DevExpress.Utils.DefaultBoolean.False;
            this.windowsUIView.AppearanceCaption.Font = ((System.Drawing.Font)(resources.GetObject("windowsUIView.AppearanceCaption.Font")));
            this.windowsUIView.AppearanceCaption.ForeColor = ((System.Drawing.Color)(resources.GetObject("windowsUIView.AppearanceCaption.ForeColor")));
            this.windowsUIView.AppearanceCaption.Options.UseFont = true;
            this.windowsUIView.AppearanceCaption.Options.UseForeColor = true;
            resources.ApplyResources(this.windowsUIView, "windowsUIView");
            this.windowsUIView.ContentContainers.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.IContentContainer[] {
            this.tileContainer});
            this.windowsUIView.TileContainerProperties.ItemSize = 170;
            // 
            // tileContainer
            // 
            this.tileContainer.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton(resources.GetString("tileContainer.Buttons"), ((System.Drawing.Image)(resources.GetObject("tileContainer.Buttons1"))), ((int)(resources.GetObject("tileContainer.Buttons2"))), ((DevExpress.XtraBars.Docking2010.ImageLocation)(resources.GetObject("tileContainer.Buttons3"))), ((DevExpress.XtraBars.Docking2010.ButtonStyle)(resources.GetObject("tileContainer.Buttons4"))), resources.GetString("tileContainer.Buttons5"), ((bool)(resources.GetObject("tileContainer.Buttons6"))), ((int)(resources.GetObject("tileContainer.Buttons7"))), ((bool)(resources.GetObject("tileContainer.Buttons8"))), ((DevExpress.Utils.SuperToolTip)(resources.GetObject("tileContainer.Buttons9"))), ((bool)(resources.GetObject("tileContainer.Buttons10"))), ((bool)(resources.GetObject("tileContainer.Buttons11"))), ((bool)(resources.GetObject("tileContainer.Buttons12"))), ((object)(resources.GetObject("tileContainer.Buttons13"))), ((object)(resources.GetObject("tileContainer.Buttons14"))), ((int)(resources.GetObject("tileContainer.Buttons15"))), ((bool)(resources.GetObject("tileContainer.Buttons16"))), ((bool)(resources.GetObject("tileContainer.Buttons17")))),
            new DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton(resources.GetString("tileContainer.Buttons18"), ((System.Drawing.Image)(resources.GetObject("tileContainer.Buttons19"))), ((int)(resources.GetObject("tileContainer.Buttons20"))), ((DevExpress.XtraBars.Docking2010.ImageLocation)(resources.GetObject("tileContainer.Buttons21"))), ((DevExpress.XtraBars.Docking2010.ButtonStyle)(resources.GetObject("tileContainer.Buttons22"))), resources.GetString("tileContainer.Buttons23"), ((bool)(resources.GetObject("tileContainer.Buttons24"))), ((int)(resources.GetObject("tileContainer.Buttons25"))), ((bool)(resources.GetObject("tileContainer.Buttons26"))), ((DevExpress.Utils.SuperToolTip)(resources.GetObject("tileContainer.Buttons27"))), ((bool)(resources.GetObject("tileContainer.Buttons28"))), ((bool)(resources.GetObject("tileContainer.Buttons29"))), ((bool)(resources.GetObject("tileContainer.Buttons30"))), ((object)(resources.GetObject("tileContainer.Buttons31"))), ((object)(resources.GetObject("tileContainer.Buttons32"))), ((int)(resources.GetObject("tileContainer.Buttons33"))), ((bool)(resources.GetObject("tileContainer.Buttons34"))), ((bool)(resources.GetObject("tileContainer.Buttons35")))),
            new DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton(resources.GetString("tileContainer.Buttons36"), ((System.Drawing.Image)(resources.GetObject("tileContainer.Buttons37"))), ((int)(resources.GetObject("tileContainer.Buttons38"))), ((DevExpress.XtraBars.Docking2010.ImageLocation)(resources.GetObject("tileContainer.Buttons39"))), ((DevExpress.XtraBars.Docking2010.ButtonStyle)(resources.GetObject("tileContainer.Buttons40"))), resources.GetString("tileContainer.Buttons41"), ((bool)(resources.GetObject("tileContainer.Buttons42"))), ((int)(resources.GetObject("tileContainer.Buttons43"))), ((bool)(resources.GetObject("tileContainer.Buttons44"))), ((DevExpress.Utils.SuperToolTip)(resources.GetObject("tileContainer.Buttons45"))), ((bool)(resources.GetObject("tileContainer.Buttons46"))), ((bool)(resources.GetObject("tileContainer.Buttons47"))), ((bool)(resources.GetObject("tileContainer.Buttons48"))), ((object)(resources.GetObject("tileContainer.Buttons49"))), ((object)(resources.GetObject("tileContainer.Buttons50"))), ((int)(resources.GetObject("tileContainer.Buttons51"))), ((bool)(resources.GetObject("tileContainer.Buttons52"))), ((bool)(resources.GetObject("tileContainer.Buttons53")))),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton(resources.GetString("tileContainer.Buttons54"), ((System.Drawing.Image)(resources.GetObject("tileContainer.Buttons55"))), ((int)(resources.GetObject("tileContainer.Buttons56"))), ((DevExpress.XtraBars.Docking2010.ImageLocation)(resources.GetObject("tileContainer.Buttons57"))), ((DevExpress.XtraBars.Docking2010.ButtonStyle)(resources.GetObject("tileContainer.Buttons58"))), resources.GetString("tileContainer.Buttons59"), ((bool)(resources.GetObject("tileContainer.Buttons60"))), ((int)(resources.GetObject("tileContainer.Buttons61"))), ((bool)(resources.GetObject("tileContainer.Buttons62"))), ((DevExpress.Utils.SuperToolTip)(resources.GetObject("tileContainer.Buttons63"))), ((bool)(resources.GetObject("tileContainer.Buttons64"))), ((bool)(resources.GetObject("tileContainer.Buttons65"))), ((bool)(resources.GetObject("tileContainer.Buttons66"))), ((object)(resources.GetObject("tileContainer.Buttons67"))), ((object)(resources.GetObject("tileContainer.Buttons68"))), ((int)(resources.GetObject("tileContainer.Buttons69"))), ((bool)(resources.GetObject("tileContainer.Buttons70"))), ((bool)(resources.GetObject("tileContainer.Buttons71")))),
            new DevExpress.XtraBars.Docking2010.WindowsUISeparator()});
            this.tileContainer.Name = "tileContainer1";
            this.tileContainer.Properties.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.tileContainer.Properties.ShowGroupText = DevExpress.Utils.DefaultBoolean.True;
            // 
            // actionList1
            // 
            this.actionList1.Actions.AddRange(new Dsu.Common.Utilities.Actions.Action[] {
            this.action1});
            this.actionList1.Count = 1;
            this.actionList1.ImageList = null;
            this.actionList1.ShowTextOnToolBar = false;
            this.actionList1.Tag = null;
            this.actionList1.UpdateCmdUISleepIntervalOnIdle = 0;
            // 
            // action1
            // 
            this.action1.Checked = false;
            this.action1.Enabled = true;
            resources.ApplyResources(this.action1, "action1");
            this.action1.Shortcut = System.Windows.Forms.Shortcut.None;
            this.action1.Tag = null;
            this.action1.Visible = true;
            this.action1.Update += new System.EventHandler(this.action1_Update);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsUIView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileContainer)).EndInit();
            this.ResumeLayout(false);

        }

   

        #endregion

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.WindowsUIView windowsUIView;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.TileContainer tileContainer;
        private Dsu.Common.Utilities.Actions.ActionList actionList1;
        private Dsu.Common.Utilities.Actions.Action action1;
    }
}

