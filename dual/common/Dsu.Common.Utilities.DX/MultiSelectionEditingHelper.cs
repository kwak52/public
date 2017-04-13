using System;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace Dsu.Common.Utilities.DX
{
    /// <summary>
    /// https://www.devexpress.com/Support/Center/Example/Details/E2779
    /// </summary>
    public class MultiSelectionEditingHelper
    {

        private GridView _View;

        /// <summary>
        /// Cell 편집 값 validation.  null 이면 no validation
        /// {(편집된 cell value), row, column} => {Validate 된 value}
        /// </summary>
        /// https://www.devexpress.com/Support/Center/Question/Details/A289
        public Func<object, int, int, object> EditValidator { get; set; }
        public MultiSelectionEditingHelper(GridView view)
        {
            _View = view;
            _View.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDownFocused;
            _View.MouseUp += _View_MouseUp;
            _View.CellValueChanged += new CellValueChangedEventHandler(_View_CellValueChanged);
            _View.MouseDown += new MouseEventHandler(_View_MouseDown);
            _View.ValidatingEditor += _View_ValidatingEditor;
        }

        private void _View_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            var gridView = (GridView) sender;
            if (EditValidator != null)
                e.Value = EditValidator(e.Value, gridView.FocusedRowHandle, gridView.FocusedColumn.AbsoluteIndex);
        }

        void _View_MouseDown(object sender, MouseEventArgs e)
        {
            if (GetInSelectedCell(e))
            {
                GridHitInfo hi = _View.CalcHitInfo(e.Location);
                if (_View.FocusedRowHandle == hi.RowHandle)
                {
                    _View.FocusedColumn = hi.Column;
                    DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }

        }


        void _View_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            OnCellValueChanged(e);
        }

        bool lockEvents;
        private void OnCellValueChanged(CellValueChangedEventArgs e)
        {
            if (lockEvents)
                return;
            lockEvents = true;
            SetSelectedCellsValues(e.Value);
            lockEvents = false;
        }

        private void SetSelectedCellsValues(object value)
        {
            try
            {
                _View.BeginUpdate();
                GridCell[] cells = _View.GetSelectedCells();
                foreach (GridCell cell in cells)
                {
                    _View.SetRowCellValue(cell.RowHandle, cell.Column, value);

                    // EditValidator 를 한번 더 호출해서, 선택된 모든 cell 에 대한 validation 을 수행하도록 한다.
                    EditValidator(value, cell.RowHandle, cell.Column.AbsoluteIndex);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                _View.EndUpdate();
            }

        }
        private bool GetInSelectedCell(MouseEventArgs e)
        {
            GridHitInfo hi = _View.CalcHitInfo(e.Location);
            return hi.InRowCell && hi.InRowCell && _View.IsCellSelected(hi.RowHandle, hi.Column);
        }

        void _View_MouseUp(object sender, MouseEventArgs e)
        {
            bool inSelectedCell = GetInSelectedCell(e);
            if (inSelectedCell)
            {
                DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                _View.ShowEditorByMouse();
            }
        }
    }
}
