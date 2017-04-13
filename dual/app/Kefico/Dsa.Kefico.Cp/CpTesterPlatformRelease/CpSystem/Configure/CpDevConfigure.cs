using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpTStepDev;
using System;
using System.Collections.Generic;
using System.Xml;
using static CpCommon.ExceptionHandler;

namespace CpTesterPlatform.CpSystem.Configure
{
	/// <summary>
	/// Loading device configurations.
	/// - Communication Device Configuration Form Example
	/// <CommDevices>
	///   <Device>
	///     <ID>TCPIP_POWERSUPPLY0</ID>
	///     <Type>NETWORKTCPIP</Type>
	///     <DllName>CpDevNetworkTCPIP</DllName>
	///     <HwName>TCPIP0</HwName>
	///     <Attributes IP_ADDRESS = "192.168.0.100" PORT_NO="9221" CR="False" LF="True" />
	///   </Device>
	/// </CommDevices>
	/// 
	/// - Normal or Virtual Device Configuration Form Example
	/// <Device>
	///     <ID>PS0</ID>
	///     <Type>POWER_SUPPLY</Type>
	///     <Virtual>true</Virtual>
	///     <DllName>CpVDevPowerSupply_Sorensen</DllName>
	///     <CommDevID>TCPIP_POWERSUPPLY0</CommDevID>
	///     <HwName>1</HwName>
	///     <Attributes VOLTAGE_LIMIT = "30" CURRENT_LIMIT="3" MAX_TEMP="100" />
	///   </Device>
	/// 
	/// - Specific device properties are defined in attributes.
	/// - If attributes are not approprate to use or needs to attribute newly,
	///   ask Kefico Personnel who takes a charge for this.
	/// - DeviceInfo is defined in CpPlatform - CpTStepDevLib Project.
	/// </summary>
	public class CpDevConfigure
	{
		public ClsDeviceList LstDeviceInfo { get; set; }
		CpDevConfigure(ClsDeviceList lstInst)
		{
			LstDeviceInfo = lstInst;
		}
		
