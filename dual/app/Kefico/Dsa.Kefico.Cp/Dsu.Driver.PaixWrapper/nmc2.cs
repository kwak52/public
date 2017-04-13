#pragma warning disable 1591

using System.Runtime.InteropServices;     // DLL support
namespace Dsu.Driver.PaixMotionControler
{
    // NMC2 Equip Type
    public enum EquipmentType
    {

        NMC2_220S = 0,
        NMC2_420S = 1,
        NMC2_620S = 2,
        NMC2_820S = 3,

        NMC2_220_DIO32 = 4,
        NMC2_220_DIO64 = 5,
        NMC2_420_DIO32 = 6,
        NMC2_420_DIO64 = 7,
        NMC2_820_DIO32 = 8,
        NMC2_820_DIO64 = 9,

        NMC2_DIO32 = 10,
        NMC2_DIO64 = 11,
        NMC2_DIO96 = 12,
        NMC2_DIO128 = 13,
        NMC2_220,				//14
        NMC2_420,				//15
        NMC2_620,				//16
        NMC2_820,				//17
        NMC2_620_DIO32,			//18
        NMC2_620_DIO64,			//19
    }

    public class NMC2
    {
        // NMC2 Enum Type
        public const short EQUIP_TYPE_NMC_2_AXIS = 0x0001;
        public const short EQUIP_TYPE_NMC_4_AXIS = 0x0003;
        public const short EQUIP_TYPE_NMC_6_AXIS = 0x0007;
        public const short EQUIP_TYPE_NMC_8_AXIS = 0x000F;
        // 16/16
        public const short EQUIP_TYPE_NMC_IO_32 = 0x0010;
        // 32/32
        public const short EQUIP_TYPE_NMC_IO_64 = 0x0030;
        // 48/48
        public const short EQUIP_TYPE_NMC_IO_96 = 0x0070;
        // 64/64
        public const short EQUIP_TYPE_NMC_IO_128 = 0x00F0;
        // 80/80
        public const short EQUIP_TYPE_NMC_IO_160 = 0x01F0;
        // 96/96
        public const short EQUIP_TYPE_NMC_IO_192 = 0x03F0;
        // 112/112
        public const short EQUIP_TYPE_NMC_IO_224 = 0x07F0;
        // 128/128
        public const short EQUIP_TYPE_NMC_IO_256 = 0x0FF0;

        public const short EQUIP_TYPE_NMC_IO_IE = 0x1000;
        public const short EQUIP_TYPE_NMC_IO_OE = 0x2000;

        public const short EQUIP_TYPE_NMC_M_IO_8 = 0x4000;

        // 모든 함수의 리턴값 
        public const short NMC_CONTI_BUF_FULL = -15;
        public const short NMC_CONTI_BUF_EMPTY = -14;
        public const short NMC_INTERPOLATION = -13;
        public const short NMC_FILE_LOAD_FAIL = -12;
        public const short NMC_ICMP_LOAD_FAIL = -11;
        public const short NMC_NOT_EXISTS = -10;
        public const short NMC_CMDNO_ERROR = -9;
        public const short NMC_NOTRESPONSE = -8;
        public const short NMC_BUSY = -7;
        public const short NMC_COMMERR = -6;
        public const short NMC_SYNTAXERR = -5;
        public const short NMC_INVALID = -4;
        public const short NMC_UNKOWN = -3;
        public const short NMC_SOCKINITERR = -2;
        public const short NMC_NOTCONNECT = -1;
        public const short NMC_OK = 0;

        // STOP MODE
        public const short NMC_STOP_OK = 0;
        public const short NMC_STOP_EMG = 1;
        public const short NMC_STOP_MLIMIT = 2;
        public const short NMC_STOP_PLIMIT = 3;
        public const short NMC_STOP_ALARM = 4;
        public const short NMC_STOP_NEARORG = 5;
        public const short NMC_STOP_ENCZ = 6;

        // HOME MODE
        public const short NMC_HOME_LIMIT_P = 0;
        public const short NMC_HOME_LIMIT_M = 1;
        public const short NMC_HOME_NEAR_P = 2;
        public const short NMC_HOME_NEAR_M = 3;
        public const short NMC_HOME_Z_P = 4;
        public const short NMC_HOME_Z_M = 5;

        public const short NMC_HOME_USE_Z = 0x80;

        public const short NMC_END_NONE = 0x00;
        public const short NMC_END_CMD_CLEAR_A_OFFSET = 0x01;
        public const short NMC_END_ENC_CLEAR_A_OFFSET = 0x02;
        public const short NMC_END_CMD_CLEAR_B_OFFSET = 0x04;
        public const short NMC_END_ENC_CLEAR_B_OFFSET = 0x08;

