﻿// http://www.codeproject.com/Articles/24385/Have-a-Great-DesignTime-Experience-with-a-Powerful

using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design;

namespace Dsu.Common.Utilities.DesignSurfaceExt
{

    internal class DesignerOptionServiceExt4SnapLines : DesignerOptionService
    {
        public DesignerOptionServiceExt4SnapLines() : base() { }

        protected override void PopulateOptionCollection(DesignerOptionCollection options)
        {
            if (null != options.Parent) return;

            DesignerOptions ops = new DesignerOptions();
            ops.UseSnapLines = true;
            ops.UseSmartTags = true;
            DesignerOptionCollection wfd = this.CreateOptionCollection(options, "WindowsFormsDesigner", null);
            this.CreateOptionCollection(wfd, "General", ops);
        }
    }//end_class

    internal class DesignerOptionServiceExt4Grid : DesignerOptionService
    {
        private Size _gridSize;

        public DesignerOptionServiceExt4Grid(Size gridSize) : base() { _gridSize = gridSize; }

        protected override void PopulateOptionCollection(DesignerOptionCollection options)
        {
            if (null != options.Parent) return;

            DesignerOptions ops = new DesignerOptions();
            ops.GridSize = _gridSize;
            ops.SnapToGrid = true;
            ops.ShowGrid = true;
            ops.UseSnapLines = false;
            ops.UseSmartTags = true;
            DesignerOptionCollection wfd = this.CreateOptionCollection(options, "WindowsFormsDesigner", null);
            this.CreateOptionCollection(wfd, "General", ops);
        }
    }//end_class

    internal class DesignerOptionServiceExt4GridWithoutSnapping : DesignerOptionService
    {
        private Size _gridSize;

        public DesignerOptionServiceExt4GridWithoutSnapping(Size gridSize) : base() { _gridSize = gridSize; }

        protected override void PopulateOptionCollection(DesignerOptionCollection options)
        {
            if (null != options.Parent) return;

            DesignerOptions ops = new DesignerOptions();
            ops.GridSize = _gridSize;
            ops.SnapToGrid = false;
            ops.ShowGrid = true;
            ops.UseSnapLines = false;
            ops.UseSmartTags = true;
            DesignerOptionCollection wfd = this.CreateOptionCollection(options, "WindowsFormsDesigner", null);
            this.CreateOptionCollection(wfd, "General", ops);
        }
    }//end_class

    internal class DesignerOptionServiceExt4NoGuides : DesignerOptionService
    {
        public DesignerOptionServiceExt4NoGuides() : base() { }

        protected override void PopulateOptionCollection(DesignerOptionCollection options)
        {
            if (null != options.Parent) return;

            DesignerOptions ops = new DesignerOptions();
            ops.GridSize = new Size(8, 8);
            ops.SnapToGrid = false;
            ops.ShowGrid = false;
            ops.UseSnapLines = false;
            ops.UseSmartTags = true;
            DesignerOptionCollection wfd = this.CreateOptionCollection(options, "WindowsFormsDesigner", null);
            this.CreateOptionCollection(wfd, "General", ops);
        }
    }//end_class

}//end_namespace
