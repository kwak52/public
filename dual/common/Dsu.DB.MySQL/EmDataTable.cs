using System.Data;
using System.Text;

namespace Dsu.DB.MySQL
{
    /// <summary>
    /// Debugging 용으로 DataTable 내용을 print 해서 보기 위함.
    /// Release 에서는 data grid 를 이용해서 표현할 것.
    /// </summary>
    public static class EmDataTable
    {
        // http://stackoverflow.com/questions/1104121/how-to-convert-a-datatable-to-a-string-in-c
        public static string ConvertToString(this DataTable dt)
        {
            var output = new StringBuilder();

            var columnsWidths = new int[dt.Columns.Count];

            // Get column widths
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var length = row[i].ToString().Length;
                    if (columnsWidths[i] < length)
                        columnsWidths[i] = length;
                }
            }

            // Get Column Titles
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var length = dt.Columns[i].ColumnName.Length;
                if (columnsWidths[i] < length)
                    columnsWidths[i] = length;
            }

            // Write Column titles
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var text = dt.Columns[i].ColumnName;
                output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
            }
            output.Append("|\n" + new string('=', output.Length) + "\n");

            // Write Rows
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var text = row[i].ToString();
                    output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
                }
                output.Append("|\n");
            }
            return output.ToString();
        }

        private static string PadCenter(string text, int maxLength)
        {
            int diff = maxLength - text.Length;
            return new string(' ', diff / 2) + text + new string(' ', (int)(diff / 2.0 + 0.5));

        }
    }
}
