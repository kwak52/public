using DevExpress.XtraBars;
using Dsa.Kefico.PDV.Enumeration;
using Dsa.Kefico.PDV.Forms;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System;
using DevExpress.XtraBars.Docking2010.Views;
using System.IO;
using System.Diagnostics;
using static ActorMessages;

namespace Dsa.Kefico.PDV
{
    public partial class frmMain
    {
        private void barButtonItem_NewEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CreateEntryForm(true);
        }
        private void barButtonItem_EditEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CreateEntryForm(false, GetGrid(ViewPDV.selectPdvGroup)?.RowHandleFilterSelect);
        }
        private void barButtonItem_TestList_ItemClick(object sender, ItemClickEventArgs e)
        {
            CreateTestListForm(true);
        }
        private void barButtonItem_EditTestList_ItemClick(object sender, ItemClickEventArgs e)
        {
            CreateTestListForm(false, GetGrid(ViewPDV.selectPdvTestList)?.RowHandleFilterSelect);

        }
        private void barButtonItem_NewRelease_ItemClick(object sender, ItemClickEventArgs e)
        {
            CreateReleaseForm(true);
        }
        private void barButtonItem_EditRelease_ItemClick(object sender, ItemClickEventArgs e)
        {
            CreateReleaseForm(false, GetGrid(ViewPDV.selectPdvView)?.RowHandleFilterSelect);
        }
     

        private void barButtonItem_Copy_ItemClick(object sender, ItemClickEventArgs e)
        {
            BaseDocument document = null;
            if (docMWS.GetDocument(ViewPDV.selectPdvGroup, out document))
            {
                if (tabbedView.ActiveDocument == document)
                    CreateEntryForm(true, GetGrid(ViewPDV.selectPdvGroup)?.RowHandleFilterSelect);
            }
            if (docMWS.GetDocument(ViewPDV.selectPdvTestList, out document))
            {
                if (tabbedView.ActiveDocument == document)
                    CreateTestListForm(true, GetGrid(ViewPDV.selectPdvTestList)?.RowHandleFilterSelect);
            }
            if (docMWS.GetDocument(ViewPDV.selectPdvView, out document))
            {
                if (tabbedView.ActiveDocument == document)
                    CreateReleaseForm(true, GetGrid(ViewPDV.selectPdvView)?.RowHandleFilterSelect);
            }
        }

        private void barButtonItem_Delete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!UserCheck()) return;

            BaseDocument document = null;
            if (docMWS.GetDocument(ViewPDV.selectPdvGroup, out document))
            {
                if (tabbedView.ActiveDocument == document)
                {
                    DataRow dr = GetGrid(ViewPDV.selectPdvGroup)?.RowHandleFilterSelect;
                    if (dr == null) return;

                    string sql = viewQuery.DeletePdvGroup(Convert.ToInt32(dr[SchemaPDV.PDVGROUP_id]));
                    if (mySqlMWS.Execute(sql))
                    {
                        WriteOutput("Entry", string.Format("Entry delete successful {0} {1}", dr[SchemaPDV.PDVGROUP_ProductGroup], dr[SchemaPDV.PDVGROUP_ProductModel]));
                        OpenDocument(ViewPDV.selectPdvGroup);
                    }
                }
            }
            if (docMWS.GetDocument(ViewPDV.selectPdvTestList, out document))
            {
                if (tabbedView.ActiveDocument == document)
                {
                    DataRow dr = GetGrid(ViewPDV.selectPdvTestList)?.RowHandleFilterSelect;
                    if (dr == null) return;

                    string sql = viewQuery.DeletePdvTestList(Convert.ToInt32(dr[SchemaPDV.PDVTESTLIST_id]));
                    if (mySqlMWS.Execute(sql))
                    {
                        WriteOutput("TestList", string.Format("TestList delete successful {0} {1} {2} {3}"
                            , dr[SchemaPDV.PDVTESTLIST_productNumber]
                            , dr[SchemaPDV.PDVTESTLIST_product]
                            , dr[SchemaPDV.PDVTESTLIST_productType]
                            , dr[SchemaPDV.PDVTESTLIST_variant]));
                        OpenDocument(ViewPDV.selectPdvTestList);
                    }
                }
            }
            if (docMWS.GetDocument(ViewPDV.selectPdvView, out document))
            {
                if (tabbedView.ActiveDocument == document)
                {
                    DataRow dr = GetGrid(ViewPDV.selectPdvView)?.RowHandleFilterSelect;
                    if (dr == null) return;

                    string sql = viewQuery.DeletePdv(Convert.ToInt32(dr[SchemaPDV.PDVVIEW_id]));
                    if (mySqlMWS.Execute(sql))
                    {
                        WriteOutput("Release", string.Format("Pdv delete successful {0} {1} {2} {3}"
                            , dr[SchemaPDV.PDVVIEW_partNumber]
                            , dr[SchemaPDV.PDVVIEW_pamGroup]
                            , dr[SchemaPDV.PDVVIEW_pamType]
                            , dr[SchemaPDV.PDVVIEW_testList]));
                        OpenDocument(ViewPDV.selectPdvView);
                    }
                }
            }
        }

        private bool UserCheck()
        {
//             if (UserId < 0)  //test ahn
//             {
//                 DataTable dtView = mySqlMWS.GetDataFromDBView(viewQuery.SelectUser(), ViewPDV.selectUser);
//                 FrmUser f = new FrmUser(dtView);
//                 if (f.ShowDialog() == DialogResult.OK)
//                     UserId = f.UserId;
//             }
// 
//             if (UserId < 0)
//                 return false;
//             else
                return true;
        }


        private void FrmRelease_UEventNewRelease(object sender)
        {
            CreateTestListForm(true);
        }


        private void CreateEntryForm(bool bNew, DataRow dr = null)
        {
            if (!UserCheck()) return;
          
            if (frmEntry == null || frmEntry.IsDisposed)
            {
                DataTable dt = GetDataTable(ViewPDV.selectPdvGroup);
                //if (dt.Rows.Count == 0) return;
                int LastID = mySqlMWS.GetLastID(ViewPDV.selectPdvGroup, SchemaPDV.PDVGROUP_id);
                int id = bNew ? LastID + 1 : Convert.ToInt32(dr[SchemaPDV.PDVGROUP_id]);
                List<string> lstGroup = GetDataList(dt, SchemaPDV.PDVGROUP_ProductGroup);
                List<string> lstProduct = GetDataList(dt, SchemaPDV.PDVGROUP_ProductModel);

                frmEntry = new FrmEntry(bNew, id, lstGroup, lstProduct, dr);
                frmEntry.FormClosing += FrmEntry_FormClosing;
            }
            else
            {
                frmEntry.Focus();
                return;
            }

            frmEntry.Owner = this;
            frmEntry.Show();
        }

    

        private void CreateTestListForm(bool bNew, DataRow dr = null)
        {
            if (!UserCheck()) return;
            if (frmTestList == null || frmTestList.IsDisposed)
            {
                DataTable dt = GetDataTable(ViewPDV.selectPdvTestList);
                //if (dt.Rows.Count == 0) return;
                int LastID = mySqlMWS.GetLastID(ViewPDV.selectPdvTestList, SchemaPDV.PDVTESTLIST_id);
                int id = bNew ? LastID + 1 : Convert.ToInt32(dr[SchemaPDV.PDVTESTLIST_id]);

                List<string> lstProductNumber = GetDataList(dt, SchemaPDV.PDVTESTLIST_productNumber);
                List<int> lstVariant = GetDataListInt(dt, SchemaPDV.PDVTESTLIST_variant);
                List<string> lstproduct = GetDataList(dt, SchemaPDV.PDVTESTLIST_product);
                List<string> lstproductType = GetDataList(dt, SchemaPDV.PDVTESTLIST_productType);
                List<string> lstfileStem = GetDataList(dt, SchemaPDV.PDVTESTLIST_fileStem);
                frmTestList = new FrmTestList(bNew, id, lstProductNumber, lstproduct, lstproductType, lstVariant, lstfileStem,  dr);
                frmTestList.FormClosing += FrmTestList_FormClosing;
            }
            else
            {
                frmTestList.Focus();
                return;
            }

            frmTestList.Owner = this;
            frmTestList.Show();
        }


        private void CreateReleaseForm(bool bNew, DataRow dr = null)
        {
            if (!UserCheck()) return;
            if (frmRelease == null || frmRelease.IsDisposed)
            {
                DataTable dt = GetDataTable(ViewPDV.selectPdvView);
                // if (dt.Rows.Count == 0) return;
                int LastID = mySqlMWS.GetLastID(ViewPDV.selectPdvView, SchemaPDV.PDVVIEW_id);
                int id = bNew ? LastID + 1 : Convert.ToInt32(dr[SchemaPDV.PDVVIEW_id]);

                DataTable dtGroup = GetDataTable(ViewPDV.selectPdvGroup);
                DataTable dtTestList = GetDataTable(ViewPDV.selectPdvTestList);
                List<string> lstTestList = GetTestList(dtTestList);
                List<string> lstGroup = GetGroup(dtGroup);
                List<string> lstPartNumber = GetDataList(dt, SchemaPDV.PDVVIEW_partNumber);
                List<string> lstProductNumber = GetDataList(dtTestList, SchemaPDV.PDVTESTLIST_productNumber);

                List<string> lstPamGroup = GetDataList(dt, SchemaPDV.PDVVIEW_pamGroup);
                List<string> lstPamType = GetDataList(dt, SchemaPDV.PDVVIEW_pamType);
                List<string> lstDataConfig = GetDataList(dt, SchemaPDV.PDVVIEW_dataConfig);
                List<string> lstDataVariant = GetDataList(dt, SchemaPDV.PDVVIEW_dataVariant);

                frmRelease = new FrmRelease(bNew, id, lstGroup, lstTestList, lstProductNumber, lstPartNumber, lstPamGroup, lstPamType, lstDataConfig, lstDataVariant, dr);
                frmRelease.FormClosing += FrmRelease_FormClosing;
                frmRelease.UEventNewRelease += FrmRelease_UEventNewRelease;
            }
            else
            {
                frmRelease.Focus();
                return;
            }

            frmRelease.Owner = this;
            frmRelease.Show();
        }

      

        private void FrmEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (frmEntry.DialogResult == DialogResult.OK)
            {
                string sql;
                if (frmEntry.New)
                    sql = viewQuery.InsertPdvGroup(frmEntry.PdvGroupId, frmEntry.ProductGroup, frmEntry.Product, frmEntry.Comment);
                else
                    sql = viewQuery.UpdatePdvGroup(frmEntry.PdvGroupId, frmEntry.ProductGroup, frmEntry.Product, frmEntry.Comment);

                if (!mySqlMWS.Execute(sql))
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    WriteOutput("Entry", frmEntry.New ? "Data add successful" : "Data change successful");
                    OpenDocument(ViewPDV.selectPdvView);
                    UpdateDocument(ViewPDV.selectPdvGroup, SchemaPDV.PDVGROUP_id, frmEntry.PdvGroupId);
                    if (frmRelease != null && !frmRelease.IsDisposed)
                        frmRelease.UpdateEntry(GetGroup(GetDataTable(ViewPDV.selectPdvGroup)));
                }

            }
        }

        private void FrmTestList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (frmTestList.DialogResult == DialogResult.OK)
            {
                string sql;
                if (frmTestList.New)
                    sql = viewQuery.InsertPdvTestList(frmTestList.ProductNumber, frmTestList.Product, frmTestList.ProductType, frmTestList.Variant, frmTestList.FileStem, frmTestList.Comment);
                else
                    sql = viewQuery.UpdatePdvTestList(frmTestList.PdvTestListId, frmTestList.FileStem, frmTestList.Comment);

                if (!mySqlMWS.Execute(sql))
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    var response = _pdvManager?.Ask(new AmRequestCreateFolder(frmTestList.ProductNumber, frmTestList.Product, frmTestList.ProductType));
                    WriteOutput("TestList", frmTestList.New ? "Data add successful " : "Data change successful");
                    OpenDocument(ViewPDV.selectPdvView);
                    UpdateDocument(ViewPDV.selectPdvTestList, SchemaPDV.PDVTESTLIST_id, frmTestList.PdvTestListId);
                    if (frmRelease != null && !frmRelease.IsDisposed)
                    {
                        DataTable dtTestList = GetDataTable(ViewPDV.selectPdvTestList);
                        List<string> lstTestList = GetTestList(dtTestList);
                        List<string> lstProductNumber = GetDataList(dtTestList, SchemaPDV.PDVTESTLIST_productNumber);
                        frmRelease.UpdateTestList(lstTestList, lstProductNumber, frmTestList.ProductNumber);
                    }
                }
            }
        }

        private void FrmRelease_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (frmRelease.DialogResult == DialogResult.OK)
            {
                DataTable dt = GetDataTable(ViewPDV.selectPdvTestList);
                string ProductNumber = (string)GetData(dt, SchemaPDV.PDVTESTLIST_productNumber, frmRelease.TestListId);
                string Product = (string)GetData(dt, SchemaPDV.PDVTESTLIST_product, frmRelease.TestListId);
                string ProductType = (string)GetData(dt, SchemaPDV.PDVTESTLIST_productType, frmRelease.TestListId);
                int Variant = (int)GetData(dt, SchemaPDV.PDVTESTLIST_variant, frmRelease.TestListId);
                string sql;
                if (frmRelease.New)
                    sql = viewQuery.InsertPdv(frmRelease, UserId);
                else
                    sql = viewQuery.UpdatePdv(frmRelease);

                if (!mySqlMWS.Execute(sql))
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    WriteOutput("Release", frmRelease.New ? "Data add successful" : "Data change successful");
                    UpdateDocument(ViewPDV.selectPdvView, SchemaPDV.PDVVIEW_id, frmRelease.PdvId);
                }
            }
        }
    }
}