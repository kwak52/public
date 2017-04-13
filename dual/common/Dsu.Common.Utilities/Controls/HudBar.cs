using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dsu.Common.Interfaces;
using Dsu.Common.Resources;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// HUD(Head Up Display) bar(View 에 embedding 되는 Control 들)을 관리하기 위한 클래스
    /// </summary>
    public class HudBar : ResizablePanel, IHudBar
    {
        public IHudContainer HudContainer { get; private set; }
        public Control Control { get { return this; } }
        public bool Movable
        {
            get { return _movable; } 
            set
            {
                _movable = value;
                if ( ! Children.Contains(_btnDrag) )
                    Controls.Add(_btnDrag);
            }
        }

        private bool _movable = true;


        public string Title { get; set; }
        public AnchorStyles AnchorStyles { get { return Control.Anchor; } set { Control.Anchor = value; } }

        protected Control[] DefaultButtons { get { return new Control[] { _btnDrag, _btnClose, _btnMinimize }; } }

        public bool Horizontal
        {
            get { return _horizontal; }
            set
            {
                _horizontal = value;
                Children.ForEach(c => c.Location = _horizontal ? new Point(c.Location.X, 0) : new Point(0, c.Location.Y));
                DefaultButtons.ForEach(b =>
                {
                    b.Width = DefaultWidth;
                    b.Height = DefaultHeight;
                });

                RecalcLayout();
            }
        }

        private bool _horizontal = true;

        public bool Closable { get { return _closable; } set { _closable = value; } } 

        private bool _closable = true;
        public bool Minimizable { get { return _minimizable; } set { _minimizable = value; } } 
        private bool _minimizable = true;

        private Control View { get { return HudContainer.HudOwner; } }


        public Control[] Children
        {
            get { return base.Controls.Cast<Control>().ToArray(); }
            set
            {
                while(Controls.Count > 0)
                    Controls.RemoveAt(0);

                if (Movable)
                    Controls.Add(_btnDrag);
                if ( Closable )
                    Controls.Add(_btnClose);
                if ( Minimizable )
                    Controls.Add(_btnMinimize);

                Controls.AddRange(value);
                
                RecalcLayout();
            }
        }

        public event EventHandler ClosingHook;

        public void AddControl(Control control) { Controls.Add(control); }
        public bool RemoveControl(Control control) { Controls.Remove(control); return true; }

        public void RecalcLayoutOnVisibility()
        {
            var widths = Children.Where(c => c.Visible).Select(c => c.Width);
            var heights = Children.Where(c => c.Visible).Select(c => c.Height);
            if (Horizontal)
            {
                Width = widths.Sum();
                Height = heights.Max() + GripSize;
            }
            else
            {
                Width = widths.Max();
                Height = heights.Sum() + GripSize;
            }            
        }

        public void RecalcLayout() { RecalcLayout(true); } 
        public void RecalcLayout(bool allowChangeLocation)
        {
            RecalcLayoutOnVisibility();

            if ( allowChangeLocation )
            {
                var anchor = AnchorStyles;

                if (anchor.HasFlag(AnchorStyles.Top))
                    Location = new Point(Location.X, 0);
                if (anchor.HasFlag(AnchorStyles.Bottom))
                    Location = new Point(Location.X, View.Height - Height);

                if (anchor.HasFlag(AnchorStyles.Left))
                    Location = new Point(0, Location.Y);
                if (anchor.HasFlag(AnchorStyles.Right))
                    Location = new Point(View.Width - Width, Location.Y);
            }

            var widths = Children.Where(c => c.Visible).Select(c => c.Width);
            var heights = Children.Where(c => c.Visible).Select(c => c.Height);
            int widthMax = widths.Max();
            int hieghtMax = heights.Max();
            int w = 0;
            int h = 0;
            Children.ForEach(c =>
            {
                if (Horizontal)
                {
                    c.Location = new Point(w, c.Location.Y);
                    c.Height = hieghtMax;
                    w += c.Width;
                }
                else
                {
                    c.Location = new Point(c.Location.X, h);
                    c.Width = widthMax;
                    h += c.Height;
                }
            });


            if ( allowChangeLocation )
            {
                var competitors = HudContainer.HudBars.Where(hb => hb != this && hb.AnchorStyles.HasFlag(AnchorStyles));
                if (competitors.Any())
                {
                    if (AnchorStyles.HasFlag(AnchorStyles.Bottom))
                        Control.Location = new Point(Control.Location.X, Control.Location.Y - competitors.Select(hb => hb.Control.Height).Sum());
                }
            }

            if (PanelMinimumSize.IsEmpty && !Size.IsEmpty)
                PanelMinimumSize = Size;
        }

        private const int DefaultHeight = 20;
        private const int DefaultWidth = 20;
        private Button _btnDrag;
        private Button _btnClose;
        private Button _btnMinimize;
        protected Button ButtonClose { get {  return _btnClose;} }
        protected Button ButtonMinimize { get { return _btnMinimize; } }
        private void CreateDragButton()
        {
            _btnDrag = new Button() { Size = new Size(15, DefaultHeight), BackColor = Color.BlueViolet };
            new ToolTip().SetToolTip(_btnDrag, "Drag this button to move.");
            _btnDrag.MouseMove += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                {
                    Point pt = _btnDrag.PointToScreen(new Point(args.X, args.Y));
                    Location = View.PointToClient(pt);
                }
            };
            _btnDrag.MouseDown += (sender, args) =>
            {
                if (args.Button == MouseButtons.Right)
                {
                    var menu = new ContextMenuStrip();
                    menu.Items.Add(new ToolStripMenuItem("Close", Images.Close, (o, a) =>
                    {
                        Close();
                    }) { Visible = Horizontal });
                    menu.Items.Add(new ToolStripMenuItem("Vertical", null, (o, a) =>
                    {
                        Horizontal = false;
                    }) { Visible = Horizontal });
                    menu.Items.Add(new ToolStripMenuItem("Horizontal", null, (o, a) =>
                    {
                        Horizontal = true;
                    }) { Visible = ! Horizontal });

                    menu.Show(this, PointToClient(MousePosition));                    
                }
            };

        }

        private void Close()
        {
            View.Controls.Remove(this);
            HudContainer.RemoveHudBar(this);
            ClosingHook.Handle(this, new EventArgs());
        }

        protected virtual void CreateCloseButton()
        {
            _btnClose = new Button() { Image = Images.Close, Size = new Size(DefaultWidth, DefaultHeight)};
            _btnClose.Click += (sender, args) => { Close(); }; 
            _btnClose.TextImageRelation = TextImageRelation.ImageAboveText;
            new ToolTip().SetToolTip(_btnClose, "Close this toolbar");
        }

        private void CreateMinimizeButton()
        {
            _btnMinimize = new Button() { Tag = true, Image = Images.ArrowBackward, Size = new Size(DefaultWidth, DefaultHeight) };
            _btnMinimize.Click += OnMinimizeButtonClick;
            _btnMinimize.TextImageRelation = TextImageRelation.ImageAboveText;
            new ToolTip().SetToolTip(_btnMinimize, "Min/Maximize this toolbar");
        }

        protected virtual void OnMinimizeButtonClick(object sender, EventArgs e)
        {
            bool minimize = (bool)_btnMinimize.Tag;
            Children.Except(DefaultButtons).ForEach(c => { c.Visible = !minimize; });
            _btnMinimize.Image = minimize ? Images.ArrowForward : Images.ArrowBackward;
            _btnMinimize.Tag = !(bool)_btnMinimize.Tag;
            RecalcLayoutOnVisibility(); //RecalcLayout(false);            
        }




        public HudBar(IHudContainer container, bool resizable = false, bool withDrag = true, bool withMinimize = true, bool withClose = true)
        {
            Resizable = resizable;

            HudContainer = container;
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            container.AddHudBar(this);

            if (withDrag)
                CreateDragButton();

            if (withMinimize)
                CreateMinimizeButton();

            if (withClose)
                CreateCloseButton();
        }
    }
}
