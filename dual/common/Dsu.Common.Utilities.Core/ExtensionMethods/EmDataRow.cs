using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    /// <summary>
    /// http://romacode.com/blog/c-helper-functions-to-map-a-datatable-or-datarow-to-a-class-object
    /// see UnitTestDataRow
    /// </summary>
    public static class EmDataRow
    {
        /// <summary>
        /// function that set the given object from the given data row 
        /// </summary>
        public static void SetItem<T>(this DataRow row, T item)
            where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }

        /// <summary>
        /// function that creates an object from the given data row
        /// </summary>
        public static T CreateItem<T>(this DataRow row)
            where T : new()
        {
            // create a new object
            T item = new T();

            // set the item
            row.SetItem(item);

            // return 
            return item;
        }

        /// <summary>
        /// function that creates a list of an object from the given data table
        /// </summary>
        public static List<T> CreateList<T>(this DataTable tbl)
            where T : new()
        {
            // define return list
            List<T> lst = new List<T>();

            // go through each row
            foreach (DataRow r in tbl.Rows)
            {
                // add to the list
                lst.Add(r.CreateItem<T>());
            }

            // return the list
            return lst;
        }
    }
}
