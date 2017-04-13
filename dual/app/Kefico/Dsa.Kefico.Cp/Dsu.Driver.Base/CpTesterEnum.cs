using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;


namespace Dsu.Driver.Base
{
    public enum CpTesterEnum
    {
        Undefined = 0,
        L_7DCT,         // Line 7속 Gunpo
        L_8FF,          // Line 8속 Vietnam
        A_78KVP,        // Audit 78속 Vietnam
        A_78KGP,        // Audit 78속 Gunpo
        A_GCVT,         // Audit Gamma CVT
    }

    public static class DriverBaseGlobals
    {
        /// Tester Hardware Identifications
        public static CpTesterEnum TesterType { get; set; }
        public static bool IsAudit78() => TesterType == CpTesterEnum.A_78KGP || TesterType == CpTesterEnum.A_78KVP;
        public static bool IsAuditGCVT() => TesterType == CpTesterEnum.A_GCVT;
        public static bool IsAudit() => IsAudit78() || IsAuditGCVT();
        public static bool IsLine() => TesterType == CpTesterEnum.L_7DCT || TesterType == CpTesterEnum.L_8FF;
        /// Vietnam
        public static bool IsLine8FF() => TesterType == CpTesterEnum.L_8FF;
        public static bool IsLine7DCT() => TesterType == CpTesterEnum.L_7DCT;

        /// driver 초기화 시 수행할 작업 지정
        public static Subject<object> DriverResetSubject = new Subject<object>();
    }
}


