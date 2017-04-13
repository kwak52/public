using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpTesterSs.UserControl
{
	public partial class UcTResultChart : DevExpress.XtraEditors.XtraUserControl
	{
		private UcMainViewSs userCtrMainView;

		public UcTResultChart(UcMainViewSs userCtrMainView)
        {
            InitializeComponent();
            this.userCtrMainView = userCtrMainView;
            this.Dock = DockStyle.Fill;
        }
	}
}
