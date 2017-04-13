﻿namespace CpTesterPlatform.CpTrendView
{
    partial class FormDaqDisplaySelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDaqDisplaySelector));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.checkedListBoxControlDO = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonSet = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemDO = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControlDO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDO)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.panelControl1.Appearance.BorderColor = System.Drawing.Color.DimGray;
            this.panelControl1.Appearance.Options.UseBorderColor = true;
            this.panelControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(233, 192);
            this.panelControl1.TabIndex = 2;
            // 
            // groupControl1
            // 
            this.groupControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupControl1.Controls.Add(this.layoutControl1);
            this.groupControl1.Location = new System.Drawing.Point(6, 7);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(217, 176);
            this.groupControl1.TabIndex = 6;
            this.groupControl1.Text = "DAQ display channels";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.checkedListBoxControlDO);
            this.layoutControl1.Controls.Add(this.simpleButtonCancel);
            this.layoutControl1.Controls.Add(this.simpleButtonSet);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 20);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(579, 96, 250, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(213, 154);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // checkedListBoxControlDO
            // 
            this.checkedListBoxControlDO.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.checkedListBoxControlDO.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkedListBoxControlDO.Location = new System.Drawing.Point(12, 34);
            this.checkedListBoxControlDO.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.checkedListBoxControlDO.Name = "checkedListBoxControlDO";
            this.checkedListBoxControlDO.Size = new System.Drawing.Size(189, 82);
            this.checkedListBoxControlDO.StyleController = this.layoutControl1;
            this.checkedListBoxControlDO.TabIndex = 9;
            this.checkedListBoxControlDO.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.checkedListBoxControlDO_ItemCheck);
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonCancel.Image")));
            this.simpleButtonCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButtonCancel.Location = new System.Drawing.Point(108, 120);
            this.simpleButtonCancel.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(93, 22);
            this.simpleButtonCancel.StyleController = this.layoutControl1;
            this.simpleButtonCancel.TabIndex = 8;
            this.simpleButtonCancel.Text = "Cancel";
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // simpleButtonSet
            // 
            this.simpleButtonSet.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonSet.Image")));
            this.simpleButtonSet.Location = new System.Drawing.Point(12, 120);
            this.simpleButtonSet.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.simpleButtonSet.Name = "simpleButtonSet";
            this.simpleButtonSet.Size = new System.Drawing.Size(92, 22);
            this.simpleButtonSet.StyleController = this.layoutControl1;
            this.simpleButtonSet.TabIndex = 7;
            this.simpleButtonSet.Text = "Set";
            this.simpleButtonSet.Click += new System.EventHandler(this.simpleButtonSet_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItemDO});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 6;
            this.layoutControlGroup1.Size = new System.Drawing.Size(213, 154);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.simpleButtonSet;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 108);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(96, 26);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.simpleButtonCancel;
            this.layoutControlItem4.Location = new System.Drawing.Point(96, 108);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(97, 26);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItemDO
            // 
            this.layoutControlItemDO.Control = this.checkedListBoxControlDO;
            this.layoutControlItemDO.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItemDO.Image")));
            this.layoutControlItemDO.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemDO.Name = "layoutControlItemDO";
            this.layoutControlItemDO.Size = new System.Drawing.Size(193, 108);
            this.layoutControlItemDO.Text = "Select DAQ channels";
            this.layoutControlItemDO.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItemDO.TextSize = new System.Drawing.Size(120, 16);
            // 
            // FormDaqDisplaySelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 192);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.MaximumSize = new System.Drawing.Size(249, 230);
            this.MinimumSize = new System.Drawing.Size(249, 230);
            this.Name = "FormDaqDisplaySelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DAQ display channel selection";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormDaqDisplaySelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControlDO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.CheckedListBoxControl checkedListBoxControlDO;
        private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSet;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemDO;
    }
}