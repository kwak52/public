using System;

namespace Dsu.LicenseKeyChecker
{
    [Serializable]
    internal class CLicenseInfo : IDisposable
    {

        public CLicenseInfo()
        {

        }

        public void Dispose()
        {            
            
        }

       
		internal bool IsLicensed { get; set; }
        internal string Product { get; set; }
		internal string ActivationCode { get; set; }
		internal string ActivationKey { get; set; }
        internal string ProcessModel { get; set; }
        internal string ProcessID { get; set; }
        internal string HardDiskModel { get; set; }
        internal string HardDiskID { get; set; }
        internal string NetworkModel { get; set; }
        internal string MacAddress { get; set; }
        internal bool IsDemo { get; set; }
        internal DateTime LicensedTime { get; set; }
        internal DateTime ExcuteTime { get; set; }
        internal int DemoDays { get; set; }
        internal int DemoHours { get; set; }
        internal double RemainsMinutes { get; set; }
        internal int UsingDays { get; set; }

    }
}
