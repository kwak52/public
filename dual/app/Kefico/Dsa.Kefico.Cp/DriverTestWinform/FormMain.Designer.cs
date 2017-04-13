namespace DriverTestWinform
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
            this.btnStatistics = new System.Windows.Forms.Button();
            this.btnSwiftChart = new System.Windows.Forms.Button();
            this.btnAddPoint = new System.Windows.Forms.Button();
            this.btnRs232c = new System.Windows.Forms.Button();
            this.btnPaixPath = new System.Windows.Forms.Button();
            this.btnPollyTest = new System.Windows.Forms.Button();
            this.btnSorensen232 = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.btnAsyncTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStatistics
            // 
            this.btnStatistics.Location = new System.Drawing.Point(12, 12);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(106, 23);
            this.btnStatistics.TabIndex = 0;
            this.btnStatistics.Text = "Statistics";
            this.btnStatistics.UseVisualStyleBackColor = true;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnSwiftChart
            // 
            this.btnSwiftChart.Location = new System.Drawing.Point(12, 41);
            this.btnSwiftChart.Name = "btnSwiftChart";
            this.btnSwiftChart.Size = new System.Drawing.Size(106, 23);
            this.btnSwiftChart.TabIndex = 1;
            this.btnSwiftChart.Text = "Swift chart";
            this.btnSwiftChart.UseVisualStyleBackColor = true;
            this.btnSwiftChart.Click += new System.EventHandler(this.btnSwiftChart_Click);
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.Location = new System.Drawing.Point(43, 70);
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Size = new System.Drawing.Size(75, 23);
            this.btnAddPoint.TabIndex = 2;
            this.btnAddPoint.Text = "Add point";
            this.btnAddPoint.UseVisualStyleBackColor = true;
            this.btnAddPoint.Click += new System.EventHandler(this.btnAddPoint_Click);
            // 
            // btnRs232c
            // 
            this.btnRs232c.Location = new System.Drawing.Point(12, 99);
            this.btnRs232c.Name = "btnRs232c";
            this.btnRs232c.Size = new System.Drawing.Size(106, 23);
            this.btnRs232c.TabIndex = 3;
            this.btnRs232c.Text = "RS 232c";
            this.btnRs232c.UseVisualStyleBackColor = true;
            this.btnRs232c.Click += new System.EventHandler(this.btnRs232c_Click);
            // 
            // btnPaixPath
            // 
            this.btnPaixPath.Location = new System.Drawing.Point(12, 173);
            this.btnPaixPath.Name = "btnPaixPath";
            this.btnPaixPath.Size = new System.Drawing.Size(106, 23);
            this.btnPaixPath.TabIndex = 4;
            this.btnPaixPath.Text = "Paix Path";
            this.btnPaixPath.UseVisualStyleBackColor = true;
            this.btnPaixPath.Click += new System.EventHandler(this.btnPaixPath_Click);
            // 
            // btnPollyTest
            // 
            this.btnPollyTest.Location = new System.Drawing.Point(12, 202);
            this.btnPollyTest.Name = "btnPollyTest";
            this.btnPollyTest.Size = new System.Drawing.Size(106, 23);
            this.btnPollyTest.TabIndex = 5;
            this.btnPollyTest.Text = "Polly test";
            this.btnPollyTest.UseVisualStyleBackColor = true;
            this.btnPollyTest.Click += new System.EventHandler(this.btnPollyTest_Click);
            // 
            // btnSorensen232
            // 
            this.btnSorensen232.Location = new System.Drawing.Point(12, 128);
            this.btnSorensen232.Name = "btnSorensen232";
            this.btnSorensen232.Size = new System.Drawing.Size(106, 23);
            this.btnSorensen232.TabIndex = 6;
            this.btnSorensen232.Text = "Sorensen 232c";
            this.btnSorensen232.UseVisualStyleBackColor = true;
            this.btnSorensen232.Click += new System.EventHandler(this.btnSorensen232_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(152, 292);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(38, 12);
            this.labelMessage.TabIndex = 7;
            this.labelMessage.Text = "label1";
            // 
            // btnAsyncTest
            // 
            this.btnAsyncTest.Location = new System.Drawing.Point(115, 257);
            this.btnAsyncTest.Name = "btnAsyncTest";
            this.btnAsyncTest.Size = new System.Drawing.Size(106, 23);
            this.btnAsyncTest.TabIndex = 8;
            this.btnAsyncTest.Text = "Async test";
            this.btnAsyncTest.UseVisualStyleBackColor = true;
            this.btnAsyncTest.Click += new System.EventHandler(this.btnAsyncTest_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 326);
            this.Controls.Add(this.btnAsyncTest);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.btnSorensen232);
            this.Controls.Add(this.btnPollyTest);
            this.Controls.Add(this.btnPaixPath);
            this.Controls.Add(this.btnRs232c);
            this.Controls.Add(this.btnAddPoint);
            this.Controls.Add(this.btnSwiftChart);
            this.Controls.Add(this.btnStatistics);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Button btnSwiftChart;
        private System.Windows.Forms.Button btnAddPoint;
        private System.Windows.Forms.Button btnRs232c;
        private System.Windows.Forms.Button btnPaixPath;
        private System.Windows.Forms.Button btnPollyTest;
        private System.Windows.Forms.Button btnSorensen232;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Button btnAsyncTest;
    }
}

