using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.Common.Utilities.Core.FSharpInterOp;
using Microsoft.FSharp.Core;
using static CSharpInterop;
using static LanguageExt.Prelude;
using static LanguageExt.FSharp;
using PsCommon;

namespace Cpt.Winform
{
    public partial class FormCptTestResult : Form
    {
        private CptManagerModule.CptManager _cptManager;
        private DataTable DataTable => gridControl1.DataSource as DataTable;

        private IEnumerable<DataRow> Rows => DataTable.Rows.Cast<DataRow>();
        private IEnumerable<DataRow> MinMaxedRows
        {
            get
            {
                return Rows
                    .Where(r => (r["min"] as string).NonNullAny() && (r["max"] as string).NonNullAny());
            }
        }

        /// <summary>
        /// Step table id 별로 정렬된 서버에서 받은 test step list
        /// </summary>
        private Dictionary<uint, Step.Step> _loadedStepMap;

        //private List<Step.Step> _sampleSteps;


        public FormCptTestResult(CptManagerModule.CptManager cptManager, IEnumerable<Step.Step> steps)
        {
            InitializeComponent();
            cbFillNonMeasureOnly.Checked = true;

            _cptManager = cptManager;
            _loadedStepMap = steps.ToDictionary(s => s.id, s => s);

            //uint nId = 0, nPositionNumber = 0, nStepNumber = 0;
            //_sampleSteps = new Step.Step[]
            //{
            //    new Step.Step() {id = nId++, positionNumber = nPositionNumber++, stepNumber = nStepNumber++, min = FSharpOption<decimal>.None, max = FSharpOption<decimal>.Some(100)},
            //    new Step.Step() {id = nId++, positionNumber = nPositionNumber++, stepNumber = nStepNumber++, min = FSharpOption<decimal>.Some(0), max = FSharpOption<decimal>.Some(10)},
            //    new Step.Step() {id = nId++, positionNumber = nPositionNumber++, stepNumber = nStepNumber++, min = FSharpOption<decimal>.Some(0), max = FSharpOption<decimal>.Some(200)},
            //    new Step.Step() {id = nId++, positionNumber = nPositionNumber++, stepNumber = nStepNumber++, min = FSharpOption<decimal>.Some(0), max = FSharpOption<decimal>.Some(100)},
            //    new Step.Step() {id = nId++, positionNumber = nPositionNumber++, stepNumber = nStepNumber++, min = FSharpOption<decimal>.Some(0), max = FSharpOption<decimal>.Some(100)},
            //    new Step.Step() {id = nId++, positionNumber = nPositionNumber++, stepNumber = nStepNumber++, min = FSharpOption<decimal>.Some(0), max = FSharpOption<decimal>.Some(100)},
            //    new Step.Step() {id = nId++, positionNumber = nPositionNumber++, stepNumber = nStepNumber++, min = FSharpOption<decimal>.Some(0), max = FSharpOption<decimal>.Some(100)},
            //    new Step.Step() {id = nId++, positionNumber = nPositionNumber++, stepNumber = nStepNumber++, min = FSharpOption<decimal>.Some(0), max = FSharpOption<decimal>.Some(100)},
            //    new Step.Step() {id = nId++, positionNumber = nPositionNumber++, stepNumber = nStepNumber++, min = FSharpOption<decimal>.Some(0), max = FSharpOption<decimal>.Some(100)},
            //}.ToList();


            //gridControl1.DataSource = Step.toDataTable(_sampleSteps, includeValue:true, includeMessage:true);
            gridControl1.DataSource = Step.toDataTable(steps, includeValue: true, includeMessage: true);


            var pause = TimeSpan.FromSeconds(1);
            _trackbarSubject
                .Window(() => Observable.Interval(pause))               // .OnceInTimeWindow(TimeSpan.FromSeconds(1))
                .SelectMany(x => x.Take(1))
                .Subscribe(evt => FillMeasuredValue())
                ;
        }

        private void FormCptTestResult_Load(object sender, EventArgs e)
        {
            gridView1.CustomDrawCell += GridCustomDraw.CustomDrawMinMaxValue;
        }


        private Subject<object> _trackbarSubject = new Subject<object>();
        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            _trackbarSubject.OnNext(1);            
        }


        private void FillMeasuredValue()
        {
            var ratio = (decimal)((double)trackBar1.Value)/(trackBar1.Maximum - trackBar1.Minimum);
            Trace.WriteLine($"{trackBar1.Value} = {ratio}%");

            MinMaxedRows
                .ForEach(r =>
                {
                    var min = ParseDecimal(r, "min").Value;
                    var max = ParseDecimal(r, "max").Value;
                    var dim = (CpSpecDimension)r["dim"];
                    //var dim = (CpSpecDimension)Dsu.Common.Utilities.Tools.ForceToInt(r["dim"]);
                    if (dim.IsOneOf(CpSpecDimension.STR, CpSpecDimension.HEX, CpSpecDimension.BIN))
                        r["value"] = r["min"];
                    else
                    {
                        var value = min + (max - min) * ratio;
                        r["value"] = value;
                    }
                });
        }

        private FSharpOption<Decimal> ParseDecimal(DataRow row, string columnName)
        {
            var cell = row[columnName] as string;
            if (cell.NonNullAny())
                return FSharpOption<decimal>.Some(Decimal.Parse(cell));

            return FSharpOption<decimal>.None;
        }
        private UInt32 ExtractUInt32(DataRow row, string columnName)
        {
            var obj = row[columnName];
            var cell = obj as string;
            if (cell.NonNullAny())
                return UInt32.Parse(cell);

            if (row[columnName] is UInt32)
                return (UInt32)row[columnName];

            throw new Exception("What's this???");
        }



