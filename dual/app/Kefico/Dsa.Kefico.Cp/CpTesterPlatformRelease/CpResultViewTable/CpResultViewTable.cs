using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpCommon.ResultLog;
using CpTesterPlatform.CpCommon;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;

namespace CpTesterPlatform.CpResultViewTable
{
    public partial class CpResultViewTable : UserControl
    {
        enum eResultTableCol
        {
            STEP = 0,
            OBJECTIVE,
            MIN,
            MEASURE,
            MAX,
            DIM,
            RESULT
        };

        DataTable ResultTable = new DataTable();
        Dictionary<CpTsShell, DataRow> TracingStepList = new Dictionary<CpTsShell, DataRow>();

        public CpResultViewTable()
        {
            InitializeComponent();
            InitResultTable();
            //InitImageCollection();
        }

        void InitResultTable()
        {
            foreach (string strColName in Enum.GetNames(typeof(eResultTableCol)))
                ResultTable.Columns.Add(strColName);
        }

        public void SetTeststepManager(CpTsManager tsMng)
        {
            ClearResult();

            foreach(CpTsShell cpTstep in tsMng.LstTestSteps)
            {
                if (cpTstep.Core.Traceability == PsKGaudi.Parser.PsCCSDefineEnumTraceability.On
                    && cpTstep.Core.Activate == PsKGaudi.Parser.PsCCSDefineEnumActivate.ACTIVATE
                    && cpTstep.Core.VariantActivate == PsKGaudi.Parser.PsCCSDefineEnumVariantAcrivate.ACTIVATE)
                {
                    DataRow dtRow = ResultTable.NewRow();

                    dtRow[(int)eResultTableCol.STEP] = cpTstep.StepNum.ToString();
                    dtRow[(int)eResultTableCol.OBJECTIVE] = cpTstep.Core.GetMO();
                    dtRow[(int)eResultTableCol.MIN] = cpTstep.Core.PairedParmsMinMax.Min;
                    dtRow[(int)eResultTableCol.MAX] = cpTstep.Core.PairedParmsMinMax.Max;
                    dtRow[(int)eResultTableCol.MEASURE] = (cpTstep.ResultLog as ClsRlMeasuring).MeasuredValue.ToString();
                    dtRow[(int)eResultTableCol.DIM] = cpTstep.Core.GetDimension();
                    dtRow[(int)eResultTableCol.RESULT] = (cpTstep.ResultLog as ClsRlMeasuring).TsMeasuringResult.ToString();

                    ResultTable.Rows.Add(dtRow);
                    TracingStepList.Add(cpTstep, dtRow);
                }                   
            }

            gridControlResult.DataSource = ResultTable;
            gridControlResult.RefreshDataSource();
            gridViewResult.BestFitColumns();
            /*
            gridViewResult.Columns[(int)(eResultTableCol.RESULT)].OptionsColumn.AllowEdit = false;
            gridViewResult.Columns[(int)(eResultTableCol.RESULT)].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
            gridViewResult.Columns[(int)(eResultTableCol.RESULT)].Fixed = FixedStyle.Left;
            gridViewResult.Columns[(int)(eResultTableCol.RESULT)].MinWidth = gridViewResult.Columns[(int)(eResultTableCol.RESULT)].GetBestWidth() +
                    m_imgcStepType.ImageSize.Width;
            gridViewResult.Columns[(int)(eResultTableCol.RESULT)].OptionsColumn.FixedWidth = false;*/
        }

        void ClearResult()
        {
            (gridControlResult.DataSource as DataTable)?.Clear();
        }
        /*
        private ImageCollection m_imgcStepType = new ImageCollection();
        private void InitImageCollection()
        {
            m_imgcStepType.AddImage(Image.FromFile(@"D:\01.KeficoWork\01.CP-Platform\trunk\CpTesterPlatform\Utils\CpResultViewTable\LedOn.ico"));
            m_imgcStepType.AddImage(Image.FromFile(@"D:\01.KeficoWork\01.CP-Platform\trunk\CpTesterPlatform\Utils\CpResultViewTable\LedOff.ico"));

            

        }*/

        private void gridViewResult_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = (sender as GridView);

            if (e.RowHandle < 0)
                return;

            if (e.Column.FieldName != eResultTableCol.RESULT.ToString())
            {
                e.Appearance.ForeColor = Color.Black;
                return;
            }

            DataRow dtRow = view.GetDataRow(e.RowHandle);

            if (dtRow[(int)eResultTableCol.RESULT].ToString() == TsResult.OK.ToString())
                e.Appearance.ForeColor = Color.Green;
            else if (dtRow[(int)eResultTableCol.RESULT].ToString() == TsResult.NG.ToString())
                e.Appearance.ForeColor = Color.Red;
            else
                e.Appearance.ForeColor = Color.Orange;
            
            /*
            if (e.Column.FieldName == eResultTableCol.RESULT.ToString() && dtRow[(int)eResultTableCol.RESULT].ToString() == TsResult.OK.ToString())
            {
                e.Graphics.DrawImage(m_imgcStepType.Images[0], e.Bounds.X, e.Bounds.Y);                
                e.Handled = false;
            }*/
        }

        void UpdateResultState()
        {
            int nRowHandle = 0;

            foreach (CpTsShell cpTstep in TracingStepList.Keys)
            {                
                DataRow dtRow = TracingStepList[cpTstep];
                
                if(dtRow[(int)eResultTableCol.RESULT].ToString() != (cpTstep.ResultLog as ClsRlMeasuring).TsMeasuringResult.ToString())
                {
                    gridViewResult.FocusedRowHandle = nRowHandle;
                    dtRow[(int)eResultTableCol.MEASURE] = (cpTstep as CpTsMacroShell)?.GetRoundMsrVal(cpTstep.ResultLog as ClsRlMeasuring).ToString();
                    dtRow[(int)eResultTableCol.RESULT] = (cpTstep.ResultLog as ClsRlMeasuring).TsMeasuringResult.ToString();
                }

                nRowHandle++;
            }
        }
        

        delegate void deleUpdateResultView();
        public void threadSafeResultViewUpdate()
        {
            if (this.InvokeRequired)
            {
                deleUpdateResultView delSetStatus = new deleUpdateResultView(threadSafeResultViewUpdate);
                this.Invoke(delSetStatus);
            }
            else
            {
                UpdateResultState();
            }
        }
    }
}
