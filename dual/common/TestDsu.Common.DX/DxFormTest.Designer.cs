namespace Test.Dsu.Common.DX
{
	partial class DxFormTest
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
			this.btnWrapControlIntoForm = new System.Windows.Forms.Button();
			this.btnAsyncActions = new System.Windows.Forms.Button();
			this.btnSleep = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.btnDelay = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnWrapControlIntoForm
			// 
			this.btnWrapControlIntoForm.Location = new System.Drawing.Point(41, 28);
			this.btnWrapControlIntoForm.Name = "btnWrapControlIntoForm";
			this.btnWrapControlIntoForm.Size = new System.Drawing.Size(75, 23);
			this.btnWrapControlIntoForm.TabIndex = 0;
			this.btnWrapControlIntoForm.Text = "Wrap";
			this.btnWrapControlIntoForm.UseVisualStyleBackColor = true;
			this.btnWrapControlIntoForm.Click += new System.EventHandler(this.btnWrapControlIntoForm_Click);
			// 
			// btnAsyncActions
			// 
			this.btnAsyncActions.Location = new System.Drawing.Point(41, 75);
			this.btnAsyncActions.Name = "btnAsyncActions";
			this.btnAsyncActions.Size = new System.Drawing.Size(75, 23);
			this.btnAsyncActions.TabIndex = 1;
			this.btnAsyncActions.Text = "Async";
			this.btnAsyncActions.UseVisualStyleBackColor = true;
			this.btnAsyncActions.Click += new System.EventHandler(this.btnAsyncActions_Click);
			// 
			// btnSleep
			// 
			this.btnSleep.Location = new System.Drawing.Point(41, 122);
			this.btnSleep.Name = "btnSleep";
			this.btnSleep.Size = new System.Drawing.Size(75, 23);
			this.btnSleep.TabIndex = 2;
			this.btnSleep.Text = "Sleep";
			this.btnSleep.UseVisualStyleBackColor = true;
			this.btnSleep.Click += new System.EventHandler(this.btnSleep_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(38, 264);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 14);
			this.label1.TabIndex = 3;
			this.label1.Text = "label1";
			// 
			// btnDelay
			// 
			this.btnDelay.Location = new System.Drawing.Point(41, 151);
			this.btnDelay.Name = "btnDelay";
			this.btnDelay.Size = new System.Drawing.Size(75, 23);
			this.btnDelay.TabIndex = 4;
			this.btnDelay.Text = "Delay";
			this.btnDelay.UseVisualStyleBackColor = true;
			this.btnDelay.Click += new System.EventHandler(this.btnDelay_Click);
			// 
			// DxFormTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(164, 299);
			this.Controls.Add(this.btnDelay);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnSleep);
			this.Controls.Add(this.btnAsyncActions);
			this.Controls.Add(this.btnWrapControlIntoForm);
			this.Name = "DxFormTest";
			this.Text = "DxFormTest";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnWrapControlIntoForm;
		private System.Windows.Forms.Button btnAsyncActions;
		private System.Windows.Forms.Button btnSleep;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnDelay;
	}
}