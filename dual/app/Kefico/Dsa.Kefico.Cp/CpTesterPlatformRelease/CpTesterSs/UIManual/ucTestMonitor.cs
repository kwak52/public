using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CpTesterPlatform.CpTesterSs;
using Dsu.Driver.Base;

namespace CpTesterPlatform.CpTester
{
    public partial class ucTestMonitor : UserControl
    {
        public string STATION_OK1 { get { return simpleButton_OK_M1.Text; } set { simpleButton_OK_M1.Text = value; } }
        public string STATION_OK2 { get { return simpleButton_OK_M2.Text; } set { simpleButton_OK_M2.Text = value; } }
        public string STATION_OK3 { get { return simpleButton_OK_M3.Text; } set { simpleButton_OK_M3.Text = value; } }
        public string STATION_OK4 { get { return simpleButton_OK_M4.Text; } set { simpleButton_OK_M4.Text = value; } }
        public string STATION_OK5 { get { return simpleButton_OK_M5.Text; } set { simpleButton_OK_M5.Text = value; } }
        public string STATION_NG1 { get { return simpleButton_NG_M1.Text; } set { simpleButton_NG_M1.Text = value; } }
        public string STATION_NG2 { get { return simpleButton_NG_M2.Text; } set { simpleButton_NG_M2.Text = value; } }
        public string STATION_NG3 { get { return simpleButton_NG_M3.Text; } set { simpleButton_NG_M3.Text = value; } }
        public string STATION_NG4 { get { return simpleButton_NG_M4.Text; } set { simpleButton_NG_M4.Text = value; } }
        public string STATION_NG5 { get { return simpleButton_NG_M5.Text; } set { simpleButton_NG_M5.Text = value; } }

        private enum StationType { Loading = 0, Unloading };
        private enum StationNG { TotalOK = 0, STN1, STN2, STN3, STN4, STN5 };
        private enum Result { OK = 0, NG, SKIP };
        private bool bOkStationSkip = true;
        private string _MesID;
        public event UnloadingDialogEventHandler UnloadingDialogHandler;

        public ucTestMonitor()
        {
            InitializeComponent();
            InitializeResult(Result.SKIP);
            groupControl_OK.Appearance.BackColor2 = Color.White;
            groupControl_NG.Appearance.BackColor2 = Color.White;
        }

        public void SetStationName()
        {
            if (DriverBaseGlobals.IsLine7DCT())
            {
                STATION_OK1 = "FD STATION";
                STATION_OK2 = "LCR STATION";
                STATION_OK3 = "ODD STATION";
                STATION_OK4 = "EVEN STATION";
                STATION_OK5 = "";
                STATION_NG1 = "FD STATION";
                STATION_NG2 = "LCR STATION";
                STATION_NG3 = "ODD STATION";
                STATION_NG4 = "EVEN STATION";
                STATION_NG5 = "";
                simpleButton_OK_M5.Visible = false;
                simpleButton_NG_M5.Visible = false;
            }
            else if(DriverBaseGlobals.IsLine8FF())
            {
                STATION_OK1 = "FD";
                STATION_OK2 = "Cap./Res.";
                STATION_OK3 = "Input Funtion";
                STATION_OK4 = "Middle Funtion";
                STATION_OK5 = "Output Funtion";
                STATION_NG1 = "FD";
                STATION_NG2 = "Cap./Res.";
                STATION_NG3 = "Input Funtion";
                STATION_NG4 = "Middle Funtion";
                STATION_NG5 = "Output Funtion";
            }
        }

