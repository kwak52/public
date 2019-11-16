namespace PLCConvertor.Forms
{
    partial class FormEditPreferences
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.textEditLabelHeader = new DevExpress.XtraEditors.TextEdit();
            this.checkEditCopySourceComment = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditAddMessagesToLabel = new DevExpress.XtraEditors.CheckEdit();
            this.comboBoxEditLogLevel = new DevExpress.XtraEditors.ComboBoxEdit();
            this.checkEditForceSplitRung = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditSplitBySection = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditLabelHeader.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditCopySourceComment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditAddMessagesToLabel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditLogLevel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditForceSplitRung.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSplitBySection.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.textEditLabelHeader);
            this.layoutControl1.Controls.Add(this.checkEditCopySourceComment);
            this.layoutControl1.Controls.Add(this.checkEditAddMessagesToLabel);
            this.layoutControl1.Controls.Add(this.comboBoxEditLogLevel);
            this.layoutControl1.Controls.Add(this.checkEditForceSplitRung);
            this.layoutControl1.Controls.Add(this.checkEditSplitBySection);
            this.layoutControl1.Location = new System.Drawing.Point(10, 10);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(281, 208);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // textEditLabelHeader
            // 
            this.textEditLabelHeader.Location = new System.Drawing.Point(98, 116);
            this.textEditLabelHeader.Name = "textEditLabelHeader";
            this.textEditLabelHeader.Size = new System.Drawing.Size(171, 24);
            this.textEditLabelHeader.StyleController = this.layoutControl1;
            this.textEditLabelHeader.TabIndex = 9;
            // 
            // checkEditCopySourceComment
            // 
            this.checkEditCopySourceComment.Location = new System.Drawing.Point(12, 64);
            this.checkEditCopySourceComment.Name = "checkEditCopySourceComment";
            this.checkEditCopySourceComment.Properties.Caption = "Copy Source Comment";
            this.checkEditCopySourceComment.Size = new System.Drawing.Size(257, 22);
            this.checkEditCopySourceComment.StyleController = this.layoutControl1;
            this.checkEditCopySourceComment.TabIndex = 8;
            this.checkEditCopySourceComment.ToolTip = "원본PLC 의 주석을 복사할지 여부";
            // 
            // checkEditAddMessagesToLabel
            // 
            this.checkEditAddMessagesToLabel.Location = new System.Drawing.Point(12, 90);
            this.checkEditAddMessagesToLabel.Margin = new System.Windows.Forms.Padding(2);
            this.checkEditAddMessagesToLabel.Name = "checkEditAddMessagesToLabel";
            this.checkEditAddMessagesToLabel.Properties.Caption = "Add messages to label";
            this.checkEditAddMessagesToLabel.Size = new System.Drawing.Size(257, 22);
            this.checkEditAddMessagesToLabel.StyleController = this.layoutControl1;
            this.checkEditAddMessagesToLabel.TabIndex = 7;
            this.checkEditAddMessagesToLabel.ToolTip = "변환 중에 발생한 메시지를 주석으로 삽입할지 여부";
            // 
            // comboBoxEditLogLevel
            // 
            this.comboBoxEditLogLevel.Location = new System.Drawing.Point(98, 144);
            this.comboBoxEditLogLevel.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxEditLogLevel.Name = "comboBoxEditLogLevel";
            this.comboBoxEditLogLevel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditLogLevel.Size = new System.Drawing.Size(171, 24);
            this.comboBoxEditLogLevel.StyleController = this.layoutControl1;
            this.comboBoxEditLogLevel.TabIndex = 6;
            // 
            // checkEditForceSplitRung
            // 
            this.checkEditForceSplitRung.EditValue = true;
            this.checkEditForceSplitRung.Location = new System.Drawing.Point(12, 38);
            this.checkEditForceSplitRung.Margin = new System.Windows.Forms.Padding(2);
            this.checkEditForceSplitRung.Name = "checkEditForceSplitRung";
            this.checkEditForceSplitRung.Properties.Caption = "Force split rung";
            this.checkEditForceSplitRung.Size = new System.Drawing.Size(257, 22);
            this.checkEditForceSplitRung.StyleController = this.layoutControl1;
            this.checkEditForceSplitRung.TabIndex = 5;
            this.checkEditForceSplitRung.ToolTip = "Force split rung by rung for possible error.";
            // 
            // checkEditSplitBySection
            // 
            this.checkEditSplitBySection.EditValue = true;
            this.checkEditSplitBySection.Location = new System.Drawing.Point(12, 12);
            this.checkEditSplitBySection.Margin = new System.Windows.Forms.Padding(2);
            this.checkEditSplitBySection.Name = "checkEditSplitBySection";
            this.checkEditSplitBySection.Properties.Caption = "Split by section";
            this.checkEditSplitBySection.Size = new System.Drawing.Size(257, 22);
            this.checkEditSplitBySection.StyleController = this.layoutControl1;
            this.checkEditSplitBySection.TabIndex = 4;
            this.checkEditSplitBySection.ToolTip = "Force split CXP section into Xg5k program.";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(281, 208);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.checkEditSplitBySection;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(261, 26);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 160);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(261, 28);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.checkEditForceSplitRung;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(261, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.comboBoxEditLogLevel;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 132);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(261, 28);
            this.layoutControlItem3.Text = "Log level";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(83, 18);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.checkEditAddMessagesToLabel;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 78);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(261, 26);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.checkEditCopySourceComment;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 52);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(261, 26);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.textEditLabelHeader;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 104);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(261, 28);
            this.layoutControlItem6.Text = "Label header";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(83, 18);
            // 
            // FormEditPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 224);
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormEditPreferences";
            this.Text = "FormEditPreferences";
            this.Load += new System.EventHandler(this.FormEditPreferences_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEditLabelHeader.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditCopySourceComment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditAddMessagesToLabel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditLogLevel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditForceSplitRung.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSplitBySection.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.CheckEdit checkEditForceSplitRung;
        private DevExpress.XtraEditors.CheckEdit checkEditSplitBySection;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditLogLevel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.CheckEdit checkEditAddMessagesToLabel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.CheckEdit checkEditCopySourceComment;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.TextEdit textEditLabelHeader;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}