using System;
using System.Windows.Forms;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Threading.Tasks;

namespace CpTesterSs.UserControl
{
    public partial class UcTStepInformation : DevExpress.XtraEditors.XtraUserControl
    {
        private UcMainViewSs userCtrMainView;

        public UcTStepInformation()
        {
            InitializeComponent();
        }

        public UcTStepInformation(UcMainViewSs userCtrMainView)
        {
            InitializeComponent();
            this.userCtrMainView = userCtrMainView;
            this.Dock = DockStyle.Fill;
        }

        public static implicit operator UcTStepInformation(UcTStepStatus v)
        {
            throw new NotImplementedException();
        }

		public async Task ChangeStepFunction(string strfunc)
		{
			  await this.DoAsync(() => { labelControlFirst.Text = strfunc; });
		}
    }
}
