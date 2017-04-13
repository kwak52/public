using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpSystem.Configure;
using CpTesterPlatform.CpSystem.Manager;
using CpTesterPlatform.CpTStepDev;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpSystem
{
	/// <summary>
	/// system manager controls components in the base system component.
	/// </summary>
	public class CpSystemManager : ISystemManager
	{
		public Stopwatch CpSystemTimer { get; set; } = new Stopwatch();
		public CpSysConfigure CnfSystem { get; set; } //! System Configuration 
		public CpHwManager MngHardware { get; set; } //! Hardware Manager - that manages device manangers.		
		public ResourceManager MngResx { get; set; }
        public static CpSystemManager TheSystemManager { get; private set; }

        public CpSystemManager()
		{
            TheSystemManager = this;
            var assembly = Assembly.GetExecutingAssembly();
			var strBaseName = assembly.GetName().Name + "." + "Properties.Resources";
			MngResx = new ResourceManager(strBaseName, assembly);
		}

		/// <summary>
		/// Loading CP-Tester configuration xml to check Hardware activation states and build Hardward manager.
		/// If hardware activitation state is true, device managers for the hardware will be built.
		/// The hardware manager manages all device managers.
		/// </summary>
		/// <param name="strCfgXmlPath"></param>
		public CpSystemManager(string strCfgXmlPath)
            : this()
		{			
			CnfSystem = CpSysConfigure.loadCpSystemConfig(strCfgXmlPath);

			var xmlDoc = new XmlDocument();
			xmlDoc.Load(strCfgXmlPath);
			 MngHardware = new CpHwManager{
				CnfDevices = CpHwConfigure.xmlLoadData(xmlDoc, "ROOT/HwConfigure/Devices"),
				CnfCommDevices = CpHwConfigure.xmlLoadData(xmlDoc, "ROOT/HwConfigure/CommDevices")};
			CreateCommDevices();
			CreateDevices();
		}

		public void LoadXmlConfig(string strXmlPath)
		{
			XDocument xDoc = XDocument.Load(strXmlPath);
			var xElement = xDoc.Element("ROOT");
			if (xElement != null)
			{
				XElement xelemSysConf = xElement.Element("SystemConfigure");
				CnfSystem = CpSysConfigure.xmlLoadData(xelemSysConf);
			}

			var xmlDoc = new XmlDocument();
			xmlDoc.Load(strXmlPath);
		   
			MngHardware = new CpHwManager{
				CnfCommDevices = CpHwConfigure.xmlLoadData(xmlDoc, "ROOT/HwConfigure/CommDevices"),
				CnfDevices = CpHwConfigure.xmlLoadData(xmlDoc, "ROOT/HwConfigure/Devices")	};
		}

		public void CloseManager()
		{
			MngHardware?.CloseDeviceManager();
			MngHardware?.CloseCommDeviceManager();
		}

		public bool InitInstrument()
		{
			if(MngHardware?.DicCommDeviceManager == null)
			{
				return false;
			}

			if(MngHardware?.DicDeviceManager == null)
			{
				return false;
			}

			foreach (var devmgrComm in MngHardware.DicCommDeviceManager)
			{
				var mngDevice = devmgrComm.Value as IDevManager;
				
				if (devmgrComm.Value.ActiveHw == false)
					continue;

				mngDevice?.OpenDevice();
			}

			foreach (var devmgr in MngHardware.DicDeviceManager)
			{
				var mngDevice = devmgr.Value as IDevManager;

				if (devmgr.Value.ActiveHw == false)
					continue;

				mngDevice?.OpenDevice();
			}

			foreach(var devmgr in MngHardware.DicDeviceManager)
			{
				var mngDevice = devmgr.Value as CpDeviceManagerBase;
				if(mngDevice.DeviceInfo.VirtualDevice)
				{
					if (devmgr.Value.ActiveHw == false)
						continue;

					if (MngHardware.DicDeviceManager.ContainsKey(mngDevice.DeviceInfo.HwName))
					{
						mngDevice.SetCommDeviceMgr(MngHardware.DicDeviceManager[mngDevice.DeviceInfo.HwName]);
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Communication devices are created in this method.
		/// </summary>
		/// <returns></returns>
		public bool CreateCommDevices()
		{            
			var lstInstError = new ClsDeviceList();
			MngHardware.DicCommDeviceManager = new Dictionary<string, CpDeviceManagerBase>();
			ClsDeviceList lstInstInfo = MngHardware.CnfCommDevices.DevConfigue.LstDeviceInfo;

            TryResult exeRst = TryAction(() =>
            {
                foreach (KeyValuePair<string, ClsDeviceInfoBase> info in lstInstInfo)
                {
                    if (!MngHardware.DicCommDeviceManager.ContainsKey(info.Value.Device_ID))
                    {
                        switch (info.Value.DeviceType)
                        {
                            //case CpDeviceType.NETWORKUDP: MngHardware.DicCommDeviceManager.Add(info.Value.Device_ID, new CpMngNetworkUdp(CnfSystem.HardwareActivation.ActiveHwFncNETWORKUDP)); break;
                            case CpDeviceType.NETWORKTCPIP: MngHardware.DicCommDeviceManager.Add(info.Value.Device_ID, new CpMngNetworkTcpIp(CnfSystem.HardwareActivation.ActiveHwFncNETWORKTCPIP)); break;
                            //case CpDeviceType.CAN: MngHardware.DicCommDeviceManager.Add(info.Value.Device_ID, new CpMngCan(CnfSystem.HardwareActivation.ActiveHwFncCAN)); break;                    
                            //case CpDeviceType.GPIB: MngHardware.DicCommDeviceManager.Add(info.Value.Device_ID, new CpMngGpib(CnfSystem.HardwareActivation.ActiveHwFncGPIB)); break;
                            //	case CpDeviceType.PLC: MngHardware.DicCommDeviceManager.Add(info.Value.Device_ID, new CpMngPlc(CnfSystem.HardwareActivation.ActiveHwFncPLC)); break;
                            case CpDeviceType.RS232: MngHardware.DicCommDeviceManager.Add(info.Value.Device_ID, new CpMngRS232(CnfSystem.HardwareActivation.ActiveHwFncRS232)); break;
                            case CpDeviceType.DAQ_CONTROLLER: MngHardware.DicCommDeviceManager.Add(info.Value.Device_ID, new CpMngDAQControl(CnfSystem.HardwareActivation.ActiveHwFncDAQController)); break;
                        }
                    }

                    IDevManager mgrDev = MngHardware.DicCommDeviceManager[info.Value.Device_ID] as IDevManager;

                    if (MngHardware.DicCommDeviceManager[info.Value.Device_ID].ActiveHw == false)
                        continue;

                    mgrDev.CreateDevice(info.Value);

                }
            });

			if (exeRst.HasException == true)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed process command key in the main frame.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + exeRst.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);

				return false;
			}

			if (lstInstError.Count <= 0) return true;
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Check Device List\r\n");
				foreach ( KeyValuePair<string, ClsDeviceInfoBase> info in lstInstError)
				{
					sb.Append(info.Value.DeviceType.ToString());
					sb.Append(" : ");
					sb.AppendLine(info.Value.HwName);
				}
				UtilTextMessageBox.UIMessageBoxForWarning("Failed to Create Instruments", sb.ToString());
			}
						
			return true;
		}

		/// <summary>
		/// Normal and virtual devices are created in this method.
		/// </summary>
		/// <returns></returns>
		public bool CreateDevices()
		{
			// ReSharper disable once CollectionNeverUpdated.Local
			var lstInstError = new ClsDeviceList();

			TryResult exeRst = TryAction(() =>
			{
				ClsDeviceList lstInstInfo = MngHardware.CnfDevices.DevConfigue.LstDeviceInfo;
				MngHardware.DicDeviceManager = new Dictionary<string, CpDeviceManagerBase>();

				foreach (KeyValuePair<string, ClsDeviceInfoBase> info in lstInstInfo)
				{
                    if (!MngHardware.DicDeviceManager.ContainsKey(info.Value.Device_ID))
					{
						switch (info.Value.DeviceType)
						{
							//case CpDeviceType.RELAYMATRIX: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngRelayMatrix(CnfSystem.HardwareActivation.ActiveHwFncSwitch)); break;
							//case CpDeviceType.SWITCHBLOCK: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngSwitchBlock(CnfSystem.HardwareActivation.ActiveHwFncSwitch)); break;                            
							//case CpDeviceType.SCOPE: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngScope(CnfSystem.HardwareActivation.ActiveHwFncScope)); break;
							//case CpDeviceType.DMM: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngDmm(CnfSystem.HardwareActivation.ActiveHwFncDMM)); break;
							//case CpDeviceType.RESISTOR: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngResistor(CnfSystem.HardwareActivation.ActiveHwFncRegister)); break;
							//case CpDeviceType.SMU: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngSmu(CnfSystem.HardwareActivation.ActiveHwFncSMU)); break;
							//case CpDeviceType.COUNTER: mngHw.DicInstrumentManager.Add(info.Value.Device_ID, new CpMngCounter()); break;
							//case CpDeviceType.COUNTER_INPUT: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngCounterInput(CnfSystem.HardwareActivation.ActiveHwFncCI)); break;
							//case CpDeviceType.FGN: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngFgn(CnfSystem.HardwareActivation.ActiveHwFncFGN)); break;
							//case CpDeviceType.RELAY: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngRelay(CnfSystem.HardwareActivation.ActiveHwFncRelay)); break;
							//case CpDeviceType.LOAD: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngLoad(CnfSystem.HardwareActivation.ActiveHwFncLoad)); break;
							//case CpDeviceType.KAM: MngHardware.DicInstrumentManager.Add(info.Value.Device_ID, new CpMngKam()); break;                            
                            //case CpDeviceType.LASERMARKER: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngLaserMarker(CnfSystem.HardwareActivation.ActiveHwFncLaserMarker)); break;
                            case CpDeviceType.TRIGGER_IO: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngTriggerIO(CnfSystem.HardwareActivation.ActiveHwFncTriggerIO)); break;
							case CpDeviceType.POWER_SUPPLY: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngPowerSupply(CnfSystem.HardwareActivation.ActiveHwFncPowerSupply)); break;
                            case CpDeviceType.LCRMETER: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngLCRMeter(CnfSystem.HardwareActivation.ActiveHwFncLCRMeter)); break;
							case CpDeviceType.LVDT: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngLVDT(CnfSystem.HardwareActivation.ActiveHwFncLVDT)); break;							
							case CpDeviceType.ANALOG_INPUT: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngAIControl(CnfSystem.HardwareActivation.ActiveHwFncAI)); break;														
							case CpDeviceType.MOTION: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngMotion(CnfSystem.HardwareActivation.ActiveHwFncMotion)); break;
							case CpDeviceType.DIGITAL_IO: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngDIOControl(CnfSystem.HardwareActivation.ActiveHwFncDigitalIO)); break;
                            case CpDeviceType.PLC: MngHardware.DicDeviceManager.Add(info.Value.Device_ID, new CpMngPlc(CnfSystem.HardwareActivation.ActiveHwFncPLC)); break;
							default:
								continue;
						}
					}

					IDevManager idevMgr = (IDevManager)MngHardware.DicDeviceManager[info.Value.Device_ID];

                    //if (MngHardware.DicDeviceManager[info.Value.Device_ID].ActiveHw == false)  //CreateDevice 생성을 통한 DLL 링크 수행 (실사용은 드라이버 함수에서 체크)
                    //  continue;

                    if (info.Value.VirtualDevice)
					{
						if (!idevMgr.CreateVirtualDevice(info.Value, MngHardware.GetCommDeviceManager(info.Value.CommDevID)))
							continue;
					}
					else
					{
						if (!idevMgr.CreateDevice(info.Value))
							continue;
					}
				}
			});
			
			if(exeRst.HasException == true)
			{
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to create a system manager.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
				UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + exeRst.Exception.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
			}
			return false;
		}

        public static IEnumerable<T> GetDeviceManagers<T>() => TheSystemManager.MngHardware.DicDeviceManager.Values.OfType<T>();
        public static T GetDeviceManager<T>() => GetDeviceManagers<T>().FirstOrDefault();
    }
}
