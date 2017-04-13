namespace ChartGantt_Form
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
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.toggleSwitch1 = new DevExpress.XtraEditors.ToggleSwitch();
            this.ucChartTree1 = new Dsu.UI.XbarGantt.ucChartTree();
            this.ucChartGantt1 = new Dsu.UI.XbarGantt.ucChartGantt();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitch1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.toggleSwitch1);
            this.splitContainerControl1.Panel1.Controls.Add(this.ucChartTree1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.ucChartGantt1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(693, 634);
            this.splitContainerControl1.SplitterPosition = 173;
            this.splitContainerControl1.TabIndex = 2;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // toggleSwitch1
            // 
            this.toggleSwitch1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.toggleSwitch1.Location = new System.Drawing.Point(13, 581);
            this.toggleSwitch1.Name = "toggleSwitch1";
            this.toggleSwitch1.Properties.OffText = "Off";
            this.toggleSwitch1.Properties.OnText = "On";
            this.toggleSwitch1.Size = new System.Drawing.Size(95, 24);
            this.toggleSwitch1.TabIndex = 2;
            // 
            // ucChartTree1
            // 
            this.ucChartTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucChartTree1.Location = new System.Drawing.Point(0, 0);
            this.ucChartTree1.Name = "ucChartTree1";
            this.ucChartTree1.Size = new System.Drawing.Size(173, 634);
            this.ucChartTree1.TabIndex = 1;
            // 
            // ucChartGantt1
            // 
            this.ucChartGantt1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucChartGantt1.Location = new System.Drawing.Point(0, 0);
            this.ucChartGantt1.Name = "ucChartGantt1";
            this.ucChartGantt1.Size = new System.Drawing.Size(515, 634);
            this.ucChartGantt1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 634);
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitch1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Dsu.UI.XbarGantt.ucChartGantt ucChartGantt1;
        private Dsu.UI.XbarGantt.ucChartTree ucChartTree1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.ToggleSwitch toggleSwitch1;
    }
}

