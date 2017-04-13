using System;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpTStepDev.Interface;
using System.Reflection;

namespace CpTesterPlatform.CpDevices
{
    /// <summary>
    /// Example to represent how to build a virtual device.
    /// A Virtual or Normal Device is constructed in run-time environment by a Device manager.     
    /// Virtual Device is an inherited class from a CpVitualDeviceManagerBase and a corresponding interface.
    /// Normal Device is an inherited class from a corresponding interface.
    /// </summary>
    public class CpVDevPowerSupply_Sorensen : CpVitualDeviceManagerBase, IPowerSupply
    {
        public CpFunctionEventHandler FuncEvtHndl { get; set; }
        public string DeviceID { get; set; }
        public int ChannelID { get; set; }

        public string CONTROLLER_ADDRESS { get; set; }

        string SPLITTER = " ";
        string VERSION = "*IDN?";
        string STATUS_CLEAR = "*CLS";
        string INITIALIZATION = "*RST";

        string VALUE_TRUE = "1";
        string VALUE_FALSE = "0";

        string ACTIVATION = "OP";

        string VOLTAGE = "V";
        string CURRENT = "I";

        string OBSERVATION = "O";
        string QUERY = "?";

        public bool DevClose()
        {
            return true;
        }

        public bool DevOpen()
        {
            WriteData(INITIALIZATION);
            WriteData(STATUS_CLEAR);           

            return true;
        }

        public bool DevReset()
        {
            WriteData(INITIALIZATION);
            WriteData(STATUS_CLEAR);

            return true;
        }

        public string GetModuleName()
        {
            return Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", string.Empty);
        }
        
        public string GetVersion()
        {
            return QueryData(VERSION);
        }
        
        public bool SetOutput(bool enable)
        {
            string strMsg = string.Empty;
            string strState = (enable == true) ? VALUE_TRUE : VALUE_FALSE;

            strMsg = ACTIVATION + ChannelID + SPLITTER + strState;

            WriteData(strMsg);

            return true;
        }
        
        public bool SetVoltage(double voltage)
        {
            string strMsg = string.Empty;            

            strMsg = VOLTAGE + ChannelID + SPLITTER + voltage.ToString();

            WriteData(strMsg);

            return true;
        }        

        public bool SetCurrent(double current)
        {
            string strMsg = string.Empty;            

            strMsg = CURRENT + ChannelID + SPLITTER + current.ToString();

            WriteData(strMsg);

            return true;
        }
            
        public bool GetOutput()
        {
            string strMsg = string.Empty;
            string strResult = string.Empty;
            string strCommand = ACTIVATION + ChannelID;
            
            strMsg = strCommand + QUERY;

            strResult = QueryData(strMsg);

            return (strResult == VALUE_TRUE) ? true : false ;
        }

        public double GetVoltage()
        {
            string strMsg = string.Empty;
            string strResult = string.Empty;
            string strCommand = VOLTAGE + ChannelID;

            strMsg = strCommand + QUERY;

            strResult = QueryData(strMsg);
            strResult = strResult.Trim(strCommand.ToCharArray());

            return Convert.ToDouble(strResult);
        }

        public double GetCurrent()
        {
            string strMsg = string.Empty;
            string strResult = string.Empty;
            string strCommand = CURRENT + ChannelID;

            strMsg = strCommand + QUERY;

            strResult = QueryData(strMsg);
            strResult = strResult.Trim(strCommand.ToCharArray());

            return Convert.ToDouble(strResult);
        }

        public double GetObservedVoltage()
        {
            string strMsg = string.Empty;
            string strResult = string.Empty;
            string strCommand = VOLTAGE + ChannelID + OBSERVATION;

            strMsg = strCommand + QUERY;

            strResult = QueryData(strMsg);
            strResult = strResult.Trim('V');

            return Convert.ToDouble(strResult);
        }

        public double GetObservedCurrent()
        {
            string strMsg = string.Empty;
            string strResult = string.Empty;
            string strCommand = CURRENT + ChannelID + OBSERVATION;

            strMsg = strCommand + QUERY;

            strResult = QueryData(strMsg);
            strResult = strResult.Trim('A');

            return Convert.ToDouble(strResult);
        }
        
        void WriteData(string strMsg)
        {
            if (CommDeviceManager is IRS232Manager)
                ((IRS232Manager)CommDeviceManager).WriteData(strMsg);
            else if (CommDeviceManager is INetworkTcpIpManager)
                ((INetworkTcpIpManager)CommDeviceManager).SendData(strMsg);
        }

        string QueryData(string strMsg)
        {
            string strResult = string.Empty;

            WriteData(STATUS_CLEAR);

            if (CommDeviceManager is IRS232Manager)
                strResult = ((IRS232Manager)CommDeviceManager).QueryData(strMsg);
            else if (CommDeviceManager is INetworkTcpIpManager)
                strResult = ((INetworkTcpIpManager)CommDeviceManager).SendQueryData(strMsg);

            return strResult;
        }
    }
}
