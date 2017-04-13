// Author : kobi.bo (kobi@zebrot.com)
// License : CPOL

namespace Dsu.Common.Utilities
{
  partial class EnumFlagsSelector<T> {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.winCheckList = new System.Windows.Forms.CheckedListBox();
      this.SuspendLayout();
      // 
      // winCheckList
      // 
      this.winCheckList.CheckOnClick = true;
      this.winCheckList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.winCheckList.FormattingEnabled = true;
      this.winCheckList.Location = new System.Drawing.Point(0,0);
      this.winCheckList.Name = "winCheckList";
      this.winCheckList.Size = new System.Drawing.Size(128,109);
      this.winCheckList.TabIndex = 1;
      this.winCheckList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.winCheckList_ItemCheck);
      this.winCheckList.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.winCheckList_Format);
      // 
      // EnumFlagsSelector
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.winCheckList);
      this.Name = "EnumFlagsSelector";
      this.Size = new System.Drawing.Size(128,111);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.CheckedListBox winCheckList;

  }
}
