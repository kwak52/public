using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.ExtensionMethods.DataGrid
{
    public static class EmDataGrid
    {
        public static void ForEach(this DataGridViewRowCollection source, Action<DataGridViewRow> action)
        {
            foreach (DataGridViewRow item in source)
                action(item);
        }

        /// <summary>
        /// datagridview 에서 selectedCells 로 부터 selected row index 를 구해서 반환한다.
        /// </summary>
        /// <param name="selectedCells">DataGridView.SelectedCells 값</param>
        /// <returns></returns>
        public static List<int> CollectRowIndice(this DataGridViewSelectedCellCollection selectedCells)
        {
            HashSet<int> set = new HashSet<int>();
            for (int i = selectedCells.Count; i > 0; --i)
                set.Add(selectedCells[i - 1].RowIndex);

            return set.ToList();
        }

        /// <summary>
        /// datagridview 에서 selectedRows 로 부터 selected row index 를 구해서 반환한다.
        /// 잘 동작하는지 test 는 안해 봄.
        /// </summary>
        /// <param name="selectedRows">DataGridView.SelectedCells 값</param>
        /// <returns></returns>
        public static List<int> CollectRowIndice(this DataGridViewSelectedRowCollection selectedRows)
        {
            return selectedRows.Cast<DataGridViewRow>().Select(r => r.Index).ToList();
        }

        /// <summary>
        /// datagridview 에서 selectedCells 로 부터 selected column index 를 구해서 반환한다.
        /// </summary>
        public static List<int> CollectColumnIndice(this DataGridViewSelectedCellCollection selectedCells)
        {
            HashSet<int> set = new HashSet<int>();
            for (int i = selectedCells.Count; i > 0; --i)
                set.Add(selectedCells[i - 1].ColumnIndex);

            return set.ToList();
        }
    }
}
