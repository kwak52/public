using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpCommon.ResultLog;
using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.DxUtility;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Dsu.Common.Utilities.ExtensionMethods;
using PsCommon;
using PsKGaudi.Parser;
using PsKGaudi.Parser.PsCCSSTDFn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CpCommon.ExceptionHandler;

namespace CpTesterSs.UserControl
{
	/// <summary>
	/// Display Test Steps.
	/// </summary>
	public partial class UcTStepList : DevExpress.XtraEditors.XtraUserControl
	{
		/// unbounded columns
		public Dictionary<int, ArrayList> DicUbBreakPointColumns { get; set; } = new Dictionary<int, ArrayList>(); /// measure, result, info, break-point.
		private UcMainViewSs userCtrMainView;

		public int SearchIndex { get; set; } = 0;

		public UcTStepList(UcMainViewSs userCtrMainView)
		{
			InitializeComponent();
			this.userCtrMainView = userCtrMainView;
			this.Dock = DockStyle.Fill;
		}

		public static void InitGridCellImage(GridView gridView, RepositoryItemImageComboBox rImageComboTestList = null)
		{   
			if (rImageComboTestList != null)
				gridView.Columns[(int)(PsCCSDefineEnumSTDFunctionSummary.STEP)].ColumnEdit = rImageComboTestList;
		}

		public void InitGridAppearance(RepositoryItemImageComboBox rImageComboTestList = null)
		{
			GridView gridView = GridViewTestSteps; 

            foreach (GridColumn col in gridView.Columns)
			{
				col.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
				col.OptionsColumn.AllowSort = DefaultBoolean.False;
			}
			gridView.OptionsView.EnableAppearanceEvenRow = true;
            
            if (rImageComboTestList != null)
				gridView.Columns[(int)(PsCCSDefineEnumSTDFunctionSummary.STEP)].ColumnEdit = rImageComboTestList;

			gridView.BestFitColumns(true);

			var optionColumn = gridView.Columns[(int) (PsCCSDefineEnumSTDFunctionSummary.STEP)].OptionsColumn;

			optionColumn.AllowEdit = false;
			optionColumn.FixedWidth = true;                        
		}

		public void InitGridColAppearance()
		{
			GridControl gridCtr = gridCtrTestSteps;
			GridView gridView = GridViewTestSteps;

			new []
			{
				UiCpDefineEnumTsColumns.MSR, // measure column.
				UiCpDefineEnumTsColumns.RST, // result column.
				UiCpDefineEnumTsColumns.INF, // info column.
			}.ForEach(e =>
			{
				UiCpAssistForDxApp.appendStringColumn(gridView, e.ToString());
			});

			/// check box column for the break-point.
			UiCpAssistForDxApp.appendCheckBoxColumn(gridCtr, gridView, UiCpDefineEnumTsColumns.BRK.ToString());

			/// rearrange columns.
			UiCpAssistForDxApp.arrangeColumns(gridView);

			/// hide columns by the user (UI).
			UiCpAssistForDxApp.setVisibleColumns(gridView, UiCpDefineEnumTsColumns.COMMENTS.ToString(), false);

			/// read only property
			foreach (GridColumn col in gridView.Columns)
			{
				if (col.FieldName == UiCpDefineEnumTsColumns.BRK.ToString())
				{
					col.OptionsColumn.ReadOnly = false;
					col.OptionsColumn.AllowEdit = true;					
				}				    

				col.OptionsColumn.ReadOnly = true;
			}
		}

		public void InitGridColUndboundData()
		{
			GridControl gridCtr = gridCtrTestSteps;
			/// unbound columns data
			DicUbBreakPointColumns.Clear();
			for (int i = 0; i < ((DataTable)(gridCtr.DataSource)).Rows.Count; i++)
			{
				string strStepNum = ((DataTable)(gridCtr.DataSource)).Rows[i].ItemArray[0] as string;
				ArrayList arUnboundData = new ArrayList();
				
				foreach(string str in Enum.GetNames(typeof(UiCpDefineEnumTsUnboundColumns)))
					arUnboundData.Add(string.Empty);

				arUnboundData.RemoveAt(0);

				arUnboundData.Add(false); ///break-point
				DicUbBreakPointColumns.Add(Convert.ToInt32(strStepNum), arUnboundData);
			}
		}                    
        
