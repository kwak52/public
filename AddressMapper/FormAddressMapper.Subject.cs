using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Dsu.PLCConverter.UI;
using Dsu.PLCConverter.UI.AddressMapperLogics;
using Dsu.PLCConvertor.Common;
using System;
using System.Linq;
using System.Reactive.Subjects;

namespace AddressMapper
{
    public enum PLCType
    {
        Omron,
        Xg5k,
    }
    partial class FormAddressMapper
    {
        void InitializeSubjects()
        {
            /// 메모리 타입 변경시 호출 됨
            Subjects.MemorySectionChangeRequestSubject.Subscribe(tpl =>
            {
                var plcType = tpl.Item1;
                var memType = tpl.Item2;
                if (plcType == PLCVendor.Omron)
                    lookUpEditOmronMemory.EditValue = _mapping.OmronPLC.Memories.FirstOrDefault(m => m.Name == memType);
                else if (plcType == PLCVendor.LSIS)
                    lookUpEditXg5kMemory.EditValue = _mapping.Xg5kPLC.Memories.FirstOrDefault(m => m.Name == memType);
            });

            /// 두 PLC 간 기종 변경시 호출 됨
            Subjects.PLCMappingChangeRequestSubject.Subscribe(newMapping =>
            {
                _logger.Info("PLC types changed.");
                Clear();

                lookUpEditOmronMemory.Properties.DataSource = null;
                lookUpEditOmronMemory.EditValue = null;
                lookUpEditXg5kMemory.Properties.DataSource = null;
                lookUpEditXg5kMemory.EditValue = null;

                lookUpEditOmronMemory.Properties.DataSource = newMapping.OmronPLC.Memories;
                lookUpEditOmronMemory.EditValue = newMapping.OmronPLC.Memories[0];
                lookUpEditXg5kMemory.Properties.DataSource = newMapping.Xg5kPLC.Memories;
                lookUpEditXg5kMemory.EditValue = newMapping.Xg5kPLC.Memories[0];

                barEditItemOmronPLC.EditValue = newMapping.OmronPLC;
                barEditItemXg5kPLC.EditValue = newMapping.Xg5kPLC;
            });
        }

        void Clear()
        {
            _mapping.OmronPLC.Clear();
            _mapping.Xg5kPLC.Clear();
            ucMemoryBarOmron.DrawRanges();
            ucMemoryBarXg5k.DrawRanges();
            gridControlRanged.DataSource = null;
            gridControlOneToOne.DataSource = null;
        }

        /// <summary>
        /// 기종 변경
        /// </summary>
        void PLCChanged(object sender1, EventArgs args1, PLCVendor vendor)
        {
            OmronPLC omron = null;
            Xg5kPLC xg5k = null;

            var edit = sender1 as LookUpEdit;
            if (edit != null)
            {
                if (vendor == PLCVendor.Omron)
                {
                    omron = (OmronPLC)edit.EditValue;
                    xg5k = (Xg5kPLC)barEditItemXg5kPLC.EditValue;
                }
                else if (vendor == PLCVendor.LSIS)
                {
                    omron = (OmronPLC)barEditItemOmronPLC.EditValue;
                    xg5k = (Xg5kPLC)edit.EditValue;
                }
            }
            if (omron != null && xg5k != null)
                Mapping = new PLCMapping(omron, xg5k);
        }

    }
}
