namespace TestPLC
{
    partial class Form1
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ucGrid_Set = new Dsu.UI.Grid.ucGrid();
            this.ucGrid_Get = new Dsu.UI.Grid.ucGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ucGrid_Set);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ucGrid_Get);
            this.splitContainer1.Size = new System.Drawing.Size(881, 530);
            this.splitContainer1.SplitterDistance = 418;
            this.splitContainer1.TabIndex = 0;
            // 
            // ucGrid_Set
            // 
            this.ucGrid_Set.ColumnFont = new System.Drawing.Font("Tahoma", 9F);
            this.ucGrid_Set.DataSource = null;
            this.ucGrid_Set.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGrid_Set.Editable = true;
            this.ucGrid_Set.Location = new System.Drawing.Point(0, 0);
            this.ucGrid_Set.MultiSelect = true;
            this.ucGrid_Set.Name = "ucGrid_Set";
            this.ucGrid_Set.RowFont = new System.Drawing.Font("Tahoma", 9F);
            this.ucGrid_Set.ShowAutoFilterRow = true;
            this.ucGrid_Set.ShowGroupPanel = false;
            this.ucGrid_Set.Size = new System.Drawing.Size(418, 530);
            this.ucGrid_Set.TabIndex = 0;
            this.ucGrid_Set.UEventMouseDoubleClick += new Dsu.UI.Grid.EventHandler.UEventHandlerMouseDoubleClick(this.ucGrid_Set_UEventMouseDoubleClick);
            // 
            // ucGrid_Get
            // 
            this.ucGrid_Get.ColumnFont = new System.Drawing.Font("Tahoma", 9F);
            this.ucGrid_Get.DataSource = null;
            this.ucGrid_Get.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGrid_Get.Editable = false;
            this.ucGrid_Get.Location = new System.Drawing.Point(0, 0);
            this.ucGrid_Get.MultiSelect = true;
            this.ucGrid_Get.Name = "ucGrid_Get";
            this.ucGrid_Get.RowFont = new System.Drawing.Font("Tahoma", 9F);
            this.ucGrid_Get.ShowAutoFilterRow = true;
            this.ucGrid_Get.ShowGroupPanel = false;
            this.ucGrid_Get.Size = new System.Drawing.Size(459, 530);
            this.ucGrid_Get.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 530);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Dsu.UI.Grid.ucGrid ucGrid_Set;
        private Dsu.UI.Grid.ucGrid ucGrid_Get;
    }
}

