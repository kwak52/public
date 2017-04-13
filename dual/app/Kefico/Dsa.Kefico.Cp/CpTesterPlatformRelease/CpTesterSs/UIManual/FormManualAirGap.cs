using System;
using System.Windows.Forms;
using CpTesterPlatform.Functions;

namespace CpTesterPlatform.CpTester
{
    public partial class FormManualAirGap : Form
    {
        public FormManualAirGap()
        {
            InitializeComponent();
        }

        private void ucManuAirGap_Load(object sender, EventArgs e)
        {
            UpdateUIAirGap();
        }

        private void UpdateUIAirGap()
        {
            filterableTextBox_AirGap.Text = (CpUtil.AirGapOffSet + CpUtil.AirGapDefault).ToString();
        }

        private void simpleButton_AirGap14_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 1.4 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap16_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 1.6 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap18_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 1.8 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap20_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 2.0 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap22_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 2.2 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap24_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 2.4 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap26_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 2.6 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap28_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 2.8 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap30_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 3.0 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap32_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 3.2 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap34_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 3.4 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }

        private void simpleButton_AirGap36_Click(object sender, EventArgs e)
        {
            CpUtil.AirGapOffSet = 3.6 - CpUtil.AirGapDefault;
            UpdateUIAirGap();
        }
    }
}
