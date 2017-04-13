using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CpTesterPlatform.CpTesterSs;
using System.Drawing;
using Dsu.Common.Utilities.ExtensionMethods;

namespace CpTesterPlatform.CpTester
{
    public partial class FormTotalResult7DCT : Form
    {
        private List<PLCResult> _lstResult = new List<PLCResult>();
        private Dictionary<int, ucSetItem> _lstResultUI = new Dictionary<int, ucSetItem>();
        private Timer timer1 = new Timer();
        public bool TotalNG { get; set; } = true;

        public FormTotalResult7DCT(List<PLCResult> lstResult, bool totalNg, string Message)
        {
            InitializeComponent();
            _lstResult = lstResult;
            TotalNG = totalNg;
            AddResultControl();
            simpleButton_Total.Text =  Message;
        }

        private void AddResultControl()
        {
            _lstResultUI.Add(1, ucSetItem1);
            _lstResultUI.Add(2, ucSetItem2);
            _lstResultUI.Add(3, ucSetItem3);
            _lstResultUI.Add(4, ucSetItem4);
            _lstResultUI.Add(5, ucSetItem5);
            _lstResultUI.Add(6, ucSetItem6);
            _lstResultUI.Add(7, ucSetItem7);
            _lstResultUI.Add(8, ucSetItem8);
            _lstResultUI.Add(9, ucSetItem9);
            _lstResultUI.Add(10, ucSetItem10);
            _lstResultUI.Add(11, ucSetItem11);
            _lstResultUI.Add(12, ucSetItem12);
            _lstResultUI.Add(13, ucSetItem13);
            _lstResultUI.Add(14, ucSetItem14);
            _lstResultUI.Add(15, ucSetItem15);
            _lstResultUI.Add(16, ucSetItem16);
            _lstResultUI.Add(17, ucSetItem17);
            _lstResultUI.Add(18, ucSetItem18);
            _lstResultUI.Add(19, ucSetItem19);
            _lstResultUI.Add(20, ucSetItem20);
            _lstResultUI.Add(21, ucSetItem21);
            _lstResultUI.Add(22, ucSetItem22);
            _lstResultUI.Add(23, ucSetItem23);
            _lstResultUI.Add(24, ucSetItem24);
            _lstResultUI.Add(25, ucSetItem25);
            _lstResultUI.Add(26, ucSetItem26);
            _lstResultUI.Add(27, ucSetItem27);
            _lstResultUI.Add(28, ucSetItem28);
            _lstResultUI.Add(29, ucSetItem29);
            _lstResultUI.Add(30, ucSetItem30);
            _lstResultUI.Add(31, ucSetItem31);
            _lstResultUI.Add(32, ucSetItem32);
            _lstResultUI.Add(33, ucSetItem33);
            _lstResultUI.Add(34, ucSetItem34);
            _lstResultUI.Add(35, ucSetItem35);
            _lstResultUI.Add(36, ucSetItem36);
            _lstResultUI.Add(37, ucSetItem37);
            _lstResultUI.Add(38, ucSetItem38);
            _lstResultUI.Add(39, ucSetItem39);
            _lstResultUI.Add(40, ucSetItem40);
            _lstResultUI.Add(41, ucSetItem41);
            _lstResultUI.Add(42, ucSetItem42);
            _lstResultUI.Add(43, ucSetItem43);
            _lstResultUI.Add(44, ucSetItem44);
            _lstResultUI.Add(45, ucSetItem45);
            _lstResultUI.Add(46, ucSetItem46);
            _lstResultUI.Add(47, ucSetItem47);
            _lstResultUI.Add(48, ucSetItem48);
        }

        private void frmTotalResult_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= _lstResult.Count; i++)
            {
                if (_lstResultUI.Count < i)
                    break;

                _lstResultUI[i].UpdateData(_lstResult[i - 1].DisplayName, _lstResult[i - 1].NG, _lstResult[i - 1].Skip, Math.Round(_lstResult[i - 1].Data, 2).ToString());
            }

            simpleButton_Total.Appearance.ForeColor = TotalNG ? Color.White : Color.DimGray;
            simpleButton_Total.Appearance.BackColor = TotalNG ? Color.IndianRed : Color.LightGreen;
        }

        public void CloseForm()
        {
            this.DoAsync(() =>
            {
                Close();
            });
        }

        private void simpleButton_Total_Click(object sender, EventArgs e)
        {
            CloseForm();
        }
    }
}