		// xml file control 
		static public CpDevConfigure xmlLoadData(XmlNode xmlNode, string strConfigueNode, ClsDeviceList lstInstInfo)
		{
			ClsDeviceInfoBase instinfobase = null;

			TryResult tResult = TryAction(() =>
			{
				foreach (XmlNode node in xmlNode.SelectNodes("Device"))
				{
					CpDeviceType insttype = (CpDeviceType)Enum.Parse(typeof(CpDeviceType), node["Type"].InnerText);
					
					///					
					TryResult rstAction = TryAction(() =>
					{
						switch (insttype)
						{
							case CpDeviceType.SWITCHBLOCK: instinfobase = new ClsSwitchInfo(insttype, node); break;
							case CpDeviceType.RELAYMATRIX: instinfobase = new ClsRelayInfo(insttype, node); break;
							case CpDeviceType.CAN: instinfobase = new ClsCANInfo(insttype, node); break;
							case CpDeviceType.NETWORKUDP: instinfobase = new ClsNetworkUdpInfo(insttype, node); break;
							case CpDeviceType.NETWORKTCPIP: instinfobase = new ClsNetworkTcpIpInfo(insttype, node); break;
							case CpDeviceType.SCOPE: instinfobase = new ClsOscilloInfo(insttype, node); break;
							case CpDeviceType.DMM: instinfobase = new ClsDMMInfo(insttype, node); break;
							case CpDeviceType.RESISTOR: instinfobase = new ClsResistorInfo(insttype, node); break;
							case CpDeviceType.SMU: instinfobase = new ClsSmuInfo(insttype, node); break;
							case CpDeviceType.COUNTER: instinfobase = new ClsCounterInfo(insttype, node); break;
							case CpDeviceType.COUNTER_INPUT: instinfobase = new ClsCounterInputInfo(insttype, node); break;
							case CpDeviceType.FGN: instinfobase = new ClsFGNInfo(insttype, node); break;
							case CpDeviceType.GPIB: instinfobase = new ClsGPIBInfo(insttype, node); break;
							case CpDeviceType.RELAY: instinfobase = new ClsRelayInfo(insttype, node); break;
							case CpDeviceType.RS232: instinfobase = new ClsRS232Info(insttype, node); break;
							case CpDeviceType.LOAD:
							case CpDeviceType.KAM: instinfobase = new ClsLoadInfo(insttype, node); break;
							case CpDeviceType.POWER_SUPPLY: instinfobase = new ClsPowerSupplyInfo(insttype, node); break;
                            case CpDeviceType.LCRMETER: instinfobase = new ClsLVDTInfo(insttype, node); break;
                            case CpDeviceType.LVDT: instinfobase = new ClsLVDTInfo(insttype, node); break;
                            case CpDeviceType.PLC: instinfobase = new ClsPLCInfo(insttype, node); break;
                            case CpDeviceType.LASERMARKER: instinfobase = new ClsLaserMarkerInfo(insttype, node); break;
                            case CpDeviceType.MOTION: instinfobase = new ClsMotionInfo(insttype, node); break;
                            case CpDeviceType.DAQ_CONTROLLER: instinfobase = new ClsDAQControllerInfo(insttype, node); break;
                            case CpDeviceType.ANALOG_INPUT: instinfobase = new ClsAnalogInputInfo(insttype, node); break;
                            case CpDeviceType.TRIGGER_IO: instinfobase = new ClsTriggerIOInfo(insttype, node); break;
                            case CpDeviceType.DIGITAL_IO: instinfobase = new ClsDIOControlInfo(insttype, node); break;
                            case CpDeviceType.ICController: instinfobase = new ClsICControllerInfo(insttype, node); break;
                            case CpDeviceType.TORQUE_METER: instinfobase = new ClsTorqueMeterInfo(insttype, node); break;
                            default:
								{
									UtilTextMessageEdits.UtilTextMsgToConsole("Could not find Instruments, DEV : " + insttype.ToString(), ConsoleColor.Red);
									break;
								}
						}
					});

					if(instinfobase == null)
						continue;

					if(rstAction.HasException == true)
                        UtilTextMessageEdits.UtilTextMsgToConsole("Configuration File Loading Error where device attributes of [Type: " + insttype + "] are not correct.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);

                    lstInstInfo.Add(node["ID"].InnerText, instinfobase);
				}

			});

			if(tResult.HasException == true)            
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to load '.xml' file for the application configuration.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);			
			
			return new CpDevConfigure(lstInstInfo);
		}

		static public CpDevConfigure xmlLoadData(XmlNode xmlNodePrt, string strConfigueNode)
		{
			ClsDeviceList lstInstInfo = new ClsDeviceList();
		
			return xmlLoadData(xmlNodePrt, strConfigueNode, lstInstInfo);   
		}

