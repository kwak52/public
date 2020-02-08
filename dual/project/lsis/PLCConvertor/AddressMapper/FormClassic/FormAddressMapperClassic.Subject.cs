using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Dsu.PLCConverter.UI;
using Dsu.PLCConverter.UI.AddressMapperLogics;
using Dsu.PLCConvertor.Common;
using System;
using System.Diagnostics;
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
        public static Subject<PLCHWSpecs> PLCHWSpecsChangeRequestSubject = new Subject<PLCHWSpecs>();

        void InitializeSubjects()
        {
            /// 메모리 타입 변경시 호출 됨
            Subjects.MemorySectionChangeRequestSubject.Subscribe((Action<Tuple<PLCVendor, string>>)(tpl =>
            {
                var plcType = tpl.Item1;
                var memType = tpl.Item2;
                if (plcType == PLCVendor.Omron)
                    lookUpEditOmronMemory.EditValue = Enumerable.FirstOrDefault<OmronMemorySection>(_mapping.OmronPLC.Memories, (Func<OmronMemorySection, bool>)(m => (bool)(m.Name == memType)));
                else if (plcType == PLCVendor.LSIS)
                    lookUpEditXg5kMemory.EditValue = _mapping.Xg5kPLC.Memories.FirstOrDefault(m => m.Name == memType);
            }));

            /// 두 PLC 간 기종 변경시 호출 됨
            Subjects.PLCMappingChangeRequestSubject.Subscribe((Action<PLCMapping>)(newMapping =>
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

                barEditItemTextOmron.EditValue = newMapping.OmronPLC.PLCType;
                barEditItemTextXg5k.EditValue = newMapping.Xg5kPLC.PLCType;
            }));

            // PLC H/W 설정 파일을 새로 loading 했을 때
            PLCHWSpecsChangeRequestSubject.Subscribe(hwSpecs => {
                Debug.Assert(hwSpecs == PLCHWSpecs);
                repositoryItemLookUpEditOmron.DataSource = hwSpecs.OmronPLCs;
                repositoryItemLookUpEditXg5k.DataSource = hwSpecs.XG5000PLCs;

                Mapping = new PLCMapping(hwSpecs.OmronPLCs[0], hwSpecs.XG5000PLCs[0]);
            });
        }

        void Clear()
        {
            _mapping.OmronPLC.Clear();
            _mapping.Xg5kPLC.Clear();
            ucMemoryBarOmron.DrawRanges();
            ucMemoryBarXg5k.DrawRanges();
            _rangeMappings.Clear();
            _oneToOneMappings.Clear();
        }
    }
}
