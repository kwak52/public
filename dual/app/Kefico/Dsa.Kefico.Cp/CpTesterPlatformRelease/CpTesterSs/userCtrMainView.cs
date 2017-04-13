using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Office.Utils;
using System.Diagnostics;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Skins;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraBars;
using DxUtility;
using CpTesterPlatform.CpUtility.TextString;
using PsKGaudi.Parser;
using PsKGaudi.Parser.PsCCSSTDFn;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpSystem.Manager;

namespace DXAppCPTester
{
    public partial class userCtrMainView : DevExpress.XtraEditors.XtraUserControl
    {
        private frmMainFrame m_xMainFrame = null;

        // unbounded columns
        private Dictionary<int, ArrayList> m_dicUbBreakPointColumns = new Dictionary<int, ArrayList>(); // measure, result, info, break-point.        
        public Dictionary<int, ArrayList> DicUbBreakPointColumns
        {
            get { return m_dicUbBreakPointColumns; }
            set { m_dicUbBreakPointColumns = value; }
        }

        private int m_nSearchIndex = 0;
        public int SearchIndex
        {
            get { return m_nSearchIndex; }
            set { m_nSearchIndex = value; }
        }

        public userCtrMainView(frmMainFrame xMainFrame)
        {
            try
            {
                InitializeComponent();
                m_xMainFrame = xMainFrame;                
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to load the user control (main view).", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
        }

        public RepositoryItemImageComboBox getInitGridIconResource(Dictionary<string, PsCCSDefineEnumSTDFuncIconType> dicStepNumWithType, GridControl gridCtr)
        {
            try
            {   
                // icons
                RepositoryItemImageComboBox rImageCombo = gridCtr.RepositoryItems.Add("ImageComboBoxEdit") as RepositoryItemImageComboBox;
                DevExpress.Utils.ImageCollection images = new DevExpress.Utils.ImageCollection();
                //images.AddImage(Image.FromFile(UIResourceDef.PATH_MACRO_ACTIVATE));
                //images.AddImage(Image.FromFile(UIResourceDef.PATH_MACRO_DEACTIVATE));
                //images.AddImage(Image.FromFile(UIResourceDef.PATH_MODULE_ACTIVATE));
                //images.AddImage(Image.FromFile(UIResourceDef.PATH_MODULE_DEACTIVATE));
                //images.AddImage(Image.FromFile(UIResourceDef.PATH_VARIANT_ACTIVATE));
                //images.AddImage(Image.FromFile(UIResourceDef.PATH_MACRO_CONTROL));
                rImageCombo.SmallImages = images;

                foreach (KeyValuePair<string, PsCCSDefineEnumSTDFuncIconType> step in dicStepNumWithType)
                    rImageCombo.Items.Add(new ImageComboBoxItem(step.Key, step.Key, (int)(step.Value)));

                rImageCombo.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near; //near : show text, Center
                rImageCombo.ReadOnly = true;
                rImageCombo.AllowDropDownWhenReadOnly = DefaultBoolean.False;
                rImageCombo.ShowDropDown = ShowDropDown.Never;

                return rImageCombo;
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to load icon for the grid control.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }

            return null;
        }

        public bool setInitGridAppearance(GridView gridView, RepositoryItemImageComboBox rImageComboTestList = null)
        {
            try
            {
                foreach (GridColumn col in gridView.Columns)
                {
                    col.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    col.OptionsColumn.AllowSort = DefaultBoolean.False;
                }
                gridView.OptionsView.EnableAppearanceEvenRow = true;
                if (rImageComboTestList != null)
                    gridView.Columns[(int)(PsCCSDefineEnumSTDFunctionSummary.STEP)].ColumnEdit = rImageComboTestList;

                gridView.BestFitColumns(true);

                gridView.Columns[(int)(PsCCSDefineEnumSTDFunctionSummary.STEP)].OptionsColumn.AllowEdit = false;
                gridView.Columns[(int)(PsCCSDefineEnumSTDFunctionSummary.STEP)].OptionsColumn.FixedWidth = true;
                return true;
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to initialize the appearance of the grid control / view.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
            return false;
        }

        public bool setInitGridColAppearance(GridControl gridCtr, GridView gridView)
        {
            try
            {
                UiCpAssistForDxApp.appendStringColumn(gridView, UiCpDefineEnumTsColumns.MSR.ToString()); // measure column.
                UiCpAssistForDxApp.appendStringColumn(gridView, UiCpDefineEnumTsColumns.RST.ToString()); // result column.
                UiCpAssistForDxApp.appendStringColumn(gridView, UiCpDefineEnumTsColumns.INF.ToString()); // info column.

                // check box column for the break-point.
                UiCpAssistForDxApp.appendCheckBoxColumn(gridCtr, gridView, UiCpDefineEnumTsColumns.BRK.ToString());

                // rearrange columns.
                UiCpAssistForDxApp.arrangeColumns(gridView);

                // hide columns by the user (UI).
                UiCpAssistForDxApp.setVisibleColumns(gridView, UiCpDefineEnumTsColumns.COMMENTS.ToString(), false);

                return true;
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to initialize the appearance of the grid control / view.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
            return false;
        }

        public bool setInitGridColUndboundData(GridControl gridCtr)
        {
            try
            {
                // unbound columns data
                m_dicUbBreakPointColumns.Clear();
                for (int i = 0; i < ((DataTable)(gridCtr.DataSource)).Rows.Count; i++)
                {
                    string strStepNum = ((DataTable)(gridCtr.DataSource)).Rows[i].ItemArray[0] as string;
                    ArrayList arUnboundData = new ArrayList(new string[] { string.Empty, string.Empty, string.Empty }); // measure, result, info
                    arUnboundData.Add(false); //break-point
                    m_dicUbBreakPointColumns.Add(Convert.ToInt32(strStepNum), arUnboundData);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to initialize the unbound data of the grid control / view.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
            return false;
        }

        public bool updateInitialGridViewAll(ClsCpTsManager cpTsManager)
        {
            try
            {
                // test list
                RepositoryItemImageComboBox rImageComboTestList = getInitGridIconResource(cpTsManager.GaudiReadData.getStepNumberWithType(), gridCtrTestSteps);                
                gridCtrTestSteps.DataSource = cpTsManager.GaudiReadData.GetDataTable();

                if (!setInitGridColAppearance(gridCtrTestSteps, gridViewTestSteps))
                {
                    UtilTextMessageEdits.UtilTextMsgToConsole("Failed to initialize the column appearance of the test-list grid control / view.", ConsoleColor.Red);
                    return false;
                }

                foreach (GridColumn col in gridViewTestSteps.Columns)
                {
                    if (col.FieldName == UiCpDefineEnumTsColumns.BRK.ToString())
                        continue;                    
                    col.OptionsColumn.ReadOnly = true;
                }

                if (!setInitGridColUndboundData(gridCtrTestSteps))
                {
                    UtilTextMessageEdits.UtilTextMsgToConsole("Failed to initialize the unbound data of the test-list grid control.", ConsoleColor.Red);
                    return false;
                }

                if (!setInitGridAppearance(gridViewTestSteps, rImageComboTestList))
                {
                    UtilTextMessageEdits.UtilTextMsgToConsole("Failed to initialize the appearance of the test-list grid control / view.", ConsoleColor.Red);
                    return false;
                }
                
                // module list
                PsCCSStdFnBase psSelStep = null;
                m_xMainFrame.MngSystem.MngTStep.GaudiReadData.GetTestStepData(1000, out psSelStep);
                if (psSelStep == null)
                {
                    UtilTextMessageEdits.UtilTextMsgToConsole("Failed to get selected step.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                    return false;
                }

                if (psSelStep.STDFuncType != PsCCSDefineEnumSTDFuncType.MACRO)
                    return false;

                PsCCSStdFnCtrMsBase psTsMsSelStep = (PsCCSStdFnCtrMsBase)psSelStep;
                if (psSelStep == null)
                    return false;

                gridCtrModuleSteps.DataSource = psTsMsSelStep.GetDataTable();
                if (!setInitGridAppearance(gridViewModuleSteps))
                {
                    UtilTextMessageEdits.UtilTextMsgToConsole("Failed to initialize the appearance of the test-list grid control / view.", ConsoleColor.Red);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to update the grid-views in the user control.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
            return false;
        }

        public bool updateGridViewForModuleBySelectedStep(int nSelStepNumber)
        {
            try
            {
                PsCCSStdFnBase psSelStep = null;
                m_xMainFrame.MngSystem.MngTStep.GaudiReadData.GetTestStepData(nSelStepNumber, out psSelStep);
                if (psSelStep == null)
                    return false;

                if (psSelStep.STDFuncType != PsCCSDefineEnumSTDFuncType.MACRO)
                {
                    DataTable dtModules = (DataTable)(gridCtrModuleSteps.DataSource);
                    if (dtModules != null)
                        dtModules.Rows.Clear();

                    return false;
                }

                PsCCSStdFnCtrMsBase psTsMsSelStep = (PsCCSStdFnCtrMsBase)psSelStep;
                if (psSelStep == null)
                    return false;

                gridCtrModuleSteps.DataSource = psTsMsSelStep.GetDataTable();              
                return true;
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to update the grid-views(module) in the user control.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
            return false;
        }

        // event functions.
        private void gridViewTestSteps_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (m_xMainFrame.MngSystem.MngTStep == null)
                    return;

                object objStepNum = gridViewTestSteps.GetRowCellValue(e.FocusedRowHandle, UiCpDefineEnumGridViewTestStep.STEP.ToString());
                int nSelStepNumber = Convert.ToInt32(objStepNum);
                if (nSelStepNumber >= 0)
                    updateGridViewForModuleBySelectedStep(nSelStepNumber);
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to update the grid-views in the user control.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }            
        }

        private void gridViewTestSteps_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                //if (m_xMainFrame.MngTStepThread.checkCrtState(ThreadCpStatus.eTsMainThread.MN_THREAD_RUN))
                //    return;

                //object objStepNum = gridViewTestSteps.GetRowCellValue(e.RowHandle, PsCCSDefineEnumSTDFunctionSummary.STEP.ToString());
                //if (objStepNum == null)
                //    return;

                //int nSelStep = Convert.ToInt32(objStepNum);

                ///* appearance for the step properties.*/
                //if (nSelStep >= 0)
                //{
                //    if (!DxGridUtil.setSelectedStepControlFnForeColor(m_xMainFrame.MngSystem.MngTStep.GaudiReadData.ListTestStep, nSelStep, e))
                //        UtilTextMessageEdit.UtilTextMsgToConsole("Failed to set the control function fore color in the grid view for the customized cell.", ConsoleColor.Red);

                //    if (!DxGridUtil.setSelectedStepDeactiveForeColor(m_xMainFrame.MngSystem.MngTStep.GaudiReadData.ListTestStep, nSelStep, e))
                //        UtilTextMessageEdit.UtilTextMsgToConsole("Failed to set deactivate fore color in the grid view for the customized cell.", ConsoleColor.Red);

                //    Color aForeColor = CommonSkins.GetSkin(gridCtrTestSteps.LookAndFeel).Colors.GetColor("DisabledText");
                //    if (!DxGridUtil.setSelectedStepVariantForeColor(m_xMainFrame.MngSystem.MngTStep.GaudiReadData.ListTestStep, nSelStep, m_xMainFrame.MngSystem.MngTStep.GaudiReadData.TestListInfo.Variant, aForeColor, e))
                //        UtilTextMessageEdit.UtilTextMsgToConsole("Failed to set the disabled fore color in the grid view for the customized cell.", ConsoleColor.Red);
                //}
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to update the grid view for the test step.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
        }

        private void gridViewTestSteps_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == UiCpDefineEnumTsColumns.BRK.ToString() && m_dicUbBreakPointColumns.Count > 0)
                {
                    // get test step number.

                    object objStepNum = gridViewTestSteps.GetRowCellValue(e.ListSourceRowIndex, PsCCSDefineEnumSTDFunctionSummary.STEP.ToString());
                    int nSelStep = Convert.ToInt32(objStepNum);

                    if (m_dicUbBreakPointColumns.ContainsKey(nSelStep))
                    {
                        ArrayList arUnboundData = m_dicUbBreakPointColumns[nSelStep];
                        if (e.IsGetData)
                            e.Value = (bool)(m_dicUbBreakPointColumns[nSelStep][(int)UiCpDefineEnumTsUnboundColumns.BRK]);
                        else
                            m_dicUbBreakPointColumns[nSelStep][(int)UiCpDefineEnumTsUnboundColumns.BRK] = e.Value;
                    }
                }
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to control the unbound data in the grid views.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
        }

        private void gridViewTestSteps_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Point pt = new Point(e.X, e.Y);
                GridHitInfo hit = gridViewTestSteps.CalcHitInfo(pt);

                if (hit.Column == null || hit.RowHandle < 0)
                    return;

                object objStepNum = gridViewTestSteps.GetRowCellValue(hit.RowHandle, PsCCSDefineEnumSTDFunctionSummary.STEP.ToString());               

                gridViewTestSteps.FocusedRowHandle = hit.RowHandle;

                if (e.Button == MouseButtons.Right)
                    popupMenuTStepView.ShowPopup(Control.MousePosition);
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to control the pop up event in the grid views.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
        }       
    
        //private void barEditItemSearchStep_EditValueChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        BarEditItem crtItem = (BarEditItem)(sender);
        //        if (crtItem != null)
        //        {
        //            string strTStep = crtItem.EditValue.ToString();
        //            setGridViewTestStepByStepNumber(Convert.ToInt32(strTStep));
        //            crtItem.EditValue = null;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        UtilTextMessageEdits.UtilTextMsgToConsole("Failed to control the pop-up event in the grid views.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
        //        UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
        //    }
        //}

        void repositoryItemSearchControl2_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode.Equals(Keys.Enter))
                {
                    setGridViewTestStepByStepNumber(SearchIndex);
                    popupMenuTStepView.Manager.CloseMenus();
                }
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to control the pop-up event in the grid views.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
        }

        void repositoryItemSearchControl2_EditValueChanging(object sender, ChangingEventArgs e)
        {
            try
            {
                //UtilTextMessageEdits.UtilTextMsgToConsole(e.OldValue.ToString() + " , " + e.NewValue.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
                if (e.NewValue != null)
                {
                    int nTstep = -1;
                    if (int.TryParse(e.NewValue.ToString(), out nTstep))
                        SearchIndex = nTstep;
                }
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to control the pop-up event in the grid views.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
        }

        public void setGridViewTestStepByStepNumber(int nStep)
        {
            try
            {
                DataTable dtTestStep = (DataTable)(gridCtrTestSteps.DataSource);
                int index = dtTestStep.Rows.IndexOf(dtTestStep.Rows.Find(nStep));
                gridViewTestSteps.FocusedRowHandle = gridViewTestSteps.GetRowHandle(-1);
                gridViewTestSteps.FocusedRowHandle = gridViewTestSteps.GetRowHandle(index);
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to change the focus of the row by the step number.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
        }

        private void barButtonItemUncheckAllSteps_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {                
                for (int i = 0; i < m_dicUbBreakPointColumns.Count; i++)
                {
                    int nStepNum = m_xMainFrame.MngSystem.MngTStep.getTStepByIndex(i).Core.StepNum;
                    if ((bool)(m_dicUbBreakPointColumns[nStepNum][(int)UiCpDefineEnumTsUnboundColumns.BRK]) == true)
                        m_dicUbBreakPointColumns[nStepNum][(int)UiCpDefineEnumTsUnboundColumns.BRK] = false;
                }

                gridCtrTestSteps.RefreshDataSource();
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to un-check all steps in the grid.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
        }
    }
}
