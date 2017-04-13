using System;
using System.Collections.Generic;
using System.Linq;
using CpTesterPlatform.CpCommon;
using CpTesterSs.UserControl;
using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpApplication.Configure;
using CpTesterPlatform.CpTesterSs;
using Dsu.Driver.Base;
using System.IO;
using CpTesterPlatform.CpLogUtil;
using System.Data;

namespace CpTesterPlatform.Functions
{
    public static class PlcUtil
    {

        public static void LogAutoSort(string directory = @"D:\CPLog\2017")
        {
            string search = "*.CpLog";
            //directory = @"C:\Users\AHN\Desktop\CG GGR LOG 8FF\AUDIT\Audit log";
            List<string> lstPath = Directory.EnumerateFiles(directory, search, SearchOption.AllDirectories).ToList();
            foreach (var path in lstPath)
            {
                GetLogData(path);
            }
        }

        private static void GetLogData(string strPath)
        {
            CpLogHeader logHeader = null;
            string strDirectory = Path.GetDirectoryName(strPath);
            string strFileName = Path.GetFileNameWithoutExtension(strPath);
            DataTable dtResult = null;
            string strExtension = Path.GetExtension(strPath);

            if (File.Exists(strPath) && Path.GetExtension(strPath) == ".CpLog")
                dtResult = CpUtilRl.LoadTestLogFromCpLogFile(strPath, strFileName + ".csv", out logHeader);

            DirectoryInfo dir = new DirectoryInfo(strDirectory + "\\" + logHeader.PART_ID.TrimStart());
            dir.Create();
            File.Move(strPath, strDirectory + "\\" + logHeader.PART_ID.TrimStart() + "\\" + strFileName + strExtension);
        }

        public static bool WirtePLCMeasure(UcMainViewSs xOwner, string function, int indexFunc, int indexPLC, out string data, bool bSkip = false)
        {
            data = string.Empty;
            CpTsManager mngStep = xOwner.MngStation.MngTStep as CpTsManager;
            var listFunction = mngStep.LstTestSteps.Where(w => w.Core.STDKeficoName == function);
            if (listFunction.Count() == 0)
                return false;
            if (indexFunc >= listFunction.Count())
                return false;

            var dataResult = listFunction.ElementAt(indexFunc);

            if (dataResult == null || dataResult.Activated == TsActivate.DEACTIVATED)
                return false;

            bool bJudgeError = dataResult.CreateMeasuringLogString()[8] == "OK" ? false : true;
            string Measuring = dataResult.CreateMeasuringLogString()[5];
            data = Measuring;
            xOwner.PlcIF.SetMeasureValue(indexPLC, Measuring, bJudgeError, bSkip);

            return bJudgeError;
        }

        public static List<bool> ResultPLC(UcMainViewSs xOwner, bool bSkip = false)
        {
            if (DriverBaseGlobals.IsLine8FF())
                return Result8FF(xOwner, bSkip);
            else if (DriverBaseGlobals.IsLine7DCT())
                return Result7DCT(xOwner, bSkip);
            else
                return new List<bool>();
        }

        private static List<bool> Result7DCT(UcMainViewSs xOwner, bool bSkip = false)
        {
            List<bool> checkNgAll = new List<bool>();
            var StationDeviceMng = xOwner.MngStation.CnfStation.DevList;
            foreach (CpStnDevConfigure StnDevConfigure in StationDeviceMng)
            {
                if (StnDevConfigure.ID == string.Empty)
                    continue;
                var device = xOwner.TheSystemManager.MngHardware.DicDeviceManager.Where(w => w.Value.DeviceInfo.Device_ID == StnDevConfigure.ID).FirstOrDefault().Value.DeviceInfo;

                switch (device.DeviceType)
                {
                    case CpDeviceType.LVDT: checkNgAll = ResultFD_7(xOwner, bSkip); break;
                    case CpDeviceType.LCRMETER: checkNgAll = ResultLCR_7(xOwner, bSkip); break;
                    case CpDeviceType.ANALOG_INPUT:
                        {
                            if (device.Device_ID == "AI0" || device.Device_ID == "AI1")
                                checkNgAll = ResultDAQ_A1orA2(xOwner, bSkip);
                            break;
                        }
                }
            }
            return checkNgAll;
        }

