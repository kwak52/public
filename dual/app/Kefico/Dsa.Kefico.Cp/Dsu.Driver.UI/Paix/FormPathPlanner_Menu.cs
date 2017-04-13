using System;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.DX;
using Dsu.Common.Utilities.Forms;

namespace Dsu.Driver.UI.Paix
{
    public partial class FormPathPlanner
    {
        private void MoveUpSelectedRows()
        {
            var oldSelections = gridViewPosition.GetSelectedRows().ToArray();
            if ( MoveRows(oldSelections, true) )
            {
                Trace.WriteLine("Moving up...");
                gridViewPosition.ClearSelection();
                oldSelections.Select(n => n - 1).ForEach(n => gridViewPosition.SelectRow(n));
                gridViewPosition.LayoutChanged();
            }
        }

        private void MoveDownSelectedRows()
        {
            var oldSelections = gridViewPosition.GetSelectedRows().ToArray();
            if (MoveRows(oldSelections, false))
            {
                Trace.WriteLine("Moving down...");
                gridViewPosition.ClearSelection();
                oldSelections.Select(n => n + 1).ForEach(n => gridViewPosition.SelectRow(n));
                gridViewPosition.LayoutChanged();
            }
        }

        private void CreatePositionViewPopupMenus()
        {
            gridViewPosition.EnableDefaultContextMenu();
            var menu = (ContextMenuStripEx)gridViewPosition.GridControl.ContextMenuStrip;
            while (menu.Items.Count > 0)
                menu.Items.RemoveAt(0);

            menu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripSeparator(),
                new ToolStripMenuItem("Move up", null, (o, e) =>
                {
                    MoveUpSelectedRows();
                }),
                new ToolStripMenuItem("Move down", null, (o, e) =>
                {
                    MoveDownSelectedRows();
                }),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Copy", null, (o, e) =>
                {
                    _clipboardPoses = gridViewPosition.GetSelectedRows().Select(r => Poses[r].Duplicate()).ToArray();
                }),
                new ToolStripMenuItem("Paste", null, (o, e) =>
                {
                    _clipboardPoses.ForEach(p =>
                        Poses.Insert(menu.RClickedRowHandle, p.Duplicate()));
                    gridViewPosition.LayoutChanged();
                }),
                new ToolStripMenuItem("Set as master pattern", null, (o, e) =>
                {
                    if (menu.RClickedRowHandle < 0)
                    {
                        MessageBox.Show("Select unique line, first.");
                        return;
                    }

                    _master = Poses[menu.RClickedRowHandle].Duplicate();
                }),

            });
        }

        private void CreateAxesViewPopupMenus()
        {
            gridViewAxes.EnableDefaultContextMenu();
            var menu = (ContextMenuStripEx)gridViewAxes.GridControl.ContextMenuStrip;
            while (menu.Items.Count > 0)
                menu.Items.RemoveAt(0);

            menu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripSeparator(),
                new ToolStripMenuItem("Fill all cells with..", null, (o, e) =>
                {
                    var form = new FormSimpleEditor()
                    {
                        Title = "Enter value:",
                        Multiline = false,
                        ReadOnly = false,
                        Contents = "10000",
                    };
                    
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var value = Int64.Parse(form.Contents);
                        _selectedPose.SpeedSpec.AxesSpec.Iter( (ax, i) => ax.StartSpeed = ax.Acceleraion = ax.Deceleraion = ax.DriveSpeed = value);
                        gridViewAxes.LayoutChanged();
                    }
                }),

                new ToolStripMenuItem("Fill this axis cells with..", null, (o, e) =>
                {
                    var n = menu.RClickedRowHandle;
                    var form = new FormSimpleEditor()
                    {
                        Title = "Enter value:",
                        Multiline = false,
                        ReadOnly = false,
                        Contents = "10000",
                    };

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var value = Int64.Parse(form.Contents);
                        var ax = _selectedPose.SpeedSpec.AxesSpec[n];
                        ax.StartSpeed = ax.Acceleraion = ax.Deceleraion = ax.DriveSpeed = value;
                        gridViewAxes.LayoutChanged();
                    }
                }),

            });
        }
    }
}
