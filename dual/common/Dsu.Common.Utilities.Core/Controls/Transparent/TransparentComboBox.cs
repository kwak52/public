using System;
using System.Drawing;
using System.Windows.Forms;
using Dsu.Common.Utilities.CustomOpenFileDialog.OS;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Transparency 를 지원하는 combobox control class
    /// </summary>
    /// 구현상의 문제점.
    /// <br/> - ControlStyles.UserPaint 사용시, OnPaint 를 정의해야 하는데, 이때는 combobox 의 외곽만 그리고, 실제 text 는 가려서 안보인다. -> GhostLabel 이용
    /// <br/> - OnPaint 를 사용할 수 없으므로 WndProc() 에서 WM_PAINT 메시지를 처리 -> OnCustomPaint() 호출
    /// 
    /// http://stackoverflow.com/questions/9358500/winforms-making-a-control-transparent
    /// http://www.codeproject.com/Articles/6971/Making-Standard-ComboBox-appear-flat
    /// http://stackoverflow.com/questions/24475812/c-sharp-override-drawitem-of-combobox
    /// http://www.c-sharpcorner.com/UploadFile/6897bc/creating-owner-drawn-combobox/
    ///[ToolboxBitmap(typeof(System.Windows.Forms.ComboBox))]
	public class TransparentComboBox: ComboBox
    {
        private GhostLabel _ghostLabel;
        public bool Transparent
        {
            get
            {
                return _transparent;
            }
            set
            {
                if (value != _transparent)
                {
                    _transparent = value;
                    SetStyle( ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, _transparent);

                    if (_transparent)
                    {
                        DrawMode = DrawMode.OwnerDrawFixed;
                        _backColorBackup = BackColor;
                        BackColor = Color.Transparent;
                        ForeColor = Color.Black;
                        DrawItem += OnCustomDrawItem;
                        Paint += OnCustomPaint;
                        if (_ghostLabel != null)
                            _ghostLabel.Visible = true;
                    }
                    else
                    {
                        DrawMode = DrawMode.Normal;
                        BackColor = _backColorBackup;
                        DrawItem -= OnCustomDrawItem;
                        if (_ghostLabel != null)
                            _ghostLabel.Visible = false;
                    }
                }
            }
        }

        public TransparentComboBox()
            : base()
        {
            Transparent = true;
        }
    

        private void OnCustomPaint(object sender, PaintEventArgs e)
        {
            if (SelectedItem != null )
            {
                using (var brush = new SolidBrush(ForeColor))
                    e.Graphics.DrawString(SelectedItem.ToString(), Font, brush, e.ClipRectangle, StringFormat.GenericDefault);
            }
        }


        //http://stackoverflow.com/questions/1517179/c-overriding-onpaint-on-progressbar-not-working
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if ( ! Transparent || !m.Msg.IsOneOf((int)Msg.WM_PAINT, (int)Msg.WM_DRAWITEM))
                return;

            using (var graphics = Graphics.FromHwnd(Handle))
            using (var textBrush = new SolidBrush(ForeColor))
            {
                var textSize = graphics.MeasureString(Text, Font);
                float x = (Width/2) - (textSize.Width/2);
                float y = (Height / 2) - (textSize.Height / 2);
                switch (m.Msg)
                {
                    case (int)Msg.WM_PAINT:
                    {
                        if (Transparent && _ghostLabel == null)
                        {
                            _ghostLabel = new GhostLabel() {Size = this.Size, Text = this.Text};
                            _ghostLabel.Width = _ghostLabel.Width - 100;
                            _ghostLabel.Height = _ghostLabel.Height - 10;
                            _ghostLabel.Location = new Point(10, 5);

                            Controls.Add(_ghostLabel);
                        }
                        else if (!Transparent && _ghostLabel != null)
                            _ghostLabel.Visible = false;

                        //graphics.FillRectangle(textBrush, new Rectangle(0, 0, Width, Height));
                        graphics.DrawString(Text, Font, textBrush, x, y);

                        break;
                    }

                    case (int)Msg.WM_DRAWITEM:
                    {
                        graphics.DrawString(Text, Font, textBrush, x, y);
                        break;
                    }
                }
            }
        }

        private bool _transparent = false;
        private Color _backColorBackup = SystemColors.Control;


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | (int)WindowExStyles.WS_EX_TRANSPARENT;
                return cp;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (_ghostLabel != null )
                _ghostLabel.Text = Text;
        }



        private void OnCustomDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                using (var tBrush = new SolidBrush(e.ForeColor))
                //using (var brush = new SolidBrush(Color.FromArgb(100, Color.LightBlue)))
                {
                    //e.Graphics.FillRectangle(brush, e.Bounds);
                    e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, tBrush, e.Bounds, StringFormat.GenericDefault);
                }
            }
            e.DrawFocusRectangle();
        }
	}
}
