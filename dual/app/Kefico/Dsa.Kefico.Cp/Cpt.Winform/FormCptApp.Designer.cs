namespace Cpt.Winform
{
    partial class FormCptApp
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
            this.send = new System.Windows.Forms.Button();
            this.textBoxGaudiFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbHotTest = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cbRoomTest = new System.Windows.Forms.CheckBox();
            this.cbTotalTest = new System.Windows.Forms.CheckBox();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerOFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cPTSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.pingMwsServerToolStripMenuItemx = new System.Windows.Forms.ToolStripMenuItem();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pingMwsServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pingSQLServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crashServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.generateTestResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulateTestResultEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.sendStepRequestRepeatedlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.poisonPillToServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbProductNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPdvId = new System.Windows.Forms.TextBox();
            this.uploadCpXmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // send
            // 
            this.send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.send.Location = new System.Drawing.Point(409, 36);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(75, 55);
            this.send.TabIndex = 2;
            this.send.Text = "send";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // textBoxGaudiFile
            // 
            this.textBoxGaudiFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGaudiFile.Location = new System.Drawing.Point(176, 32);
            this.textBoxGaudiFile.Name = "textBoxGaudiFile";
            this.textBoxGaudiFile.ReadOnly = true;
            this.textBoxGaudiFile.Size = new System.Drawing.Size(227, 21);
            this.textBoxGaudiFile.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(141, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "File:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "Gate:";
            // 
            // cbHotTest
            // 
            this.cbHotTest.AutoSize = true;
            this.cbHotTest.Enabled = false;
            this.cbHotTest.Location = new System.Drawing.Point(59, 70);
            this.cbHotTest.Name = "cbHotTest";
            this.cbHotTest.Size = new System.Drawing.Size(32, 16);
            this.cbHotTest.TabIndex = 13;
            this.cbHotTest.Text = "H";
            this.toolTip1.SetToolTip(this.cbHotTest, "Hot test");
            this.cbHotTest.UseVisualStyleBackColor = true;
            // 
            // cbRoomTest
            // 
            this.cbRoomTest.AutoSize = true;
            this.cbRoomTest.Enabled = false;
            this.cbRoomTest.Location = new System.Drawing.Point(99, 70);
            this.cbRoomTest.Name = "cbRoomTest";
            this.cbRoomTest.Size = new System.Drawing.Size(32, 16);
            this.cbRoomTest.TabIndex = 14;
            this.cbRoomTest.Text = "R";
            this.toolTip1.SetToolTip(this.cbRoomTest, "Room test");
            this.cbRoomTest.UseVisualStyleBackColor = true;
            // 
            // cbTotalTest
            // 
            this.cbTotalTest.AutoSize = true;
            this.cbTotalTest.Enabled = false;
            this.cbTotalTest.Location = new System.Drawing.Point(139, 70);
            this.cbTotalTest.Name = "cbTotalTest";
            this.cbTotalTest.Size = new System.Drawing.Size(32, 16);
            this.cbTotalTest.TabIndex = 15;
            this.cbTotalTest.Text = "T";
            this.toolTip1.SetToolTip(this.cbTotalTest, "Total test");
            this.cbTotalTest.UseVisualStyleBackColor = true;
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(12, 97);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(472, 472);
            this.gridControl1.TabIndex = 16;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configToolStripMenuItem,
            this.serverToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(496, 24);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.powerONToolStripMenuItem,
            this.powerOFFToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem,
            this.uploadCpXmlToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // powerONToolStripMenuItem
            // 
            this.powerONToolStripMenuItem.Name = "powerONToolStripMenuItem";
            this.powerONToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.powerONToolStripMenuItem.Text = "Power ON";
            this.powerONToolStripMenuItem.Click += new System.EventHandler(this.powerONToolStripMenuItem_Click);
            // 
            // powerOFFToolStripMenuItem
            // 
            this.powerOFFToolStripMenuItem.Name = "powerOFFToolStripMenuItem";
            this.powerOFFToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.powerOFFToolStripMenuItem.Text = "Power OFF";
            this.powerOFFToolStripMenuItem.Click += new System.EventHandler(this.powerOFFToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(156, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cPTSetupToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripSeparator2,
            this.pingMwsServerToolStripMenuItemx});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.configToolStripMenuItem.Text = "Config";
            // 
            // cPTSetupToolStripMenuItem
            // 
            this.cPTSetupToolStripMenuItem.Name = "cPTSetupToolStripMenuItem";
            this.cPTSetupToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.cPTSetupToolStripMenuItem.Text = "CP tester setup...";
            this.cPTSetupToolStripMenuItem.Click += new System.EventHandler(this.cPTSetupToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
            // 
            // pingMwsServerToolStripMenuItemx
            // 
            this.pingMwsServerToolStripMenuItemx.Name = "pingMwsServerToolStripMenuItemx";
            this.pingMwsServerToolStripMenuItemx.Size = new System.Drawing.Size(169, 22);
            this.pingMwsServerToolStripMenuItemx.Text = "Ping server";
            this.pingMwsServerToolStripMenuItemx.Click += new System.EventHandler(this.pingMwsServerToolStripMenuItem_Click);
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pingMwsServerToolStripMenuItem,
            this.pingSQLServerToolStripMenuItem,
            this.crashServerToolStripMenuItem,
            this.toolStripSeparator4,
            this.generateTestResultToolStripMenuItem,
            this.simulateTestResultEditToolStripMenuItem,
            this.toolStripSeparator5,
            this.sendStepRequestRepeatedlyToolStripMenuItem,
            this.poisonPillToServerToolStripMenuItem});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.serverToolStripMenuItem.Text = "Server";
            // 
            // pingMwsServerToolStripMenuItem
            // 
            this.pingMwsServerToolStripMenuItem.Name = "pingMwsServerToolStripMenuItem";
            this.pingMwsServerToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.pingMwsServerToolStripMenuItem.Text = "Ping MWS Server";
            this.pingMwsServerToolStripMenuItem.Click += new System.EventHandler(this.pingMwsServerToolStripMenuItem_Click);
            // 
            // pingSQLServerToolStripMenuItem
            // 
            this.pingSQLServerToolStripMenuItem.Name = "pingSQLServerToolStripMenuItem";
            this.pingSQLServerToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.pingSQLServerToolStripMenuItem.Text = "Ping SQL Server";
            this.pingSQLServerToolStripMenuItem.Click += new System.EventHandler(this.pingSQLServerToolStripMenuItem_Click);
            // 
            // crashServerToolStripMenuItem
            // 
            this.crashServerToolStripMenuItem.Name = "crashServerToolStripMenuItem";
            this.crashServerToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.crashServerToolStripMenuItem.Text = "Crash Server";
            this.crashServerToolStripMenuItem.ToolTipText = "Crash server for debugging purpose only.";
            this.crashServerToolStripMenuItem.Click += new System.EventHandler(this.crashServerToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(233, 6);
            // 
            // generateTestResultToolStripMenuItem
            // 
            this.generateTestResultToolStripMenuItem.Name = "generateTestResultToolStripMenuItem";
            this.generateTestResultToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.generateTestResultToolStripMenuItem.Text = "Generate test result...";
            this.generateTestResultToolStripMenuItem.Click += new System.EventHandler(this.generateTestResultToolStripMenuItem_Click);
            // 
            // simulateTestResultEditToolStripMenuItem
            // 
            this.simulateTestResultEditToolStripMenuItem.Name = "simulateTestResultEditToolStripMenuItem";
            this.simulateTestResultEditToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.simulateTestResultEditToolStripMenuItem.Text = "Simulate test result edit...";
            this.simulateTestResultEditToolStripMenuItem.Click += new System.EventHandler(this.simulateTestResultEditToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(233, 6);
            // 
            // sendStepRequestRepeatedlyToolStripMenuItem
            // 
            this.sendStepRequestRepeatedlyToolStripMenuItem.Name = "sendStepRequestRepeatedlyToolStripMenuItem";
            this.sendStepRequestRepeatedlyToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.sendStepRequestRepeatedlyToolStripMenuItem.Text = "Send step request repeatedly";
            this.sendStepRequestRepeatedlyToolStripMenuItem.ToolTipText = "Debugging purpose only.. Simulates repeated step request.";
            this.sendStepRequestRepeatedlyToolStripMenuItem.Click += new System.EventHandler(this.sendStepRequestRepeatedlyToolStripMenuItem_Click);
            // 
            // poisonPillToServerToolStripMenuItem
            // 
            this.poisonPillToServerToolStripMenuItem.Name = "poisonPillToServerToolStripMenuItem";
            this.poisonPillToServerToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.poisonPillToServerToolStripMenuItem.Text = "Poison pill to server";
            this.poisonPillToServerToolStripMenuItem.ToolTipText = "This test will stop the actor system, and it will cause system crash.";
            this.poisonPillToServerToolStripMenuItem.Click += new System.EventHandler(this.poisonPillToServerToolStripMenuItem_Click);
            // 
            // tbProductNumber
            // 
            this.tbProductNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProductNumber.Location = new System.Drawing.Point(290, 66);
            this.tbProductNumber.Name = "tbProductNumber";
            this.tbProductNumber.ReadOnly = true;
            this.tbProductNumber.Size = new System.Drawing.Size(113, 21);
            this.tbProductNumber.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(184, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "Product Number:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 12);
            this.label2.TabIndex = 22;
            this.label2.Text = "PdvId:";
            // 
            // textBoxPdvId
            // 
            this.textBoxPdvId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPdvId.Location = new System.Drawing.Point(58, 32);
            this.textBoxPdvId.Name = "textBoxPdvId";
            this.textBoxPdvId.ReadOnly = true;
            this.textBoxPdvId.Size = new System.Drawing.Size(62, 21);
            this.textBoxPdvId.TabIndex = 23;
            // 
            // uploadCpXmlToolStripMenuItem
            // 
            this.uploadCpXmlToolStripMenuItem.Name = "uploadCpXmlToolStripMenuItem";
            this.uploadCpXmlToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.uploadCpXmlToolStripMenuItem.Text = "Upload CpXml..";
            this.uploadCpXmlToolStripMenuItem.ToolTipText = "Upload CpXml on current pdvId.";
            this.uploadCpXmlToolStripMenuItem.Click += new System.EventHandler(this.uploadCpXmlToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FormCptApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 577);
            this.Controls.Add(this.textBoxPdvId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbProductNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.cbTotalTest);
            this.Controls.Add(this.cbRoomTest);
            this.Controls.Add(this.cbHotTest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxGaudiFile);
            this.Controls.Add(this.send);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormCptApp";
            this.Text = "Cp tester";
            this.Load += new System.EventHandler(this.FormCptApp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button send;
        private System.Windows.Forms.TextBox textBoxGaudiFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbHotTest;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox cbRoomTest;
        private System.Windows.Forms.CheckBox cbTotalTest;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cPTSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem pingMwsServerToolStripMenuItemx;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem powerONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem powerOFFToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pingMwsServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateTestResultToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem simulateTestResultEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crashServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pingSQLServerToolStripMenuItem;
        private System.Windows.Forms.TextBox tbProductNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPdvId;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem sendStepRequestRepeatedlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem poisonPillToServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadCpXmlToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

