using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Dsu.Common.Utilities.ExtensionMethods;
using static CSharpInterop;
using PsCommon;

namespace Cpt.Winform
{
    public partial class FormCptSimulateTestListEdit : Form
    {
        private CptManagerModule.CptManager _cptManager;
        private DataTable DataTable => gridControl1.DataSource as DataTable;
        private IEnumerable<DataRow> Rows => DataTable.Rows.Cast<DataRow>();

        public FormCptSimulateTestListEdit(CptManagerModule.CptManager cptManager, IEnumerable<Step.Step> steps)
        {
            InitializeComponent();

            _cptManager = cptManager;
            gridControl1.DataSource = Step.toDataTable(steps, includeValue: false, includeMessage: false);
        }

        private void FormCptSimulateTestListEdit_Load(object sender, EventArgs e)
        {
            gridView1.CustomDrawCell += GridCustomDraw.CustomDrawMinMaxValue;
        }

        private string GetCellString(DataRow row, string columnName)
        {
            var cell = row[columnName];
            if (cell == null || cell is DBNull)
                return "NULL";

            if (cell is string)
            {
                if (((string)cell).IsNullOrEmpty())
                    return "NULL";

                return $"'{(string)cell}'";
            }

            if (cell is decimal)
                return cell.ToString();

            if (cell is int || cell is uint || cell is UInt64 || cell is short || cell is ushort)
                return cell.ToString();

            return cell.ToString();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            btnUpload.Enabled = false;
            try
            {
                PrintYellow("Uploading (simulated) modified test list..");
                var pdvId = _cptManager.PdvId.Value;


                // let sql = "INSERT INTO tt_step_parsed(pdvId, position, step, min, max, dim, modName)\nVALUES\n\t" + values + "\n;"
                var values = Rows.Select(r =>
                {
                    var id = GetCellString(r, "id");
                    var step = GetCellString(r, "step");
                    var position = GetCellString(r, "position");
                    var min = GetCellString(r, "min");
                    var max = GetCellString(r, "max");
                    var dim = GetCellString(r, "dim");
                    var fncId = GetCellString(r, "fncId");
                    var modName = GetCellString(r, "modName");
                    var parameter = GetCellString(r, "parameter");

                    if (min.IsNullOrEmpty() || max.IsNullOrEmpty())
                        Trace.WriteLine("ERROR");

                    return $"\n({pdvId}, {position}, {step}, {min}, {max}, {dim}, {fncId}, {modName}, {parameter})";
                }).ToArray();

                var valueString = String.Join(",", values) + "\n;";
                var dataTable = _cptManager.UploadModifiedTestListDebugging(valueString);
                gridControl1.DataSource = dataTable;
                Trace.WriteLine(valueString);

                PrintGreen("Done!");
            }
            finally
            {
                this.Do(() => btnUpload.Enabled = true);
            }
        }
    }
}
