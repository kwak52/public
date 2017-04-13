using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.Functions;
using CpTesterPlatform.CpTester;
using System.Linq.Expressions;
using CpTesterSs.UIManual;
using Dsu.Driver.Base;

namespace CpTesterPlatform.CpTesterSs
{
    public class CpPlcIF
    {
        public CpMngPlc MngPlcRead { get; set; }
        public CpMngPlc MngPlcWrite { get; set; }
        public int STATION { get; private set; }
        public bool PLC_START { get { return GetBit(Name(() => PLC_START)); } }
        public bool PLC_PASS { get { return GetBit(Name(() => PLC_PASS)); } }
        public bool PLC_STOP { get { return GetBit(Name(() => PLC_STOP)); } }
        public bool PLC_AUTO { get { return GetBit(Name(() => PLC_AUTO)); } }
        public bool PLC_READY { get { return GetBit(Name(() => PLC_READY)); } }
        public bool PLC_ALL_EMG { get { return GetBit(Name(() => PLC_ALL_EMG)); } }
        public bool PLC_ALL_STOP { get { return GetBit(Name(() => PLC_ALL_STOP)); } }

        public bool PC_RUNNING { get { return GetBit(Name(() => PC_RUNNING)); } set { SetBit(Name(() => PC_RUNNING), value); } }
        public bool PC_FINISHED { get { return GetBit(Name(() => PC_FINISHED)); } set { SetBit(Name(() => PC_FINISHED), value); } }
        public bool PC_STOP { get { return GetBit(Name(() => PC_STOP)); } set { SetBit(Name(() => PC_STOP), value); } }
        public bool PC_READY { get { return GetBit(Name(() => PC_READY)); } set { SetBit(Name(() => PC_READY), value); } }
        public bool PC_AUTO { get { return GetBit(Name(() => PC_AUTO)); } set { SetBit(Name(() => PC_AUTO), value); } }
        public bool PC_CONNECTORTEST { get { return GetBit(Name(() => PC_CONNECTORTEST)); } set { SetBit(Name(() => PC_CONNECTORTEST), value); } }
        public bool PC_ALL_STOP { get { return GetBit(Name(() => PC_ALL_STOP)); } set { SetBit(Name(() => PC_ALL_STOP), value); } }
        public bool PC_ALL_READY { get { return GetBit(Name(() => PC_ALL_READY)); } set { SetBit(Name(() => PC_ALL_READY), value); } }
        public bool PC_ALL_AUTO { get { return GetBit(Name(() => PC_ALL_AUTO)); } set { SetBit(Name(() => PC_ALL_AUTO), value); } }
        public bool PC_ALL_CONNECTOR_READY { get { return GetBit(Name(() => PC_ALL_CONNECTOR_READY)); } set { SetBit(Name(() => PC_ALL_CONNECTOR_READY), value); } }

        public int PLC_JUDGE { get { return GetWord(Name(() => PLC_JUDGE)); } set { SetWord(Name(() => PLC_JUDGE), value); } }
        public int PC_TOTAL_JUDGE { get { return GetWord(Name(() => PC_TOTAL_JUDGE)); } set { SetWord(Name(() => PC_TOTAL_JUDGE), value); } }

        public event UnloadingPassEventHandler UnloadingCloseHandler;
        public event UnloadingCloseEventHandler UnloadingPassHandler;
        public event UnloadingResultEventHandler UnloadingResultHandler;
        public event StopEventHandler StopHandler;
        private Dictionary<string, string> DicAddress = new Dictionary<string, string>();

