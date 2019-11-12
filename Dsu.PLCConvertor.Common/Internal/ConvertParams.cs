using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// PLC 변환에 사용될 parameters
    /// </summary>
    public class ConvertParams
    {
        /// <summary>
        /// 원본 PLC type
        /// </summary>
        public PLCVendor SourceType { get; private set; }
        /// <summary>
        /// target PLC type
        /// </summary>
        public PLCVendor TargetType { get; private set; }
        /// <summary>
        /// Source PLC 의 시작 step.  Message txt file 에 source step 을 기록하기 위해서 사용
        /// 직접 file 에 write 하지 않는 이유는 추후 병렬화 등을 고려해서 임
        /// </summary>
        public int SourceStartStep { get; set; }
        /// <summary>
        /// Target PLC 의 시작 step.  Message txt file 에 source step 을 기록하기 위해서 사용
        /// 직접 file 에 write 하지 않는 이유는 추후 병렬화 등을 고려해서 임
        /// </summary>
        public int TargetStartStep { get; set; }


        /// <summary>
        /// Source 측 PLC 변수.  Device comment 및 type 등의 정보가 담겨 있다.
        /// </summary>
        public static Dictionary<string, PLCVariable> SourceVariableMap { get; internal set; }
        /// <summary>
        /// 변환에 실제 사용된 source PLC 의 device address 들
        /// </summary>
        public static Dictionary<string, PLCVariable> UsedSourceDevices { get; } = new Dictionary<string, PLCVariable>();

        /// <summary>
        /// 하나의 section/program 에 대한 변환이 종료되면, source/target 의 시작 step 을 reset 함
        /// </summary>
        public void ResetStartStep()
        {
            SourceStartStep = 0;
            TargetStartStep = 0;
        }

        /// <summary>
        /// 강제로 section 에 의해서 구분할 지의 여부.
        /// </summary>
        public bool SplitBySection { get; set; } = true;

        public ConvertParams(PLCVendor sourceType, PLCVendor targetType, int soruceStartStep=0, int targetStartStep=0)
        {
            SourceType = sourceType;
            TargetType = targetType;
            SourceStartStep = soruceStartStep;
            TargetStartStep = targetStartStep;
        }
    }
}