        public void InitGridRowAppearance()
        {
            GridControl gridCtr = gridCtrTestSteps;
            GridView gridView = GridViewTestSteps;
            CpStnManager StnMng = userCtrMainView.MngStation;
            CpTsManager TsMng = userCtrMainView.MngStation.MngTStep as CpTsManager;
            List<DataRow> vDelRows = new List<DataRow>();

            gridCtr.BeginInit();

            foreach (DataRow dtRow in (gridCtr.DataSource as DataTable).Rows)
            {
                if (Convert.ToInt32(dtRow[PsCCSDefineEnumSTDFunctionSummary.STEP.ToString()].ToString()) < Convert.ToInt32(StnMng.CnfTestCondition.StartStep))
                    vDelRows.Add(dtRow);                    
            }           
            foreach(DataRow dtRow in vDelRows)
                (gridCtr.DataSource as DataTable).Rows.Remove(dtRow);

            gridCtr.EndInit();
        }

        /// event functions.
        private void gridViewTestSteps_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
		{
			TryAction(() =>
			{
				if (e.Column.FieldName == UiCpDefineEnumTsUnboundColumns.BRK.ToString() && DicUbBreakPointColumns.Count > 0)
				{
					/// get test step number.
					object objStepNum = gridViewTestSteps.GetRowCellValue(e.ListSourceRowIndex, PsKGaudi.Parser.PsCCSDefineEnumSTDFunctionSummary.STEP.ToString());
					int nSelStep = Convert.ToInt32(objStepNum);

					if (DicUbBreakPointColumns.ContainsKey(nSelStep))
					{
						ArrayList arUnboundData = DicUbBreakPointColumns[nSelStep];
						if (e.IsGetData)
							e.Value = (bool)(DicUbBreakPointColumns[nSelStep][(int)UiCpDefineEnumTsUnboundColumns.BRK]);
						else
							DicUbBreakPointColumns[nSelStep][(int)UiCpDefineEnumTsUnboundColumns.BRK] = e.Value;
					}

				}
			});
		}

		private void gridViewTestSteps_MouseUp(object sender, MouseEventArgs e)
		{
			TryAction(() =>
			{
				Point pt = new Point(e.X, e.Y);
				GridHitInfo hit = gridViewTestSteps.CalcHitInfo(pt);

				if (hit.Column == null || hit.RowHandle < 0)
					return;

				object objStepNum = gridViewTestSteps.GetRowCellValue(hit.RowHandle, PsKGaudi.Parser.PsCCSDefineEnumSTDFunctionSummary.STEP.ToString());

				gridViewTestSteps.FocusedRowHandle = hit.RowHandle;

				if (e.Button == MouseButtons.Right)
					popupMenuTStepView.ShowPopup(Control.MousePosition);
			});
		}       

		void repositoryItemSearchControl2_KeyDown(object sender, KeyEventArgs e)
		{
			TryAction(() =>
			{
				if (e.KeyCode.Equals(Keys.Enter))
				{
					setGridViewTestStepByStepNumber(SearchIndex);
					popupMenuTStepView.Manager.CloseMenus();
				}
			});
		}

		void repositoryItemSearchControl2_EditValueChanging(object sender, ChangingEventArgs e)
		{
			TryAction(() =>
			{				
				if (e.NewValue != null)
				{
					int nTstep = -1;
					if (int.TryParse(e.NewValue.ToString(), out nTstep))
						SearchIndex = nTstep;
				}
			});
		}

		public void setGridViewTestStepByStepNumber(int nStep)
		{
			TryAction(() =>
			{
				DataTable dtTestStep = (DataTable)(gridCtrTestSteps.DataSource);
				int index = dtTestStep.Rows.IndexOf(dtTestStep.Rows.Find(nStep));
				gridViewTestSteps.FocusedRowHandle = gridViewTestSteps.GetRowHandle(-1);
				gridViewTestSteps.FocusedRowHandle = gridViewTestSteps.GetRowHandle(index);
			});
		}

		private void userCtrTStepList_Load(object sender, EventArgs e)
		{

        }

