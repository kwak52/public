namespace Dsu.Driver.UI.Paix
{
    partial class FormRelativeMove
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelX = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelZ = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelTilt = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnRear = new Dsu.Common.Utilities.RepeatButton();
            this.btnFront = new Dsu.Common.Utilities.RepeatButton();
            this.btnRight = new Dsu.Common.Utilities.RepeatButton();
            this.btnLeft = new Dsu.Common.Utilities.RepeatButton();
            this.btnUp = new Dsu.Common.Utilities.RepeatButton();
            this.btnDown = new Dsu.Common.Utilities.RepeatButton();
            this.btnCCW = new Dsu.Common.Utilities.RepeatButton();
            this.btnCW = new Dsu.Common.Utilities.RepeatButton();
            this.btnStop = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 251);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(362, 22);
            this.statusStrip1.TabIndex = 11;
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
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(1, 17);
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
            // btnRear
            // 
            this.btnRear.EnableRepeat = true;
            this.btnRear.Location = new System.Drawing.Point(105, 12);
            this.btnRear.Name = "btnRear";
            this.btnRear.Size = new System.Drawing.Size(137, 36);
            this.btnRear.TabIndex = 12;
            this.btnRear.Text = "Rear";
            this.btnRear.UseVisualStyleBackColor = true;
            this.btnRear.Click += new System.EventHandler(this.btnRear_Click);
            // 
            // btnFront
            // 
            this.btnFront.EnableRepeat = true;
            this.btnFront.Location = new System.Drawing.Point(105, 98);
            this.btnFront.Name = "btnFront";
            this.btnFront.Size = new System.Drawing.Size(137, 36);
            this.btnFront.TabIndex = 13;
            this.btnFront.Text = "Front";
            this.btnFront.UseVisualStyleBackColor = true;
            this.btnFront.Click += new System.EventHandler(this.btnFront_Click);
            // 
            // btnRight
            // 
            this.btnRight.EnableRepeat = true;
            this.btnRight.Location = new System.Drawing.Point(248, 47);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(75, 55);
            this.btnRight.TabIndex = 14;
            this.btnRight.Text = "Right";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.EnableRepeat = true;
            this.btnLeft.Location = new System.Drawing.Point(24, 47);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(75, 55);
            this.btnLeft.TabIndex = 15;
            this.btnLeft.Text = "Left";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnUp
            // 
            this.btnUp.EnableRepeat = true;
            this.btnUp.Location = new System.Drawing.Point(105, 56);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(66, 36);
            this.btnUp.TabIndex = 16;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.EnableRepeat = true;
            this.btnDown.Location = new System.Drawing.Point(176, 55);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(66, 36);
            this.btnDown.TabIndex = 17;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnCCW
            // 
            this.btnCCW.EnableRepeat = true;
            this.btnCCW.Location = new System.Drawing.Point(176, 169);
            this.btnCCW.Name = "btnCCW";
            this.btnCCW.Size = new System.Drawing.Size(66, 36);
            this.btnCCW.TabIndex = 19;
            this.btnCCW.Text = "CCW";
            this.btnCCW.UseVisualStyleBackColor = true;
            this.btnCCW.Click += new System.EventHandler(this.btnCCW_Click);
            // 
            // btnCW
            // 
            this.btnCW.EnableRepeat = true;
            this.btnCW.Location = new System.Drawing.Point(105, 170);
            this.btnCW.Name = "btnCW";
            this.btnCW.Size = new System.Drawing.Size(66, 36);
            this.btnCW.TabIndex = 18;
            this.btnCW.Text = "CW";
            this.btnCW.UseVisualStyleBackColor = true;
            this.btnCW.Click += new System.EventHandler(this.btnCW_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(248, 119);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 118);
            this.btnStop.TabIndex = 20;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // FormRelativeMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 273);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnCCW);
            this.Controls.Add(this.btnCW);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnFront);
            this.Controls.Add(this.btnRear);
            this.Controls.Add(this.statusStrip1);
            this.MinimumSize = new System.Drawing.Size(370, 300);
            this.Name = "FormRelativeMove";
            this.Text = "FormRelativeMove";
            this.Load += new System.EventHandler(this.FormRelativeMove_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelX;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelY;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelZ;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelTilt;
        private Common.Utilities.RepeatButton btnRear;
        private Common.Utilities.RepeatButton btnFront;
        private Common.Utilities.RepeatButton btnRight;
        private Common.Utilities.RepeatButton btnLeft;
        private Common.Utilities.RepeatButton btnUp;
        private Common.Utilities.RepeatButton btnDown;
        private Common.Utilities.RepeatButton btnCCW;
        private Common.Utilities.RepeatButton btnCW;
        private System.Windows.Forms.Button btnStop;
    }
}