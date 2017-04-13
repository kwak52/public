using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;
using Dsu.Common.Interfaces;
using Dsu.Common.Resources;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    public class HudBarForm : HudBar
    {
        public Form Form { get; private set; }

        public Panel Panel { get; private set; }

        private Size _panelDefaultSize;

        public HudBarForm(IHudContainer container, Form form, bool withDrag = true, bool withMinimize = true, bool withClose = true)
            : base(container, withDrag, withMinimize, withClose)
        {
            Contract.Requires(container != null && form != null);
            Form = form;
            Resizable = form.FormBorderStyle == FormBorderStyle.Sizable;
            form.MinimumSize = new Size(Math.Min(form.MinimumSize.Width, form.ClientSize.Width), Math.Min(form.MinimumSize.Height, form.ClientSize.Height));
            Panel = new Panel() { Size = form.ClientSize };
            _panelDefaultSize = Panel.Size;

            form.EmbedToControl(Panel);
            form.Closed += (sender, args) => { container.RemoveHudBar(this); };

            if (Resizable)
            {
                var initialPanelSize = Panel.Size;
                Resize += (sender, args) =>
                {
                    if (!PanelMinimumSize.IsEmpty)
                    {
                        Panel.Size = initialPanelSize + (Size - PanelMinimumSize);
                        DefaultButtons.ForEach(b =>
                        {
                            if (Horizontal)
                                b.Height = Height - GripSize;
                            else
                                b.Width = Width - GripSize;
                        });
                    }
                };
            }

            Children = new Control[] { Panel };
        }


        protected override void OnMinimizeButtonClick(object sender, EventArgs e)
        {
            bool minimize = (bool)ButtonMinimize.Tag;
            Panel.Visible = !minimize;
            if (!minimize)
                Panel.Size = _panelDefaultSize;

            ButtonMinimize.Image = minimize ? Images.ArrowForward : Images.ArrowBackward;
            ButtonMinimize.Tag = !(bool)ButtonMinimize.Tag;
            RecalcLayoutOnVisibility(); //RecalcLayout(false);            
        }

        protected override void CreateCloseButton()
        {
            base.CreateCloseButton();
            ButtonClose.Click += (sender, args) => { Form.Close(); };
        }

        public static HudBarForm EmbedForm(IHudContainer container, Form form, bool? hudResizable = null )
        {
            if ( hudResizable.HasValue )
                if (hudResizable.Value != (form.FormBorderStyle == FormBorderStyle.Sizable))
                    form.FormBorderStyle = hudResizable.Value ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;

            return new HudBarForm(container, form);
        }
    }
}