        private const string sFD_INPUT_DATA = "FD_INPUT_OFFSET_DATA";
        private const string sFD_INPUT_SIGN = "FD_INPUT_OFFSET_SIGN";
        private const string sFD_MIDDLE_DATA = "FD_MIDDLE_OFFSET_DATA";
        private const string sFD_MIDDLE_SIGN = "FD_MIDDLE_OFFSET_SIGN";
        private const string sFD_OUTPUT_DATA = "FD_OUTPUT_OFFSET_DATA";
        private const string sFD_OUTPUT_SIGN = "FD_OUTPUT_OFFSET_SIGN";
        private const string sStation_Index = "INDEX_NO";
        private const string sUNLOADING_INDEX_NO = "UNLOADING_INDEX_NO";
        private const string sUNLOADING_DATA_GET = "UNLOADING_DATA_GET";
        private const string sUNLOADING_DATA_CLOSE = "UNLOADING_DATA_CLOSE";
        private const string sUNLOADING_PASS_MODE = "UNLOADING_PASS_MODE";
        private const string sFD_MASTER_SET = "FD_MASTER_SET";

        public CpPlcIF(int station)
        {
            STATION = station;
        }

        public string Name<T>(Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);
            return body.Member.Name;
        }

        public void AddItem(string name, string Address)
        {
            if (!name.Contains(':') || name.Split(':')[0] == "-" || name.Split(':')[1] == "")
                return;

            int stnNumber = Convert.ToInt16(name.Split(':')[0]);
            string symbol = name.Split(':')[1];

            if (!DicAddress.ContainsKey(symbol))
            {
                if (stnNumber == STATION || stnNumber == -1)  //stnNumber -1 일 경우 전체 모니터링에 해당함
                    DicAddress.Add(symbol, Address);
            }
        }

        private bool GetBit(string name)
        {
            if (MngPlcRead == null)
                return false;
            if (DicAddress.ContainsKey(name))
            {
                int value = MngPlcRead.ReadMonitor(DicAddress[name]);
                if (value != -1)
                    return Convert.ToBoolean(value);

                UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("Failed to GetBit PLC Device [{0}][{1}] Check HwConfig.cnf file", name, DicAddress[name]), ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
            else
                UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("Failed to GetBit PLC Device [{0}] Check HwConfig.cnf file", name), ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);

            return false;
        }

        private void SetBit(string name, bool bValue)
        {
            if (MngPlcWrite == null)
                return;
            if (DicAddress.ContainsKey(name))
                MngPlcWrite.WriteBitDevice(DicAddress[name], new int[1] { bValue ? 1 : 0 });
            else
                UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("Failed to SetBit PLC Device [{0}] Check HwConfig.cnf file", name), ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
        }

