using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Dsu.Common.Utilities.DX
{
	public partial class DXFormContainer : DevExpress.XtraEditors.XtraForm
	{
		public PanelControl Panel { get { return panelControl; } }
		public DXFormContainer()
		{
			InitializeComponent();
		}
	}
}