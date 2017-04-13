using CpTesterPlatform.CpApplication.Manager;
using CpTesterPlatform.CpLogUtil;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using PsKGaudi.Parser.PsCCSSTDFn;
using PsKGaudi.Parser.PsCCSSTDFn.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static CpCommon.ExceptionHandler;
using static PsKGaudi.Parser.PsCCS;

namespace CpTesterPlatform.CpTrendView
{
    public partial class CpTrendViewChart : UserControl
    {
        ///Log File Name Example: 20170304131203_SAMPLE9000_00_1_NG
        ///- Written Date Index: 0
        ///- Product ID Index: 1
        ///- Variant Index: 2
        ///- Station Index: 3
        ///- Test Result Index:4
        enum eFileNameIndex
        {
            WrittenDate = 0,
            ProductID,
            Variant,
            Station,
            TestResult
        }

        ///Log File Path Address Definition
        ///- Year Index: 0
        ///- Month Index: 1
        ///- Day Index: 2
        enum eLogAddressDef
        {
            Year = 0,
            Month,
            Day
        }

        public int StationIndex { get; set; } = 0;
        public Dictionary<PsCCSStdFnBase, bool> m_dicDisplayingObjects = new Dictionary<PsCCSStdFnBase, bool>();
        public DateTime LastLogLimitDate { set; get; }
        public int DisplayLimit { get; set; } = 20;
        public int InitialDisplayLimit { get; set; } = 20;
        public int CategoryLimit { get; set; } = 1;

        CpTsManager TsMng { get; set; }
        string LogPath { set; get; }
        List<KeyValuePair<DateTime, DataTable>> m_vdtStoredLogs = new List<KeyValuePair<DateTime, DataTable>>();
        Dictionary<PsCCSStdFnBase, Series> m_dicShowingSteps = new Dictionary<PsCCSStdFnBase, Series>();
        Dictionary<Series, PsCCSParamsPairMinMax> m_dicMinMax = new Dictionary<Series, PsCCSParamsPairMinMax>();
        Color ErrorColor = Color.Red;
        List<Color> colorSeries = new List<Color>();

        void InitColorSet()
        {
            colorSeries.Add(Color.DarkBlue);
            colorSeries.Add(Color.YellowGreen);
            colorSeries.Add(Color.SandyBrown);
            colorSeries.Add(Color.BlueViolet);
            colorSeries.Add(Color.Orange);
            colorSeries.Add(Color.OliveDrab);
            colorSeries.Add(Color.Violet);
            colorSeries.Add(Color.Tomato);
            colorSeries.Add(Color.DarkGray);
            colorSeries.Add(Color.DarkGreen);
            colorSeries.Add(Color.DarkSalmon);
            colorSeries.Add(Color.Cyan);
        }

        public CpTrendViewChart()
		{
			InitializeComponent();
            InitColorSet();
		}

		public void SetDisplayLimit(int nLimit)
		{
			DisplayLimit = nLimit;
		}

		public void SetLogPath(string strPath)
		{
			LogPath = strPath;
		}

		public void SetTestStepMng(CpTsManager tsMng)
		{
			TsMng = tsMng;
		}

