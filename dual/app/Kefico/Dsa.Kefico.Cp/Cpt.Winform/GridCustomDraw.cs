using DevExpress.XtraGrid.Views.Base;
using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Linq;
using PsCommon;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Cpt.Winform
{
    public class GridCustomDraw
    {
        /// dimension 이  {HEX, BIN, STR} 중의 하나일 경우에 Min, Max, Value column 을 custom draw 수행
        /// - 일반 dimension 에 대해서 소수점 이하 .0 으로만 끝나는 부분 잘라내기
        public static void CustomDrawMinMaxValue(object o, RowCellCustomDrawEventArgs e)
        {
            var colName = e.Column.FieldName;
            if (!colName.IsOneOf("min", "max", "value"))
            {
                e.Handled = false;
                return;
            }

            try
            {
                var cell = e.Cell as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo;
                var value = e.CellValue.ToString();
                if (value.IsNullOrEmpty())
                    return;

                // 현재 그릴 cell 의 row 중에서 column 명이 "dim" 으로 끝나는 cell 을 찾음 -> dim 값 결정에 사용됨.
                var dimCell = cell.RowInfo.Cells.OfNotNull().Where(c => c.Column != null && c.Column.FieldName == "dim").FirstOrDefault();
                if (dimCell == null)
                    return;

                var dim = (CpSpecDimension)Dsu.Common.Utilities.Tools.ForceToInt(dimCell.CellValue);

                var row = e.RowHandle;
                var t = e.Cell.GetType();
                switch (dim)
                {
                    // 십진 문자열(소수점이하 .00000을 포함할 수 있음)을 16진 문자열로 변환
                    case CpSpecDimension.HEX:
                        e.DisplayText = GaudiFileParserApi.String2HexString(value);
                        e.Handled = true;
                        return;

                    // 임의의 문자열에 대한 ascii encoding 된 십진 문자열을 다시 ascii 로 원복 변환
                    case CpSpecDimension.STR:       // function 이 HEXSTRINGTOSTR 인지 검사할 필요가 있는가???
                    case CpSpecDimension.BIN:
                        if (value.NonNullAny())
                        {
                            e.DisplayText = GaudiFileParserApi.Number2Ascii(GaudiFileParserApi.String2BigInt(value));
                            e.Handled = true; return;
                        }
                        break;

                    default:
                        {
                            // 일반 dimension 에서 소수점 이하 0으로 끝나는 부분을 제거함. e.g "15.00000" -> "15"
                            var match = Regex.Match(value, @".*(\.0+)$");
                            if (match.Groups.Count == 2)
                            {
                                e.DisplayText = Regex.Replace(value, @"\.0+", "");
                                e.Handled = true; return;
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            e.Handled = false;
        }
        public static void CustomDrawDimensionColumn(object o, RowCellCustomDrawEventArgs e)
        {
            var colName = e.Column.FieldName;
            if (!colName.IsOneOf("dim", "dimension"))
            {
                e.Handled = false;
                return;
            }

            try
            {
                var cell = e.Cell as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo;
                var value = e.CellValue.ToString();
                if (value.IsNullOrEmpty())
                    return;

                // 이미 맞는 type 으로 정의되어 있다면 추가 작업 불필요
                if (e.Column.ColumnType == typeof(CpSpecDimension))
                {
                    e.Handled = false;
                    return;
                }

                // int type 의 dimension 을 CpSpecDimension type 으로 변환
                int result = -1;
                if (Int32.TryParse(value, out result))
                {
                    e.DisplayText = ((CpSpecDimension)result).ToString();
                    e.Handled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                e.Handled = false;
                Trace.WriteLine(ex);
            }
        }
    }
}
