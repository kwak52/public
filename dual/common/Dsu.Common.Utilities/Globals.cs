using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;

namespace Dsu.Common.Utilities
{
    public static class Globals
    {
        public static log4net.ILog Logger { get; set; }
        public static Subject<bool> ApplicationExitSubject = new Subject<bool>();

        private static bool _isDeveloperMode;
        public static bool _isSkipLoadLayout { get; set; }
        public static bool IsDeveloperMode
        {
            get { return _isDeveloperMode; }
            set
            {
                _isDeveloperMode = value;
                DeveloperModeChangedSubject.OnNext(value);
            }
        }
        public static Subject<bool> DeveloperModeChangedSubject = new Subject<bool>();
    }
}
