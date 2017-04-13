using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpTesterPlatform.CpTester
{
    public partial class ucSetItem : UserControl
    {
        public string Label { get { return simpleButton_Label.Text; } set { simpleButton_Label.Text = value; } }
        public string Value { get { return simpleButton_Result.Text; } set { simpleButton_Result.Text = value; } }
        public bool NG { get; set; }
        public bool SKIP { get; set; }

        public ucSetItem()
        {
            InitializeComponent();
        }

        public void UpdateData()
        {
            InitializeComponent();
        }

        public void UpdateData(string name, bool nG, bool skip, string v)
        {
            Label = name;
            Value = v;
            NG = nG;
            SKIP = skip;

            if (Label == "") Value = "";

            if (SKIP)
            {
                simpleButton_OK.Text = "SKIP";
                simpleButton_OK.Appearance.ForeColor = Color.Purple;
                simpleButton_OK.Appearance.ForeColor = Color.White;
                Value = "";
            }
            else
            {
                simpleButton_OK.Text = NG ? "NG" : "OK";

                simpleButton_OK.Appearance.ForeColor = NG ? Color.White : Color.DimGray;
                simpleButton_OK.Appearance.BackColor = NG ? Color.IndianRed : Color.LightGreen;
            }
        }
    }
}
