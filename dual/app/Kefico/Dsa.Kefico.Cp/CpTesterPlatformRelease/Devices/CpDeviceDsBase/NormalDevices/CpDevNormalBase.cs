using CpTesterPlatform.CpCommon;
using System;
using System.Reflection;
using System.Threading;

namespace CpTesterPlatform.CpDevices
{
    public abstract class CpDevNormalBase
    {
        public CpFunctionEventHandler FuncEvtHndl { get; set; }
        public string DeviceID { get; set; }
        protected CancellationTokenSource _cts;


        public virtual bool DevOpen() { throw new NotImplementedException(); }
        public virtual bool DevClose() { throw new NotImplementedException(); }
        public virtual bool DevReset() { return true; }
        public virtual string GetModuleName()
        {
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
        }
    }
}
