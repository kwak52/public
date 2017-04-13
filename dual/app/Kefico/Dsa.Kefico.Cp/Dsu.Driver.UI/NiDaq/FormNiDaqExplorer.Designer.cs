namespace Dsu.Driver.UI.NiDaq
{
    partial class FormNiDaqExplorer
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSquarewaveFilteredDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dAQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateAOWavesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceTreeCtrl1 = new Dsu.Driver.UI.NiDaq.DeviceTreeCtrl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.batchWidthAnalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dAQToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(743, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDataToolStripMenuItem,
            this.openSquarewaveFilteredDataToolStripMenuItem,
            this.toolStripSeparator1,
            this.batchWidthAnalToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openDataToolStripMenuItem
            // 
            this.openDataToolStripMenuItem.Name = "openDataToolStripMenuItem";
            this.openDataToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.openDataToolStripMenuItem.Text = "Open data...";
            this.openDataToolStripMenuItem.ToolTipText = "Open newline separated series data.\r\n\r\nShift key 누른 상태에서 열면\r\n최대 1만개까지만 읽어 들임.";
            this.openDataToolStripMenuItem.Click += new System.EventHandler(this.openDataToolStripMenuItem_Click);
            // 
            // openSquarewaveFilteredDataToolStripMenuItem
            // 
            this.openSquarewaveFilteredDataToolStripMenuItem.Name = "openSquarewaveFilteredDataToolStripMenuItem";
            this.openSquarewaveFilteredDataToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.openSquarewaveFilteredDataToolStripMenuItem.Text = "Open squarewave filtered data...";
            this.openSquarewaveFilteredDataToolStripMenuItem.Click += new System.EventHandler(this.openSquarewaveFilteredDataToolStripMenuItem_Click);
            // 
            // dAQToolStripMenuItem
            // 
            this.dAQToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateAOWavesToolStripMenuItem});
            this.dAQToolStripMenuItem.Name = "dAQToolStripMenuItem";
            this.dAQToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.dAQToolStripMenuItem.Text = "DAQ";
            // 
            // generateAOWavesToolStripMenuItem
            // 
            this.generateAOWavesToolStripMenuItem.Name = "generateAOWavesToolStripMenuItem";
            this.generateAOWavesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.generateAOWavesToolStripMenuItem.Text = "Generate AO waves...";
            this.generateAOWavesToolStripMenuItem.Click += new System.EventHandler(this.generateAOWavesToolStripMenuItem_Click);
            // 
            // deviceTreeCtrl1
            // 
            this.deviceTreeCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceTreeCtrl1.Location = new System.Drawing.Point(0, 0);
            this.deviceTreeCtrl1.Name = "deviceTreeCtrl1";
            this.deviceTreeCtrl1.Size = new System.Drawing.Size(163, 397);
            this.deviceTreeCtrl1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.deviceTreeCtrl1);
            this.splitContainer1.Size = new System.Drawing.Size(743, 397);
            this.splitContainer1.SplitterDistance = 163;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(240, 6);
            // 
            // batchWidthAnalToolStripMenuItem
            // 
            this.batchWidthAnalToolStripMenuItem.Name = "batchWidthAnalToolStripMenuItem";
            this.batchWidthAnalToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.batchWidthAnalToolStripMenuItem.Text = "Batch width anal..";
            this.batchWidthAnalToolStripMenuItem.Click += new System.EventHandler(this.batchWidthAnalToolStripMenuItem_Click);
            // 
            // FormNiDaqExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 421);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormNiDaqExplorer";
            this.Text = "NiDaq Explorer";
            this.Load += new System.EventHandler(this.FormNiDaqExplorer_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dAQToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateAOWavesToolStripMenuItem;
        private Dsu.Driver.UI.NiDaq.DeviceTreeCtrl deviceTreeCtrl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem openDataToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem openSquarewaveFilteredDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem batchWidthAnalToolStripMenuItem;
    }
}

