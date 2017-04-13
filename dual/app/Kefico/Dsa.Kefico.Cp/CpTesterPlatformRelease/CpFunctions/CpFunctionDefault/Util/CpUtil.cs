using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using static CpCommon.ExceptionHandler;
using Dsu.Driver.Math;
using System.IO;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpTStepDev;
using System.Linq.Expressions;
using System.Data;
using static CpBase.CpLog4netLogging;
using Dsu.Driver.Base;
using PsKGaudi.Parser.PsCCSSTDFn;

namespace CpTesterPlatform.Functions
{

    public static class CpUtil
    {
        public static bool UsingLimeDAQ = true;
        public static double FD_INPUT_OFFSET = 0.0;
        public static double FD_MIDDLE_OFFSET = 0.0;
        public static double FD_OUTPUT_OFFSET = 0.0;
        public static bool FD_MASTER_USE = false;
        public static double AirGapOffSet = 0.0;
        public const double AirGapDefault = 2.2;
        public static string PartID;

        private static int Station0_Index = -1;
        private static int Station1_Index = -1;
        private static int Station2_Index = -1;
        private static int Station3_Index = -1;
        private static int Station4_Index = -1;

        public static Dictionary<string, CpPartID> Station_PartID = new Dictionary<string, CpPartID>();

        public const int MonitoringPLC = 999;
        public static bool InitStart = true;

        public static DataTable dtPLC = new DataTable();

        public static void UpdateStationIndex(int station, int data)
        {
            switch (station)
            {
                case 0: Station0_Index = data; break;
                case 1: Station1_Index = data; break;
                case 2: Station2_Index = data; break;
                case 3: Station3_Index = data; break;
                case 4: Station4_Index = data; break;
            }
        }

        public static string GetStationindex(int station)
        {
            int data = -1;
            switch (station)
            {
                case 0: data = Station0_Index; break;
                case 1: data = Station1_Index; break;
                case 2: data = Station2_Index; break;
                case 3: data = Station3_Index; break;
                case 4: data = Station4_Index; break;
            }
            return data == -1 ? "": data.ToString();
        }

        public static string GetMesID(int station)
        {
            string index = GetStationindex(station);

            if (Station_PartID.ContainsKey(index))
                return Station_PartID[index].MesID;
            else
                return "";
        }

        public static DateTime GetMesTime(int station)
        {
            string index = GetStationindex(station);

            if (Station_PartID.ContainsKey(index))
                return Station_PartID[index].CreateTime;
            else
                return DateTime.Now;
        }

        public static string GetPartID(int stationID)
        {
            if (PartID.Contains("+"))
                return string.Format("{0}{1}", PartID.Split('+')[0], Convert.ToInt16(GetStationindex(stationID)) + Convert.ToInt16(PartID.Split('+')[1]));
            else
                return "";
        }

        public static bool UsingRobotStep(CpSystemManager cpMngSystem, IStnManager iMngStation, PsCCSStdFnBase cpTsParent)
        {
            if (DriverBaseGlobals.IsLine())
                return false;

            foreach (var wire in cpTsParent.GetWiringForControlBlock())
            {
                if (wire.PinAlias == "ROBOT" && wire.PinPositive != "-")
                    return true;
            }

            return false;
        }
    
        ///HaConfigure 상에 사용 True/False 사용여부 체크 
        public static bool CheckEnableDevice(CpSystemManager cpMngSystem, CpDeviceType cpDeviceType)
        {
            var deviceMng = cpMngSystem.MngHardware.DicDeviceManager.Values.Where(mng => mng.DeviceInfo.DeviceType == cpDeviceType).FirstOrDefault();
            if (deviceMng == null || !deviceMng.ActiveHw)
                return false;
            else
                return true;
        }

        ///Control Block에 해당하는 Manager Deivce 얻음
        public static CpDeviceManagerBase GetManagerControlDevice(CpSystemManager cpMngSystem, IStnManager iMngStation, string pinName)
        {
            ClsTsCtrBlockBase ctbase = iMngStation.MngTStep.MngControlBlock.DicAnsteuerWithCtrBlock.Values.Where(wire => wire.CtrBlockPinName == pinName).FirstOrDefault();
            if (ctbase == null)
                return null;

            return cpMngSystem.MngHardware.GetDeviceManager(ctbase.CtrBlockRelDevID);
        }

        ///Normal Device 해당하는 Manager Deivce 얻음
        public static CpDeviceManagerBase GetManagerDevice(CpSystemManager cpMngSystem, string DeviceName)
        {
            CpDeviceManagerBase ctbase = cpMngSystem.MngHardware.DicDeviceManager.Values.Where(wire => wire.DeviceInfo.Device_ID == DeviceName).FirstOrDefault();

            return ctbase;
        }

        ///Normal Device 해당하는 Manager Deivce 얻음
        public static CpDeviceManagerBase GetManagerDevice(CpSystemManager cpMngSystem, CpDeviceType deviceType)
        {
            CpDeviceManagerBase ctbase = cpMngSystem.MngHardware.DicDeviceManager.Values.Where(w => w.DeviceInfo.DeviceType == deviceType).FirstOrDefault();

            return ctbase;
        }

        public static List<IDevManager> GetManagerDevices(CpSystemManager MngSystem, CpDeviceType DeviceType)
        {
            List<IDevManager> lstMng = new List<IDevManager>();
            foreach (var info in MngSystem.MngHardware.CnfDevices.DevConfigue.LstDeviceInfo)
            {
                if (info.Value.DeviceType == DeviceType)
                {
                    lstMng.Add((IDevManager)MngSystem.MngHardware.DicDeviceManager[info.Value.Device_ID]);
                }
            }

            return lstMng;
        }


        public static List<IDevManager> GetRobotDevice(List<IDevManager> list, bool bRobot)
        {
            List<IDevManager> lstMng = new List<IDevManager>();
            foreach (var Motion in list)
            {
                CpMngMotion mngMotion = Motion as CpMngMotion;
                if (((ClsMotionInfo)mngMotion.DeviceInfo).AXIS_ROBOT == bRobot)
                    lstMng.Add(Motion);
            }

            return lstMng;
        }

    

        public static void ConsoleWrite(IStnManager iMngStation, string funName)
        {
            var ps = iMngStation.MngTStep.GaudiReadData.ListTestStep[iMngStation.MngTStep.TsCurrentNumIndex];
            UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("STN[{0}][{1}] Step {2} function {3}", iMngStation.StationId, funName, ps.StepNum, ps.STDKeficoName), ConsoleColor.Green, CpDefineEnumDebugPrintLogLevel.INFO);
        }



        public static string GetStringFromASCII(int[] arrData)
        {
            string sData = "";
            foreach (short s in arrData)
            {
                byte[] intBytes = BitConverter.GetBytes(s);
                foreach (byte b in intBytes)
                {
                    if (b == 0) continue;
                    sData += ((Char)b).ToString();             // Ascii To Char
                }
            }
            return sData;
        }

        public static short[] HexStringToBytes(string hexString)
        {
            if (hexString.Length % 2 != 0)
                hexString += "\0";

            char[] result = hexString.ToCharArray();
            short[] arrShort = new short[hexString.Length / 2];
            byte[] bytes = new byte[hexString.Length];

            for (int i = 0; i < result.Length; i++)
                bytes[i] = Convert.ToByte(result[i]);

            for (int i = 0; i < bytes.Length; i += 2)
                arrShort[i / 2] = BitConverter.ToInt16(bytes, i);

            return arrShort;
        }

    
    }
}