        public int GetWord(string name)
        {
            if (MngPlcRead == null)
                return -1;
            if (DicAddress.ContainsKey(name))
            {
                int value = MngPlcRead.ReadMonitor(DicAddress[name]);
                if (value != -1)
                    return Convert.ToInt32(value);

                UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("Failed to GetWord PLC Device [{0}][{1}] Check HwConfig.cnf file", name, DicAddress[name]), ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
            else
                UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("Failed to GetWord PLC Device [{0}] Check HwConfig.cnf file", name), ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
            return -1;
        }

        public void SetWord(string name, int Value)
        {
            if (MngPlcWrite == null)
                return;
            if (DicAddress.ContainsKey(name))
                MngPlcWrite.WriteDevice(DicAddress[name], Value);
            else
                UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("Failed to PLC SetWord Device [{0}] value {1} Check HwConfig.cnf file", name, Value), ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
        }

        public string GetProductId()
        {
            int[] arrData = new int[12];
            for (int i = 0; i < arrData.Length; i++)
                arrData[i] = GetWord("PLC_MODEL_NAME" + i.ToString());

            if (arrData.Where(w => w == -1).Count() > 0)
                return "";

            string ProductId = CpUtil.GetStringFromASCII(arrData);

            return ProductId.Split(' ')[0];
        }

        public void CreatEventPLC()
        {
            MngPlcRead.EventPLC += MngPlcRead_EventPLC;
        }

        private void MngPlcRead_EventPLC(string address, int value)
        {
            EventPLCFdOffset(address);
            MasterSet(address, value);

            EventPLCEmergency(address, value);

            EventPLCStationId(address);

            if (STATION == CpUtil.MonitoringPLC)
            {
                EventPLCTotolResult(address, value);
                EventPassMode(address, value);
            }
        }

        private void EventPassMode(string address, int value)
        {
            if ((DicAddress.ContainsKey(sUNLOADING_PASS_MODE) && DicAddress[sUNLOADING_PASS_MODE] == address))
                UnloadingPassHandler?.Invoke(value == 1 ? true : false);
        }

        private void MasterSet(string address, int value)
        {
            if ((DicAddress.ContainsKey(sFD_MASTER_SET) && DicAddress[sFD_MASTER_SET] == address))
                CpUtil.FD_MASTER_USE = value != 0 ? true : false;
        }

        private void EventPLCTotolResult(string address, int value)
        {
            if ((DicAddress.ContainsKey(sUNLOADING_DATA_CLOSE) && DicAddress[sUNLOADING_DATA_CLOSE] == address))
                UnloadingCloseHandler?.Invoke(true);

            if ((DicAddress.ContainsKey(sUNLOADING_DATA_GET) && DicAddress[sUNLOADING_DATA_GET] == address) && value == 1)
            {
                if (CpUtil.InitStart)
                {
                    CpUtil.InitStart = false;
                    return;
                }
                string Message = "";
                int CpStation = 0;

                if (DriverBaseGlobals.IsLine8FF())
                {
                    Message = GetMessage8FF();
                    CpStation = GetStation8FF();
                }
                else if (DriverBaseGlobals.IsLine7DCT())
                {
                    Message = GetMessage7DCT();
                    CpStation = GetStation7DCT();
                }

                Task.Run(() =>
                {
                    int StationIndex = GetWord(sUNLOADING_INDEX_NO);
                    var results = DriverBaseGlobals.IsLine8FF() ? PlcUtil.ReadTotalResult_8(this) : PlcUtil.ReadTotalResult_7(this);
                    UnloadingResultHandler?.Invoke(results, PLC_JUDGE != 0 ? true : false
                        , Message + " StationIndex " + StationIndex.ToString(), CpStation, StationIndex);
                });
            }
        }

        private int GetStation8FF()
        {
            int CpStation = 0;

            if (PLC_JUDGE == 0) CpStation = 0;           //  "Total OK";
            else if (PLC_JUDGE == 1) CpStation = 2;      //"STN2 LCR Check NG";
            else if (PLC_JUDGE == 2) CpStation = -1;      //"STN2 Color Check NG";
            else if (PLC_JUDGE == 3) CpStation = 3;      //"STN3 Station Function NG";
            else if (PLC_JUDGE == 4) CpStation = 4;      //"STN3 Station Function NG";
            else if (PLC_JUDGE == 5) CpStation = 5;      //"STN5 Station Function NG";
            else if (PLC_JUDGE == 6) CpStation = 1;      //"STN1 FD Check NG";
            else if (PLC_JUDGE == 7) CpStation = -1;      //"STN1 FD Connector NG";
            return CpStation;
        }

        private string GetMessage8FF()
        {
            string Message = "";
            if (PLC_JUDGE == 0) Message =        "Total OK";
            else if (PLC_JUDGE == 1) Message = "STN2 LCR Check NG";
            else if (PLC_JUDGE == 2) Message = "STN2 Color Check NG";
            else if (PLC_JUDGE == 3) Message = "STN3 Station Function NG";
            else if (PLC_JUDGE == 4) Message = "STN3 Station Function NG";
            else if (PLC_JUDGE == 5) Message = "STN5 Station Function NG";
            else if (PLC_JUDGE == 6) Message = "STN1 FD Check NG";
            else if (PLC_JUDGE == 7) Message = "STN1 FD Connector NG";
            else
                Message = "NG";

            return Message;
        }

        private int GetStation7DCT()
        {
            int CpStation = 0;

            if (PLC_JUDGE == 0) CpStation = 0;                 //  "Total OK";
            else if (PLC_JUDGE == 1) CpStation = 1;            //"STN1 FD Check NG";
            else if (PLC_JUDGE == 2) CpStation = 2;            //"STN1 LCR Check NG";
            else if (PLC_JUDGE == 3) CpStation = 12;            //"STN1 FD & LCR Check NG";
            else if (PLC_JUDGE == 4) CpStation = -1;            //"STN2 LEAK ODD Check NG";
            else if (PLC_JUDGE == 5) CpStation = -1;            //"STN2 LEAK EVEN Check NG";
            else if (PLC_JUDGE == 6) CpStation = 3;            //"STN4 ODD Function NG";
            else if (PLC_JUDGE == 7) CpStation = 4;            //"STN4 EVEN Function NG";
            else if (PLC_JUDGE == 8) CpStation = 34;            //"STN4 ODD & EVEN Function NG";
            else if (PLC_JUDGE == 9) CpStation = -1;            //"STN4 Connector NG";
            else if (PLC_JUDGE == 10) CpStation = -1;            //"STN5 Vistion NG";

            return CpStation;
        }

        private string GetMessage8FFBackup()
        {
            string Message = "";
            int Station = 0;
            if (PLC_JUDGE == 0) Message = "Total OK";
            else if (PLC_JUDGE == 1)
            {
                Message = "STN2 LCR Check NG";
                Station = 2;
            }
            else if (PLC_JUDGE == 2)
            {
                Message = "STN2 Color Check NG";
                Station = 2;
            }
            else if (PLC_JUDGE == 3)
            {
                Message = "STN3 Station Function NG";
                Station = 3;
            }
            else if (PLC_JUDGE == 4)
            {
                Message = "STN3 Station Function NG";
                Station = 3;
            }
            else if (PLC_JUDGE == 5)
            {
                Message = "STN5 Station Function NG";
                Station = 5;
            }
            else if (PLC_JUDGE == 6)
            {
                Message = "STN1 FD Check NG";
                Station = 1;
            }
            else if (PLC_JUDGE == 7)
            {
                Message = "STN1 FD Connector NG";
                Station = 1;
            }
            else
                Message = "NG";

            return Station.ToString() + ";" + Message;
        }

        private string GetMessage7DCT()
        {
            string Message = "";
            if (PLC_JUDGE == 0) Message =        "Total OK";
            else if (PLC_JUDGE == 1) Message = "STN1 FD Check NG";
            else if (PLC_JUDGE == 2) Message = "STN1 LCR Check NG";
            else if (PLC_JUDGE == 3) Message = "STN1 FD & LCR Check NG";
            else if (PLC_JUDGE == 4) Message = "STN2 LEAK ODD Check NG";
            else if (PLC_JUDGE == 5) Message = "STN2 LEAK EVEN Check NG";
            else if (PLC_JUDGE == 6) Message = "STN4 ODD Function NG";
            else if (PLC_JUDGE == 7) Message = "STN4 EVEN Function NG";
            else if (PLC_JUDGE == 8) Message = "STN4 ODD & EVEN Function NG";
            else if (PLC_JUDGE == 9) Message = "STN4 Connector NG";
            else if (PLC_JUDGE == 10) Message = "STN5 Vistion NG";
            else
                Message = "NG";

            return Message;
        }

        public void SetMeasureValue(int measureIndex, string Value, bool JudgeNg, bool bSkip = false)
        {
   

            string deviceInt = "PC_MEASURE_INT_" + measureIndex.ToString();
            string deviceDot = "PC_MEASURE_DOT_" + measureIndex.ToString();
            string deviceJudge = "PC_MEASURE_JUDGE_" + measureIndex.ToString();

            if (bSkip)
            {
                SetWord(deviceInt, 0);
                SetWord(deviceDot, 0);
                SetWord(deviceJudge, 2);
            }
            else
            {
                if (Value == string.Empty)
                    return;

                if (Value.Length > 9)
                {
                    UtilTextMessageEdits.UtilTextMsgToConsole(string.Format("Failed to SetMeasureValue PLC Write Data [{0}] Check data size", Value), ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                    return;
                }

                SetWord(deviceInt, Convert.ToInt32(Value.Split('.')[0]));
                SetWord(deviceJudge, JudgeNg ? 1 : 0);

                if (Value.Contains("."))
                    SetWord(deviceDot, Convert.ToInt32(Convert.ToDouble("0." + Value.Split('.')[1]) * 10000));
                else
                    SetWord(deviceDot, 0);
            }
        }

        public double GetMeasureValue(int stn, int measureIndex, out bool bJudgeNG, out bool bJudgeSkip)
        {

            string deviceInt = string.Format("{0}_PC_MEASURE_INT_{1}", stn, measureIndex);
            string deviceDot = string.Format("{0}_PC_MEASURE_DOT_{1}", stn, measureIndex);
            string deviceJudge = string.Format("{0}_PC_MEASURE_JUDGE_{1}", stn, measureIndex);

            int data = GetWord(deviceInt);
            int dot = GetWord(deviceDot);
            int judge = GetWord(deviceJudge);
            bJudgeNG = judge == 0 ? false : true;
            bJudgeSkip = judge == 2 ? true : false;

            return data + (double)dot / 10000;
        }

        public void SendMarkingData()
        {
            string mark3, mark4;

            CpPartID cpPartID;
            if (CpUtil.Station_PartID.ContainsKey(CpUtil.GetStationindex(STATION))) // "0" FD Station
                cpPartID = CpUtil.Station_PartID[CpUtil.GetStationindex(STATION)];
            else
            {
                DateTime dt = GetMarkingData(out mark3, out mark4);
                cpPartID = new CpPartID(dt, mark3, mark4);
            }

            short[] arrMark3 = CpUtil.HexStringToBytes(cpPartID.Mark3);
            short[] arrMark4 = CpUtil.HexStringToBytes(cpPartID.Mark4);

            for (int i = 0; i < arrMark3.Length; i++)
                SetWord("PC_MARKING3_" + i.ToString(), Convert.ToInt16(arrMark3[i]));
            for (int i = 0; i < arrMark4.Length; i++)
                SetWord("PC_MARKING4_" + i.ToString(), Convert.ToInt16(arrMark4[i]));
        }

        public DateTime GetMarkingData(out string mark3, out string mark4)
        {
            DateTime dt = DateTime.Now;
            if (dt.Hour < 6)
                dt = dt.AddDays(-1);

            mark3 = string.Format("  {0}{1}{2}{3}", 4, dt.ToString("yy").Substring(1, 1), Convert.ToInt16(dt.ToString("MM")) + 40, dt.ToString("dd"));
            mark4 = string.Format("  {0}{1}{2}{3}  ", dt.ToString("HH"), dt.ToString("mm"), dt.ToString("ss"), "c");
            if (dt.Hour >= 6 && dt.Hour < 14)
                mark3 += "X1  ";
            else if (dt.Hour >= 14 && dt.Hour < 22)
                mark3 += "X2  ";
            else
                mark3 += "X3  ";

            return dt;
        }

        public void UpdateMESPartId()
        {
            string mark3, mark4;
            DateTime dt = GetMarkingData(out mark3, out mark4);
            CpPartID cpPartID = new CpPartID(dt, mark3, mark4);

            if (!CpUtil.Station_PartID.ContainsKey(CpUtil.GetStationindex(0))) // "0" FD Station
                CpUtil.Station_PartID.Add(CpUtil.GetStationindex(0), cpPartID);
            else
                CpUtil.Station_PartID[CpUtil.GetStationindex(0)] = cpPartID;
        }

        private void EventPLCStationId(string address)
        {
            if ((DicAddress.ContainsKey(sStation_Index) && DicAddress[sStation_Index] == address))
            {
                int data = MngPlcRead.ReadMonitor(DicAddress[sStation_Index]);
                CpUtil.UpdateStationIndex(STATION, data);
            }
        }

        private void EventPLCFdOffset(string address)
        {
            if ((DicAddress.ContainsKey(sFD_INPUT_DATA) && DicAddress[sFD_INPUT_DATA] == address)
                || (DicAddress.ContainsKey(sFD_INPUT_SIGN) && DicAddress[sFD_INPUT_SIGN] == address)
                || (DicAddress.ContainsKey(sFD_MIDDLE_DATA) && DicAddress[sFD_MIDDLE_DATA] == address)
                || (DicAddress.ContainsKey(sFD_MIDDLE_SIGN) && DicAddress[sFD_MIDDLE_SIGN] == address)
                || (DicAddress.ContainsKey(sFD_OUTPUT_DATA) && DicAddress[sFD_OUTPUT_DATA] == address)
                || (DicAddress.ContainsKey(sFD_OUTPUT_SIGN) && DicAddress[sFD_OUTPUT_SIGN] == address))
            {
                int data = MngPlcRead.ReadMonitor(DicAddress[sFD_INPUT_DATA]);
                int sign = MngPlcRead.ReadMonitor(DicAddress[sFD_INPUT_SIGN]);
                CpUtil.FD_INPUT_OFFSET = GetDataOffset(data, sign);

                data = MngPlcRead.ReadMonitor(DicAddress[sFD_MIDDLE_DATA]);
                sign = MngPlcRead.ReadMonitor(DicAddress[sFD_MIDDLE_SIGN]);
                CpUtil.FD_MIDDLE_OFFSET = GetDataOffset(data, sign);

                data = MngPlcRead.ReadMonitor(DicAddress[sFD_OUTPUT_DATA]);
                sign = MngPlcRead.ReadMonitor(DicAddress[sFD_OUTPUT_SIGN]);
                CpUtil.FD_OUTPUT_OFFSET = GetDataOffset(data, sign);
            }
        }

        private void EventPLCEmergency(string address, int value)
        {
            if (DicAddress.ContainsKey(Name(() => PLC_ALL_EMG)))
            {
                if (DicAddress[Name(() => PLC_ALL_EMG)] == address)
                {
                    if (value == 0)
                        StopHandler?.Invoke(false);
                    else
                        StopHandler?.Invoke(true);
                }
            }
        }

        private double GetDataOffset(int data, int sign)
        {
            double Sign = sign == 1 ? -1 : 1;
            return (double)(data * Sign) / 1000.0; //sign is 1 => minus
        }

        public void InitialInterface()
        {
            PC_TOTAL_JUDGE = 0;  // 1 is NG
            PC_READY = true;
            PC_AUTO = true;
            PC_STOP = false;


            PC_ALL_READY = true;
            PC_ALL_AUTO = true;
            PC_ALL_STOP = false;

            PC_FINISHED = false;
            PC_RUNNING = false;
        }

        public void StopInterface()
        {
            PC_STOP = true;
            PC_ALL_STOP = true;

            PC_READY = false;
            PC_AUTO = false;
            PC_ALL_READY = false;
            PC_ALL_AUTO = false;
            PC_RUNNING = false;
            PC_FINISHED = false;

            ResetData();
        }

        public void ResetData()
        {
            for (int i = 0; i < 18; i++)
            {
                string deviceInt = "PC_MEASURE_INT_" + i.ToString();
                string deviceDot = "PC_MEASURE_DOT_" + i.ToString();
                string deviceJudge = "PC_MEASURE_JUDGE_" + i.ToString();
                if (DicAddress.ContainsKey(deviceInt))
                    SetWord(deviceInt, 0);
                if (DicAddress.ContainsKey(deviceDot))
                    SetWord(deviceDot, 0);
                if (DicAddress.ContainsKey(deviceJudge))
                    SetWord(deviceJudge, 0);
            }
        }

        public void AutoStartInterface()
        {
            PC_STOP = false;
            PC_READY = true;
            PC_AUTO = true;
            PC_ALL_STOP = false;
            PC_ALL_READY = true;
            PC_ALL_AUTO = true;
        }
    }
}