        public void UdpateNgStation(int NgStation, string stationMessage, string MesID)
        {
            labelControl_Unloading.Text = stationMessage;
            labelControl_LoadingOK.Text = _MesID;

            InitializeResult(Result.SKIP);

            if (bOkStationSkip)
                UpdateOkStation(Result.SKIP); //임시로 Unloading(Ng Station) 이벤트를 이용해서 Ok Station 정보 업데이트
            else
                UpdateOkStation(Result.OK);

            bOkStationSkip = NgStation != (int)StationNG.TotalOK ? true : false;
            _MesID = MesID;

            if (NgStation == (int)StationNG.TotalOK)
            {
                SetButtonState(0, StationType.Unloading, Result.OK);
                SetButtonState(1, StationType.Unloading, Result.OK);
                SetButtonState(2, StationType.Unloading, Result.OK);
                SetButtonState(3, StationType.Unloading, Result.OK);
                SetButtonState(4, StationType.Unloading, Result.OK);
                SetButtonState(5, StationType.Unloading, Result.OK);
            }
            else if (NgStation == (int)StationNG.STN1)
            {
                SetButtonState(0, StationType.Unloading, Result.NG);
                SetButtonState(1, StationType.Unloading, Result.NG);
                SetButtonState(2, StationType.Unloading, Result.SKIP);
                SetButtonState(3, StationType.Unloading, Result.SKIP);
                SetButtonState(4, StationType.Unloading, Result.SKIP);
                SetButtonState(5, StationType.Unloading, Result.SKIP);
            }
            else if (NgStation == (int)StationNG.STN2)
            {
                SetButtonState(0, StationType.Unloading, Result.NG);
                SetButtonState(1, StationType.Unloading, Result.OK);
                SetButtonState(2, StationType.Unloading, Result.NG);
                SetButtonState(3, StationType.Unloading, Result.SKIP);
                SetButtonState(4, StationType.Unloading, Result.SKIP);
                SetButtonState(5, StationType.Unloading, Result.SKIP);
            }
            else if (NgStation == (int)StationNG.STN3)
            {
                SetButtonState(0, StationType.Unloading, Result.NG);
                SetButtonState(1, StationType.Unloading, Result.OK);
                SetButtonState(2, StationType.Unloading, Result.OK);
                SetButtonState(3, StationType.Unloading, Result.NG);
                SetButtonState(4, StationType.Unloading, Result.SKIP);
                SetButtonState(5, StationType.Unloading, Result.SKIP);
            }
            else if (NgStation == (int)StationNG.STN4)
            {
                SetButtonState(0, StationType.Unloading, Result.NG);
                SetButtonState(1, StationType.Unloading, Result.OK);
                SetButtonState(2, StationType.Unloading, Result.OK);
                SetButtonState(3, StationType.Unloading, Result.OK);
                SetButtonState(4, StationType.Unloading, Result.NG);
                SetButtonState(5, StationType.Unloading, Result.SKIP);
            }
            else if (NgStation == (int)StationNG.STN5)
            {
                SetButtonState(0, StationType.Unloading, Result.NG);
                SetButtonState(1, StationType.Unloading, Result.OK);
                SetButtonState(2, StationType.Unloading, Result.OK);
                SetButtonState(3, StationType.Unloading, Result.OK);
                SetButtonState(4, StationType.Unloading, Result.OK);
                SetButtonState(5, StationType.Unloading, Result.NG);
            }
            else if (DriverBaseGlobals.IsLine7DCT() && NgStation == 12)
            {
                //1, 2 test all ng
                SetButtonState(0, StationType.Unloading, Result.NG);
                SetButtonState(1, StationType.Unloading, Result.NG);
                SetButtonState(2, StationType.Unloading, Result.NG);
                SetButtonState(3, StationType.Unloading, Result.SKIP);
                SetButtonState(4, StationType.Unloading, Result.SKIP);
                SetButtonState(5, StationType.Unloading, Result.SKIP);

            }
            else if (DriverBaseGlobals.IsLine7DCT() && NgStation == 34)
            {
                //3, 4 test all  ng
                SetButtonState(0, StationType.Unloading, Result.NG);
                SetButtonState(1, StationType.Unloading, Result.OK);
                SetButtonState(2, StationType.Unloading, Result.OK);
                SetButtonState(3, StationType.Unloading, Result.NG);
                SetButtonState(4, StationType.Unloading, Result.NG);
                SetButtonState(5, StationType.Unloading, Result.SKIP);
            }
            else if (DriverBaseGlobals.IsLine7DCT() && NgStation == -1) 
            {
                // -1  PLC NG
                SetButtonState(0, StationType.Unloading, Result.NG);
                SetButtonState(1, StationType.Unloading, Result.OK);
                SetButtonState(2, StationType.Unloading, Result.OK);
                SetButtonState(3, StationType.Unloading, Result.OK);
                SetButtonState(4, StationType.Unloading, Result.OK);
                SetButtonState(5, StationType.Unloading, Result.OK);
            }
        }