        private int CountMinMax(DataRow row)
        {
            var m = row["min"];
            var M = row["max"];

            return ((m is DBNull || (m is string && ((string)m).IsNullOrEmpty())) ? 0 : 1)
                + ((M is DBNull || (M is string && ((string)M).IsNullOrEmpty())) ? 0 : 1)
                ;

        }


        private System.Tuple<List<DataRow>, List<DataRow>> GroupRowsByHavingMinMax()
        {
            var query =
                from r in Rows
                group r by CountMinMax(r)
                into g
                select new { MinMax = g.Key, SubRows = g.ToList() }
                ;
            var dict = query.ToDictionary(q => q.MinMax, q => q.SubRows);

            if (dict.ContainsKey(1))
            {
                MessageBox.Show("Just one of either min/max not specified.");
                return null;
            }

            var noMinMaxed = dict.ContainsKey(0) ? dict[0] : new List<DataRow>();
            var minMaxed = dict.ContainsKey(2) ? dict[2] : new List<DataRow>();

            return System.Tuple.Create(noMinMaxed, minMaxed);
        }


        /*
            G|X)
                [[ $oper = 'G' ]] && ok=1 || ok=0
                sql "CALL _generateBundleDataIntoTemporaryTable($pdvId, $ok);
                     CALL insertMeasure('$date', '$time', 33.2, $ccsId, $pdvId,
                        $ecuId, $eprom, $fixture, $batchName, $ok);"
         */
        /// <summary>
        /// tt_bundle 에 bundle data 를 먼저 isnert 후, insertMeasure 호출
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnUpload_Click(object sender, EventArgs e)
        {
            var start = DateTime.Now;
            PrintYellow("Uploading test result to database..");

            btnUpload.Enabled = false;
            await Task.Run(() =>
            {
                try
                {
                    var tpl = GroupRowsByHavingMinMax();
                    var noMinMaxed = tpl.Item1;
                    var minMaxed = tpl.Item2;

                    if (noMinMaxed.Any(r => r["message"] is DBNull))
                    {
                        MessageBox.Show("There is row(s) which has neither min/max nor message.");
                        return;
                    }



                    if (minMaxed.Any(r => r["value"] is DBNull || r["value"] == null || ((string)r["value"]).IsNullOrEmpty()))
                    {
                        MessageBox.Show("There is row(s) which has min/max, but with missing value");
                        return;
                    }

                    var testSummary = new CptModule.TestSummary()
                    {
                        StartTime = start,
                        Duration = (decimal)(DateTime.Now - start).TotalSeconds,
                    };

                    IEnumerable<Step.UploadStep> uSteps = (
                                //var filteredSteps = (
                                from r in minMaxed
                                let min = ParseDecimal(r, "min")
                                let max = ParseDecimal(r, "max")
                                let id = (uint) r["id"]
                                let loadedStep = _loadedStepMap[id]
                                let value = ParseDecimal(r, "value")
                                // 시험기에서 OK/NG 판정
                                let isOK = min.IsSome() && max.IsSome() && value.IsSome() && min.Value <= value.Value && value.Value <= max.Value
                                let stepNumber = ExtractUInt32(r, "step")
                                let message = r["message"] is DBNull ? FSharpOption<string>.None : createSome((string) r["message"])
                                select new Step.UploadStep(loadedStep, isOK, value, message))
                            .Union(
                                from r in noMinMaxed
                                let id = (uint) r["id"]
                                let loadedStep = _loadedStepMap[id]
                                let message = createSome((string) r["message"])
                                let none = FSharpOption<decimal>.None
                                let stepNumber = ExtractUInt32(r, "step")
                                let isOK = true
                                select new Step.UploadStep(loadedStep, isOK, none, message)
                            )
                            .OrderBy(s => s.StepNumber)
                        ;

                    if (cbUntilNG.Checked)
                    {
                        var foundNG = false;
                        uSteps = uSteps.TakeWhile(s =>
                        {
                            if (s.IsOK && !foundNG)
                                return true;
                            if (foundNG)
                                return false;

                            foundNG = true;
                            return true;
                        });
                    }
                    else
                    {
                        int value = 0;
                        if (int.TryParse(textBoxLastStepNumber.Text, out value))
                        {
                            uSteps = uSteps.TakeWhile(s => s.StepNumber <= value);
                        }
                    }


                    var uploadSteps = uSteps.OrderBy(s => s.StepNumber).ToArray();

                    this.Do(() =>
                    {
                        statusStrip1.Text = "";
                        var result = _cptManager.UploadTestResult(testSummary, uploadSteps);
                        match(fs(result),
                            Some: v =>
                            {
                                PrintGreen("Done!");
                                statusStrip1.Text = $"Upload {uploadSteps.Count()} test result succeeded.";
                                MessageBox.Show($"Upload test result succeeded with code {v}");
                            },
                            None: () =>
                            {
                                PrintRed("Failed!");
                                MessageBox.Show("Upload failed!");
                            });
                    });
                }
                finally
                {
                    this.Do(() => btnUpload.Enabled = true);
                }

            });
        }

        private void btnFillMessage_Click(object sender, EventArgs e)
        {
            var targetRows = cbFillNonMeasureOnly.Checked ? GroupRowsByHavingMinMax().Item1 : Rows;
            targetRows.ForEach(r =>
            {
                r["message"] = textBoxMessage.Text;
            });
        }

        private void action1_Update(object sender, EventArgs e)
        {
            textBoxLastStepNumber.Enabled = !cbUntilNG.Checked;
        }
    }
}