        private static List<bool> Result8FF(UcMainViewSs xOwner, bool bSkip = false)
        {
            List<bool> checkNgAll = new List<bool>();
            var StationDeviceMng = xOwner.MngStation.CnfStation.DevList;
            foreach (CpStnDevConfigure StnDevConfigure in StationDeviceMng)
            {
                if (StnDevConfigure.ID == string.Empty)
                    continue;
                var device = xOwner.TheSystemManager.MngHardware.DicDeviceManager.Where(w => w.Value.DeviceInfo.Device_ID == StnDevConfigure.ID).FirstOrDefault().Value.DeviceInfo;

                switch (device.DeviceType)
                {
                    case CpDeviceType.LVDT: checkNgAll = ResultFD_8(xOwner, bSkip);  break;
                    case CpDeviceType.LCRMETER: checkNgAll = ResultLCR_8(xOwner, bSkip);  break;
                    case CpDeviceType.ANALOG_INPUT:
                        {
                            if (device.Device_ID == "AI0")
                                checkNgAll = ResultDAQ_A0_8(xOwner, bSkip);
                            else if (device.Device_ID == "AI1" || device.Device_ID == "AI2")
                                checkNgAll = ResultDAQ_A1orA2(xOwner, bSkip);
                            break;
                        }
                }
            }
            return checkNgAll;
        }

        public static bool WritePLCCurrent(UcMainViewSs xOwner, bool bNg, string measure, int index, bool bSkip = false)
        {
            if(bSkip)
            {
                xOwner.PlcIF.SetMeasureValue(index, "0", bNg, bSkip);
                return false;
            }
            else if (measure != "")
            {
                xOwner.PlcIF.SetMeasureValue(index, (Convert.ToDouble(measure) / 100 * 1000).ToString(), bNg, bSkip);  //700 high current 3
                return false;
            }
            else
                return true;
        }

        public static List<bool> ResultFD_8(UcMainViewSs xOwner, bool bSkip = false)
        {
            List<bool> checkNgAll = new List<bool>();
            string measure;

            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_FD", 0, 1, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_FD", 1, 2, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_FD", 2, 3, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_FD", 3, 3, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_FD", 4, 3, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_FD", 5, 3, out measure, bSkip));