        private void SetAxisYRange()
        {
            XYDiagram diagram = (XYDiagram) chartControlTrend.Diagram;
            int nCnt = 0;

            diagram.SecondaryAxesY.Clear();
            m_dicMinMax.Clear();

            foreach (Series srs in chartControlTrend.Series)
            {
                PsCCSStdFnBase stepFn = m_dicShowingSteps.Where(x => x.Value == srs).ElementAt(0).Key;
                
                if (!m_dicDisplayingObjects[stepFn])
                    continue;
                if (srs.Points.Count <= 0)
                    continue;

                double dMaxVal = srs.Points.Max(x => x.UserValues[0]);
                double dMinVal = srs.Points.Min(x => x.UserValues[0]);
                double dMaxAllow = dMaxVal;
                double dMinAllow = dMinVal;
                bool bMinAllowUsing = false;
                bool bMaxAllowUsing = false;

                m_dicMinMax.Add(srs, stepFn.PairedParmsMinMax);

                if (double.TryParse(stepFn.PairedParmsMinMax.Max, out dMaxAllow))
                    bMaxAllowUsing = true;
                if (double.TryParse(stepFn.PairedParmsMinMax.Min, out dMinAllow))
                    bMinAllowUsing = true;

                dMaxVal = dMaxAllow > dMaxVal ? dMaxAllow : dMaxVal;
                dMinVal = dMinAllow < dMinVal ? dMinAllow : dMinVal;

                double dGap = dMaxVal - dMinVal;      

                if(nCnt == 0)
                {
                    diagram.AxisY.WholeRange.MaxValue = dMaxVal + (0.2 * dGap);
                    diagram.AxisY.WholeRange.MinValue = dMinVal - (0.2 * dGap);
                    //diagram.AxisY.WholeRange.Auto = (!bMinAllowUsing || !bMaxAllowUsing) ? false : true;
                    diagram.AxisY.Color = colorSeries[nCnt%colorSeries.Count];
                    srs.View.Color = colorSeries[nCnt % colorSeries.Count];

                    diagram.AxisY.ConstantLines.Clear();

                    if(bMaxAllowUsing)
                        diagram.AxisY.ConstantLines.Add(GetConstantLine(srs, dMaxAllow));
                    if(bMinAllowUsing)
                        diagram.AxisY.ConstantLines.Add(GetConstantLine(srs, dMinAllow));
                }
                else
                {
                    SecondaryAxisY axisY = new SecondaryAxisY();
                    
                    axisY.WholeRange.MaxValue = dMaxVal + (0.5 * dGap);
                    axisY.WholeRange.MinValue = dMinVal + (0.5 * dGap);
                    //diagram.AxisY.WholeRange.Auto = (!bMinAllowUsing || !bMaxAllowUsing) ? false : true;
                    diagram.SecondaryAxesY.Add(axisY);

                    axisY.Color = colorSeries[nCnt % colorSeries.Count];
                    srs.View.Color = colorSeries[nCnt % colorSeries.Count];

                    axisY.ConstantLines.Clear();

                    if (bMaxAllowUsing)
                        axisY.ConstantLines.Add(GetConstantLine(srs, dMaxAllow));
                    if (bMinAllowUsing)
                        axisY.ConstantLines.Add(GetConstantLine(srs, dMinAllow));

                    ((LineSeriesView)srs.View).AxisY = axisY;
                }

                nCnt++;
            }

            if(nCnt > 0)
                ((XYDiagram)chartControlTrend.Diagram).AxisY.WholeRange.Auto = false;
        }

        ConstantLine GetConstantLine(Series srs, double dVal)
        {
            ConstantLine constLine = new ConstantLine();

            constLine.ShowInLegend = false;
            constLine.ShowBehind = true;
            constLine.AxisValue = dVal;
            constLine.LineStyle.Thickness = 2;
            constLine.Color = Color.DarkRed;

            return constLine;
        }

        private void SetAxisXRange()
        {
            XYDiagram diagram = (XYDiagram)chartControlTrend.Diagram;
            diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Automatic;
            diagram.AxisX.NumericScaleOptions.AutoGrid = true;
            diagram.AxisX.Logarithmic = false;
            //diagram.AxisX.DateTimeScaleOptions.MeasureUnit = dtimeUnit;
            //diagram.AxisX.DateTimeScaleOptions.GridAlignment = dtimeGrid;

            diagram.EnableAxisXScrolling = true;
            diagram.EnableAxisXZooming = true;
        }

        private void AddToolTip()
        {
            // Disable a crosshair cursor.
            chartControlTrend.CrosshairEnabled = DefaultBoolean.False;

            // Enable chart tooltips. 
            chartControlTrend.ToolTipEnabled = DefaultBoolean.True;

            ToolTipController controller = new ToolTipController();
            chartControlTrend.ToolTipController = controller;
            controller.ShowBeak = true;

            // Change the default tooltip mouse position to relative position.
            ToolTipRelativePosition relativePosition = new ToolTipRelativePosition();
            chartControlTrend.ToolTipOptions.ToolTipPosition = relativePosition;

            // Specify the tooltip relative position offsets.  
            relativePosition.OffsetX = 2;
            relativePosition.OffsetY = 2;
        }

        public void ClearChart()
        {
            XYDiagram diagram = (XYDiagram) chartControlTrend.Diagram;

            if (diagram != null)
            {
                diagram.AxisY.ConstantLines.Clear();
                diagram.AxisX.Strips.Clear();
            }

            chartControlTrend.Series.Clear();
        }

