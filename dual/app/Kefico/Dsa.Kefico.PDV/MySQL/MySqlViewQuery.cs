using System;
using Dsa.Kefico.PDV.Enumeration;
using Dsa.Kefico.PDV.Forms;

namespace Dsa.Kefico.PDV
{
    public class ViewQuery
    {

        public ViewQuery()
        {
        }

        public string Query(FilterPDV filterMWS, string Views)
        {
            string viewQuery = string.Empty; ;

            if (Views == ViewPDV.selectPdvView)
                viewQuery = ViewSelectPdv(filterMWS);
            else if (Views == ViewPDV.selectPdvGroup)
                viewQuery = ViewSelectPdvGroup(filterMWS);
            else if (Views == ViewPDV.selectPdvTestList)
                viewQuery = ViewSelectPdvTestList(filterMWS);

            return viewQuery;
        }

        public string SelectUser()
        {
            string SQL = string.Format("select *  from {0}", ViewPDV.selectUser);

            return SQL;
        }


        public string UpdatePdvGroup(int id, string group, string product, string comment)
        {
            string SQL = string.Format("update pdvGroup set  {0}='{1}', {2}='{3}', {4}='{5}' where {6}='{7}'"
                , SchemaPDV.PDVGROUP_ProductGroup, group
                , SchemaPDV.PDVGROUP_ProductModel, product
                , SchemaPDV.PDVGROUP_Comment, comment
                , SchemaPDV.PDVGROUP_id, id);

            return SQL;
        }

        public string InsertPdvGroup(int id, string group, string product, string comment)
        {
            string SQL = string.Format("insert into pdvGroup values ({0}, '{1}', '{2}', '{3}')", id, group, product, comment);

            return SQL;
        }


        public string DeletePdvGroup(int id)
        {
            string SQL = string.Format("delete from pdvGroup where {0}={1}", SchemaPDV.PDVGROUP_id, id);

            return SQL;
        }

        public string InsertPdvTestList(string partNumber, string product, string productType, int variant, string fileStem, string comment)
        {
            string SQL = string.Format("insert into pdvTestList({0}, {2}, {4}, {6}, {8}, {10}, {12}) values ('{1}', '{3}', '{5}', {7}, '{9}', '{11}', '{13}')"
              , SchemaPDV.PDVTESTLIST_productNumber, partNumber
              , SchemaPDV.PDVTESTLIST_product, product
              , SchemaPDV.PDVTESTLIST_productType, productType
              , SchemaPDV.PDVTESTLIST_variant, variant
              , SchemaPDV.PDVTESTLIST_createdDt, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
              , SchemaPDV.PDVTESTLIST_fileStem, fileStem
              , SchemaPDV.PDVTESTLIST_Comment, comment);

            return SQL;
        }

        public string UpdatePdvTestList(int id, string fileStem, string comment)
        {
            string SQL = string.Format("update pdvTestList set {0}={1}, {2}='{3}' where {4}='{5}'"
           , SchemaPDV.PDVTESTLIST_fileStem, fileStem
           , SchemaPDV.PDVTESTLIST_Comment, comment
           , SchemaPDV.PDVTESTLIST_id, id);

            return SQL;
        }

        public string DeletePdvTestList(int id)
        {
            string SQL = string.Format("delete from pdvTestList where {0}={1}", SchemaPDV.PDVTESTLIST_id, id);

            return SQL;
        }

        public string InsertPdv(FrmRelease f, int userId)
        {
            string SQL = string.Format("insert into pdv({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})"
                , SchemaPDV.PDV_id
                , "groupId"
                , "testListId"
                , "userId"
                , SchemaPDV.PDVVIEW_partNumber
                , SchemaPDV.PDVVIEW_pamType
                , SchemaPDV.PDVVIEW_pamGroup
                , "isProduction"
                , "createdDt"
                , "revision"
                , "comment"
                , SchemaPDV.PDVVIEW_dataConfig
                , SchemaPDV.PDVVIEW_dataVariant
                , SchemaPDV.PDVVIEW_ChangeNumber);

            SQL += string.Format("values({0},{1},{2},{3},'{4}','{5}','{6}',{7},'{8}',{9},'{10}','{11}','{12}','{13}')"
               , f.PdvId
               , f.ProductGroupId
               , f.TestListId
               , userId
               , f.PartNumber
               , f.PamType
               , f.PamGroup
               , false
               , DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
               , 1
               , f.Comment
               , f.DataConfig
               , f.DataVariant
               , f.ChangeNumber);
            return SQL;
        }

        public string UpdatePdv(FrmRelease f)
        {
            string SQL = string.Format("update pdv set {0}={1},{2}={3},{4}='{5}',{6}='{7}',{8}='{9}',{10}='{11}',{12}='{13}',{14}='{15}'"
                , "groupId", f.ProductGroupId
                , "testListId", f.TestListId
                , SchemaPDV.PDVVIEW_pamGroup, f.PamGroup
                , SchemaPDV.PDVVIEW_pamType, f.PamType
                , "comment", f.Comment
                , SchemaPDV.PDVVIEW_dataConfig, f.DataConfig
                , SchemaPDV.PDVVIEW_dataVariant, f.DataVariant
                , SchemaPDV.PDVVIEW_ChangeNumber, f.ChangeNumber);

            SQL += string.Format("where {0}={1}"
                , SchemaPDV.PDV_id, f.PdvId);

            return SQL;
        }

        public string DeletePdv(int id)
        {
            string SQL = string.Format("delete from pdv where {0}={1}", SchemaPDV.PDV_id, id);

            return SQL;
        }

        private string ViewSelectPdv(FilterPDV f)
        {
            string Function = string.Format("select * from {0} where {1}", ViewPDV.selectPdvView, f.GetDayWhere());

            return Function;
        }

        private string ViewSelectPdvGroup(FilterPDV filterMWS)
        {
            string Function = "select  * from " + ViewPDV.selectPdvGroup;
            // string SQL = string.Format("{0}({1})", Function, filterMWS.GetSummary());
            return Function;
        }

        private string ViewSelectPdvTestList(FilterPDV filterMWS)
        {
            string Function = "select  * from " + ViewPDV.selectPdvTestList;
            // string SQL = string.Format("{0}({1})", Function, filterMWS.GetSummary());
            return Function;
        }
    }
}
