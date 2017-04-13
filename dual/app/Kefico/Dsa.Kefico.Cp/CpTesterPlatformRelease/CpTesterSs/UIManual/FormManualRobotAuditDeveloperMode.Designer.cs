namespace CpTesterPlatform.CpTester
{
    partial class FormManualRobotAuditDeveloperMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManualRobotAuditDeveloperMode));
            this.btnForceHomeSet = new System.Windows.Forms.Button();
            this.btnDoMachineOrigin = new System.Windows.Forms.Button();
            this.btnTestBreak = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageAudit78 = new System.Windows.Forms.TabPage();
            this.cbAdjustEncPosition = new System.Windows.Forms.CheckBox();
            this.btnUnpark = new System.Windows.Forms.Button();
            this.btnPark = new System.Windows.Forms.Button();
            this.tabPageGCVT = new System.Windows.Forms.TabPage();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnMoveToRobotOrigin = new System.Windows.Forms.Button();
            this.btnMoveToKissOrigin = new System.Windows.Forms.Button();
            this.simpleButton_Emergency = new DevExpress.XtraEditors.SimpleButton();
            this.actionList1 = new Dsu.Common.Utilities.Actions.ActionList(this.components);
            this.action1 = new Dsu.Common.Utilities.Actions.Action(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageAudit78.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnForceHomeSet
            // 
            this.btnForceHomeSet.Location = new System.Drawing.Point(6, 6);
            this.btnForceHomeSet.Name = "btnForceHomeSet";
            this.btnForceHomeSet.Size = new System.Drawing.Size(103, 36);
            this.btnForceHomeSet.TabIndex = 2;
            this.btnForceHomeSet.Text = "Force home set";
            this.btnForceHomeSet.UseVisualStyleBackColor = true;
            this.btnForceHomeSet.Click += new System.EventHandler(this.btnForceHomeSet_Click);
            // 
            // btnDoMachineOrigin
            // 
            this.btnDoMachineOrigin.Location = new System.Drawing.Point(6, 59);
            this.btnDoMachineOrigin.Name = "btnDoMachineOrigin";
            this.btnDoMachineOrigin.Size = new System.Drawing.Size(103, 27);
            this.btnDoMachineOrigin.TabIndex = 3;
            this.btnDoMachineOrigin.Text = "Do robot origin";
            this.toolTip1.SetToolTip(this.btnDoMachineOrigin, "원점 수행");
            this.btnDoMachineOrigin.UseVisualStyleBackColor = true;
            this.btnDoMachineOrigin.Click += new System.EventHandler(this.btnDoMachineOrigin_Click);
            // 
            // btnTestBreak
            // 
            this.btnTestBreak.Location = new System.Drawing.Point(6, 92);
            this.btnTestBreak.Name = "btnTestBreak";
            this.btnTestBreak.Size = new System.Drawing.Size(103, 27);
            this.btnTestBreak.TabIndex = 4;
            this.btnTestBreak.Text = "Test break on/off";
            this.btnTestBreak.UseVisualStyleBackColor = true;
            this.btnTestBreak.Click += new System.EventHandler(this.btnTestBreak_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageAudit78);
            this.tabControl1.Controls.Add(this.tabPageGCVT);
            this.tabControl1.Location = new System.Drawing.Point(12, 102);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(263, 189);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPageAudit78
            // 
            this.tabPageAudit78.Controls.Add(this.cbAdjustEncPosition);
            this.tabPageAudit78.Controls.Add(this.btnForceHomeSet);
            this.tabPageAudit78.Controls.Add(this.btnUnpark);
            this.tabPageAudit78.Controls.Add(this.btnPark);
            this.tabPageAudit78.Controls.Add(this.btnDoMachineOrigin);
            this.tabPageAudit78.Controls.Add(this.btnTestBreak);
            this.tabPageAudit78.Location = new System.Drawing.Point(4, 22);
            this.tabPageAudit78.Name = "tabPageAudit78";
            this.tabPageAudit78.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAudit78.Size = new System.Drawing.Size(255, 163);
            this.tabPageAudit78.TabIndex = 0;
            this.tabPageAudit78.Text = "Audit78";
            this.tabPageAudit78.UseVisualStyleBackColor = true;
            // 
            // cbAdjustEncPosition
            // 
            this.cbAdjustEncPosition.AutoSize = true;
            this.cbAdjustEncPosition.Location = new System.Drawing.Point(3, 138);
            this.cbAdjustEncPosition.Name = "cbAdjustEncPosition";
            this.cbAdjustEncPosition.Size = new System.Drawing.Size(118, 17);
            this.cbAdjustEncPosition.TabIndex = 7;
            this.cbAdjustEncPosition.Text = "Adjust Enc poisition";
            this.cbAdjustEncPosition.UseVisualStyleBackColor = true;
            // 
            // btnUnpark
            // 
            this.btnUnpark.Location = new System.Drawing.Point(134, 92);
            this.btnUnpark.Name = "btnUnpark";
            this.btnUnpark.Size = new System.Drawing.Size(103, 27);
            this.btnUnpark.TabIndex = 6;
            this.btnUnpark.Text = "Unpark";
            this.toolTip1.SetToolTip(this.btnUnpark, "Unbreak");
            this.btnUnpark.UseVisualStyleBackColor = true;
            this.btnUnpark.Click += new System.EventHandler(this.btnUnpark_Click);
            // 
            // btnPark
            // 
            this.btnPark.Location = new System.Drawing.Point(134, 59);
            this.btnPark.Name = "btnPark";
            this.btnPark.Size = new System.Drawing.Size(103, 27);
            this.btnPark.TabIndex = 5;
            this.btnPark.Text = "Park";
            this.toolTip1.SetToolTip(this.btnPark, "Break");
            this.btnPark.UseVisualStyleBackColor = true;
            this.btnPark.Click += new System.EventHandler(this.btnPark_Click);
            // 
            // tabPageGCVT
            // 
            this.tabPageGCVT.Location = new System.Drawing.Point(4, 22);
            this.tabPageGCVT.Name = "tabPageGCVT";
            this.tabPageGCVT.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGCVT.Size = new System.Drawing.Size(305, 237);
            this.tabPageGCVT.TabIndex = 1;
            this.tabPageGCVT.Text = "GCVT";
            this.tabPageGCVT.UseVisualStyleBackColor = true;
            // 
            // btnMoveToRobotOrigin
            // 
            this.btnMoveToRobotOrigin.Location = new System.Drawing.Point(12, 12);
            this.btnMoveToRobotOrigin.Name = "btnMoveToRobotOrigin";
            this.btnMoveToRobotOrigin.Size = new System.Drawing.Size(125, 27);
            this.btnMoveToRobotOrigin.TabIndex = 6;
            this.btnMoveToRobotOrigin.Text = "Move to robot origin";
            this.toolTip1.SetToolTip(this.btnMoveToRobotOrigin, "로봇 원점으로 이동");
            this.btnMoveToRobotOrigin.UseVisualStyleBackColor = true;
            this.btnMoveToRobotOrigin.Click += new System.EventHandler(this.btnMoveToRobotOrigin_Click);
            // 
            // btnMoveToKissOrigin
            // 
            this.btnMoveToKissOrigin.Location = new System.Drawing.Point(12, 41);
            this.btnMoveToKissOrigin.Name = "btnMoveToKissOrigin";
            this.btnMoveToKissOrigin.Size = new System.Drawing.Size(125, 27);
            this.btnMoveToKissOrigin.TabIndex = 27;
            this.btnMoveToKissOrigin.Text = "Move to tester origin";
            this.toolTip1.SetToolTip(this.btnMoveToKissOrigin, "기구 원점(kiss origin)으로 이동");
            this.btnMoveToKissOrigin.UseVisualStyleBackColor = true;
            this.btnMoveToKissOrigin.Click += new System.EventHandler(this.btnMoveToKissOrigin_Click);
            // 
            // simpleButton_Emergency
            // 
            this.simpleButton_Emergency.Appearance.BackColor = System.Drawing.Color.Firebrick;
            this.simpleButton_Emergency.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton_Emergency.Appearance.ForeColor = System.Drawing.Color.Black;
            this.simpleButton_Emergency.Appearance.Options.UseBackColor = true;
            this.simpleButton_Emergency.Appearance.Options.UseFont = true;
            this.simpleButton_Emergency.Appearance.Options.UseForeColor = true;
            this.simpleButton_Emergency.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton_Emergency.Image")));
            this.simpleButton_Emergency.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.simpleButton_Emergency.Location = new System.Drawing.Point(150, 12);
            this.simpleButton_Emergency.Name = "simpleButton_Emergency";
            this.simpleButton_Emergency.Size = new System.Drawing.Size(123, 56);
            this.simpleButton_Emergency.TabIndex = 26;
            this.simpleButton_Emergency.Text = "Stop";
            this.simpleButton_Emergency.Click += new System.EventHandler(this.simpleButton_Emergency_Click);
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
            this.action1.Hint = null;
            this.action1.Shortcut = System.Windows.Forms.Shortcut.None;
            this.action1.Tag = null;
            this.action1.Text = "8957111";
            this.action1.Visible = true;
            this.action1.Update += new System.EventHandler(this.action1_Update);
            // 
            // FormManualRobotAudit78DeveloperMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 303);
            this.Controls.Add(this.btnMoveToKissOrigin);
            this.Controls.Add(this.simpleButton_Emergency);
            this.Controls.Add(this.btnMoveToRobotOrigin);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormManualRobotAudit78DeveloperMode";
            this.Text = "Developer Mode";
            this.Load += new System.EventHandler(this.FormManualRobotAuditDeveloperMode_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageAudit78.ResumeLayout(false);
            this.tabPageAudit78.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnForceHomeSet;
        private System.Windows.Forms.Button btnDoMachineOrigin;
        private System.Windows.Forms.Button btnTestBreak;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageAudit78;
        private System.Windows.Forms.TabPage tabPageGCVT;
        private System.Windows.Forms.Button btnUnpark;
        private System.Windows.Forms.Button btnPark;
        private System.Windows.Forms.CheckBox cbAdjustEncPosition;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnMoveToRobotOrigin;
        private DevExpress.XtraEditors.SimpleButton simpleButton_Emergency;
        private Dsu.Common.Utilities.Actions.ActionList actionList1;
        private Dsu.Common.Utilities.Actions.Action action1;
        private System.Windows.Forms.Button btnMoveToKissOrigin;
    }
}