        public void ClearSeries()
        {
            foreach (PsCCSStdFnBase showingStep in m_dicShowingSteps.Keys)
                m_dicShowingSteps[showingStep].Points.Clear();
        }


        public void UpdateTrendChart()
        {
            UpdateViewingLogList();
            DisplayTrendChart();
        }

        public void DisplayTrendChart()
        {
            chartControlTrend.BeginInit();
            
            foreach (PsCCSStdFnBase showingStep in m_dicShowingSteps.Keys)
            {
                CompleteSeries(showingStep, m_dicShowingSteps[showingStep]);

                LineSeriesView lineSeriesView = new DevExpress.XtraCharts.LineSeriesView();
                lineSeriesView.LineMarkerOptions.Size = 7;
                lineSeriesView.LineStyle.Thickness = 3;
                lineSeriesView.MarkerVisibility = DefaultBoolean.True;
                m_dicShowingSteps[showingStep].View = lineSeriesView;

                // Specify the tooltip point pattern.
                m_dicShowingSteps[showingStep].ToolTipPointPattern = "{A:MM-dd HH:mm:ss} \r\n {V:#0.00}";                
                m_dicShowingSteps[showingStep].ShowInLegend = m_dicDisplayingObjects[showingStep];
                m_dicShowingSteps[showingStep].Visible = m_dicDisplayingObjects[showingStep];                
            }

            chartControlTrend.EndInit();

            if (m_dicDisplayingObjects.Where(x => x.Value == true).Count() > 0)
            {                
                SetAxisXRange();
                SetAxisYRange();
                AddToolTip();
            }        
        }

        public void CompleteSeries(PsCCSStdFnBase showingStep, Series srsView)
        {
            srsView.Points.Clear();

            foreach(KeyValuePair<DateTime, DataTable> value in m_vdtStoredLogs)
            {
                string strQueryExpression = "STEP = '" + showingStep.StepNum +"'";
                DataRow [] adtRow = value.Value.Select(strQueryExpression);
                string strValue = adtRow.FirstOrDefault()?.ItemArray[5].ToString();
                double dValue = -999;

                if(!double.TryParse(strValue, out dValue))
                    continue;

                SeriesPoint srsPoint = new SeriesPoint(value.Key.ToString("dd HH:mm:ss"), dValue);
                
                srsView.Points.Add(srsPoint);                
            }
        }

        public void InitTrendChart()
        {
            InitViewingLogList();
            InitChartDisplay();
            DisplayTrendChart();
        }

        void InitChartDisplay()
        {
            PsCCSGaudiFile testlist = TsMng.GaudiReadData;
            XYDiagram diagram = (XYDiagram) chartControlTrend.Diagram;
            int nCntData = 0;
            
            foreach (PsCCSStdFnBase writtenStep in testlist.ListTestStep)
            {
                if(writtenStep.Traceability == PsKGaudi.Parser.PsCCSDefineEnumTraceability.On
                    && writtenStep.Activate == PsKGaudi.Parser.PsCCSDefineEnumActivate.ACTIVATE
                    && writtenStep.VariantActivate == PsKGaudi.Parser.PsCCSDefineEnumVariantAcrivate.ACTIVATE)
                {
                    Series srsStep = new Series(writtenStep.GetMO(), ViewType.Line);
                    bool bShowingStep = nCntData >= CategoryLimit ? false : true;

                    m_dicShowingSteps.Add(writtenStep, srsStep);
                    m_dicDisplayingObjects.Add(writtenStep, bShowingStep);
                    srsStep.ShowInLegend = bShowingStep;
                    chartControlTrend.Series.Add(srsStep);

                    nCntData++;            
                }
            }

            chartControlTrend.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            chartControlTrend.Legend.AlignmentVertical = LegendAlignmentVertical.Top;
        }

        void InitViewingLogList()
        {
            List<string> vstrLogpaths = GetValidLogPathList();

            if(vstrLogpaths.Count > 0)
                vstrLogpaths.Reverse();

            foreach(string strPath in vstrLogpaths)
            {
                DateTime time;
                DataTable dtResult = null;

                GetLogWrittenDate(Path.GetFileName(strPath), out time);

                dtResult = GetLogData(strPath);

                if(dtResult != null)
                    m_vdtStoredLogs.Add(new KeyValuePair<DateTime, DataTable>(time, dtResult));
            }

            if (m_vdtStoredLogs.Count >= InitialDisplayLimit)
                m_vdtStoredLogs.RemoveRange(0, m_vdtStoredLogs.Count - InitialDisplayLimit);            
        }

