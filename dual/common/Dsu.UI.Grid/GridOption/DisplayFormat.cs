using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraGrid.Columns;
using Dsu.UI.Grid.Enumeration;
using System.Drawing;

namespace Dsu.UI.Grid.GridOption
{
    public sealed class DisplayFormat
    {
        #region DateTime
        //  "yyyy-MM-dd ddd tt hh:mm";  //2016-08-18 목 오전 09:52
        //  "yyyy-MM-dd ddd";  //2016-08-18 목
        //  "tt hh:mm:ss.ff";  //오전 09:40:13.317

        //  "ss.ff";  //18-목 09:55
        //  "MM-dd";  //08-18
        //  "dd-ddd HH:mm";  //18-목 09:55
        public static void ViewDataTypePattern(GridColumn gridColumn, bool bDateOnly = false)
        {
            if (gridColumn.ColumnType == typeof(DateTime))
            {
                gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;

                if (bDateOnly)
                    gridColumn.DisplayFormat.FormatString = "yyyy-MM-dd ddd";
                else
                    gridColumn.DisplayFormat.FormatString = "g";
                //   gridColumn.DisplayFormat.FormatString = "dd-ddd HH:mm:ss";

            }
//             else if (gridColumn.ColumnType.IsValueType && gridColumn.ColumnType != typeof(DateTime))
//             {
//                 gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
// 
//                 if (gridColumn.ColumnType.Name.Contains("Int"))
//                     gridColumn.DisplayFormat.FormatString = "D";
//                 else
//                     gridColumn.DisplayFormat.FormatString = "F";
//                 //                     gridColumn.DisplayFormat.FormatString = "E";
//                 //                     gridColumn.DisplayFormat.FormatString = "X";
//                 //  https://documentation.devexpress.com/#WindowsForms/CustomDocument1498
//             }
        }

        #endregion

        #region Numeric
        public static void ViewNumericPattern(GridColumn gridColumn)
        {
           

      
        }

        #endregion

        #region String


        #endregion

        public static void ViewResultColor(GridColumn gridColumn)
        {
            if (gridColumn.FieldName.ToUpper() == "NG"
                || gridColumn.FieldName.ToUpper() == "RESULT"
                || gridColumn.FieldName.ToUpper() == "CPK"
                || gridColumn.FieldName.ToUpper() == "INFO"
                || gridColumn.FieldName.ToUpper() == "%GOOD"
                || gridColumn.FieldName.ToUpper() == "%GOOD100")
            {
                gridColumn.AppearanceCell.BackColor = Color.LightGreen;
            }
        }
    }
}
