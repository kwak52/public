namespace Dsu.PLCConverter.UI
{
    partial class FormRange
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
            this.textEditEnd = new DevExpress.XtraEditors.TextEdit();
            this.textEditStart = new DevExpress.XtraEditors.TextEdit();
            this.comboUnit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.btnApply = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboUnit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.textEditEnd);
            this.layoutControl1.Controls.Add(this.textEditStart);
            this.layoutControl1.Controls.Add(this.comboUnit);
            this.layoutControl1.Location = new System.Drawing.Point(12, 12);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(250, 125);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // textEditEnd
            // 
            this.textEditEnd.Location = new System.Drawing.Point(41, 80);
            this.textEditEnd.Name = "textEditEnd";
            this.textEditEnd.Size = new System.Drawing.Size(183, 30);
            this.textEditEnd.StyleController = this.layoutControl1;
            this.textEditEnd.TabIndex = 6;
            this.textEditEnd.Validating += new System.ComponentModel.CancelEventHandler(this.textEditRange_Validating);
            // 
            // textEditStart
            // 
            this.textEditStart.Location = new System.Drawing.Point(41, 46);
            this.textEditStart.Name = "textEditStart";
            this.textEditStart.Size = new System.Drawing.Size(183, 30);
            this.textEditStart.StyleController = this.layoutControl1;
            this.textEditStart.TabIndex = 5;
            this.textEditStart.Validating += new System.ComponentModel.CancelEventHandler(this.textEditRange_Validating);
            // 
            // comboUnit
            // 
            this.comboUnit.Location = new System.Drawing.Point(41, 12);
            this.comboUnit.Name = "comboUnit";
            this.comboUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboUnit.Size = new System.Drawing.Size(197, 30);
            this.comboUnit.StyleController = this.layoutControl1;
            this.comboUnit.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(250, 125);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.comboUnit;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(230, 34);
            this.layoutControlItem1.Text = "단위";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(26, 22);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(216, 34);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(14, 71);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.textEditStart;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 34);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(216, 34);
            this.layoutControlItem2.Text = "시작";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(26, 22);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.textEditEnd;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 68);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(216, 37);
            this.layoutControlItem3.Text = "끝";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(26, 22);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(138, 143);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(112, 34);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "적용";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // FormRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 191);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.layoutControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRange";
            this.Text = "메모리 영역 선택";
            this.Load += new System.EventHandler(this.FormRange_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEditEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboUnit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.TextEdit textEditEnd;
        private DevExpress.XtraEditors.TextEdit textEditStart;
        private DevExpress.XtraEditors.ComboBoxEdit comboUnit;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton btnApply;
    }
}