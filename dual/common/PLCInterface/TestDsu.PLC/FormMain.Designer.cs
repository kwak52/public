namespace TestDsu.PLC
{
    partial class FormMain
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageMelsec = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnMonitor = new System.Windows.Forms.Button();
            this.enumEditorPLCType = new Dsu.Common.Utilities.EnumEditor();
            this.actionList1 = new Dsu.Common.Utilities.Actions.ActionList(this.components);
            this.action1 = new Dsu.Common.Utilities.Actions.Action(this.components);
            this.btnStopMonitor = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageMelsec);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 142);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(226, 204);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageMelsec
            // 
            this.tabPageMelsec.Location = new System.Drawing.Point(4, 22);
            this.tabPageMelsec.Name = "tabPageMelsec";
            this.tabPageMelsec.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMelsec.Size = new System.Drawing.Size(218, 178);
            this.tabPageMelsec.TabIndex = 0;
            this.tabPageMelsec.Text = "Melsec";
            this.tabPageMelsec.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(218, 178);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "host/IP address:";
            // 
            // tbHost
            // 
            this.tbHost.Location = new System.Drawing.Point(143, 18);
            this.tbHost.Name = "tbHost";
            this.tbHost.Size = new System.Drawing.Size(209, 21);
            this.tbHost.TabIndex = 2;
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(143, 45);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(209, 21);
            this.tbPort.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port:";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(168, 72);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(89, 23);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(263, 72);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(89, 23);
            this.btnDisconnect.TabIndex = 6;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnMonitor
            // 
            this.btnMonitor.Location = new System.Drawing.Point(168, 113);
            this.btnMonitor.Name = "btnMonitor";
            this.btnMonitor.Size = new System.Drawing.Size(89, 23);
            this.btnMonitor.TabIndex = 8;
            this.btnMonitor.Text = "Monitor";
            this.btnMonitor.UseVisualStyleBackColor = true;
            this.btnMonitor.Click += new System.EventHandler(this.btnMonitor_Click);
            // 
            // enumEditorPLCType
            // 
            this.enumEditorPLCType.ControlSpacing = 20;
            this.enumEditorPLCType.EnumType = null;
            this.enumEditorPLCType.EnumValue = ((long)(0));
            this.enumEditorPLCType.LableFormat = "{0}";
            this.enumEditorPLCType.LayoutMode = Dsu.Common.Utilities.LayoutMode.Portrait;
            this.enumEditorPLCType.Location = new System.Drawing.Point(240, 133);
            this.enumEditorPLCType.Name = "enumEditorPLCType";
            this.enumEditorPLCType.Size = new System.Drawing.Size(150, 150);
            this.enumEditorPLCType.TabIndex = 7;
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
            this.action1.Text = "61156362";
            this.action1.Visible = true;
            this.action1.Update += new System.EventHandler(this.action1_Update);
            // 
            // btnStopMonitor
            // 
            this.btnStopMonitor.Location = new System.Drawing.Point(263, 113);
            this.btnStopMonitor.Name = "btnStopMonitor";
            this.btnStopMonitor.Size = new System.Drawing.Size(89, 23);
            this.btnStopMonitor.TabIndex = 9;
            this.btnStopMonitor.Text = "Stop Monitor";
            this.btnStopMonitor.UseVisualStyleBackColor = true;
            this.btnStopMonitor.Click += new System.EventHandler(this.btnStopMonitor_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 358);
            this.Controls.Add(this.btnStopMonitor);
            this.Controls.Add(this.btnMonitor);
            this.Controls.Add(this.enumEditorPLCType);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbHost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageMelsec;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private global::Dsu.Common.Utilities.EnumEditor enumEditorPLCType;
        private System.Windows.Forms.Button btnMonitor;
        private Dsu.Common.Utilities.Actions.ActionList actionList1;
        private Dsu.Common.Utilities.Actions.Action action1;
        private System.Windows.Forms.Button btnStopMonitor;
    }
}

