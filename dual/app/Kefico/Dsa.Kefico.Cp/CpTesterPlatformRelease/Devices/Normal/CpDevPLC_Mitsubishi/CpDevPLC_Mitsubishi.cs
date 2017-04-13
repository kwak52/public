using System;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev.Interface;
using static CpCommon.ExceptionHandler;
using System.Reflection;
using Dsu.PLC.Melsec;
using Dsu.PLC;
using Dsu.PLC.Common;
using System.Reactive.Linq;
using Dsu.Driver.Util.Emergency;
using System.Diagnostics;

namespace CpTesterPlatform.CpDevices
{
    public class CpDevPLC_Mitsubishi : IPLC
    {
        public CpFunctionEventHandler FuncEvtHndl { get; set; }
        public string CPU_TYPE_STR { set; get; } //Example: "Q03UDECPU";        
        public string PLC_IP_ADDR { set; get; }
        public int PLC_PORT_NUMBER { set; get; }
        public string CONNECTION_TYPE_STR { set; get; }
        public int TIMEOUT { set; get; }
        public bool READPORT { set; get; }
        public string DeviceID { set; get; }

        private MxConnection _conn;  // Read port UDP   Write port TCP
        private bool MonitoringStarted = false;

        public bool DevClose()
        {
            return true; //ahn
        }

        public bool DevOpen()
        {
            var oResult = TryFunc(() =>
            {
                TransportProtocol ConType = CONNECTION_TYPE_STR.ToUpper() == "TCP" ? TransportProtocol.Tcp : TransportProtocol.Udp;
                _conn = new MxConnection(new MxConnectionParameters(PLC_IP_ADDR, Convert.ToUInt16(PLC_PORT_NUMBER), ConType));
                _conn.Connect();
                return true;
            });

            if (oResult.HasException) return false;
            return oResult.Result;
        }

        public bool DevReset()
        {
            throw new NotImplementedException();
        }

        public bool AddDevices(string deviceNames)
        {
            if (!MonitoringStarted)
                _conn.CreateTag(deviceNames);
            else
                _conn.AddMonitoringTag(_conn.CreateTag(deviceNames));

            return true;
        }

        public void SingleScanStart()
        {
            if (!MonitoringStarted)
            {
                MonitoringStarted = true;
                if (_conn.Tags.Count == 0)
                    return;


                int readCount = 0;
                _conn.PerRequestDelay = TIMEOUT;
                _conn.Subject.OfType<TagValueChangedEvent>().Subscribe(evt =>
                {
                    readCount++;
                    EventTag((MxTag)evt.Tag);
                });
                _conn.StartDataExchangeLoopAsync();
                // _conn.Disconnect();


                Stopwatch xSW = new Stopwatch();
                xSW.Start();

                while (true)
                {
                    if (_conn.Tags.Count <= readCount)
                        break;
                    else
                        System.Threading.Thread.Sleep(50);

                    if (xSW.ElapsedMilliseconds > 7000)
                        break;
                }
            }
        }

        private void EventTag(MxTag tag)
        {
            
            SignalManager.RawSignalSubject.OnNext(new PlcSignal(tag.Name, DeviceID, Convert.ToInt32(tag.Value)));

            if (tag.IsBitDevice)
                FuncEvtHndl.DoPLCReceive(tag.Name + ";" + (Convert.ToBoolean(tag.Value) ? 1 : 0).ToString());
            else
                FuncEvtHndl.DoPLCReceive(tag.Name + ";" + tag.Value.ToString());
        }

        public string GetModuleName()
        {
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
        }

        public bool WriteBitDevice(string strDeviceWrite, int[] value)
        {
            //Console.WriteLine(string.Format("{0} : {1}", strDeviceWrite, value[0]));
            return _conn.McProtocol.SetBitDevice(strDeviceWrite, 1, value) == 0;
        }

        public bool WriteDevice(string strDeviceWrite, int value)
        {
            //Console.WriteLine(string.Format("{0} : {1}", strDeviceWrite, value));
            return _conn.McProtocol.SetDevice(strDeviceWrite, Convert.ToInt32(value)) == 0;
        }

        public int ReadDevice(string strDeviceRead)
        {
            int value;
            _conn.McProtocol.GetDevice(strDeviceRead, out value);
            return value;
        }

    }
}