        void UpdateViewingLogList()
        {
            List<string> vstrLogFiles= new List<string>();
            string strRecentPath = GetRecentLogPath();
            DateTime timeLastLog = m_vdtStoredLogs.Count > 0 ? m_vdtStoredLogs[m_vdtStoredLogs.Count - 1].Key : new DateTime();

            vstrLogFiles.AddRange(GetLogFilePaths(strRecentPath));
            vstrLogFiles.Reverse();

            foreach (string strFile in vstrLogFiles)
            {
                DateTime timeLogToImport;
                DataTable dtResult = null;

                GetLogWrittenDate(Path.GetFileName(strFile), out timeLogToImport);

                dtResult = GetLogData(strFile);

                if (timeLogToImport > timeLastLog && timeLogToImport > LastLogLimitDate && dtResult != null )
                    m_vdtStoredLogs.Add(new KeyValuePair<DateTime, DataTable>(timeLogToImport, dtResult));
            }
            
            if (m_vdtStoredLogs.Count >= DisplayLimit)
                m_vdtStoredLogs.RemoveRange(0, m_vdtStoredLogs.Count - DisplayLimit);
        }

        DataTable GetLogData(string strPath)
        {
            CpLogHeader logHeader = null;
            string strFileName = Path.GetFileNameWithoutExtension(strPath);
            DataTable dtResult = null;
            string strExtension = Path.GetExtension(strPath);

            if (File.Exists(strPath) && Path.GetExtension(strPath) == ".CpLog")
                dtResult = CpUtilRl.LoadTestLogFromCpLogFile(strPath, strFileName + ".csv", out logHeader);
            
            return dtResult;
        }

		List<string> GetValidLogPathList()
		{
			var oResult = TryFunc(() => {

				List<string> vstrData = new List<string>();
				string strRecentPath = GetRecentLogPath();

                vstrData.AddRange(GetLogFilePaths(strRecentPath));

                while(vstrData.Count <= DisplayLimit)
                {
                    strRecentPath = GetNextLogPath(strRecentPath);

                    if (strRecentPath == string.Empty)
                        break;
                    
                    vstrData.AddRange(GetLogFilePaths(strRecentPath));
                }

                if (vstrData.Count > DisplayLimit)
                    vstrData.RemoveRange(DisplayLimit, vstrData.Count - DisplayLimit);

                return vstrData;
			});

			if(!oResult.Succeeded)
				return new List<string>();
			return oResult.Result;
		}

        List<string> GetLogFilePaths(string strLogPath)
        {
            List<string> vstrData = new List<string>();
            
            if (string.IsNullOrEmpty(strLogPath))
                return vstrData;

                List<string> vstrExistData = GetLogFileNames(strLogPath);

            if (vstrExistData.Count <= 0)
                return vstrData;

            while (vstrExistData.Count > 0)
            {
                string strRecentFile = GetNextRecentLogFileName(string.Empty, strLogPath, vstrExistData);

                vstrData.Add(strRecentFile);
                vstrExistData.Remove(strRecentFile);
            }            

            return vstrData;
        }