        private void UpdateOkStation(Result stationResult)
        {
            SetButtonState(0, StationType.Loading, stationResult);
            SetButtonState(1, StationType.Loading, stationResult);
            SetButtonState(2, StationType.Loading, stationResult);
            SetButtonState(3, StationType.Loading, stationResult);
            SetButtonState(4, StationType.Loading, stationResult);
            SetButtonState(5, StationType.Loading, stationResult);
        }

        private void SetButtonState(int index, StationType stationType, Result stationResult)
        {
            if (stationType == StationType.Loading)
            {
                switch (index)
                {
                    case 0: groupControl_OK.Appearance.BackColor = GetColorTotal(stationResult); break;
                    case 1: simpleButton_OK_M1.Appearance.BackColor = GetColor(stationResult); break;
                    case 2: simpleButton_OK_M2.Appearance.BackColor = GetColor(stationResult); break;
                    case 3: simpleButton_OK_M3.Appearance.BackColor = GetColor(stationResult); break;
                    case 4: simpleButton_OK_M4.Appearance.BackColor = GetColor(stationResult); break;
                    case 5: simpleButton_OK_M5.Appearance.BackColor = GetColor(stationResult); break;
                }
            }
            else if (stationType == StationType.Unloading)
            {
                switch (index)
                {
                    case 0: groupControl_NG.Appearance.BackColor = GetColorTotal(stationResult); break;
                    case 1: simpleButton_NG_M1.Appearance.BackColor = GetColor(stationResult); break;
                    case 2: simpleButton_NG_M2.Appearance.BackColor = GetColor(stationResult); break;
                    case 3: simpleButton_NG_M3.Appearance.BackColor = GetColor(stationResult); break;
                    case 4: simpleButton_NG_M4.Appearance.BackColor = GetColor(stationResult); break;
                    case 5: simpleButton_NG_M5.Appearance.BackColor = GetColor(stationResult); break;
                }
            }
        }

        private void simpleButton_NG_Click(object sender, EventArgs e)
        {
            groupControl_OK.Appearance.BackColor2 = Color.White;
            groupControl_NG.Appearance.BackColor2 = Color.White;

            UnloadingDialogHandler?.Invoke(true);
        }

        private void InitializeResult(Result result)
        {
            groupControl_OK.Appearance.BackColor = GetColor(result);
            simpleButton_OK_M1.Appearance.BackColor = GetColor(result);
            simpleButton_OK_M2.Appearance.BackColor = GetColor(result);
            simpleButton_OK_M3.Appearance.BackColor = GetColor(result);
            simpleButton_OK_M4.Appearance.BackColor = GetColor(result);
            simpleButton_OK_M5.Appearance.BackColor = GetColor(result);

            groupControl_NG.Appearance.BackColor = GetColor(result);
            simpleButton_NG_M1.Appearance.BackColor = GetColor(result);
            simpleButton_NG_M2.Appearance.BackColor = GetColor(result);
            simpleButton_NG_M3.Appearance.BackColor = GetColor(result);
            simpleButton_NG_M4.Appearance.BackColor = GetColor(result);
            simpleButton_NG_M5.Appearance.BackColor = GetColor(result);
        }

        private Color GetColor(Result stationResult)
        {
            if (stationResult == Result.OK)
                return Color.ForestGreen;
            else if (stationResult == Result.NG)
                return Color.Red;
            else if (stationResult == Result.SKIP)
                return Color.Gray;
            else
                return Color.Gray;
        }


        private Color GetColorTotal(Result stationResult)
        {
            if (stationResult == Result.OK)
                return Color.LightGreen;
            else if (stationResult == Result.NG)
                return Color.Red;
            else if (stationResult == Result.SKIP)
                return Color.Gray;
            else
                return Color.Gray;
        }

    }
}