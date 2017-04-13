namespace Dsu.Driver.UI.Paix
{
    partial class FormPathPlanner
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importPAIXNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGroupsIntoFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showPositionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridControlPosition = new DevExpress.XtraGrid.GridControl();
            this.gridViewPosition = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemButtonEditGo = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemButtonEditPreview = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.actionList1 = new Dsu.Common.Utilities.Actions.ActionList(this.components);
            this.action1 = new Dsu.Common.Utilities.Actions.Action(this.components);
            this.btnStop = new System.Windows.Forms.Button();
            this.gridControlAxes = new DevExpress.XtraGrid.GridControl();
            this.gridViewAxes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelX = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelZ = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelTilt = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.labelFilePath = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel11 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCmdX = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCmdY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCmdZ = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCmdTilt = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditGo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAxes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAxes)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(716, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.openToolStripMenuItem,
            this.newToolStripMenuItem,
            this.toolStripSeparator1,
            this.importPAIXNodeToolStripMenuItem,
            this.saveGroupsIntoFilesToolStripMenuItem,
            this.showPositionsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(201, 6);
            // 
            // importPAIXNodeToolStripMenuItem
            // 
            this.importPAIXNodeToolStripMenuItem.Name = "importPAIXNodeToolStripMenuItem";
            this.importPAIXNodeToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.importPAIXNodeToolStripMenuItem.Text = "Import PAIX node...";
            this.importPAIXNodeToolStripMenuItem.Click += new System.EventHandler(this.importPAIXNodeToolStripMenuItem_Click);
            // 
            // saveGroupsIntoFilesToolStripMenuItem
            // 
            this.saveGroupsIntoFilesToolStripMenuItem.Name = "saveGroupsIntoFilesToolStripMenuItem";
            this.saveGroupsIntoFilesToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.saveGroupsIntoFilesToolStripMenuItem.Text = "Save groups into files...";
            this.saveGroupsIntoFilesToolStripMenuItem.Click += new System.EventHandler(this.saveGroupsIntoFilesToolStripMenuItem_Click);
            // 
            // showPositionsToolStripMenuItem
            // 
            this.showPositionsToolStripMenuItem.Name = "showPositionsToolStripMenuItem";
            this.showPositionsToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.showPositionsToolStripMenuItem.Text = "Show positions...";
            this.showPositionsToolStripMenuItem.Click += new System.EventHandler(this.showPositionsToolStripMenuItem_Click);
            // 
            // gridControlPosition
            // 
            this.gridControlPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlPosition.Location = new System.Drawing.Point(6, 169);
            this.gridControlPosition.MainView = this.gridViewPosition;
            this.gridControlPosition.Name = "gridControlPosition";
            this.gridControlPosition.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEditGo,
            this.repositoryItemCheckEdit1,
            this.repositoryItemTextEdit1,
            this.repositoryItemButtonEditPreview});
            this.gridControlPosition.Size = new System.Drawing.Size(703, 192);
            this.gridControlPosition.TabIndex = 2;
            this.gridControlPosition.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewPosition});
            // 
            // gridViewPosition
            // 
            this.gridViewPosition.GridControl = this.gridControlPosition;
            this.gridViewPosition.Name = "gridViewPosition";
            this.gridViewPosition.OptionsSelection.MultiSelect = true;
            this.gridViewPosition.OptionsView.ShowGroupPanel = false;
            // 
            // repositoryItemButtonEditGo
            // 
            this.repositoryItemButtonEditGo.AllowFocused = false;
            this.repositoryItemButtonEditGo.AutoHeight = false;
            this.repositoryItemButtonEditGo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEditGo.Name = "repositoryItemButtonEditGo";
            this.repositoryItemButtonEditGo.ReadOnly = true;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemButtonEditPreview
            // 
            this.repositoryItemButtonEditPreview.AutoHeight = false;
            this.repositoryItemButtonEditPreview.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEditPreview.Name = "repositoryItemButtonEditPreview";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(87, 402);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(71, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add";
            this.toolTip1.SetToolTip(this.btnAdd, "Add new row.");
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(87, 426);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(71, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.toolTip1.SetToolTip(this.btnDelete, "Delete selected row(s).");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            this.action1.Text = "11426040";
            this.action1.Visible = true;
            this.action1.Update += new System.EventHandler(this.action1_Update);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(458, 369);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(251, 72);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Stop";
            this.toolTip1.SetToolTip(this.btnStop, "Stop the robot.");
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // gridControlAxes
            // 
            this.gridControlAxes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlAxes.Location = new System.Drawing.Point(6, 40);
            this.gridControlAxes.MainView = this.gridViewAxes;
            this.gridControlAxes.Name = "gridControlAxes";
            this.gridControlAxes.Size = new System.Drawing.Size(703, 123);
            this.gridControlAxes.TabIndex = 7;
            this.gridControlAxes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAxes});
            // 
            // gridViewAxes
            // 
            this.gridViewAxes.GridControl = this.gridControlAxes;
            this.gridViewAxes.Name = "gridViewAxes";
            this.gridViewAxes.OptionsCustomization.AllowSort = false;
            this.gridViewAxes.OptionsView.ShowGroupPanel = false;
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Location = new System.Drawing.Point(379, 386);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(55, 38);
            this.btnPlay.TabIndex = 8;
            this.btnPlay.Text = "Play";
            this.toolTip1.SetToolTip(this.btnPlay, "Move robots with checked rows pose.");
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckAll.Location = new System.Drawing.Point(164, 426);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(65, 23);
            this.btnCheckAll.TabIndex = 9;
            this.btnCheckAll.Text = "CheckAll";
            this.toolTip1.SetToolTip(this.btnCheckAll, "Check all the rows.");
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabelX,
            this.toolStripStatusLabelY,
            this.toolStripStatusLabelZ,
            this.toolStripStatusLabelTilt});
            this.statusStrip1.Location = new System.Drawing.Point(0, 472);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(716, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(351, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.AutoSize = false;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabel4.Text = "Encoder";
            // 
            // toolStripStatusLabelX
            // 
            this.toolStripStatusLabelX.AutoSize = false;
            this.toolStripStatusLabelX.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelX.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripStatusLabelX.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelX.Name = "toolStripStatusLabelX";
            this.toolStripStatusLabelX.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabelX.Text = "x";
            // 
            // toolStripStatusLabelY
            // 
            this.toolStripStatusLabelY.AutoSize = false;
            this.toolStripStatusLabelY.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelY.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripStatusLabelY.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelY.Name = "toolStripStatusLabelY";
            this.toolStripStatusLabelY.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabelY.Text = "y";
            // 
            // toolStripStatusLabelZ
            // 
            this.toolStripStatusLabelZ.AutoSize = false;
            this.toolStripStatusLabelZ.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelZ.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripStatusLabelZ.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelZ.Name = "toolStripStatusLabelZ";
            this.toolStripStatusLabelZ.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabelZ.Text = "z";
            // 
            // toolStripStatusLabelTilt
            // 
            this.toolStripStatusLabelTilt.AutoSize = false;
            this.toolStripStatusLabelTilt.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelTilt.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripStatusLabelTilt.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelTilt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.toolStripStatusLabelTilt.Name = "toolStripStatusLabelTilt";
            this.toolStripStatusLabelTilt.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabelTilt.Text = "tilt";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // labelFilePath
            // 
            this.labelFilePath.AutoSize = true;
            this.labelFilePath.Location = new System.Drawing.Point(8, 25);
            this.labelFilePath.Name = "labelFilePath";
            this.labelFilePath.Size = new System.Drawing.Size(38, 12);
            this.labelFilePath.TabIndex = 12;
            this.labelFilePath.Text = "label1";
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDown.Location = new System.Drawing.Point(10, 426);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(71, 23);
            this.btnDown.TabIndex = 14;
            this.btnDown.Text = "Down";
            this.toolTip1.SetToolTip(this.btnDown, "Move down selected row(s).");
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUp.Location = new System.Drawing.Point(10, 402);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(71, 23);
            this.btnUp.TabIndex = 13;
            this.btnUp.Text = "Up";
            this.toolTip1.SetToolTip(this.btnUp, "Move up selected row(s).");
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // statusStrip2
            // 
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel11,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabelCmdX,
            this.toolStripStatusLabelCmdY,
            this.toolStripStatusLabelCmdZ,
            this.toolStripStatusLabelCmdTilt});
            this.statusStrip2.Location = new System.Drawing.Point(0, 450);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip2.Size = new System.Drawing.Size(716, 22);
            this.statusStrip2.TabIndex = 15;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // toolStripStatusLabel11
            // 
            this.toolStripStatusLabel11.Name = "toolStripStatusLabel11";
            this.toolStripStatusLabel11.Size = new System.Drawing.Size(349, 17);
            this.toolStripStatusLabel11.Spring = true;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.AutoSize = false;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabel3.Text = "Command";
            // 
            // toolStripStatusLabelCmdX
            // 
            this.toolStripStatusLabelCmdX.AutoSize = false;
            this.toolStripStatusLabelCmdX.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelCmdX.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripStatusLabelCmdX.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelCmdX.Name = "toolStripStatusLabelCmdX";
            this.toolStripStatusLabelCmdX.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabelCmdX.Text = "x";
            // 
            // toolStripStatusLabelCmdY
            // 
            this.toolStripStatusLabelCmdY.AutoSize = false;
            this.toolStripStatusLabelCmdY.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelCmdY.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripStatusLabelCmdY.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelCmdY.Name = "toolStripStatusLabelCmdY";
            this.toolStripStatusLabelCmdY.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabelCmdY.Text = "y";
            // 
            // toolStripStatusLabelCmdZ
            // 
            this.toolStripStatusLabelCmdZ.AutoSize = false;
            this.toolStripStatusLabelCmdZ.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelCmdZ.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripStatusLabelCmdZ.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelCmdZ.Name = "toolStripStatusLabelCmdZ";
            this.toolStripStatusLabelCmdZ.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabelCmdZ.Text = "z";
            // 
            // toolStripStatusLabelCmdTilt
            // 
            this.toolStripStatusLabelCmdTilt.AutoSize = false;
            this.toolStripStatusLabelCmdTilt.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelCmdTilt.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripStatusLabelCmdTilt.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelCmdTilt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.toolStripStatusLabelCmdTilt.Name = "toolStripStatusLabelCmdTilt";
            this.toolStripStatusLabelCmdTilt.Size = new System.Drawing.Size(70, 17);
            this.toolStripStatusLabelCmdTilt.Text = "tilt command";
            // 
            // FormPathPlanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 494);
            this.Controls.Add(this.statusStrip2);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.labelFilePath);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.gridControlPosition);
            this.Controls.Add(this.gridControlAxes);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(524, 464);
            this.Name = "FormPathPlanner";
            this.Text = "FormPathPlanner";
            this.Load += new System.EventHandler(this.FormPathPlanner_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditGo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAxes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAxes)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private DevExpress.XtraGrid.GridControl gridControlPosition;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewPosition;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEditGo;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private Common.Utilities.Actions.ActionList actionList1;
        private System.Windows.Forms.Button btnStop;
        private DevExpress.XtraGrid.GridControl gridControlAxes;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewAxes;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnCheckAll;
        private Common.Utilities.Actions.Action action1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem importPAIXNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGroupsIntoFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showPositionsToolStripMenuItem;
        private System.Windows.Forms.Label labelFilePath;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEditPreview;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelX;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelY;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelZ;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelTilt;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel11;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCmdX;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCmdY;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCmdZ;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCmdTilt;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}