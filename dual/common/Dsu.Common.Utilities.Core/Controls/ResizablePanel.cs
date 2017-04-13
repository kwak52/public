using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    /// <summary> Resizable panel class </summary>
    /// https://social.msdn.microsoft.com/Forums/en-US/a184f136-0e54-4b56-86be-9a1c57212344/resizing-panel-control?forum=Vsexpressvcs
    public partial class ResizablePanel : Panel
    {
        private const int defaultGripSize = 10;
        private int? _gripSize = defaultGripSize;
        private bool _dragging;
        private Point _dragPos;

        public bool Resizable
        {
            get { return _gripSize.HasValue; }
            set
            {
                if (value)
                    _gripSize = defaultGripSize;
                else
                    _gripSize = null;
            }
        }

        public int GripSize { get { return _gripSize.HasValue ? _gripSize.Value : 0; } }

        public ResizablePanel(bool resizable=true)
        {
            Resizable = resizable;
            base.DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if ( Resizable )
            {
                ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor,
                    new Rectangle(this.ClientSize.Width - GripSize, this.ClientSize.Height - GripSize, GripSize,
                        GripSize));
            }

            base.OnPaint(e);
        }

        private bool IsOnGrip(Point pos)
        {
            return pos.X >= this.ClientSize.Width - _gripSize &&
                   pos.Y >= this.ClientSize.Height - _gripSize;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _dragging = IsOnGrip(e.Location);
            _dragPos = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _dragging = false;
            base.OnMouseUp(e);
        }


        public Size PanelMinimumSize { get; set; }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ( Resizable )
            {
                if (_dragging)
                {
                    var w = this.Width + e.X - _dragPos.X;
                    var h = this.Height + e.Y - _dragPos.Y;
                    if (! PanelMinimumSize.IsEmpty)
                    {
                        w = Math.Max(w, PanelMinimumSize.Width);
                        h = Math.Max(h, PanelMinimumSize.Height);
                    }
                    this.Size = new Size(w, h);
                    _dragPos = e.Location;
                }
                else if (IsOnGrip(e.Location))
                    this.Cursor = Cursors.SizeNWSE;
                else
                    this.Cursor = Cursors.Default;
            }

            base.OnMouseMove(e);
        }
    }
}