		public void xmlSaveData(XmlDocument xmlDoc, ref XmlNode xmlNodePrt)
		{
			XmlNode xmlNodeToAppend = xmlNodePrt;

			TryResult exeRst = TryAction(() =>
			{
				foreach (KeyValuePair<string, ClsDeviceInfoBase> instinfo in LstDeviceInfo)
				{
					XmlElement xmlInstElm = xmlDoc.CreateElement("Item");

					xmlInstElm.SetAttribute("DEVTYPE", instinfo.Value.DeviceType.ToString());
					xmlInstElm.SetAttribute("Device_ID", instinfo.Value.Device_ID);
					xmlInstElm.SetAttribute("HWNAME", instinfo.Value.HwName);

					switch (instinfo.Value.DeviceType)
					{
						case CpDeviceType.SWITCHBLOCK:
						case CpDeviceType.RELAYMATRIX:
							{
								ClsSwitchInfo switchinfo = instinfo.Value as ClsSwitchInfo;

								xmlInstElm.SetAttribute("TOPOLOGY", switchinfo.TopologyName);
								xmlInstElm.SetAttribute("ROWNAME", switchinfo.RowName);
								xmlInstElm.SetAttribute("ROWMAIN", switchinfo.RowMain);
								xmlInstElm.SetAttribute("CARDNUM", switchinfo.CardNum.ToString());
								xmlInstElm.SetAttribute("COLNUM", switchinfo.ColNum.ToString());
								xmlInstElm.SetAttribute("ROWNUM", switchinfo.RowNum.ToString());
								xmlInstElm.SetAttribute("COLNAME", switchinfo.ColName);
								xmlInstElm.SetAttribute("MULTISWITCH", switchinfo.MultiSwitch.ToString());
							}
							break;
						case CpDeviceType.SCOPE:
							{
								ClsOscilloInfo oscinfo = instinfo.Value as ClsOscilloInfo;
								xmlInstElm.SetAttribute("CHNUM", oscinfo.ChannelNum);
								xmlInstElm.SetAttribute("PROBEDIV", oscinfo.ProbeDivider.ToString());
							}
							break;
						case CpDeviceType.RESISTOR:
							{
								ClsResistorInfo reginfo = instinfo.Value as ClsResistorInfo;
								xmlInstElm.SetAttribute("SLOTNUM", reginfo.SlotNumber.ToString());
								xmlInstElm.SetAttribute("CHNUM", reginfo.ChannelNum.ToString());
								xmlInstElm.SetAttribute("Offset", reginfo.Offset.ToString());
							}
							break;
						case CpDeviceType.SMU:
							{
								ClsSmuInfo pwrinfo = instinfo.Value as ClsSmuInfo;
								xmlInstElm.SetAttribute("CHNUM", pwrinfo.ChannelNum.ToString());
								xmlInstElm.SetAttribute("LIMITVOLT", pwrinfo.LimitVolt.ToString());
								xmlInstElm.SetAttribute("LIMITCURRENT", pwrinfo.LimitCurr.ToString());
							}
							break;
						case CpDeviceType.GPIB:
							{
								ClsGPIBInfo gpibinfo = instinfo.Value as ClsGPIBInfo;
								xmlInstElm.SetAttribute("BOARDNUM", gpibinfo.BoardNum.ToString());
								xmlInstElm.SetAttribute("PRIMARY_ADDR", gpibinfo.PrimaryAddr.ToString());
								xmlInstElm.SetAttribute("SECONDARY_ADDR", gpibinfo.SecondaryAddr.ToString());
							}
							break;
						case CpDeviceType.POWER_SUPPLY:
							{
								ClsPowerSupplyInfo pwinfo = instinfo.Value as ClsPowerSupplyInfo;
                                xmlInstElm.SetAttribute("VOLTAGE_LIMIT", pwinfo.VOLTAGE_LIMIT.ToString());
                                xmlInstElm.SetAttribute("CURRENT_LIMIT", pwinfo.CURRENT_LIMIT.ToString());
                              //  xmlInstElm.SetAttribute("CHANNEL", pwinfo.CHANNEL.ToString());
                            }
							break;
						case CpDeviceType.RELAY:
                            {
                                ClsRelayInfo relayinfo = instinfo.Value as ClsRelayInfo;
                            }
                            break;
                        case CpDeviceType.CAN: /*FALL THROUGH*/
						case CpDeviceType.COUNTER: /*FALL THROUGH*/
						case CpDeviceType.FGN: /*FALL THROUGH*/
						case CpDeviceType.RS232: /*FALL THROUGH*/
						default:

							break;
					}

					xmlNodeToAppend.AppendChild(xmlInstElm);
				}
			});

            if (exeRst.HasException == true)
                UtilTextMessageEdits.UtilTextMsgToConsole("Error attributes where : " + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name, ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);

            if (exeRst.HasException == true)
				UtilTextMessageEdits.UtilTextMsgToConsole("Failed to save the test condition for the tester.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);

			xmlNodePrt = xmlNodeToAppend;
		}
	}
}