            return checkNgAll;
        }

        public static List<bool> ResultFD_7(UcMainViewSs xOwner, bool bSkip = false)
        {
            List<bool> checkNgAll = new List<bool>();
            string measure;

            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_FD", 0, 1, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_FD", 1, 2, out measure, bSkip));

            return checkNgAll;
        }

        public static List<bool> ResultLCR_8(UcMainViewSs xOwner, bool bSkip = false)
        {
            List<bool> checkNgAll = new List<bool>();
            string measure;

            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_LCR", 13, 1, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_LCR", 14, 2, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_LCR", 15, 3, out measure, bSkip));
            return checkNgAll;
        }

        public static List<bool> ResultLCR_7(UcMainViewSs xOwner, bool bSkip = false)
        {
            List<bool> checkNgAll = new List<bool>();
            string measure;

            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_LCR", 2, 1, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "GETDATA_LCR", 3, 2, out measure, bSkip));

            return checkNgAll;
        }

        public static List<bool> ResultDAQ_A0_8(UcMainViewSs xOwner, bool bSkip = false)
        {
            List<bool> checkNgAll = new List<bool>();
            string measureVoltage;
            string measure;

            checkNgAll.Add(WirtePLCMeasure(xOwner, "DCV_CP", 0, 1, out measureVoltage, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_D", 1, 2, out measure, bSkip));
            //700 CW
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_V", 2, 5, out measure, bSkip));
            checkNgAll.Add(WritePLCCurrent(xOwner, checkNgAll.Last(), measure, 3, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_V", 3, 6, out measure, bSkip));
            checkNgAll.Add(WritePLCCurrent(xOwner, checkNgAll.Last(), measure, 4, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_D", 0, 7, out measure, bSkip));
            //2000 CW
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_V", 0, 10, out measure, bSkip));
            checkNgAll.Add(WritePLCCurrent(xOwner, checkNgAll.Last(), measure, 8, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_V", 1, 11, out measure, bSkip));
            checkNgAll.Add(WritePLCCurrent(xOwner, checkNgAll.Last(), measure, 9, bSkip));

            return checkNgAll;
        }

        public static List<bool> ResultDAQ_A1orA2(UcMainViewSs xOwner, bool bSkip = false)
        {
            List<bool> checkNgAll = new List<bool>();
            string measure;

            checkNgAll.Add(WirtePLCMeasure(xOwner, "DCV_CP", 0, 1, out measure, bSkip));
            //700  CW
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_W", 1, 2, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_V", 2, 5, out measure, bSkip));
            checkNgAll.Add(WritePLCCurrent(xOwner, checkNgAll.Last(), measure, 3, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_V", 3, 6, out measure, bSkip));
            checkNgAll.Add(WritePLCCurrent(xOwner, checkNgAll.Last(), measure, 4, bSkip));
            //2000  CW
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_W", 0, 7, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_V", 0, 10, out measure, bSkip));
            checkNgAll.Add(WritePLCCurrent(xOwner, checkNgAll.Last(), measure, 8, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_V", 1, 11, out measure, bSkip));
            checkNgAll.Add(WritePLCCurrent(xOwner, checkNgAll.Last(), measure, 9, bSkip));
            //700  CCW
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_W", 2, 12, out measure, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_V", 0, 15, out measure, bSkip));
            checkNgAll.Add(WritePLCCurrent(xOwner, checkNgAll.Last(), measure, 13, bSkip));
            checkNgAll.Add(WirtePLCMeasure(xOwner, "PULSE_V", 1, 16, out measure, bSkip));
            checkNgAll.Add(WritePLCCurrent(xOwner, checkNgAll.Last(), measure, 14, bSkip));

            return checkNgAll;
        }



        public static List<PLCResult> ReadTotalResult_8(CpPlcIF plcIF)
        {
            List<PLCResult> lstData = new List<PLCResult>();
            bool Ng = false;
            bool Skip = false;
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(0, 1, out Ng, out Skip), Ng, Skip, "FD(mm) INPUT"));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(0, 2, out Ng, out Skip), Ng, Skip, "FD(mm) MIDDLE"));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(0, 3, out Ng, out Skip), Ng, Skip, "FD(mm) OUT"));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(1, 1, out Ng, out Skip), Ng, Skip, "CAPACITOR(nF) INPUT"));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(1, 2, out Ng, out Skip), Ng, Skip, "CAPACITOR(nF) MIDDLE"));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(1, 3, out Ng, out Skip), Ng, Skip, "CAPACITOR(nF) OUT"));

            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 2, out Ng, out Skip), Ng, Skip, "INPUT 700RPM CW DUTY(%) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 3, out Ng, out Skip), Ng, Skip, "INPUT 700RPM CW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 4, out Ng, out Skip), Ng, Skip, "INPUT 700RPM CW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 5, out Ng, out Skip), Ng, Skip, "INPUT 700RPM CW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 6, out Ng, out Skip), Ng, Skip, "INPUT 700RPM CW LOW Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 7, out Ng, out Skip), Ng, Skip, "INPUT 2000RPM CW DUTY(%) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 8, out Ng, out Skip), Ng, Skip, "INPUT 2000RPM CW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 9, out Ng, out Skip), Ng, Skip, "INPUT 2000RPM CW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 10, out Ng, out Skip), Ng, Skip, "INPUT 2000RPM CW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 11, out Ng, out Skip), Ng, Skip, "INPUT 2000RPM CW LOW Voltage(V) "));

            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 2, out Ng, out Skip), Ng, Skip, "MIDDLE 700RPM CW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 3, out Ng, out Skip), Ng, Skip, "MIDDLE 700RPM CW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 4, out Ng, out Skip), Ng, Skip, "MIDDLE 700RPM CW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 5, out Ng, out Skip), Ng, Skip, "MIDDLE 700RPM CW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 6, out Ng, out Skip), Ng, Skip, "MIDDLE 700RPM CW LOW Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 7, out Ng, out Skip), Ng, Skip, "MIDDLE 2000RPM CW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 8, out Ng, out Skip), Ng, Skip, "MIDDLE 2000RPM CW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 9, out Ng, out Skip), Ng, Skip, "MIDDLE 2000RPM CW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 10, out Ng, out Skip), Ng, Skip, "MIDDLE 2000RPM CW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 11, out Ng, out Skip), Ng, Skip, "MIDDLE 2000RPM CW LOW Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 12, out Ng, out Skip), Ng, Skip, "MIDDLE 700RPM CCW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 13, out Ng, out Skip), Ng, Skip, "MIDDLE 700RPM CCW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 14, out Ng, out Skip), Ng, Skip, "MIDDLE 700RPM CCW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 15, out Ng, out Skip), Ng, Skip, "MIDDLE 700RPM CCW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 16, out Ng, out Skip), Ng, Skip, "MIDDLE 700RPM CCW LOW Voltage(V) "));
            lstData.Add(new PLCResult(0.0, false, false, ""));


            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 2, out Ng, out Skip), Ng, Skip, "OUTPUT 700RPM CW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 3, out Ng, out Skip), Ng, Skip, "OUTPUT 700RPM CW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 4, out Ng, out Skip), Ng, Skip, "OUTPUT 700RPM CW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 5, out Ng, out Skip), Ng, Skip, "OUTPUT 700RPM CW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 6, out Ng, out Skip), Ng, Skip, "OUTPUT 700RPM CW LOW Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 7, out Ng, out Skip), Ng, Skip, "OUTPUT 2000RPM CW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 8, out Ng, out Skip), Ng, Skip, "OUTPUT 2000RPM CW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 9, out Ng, out Skip), Ng, Skip, "OUTPUT 2000RPM CW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 10, out Ng, out Skip), Ng, Skip, "OUTPUT 2000RPM CW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 11, out Ng, out Skip), Ng, Skip, "OUTPUT 2000RPM CW LOW Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 12, out Ng, out Skip), Ng, Skip, "OUTPUT 700RPM CCW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 13, out Ng, out Skip), Ng, Skip, "OUTPUT 700RPM CCW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 14, out Ng, out Skip), Ng, Skip, "OUTPUT 700RPM CCW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 15, out Ng, out Skip), Ng, Skip, "OUTPUT 700RPM CCW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(4, 16, out Ng, out Skip), Ng, Skip, "OUTPUT 700RPM CCW LOW Voltage(V) "));

            return lstData;
        }

        public static List<PLCResult> ReadTotalResult_7(CpPlcIF plcIF)
        {
            List<PLCResult> lstData = new List<PLCResult>();
            bool Ng = false;
            bool Skip = false;
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(0, 1, out Ng, out Skip), Ng, Skip, "FD(mm) ODD"));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(0, 2, out Ng, out Skip), Ng, Skip, "FD(mm) EVEN"));
            lstData.Add(new PLCResult(0.0, false, false, ""));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(1, 1, out Ng, out Skip), Ng, Skip, "CAPACITOR(nF) ODD"));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(1, 2, out Ng, out Skip), Ng, Skip, "CAPACITOR(nF) EVEN"));
            lstData.Add(new PLCResult(0.0, false, false, ""));

            lstData.Add(new PLCResult(0.0, false, false, ""));
            lstData.Add(new PLCResult(0.0, false, false, ""));
            lstData.Add(new PLCResult(0.0, false, false, ""));
            lstData.Add(new PLCResult(0.0, false, false, ""));
            lstData.Add(new PLCResult(0.0, false, false, ""));
            lstData.Add(new PLCResult(0.0, false, false, ""));
            lstData.Add(new PLCResult(0.0, false, false, ""));
            lstData.Add(new PLCResult(0.0, false, false, ""));
            lstData.Add(new PLCResult(0.0, false, false, ""));
            lstData.Add(new PLCResult(0.0, false, false, ""));

            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 2, out Ng, out Skip), Ng, Skip, "ODD 700RPM CW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 3, out Ng, out Skip), Ng, Skip, "ODD 700RPM CW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 4, out Ng, out Skip), Ng, Skip, "ODD 700RPM CW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 5, out Ng, out Skip), Ng, Skip, "ODD 700RPM CW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 6, out Ng, out Skip), Ng, Skip, "ODD 700RPM CW LOW Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 7, out Ng, out Skip), Ng, Skip, "ODD 2000RPM CW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 8, out Ng, out Skip), Ng, Skip, "ODD 2000RPM CW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 9, out Ng, out Skip), Ng, Skip, "ODD 2000RPM CW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 10, out Ng, out Skip), Ng, Skip, "ODD 2000RPM CW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 11, out Ng, out Skip), Ng, Skip, "ODD 2000RPM CW LOW Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 12, out Ng, out Skip), Ng, Skip, "ODD 700RPM CCW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 13, out Ng, out Skip), Ng, Skip, "ODD 700RPM CCW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 14, out Ng, out Skip), Ng, Skip, "ODD 700RPM CCW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 15, out Ng, out Skip), Ng, Skip, "ODD 700RPM CCW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(2, 16, out Ng, out Skip), Ng, Skip, "ODD 700RPM CCW LOW Voltage(V) "));
            lstData.Add(new PLCResult(0.0, false, false, ""));


            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 2, out Ng, out Skip), Ng, Skip, "EVEN 700RPM CW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 3, out Ng, out Skip), Ng, Skip, "EVEN 700RPM CW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 4, out Ng, out Skip), Ng, Skip, "EVEN 700RPM CW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 5, out Ng, out Skip), Ng, Skip, "EVEN 700RPM CW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 6, out Ng, out Skip), Ng, Skip, "EVEN 700RPM CW LOW Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 7, out Ng, out Skip), Ng, Skip, "EVEN 2000RPM CW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 8, out Ng, out Skip), Ng, Skip, "EVEN 2000RPM CW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 9, out Ng, out Skip), Ng, Skip, "EVEN 2000RPM CW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 10, out Ng, out Skip), Ng, Skip, "EVEN 2000RPM CW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 11, out Ng, out Skip), Ng, Skip, "EVEN 2000RPM CW LOW Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 12, out Ng, out Skip), Ng, Skip, "EVEN 700RPM CCW WIDTH(μs) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 13, out Ng, out Skip), Ng, Skip, "EVEN 700RPM CCW HIGH Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 14, out Ng, out Skip), Ng, Skip, "EVEN 700RPM CCW LOW Current(mA) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 15, out Ng, out Skip), Ng, Skip, "EVEN 700RPM CCW HIGH Voltage(V) "));
            lstData.Add(new PLCResult(plcIF.GetMeasureValue(3, 16, out Ng, out Skip), Ng, Skip, "EVEN 700RPM CCW LOW Voltage(V) "));
            lstData.Add(new PLCResult(0.0, false, false, ""));


            return lstData;
        }
    }
}