		public static RepositoryItemImageComboBox GetInitialGridIconResx(Dictionary<string, PsCCSDefineEnumSTDFuncIconType> dicStepNumWithType,
																		 GridControl gridCtr)
		{
			// icons
			RepositoryItemImageComboBox rImageCombo = gridCtr.RepositoryItems.Add("ImageComboBoxEdit") as RepositoryItemImageComboBox;
			ImageCollection images = new ImageCollection();

			images.AddImage(Image.FromFile(PsCmDefineUiIconPath.PATH_MACRO_ACTIVATE));
			images.AddImage(Image.FromFile(PsCmDefineUiIconPath.PATH_MACRO_DEACTIVATE));
			images.AddImage(Image.FromFile(PsCmDefineUiIconPath.PATH_MODULE_ACTIVATE));
			images.AddImage(Image.FromFile(PsCmDefineUiIconPath.PATH_MODULE_DEACTIVATE));
			images.AddImage(Image.FromFile(PsCmDefineUiIconPath.PATH_VARIANT_ACTIVATE));
			images.AddImage(Image.FromFile(PsCmDefineUiIconPath.PATH_MACRO_CONTROL));

			rImageCombo.SmallImages = images;

			foreach (KeyValuePair<string, PsCCSDefineEnumSTDFuncIconType> step in dicStepNumWithType)
				rImageCombo.Items.Add(new ImageComboBoxItem(step.Key, step.Key, (int)(step.Value)));

			rImageCombo.GlyphAlignment = HorzAlignment.Near; //near : show text, Center
			rImageCombo.ReadOnly = true;
			rImageCombo.AllowDropDownWhenReadOnly = DefaultBoolean.False;
			rImageCombo.ShowDropDown = ShowDropDown.Never;
			return rImageCombo;
		}

		public void SetDataSource(DataTable srcTestList)
        {
            srcTestList.Columns[(int)PsCCSDefineEnumSTDFunctionSummary.DIMENSION].ColumnName = UiCpDefineEnumTsColumns.DIM.ToString();
            gridCtrTestSteps.DataSource = srcTestList;
        }
		
		/// gate for tasks
		public static async Task AsyncUpdateInitialGridViewAll(CpTsManager cpTsManager, UcTStepList userCtrTsList)
		{
			await userCtrTsList.DoAsync(() =>
			{
				Debug.Assert(cpTsManager.GaudiReadData.GetDataTable() != null);
				userCtrTsList.SetDataSource(cpTsManager.GaudiReadData.GetDataTable());
				userCtrTsList.InitGridColAppearance();
				userCtrTsList.InitGridColUndboundData();
				userCtrTsList.InitGridAppearance();
                userCtrTsList.InitGridRowAppearance();
            });
		}

		private void barButtonItemUncheckAllSteps_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			for (int i = 0; i < DicUbBreakPointColumns.Count; i++)
			{
				int nStepNum = ((CpTsManager) userCtrMainView.MngStation.MngTStep).getTStepByIndex(i).Core.StepNum;
				if ((bool)(DicUbBreakPointColumns[nStepNum][(int)UiCpDefineEnumTsUnboundColumns.BRK]) == true)
					DicUbBreakPointColumns[nStepNum][(int)UiCpDefineEnumTsUnboundColumns.BRK] = false;
			}

			gridCtrTestSteps.RefreshDataSource();
		}

		private void gridViewTestSteps_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
		{
			 if (userCtrMainView.MngStation.MngTStep == null)
					return;

				object objStepNum = gridViewTestSteps.GetRowCellValue(e.FocusedRowHandle, UiCpDefineEnumGridViewTestStep.STEP.ToString());
				int nSelStepNumber = Convert.ToInt32(objStepNum);
				if (nSelStepNumber >= 0)
					updateGridViewForModuleBySelectedStep(nSelStepNumber);
		}
				
		public void UpdateFocusForSelectedStep(int nCrtStepIndex)
		{
			this.DoAsync(() =>
			{
				if (nCrtStepIndex != -1)
					setGridViewTestStepByStepNumber(nCrtStepIndex);
			});
		}

		public bool updateGridViewForModuleBySelectedStep(int nSelStepNumber)
		{
			
			PsCCSStdFnBase psSelStep = null;
			((CpTsManager) userCtrMainView.MngStation.MngTStep).GaudiReadData.GetTestStepData(nSelStepNumber, out psSelStep);
			if (psSelStep == null)
				return false;

			return true;			
		}

        public void FilterNGSteps(bool bFilter)
        {

          
        }
		private void gridViewTestSteps_RowClick(object sender, RowClickEventArgs e)
		{
			var hitInfo = gridViewTestSteps.CalcHitInfo(e.X, e.Y);

			if(hitInfo.Column.FieldName == UiCpDefineEnumTsUnboundColumns.BRK.ToString() && DicUbBreakPointColumns.Count > 0)
			{
				object objStepNum = gridViewTestSteps.GetRowCellValue(e.RowHandle, PsKGaudi.Parser.PsCCSDefineEnumSTDFunctionSummary.STEP.ToString());
				int nSelStep = Convert.ToInt32(objStepNum);
				bool bCurBrkValue = (bool)(DicUbBreakPointColumns[nSelStep][(int) UiCpDefineEnumTsUnboundColumns.BRK]);
				bool bNxtBrkValue = (bCurBrkValue == true) ? false : true;

				(DicUbBreakPointColumns[nSelStep][(int) UiCpDefineEnumTsUnboundColumns.BRK]) = bNxtBrkValue;
				gridViewTestSteps.SetRowCellValue(e.RowHandle, GridViewTestSteps.FocusedColumn, bNxtBrkValue);
			}
		}		

