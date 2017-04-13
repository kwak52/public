using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.DX
{
	public static class EmDxForm
	{
		public static DXFormContainer WrapIntoForm(this Control ctrl)
		{
			var form = new DXFormContainer();
			form.Panel.Controls.Add(ctrl);
			return form;
		}
	}
}
