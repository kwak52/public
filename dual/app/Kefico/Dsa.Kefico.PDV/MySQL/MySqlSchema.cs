
namespace Dsa.Kefico.PDV
{
    public static class SchemaPDV
    {
        public const string TSV_id = "id";
        public const string TSV_ccsId = "_ccsId";
        public const string TSV_pdvId = "_pdvId";
        public const string TSV_isFromDynamic = "_isFromDynamic";
        public const string TSV_batchName = "_batchName";

        public const string SSV_positionId = "_positionId";
        public const string SSV_min = "min";
        public const string SSV_max = "max";

        public const string PSV_startDateTime = "startDateTime";
        public const string PSV_startDay = "_startDay";
        public const string PSV_startTime = "time";
        public const string PSV_value = "value";
        public const string PSV_id = "id";
        public const string PSV_measureid = "_measureId";

        public const string BV_positionId = "_positionId";
        public const string BV_displayType = "_displayType";

        public const string SPC_startDateTime = "startDateTime";
        public const string SPC_host = "host";
        public const string SPC_partNumber = "partNumber";
        public const string SPC_position = "position";
        public const string SPC_step = "step";
        public const string SPC_modName = "modName";
        public const string SPC_min = "min";
        public const string SPC_max = "max";
        public const string SPC_dim = "dim";

        public const string MV_startDay = "startDay";
        public const string MV_batchName = "batchName";

        public const string PDVGROUP_id = "id";
        public const string PDVGROUP_ProductGroup = "ProductGroup";
        public const string PDVGROUP_ProductModel = "ProductModel";
        public const string PDVGROUP_Comment = "comment";

        public const string PDVTESTLIST_id = "id";
        public const string PDVTESTLIST_product = "product";
        public const string PDVTESTLIST_productType = "productType";
        public const string PDVTESTLIST_productNumber = "productNumber";
        public const string PDVTESTLIST_variant = "version";
        public const string PDVTESTLIST_createdDt = "createdDt";
        public const string PDVTESTLIST_pdvGroupId = "pdvGroup";
        public const string PDVTESTLIST_fileStem = "fileStem";
        public const string PDVTESTLIST_Comment = "comment";

        public const string PDV_id = "id";
        public const string PDV_createdDt = "createdDt";
        public const string PDVVIEW_id = "_id";
        public const string PDVVIEW_groupEntry = "groupEntry";
        public const string PDVVIEW_testList = "testList";
        public const string PDVVIEW_partNumber = "partNumber";
        public const string PDVVIEW_pamGroup = "pamGroup";
        public const string PDVVIEW_pamType = "pamType";
        public const string PDVVIEW_dataConfig = "dataConfig";
        public const string PDVVIEW_dataVariant = "dataVariant";
        public const string PDVVIEW_Comment = "comment";
        public const string PDVVIEW_ChangeNumber = "changeNumber";
        public const string PDVVIEW_UserId = "userId";
    }
}