		public bool updateGridViewStepsByShowOption(bool bShowDeactivatedSteps)
        {
            return TryAction(() =>
            {
                DataTable dtResult = gridViewTestSteps.DataSource as DataTable;


				//gvResult.selectr


            }).Succeeded;
        }

        private void gridViewTestSteps_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = (sender as GridView);

            if (e.RowHandle < 0)
                return;

            CpStnManager StnMng = userCtrMainView.MngStation;
            CpTsManager TsMng = userCtrMainView.MngStation.MngTStep as CpTsManager;
            string strRowStep = view.GetRowCellValue(e.RowHandle, PsCCSDefineEnumSTDFunctionSummary.STEP.ToString()).ToString();
            PsCCSStdFnBase stdFnStep = userCtrMainView.MngStation.MngTStep.GaudiReadData.ListTestStep.Find(x => x.StepNum == Convert.ToInt32(strRowStep));

            if (userCtrMainView.MngStation.MngTStep.DicTsResult.ContainsKey(stdFnStep?.StepNum ?? -1))
            {
                TsResult stepResult = TsMng.DicTsResult[stdFnStep.StepNum];
                TsResult stepMsResult = (TsMng.getTStepByNum(stdFnStep.StepNum).ResultLog as ClsRlMeasuring)?.TsMeasuringResult ?? TsResult.NONE;
              
                if (e.Column.FieldName == UiCpDefineEnumTsUnboundColumns.RST.ToString())
                {                      
                    if (TsMng.DicTsResult[stdFnStep.StepNum] == TsResult.OK)
                        e.Appearance.ForeColor = Color.Green;
                    else if (TsMng.DicTsResult[stdFnStep.StepNum] == TsResult.NG || stepMsResult == TsResult.NG)
                        e.Appearance.ForeColor = Color.Red;
                    else if (TsMng.DicTsResult[stdFnStep.StepNum] == TsResult.ERROR)
                        e.Appearance.ForeColor = Color.Orange;
                    else
                        e.Appearance.ForeColor = Color.Transparent;
                 
                    e.Appearance.DrawString(e.Cache, stepResult.ToString(), e.Bounds);
                }
                else if (e.Column.FieldName == UiCpDefineEnumTsUnboundColumns.MSR.ToString())
                {
                    e.Appearance.DrawString(e.Cache, (TsMng.getTStepByNum(stdFnStep.StepNum).ResultLog as ClsRlMeasuring)?.MeasuredValue.ToString(), e.Bounds);
                    e.Appearance.ForeColor = stepMsResult == TsResult.NG ? Color.Red : Color.Green;
                }
                else if (e.Column.FieldName == UiCpDefineEnumTsUnboundColumns.INF.ToString())
                {
                    e.Appearance.DrawString(e.Cache, (TsMng.getTStepByNum(stdFnStep.StepNum) as CpTsMacroShell)?.GetDistInfo().ToString(), e.Bounds);
                    e.Appearance.ForeColor = Color.Black;
                }

            }
            else
                e.Appearance.BackColor = Color.Transparent;
        }

        private void gridViewTestSteps_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = (sender as GridView);

            if (e.RowHandle < 0)
                return;
            CpStnManager StnMng = userCtrMainView.MngStation;
            CpTsManager TsMng = userCtrMainView.MngStation.MngTStep as CpTsManager;
            PsCCSStdFnBase stdFnStep = userCtrMainView.MngStation.MngTStep.GaudiReadData.ListTestStep[e.RowHandle];
                        
            if (stdFnStep.Activate == PsCCSDefineEnumActivate.DEACTIVATE)
                e.Appearance.ForeColor = Color.Salmon;

            if (stdFnStep.VariantActivate == PsCCSDefineEnumVariantAcrivate.DEACTIVATE)
                e.Appearance.ForeColor = Color.Gray;       
        }

        private void gridViewTestSteps_CustomRowFilter(object sender, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e)
        {
           
        }
    }
}
