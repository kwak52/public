using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    public class ConvertParams
    {
        public PLCVendor SourceType { get; private set; }
        public PLCVendor TargetType { get; private set; }
        public int SourceStartStep { get; set; }
        public int TargetStartStep { get; set; }

        public void ResetStartStep()
        {
            SourceStartStep = 0;
            TargetStartStep = 0;
        }

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
