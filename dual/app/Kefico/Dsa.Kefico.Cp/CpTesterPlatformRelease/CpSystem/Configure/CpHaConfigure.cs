using System;
using System.Xml;
using DevExpress.XtraEditors.DXErrorProvider;
using CpTesterPlatform.CpCommon;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Xml.Linq;
using CpUtility.ExtensionMethods;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpSystem.Configure
{
    /// <summary>    
    /// Ha : Hardware Activation
    /// Loading from CpTesterConfiguration.xml
    /// </summary>
    /// <returns></returns>

    public class CpHaConfigure : IDXDataErrorInfo
	{
		public bool ActiveHwFncCAN { get; private set; }
		public bool ActiveHwFncRS232 { get; private set; }
		public bool ActiveHwFncGPIB { get; private set; }
		public bool ActiveHwFncNETWORKUDP { get; private set; }
		public bool ActiveHwFncNETWORKTCPIP { get; private set; }
		public bool ActiveHwFncDAQController { get; private set; }
        public bool ActiveHwFncSwitch { get; private set; }
		public bool ActiveHwFncDMM { get; private set; }
		public bool ActiveHwFncSMU { get; private set; }
		public bool ActiveHwFncRegister { get; private set; }		
		public bool ActiveHwFncFGN { get; private set; }
		public bool ActiveHwFncLoad { get; private set; }        
		public bool ActiveHwFncScope { get; private set; }
		public bool ActiveHwFncRelay { get; private set; }
		public bool ActiveHwFncPowerSupply { get; private set; }        
		public bool ActiveHwFncLVDT { get; private set; }
		public bool ActiveHwFncLCRMeter { get; private set; }
		public bool ActiveHwFncLaserMarker { get; private set; }
		public bool ActiveHwFncMotion { get; private set; }
		public bool ActiveHwFncPLC { get; private set; }		
		public bool ActiveHwFncCI { get; private set; }        
		public bool ActiveHwFncAI { get; private set; }
		public bool ActiveHwFncTriggerIO { get; private set; }
        public bool ActiveHwFncDigitalIO { get; private set; }

        /// <summary>
        /// 모든 parameter 는 ActiveHwFnc 의 bool 상태를 표시하는 문자열 값. e.g {"true", "false"}
        /// </summary>
        /// return new CpHaConfigure		
        public CpHaConfigure(
			string sNetworkUdp, 
			string sTcpIp, 
			string sCAN, 		
			string sDMM, 
			string sSMU, 
			string sRegister, 		
			string sFGN, 
			string sLoad, 
			string sGPIB, 
			string sScope, 
			string sRelay, 
			string sScopePowerSupply, 
			string sRS232, 
			string sLVDT, 
			string sLCRMeter, 
			string sLaserMarker, 
			string sMotion, 
			string sPLC, 
			string sDAQController, 
			string sAI, 
			string sCI,
			string sTrgIO,
            string sDIO)
		{

            ActiveHwFncCAN = sCAN.ToBool();
			ActiveHwFncRS232 = sRS232.ToBool();
			ActiveHwFncGPIB = sGPIB.ToBool();
            ActiveHwFncNETWORKUDP = sNetworkUdp.ToBool();
			ActiveHwFncNETWORKTCPIP = sTcpIp.ToBool();
			ActiveHwFncPLC = sPLC.ToBool();
			ActiveHwFncDAQController = sDAQController.ToBool();
			
			ActiveHwFncDMM = sDMM.ToBool();
			ActiveHwFncSMU = sSMU.ToBool();
			ActiveHwFncRegister = sRegister.ToBool();
			ActiveHwFncFGN = sFGN.ToBool();
			ActiveHwFncLoad = sLoad.ToBool();			
			ActiveHwFncScope = sScope.ToBool();
			ActiveHwFncRelay = sRelay.ToBool();
			ActiveHwFncPowerSupply = sScopePowerSupply.ToBool();            
			ActiveHwFncLVDT = sLVDT.ToBool();
			ActiveHwFncLCRMeter = sLCRMeter.ToBool();
			ActiveHwFncLaserMarker = sLaserMarker.ToBool();
			ActiveHwFncMotion = sMotion.ToBool();
			ActiveHwFncLaserMarker = sLaserMarker.ToBool();
			ActiveHwFncMotion = sMotion.ToBool();
			ActiveHwFncPLC = sPLC.ToBool();
			ActiveHwFncAI = sAI.ToBool();
			ActiveHwFncCI = sCI.ToBool();
			ActiveHwFncTriggerIO = sTrgIO.ToBool();
            ActiveHwFncDigitalIO = sDIO.ToBool();
        }

		// Implements the IDXDataErrorInfo,GetProperty method.
		public void GetPropertyError(string propertyName, ErrorInfo info) {}
		
		public void GetError(ErrorInfo info) {}

		// load
		static public CpHaConfigure xmlLoadData(XElement xelemRoot)
		{
            var tResult = TryFunc(() =>
            {
                string strCAN = xelemRoot.Element(CpDeviceType.CAN.ToString()).Value;
				string sRS232 = xelemRoot.Element(CpDeviceType.RS232.ToString()).Value;
				string strGPIB = xelemRoot.Element(CpDeviceType.GPIB.ToString()).Value;
				string sTCP = xelemRoot.Element(CpDeviceType.NETWORKTCPIP.ToString()).Value;
				string sDaqController = xelemRoot.Element(CpDeviceType.DAQ_CONTROLLER.ToString()).Value;
                string sNetworkUdp = xelemRoot.Element(CpDeviceType.NETWORKUDP.ToString()).Value;
                
                string strDMM = xelemRoot.Element(CpDeviceType.DMM.ToString()).Value;
                string strSMU = xelemRoot.Element(CpDeviceType.SMU.ToString()).Value;
                string strRegister = xelemRoot.Element(CpDeviceType.RESISTOR.ToString()).Value;
                string strFGN = xelemRoot.Element(CpDeviceType.FGN.ToString()).Value;
                string strLoad = xelemRoot.Element(CpDeviceType.LOAD.ToString()).Value;
                string strScope = xelemRoot.Element(CpDeviceType.SCOPE.ToString()).Value;
                string sRelay = xelemRoot.Element(CpDeviceType.RELAY.ToString()).Value;
                string sScopePowerSupply = xelemRoot.Element(CpDeviceType.POWER_SUPPLY.ToString()).Value;
				string sLVDT = xelemRoot.Element(CpDeviceType.LVDT.ToString()).Value;
				string sLCRMeter = xelemRoot.Element(CpDeviceType.LCRMETER.ToString()).Value;				
				string sLaserMarker = xelemRoot.Element(CpDeviceType.LASERMARKER.ToString()).Value;
				string sMotion = xelemRoot.Element(CpDeviceType.MOTION.ToString()).Value;
				string sPLC = xelemRoot.Element(CpDeviceType.PLC.ToString()).Value;
				string sAI = xelemRoot.Element(CpDeviceType.ANALOG_INPUT.ToString()).Value;
				string sCI = xelemRoot.Element(CpDeviceType.COUNTER_INPUT.ToString()).Value;
				string sTrgIO = xelemRoot.Element(CpDeviceType.TRIGGER_IO.ToString()).Value;
                string sDIO = xelemRoot.Element(CpDeviceType.DIGITAL_IO.ToString()).Value;

                return new CpHaConfigure(sNetworkUdp, sTCP, strCAN, strDMM, strSMU, strRegister,
				strFGN, strLoad, strGPIB, strScope, sRelay, sScopePowerSupply, sRS232,
				sLVDT, sLCRMeter, sLaserMarker, sMotion, sPLC, sDaqController, sAI, sCI, sTrgIO, sDIO);
			});
            return tResult.Succeeded ? tResult.Result : null;
        }

        // save
        public void xmlSaveData(XmlDocument xmlDoc, XmlNode xmlNodePrt)
		{
            TryAction(() =>
            {
                new[]
				{
					new Tuple<string, bool>("CAN", ActiveHwFncCAN),
                    new Tuple<string, bool>("GPIB", ActiveHwFncGPIB),
                    new Tuple<string, bool>("RS232", ActiveHwFncRS232),
                    new Tuple<string, bool>("NETWORKUDP", ActiveHwFncNETWORKUDP),
                    new Tuple<string, bool>("Switch", ActiveHwFncSwitch),
		            new Tuple<string, bool>("DMM", ActiveHwFncDMM),
		            new Tuple<string, bool>("SMU", ActiveHwFncSMU),
		            new Tuple<string, bool>("Register", ActiveHwFncRegister),
		            new Tuple<string, bool>("FGN", ActiveHwFncFGN),
		            new Tuple<string, bool>("Load", ActiveHwFncLoad),
		            new Tuple<string, bool>("Scope", ActiveHwFncScope),
                    new Tuple<string, bool>("Relay", ActiveHwFncRelay),
                    new Tuple<string, bool>("ScopePowerSupply", ActiveHwFncPowerSupply),
				}.ForEach(tpl =>
				{
					string name = tpl.Item1;
					bool activate = tpl.Item2;
					XmlNode xmlNode = xmlDoc.CreateNode("element", name, "");
					xmlNode.InnerText = activate.ToString();
					xmlNodePrt.AppendChild(xmlNode);
				});
			});
		}
	}
}
