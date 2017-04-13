using System.Collections.Generic;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpTStepDev.Interface;
using System.Linq;
using CpTesterPlatform.CpTStepDev;
using CpTesterPlatform.CpSystem.Configure;

namespace CpTesterPlatform.CpSystem.Manager
{
    /// <summary>
    /// Hardware Manager manages device managers.
    /// Find a device manager by a device ID.
    /// Find a communication device manager by a communication device ID.
    /// </summary>
	public class CpHwManager
	{		
		public CpHwConfigure CnfCommDevices { get; set; }
		public CpHwConfigure CnfDevices { get; set; }

		public void CloseDeviceManager()
		{
			if (DicDeviceManager == null) return;
			foreach (var device in DicDeviceManager)
			{
				if(device.Value.ActiveHw == false)
					continue;

				var mngTemp = device.Value as IDevManager;
				mngTemp?.CloseDevice();
			}
		}

		public void CloseCommDeviceManager()
		{
			if (DicCommDeviceManager == null) return;
			foreach (var commdevice in DicCommDeviceManager)
			{
				if(commdevice.Value.ActiveHw == false)
					continue;

				var mngTemp = commdevice.Value as IDevManager;
				mngTemp?.CloseDevice();
			}
		}

		public Dictionary<string, CpDeviceManagerBase> DicDeviceManager { get; set; } = null;
		public Dictionary<string, CpDeviceManagerBase> DicCommDeviceManager { get; set; } = null;	

		public List<string> GetDeviceIdList(CpDeviceType devType)
		{
			List<string> vstrIds = new List<string>();

			foreach(string strDevID in DicDeviceManager.Keys)
			{
				 if (DicDeviceManager[strDevID].DeviceInfo == null)
					DicDeviceManager[strDevID].DeviceInfo = new CpTStepDev.ClsDeviceInfoBase( CpDeviceType.NONE, null);
				if (DicDeviceManager[strDevID].DeviceInfo.DeviceType == devType)
					vstrIds.Add(strDevID);
			}

			return vstrIds;
		}

		public List<string> GetCommDeviceIdList(CpDeviceType devType)
		{
			List<string> vstrIds = new List<string>();


			foreach(string strDevID in DicCommDeviceManager.Keys)
			{
				 if (DicCommDeviceManager[strDevID].DeviceInfo == null)
					DicCommDeviceManager[strDevID].DeviceInfo = new CpTStepDev.ClsDeviceInfoBase( CpDeviceType.NONE, null);
				if (DicCommDeviceManager[strDevID].DeviceInfo.DeviceType == devType)
					vstrIds.Add(strDevID);
			}

			return vstrIds;
		}
		
		public CpDeviceManagerBase GetDeviceManager(string devId)
		{
			if(DicDeviceManager.ContainsKey(devId))
				return DicDeviceManager[devId];

			return null;
		}
        
		public CpDeviceManagerBase GetCommDeviceManager(string devId)
		{
			if(DicCommDeviceManager.ContainsKey(devId))
				return DicCommDeviceManager[devId];

			return null;
		}
	}
}
