using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using DevExpress.XtraSplashScreen;
using Dsa.Kefico.MWS.Enumeration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dsa.Kefico.MWS
{
    public partial class frmMain
    {
        private void InitControl()
        {
            barButtonItem_Summary_Apply.Enabled = true;

            repositoryItemComboBox_LuneGroup.Items.AddRange(new string[] { "All Line", "1 Line", "2 Line", "3 Line", });
            barEditItem_EndDay.EditValueChanged += RepositoryItemEdit_SummaryChanged;
            barEditItem_StartDay.EditValueChanged += RepositoryItemEdit_SummaryChanged;
            barEditItem_LineGroup.EditValueChanged += RepositoryItemEdit_SummaryChanged;
            barEditItem_QuickView.EditValueChanged += RepositoryItemEdit_SummaryChanged;
            barEditItem_StartDay.EditValue = DateTime.Now.AddDays(0);
            barEditItem_EndDay.EditValue = DateTime.Now;
            barEditItem_LineGroup.EditValue = "All Line";
            barEditItem_QuickView.EditValue = true;
        }

        public DateTime TimeSummaryStart
        {
            get
            {
                if (barEditItem_StartDay.EditValue is DateTime)
                    return (DateTime)barEditItem_StartDay.EditValue;
                else
                    return DateTime.Now;
            }
            set { barEditItem_StartDay.EditValue = value; }
        }
        public DateTime TimeSummaryEnd
        {
            get
            {
                if (barEditItem_EndDay.EditValue is DateTime)
                    return (DateTime)barEditItem_EndDay.EditValue;
                else
                    return DateTime.Now;
            }
            set { barEditItem_EndDay.EditValue = value; }
        }
        public string LineGroup
        {
            get
            {
                if (barEditItem_LineGroup.EditValue is string)
                    return (string)barEditItem_LineGroup.EditValue;
                else
                    return string.Empty;
            }
            set { barEditItem_LineGroup.EditValue = value; }
        }
        public int SummaryID
        {
            get
            {
                if (barEditItem_TsvID.EditValue is int)
                    return (int)barEditItem_TsvID.EditValue;
                else
                    return 0;
            }
            set { barEditItem_TsvID.EditValue = value; }
        }
        public string SummaryID_Text { set { barStaticItem_TsvID.Caption = value; } }
        public int MeasureID
        {
            get
            {
                if (barEditItem_MeasureID.EditValue is int)
                    return (int)barEditItem_MeasureID.EditValue;
                else
                    return 0;
            }
            set { barEditItem_MeasureID.EditValue = value; }
        }
        public string MeasureID_Text { set { barStaticItem_MeasureID.Caption = value; } }
        public int PositionID
        {
            get
            {
                if (barEditItem_PositionID.EditValue is int)
                    return (int)barEditItem_PositionID.EditValue;
                else
                    return 0;
            }
            set { barEditItem_PositionID.EditValue = value; }
        }
        public string PositionID_Text { set { barStaticItem_PositionID.Caption = value; } }
        public bool QuickView
        {
            get
            {
                if (barEditItem_QuickView.EditValue is bool)
                    return (bool)barEditItem_QuickView.EditValue;
                else
                    return false;
            }
            set { barEditItem_QuickView.EditValue = value; }
        }
        public bool ChartScaleAuto
        {
            get
            {
                if (barEditItem_ChartScaleAuto.EditValue is bool)
                    return (bool)barEditItem_ChartScaleAuto.EditValue;
                else
                    return false;
            }
            set { barEditItem_ChartScaleAuto.EditValue = value; }
        }

        public bool SelectTest
        {
            get
            {
                if (barEditItem_SelectTest.EditValue is bool)
                    return (bool)barEditItem_SelectTest.EditValue;
                else
                    return false;
            }
            set { barEditItem_SelectTest.EditValue = value; }
        }

        public bool LogDebug
        {
            get
            {
                return barToggleSwitchItem_LogLevel.Checked;
            }
        }

    }
}