        // LOG
        public const short NMC_LOG_NONE = 0;
        public const short NMC_LOG_DEV = 0x01;
        public const short NMC_LOG_MO_MOV = 0x02;// 모션함수중 MOVE
        public const short NMC_LOG_MO_SET = 0x04;// 모션함수중 GET
        public const short NMC_LOG_MO_GET = 0x08;// 모션함수중 SET
        public const short NMC_LOG_MO_EXPRESS = 0x10;// 모션함수중 각종 상태값 읽는(빈번히 발생)
        public const short NMC_LOG_IO_SET = 0x20;
        public const short NMC_LOG_IO_GET = 0x40;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCPARALOGIC
        {
            public short nEmg;			 // EMG
            public short nEncCount;		// 엔코더 카운트 모드

            public short nEncDir;		// 엔코더 카운트 방향
            public short nEncZ;			// 엔코더 Z

            public short nNear;			// NEAR(HOME)
            public short nMLimit;		// - Limit

            public short nPLimit;		// + Limit
            public short nAlarm;		// Alarm

            public short nInp;			// INPOSITION
            public short nSReady;		// Servo Ready

            public short nPulseMode;	// 1p/2p Mode
            //-------------------------------------------------------------

            public short nLimitStopMode; // Limit stop mode
            public short nBusyOff;		// Busy off mode

            public short nSWEnable;		// sw limit 활성화 여부
            //-------------------------------------------------------------
            public double dSWMLimitPos;
            public double dSWPLimitPos;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCPARALOGICEX
        {
            public short nEmg;			 // EMG
            public short nEncCount;		// 엔코더 카운트 모드

            public short nEncDir;		// 엔코더 카운트 방향
            public short nEncZ;			// 엔코더 Z

            public short nNear;			// NEAR(HOME)
            public short nMLimit;		// - Limit

            public short nPLimit;		// + Limit
            public short nAlarm;		// Alarm

            public short nInp;			// INPOSITION
            public short nSReady;		// Servo Ready

            public short nPulseMode;	// 1p/2p Mode
            //-------------------------------------------------------------

            public short nLimitStopMode; // Limit stop mode
            public short nBusyOff;		// Busy off mode

            public short nSWEnable;		// sw limit 활성화 여부
            //-------------------------------------------------------------
            public double dSWMLimitPos;
            public double dSWPLimitPos;
            //-------------------------------------------------------------
            // 원점 완료상태 해지 사용여부
            public short nHDoneCancelAlarm;		    // Alarm 발생 시 사용여부
            public short nHDoneCancelServoOff;	    // Servo Off 시 사용여부
            public short nHDoneCancelCurrentOff;	// Current Off 시 사용여부
            public short nHDoneCancelServoReady;	// Servo Ready 해제 시 사용여부
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public short[] nDummy;                //예약
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCPARASPEED
        {
            public double dStart;
            public double dAcc;
            public double dDec;
            public double dDrive;
            public double dJerkAcc;
            public double dJerkDec;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCAXESMOTIONOUT
        {
            /// 모터 전류 출력 상태(0=OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nCurrentOn;
            /// ServoOn 출력 상태(0=OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nServoOn;
            /// DCC 출력 상태(0=OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nDCCOn;
            /// Reset Alarm 출력 상태(0=OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nAlarmResetOn;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCAXESEXPR
        {
            /// BUSY : Pulse 출력 상태(0:Idle, 1:Busy)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nBusy;
            /// Error 발생 여부(0:None error, 1:Error)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nError;
            /// Near(Home) Sensor 입력 상태(0:OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nNear;
            /// + Limit Sensor 입력 상태(0:OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nPLimit;
            /// - Limit Sensor 입력 상태(0:OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nMLimit;
            /// Alarm Sensor 입력 상태(0:OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nAlarm;
            /// 그룹별 EMG 입력 상태(0:OFF, 1:ON), Emergency는 그룹별로 동작 되며, 4개의 묶음으로 그룹지어 집니다. 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] nEmer;
            /// Software + Limit 입력 상태(0:OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nSwPLimit;
            /// Inposition 입력 상태(0:OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nInpo;
            /// 홈 서치 동작 여부(0:동작중, 1: None)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nHome;

            /// Encoder Z 입력 상태(0:OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nEncZ;
            /// Near 입력 상태 : 구NMC-403S 에서만 지원
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nOrg;
            /// Servo Ready 입력 상태(0:OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nSReady;

            /// 연속 보간 상태(0=완료, 1=동작중)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] nContStatus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public short[] nDummy;
            /// Software – Limit 상태(0:OFF, 1:ON)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nSwMLimit;

            /// 엔코더 펄스(UnitPerPulse 적용 안된 펄스 값
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] lEnc;
            /// 지령 펄스(UnitPerPulse 적용 안된 펄스 값
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] lCmd;
            /// 엔코더 위치(UnitPerPulse 적용된 위치 값
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public double[] dEnc;
            /// 지령 위치(UnitPerPulse 적용 안된 위치 값
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public double[] dCmd;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] dummy;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCSTOPMODE
        {
            // 1 축
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nEmg;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nMLimit;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nPLimit;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nAlarm;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nNear;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nEncZ;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCCONTSTATUS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] nStatus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] nExeNodeNo;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCHOMEFLAG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nSrchFlag;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] nStatusFlag;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCEQUIPLIST
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public int[] lIp;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public int[] lModelType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public short[] nMotionType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public short[] nDioType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public short[] nEXDIo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public short[] nMDio;

        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCMAPDATA
        {
            int nMapCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 52)]
            public int[] lMapData;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 52)]
            public double[] dMapData;
        };


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCCONTISTATUS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] nContiRun;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] nContiWait;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] nContiRemainBuffNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public ulong[] nContiExecutionNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public short[] nDummy;
        };
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_OpenDevice")]
        public static extern short OpenDevice(short nNmcNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_OpenDeviceEx")]
        public static extern short OpenDeviceEx(short nNmcNo, long lWaitTime);
        [DllImport("NMC2.dll", EntryPoint = "nmc_CloseDevice")]
        public static extern void CloseDevice(short nNmcNo);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetParaLogic")]
        public static extern short GetParaLogic(short nNmcNo, short nAxisNo, out NMCPARALOGIC pLogic);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetParaLogicEx")]
        public static extern short GetParaLogicEx(short nNmcNo, short nAxisNo, out NMCPARALOGICEX pLogicEx);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetParaSpeed")]
        public static extern short GetParaSpeed(short nNmcNo, short nAxisNo, out NMCPARASPEED pSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetRingCountMode")]
        public static extern short GetRingCountMode(short nNmcNo, short nAxisNo, out int plMaxPulse, out int plMaxEncoder, out short pnRingMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetParaTargetPos")]
        public static extern short GetParaTargetPos(short nNmcNo, short nAxisNo, out double pdTargetPos);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDriveAxesSpeed")]
        public static extern short GetDriveAxesSpeed(short nNmcNo, double[] pDrvSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetAxesMotionOut")]
        public static extern short GetAxesMotionOut(short nNmcNo, out NMCAXESMOTIONOUT pOutStatus);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEmgLogic")]
        public static extern short SetEmgLogic(short nNmcNo, short nGroupNo, short nLogic);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetPlusLimitLogic")]
        public static extern short SetPlusLimitLogic(short nNmcNo, short nAxisNo, short nLogic);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMinusLimitLogic")]
        public static extern short SetMinusLimitLogic(short nNmcNo, short nAxisNo, short nLogic);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetSWLimitLogic")]
        public static extern short SetSWLimitLogic(short nNmcNo, short nAxisNo, short nUse, double dSwMinusPos, double dSwPlusPos);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetSWLimitLogicEx")]
        public static extern short SetSWLimitLogicEx(short nNmcNo, short nAxisNo, short nUse, double dSwMinusPos, double dSwPlusPos, short nOpt);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetAlarmLogic")]
        public static extern short SetAlarmLogic(short nNmcNo, short nAxisNo, short nLogic);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetNearLogic")]
        public static extern short SetNearLogic(short nNmcNo, short nAxisNo, short nLogic);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetInPoLogic")]
        public static extern short SetInPoLogic(short nNmcNo, short nAxisNo, short nLogic);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetSReadyLogic")]
        public static extern short SetSReadyLogic(short nNmcNo, short nAxisNo, short nLogic);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEncoderCount")]
        public static extern short SetEncoderCount(short nNmcNo, short nAxisNo, short nCountMode);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEncoderDir")]
        public static extern short SetEncoderDir(short nNmcNo, short nAxisNo, short nCountDir);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEncoderMode")]
        public static extern short SetEncoderMode(short nNmcNo, short nAxisNo, short nMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEncoderZLogic")]
        public static extern short SetEncoderZLogic(short nNmcNo, short nAxisNo, short nLogic);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetHomeDoneAutoCancel")]
        public static extern short GetHomeDoneAutoCancel(short nNmcNo, short nAxisNo, out short pnAlarm, out short pnServoOff, out short pnCurrentOff, out short pnServoReady);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetHomeDoneAutoCancel")]
        public static extern short SetHomeDoneAutoCancel(short nNmcNo, short nAxisNo, short nAlarm, short nServoOff, short nCurrentOff, short nServoReady);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetParaLogic")]
        public static extern short SetParaLogic(short nNmcNo, short nAxisNo, out NMCPARALOGIC pLogic);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetParaLogicEx")]
        public static extern short SetParaLogicEx(short nNmcNo, short nAxisNo, out NMCPARALOGICEX pLogicEx);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetParaLogicFile")]
        public static extern short SetParaLogicFile(short nNmcNo, char[] pStr);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetPulseMode")]
        public static extern short SetPulseMode(short nNmcNo, short nAxisNo, short nMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetPulseLogic")]
        public static extern short SetPulseLogic(short nNmcNo, short nAxisNo, short nLogic);
        [DllImport("NMC2.dll", EntryPoint = "nmc_Set2PulseDir")]
        public static extern short Set2PulseDir(short nNmcNo, short nAxisNo, short nDir);
        [DllImport("NMC2.dll", EntryPoint = "nmc_Set1PulseDir")]
        public static extern short Set1PulseDir(short nNmcNo, short nAxisNo, short nDir);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetPulseActive")]
        public static extern short SetPulseActive(short nNmcNo, short nAxisNo, short nPulseActive);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetSCurveSpeed")]
        public static extern short SetSCurveSpeed(short nNmcNo, short nAxisNo, double dStartSpeed,
                                        double dAcc, double dDec, double dDriveSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetSpeed")]
        public static extern short SetSpeed(short nNmcNo, short nAxisNo, double dStartSpeed,
                              double dAcc, double dDec, double dDriveSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetOverrideRunSpeed")]
        public static extern short SetOverrideRunSpeed(short nNmcNo, short nAxisNo, double dAcc, double dDec, double dDriveSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetOverrideDriveSpeed")]
        public static extern short SetOverrideDriveSpeed(short nNmcNo, short nAxisNo, double dDriveSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetAccSpeed")]
        public static extern short SetAccSpeed(short nNmcNo, short nAxisNo, double dAcc);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDecSpeed")]
        public static extern short SetDecSpeed(short nNmcNo, short nAxisNo, double dDec);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_AbsMove")]
        public static extern short AbsMove(short nNmcNo, short nAxisNo, double dPos);
        [DllImport("NMC2.dll", EntryPoint = "nmc_RelMove")]
        public static extern short RelMove(short nNmcNo, short nAxisNo, double dAmount);
        [DllImport("NMC2.dll", EntryPoint = "nmc_VelMove")]
        public static extern short VelMove(short nNmcNo, short nAxisNo, double dPos, double dDrive, short nMode);

        [DllImport("NMC2.dll", EntryPoint = "nmc_AbsOver")]
        public static extern short AbsOver(short nNmcNo, short nAxisNo, double dPos);

        [DllImport("NMC2.dll", EntryPoint = "nmc_VarRelMove")]
        public static extern short VarRelMove(short nNmcNo, short nAxisCount, short[] pnAxisNo, double[] pdAmount);
        [DllImport("NMC2.dll", EntryPoint = "nmc_VarAbsMove")]
        public static extern short VarAbsMove(short nNmcNo, short nAxisCount, short[] pnAxisNo, double[] pdPosList);
        [DllImport("NMC2.dll", EntryPoint = "nmc_VarAbsOver")]
        public static extern short VarAbsOver(short nNmcNo, short nAxisCount, short[] pnAxisNo, double[] pdPosList);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_JogMove")]
        public static extern short JogMove(short nNmcNo, short nAxis, short Dnir);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SuddenStop")]
        public static extern short SuddenStop(short nNmcNo, short nAxisNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_DecStop")]
        public static extern short DecStop(short nNmcNo, short nAxisNo);

        [DllImport("NMC2.dll", EntryPoint = "nmc_AllAxisStop")]
        public static extern short AllAxisStop(short nNmcNo, short nMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_MultiAxisStop")]
        public static extern short MultiAxisStop(short nNmcNo, short nCount, short[] pnAxisSelect, short nMode);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetAxesExpress")]
        public static extern short GetAxesExpress(short nNmcNo, out NMCAXESEXPR pNmcData);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetStopInfo")]
        public static extern short GetStopInfo(short nNmcNo, short[] pnStopMode);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetCmdPos")]
        public static extern short SetCmdPos(short nNmcNo, short nAxisNo, double dPos);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEncPos")]
        public static extern short SetEncPos(short nNmcNo, short nAxisNo, double dPos);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetCmdEncPos")]
        public static extern short SetCmdEncPos(short nNmcNo, short nAxisNo, double dPos);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_HomeMove")]
        public static extern short HomeMove(short nNmcNo, short nAxisNo, short nHomeMode, short nHomeEndMode, double dOffset, short nReserve);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetHomeStatus")]
        public static extern short GetHomeStatus(short nNmcNo, out NMCHOMEFLAG pHomeFlag);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetHomeSpeed")]
        public static extern short SetHomeSpeed(short nNmcNo, short nAxisNo,
                                  double dHomeSpeed0, double dHomeSpeed1, double dHomeSpeed2);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetHomeSpeedEx")]
        public static extern short SetHomeSpeedEx(short nNmcNo, short nAxisNo,
                                  double dHomeSpeed0, double dHomeSpeed1, double dHomeSpeed2, double dOffsetSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetHomeSpeedAccDec")]
        public static extern short SetHomeSpeedAccDec(short nNmcNo, short nAxisNo, double dHomeSpeed0, double dStart0, double dAcc0, double dDec0, double dHomeSpeed1, double dStart1, double dAcc1, double dDec1,
                              double dHomeSpeed2, double dStart2, double dAcc2, double dDec2, double dOffsetSpeed, double dOffsetStart, double dOffsetAcc, double dOffsetDec);
        [DllImport("NMC2.dll", EntryPoint = "nmc_HomeDoneCancel")]
        public static extern short HomeDoneCancel(short nNmcNo, short nAxisNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetHomeDelay")]
        public static extern short SetHomeDelay(short nNmcNo, int nHomeDelay);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_Interpolation2Axis")]
        public static extern short Interpolation2Axis(short nNmcNo, short nAxisNo0, double dPos0,
                                        short nAxisNo1, double dPos1, short nOpt);
        [DllImport("NMC2.dll", EntryPoint = "nmc_Interpolation3Axis")]
        public static extern short Interpolation3Axis(short nNmcNo, short nAxisNo0, double dPos0,
                short nAxisNo1, double dPos1, short nAxisNo2, double dPos2, short nOpt);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_InterpolationArc")]
        public static extern short InterpolationArc(short nNmcNo, short nAxisNo0, short nAxisNo1,
                                      double dCenter0, double dCenter1, double dAngle, short nOpt);

        [DllImport("NMC2.dll", EntryPoint = "nmc_InterpolationRelCircle")]
        public static extern short InterpolationRelCircle(short nNmcNo, short nAxisNo0, double CenterPulse0, double EndPulse0,
                short nAxisNo1, double CenterPulse1, double EndPulse1, short nDir);
        [DllImport("NMC2.dll", EntryPoint = "nmc_InterpolationAbsCircle")]
        public static extern short InterpolationAbsCircle(short nNmcNo, short nAxisNo0, double CenterPulse0, double EndPulse0,
                short nAxisNo1, double CenterPulse1, double EndPulse1, short nDir);

        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetCurrentOn")]
        public static extern short SetCurrentOn(short nNmcNo, short nAxisNo, short nOut);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetServoOn")]
        public static extern short SetServoOn(short nNmcNo, short nAxisNo, short nOut);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetAlarmResetOn")]
        public static extern short SetAlarmResetOn(short nNmcNo, short nAxisNo, short nOut);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDccOn")]
        public static extern short SetDccOn(short nNmcNo, short nAxisNo, short nOut);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMultiCurrentOn")]
        public static extern short SetMultiCurrentOn(short nNmcNo, short nCount, short[] pnAxisSelect, short nOut);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMultiServoOn")]
        public static extern short SetMultiServoOn(short nNmcNo, short nCount, short[] pnAxisSelect, short nOut);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMultiAlarmOn")]
        public static extern short SetMultiAlarmOn(short nNmcNo, short nCount, short[] pnAxisSelect, short nOut);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMultiDccOn")]
        public static extern short SetMultiDccOn(short nNmcNo, short nCount, short[] pnAxisSelect, short nOut);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEnableNear")]
        public static extern short SetEnableNear(short nNmcNo, short nAxisNo, short nMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEnableEncZ")]
        public static extern short SetEnableEncZ(short nNmcNo, short nAxisNo, short nMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetLimitStopMode")]
        public static extern short SetLimitStopMode(short nNmcNo, short nAxisNo, short nStopMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetBusyOffMode")]
        public static extern short SetBusyOffMode(short nNmcNo, short nAxisNo, short nMode);

        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetRingCountMode")]
        public static extern short SetRingCountMode(short nNmcNo, short nAxisNo, int lMaxPulse, int lMaxEncoder, short nRingMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_MoveRing")]
        public static extern short MoveRing(short nNmcNo, short nAxisNo, double dPos, short nMoveMode);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetSyncSetup")]
        public static extern short SetSyncSetup(short nNmcNo, short nMainAxisNo,
                                short nSubAxisNoEnable0, short nSubAxisNoEnable1, short nSubAxisNoEnable2);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetSync")]
        public static extern short SetSync(short nNmcNo, short nGroupNo, short[] pnSyncGrpList0, short[] pnSyncGrpList1);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SyncFree")]
        public static extern short SyncFree(short nNmcNo, short nGroupNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMDIOOutPin")]
        public static extern short SetMDIOOutPin(short nNmcNo, short nPinNo, short nOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMDIOOutTogPin")]
        public static extern short SetMDIOOutTogPin(short nNmcNo, short nPinNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMDIOOutPins")]
        public static extern short SetMDIOOutPins(short nNmcNo, short nCount, short[] pnPinNo, short[] pnStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMDIOOutTogPins")]
        public static extern short SetMDIOOutTogPins(short nNmcNo, short nCount, short[] pnPinNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetMDIOInPin")]
        public static extern short GetMDIOInPin(short nNmcNo, short nPinNo, out short pnInStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_GetMDIOInput")]
        public static extern short GetMDIOInput(short nNmcNo, short[] pnInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetMDIOInputBit")]
        public static extern short GetMDIOInputBit(short nNmcNo, short nBitNo, out short pnInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetMDIOOutput")]
        public static extern short GetMDIOOutput(short nNmcNo, short[] pnOutStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMDIOOutput")]
        public static extern short SetMDIOOutput(short nNmcNo, short[] pnOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMDIOOutputBit")]
        public static extern short SetMDIOOutputBit(short nNmcNo, short nBitNo, short nOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMDIOOutputTog")]
        public static extern short SetMDIOOutputTog(short nNmcNo, short nBitNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMDIOOutputAll")]
        public static extern short SetMDIOOutputAll(short nNmcNo, short[] pnOnBitNo, short[] pnOffBitNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMDIOOutputTogAll")]
        public static extern short SetMDIOOutputTogAll(short nNmcNo, short[] pnBitNo);

        [DllImport("NMC2.dll", EntryPoint = "nmc_GetMDIOInput32")]
        public static extern short GetMDIOInput32(short nNmcNo, out int plInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetMDIOOutput32")]
        public static extern short GetMDIOOutput32(short nNmcNo, out int plOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMDIOOutput32")]
        public static extern short SetMDIOOutput32(short nNmcNo, int lOutStatus);

        //-----------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutPin")]
        public static extern short SetDIOOutPin(short nNmcNo, short nPinNo, short nOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutTogPin")]
        public static extern short SetDIOOutTogPin(short nNmcNo, short nPinNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutPins")]
        public static extern short SetDIOOutPins(short nNmcNo, short nCount, short[] pnPinNo, short[] pnStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutTogPins")]
        public static extern short SetDIOOutTogPins(short nNmcNo, short nCount, short[] pnPinNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOInPin")]
        public static extern short GetDIOInPin(short nNmcNo, short nPinNo, out short pnInStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOInput")]
        public static extern short GetDIOInput(short nNmcNo, short[] pnInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOInput128")]
        public static extern short GetDIOInput128(short nNmcNo, short[] pnInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOInputBit")]
        public static extern short GetDIOInputBit(short nNmcNo, short nBitNo, out short pnInStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOOutput")]
        public static extern short GetDIOOutput(short nNmcNo, short[] pnOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOOutput128")]
        public static extern short GetDIOOutput128(short nNmcNo, short[] pnOutStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutput")]
        public static extern short SetDIOOutput(short nNmcNo, short[] pnOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutput128")]
        public static extern short SetDIOOutput128(short nNmcNo, short[] pnOutStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutputBit")]
        public static extern short SetDIOOutputBit(short nNmcNo, short nBitno, short nOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutputTog")]
        public static extern short SetDIOOutputTog(short nNmcNo, short nBitNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutputAll")]
        public static extern short SetDIOOutputAll(short nNmcNo, short[] pnOnBitNo, short[] pnOffBitNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutputTogAll")]
        public static extern short SetDIOOutputTogAll(short nNmcNo, short[] pnBitNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOInput64")]
        public static extern short GetDIOInput64(short nNmcNo, out long plInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOOutput64")]
        public static extern short GetDIOOutput64(short nNmcNo, out long plOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutput64")]
        public static extern short SetDIOOutput64(short nNmcNo, long lOutStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOInput32")]
        public static extern short GetDIOInput32(short nNmcNo, short nIndex, out int plInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOOutput32")]
        public static extern short GetDIOOutput32(short nNmcNo, short nIndex, out int plOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDIOOutput32")]
        public static extern short SetDIOOutput32(short nNmcNo, short nIndex, int lOutStatus);
        //-----------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEXDIOOutPin")]
        public static extern short SetEXDIOOutPin(short nNmcNo, short nPinNo, out short nOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEXDIOOutTogPin")]
        public static extern short SetEXDIOOutTogPin(short nNmcNo, short nPinNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEXDIOOutPins")]
        public static extern short SetEXDIOOutPins(short nNmcNo, short nCount, short[] pnPinNo, short[] pnStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEXDIOOutTogPins")]
        public static extern short SetEXDIOOutTogPins(short nNmcNo, short nCount, short[] pnPinNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetEXDIOInPin")]
        public static extern short GetEXDIOInPin(short nNmcNo, short nPinNo, out short pnInStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_GetEXDIOInput")]
        public static extern short GetEXDIOInput(short nNmcNo, short[] pnInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetEXDIOInputBit")]
        public static extern short GetEXDIOInputBit(short nNmcNo, short nBitNo, out short pnInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetEXDIOOutput")]
        public static extern short GetEXDIOOutput(short nNmcNo, short[] pnOutStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEXDIOOutput")]
        public static extern short SetEXDIOOutput(short nNmcNo, short[] pnOutStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEXDIOOutputBit")]
        public static extern short SetEXDIOOutputBit(short nNmcNo, short nBitNo, short nOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEXDIOOutputTog")]
        public static extern short SetEXDIOOutputTog(short nNmcNo, short nBitNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEXDIOOutputAll")]
        public static extern short SetEXDIOOutputAll(short nNmcNo, short[] pnOnBitNo, short[] pnOffBitNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEXDIOOutputTogAll")]
        public static extern short SetEXDIOOutputTogAll(short nNmcNo, short[] pnBitNo);

        [DllImport("NMC2.dll", EntryPoint = "nmc_GetEXDIOInput32")]
        public static extern short GetEXDIOInput32(short nNmcNo, out int plInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetEXDIOOutput32")]
        public static extern short GetEXDIOOutput32(short nNmcNo, out int plOutStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEXDIOOutput32")]
        public static extern short SetEXDIOOutput32(short nNmcNo, int lOutStatus);
        //-----------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetOutLimitTimePin")]
        public static extern short SetOutLimitTimePin(short nNmcNo, short nIoType, short nPinNo, short nOn, int nTime);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetOutLimitTimePin")]
        public static extern short GetOutLimitTimePin(short nNmcNo, short nIoType, short nPinNo, out short pnSet, out short pnStatus, out short pnOutStatus, out int pnRemainTime);
        //-----------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetFirmVersion")]
        public static extern short GetFirmVersion(short nNmcNo, out char pStr);
        //-----------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetUnitPerPulse")]
        public static extern short SetUnitPerPulse(short nNmcNo, short nAxisNo, double dRatio);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetUnitPerPulse")]
        public static extern double GetUnitPerPulse(short nNmcNo, short nAxisNo);
        //-----------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetProtocolMethod")]
        public static extern void SetProtocolMethod(short nNmcNo, short nMethod);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetProtocolMethod")]
        public static extern short GetProtocolMethod(short nNmcNo);
        //-----------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetIPAddress")]
        public static extern short GetIPAddress(out short pnField0, out short pnField1, out short pnField2, out short pnField3);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetIPAddress")]
        public static extern short SetIPAddress(short nNmcNo, short nField0, short nField1, short nField2);
        [DllImport("NMC2.dll", EntryPoint = "nmc_WriteIPAddress")]
        public static extern short WriteIPAddress(short nNmcNo, short[] pnIP, short[] pnSubNet, short nGateway);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDefaultIPAddress")]
        public static extern short SetDefaultIPAddress(short nNmcNo);
        //-----------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDeviceType")]
        public static extern short GetDeviceType(short nNmcNo, out int plDeviceType);

        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDeviceInfo")]
        public static extern short GetDeviceInfo(short nNmcNo, out short pnMotionType, out short pnDioType, out short pnEXDio, out short pnMDio);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetEnumList")]
        public static extern int GetEnumList(short[] pnIp, out NMCEQUIPLIST pNmcList);  // PAIX 문의해서 API 수정된 부분.
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetDIOInfo")]
        public static extern int GetDIOInfo(short nNmcNo, out short pnInCount, out short pnOutCount);
        //-----------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_DIOTest")]
        public static extern short DIOTest(short nNmcNo, short nMode, short nDelay);
        //-----------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_MotCfgSaveToROM")]
        public static extern short MotCfgSaveToROM(short nNmcNo, short nMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_MotCfgSetDefaultROM")]
        public static extern short MotCfgSetDefaultROM(short nNmcNo, short nMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_MotCfgLoadFromROM")]
        public static extern short MotCfgLoadFromROM(short nNmcNo, short nMode);

        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_AccOffsetCount")]
        public static extern short AccOffsetCount(short nNmcNo, short nAxisNo, int lPulse);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_PingCheck")]
        public static extern short PingCheck(short nNmcNo, int lWaitTime);

        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetBusyStatus")]
        public static extern short GetBusyStatus(short nNmcNo, short nAxisNo, out short pnBusyStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetBusyStatusAll")]
        public static extern short GetBusyStatusAll(short nNmcNo, short[] pnBusyStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetTriggerCfg")]
        public static extern short SetTriggerCfg(short nNmcNo, short nAxis, short nCmpMode, int lCmpAmount, double dDioOnTime, short nPinNo, short nDioType, short nReserve);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetTriggerEnable")]
        public static extern short SetTriggerEnable(short nNmcNo, short nAxis, short nEnable);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetMapIO")]
        public static extern short GetMapIO(short nNmcNo, short[] pnMapInStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_MapMove")]
        public static extern short MapMove(short nNmcNo, short nAxis, double dPos, short nMapIndex, short nOpt);
        [DllImport("NMC2.dll", EntryPoint = "nmc_MapMoveEx")]
        public static extern short MapMoveEx(short nNmcNo, short nAxis, double dPos, short nMapIndex, short nOpt, short nPosType);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetMapData")]
        public static extern short GetMapData(short nNmcNo, short nMapIndex, out NMCMAPDATA pNmcMapData);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetMapDataEx")]
        public static extern short GetMapDataEx(short nNmcNo, short nMapIndex, short nDataIndex, out NMCMAPDATA pNmcMapData);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetAxesCmdSpeed")]
        public static extern short GetAxesCmdSpeed(short nNmcNo, double[] pDrvSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetAxesEncSpeed")]
        public static extern short GetAxesEncSpeed(short nNmcNo, double[] pdEncSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetAxesCmdEncSpeed")]
        public static extern short GetAxesCmdEncSpeed(short nNmcNo, double[] pdCmdSpeed, double[] pdEncSpeed);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetGantryAxis")]
        public static extern short SetGantryAxis(short nNmcNo, short nGroupNo, short nMain, short nSub);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetGantryEnable")]
        public static extern short SetGantryEnable(short nNmcNo, short nGroupNo, short nGantryEnable);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetGantryInfo")]
        public static extern short GetGantryInfo(short nNmcNo, short[] pnEnable, short[] pnMainAxes, short[] pnSubAxes);
        //------------------------------------------------------------------------------
        [DllImport("NMC2.dll", EntryPoint = "nmc_ContRun")]
        public static extern short ContRun(short nNmcNo, short nGroupNo, short nRunMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetContStatus")]
        public static extern short GetContStatus(short nNmcNo, out NMCCONTSTATUS pContStatus);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetContNodeLine")]
        public static extern short SetContNodeLine(short nNmcNo, short nGroupNo, short nNodeNo,
                short nAxisNo0, short nAxisNo1,
                double dPos0, double dPos1,
                double dStart, double dAcc, double dDec, double dDriveSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetContNodeLineIO")]
        public static extern short SetContNodeLineIO(short nNmcNo, short nGroupNo, short nNodeNo,
                short nAxisNo0, short nAxisNo1,
                double dPos0, double dPos1,
                double dStart, double dAcc, double dDec, double dDriveSpeed, short nOnOff);


        [DllImport("NMC2.dll", EntryPoint = "nmc_SetContNode3Line")]
        public static extern short SetContNode3Line(short nNmcNo, short nGroupNo, short nNodeNo,
                    short nAxisNo0, short nAxisNo1, short nAxisNo2,
                    double dPos0, double dPos1, double dPos2,
                    double dStart, double dAcc, double dDec, double dDriveSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetContNode3LineIO")]
        public static extern short SetContNode3LineIO(short nNmcNo, short nGroupNo, short nNodeNo,
                    short nAxisNo0, short nAxisNo1, short nAxisNo2,
                    double dPos0, double dPos1, double dPos2,
                    double dStart, double dAcc, double dDec, double dDriveSpeed, short nOnOff);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetContNodeArc")]
        public static extern short SetContNodeArc(short nNmcNo, short nGroupNo, short nNodeNo,
                short nAxisNo0, short nAxisNo1,
                double dCenter0, double dCenter1, double dAngle,
                double dStart, double dAcc, double dDec, double dDriveSpeed);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetContNodeArcIO")]
        public static extern short SetContNodeArcIO(short nNmcNo, short nGroupNo, short nNodeNo,
                short nAxisNo0, short nAxisNo1,
                double dCenter0, double dCenter1, double dAngle,
                double dStart, double dAcc, double dDec, double dDriveSpeed, short nOnOff);

        [DllImport("NMC2.dll", EntryPoint = "nmc_ContNodeClear")]
        public static extern short ContNodeClear(short nNmcNo, short nGroupNo);

        [DllImport("NMC2.dll", EntryPoint = "nmc_ContSetIO")]
        public static extern short ContSetIO(short nNmcNo, short nGroupNo, short nIoType, short nIoPinNo, short nEndNodeOnOff);

        [DllImport("NMC2.dll", EntryPoint = "nmc_GetCmdPos")]
        public static extern short GetCmdPos(short nNmcNo, short nAxis, out int plCmdPos);
        [DllImport("NMC2.dll", EntryPoint = "nmc_GetEncPos")]
        public static extern short GetEncPos(short nNmcNo, short nAxis, out int plEncPos);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDisconectedStopMode")]
        public static extern short SetDisconectedStopMode(short nNmcNo, int lTimeInterval, short nStopMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetDisconnectedStopMode")]
        public static extern short SetDisconnectedStopMode(short nNmcNo, int lTimeInterval, short nStopMode);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetEmgEnable")]
        public static extern short SetEmgEnable(short nNmcNo, short nEnable);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetSerialConfig")]
        public static extern short SetSerialConfig(short nNmcNo, short nBaud, short nData, short nStop, short nParity);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SetSerialMode")]
        public static extern short SetSerialMode(short nNmcNo, short nMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SerialWrite")]
        public static extern short SerialWrite(short nNmcNo, short nLen, char[] pStr);
        [DllImport("NMC2.dll", EntryPoint = "nmc_SerialRead")]
        public static extern short SerialRead(short nNmcNo, out short pnReadLen, char[] pReadStr);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetMpgMode")]
        public static extern short SetMpgMode(short nNmcNo, short nAxisNo, short nMode, long lPulse);

        [DllImport("NMC2.dll", EntryPoint = "nmc_ContiSetNodeClear")] // 무제한 연속 보간 큐버퍼 초기화 함수 - UCI 
        public static extern short ContiSetNodeClear(short nNmcNo, short nGroupNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_ContiSetMode")] // 무제한 연속 보간 초기 설정 함수 - UCI
        public static extern short ContiSetMode(short nNmcNo, short nGroupNo, short nAVTRIMode, short nEmptyMode, short n1stAxis, short n2ndAxis, short n3rdAxis, double dMaxDrvSpeed, short nIoType, int nIoCtrlPinMask, int nIoCtrlEndVal);
        [DllImport("NMC2.dll", EntryPoint = "nmc_ContiGetStatus")] // 무제한 연속 보간 상태 체크 함수 - UCI
        public static extern short ContiGetStatus(short nNmcNo, out NMCCONTISTATUS pContiStatus);
        [DllImport("NMC2.dll", EntryPoint = "nmc_ContiAddNodeLine2Axis")] // 무제한 연속 보간 2축 직선 보간 함수 - UCI
        public static extern short ContiAddNodeLine2Axis(short nNmcNo, short nGroupNo, double dPos0, double dPos1, double dStart, double dAcc, double dDec, double dDrvSpeed, int nIoCtrlVal);
        [DllImport("NMC2.dll", EntryPoint = "nmc_ContiAddNodeLine3Axis")] // 무제한 연속 보간 3축 직선 보간 함수 - UCI
        public static extern short ContiAddNodeLine3Axis(short nNmcNo, short nGroupNo, double dPos0, double dPos1, double dPos2, double dStart, double dAcc, double dDec, double dDrvSpeed, int nIoCtrlVal);
        [DllImport("NMC2.dll", EntryPoint = "nmc_ContiAddNodeArc")] // 무제한 연속 보간 2축 원호 보간 함수 - UCI
        public static extern short ContiAddNodeArc(short nNmcNo, short nGroupNo, double dCenter0, double dCenter1, double dAngle, double dStart, double dAcc, double dDec, double dDrvSpeed, int nIoCtrlVal);
        [DllImport("NMC2.dll", EntryPoint = "nmc_ContiSetCloseNode")] // 무제한 연속 보간 노드 추가 종료 함수 - UCI
        public static extern short ContiSetCloseNode(short nNmcNo, short nGroupNo);
        [DllImport("NMC2.dll", EntryPoint = "nmc_ContiRunStop")] // 무제한 연속 보간 실행/정지 함수 - UCI
        public static extern short ContiRunStop(short nNmcNo, short nGroupNo, short nRunMode);

        [DllImport("NMC2.dll", EntryPoint = "nmc_AVTRISetMode")] // 삼각파형 방지 기능 설정
        public static extern short AVTRISetMode(short nNmcNo, short nAxis, short nAVTRIMode);
        [DllImport("NMC2.dll", EntryPoint = "nmc_AVTRIGetMode")] // 삼각파형 방지기능 설정값 가져오기
        public static extern short AVTRIGetMode(short nNmcNo, short nAxis, out short nAVTRIMode);

        [DllImport("NMC2.dll", EntryPoint = "nmc_SetWaitTime")] //응답대기시간 설정
        public static extern short SetWaitTime(short nNmcNo, long lWaitTime);
    };
};
//------------------------------------------------------------------------------

//DESCRIPTION  'NMC Windows Dynamic Link Library'     -- *def file* description ....


