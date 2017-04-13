using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DXAppCPTester.CpTesterCommon.Util;
using DXAppCPTester.CPSystem.Instrument;
using DXAppCPTester.CpTesterCommon.DefineCommonEnum;
using DXAppCPTester.CPSystem.Assist;
using DXAppCPTester.Configure;
using System.Diagnostics;

namespace DXAppCPTester.TestStep.Manager
{
    /// <summary>
    /// Managed hardware instance
    /// Managed instruments information
    /// </summary>
    public class ClsCpSystemManager
    {   
        private Stopwatch m_swCpSystemTimer = new Stopwatch();
        public System.Diagnostics.Stopwatch CpSystemTimer
        {
            get { return m_swCpSystemTimer; }
            set { m_swCpSystemTimer = value; }
        }

        private ClsCpHwManager m_cpMngHardware = null;
        public DXAppCPTester.TestStep.Manager.ClsCpHwManager MngHardware
        {
            get { return m_cpMngHardware; }
            set { m_cpMngHardware = value; }
        }

        private ClsCpTsManager m_cpMngTStep = null;
        public DXAppCPTester.TestStep.Manager.ClsCpTsManager MngTStep
        {
            get { return m_cpMngTStep; }
            set { m_cpMngTStep = value; }
        }

        private ClsCpSysConfigure m_cnfSystem = null;
        public DXAppCPTester.Configure.ClsCpSysConfigure CnfSystem
        {
            get { return m_cnfSystem; }
            set { m_cnfSystem = value; }
        }

        private ClsCpHwConfigure m_cnfHardware = null;
        public DXAppCPTester.Configure.ClsCpHwConfigure CnfHardware
        {
            get { return m_cnfHardware; }
            set { m_cnfHardware = value; }
        }

        public ClsCpSystemManager(ClsCpHwManager mngHardware)
        {
            m_cpMngHardware = mngHardware;
        }

        public void closeManager()
        {
            if (m_cpMngHardware != null)
                m_cpMngHardware.closeManager();
        }

        public static ClsCpSystemManager createInstrument(ClsCpSysConfigure sysconfig, ClsCpHwConfigure hwconfig)// ClsInstrumentList lstInstInfo)
        {
            try
            {
                ClsInstrumentList lstInstInfo = hwconfig.NiConfigue.LstInstInfo;
                ClsCpHwManager mngHw = new ClsCpHwManager();
                mngHw.DicInstrumentManager = new Dictionary<CpHwInstrumentType, ClsCpInstrumentManagerBase>();

                foreach (ClsInstInfoBase info in lstInstInfo)
                {
                    if (!mngHw.DicInstrumentManager.ContainsKey(info.InstrumentType))
                    {
                        switch (info.InstrumentType)
                        {
                            case CpHwInstrumentType.SWITCH: mngHw.DicInstrumentManager.Add(info.InstrumentType, new ClsCpSwitchManager(sysconfig.HaConfigure)); break;
                            case CpHwInstrumentType.CAN: mngHw.DicInstrumentManager.Add(info.InstrumentType, new ClsCpCANManager(sysconfig.HaConfigure)); break;
                            case CpHwInstrumentType.OSCILLO: mngHw.DicInstrumentManager.Add(info.InstrumentType, new ClsCpOscilloManager(sysconfig.HaConfigure)); break;
                            case CpHwInstrumentType.DMM: mngHw.DicInstrumentManager.Add(info.InstrumentType, new ClsCpDMMManager(sysconfig.HaConfigure)); break;
                            case CpHwInstrumentType.REGISTER: mngHw.DicInstrumentManager.Add(info.InstrumentType, new ClsCpRegisterManager(sysconfig.HaConfigure)); break;
                            case CpHwInstrumentType.POWER: mngHw.DicInstrumentManager.Add(info.InstrumentType, new ClsCpPowerManager(sysconfig.HaConfigure)); break;
                            case CpHwInstrumentType.COUNTER: mngHw.DicInstrumentManager.Add(info.InstrumentType, new ClsCpCounterManager()); break;
                            case CpHwInstrumentType.FGN: mngHw.DicInstrumentManager.Add(info.InstrumentType, new ClsCpFGNManager(sysconfig.HaConfigure)); break;
                            case CpHwInstrumentType.GPIB: mngHw.DicInstrumentManager.Add(info.InstrumentType, new ClsCpGPIBManager(sysconfig.HaConfigure)); break;
                            case CpHwInstrumentType.RELAY: mngHw.DicInstrumentManager.Add(info.InstrumentType, new ClsCpRelayManager(sysconfig.HaConfigure)); break;
                        }
                    }

                    mngHw.DicInstrumentManager[info.InstrumentType].addInstrument(info);
                }

                ClsCpCounterManager countermgr = mngHw.DicInstrumentManager[CpHwInstrumentType.COUNTER] as ClsCpCounterManager;
                ClsCpOscilloManager oscmgr = mngHw.DicInstrumentManager[CpHwInstrumentType.OSCILLO] as ClsCpOscilloManager;
                if (countermgr != null && oscmgr != null)
                {
                    countermgr.setOSCManager(oscmgr);
                }

                // create mitech board
                ClsMitechBoardList lstboard = hwconfig.MitechConfigue.LstBoardInfo;
                mngHw.ClsMitchBoardManager = new ClsCpLoadManager(sysconfig.HaConfigure, hwconfig.MitechConfigue.DicBoardTypeDef);

                foreach (ClsMitechInfoBase info in lstboard)
                {
                    mngHw.ClsMitchBoardManager.addBoard(info);
                }
                
                // create mitech load unit
                ClsMitechLoadUnitList lstloadunit = hwconfig.MitechConfigue.LstLoadUnitInfo;
                ClsCpLoadManager mngload = mngHw.ClsMitchBoardManager as ClsCpLoadManager;
                foreach (ClsLoadUnitInfo info in lstloadunit)
                {
                    mngload.addUnit(info);
                }

                mngHw.runUDPReceiver();

                return new ClsCpSystemManager(mngHw);
            }
            catch (System.Exception ex)
            {
                UtilTextMessageEdits.UtilTextMsgToConsole("Failed to create a system manager.", ConsoleColor.Red, CpDefineEnumDebugPrintLogLevel.FATAL);
                UtilTextMessageEdits.UtilTextMsgToConsole("\t- Reason : " + ex.ToString(), ConsoleColor.White, CpDefineEnumDebugPrintLogLevel.FATAL);
            }
            return null;
        }
    }
}
