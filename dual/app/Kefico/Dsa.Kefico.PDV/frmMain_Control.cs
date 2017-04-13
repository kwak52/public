using System;

namespace Dsa.Kefico.PDV
{
    public partial class frmMain
    {
        private void InitControl()
        {
            repositoryItemComboBox_LuneGroup.Items.AddRange(new string[] { "All Line", "1 Line", "2 Line", "3 Line", });
            barEditItem_EndDay.EditValueChanged += RepositoryItemEdit_SummaryChanged;
            barEditItem_StartDay.EditValueChanged += RepositoryItemEdit_SummaryChanged;
            barEditItem_LineGroup.EditValueChanged += RepositoryItemEdit_SummaryChanged;
            barEditItem_QuickView.EditValueChanged += RepositoryItemEdit_SummaryChanged;
            barEditItem_StartDay.EditValue = DateTime.Now.AddDays(-200);
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

        public bool LogDebug
        {
            get
            {
                    return barToggleSwitchItem_LogLevel.Checked;
            }
        }


    }
}