        ///Return log path
        string GetRecentLogPath()
		{
            if (!Directory.Exists(LogPath))
                return string.Empty;

            string strYear = GetRecentDirName(LogPath);
            string strMonth = GetRecentDirName(LogPath + @"\" + strYear);
            string strDay = GetRecentDirName(LogPath + @"\" + strYear + @"\" + strMonth);

            return LogPath + @"\" + strYear + @"\" + strMonth + @"\" + strDay;
		}

		string GetNextLogPath(string strPath)
		{
            string strYear = string.Empty;
            string strMonth = string.Empty;
            string strDay = string.Empty;

            if (!GetDateFromLogPath(strPath, out strYear, out strMonth, out strDay))
                return string.Empty;

            strDay = GetRecentDirName(LogPath + @"\" + strYear + @"\" + strMonth, Convert.ToInt32(strDay));

            if (strDay != string.Empty)
                return LogPath + @"\" + strYear + @"\" + strMonth + @"\" + strDay;

            strMonth = GetRecentDirName(LogPath + @"\" + strYear + @"\", Convert.ToInt32(strMonth));

            if (strMonth != string.Empty)
            {
                strDay = GetRecentDirName(LogPath + @"\" + strYear + @"\" + strMonth);

                if (strDay == string.Empty)
                    return string.Empty;

                return LogPath + @"\" + strYear + @"\" + strMonth + @"\" + strDay;
            }

            strYear = GetRecentDirName(LogPath, Convert.ToInt32(strYear));

            if(strYear != string.Empty)
            {
                strMonth = GetRecentDirName(LogPath + @"\" + strYear + @"\");
                strDay = GetRecentDirName(LogPath + @"\" + strYear + @"\" + strMonth);

                if (strDay == string.Empty || strMonth == string.Empty)
                    return string.Empty;

                return LogPath + @"\" + strYear + @"\" + strMonth + @"\" + strDay;
            }

            return string.Empty;
        }
		
		bool GetDateFromLogPath(string strLogPath, out string strYear, out string strMonth, out string strDay)
		{
			int nYear = -1;
            int nMonth = -1;
            int nDay = -1;

            strYear = string.Empty;
            strMonth = string.Empty;
            strDay = string.Empty;

            if (!strLogPath.Contains(LogPath))
				return false;
            
            int nTo = strLogPath.IndexOf(LogPath);
            string strBelowPath = FilterPathString(strLogPath, LogPath);
            
            List<string> vstrData = strBelowPath.Split('\\').ToList();

			if(!int.TryParse(vstrData[(int) eLogAddressDef.Year], out nYear)
				|| !int.TryParse(vstrData[(int) eLogAddressDef.Month], out nMonth)
				|| !int.TryParse(vstrData[(int) eLogAddressDef.Day], out nDay))
				return false;

            strYear = vstrData[(int)eLogAddressDef.Year];
            strMonth = vstrData[(int)eLogAddressDef.Month];
            strDay = vstrData[(int)eLogAddressDef.Day];

            return true;
		}
        
		string GetRecentDirName(string strDir, int nExcludeDir = -1)
		{
            if (!Directory.Exists(strDir))
                return string.Empty;

            List<string> vstrDir = Directory.GetDirectories(strDir).ToList();
            Dictionary<int, string> dicnDirPath = new Dictionary<int, string>();
            
            foreach (string strValue in vstrDir)
			{
				int nValue;
                int nFrom = strValue.IndexOf(strDir) + strDir.Length;
                string strBelowPath = FilterPathString(strValue, strDir);

                if (int.TryParse(strBelowPath, out nValue))
                    dicnDirPath.Add(nValue, strBelowPath);
			}

			if(nExcludeDir > 0)
                dicnDirPath = dicnDirPath.Where(x => x.Key < nExcludeDir).ToDictionary(x => x.Key, x => x.Value) as Dictionary<int, string>;
            
			return dicnDirPath.Count == 0 ? string.Empty : dicnDirPath[dicnDirPath.Keys.Max()];
		}

        List<string> GetLogFileNames(string strLogPath)
        {
            if (!Directory.Exists(strLogPath))
                return null;

            List<string> vstrFiles = new List<string>();

            foreach(string strFilePath in Directory.GetFiles(strLogPath))
            {
                DateTime time;
                string strFileName = FilterPathString(strFilePath, strLogPath);

                if (GetLogWrittenDate(strFileName, out time) && (GetLogWrittenStation(strFileName) == StationIndex))
                    vstrFiles.Add(strFilePath);
            }

            return vstrFiles;
        }

		string GetNextRecentLogFileName(string strCurLogName, string strPath, List<string> vstrFiles)
		{
			List<DateTime> vtime = new List<DateTime>();
			DateTime timeCur;

            if (vstrFiles == null)
                return string.Empty;
			
			if(!GetLogWrittenDate(strCurLogName, out timeCur))
				timeCur = DateTime.Now;

			foreach(string strFile in vstrFiles)
			{
				DateTime dateFile;
                string strBelowPath = FilterPathString(strFile, strPath);

                if (GetLogWrittenDate(strBelowPath, out dateFile) && (GetLogWrittenStation(strBelowPath) == StationIndex))
					vtime.Add(dateFile);
			}

			if(vtime.Where(x => x.Ticks == timeCur.Ticks).DefaultIfEmpty() != null)
				vtime.Remove(vtime.Find(x => x.Ticks == timeCur.Ticks));

			return vstrFiles[vtime.FindIndex(x => x == vtime.Max())];
		}

		bool GetLogWrittenDate(string strFileName, out DateTime writtenDate)
		{
			List<string> vstrParsed = strFileName.Split('_').ToList();
			
			writtenDate = new DateTime();
			
			if(vstrParsed.Count != (Enum.GetValues(typeof(eFileNameIndex))).Length)
				return false;

            string strWrittenDate = vstrParsed[(int)eFileNameIndex.WrittenDate];
            string strFormat = "yyyyMMddHHmmss";
            if (DateTime.TryParseExact(strWrittenDate, strFormat,
                           System.Globalization.CultureInfo.InvariantCulture,
                           System.Globalization.DateTimeStyles.None, out writtenDate))
                return true;
						
			return false;
		}

        int GetLogWrittenStation(string strFileName)
        {
            List<string> vstrParsed = strFileName.Split('_').ToList();
            int nResult = -1;

            if (vstrParsed.Count != (Enum.GetValues(typeof(eFileNameIndex))).Length)
                return nResult;

            if (int.TryParse(vstrParsed[(int)eFileNameIndex.Station], out nResult))
                return nResult;
            
            return nResult;
        }

        string FilterPathString(string strOrg, string strFilter)
        {
            int nFrom = strOrg.IndexOf(strFilter) + strFilter.Length;
            string strBelowPath = strOrg.Substring(nFrom, strOrg.Length - nFrom);

            strBelowPath = strBelowPath.Trim('\\');
            strBelowPath = strBelowPath.Trim('/');

            return strBelowPath;
        }

        private void chartControlTrend_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                TryAction(() =>
                {
                    Point pt = new Point(e.X, e.Y);
                    ChartHitInfo hit = chartControlTrend.CalcHitInfo(pt);

                    if (hit == null)
                        return;

                    popupMenuTrendChart.ShowPopup(Control.MousePosition);
                });
            }
        }

        private void barButtonItemClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LastLogLimitDate = (m_vdtStoredLogs.Count > 0) ? m_vdtStoredLogs[m_vdtStoredLogs.Count - 1].Key : new DateTime();

            m_vdtStoredLogs.Clear();
            ClearSeries();
        }

        private void barButtonItemCfgDisplay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmCfgTrendView frmOption = new FrmCfgTrendView();
            
            frmOption.DicDisplayingObjects = m_dicDisplayingObjects;
            frmOption.DicShowingSteps = m_dicShowingSteps;
            frmOption.DisplayLimit = DisplayLimit;
            frmOption.CategoryLimit = CategoryLimit;

            frmOption.Location = Cursor.Position;

            if (frmOption.ShowDialog() == DialogResult.OK)
            {
                DisplayLimit = frmOption.DisplayLimit;
                m_dicDisplayingObjects = frmOption.DicDisplayingObjects;

                DisplayTrendChart();
            }
        }

        private void chartControlTrend_CustomDrawSeries(object sender, CustomDrawSeriesEventArgs e)
        {
            
        }

        private void chartControlTrend_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            TryAction(() => {
                bool bMinAllowUsing = false;
                bool bMaxAllowUsing = false;
                double dMaxAllow = -1;
                double dMinAllow = -1;
                SeriesPoint point = e.SeriesPoint;
                double dCurValue = point.Values[0];

                if (double.TryParse(m_dicMinMax[e.Series].Max, out dMaxAllow))
                    bMaxAllowUsing = true;
                if (double.TryParse(m_dicMinMax[e.Series].Min, out dMinAllow))
                    bMinAllowUsing = true;

                if ((bMaxAllowUsing && dCurValue > dMaxAllow) || (bMinAllowUsing && dCurValue < dMinAllow))
                {
                    point.Annotations.Clear();
                    point.Color = ErrorColor;

                    var annotation = point.Annotations.AddTextAnnotation("NG", dCurValue.ToString());
                    annotation.AnchorPoint = new SeriesPointAnchorPoint(point);
                    annotation.ShapePosition = new RelativePosition();
                    RelativePosition position = annotation.ShapePosition as RelativePosition;
                    position.ConnectorLength = 25;
                    position.Angle = 90;
                    annotation.RuntimeMoving = true;
                }
            });
        }
    }
}
