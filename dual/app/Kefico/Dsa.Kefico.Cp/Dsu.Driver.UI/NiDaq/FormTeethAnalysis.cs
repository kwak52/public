using Dsu.Driver.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dsu.Driver.UI.NiDaq
{
    public partial class FormTeethAnalysis : Form
    {
        private ToothAnalysis[] _toothAnalysis;
        public FormTeethAnalysis(ToothAnalysis[] toothAnalysis)
        {
            InitializeComponent();
            _toothAnalysis = toothAnalysis;
            gridControl1.DataSource = toothAnalysis;
        }
    }
}
